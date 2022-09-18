using RetroDevStudio.Formats;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace RetroDevStudio.Documents
{
  public partial class Disassembler : CompilableDocument
  {
    private Formats.DisassemblyProject  m_DisassemblyProject = new RetroDevStudio.Formats.DisassemblyProject();

    private Parser.Disassembler         m_Disassembler = new RetroDevStudio.Parser.Disassembler( Tiny64.Processor.Create6510() );

    private string                      m_OpenedFilename = "";

    private FastColoredTextBoxNS.TextStyle[]          m_TextStyles = new FastColoredTextBoxNS.TextStyle[(int)Types.ColorableElement.LAST_ENTRY];
    private System.Text.RegularExpressions.Regex[]    m_TextRegExp = new System.Text.RegularExpressions.Regex[(int)Types.ColorableElement.LAST_ENTRY];

    private int                         m_ContextMenuOpeningInLineIndex = -1;
    private int                         m_ContextMenuCharPos = -1;




    public override FastColoredTextBoxNS.FastColoredTextBox SourceControl
    {
      get
      {
        return editDisassembly;
      }
    }



    public Disassembler( StudioCore Core )
    {
      this.Core = Core;
      DocumentInfo.Type = ProjectElement.ElementType.DISASSEMBLER;
      DocumentInfo.UndoManager.MainForm = Core.MainForm;

      string opCodes = @"\b(lda|sta|ldy|sty|ldx|stx|rts|jmp|jsr|rti|sei|cli|asl|lsr|inc|dec|inx|dex|iny|dey|cpx|cpy|cmp|bit|bne|beq|bcc|bcs|bpl|bmi|adc|sec|clc|sbc|tax|tay|tya|txa|pha|pla|eor|and|ora|ror|rol|php|plp|clv|cld|bvc|bvs|brk|nop|txs|tsx|slo|rla|sre|rra|sax|lax|dcp|isc|anc|alr|arr|xaa|axs|ahx|shy|shx|tas|las|sed)\b";
      string pseudoOps = @"(!byte|!by|!basic|!8|!08|!word|!wo|!16|!text|!tx|!scr|!pet|!raw|!pseudopc|!realpc|!bank|!convtab|!ct|!binary|!bin|!bi|!source|!src|!to|!zone|!zn|!error|!serious|!warn|"
        + @"!message|!ifdef|!ifndef|!if|!fill|!fi|!align|!endoffile|!nowarn|!for|!end|!macro|!trace|!media|!mediasrc|!sl|!cpu|!set)\b";

      m_TextRegExp[(int)Types.ColorableElement.LITERAL_NUMBER] = new System.Text.RegularExpressions.Regex( @"\b\d+[\.]?\d*([eE]\-?\d+)?[lLdDfF]?\b|\B\$[a-fA-F\d]+\b|\b0x[a-fA-F\d]+\b" );
      m_TextRegExp[(int)Types.ColorableElement.LITERAL_STRING] = new System.Text.RegularExpressions.Regex( @"""""|''|"".*?[^\\]""|'.*?[^\\]'" );

      m_TextRegExp[(int)Types.ColorableElement.CODE] = new System.Text.RegularExpressions.Regex( opCodes, System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled );
      m_TextRegExp[(int)Types.ColorableElement.PSEUDO_OP] = new System.Text.RegularExpressions.Regex( pseudoOps, System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled );

      m_TextRegExp[(int)Types.ColorableElement.LABEL] = new System.Text.RegularExpressions.Regex( @"[+\-a-zA-Z]+[a-zA-Z_\d]*[:]*" );
      m_TextRegExp[(int)Types.ColorableElement.COMMENT] = new System.Text.RegularExpressions.Regex( @";.*" );

      InitializeComponent();

      GR.Image.DPIHandler.ResizeControlsForDPI( this );

      editDisassembly.TextChanged += new EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>( editDisassembly_TextChanged );

      RefreshDisplayOptions();
    }



    void editDisassembly_TextChanged( object sender, FastColoredTextBoxNS.TextChangedEventArgs e )
    {
      //clear previous highlighting
      e.ChangedRange.ClearStyle( m_TextStyles );

      // apply all styles
      e.ChangedRange.SetStyle( m_TextStyles[SyntaxElementStylePrio( Types.ColorableElement.LITERAL_NUMBER )], m_TextRegExp[(int)Types.ColorableElement.LITERAL_NUMBER] );
      e.ChangedRange.SetStyle( m_TextStyles[SyntaxElementStylePrio( Types.ColorableElement.LITERAL_STRING )], m_TextRegExp[(int)Types.ColorableElement.LITERAL_STRING] );
      e.ChangedRange.SetStyle( m_TextStyles[SyntaxElementStylePrio( Types.ColorableElement.CODE )], m_TextRegExp[(int)Types.ColorableElement.CODE] );
      e.ChangedRange.SetStyle( m_TextStyles[SyntaxElementStylePrio( Types.ColorableElement.PSEUDO_OP )], m_TextRegExp[(int)Types.ColorableElement.PSEUDO_OP] );
      e.ChangedRange.SetStyle( m_TextStyles[SyntaxElementStylePrio( Types.ColorableElement.LABEL )], m_TextRegExp[(int)Types.ColorableElement.LABEL] );
      e.ChangedRange.SetStyle( m_TextStyles[SyntaxElementStylePrio( Types.ColorableElement.COMMENT )], m_TextRegExp[(int)Types.ColorableElement.COMMENT] );
    }



    private void SetHexData( GR.Memory.ByteBuffer Data )
    {
      hexView.ByteProvider = new Be.Windows.Forms.DynamicByteProvider( Data.Data() );
      hexView.ByteProvider.Changed += new EventHandler( ByteProvider_Changed );
    }



    void ByteProvider_Changed( object sender, EventArgs e )
    {
      // changed
      GR.Memory.ByteBuffer    data = DataFromHex();

      m_Disassembler.SetData( data );
      m_DisassemblyProject.Data = data;
      UpdateDisassembly();
    }


    private GR.Memory.ByteBuffer DataFromHex()
    {
      Be.Windows.Forms.DynamicByteProvider dynProvider = (Be.Windows.Forms.DynamicByteProvider)hexView.ByteProvider;

      List<byte> dataBytes = dynProvider.Bytes;
      if ( dataBytes.Count == 0 )
      {
        return new GR.Memory.ByteBuffer();
      }

      long     dataStart = 0;
      long     dataLength = hexView.ByteProvider.Length;

      GR.Memory.ByteBuffer data = new GR.Memory.ByteBuffer( (uint)dataLength );
      for ( int i = 0; i < dataBytes.Count; ++i )
      {
        data.SetU8At( i, dataBytes[(int)dataStart + i] );
      }
      return data;
    }



    private void btnOpenBinary_Click( object sender, EventArgs e )
    {
      OpenFileDialog    openDialog = new OpenFileDialog();

      openDialog.Title = "Choose binary file";
      if ( openDialog.ShowDialog() == DialogResult.OK )
      {
        m_OpenedFilename = openDialog.FileName;

        GR.Memory.ByteBuffer    data = GR.IO.File.ReadAllBytes( openDialog.FileName );
        if ( data != null )
        {
          ushort    loadAddress = 0x801;

          if ( System.IO.Path.GetExtension( openDialog.FileName ).ToUpper() == ".PRG" )
          {
            // treat first two bytes as load address
            loadAddress = data.UInt16At( 0 );

            data = data.SubBuffer( 2 );
          }
          m_Disassembler.SetData( data );
          m_DisassemblyProject.Data = data;

          editStartAddress.Text = "$" + loadAddress.ToString( "X4" );

          int sysAddress = -1;

          if ( m_Disassembler.HasBASICJumpAddress( loadAddress, out sysAddress ) )
          {
            m_DisassemblyProject.JumpedAtAddresses.Add( sysAddress );

            ListViewItem    item = new ListViewItem();
            FillItemFromAddress( item, sysAddress );
            listJumpedAtAddresses.Items.Add( item );
          }
          else
          {
            // we have no jump address, assume first byte as address
            m_DisassemblyProject.JumpedAtAddresses.Add( loadAddress );

            ListViewItem    item = new ListViewItem();
            FillItemFromAddress( item, loadAddress );
            listJumpedAtAddresses.Items.Add( item );
          }

          SetHexData( data );

          UpdateDisassembly();
        }
      }
    }



    public override void RefreshDisplayOptions()
    {
      base.RefreshDisplayOptions();

      // Font
      editDisassembly.Font = new System.Drawing.Font( Core.Settings.SourceFontFamily, Core.Settings.SourceFontSize, Core.Settings.SourceFontStyle );

      // Colors
      ApplySyntaxColoring( Types.ColorableElement.COMMENT );
      ApplySyntaxColoring( Types.ColorableElement.LITERAL_NUMBER );
      ApplySyntaxColoring( Types.ColorableElement.LITERAL_STRING );
      ApplySyntaxColoring( Types.ColorableElement.LABEL );
      ApplySyntaxColoring( Types.ColorableElement.CODE );
      ApplySyntaxColoring( Types.ColorableElement.EMPTY_SPACE );
      ApplySyntaxColoring( Types.ColorableElement.OPERATOR );
      ApplySyntaxColoring( Types.ColorableElement.PSEUDO_OP );

      editDisassembly.Language = FastColoredTextBoxNS.Language.VB;
      editDisassembly.CommentPrefix = ";";

      //editSource.Indentation.UseTabs = !Core.Settings.TabConvertToSpaces;
      editDisassembly.AllowTabs = true; //Core.Settings.AllowTabs;
      editDisassembly.ConvertTabsToSpaces = Core.Settings.TabConvertToSpaces;
      editDisassembly.TabLength = Core.Settings.TabSize;

      //call OnTextChanged for refresh syntax highlighting
      editDisassembly.OnTextChanged();
    }



    int SyntaxElementStylePrio( Types.ColorableElement Element )
    {
      int     value = 10;

      switch ( Element )
      {
        case RetroDevStudio.Types.ColorableElement.CODE:
          value = 6;
          break;
        case RetroDevStudio.Types.ColorableElement.COMMENT:
          value = 2;
          break;
        case RetroDevStudio.Types.ColorableElement.CURRENT_DEBUG_LINE:
          value = 1;
          break;
        case RetroDevStudio.Types.ColorableElement.EMPTY_SPACE:
          value = 0;
          break;
        case RetroDevStudio.Types.ColorableElement.LABEL:
          value = 7;
          break;
        case RetroDevStudio.Types.ColorableElement.LITERAL_NUMBER:
          value = 3;
          break;
        case RetroDevStudio.Types.ColorableElement.LITERAL_STRING:
          value = 4;
          break;
        case RetroDevStudio.Types.ColorableElement.PSEUDO_OP:
          value = 5;
          break;
        case RetroDevStudio.Types.ColorableElement.NONE:
          value = 9;
          break;
        case RetroDevStudio.Types.ColorableElement.OPERATOR:
          value = 8;
          break;
      }
      return value;
    }



    void ApplySyntaxColoring( Types.ColorableElement Element )
    {
      System.Drawing.Brush      foreBrush = new System.Drawing.SolidBrush( GR.Color.Helper.FromARGB( Core.Settings.FGColor( Element ) ) );
      System.Drawing.Brush      backBrush = null;
      System.Drawing.FontStyle  fontStyle = System.Drawing.FontStyle.Regular;

      backBrush = new System.Drawing.SolidBrush( GR.Color.Helper.FromARGB( Core.Settings.BGColor( Element ) ) );
      m_TextStyles[SyntaxElementStylePrio( Element )] = new FastColoredTextBoxNS.TextStyle( foreBrush, backBrush, fontStyle );

      //editSource.AddStyle( m_TextStyles[(int)Element] );
      editDisassembly.SelectionColor = GR.Color.Helper.FromARGB( Core.Settings.FGColor( Types.ColorableElement.SELECTED_TEXT ) );
      editDisassembly.Styles[SyntaxElementStylePrio( Element )] = m_TextStyles[SyntaxElementStylePrio( Element )];
    }



    private void UpdateDisassembly()
    {
      int     topLine = editDisassembly.VisibleRange.Start.iLine;

      string  disassembly;
      var settings = new DisassemblerSettings() { AddLineAddresses = checkShowLineAddresses.Checked, AddAssembledBytes = checkShowHexData.Checked };

      if ( m_Disassembler.Disassemble( m_DisassemblyProject.DataStartAddress, m_DisassemblyProject.JumpedAtAddresses, m_DisassemblyProject.NamedLabels, settings, out disassembly, out int dummy ) )
      {
        editDisassembly.Text = disassembly;

        int firstLine = editDisassembly.VisibleRange.Start.iLine;

        editDisassembly.VerticalScroll.Value += topLine - firstLine;
      }
      else
      {
        editDisassembly.Text = disassembly;
      }
    }



    private void editStartAddress_TextChanged( object sender, EventArgs e )
    {
      string    startAddressT = editStartAddress.Text;

      if ( startAddressT.Length > 0 )
      {
        if ( startAddressT[0] == '$' )
        {
          m_DisassemblyProject.DataStartAddress = GR.Convert.ToI32( startAddressT.Substring( 1 ), 16 );
        }
        else if ( startAddressT.StartsWith( "0x" ) )
        {
          m_DisassemblyProject.DataStartAddress = GR.Convert.ToI32( startAddressT.Substring( 2 ), 16 );
        }
        else
        {
          m_DisassemblyProject.DataStartAddress = GR.Convert.ToI32( startAddressT );
        }
        hexView.LineInfoOffset = m_DisassemblyProject.DataStartAddress;
        UpdateDisassembly();
      }
    }



    private void FillItemFromAddress( ListViewItem Item, int Address )
    {
      Item.Text = "$" + Address.ToString( "X4" ) + "   " + Address;
      Item.Tag = Address;
    }



    private void btnAddJumpAddress_Click( object sender, EventArgs e )
    {
      string    addressT = editJumpAddress.Text;
      int       address = 0;

      if ( addressT.Length > 0 )
      {
        if ( addressT[0] == '$' )
        {
          address = GR.Convert.ToI32( addressT.Substring( 1 ), 16 );
        }
        else if ( addressT.StartsWith( "0x" ) )
        {
          address = GR.Convert.ToI32( addressT.Substring( 2 ), 16 );
        }
        else
        {
          address = GR.Convert.ToI32( addressT );
        }
        AddJumpedAtAddress( (ushort)address );
      }
    }



    private void AddJumpedAtAddress( ushort Address )
    {
      if ( !m_DisassemblyProject.JumpedAtAddresses.ContainsValue( Address ) )
      {
        m_DisassemblyProject.JumpedAtAddresses.Add( Address );


        ListViewItem    item = new ListViewItem();
        FillItemFromAddress( item, Address );
        listJumpedAtAddresses.Items.Add( item );

        UpdateDisassembly();
      }
    }



    private void btnDeleteJumpedAtAddress_Click( object sender, EventArgs e )
    {
      if ( listJumpedAtAddresses.SelectedItems.Count == 0 )
      {
        return;
      }
      int     address = (int)listJumpedAtAddresses.SelectedItems[0].Tag;

      m_DisassemblyProject.JumpedAtAddresses.Remove( address );
      listJumpedAtAddresses.Items.Remove( listJumpedAtAddresses.SelectedItems[0] );

      UpdateDisassembly();
    }



    private void listJumpedAtAddresses_SelectedIndexChanged( object sender, EventArgs e )
    {
      btnDeleteJumpedAtAddress.Enabled = ( listJumpedAtAddresses.SelectedItems.Count != 0 );
    }



    private void btnOpenProject_Click( object sender, EventArgs e )
    {
      OpenFileDialog    openDialog = new OpenFileDialog();

      openDialog.Title = "Choose binary file";
      openDialog.Filter = "Disassembly Projects|*.disassembly|All Files|*.*";
      if ( openDialog.ShowDialog() == DialogResult.OK )
      {
        OpenDisassemblyProject( openDialog.FileName );
      }
    }



    private void OpenDisassemblyProject( string FileName )
    {
      GR.Memory.ByteBuffer    data = GR.IO.File.ReadAllBytes( FileName );
      if ( ( data != null )
      &&   ( m_DisassemblyProject.ReadFromBuffer( data ) ) )
      {
        m_Disassembler.SetData( m_DisassemblyProject.Data );
        editStartAddress.Text = "$" + m_DisassemblyProject.DataStartAddress.ToString( "X4" );
        editDisassemblyProjectName.Text = m_DisassemblyProject.Description;
        listJumpedAtAddresses.Items.Clear();
        foreach ( int jumpAddress in m_DisassemblyProject.JumpedAtAddresses )
        {
          ListViewItem    item = new ListViewItem();
          FillItemFromAddress( item, jumpAddress );
          item.Tag = jumpAddress;
          listJumpedAtAddresses.Items.Add( item );
        }

        listNamedLabels.Items.Clear();
        foreach ( var namedLabel in m_DisassemblyProject.NamedLabels )
        {
          ListViewItem  item = new ListViewItem( namedLabel.Value );
          item.SubItems.Add( "$" + namedLabel.Key.ToString( "X4" ) );
          listNamedLabels.Items.Add( item );
        }

        SetHexData( m_DisassemblyProject.Data );
        UpdateDisassembly();
      }
    }



    private void btnSaveProject_Click( object sender, EventArgs e )
    {
      SaveFileDialog    saveDialog = new SaveFileDialog();

      saveDialog.Title = "Choose project file name";
      saveDialog.Filter = "Disassembly Projects|*.disassembly|All Files|*.*";
      if ( saveDialog.ShowDialog() == DialogResult.OK )
      {
        GR.IO.File.WriteAllBytes( saveDialog.FileName, m_DisassemblyProject.SaveToBuffer() );
      }
    }



    private void editDisassemblyProjectName_TextChanged( object sender, EventArgs e )
    {
      m_DisassemblyProject.Description = editDisassemblyProjectName.Text;
    }



    private void btnExportAssembly_Click( object sender, EventArgs e )
    {
      string  disassembly;

      var settings = new DisassemblerSettings();

      if ( !m_Disassembler.Disassemble( m_DisassemblyProject.DataStartAddress, m_DisassemblyProject.JumpedAtAddresses, m_DisassemblyProject.NamedLabels, settings, out disassembly, out int dummy ) )
      {
        return;
      }

      string    newFilename = "disassembly.asm";

      try
      {
        newFilename = System.IO.Path.Combine( System.IO.Path.GetDirectoryName( m_OpenedFilename ), System.IO.Path.GetFileNameWithoutExtension( m_OpenedFilename ) ) + ".asm";
      }
      catch ( Exception )
      {
        newFilename = "disassembly.asm";
      }

      if ( Core.Navigating.Solution != null )
      {
        while ( Core.Navigating.Solution.FilenameUsed( newFilename ) )
        {
          newFilename = System.IO.Path.Combine( System.IO.Path.GetDirectoryName( m_OpenedFilename ), System.IO.Path.GetFileNameWithoutExtension( m_OpenedFilename ) ) + "1.asm";
        }
      }
      SourceASMEx document = new SourceASMEx( Core );

      document.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.Document;
      document.Core = Core;
      document.Text = System.IO.Path.GetFileName( newFilename );
      document.FillContent( disassembly, false, false );
      document.Show( Core.MainForm.panelMain );
    }



    private void listNamedLabels_SelectedIndexChanged( object sender, EventArgs e )
    {
      btnDeleteNamedLabel.Enabled = ( listNamedLabels.SelectedItems.Count != 0 );
    }



    private void btnDeleteNamedLabel_Click( object sender, EventArgs e )
    {
      if ( listNamedLabels.SelectedItems.Count == 0 )
      {
        return;
      }
      int     address = GR.Convert.ToI32( listNamedLabels.SelectedItems[0].SubItems[1].Text.Substring( 1 ), 16 );

      m_DisassemblyProject.NamedLabels.Remove( address );
      listNamedLabels.Items.Remove( listNamedLabels.SelectedItems[0] );
      UpdateDisassembly();
    }



    private void btnAddNamedLabel_Click( object sender, EventArgs e )
    {
      string    addressT = editLabelAddress.Text;

      int   address = 0;

      if ( addressT[0] == '$' )
      {
        address = GR.Convert.ToI32( addressT.Substring( 1 ), 16 );
      }
      else if ( addressT.StartsWith( "0x" ) )
      {
        address = GR.Convert.ToI32( addressT.Substring( 2 ), 16 );
      }
      else
      {
        address = GR.Convert.ToI32( addressT );
      }

      if ( !m_DisassemblyProject.NamedLabels.ContainsKey( address ) )
      {
        m_DisassemblyProject.NamedLabels[address] = editLabelName.Text;

        ListViewItem  item = new ListViewItem( editLabelName.Text );
        item.SubItems.Add( "$" + address.ToString( "X4" ) );

        listNamedLabels.Items.Add( item );

        UpdateDisassembly();
      }
      else if ( m_DisassemblyProject.NamedLabels[address] != editLabelName.Text )
      {
        for ( int i = 0; i < listNamedLabels.Items.Count; ++i )
        {
          if ( address == GR.Convert.ToI32( listNamedLabels.Items[i].SubItems[1].Text.Substring( 1 ), 16 ) )
          {
            listNamedLabels.Items[i].Text = editLabelName.Text;
            break;
          }
        }
        m_DisassemblyProject.NamedLabels[address] = editLabelName.Text;
        UpdateDisassembly();
      }
    }



    private void editLabelAddress_TextChanged( object sender, EventArgs e )
    {
      btnAddNamedLabel.Enabled = IsValidNamedLabel();
    }



    private bool IsValidNamedLabel()
    {
      if ( ( editLabelAddress.Text.Length == 0 )
      || ( editLabelName.Text.Length == 0 ) )
      {
        return false;
      }
      return true;
    }



    private void btnImportBinary_Click( object sender, EventArgs e )
    {
      if ( !Clipboard.ContainsText() )
      {
        return;
      }

      string    text = Clipboard.GetText().Replace( " ", "" );

      GR.Memory.ByteBuffer    data = new GR.Memory.ByteBuffer();

      if ( !data.AppendHex( text ) )
      {
        return;
      }
      m_Disassembler.SetData( data );
      m_DisassemblyProject.Data = data;

      SetHexData( data );

      UpdateDisassembly();
    }



    private void btnReloadFile_Click( object sender, EventArgs e )
    {
      GR.Memory.ByteBuffer    data = GR.IO.File.ReadAllBytes( m_OpenedFilename );
      if ( data != null )
      {
        ushort    loadAddress = 0x801;

        if ( System.IO.Path.GetExtension( m_OpenedFilename ).ToUpper() == ".PRG" )
        {
          // treat first two bytes as load address
          loadAddress = data.UInt16At( 0 );

          data = data.SubBuffer( 2 );
        }
        m_Disassembler.SetData( data );
        m_DisassemblyProject.Data = data;

        SetHexData( data );

        UpdateDisassembly();
      }

    }



    private void addJumpAddressToolStripMenuItem_Click( object sender, EventArgs e )
    {
      int     addressInLine = ExtractAddressFromLine( m_ContextMenuOpeningInLineIndex );

      if ( ( addressInLine >= 0 )
      &&   ( addressInLine < 0x10000 ) )
      {
        AddJumpedAtAddress( (ushort)addressInLine );
      }
    }



    private int ExtractAddressFromLine( int LineIndex )
    {
      if ( ( LineIndex < 0 )
      ||   ( LineIndex >= editDisassembly.LinesCount ) )
      {
        return - 1;
      }
      string    curLine = editDisassembly.Lines[LineIndex];
      if ( ( curLine.Length >= 5 )
      &&   ( curLine.StartsWith( "$" ) ) )
      {
        return GR.Convert.ToI32( curLine.Substring( 1, 4 ), 16 );
      }
      return -1;
    }



    private int ExtractAddressFromPosition( int LineIndex, int CharIndex )
    {
      if ( ( LineIndex < 0 )
      ||   ( LineIndex >= editDisassembly.LinesCount ) )
      {
        return -1;
      }
      string    curLine = editDisassembly.Lines[LineIndex];
      if ( ( CharIndex < 0 )
      ||   ( CharIndex > curLine.Length ) )
      {
        return -1;
      }

      int     startPos = CharIndex;
      int     endPos = CharIndex;

      while ( ( endPos < curLine.Length )
      &&      ( IsHexChar( curLine[endPos] ) ) )
      {
        ++endPos;
      }

      while ( ( startPos > 0 )
      &&      ( IsHexChar( curLine[startPos - 1] ) ) ) 
      {
        --startPos;
      }

      string    substring = curLine.Substring( startPos, endPos - startPos );

      return GR.Convert.ToI32( substring, 16 );
    }



    private bool IsHexChar( char Character )
    {
      if ( char.IsDigit( Character ) )
      {
        return true;
      }
      if ( ( ( Character >= 'A' )
      &&     ( Character <= 'F' ) )
      ||   ( ( Character >= 'a' )
      &&     ( Character <= 'f' ) ) )
      {
        return true;
      }
      return false;
    }



    private void contextMenuDisassembler_Opening( object sender, CancelEventArgs e )
    {
      System.Drawing.Point mousePos = editDisassembly.PointToClient( Control.MousePosition );

      int position                    = editDisassembly.PointToPosition( mousePos );
      m_ContextMenuOpeningInLineIndex = editDisassembly.PositionToPlace( position ).iLine;
      m_ContextMenuCharPos            = editDisassembly.PositionToPlace( position ).iChar;

      int     addressInLine = ExtractAddressFromLine( m_ContextMenuOpeningInLineIndex );
      int     addressAtCursor = ExtractAddressFromPosition( m_ContextMenuOpeningInLineIndex, m_ContextMenuCharPos );

      if ( addressInLine != -1 )
      {
        addJumpAddressToolStripMenuItem.Text = "Add jump address ($" + addressInLine.ToString( "X4" ) + ")";
      }
      else
      {
        addJumpAddressToolStripMenuItem.Text = "Add jump address";
      }
      if ( addressAtCursor != -1 )
      {
        addAsLabelToolStripMenuItem.Text = "Add as label ($" + addressAtCursor.ToString( "X4" ) + ")";
        addAsLabelToolStripMenuItem.Enabled = true;
      }
      else
      {
        addAsLabelToolStripMenuItem.Enabled = false;
      }
    }



    private void checkShowLineAddresses_CheckedChanged( object sender, EventArgs e )
    {
      UpdateDisassembly();
    }



    private void checkShowHexData_CheckedChanged( object sender, EventArgs e )
    {
      UpdateDisassembly();
    }



    private void addAsLabelToolStripMenuItem_Click( object sender, EventArgs e )
    {
      int     addressAtCursor = ExtractAddressFromPosition( m_ContextMenuOpeningInLineIndex, m_ContextMenuCharPos );

      if ( ( addressAtCursor >= 0 )
      &&   ( addressAtCursor < 0x10000 ) )
      {
        int address = addressAtCursor;

        string    autoNamedLabel = "label_" + address.ToString( "X4" );
        if ( !m_DisassemblyProject.NamedLabels.ContainsKey( address ) )
        {
          m_DisassemblyProject.NamedLabels[address] = autoNamedLabel;

          ListViewItem  item = new ListViewItem( autoNamedLabel );
          item.SubItems.Add( "$" + address.ToString( "X4" ) );

          listNamedLabels.Items.Add( item );

          UpdateDisassembly();
        }
      }
    }



    public override bool Load()
    {
      m_DisassemblyProject.Description = DocumentInfo.DocumentFilename;

      OpenDisassemblyProject( DocumentInfo.FullPath );
      return true;
    }




  }
}
