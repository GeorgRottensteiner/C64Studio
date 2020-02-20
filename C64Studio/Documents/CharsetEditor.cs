using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using C64Studio.Formats;
using C64Studio.Types;
using GR.Memory;
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
      get => base.DocumentInfo;
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

      characterEditor.UndoManager = DocumentInfo.UndoManager;
      characterEditor.Core = Core;

      for ( int i = 0; i < 256; ++i )
      {
        CustomRenderer.PaletteManager.ApplyPalette( m_Charset.Characters[i].Image );
      }

      comboExportRange.Items.Add( "All" );
      comboExportRange.Items.Add( "Selection" );
      comboExportRange.Items.Add( "Range" );
      comboExportRange.SelectedIndex = 0;

      checkExportToDataIncludeRes.Checked = true;
      checkExportToDataWrap.Checked = true;

      ListViewItem    itemUn = new ListViewItem( "Uncategorized" );
      itemUn.Tag = 0;
      itemUn.SubItems.Add( "0" );
      listCategories.Items.Add( itemUn );
      RefreshCategoryCounts();

      Modified = false;
    }



    private void CharacterEditor_Modified()
    {
      SetModified();
    }



    internal void CharacterChanged( int charIndex )
    {
      characterEditor.CharacterChanged( charIndex );
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
      if ( OpenFile( "Open Charset Project", C64Studio.Types.Constants.FILEFILTER_CHARSET_PROJECT + C64Studio.Types.Constants.FILEFILTER_ALL, out string filename ) )
      {
        OpenProject( filename );
      }
    }



    public void Clear()
    {
      for ( int i = 0; i < 256; ++i )
      {
        m_Charset.Characters[i].Color = 0;
        m_Charset.Characters[i].Mode = C64Studio.Types.CharsetMode.HIRES;
        for ( int j = 0; j < 8; ++j )
        {
          m_Charset.Characters[i].Data.SetU8At( j, 0 );
        }
      }
      DocumentInfo.DocumentFilename = "";

      m_Charset.Categories.Clear();
      AddCategory( 0, "Uncategorized" );

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
        OpenProject( DocumentInfo.FullPath );
      }
      catch ( System.IO.IOException ex )
      {
        System.Windows.Forms.MessageBox.Show( "Could not load charset project file " + DocumentInfo.FullPath + ".\r\n" + ex.Message, "Could not load file" );
        return false;
      }
      SetUnmodified();
      return true;
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

      editCharactersCount.Text        = m_Charset.NumCharacters.ToString();
      editCharactersFrom.Text         = m_Charset.StartCharacter.ToString();

      Modified = false;

      listCategories.Items.Clear();

      int categoryIndex = 0;
      foreach ( var category in m_Charset.Categories )
      {
        ListViewItem    itemCat = new ListViewItem( category );
        itemCat.SubItems.Add( "0" );
        itemCat.Tag = categoryIndex;
        listCategories.Items.Add( itemCat );
        ++categoryIndex;
      }
      RefreshCategoryCounts();

      saveCharsetProjectToolStripMenuItem.Enabled = true;
      closeCharsetProjectToolStripMenuItem.Enabled = true;
    }



    public override GR.Memory.ByteBuffer SaveToBuffer()
    {
      return m_Charset.SaveToBuffer();
    }



    private bool SaveProject( bool SaveAs )
    {
      string    saveFilename = DocumentInfo.FullPath;

      if ( ( String.IsNullOrEmpty( DocumentInfo.DocumentFilename ) )
      ||   ( SaveAs ) )
      {
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
        if ( SaveAs )
        {
          saveFilename = saveDlg.FileName;
        }
        else
        {
          DocumentInfo.DocumentFilename = saveDlg.FileName;
          if ( DocumentInfo.Element != null )
          {
            if ( string.IsNullOrEmpty( DocumentInfo.Project.Settings.BasePath ) )
            {
              DocumentInfo.DocumentFilename = saveDlg.FileName;
            }
            else
            {
              DocumentInfo.DocumentFilename = GR.Path.RelativePathTo( saveDlg.FileName, false, System.IO.Path.GetFullPath( DocumentInfo.Project.Settings.BasePath ), true );
            }
            DocumentInfo.Element.Name = System.IO.Path.GetFileNameWithoutExtension( DocumentInfo.DocumentFilename );
            DocumentInfo.Element.Node.Text = System.IO.Path.GetFileName( DocumentInfo.DocumentFilename );
            DocumentInfo.Element.Filename = DocumentInfo.DocumentFilename;
          }
          saveFilename = DocumentInfo.FullPath;
        }
      }

      if ( !SaveAs )
      {
        m_Charset.Name = DocumentInfo.DocumentFilename;
        m_Charset.UsedTiles = GR.Convert.ToU32( editCharactersFrom.Text );
      }
      GR.Memory.ByteBuffer projectFile = SaveToBuffer();

      return SaveDocumentData( saveFilename, projectFile, SaveAs );
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
          SaveProject( false );
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
      SaveProject( false );
    }



    public override bool Save()
    {
      return SaveProject( false );
    }



    public override bool SaveAs()
    {
      return SaveProject( true );
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
        charSet.Append( m_Charset.Characters[i].Data );
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
          for ( int i = 0; i < m_Charset.NumCharacters; ++i )
          {
            listIndices.Add( m_Charset.StartCharacter + i );
          }
          break;
      }
      return listIndices;
    }



    private void btnExportCharsetToData_Click( object sender, EventArgs e )
    {
      int wrapByteCount = GR.Convert.ToI32( editWrapByteCount.Text );
      if ( wrapByteCount <= 0 )
      {
        wrapByteCount = 8;
      }
      string prefix = editPrefix.Text;

      List<int>     exportIndices = ListOfExportIndices();

      GR.Memory.ByteBuffer charSet = new GR.Memory.ByteBuffer();
      foreach ( int index in exportIndices )
      {
        charSet.Append( m_Charset.Characters[index].Data );
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
          colorData.AppendU8( (byte)m_Charset.Characters[index].Color );
        }
        resultText += Util.ToASMData( colorData, wrapData, wrapByteCount, prefixRes ? prefix : "" );
      }

      editDataExport.Text = resultText;
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
          m_Charset.BackgroundColor = project.BackgroundColor;
          m_Charset.MultiColor1 = project.MultiColor1;
          m_Charset.MultiColor2 = project.MultiColor2;
          m_Charset.NumCharacters = project.NumCharacters;
          m_Charset.ShowGrid = project.ShowGrid;

          for ( int i = 0; i < 256; ++i )
          {
            m_Charset.Characters[i].Color = project.Characters[i].Color;
            m_Charset.Characters[i].Data = new GR.Memory.ByteBuffer( project.Characters[i].Data );
            m_Charset.Characters[i].Mode = project.Characters[i].Mode;
          }

          editCharactersFrom.Text = m_Charset.NumCharacters.ToString();

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

          m_Charset.BackgroundColor = cpProject.BackgroundColor;
          m_Charset.MultiColor1     = cpProject.MultiColor1;
          m_Charset.MultiColor2     = cpProject.MultiColor2;

          m_Charset.NumCharacters = cpProject.NumChars;
          if ( m_Charset.NumCharacters > 256 )
          {
            m_Charset.NumCharacters = 256;
          }
          for ( int charIndex = 0; charIndex < m_Charset.NumCharacters; ++charIndex )
          {
            m_Charset.Characters[charIndex].Data = cpProject.Characters[charIndex].Data;
            m_Charset.Characters[charIndex].Color = cpProject.Characters[charIndex].Color;
            m_Charset.Characters[charIndex].Mode = cpProject.MultiColor ? Types.CharsetMode.MULTICOLOR : C64Studio.Types.CharsetMode.HIRES;
          }

          editCharactersFrom.Text = m_Charset.NumCharacters.ToString();
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
      int charsToImport = (int)CharData.Length / 8;
      if ( charsToImport > 256 )
      {
        charsToImport = 256;
      }
      for ( int i = 0; i < charsToImport; ++i )
      {
        for ( int j = 0; j < 8; ++j )
        {
          m_Charset.Characters[i].Data.SetU8At( j, CharData.ByteAt( i * 8 + j ) );
        }
        m_Charset.Characters[i].Mode = C64Studio.Types.CharsetMode.HIRES;
        m_Charset.Characters[i].Color = 1;
      }
      characterEditor.CharsetUpdated( m_Charset );
      SetModified();
    }



    private void btnDefaultUppercase_Click( object sender, EventArgs e )
    {
      for ( int i = 0; i < 256; ++i )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharsetCharChange( this, m_Charset, i ), i == 0 );
      }
      for ( int i = 0; i < 256; ++i )
      {
        for ( int j = 0; j < 8; ++j )
        {
          m_Charset.Characters[i].Data.SetU8At( j, Types.ConstantData.UpperCaseCharset.ByteAt( i * 8 + j ) );
        }
        m_Charset.Characters[i].Mode = C64Studio.Types.CharsetMode.HIRES;
        m_Charset.Characters[i].Color = 1;
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
          m_Charset.Characters[i].Data.SetU8At( j, Types.ConstantData.LowerCaseCharset.ByteAt( i * 8 + j ) );
        }
        m_Charset.Characters[i].Mode = C64Studio.Types.CharsetMode.HIRES;
        m_Charset.Characters[i].Color = 1;
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

      GR.Image.MemoryImage    targetImg = new GR.Image.MemoryImage( 128, 128, m_Charset.Characters[0].Image.PixelFormat );
      CustomRenderer.PaletteManager.ApplyPalette( targetImg );

      List<int>     exportIndices = ListOfExportIndices();

      foreach ( int i in exportIndices )
      {
        m_Charset.Characters[i].Image.DrawTo( targetImg, ( i % 16 ) * 8, ( i / 16 ) * 8 );
      }
      System.Drawing.Bitmap bmpTarget = targetImg.GetAsBitmap();
      bmpTarget.Save( saveDlg.FileName, System.Drawing.Imaging.ImageFormat.Png );

    }



    private bool ImportChar( GR.Image.FastImage Image, int CharIndex, bool ForceMulticolor )
    {
      if ( Image.PixelFormat != System.Drawing.Imaging.PixelFormat.Format8bppIndexed )
      {
        // invalid format
        return false;
      }
      // Match image data
      GR.Memory.ByteBuffer Buffer = new GR.Memory.ByteBuffer( m_Charset.Characters[CharIndex].Data );

      int   chosenCharColor = -1;

      bool  isMultiColor = false;

      unsafe
      {
        // determine single/multi color
        bool[]  usedColor = new bool[16];
        int     numColors = 0;
        bool    hasSinglePixel = false;
        bool    usedBackgroundColor = false;

        for ( int y = 0; y < Image.Height; ++y )
        {
          for ( int x = 0; x < Image.Width; ++x )
          {
            int     colorIndex = (int)Image.GetPixelData( x, y ) % 16;
            if ( colorIndex >= 16 )
            {
              return false;
            }
            if ( ( x % 2 ) == 0 )
            {
              if ( colorIndex != (int)Image.GetPixelData( x + 1, y ) % 16 )
              {
                // not a double pixel, must be single color then
                hasSinglePixel = true;
              }
            }

            if ( !usedColor[colorIndex] )
            {
              if ( colorIndex == m_Charset.BackgroundColor )
              {
                usedBackgroundColor = true;
              }
              usedColor[colorIndex] = true;
              numColors++;
            }
          }
        }
        //Debug.Log( "For Char " + CharIndex + ", hasSinglePixel = " + hasSinglePixel + ", numColors = " + numColors + ", usedBackgroundColor = " + usedBackgroundColor );
        if ( ( hasSinglePixel )
        &&   ( numColors > 2 ) )
        {
          return false;
        }
        if ( ( hasSinglePixel )
        &&   ( numColors == 2 )
        &&   ( !usedBackgroundColor ) )
        {
          return false;
        }
        if ( ( !hasSinglePixel )
        &&   ( numColors > 4 ) )
        {
          return false;
        }
        if ( ( !hasSinglePixel )
        &&   ( numColors == 4 )
        &&   ( !usedBackgroundColor ) )
        {
          return false;
        }
        int     otherColorIndex = 16;
        if ( ( !hasSinglePixel )
        &&   ( numColors == 2 )
        &&   ( usedBackgroundColor ) )
        {
          for ( int i = 0; i < 16; ++i )
          {
            if ( ( usedColor[i] )
            &&   ( i != m_Charset.BackgroundColor ) )
            {
              otherColorIndex = i;
              break;
            }
          }
        }
        if ( ( !ForceMulticolor )
        &&   ( ( hasSinglePixel )
        ||     ( ( numColors == 2 )
        &&       ( usedBackgroundColor )
        &&       ( otherColorIndex < 8 ) ) ) )
        //||   ( numColors == 2 ) )
        {
          // eligible for single color
          int usedFreeColor = -1;
          for ( int i = 0; i < 16; ++i )
          {
            if ( usedColor[i] )
            {
              if ( i != m_Charset.BackgroundColor )
              {
                if ( usedFreeColor != -1 )
                {
                  return false;
                }
                usedFreeColor = i;
              }
            }
          }

          for ( int y = 0; y < Image.Height; ++y )
          {
            for ( int x = 0; x < Image.Width; ++x )
            {
              int ColorIndex = (int)Image.GetPixelData( x, y ) % 16;

              int BitPattern = 0;

              if ( ColorIndex != m_Charset.BackgroundColor )
              {
                BitPattern = 1;
              }

              // noch nicht verwendete Farbe
              if ( BitPattern == 1 )
              {
                chosenCharColor = ColorIndex;
              }
              byte byteMask = (byte)( 255 - ( 1 << ( ( 7 - ( x % 8 ) ) ) ) );
              Buffer.SetU8At( y + x / 8, (byte)( ( Buffer.ByteAt( y + x / 8 ) & byteMask ) | ( BitPattern << ( ( 7 - ( x % 8 ) ) ) ) ) );
            }
          }
        }
        else
        {
          // multi color
          isMultiColor = true;
          int     usedMultiColors = 0;
          int     usedFreeColor = -1;
          for ( int i = 0; i < 16; ++i )
          {
            if ( usedColor[i] )
            {
              if ( ( i == m_Charset.MultiColor1 )
              ||   ( i == m_Charset.MultiColor2 )
              ||   ( i == m_Charset.BackgroundColor ) )
              {
                ++usedMultiColors;
              }
              else
              {
                usedFreeColor = i;
              }
            }
          }
          if ( numColors - usedMultiColors > 1 )
          {
            // only one free color allowed
            return false;
          }
          for ( int y = 0; y < Image.Height; ++y )
          {
            for ( int x = 0; x < Image.Width / 2; ++x )
            {
              int ColorIndex = (int)Image.GetPixelData( 2 * x, y ) % 16;

              byte BitPattern = 0;

              if ( ColorIndex == m_Charset.BackgroundColor )
              {
                BitPattern = 0x00;
              }
              else if ( ColorIndex == m_Charset.MultiColor1 )
              {
                BitPattern = 0x01;
              }
              else if ( ColorIndex == m_Charset.MultiColor2 )
              {
                BitPattern = 0x02;
              }
              else
              {
                // noch nicht verwendete Farbe
                chosenCharColor = usedFreeColor;
                BitPattern = 0x03;
              }
              byte byteMask = (byte)( 255 - ( 3 << ( ( 3 - ( x % 4 ) ) * 2 ) ) );
              Buffer.SetU8At( y + x / 4, (byte)( ( Buffer.ByteAt( y + x / 4 ) & byteMask ) | ( BitPattern << ( ( 3 - ( x % 4 ) ) * 2 ) ) ) );
            }
          }
        }
      }
      for ( int i = 0; i < 8; ++i )
      {
        m_Charset.Characters[CharIndex].Data.SetU8At( i, Buffer.ByteAt( i ) );
      }
      if ( chosenCharColor == -1 )
      {
        chosenCharColor = 0;
      }
      m_Charset.Characters[CharIndex].Color = chosenCharColor;
      if ( ( isMultiColor )
      &&   ( chosenCharColor < 8 ) )
      {
        m_Charset.Characters[CharIndex].Color = chosenCharColor + 8;
      }
      m_Charset.Characters[CharIndex].Mode = isMultiColor ? Types.CharsetMode.MULTICOLOR : C64Studio.Types.CharsetMode.HIRES;
      return true;
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



    private void editCategoryName_TextChanged( object sender, EventArgs e )
    {
      bool    validCategory = ( editCategoryName.Text.Length > 0 );
      foreach ( string category in m_Charset.Categories )
      {
        if ( category == editCategoryName.Text )
        {
          validCategory = false;
          break;
        }
      }
      btnAddCategory.Enabled = validCategory;
    }



    private void btnAddCategory_Click( object sender, EventArgs e )
    {
      string    newCategory = editCategoryName.Text;

      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharsetAddCategory( this, m_Charset, m_Charset.Categories.Count ) );

      AddCategory( m_Charset.Categories.Count, newCategory );
    }



    public void AddCategory( int Index, string Category )
    {
      m_Charset.Categories.Insert( Index, Category );
      

      ListViewItem    itemNew = new ListViewItem( Category );
      itemNew.Tag = Index;
      itemNew.SubItems.Add( "0" );
      listCategories.Items.Insert( Index, itemNew );

      characterEditor.AddCategory( Index, itemNew.Text );

      RefreshCategoryCounts();
    }



    private void RefreshCategoryCounts()
    {
      GR.Collections.Map<int,int>   catCounts = new GR.Collections.Map<int, int>();

      for ( int i = 0; i < 256; ++i )
      {
        catCounts[m_Charset.Characters[i].Category]++;
      }

      int itemIndex = 0;
      foreach ( ListViewItem item in listCategories.Items )
      {
        item.SubItems[1].Text = catCounts[(int)item.Tag].ToString();
        item.Tag = itemIndex;
        ++itemIndex;
      }
    }



    private void listCategories_SelectedIndexChanged( object sender, EventArgs e )
    {
      bool    deleteAllowed = ( listCategories.SelectedItems.Count > 0 );
      bool    collapseAllowed = ( listCategories.SelectedItems.Count > 0 );
      if ( deleteAllowed )
      {
        if ( (int)listCategories.SelectedItems[0].Tag == 0 )
        {
          deleteAllowed = false;
        }
      }
      btnDelete.Enabled = deleteAllowed;
      btnCollapseCategory.Enabled = collapseAllowed;
      btnReseatCategory.Enabled = collapseAllowed;
    }



    private void btnDelete_Click( object sender, EventArgs e )
    {
      if ( listCategories.SelectedItems.Count == 0 )
      {
        return;
      }
      int category = (int)listCategories.SelectedItems[0].Tag;

      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharsetRemoveCategory( this, m_Charset, category ) );

      RemoveCategory( category );
    }



    public void RemoveCategory( int CategoryIndex )
    {
      listCategories.Items.RemoveAt( CategoryIndex );

      characterEditor.RemoveCategory( CategoryIndex );

      m_Charset.Categories.RemoveAt( CategoryIndex );

      for ( int i = 0; i < 256; ++i )
      {
        if ( m_Charset.Characters[i].Category >= CategoryIndex )
        {
          --m_Charset.Characters[i].Category;
        }
      }
      RefreshCategoryCounts();
    }



    private void btnCollapseCategory_Click( object sender, EventArgs e )
    {
      // collapses similar looking characters
      if ( listCategories.SelectedItems.Count == 0 )
      {
        return;
      }

      for ( int i = 0; i < 256; ++i )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharsetCharChange( this, m_Charset, i ), i == 0 );
      }

      int category = (int)listCategories.SelectedItems[0].Tag;
      int collapsedCount = 0;

      for ( int i = 0; i < 256 - collapsedCount; ++i )
      {
        if ( m_Charset.Characters[i].Category == category )
        {
          for ( int j = i + 1; j < 256 - collapsedCount; ++j )
          {
            if ( m_Charset.Characters[j].Category == category )
            {
              if ( ( m_Charset.Characters[i].Data.Compare( m_Charset.Characters[j].Data ) == 0 )
              &&   ( m_Charset.Characters[i].Color == m_Charset.Characters[j].Color ) )
              {
                // collapse!
                //Debug.Log( "Collapse " + j.ToString() + " into " + i.ToString() );
                for ( int l = j; l < 256 - 1 - collapsedCount; ++l )
                {
                  m_Charset.Characters[l].Data  = m_Charset.Characters[l + 1].Data;
                  m_Charset.Characters[l].Color = m_Charset.Characters[l + 1].Color;
                  m_Charset.Characters[l].Category = m_Charset.Characters[l + 1].Category;
                  m_Charset.Characters[l].Mode = m_Charset.Characters[l + 1].Mode;
                }
                for ( int l = 0; l < 8; ++l )
                {
                  m_Charset.Characters[255 - collapsedCount].Data.SetU8At( l, 0 );
                }
                m_Charset.Characters[255 - collapsedCount].Color = 0;
                ++collapsedCount;
                --j;
                continue;
              }
            }
          }
        }
      }
      if ( collapsedCount > 0 )
      {
        characterEditor.CharsetUpdated( m_Charset );
        RefreshCategoryCounts();
        Modified = true;
      }
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
      if ( m_Charset.StartCharacter != startChar )
      {
        m_Charset.StartCharacter = startChar;
        Modified = true;
        if ( startChar + m_Charset.NumCharacters > 256 )
        {
          m_Charset.NumCharacters = 256 - startChar;
          editCharactersCount.Text = m_Charset.NumCharacters.ToString();
        }
      }
    }



    private void editUsedCharacters_TextChanged( object sender, EventArgs e )
    {
      int   numChars = GR.Convert.ToI32( editCharactersCount.Text );
      if ( ( numChars <= 0 )
      ||   ( numChars > 256 ) )
      {
        numChars = 256 - m_Charset.StartCharacter;
        editCharactersCount.Text = numChars.ToString();
      }
      if ( m_Charset.NumCharacters != numChars )
      {
        m_Charset.NumCharacters = numChars;
        Modified = true;
      }
    }



    private void btnSortByCategory_Click( object sender, EventArgs e )
    {
      for ( int i = 0; i < 256; ++i )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharsetCharChange( this, m_Charset, i ), i == 0 );
      }


      // resorts characters by category
      List<Formats.CharData>    newList = new List<C64Studio.Formats.CharData>();
      for ( int j = 0; j < m_Charset.Categories.Count; ++j )
      {
        for ( int i = 0; i < 256; ++i )
        {
          if ( m_Charset.Characters[i].Category == j )
          {
            newList.Add( m_Charset.Characters[i] );
          }
        }
      }

      m_Charset.Characters = newList;
      characterEditor.CharsetUpdated( m_Charset );
      RefreshCategoryCounts();
      Modified = true;
    }



    private void btnReseatCategory_Click( object sender, EventArgs e )
    {
      if ( listCategories.SelectedItems.Count == 0 )
      {
        return;
      }

      for ( int i = 0; i < 256; ++i )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharsetCharChange( this, m_Charset, i ), i == 0 );
      }

      int category = (int)listCategories.SelectedItems[0].Tag;
      int catTarget = GR.Convert.ToI32( editCollapseIndex.Text );
      int catTargetStart = catTarget;

      List<Formats.CharData> newList = new List<C64Studio.Formats.CharData>();
      int[] targetIndex = new int[256];
      int     numCatEntries = 0;

      for ( int i = 0; i < 256; ++i )
      {
        targetIndex[i] = i;
        if ( m_Charset.Characters[i].Category == category )
        {
          ++numCatEntries;
        }
      }

      int lastIndex = 0;
      for ( int j = 0; j < 256; ++j )
      {
        if ( m_Charset.Characters[j].Category != category )
        {
          if ( newList.Count == catTargetStart )
          {
            break;
          }
          newList.Add( m_Charset.Characters[j] );
          lastIndex = j;
        }
      }
      if ( lastIndex == 0 )
      {
        // nothing to do
        return;
      }
      for ( int j = 0; j < 256; ++j )
      {
        if ( m_Charset.Characters[j].Category == category )
        {
          newList.Add( m_Charset.Characters[j] );
        }
      }
      for ( int j = lastIndex + 1; j < 256; ++j )
      {
        if ( m_Charset.Characters[j].Category != category )
        {
          newList.Add( m_Charset.Characters[j] );
        }
      }
      m_Charset.Characters = newList;
      characterEditor.CharsetUpdated( m_Charset );
      RefreshCategoryCounts();
      Modified = true;
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

      List<int>     exportIndices = ListOfExportIndices();

      GR.Memory.ByteBuffer charSet = new GR.Memory.ByteBuffer();
      foreach ( int index in exportIndices )
      {
        charSet.Append( m_Charset.Characters[index].Data );
      }

      string    resultText = Util.ToBASICData( charSet, startLine, lineOffset );

      editDataExport.Text = resultText;
    }



    private void btnImportCharsetFromASM_Click( object sender, EventArgs e )
    {
      Parser.ASMFileParser asmParser = new C64Studio.Parser.ASMFileParser();

      Parser.CompileConfig config = new Parser.CompileConfig();
      config.TargetType = Types.CompileTargetType.PLAIN;
      config.OutputFile = "temp.bin";
      config.Assembler = Types.AssemblerType.C64_STUDIO;

      string    temp = "* = $0801\n" + editDataImport.Text;
      if ( ( asmParser.Parse( temp, null, config, null ) )
      &&   ( asmParser.Assemble( config ) ) )
      {
        GR.Memory.ByteBuffer charData = asmParser.AssembledOutput.Assembly;

        ImportFromData( charData );
      }
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
      if ( !characterEditor.Focused )
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
        charSet.Append( m_Charset.Characters[index].Data );
      }

      string    resultText = Util.ToBASICHexData( charSet, startLine, lineOffset );

      editDataExport.Text = resultText;
    }



    private void characterEditor_Modified()
    {
      SetModified();
    }



    private void characterEditor_CategoryModified()
    {
      RefreshCategoryCounts();
    }



  }
}


