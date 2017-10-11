using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
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

    private int                         m_CurrentChar = 0;
    private int                         m_CurrentColor = 1;
    private ColorType                   m_CurrentColorType = ColorType.CHAR_COLOR;
    private bool                        m_ButtonReleased = false;
    private bool                        m_RButtonReleased = false;

    private Formats.CharsetProject      m_Charset = new C64Studio.Formats.CharsetProject();

    private GR.Image.MemoryImage        m_ImagePlayground = new GR.Image.MemoryImage( 256, 256, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );



    public CharsetEditor( StudioCore Core )
    {
      this.Core = Core;
      DocumentInfo.Type = ProjectElement.ElementType.CHARACTER_SET;
      DocumentInfo.UndoManager.MainForm = Core.MainForm;
      m_IsSaveable = true;
      InitializeComponent();

      pictureEditor.DisplayPage.Create( 8, 8, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );
      pictureEditor.PostPaint += new GR.Forms.FastPictureBox.PostPaintCallback( pictureEditor_PostPaint );
      picturePlayground.DisplayPage.Create( 128, 128, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );
      panelCharacters.PixelFormat = System.Drawing.Imaging.PixelFormat.Format8bppIndexed;
      panelCharacters.SetDisplaySize( 128, 128 );
      panelCharColors.DisplayPage.Create( 128, 8, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );
      m_ImagePlayground.Create( 256, 256, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );

      CustomRenderer.PaletteManager.ApplyPalette( pictureEditor.DisplayPage );
      CustomRenderer.PaletteManager.ApplyPalette( picturePlayground.DisplayPage );
      CustomRenderer.PaletteManager.ApplyPalette( panelCharacters.DisplayPage );
      CustomRenderer.PaletteManager.ApplyPalette( m_ImagePlayground );
      CustomRenderer.PaletteManager.ApplyPalette( panelCharColors.DisplayPage );
      for ( int i = 0; i < 256; ++i )
      {
        CustomRenderer.PaletteManager.ApplyPalette( m_Charset.Characters[i].Image );
        panelCharacters.Items.Add( i.ToString(), m_Charset.Characters[i].Image );
      }

      for ( int i = 0; i < 16; ++i )
      {
        comboBackground.Items.Add( i.ToString( "d2" ) );
        comboMulticolor1.Items.Add( i.ToString( "d2" ) );
        comboMulticolor2.Items.Add( i.ToString( "d2" ) );
        comboBGColor4.Items.Add( i.ToString( "d2" ) );
        comboCharColor.Items.Add( i.ToString( "d2" ) );
      }
      comboBackground.SelectedIndex = 0;
      comboMulticolor1.SelectedIndex = 0;
      comboMulticolor2.SelectedIndex = 0;
      comboBGColor4.SelectedIndex = 0;
      comboCharColor.SelectedIndex = 1;

      comboExportRange.Items.Add( "All" );
      comboExportRange.Items.Add( "Selection" );
      comboExportRange.Items.Add( "Range" );
      comboExportRange.SelectedIndex = 0;

      comboCharsetMode.Items.Add( "HiRes" );
      comboCharsetMode.Items.Add( "MultiColor" );
      comboCharsetMode.Items.Add( "Enhanced Char Mode (ECM)" );
      comboCharsetMode.SelectedIndex = 0;

      radioCharColor.Checked = true;
      panelCharacters.SelectedIndex = 0;

      checkExportToDataIncludeRes.Checked = true;
      checkExportToDataWrap.Checked = true;

      ListViewItem    itemUn = new ListViewItem( "Uncategorized" );
      itemUn.Tag = 0;
      itemUn.SubItems.Add( "0" );
      listCategories.Items.Add( itemUn );
      RefreshCategoryCounts();
      comboCategories.Items.Add( itemUn.Name );

      RedrawColorChooser();

      panelCharacters.KeyDown += new KeyEventHandler( HandleKeyDown );
      pictureEditor.PreviewKeyDown += new PreviewKeyDownEventHandler( pictureEditor_PreviewKeyDown );
      Modified = false;
    }



    void pictureEditor_PreviewKeyDown( object sender, PreviewKeyDownEventArgs e )
    {
      KeyEventArgs ke = new KeyEventArgs( e.KeyData );
      HandleKeyDown( sender, ke );
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



    void MirrorX()
    {
      List<int>     selectedChars = panelCharacters.SelectedIndices;

      DocumentInfo.UndoManager.StartUndoGroup();
      foreach ( var index in selectedChars )
      {
        DocumentInfo.UndoManager.AddGroupedUndoTask( new Undo.UndoCharsetCharChange( this, m_Charset, index ) );

        var processedChar = m_Charset.Characters[index];

        for ( int y = 0; y < 8; ++y )
        {
          if ( ( processedChar.Mode == C64Studio.Types.CharsetMode.MULTICOLOR )
          &&   ( processedChar.Color >= 8 ) )
          {
            byte result = (byte)( (byte)( ( processedChar.Data.ByteAt( y ) & 0xc0 ) >> 6 )
                                | (byte)( ( processedChar.Data.ByteAt( y ) & 0x30 ) >> 2 )
                                | (byte)( ( processedChar.Data.ByteAt( y ) & 0x0c ) << 2 )
                                | (byte)( ( processedChar.Data.ByteAt( y ) & 0x03 ) << 6 ) );
            processedChar.Data.SetU8At( y, result );
          }
          else
          {
            byte result = (byte)( (byte)( ( processedChar.Data.ByteAt( y ) & 0x80 ) >> 7 )
                                | (byte)( ( processedChar.Data.ByteAt( y ) & 0x40 ) >> 5 )
                                | (byte)( ( processedChar.Data.ByteAt( y ) & 0x20 ) >> 3 )
                                | (byte)( ( processedChar.Data.ByteAt( y ) & 0x10 ) >> 1 )
                                | (byte)( ( processedChar.Data.ByteAt( y ) & 0x08 ) << 1 )
                                | (byte)( ( processedChar.Data.ByteAt( y ) & 0x04 ) << 3 )
                                | (byte)( ( processedChar.Data.ByteAt( y ) & 0x02 ) << 5 )
                                | (byte)( ( processedChar.Data.ByteAt( y ) & 0x01 ) << 7 ) );
            processedChar.Data.SetU8At( y, result );
          }
        }
        RebuildCharImage( index );
        panelCharacters.InvalidateItemRect( index );
      }
      pictureEditor.Invalidate();
      Modified = true;
    }



    void MirrorY()
    {
      List<int>     selectedChars = panelCharacters.SelectedIndices;

      DocumentInfo.UndoManager.StartUndoGroup();
      foreach ( var index in selectedChars )
      {
        DocumentInfo.UndoManager.AddGroupedUndoTask( new Undo.UndoCharsetCharChange( this, m_Charset, index ) );

        for ( int y = 0; y < 4; ++y )
        {
          byte oldValue = m_Charset.Characters[index].Data.ByteAt( y );
          m_Charset.Characters[index].Data.SetU8At( y, m_Charset.Characters[index].Data.ByteAt( 7 - y ) );
          m_Charset.Characters[index].Data.SetU8At( 7 - y, oldValue );
        }
        RebuildCharImage( index );
        panelCharacters.InvalidateItemRect( index );
      }
      pictureEditor.Invalidate();
      Modified = true;
    }



    void HandleKeyDown( object sender, KeyEventArgs e )
    {
      if ( ( e.Modifiers == Keys.Control )
      &&   ( e.KeyCode == Keys.C ) )
      {
        // copy
        CopyToClipboard();
      }
      else if ( ( e.Modifiers == Keys.Control )
      &&        ( e.KeyCode == Keys.V ) )
      {
        // paste
        PasteClipboardImageToChar();
      }
      else if ( e.KeyCode == Keys.H )
      {
        // mirror horizontally
        MirrorX();
      }
      else if ( e.KeyCode == Keys.V )
      {
        // mirror vertically
        MirrorY();
      }
    }



    private void comboColor_DrawItem( object sender, DrawItemEventArgs e )
    {
      ComboBox combo = (ComboBox)sender;

      e.DrawBackground();
      System.Drawing.Rectangle itemRect = new System.Drawing.Rectangle( e.Bounds.Left + 20, e.Bounds.Top, e.Bounds.Width - 20, e.Bounds.Height );
      if ( ( e.State & DrawItemState.Disabled ) != 0 )
      {
        e.Graphics.FillRectangle( System.Drawing.SystemBrushes.GrayText, itemRect );
        e.Graphics.DrawString( combo.Items[e.Index].ToString(), combo.Font, new System.Drawing.SolidBrush( System.Drawing.Color.Gray ), 3.0f, e.Bounds.Top + 1.0f );
      }
      else if ( ( e.State & DrawItemState.Selected ) != 0 )
      {
        e.Graphics.FillRectangle( Types.ConstantData.Palette.ColorBrushes[e.Index], itemRect );
        e.Graphics.DrawString( combo.Items[e.Index].ToString(), combo.Font, new System.Drawing.SolidBrush( System.Drawing.Color.White ), 3.0f, e.Bounds.Top + 1.0f );
      }
      else
      {
        e.Graphics.FillRectangle( Types.ConstantData.Palette.ColorBrushes[e.Index], itemRect );
        e.Graphics.DrawString( combo.Items[e.Index].ToString(), combo.Font, new System.Drawing.SolidBrush( System.Drawing.Color.Black ), 3.0f, e.Bounds.Top + 1.0f );
      }
    }



    void pictureEditor_PostPaint( GR.Image.FastImage TargetBuffer )
    {
      if ( m_Charset.ShowGrid )
      {
        if ( ( m_Charset.Characters[m_CurrentChar].Mode == C64Studio.Types.CharsetMode.MULTICOLOR )
        &&   ( m_Charset.Characters[m_CurrentChar].Color >= 8 ) )
        {
          for ( int i = 0; i < 4; ++i )
          {
            for ( int j = 0; j < TargetBuffer.Height; ++j )
            {
              TargetBuffer.SetPixel( i * ( pictureEditor.ClientRectangle.Width / 4 ), j, 0xffffffff );
            }
          }
          for ( int i = 0; i < 8; ++i )
          {
            for ( int j = 0; j < TargetBuffer.Height; ++j )
            {
              TargetBuffer.SetPixel( j, i * ( pictureEditor.ClientRectangle.Width / 8 ), 0xffffffff );
            }
          }
        }
        else
        {
          for ( int i = 0; i < 8; ++i )
          {
            for ( int j = 0; j < TargetBuffer.Width; ++j )
            {
              TargetBuffer.SetPixel( i * ( pictureEditor.ClientRectangle.Width / 8 ), j, 0xffffffff );
            }
            for ( int j = 0; j < TargetBuffer.Height; ++j )
            {
              TargetBuffer.SetPixel( j, i * ( pictureEditor.ClientRectangle.Width / 8 ), 0xffffffff );
            }
          }
        }
      }
    }



    void RebuildCharImage( int CharIndex )
    {
      Displayer.CharacterDisplayer.DisplayChar( m_Charset, CharIndex, m_Charset.Characters[CharIndex].Image, 0, 0 );

      bool playgroundChanged = false;
      for ( int i = 0; i < 16; ++i )
      {
        for ( int j = 0; j < 16; ++j )
        {
          if ( ( m_Charset.PlaygroundChars[i + j * 16] & 0xff ) == CharIndex )
          {
            playgroundChanged = true;
            Displayer.CharacterDisplayer.DisplayChar( m_Charset, CharIndex, m_ImagePlayground, i * 8, j * 8, m_Charset.PlaygroundChars[i + j * 16] >> 8 );
          }
        }
      }
      if ( playgroundChanged )
      {
        RedrawPlayground();
      }
      if ( CharIndex == m_CurrentChar )
      {
        RedrawColorChooser();
      }
    }



    private void comboMulticolor_DrawItem( object sender, DrawItemEventArgs e )
    {
      ComboBox combo = (ComboBox)sender;

      e.DrawBackground();
      System.Drawing.Rectangle itemRect = new System.Drawing.Rectangle( e.Bounds.Left + 20, e.Bounds.Top, e.Bounds.Width - 20, e.Bounds.Height );
      if ( e.Index >= 8 )
      {
        itemRect = new System.Drawing.Rectangle( e.Bounds.Left + 20, e.Bounds.Top, ( e.Bounds.Width - 20 ) / 2, e.Bounds.Height );
        e.Graphics.FillRectangle( Types.ConstantData.Palette.ColorBrushes[e.Index], itemRect );
        itemRect = new System.Drawing.Rectangle( e.Bounds.Left + 20 + ( e.Bounds.Width - 20 ) / 2, e.Bounds.Top, e.Bounds.Width - ( e.Bounds.Width - 20 ) / 2, e.Bounds.Height );
        e.Graphics.FillRectangle( Types.ConstantData.Palette.ColorBrushes[e.Index - 8], itemRect );
      }
      else
      {
        e.Graphics.FillRectangle( Types.ConstantData.Palette.ColorBrushes[e.Index], itemRect );
      }
      if ( ( e.State & DrawItemState.Selected ) != 0 )
      {
        e.Graphics.DrawString( combo.Items[e.Index].ToString(), combo.Font, new System.Drawing.SolidBrush( System.Drawing.Color.White ), 3.0f, e.Bounds.Top + 1.0f );
      }
      else
      {
        e.Graphics.DrawString( combo.Items[e.Index].ToString(), combo.Font, new System.Drawing.SolidBrush( System.Drawing.Color.Black ), 3.0f, e.Bounds.Top + 1.0f );
      }

    }



    private void pictureEditor_MouseDown( object sender, MouseEventArgs e )
    {
      pictureEditor.Focus();
      HandleMouseOnEditor( e.X, e.Y, e.Button );
    }



    private void HandleMouseOnEditor( int X, int Y, MouseButtons Buttons )
    {
      int     charX = X / ( pictureEditor.ClientRectangle.Width / 8 );
      int     charY = Y / ( pictureEditor.ClientRectangle.Height / 8 );
      int     affectedCharIndex = m_CurrentChar;
      var     origAffectedChar = m_Charset.Characters[m_CurrentChar];
      var     affectedChar = m_Charset.Characters[m_CurrentChar];
      if ( affectedChar.Mode == C64Studio.Types.CharsetMode.ECM )
      {
        affectedCharIndex %= 64;
        affectedChar = m_Charset.Characters[affectedCharIndex];
      }

      if ( ( Buttons & MouseButtons.Left ) != 0 )
      {
        byte    charByte = affectedChar.Data.ByteAt( charY );
        byte    newByte = charByte;
        int     colorIndex = affectedChar.Color;

        if ( ( affectedChar.Mode != C64Studio.Types.CharsetMode.MULTICOLOR )
        ||   ( affectedChar.Color < 8 ) )
        {
          // single color
          charX = 7 - charX;
          if ( m_CurrentColorType == ColorType.CHAR_COLOR )
          {
            newByte |= (byte)( 1 << charX );
          }
          else
          {
            newByte &= (byte)~( 1 << charX );
            colorIndex = m_Charset.BackgroundColor;
          }
        }
        else
        {
          // multi color
          charX = X / ( pictureEditor.ClientRectangle.Width / 4 );
          charX = 3 - charX;

          newByte &= (byte)~( 3 << ( 2 * charX ) );

          int     replacementBytes = 0;

          switch ( m_CurrentColorType )
          {
            case ColorType.BACKGROUND:
              colorIndex = m_Charset.BackgroundColor;
              break;
            case ColorType.CHAR_COLOR:
              replacementBytes = 3;
              colorIndex = affectedChar.Color;
              break;
            case ColorType.MULTICOLOR_1:
              replacementBytes = 1;
              colorIndex = m_Charset.MultiColor1;
              break;
            case ColorType.MULTICOLOR_2:
              replacementBytes = 2;
              colorIndex = m_Charset.MultiColor2;
              break;
          }
          newByte |= (byte)( replacementBytes << ( 2 * charX ) );
        }
        if ( newByte != charByte )
        {
          Modified = true;

          if ( m_ButtonReleased )
          {
            DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharsetCharChange( this, m_Charset, affectedCharIndex ) );
            m_ButtonReleased = false;
          }

          affectedChar.Data.SetU8At( charY, newByte );

          if ( origAffectedChar.Mode == C64Studio.Types.CharsetMode.ECM )
          {
            for ( int i = 0; i < 4; ++i )
            {
              RebuildCharImage( affectedCharIndex + i * 64 );
              panelCharacters.InvalidateItemRect( affectedCharIndex + i * 64 );
            }
          }
          else
          {
            RebuildCharImage( affectedCharIndex );
            panelCharacters.InvalidateItemRect( affectedCharIndex );
          }
          pictureEditor.Invalidate();
        }
      }
      else
      {
        m_ButtonReleased = true;
      }
      if ( ( Buttons & MouseButtons.Right ) != 0 )
      {
        byte charByte = affectedChar.Data.ByteAt( charY );
        byte newByte = charByte;

        if ( ( affectedChar.Mode != C64Studio.Types.CharsetMode.MULTICOLOR )
        ||   ( affectedChar.Color < 8 ) )
        {
          // single color
          charX = 7 - charX;
          newByte &= (byte)~( 1 << charX );
        }
        else
        {
          // multi color
          charX = X / ( pictureEditor.ClientRectangle.Width / 4 );
          charX = 3 - charX;

          newByte &= (byte)~( 3 << ( 2 * charX ) );
        }
        if ( newByte != charByte )
        {
          if ( m_RButtonReleased )
          {
            m_RButtonReleased = false;
            DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharsetCharChange( this, m_Charset, affectedCharIndex ) );
          }

          Modified = true;
          affectedChar.Data.SetU8At( charY, newByte );

          if ( origAffectedChar.Mode == C64Studio.Types.CharsetMode.ECM )
          {
            for ( int i = 0; i < 4; ++i )
            {
              RebuildCharImage( affectedCharIndex + i * 64 );
              panelCharacters.InvalidateItemRect( affectedCharIndex + i * 64 );
            }
          }
          else
          {
            RebuildCharImage( affectedCharIndex );
            panelCharacters.InvalidateItemRect( affectedCharIndex );
          }
          pictureEditor.Invalidate();
        }
      }
      else
      {
        m_RButtonReleased = true;
      }
    }



    private void radioBackground_CheckedChanged( object sender, EventArgs e )
    {
      m_CurrentColorType = ColorType.BACKGROUND;
    }



    private void radioMultiColor1_CheckedChanged( object sender, EventArgs e )
    {
      m_CurrentColorType = ColorType.MULTICOLOR_1;
    }



    private void radioMulticolor2_CheckedChanged( object sender, EventArgs e )
    {
      m_CurrentColorType = ColorType.MULTICOLOR_2;
    }



    private void radioCharColor_CheckedChanged( object sender, EventArgs e )
    {
      m_CurrentColorType = ColorType.CHAR_COLOR;
    }



    private void pictureEditor_MouseMove( object sender, MouseEventArgs e )
    {
      HandleMouseOnEditor( e.X, e.Y, e.Button );
    }



    private void comboBackground_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( m_Charset.BackgroundColor != comboBackground.SelectedIndex )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharsetValuesChange( this, m_Charset ) );

        m_Charset.BackgroundColor = comboBackground.SelectedIndex;
        for ( int i = 0; i < 256; ++i )
        {
          RebuildCharImage( i );
        }
        Modified = true;
        pictureEditor.Invalidate();
        panelCharacters.Invalidate();
      }
    }



    private void comboMulticolor1_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( m_Charset.MultiColor1 != comboMulticolor1.SelectedIndex )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharsetValuesChange( this, m_Charset ) );

        m_Charset.MultiColor1 = comboMulticolor1.SelectedIndex;
        for ( int i = 0; i < 256; ++i )
        {
          RebuildCharImage( i );
        }
        Modified = true;
        pictureEditor.Invalidate();
        panelCharacters.Invalidate();
      }
    }



    private void comboMulticolor2_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( m_Charset.MultiColor2 != comboMulticolor2.SelectedIndex )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharsetValuesChange( this, m_Charset ) );

        m_Charset.MultiColor2 = comboMulticolor2.SelectedIndex;
        for ( int i = 0; i < 256; ++i )
        {
          RebuildCharImage( i );
        }
        Modified = true;
        pictureEditor.Invalidate();
        panelCharacters.Invalidate();
      }
    }



    private void comboCharColor_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( DoNotUpdateFromControls )
      {
        return;
      }

      List<int>   selectedChars = panelCharacters.SelectedIndices;
      if ( selectedChars.Count == 0 )
      {
        selectedChars.Add( m_CurrentChar );
      }

      bool    modified = false;
      foreach ( int selChar in selectedChars )
      {
        if ( m_Charset.Characters[selChar].Color != comboCharColor.SelectedIndex )
        {
          DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharsetCharChange( this, m_Charset, selChar ), modified == false );

          m_Charset.Characters[selChar].Color = comboCharColor.SelectedIndex;
          RebuildCharImage( selChar );
          modified = true;
          panelCharacters.InvalidateItemRect( selChar );
        }
      }
      if ( modified )
      {
        Modified = true;
        pictureEditor.Invalidate();
      }
    }



    private void openToolStripMenuItem_Click( object sender, EventArgs e )
    {
      string filename;

      if ( OpenFile( "Open Charset Project", C64Studio.Types.Constants.FILEFILTER_CHARSET_PROJECT + C64Studio.Types.Constants.FILEFILTER_ALL, out filename ) )
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
        RebuildCharImage( i );
      }
      comboCategories.Items.Clear();
      DocumentInfo.DocumentFilename = "";

      m_Charset.Categories.Clear();

      AddCategory( 0, "Uncategorized" );
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
      Clear();

      GR.Memory.ByteBuffer    projectFile = GR.IO.File.ReadAllBytes( Filename );

      DocumentInfo.DocumentFilename = Filename;

      OpenProject( projectFile );
    }



    public void OpenProject( GR.Memory.ByteBuffer ProjectData )
    {
      if ( !m_Charset.ReadFromBuffer( ProjectData ) )
      {
        return;
      }
      comboBackground.SelectedIndex   = m_Charset.BackgroundColor;
      comboMulticolor1.SelectedIndex  = m_Charset.MultiColor1;
      comboMulticolor2.SelectedIndex  = m_Charset.MultiColor2;
      editCharactersCount.Text        = m_Charset.NumCharacters.ToString();
      editCharactersFrom.Text         = m_Charset.StartCharacter.ToString();
      checkShowGrid.Checked           = m_Charset.ShowGrid;

      for ( int i = 0; i < 256; ++i )
      {
        RebuildCharImage( i );
      }
      panelCharacters.Invalidate();
      pictureEditor.Invalidate();
      RedrawColorChooser();
      Modified = false;

      comboCategories.Items.Clear();
      listCategories.Items.Clear();

      int categoryIndex = 0;
      foreach ( var category in m_Charset.Categories )
      {
        ListViewItem    itemCat = new ListViewItem( category );
        itemCat.SubItems.Add( "0" );
        itemCat.Tag = categoryIndex;
        listCategories.Items.Add( itemCat );
        comboCategories.Items.Add( category );
        ++categoryIndex;
      }
      RefreshCategoryCounts();
      SelectCategory( m_Charset.Characters[m_CurrentChar].Category );

      saveCharsetProjectToolStripMenuItem.Enabled = true;
      closeCharsetProjectToolStripMenuItem.Enabled = true;
    }



    private void SelectCategory( int Category )
    {
      comboCategories.SelectedItem = m_Charset.Categories[Category];
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
      if ( !GR.IO.File.WriteAllBytes( saveFilename, projectFile ) )
      {
        return false;
      }
      if ( !SaveAs )
      {
        Modified = false;
      }
      return true;
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
      panelCharacters.Invalidate();
      pictureEditor.Invalidate();

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
          listIndices.AddRange( panelCharacters.SelectedIndices );
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
            RebuildCharImage( i );
          }

          comboBackground.SelectedIndex = m_Charset.BackgroundColor;
          comboMulticolor1.SelectedIndex = m_Charset.MultiColor1;
          comboMulticolor2.SelectedIndex = m_Charset.MultiColor2;
          editCharactersFrom.Text = m_Charset.NumCharacters.ToString();
          checkShowGrid.Checked = m_Charset.ShowGrid;

          panelCharacters.Invalidate();
          pictureEditor.Invalidate();
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

            RebuildCharImage( charIndex );
          }

          comboBackground.SelectedIndex = m_Charset.BackgroundColor;
          comboMulticolor1.SelectedIndex = m_Charset.MultiColor1;
          comboMulticolor2.SelectedIndex = m_Charset.MultiColor2;
          editCharactersFrom.Text = m_Charset.NumCharacters.ToString();

          panelCharacters.Invalidate();
          pictureEditor.Invalidate();
          SetModified();
          return;



        }
        // treat as binary .chr file
        GR.Memory.ByteBuffer charData = GR.IO.File.ReadAllBytes( filename );

        int charsToImport = (int)charData.Length / 8;
        if ( charsToImport > 256 )
        {
          charsToImport = 256;
        }
        for ( int i = 0; i < charsToImport; ++i )
        {
          for ( int j = 0; j < 8; ++j )
          {
            m_Charset.Characters[i].Data.SetU8At( j, charData.ByteAt( i * 8 + j ) );
          }
          m_Charset.Characters[i].Mode = C64Studio.Types.CharsetMode.HIRES;
          m_Charset.Characters[i].Color = 1;
          RebuildCharImage( i );
        }
        panelCharacters.Invalidate();
        pictureEditor.Invalidate();
        SetModified();
      }
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
        RebuildCharImage( i );
      }
      panelCharacters.Invalidate();
      pictureEditor.Invalidate();
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
        RebuildCharImage( i );
      }
      panelCharacters.Invalidate();
      pictureEditor.Invalidate();
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
      RebuildCharImage( CharIndex );
      return true;
    }



    private void btnPasteFromClipboard_Click( object sender, EventArgs e )
    {
      if ( !Clipboard.ContainsImage() )
      {
        return;
      }
      IDataObject dataObj = Clipboard.GetDataObject();

      GR.Image.FastImage    imgClip = null;
      foreach ( string format in dataObj.GetFormats() )
      {
        if ( format == "DeviceIndependentBitmap" )
        {
          object dibData = dataObj.GetData( format );
          imgClip = GR.Image.FastImage.CreateImageFromHDIB( dibData );
          break;
        }
      }
      if ( imgClip == null )
      {
        return;
      }

      PasteImage( imgClip, checkPasteMultiColor.Checked );
    }



    private void PasteImage( GR.Image.FastImage Image, bool ForceMulticolor )
    {
      GR.Image.FastImage mappedImage = null;

      Types.MulticolorSettings   mcSettings = new Types.MulticolorSettings();
      mcSettings.BackgroundColor  = m_Charset.BackgroundColor;
      mcSettings.MultiColor1      = m_Charset.MultiColor1;
      mcSettings.MultiColor2      = m_Charset.MultiColor2;

      bool pasteAsBlock = false;
      if ( !Core.MainForm.ImportImage( "", Image, Types.GraphicType.CHARACTERS, mcSettings, out mappedImage, out mcSettings, out pasteAsBlock ) )
      {
        Image.Dispose();
        return;
      }

      if ( mappedImage.PixelFormat != System.Drawing.Imaging.PixelFormat.Format8bppIndexed )
      {
        mappedImage.Dispose();
        System.Windows.Forms.MessageBox.Show( "Image format invalid!\nNeeds to be 8bit index" );
        return;
      }

      comboBackground.SelectedIndex   = mcSettings.BackgroundColor;
      comboMulticolor1.SelectedIndex  = mcSettings.MultiColor1;
      comboMulticolor2.SelectedIndex  = mcSettings.MultiColor2;

      int charsX = ( mappedImage.Width + 7 ) / 8;
      int charsY = ( mappedImage.Height + 7 ) / 8;
      int curCharX = m_CurrentChar % 16;
      int curCharY = m_CurrentChar / 16;
      int currentTargetChar = m_CurrentChar;

      for ( int j = 0; j < charsY; ++j )
      {
        for ( int i = 0; i < charsX; ++i )
        {
          if ( pasteAsBlock )
          {
            int localCharX = ( curCharX + i ) % 16;
            int localCharY = curCharY + j;
            if ( curCharX + i >= 16 )
            {
              // wrap
              localCharY += charsY * ( ( curCharX + i ) / 16 );
            }
            if ( localCharY >= 16 )
            {
              continue;
            }
            currentTargetChar = localCharY * 16 + localCharX;
          }
          else if ( currentTargetChar >= 256 )
          {
            continue;
          }

          int copyWidth = mappedImage.Width - i * 8;
          if ( copyWidth > 8 )
          {
            copyWidth = 8;
          }
          int copyHeight = mappedImage.Height - j * 8;
          if ( copyHeight > 8 )
          {
            copyHeight = 8;
          }
          GR.Image.FastImage singleChar = mappedImage.GetImage( i * 8, j * 8, copyWidth, copyHeight );

          ImportChar( singleChar, currentTargetChar, ForceMulticolor );
          panelCharacters.InvalidateItemRect( currentTargetChar );

          if ( currentTargetChar == m_CurrentChar )
          {
            comboCharColor.SelectedIndex = m_Charset.Characters[m_CurrentChar].Color;
          }
          if ( !pasteAsBlock )
          {
            ++currentTargetChar;
          }
        }
      }

      pictureEditor.Invalidate();
      Modified = true;
    }



    private void btnImportCharsetFromFile_Click( object sender, EventArgs e )
    {
      string filename;

      if ( !OpenFile( "Import Sprites from Image", C64Studio.Types.Constants.FILEFILTER_IMAGE_FILES, out filename ) )
      {
        return;
      }
      System.Drawing.Bitmap bmpImage = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile( filename );
      GR.Image.FastImage imgClip = GR.Image.FastImage.FromImage( bmpImage );
      bmpImage.Dispose();
      if ( ( ( imgClip.Width % 8 ) != 0 )
      ||   ( ( imgClip.Height % 8 ) != 0 )
      ||   ( imgClip.PixelFormat != System.Drawing.Imaging.PixelFormat.Format8bppIndexed ) )
      {
        imgClip.Dispose();
        System.Windows.Forms.MessageBox.Show( "Image format invalid!\nNeeds to be 8bit index and have width/height a multiple of 8" );
        return;
      }
      int     charsX = imgClip.Width / 8;
      int     charsY = imgClip.Height / 8;

      for ( int i = 0; i < charsX; ++i )
      {
        for ( int j = 0; j < charsY; ++j )
        {
          if ( ( i >= 16 )
          ||   ( j >= 16 ) )
          {
            continue;
          }

          GR.Image.FastImage    singleChar = imgClip.GetImage( i * 8, j * 8, 8, 8 );

          ImportChar( singleChar, j * 16 + i, checkPasteMultiColor.Checked );
          panelCharacters.InvalidateItemRect( j * 16 + i );
        }
      }
      pictureEditor.Invalidate();
      Modified = true;
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
      comboCategories.Items.Insert( Index, itemNew.Text );

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
      comboCategories.Items.RemoveAt( CategoryIndex );
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



    private void comboCategories_SelectedIndexChanged( object sender, EventArgs e )
    {
      string    category = comboCategories.SelectedItem.ToString();

      int       categoryIndex = 0;
      foreach ( var categoryInfo in m_Charset.Categories )
      {
        if ( categoryInfo == category )
        {
          break;
        }
        ++categoryIndex;
      }
      if ( ( categoryIndex != -1 )
      &&   ( m_Charset.Characters[m_CurrentChar].Category != categoryIndex ) )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharsetCharChange( this, m_Charset, m_CurrentChar ) );

        m_Charset.Characters[m_CurrentChar].Category = categoryIndex;
        RefreshCategoryCounts();
        Modified = true;
      }
    }



    void ShiftLeft()
    {
      List<int>     selectedChars = panelCharacters.SelectedIndices;

      DocumentInfo.UndoManager.StartUndoGroup();
      foreach ( var index in selectedChars )
      {
        DocumentInfo.UndoManager.AddGroupedUndoTask( new Undo.UndoCharsetCharChange( this, m_Charset, index ) );

        for ( int y = 0; y < 8; ++y )
        {
          if ( ( m_Charset.Characters[index].Mode == C64Studio.Types.CharsetMode.MULTICOLOR )
          && ( m_Charset.Characters[index].Color >= 8 ) )
          {
            byte result = (byte)( (byte)( ( m_Charset.Characters[m_CurrentChar].Data.ByteAt( y ) & 0xc0 ) >> 6 )
                                | (byte)( ( m_Charset.Characters[index].Data.ByteAt( y ) & 0x3f ) << 2 ) );
            m_Charset.Characters[index].Data.SetU8At( y, result );
          }
          else
          {
            byte result = (byte)( (byte)( ( m_Charset.Characters[index].Data.ByteAt( y ) & 0x80 ) >> 7 )
                                | (byte)( ( m_Charset.Characters[index].Data.ByteAt( y ) & 0x7f ) << 1 ) );
            m_Charset.Characters[index].Data.SetU8At( y, result );
          }
        }
        RebuildCharImage( index );
        panelCharacters.InvalidateItemRect( index );
      }
      pictureEditor.Invalidate();
      Modified = true;
    }



    void ShiftRight()
    {
      List<int>     selectedChars = panelCharacters.SelectedIndices;

      DocumentInfo.UndoManager.StartUndoGroup();
      foreach ( var index in selectedChars )
      {
        DocumentInfo.UndoManager.AddGroupedUndoTask( new Undo.UndoCharsetCharChange( this, m_Charset, index ) );

        for ( int y = 0; y < 8; ++y )
        {
          if ( ( m_Charset.Characters[index].Mode == C64Studio.Types.CharsetMode.MULTICOLOR )
          &&   ( m_Charset.Characters[index].Color >= 8 ) )
          {
            byte result = (byte)( (byte)( ( m_Charset.Characters[index].Data.ByteAt( y ) & 0xfc ) >> 2 )
                                | (byte)( ( m_Charset.Characters[index].Data.ByteAt( y ) & 0x03 ) << 6 ) );
            m_Charset.Characters[index].Data.SetU8At( y, result );
          }
          else
          {
            byte result = (byte)( (byte)( ( m_Charset.Characters[index].Data.ByteAt( y ) & 0x01 ) << 7 )
                                | (byte)( ( m_Charset.Characters[index].Data.ByteAt( y ) & 0xfe ) >> 1 ) );
            m_Charset.Characters[index].Data.SetU8At( y, result );
          }
        }
        RebuildCharImage( index );
        panelCharacters.InvalidateItemRect( index );
      }
      pictureEditor.Invalidate();
      Modified = true;
    }



    private void btnShiftLeft_Click( object sender, EventArgs e )
    {
      ShiftLeft();
    }



    private void btnShiftRight_Click( object sender, EventArgs e )
    {
      ShiftRight();
    }



    private void btnShiftUp_Click( object sender, EventArgs e )
    {
      List<int>     selectedChars = panelCharacters.SelectedIndices;

      DocumentInfo.UndoManager.StartUndoGroup();
      foreach ( var index in selectedChars )
      {
        DocumentInfo.UndoManager.AddGroupedUndoTask( new Undo.UndoCharsetCharChange( this, m_Charset, index ) );

        byte  temp = m_Charset.Characters[index].Data.ByteAt( 0 );
        for ( int y = 0; y < 7; ++y )
        {
          m_Charset.Characters[index].Data.SetU8At( y, m_Charset.Characters[index].Data.ByteAt( y + 1 ) );
        }
        m_Charset.Characters[index].Data.SetU8At( 7, temp );
        RebuildCharImage( index );
        panelCharacters.InvalidateItemRect( index );
      }
      pictureEditor.Invalidate();
      Modified = true;
    }



    private void btnShiftDown_Click( object sender, EventArgs e )
    {
      List<int>     selectedChars = panelCharacters.SelectedIndices;

      DocumentInfo.UndoManager.StartUndoGroup();
      foreach ( var index in selectedChars )
      {
        DocumentInfo.UndoManager.AddGroupedUndoTask( new Undo.UndoCharsetCharChange( this, m_Charset, index ) );

        byte  temp = m_Charset.Characters[index].Data.ByteAt( 7 );
        for ( int y = 0; y < 7; ++y )
        {
          m_Charset.Characters[index].Data.SetU8At( 7 - y, m_Charset.Characters[index].Data.ByteAt( 7 - y - 1 ) );
        }
        m_Charset.Characters[index].Data.SetU8At( 0, temp );
        RebuildCharImage( index );
        panelCharacters.InvalidateItemRect( index );
      }
      pictureEditor.Invalidate();
      Modified = true;
    }



    private void btnMirrorX_Click( object sender, EventArgs e )
    {
      MirrorX();
    }



    private void btnMirrorY_Click( object sender, EventArgs e )
    {
      MirrorY();
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
        for ( int i = 0; i < 256; ++i )
        {
          RebuildCharImage( i );
        }
        panelCharacters.Invalidate();
        pictureEditor.Invalidate();
        RefreshCategoryCounts();
        Modified = true;
      }
    }



    private void CopyToClipboard()
    {
      List<int> selectedImages = panelCharacters.SelectedIndices;
      if ( selectedImages.Count == 0 )
      {
        return;
      }

      GR.Memory.ByteBuffer dataSelection = new GR.Memory.ByteBuffer();

      dataSelection.AppendI32( selectedImages.Count );
      dataSelection.AppendI32( panelCharacters.IsSelectionColumnBased ? 1 : 0 );
      int prevIndex = selectedImages[0];
      foreach ( int index in selectedImages )
      {
        // delta in indices
        dataSelection.AppendI32( index - prevIndex );
        prevIndex = index;

        dataSelection.AppendI32( (int)m_Charset.Characters[index].Mode );
        dataSelection.AppendI32( m_Charset.Characters[index].Color );
        dataSelection.AppendU32( 8 );
        dataSelection.AppendU32( 8 );
        dataSelection.AppendU32( m_Charset.Characters[index].Data.Length );
        dataSelection.Append( m_Charset.Characters[index].Data );
        dataSelection.AppendI32( index );
      }

      DataObject dataObj = new DataObject();

      dataObj.SetData( "C64Studio.ImageList", false, dataSelection.MemoryStream() );

      GR.Memory.ByteBuffer      dibData = m_Charset.Characters[m_CurrentChar].Image.CreateHDIBAsBuffer();

      System.IO.MemoryStream    ms = dibData.MemoryStream();

      // WTF - SetData requires streams, NOT global data (HGLOBAL)
      dataObj.SetData( "DeviceIndependentBitmap", ms );

      Clipboard.SetDataObject( dataObj, true );
    }



    private void PasteClipboardImageToChar()
    {
      IDataObject dataObj = Clipboard.GetDataObject();
      if ( dataObj == null )
      {
        System.Windows.Forms.MessageBox.Show( "No image on clipboard" );
        return;
      }
      if ( dataObj.GetDataPresent( "C64Studio.ImageList" ) )
      {
        System.IO.MemoryStream ms = (System.IO.MemoryStream)dataObj.GetData( "C64Studio.ImageList" );

        GR.Memory.ByteBuffer spriteData = new GR.Memory.ByteBuffer( (uint)ms.Length );

        ms.Read( spriteData.Data(), 0, (int)ms.Length );

        GR.IO.MemoryReader memIn = spriteData.MemoryReader();

        int rangeCount = memIn.ReadInt32();
        bool columnBased = ( memIn.ReadInt32() > 0 ) ? true : false;

        int pastePos = panelCharacters.SelectedIndex;
        if ( pastePos == -1 )
        {
          pastePos = 0;
        }

        for ( int i = 0; i < rangeCount; ++i )
        {
          int indexGap = memIn.ReadInt32();
          pastePos += indexGap;

          if ( pastePos >= m_Charset.Characters.Count )
          {
            break;
          }

          DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharsetCharChange( this, m_Charset, pastePos ), i == 0 );

          m_Charset.Characters[pastePos].Mode = (C64Studio.Types.CharsetMode)memIn.ReadInt32();
          m_Charset.Characters[pastePos].Color = memIn.ReadInt32();

          uint width   = memIn.ReadUInt32();
          uint height  = memIn.ReadUInt32();

          uint dataLength = memIn.ReadUInt32();

          GR.Memory.ByteBuffer    tempBuffer = new GR.Memory.ByteBuffer();
          tempBuffer.Reserve( (int)dataLength );
          memIn.ReadBlock( tempBuffer, dataLength );

          m_Charset.Characters[pastePos].Data = new GR.Memory.ByteBuffer( 8 );

          if ( ( width == 8 )
          &&   ( height == 8 ) )
          {
            tempBuffer.CopyTo( m_Charset.Characters[pastePos].Data, 0, 8 );
          }
          else if ( ( width == 24 )
          &&        ( height == 21 ) )
          {
            for ( int j = 0; j < 8; ++j )
            {
              m_Charset.Characters[pastePos].Data.SetU8At( j, tempBuffer.ByteAt( j * 3 ) );
            }
          }
          else
          {
            tempBuffer.CopyTo( m_Charset.Characters[pastePos].Data, 0, Math.Min( 8, (int)dataLength ) );
          }


          int index = memIn.ReadInt32();

          RebuildCharImage( pastePos );
          panelCharacters.InvalidateItemRect( pastePos );

          if ( pastePos == m_CurrentChar )
          {
            comboCharColor.SelectedIndex = m_Charset.Characters[pastePos].Color;
          }
        }
        pictureEditor.Invalidate();
        Modified = true;
        return;
      }
      else if ( !Clipboard.ContainsImage() )
      {
        System.Windows.Forms.MessageBox.Show( "No image on clipboard" );
        return;
      }
      GR.Image.FastImage imgClip = null;
      foreach ( string format in dataObj.GetFormats() )
      {
        if ( format == "DeviceIndependentBitmap" )
        {
          object dibData = dataObj.GetData( format );
          imgClip = GR.Image.FastImage.CreateImageFromHDIB( dibData );
          break;
        }
      }
      if ( imgClip == null )
      {
        System.Windows.Forms.MessageBox.Show( "No image on clipboard" );
        return;
      }
      PasteImage( imgClip, checkPasteMultiColor.Checked );
    }



    private void btnCopy_Click( object sender, EventArgs e )
    {
      CopyToClipboard();
    }



    private void btnPaste_Click( object sender, EventArgs e )
    {
      PasteClipboardImageToChar();
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
      for ( int i = 0; i < 256; ++i )
      {
        RebuildCharImage( i );
        panelCharacters.Items[i].MemoryImage = m_Charset.Characters[i].Image;
      }
      panelCharacters.Invalidate();
      pictureEditor.Invalidate();
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
      for ( int i = 0; i < 256; ++i )
      {
        RebuildCharImage( i );
        panelCharacters.Items[i].MemoryImage = m_Charset.Characters[i].Image;
      }
      panelCharacters.Invalidate();
      pictureEditor.Invalidate();
      RefreshCategoryCounts();
      Modified = true;
    }



    private void checkShowGrid_CheckedChanged( object sender, EventArgs e )
    {
      m_Charset.ShowGrid = checkShowGrid.Checked;
      pictureEditor.Invalidate();
    }



    private void btnInvert_Click( object sender, EventArgs e )
    {
      List<int>     selectedChars = panelCharacters.SelectedIndices;

      DocumentInfo.UndoManager.StartUndoGroup();
      foreach ( var index in selectedChars )
      {
        DocumentInfo.UndoManager.AddGroupedUndoTask( new Undo.UndoCharsetCharChange( this, m_Charset, index ) );

        for ( int y = 0; y < 8; ++y )
        {
          byte result = (byte)( ~m_Charset.Characters[index].Data.ByteAt( y ) );
          m_Charset.Characters[index].Data.SetU8At( y, result );
        }
        RebuildCharImage( index );
        panelCharacters.InvalidateItemRect( index );
      }
      pictureEditor.Invalidate();
      Modified = true;
    }



    private void btnRotateLeft_Click( object sender, EventArgs e )
    {
      List<int>     selectedChars = panelCharacters.SelectedIndices;

      DocumentInfo.UndoManager.StartUndoGroup();
      foreach ( var index in selectedChars )
      {
        DocumentInfo.UndoManager.AddGroupedUndoTask( new Undo.UndoCharsetCharChange( this, m_Charset, index ) );

        GR.Memory.ByteBuffer resultData = new GR.Memory.ByteBuffer( 8 );

        if ( ( m_Charset.Characters[index].Mode == C64Studio.Types.CharsetMode.MULTICOLOR )
        ||   ( m_Charset.Characters[index].Color >= 8 ) )
        {
          for ( int i = 0; i < 8; i += 2 )
          {
            for ( int j = 0; j < 8; ++j )
            {
              int sourceX = 7 - j;
              int sourceY = i;

              if ( ( sourceX < 0 )
              ||   ( sourceX >= 8 )
              ||   ( sourceY < 0 )
              ||   ( sourceY >= 8 ) )
              {
                continue;
              }
              int maskOffset = 6 - ( ( sourceX % 8 ) / 2 ) * 2;
              byte sourceColor = (byte)( ( m_Charset.Characters[index].Data.ByteAt( sourceY ) & ( 3 << maskOffset ) ) >> maskOffset );

              maskOffset = 6 - ( ( i % 8 ) / 2 ) * 2;
              resultData.SetU8At( j, (byte)( resultData.ByteAt( j ) | ( sourceColor << maskOffset ) ) );
            }
          }
        }
        else
        {
          for ( int i = 0; i < 8; ++i )
          {
            for ( int j = 0; j < 8; ++j )
            {
              int sourceX = i;
              int sourceY = j;
              int targetX = j;
              int targetY = 7 - i;
              if ( ( m_Charset.Characters[index].Data.ByteAt( sourceY ) & ( 1 << ( 7 - ( sourceX % 8 ) ) ) ) != 0 )
              {
                resultData.SetU8At( targetY, (byte)( resultData.ByteAt( targetY ) | ( 1 << ( 7 - targetX % 8 ) ) ) );
              }
            }
          }
        }
        m_Charset.Characters[index].Data = resultData;
        RebuildCharImage( index );
        panelCharacters.InvalidateItemRect( index );
      }
      pictureEditor.Invalidate();
      Modified = true;
    }



    private void btnRotateRight_Click( object sender, EventArgs e )
    {
      List<int>     selectedChars = panelCharacters.SelectedIndices;

      DocumentInfo.UndoManager.StartUndoGroup();
      foreach ( var index in selectedChars )
      {
        DocumentInfo.UndoManager.AddGroupedUndoTask( new Undo.UndoCharsetCharChange( this, m_Charset, index ) );

        GR.Memory.ByteBuffer resultData = new GR.Memory.ByteBuffer( 8 );

        if ( ( m_Charset.Characters[index].Mode == C64Studio.Types.CharsetMode.MULTICOLOR )
        ||   ( m_Charset.Characters[index].Color >= 8 ) )
        {
          for ( int i = 0; i < 8; i += 2 )
          {
            for ( int j = 0; j < 8; ++j )
            {
              int sourceX = j;
              int sourceY = 7 - i;

              if ( ( sourceX < 0 )
              ||   ( sourceX >= 8 )
              ||   ( sourceY < 0 )
              ||   ( sourceY >= 8 ) )
              {
                continue;
              }
              int maskOffset = 6 - ( ( sourceX % 8 ) / 2 ) * 2;
              byte sourceColor = (byte)( ( m_Charset.Characters[index].Data.ByteAt( sourceY ) & ( 3 << maskOffset ) ) >> maskOffset );

              maskOffset = 6 - ( ( i % 8 ) / 2 ) * 2;
              resultData.SetU8At( j, (byte)( resultData.ByteAt( j ) | ( sourceColor << maskOffset ) ) );
            }
          }
        }
        else
        {
          for ( int i = 0; i < 8; ++i )
          {
            for ( int j = 0; j < 8; ++j )
            {
              int sourceX = i;
              int sourceY = j;
              int targetX = 7 - j;
              int targetY = i;
              if ( ( m_Charset.Characters[index].Data.ByteAt( sourceY ) & ( 1 << ( 7 - ( sourceX % 8 ) ) ) ) != 0 )
              {
                resultData.SetU8At( targetY, (byte)( resultData.ByteAt( targetY ) | ( 1 << ( 7 - targetX % 8 ) ) ) );
              }
            }
          }
        }
        m_Charset.Characters[index].Data = resultData;
        RebuildCharImage( index );
        panelCharacters.InvalidateItemRect( index );
      }
      pictureEditor.Invalidate();
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





    private void exchangeMultiColors1And2ToolStripMenuItem_Click( object sender, EventArgs e )
    {
      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharsetExchangeMultiColors( this, C64Studio.Undo.UndoCharsetExchangeMultiColors.ExchangeMode.MULTICOLOR_1_WITH_MULTICOLOR_2 ) );

      ExchangeMultiColors();
    }



    public void ExchangeMultiColors()
    {
      int   temp = m_Charset.MultiColor1;
      m_Charset.MultiColor1 = m_Charset.MultiColor2;
      m_Charset.MultiColor2 = temp;

      int   charIndex = 0;
      foreach ( var charInfo in m_Charset.Characters )
      {
        if ( ( charInfo.Mode == C64Studio.Types.CharsetMode.MULTICOLOR )
        &&   ( charInfo.Color >= 8 ) )
        {
          for ( int y = 0; y < 8; ++y )
          {
            for ( int x = 0; x < 4; ++x )
            {
              int pixelValue = ( charInfo.Data.ByteAt( y ) & ( 3 << ( ( 3 - x ) * 2 ) ) ) >> ( ( 3 - x ) * 2 );

              if ( pixelValue == 1 )
              {
                byte  newValue = (byte)( charInfo.Data.ByteAt( y ) & ~( 3 << ( ( 3 - x ) * 2 ) ) );

                newValue |= (byte)( 2 << ( ( 3 - x ) * 2 ) );

                charInfo.Data.SetU8At( y, newValue );
              }
              else if ( pixelValue == 2 )
              {
                byte  newValue = (byte)( charInfo.Data.ByteAt( y ) & ~( 3 << ( ( 3 - x ) * 2 ) ) );

                newValue |= (byte)( 1 << ( ( 3 - x ) * 2 ) );

                charInfo.Data.SetU8At( y, newValue );
              }
            }
          }
          RebuildCharImage( charIndex );
          panelCharacters.Invalidate();
        }
        ++charIndex;
      }
      comboMulticolor1.SelectedIndex = m_Charset.MultiColor1;
      comboMulticolor2.SelectedIndex = m_Charset.MultiColor2;

      Modified = true;
    }



    private void panelCharacters_SelectionChanged( object sender, EventArgs e )
    {
      int newChar = panelCharacters.SelectedIndex;
      if ( ( newChar != -1 )
      &&   ( panelCharacters.SelectedIndices.Count == 1 ) )
      {
        labelCharNo.Text = "Character: " + newChar.ToString();
        m_CurrentChar = newChar;
        if ( comboCharColor.SelectedIndex != m_Charset.Characters[m_CurrentChar].Color )
        {
          comboCharColor.SelectedIndex = m_Charset.Characters[m_CurrentChar].Color;
        }
        if ( comboCharsetMode.SelectedIndex != (int)m_Charset.Characters[m_CurrentChar].Mode )
        {
          comboCharsetMode.SelectedIndex = (int)m_Charset.Characters[m_CurrentChar].Mode;
        }
        SelectCategory( m_Charset.Characters[m_CurrentChar].Category );
        pictureEditor.Image = m_Charset.Characters[m_CurrentChar].Image;
        pictureEditor.Invalidate();

        RedrawColorChooser();
      }
    }



    private void exchangeMultiColor1AndBGColorToolStripMenuItem_Click( object sender, EventArgs e )
    {
      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharsetExchangeMultiColors( this, C64Studio.Undo.UndoCharsetExchangeMultiColors.ExchangeMode.MULTICOLOR_1_WITH_BACKGROUND ) );

      ExchangeMultiColor1WithBackground();
    }



    public void ExchangeMultiColor1WithBackground()
    {
      int   temp = m_Charset.BackgroundColor;
      m_Charset.BackgroundColor = m_Charset.MultiColor1;
      m_Charset.MultiColor1 = temp;

      int   charIndex = 0;
      foreach ( var charInfo in m_Charset.Characters )
      {
        if ( ( charInfo.Mode == C64Studio.Types.CharsetMode.MULTICOLOR )
        &&   ( charInfo.Color >= 8 ) )
        {
          for ( int y = 0; y < 8; ++y )
          {
            for ( int x = 0; x < 4; ++x )
            {
              int pixelValue = ( charInfo.Data.ByteAt( y ) & ( 3 << ( ( 3 - x ) * 2 ) ) ) >> ( ( 3 - x ) * 2 );

              if ( pixelValue == 1 )
              {
                byte  newValue = (byte)( charInfo.Data.ByteAt( y ) & ~( 3 << ( ( 3 - x ) * 2 ) ) );

                charInfo.Data.SetU8At( y, newValue );
              }
              else if ( pixelValue == 0 )
              {
                byte  newValue = (byte)( charInfo.Data.ByteAt( y ) & ~( 3 << ( ( 3 - x ) * 2 ) ) );

                newValue |= (byte)( 1 << ( ( 3 - x ) * 2 ) );

                charInfo.Data.SetU8At( y, newValue );
              }
            }
          }
        }
        RebuildCharImage( charIndex );
        panelCharacters.Invalidate();
        ++charIndex;
      }
      comboMulticolor1.SelectedIndex  = m_Charset.MultiColor1;
      comboBackground.SelectedIndex   = m_Charset.BackgroundColor;

      Modified = true;
    }



    private void exchangeMultiColor2AndBGColorToolStripMenuItem_Click( object sender, EventArgs e )
    {
      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharsetExchangeMultiColors( this, C64Studio.Undo.UndoCharsetExchangeMultiColors.ExchangeMode.MULTICOLOR_2_WITH_BACKGROUND ) );

      ExchangeMultiColor2WithBackground();
    }



    public void ExchangeMultiColor2WithBackground()
    {
      int   temp = m_Charset.BackgroundColor;
      m_Charset.BackgroundColor = m_Charset.MultiColor2;
      m_Charset.MultiColor2 = temp;

      int   charIndex = 0;
      foreach ( var charInfo in m_Charset.Characters )
      {
        if ( ( charInfo.Mode == C64Studio.Types.CharsetMode.MULTICOLOR )
        &&   ( charInfo.Color >= 8 ) )
        {
          for ( int y = 0; y < 8; ++y )
          {
            for ( int x = 0; x < 4; ++x )
            {
              int pixelValue = ( charInfo.Data.ByteAt( y ) & ( 3 << ( ( 3 - x ) * 2 ) ) ) >> ( ( 3 - x ) * 2 );

              if ( pixelValue == 2 )
              {
                byte  newValue = (byte)( charInfo.Data.ByteAt( y ) & ~( 3 << ( ( 3 - x ) * 2 ) ) );

                //newValue |= (byte)( 2 << ( ( 3 - x ) * 2 ) );

                charInfo.Data.SetU8At( y, newValue );
              }
              else if ( pixelValue == 0 )
              {
                byte  newValue = (byte)( charInfo.Data.ByteAt( y ) & ~( 3 << ( ( 3 - x ) * 2 ) ) );

                newValue |= (byte)( 2 << ( ( 3 - x ) * 2 ) );

                charInfo.Data.SetU8At( y, newValue );
              }
            }
          }
        }
        RebuildCharImage( charIndex );
        panelCharacters.Invalidate();
        ++charIndex;
      }
      comboMulticolor2.SelectedIndex = m_Charset.MultiColor2;
      comboBackground.SelectedIndex = m_Charset.BackgroundColor;

      Modified = true;
    }



    public void PlaygroundCharacterChanged( int X, int Y )
    {
      Displayer.CharacterDisplayer.DisplayChar( m_Charset, m_Charset.PlaygroundChars[X + Y * 16] & 0xff, m_ImagePlayground, X * 8, Y * 8, m_Charset.PlaygroundChars[X + Y * 16] >> 8 );
      RedrawPlayground();
    }



    public void CharacterChanged( int CharIndex )
    {
      DoNotUpdateFromControls = true;

      bool currentCharChanged = false;

      if ( m_Charset.Characters[CharIndex].Mode == C64Studio.Types.CharsetMode.ECM )
      {
        for ( int i = 0; i < 4; ++i )
        {
          RebuildCharImage( ( CharIndex + i * 64 ) % 256 );
          panelCharacters.InvalidateItemRect( ( CharIndex + i * 64 ) % 256 );

          if ( m_CurrentChar == CharIndex )
          {
            currentCharChanged = true;
          }
        }
      }
      else
      {
        RebuildCharImage( CharIndex );
        panelCharacters.InvalidateItemRect( CharIndex );
        currentCharChanged = ( m_CurrentChar == CharIndex );
      }

      RefreshCategoryCounts();

      if ( currentCharChanged )
      {
        panelCharacters_SelectionChanged( null, null );
        comboCharColor.SelectedIndex = m_Charset.Characters[CharIndex].Color;

        comboCharsetMode.SelectedIndex = (int)m_Charset.Characters[CharIndex].Mode;
      }

      DoNotUpdateFromControls = false;
    }



    public void ColorsChanged()
    {
      comboMulticolor1.SelectedIndex  = m_Charset.MultiColor1;
      comboMulticolor2.SelectedIndex  = m_Charset.MultiColor2;
      comboBGColor4.SelectedIndex     = m_Charset.BGColor4;
      comboBackground.SelectedIndex   = m_Charset.BackgroundColor;

      for ( int i = 0; i < 256; ++i )
      {
        RebuildCharImage( i );
      }
      pictureEditor.Invalidate();
      panelCharacters.Invalidate();
    }



    private void btnClear_Click( object sender, EventArgs e )
    {
      bool  wasModified = false;
      var   selectedChars = panelCharacters.SelectedIndices;
      bool  firstUndoStep = true;

      DoNotUpdateFromControls = true;

      foreach ( int i in selectedChars )
      {
        wasModified = true;

        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharsetCharChange( this, m_Charset, i ), firstUndoStep );
        firstUndoStep = false;

        for ( int j = 0; j < 8; ++j )
        {
          m_Charset.Characters[i].Data.SetU8At( j, 0 );
        }
        RebuildCharImage( i );
        panelCharacters.InvalidateItemRect( i );

        if ( i == m_CurrentChar )
        {
          comboCharsetMode.SelectedIndex = (int)Types.CharsetMode.HIRES;
          comboCharColor.SelectedIndex = m_Charset.Characters[i].Color;
        }
      }
      if ( wasModified )
      {
        pictureEditor.Invalidate();
        SetModified();
      }
      DoNotUpdateFromControls = false;
    }



    private void labelCharNo_Paint( object sender, PaintEventArgs e )
    {
      e.Graphics.FillRectangle( System.Drawing.SystemBrushes.Window, labelCharNo.ClientRectangle );
      e.Graphics.DrawString( labelCharNo.Text, labelCharNo.Font, System.Drawing.SystemBrushes.WindowText, labelCharNo.ClientRectangle );

      if ( !Types.ConstantData.ScreenCodeToChar.ContainsKey( (byte)m_CurrentChar ) )
      {
        Debug.Log( "Missing char for " + m_CurrentChar );
      }
      else
      {
        e.Graphics.DrawString( "" + Types.ConstantData.ScreenCodeToChar[(byte)m_CurrentChar].CharValue, new System.Drawing.Font( Core.MainForm.m_FontC64.Families[0], 16, System.Drawing.GraphicsUnit.Pixel ), System.Drawing.SystemBrushes.WindowText, 100, 0 );
      }
    }



    private void comboBGColor4_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( m_Charset.BGColor4 != comboBGColor4.SelectedIndex )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharsetValuesChange( this, m_Charset ) );

        m_Charset.BGColor4 = comboBGColor4.SelectedIndex;
        for ( int i = 0; i < 256; ++i )
        {
          RebuildCharImage( i );
        }
        Modified = true;
        pictureEditor.Invalidate();
        panelCharacters.Invalidate();
      }
    }



    private void comboCharsetMode_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( DoNotUpdateFromControls )
      {
        return;
      }

      List<int>   selectedChars = panelCharacters.SelectedIndices;
      if ( selectedChars.Count == 0 )
      {
        selectedChars.Add( m_CurrentChar );
      }

      bool modified = false;
      foreach ( int selChar in selectedChars )
      {
        if ( m_Charset.Characters[selChar].Mode != (C64Studio.Types.CharsetMode)comboCharsetMode.SelectedIndex )
        {
          DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharsetCharChange( this, m_Charset, selChar ), !modified );
          m_Charset.Characters[selChar].Mode = (C64Studio.Types.CharsetMode)comboCharsetMode.SelectedIndex;
          RebuildCharImage( selChar );
          panelCharacters.InvalidateItemRect( selChar );
          modified = true;
        }
      }

      comboBGColor4.Enabled = ( m_Charset.Characters[m_CurrentChar].Mode == C64Studio.Types.CharsetMode.ECM );
      radioBGColor4.Enabled = ( m_Charset.Characters[m_CurrentChar].Mode == C64Studio.Types.CharsetMode.ECM );
      comboMulticolor1.Enabled = ( m_Charset.Characters[m_CurrentChar].Mode != C64Studio.Types.CharsetMode.HIRES );
      radioMultiColor1.Enabled = ( m_Charset.Characters[m_CurrentChar].Mode != C64Studio.Types.CharsetMode.HIRES );
      comboMulticolor2.Enabled = ( m_Charset.Characters[m_CurrentChar].Mode != C64Studio.Types.CharsetMode.HIRES );
      radioMulticolor2.Enabled = ( m_Charset.Characters[m_CurrentChar].Mode != C64Studio.Types.CharsetMode.HIRES );

      if ( m_Charset.Characters[m_CurrentChar].Mode == C64Studio.Types.CharsetMode.ECM )
      {
        radioMultiColor1.Text = "BGColor 2";
        radioMulticolor2.Text = "BGColor 3";
      }
      else
      {
        radioMultiColor1.Text = "Multicolor 1";
        radioMulticolor2.Text = "Multicolor 2";
      }

      if ( modified )
      {
        Modified = true;
        pictureEditor.Invalidate();
      }
    }



    private void RedrawPlayground()
    {
      picturePlayground.DisplayPage.DrawFromMemoryImage( m_ImagePlayground, 0, 0 );
      picturePlayground.Invalidate();
    }



    private void RedrawColorChooser()
    {
      for ( byte i = 0; i < 16; ++i )
      {
        Displayer.CharacterDisplayer.DisplayChar( m_Charset, m_CurrentChar, panelCharColors.DisplayPage, i * 8, 0, i );
      }
      for ( int i = 0; i < 8; ++i )
      {
        panelCharColors.DisplayPage.SetPixel( m_CurrentColor * 8 + i, 0, 16 );
        panelCharColors.DisplayPage.SetPixel( m_CurrentColor * 8 + i, 7, 16 );
        panelCharColors.DisplayPage.SetPixel( m_CurrentColor * 8, i, 16 );
        panelCharColors.DisplayPage.SetPixel( m_CurrentColor * 8 + 7, i, 16 );
      }
      panelCharColors.Invalidate();
    }



    private void picturePlayground_MouseDown( object sender, MouseEventArgs e )
    {
      picturePlayground.Focus();
      HandleMouseOnPlayground( e.X, e.Y, e.Button );
    }



    private void picturePlayground_MouseMove( object sender, MouseEventArgs e )
    {
      HandleMouseOnPlayground( e.X, e.Y, e.Button );
    }



    private void panelCharColors_MouseDown( object sender, MouseEventArgs e )
    {
      HandleMouseOnColorChooser( e.X, e.Y, e.Button );
    }



    private void panelCharColors_MouseMove( object sender, MouseEventArgs e )
    {
      HandleMouseOnColorChooser( e.X, e.Y, e.Button );
    }



    private void HandleMouseOnColorChooser( int X, int Y, MouseButtons Buttons )
    {
      if ( ( Buttons & MouseButtons.Left ) == MouseButtons.Left )
      {
        int colorIndex = X / 16;
        m_CurrentColor = (byte)colorIndex;
        RedrawColorChooser();
      }
    }



    private void HandleMouseOnPlayground( int X, int Y, MouseButtons Buttons )
    {
      int     charX = X / ( pictureEditor.ClientRectangle.Width / 16 );
      int     charY = Y / ( pictureEditor.ClientRectangle.Height / 16 );

      if ( ( Buttons & MouseButtons.Left ) == 0 )
      {
        m_ButtonReleased = true;
      }

      if ( ( charX < 0 )
      ||   ( charX >= 16 )
      ||   ( charY < 0 )
      ||   ( charY >= 16 ) )
      {
        return;
      }

      if ( ( Buttons & MouseButtons.Left ) != 0 )
      {
        if ( m_Charset.PlaygroundChars[charX + charY * 16] != (ushort)( m_CurrentChar | ( m_CurrentColor << 8 ) ) )
        {
          DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharsetPlaygroundCharChange( this, m_Charset, charX, charY ) );

          Displayer.CharacterDisplayer.DisplayChar( m_Charset, m_CurrentChar, m_ImagePlayground, charX * 8, charY * 8, m_CurrentColor );
          RedrawPlayground();

          m_Charset.PlaygroundChars[charX + charY * 16] = (ushort)( m_CurrentChar | ( m_CurrentColor << 8 ) );
          Modified = true;
        }
      }
      if ( ( Buttons & MouseButtons.Right ) != 0 )
      {
        m_CurrentChar = (byte)( m_Charset.PlaygroundChars[charX + charY * 16] & 0x00ff );
        m_CurrentColor = (byte)( m_Charset.PlaygroundChars[charX + charY * 16] >> 8 );
        panelCharacters.SelectedIndex = m_CurrentChar;
        RedrawColorChooser();
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



  }
}


