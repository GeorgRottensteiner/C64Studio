using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using GR.Image;
using GR.Memory;
using RetroDevStudio.Dialogs;



namespace RetroDevStudio.Documents
{
  public partial class BinaryDisplay : BaseDocument
  {
    private ToolStripMenuItem m_MenuItemChangeOffset;

    public BinaryDisplay( StudioCore Core, GR.Memory.ByteBuffer WorkData, bool AllowEditing, bool FixedWidth )
    {
      this.Core = Core;
      DocumentInfo.UndoManager.MainForm = Core.MainForm;
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



    private void btnToText_Click( object sender, EventArgs e )
    {
      GR.Memory.ByteBuffer data = DataFromHex();

      int     wrapCount = GR.Convert.ToI32( editWrapCount.Text );
      if ( wrapCount <= 0 )
      {
        wrapCount = 40;
      }

      textBinaryData.Text = Util.ToASMData( data, wrapCount > 0, wrapCount, "!byte " );
    }



    private void btnFromASM_Click( object sender, EventArgs e )
    {
      Parser.ASMFileParser asmParser = new RetroDevStudio.Parser.ASMFileParser();

      Parser.CompileConfig    config = new Parser.CompileConfig();
      config.TargetType = Types.CompileTargetType.PLAIN;
      config.OutputFile = "temp.bin";
      config.Assembler  = Types.AssemblerType.C64_STUDIO;

      string    temp = "* = $0801\n" + textBinaryData.Text;
      if ( ( asmParser.Parse( temp, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) )
      &&   ( asmParser.Assemble( config ) ) )
      {
        SetHexData( asmParser.AssembledOutput.Assembly );
      }
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



    private void btnToBASIC_Click( object sender, EventArgs e )
    {
      GR.Memory.ByteBuffer data = DataFromHex();
      bool insertSpaces = checkInsertSpaces.Checked;

      int         lineDelta = GR.Convert.ToI32( editToBASICLineDelta.Text );
      if ( lineDelta <= 0 )
      {
        lineDelta = 1;
      }

      int         curLineNumber = GR.Convert.ToI32( editToBASICStartLine.Text );
      if ( curLineNumber < 0 )
      {
        curLineNumber = 0;
      }
      textBinaryData.Text = Util.ToBASICData( data, curLineNumber, lineDelta, GR.Convert.ToI32( editWrapCount.Text ), GR.Convert.ToI32( editWrapCharsCount.Text ), insertSpaces );
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



    private void btnInterleave_Click( object sender, EventArgs e )
    {
      DlgInterleaveData   dlgInterleave = new DlgInterleaveData( Core, DataFromHex() );
      if ( dlgInterleave.ShowDialog() == DialogResult.OK )
      {
        SetHexData( dlgInterleave.InterleavedData );
        SetModified();
      }
    }



    private void btnImport_Click( object sender, EventArgs e )
    {
      Import();
    }



    private void btnExport_Click( object sender, EventArgs e )
    {
      Export();
    }



    private void btnToHex_Click( object sender, EventArgs e )
    {
      GR.Memory.ByteBuffer data = DataFromHex();

      textBinaryData.Text = data.ToString();
    }



    private void btnFromHex_Click( object sender, EventArgs e )
    {
      string    binaryText = textBinaryData.Text.Replace( " ", "" ).Replace( "\r", "" ).Replace( "\n", "" );

      GR.Memory.ByteBuffer    data = new GR.Memory.ByteBuffer( binaryText );
      SetHexData( data );
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



    protected override bool QueryFilename( out string Filename )
    {
      Filename = "";

      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Save binary file as";
      saveDlg.Filter = FilterString( Types.Constants.FILEFILTER_BINARY_FILES + Types.Constants.FILEFILTER_ALL );
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



    private void btnFromBASIC_Click( object sender, EventArgs e )
    {
      string[]  lines = textBinaryData.Text.Split( new char[] { '\n' } );

      GR.Memory.ByteBuffer    resultData = new GR.Memory.ByteBuffer();

      for ( int i = 0; i < lines.Length; ++i )
      {
        string    cleanLine = lines[i].Trim().ToUpper();

        int   dataPos = cleanLine.IndexOf( "DATA" );
        if ( dataPos != -1 )
        {
          int     commaPos = -1;
          int     byteStartPos = dataPos + 4;

          do
          {
            commaPos = cleanLine.IndexOf( ',', byteStartPos );
            if ( commaPos == -1 )
            {
              commaPos = cleanLine.Length;
            }
            int     value = GR.Convert.ToI32( cleanLine.Substring( byteStartPos, commaPos - byteStartPos ).Trim() );
            resultData.AppendU8( (byte)value );

            byteStartPos = commaPos + 1;
          }
          while ( commaPos < cleanLine.Length );
        }
      }

      SetHexData( resultData );
    }



    private void btnToBASICHex_Click( object sender, EventArgs e )
    {
      GR.Memory.ByteBuffer data = DataFromHex();
      bool insertSpaces = checkInsertSpaces.Checked;

      int         lineDelta = GR.Convert.ToI32( editToBASICLineDelta.Text );
      if ( lineDelta <= 0 )
      {
        lineDelta = 1;
      }

      int         curLineNumber = GR.Convert.ToI32( editToBASICStartLine.Text );
      if ( curLineNumber < 0 )
      {
        curLineNumber = 0;
      }

      textBinaryData.Text = Util.ToBASICHexData( data, curLineNumber, lineDelta, GR.Convert.ToI32( editWrapCount.Text ), GR.Convert.ToI32( editWrapCharsCount.Text ), insertSpaces );
    }



    private void btnFromBASICHex_Click( object sender, EventArgs e )
    {
      SetHexData( Util.FromBASICHex( textBinaryData.Text ) );
    }



    private void btnUpsize_Click( object sender, EventArgs e )
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



    private void textBinaryData_PreviewKeyDown( object sender, PreviewKeyDownEventArgs e )
    {
      if ( e.KeyData == ( Keys.A | Keys.Control ) )
      {
        textBinaryData.SelectAll();
      }
    }



    private void btnDeleteNthByte_Click( object sender, EventArgs e )
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



    private void btnPackNibbles_Click( object sender, EventArgs e )
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



    private void btnSwizzle_Click( object sender, EventArgs e )
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



    private void btnDivide_Click( object sender, EventArgs e )
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



  }
}

