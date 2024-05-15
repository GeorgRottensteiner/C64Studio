using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using RetroDevStudio.Controls;
using RetroDevStudio.Formats;
using RetroDevStudio.Types;
using GR.Memory;
using RetroDevStudio;
using WeifenLuo.WinFormsUI.Docking;

namespace RetroDevStudio.Documents
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

    public Formats.CharsetProject       m_Charset = new RetroDevStudio.Formats.CharsetProject();

    private System.Drawing.Font         m_DefaultOutputFont = null;
    private ExportCharsetFormBase       m_ExportForm = null;
    private ImportCharsetFormBase       m_ImportForm = null;



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
      SuspendLayout();
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

      comboExportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "as assembly", typeof( ExportCharsetAsAssembly ) ) );
      comboExportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "to binary file", typeof( ExportCharsetAsBinaryFile ) ) );
      comboExportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "to image file", typeof( ExportCharsetAsImageFile ) ) );
      comboExportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "to image (clipboard)", typeof( ExportCharsetAsImage ) ) );
      comboExportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "as BASIC DATA statements", typeof( ExportCharsetAsBASICData ) ) );
      comboExportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "as BASIC CHARDEF statements", typeof( ExportCharsetAsBASICChardef ) ) );
      comboExportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "as S-BASIC FCM CHARDEF statements", typeof( ExportCharsetAsSBASICFCMChardef ) ) );
      comboExportMethod.SelectedIndex = 0;
      m_DefaultOutputFont = editDataExport.Font;

      comboImportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "from character set/binary file", typeof( ImportCharsetFromBinaryFile ) ) );
      comboImportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "from image file", typeof( ImportCharsetFromImageFile ) ) );
      comboImportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "from assembly", typeof( ImportCharsetFromASM ) ) );
      comboImportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "from BASIC DATA statements", typeof( ImportCharsetFromBASICDATA ) ) );
      comboImportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "from HEX", typeof( ImportCharsetFromHex ) ) );
      comboImportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "set default character sets", typeof( ImportCharsetFromDefault ) ) );
      comboImportMethod.SelectedIndex = 0;

      CreateDefaultUppercaseCharset();

      Modified = false;
      ResumeLayout();
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

      if ( OpenFile( "Open Charset Project", RetroDevStudio.Types.Constants.FILEFILTER_CHARSET_PROJECT + RetroDevStudio.Types.Constants.FILEFILTER_ALL, out filename ) )
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



    public override bool LoadDocument()
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
        var endButtons = MessageBoxButtons.YesNoCancel;
        if ( Core.ShuttingDown )
        {
          endButtons = MessageBoxButtons.YesNo;
        }

        DialogResult doSave = MessageBox.Show( "There are unsaved changes in your character set. Save now?", "Save changes?", endButtons );
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



    private List<int> ListOfExportIndices()
    {
      List<int>   listIndices = new List<int>();

      switch ( comboExportRange.SelectedIndex )
      {
        case 0:
          // all
          for ( int i = 0; i < m_Charset.TotalNumberOfCharacters; ++i )
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



    public void ImportFromData( ByteBuffer CharData )
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



    private void editStartCharacters_TextChanged( object sender, EventArgs e )
    {
      int   startChar = GR.Convert.ToI32( editCharactersFrom.Text );
      if ( ( startChar <= 0 )
      ||   ( startChar > m_Charset.TotalNumberOfCharacters ) )
      {
        startChar = 0;
        editCharactersFrom.Text = startChar.ToString();
      }
      if ( m_Charset.ExportStartCharacter != startChar )
      {
        m_Charset.ExportStartCharacter = startChar;
        Modified = true;
        if ( startChar + m_Charset.ExportNumCharacters > m_Charset.TotalNumberOfCharacters )
        {
          m_Charset.ExportNumCharacters = m_Charset.TotalNumberOfCharacters - startChar;
          editCharactersCount.Text = m_Charset.ExportNumCharacters.ToString();
        }
      }
    }



    private void editUsedCharacters_TextChanged( object sender, EventArgs e )
    {
      int   numChars = GR.Convert.ToI32( editCharactersCount.Text );
      if ( ( numChars <= 0 )
      ||   ( numChars > m_Charset.TotalNumberOfCharacters ) )
      {
        numChars = m_Charset.TotalNumberOfCharacters - m_Charset.ExportStartCharacter;
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
        case Function.COPY:
          characterEditor.Copy();
          return true;
        case Function.PASTE:
          characterEditor.Paste();
          return true;
      }
      return base.ApplyFunction( Function );
    }



    private void characterEditor_Modified( List<int> AffectedChars )
    {
      SetModified();
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



    private void comboExportMethod_SelectedIndexChanged(object sender, EventArgs e)
    {
      if ( m_ExportForm != null )
      {
        m_ExportForm.Dispose();
        m_ExportForm = null;
      }

      editDataExport.Text = "";
      editDataExport.Font = m_DefaultOutputFont;

      var item = (GR.Generic.Tupel<string, Type>)comboExportMethod.SelectedItem;
      if ( ( item == null )
      ||   (item.second == null ) )
      {
        return;
      }
      m_ExportForm = (ExportCharsetFormBase)Activator.CreateInstance(item.second, new object[] { Core });
      m_ExportForm.Parent = panelExport;
      m_ExportForm.CreateControl();
    }



    private void btnExport_Click( DecentForms.ControlBase Sender )
    {
      List<int> exportIndices = ListOfExportIndices();

      GR.Memory.ByteBuffer charSet = new GR.Memory.ByteBuffer();
      foreach ( int index in exportIndices )
      {
        charSet.Append(m_Charset.Characters[index].Tile.Data);
      }

      var exportInfo = new ExportCharsetInfo()
      {
        Charset         = m_Charset,
        ExportIndices   = ListOfExportIndices()
      };

      editDataExport.Text = "";
      editDataExport.Font = m_DefaultOutputFont;
      m_ExportForm.HandleExport( exportInfo, editDataExport, DocumentInfo );
    }



    private void comboImportMethod_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( m_ImportForm != null )
      {
        m_ImportForm.Dispose();
        m_ImportForm = null;
      }

      var item = (GR.Generic.Tupel<string, Type>)comboImportMethod.SelectedItem;
      if ( ( item == null )
      || ( item.second == null ) )
      {
        return;
      }
      m_ImportForm = (ImportCharsetFormBase)Activator.CreateInstance( item.second, new object[] { Core } );
      m_ImportForm.Parent = panelImport;
      m_ImportForm.Size = panelImport.ClientSize;
      m_ImportForm.CreateControl();
    }



    private void btnImport_Click( DecentForms.ControlBase Sender )
    {
      var undo1 = new Undo.UndoCharacterEditorCharChange( characterEditor, m_Charset, 0, m_Charset.TotalNumberOfCharacters );
      var undo2 = new Undo.UndoCharacterEditorValuesChange( characterEditor, m_Charset );

      if ( m_ImportForm.HandleImport( m_Charset, this ) )
      {
        DocumentInfo.UndoManager.StartUndoGroup();
        DocumentInfo.UndoManager.AddUndoTask( undo1, false );
        DocumentInfo.UndoManager.AddUndoTask( undo2, false );
        SetModified();
      }
    }



    public override bool CopyPossible
    {
      get
      {
        return characterEditor.EditorFocused;
      }
    }



    public override bool PastePossible
    {
      get
      {
        return characterEditor.EditorFocused;
      }
    }
  }
}


