using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using C64Studio.Types;
using RetroDevStudio;
using RetroDevStudio.Types;



namespace C64Studio
{
  public partial class GraphicScreenEditor : BaseDocument, IDisposable
  {
    private enum ImageInsertionMode
    {
      AS_FULL_SCREEN,
      AT_SELECTED_LOCATION,
      AS_FLOATING_SELECTION
    }

    private enum ColorType
    {
      BACKGROUND = 0,
      MULTICOLOR_1,
      MULTICOLOR_2,
      CHAR_COLOR
    }

    private enum PaintTool
    {
      DRAW_PIXEL,
      DRAW_RECTANGLE,
      DRAW_BOX,
      FLOOD_FILL,
      SELECT,
      VALIDATE
    }

    private enum ExportBinaryType
    {
      [Description( "Bitmap, Screen, Color" )] 
      BITMAP_SCREEN_COLOR = 0,
      [Description( "Bitmap, Color, Screen" )] 
      BITMAP_COLOR_SCREEN,
      [Description( "Bitmap, Screen" )] 
      BITMAP_SCREEN,
      [Description( "Bitmap, Color" )] 
      BITMAP_COLOR,
      [Description( "Bitmap" )] 
      BITMAP
    };



    private enum ExportDataType
    {
      [Description( "Hires Bitmap" )]
      HIRES_BITMAP,
      [Description( "Multicolor Bitmap" )]
      MULTICOLOR_BITMAP,
      [Description( "HiRes Charset" )]
      HIRES_CHARSET,
      [Description( "HiRes Charset and screen data assembly" )]
      HIRES_CHARSET_SCREEN_ASSEMBLY,
      [Description( "Multicolor Charset" )]
      MULTICOLOR_CHARSET,
      [Description( "Multicolor Charset and screen data assembly" )]
      MULTICOLOR_CHARSET_SCREEN_ASSEMBLY,
      [Description( "Used chars to Clipboard" )]
      CHARACTERS_TO_CLIPBOARD
    };


    private bool[,]                     m_ErrornousChars = new bool[40, 25];

    private bool                        m_ButtonReleased = false;

    private byte                        m_CurrentColor = 1;

    private List<Formats.CharData>      m_Chars = new List<Formats.CharData>();
    private System.Drawing.Point        m_SelectedChar = new System.Drawing.Point( -1, -1 );

    public Formats.GraphicScreenProject m_GraphicScreenProject = new C64Studio.Formats.GraphicScreenProject();

    private PaintTool                   m_PaintTool = PaintTool.VALIDATE;

    private System.Drawing.Point        m_DragStartPoint = new System.Drawing.Point( -1, -1 );
    private System.Drawing.Point        m_DragCurrentPoint;
    private System.Drawing.Rectangle    m_Selection = new System.Drawing.Rectangle( 0, 0, 0, 0 );

    private bool                        m_SelectionFloating = false;
    private GR.Image.IImage             m_SelectionFloatingImage = null;
    private System.Drawing.Point        m_SelectionFloatingPos = new System.Drawing.Point( 0, 0 );



    public GraphicScreenEditor( StudioCore Core )
    {
      this.Core = Core;
      DocumentInfo.Type = ProjectElement.ElementType.GRAPHIC_SCREEN;
      DocumentInfo.UndoManager.MainForm = Core.MainForm;
      m_IsSaveable = true;
      InitializeComponent();

      GR.Image.DPIHandler.ResizeControlsForDPI( this );

      pictureEditor.DisplayPage.Create( 320, 200, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );
      charEditor.DisplayPage.Create( 8, 8, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );
      m_GraphicScreenProject.Image.Create( 320, 200, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );
      colorSelector.DisplayPage.Create( 16 * 8, 8, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );

      PaletteManager.ApplyPalette( pictureEditor.DisplayPage );
      PaletteManager.ApplyPalette( charEditor.DisplayPage );
      PaletteManager.ApplyPalette( m_GraphicScreenProject.Image );
      PaletteManager.ApplyPalette( colorSelector.DisplayPage );

      foreach ( C64Studio.Formats.GraphicScreenProject.ColorMappingTarget entry in System.Enum.GetValues( typeof( C64Studio.Formats.GraphicScreenProject.ColorMappingTarget ) ) )
      {
        comboColorMappingTargets.Items.Add( GR.EnumHelper.GetDescription( entry ) );
      }

      for ( int i = 0; i < 16; ++i )
      {
        comboBackground.Items.Add( i.ToString( "d2" ) );
        comboMulticolor1.Items.Add( i.ToString( "d2" ) );
        comboMulticolor2.Items.Add( i.ToString( "d2" ) );
        comboCharColor.Items.Add( i.ToString( "d2" ) );

        listColorMappingColors.Items.Add( i.ToString( "d2" ) );

        colorSelector.DisplayPage.Box( i * 8, 0, 8, 8, (uint)i );
      }
      colorSelector.DisplayPage.Rectangle( m_CurrentColor * 8, 0, 8, 8, 16 );
      comboBackground.SelectedIndex = 0;
      comboMulticolor1.SelectedIndex = 0;
      comboMulticolor2.SelectedIndex = 0;
      comboCharColor.SelectedIndex = 1;
      comboColorMappingTargets.SelectedIndex = 0;

      foreach ( Formats.GraphicScreenProject.CheckType checkType in System.Enum.GetValues( typeof( Formats.GraphicScreenProject.CheckType ) ) )
      {
        comboCheckType.Items.Add( GR.EnumHelper.GetDescription( checkType ) );
      }
      foreach ( ExportDataType exportType in System.Enum.GetValues( typeof( ExportDataType ) ) )
      {
        comboExportType.Items.Add( GR.EnumHelper.GetDescription( exportType ) );
      }

      comboCheckType.SelectedIndex = 1;
      comboExportType.SelectedIndex = 1;

      foreach ( ExportBinaryType exportType in System.Enum.GetValues( typeof( ExportBinaryType ) ) )
      {
        comboExportData.Items.Add( GR.EnumHelper.GetDescription( exportType ) );
      }
      comboExportData.SelectedIndex = 0;

      checkExportToDataIncludeRes.Checked = true;
      checkExportToDataWrap.Checked = true;

      for ( int j = 0; j < BlockHeight; ++j )
      {
        for ( int i = 0; i < BlockWidth; ++i )
        {
          m_Chars.Add( new Formats.CharData() );
        }
      }

      editScreenWidth.Text = "320";
      editScreenHeight.Text = "200";
      SetScreenSize( 320, 200 );

      Core.MainForm.ApplicationEvent += new MainForm.ApplicationEventHandler( MainForm_ApplicationEvent );

      comboCharScreens.Items.Add( new Types.ComboItem( "To new file" ) );
      foreach ( DocumentInfo doc in Core.MainForm.DocumentInfos )
      {
        if ( doc.Type == ProjectElement.ElementType.CHARACTER_SCREEN )
        {
          comboCharScreens.Items.Add( new Types.ComboItem( doc.DocumentFilename, doc ) );
        }
      }
      comboCharScreens.SelectedIndex = 0;

      pictureEditor.PreviewKeyDown += new PreviewKeyDownEventHandler( pictureEditor_PreviewKeyDown );
    }



    void pictureEditor_PreviewKeyDown( object sender, PreviewKeyDownEventArgs e )
    {
      KeyEventArgs ke = new KeyEventArgs( e.KeyData );
      HandleKeyDown( sender, ke );
    }



    private int BlockWidth
    {
      get
      {
        return ( m_GraphicScreenProject.ScreenWidth + 7 ) / 8;
      }
    }



    private int BlockHeight
    {
      get
      {
        return ( m_GraphicScreenProject.ScreenHeight + 7 ) / 8;
      }
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



    void HandleKeyDown( object sender, KeyEventArgs e )
    {
      if ( ( e.Modifiers == Keys.Control )
      &&   ( e.KeyCode == Keys.C ) )
      {
        // copy
        CopySelectedImageToClipboard();
      }
      else if ( ( e.Modifiers == Keys.Control )
      &&        ( e.KeyCode == Keys.V ) )
      {
        // paste
        PasteClipboardImageToSelectedChar();
      }
    }



    private void comboColor_DrawItem( object sender, DrawItemEventArgs e )
    {
      ComboBox combo = (ComboBox)sender;

      Core.Theming.DrawSingleColorComboBox( combo, e, ConstantData.Palette );
    }



    private void comboMulticolor_DrawItem( object sender, DrawItemEventArgs e )
    {
      ComboBox combo = (ComboBox)sender;

      Core.Theming.DrawMultiColorComboBox( combo, e, ConstantData.Palette );
    }



    private void pictureEditor_MouseDown( object sender, MouseEventArgs e )
    {
      pictureEditor.Focus();
      HandleMouseOnEditor( e.X, e.Y, e.Button );
    }



    private void HandleMouseOnEditor( int X, int Y, MouseButtons Buttons )
    {
      int     charX = X / ( pictureEditor.ClientRectangle.Width / 40 ) + m_GraphicScreenProject.ScreenOffsetX;
      int     charY = Y / ( pictureEditor.ClientRectangle.Height / 25 ) + m_GraphicScreenProject.ScreenOffsetY;

      int     pixelX = ( X / ( pictureEditor.ClientRectangle.Width / pictureEditor.DisplayPage.Width ) ) + m_GraphicScreenProject.ScreenOffsetX;
      int     pixelY = ( Y / ( pictureEditor.ClientRectangle.Height / pictureEditor.DisplayPage.Height ) ) + m_GraphicScreenProject.ScreenOffsetY;

      if ( pixelX < 0 )
      {
        pixelX = 0;
      }
      if ( pixelX >= m_GraphicScreenProject.ScreenWidth )
      {
        pixelX = m_GraphicScreenProject.ScreenWidth - 1;
      }
      if ( pixelY < 0 )
      {
        pixelY = 0;
      }
      if ( pixelY >= m_GraphicScreenProject.ScreenHeight )
      {
        pixelY = m_GraphicScreenProject.ScreenHeight - 1;
      }

      if ( ( Buttons & MouseButtons.Left ) != 0 )
      {
        switch ( m_PaintTool )
        {
          case PaintTool.DRAW_PIXEL:
            if ( m_ButtonReleased )
            {
              m_ButtonReleased = false;
              DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoGraphicScreenImageChange( m_GraphicScreenProject, this, 0, 0, m_GraphicScreenProject.ScreenWidth, m_GraphicScreenProject.ScreenHeight ) );
            }
            if ( m_GraphicScreenProject.MultiColor )
            {
              m_GraphicScreenProject.Image.SetPixel( pixelX ^ 1, pixelY, m_CurrentColor );
            }
            m_GraphicScreenProject.Image.SetPixel( pixelX, pixelY, m_CurrentColor );
            Redraw();
            pictureEditor.Invalidate();
            Modified = true;
            break;
          case PaintTool.DRAW_RECTANGLE:
          case PaintTool.DRAW_BOX:
          case PaintTool.SELECT:
            if ( m_ButtonReleased )
            {
              m_ButtonReleased = false;
              if ( ( m_PaintTool == PaintTool.SELECT )
              &&   ( m_SelectionFloating ) )
              {
                // insert floating selection
                DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoGraphicScreenImageChange( m_GraphicScreenProject, this, pixelX, pixelY, m_SelectionFloatingImage.Width, m_SelectionFloatingImage.Height ) );

                m_SelectionFloatingImage.DrawTo( m_GraphicScreenProject.Image, pixelX, pixelY );
                Redraw();
                SetModified();
                pictureEditor.Invalidate();

                m_DragStartPoint.X = -1;
                m_SelectionFloating = false;
                m_SelectionFloatingImage.Dispose();
                m_SelectionFloatingImage = null;
                return;
              }
              m_DragStartPoint = new System.Drawing.Point( pixelX, pixelY );
            }
            m_DragCurrentPoint = new System.Drawing.Point( pixelX, pixelY );
            Redraw();
            pictureEditor.Invalidate();
            break;
          case PaintTool.VALIDATE:
            if ( ( charX < 0 )
            ||   ( charX >= BlockWidth )
            ||   ( charY < 0 )
            ||   ( charY >= BlockHeight ) )
            {
              return;
            }

            if ( ( m_SelectedChar.X != charX )
            ||   ( m_SelectedChar.Y != charY ) )
            {
              m_SelectedChar.X = charX;
              m_SelectedChar.Y = charY;

              charEditor.DisplayPage.DrawImage( m_GraphicScreenProject.Image, 0, 0, m_SelectedChar.X * 8, m_SelectedChar.Y * 8, 8, 8 );
              charEditor.Invalidate();

              if ( m_GraphicScreenProject.SelectedCheckType == C64Studio.Formats.GraphicScreenProject.CheckType.MULTICOLOR_BITMAP )
              {
                checkMulticolor.Checked = true;
              }
              else if ( m_GraphicScreenProject.SelectedCheckType == C64Studio.Formats.GraphicScreenProject.CheckType.MULTICOLOR_CHARSET )
              {
                checkMulticolor.Checked = true;
              }
              else
              {
                checkMulticolor.Checked = false;
              }
              comboCharColor.SelectedIndex = m_Chars[charX + charY * BlockWidth].Tile.CustomColor;

              Redraw();
            }
            if ( !string.IsNullOrEmpty( m_Chars[charX + charY * BlockWidth].Error ) )
            {
              labelCharInfo.Text = m_Chars[charX + charY * BlockWidth].Error;
              labelCharInfoExport.Text = m_Chars[charX + charY * BlockWidth].Error;
            }
            else
            {
              C64Studio.Formats.CharData usedChar = m_Chars[charX + charY * BlockWidth];
              if ( usedChar.Replacement != null )
              {
                while ( usedChar.Replacement != null )
                {
                  usedChar = usedChar.Replacement;
                }
                labelCharInfo.Text = "Duplicate of " + usedChar.Index;
              }
              else
              {
                labelCharInfo.Text = "Determined index " + usedChar.Index;
              }
            }
            comboCharColor.SelectedIndex = m_Chars[charX + charY * BlockWidth].Tile.CustomColor;
            break;
          case PaintTool.FLOOD_FILL:
            if ( m_ButtonReleased )
            {
              m_ButtonReleased = false;
              DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoGraphicScreenImageChange( m_GraphicScreenProject, this, 0, 0, m_GraphicScreenProject.ScreenWidth, m_GraphicScreenProject.ScreenHeight ) );
              FloodFill( pixelX, pixelY, m_CurrentColor );
              Redraw();
              pictureEditor.Invalidate();
              Modified = true;
            }
            break;
        }
      }
      else if ( !m_ButtonReleased )
      {
        switch ( m_PaintTool )
        {
          case PaintTool.DRAW_BOX:
          case PaintTool.DRAW_RECTANGLE:
          case PaintTool.SELECT:
            int     x1 = Math.Min( m_DragStartPoint.X, m_DragCurrentPoint.X );
            int     x2 = Math.Max( m_DragStartPoint.X, m_DragCurrentPoint.X );
            int     y1 = Math.Min( m_DragStartPoint.Y, m_DragCurrentPoint.Y );
            int     y2 = Math.Max( m_DragStartPoint.Y, m_DragCurrentPoint.Y );

            if ( ( m_GraphicScreenProject.MultiColor )
            &&   ( m_PaintTool != PaintTool.SELECT ) )
            {
              x1 &= ~1;
              x2 |= 1;
            }

            switch ( m_PaintTool )
            {
              case PaintTool.DRAW_BOX:
                DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoGraphicScreenImageChange( m_GraphicScreenProject, this, x1, y1, x2 - x1 + 1, y2 - y1 + 1 ) );
                m_GraphicScreenProject.Image.Box( x1, y1, x2 - x1 + 1, y2 - y1 + 1, m_CurrentColor );
                break;
              case PaintTool.DRAW_RECTANGLE:
                DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoGraphicScreenImageChange( m_GraphicScreenProject, this, x1, y1, x2 - x1 + 1, y2 - y1 + 1 ) );
                m_GraphicScreenProject.Image.Rectangle( x1, y1, x2 - x1 + 1, y2 - y1 + 1, m_CurrentColor );
                if ( m_GraphicScreenProject.MultiColor )
                {
                  m_GraphicScreenProject.Image.Line( x1 + 1, y1, x1 + 1, y2, m_CurrentColor );
                  m_GraphicScreenProject.Image.Line( x2 - 1, y1, x2 - 1, y2, m_CurrentColor );
                }
                break;
              case PaintTool.SELECT:
                // only make selection if we didn't place a floating selection before
                if ( m_DragStartPoint.X != -1 )
                {
                  m_Selection = new System.Drawing.Rectangle( x1, y1, x2 - x1, y2 - y1 );
                }
                break;
            }
            m_DragStartPoint.X = -1;
            Redraw();
            pictureEditor.Invalidate();
            Modified = true;
            break;
        }
        m_ButtonReleased = true;
      }
      else
      {
        switch ( m_PaintTool )
        {
          case PaintTool.SELECT:
            if ( m_SelectionFloating )
            {
              m_SelectionFloatingPos.X = pixelX;
              m_SelectionFloatingPos.Y = pixelY;
              Redraw();
            }
            break;
        }
      }

      if ( ( Buttons & MouseButtons.Right ) != 0 )
      {
      }
    }



    private void FloodFill( int X, int Y, byte CurrentColor )
    {
      Core.Imaging.FloodFill( m_GraphicScreenProject.Image, X, Y, CurrentColor );
    }



    private void radioBackground_CheckedChanged( object sender, EventArgs e )
    {
    }



    private void radioMultiColor1_CheckedChanged( object sender, EventArgs e )
    {
    }



    private void radioMulticolor2_CheckedChanged( object sender, EventArgs e )
    {
    }



    private void radioCharColor_CheckedChanged( object sender, EventArgs e )
    {
    }



    private void pictureEditor_MouseMove( object sender, MouseEventArgs e )
    {
      HandleMouseOnEditor( e.X, e.Y, e.Button );
    }



    private void checkMulticolor_CheckedChanged( object sender, EventArgs e )
    {
      if ( m_GraphicScreenProject.MultiColor != checkMulticolor.Checked )
      {
        m_GraphicScreenProject.MultiColor = checkMulticolor.Checked;
        Modified = true;
      }
    }



    private void comboBackground_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( m_GraphicScreenProject.Colors.BackgroundColor != comboBackground.SelectedIndex )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoGraphicScreenValuesChange( m_GraphicScreenProject, this ) );
        m_GraphicScreenProject.Colors.BackgroundColor = comboBackground.SelectedIndex;
        Modified = true;
        pictureEditor.Invalidate();
      }
    }



    private void comboMulticolor1_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( m_GraphicScreenProject.Colors.MultiColor1 != comboMulticolor1.SelectedIndex )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoGraphicScreenValuesChange( m_GraphicScreenProject, this ) );
        m_GraphicScreenProject.Colors.MultiColor1 = comboMulticolor1.SelectedIndex;
        Modified = true;
        pictureEditor.Invalidate();
      }
    }



    private void comboMulticolor2_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( m_GraphicScreenProject.Colors.MultiColor2 != comboMulticolor2.SelectedIndex )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoGraphicScreenValuesChange( m_GraphicScreenProject, this ) );
        m_GraphicScreenProject.Colors.MultiColor2 = comboMulticolor2.SelectedIndex;
        Modified = true;
        pictureEditor.Invalidate();
      }
    }



    private void comboCharColor_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( m_SelectedChar.X != -1 )
      {
        m_Chars[m_SelectedChar.X + m_SelectedChar.Y * BlockWidth].Tile.CustomColor = comboCharColor.SelectedIndex;
      }
    }



    private bool ImportImage( string Filename, GR.Image.FastImage IncomingImage, ImageInsertionMode InsertMode )
    {
      GR.Image.FastImage mappedImage = null;

      var mcSettings = new ColorSettings();
      mcSettings.BackgroundColor  = m_GraphicScreenProject.Colors.BackgroundColor;
      mcSettings.MultiColor1      = m_GraphicScreenProject.Colors.MultiColor1;
      mcSettings.MultiColor2      = m_GraphicScreenProject.Colors.MultiColor2;
      mcSettings.Palette          = m_GraphicScreenProject.Colors.Palette;

      bool pasteAsBlock = false;

      var importType = Types.GraphicType.BITMAP;
      if ( ( m_GraphicScreenProject.SelectedCheckType == Formats.GraphicScreenProject.CheckType.MEGA65_FCM_CHARSET )
      ||   ( m_GraphicScreenProject.SelectedCheckType == Formats.GraphicScreenProject.CheckType.MEGA65_FCM_CHARSET_16BIT ) )
      {
        importType = GraphicType.CHARACTERS_FCM;
      }
      if ( !Core.MainForm.ImportImage( Filename, IncomingImage, importType, mcSettings, out mappedImage, out mcSettings, out pasteAsBlock ) )
      {
        return false;
      }

      if ( mappedImage.PixelFormat != System.Drawing.Imaging.PixelFormat.Format8bppIndexed )
      {
        mappedImage.Dispose();
        System.Windows.Forms.MessageBox.Show( "Image format invalid!\nNeeds to be 8bit index" );
        return false;
      }

      Dialogs.DlgImportImageResize.ImportBehaviour    behaviour = C64Studio.Dialogs.DlgImportImageResize.ImportBehaviour.CLIP_IMAGE;

      if ( InsertMode == ImageInsertionMode.AS_FULL_SCREEN )
      {
        if ( ( mappedImage.Width != m_GraphicScreenProject.Image.Width )
        ||   ( mappedImage.Height != m_GraphicScreenProject.Image.Height ) )
        {
          Dialogs.DlgImportImageResize    dlg = new C64Studio.Dialogs.DlgImportImageResize( mappedImage.Width, mappedImage.Height, m_GraphicScreenProject.Image.Width, m_GraphicScreenProject.Image.Height, Core );

          dlg.ShowDialog();
          if ( dlg.ChosenResult == C64Studio.Dialogs.DlgImportImageResize.ImportBehaviour.CANCEL )
          {
            return false;
          }
          behaviour = dlg.ChosenResult;
        }
      }
      if ( behaviour == C64Studio.Dialogs.DlgImportImageResize.ImportBehaviour.ADJUST_SCREEN_SIZE )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoGraphicScreenSizeChange( m_GraphicScreenProject, this, m_GraphicScreenProject.ScreenWidth, m_GraphicScreenProject.ScreenHeight ) );

        SetScreenSize( mappedImage.Width, mappedImage.Height );

        DocumentInfo.UndoManager.AddGroupedUndoTask( new Undo.UndoGraphicScreenImageChange( m_GraphicScreenProject, this, 0, 0, m_GraphicScreenProject.ScreenWidth, m_GraphicScreenProject.ScreenHeight ) );
      }

      m_GraphicScreenProject.Colors.Palette = mcSettings.Palette;
      PaletteManager.ApplyPalette( m_GraphicScreenProject.Image, mcSettings.Palette );
      PaletteManager.ApplyPalette( pictureEditor.DisplayPage, mcSettings.Palette );

      if ( InsertMode == ImageInsertionMode.AT_SELECTED_LOCATION )
      {
        mappedImage.DrawTo( m_GraphicScreenProject.Image, m_SelectedChar.X * 8, m_SelectedChar.Y * 8 );

        charEditor.DisplayPage.DrawImage( m_GraphicScreenProject.Image, 0, 0, m_SelectedChar.X * 8, m_SelectedChar.Y * 8, 8, 8 );
        charEditor.Invalidate();
      }
      else if ( InsertMode == ImageInsertionMode.AS_FLOATING_SELECTION )
      {
        m_SelectionFloating       = true;
        m_SelectionFloatingImage = mappedImage.GetImage( 0, 0, mappedImage.Width, mappedImage.Height );
        Redraw();
        return true;
      }
      else
      {
        mappedImage.DrawTo( m_GraphicScreenProject.Image, 0, 0 );
      }
      mappedImage.Dispose();

      comboBackground.SelectedIndex   = NormalizeColor( mcSettings.BackgroundColor );
      comboMulticolor1.SelectedIndex  = NormalizeColor( mcSettings.MultiColor1 );
      comboMulticolor2.SelectedIndex  = NormalizeColor( mcSettings.MultiColor2 );

      btnCheck_Click( null, null );
      Redraw();
      Modified = true;
      return true;
    }



    private int NormalizeColor( int ColorIndex )
    {
      if ( ( ColorIndex < 0 )
      ||   ( ColorIndex >= 16 ) )
      {
        return 0;
      }
      return ColorIndex;
    }



    public void SetScreenSize( int Width, int Height )
    {
      m_GraphicScreenProject.ScreenWidth  = Width;
      m_GraphicScreenProject.ScreenHeight = Height;

      editScreenWidth.Text  = Width.ToString();
      editScreenHeight.Text = Height.ToString();

      m_GraphicScreenProject.Image = m_GraphicScreenProject.Image.GetImage( 0, 0, Width, Height ) as GR.Image.MemoryImage;

      m_ErrornousChars = new bool[( Width + 7 ) / 8, ( Height + 7 ) / 8];
      m_Chars.Clear();

      int     numBytes = Lookup.NumBytesOfSingleCharacterBitmap( Lookup.CharacterModeFromCheckType( m_GraphicScreenProject.SelectedCheckType ) );
      for ( int j = 0; j < BlockHeight; ++j )
      {
        for ( int i = 0; i < BlockWidth; ++i )
        {
          var charData = new Formats.CharData();

          charData.Tile.Data.Resize( (uint)numBytes );
          m_Chars.Add( charData );
        }
      }
      if ( ( m_SelectedChar.X >= BlockWidth )
      ||   ( m_SelectedChar.Y >= BlockHeight ) )
      {
        m_SelectedChar.X = -1;
        m_SelectedChar.Y = -1;
      }

      AdjustScrollbars();
    }



    private void importImagetoolStripMenuItem_Click( object sender, EventArgs e )
    {
      string filename;

      if ( OpenFile( "Open Image", Types.Constants.FILEFILTER_IMAGE_FILES + Types.Constants.FILEFILTER_ALL, out filename ) )
      {
        ImportImage( filename, null, ImageInsertionMode.AS_FULL_SCREEN );
      }
    }



    public void Clear()
    {
      DocumentInfo.DocumentFilename = "";
      // TODO - Clear
    }



    private void OpenProject( string Filename )
    {
      GR.Memory.ByteBuffer data = GR.IO.File.ReadAllBytes( Filename );
      if ( data == null )
      {
        return;
      }

      if ( !m_GraphicScreenProject.ReadFromBuffer( data ) )
      {
        return;
      }

      comboBackground.SelectedIndex   = m_GraphicScreenProject.Colors.BackgroundColor;
      checkMulticolor.Checked = m_GraphicScreenProject.MultiColor;
      comboMulticolor1.SelectedIndex = m_GraphicScreenProject.Colors.MultiColor1;
      comboMulticolor2.SelectedIndex = m_GraphicScreenProject.Colors.MultiColor2;
      comboCheckType.SelectedIndex = (int)m_GraphicScreenProject.SelectedCheckType;

      PaletteManager.ApplyPalette( pictureEditor.DisplayPage, m_GraphicScreenProject.Colors.Palette );
      PaletteManager.ApplyPalette( charEditor.DisplayPage, m_GraphicScreenProject.Colors.Palette );

      

      SetScreenSize( m_GraphicScreenProject.Image.Width, m_GraphicScreenProject.Image.Height );
      AdjustScrollbars();

      screenHScroll.Value = m_GraphicScreenProject.ScreenOffsetX;
      screenVScroll.Value = m_GraphicScreenProject.ScreenOffsetY;

      Redraw();
      EnableFileWatcher();
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
        System.Windows.Forms.MessageBox.Show( "Could not load graphic screen project file " + DocumentInfo.FullPath + ".\r\n" + ex.Message, "Could not load file" );
        return false;
      }
      SetUnmodified();
      return true;
    }



    public override GR.Memory.ByteBuffer SaveToBuffer()
    {
      return m_GraphicScreenProject.SaveToBuffer();
    }



    protected override bool QueryFilename( out string Filename )
    {
      Filename = "";

      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Save Graphic Screen Project as";
      saveDlg.Filter = "Graphic Screen Projects|*.graphicscreen|All Files|*.*";
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
        DialogResult doSave = MessageBox.Show( "There are unsaved changes in your graphic screen. Save now?", "Save changes?", MessageBoxButtons.YesNoCancel );
        if ( doSave == DialogResult.Cancel )
        {
          return;
        }
        if ( doSave == DialogResult.Yes )
        {
          Save( SaveMethod.SAVE );
        }
      }
      Clear();
      DocumentInfo.DocumentFilename = "";
      Modified = false;
      pictureEditor.Invalidate();

      closeCharsetProjectToolStripMenuItem.Enabled = false;
      saveCharsetProjectToolStripMenuItem.Enabled = false;
    }



    private void saveCharsetProjectToolStripMenuItem_Click( object sender, EventArgs e )
    {
      Save( SaveMethod.SAVE );
    }



    private string ToASMData( GR.Memory.ByteBuffer Data )
    {
      int wrapByteCount = GetExportWrapCount();
      if ( wrapByteCount <= 0 )
      {
        wrapByteCount = 8;
      }
      string prefix = editPrefix.Text;

      bool wrapData = checkExportToDataWrap.Checked;
      bool prefixRes = checkExportToDataIncludeRes.Checked;

      string line = "";
      if ( wrapData )
      {
        if ( prefixRes )
        {
          line += prefix;
        }
        int byteCount = 0;
        for ( int i = 0; i < Data.Length; ++i )
        {
          line += "$" + Data.ByteAt( i ).ToString( "x2" );
          ++byteCount;
          if ( ( byteCount < wrapByteCount )
          &&   ( i < Data.Length - 1 ) )
          {
            line += ",";
          }
          if ( byteCount == wrapByteCount )
          {
            byteCount = 0;
            line += System.Environment.NewLine;
            if ( ( i < Data.Length - 1 )
            &&   ( prefixRes ) )
            {
              line += prefix;
            }
          }
        }
      }
      else
      {
        if ( prefixRes )
        {
          line += prefix;
        }
        for ( int i = 0; i < Data.Length; ++i )
        {
          line += "$" + Data.ByteAt( i ).ToString( "x2" );
          if ( i < Data.Length - 1 )
          {
            line += ",";
          }
        }
      }
      return line;
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
      string    filename;
      if ( OpenFile( "Open file", Types.Constants.FILEFILTER_ALL, out filename ) )
      {
        GR.Memory.ByteBuffer imageData = GR.IO.File.ReadAllBytes( filename );

        if ( imageData.Length == 10003 )
        {
          // could be a Koala painter image
          if ( imageData.UInt16At( 0 ) == 0x6000 )
          {
            // background color
            comboBackground.SelectedIndex = imageData.ByteAt( 10002 ) % 16;

            for ( int i = 0; i < BlockWidth * BlockHeight; ++i )
            {
              byte screenByte = imageData.ByteAt( 2 + 8000 + i );
              byte colorByte = imageData.ByteAt( 2 + 8000 + 1000 + i );
              for ( int j = 0; j < 8; ++j )
              {
                byte pixelData = imageData.ByteAt( 2 + i * 8 + j );

                byte pixelMask = 0xc0;
                for ( int k = 0; k < 4; ++k )
                {
                  byte byteValue = (byte)( pixelData & pixelMask );

                  byteValue >>= 6 - 2 * k;

                  int     colorIndex = m_GraphicScreenProject.Colors.BackgroundColor;

                  switch ( byteValue )
                  {
                    case 0:
                      // background
                      break;
                    case 0x01:
                      colorIndex = ( screenByte >> 4 );                      
                      break;
                    case 0x02:
                      colorIndex = screenByte & 0x0f;
                      break;
                    case 0x03:
                      colorIndex = colorByte % 16;
                      break;
                  }
                  m_GraphicScreenProject.Image.SetPixel( ( i % BlockWidth ) * 8 + k * 2, ( i / BlockWidth ) * 8 + j, (uint)colorIndex );
                  m_GraphicScreenProject.Image.SetPixel( ( i % BlockWidth ) * 8 + k * 2 + 1, ( i / BlockWidth ) * 8 + j, (uint)colorIndex );
                  pixelMask >>= 2;
                }
              }
            }
            Redraw();
            SetModified();
          }
        }

        /*
        0000 - 1F3F : Bitmap 8000 Bytes
        1F40 - 2327 : Bildschirmspeicher 1000 Bytes
        2328 - 270F : Farb-RAM 1000 Bytes
        2710        : Hintergrundfarbe 1 Byte
         */

        pictureEditor.Invalidate();
      }

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

      ImportImage( null, imgClip, ImageInsertionMode.AS_FULL_SCREEN );
      /*

      GR.Image.FastImage mappedImage = null;

      Types.MulticolorSettings   mcSettings = new Types.MulticolorSettings();
      mcSettings.BackgroundColor  = m_GraphicScreenProject.BackgroundColor;
      mcSettings.MultiColor1      = m_GraphicScreenProject.MultiColor1;
      mcSettings.MultiColor2      = m_GraphicScreenProject.MultiColor2;

      bool pasteAsBlock = false;
      if ( !Core.MainForm.ImportImage( "", imgClip, Types.GraphicType.CHARACTERS, mcSettings, out mappedImage, out mcSettings, out pasteAsBlock ) )
      {
        imgClip.Dispose();
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

      mappedImage.DrawTo( m_GraphicScreenProject.Image, 0, 0 );
      Redraw();
      pictureEditor.Invalidate();
      Modified = true;*/
    }



    private void btnImportCharsetFromFile_Click( object sender, EventArgs e )
    {
      string filename;

      if ( !OpenFile( "Import from Image", Types.Constants.FILEFILTER_IMAGE_FILES + Types.Constants.FILEFILTER_ALL, out filename ) )
      {
        return;
      }
      ImportImage( filename, null, ImageInsertionMode.AS_FULL_SCREEN );
    }



    private void CopyImageToClipboard()
    {
      Core.Imaging.ImageToClipboard( m_GraphicScreenProject.Image );
    }



    private void CopySelectedImageToClipboard()
    {
      if ( ( m_PaintTool == PaintTool.SELECT )
      &&   ( m_Selection.X != -1 ) )
      {
        Core.Imaging.ImageToClipboard( m_GraphicScreenProject.Image, m_Selection );
        return;
      }
      if ( m_SelectedChar.X == -1 )
      {
        CopyImageToClipboard();
        return;
      }

      Core.Imaging.ImageToClipboard( m_GraphicScreenProject.Image, m_SelectedChar.X * 8, m_SelectedChar.Y * 8, 8, 8 );
    }



    private void PasteClipboardImageToSelectedChar()
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

      ImageInsertionMode    mode = ImageInsertionMode.AT_SELECTED_LOCATION;

      if ( m_PaintTool == PaintTool.SELECT )
      {
        if ( m_SelectionFloating )
        {
          m_SelectionFloatingImage.Dispose();
          m_SelectionFloatingImage = null;
        }
        m_SelectionFloating = true;
        mode = ImageInsertionMode.AS_FLOATING_SELECTION;
      }
      ImportImage( null, imgClip, mode );
    }



    private void btnCopy_Click( object sender, EventArgs e )
    {
      CopySelectedImageToClipboard();
    }



    private void btnPaste_Click( object sender, EventArgs e )
    {
      PasteClipboardImageToSelectedChar();
    }



    private void Redraw()
    {
      m_GraphicScreenProject.Image.DrawTo( pictureEditor.DisplayPage, -m_GraphicScreenProject.ScreenOffsetX * 8, -m_GraphicScreenProject.ScreenOffsetY * 8 );

      switch ( m_PaintTool )
      {
        case PaintTool.DRAW_BOX:
        case PaintTool.DRAW_RECTANGLE:
          if ( m_DragStartPoint.X != -1 )
          {
            int     x1 = Math.Min( m_DragStartPoint.X, m_DragCurrentPoint.X );
            int     x2 = Math.Max( m_DragStartPoint.X, m_DragCurrentPoint.X );
            int     y1 = Math.Min( m_DragStartPoint.Y, m_DragCurrentPoint.Y );
            int     y2 = Math.Max( m_DragStartPoint.Y, m_DragCurrentPoint.Y );

            if ( m_GraphicScreenProject.MultiColor )
            {
              x1 &= ~1;
              x2 |= 1;
            }

            pictureEditor.DisplayPage.Rectangle( x1, y1, x2 - x1 + 1, y2 - y1 + 1, m_CurrentColor );
            if ( m_GraphicScreenProject.MultiColor )
            {
              pictureEditor.DisplayPage.Line( x1 + 1, y1, x1 + 1, y2, m_CurrentColor );
              pictureEditor.DisplayPage.Line( x2 - 1, y1, x2 - 1, y2, m_CurrentColor );
            }
          }
          break;
        case PaintTool.SELECT:
          if ( m_SelectionFloating )
          {
            pictureEditor.DisplayPage.DrawImage( m_SelectionFloatingImage, m_SelectionFloatingPos.X, m_SelectionFloatingPos.Y );
          }
          break;
      }
      pictureEditor.Invalidate();
    }



    private void pictureEditor_Paint( object sender, PaintEventArgs e )
    {
      for ( int j = 0; j < BlockHeight; ++j )
      {
        for ( int i = 0; i < BlockWidth; ++i )
        {
          if ( m_ErrornousChars[i, j] )
          {
            e.Graphics.DrawRectangle( System.Drawing.SystemPens.ControlLight, i * 16, j * 16, 16, 16 );
          }
        }
      }
    }



    private bool CheckCharBox( Formats.CharData cd, int X, int Y, bool CheckForMC )
    {
      // Match image data
      int chosenCharColor = -1;

      cd.Replacement = null;
      cd.Index = 0;

      // clear data
      for ( int i = 0; i < cd.Tile.Data.Length; ++i )
      {
        cd.Tile.Data.SetU8At( i, 0 );
      }

      bool  isMultiColor = false;

      {
        // determine single/multi color
        bool[] usedColor = new bool[16];
        int numColors = 0;
        bool hasSinglePixel = false;
        bool usedBackgroundColor = false;

        for ( int y = 0; y < 8; ++y )
        {
          for ( int x = 0; x < 8; ++x )
          {
            int colorIndex = (int)m_GraphicScreenProject.Image.GetPixel( X + x, Y + y ) % 16;
            if ( colorIndex >= 16 )
            {
              cd.Error = "Color index >= 16";
              return false;
            }
            if ( ( x % 2 ) == 0 )
            {
              if ( colorIndex != (int)m_GraphicScreenProject.Image.GetPixel( X + x + 1, Y + y ) % 16 )
              {
                // not a double pixel, must be single color then
                hasSinglePixel = true;
              }
            }

            if ( !usedColor[colorIndex] )
            {
              if ( colorIndex == m_GraphicScreenProject.Colors.BackgroundColor )
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
          cd.Error = "Has single pixel, but more than 2 colors";
          return false;
        }
        if ( numColors > 2 )
        {
          isMultiColor = true;
        }
        if ( ( !CheckForMC )
        &&   ( numColors > 2 ) )
        {
          cd.Error = "Has too many colors";
          return false;
        }
        if ( ( !CheckForMC )
        &&   ( numColors == 2 )
        &&   ( !usedBackgroundColor ) )
        {
          cd.Error = "Uses two colors different from background color";
          return false;
        }
        if ( ( hasSinglePixel )
        &&   ( numColors == 2 )
        &&   ( !usedBackgroundColor ) )
        {
          cd.Error = "Has single pixel, but more than 2 colors different from background color";
          return false;
        }
        if ( ( CheckForMC )
        &&   ( !hasSinglePixel )
        &&   ( numColors > 4 ) )
        {
          cd.Error = "Has more than 4 colors";
          return false;
        }
        if ( ( !hasSinglePixel )
        &&   ( numColors == 4 )
        &&   ( !usedBackgroundColor ) )
        {
          cd.Error = "Has more than 4 colors different from background color";
          return false;
        }
        int otherColorIndex = 16;
        if ( ( !hasSinglePixel )
        &&   ( numColors == 2 )
        &&   ( usedBackgroundColor ) )
        {
          for ( int i = 0; i < 16; ++i )
          {
            if ( ( usedColor[i] )
            && ( i != m_GraphicScreenProject.Colors.BackgroundColor ) )
            {
              otherColorIndex = i;
              break;
            }
          }
        }
        if ( ( hasSinglePixel )
        ||   ( !CheckForMC )
        ||   ( ( numColors == 2 )
        &&     ( usedBackgroundColor )
        &&     ( otherColorIndex < 8 ) ) )
        {
          // eligible for single color
          isMultiColor = false;
          int usedFreeColor = -1;
          for ( int i = 0; i < 16; ++i )
          {
            if ( usedColor[i] )
            {
              if ( i != m_GraphicScreenProject.Colors.BackgroundColor )
              {
                if ( usedFreeColor != -1 )
                {
                  cd.Error = "More than 1 free color";
                  return false;
                }
                usedFreeColor = i;
              }
            }
          }

          if ( ( hasSinglePixel )
          &&   ( CheckForMC )
          &&   ( numColors == 2 )
          &&   ( usedFreeColor >= 8 ) )
          {
            cd.Error = "Hires char cannot use free color with index " + usedFreeColor;
            return false;
          }

          for ( int y = 0; y < 8; ++y )
          {
            for ( int x = 0; x < 8; ++x )
            {
              int ColorIndex = (int)m_GraphicScreenProject.Image.GetPixel( X + x, Y + y ) % 16;

              int BitPattern = 0;

              if ( ColorIndex != m_GraphicScreenProject.Colors.BackgroundColor )
              {
                BitPattern = 1;
              }

              // noch nicht verwendete Farbe
              if ( BitPattern == 1 )
              {
                chosenCharColor = ColorIndex;
              }
              cd.Tile.Data.SetU8At( y + x / 8, (byte)( cd.Tile.Data.ByteAt( y + x / 8 ) | ( BitPattern << ( ( 7 - ( x % 8 ) ) ) ) ) );
            }
          }
        }
        else
        {
          // multi color
          isMultiColor = true;
          int usedMultiColors = 0;
          int usedFreeColor = -1;
          for ( int i = 0; i < 16; ++i )
          {
            if ( usedColor[i] )
            {
              if ( ( i == m_GraphicScreenProject.Colors.MultiColor1 )
              ||   ( i == m_GraphicScreenProject.Colors.MultiColor2 )
              ||   ( i == m_GraphicScreenProject.Colors.BackgroundColor ) )
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
            cd.Error = "More than 1 free color";
            return false;
          }
          if ( usedFreeColor >= 8 )
          {
            cd.Error = "Free color must be of index < 8";
            return false;
          }
          for ( int y = 0; y < 8; ++y )
          {
            for ( int x = 0; x < 4; ++x )
            {
              int ColorIndex = (int)m_GraphicScreenProject.Image.GetPixel( X + 2 * x, Y + y ) % 16;

              byte BitPattern = 0;

              if ( ColorIndex == m_GraphicScreenProject.Colors.BackgroundColor )
              {
                BitPattern = 0x00;
              }
              else if ( ColorIndex == m_GraphicScreenProject.Colors.MultiColor1 )
              {
                BitPattern = 0x01;
              }
              else if ( ColorIndex == m_GraphicScreenProject.Colors.MultiColor2 )
              {
                BitPattern = 0x02;
              }
              else
              {
                // noch nicht verwendete Farbe
                chosenCharColor = usedFreeColor;
                BitPattern = 0x03;
              }
              cd.Tile.Data.SetU8At( y + x / 4, (byte)( cd.Tile.Data.ByteAt( y + x / 4 ) | ( BitPattern << ( ( 3 - ( x % 4 ) ) * 2 ) ) ) );
            }
          }
          if ( usedFreeColor == -1 )
          {
            // only the two multi colors were used, we need to force multi color index though
            chosenCharColor = 8;
          }
        }
      }
      if ( chosenCharColor == -1 )
      {
        chosenCharColor = 0;
      }
      cd.Tile.CustomColor = chosenCharColor;
      if ( ( isMultiColor )
      &&   ( chosenCharColor < 8 ) )
      {
        cd.Tile.CustomColor = chosenCharColor + 8;
      }
      return true;
    }



    private bool CheckForMCCharsetErrors()
    {
      btnExportAs.Enabled = false;

      bool      foundError = false;
      for ( int j = 0; j < BlockHeight; ++j )
      {
        for ( int i = 0; i < BlockWidth; ++i )
        {
          CheckCharBox( m_Chars[i + j * BlockWidth], i * 8, j * 8, true );
          if ( m_Chars[i + j * BlockWidth].Error.Length > 0 )
          {
            m_ErrornousChars[i, j] = true;
            foundError = true;
          }
          else
          {
            m_ErrornousChars[i, j] = false;
          }
        }
      }
      if ( foundError )
      {
        return true;
      }

      // check for duplicates
      int items = m_Chars.Count;
      int foldedItems = 0;
      int curIndex = 0;

      for ( int index1 = 0; index1 < m_Chars.Count; ++index1 )
      {
        bool wasFolded = false;
        for ( int index2 = 0; index2 < index1; ++index2 )
        {
          if ( m_Chars[index1].Tile.Data.Compare( m_Chars[index2].Tile.Data ) == 0 )
          {
            // same data
            if ( m_Chars[index2].Replacement != null )
            {
              m_Chars[index1].Replacement = m_Chars[index2].Replacement;
            }
            else
            {
              m_Chars[index1].Replacement = m_Chars[index2];
            }
            ++foldedItems;
            wasFolded = true;
            break;
          }
        }
        if ( !wasFolded )
        {
          // item was not folded
          m_Chars[index1].Index = curIndex;
          ++curIndex;
        }
      }
      labelCharInfo.Text = "";
      labelCharInfoExport.Text = "";
      if ( items - foldedItems > 256 )
      {
        labelCharInfo.Text = "Too many unique characters (" + ( items - foldedItems ) + ")";
        labelCharInfoExport.Text = labelCharInfo.Text;
        return true;
      }
      labelCharInfo.Text = ( items - foldedItems ).ToString() + " unique chars, duplicates removed " + foldedItems;
      labelCharInfoExport.Text = labelCharInfo.Text;

      btnExportAs.Enabled = true;
      return false;
    }



    private bool CheckForHiResCharsetErrors()
    {
      btnExportAs.Enabled = false;

      bool      foundError = false;
      for ( int j = 0; j < BlockHeight; ++j )
      {
        for ( int i = 0; i < BlockWidth; ++i )
        {
          CheckCharBox( m_Chars[i + j * BlockWidth], i * 8, j * 8, false );
          if ( m_Chars[i + j * BlockWidth].Error.Length > 0 )
          {
            m_ErrornousChars[i, j] = true;
            foundError = true;
          }
          else
          {
            m_ErrornousChars[i, j] = false;
          }
        }
      }
      if ( foundError )
      {
        return true;
      }

      // check for duplicates
      int items = m_Chars.Count;
      int foldedItems = 0;
      int curIndex = 0;

      for ( int index1 = 0; index1 < m_Chars.Count; ++index1 )
      {
        bool wasFolded = false;
        for ( int index2 = 0; index2 < index1; ++index2 )
        {
          if ( m_Chars[index1].Tile.Data.Compare( m_Chars[index2].Tile.Data ) == 0 )
          {
            // same data
            if ( m_Chars[index2].Replacement != null )
            {
              m_Chars[index1].Replacement = m_Chars[index2].Replacement;
            }
            else
            {
              m_Chars[index1].Replacement = m_Chars[index2];
            }
            ++foldedItems;
            wasFolded = true;
            break;
          }
        }
        if ( !wasFolded )
        {
          // item was not folded
          m_Chars[index1].Index = curIndex;
          ++curIndex;
        }
      }
      labelCharInfo.Text = "";
      labelCharInfoExport.Text = "";
      if ( items - foldedItems > 256 )
      {
        labelCharInfo.Text = "Too many unique characters (" + ( items - foldedItems ) + ")";
        labelCharInfoExport.Text = labelCharInfo.Text;
        return true;
      }
      labelCharInfo.Text = ( items - foldedItems ).ToString() + " unique chars, duplicates removed " + foldedItems;
      labelCharInfoExport.Text = labelCharInfo.Text;

      btnExportAs.Enabled = true;
      return false;
    }



    private bool CheckForFCMCharsetErrors( int AllowedNumberOfChars )
    {
      btnExportAs.Enabled = false;

      bool      foundError = false;
      for ( int j = 0; j < BlockHeight; ++j )
      {
        for ( int i = 0; i < BlockWidth; ++i )
        {
          // nothing can go wrong, 256 colors for free use
          m_ErrornousChars[i, j] = false;
        }
      }
      if ( foundError )
      {
        return true;
      }

      // check for duplicates
      int items = m_Chars.Count;
      int foldedItems = 0;
      int curIndex = 0;

      for ( int index1 = 0; index1 < m_Chars.Count; ++index1 )
      {
        var  tile1 = ( (GR.Image.MemoryImage)m_GraphicScreenProject.Image.GetImage( ( index1 % ( ( m_GraphicScreenProject.ScreenWidth + 7 ) / 8 ) ) * 8,
          ( index1 / ( ( m_GraphicScreenProject.ScreenWidth + 7 ) / 8 ) ) * 8,
          8, 8 ) ).GetAsData();
        bool wasFolded = false;
        for ( int index2 = 0; index2 < index1; ++index2 )
        {
          var  tile2 = ( (GR.Image.MemoryImage)m_GraphicScreenProject.Image.GetImage( ( index2 % ( ( m_GraphicScreenProject.ScreenWidth + 7 ) / 8 ) ) * 8,
                                    ( index2 / ( ( m_GraphicScreenProject.ScreenWidth + 7 ) / 8 ) ) * 8,
                                    8, 8 ) ).GetAsData();

          if ( tile1.Compare( tile2 ) == 0 )
          {
            // same data
            if ( m_Chars[index2].Replacement != null )
            {
              m_Chars[index1].Replacement = m_Chars[index2].Replacement;
            }
            else
            {
              m_Chars[index1].Replacement = m_Chars[index2];
            }
            ++foldedItems;
            wasFolded = true;
            break;
          }
        }
        if ( !wasFolded )
        {
          // item was not folded
          m_Chars[index1].Index = curIndex;
          m_Chars[index1].Tile.Data = tile1;
          ++curIndex;
        }
      }
      labelCharInfo.Text = "";
      labelCharInfoExport.Text = "";
      if ( items - foldedItems > AllowedNumberOfChars )
      {
        labelCharInfo.Text = "Too many unique characters (" + ( items - foldedItems ) + ")";
        labelCharInfoExport.Text = labelCharInfo.Text;
        return true;
      }
      labelCharInfo.Text = ( items - foldedItems ).ToString() + " unique chars, duplicates removed " + foldedItems;
      labelCharInfoExport.Text = labelCharInfo.Text;

      btnExportAs.Enabled = true;
      return false;
    }



    private void CheckForHiResBitmapErrors()
    {
      for ( int y = 0; y < BlockHeight; ++y )
      {
        for ( int x = 0; x < BlockWidth; ++x )
        {
          // ein zeichen-block
          m_ErrornousChars[x, y] = false;

          // determine single/multi color
          bool[] usedColor = new bool[16];
          int numColors = 0;
          bool usedBackgroundColor = false;
          int determinedBackgroundColor = -1;// comboBackground.SelectedIndex;

          for ( int charY = 0; charY < 8; ++charY )
          {
            for ( int charX = 0; charX < 8; ++charX )
            {
              byte  colorIndex = (byte)m_GraphicScreenProject.Image.GetPixel( x * 8 + charX, y * 8 + charY );

              if ( colorIndex >= 16 )
              {
                m_ErrornousChars[x, y] = true;
                m_Chars[x + y * BlockWidth].Error = "Encountered color index >= 16 at " + ( x * 8 + charX ) + "," + ( y * 8 + charY );
              }
              else
              {
                if ( !usedColor[colorIndex] )
                {
                  if ( ( determinedBackgroundColor != -1 )
                  &&   ( colorIndex == determinedBackgroundColor ) )
                  {
                    usedBackgroundColor = true;
                  }
                  usedColor[colorIndex] = true;
                  numColors++;
                }
              }
            }
          }
          if ( numColors > 2 )
          {
            m_ErrornousChars[x, y] = true;
            m_Chars[x + y * BlockWidth].Error = "Uses more than two colors at " + ( x * 8 ) + "," + ( y * 8 );
            continue;
          }
          /*
          if ( determinedBackgroundColor == -1 )
          {
            // set background color as one of the 2 found, prefer 0, than the lower one
            if ( usedColor[0] )
            {
              determinedBackgroundColor = 0;
            }
            else
            {
              for ( int i = 0; i < 16; ++i )
              {
                if ( usedColor[i] )
                {
                  determinedBackgroundColor = i;
                  break;
                }
              }
            }
          }*/
          if ( ( determinedBackgroundColor != -1 )
          &&   ( !usedBackgroundColor )
          &&   ( numColors >= 2 ) )
          {
            m_ErrornousChars[x, y] = true;
            m_Chars[x + y * BlockWidth].Error = "Looks like single color, but doesn't use the set background color " + ( x * 8 ) + "," + ( y * 8 );
            continue;
          }
          int usedFreeColor = -1;
          for ( int i = 0; i < 16; ++i )
          {
            if ( usedColor[i] )
            {
              if ( ( i != determinedBackgroundColor )
              &&   ( determinedBackgroundColor != -1 ) )
              {
                if ( usedFreeColor != -1 )
                {
                  m_ErrornousChars[x, y] = true;
                  m_Chars[x + y * BlockWidth].Error = "Uses more than one free color " + ( x * 8 ) + "," + ( y * 8 );
                  continue;
                }
                usedFreeColor = i;
              }
            }
          }
        }
      }
    }



    private void CheckForMCBitmapErrors()
    {
      GR.Collections.Map<byte,byte>     usedColors = new GR.Collections.Map<byte,byte>();

      GR.Memory.ByteBuffer              screenChar = new GR.Memory.ByteBuffer( 1000 );
      GR.Memory.ByteBuffer              screenColor = new GR.Memory.ByteBuffer( 1000 );
      GR.Memory.ByteBuffer              bitmapData = new GR.Memory.ByteBuffer( 8192 );

      for ( int y = 0; y < BlockHeight; ++y )
      {
        for ( int x = 0; x < BlockWidth; ++x )
        {
          // ein zeichen-block
          usedColors.Clear();
          m_ErrornousChars[x, y] = false;
          for ( int charY = 0; charY < 8; ++charY )
          {
            for ( int charX = 0; charX < 4; ++charX )
            {
              byte  colorIndex = (byte)m_GraphicScreenProject.Image.GetPixel( x * 8 + charX * 2, y * 8 + charY );
              if ( colorIndex >= 16 )
              {
                m_Chars[x + y * BlockWidth].Error = "Color index >= 16 at " + ( x * 8 + charX * 2 ).ToString() + ", " + ( y * 8 + charY ).ToString() + " (" + charX + "," + charY + ")";
                m_ErrornousChars[x, y] = true;
              }
              byte  colorIndex2 = (byte)m_GraphicScreenProject.Image.GetPixel( x * 8 + charX * 2 + 1, y * 8 + charY );
              if ( colorIndex != colorIndex2 )
              {
                m_Chars[x + y * BlockWidth].Error = "Used HiRes pixel >= 16 at " + ( x * 8 + charX * 2 ).ToString() + ", " + ( y * 8 + charY ).ToString() + " (" + charX + "," + charY + ")";
                m_ErrornousChars[x, y] = true;
              }

              if ( colorIndex != m_GraphicScreenProject.Colors.BackgroundColor )
              {
                // remember used color
                usedColors.Add( colorIndex, 0 );
              }
            }
          }
          // more than 3 colors?
          if ( usedColors.Count > 3 )
          {
            m_Chars[x + y * BlockWidth].Error = "Too many colors used";
            m_ErrornousChars[x, y] = true;
          }
          else
          {
            if ( usedColors.Count > 0 )
            {
              int   colorTarget = 0;
              List<byte> keys = new List<byte>( usedColors.Keys );
              foreach ( byte colorIndex in keys )
              {
                if ( colorTarget == 0 )
                {
                  // upper screen char nibble
                  byte value = screenChar.ByteAt( x + y * BlockWidth );
                  value &= 0x0f;
                  value |= (byte)( colorIndex << 4 );

                  screenChar.SetU8At( x + y * BlockWidth, value );
                  usedColors[colorIndex] = 1;
                }
                else if ( colorTarget == 1 )
                {
                  // lower nibble in screen char
                  byte value = screenChar.ByteAt( x + y * BlockWidth );
                  value &= 0xf0;
                  value |= (byte)( colorIndex );

                  screenChar.SetU8At( x + y * BlockWidth, value );
                  usedColors[colorIndex] = 2;
                }
                else if ( colorTarget == 2 )
                {
                  // color ram
                  screenColor.SetU8At( x + y * BlockWidth, colorIndex );
                  usedColors[colorIndex] = 3;
                }
                ++colorTarget;
              }
            }
            // write out bits
            for ( int charY = 0; charY < 8; ++charY )
            {
              for ( int charX = 0; charX < 4; ++charX )
              {
                byte colorIndex = (byte)m_GraphicScreenProject.Image.GetPixel( x * 8 + charX * 2, y * 8 + charY );
                if ( colorIndex != m_GraphicScreenProject.Colors.BackgroundColor )
                {
                  // other color
                  byte colorValue = usedColors[colorIndex];
                  int bitmapIndex = x * 8 + y * 320 + charY;

                  byte value = bitmapData.ByteAt( bitmapIndex );
                  if ( charX == 0 )
                  {
                    value &= 0x3f;
                    value |= (byte)( colorValue << 6 );
                  }
                  else if ( charX == 1 )
                  {
                    value &= 0xcf;
                    value |= (byte)( colorValue << 4 );
                  }
                  else if ( charX == 2 )
                  {
                    value &= 0xf3;
                    value |= (byte)( colorValue << 2 );
                  }
                  else
                  {
                    value &= 0xfc;
                    value |= colorValue;
                  }
                  bitmapData.SetU8At( bitmapIndex, value );
                }
              }
            }
          }
        }
      }
    }



    private void btnCheck_Click( object sender, EventArgs e )
    {
      foreach ( Formats.CharData aChar in m_Chars )
      {
        aChar.Error = "";
        aChar.Tile.CustomColor = 0;
        aChar.Replacement = null;
      }

      switch ( (Formats.GraphicScreenProject.CheckType)comboCheckType.SelectedIndex )
      {
        case C64Studio.Formats.GraphicScreenProject.CheckType.HIRES_BITMAP:
          CheckForHiResBitmapErrors();
          break;
        case C64Studio.Formats.GraphicScreenProject.CheckType.MULTICOLOR_BITMAP:
          CheckForMCBitmapErrors();
          break;
        case C64Studio.Formats.GraphicScreenProject.CheckType.HIRES_CHARSET:
          CheckForHiResCharsetErrors();
          break;
        case C64Studio.Formats.GraphicScreenProject.CheckType.MULTICOLOR_CHARSET:
          CheckForMCCharsetErrors();
          break;
        case C64Studio.Formats.GraphicScreenProject.CheckType.MEGA65_FCM_CHARSET:
          CheckForFCMCharsetErrors( 256 );
          break;
        case C64Studio.Formats.GraphicScreenProject.CheckType.MEGA65_FCM_CHARSET_16BIT:
          CheckForFCMCharsetErrors( 8192 );
          break;
        default:
          Debug.Log( "Unsupported CheckType: " + (Formats.GraphicScreenProject.CheckType)comboCheckType.SelectedIndex );
          break;
      }
      Redraw();
    }



    private void charEditor_MouseDown( object sender, MouseEventArgs e )
    {
      charEditor.Focus();
      HandleMouseOnCharEditor( e.X, e.Y, e.Button );
    }



    private void charEditor_MouseMove( object sender, MouseEventArgs e )
    {
      if ( charEditor.ClientRectangle.Contains( e.X, e.Y ) )
      {
        HandleMouseOnCharEditor( e.X, e.Y, e.Button );
      }
    }



    private void HandleMouseOnCharEditor( int X, int Y, MouseButtons Buttons )
    {
      if ( m_SelectedChar.X == -1 )
      {
        return;
      }
      Formats.CharData    charToEdit = m_Chars[m_SelectedChar.X + m_SelectedChar.Y * BlockWidth];

      int     charX = X / ( charEditor.ClientRectangle.Width / 8 );
      int     charY = Y / ( charEditor.ClientRectangle.Height / 8 );

      if ( ( Buttons & MouseButtons.Left ) != 0 )
      {
        if ( m_ButtonReleased )
        {
          m_ButtonReleased = false;
          DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoGraphicScreenImageChange( m_GraphicScreenProject, this, m_SelectedChar.X * 8, m_SelectedChar.Y * 8, 8, 8 ) );
        }
        if ( ( m_GraphicScreenProject.SelectedCheckType == Formats.GraphicScreenProject.CheckType.MULTICOLOR_BITMAP )
        ||   ( m_GraphicScreenProject.SelectedCheckType == Formats.GraphicScreenProject.CheckType.MULTICOLOR_CHARSET ) )
        {
          byte    colorToSet = m_CurrentColor;

          charX /= 2;
          m_GraphicScreenProject.Image.SetPixel( m_SelectedChar.X * 8 + charX * 2, m_SelectedChar.Y * 8 + charY, colorToSet );
          m_GraphicScreenProject.Image.SetPixel( m_SelectedChar.X * 8 + charX * 2 + 1, m_SelectedChar.Y * 8 + charY, colorToSet );

          charEditor.DisplayPage.SetPixel( charX * 2, charY, colorToSet );
          charEditor.DisplayPage.SetPixel( charX * 2 + 1, charY, colorToSet );
          charEditor.Invalidate();
          Modified = true;
          Redraw();
        }
        else
        {
          byte    colorToSet = m_CurrentColor;

          m_GraphicScreenProject.Image.SetPixel( m_SelectedChar.X * 8 + charX, m_SelectedChar.Y * 8 + charY, colorToSet );

          charEditor.DisplayPage.SetPixel( charX, charY, colorToSet );
          charEditor.Invalidate();
          Modified = true;
          Redraw();
        }
      }
      else
      {
        m_ButtonReleased = true;
      }
      if ( ( Buttons & MouseButtons.Right ) != 0 )
      {
        if ( ( m_GraphicScreenProject.SelectedCheckType == Formats.GraphicScreenProject.CheckType.MULTICOLOR_BITMAP )
        ||   ( m_GraphicScreenProject.SelectedCheckType == Formats.GraphicScreenProject.CheckType.MULTICOLOR_CHARSET ) )
        {
          byte    colorToSet = (byte)m_GraphicScreenProject.Colors.BackgroundColor;
          charX /= 2;
          m_GraphicScreenProject.Image.SetPixel( m_SelectedChar.X * 8 + charX * 2, m_SelectedChar.Y * 8 + charY, colorToSet );
          m_GraphicScreenProject.Image.SetPixel( m_SelectedChar.X * 8 + charX * 2 + 1, m_SelectedChar.Y * 8 + charY, colorToSet );

          charEditor.DisplayPage.SetPixel( charX * 2, charY, colorToSet );
          charEditor.DisplayPage.SetPixel( charX * 2 + 1, charY, colorToSet );
          charEditor.Invalidate();
          Modified = true;
          Redraw();
        }
        else
        {
          byte  colorToSet = (byte)m_GraphicScreenProject.Colors.BackgroundColor;

          m_GraphicScreenProject.Image.SetPixel( m_SelectedChar.X * 8 + charX, m_SelectedChar.Y * 8 + charY, colorToSet );

          charEditor.DisplayPage.SetPixel( charX, charY, colorToSet );
          charEditor.Invalidate();
          Modified = true;
          Redraw();
        }
      }

    }



    private void btnExportAsCharset_Click( object sender, EventArgs e )
    {
      switch ( (ExportDataType)comboExportType.SelectedIndex )
      {
        case ExportDataType.HIRES_BITMAP:
          ExportHiresBitmap();
          break;
        case ExportDataType.MULTICOLOR_BITMAP:
          ExportMCBitmap();
          break;
        case ExportDataType.HIRES_CHARSET:
          ExportHiResCharset( false );
          break;
        case ExportDataType.HIRES_CHARSET_SCREEN_ASSEMBLY:
          ExportHiResCharset( true );
          break;
        case ExportDataType.MULTICOLOR_CHARSET:
          ExportMCCharset( false );
          break;
        case ExportDataType.MULTICOLOR_CHARSET_SCREEN_ASSEMBLY:
          ExportMCCharset( true );
          break;
        case ExportDataType.CHARACTERS_TO_CLIPBOARD:
          UsedCharsToClipboard();
          break;
      }
    }



    private void ExportHiresBitmap()
    {
      GR.Memory.ByteBuffer screenChar;
      GR.Memory.ByteBuffer screenColor;
      GR.Memory.ByteBuffer bitmapData;

      m_GraphicScreenProject.ImageToHiresBitmapData( m_GraphicScreenProject.ColorMapping, m_Chars, m_ErrornousChars, 0, 0, BlockWidth, BlockHeight, out bitmapData, out screenChar, out screenColor );

      // export data
      string result = ";bitmap data" + System.Environment.NewLine + ToASMData( bitmapData );

      result += System.Environment.NewLine + System.Environment.NewLine + ";screen ram data" + System.Environment.NewLine + ToASMData( screenChar );
      result += System.Environment.NewLine + System.Environment.NewLine + ";screen color data" + System.Environment.NewLine + ToASMData( screenColor );

      editDataExport.Text = result;
    }



    private void ExportHiresBitmap( int StartLine, int LineOffset, bool Hex, int WrapByteCount )
    {
      GR.Memory.ByteBuffer screenChar;
      GR.Memory.ByteBuffer screenColor;
      GR.Memory.ByteBuffer bitmapData;

      m_GraphicScreenProject.ImageToHiresBitmapData( m_GraphicScreenProject.ColorMapping, m_Chars, m_ErrornousChars, 0, 0, BlockWidth, BlockHeight, out bitmapData, out screenChar, out screenColor );

      if ( Hex )
      {
        editDataExport.Text = Util.ToBASICHexData( bitmapData + screenChar + screenColor, StartLine, LineOffset, WrapByteCount, 0 );
      }
      else
      {
        editDataExport.Text = Util.ToBASICData( bitmapData + screenChar + screenColor, StartLine, LineOffset, WrapByteCount, 0 );
      }
    }



    private void ExportMCBitmap()
    {
      GR.Memory.ByteBuffer              screenChar;
      GR.Memory.ByteBuffer              screenColor;
      GR.Memory.ByteBuffer              bitmapData;
      //Dictionary<int,byte>              forcedPattern = new Dictionary<int, byte>();

      m_GraphicScreenProject.ImageToMCBitmapData( m_GraphicScreenProject.ColorMapping, m_Chars, m_ErrornousChars, 0, 0, BlockWidth, BlockHeight, out bitmapData, out screenChar, out screenColor );

      // export data
      string    result = ";bitmap data" + System.Environment.NewLine + ToASMData( bitmapData );

      result += System.Environment.NewLine + System.Environment.NewLine + ";screen ram data" + System.Environment.NewLine + ToASMData( screenChar );
      result += System.Environment.NewLine + System.Environment.NewLine + ";screen color data" + System.Environment.NewLine + ToASMData( screenColor );

      editDataExport.Text = result;
    }



    private void ExportMCBitmap( int StartLine, int LineOffset, bool Hex, int WrapByteCount )
    {
      GR.Memory.ByteBuffer              screenChar;
      GR.Memory.ByteBuffer              screenColor;
      GR.Memory.ByteBuffer              bitmapData;
      //Dictionary<int,byte>              forcedPattern = new Dictionary<int, byte>();

      m_GraphicScreenProject.ImageToMCBitmapData( m_GraphicScreenProject.ColorMapping, m_Chars, m_ErrornousChars, 0, 0, BlockWidth, BlockHeight, out bitmapData, out screenChar, out screenColor );

      if ( Hex )
      {
        editDataExport.Text = Util.ToBASICHexData( bitmapData + screenChar + screenColor, StartLine, LineOffset, WrapByteCount, 0 );
      }
      else
      {
        editDataExport.Text = Util.ToBASICData( bitmapData + screenChar + screenColor, StartLine, LineOffset, WrapByteCount, 0 );
      }
    }



    private void ExportHiResCharset( bool ExportScreenAssembly )
    {
      // export possible
      Formats.CharsetProject projectToExport = new C64Studio.Formats.CharsetProject();
      int curCharIndex = 0;

      projectToExport.Colors.MultiColor1 = m_GraphicScreenProject.Colors.MultiColor1;
      projectToExport.Colors.MultiColor2 = m_GraphicScreenProject.Colors.MultiColor2;
      projectToExport.Colors.BackgroundColor = m_GraphicScreenProject.Colors.BackgroundColor;
      foreach ( Formats.CharData charData in m_Chars )
      {
        if ( charData.Replacement == null )
        {
          if ( curCharIndex >= 256 )
          {
            System.Windows.Forms.MessageBox.Show( "Too many characters!" );
            return;
          }
          projectToExport.Characters[curCharIndex] = charData.Clone();
          ++curCharIndex;
        }
      }
      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Save Charset Project as";
      saveDlg.Filter = "Charset Projects|*.charsetproject|All Files|*.*";
      if ( saveDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK )
      {
        GR.IO.File.WriteAllBytes( saveDlg.FileName, projectToExport.SaveToBuffer() );
      }
      if ( ExportScreenAssembly )
      {
        GR.Memory.ByteBuffer screenCharData   = new GR.Memory.ByteBuffer( (uint)( BlockWidth * BlockHeight ) );
        GR.Memory.ByteBuffer screenColorData  = new GR.Memory.ByteBuffer( (uint)( BlockWidth * BlockHeight ) );

        for ( int y = 0; y < BlockHeight; ++y )
        {
          for ( int x = 0; x < BlockWidth; ++x )
          {
            C64Studio.Formats.CharData  charUsed = m_Chars[x + y * BlockWidth];
            while ( charUsed.Replacement != null )
            {
              charUsed = charUsed.Replacement;
            }
            screenCharData.SetU8At( x + y * BlockWidth, (byte)charUsed.Index );
            screenColorData.SetU8At( x + y * BlockWidth, (byte)charUsed.Tile.CustomColor );
          }
        }
        string screenDataASM = ToASMData( screenCharData );
        string colorDataASM = ToASMData( screenColorData );

        editDataExport.Text = ";screen char data" + System.Environment.NewLine + screenDataASM + System.Environment.NewLine + ";screen color data" + System.Environment.NewLine + colorDataASM;
      }
    }



    private void ExportHiResCharset( int StartLine, int LineOffset, bool Hex, int WrapByteCount )
    {
      // export possible
      Formats.CharsetProject projectToExport = new C64Studio.Formats.CharsetProject();
      int curCharIndex = 0;

      projectToExport.Colors.MultiColor1 = m_GraphicScreenProject.Colors.MultiColor1;
      projectToExport.Colors.MultiColor2 = m_GraphicScreenProject.Colors.MultiColor2;
      projectToExport.Colors.BackgroundColor = m_GraphicScreenProject.Colors.BackgroundColor;
      // TODO
      //projectToExport.Colors.BGColor4 = m_GraphicScreenProject.
      foreach ( Formats.CharData charData in m_Chars )
      {
        if ( charData.Replacement == null )
        {
          if ( curCharIndex >= 256 )
          {
            System.Windows.Forms.MessageBox.Show( "Too many characters!" );
            return;
          }
          projectToExport.Characters[curCharIndex] = charData.Clone();
          ++curCharIndex;
        }
      }

      GR.Memory.ByteBuffer screenCharData   = new GR.Memory.ByteBuffer( (uint)( BlockWidth * BlockHeight ) );
      GR.Memory.ByteBuffer screenColorData  = new GR.Memory.ByteBuffer( (uint)( BlockWidth * BlockHeight ) );

      for ( int y = 0; y < BlockHeight; ++y )
      {
        for ( int x = 0; x < BlockWidth; ++x )
        {
          C64Studio.Formats.CharData  charUsed = m_Chars[x + y * BlockWidth];
          while ( charUsed.Replacement != null )
          {
            charUsed = charUsed.Replacement;
          }
          screenCharData.SetU8At( x + y * BlockWidth, (byte)charUsed.Index );
          screenColorData.SetU8At( x + y * BlockWidth, (byte)charUsed.Tile.CustomColor );
        }
      }

      if ( Hex )
      {
        editDataExport.Text = Util.ToBASICHexData( screenCharData + screenColorData, StartLine, LineOffset, WrapByteCount, 0 );
      }
      else
      {
        editDataExport.Text = Util.ToBASICData( screenCharData + screenColorData, StartLine, LineOffset, WrapByteCount, 0 );
      }
    }



    private void ExportMCCharset( bool ExportScreenAssembly )
    {
      // export possible
      Formats.CharsetProject projectToExport = new C64Studio.Formats.CharsetProject();
      int curCharIndex = 0;

      projectToExport.Colors.MultiColor1 = m_GraphicScreenProject.Colors.MultiColor1;
      projectToExport.Colors.MultiColor2 = m_GraphicScreenProject.Colors.MultiColor2;
      projectToExport.Colors.BackgroundColor = m_GraphicScreenProject.Colors.BackgroundColor;
      foreach ( Formats.CharData charData in m_Chars )
      {
        if ( charData.Replacement == null )
        {
          if ( curCharIndex >= 256 )
          {
            System.Windows.Forms.MessageBox.Show( "Too many characters!" );
            return;
          }
          projectToExport.Characters[curCharIndex] = charData.Clone();
          ++curCharIndex;
        }
      }
      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Save Charset Project as";
      saveDlg.Filter = "Charset Projects|*.charsetproject|All Files|*.*";
      if ( saveDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK )
      {
        GR.IO.File.WriteAllBytes( saveDlg.FileName, projectToExport.SaveToBuffer() );
      }
      if ( ExportScreenAssembly )
      {
        GR.Memory.ByteBuffer screenCharData   = new GR.Memory.ByteBuffer( (uint)( BlockWidth * BlockHeight ) );
        GR.Memory.ByteBuffer screenColorData  = new GR.Memory.ByteBuffer( (uint)( BlockWidth * BlockHeight ) );

        for ( int y = 0; y < BlockHeight; ++y )
        {
          for ( int x = 0; x < BlockWidth; ++x )
          {
            C64Studio.Formats.CharData  charUsed = m_Chars[x + y * BlockWidth];
            while ( charUsed.Replacement != null )
            {
              charUsed = charUsed.Replacement;
            }
            screenCharData.SetU8At( x + y * BlockWidth, (byte)charUsed.Index );
            screenColorData.SetU8At( x + y * BlockWidth, (byte)charUsed.Tile.CustomColor );
          }
        }
        string screenDataASM = ToASMData( screenCharData );
        string colorDataASM = ToASMData( screenColorData );

        editDataExport.Text = ";screen char data" + System.Environment.NewLine + screenDataASM + System.Environment.NewLine + ";screen color data" + System.Environment.NewLine + colorDataASM;
      }
    }



    private void ExportMCCharset( int StartLine, int LineOffset, bool Hex, int WrapByteCount )
    {
      // export possible
      Formats.CharsetProject projectToExport = new C64Studio.Formats.CharsetProject();
      int curCharIndex = 0;

      projectToExport.Colors.MultiColor1 = m_GraphicScreenProject.Colors.MultiColor1;
      projectToExport.Colors.MultiColor2 = m_GraphicScreenProject.Colors.MultiColor2;
      projectToExport.Colors.BackgroundColor = m_GraphicScreenProject.Colors.BackgroundColor;
      foreach ( Formats.CharData charData in m_Chars )
      {
        if ( charData.Replacement == null )
        {
          if ( curCharIndex >= 256 )
          {
            System.Windows.Forms.MessageBox.Show( "Too many characters!" );
            return;
          }
          projectToExport.Characters[curCharIndex] = charData.Clone();
          ++curCharIndex;
        }
      }
      GR.Memory.ByteBuffer screenCharData   = new GR.Memory.ByteBuffer( (uint)( BlockWidth * BlockHeight ) );
      GR.Memory.ByteBuffer screenColorData  = new GR.Memory.ByteBuffer( (uint)( BlockWidth * BlockHeight ) );

      for ( int y = 0; y < BlockHeight; ++y )
      {
        for ( int x = 0; x < BlockWidth; ++x )
        {
          C64Studio.Formats.CharData  charUsed = m_Chars[x + y * BlockWidth];
          while ( charUsed.Replacement != null )
          {
            charUsed = charUsed.Replacement;
          }
          screenCharData.SetU8At( x + y * BlockWidth, (byte)charUsed.Index );
          screenColorData.SetU8At( x + y * BlockWidth, (byte)charUsed.Tile.CustomColor );
        }
      }

      if ( Hex )
      {
        editDataExport.Text = Util.ToBASICHexData( screenCharData + screenColorData, StartLine, LineOffset, WrapByteCount, 0 );
      }
      else
      {
        editDataExport.Text = Util.ToBASICData( screenCharData + screenColorData, StartLine, LineOffset, WrapByteCount, 0 );
      }
    }



    private void UsedCharsToClipboard()
    {
      // export possible
      Formats.CharsetProject projectToExport = new C64Studio.Formats.CharsetProject();
      int curCharIndex = 0;

      projectToExport.Colors.MultiColor1     = m_GraphicScreenProject.Colors.MultiColor1;
      projectToExport.Colors.MultiColor2     = m_GraphicScreenProject.Colors.MultiColor2;
      projectToExport.Colors.BackgroundColor = m_GraphicScreenProject.Colors.BackgroundColor;
      foreach ( Formats.CharData charData in m_Chars )
      {
        if ( charData.Replacement == null )
        {
          if ( curCharIndex >= 256 )
          {
            System.Windows.Forms.MessageBox.Show( "Too many characters!" );
            return;
          }
          projectToExport.Characters[curCharIndex] = charData.Clone();
          ++curCharIndex;
        }
      }

      // selection to clipboard
      GR.Memory.ByteBuffer dataSelection = new GR.Memory.ByteBuffer();

      dataSelection.AppendI32( curCharIndex );
      // selection is range
      dataSelection.AppendI32( 0 );

      dataSelection.AppendI32( projectToExport.Colors.Palette.NumColors );
      for ( int i = 0; i < projectToExport.Colors.Palette.NumColors; ++i )
      {
        dataSelection.AppendU32( projectToExport.Colors.Palette.ColorValues[i] );
      }


      for ( int i = 0; i < curCharIndex; ++i )
      {
        // delta in indices
        dataSelection.AppendI32( ( i == 0 ) ? 0 : 1 );

        dataSelection.AppendI32( 0 ); // was (int)projectToExport.Characters[i].Mode );
        dataSelection.AppendI32( projectToExport.Characters[i].Tile.CustomColor );
        dataSelection.AppendU32( 8 );
        dataSelection.AppendU32( 8 );
        dataSelection.AppendU32( projectToExport.Characters[i].Tile.Data.Length );
        dataSelection.Append( projectToExport.Characters[i].Tile.Data );
        dataSelection.AppendI32( i );
      }

      DataObject dataObj = new DataObject();

      dataObj.SetData( "C64Studio.ImageList", false, dataSelection.MemoryStream() );

      Clipboard.SetDataObject( dataObj, true );
    }



    private void comboCheckType_SelectedIndexChanged( object sender, EventArgs e )
    {
      comboExportType.SelectedIndex = comboCheckType.SelectedIndex;
      m_GraphicScreenProject.SelectedCheckType = (C64Studio.Formats.GraphicScreenProject.CheckType)comboCheckType.SelectedIndex;

      int     numBytes = Lookup.NumBytesOfSingleCharacterBitmap( Lookup.CharacterModeFromCheckType( m_GraphicScreenProject.SelectedCheckType ) );
      if ( ( m_Chars.Count > 0 )
      &&   ( numBytes != m_Chars[0].Tile.Data.Length ) )
      {
        foreach ( var character in m_Chars )
        {
          character.Tile.Data.Resize( (uint)numBytes );
        }
      }

      int     numColors = Lookup.NumberOfColorsInDisplayMode( m_GraphicScreenProject.SelectedCheckType );

      if ( numColors != m_GraphicScreenProject.Colors.Palette.NumColors )
      {
        m_GraphicScreenProject.Colors.Palette = PaletteManager.PaletteFromNumColors( numColors );
        PaletteManager.ApplyPalette( pictureEditor.DisplayPage, m_GraphicScreenProject.Colors.Palette );
        PaletteManager.ApplyPalette( charEditor.DisplayPage, m_GraphicScreenProject.Colors.Palette );

        pictureEditor.Invalidate();
      }
    }



    private void editDataExport_PreviewKeyDown( object sender, PreviewKeyDownEventArgs e )
    {
      //Debug.Log( "Key = " + e.KeyData.ToString() );
      if ( e.KeyData == ( Keys.A | Keys.Control ) )
      {
        editDataExport.SelectAll();
      }
    }



    private void btnExportAsBinary_Click( object sender, EventArgs e )
    {
      int selExportType = comboExportData.SelectedIndex;
      if ( selExportType == -1 )
      {
        return;
      }

      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Save binary data as";
      saveDlg.Filter = "Binary Data|*.bin|All Files|*.*";
      if ( DocumentInfo.Project != null )
      {
        saveDlg.InitialDirectory = DocumentInfo.Project.Settings.BasePath;
      }
      if ( saveDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
      {
        return;
      }

      ExportBinaryType exportTypeToUse = ExportBinaryType.BITMAP_SCREEN_COLOR;
      foreach ( ExportBinaryType exportType in System.Enum.GetValues( typeof( ExportBinaryType ) ) )
      {
        if ( comboExportData.Items[selExportType].ToString() == GR.EnumHelper.GetDescription( exportType ) )
        {
          exportTypeToUse = exportType;
          break;
        }
      }

      GR.Memory.ByteBuffer screenChar   = new GR.Memory.ByteBuffer();
      GR.Memory.ByteBuffer screenColor  = new GR.Memory.ByteBuffer();
      GR.Memory.ByteBuffer bitmapData   = new GR.Memory.ByteBuffer();

      switch ( comboExportType.SelectedIndex )
      {
        case 0:
          // hires bitmap
          m_GraphicScreenProject.ImageToHiresBitmapData( m_GraphicScreenProject.ColorMapping, m_Chars, m_ErrornousChars, 0, 0, BlockWidth, BlockHeight, out bitmapData, out screenChar, out screenColor );
          break;
        case 1:
          // MC bitmap
          m_GraphicScreenProject.ImageToMCBitmapData( m_GraphicScreenProject.ColorMapping, m_Chars, m_ErrornousChars, 0, 0, BlockWidth, BlockHeight, out bitmapData, out screenChar, out screenColor );
          break;
        case 2:
          // hires charset
          break;
        case 3:
          // MC charset
          //ExportMCCharset( false );
          break;
        case 4:
          // MC charset and screen assembly
          //ExportMCCharset( true );
          break;
      }


      GR.Memory.ByteBuffer exportData = new GR.Memory.ByteBuffer();

      switch ( exportTypeToUse )
      {
        case ExportBinaryType.BITMAP_SCREEN_COLOR:
          exportData.Append( bitmapData );
          exportData.Append( screenChar );
          exportData.Append( screenColor );
          break;
        case ExportBinaryType.BITMAP_COLOR_SCREEN:
          exportData.Append( bitmapData );
          exportData.Append( screenColor );
          exportData.Append( screenChar );
          break;
        case ExportBinaryType.BITMAP:
          exportData.Append( bitmapData );
          break;
        case ExportBinaryType.BITMAP_COLOR:
          exportData.Append( bitmapData );
          exportData.Append( screenColor );
          break;
        case ExportBinaryType.BITMAP_SCREEN:
          exportData.Append( bitmapData );
          exportData.Append( screenChar );
          break;
      }
      if ( !GR.IO.File.WriteAllBytes( saveDlg.FileName, exportData ) )
      {
        MessageBox.Show( "Could not write data to file" );
      }
    }



    private void AdjustScrollbars()
    {
      screenHScroll.Minimum = 0;
      screenHScroll.SmallChange = 1;
      screenHScroll.LargeChange = 1;
      screenVScroll.SmallChange = 1;
      screenVScroll.LargeChange = 1;

      if ( m_GraphicScreenProject.Image.Width <= 320 )
      {
        screenHScroll.Maximum = 0;
        screenHScroll.Enabled = false;
        m_GraphicScreenProject.ScreenOffsetX = 0;
      }
      else
      {
        screenHScroll.Maximum = ( m_GraphicScreenProject.Image.Width - 320 + 7 ) / 8;
        screenHScroll.Enabled = true;
      }
      if ( m_GraphicScreenProject.ScreenOffsetX > screenHScroll.Maximum )
      {
        m_GraphicScreenProject.ScreenOffsetX = screenHScroll.Maximum;
      }

      screenVScroll.Minimum = 0;
      if ( m_GraphicScreenProject.Image.Height <= 200 )
      {
        screenVScroll.Maximum = 0;
        screenVScroll.Enabled = false;
        m_GraphicScreenProject.ScreenOffsetY = 0;
      }
      else
      {
        screenVScroll.Maximum = ( m_GraphicScreenProject.Image.Height - 200 + 7 ) / 8;
        screenVScroll.Enabled = true;
      }
      if ( m_GraphicScreenProject.ScreenOffsetY > screenVScroll.Maximum )
      {
        m_GraphicScreenProject.ScreenOffsetY = screenVScroll.Maximum;
      }
    }



    private void screenHScroll_Scroll( object sender, ScrollEventArgs e )
    {
      if ( m_GraphicScreenProject.ScreenOffsetX != e.NewValue )
      {
        m_GraphicScreenProject.ScreenOffsetX = e.NewValue;
        Redraw();
      }
    }



    private void screenVScroll_Scroll( object sender, ScrollEventArgs e )
    {
      if ( m_GraphicScreenProject.ScreenOffsetY != e.NewValue )
      {
        m_GraphicScreenProject.ScreenOffsetY = e.NewValue;
        Redraw();
      }
    }



    private void btnExportToCharScreen_Click( object sender, EventArgs e )
    {
      if ( comboCharScreens.SelectedIndex == -1 )
      {
        return;
      }
      // automatic check
      btnCheck_Click( sender, null );
      if ( !btnExportAs.Enabled )
      {
        return;
      }

      DocumentInfo    docToImportTo = (DocumentInfo)( (Types.ComboItem)comboCharScreens.SelectedItem ).Tag;

      CharsetScreenEditor   charScreen = null;

      if ( docToImportTo == null )
      {
        // import to new file
        charScreen = (CharsetScreenEditor)Core.MainForm.CreateNewDocument( ProjectElement.ElementType.CHARACTER_SCREEN, Core.MainForm.CurrentProject );
      }
      else
      {
        // import to existing file
        if ( docToImportTo.BaseDoc == null )
        {
          docToImportTo.Project.ShowDocument( docToImportTo.Element );
        }
        charScreen = (CharsetScreenEditor)docToImportTo.BaseDoc;
      }

      if ( charScreen != null )
      {
        Formats.CharsetScreenProject    project = new C64Studio.Formats.CharsetScreenProject();
        Formats.CharsetProject          charset = new C64Studio.Formats.CharsetProject();

        project.SetScreenSize( BlockWidth, BlockHeight );

        charset.Colors.BackgroundColor = m_GraphicScreenProject.Colors.BackgroundColor;
        charset.Colors.MultiColor1     = m_GraphicScreenProject.Colors.MultiColor1;
        charset.Colors.MultiColor2     = m_GraphicScreenProject.Colors.MultiColor2;

        switch ( m_GraphicScreenProject.SelectedCheckType )
        {
          case Formats.GraphicScreenProject.CheckType.HIRES_BITMAP:
          case Formats.GraphicScreenProject.CheckType.HIRES_CHARSET:
            project.Mode = TextMode.COMMODORE_40_X_25_HIRES;
            charset.Mode = TextCharMode.COMMODORE_HIRES;
            break;
          case Formats.GraphicScreenProject.CheckType.MULTICOLOR_BITMAP:
          case Formats.GraphicScreenProject.CheckType.MULTICOLOR_CHARSET:
            project.Mode = TextMode.COMMODORE_40_X_25_MULTICOLOR;
            charset.Mode = TextCharMode.COMMODORE_MULTICOLOR;
            break;
          case Formats.GraphicScreenProject.CheckType.MEGA65_FCM_CHARSET:
            project.Mode = TextMode.MEGA65_40_X_25_FCM;
            charset.Mode = TextCharMode.MEGA65_FCM;
            break;
          case Formats.GraphicScreenProject.CheckType.MEGA65_FCM_CHARSET_16BIT:
            project.Mode = TextMode.MEGA65_40_X_25_FCM_16BIT;
            charset.Mode = TextCharMode.MEGA65_FCM_16BIT;
            break;
        }
        charset.Colors.Palette          = new Palette( m_GraphicScreenProject.Colors.Palette );
        charset.Colors.BackgroundColor  = project.CharSet.Colors.BackgroundColor;
        charset.Colors.MultiColor1      = project.CharSet.Colors.MultiColor1;
        charset.Colors.MultiColor2      = project.CharSet.Colors.MultiColor2;
        charset.Colors.BGColor4         = project.CharSet.Colors.BGColor4;

        for ( int i = 0; i < m_Chars.Count; ++i )
        {
          project.Chars[i] = (uint)( ( m_Chars[i].Index & 0xffff ) | ( m_Chars[i].Tile.CustomColor << 16 ) );
          if ( m_Chars[i].Replacement == null )
          {
            charset.Characters[m_Chars[i].Index].Tile.Data = m_Chars[i].Tile.Data;
          }
          else
          {
            project.Chars[i] = (uint)( ( m_Chars[i].Replacement.Index & 0xffff ) | ( m_Chars[i].Tile.CustomColor << 16 ) );
          }
        }

        charScreen.InjectProjects( project, charset );
        charScreen.SetModified();
      }
    }



    void MainForm_ApplicationEvent( C64Studio.Types.ApplicationEvent Event )
    {
      if ( ( Event.EventType == C64Studio.Types.ApplicationEvent.Type.DOCUMENT_INFO_CREATED )
      &&   ( Event.Doc.Type == ProjectElement.ElementType.CHARACTER_SCREEN ) )
      {
        string    nameToUse = Event.Doc.DocumentFilename ?? "New File";
        comboCharScreens.Items.Add( new Types.ComboItem( nameToUse, Event.Doc ) );
      }
      if ( ( Event.EventType == C64Studio.Types.ApplicationEvent.Type.DOCUMENT_INFO_REMOVED )
      &&   ( Event.Doc.Type == ProjectElement.ElementType.CHARACTER_SCREEN ) )
      {
        foreach ( Types.ComboItem comboItem in comboCharScreens.Items )
        {
          if ( (DocumentInfo)comboItem.Tag == Event.Doc )
          {
            comboCharScreens.Items.Remove( comboItem );
            if ( comboCharScreens.SelectedIndex == -1 )
            {
              comboCharScreens.SelectedIndex = 0;
            }
            break;
          }
        }
      }
    }



    private void btnApplyScreenSize_Click( object sender, EventArgs e )
    {
      int     newWidth  = GR.Convert.ToI32( editScreenWidth.Text );
      int     newHeight = GR.Convert.ToI32( editScreenHeight.Text );

      if ( ( newWidth > 0 )
      &&   ( newHeight > 0 ) )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoGraphicScreenSizeChange( m_GraphicScreenProject, this, m_GraphicScreenProject.ScreenWidth, m_GraphicScreenProject.ScreenHeight ) );
        SetScreenSize( newWidth, newHeight );
        Redraw();
      }
    }



    private void editScreenWidth_TextChanged( object sender, EventArgs e )
    {
      int     newWidth  = GR.Convert.ToI32( editScreenWidth.Text );
      int     newHeight = GR.Convert.ToI32( editScreenHeight.Text );

      btnApplyScreenSize.Enabled = ( ( newWidth > 0 ) 
                                &&   ( ( newWidth % 8 ) == 0 )
                                &&   ( newHeight> 0 ) 
                                &&   ( ( newHeight % 8 ) == 0 ) );
    }



    private void editScreenHeight_TextChanged( object sender, EventArgs e )
    {
      int     newWidth  = GR.Convert.ToI32( editScreenWidth.Text );
      int     newHeight = GR.Convert.ToI32( editScreenHeight.Text );

      btnApplyScreenSize.Enabled = ( ( newWidth > 0 )
                                &&   ( newHeight > 0 ) );
    }



    public void UpdateArea( int X, int Y, int Width, int Height )
    {
      pictureEditor.Invalidate();
      // in case the selected char was modified
      charEditor.DisplayPage.DrawImage( m_GraphicScreenProject.Image, 0, 0, m_SelectedChar.X * 8, m_SelectedChar.Y * 8, 8, 8 );
      charEditor.Invalidate();
      Redraw();
    }



    private void btnColorMappingRemove_Click( object sender, EventArgs e )
    {

    }



    private void listColorMappingTargets_SelectedIndexChanged( object sender, ArrangedItemEntry Item )
    {
      UpdateColorMappingButtons();
    }



    private void listColorMappingColors_SelectedIndexChanged( object sender, EventArgs e )
    {
      UpdateCurrentColorMapping();
    }



    private void UpdateCurrentColorMapping()
    { 
      listColorMappingTargets.Items.Clear();
      if ( listColorMappingColors.SelectedIndex == -1 )
      {
        UpdateColorMappingButtons();
        return;
      }
      foreach ( var entry in m_GraphicScreenProject.ColorMapping[listColorMappingColors.SelectedIndex] )
      {
        listColorMappingTargets.Items.Add( GR.EnumHelper.GetDescription( entry ) );
      }
      UpdateColorMappingButtons();
    }



    private void comboColorMappingTargets_SelectedIndexChanged( object sender, EventArgs e )
    {
      UpdateColorMappingButtons();
    }



    void UpdateColorMappingButtons()
    {
      C64Studio.Formats.GraphicScreenProject.ColorMappingTarget   targetIndex = (C64Studio.Formats.GraphicScreenProject.ColorMappingTarget)comboColorMappingTargets.SelectedIndex;

      listColorMappingTargets.DeleteButtonEnabled = ( ( listColorMappingTargets.SelectedIndices.Count > 0 )
                                                && ( listColorMappingTargets.SelectedIndices[0] + 1 < listColorMappingTargets.Items.Count ) );

      if ( ( listColorMappingTargets.Items.Count <= 1 )
      ||   ( listColorMappingTargets.SelectedIndices.Count < 1 ) )
      {
        listColorMappingTargets.MoveUpButtonEnabled = false;
        listColorMappingTargets.MoveDownButtonEnabled = false;
      }
      else
      {
        listColorMappingTargets.MoveUpButtonEnabled = ( ( listColorMappingTargets.SelectedIndices[0] > 0 )
                                                  && ( ColorMappingFromItem( listColorMappingTargets.Items[listColorMappingTargets.SelectedIndices[0]] ) != Formats.GraphicScreenProject.ColorMappingTarget.ANY ) );
        listColorMappingTargets.MoveDownButtonEnabled = ( ( listColorMappingTargets.SelectedIndices[0] + 1 < listColorMappingTargets.Items.Count )
                                                       && ( ColorMappingFromItem( listColorMappingTargets.Items[listColorMappingTargets.SelectedIndices[0] + 1] ) != Formats.GraphicScreenProject.ColorMappingTarget.ANY ) );
      }


      int     sourceColor = listColorMappingColors.SelectedIndex;
      if ( sourceColor == -1 )
      {
        listColorMappingTargets.AddButtonEnabled = false;
        return;
      }
      listColorMappingTargets.AddButtonEnabled = ( !m_GraphicScreenProject.ColorMapping[sourceColor].Contains( targetIndex ) );
    }



    private void listColorMappingColors_DrawItem( object sender, DrawItemEventArgs e )
    {
      ListBox list = (ListBox)sender;

      e.DrawBackground();

      int offset = (int)e.Graphics.MeasureString( "22", list.Font ).Width + 5 + 3;
      System.Drawing.Rectangle itemRect = new System.Drawing.Rectangle( e.Bounds.Left + offset, e.Bounds.Top, e.Bounds.Width - offset, e.Bounds.Height );
      if ( e.Index != -1 )
      {
        e.Graphics.FillRectangle( ConstantData.Palette.ColorBrushes[e.Index], itemRect );
        if ( ( e.State & DrawItemState.Selected ) != 0 )
        {
          e.Graphics.DrawString( list.Items[e.Index].ToString(), list.Font, new System.Drawing.SolidBrush( System.Drawing.Color.White ), 3.0f, e.Bounds.Top + 1.0f );
        }
        else
        {
          e.Graphics.DrawString( list.Items[e.Index].ToString(), list.Font, new System.Drawing.SolidBrush( System.Drawing.Color.Black ), 3.0f, e.Bounds.Top + 1.0f );
        }
      }
    }



    private ArrangedItemEntry listColorMappingTargets_AddingItem( object sender )
    {
      C64Studio.Formats.GraphicScreenProject.ColorMappingTarget   targetIndex = (C64Studio.Formats.GraphicScreenProject.ColorMappingTarget)comboColorMappingTargets.SelectedIndex;

      int     sourceColor = listColorMappingColors.SelectedIndex;
      if ( sourceColor == -1 )
      {
        return null;
      }

      if ( m_GraphicScreenProject.ColorMapping[sourceColor].Contains( targetIndex ) )
      {
        return null;
      }

      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoGraphicScreenValuesChange( m_GraphicScreenProject, this ) );
      m_GraphicScreenProject.ColorMapping[sourceColor].Insert( m_GraphicScreenProject.ColorMapping[sourceColor].Count - 1, targetIndex );

      var newItem = new ArrangedItemEntry( GR.EnumHelper.GetDescription( targetIndex ) );

      Modified = true;
      UpdateColorMappingButtons();
      return newItem;
    }



    private void listColorMappingTargets_ItemRemoved( object sender, ArrangedItemEntry Item )
    {
      int     sourceColor = listColorMappingColors.SelectedIndex;
      if ( sourceColor == -1 )
      {
        return;
      }
      if ( m_GraphicScreenProject.ColorMapping[sourceColor].Count == 1 )
      {
        return;
      }

      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoGraphicScreenValuesChange( m_GraphicScreenProject, this ) );
      foreach ( C64Studio.Formats.GraphicScreenProject.ColorMappingTarget entry in System.Enum.GetValues( typeof( C64Studio.Formats.GraphicScreenProject.ColorMappingTarget ) ) )
      {
        if ( GR.EnumHelper.GetDescription( entry ) == Item.Text )
        {
          m_GraphicScreenProject.ColorMapping[sourceColor].Remove( entry );
          break;
        }
      }
      Modified = true;
    }



    private void listColorMappingTargets_ItemMoved( object sender, ArrangedItemEntry Item1, ArrangedItemEntry Item2 )
    {
      int     sourceColor = listColorMappingColors.SelectedIndex;
      if ( sourceColor == -1 )
      {
        return;
      }

      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoGraphicScreenValuesChange( m_GraphicScreenProject, this ) );
      m_GraphicScreenProject.ColorMapping[sourceColor].Clear();

      foreach ( ArrangedItemEntry item in listColorMappingTargets.Items )
      {
        m_GraphicScreenProject.ColorMapping[sourceColor].Add( ColorMappingFromItem( item ) );
      }
      Modified = true;
    }



    private C64Studio.Formats.GraphicScreenProject.ColorMappingTarget ColorMappingFromItem( ArrangedItemEntry Item )
    {
      foreach ( C64Studio.Formats.GraphicScreenProject.ColorMappingTarget entry in System.Enum.GetValues( typeof( C64Studio.Formats.GraphicScreenProject.ColorMappingTarget ) ) )
      {
        if ( GR.EnumHelper.GetDescription( entry ) == Item.Text )
        {
          return entry;
        }
      }
      return Formats.GraphicScreenProject.ColorMappingTarget.ANY;
    }



    private bool listColorMappingTargets_MovingItem( object sender, ArrangedItemEntry Item1, ArrangedItemEntry Item2 )
    {
            /*
      var     colorMapping1 = ColorMappingFromItem( Item1 );
      var     colorMapping2 = ColorMappingFromItem( Item2 );

      if ( ( colorMapping1 == Formats.GraphicScreenProject.ColorMappingTarget.ANY )
      ||   ( colorMapping2 == Formats.GraphicScreenProject.ColorMappingTarget.ANY ) )
      {
        return false;
      }*/
      return true;
    }



    private void listColorMappingTargets_ItemAdded( object sender, ArrangedItemEntry Item )
    {
      listColorMappingTargets.Items.Remove( Item );
      listColorMappingTargets.Items.Insert( listColorMappingTargets.Items.Count - 1, Item );
    }



    private void btnExportToBASICData_Click( object sender, EventArgs e )
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
      if ( wrapByteCount < 10 )
      {
        wrapByteCount = 10;
      }

      switch ( (ExportDataType)comboExportType.SelectedIndex )
      {
        case ExportDataType.HIRES_BITMAP:
          ExportHiresBitmap( startLine, lineOffset, false, wrapByteCount );
          break;
        case ExportDataType.MULTICOLOR_BITMAP:
          ExportMCBitmap( startLine, lineOffset, false, wrapByteCount );
          break;
        case ExportDataType.HIRES_CHARSET:
        case ExportDataType.HIRES_CHARSET_SCREEN_ASSEMBLY:
          ExportHiResCharset( startLine, lineOffset, false, wrapByteCount );
          break;
        case ExportDataType.MULTICOLOR_CHARSET:
        case ExportDataType.MULTICOLOR_CHARSET_SCREEN_ASSEMBLY:
          ExportMCCharset( startLine, lineOffset, false, wrapByteCount );
          break;
        case ExportDataType.CHARACTERS_TO_CLIPBOARD:
          UsedCharsToClipboard();
          break;
      }
    }



    private int GetExportWrapCount()
    {
      if ( !checkExportToDataWrap.Checked )
      {
        return 80;
      }
      return GR.Convert.ToI32( editWrapByteCount.Text );
    }



    private void colorSelector_MouseDown( object sender, MouseEventArgs e )
    {
      int     colorIndex = ( e.X * 16 * 8 / colorSelector.ClientSize.Width ) / 8;
      colorSelector.DisplayPage.Rectangle( m_CurrentColor * 8, 0, 8, 8, (uint)m_CurrentColor );

      m_CurrentColor = (byte)colorIndex;
      colorSelector.DisplayPage.Rectangle( m_CurrentColor * 8, 0, 8, 8, 16 );
      colorSelector.Invalidate();
    }



    private void btnExportToImage_Click( object sender, EventArgs e )
    {
      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Export Characters to Image";
      saveDlg.Filter = Core.MainForm.FilterString( C64Studio.Types.Constants.FILEFILTER_IMAGE_FILES );
      if ( saveDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
      {
        return;
      }

      string    extension = System.IO.Path.GetExtension( saveDlg.FileName ).ToUpper();

      if ( ( extension == ".KLA" )
      ||   ( extension == ".KOA" ) )
      {
        if ( ( m_GraphicScreenProject.ScreenWidth != 320 )
        ||   ( m_GraphicScreenProject.ScreenHeight != 200 ) )
        {
          MessageBox.Show( "A graphic can only be exported to Koala format if the size is 320x200!", "Can't export to Koala" );
          return;
        }
      }

      GR.Memory.ByteBuffer              screenChar;
      GR.Memory.ByteBuffer              screenColor;
      GR.Memory.ByteBuffer              bitmapData;

      m_GraphicScreenProject.ImageToMCBitmapData( m_GraphicScreenProject.ColorMapping, m_Chars, m_ErrornousChars, 0, 0, BlockWidth, BlockHeight, out bitmapData, out screenChar, out screenColor );

      System.Drawing.Imaging.ImageFormat formatToSave = System.Drawing.Imaging.ImageFormat.Png;

      if ( ( extension == ".KLA" )
      ||   ( extension == ".KOA" ) )
      {
        var koalaData = C64Studio.Converter.KoalaToBitmap.KoalaFromBitmap( bitmapData, screenChar, screenColor, (byte)m_GraphicScreenProject.Colors.BackgroundColor );

        if ( !GR.IO.File.WriteAllBytes( saveDlg.FileName, koalaData ) )
        {
          MessageBox.Show( "Could not export to file " + saveDlg.FileName, "Error writing to file" );
        }
        return;
      }
      if ( extension == ".BMP" )
      {
        formatToSave = System.Drawing.Imaging.ImageFormat.Bmp;
      }
      else if ( extension == ".PNG" )
      {
        formatToSave = System.Drawing.Imaging.ImageFormat.Png;
      }
      else if ( extension == ".GIF" )
      {
        formatToSave = System.Drawing.Imaging.ImageFormat.Gif;
      }
      else
      {
        MessageBox.Show( "Unsupported image file format " + extension + ". Please make sure to use the proper extension.", "Unsupported format" );
        return;
      }

      var bitmap = m_GraphicScreenProject.Image.GetAsBitmap();
      try
      {
        bitmap.Save( saveDlg.FileName, formatToSave );
      }
      catch ( Exception ex )
      {
        MessageBox.Show( "An error occurred while writing to file: " + ex.Message, "An Error occurred" );
      }
    }



    private void btnFullCopyToClipboard_Click( object sender, EventArgs e )
    {
      CopyImageToClipboard();
    }



    private void btnToolPaint_CheckedChanged( object sender, EventArgs e )
    {
      m_PaintTool = PaintTool.DRAW_PIXEL;
      Redraw();
    }



    private void btnToolRect_CheckedChanged( object sender, EventArgs e )
    {
      m_PaintTool = PaintTool.DRAW_RECTANGLE;
      Redraw();
    }



    private void btnToolQuad_CheckedChanged( object sender, EventArgs e )
    {
      m_PaintTool = PaintTool.DRAW_BOX;
      Redraw();
    }



    private void btnToolFill_CheckedChanged( object sender, EventArgs e )
    {
      m_PaintTool = PaintTool.FLOOD_FILL;
      Redraw();
    }



    private void btnToolSelect_CheckedChanged( object sender, EventArgs e )
    {
      m_PaintTool = PaintTool.SELECT;
      Redraw();
    }



    private void btnToolValidate_CheckedChanged( object sender, EventArgs e )
    {
      m_PaintTool = PaintTool.VALIDATE;
      Redraw();
    }



    private void pictureEditor_MouseUp( object sender, MouseEventArgs e )
    {
      HandleMouseOnEditor( e.X, e.Y, e.Button );
    }



    public new void Dispose()
    {
      if ( m_SelectionFloatingImage != null )
      {
        m_SelectionFloatingImage.Dispose();
        m_SelectionFloatingImage = null;
      }
      base.Dispose();
    }



    private void btnMirrorX_Click( object sender, EventArgs e )
    {
      MirrorH();
    }



    private void MirrorH()
    {
      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoGraphicScreenImageChange( m_GraphicScreenProject, this, m_SelectedChar.X * 8, m_SelectedChar.Y * 8, 8, 8 ) );

      for ( int j = 0; j < 8; ++j )
      {
        for ( int i = 0; i < 4; ++i )
        {
          uint  curColor = m_GraphicScreenProject.Image.GetPixel( m_SelectedChar.X * 8 + i, m_SelectedChar.Y * 8 + j );
          m_GraphicScreenProject.Image.SetPixel( m_SelectedChar.X * 8 + i, m_SelectedChar.Y * 8 + j, 
            m_GraphicScreenProject.Image.GetPixel( m_SelectedChar.X * 8 + 7 - i, m_SelectedChar.Y * 8 + j ) );
          m_GraphicScreenProject.Image.SetPixel( m_SelectedChar.X * 8 + 7 - i, m_SelectedChar.Y * 8 + j, curColor );
        }
      }

      charEditor.DisplayPage.DrawImage( m_GraphicScreenProject.Image, 0, 0, m_SelectedChar.X * 8, m_SelectedChar.Y * 8, 8, 8 );
      charEditor.Invalidate();

      Redraw();
      pictureEditor.Invalidate();
      Modified = true;
    }



    private void btnMirrorY_Click( object sender, EventArgs e )
    {
      MirrorV();
    }



    private void MirrorV()
    {
      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoGraphicScreenImageChange( m_GraphicScreenProject, this, m_SelectedChar.X * 8, m_SelectedChar.Y * 8, 8, 8 ) );

      for ( int j = 0; j < 4; ++j )
      {
        for ( int i = 0; i < 8; ++i )
        {
          uint  curColor = m_GraphicScreenProject.Image.GetPixel( m_SelectedChar.X * 8 + i, m_SelectedChar.Y * 8 + j );
          m_GraphicScreenProject.Image.SetPixel( m_SelectedChar.X * 8 + i, m_SelectedChar.Y * 8 + j,
            m_GraphicScreenProject.Image.GetPixel( m_SelectedChar.X * 8 + i, m_SelectedChar.Y * 8 + 7 - j ) );
          m_GraphicScreenProject.Image.SetPixel( m_SelectedChar.X * 8 + i, m_SelectedChar.Y * 8 + 7 - j, curColor );
        }
      }

      charEditor.DisplayPage.DrawImage( m_GraphicScreenProject.Image, 0, 0, m_SelectedChar.X * 8, m_SelectedChar.Y * 8, 8, 8 );
      charEditor.Invalidate();

      Redraw();
      pictureEditor.Invalidate();
      Modified = true;
    }



    private void btnShiftUp_Click( object sender, EventArgs e )
    {
      ShiftUp();
    }



    private void ShiftUp()
    {
      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoGraphicScreenImageChange( m_GraphicScreenProject, this, m_SelectedChar.X * 8, m_SelectedChar.Y * 8, 8, 8 ) );

      for ( int i = 0; i < 8; ++i )
      {
        uint  curColor = m_GraphicScreenProject.Image.GetPixel( m_SelectedChar.X * 8 + i, m_SelectedChar.Y * 8 );
        for ( int j = 0; j < 7; ++j )
        {
          
          m_GraphicScreenProject.Image.SetPixel( m_SelectedChar.X * 8 + i, m_SelectedChar.Y * 8 + j,
            m_GraphicScreenProject.Image.GetPixel( m_SelectedChar.X * 8 + i, m_SelectedChar.Y * 8 + j + 1 ) );
        }
        m_GraphicScreenProject.Image.SetPixel( m_SelectedChar.X * 8 + i, m_SelectedChar.Y * 8 + 7, curColor );
      }

      charEditor.DisplayPage.DrawImage( m_GraphicScreenProject.Image, 0, 0, m_SelectedChar.X * 8, m_SelectedChar.Y * 8, 8, 8 );
      charEditor.Invalidate();

      Redraw();
      pictureEditor.Invalidate();
      Modified = true;
    }



    private void btnShiftDown_Click( object sender, EventArgs e )
    {
      ShiftDown();
    }



    private void ShiftDown()
    {
      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoGraphicScreenImageChange( m_GraphicScreenProject, this, m_SelectedChar.X * 8, m_SelectedChar.Y * 8, 8, 8 ) );

      for ( int i = 0; i < 8; ++i )
      {
        uint  curColor = m_GraphicScreenProject.Image.GetPixel( m_SelectedChar.X * 8 + i, m_SelectedChar.Y * 8 + 7 );
        for ( int j = 0; j < 7; ++j )
        {
          m_GraphicScreenProject.Image.SetPixel( m_SelectedChar.X * 8 + i, m_SelectedChar.Y * 8 + 7 - j,
            m_GraphicScreenProject.Image.GetPixel( m_SelectedChar.X * 8 + i, m_SelectedChar.Y * 8 + 6 - j ) );
        }
        m_GraphicScreenProject.Image.SetPixel( m_SelectedChar.X * 8 + i, m_SelectedChar.Y * 8, curColor );
      }

      charEditor.DisplayPage.DrawImage( m_GraphicScreenProject.Image, 0, 0, m_SelectedChar.X * 8, m_SelectedChar.Y * 8, 8, 8 );
      charEditor.Invalidate();

      Redraw();
      pictureEditor.Invalidate();
      Modified = true;
    }



    private void btnShiftRight_Click( object sender, EventArgs e )
    {
      ShiftRight();
    }



    private void ShiftRight()
    {
      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoGraphicScreenImageChange( m_GraphicScreenProject, this, m_SelectedChar.X * 8, m_SelectedChar.Y * 8, 8, 8 ) );

      for ( int i = 0; i < 8; ++i )
      {
        uint  curColor = m_GraphicScreenProject.Image.GetPixel( m_SelectedChar.X * 8 + 7, m_SelectedChar.Y * 8 + i );
        for ( int j = 0; j < 7; ++j )
        {
          m_GraphicScreenProject.Image.SetPixel( m_SelectedChar.X * 8 + 7 - j, m_SelectedChar.Y * 8 + i,
            m_GraphicScreenProject.Image.GetPixel( m_SelectedChar.X * 8 + 6 - j, m_SelectedChar.Y * 8 + i ) );
        }
        m_GraphicScreenProject.Image.SetPixel( m_SelectedChar.X * 8, m_SelectedChar.Y * 8 + i, curColor );
      }

      charEditor.DisplayPage.DrawImage( m_GraphicScreenProject.Image, 0, 0, m_SelectedChar.X * 8, m_SelectedChar.Y * 8, 8, 8 );
      charEditor.Invalidate();

      Redraw();
      pictureEditor.Invalidate();
      Modified = true;
    }



    private void btnShiftLeft_Click( object sender, EventArgs e )
    {
      ShiftLeft();
    }



    private void ShiftLeft()
    {
      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoGraphicScreenImageChange( m_GraphicScreenProject, this, m_SelectedChar.X * 8, m_SelectedChar.Y * 8, 8, 8 ) );

      for ( int i = 0; i < 8; ++i )
      {
        uint  curColor = m_GraphicScreenProject.Image.GetPixel( m_SelectedChar.X * 8, m_SelectedChar.Y * 8 + i );
        for ( int j = 0; j < 7; ++j )
        {
          m_GraphicScreenProject.Image.SetPixel( m_SelectedChar.X * 8 + j, m_SelectedChar.Y * 8 + i,
            m_GraphicScreenProject.Image.GetPixel( m_SelectedChar.X * 8 + j + 1, m_SelectedChar.Y * 8 + i ) );
        }
        m_GraphicScreenProject.Image.SetPixel( m_SelectedChar.X * 8 + 7, m_SelectedChar.Y * 8 + i, curColor );
      }

      charEditor.DisplayPage.DrawImage( m_GraphicScreenProject.Image, 0, 0, m_SelectedChar.X * 8, m_SelectedChar.Y * 8, 8, 8 );
      charEditor.Invalidate();

      Redraw();
      pictureEditor.Invalidate();
      Modified = true;
    }



    public override bool ApplyFunction( Function Function )
    {
      if ( !pictureEditor.Focused )
      {
        return false;
      }

      switch ( Function )
      {
        case Function.GRAPHIC_ELEMENT_MIRROR_H:
          MirrorH();
          return true;
        case Function.GRAPHIC_ELEMENT_MIRROR_V:
          MirrorV();
          return true;
        case Function.GRAPHIC_ELEMENT_SHIFT_D:
          ShiftDown();
          return true;
        case Function.GRAPHIC_ELEMENT_SHIFT_L:
          ShiftLeft();
          return true;
        case Function.GRAPHIC_ELEMENT_SHIFT_R:
          ShiftRight();
          return true;
        case Function.GRAPHIC_ELEMENT_SHIFT_U:
          ShiftUp();
          return true;
      }
      return base.ApplyFunction( Function );
    }



    private void btnExportToBASICDataHex_Click( object sender, EventArgs e )
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
      if ( wrapByteCount < 10 )
      {
        wrapByteCount = 10;
      }

      switch ( (ExportDataType)comboExportType.SelectedIndex )
      {
        case ExportDataType.HIRES_BITMAP:
          ExportHiresBitmap( startLine, lineOffset, true, wrapByteCount );
          break;
        case ExportDataType.MULTICOLOR_BITMAP:
          ExportMCBitmap( startLine, lineOffset, true, wrapByteCount );
          break;
        case ExportDataType.HIRES_CHARSET:
        case ExportDataType.HIRES_CHARSET_SCREEN_ASSEMBLY:
          ExportHiResCharset( startLine, lineOffset, true, wrapByteCount );
          break;
        case ExportDataType.MULTICOLOR_CHARSET:
        case ExportDataType.MULTICOLOR_CHARSET_SCREEN_ASSEMBLY:
          ExportMCCharset( startLine, lineOffset, true, wrapByteCount );
          break;
        case ExportDataType.CHARACTERS_TO_CLIPBOARD:
          UsedCharsToClipboard();
          break;
      }
    }



    private void pictureEditor_PostPaint( GR.Image.FastImage TargetBuffer )
    {
      uint  selColor = Core.Settings.FGColor( ColorableElement.SELECTION_FRAME );

      switch ( m_PaintTool )
      {
        case PaintTool.VALIDATE:
          for ( int j = 0; j < BlockHeight; ++j )
          {
            for ( int i = 0; i < BlockWidth; ++i )
            {
              if ( m_ErrornousChars[i, j] )
              {
                int  sx1 = ( ( i - m_GraphicScreenProject.ScreenOffsetX ) * pictureEditor.ClientRectangle.Width ) / 40;
                int  sx2 = ( ( i + 1 - m_GraphicScreenProject.ScreenOffsetX ) * pictureEditor.ClientRectangle.Width ) / 40;
                int  sy1 = ( ( j - m_GraphicScreenProject.ScreenOffsetY ) * pictureEditor.ClientRectangle.Height ) / 25;
                int  sy2 = ( ( j + 1 - m_GraphicScreenProject.ScreenOffsetY ) * pictureEditor.ClientRectangle.Height ) / 25;

                for ( int x = sx1; x <= sx2; ++x )
                {
                  TargetBuffer.SetPixel( x, sy1, (uint)( ( x & 1 ) * selColor ) );
                }
                for ( int y = sy1; y <= sy2; ++y )
                {
                  TargetBuffer.SetPixel( sx1, y, (uint)( ( y & 1 ) * selColor ) );
                }
              }
            }
          }
          if ( m_SelectedChar.X != -1 )
          {
            int  sx1 = ( ( m_SelectedChar.X - m_GraphicScreenProject.ScreenOffsetX ) * pictureEditor.ClientRectangle.Width ) / 40;
            int  sx2 = ( ( m_SelectedChar.X + 1 - m_GraphicScreenProject.ScreenOffsetX ) * pictureEditor.ClientRectangle.Width ) / 40;
            int  sy1 = ( ( m_SelectedChar.Y - m_GraphicScreenProject.ScreenOffsetY ) * pictureEditor.ClientRectangle.Height ) / 25;
            int  sy2 = ( ( m_SelectedChar.Y + 1 - m_GraphicScreenProject.ScreenOffsetY ) * pictureEditor.ClientRectangle.Height ) / 25;

            TargetBuffer.Rectangle( sx1, sy1, sx2 - sx1, sy2 - sy1, selColor );
          }
          break;
        case PaintTool.SELECT:
          if ( m_DragStartPoint.X != -1 )
          {
            int     x1 = Math.Min( m_DragStartPoint.X, m_DragCurrentPoint.X );
            int     x2 = Math.Max( m_DragStartPoint.X, m_DragCurrentPoint.X );
            int     y1 = Math.Min( m_DragStartPoint.Y, m_DragCurrentPoint.Y );
            int     y2 = Math.Max( m_DragStartPoint.Y, m_DragCurrentPoint.Y );

            int  sx1 = ( ( x1 - m_GraphicScreenProject.ScreenOffsetX ) * pictureEditor.ClientRectangle.Width ) / m_GraphicScreenProject.ScreenWidth;
            int  sx2 = ( ( x2 - m_GraphicScreenProject.ScreenOffsetX ) * pictureEditor.ClientRectangle.Width ) / m_GraphicScreenProject.ScreenWidth;
            int  sy1 = ( ( y1 - m_GraphicScreenProject.ScreenOffsetY ) * pictureEditor.ClientRectangle.Height ) / m_GraphicScreenProject.ScreenHeight;
            int  sy2 = ( ( y2 - m_GraphicScreenProject.ScreenOffsetY ) * pictureEditor.ClientRectangle.Height ) / m_GraphicScreenProject.ScreenHeight;

            TargetBuffer.Rectangle( sx1, sy1, sx2 - sx1, sy2 - sy1, selColor );
          }
          if ( m_Selection.Width > 0 )
          {
            int  sx1 = ( ( m_Selection.X - m_GraphicScreenProject.ScreenOffsetX ) * pictureEditor.ClientRectangle.Width ) / m_GraphicScreenProject.ScreenWidth;
            int  sx2 = ( ( m_Selection.X + m_Selection.Width - m_GraphicScreenProject.ScreenOffsetX ) * pictureEditor.ClientRectangle.Width ) / m_GraphicScreenProject.ScreenWidth;
            int  sy1 = ( ( m_Selection.Y - m_GraphicScreenProject.ScreenOffsetY ) * pictureEditor.ClientRectangle.Height ) / m_GraphicScreenProject.ScreenHeight;
            int  sy2 = ( ( m_Selection.Y + m_Selection.Height - m_GraphicScreenProject.ScreenOffsetY ) * pictureEditor.ClientRectangle.Height ) / m_GraphicScreenProject.ScreenHeight;

            TargetBuffer.Rectangle( sx1, sy1, sx2 - sx1, sy2 - sy1, selColor );
          }
          break;
      }

      // draw outside area
      int     fillWidth = 0;
      int     fillHeight = 0;

      //if ( m_GraphicScreenProject.Image.Width < pictureEditor.DisplayPage.Width )

      if ( m_GraphicScreenProject.ScreenWidth - m_GraphicScreenProject.ScreenOffsetX < pictureEditor.DisplayPage.Width )
      {
        fillWidth = pictureEditor.DisplayPage.Width - ( m_GraphicScreenProject.ScreenWidth - m_GraphicScreenProject.ScreenOffsetX );
      }
      if ( m_GraphicScreenProject.ScreenHeight - m_GraphicScreenProject.ScreenOffsetY < pictureEditor.DisplayPage.Height )
      {
        fillHeight = pictureEditor.DisplayPage.Height - ( m_GraphicScreenProject.ScreenHeight - m_GraphicScreenProject.ScreenOffsetY );
      }
      if ( ( fillWidth > 0 )
      &&   ( fillHeight > 0 ) )
      {
        // bottom right
        TargetBuffer.Box( ( pictureEditor.DisplayPage.Width - fillWidth ) * ( pictureEditor.ClientRectangle.Width / pictureEditor.DisplayPage.Width ),
                          ( pictureEditor.DisplayPage.Height - fillHeight ) * ( pictureEditor.ClientRectangle.Height / pictureEditor.DisplayPage.Height ),
                          pictureEditor.ClientRectangle.Width - ( pictureEditor.DisplayPage.Width - fillWidth ) * ( pictureEditor.ClientRectangle.Width / pictureEditor.DisplayPage.Width ),
                          pictureEditor.ClientRectangle.Height - ( pictureEditor.DisplayPage.Height - fillHeight ) * ( pictureEditor.ClientRectangle.Height / pictureEditor.DisplayPage.Height ),
                          selColor );
      }
      if ( fillWidth > 0 )
      {
        // right
        TargetBuffer.Box( ( pictureEditor.DisplayPage.Width - fillWidth ) * ( pictureEditor.ClientRectangle.Width / pictureEditor.DisplayPage.Width ),
                          0,
                          pictureEditor.ClientRectangle.Width - ( pictureEditor.DisplayPage.Width - fillWidth ) * ( pictureEditor.ClientRectangle.Width / pictureEditor.DisplayPage.Width ),
                          pictureEditor.ClientRectangle.Height,
                          selColor );
      }
      if ( fillHeight > 0 )
      {
        // bottom
        TargetBuffer.Box( 0,
                          ( pictureEditor.DisplayPage.Height - fillHeight ) * ( pictureEditor.ClientRectangle.Height / pictureEditor.DisplayPage.Height ),
                          pictureEditor.ClientRectangle.Width,
                          pictureEditor.ClientRectangle.Height - ( pictureEditor.DisplayPage.Height - fillHeight ) * ( pictureEditor.ClientRectangle.Height / pictureEditor.DisplayPage.Height ),
                          selColor );
      }
    }



    private void btnClearScreen_Click( object sender, EventArgs e )
    {
      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoGraphicScreenImageChange( m_GraphicScreenProject, this, 0, 0, m_GraphicScreenProject.ScreenWidth, m_GraphicScreenProject.ScreenHeight ) );
      m_GraphicScreenProject.Image.Box( 0, 0, m_GraphicScreenProject.ScreenWidth, m_GraphicScreenProject.ScreenHeight, 0 );
      Redraw();
      SetModified();
    }



    public void ColorValuesChanged()
    {
      UpdateCurrentColorMapping();
      comboBackground.SelectedIndex = m_GraphicScreenProject.Colors.BackgroundColor;
      checkMulticolor.Checked = m_GraphicScreenProject.MultiColor;
      comboMulticolor1.SelectedIndex = m_GraphicScreenProject.Colors.MultiColor1;
      comboMulticolor2.SelectedIndex = m_GraphicScreenProject.Colors.MultiColor2;
    }



  }
}

