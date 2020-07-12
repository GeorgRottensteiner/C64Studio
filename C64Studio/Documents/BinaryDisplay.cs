using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace C64Studio
{
  public partial class BinaryDisplay : BaseDocument
  {
    private ToolStripMenuItem m_MenuItemChangeOffset;

    public BinaryDisplay( StudioCore Core, GR.Memory.ByteBuffer WorkData, bool AllowEditing, bool FixedWidth )
    {
      this.Core = Core;
      DocumentInfo.UndoManager.MainForm = Core.MainForm;
      InitializeComponent();
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
      var formDisplayOffset = new FormSetOffset();
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
      return new Size( 660, 480 );
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

      if ( OpenFile( "Open Binary File", C64Studio.Types.Constants.FILEFILTER_ALL, out filename ) )
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
      Parser.ASMFileParser asmParser = new C64Studio.Parser.ASMFileParser();

      Parser.CompileConfig    config = new Parser.CompileConfig();
      config.TargetType = Types.CompileTargetType.PLAIN;
      config.OutputFile = "temp.bin";
      config.Assembler  = Types.AssemblerType.C64_STUDIO;

      string    temp = "* = $0801\n" + textBinaryData.Text;
      if ( ( asmParser.Parse( temp, null, config, null ) )
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

      textBinaryData.Text = Util.ToBASICData( data, curLineNumber, lineDelta );
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



    public override bool Load()
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



    public override bool SaveAs()
    {
      return SaveBinary( true );
    }



    public override bool Save()
    {
      return SaveBinary( false );
    }



    private string FilterString( string Source )
    {
      return Source.Substring( 0, Source.Length - 1 );
    }



    private bool SaveBinary( bool SaveAs )
    {
      string    saveFilename = "";

      if ( ( DocumentInfo.DocumentFilename == null )
      ||   ( SaveAs ) )
      {
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

        if ( SaveAs )
        {
          saveFilename = saveDlg.FileName;
        }
        else
        {
          if ( DocumentInfo.Project == null )
          {
            DocumentInfo.DocumentFilename = saveDlg.FileName;
          }
          else
          {
            SetDocumentFilename( GR.Path.RelativePathTo( saveDlg.FileName, false, System.IO.Path.GetFullPath( DocumentInfo.Project.Settings.BasePath ), true ) );
            DocumentInfo.Element.Name = System.IO.Path.GetFileName( DocumentInfo.DocumentFilename );
            DocumentInfo.Element.Filename = DocumentInfo.DocumentFilename;
            if ( DocumentInfo.Element.Settings.Count == 0 )
            {
              DocumentInfo.Element.Settings["Default"] = new ProjectElement.PerConfigSettings();
            }
          }
          Text = System.IO.Path.GetFileNameWithoutExtension( DocumentInfo.DocumentFilename ) + "*";
          SetupWatcher();
          saveFilename = DocumentInfo.FullPath;
        }
      }
      else
      {
        saveFilename = DocumentInfo.FullPath;
      }

      if ( !DoSave( saveFilename ) )
      {
        return false;
      }
      if ( !SaveAs )
      {
        SetUnmodified();
      }
      return true;
    }



    private bool DoSave( string Filename )
    {
      try
      {
        DisableFileWatcher();

        GR.Memory.ByteBuffer data = DataFromHex();
        GR.IO.File.WriteAllBytes( Filename, data );
      }
      catch ( System.IO.IOException ex )
      {
        System.Windows.Forms.MessageBox.Show( "Could not save file " + Filename + ".\r\n" + ex.ToString(), "Could not save file" );
        EnableFileWatcher();
        return false;
      }
      EnableFileWatcher();
      return true;
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

      textBinaryData.Text = Util.ToBASICHexData( data, curLineNumber, lineDelta );
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



  }
}

