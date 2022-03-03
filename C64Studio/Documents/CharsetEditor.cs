using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using C64Studio.Formats;
using C64Studio.Types;
using GR.Memory;
using RetroDevStudio;
using RetroDevStudio.Types;
using WeifenLuo.WinFormsUI.Docking;

namespace C64Studio
{
  public partial class CharsetEditor : BaseDocument
  {
    private enum ColorType
    {
      BACKGROUND = 0,
      MULTICOLOR_1,
      MULTICOLOR_2,
      CHAR_COLOR
    }

    public Formats.CharsetProject       m_Charset = new C64Studio.Formats.CharsetProject();



    public override DocumentInfo DocumentInfo
    {
      get
      {
        return base.DocumentInfo;
      }
      set
      {
        base.DocumentInfo = value;
        characterEditor.UndoManager = DocumentInfo.UndoManager;
      }
    }



    public CharsetEditor( StudioCore Core )
    {
      this.Core = Core;
      DocumentInfo.Type = ProjectElement.ElementType.CHARACTER_SET;
      DocumentInfo.UndoManager.MainForm = Core.MainForm;
      m_IsSaveable = true;
      InitializeComponent();
      characterEditor.Core = Core;

      GR.Image.DPIHandler.ResizeControlsForDPI( this );

      characterEditor.UndoManager = DocumentInfo.UndoManager;
      characterEditor.Core = Core;

      for ( int i = 0; i < 256; ++i )
      {
        PaletteManager.ApplyPalette( m_Charset.Characters[i].Tile.Image );
      }

      comboExportRange.Items.Add( "All" );
      comboExportRange.Items.Add( "Selection" );
      comboExportRange.Items.Add( "Range" );
      comboExportRange.SelectedIndex = 0;

      checkExportToDataIncludeRes.Checked = true;
      checkExportToDataWrap.Checked = true;

      CreateDefaultUppercaseCharset();

      Modified = false;
    }



    internal void CharacterChanged( int charIndex )
    {
      characterEditor.CharacterChanged( charIndex, 1 );
    }



    internal void SetCharsetProject( CharsetProject Charset )
    {
      m_Charset = Charset;
      CharsetWasImported();
    }



    private new bool Modified
    {
      get
      {
        return base.Modified;
      }
      set
      {
        if ( value )
        {
          SetModified();
        }
        else
        {
          SetUnmodified();
        }
        saveCharsetProjectToolStripMenuItem.Enabled = Modified;
      }
    }



    private void openToolStripMenuItem_Click( object sender, EventArgs e )
    {
      string    filename;

      if ( OpenFile( "Open Charset Project", C64Studio.Types.Constants.FILEFILTER_CHARSET_PROJECT + C64Studio.Types.Constants.FILEFILTER_ALL, out filename ) )
      {
        OpenProject( filename );
      }
    }



    public void Clear()
    {
      for ( int i = 0; i < m_Charset.TotalNumberOfCharacters; ++i )
      {
        m_Charset.Characters[i].Tile.CustomColor = 0;
        for ( int j = 0; j < m_Charset.Characters[i].Tile.Data.Length; ++j )
        {
          m_Charset.Characters[i].Tile.Data.SetU8At( j, 0 );
        }
      }
      DocumentInfo.DocumentFilename = "";

      m_Charset.Categories.Clear();
      m_Charset.Categories.Add( "Uncategorized" );

      characterEditor.CharsetUpdated( m_Charset );
    }



    public override bool Load()
    {
      if ( string.IsNullOrEmpty( DocumentInfo.DocumentFilename ) )
      {
        return false;
      }
      try
      {
        if ( IsBinaryFile() )
        {
          GR.Memory.ByteBuffer charData = GR.IO.File.ReadAllBytes( DocumentInfo.FullPath );

          ImportFromData( charData );
        }
        else
        {
          OpenProject( DocumentInfo.FullPath );
        }
      }
      catch ( System.IO.IOException ex )
      {
        System.Windows.Forms.MessageBox.Show( "Could not load charset project file " + DocumentInfo.FullPath + ".\r\n" + ex.Message, "Could not load file" );
        return false;
      }
      SetUnmodified();
      return true;
    }



    private bool IsBinaryFile()
    {
      string extension = System.IO.Path.GetExtension( DocumentInfo.FullPath.ToString() ).ToUpper();

      return ( extension == ".CHR" );
    }



    public void OpenProject( string Filename )
    {
      DisableFileWatcher();

      Clear();

      GR.Memory.ByteBuffer    projectFile = GR.IO.File.ReadAllBytes( Filename );

      DocumentInfo.DocumentFilename = Filename;

      OpenProject( projectFile );

      EnableFileWatcher();
    }



    public void OpenProject( GR.Memory.ByteBuffer ProjectData )
    {
      if ( !m_Charset.ReadFromBuffer( ProjectData ) )
      {
        return;
      }
      CharsetWasImported();
    }



    public void CharsetWasImported()
    {
      characterEditor.CharsetUpdated( m_Charset );

      editCharactersCount.Text        = m_Charset.ExportNumCharacters.ToString();
      editCharactersFrom.Text         = m_Charset.ExportStartCharacter.ToString();

      Modified = false;

      saveCharsetProjectToolStripMenuItem.Enabled = true;
      closeCharsetProjectToolStripMenuItem.Enabled = true;
    }



    public override GR.Memory.ByteBuffer SaveToBuffer()
    {
      return m_Charset.SaveToBuffer();
    }



    protected override bool QueryFilename( out string Filename )
    {
      Filename = "";
      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Save Charset Project as";
      saveDlg.Filter = "Charset Projects|*.charsetproject|All Files|*.*";
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
      m_Charset.Name      = DocumentInfo.DocumentFilename;
      m_Charset.UsedTiles = GR.Convert.ToU32( editCharactersFrom.Text );

      if ( IsBinaryFile() )
      {
        // save binary only!
        GR.Memory.ByteBuffer charSet = new GR.Memory.ByteBuffer();
        charSet.Reserve( m_Charset.TotalNumberOfCharacters * Lookup.NumBytesOfSingleCharacterBitmap( m_Charset.Mode ) );

        for ( int i = 0; i < m_Charset.TotalNumberOfCharacters; ++i )
        {
          charSet.Append( m_Charset.Characters[i].Tile.Data );
        }

        return SaveDocumentData( FullPath, charSet );
      }
      GR.Memory.ByteBuffer projectFile = SaveToBuffer();

      return SaveDocumentData( FullPath, projectFile );
    }



    private void closeCharsetProjectToolStripMenuItem_Click( object sender, EventArgs e )
    {
      if ( DocumentInfo.DocumentFilename == "" )
      {
        return;
      }
      if ( Modified )
      {
        DialogResult doSave = MessageBox.Show( "There are unsaved changes in your character set. Save now?", "Save changes?", MessageBoxButtons.YesNoCancel );
        if ( doSave == DialogResult.Cancel )
        {
          return;
        }
        if ( doSave == DialogResult.Yes )
        {
          Save( BaseDocument.SaveMethod.SAVE );
        }
      }
      Clear();
      DocumentInfo.DocumentFilename = "";
      Modified = false;

      closeCharsetProjectToolStripMenuItem.Enabled = false;
      saveCharsetProjectToolStripMenuItem.Enabled = false;
    }



    private void saveCharsetProjectToolStripMenuItem_Click( object sender, EventArgs e )
    {
      Save( SaveMethod.SAVE );
    }



    private void btnExportCharset_Click( object sender, EventArgs e )
    {
      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.FileName = m_Charset.ExportFilename;
      saveDlg.Title = "Export Charset to";
      saveDlg.Filter = "Charset|*.chr|All Files|*.*";
      if ( DocumentInfo.Project != null )
      {
        saveDlg.InitialDirectory = DocumentInfo.Project.Settings.BasePath;
      }
      if ( saveDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
      {
        return;
      }
      if ( m_Charset.ExportFilename != saveDlg.FileName )
      {
        m_Charset.ExportFilename = saveDlg.FileName;
        Modified = true;
      }
      GR.Memory.ByteBuffer charSet = new GR.Memory.ByteBuffer();

      List<int>     exportIndices = ListOfExportIndices();
      foreach ( int i in exportIndices )
      {
        charSet.Append( m_Charset.Characters[i].Tile.Data );
      }
      if ( checkPrefixLoadAddress.Checked )
      {
        ushort address = GR.Convert.ToU16( editPrefixLoadAddress.Text, 16 );

        var addressData = new ByteBuffer();
        addressData.AppendU16( address );
        charSet = addressData + charSet;
      }
      GR.IO.File.WriteAllBytes( m_Charset.ExportFilename, charSet );
    }



    private List<int> ListOfExportIndices()
    {
      List<int>   listIndices = new List<int>();

      switch ( comboExportRange.SelectedIndex )
      {
        case 0:
          // all
          for ( int i = 0; i < 256; ++i )
          {
            listIndices.Add( i );
          }
          break;
        case 1:
          // selection
          listIndices.AddRange( characterEditor.SelectedIndices );
          break;
        case 2:
          // range
          for ( int i = 0; i < m_Charset.ExportNumCharacters; ++i )
          {
            listIndices.Add( m_Charset.ExportStartCharacter + i );
          }
          break;
      }
      return listIndices;
    }



    private void btnExportCharsetToData_Click( object sender, EventArgs e )
    {
      int wrapByteCount = GetExportWrapCount();
      if ( wrapByteCount <= 0 )
      {
        wrapByteCount = 8;
      }
      string prefix = editPrefix.Text;

      List<int>     exportIndices = ListOfExportIndices();

      GR.Memory.ByteBuffer charSet = new GR.Memory.ByteBuffer();
      foreach ( int index in exportIndices )
      {
        charSet.Append( m_Charset.Characters[index].Tile.Data );
      }

      bool wrapData = checkExportToDataWrap.Checked;
      bool prefixRes = checkExportToDataIncludeRes.Checked;

      string    resultText = "CHARS" + System.Environment.NewLine;
      resultText += Util.ToASMData( charSet, wrapData, wrapByteCount, prefixRes ? prefix : "" );
      if ( checkIncludeColor.Checked )
      {
        resultText += System.Environment.NewLine + "COLORS" + System.Environment.NewLine;

        GR.Memory.ByteBuffer    colorData = new GR.Memory.ByteBuffer();
        foreach ( int index in exportIndices )
        {
          colorData.AppendU8( (byte)m_Charset.Characters[index].Tile.CustomColor );
        }
        resultText += Util.ToASMData( colorData, wrapData, wrapByteCount, prefixRes ? prefix : "" );
      }

      editDataExport.Text = resultText;
    }



    private int GetExportWrapCount()
    {
      if ( checkExportToDataWrap.Checked )
      {
        return GR.Convert.ToI32( editWrapByteCount.Text );
      }
      return 80;
    }



    private void checkExportToDataWrap_CheckedChanged( object sender, EventArgs e )
    {
      editWrapByteCount.Enabled = checkExportToDataWrap.Checked;
    }



    private void checkExportToDataIncludeRes_CheckedChanged( object sender, EventArgs e )
    {
      editPrefix.Enabled = checkExportToDataIncludeRes.Checked;
    }



    private void btnImportCharset_Click( object sender, EventArgs e )
    {
      string filename;

      //Clear();
      if ( OpenFile( "Open charset", C64Studio.Types.Constants.FILEFILTER_CHARSET + C64Studio.Types.Constants.FILEFILTER_CHARSET_CHARPAD + C64Studio.Types.Constants.FILEFILTER_ALL, out filename ) )
      {
        if ( System.IO.Path.GetExtension( filename ).ToUpper() == ".CHARSETPROJECT" )
        {
          // a project
          GR.Memory.ByteBuffer projectFile = GR.IO.File.ReadAllBytes( filename );

          C64Studio.Formats.CharsetProject project = new C64Studio.Formats.CharsetProject();
          if ( !project.ReadFromBuffer( projectFile ) )
          {
            return;
          }
          m_Charset.Colors = new ColorSettings( project.Colors );
          m_Charset.ExportNumCharacters = project.ExportNumCharacters;
          m_Charset.ShowGrid = project.ShowGrid;

          for ( int i = 0; i < m_Charset.TotalNumberOfCharacters; ++i )
          {
            m_Charset.Characters[i].Tile.CustomColor = project.Characters[i].Tile.CustomColor;
            m_Charset.Characters[i].Tile.Data = new GR.Memory.ByteBuffer( project.Characters[i].Tile.Data );
          }

          editCharactersFrom.Text = m_Charset.ExportNumCharacters.ToString();

          characterEditor.CharsetUpdated( m_Charset );

          Modified = true;
          return;
        }
        else if ( System.IO.Path.GetExtension( filename ).ToUpper() == ".CTM" )
        {
          // a charpad project file
          GR.Memory.ByteBuffer projectFile = GR.IO.File.ReadAllBytes( filename );

          Formats.CharpadProject    cpProject = new C64Studio.Formats.CharpadProject();
          if ( !cpProject.LoadFromFile( projectFile ) )
          {
            return;
          }

          m_Charset.Colors.BackgroundColor = cpProject.BackgroundColor;
          m_Charset.Colors.MultiColor1     = cpProject.MultiColor1;
          m_Charset.Colors.MultiColor2     = cpProject.MultiColor2;

          m_Charset.ExportNumCharacters = cpProject.NumChars;
          if ( m_Charset.ExportNumCharacters > 256 )
          {
            m_Charset.ExportNumCharacters = 256;
          }
          for ( int charIndex = 0; charIndex < m_Charset.ExportNumCharacters; ++charIndex )
          {
            m_Charset.Characters[charIndex].Tile.Data = cpProject.Characters[charIndex].Data;
            m_Charset.Characters[charIndex].Tile.CustomColor = cpProject.Characters[charIndex].Color;
          }

          editCharactersFrom.Text = m_Charset.ExportNumCharacters.ToString();
          characterEditor.CharsetUpdated( m_Charset );
          SetModified();
          return;
        }
        // treat as binary .chr file
        GR.Memory.ByteBuffer charData = GR.IO.File.ReadAllBytes( filename );

        ImportFromData( charData );
      }
    }



    private void ImportFromData( ByteBuffer CharData )
    {
      int     numBytesPerChar = Lookup.NumBytesOfSingleCharacterBitmap( m_Charset.Mode );

      if ( CharData.Length == numBytesPerChar * m_Charset.TotalNumberOfCharacters + 2 )
      {
        // assume 2 bytes load address 
        CharData = CharData.SubBuffer( 2 );
      }
      int charsToImport = (int)CharData.Length / numBytesPerChar;
      if ( charsToImport > m_Charset.TotalNumberOfCharacters )
      {
        charsToImport = m_Charset.TotalNumberOfCharacters;
      }
      // TODO - Format!
      for ( int i = 0; i < charsToImport; ++i )
      {
        for ( int j = 0; j < numBytesPerChar; ++j )
        {
          m_Charset.Characters[i].Tile.Data.SetU8At( j, CharData.ByteAt( i * numBytesPerChar + j ) );
        }
        m_Charset.Characters[i].Tile.CustomColor = 1;
      }
      characterEditor.CharsetUpdated( m_Charset );
      SetModified();
    }



    private void CreateDefaultUppercaseCharset()
    {
      for ( int i = 0; i < 256; ++i )
      {
        for ( int j = 0; j < 8; ++j )
        {
          m_Charset.Characters[i].Tile.Data.SetU8At( j, ConstantData.UpperCaseCharsetC64.ByteAt( i * 8 + j ) );
        }
        m_Charset.Characters[i].Tile.CustomColor = 1;
      }
      characterEditor.CharsetUpdated( m_Charset );
      SetModified();
    }



    private void btnDefaultLowercase_Click( object sender, EventArgs e )
    {
      for ( int i = 0; i < 256; ++i )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharsetCharChange( this, m_Charset, i ), i == 0 );
      }
      for ( int i = 0; i < 256; ++i )
      {
        for ( int j = 0; j < 8; ++j )
        {
          m_Charset.Characters[i].Tile.Data.SetU8At( j, ConstantData.LowerCaseCharsetC64.ByteAt( i * 8 + j ) );
        }
        m_Charset.Characters[i].Tile.CustomColor = 1;
      }
      characterEditor.CharsetUpdated( m_Charset );
      SetModified();
    }



    private void btnExportCharsetToImage_Click( object sender, EventArgs e )
    {
      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Export Characters to Image";
      saveDlg.Filter = Core.MainForm.FilterString( C64Studio.Types.Constants.FILEFILTER_IMAGE_FILES );
      if ( saveDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
      {
        return;
      }

      GR.Image.MemoryImage    targetImg = new GR.Image.MemoryImage( 128, 128, m_Charset.Characters[0].Tile.Image.PixelFormat );
      PaletteManager.ApplyPalette( targetImg );

      List<int>     exportIndices = ListOfExportIndices();

      foreach ( int i in exportIndices )
      {
        m_Charset.Characters[i].Tile.Image.DrawTo( targetImg, ( i % 16 ) * 8, ( i / 16 ) * 8 );
      }
      System.Drawing.Bitmap bmpTarget = targetImg.GetAsBitmap();
      bmpTarget.Save( saveDlg.FileName, System.Drawing.Imaging.ImageFormat.Png );
    }



    private void btnImportCharsetFromFile_Click( object sender, EventArgs e )
    {
      string filename;

      if ( !OpenFile( "Import Charset from Image", C64Studio.Types.Constants.FILEFILTER_IMAGE_FILES, out filename ) )
      {
        return;
      }

      GR.Image.FastImage imgClip = Core.Imaging.LoadImageFromFile( filename );

      characterEditor.PasteImage( filename, imgClip, false );
    }



    private void editStartCharacters_TextChanged( object sender, EventArgs e )
    {
      int   startChar = GR.Convert.ToI32( editCharactersFrom.Text );
      if ( ( startChar <= 0 )
      ||   ( startChar > 256 ) )
      {
        startChar = 0;
        editCharactersFrom.Text = startChar.ToString();
      }
      if ( m_Charset.ExportStartCharacter != startChar )
      {
        m_Charset.ExportStartCharacter = startChar;
        Modified = true;
        if ( startChar + m_Charset.ExportNumCharacters > 256 )
        {
          m_Charset.ExportNumCharacters = 256 - startChar;
          editCharactersCount.Text = m_Charset.ExportNumCharacters.ToString();
        }
      }
    }



    private void editUsedCharacters_TextChanged( object sender, EventArgs e )
    {
      int   numChars = GR.Convert.ToI32( editCharactersCount.Text );
      if ( ( numChars <= 0 )
      ||   ( numChars > 256 ) )
      {
        numChars = 256 - m_Charset.ExportStartCharacter;
        editCharactersCount.Text = numChars.ToString();
      }
      if ( m_Charset.ExportNumCharacters != numChars )
      {
        m_Charset.ExportNumCharacters = numChars;
        Modified = true;
      }
    }



    private void btnReseatCategory_Click( object sender, EventArgs e )
    {
    }



    private void comboExportRange_SelectedIndexChanged( object sender, EventArgs e )
    {
      labelCharactersFrom.Enabled = ( comboExportRange.SelectedIndex == 2 );
      editCharactersFrom.Enabled = ( comboExportRange.SelectedIndex == 2 );
      labelCharactersTo.Enabled = ( comboExportRange.SelectedIndex == 2 );
      editCharactersCount.Enabled = ( comboExportRange.SelectedIndex == 2 );
    }



    private void editDataExport_KeyPress( object sender, KeyPressEventArgs e )
    {
      if ( ( System.Windows.Forms.Control.ModifierKeys == Keys.Control ) 
      &&   ( e.KeyChar == 1 ) )
      {
        editDataExport.SelectAll();
        e.Handled = true;
      }
    }



    private void btnExportCharsetToBASIC_Click( object sender, EventArgs e )
    {
      int startLine = GR.Convert.ToI32( editExportBASICLineNo.Text );
      if ( ( startLine < 0 )
      ||   ( startLine > 63999 ) )
      {
        startLine = 10;
      }
      int lineOffset = GR.Convert.ToI32( editExportBASICLineOffset.Text );
      if ( ( lineOffset < 0 )
      ||   ( lineOffset > 63999 ) )
      {
        startLine = 10;
      }
      int wrapByteCount = GetExportWrapCount();

      List<int>     exportIndices = ListOfExportIndices();

      GR.Memory.ByteBuffer charSet = new GR.Memory.ByteBuffer();
      foreach ( int index in exportIndices )
      {
        charSet.Append( m_Charset.Characters[index].Tile.Data );
      }

      string    resultText = Util.ToBASICData( charSet, startLine, lineOffset, wrapByteCount );

      editDataExport.Text = resultText;
    }



    private void btnImportCharsetFromASM_Click( object sender, EventArgs e )
    {
      ImportFromData( Util.FromASMData( editDataImport.Text ) );
    }



    private void editDataImport_KeyPress( object sender, KeyPressEventArgs e )
    {
      if ( ( System.Windows.Forms.Control.ModifierKeys == Keys.Control )
      &&   ( e.KeyChar == 1 ) )
      {
        editDataImport.SelectAll();
        e.Handled = true;
      }
    }



    private void btnClearImportData_Click( object sender, EventArgs e )
    {
      editDataImport.Text = "";
    }



    public override bool ApplyFunction( Function Function )
    {
      if ( !characterEditor.EditorFocused )
      {
        return false;
      }
      switch ( Function )
      {
        case Function.GRAPHIC_ELEMENT_MIRROR_H:
          characterEditor.MirrorX();
          return true;
        case Function.GRAPHIC_ELEMENT_MIRROR_V:
          characterEditor.MirrorY();
          return true;
        case Function.GRAPHIC_ELEMENT_SHIFT_D:
          characterEditor.ShiftDown();
          return true;
        case Function.GRAPHIC_ELEMENT_SHIFT_U:
          characterEditor.ShiftUp();
          return true;
        case Function.GRAPHIC_ELEMENT_SHIFT_L:
          characterEditor.ShiftLeft();
          return true;
        case Function.GRAPHIC_ELEMENT_SHIFT_R:
          characterEditor.ShiftRight();
          return true;
        case Function.GRAPHIC_ELEMENT_ROTATE_L:
          characterEditor.RotateLeft();
          return true;
        case Function.GRAPHIC_ELEMENT_ROTATE_R:
          characterEditor.RotateRight();
          return true;
        case Function.GRAPHIC_ELEMENT_INVERT:
          characterEditor.Invert();
          return true;
        case Function.GRAPHIC_ELEMENT_PREVIOUS:
          characterEditor.Previous();
          return true;
        case Function.GRAPHIC_ELEMENT_NEXT:
          characterEditor.Next();
          return true;
        case Function.GRAPHIC_ELEMENT_CUSTOM_COLOR:
          characterEditor.CustomColor();
          return true;
        case Function.GRAPHIC_ELEMENT_MULTI_COLOR_1:
          characterEditor.MultiColor1();
          return true;
        case Function.GRAPHIC_ELEMENT_MULTI_COLOR_2:
          characterEditor.MultiColor2();
          return true;
        case Function.GRAPHIC_ELEMENT_BACKGROUND_COLOR:
          characterEditor.BackgroundColor();
          return true;
      }
      return base.ApplyFunction( Function );
    }



    private void btnExportCharsetToBASICHex_Click( object sender, EventArgs e )
    {
      int startLine = GR.Convert.ToI32( editExportBASICLineNo.Text );
      if ( ( startLine < 0 )
      ||   ( startLine > 63999 ) )
      {
        startLine = 10;
      }
      int lineOffset = GR.Convert.ToI32( editExportBASICLineOffset.Text );
      if ( ( lineOffset < 0 )
      ||   ( lineOffset > 63999 ) )
      {
        startLine = 10;
      }

      List<int>     exportIndices = ListOfExportIndices();

      GR.Memory.ByteBuffer charSet = new GR.Memory.ByteBuffer();
      foreach ( int index in exportIndices )
      {
        charSet.Append( m_Charset.Characters[index].Tile.Data );
      }

      string    resultText = Util.ToBASICHexData( charSet, startLine, lineOffset );

      editDataExport.Text = resultText;
    }



    private void characterEditor_Modified()
    {
      SetModified();
    }



    private void btnImportCharsetFromBASIC_Click( object sender, EventArgs e )
    {
      ImportFromData( Util.FromBASIC( editDataImport.Text ) );
      Modified = true;
    }



    private void btnImportCharsetFromBASICHex_Click( object sender, EventArgs e )
    {
      ImportFromData( Util.FromBASICHex( editDataImport.Text ) );
      Modified = true;
    }



    private void editPrefixLoadAddress_TextChanged( object sender, EventArgs e )
    {
      checkPrefixLoadAddress.Checked = true;
    }



    private void c64UppercaseToolStripMenuItem_Click( object sender, EventArgs e )
    {
      SetDefaultCharset( ConstantData.UpperCaseCharsetC64 );
    }



    private void SetDefaultCharset( ByteBuffer CharsetData )
    {
      for ( int i = 0; i < m_Charset.TotalNumberOfCharacters; ++i )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharsetCharChange( this, m_Charset, i ), i == 0 );
      }

      // TODO - and with mega65??
      for ( int i = 0; i < m_Charset.TotalNumberOfCharacters; ++i )
      {
        for ( int j = 0; j < 8; ++j )
        {
          m_Charset.Characters[i].Tile.Data.SetU8At( j, CharsetData.ByteAt( i * 8 + j ) );
        }
        m_Charset.Characters[i].Tile.CustomColor = 1;
      }
      characterEditor.CharsetUpdated( m_Charset );
      SetModified();
    }



    private void c64LowercaseToolStripMenuItem_Click( object sender, EventArgs e )
    {
      SetDefaultCharset( ConstantData.LowerCaseCharsetC64 );
    }



    private void viC20UppercaseToolStripMenuItem_Click( object sender, EventArgs e )
    {
      SetDefaultCharset( ConstantData.UpperCaseCharsetViC20 );
    }



    private void viC20LowercaseToolStripMenuItem_Click( object sender, EventArgs e )
    {
      SetDefaultCharset( ConstantData.LowerCaseCharsetViC20 );
    }



    private void btnDefaultCharset_Click( object sender, EventArgs e )
    {
      contextMenuDefaultCharsets.Show( btnDefaultCharset, new Point( 0, btnDefaultCharset.Height ) );
    }



  }
}


