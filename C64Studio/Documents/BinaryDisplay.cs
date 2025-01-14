using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using GR.Image;
using GR.Memory;
using RetroDevStudio.Controls;
using RetroDevStudio.Dialogs;
using RetroDevStudio.Formats;



namespace RetroDevStudio.Documents
{
  public partial class BinaryDisplay : BaseDocument
  {
    private ToolStripMenuItem m_MenuItemChangeOffset;

    private ExportBinaryFormBase        _ExportForm = null;
    private ImportBinaryFormBase        _ImportForm = null;



    public BinaryDisplay( StudioCore Core, GR.Memory.ByteBuffer WorkData, bool AllowEditing, bool FixedWidth )
    {
      this.Core = Core;
      DocumentInfo.UndoManager.MainForm = Core.MainForm;
      AutoScaleMode = AutoScaleMode.None;
      InitializeComponent();

      DPIHandler.ResizeControlsForDPI( this );

      if ( WorkData == null )
      {
        SetHexData( new GR.Memory.ByteBuffer() );
      }
      else
      {
        SetHexData( WorkData );
      }
      this.AllowEditing = AllowEditing;
      this.FixedWidth = FixedWidth;

      hexView.ByteProvider.Changed += new EventHandler( ByteProvider_Changed );

      // modify context menu
      hexView.ContextMenuStrip.Items.Add( "-" );

      m_MenuItemChangeOffset = new ToolStripMenuItem( "Offset Displayed Address" );
      m_MenuItemChangeOffset.Click += OnMenuItemChangeOffsetClick;
      hexView.ContextMenuStrip.Items.Add( m_MenuItemChangeOffset );

      comboExportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "as assembly", typeof( ExportBinaryAsAssembly ) ) );
      comboExportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "as hex", typeof( ExportBinaryAsHex ) ) );
      comboExportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "as BASIC DATA statements", typeof( ExportBinaryAsBASICData ) ) );
      comboExportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "as BASIC PRINT", typeof( ExportBinaryAsPRINTStatement ) ) );
      comboExportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "to file", typeof( ExportBinaryAsFile ) ) );
      comboExportMethod.SelectedIndex = 0;

      comboImportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "from file", typeof( ImportBinaryFromFile ) ) );
      comboImportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "from assembly", typeof( ImportBinaryFromASM ) ) );
      comboImportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "from hex", typeof( ImportBinaryFromHex ) ) );
      comboImportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "from BASIC DATA statements", typeof( ImportBinaryFromBASICDATA ) ) );
      comboImportMethod.SelectedIndex = 0;
    }



    private void OnMenuItemChangeOffsetClick( object sender, EventArgs e )
    {
      var formDisplayOffset = new FormSetOffset( Core );
      formDisplayOffset.DisplayOffset = hexView.DisplayedAddressOffset;
      if ( formDisplayOffset.ShowDialog() == DialogResult.OK )
      {
        hexView.DisplayedAddressOffset = formDisplayOffset.DisplayOffset;
        hexView.Invalidate();
      }
    }



    void ByteProvider_Changed( object sender, EventArgs e )
    {
      // changed
      SetModified();
    }



    public override Size GetPreferredSize( Size proposedSize )
    {
      return new Size( 733, 514 );
    }



    public void SetHexData( GR.Memory.ByteBuffer Data )
    {
      if ( Data == null )
      {
        return;
      }
      hexView.ByteProvider = new Be.Windows.Forms.DynamicByteProvider( Data.Data() );
      hexView.ByteProvider.Changed += new EventHandler( ByteProvider_Changed );
    }


    public bool AllowEditing
    {
      get
      {
        return !hexView.ReadOnly;
      }
      set
      {
        hexView.ReadOnly = !value;
        if ( hexView.ReadOnly )
        {
          tabMain.TabPages.Remove( tabModify );
        }
        else if ( !tabMain.TabPages.Contains( tabModify ) )
        {
          tabMain.TabPages.Add( tabModify );
        }

        importFromFileToolStripMenuItem.Visible = value;
      }
    }



    public bool FixedWidth
    {
      get
      {
        return hexView.UseFixedBytesPerLine;
      }
      set
      {
        hexView.UseFixedBytesPerLine = value;
      }
    }



    private void closeToolStripMenuItem_Click( object sender, EventArgs e )
    {
      Close();
    }



    private void importFromFileToolStripMenuItem_Click( object sender, EventArgs e )
    {
      Import();
    }



    private void Import()
    {
      string filename;

      if ( OpenFile( "Open Binary File", RetroDevStudio.Types.Constants.FILEFILTER_ALL, out filename ) )
      {
        GR.Memory.ByteBuffer data = GR.IO.File.ReadAllBytes( filename );
        if ( data != null )
        {
          SetHexData( data );
        }
      }
    }



    private GR.Memory.ByteBuffer DataFromHex()
    {
      Be.Windows.Forms.DynamicByteProvider dynProvider = (Be.Windows.Forms.DynamicByteProvider)hexView.ByteProvider;

      List<byte> dataBytes = dynProvider.Bytes;
      if ( dataBytes.Count == 0 )
      {
        return new GR.Memory.ByteBuffer();
      }

      long     dataStart = hexView.SelectionStart;
      long     dataLength = hexView.SelectionLength;
      if ( hexView.SelectionLength == 0 )
      {
        dataStart = 0;
        dataLength = hexView.ByteProvider.Length;
      }


      GR.Memory.ByteBuffer data = new GR.Memory.ByteBuffer( (uint)dataLength );
      for ( int i = 0; i < dataLength; ++i )
      {
        data.SetU8At( i, dataBytes[(int)dataStart + i] );
      }
      return data;
    }



    private void exportToFileToolStripMenuItem_Click( object sender, EventArgs e )
    {
      Export();
    }



    private void Export()
    {
      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.FileName = DocumentFilename;
      saveDlg.Title = "Export binary data to";
      saveDlg.Filter = "All Files|*.*";
      if ( DocumentInfo.Project != null )
      {
        saveDlg.InitialDirectory = DocumentInfo.Project.Settings.BasePath;
      }
      if ( saveDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
      {
        return;
      }
      GR.Memory.ByteBuffer data = DataFromHex();

      GR.IO.File.WriteAllBytes( saveDlg.FileName, data );
    }



    private void interleaveToolStripMenuItem_Click( object sender, EventArgs e )
    {
      DlgInterleaveData   dlgInterleave = new DlgInterleaveData( Core, DataFromHex() );
      if ( dlgInterleave.ShowDialog() == DialogResult.OK )
      {
        SetHexData( dlgInterleave.InterleavedData );
        SetModified();
      }
    }



    private void btnInterleave_Click( DecentForms.ControlBase Sender )
    {
      DlgInterleaveData   dlgInterleave = new DlgInterleaveData( Core, DataFromHex() );
      if ( dlgInterleave.ShowDialog() == DialogResult.OK )
      {
        SetHexData( dlgInterleave.InterleavedData );
        SetModified();
      }
    }



    public override bool LoadDocument()
    {
      if ( DocumentInfo.DocumentFilename == null )
      {
        return false;
      }
      try
      {
        GR.Memory.ByteBuffer    data = GR.IO.File.ReadAllBytes( DocumentInfo.FullPath );
        if ( data != null )
        {
          SetHexData( data );
        }
      }
      catch ( System.Exception ex )
      {
        System.Windows.Forms.MessageBox.Show( "Could not load binary file " + DocumentInfo.FullPath + ".\r\n" + ex.Message, "Could not load file" );
        return false;
      }
      SetUnmodified();
      if ( string.IsNullOrEmpty( m_FileWatcher.Path ) )
      {
        SetupWatcher();
        EnableFileWatcher();
      }
      return true;
    }



    private string FilterString( string Source )
    {
      return Source.Substring( 0, Source.Length - 1 );
    }



    protected override bool QueryFilename( string PreviousFilename, out string Filename )
    {
      Filename = "";

      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Save binary file as";
      saveDlg.Filter = FilterString( Types.Constants.FILEFILTER_BINARY_FILES + Types.Constants.FILEFILTER_ALL );
      saveDlg.FileName = GR.Path.GetFileName( PreviousFilename );
      if ( DocumentInfo.Project != null )
      {
        saveDlg.InitialDirectory = DocumentInfo.Project.Settings.BasePath;
      }
      if ( saveDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
      {
        return false;
      }

      Filename = saveDlg.FileName;
      return true;
    }



    protected override bool PerformSave( string FullPath )
    {
      return SaveDocumentData( FullPath, DataFromHex() );
    }



    private void btnUpsize_Click( DecentForms.ControlBase Sender )
    {
      var data = DataFromHex();

      GR.Memory.ByteBuffer    newData = new GR.Memory.ByteBuffer( data.Length * 2 );

      for ( int i = 0; i < data.Length; ++i )
      {
        byte    value = data.ByteAt( i );

        newData.SetU16At( i * 2, value );
      }
      SetHexData( newData );
    }



    private void btnDeleteNthByte_Click( DecentForms.ControlBase Sender )
    {
      int nthByte = GR.Convert.ToI32( editDeleteNthByte.Text );
      if ( nthByte <= 0 )
      {
        return;
      }
      var data = DataFromHex();

      int   curPos = 0;

      while ( curPos + nthByte < data.Length )
      {
        data = data.SubBuffer( 0, curPos + nthByte - 1 ) + data.SubBuffer( curPos + nthByte );
        curPos += nthByte - 1;
      }
      SetHexData( data );
    }



    private void btnPackNibbles_Click( DecentForms.ControlBase Sender )
    {
      var data = DataFromHex();
      var resultingData = new ByteBuffer( ( data.Length + 1 ) / 2 );

      int   curPos = 0;

      while ( curPos + 1 < data.Length )
      {
        var curByte = (byte)( data.ByteAt( curPos ) | ( data.ByteAt( curPos + 1 ) << 4 ) );

        resultingData.SetU8At( curPos / 2, curByte );
        curPos += 2;
      }
      SetHexData( resultingData );
    }



    private void btnSwizzle_Click( DecentForms.ControlBase Sender )
    {
      var data = DataFromHex();

      int   curPos = 0;

      while ( curPos < data.Length )
      {
        var curByte = data.ByteAt( curPos );

        curByte = (byte)( ( curByte << 4 ) | ( curByte >> 4 ) );

        data.SetU8At( curPos, curByte );
        ++curPos;
      }
      SetHexData( data );
    }



    private void btnDivide_Click( DecentForms.ControlBase Sender )
    {
      int   divisor = GR.Convert.ToI32( editDivideBy.Text );
      if ( divisor <= 0 )
      {
        return;
      }

      var data = DataFromHex();

      GR.Memory.ByteBuffer    newData = new GR.Memory.ByteBuffer( data.Length );

      for ( int i = 0; i < data.Length; ++i )
      {
        byte    value = data.ByteAt( i );

        newData.SetU8At( i, (byte)( value / divisor ) );
      }
      SetHexData( newData );
    }



    private void comboExportMethod_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( _ExportForm != null )
      {
        _ExportForm.Dispose();
        _ExportForm = null;
      }

      var item = (GR.Generic.Tupel<string, Type>)comboExportMethod.SelectedItem;
      if ( ( item == null )
      ||   ( item.second == null ) )
      {
        return;
      }
      _ExportForm = (ExportBinaryFormBase)Activator.CreateInstance( item.second, new object[] { Core } );
      _ExportForm.AutoScaleMode = AutoScaleMode.None;
      _ExportForm.Dock          = DockStyle.Fill;
      _ExportForm.Parent        = panelExport;
      _ExportForm.CreateControl();
    }



    private void btnExport_Click_1( DecentForms.ControlBase Sender )
    {
      var exportInfo = new ExportBinaryInfo()
      {
        Data = DataFromHex()
      };

      _ExportForm.HandleExport( exportInfo, DocumentInfo );
    }



    private void comboImportMethod_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( _ImportForm != null )
      {
        _ImportForm.Dispose();
        _ImportForm = null;
      }

      var item = (GR.Generic.Tupel<string, Type>)comboImportMethod.SelectedItem;
      if ( ( item == null )
      ||   ( item.second == null ) )
      {
        return;
      }
      _ImportForm = (ImportBinaryFormBase)Activator.CreateInstance( item.second, new object[] { Core } );
      _ImportForm.AutoScaleMode = AutoScaleMode.None;
      _ImportForm.Dock          = DockStyle.Fill;
      _ImportForm.Parent        = panelImport;
      _ImportForm.CreateControl();
    }



    private void btnImport_Click( DecentForms.ControlBase Sender )
    {
      //var undo1 = new Undo.UndoCharacterEditorCharChange( characterEditor, m_Charset, 0, m_Charset.TotalNumberOfCharacters );
      //var undo2 = new Undo.UndoCharacterEditorValuesChange( characterEditor, m_Charset );

      if ( _ImportForm.HandleImport( DocumentInfo, this, out ByteBuffer data ) )
      {
        //DocumentInfo.UndoManager.StartUndoGroup();
        //DocumentInfo.UndoManager.AddUndoTask( undo1, false );
        //DocumentInfo.UndoManager.AddUndoTask( undo2, false );
        SetHexData( data );
        SetModified();
      }
    }



  }
}

