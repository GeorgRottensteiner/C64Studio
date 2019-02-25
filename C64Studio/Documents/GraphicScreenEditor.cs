using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace C64Studio
{
  public partial class GraphicScreenEditor : BaseDocument
  {
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


    private int                         m_CurrentChar = 0;

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



    public GraphicScreenEditor( StudioCore Core )
    {
      this.Core = Core;
      DocumentInfo.Type = ProjectElement.ElementType.GRAPHIC_SCREEN;
      DocumentInfo.UndoManager.MainForm = Core.MainForm;
      m_IsSaveable = true;
      InitializeComponent();

      pictureEditor.DisplayPage.Create( 320, 200, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );
      charEditor.DisplayPage.Create( 8, 8, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );
      m_GraphicScreenProject.Image.Create( 320, 200, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );
      colorSelector.DisplayPage.Create( 16 * 8, 8, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );

      CustomRenderer.PaletteManager.ApplyPalette( pictureEditor.DisplayPage );
      CustomRenderer.PaletteManager.ApplyPalette( charEditor.DisplayPage );
      CustomRenderer.PaletteManager.ApplyPalette( m_GraphicScreenProject.Image );
      CustomRenderer.PaletteManager.ApplyPalette( colorSelector.DisplayPage );

      foreach ( C64Studio.Formats.GraphicScreenProject.ColorMappingTarget entry in System.Enum.GetValues( typeof( C64Studio.Formats.GraphicScreenProject.ColorMappingTarget ) ) )
      {
        if ( entry != Formats.GraphicScreenProject.ColorMappingTarget.ANY )
        {
          comboColorMappingTargets.Items.Add( GR.EnumHelper.GetDescription( entry ) );
        }
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

      e.DrawBackground();
      System.Drawing.Rectangle itemRect = new System.Drawing.Rectangle( e.Bounds.Left + 20, e.Bounds.Top, e.Bounds.Width - 20, e.Bounds.Height );
      if ( e.Index != -1 )
      {
        e.Graphics.FillRectangle( Types.ConstantData.Palette.ColorBrushes[e.Index], itemRect );
        if ( ( e.State & DrawItemState.Selected ) != 0 )
        {
          e.Graphics.DrawString( combo.Items[e.Index].ToString(), combo.Font, new System.Drawing.SolidBrush( System.Drawing.Color.White ), 3.0f, e.Bounds.Top + 1.0f );
        }
        else
        {
          e.Graphics.DrawString( combo.Items[e.Index].ToString(), combo.Font, new System.Drawing.SolidBrush( System.Drawing.Color.Black ), 3.0f, e.Bounds.Top + 1.0f );
        }
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
      int     charX = X / ( pictureEditor.ClientRectangle.Width / 40 ) + m_GraphicScreenProject.ScreenOffsetX;
      int     charY = Y / ( pictureEditor.ClientRectangle.Height / 25 ) + m_GraphicScreenProject.ScreenOffsetY;

      int     pixelX = ( X / ( pictureEditor.ClientRectangle.Width / pictureEditor.DisplayPage.Width ) ) + m_GraphicScreenProject.ScreenOffsetX;
      int     pixelY = ( Y / ( pictureEditor.ClientRectangle.Height / pictureEditor.DisplayPage.Height ) ) + m_GraphicScreenProject.ScreenOffsetY;

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

              charEditor.DisplayPage.DrawFromMemoryImage( m_GraphicScreenProject.Image, 0, 0, m_SelectedChar.X * 8, m_SelectedChar.Y * 8, 8, 8 );
              charEditor.Invalidate();

              if ( m_GraphicScreenProject.SelectedCheckType == C64Studio.Formats.GraphicScreenProject.CheckType.MULTICOLOR_BITMAP )
              {
                m_Chars[charX + charY * BlockWidth].Mode = C64Studio.Types.CharsetMode.MULTICOLOR;
                checkMulticolor.Checked = true;
              }
              else if ( m_GraphicScreenProject.SelectedCheckType == C64Studio.Formats.GraphicScreenProject.CheckType.MULTICOLOR_CHARSET )
              {
                checkMulticolor.Checked = ( m_Chars[charX + charY * BlockWidth].Mode == C64Studio.Types.CharsetMode.MULTICOLOR );
              }
              else
              {
                m_Chars[charX + charY * BlockWidth].Mode = C64Studio.Types.CharsetMode.HIRES;
                checkMulticolor.Checked = false;
              }
              comboCharColor.SelectedIndex = m_Chars[charX + charY * BlockWidth].Color;

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
            comboCharColor.SelectedIndex = m_Chars[charX + charY * BlockWidth].Color;
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

            if ( m_GraphicScreenProject.MultiColor )
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
                m_GraphicScreenProject.Image.Rectangle( x1, y1, x2 - x1 + 1, y2 - y1 + 1, m_CurrentColor );
                if ( m_GraphicScreenProject.MultiColor )
                {
                  m_GraphicScreenProject.Image.Line( x1 + 1, y1, x1 + 1, y2, m_CurrentColor );
                  m_GraphicScreenProject.Image.Line( x2 - 1, y1, x2 - 1, y2, m_CurrentColor );
                }
                break;
              case PaintTool.SELECT:
                m_Selection = new System.Drawing.Rectangle( x1, y1, x2 - x1 + 1, y2 - y1 + 1 );
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
      if ( ( Buttons & MouseButtons.Right ) != 0 )
      {
      }
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
      if ( m_SelectedChar.X != -1 )
      {
        if ( ( checkMulticolor.Checked )
        &&   ( m_Chars[m_SelectedChar.X + m_SelectedChar.Y * BlockWidth].Mode != C64Studio.Types.CharsetMode.MULTICOLOR ) )
        {
          m_Chars[m_SelectedChar.X + m_SelectedChar.Y * BlockWidth].Mode = C64Studio.Types.CharsetMode.MULTICOLOR;
          Modified = true;
        }
        else if ( ( !checkMulticolor.Checked )
        &&        ( m_Chars[m_SelectedChar.X + m_SelectedChar.Y * BlockWidth].Mode == C64Studio.Types.CharsetMode.MULTICOLOR ) )
        {
          m_Chars[m_SelectedChar.X + m_SelectedChar.Y * BlockWidth].Mode = C64Studio.Types.CharsetMode.HIRES;
          Modified = true;
        }
      }
      if ( m_GraphicScreenProject.MultiColor != checkMulticolor.Checked )
      {
        m_GraphicScreenProject.MultiColor = checkMulticolor.Checked;
        Modified = true;
      }
    }



    private void comboBackground_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( m_GraphicScreenProject.BackgroundColor != comboBackground.SelectedIndex )
      {
        m_GraphicScreenProject.BackgroundColor = comboBackground.SelectedIndex;
        Modified = true;
        pictureEditor.Invalidate();
      }
    }



    private void comboMulticolor1_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( m_GraphicScreenProject.MultiColor1 != comboMulticolor1.SelectedIndex )
      {
        m_GraphicScreenProject.MultiColor1 = comboMulticolor1.SelectedIndex;
        Modified = true;
        pictureEditor.Invalidate();
      }
    }



    private void comboMulticolor2_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( m_GraphicScreenProject.MultiColor2 != comboMulticolor2.SelectedIndex )
      {
        m_GraphicScreenProject.MultiColor2 = comboMulticolor2.SelectedIndex;
        Modified = true;
        pictureEditor.Invalidate();
      }
    }



    private void comboCharColor_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( m_SelectedChar.X != -1 )
      {
        m_Chars[m_SelectedChar.X + m_SelectedChar.Y * BlockWidth].Color = comboCharColor.SelectedIndex;
      }
    }



    private bool ImportImage( string Filename, GR.Image.FastImage IncomingImage, bool InsertAtSelectedLocation )
    {
      GR.Image.FastImage mappedImage = null;

      Types.MulticolorSettings   mcSettings = new Types.MulticolorSettings();
      mcSettings.BackgroundColor  = m_GraphicScreenProject.BackgroundColor;
      mcSettings.MultiColor1      = m_GraphicScreenProject.MultiColor1;
      mcSettings.MultiColor2      = m_GraphicScreenProject.MultiColor2;

      bool pasteAsBlock = false;
      if ( !Core.MainForm.ImportImage( Filename, IncomingImage, Types.GraphicType.BITMAP, mcSettings, out mappedImage, out mcSettings, out pasteAsBlock ) )
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

      if ( !InsertAtSelectedLocation )
      {
        if ( ( mappedImage.Width != m_GraphicScreenProject.Image.Width )
        ||   ( mappedImage.Height != m_GraphicScreenProject.Image.Height ) )
        {
          Dialogs.DlgImportImageResize    dlg = new C64Studio.Dialogs.DlgImportImageResize( mappedImage.Width, mappedImage.Height, m_GraphicScreenProject.Image.Width, m_GraphicScreenProject.Image.Height );

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

      if ( InsertAtSelectedLocation )
      {
        mappedImage.DrawTo( m_GraphicScreenProject.Image, m_SelectedChar.X * 8, m_SelectedChar.Y * 8 );
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

      m_GraphicScreenProject.Image = m_GraphicScreenProject.Image.GetImage( 0, 0, Width, Height );

      m_ErrornousChars = new bool[( Width + 7 ) / 8, ( Height + 7 ) / 8];
      m_Chars.Clear();
      for ( int j = 0; j < BlockHeight; ++j )
      {
        for ( int i = 0; i < BlockWidth; ++i )
        {
          m_Chars.Add( new Formats.CharData() );
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
        ImportImage( filename, null, false );
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

      comboBackground.SelectedIndex   = m_GraphicScreenProject.BackgroundColor;
      checkMulticolor.Checked = m_GraphicScreenProject.MultiColor;
      comboMulticolor1.SelectedIndex = m_GraphicScreenProject.MultiColor1;
      comboMulticolor2.SelectedIndex = m_GraphicScreenProject.MultiColor2;
      comboCheckType.SelectedIndex = (int)m_GraphicScreenProject.SelectedCheckType;

      SetScreenSize( m_GraphicScreenProject.Image.Width, m_GraphicScreenProject.Image.Height );
      AdjustScrollbars();

      screenHScroll.Value = m_GraphicScreenProject.ScreenOffsetX;
      screenVScroll.Value = m_GraphicScreenProject.ScreenOffsetY;

      Redraw();
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



    private bool SaveProject( bool SaveAs )
    {
      string saveFilename = DocumentInfo.FullPath;

      if ( ( String.IsNullOrEmpty( DocumentInfo.DocumentFilename ) )
      ||   ( SaveAs ) )
      {
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
        Name = DocumentInfo.DocumentFilename;
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
        DialogResult doSave = MessageBox.Show( "There are unsaved changes in your graphic screen. Save now?", "Save changes?", MessageBoxButtons.YesNoCancel );
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



    private string ToASMData( GR.Memory.ByteBuffer Data )
    {
      int wrapByteCount = GR.Convert.ToI32( editWrapByteCount.Text );
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

                  int     colorIndex = m_GraphicScreenProject.BackgroundColor;

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
      Modified = true;
    }



    private void btnImportCharsetFromFile_Click( object sender, EventArgs e )
    {
      string filename;

      if ( !OpenFile( "Import from Image", Types.Constants.FILEFILTER_IMAGE_FILES + Types.Constants.FILEFILTER_ALL, out filename ) )
      {
        return;
      }
      ImportImage( filename, null, false );
    }



    private void CopyImageToClipboard()
    {
      GR.Memory.ByteBuffer      dibData = m_GraphicScreenProject.Image.CreateHDIBAsBuffer();

      System.IO.MemoryStream    ms = dibData.MemoryStream();

      Clipboard.SetData( "DeviceIndependentBitmap", ms );
    }



    private void CopySelectedImageToClipboard()
    {
      if ( m_SelectedChar.X == -1 )
      {
        CopyImageToClipboard();
        return;
      }

      GR.Memory.ByteBuffer      dibData = m_GraphicScreenProject.Image.GetImage( m_SelectedChar.X * 8, m_SelectedChar.Y * 8, 8, 8 ).CreateHDIBAsBuffer();

      System.IO.MemoryStream    ms = dibData.MemoryStream();

      Clipboard.SetData( "DeviceIndependentBitmap", ms );
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

      ImportImage( null, imgClip, true );
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
        case PaintTool.VALIDATE:
          for ( int j = 0; j < BlockHeight; ++j )
          {
            for ( int i = 0; i < BlockWidth; ++i )
            {
              if ( m_ErrornousChars[i, j] )
              {
                for ( int x = 0; x < 8; ++x )
                {
                  pictureEditor.DisplayPage.SetPixel( i * 8 + x - m_GraphicScreenProject.ScreenOffsetX * 8, j * 8 - m_GraphicScreenProject.ScreenOffsetY * 8, (uint)( 1 + ( x & 1 ) * 15 ) );
                  pictureEditor.DisplayPage.SetPixel( i * 8 - m_GraphicScreenProject.ScreenOffsetX * 8, j * 8 + x - m_GraphicScreenProject.ScreenOffsetY * 8, (uint)( 1 + ( x & 1 ) * 15 ) );
                }
              }
            }
          }
          if ( m_SelectedChar.X != -1 )
          {
            for ( int x = 0; x < 8; ++x )
            {
              pictureEditor.DisplayPage.SetPixel( m_SelectedChar.X * 8 + x - m_GraphicScreenProject.ScreenOffsetX * 8, m_SelectedChar.Y * 8 - m_GraphicScreenProject.ScreenOffsetY * 8, 16 );
              pictureEditor.DisplayPage.SetPixel( m_SelectedChar.X * 8 - m_GraphicScreenProject.ScreenOffsetX * 8, m_SelectedChar.Y * 8 + x - m_GraphicScreenProject.ScreenOffsetY * 8, 16 );
            }
          }
          break;
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
          if ( m_Selection.Width > 0 )
          {
            pictureEditor.DisplayPage.Rectangle( m_Selection.X, m_Selection.Y, m_Selection.Width, m_Selection.Height, 16 );
          }
          break;
      }
      // color for area outside active image
      if ( m_GraphicScreenProject.Image.Width < pictureEditor.DisplayPage.Width )
      {
        pictureEditor.DisplayPage.Box( m_GraphicScreenProject.Image.Width, 0, pictureEditor.DisplayPage.Width - m_GraphicScreenProject.Image.Width, m_GraphicScreenProject.Image.Height, 16 );
      }
      if ( m_GraphicScreenProject.Image.Height < pictureEditor.DisplayPage.Height )
      {
        pictureEditor.DisplayPage.Box( 0, m_GraphicScreenProject.Image.Height, m_GraphicScreenProject.Image.Width, pictureEditor.DisplayPage.Height - m_GraphicScreenProject.Image.Height, 16 );
      }
      if ( ( m_GraphicScreenProject.Image.Width < pictureEditor.DisplayPage.Width )
      &&   ( m_GraphicScreenProject.Image.Height < pictureEditor.DisplayPage.Height ) )
      {
        pictureEditor.DisplayPage.Box( m_GraphicScreenProject.Image.Width,
                                       m_GraphicScreenProject.Image.Height,
                                       pictureEditor.DisplayPage.Width - m_GraphicScreenProject.Image.Width,
                                       pictureEditor.DisplayPage.Height - m_GraphicScreenProject.Image.Height,
                                       16 );
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
      /*
      if ( pictureEditor.DisplayPage.Width < pictureEditor.ClientSize.Width )
      {
        e.Graphics.FillRectangle( System.Drawing.SystemBrushes.ControlLight, pictureEditor.DisplayPage.Width, 0, pictureEditor.ClientSize.Width - pictureEditor.DisplayPage.Width, pictureEditor.ClientSize.Height );
      }
      if ( pictureEditor.DisplayPage.Height < pictureEditor.ClientSize.Height )
      {
        e.Graphics.FillRectangle( System.Drawing.SystemBrushes.ControlLight, 0, pictureEditor.DisplayPage.Height, pictureEditor.DisplayPage.Width, pictureEditor.ClientSize.Height - pictureEditor.DisplayPage.Height );
      }
      if ( ( pictureEditor.DisplayPage.Width < pictureEditor.ClientSize.Width )
      &&   ( pictureEditor.DisplayPage.Height < pictureEditor.ClientSize.Height ) )
      {
        e.Graphics.FillRectangle( System.Drawing.SystemBrushes.ControlLight, 
                                  pictureEditor.DisplayPage.Width, 
                                  pictureEditor.DisplayPage.Height,
                                  pictureEditor.ClientSize.Width - pictureEditor.DisplayPage.Width, 
                                  pictureEditor.ClientSize.Height - pictureEditor.DisplayPage.Height );
      }*/
    }



    private bool CheckCharBox( Formats.CharData cd, int X, int Y, bool CheckForMC )
    {
      // Match image data
      int chosenCharColor = -1;

      cd.Replacement = null;
      cd.Index = 0;

      // clear data
      for ( int i = 0; i < 8; ++i )
      {
        cd.Data.SetU8At( i, 0 );
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
              if ( colorIndex == m_GraphicScreenProject.BackgroundColor )
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
            && ( i != m_GraphicScreenProject.BackgroundColor ) )
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
        //||   ( numColors == 2 ) )
        {
          // eligible for single color
          isMultiColor = false;
          int usedFreeColor = -1;
          for ( int i = 0; i < 16; ++i )
          {
            if ( usedColor[i] )
            {
              if ( i != m_GraphicScreenProject.BackgroundColor )
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

              if ( ColorIndex != m_GraphicScreenProject.BackgroundColor )
              {
                BitPattern = 1;
              }

              // noch nicht verwendete Farbe
              if ( BitPattern == 1 )
              {
                chosenCharColor = ColorIndex;
              }
              cd.Data.SetU8At( y + x / 8, (byte)( cd.Data.ByteAt( y + x / 8 ) | ( BitPattern << ( ( 7 - ( x % 8 ) ) ) ) ) );
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
              if ( ( i == m_GraphicScreenProject.MultiColor1 )
              ||   ( i == m_GraphicScreenProject.MultiColor2 )
              ||   ( i == m_GraphicScreenProject.BackgroundColor ) )
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

              if ( ColorIndex == m_GraphicScreenProject.BackgroundColor )
              {
                BitPattern = 0x00;
              }
              else if ( ColorIndex == m_GraphicScreenProject.MultiColor1 )
              {
                BitPattern = 0x01;
              }
              else if ( ColorIndex == m_GraphicScreenProject.MultiColor2 )
              {
                BitPattern = 0x02;
              }
              else
              {
                // noch nicht verwendete Farbe
                chosenCharColor = usedFreeColor;
                BitPattern = 0x03;
              }
              cd.Data.SetU8At( y + x / 4, (byte)( cd.Data.ByteAt( y + x / 4 ) | ( BitPattern << ( ( 3 - ( x % 4 ) ) * 2 ) ) ) );
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
      cd.Color = chosenCharColor;
      if ( ( isMultiColor )
      &&   ( chosenCharColor < 8 ) )
      {
        cd.Color = chosenCharColor + 8;
      }
      cd.Mode = CheckForMC ? Types.CharsetMode.MULTICOLOR : C64Studio.Types.CharsetMode.HIRES;
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
          if ( m_Chars[index1].Data.Compare( m_Chars[index2].Data ) == 0 )
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
            if ( m_Chars[i + j * BlockWidth].Mode == C64Studio.Types.CharsetMode.MULTICOLOR )
            {
              m_Chars[i + j * BlockWidth].Error = "Char is multi color";
              m_ErrornousChars[i, j] = true;
              foundError = true;
            }
            else
            {
              m_ErrornousChars[i, j] = false;
            }
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
          if ( m_Chars[index1].Data.Compare( m_Chars[index2].Data ) == 0 )
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

              if ( colorIndex != m_GraphicScreenProject.BackgroundColor )
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
                if ( colorIndex != m_GraphicScreenProject.BackgroundColor )
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
        aChar.Mode = C64Studio.Types.CharsetMode.HIRES;
        aChar.Color = 0;
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
        if ( charToEdit.Mode == C64Studio.Types.CharsetMode.MULTICOLOR )
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
        if ( charToEdit.Mode == C64Studio.Types.CharsetMode.MULTICOLOR )
        {
          byte    colorToSet = (byte)m_GraphicScreenProject.BackgroundColor;
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
          byte  colorToSet = (byte)m_GraphicScreenProject.BackgroundColor;

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

      m_GraphicScreenProject.ImageToHiresBitmapData( m_Chars, m_ErrornousChars, out bitmapData, out screenChar, out screenColor );

      // export data
      string result = ";bitmap data" + System.Environment.NewLine + ToASMData( bitmapData );

      result += System.Environment.NewLine + System.Environment.NewLine + ";screen ram data" + System.Environment.NewLine + ToASMData( screenChar );
      result += System.Environment.NewLine + System.Environment.NewLine + ";screen color data" + System.Environment.NewLine + ToASMData( screenColor );

      editDataExport.Text = result;
    }



    private void ExportHiresBitmap( int StartLine, int LineOffset )
    {
      GR.Memory.ByteBuffer screenChar;
      GR.Memory.ByteBuffer screenColor;
      GR.Memory.ByteBuffer bitmapData;

      m_GraphicScreenProject.ImageToHiresBitmapData( m_Chars, m_ErrornousChars, out bitmapData, out screenChar, out screenColor );

      editDataExport.Text = Util.ToBASICData( bitmapData + screenChar + screenColor, StartLine, LineOffset );
    }



    private void ExportMCBitmap()
    {
      GR.Memory.ByteBuffer              screenChar;
      GR.Memory.ByteBuffer              screenColor;
      GR.Memory.ByteBuffer              bitmapData;
      //Dictionary<int,byte>              forcedPattern = new Dictionary<int, byte>();

      m_GraphicScreenProject.ImageToMCBitmapData( m_GraphicScreenProject.ColorMapping, m_Chars, m_ErrornousChars, out bitmapData, out screenChar, out screenColor );

      // export data
      string    result = ";bitmap data" + System.Environment.NewLine + ToASMData( bitmapData );

      result += System.Environment.NewLine + System.Environment.NewLine + ";screen ram data" + System.Environment.NewLine + ToASMData( screenChar );
      result += System.Environment.NewLine + System.Environment.NewLine + ";screen color data" + System.Environment.NewLine + ToASMData( screenColor );

      editDataExport.Text = result;
    }



    private void ExportMCBitmap( int StartLine, int LineOffset )
    {
      GR.Memory.ByteBuffer              screenChar;
      GR.Memory.ByteBuffer              screenColor;
      GR.Memory.ByteBuffer              bitmapData;
      //Dictionary<int,byte>              forcedPattern = new Dictionary<int, byte>();

      m_GraphicScreenProject.ImageToMCBitmapData( m_GraphicScreenProject.ColorMapping, m_Chars, m_ErrornousChars, out bitmapData, out screenChar, out screenColor );

      editDataExport.Text = Util.ToBASICData( bitmapData + screenChar + screenColor, StartLine, LineOffset );
    }



    private void ExportHiResCharset( bool ExportScreenAssembly )
    {
      // export possible
      Formats.CharsetProject projectToExport = new C64Studio.Formats.CharsetProject();
      int curCharIndex = 0;

      projectToExport.MultiColor1 = m_GraphicScreenProject.MultiColor1;
      projectToExport.MultiColor2 = m_GraphicScreenProject.MultiColor2;
      projectToExport.BackgroundColor = m_GraphicScreenProject.BackgroundColor;
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
            screenColorData.SetU8At( x + y * BlockWidth, (byte)charUsed.Color );
          }
        }
        string screenDataASM = ToASMData( screenCharData );
        string colorDataASM = ToASMData( screenColorData );

        editDataExport.Text = ";screen char data" + System.Environment.NewLine + screenDataASM + System.Environment.NewLine + ";screen color data" + System.Environment.NewLine + colorDataASM;
      }
    }



    private void ExportHiResCharset( int StartLine, int LineOffset )
    {
      // export possible
      Formats.CharsetProject projectToExport = new C64Studio.Formats.CharsetProject();
      int curCharIndex = 0;

      projectToExport.MultiColor1 = m_GraphicScreenProject.MultiColor1;
      projectToExport.MultiColor2 = m_GraphicScreenProject.MultiColor2;
      projectToExport.BackgroundColor = m_GraphicScreenProject.BackgroundColor;
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
          screenColorData.SetU8At( x + y * BlockWidth, (byte)charUsed.Color );
        }
      }

      editDataExport.Text = Util.ToBASICData( screenCharData + screenColorData, StartLine, LineOffset );
    }



    private void ExportMCCharset( bool ExportScreenAssembly )
    {
      // export possible
      Formats.CharsetProject projectToExport = new C64Studio.Formats.CharsetProject();
      int curCharIndex = 0;

      projectToExport.MultiColor1 = m_GraphicScreenProject.MultiColor1;
      projectToExport.MultiColor2 = m_GraphicScreenProject.MultiColor2;
      projectToExport.BackgroundColor = m_GraphicScreenProject.BackgroundColor;
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
            screenColorData.SetU8At( x + y * BlockWidth, (byte)charUsed.Color );
          }
        }
        string screenDataASM = ToASMData( screenCharData );
        string colorDataASM = ToASMData( screenColorData );

        editDataExport.Text = ";screen char data" + System.Environment.NewLine + screenDataASM + System.Environment.NewLine + ";screen color data" + System.Environment.NewLine + colorDataASM;
      }
    }



    private void ExportMCCharset( int StartLine, int LineOffset )
    {
      // export possible
      Formats.CharsetProject projectToExport = new C64Studio.Formats.CharsetProject();
      int curCharIndex = 0;

      projectToExport.MultiColor1 = m_GraphicScreenProject.MultiColor1;
      projectToExport.MultiColor2 = m_GraphicScreenProject.MultiColor2;
      projectToExport.BackgroundColor = m_GraphicScreenProject.BackgroundColor;
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
          screenColorData.SetU8At( x + y * BlockWidth, (byte)charUsed.Color );
        }
      }

      editDataExport.Text = Util.ToBASICData( screenCharData + screenColorData, StartLine, LineOffset );
    }



    private void UsedCharsToClipboard()
    {
      // export possible
      Formats.CharsetProject projectToExport = new C64Studio.Formats.CharsetProject();
      int curCharIndex = 0;

      projectToExport.MultiColor1     = m_GraphicScreenProject.MultiColor1;
      projectToExport.MultiColor2     = m_GraphicScreenProject.MultiColor2;
      projectToExport.BackgroundColor = m_GraphicScreenProject.BackgroundColor;
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
      for ( int i = 0; i < curCharIndex; ++i )
      {
        // delta in indices
        dataSelection.AppendI32( ( i == 0 ) ? 0 : 1 );

        dataSelection.AppendI32( (int)projectToExport.Characters[i].Mode );
        dataSelection.AppendI32( projectToExport.Characters[i].Color );
        dataSelection.AppendU32( 8 );
        dataSelection.AppendU32( 8 );
        dataSelection.AppendU32( projectToExport.Characters[i].Data.Length );
        dataSelection.Append( projectToExport.Characters[i].Data );
        dataSelection.AppendI32( i );
      }

      DataObject dataObj = new DataObject();

      dataObj.SetData( "C64Studio.ImageList", false, dataSelection.MemoryStream() );

      GR.Memory.ByteBuffer      dibData = projectToExport.Characters[m_CurrentChar].Image.CreateHDIBAsBuffer();

      System.IO.MemoryStream    ms = dibData.MemoryStream();

      // WTF - SetData requires streams, NOT global data (HGLOBAL)
      dataObj.SetData( "DeviceIndependentBitmap", ms );

      Clipboard.SetDataObject( dataObj, true );
    }



    private void comboCheckType_SelectedIndexChanged( object sender, EventArgs e )
    {
      comboExportType.SelectedIndex = comboCheckType.SelectedIndex;
      m_GraphicScreenProject.SelectedCheckType = (C64Studio.Formats.GraphicScreenProject.CheckType)comboCheckType.SelectedIndex;
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
          m_GraphicScreenProject.ImageToHiresBitmapData( m_Chars, m_ErrornousChars, out bitmapData, out screenChar, out screenColor );
          break;
        case 1:
          // MC bitmap
          m_GraphicScreenProject.ImageToMCBitmapData( m_GraphicScreenProject.ColorMapping, m_Chars, m_ErrornousChars, out bitmapData, out screenChar, out screenColor );
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
      GR.IO.File.WriteAllBytes( saveDlg.FileName, exportData );
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

      bool    forceMulticolor = true;

      switch ( (Formats.GraphicScreenProject.CheckType)comboCheckType.SelectedIndex )
      {
        case C64Studio.Formats.GraphicScreenProject.CheckType.HIRES_BITMAP:
        case C64Studio.Formats.GraphicScreenProject.CheckType.HIRES_CHARSET:
          forceMulticolor = false;
          break;
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

        project.BackgroundColor = m_GraphicScreenProject.BackgroundColor;
        project.MultiColor1     = m_GraphicScreenProject.MultiColor1;
        project.MultiColor2     = m_GraphicScreenProject.MultiColor2;

        project.Mode            = forceMulticolor ? Types.CharsetMode.MULTICOLOR : Types.CharsetMode.HIRES;
        charset.BackgroundColor = project.BackgroundColor;
        charset.MultiColor1     = project.MultiColor1;
        charset.MultiColor2     = project.MultiColor2;
        charset.BGColor4        = project.BGColor4;

        for ( int i = 0; i < m_Chars.Count; ++i )
        {
          project.Chars[i] = (ushort)( ( m_Chars[i].Index & 0xff ) | ( m_Chars[i].Color << 8 ) );
          if ( m_Chars[i].Replacement == null )
          {
            charset.Characters[m_Chars[i].Index].Data = m_Chars[i].Data;
            charset.Characters[m_Chars[i].Index].Mode = forceMulticolor ? Types.CharsetMode.MULTICOLOR : C64Studio.Types.CharsetMode.HIRES;
          }
          else
          {
            project.Chars[i] = (ushort)( ( m_Chars[i].Replacement.Index & 0xff ) | ( m_Chars[i].Color << 8 ) );
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
      charEditor.DisplayPage.DrawFromMemoryImage( m_GraphicScreenProject.Image, 0, 0, m_SelectedChar.X * 8, m_SelectedChar.Y * 8, 8, 8 );
      charEditor.Invalidate();
      Redraw();
    }



    private void btnColorMappingRemove_Click( object sender, EventArgs e )
    {

    }



    private void listColorMappingTargets_SelectedIndexChanged( object sender, ListViewItem Item )
    {
      UpdateColorMappingButtons();
    }



    private void listColorMappingColors_SelectedIndexChanged( object sender, EventArgs e )
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
      System.Drawing.Rectangle itemRect = new System.Drawing.Rectangle( e.Bounds.Left + 20, e.Bounds.Top, e.Bounds.Width - 20, e.Bounds.Height );
      if ( e.Index != -1 )
      {
        e.Graphics.FillRectangle( Types.ConstantData.Palette.ColorBrushes[e.Index], itemRect );
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



    private ListViewItem listColorMappingTargets_AddingItem( object sender )
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
      m_GraphicScreenProject.ColorMapping[sourceColor].Insert( m_GraphicScreenProject.ColorMapping[sourceColor].Count - 1, targetIndex );

      var newItem = new ListViewItem( GR.EnumHelper.GetDescription( targetIndex ) );

      Modified = true;
      UpdateColorMappingButtons();
      return newItem;
    }



    private void listColorMappingTargets_ItemRemoved( object sender, ListViewItem Item )
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

      foreach ( C64Studio.Formats.GraphicScreenProject.ColorMappingTarget entry in System.Enum.GetValues( typeof( C64Studio.Formats.GraphicScreenProject.ColorMappingTarget ) ) )
      {
        if ( GR.EnumHelper.GetDescription( entry ) == Item.Text )
        {
          m_GraphicScreenProject.ColorMapping[sourceColor].Remove( entry );
          break;
        }
      }
    }



    private void listColorMappingTargets_ItemMoved( object sender, ListViewItem Item1, ListViewItem Item2 )
    {
      int     sourceColor = listColorMappingColors.SelectedIndex;
      if ( sourceColor == -1 )
      {
        return;
      }

      m_GraphicScreenProject.ColorMapping[sourceColor].Clear();

      foreach ( ListViewItem item in listColorMappingTargets.Items )
      {
        m_GraphicScreenProject.ColorMapping[sourceColor].Add( ColorMappingFromItem( item ) );
      }
    }



    private C64Studio.Formats.GraphicScreenProject.ColorMappingTarget ColorMappingFromItem( ListViewItem Item )
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



    private bool listColorMappingTargets_MovingItem( object sender, ListViewItem Item1, ListViewItem Item2 )
    {
      var     colorMapping1 = ColorMappingFromItem( Item1 );
      var     colorMapping2 = ColorMappingFromItem( Item2 );

      if ( ( colorMapping1 == Formats.GraphicScreenProject.ColorMappingTarget.ANY )
      ||   ( colorMapping2 == Formats.GraphicScreenProject.ColorMappingTarget.ANY ) )
      {
        return false;
      }
      return true;
    }



    private void listColorMappingTargets_ItemAdded( object sender, ListViewItem Item )
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

      switch ( (ExportDataType)comboExportType.SelectedIndex )
      {
        case ExportDataType.HIRES_BITMAP:
          ExportHiresBitmap( startLine, lineOffset );
          break;
        case ExportDataType.MULTICOLOR_BITMAP:
          ExportMCBitmap( startLine, lineOffset );
          break;
        case ExportDataType.HIRES_CHARSET:
        case ExportDataType.HIRES_CHARSET_SCREEN_ASSEMBLY:
          ExportHiResCharset( startLine, lineOffset );
          break;
        case ExportDataType.MULTICOLOR_CHARSET:
        case ExportDataType.MULTICOLOR_CHARSET_SCREEN_ASSEMBLY:
          ExportMCCharset( startLine, lineOffset );
          break;
        case ExportDataType.CHARACTERS_TO_CLIPBOARD:
          UsedCharsToClipboard();
          break;
      }
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

      m_GraphicScreenProject.ImageToMCBitmapData( m_GraphicScreenProject.ColorMapping, m_Chars, m_ErrornousChars, out bitmapData, out screenChar, out screenColor );

      System.Drawing.Imaging.ImageFormat formatToSave = System.Drawing.Imaging.ImageFormat.Png;

      if ( ( extension == ".KLA" )
      ||   ( extension == ".KOA" ) )
      {
        var koalaData = C64Studio.Converter.KoalaToBitmap.KoalaFromBitmap( bitmapData, screenChar, screenColor, (byte)m_GraphicScreenProject.BackgroundColor );

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


  }
}

