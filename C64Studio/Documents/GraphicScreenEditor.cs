using GR.Image;
using GR.Memory;
using RetroDevStudio.Controls;
using RetroDevStudio.Converter;
using RetroDevStudio.Formats;
using RetroDevStudio.Properties;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Windows.Forms;



namespace RetroDevStudio.Documents
{
  public partial class GraphicScreenEditor : BaseDocument, IDisposable
  {
    public enum ImageInsertionMode
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
      DRAW_FREE_LINE,
      DRAW_LINE,
      DRAW_RECTANGLE,
      DRAW_BOX,
      FLOOD_FILL,
      SELECT,
      VALIDATE
    }

    private bool[,]                     m_ErrornousChars = new bool[40, 25];

    private bool                        m_ButtonReleased = false;

    private byte                        m_CurrentColor = 1;

    private List<Formats.CharData>      m_Chars = new List<Formats.CharData>();
    private System.Drawing.Point        m_SelectedChar = new System.Drawing.Point( -1, -1 );

    public Formats.GraphicScreenProject m_GraphicScreenProject = new RetroDevStudio.Formats.GraphicScreenProject();

    private PaintTool                   m_PaintTool = PaintTool.VALIDATE;

    private System.Drawing.Font         m_DefaultOutputFont = null;

    private System.Drawing.Point        m_DragStartPoint = new System.Drawing.Point( -1, -1 );
    private System.Drawing.Point        m_DragCurrentPoint;
    private System.Drawing.Rectangle    m_Selection = new System.Drawing.Rectangle( 0, 0, 0, 0 );

    private bool                        m_SelectionFloating = false;
    private GR.Image.IImage             m_SelectionFloatingImage = null;
    private System.Drawing.Point        m_SelectionFloatingPos = new System.Drawing.Point( 0, 0 );

    private int                         m_ZoomFactor = 1;
    private bool                        m_DragViewModeActiveBySpace = false;
    private bool                        m_DragViewModeActiveByPressedMiddleButton = false;
    private bool                        m_DragView = false;
    private System.Drawing.Point        m_DragPoint = new System.Drawing.Point();

    private System.Drawing.Point        m_LastPaintedPixelPos = new System.Drawing.Point();

    private System.Drawing.Point        m_LastPixelUnderMouse = new Point();

    private ColorChooserBase            _ColorChooserDlg = null;
    private ColorPickerGraphicBase      _colorPickerDlg = null;

    private ImportGraphicScreenFormBase _ImportForm = null;
    private ExportGraphicScreenFormBase _ExportForm = null;

    private int                         _distanceBetweenChooserAndPickerDialog = 0;



    public GraphicScreenEditor( StudioCore Core )
    {
      this.Core = Core;
      DocumentInfo.Type = ProjectElement.ElementType.GRAPHIC_SCREEN;
      DocumentInfo.UndoManager.MainForm = Core.MainForm;
      m_IsSaveable = true;
      InitializeComponent();

      m_DefaultOutputFont = editExportOutput.Font;

      GR.Image.DPIHandler.ResizeControlsForDPI( this );

      _distanceBetweenChooserAndPickerDialog = panelColorSettings.Top - panelColorChooser.Bottom;

      pictureEditor.DisplayPage.Create( 320, 200, GR.Drawing.PixelFormat.Format8bppIndexed );
      charEditor.DisplayPage.Create( 8, 8, GR.Drawing.PixelFormat.Format8bppIndexed );
      m_GraphicScreenProject.Image.Create( 320, 200, GR.Drawing.PixelFormat.Format8bppIndexed );

      PaletteManager.ApplyPalette( pictureEditor.DisplayPage );
      PaletteManager.ApplyPalette( charEditor.DisplayPage );
      PaletteManager.ApplyPalette( m_GraphicScreenProject.Image );

      UpdateColorMappingOptions();

      for ( int i = 0; i < 16; ++i )
      {
        listColorMappingColors.Items.Add( i.ToString( "d2" ) );
      }

      comboColorMappingTargets.SelectedIndex = 0;

      foreach ( Formats.GraphicScreenProject.CheckType checkType in System.Enum.GetValues( typeof( Formats.GraphicScreenProject.CheckType ) ) )
      {
        comboCheckType.Items.Add( GR.EnumHelper.GetDescription( checkType ) );
      }
      comboCheckType.SelectedIndex = 1;

      SetCharCheckList();

      editScreenWidth.Text = "320";
      editScreenHeight.Text = "200";
      SetScreenSize( 320, 200 );

      Core.MainForm.ApplicationEvent += new MainForm.ApplicationEventHandler( MainForm_ApplicationEvent );

      comboImportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "from image", typeof( ImportGraphicScreenFromImage ) ) );
      comboImportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "from Koala file (.prg)", typeof( ImportGraphicScreenFromKoalaFile ) ) );
      comboImportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "from MiniPaint (VIC20) (.prg)", typeof( ImportGraphicScreenFromMiniPaint ) ) );
      comboImportMethod.SelectedIndex = 0;

      comboExportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "as assembly", typeof( ExportGraphicScreenAsAssembly ) ) );
      comboExportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "to binary file", typeof( ExportGraphicScreenAsBinaryFile ) ) );
      comboExportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "as BASIC Data statements", typeof( ExportGraphicScreenAsBASICData ) ) );
      comboExportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "as BASIC PRINT code (convert to charset)", typeof( ExportGraphicScreenAsBASIC ) ) );
      comboExportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "to image file", typeof( ExportGraphicScreenAsImage ) ) );
      comboExportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "to charset project file", typeof( ExportGraphicScreenAsCharsetFile ) ) );
      comboExportMethod.SelectedIndex = 0;

      pictureEditor.KeyUp += PictureEditor_KeyUp;
      pictureEditor.LostFocus += PictureEditor_LostFocus;
      pictureEditor.MouseWheel += PictureEditor_MouseWheel;
      ChangeColorChooserDialog();
      ChangeColorPickerDialog();

      UpdateCursorInfo();
    }



    private void ChangeColorPickerDialog()
    {
      if ( _colorPickerDlg != null )
      {
        panelColorChooser.Controls.Remove( _colorPickerDlg );
        _colorPickerDlg.Dispose();
        _colorPickerDlg = null;
      }

      switch ( m_GraphicScreenProject.SelectedCheckType )
      {
        case GraphicScreenProject.CheckType.HIRES_CHARSET:
        case GraphicScreenProject.CheckType.HIRES_BITMAP:
        case GraphicScreenProject.CheckType.MULTICOLOR_BITMAP:
        case GraphicScreenProject.CheckType.MULTICOLOR_CHARSET:
          _colorPickerDlg = new ColorPickerGraphicCommodore( Core, (byte)m_CurrentColor );
          break;
        case GraphicScreenProject.CheckType.VIC20_CHARSET:
        case GraphicScreenProject.CheckType.VIC20_CHARSET_8X16:
          _colorPickerDlg = new ColorPickerGraphicCommodoreVIC20( Core, (byte)m_CurrentColor );
          break;
        case GraphicScreenProject.CheckType.MEGA65_BITMAP:
          _colorPickerDlg = new ColorPickerGraphicFullPalette( Core, (byte)m_CurrentColor );
          _colorPickerDlg.UpdatePalette( m_GraphicScreenProject.Colors.Palette );
          break;

        /*
                case TextCharMode.X16_HIRES:
                  _colorPickerDlg = new ColorPickerX16( Core, m_CharsetScreen.CharSet, m_CurrentChar, (byte)m_CurrentColor );
                  break;
                case TextCharMode.NES:
                  _colorPickerDlg = new ColorPickerNES( Core, m_CharsetScreen.CharSet, m_CurrentChar, (byte)m_CurrentColor );
                  break;
                case TextCharMode.VIC20_8X16:
                  _colorPickerDlg = new ColorPickerCommodoreVIC20X16( Core, m_CharsetScreen.CharSet, m_CurrentChar, (byte)m_CurrentColor );
                  break;
                case TextCharMode.MEGA65_HIRES:
                case TextCharMode.MEGA65_NCM:
                case TextCharMode.MEGA65_FCM:
                case TextCharMode.MEGA65_ECM:
                case TextCharMode.MEGA65_FCM_16BIT:
                  _colorPickerDlg = new ColorPickerMega65_32( Core, m_CharsetScreen.CharSet, m_CurrentChar, (byte)m_CurrentColor );
                  break;*/
        default:
          _colorPickerDlg = new ColorPickerGraphicCommodore( Core, (byte)m_CurrentColor );
          break;
      }
      _colorPickerDlg.SelectedColorChanged    += _ColorChooserDlg_SelectedColorChanged;
      _colorPickerDlg.PaletteModified         += _colorPickerDlg_PaletteModified;
      _colorPickerDlg.Redraw();
      panelColorChooser.Controls.Add( _colorPickerDlg );
      panelColorChooser.Size = _colorPickerDlg.Size;

      panelColorSettings.Top = panelColorChooser.Bottom + _distanceBetweenChooserAndPickerDialog;
    }



    private void _colorPickerDlg_PaletteModified( Palette palette )
    {
      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoGraphicScreenValuesChange( m_GraphicScreenProject, this ) );

      m_GraphicScreenProject.Colors.Palette = palette;
      PaletteManager.ApplyPalette( m_GraphicScreenProject.Image, palette );
      PaletteManager.ApplyPalette( pictureEditor.DisplayPage, palette );
      PaletteManager.ApplyPalette( charEditor.DisplayPage, palette );
      Redraw();
      charEditor.Invalidate();
    }



    private void _ColorChooserDlg_SelectedColorChanged( ushort Color )
    {
      m_CurrentColor = (byte)Color;
    }



    private void PictureEditor_MouseWheel( object sender, MouseEventArgs e )
    {
      if ( e.Delta > 0 )
      {
        ZoomIn();
      }
      else
      {
        ZoomOut();
      }
    }



    private void ChangeColorChooserDialog()
    {
      if ( _ColorChooserDlg != null )
      {
        panelColorSettings.Controls.Remove( _ColorChooserDlg );
        _ColorChooserDlg.Dispose();
        _ColorChooserDlg = null;
      }

      switch ( m_GraphicScreenProject.SelectedCheckType )
      {
        case Formats.GraphicScreenProject.CheckType.HIRES_CHARSET:
        case Formats.GraphicScreenProject.CheckType.MULTICOLOR_BITMAP:
          _ColorChooserDlg = new ColorChooserCommodoreHiRes( Core, m_GraphicScreenProject.Colors );
          break;
        case Formats.GraphicScreenProject.CheckType.MULTICOLOR_CHARSET:
          _ColorChooserDlg = new ColorChooserCommodoreMultiColor( Core, m_GraphicScreenProject.Colors );
          break;
        case Formats.GraphicScreenProject.CheckType.VIC20_CHARSET:
        case Formats.GraphicScreenProject.CheckType.VIC20_CHARSET_8X16:
          _ColorChooserDlg = new ColorChooserCommodoreVIC20( Core, m_GraphicScreenProject.Colors );
          break;
        case GraphicScreenProject.CheckType.MEGA65_FCM_CHARSET:
        case GraphicScreenProject.CheckType.MEGA65_FCM_CHARSET_16BIT:
        case GraphicScreenProject.CheckType.MEGA65_BITMAP:
        case Formats.GraphicScreenProject.CheckType.HIRES_BITMAP:
          _ColorChooserDlg = new ColorChooserBase( Core, m_GraphicScreenProject.Colors );
          break;
        default:
          Debug.Log( "ChangeColorSettingsDialog unsupported Mode " + m_GraphicScreenProject.SelectedCheckType );
          _ColorChooserDlg = new ColorChooserBase( Core, m_GraphicScreenProject.Colors );
          break;
      }
      panelColorSettings.Controls.Add( _ColorChooserDlg );

      /*
      _ColorSettingsDlg.SelectedColorChanged += _ColorSettingsDlg_SelectedColorChanged;
      _ColorSettingsDlg.ColorsModified += _ColorSettingsDlg_ColorsModified;
      _ColorSettingsDlg.ColorsExchanged += _ColorSettingsDlg_ColorsExchanged;
      _ColorSettingsDlg.PaletteModified += _ColorSettingsDlg_PaletteModified;
      _ColorSettingsDlg.PaletteMappingModified += _ColorSettingsDlg_PaletteMappingModified;
      _ColorSettingsDlg.PaletteSelected += _ColorSettingsDlg_PaletteSelected;
      _ColorSettingsDlg.PaletteMappingSelected += _ColorSettingsDlg_PaletteMappingSelected;
      
      _ColorSettingsDlg_SelectedColorChanged( _ColorSettingsDlg.SelectedColor );
      */
      _ColorChooserDlg.ColorsModified += _ColorChooserDlg_ColorsModified;
    }



    private void _ColorChooserDlg_ColorsModified( Types.ColorType Color, ColorSettings Colors )
    {
    }



    private void UpdateCursorInfo()
    {
      labelCursorInfo.Text = $"{m_LastPixelUnderMouse.X}, {m_LastPixelUnderMouse.Y} ({m_LastPixelUnderMouse.X / CheckBlockWidth}, {m_LastPixelUnderMouse.Y / CheckBlockHeight})";
    }



    private int CheckBlockWidth
    {
      get
      {
        return 8;
      }
    }



    private int CheckBlockHeight
    {
      get
      {
        switch ( m_GraphicScreenProject.SelectedCheckType )
        {
          case Formats.GraphicScreenProject.CheckType.VIC20_CHARSET_8X16:
            return 16;
        }
        return 8;
      }
    }



    private int BlockWidth
    {
      get
      {
        return ( m_GraphicScreenProject.ScreenWidth + CheckBlockWidth - 1 ) / CheckBlockWidth;
      }
    }



    private int BlockHeight
    {
      get
      {
        return ( m_GraphicScreenProject.ScreenHeight + CheckBlockHeight - 1 ) / CheckBlockHeight;
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
      int pixelX = ToLocalX( X );
      int pixelY = ToLocalY( Y );
      int charX = pixelX / CheckBlockWidth;
      int charY = pixelY / CheckBlockHeight;
      bool mouseMoved = false;

      if ( m_LastPixelUnderMouse.X != pixelX )
      {
        m_LastPixelUnderMouse.X = pixelX;
        mouseMoved = true;
      }
      if ( m_LastPixelUnderMouse.Y != pixelY )
      {
        m_LastPixelUnderMouse.Y = pixelY;
        mouseMoved = true;
      }

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

      if ( ( Buttons & MouseButtons.Middle ) != 0 )
      {
        if ( !m_DragViewModeActiveByPressedMiddleButton )
        {
          m_DragViewModeActiveByPressedMiddleButton = true;
          m_DragView = true;
          m_DragPoint = pictureEditor.PointToClient( MousePosition );
          pictureEditor.Cursor = Core.MainForm.CursorGrab;
        }
        HandleViewDragging( X, Y );
        return;
      }
      else
      {
        if ( m_DragViewModeActiveByPressedMiddleButton )
        {
          m_DragViewModeActiveByPressedMiddleButton = false;
          m_DragView = false;
          pictureEditor.Cursor = Cursors.Default;
          return;
        }
      }

      if ( ( Buttons & MouseButtons.Left ) != 0 )
      {
        if ( m_DragViewModeActiveBySpace )
        {
          HandleViewDragging( X, Y );
          return;
        }

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
            UpdateArea( pixelX ^ 1, pixelY, 2, 1 );
            Redraw();
            pictureEditor.Invalidate();
            Modified = true;
            break;
          case PaintTool.DRAW_FREE_LINE:
            if ( m_ButtonReleased )
            {
              m_ButtonReleased = false;
              DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoGraphicScreenImageChange( m_GraphicScreenProject, this, 0, 0, m_GraphicScreenProject.ScreenWidth, m_GraphicScreenProject.ScreenHeight ) );

              if ( m_GraphicScreenProject.MultiColor )
              {
                m_GraphicScreenProject.Image.SetPixel( pixelX ^ 1, pixelY, m_CurrentColor );
              }
              m_GraphicScreenProject.Image.SetPixel( pixelX, pixelY, m_CurrentColor );
              UpdateArea( pixelX ^ 1, pixelY, 2, 1 );
            }
            else
            {
              // paint according to mode!
              m_GraphicScreenProject.Image.Line( m_LastPaintedPixelPos.X, m_LastPaintedPixelPos.Y, pixelX, pixelY, m_CurrentColor );

              int     x1 = Math.Min( m_LastPaintedPixelPos.X, pixelX );
              int     x2 = Math.Max( m_LastPaintedPixelPos.X, pixelX );
              int     y1 = Math.Min( m_LastPaintedPixelPos.Y, pixelY );
              int     y2 = Math.Max( m_LastPaintedPixelPos.Y, pixelY );

              UpdateArea( x1, y1, x2 - x1 + 1, y2 - y1 + 1 );
            }
            Modified = true;
            m_LastPaintedPixelPos = new Point( pixelX, pixelY );
            break;
          case PaintTool.DRAW_LINE:
            if ( m_ButtonReleased )
            {
              m_ButtonReleased = false;
              if ( ( m_PaintTool == PaintTool.SELECT )
              &&   ( m_SelectionFloating ) )
              {
                // insert floating selection
                DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoGraphicScreenImageChange( m_GraphicScreenProject, this, pixelX, pixelY, m_SelectionFloatingImage.Width, m_SelectionFloatingImage.Height ) );

                m_SelectionFloatingImage.DrawTo( m_GraphicScreenProject.Image, pixelX, pixelY );
                UpdateArea( pixelX, pixelY, m_SelectionFloatingImage.Width, m_SelectionFloatingImage.Height );
                SetModified();

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

              charEditor.DisplayPage.DrawImage( m_GraphicScreenProject.Image, 0, 0, m_SelectedChar.X * CheckBlockWidth, m_SelectedChar.Y * CheckBlockHeight, CheckBlockWidth, CheckBlockHeight );
              charEditor.Invalidate();

              Redraw();
            }
            if ( !string.IsNullOrEmpty( m_Chars[charX + charY * BlockWidth].Error ) )
            {
              labelCharInfo.Text = m_Chars[charX + charY * BlockWidth].Error;
            }
            else
            {
              RetroDevStudio.Formats.CharData usedChar = m_Chars[charX + charY * BlockWidth];
              if ( usedChar.Replacement != null )
              {
                while ( usedChar.Replacement != null )
                {
                  usedChar = usedChar.Replacement;
                }
                labelCharInfo.Text = $"Duplicate of {usedChar.Index}, color {m_Chars[charX + charY * BlockWidth].Tile.CustomColor}";
              }
              else
              {
                labelCharInfo.Text = $"Determined index {usedChar.Index}, color {m_Chars[charX + charY * BlockWidth].Tile.CustomColor}";
              }
            }
            _ColorChooserDlg.ColorChanged( Types.ColorType.CUSTOM_COLOR, m_Chars[charX + charY * BlockWidth].Tile.CustomColor );
            break;
          case PaintTool.FLOOD_FILL:
            if ( m_ButtonReleased )
            {
              m_ButtonReleased = false;
              DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoGraphicScreenImageChange( m_GraphicScreenProject, this, 0, 0, m_GraphicScreenProject.ScreenWidth, m_GraphicScreenProject.ScreenHeight ) );
              FloodFill( pixelX, pixelY, m_CurrentColor );
              UpdateArea( 0, 0, pictureEditor.DisplayPage.Width, pictureEditor.DisplayPage.Height );
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
            {
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

                  UpdateArea( x1, y1, x2 - x1 + 1, y2 - y1 + 1 );
                  Modified = true;
                  break;
                case PaintTool.DRAW_RECTANGLE:
                  DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoGraphicScreenImageChange( m_GraphicScreenProject, this, x1, y1, x2 - x1 + 1, y2 - y1 + 1 ) );
                  m_GraphicScreenProject.Image.Rectangle( x1, y1, x2 - x1 + 1, y2 - y1 + 1, m_CurrentColor );
                  if ( m_GraphicScreenProject.MultiColor )
                  {
                    m_GraphicScreenProject.Image.Line( x1 + 1, y1, x1 + 1, y2, m_CurrentColor );
                    m_GraphicScreenProject.Image.Line( x2 - 1, y1, x2 - 1, y2, m_CurrentColor );
                  }
                  UpdateArea( x1, y1, x2 - x1 + 1, y2 - y1 + 1 );
                  Modified = true;
                  break;
                case PaintTool.SELECT:
                  // only make selection if we didn't place a floating selection before
                  if ( m_DragStartPoint.X != -1 )
                  {
                    m_Selection = new System.Drawing.Rectangle( x1, y1, x2 - x1 + 1, y2 - y1 + 1 );
                  }
                  UpdateArea( 0, 0, pictureEditor.DisplayPage.Width, pictureEditor.DisplayPage.Height );
                  break;
              }
              m_DragStartPoint.X = -1;
            }
            break;
          case PaintTool.DRAW_LINE:
            {
              int     x1 = Math.Min( m_DragStartPoint.X, m_DragCurrentPoint.X );
              int     x2 = Math.Max( m_DragStartPoint.X, m_DragCurrentPoint.X );
              int     y1 = Math.Min( m_DragStartPoint.Y, m_DragCurrentPoint.Y );
              int     y2 = Math.Max( m_DragStartPoint.Y, m_DragCurrentPoint.Y );

              if ( m_GraphicScreenProject.MultiColor )
              {
                x1 &= ~1;
                x2 &= ~1;
              }

              DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoGraphicScreenImageChange( m_GraphicScreenProject, this, x1, y1, x2 - x1 + 1, y2 - y1 + 1 ) );

              int xx1 = m_DragStartPoint.X;
              int yy1 = m_DragStartPoint.Y;
              int xx2 = m_DragCurrentPoint.X;
              int yy2 = m_DragCurrentPoint.Y;
              m_GraphicScreenProject.Image.Line( xx1, yy1, xx2, yy2, m_CurrentColor );
              if ( m_GraphicScreenProject.MultiColor )
              {
                m_GraphicScreenProject.Image.Line( xx1 + 1, yy1, xx2 + 1, yy2, m_CurrentColor );
              }
              UpdateArea( x1, y1, x2 - x1 + 1, y2 - y1 + 1 );

              m_DragStartPoint.X = -1;
              Redraw();
              pictureEditor.Invalidate();
              Modified = true;
            }
            break;
        }
        m_ButtonReleased = true;
      }
      else
      {
        if ( ( m_DragViewModeActiveBySpace )
        &&   ( m_DragView ) )
        {
          m_DragView = false;
          pictureEditor.Cursor = Core.MainForm.CursorHand;
        }


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
        switch ( m_PaintTool )
        {
          case PaintTool.DRAW_PIXEL:
          default:
            m_CurrentColor = (byte)m_GraphicScreenProject.Image.GetPixel( pixelX, pixelY );
            _colorPickerDlg.SelectedColor = m_CurrentColor;
            break;
        }
      }
      if ( mouseMoved )
      {
        UpdateCursorInfo();
      }
    }



    private void HandleViewDragging( int x, int y )
    {
      if ( !m_DragView )
      {
        m_DragView = true;
        m_DragPoint = pictureEditor.PointToClient( MousePosition );
        pictureEditor.Cursor = Core.MainForm.CursorGrab;
      }
      else
      {
        int   deltaX = x - m_DragPoint.X;
        int   deltaY = y - m_DragPoint.Y;
        int   requiredDelta = 2; // y tho?

        if ( ( deltaX != 0 )
        ||   ( deltaY != 0 ) )
        {
          int   actDX = 0;
          int   actDY = 0;
          while ( deltaX >= requiredDelta )
          {
            deltaX -= requiredDelta;
            --actDX;
          }
          while ( deltaX <= -requiredDelta )
          {
            deltaX += requiredDelta;
            ++actDX;
          }
          while ( deltaY >= requiredDelta )
          {
            deltaY -= requiredDelta;
            --actDY;
          }
          while ( deltaY <= -requiredDelta )
          {
            deltaY += requiredDelta;
            ++actDY;
          }

          if ( ( actDX != 0 )
          ||   ( actDY != 0 ) )
          {
            int tempDX = actDX;
            int tempDY = actDY;

            while ( ( actDX < 0 )
            &&      ( screenHScroll.Value > 0 ) )
            {
              --screenHScroll.Value;
              ++actDX;
            }
            while ( ( actDX > 0 )
            &&      ( screenHScroll.Value < screenHScroll.Maximum ) )
            {
              ++screenHScroll.Value;
              --actDX;
            }
            while ( ( actDY < 0 )
            &&      ( screenVScroll.Value > 0 ) )
            {
              --screenVScroll.Value;
              ++actDY;
            }
            while ( ( actDY > 0 )
            &&      ( screenVScroll.Value < screenVScroll.Maximum ) )
            {
              ++screenVScroll.Value;
              --actDY;
            }
            m_DragPoint.Offset( -tempDX * requiredDelta, -tempDY * requiredDelta );
            screenHScroll_Scroll( screenHScroll );
            screenVScroll_Scroll( screenVScroll );
          }
        }
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



    public bool ImportImage( string Filename, GR.Image.IImage IncomingImage, ImageInsertionMode InsertMode )
    {
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
      else if ( ( m_GraphicScreenProject.SelectedCheckType == Formats.GraphicScreenProject.CheckType.VIC20_CHARSET )
      ||        ( m_GraphicScreenProject.SelectedCheckType == Formats.GraphicScreenProject.CheckType.VIC20_CHARSET_8X16 ) )
      {
        importType = GraphicType.CHARACTERS_MULTICOLOR;
      }
      else if ( m_GraphicScreenProject.SelectedCheckType == Formats.GraphicScreenProject.CheckType.MEGA65_BITMAP )
      {
        importType = GraphicType.BITMAP_8BIT;
        if ( IncomingImage != null )
        {
          mcSettings.Palette = IncomingImage.Palette;
        }
      }
      if ( !Core.MainForm.ImportImage( Filename, IncomingImage, importType, mcSettings,
                                       CheckBlockWidth, CheckBlockHeight, 
                                       out GR.Image.IImage mappedImage, out mcSettings, out pasteAsBlock, out importType ) )
      {
        return false;
      }

      if ( ( mappedImage.PixelFormat & GR.Drawing.PixelFormat.Indexed ) == 0 )
      {
        // quantize to palette
        var quant = new RetroDevStudio.Converter.ColorQuantizer( 256 );

        quant.AddSourceToColorCube( mappedImage );
        quant.Calculate();

        var resultingImage = quant.Reduce( mappedImage );
        mcSettings.Palette = new Palette( 1 << resultingImage.BitsPerPixel );
        for ( int i = 0; i < mcSettings.Palette.NumColors; ++i )
        {
          mcSettings.Palette.ColorValues[i] = resultingImage.PaletteColor( i ) | 0xff000000;
        }
        mcSettings.Palette.CreateBrushes();
        mappedImage = resultingImage;
      }
      /*
      if ( mappedImage.PixelFormat != GR.Drawing.PixelFormat.Format8bppIndexed )
      {
        mappedImage.Dispose();
        Core.Notification.MessageBox( "Unsupported image format", "Image format invalid!\nNeeds to be 8bit index" );
        return false;
      }*/

      Dialogs.DlgImportImageResize.ImportBehaviour    behaviour = RetroDevStudio.Dialogs.DlgImportImageResize.ImportBehaviour.CLIP_IMAGE;

      if ( InsertMode == ImageInsertionMode.AS_FULL_SCREEN )
      {
        if ( ( mappedImage.Width != m_GraphicScreenProject.Image.Width )
        ||   ( mappedImage.Height != m_GraphicScreenProject.Image.Height ) )
        {
          Dialogs.DlgImportImageResize    dlg = new RetroDevStudio.Dialogs.DlgImportImageResize( mappedImage.Width, mappedImage.Height, m_GraphicScreenProject.Image.Width, m_GraphicScreenProject.Image.Height, Core );

          dlg.ShowDialog();
          if ( dlg.ChosenResult == RetroDevStudio.Dialogs.DlgImportImageResize.ImportBehaviour.CANCEL )
          {
            return false;
          }
          behaviour = dlg.ChosenResult;
        }
      }
      if ( behaviour == RetroDevStudio.Dialogs.DlgImportImageResize.ImportBehaviour.ADJUST_SCREEN_SIZE )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoGraphicScreenSizeChange( m_GraphicScreenProject, this, m_GraphicScreenProject.ScreenWidth, m_GraphicScreenProject.ScreenHeight ) );

        SetScreenSize( mappedImage.Width, mappedImage.Height );

        DocumentInfo.UndoManager.AddGroupedUndoTask( new Undo.UndoGraphicScreenImageChange( m_GraphicScreenProject, this, 0, 0, m_GraphicScreenProject.ScreenWidth, m_GraphicScreenProject.ScreenHeight ) );
      }

      m_GraphicScreenProject.Colors.Palette = mcSettings.Palette;
      m_GraphicScreenProject.SanitizePalettes();
      PaletteManager.ApplyPalette( m_GraphicScreenProject.Image, mcSettings.Palette );
      PaletteManager.ApplyPalette( pictureEditor.DisplayPage, mcSettings.Palette );
      PaletteManager.ApplyPalette( charEditor.DisplayPage, mcSettings.Palette );

      if ( InsertMode == ImageInsertionMode.AT_SELECTED_LOCATION )
      {
        mappedImage.DrawTo( m_GraphicScreenProject.Image, m_SelectedChar.X * CheckBlockWidth, m_SelectedChar.Y * CheckBlockHeight );

        charEditor.DisplayPage.DrawImage( m_GraphicScreenProject.Image, 0, 0, m_SelectedChar.X * CheckBlockWidth, m_SelectedChar.Y * CheckBlockHeight, CheckBlockWidth, CheckBlockHeight );
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

      _ColorChooserDlg.ColorChanged( Types.ColorType.BACKGROUND, NormalizeColor( mcSettings.BackgroundColor ) );
      _ColorChooserDlg.ColorChanged( Types.ColorType.MULTICOLOR_1, NormalizeColor( mcSettings.MultiColor1 ) );
      _ColorChooserDlg.ColorChanged( Types.ColorType.MULTICOLOR_2, NormalizeColor( mcSettings.MultiColor2 ) );
      _colorPickerDlg.UpdatePalette( m_GraphicScreenProject.Colors.Palette );

      btnCheck_Click( null );
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

      m_ErrornousChars = new bool[( Width + CheckBlockWidth - 1 ) / CheckBlockWidth, ( Height + CheckBlockHeight - 1 ) / CheckBlockHeight];

      SetCharCheckList();
      if ( ( m_SelectedChar.X >= BlockWidth )
      ||   ( m_SelectedChar.Y >= BlockHeight ) )
      {
        m_SelectedChar.X = -1;
        m_SelectedChar.Y = -1;
      }

      AdjustScrollbars();
    }



    private void SetCharCheckList()
    {
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

      comboCheckType.SelectedIndex = (int)m_GraphicScreenProject.SelectedCheckType;
      ChangeColorChooserDialog();
      ChangeColorPickerDialog();

      _ColorChooserDlg.ColorChanged( Types.ColorType.BACKGROUND, m_GraphicScreenProject.Colors.BackgroundColor );
      _ColorChooserDlg.ColorChanged( Types.ColorType.MULTICOLOR_1, m_GraphicScreenProject.Colors.MultiColor1 );
      _ColorChooserDlg.ColorChanged( Types.ColorType.MULTICOLOR_2, m_GraphicScreenProject.Colors.MultiColor2 );

      PaletteManager.ApplyPalette( pictureEditor.DisplayPage, m_GraphicScreenProject.Colors.Palette );
      PaletteManager.ApplyPalette( charEditor.DisplayPage, m_GraphicScreenProject.Colors.Palette );
      charEditor.Invalidate();

      SetScreenSize( m_GraphicScreenProject.Image.Width, m_GraphicScreenProject.Image.Height );
      AdjustScrollbars();

      screenHScroll.Value = m_GraphicScreenProject.ScreenOffsetX;
      screenVScroll.Value = m_GraphicScreenProject.ScreenOffsetY;

      Redraw();
      EnableFileWatcher();
    }



    public override bool LoadDocument()
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
        Core.Notification.MessageBox( "Could not load file", "Could not load graphic screen project file " + DocumentInfo.FullPath + ".\r\n" + ex.Message );
        return false;
      }
      SetUnmodified();
      return true;
    }



    public override GR.Memory.ByteBuffer SaveToBuffer()
    {
      return m_GraphicScreenProject.SaveToBuffer();
    }



    protected override bool QueryFilename( string PreviousFilename, out string Filename )
    {
      Filename = "";

      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Save Graphic Screen Project as";
      saveDlg.Filter = "Graphic Screen Projects|*.graphicscreen|All Files|*.*";
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
        DialogResult doSave = MessageBox.Show( "There are unsaved changes in your graphic screen. Save now?", "Save changes?", endButtons );
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



    private void btnImportCharset_Click( DecentForms.ControlBase Sender )
    {
      string    filename;
      if ( OpenFile( "Open file", Types.Constants.FILEFILTER_ALL, out filename ) )
      {
        GR.Memory.ByteBuffer imageData = GR.IO.File.ReadAllBytes( filename );

        ImportKoalaPicture( imageData );
      }
    }



    public void ImportKoalaPicture( ByteBuffer imageData )
    {
      // 0000 - 1F3F : Bitmap 8000 Bytes
      // 1F40 - 2327 : Bildschirmspeicher 1000 Bytes
      // 2328 - 270F : Farb-RAM 1000 Bytes
      // 2710        : Hintergrundfarbe 1 Byte
      if ( imageData.Length == 10003 )
      {
        // could be a Koala painter image
        if ( imageData.UInt16At( 0 ) == 0x6000 )
        {
          // background color
          _ColorChooserDlg.ColorChanged( Types.ColorType.BACKGROUND, imageData.ByteAt( 10002 ) % 16 );

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
          pictureEditor.Invalidate();
        }
      }
    }



    public bool ImportIFFPicture( ByteBuffer imageData )
    {
      var image = IFFToBitmap.BitmapFromIFF( imageData );
      if ( image == null )
      {
        return false;
      }

      comboCheckType.SelectedIndex = (int)GraphicScreenProject.CheckType.MEGA65_BITMAP;
      return ImportImage( null, image, ImageInsertionMode.AS_FULL_SCREEN );
    }



    private void btnPasteFromClipboard_Click( DecentForms.ControlBase Sender )
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
    }



    private void btnImportCharsetFromFile_Click( DecentForms.ControlBase Sender )
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

      Core.Imaging.ImageToClipboard( m_GraphicScreenProject.Image, m_SelectedChar.X * CheckBlockWidth, m_SelectedChar.Y * CheckBlockHeight, CheckBlockWidth, CheckBlockHeight );
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



    private void btnCopy_Click( DecentForms.ControlBase Sender )
    {
      CopySelectedImageToClipboard();
    }



    private void btnPaste_Click( DecentForms.ControlBase Sender )
    {
      PasteClipboardImageToSelectedChar();
    }



    private void Redraw()
    {
      m_GraphicScreenProject.Image.DrawTo( pictureEditor.DisplayPage, -m_GraphicScreenProject.ScreenOffsetX / m_ZoomFactor, -m_GraphicScreenProject.ScreenOffsetY / m_ZoomFactor );

      int   leftX = m_GraphicScreenProject.ScreenOffsetX / m_ZoomFactor;
      int   leftY = m_GraphicScreenProject.ScreenOffsetY / m_ZoomFactor;

      switch ( m_PaintTool )
      {
        case PaintTool.DRAW_LINE:
          if ( m_DragStartPoint.X != -1 )
          {
            int     x1 = m_DragStartPoint.X;
            int     x2 = m_DragCurrentPoint.X;
            int     y1 = m_DragStartPoint.Y;
            int     y2 = m_DragCurrentPoint.Y;

            if ( m_GraphicScreenProject.MultiColor )
            {
              x1 &= ~1;
              x2 &= ~1;
            }

            x1 -= leftX;
            x2 -= leftX;
            y1 -= leftY;
            y2 -= leftY;

            pictureEditor.DisplayPage.Line( x1, y1, x2, y2, m_CurrentColor );
            if ( m_GraphicScreenProject.MultiColor )
            {
              pictureEditor.DisplayPage.Line( x1 + 1, y1, x2 + 1, y2, m_CurrentColor );
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

            x1 -= leftX;
            x2 -= leftX;
            y1 -= leftY;
            y2 -= leftY;

            if ( m_PaintTool == PaintTool.DRAW_BOX )
            {
              pictureEditor.DisplayPage.Box( x1, y1, x2 - x1 + 1, y2 - y1 + 1, m_CurrentColor );
            }
            else
            {
              pictureEditor.DisplayPage.Rectangle( x1, y1, x2 - x1 + 1, y2 - y1 + 1, m_CurrentColor );
            }
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
            pictureEditor.DisplayPage.DrawImage( m_SelectionFloatingImage, m_SelectionFloatingPos.X - leftX, m_SelectionFloatingPos.Y - leftY );
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



    private bool CheckForMCCharsetErrors()
    {
      bool      foundError = false;
      for ( int j = 0; j < BlockHeight; ++j )
      {
        for ( int i = 0; i < BlockWidth; ++i )
        {
          m_GraphicScreenProject.CheckCharBox( m_Chars[i + j * BlockWidth], 
                                               i * CheckBlockWidth, j * CheckBlockHeight, 
                                               CheckBlockWidth, CheckBlockHeight,
                                               true );
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
      if ( items - foldedItems > 256 )
      {
        labelCharInfo.Text = "Too many unique characters (" + ( items - foldedItems ) + ")";
        return true;
      }
      labelCharInfo.Text = ( items - foldedItems ).ToString() + " unique chars, duplicates removed " + foldedItems;
      return false;
    }



    private bool CheckForVIC20CharsetErrors()
    {
      bool      foundError = false;
      for ( int j = 0; j < BlockHeight; ++j )
      {
        for ( int i = 0; i < BlockWidth; ++i )
        {
          m_GraphicScreenProject.CheckCharBox( m_Chars[i + j * BlockWidth], 
                                               i * CheckBlockWidth, j * CheckBlockHeight,
                                               CheckBlockWidth, CheckBlockHeight,
                                               true );
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
      if ( items - foldedItems > 256 )
      {
        labelCharInfo.Text = "Too many unique characters (" + ( items - foldedItems ) + ")";
        return true;
      }
      labelCharInfo.Text = ( items - foldedItems ).ToString() + " unique chars, duplicates removed " + foldedItems;
      return false;
    }



    private bool CheckForHiResCharsetErrors()
    {
      bool      foundError = false;
      for ( int j = 0; j < BlockHeight; ++j )
      {
        for ( int i = 0; i < BlockWidth; ++i )
        {
          m_GraphicScreenProject.CheckCharBox( m_Chars[i + j * BlockWidth], 
                                               i * CheckBlockWidth, j * CheckBlockHeight,
                                               CheckBlockWidth, CheckBlockHeight, 
                                               false );
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
      if ( items - foldedItems > 256 )
      {
        labelCharInfo.Text = "Too many unique characters (" + ( items - foldedItems ) + ")";
        return true;
      }
      labelCharInfo.Text = ( items - foldedItems ).ToString() + " unique chars, duplicates removed " + foldedItems;
      return false;
    }



    private void ClearErrorFlags()
    {
      for ( int j = 0; j < BlockHeight; ++j )
      {
        for ( int i = 0; i < BlockWidth; ++i )
        {
          m_ErrornousChars[i, j] = false;
        }
      }
    }



    private bool CheckForFCMCharsetErrors( int AllowedNumberOfChars )
    {
      Debug.Log( "CheckExportType inside c" );
      ClearErrorFlags();
      Debug.Log( "CheckExportType inside d" );

      // check for duplicates
      int items = m_Chars.Count;
      int foldedItems = 0;
      int curIndex = 0;

      Debug.Log( "CheckExportType inside e" );
      var images = new ByteBuffer[m_Chars.Count];
      for ( int index1 = 0; index1 < m_Chars.Count; ++index1 )
      {
        images[index1] = ( (GR.Image.MemoryImage)m_GraphicScreenProject.Image.GetImage( ( index1 % ( ( m_GraphicScreenProject.ScreenWidth + CheckBlockWidth - 1 ) / CheckBlockWidth ) ) * CheckBlockWidth,
          ( index1 / ( ( m_GraphicScreenProject.ScreenWidth + CheckBlockWidth - 1 ) / CheckBlockWidth ) ) * CheckBlockHeight,
          CheckBlockWidth, CheckBlockHeight ) ).GetAsData();
      }

      for ( int index1 = 0; index1 < m_Chars.Count; ++index1 )
      {
        var  tile1 = images[index1];
        bool wasFolded = false;
        for ( int index2 = 0; index2 < index1; ++index2 )
        {
          var  tile2 = images[index2];

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
      Debug.Log( "CheckExportType inside f" );
      labelCharInfo.Text = "";
      if ( items - foldedItems > AllowedNumberOfChars )
      {
        labelCharInfo.Text = "Too many unique characters (" + ( items - foldedItems ) + ")";
        return true;
      }
      labelCharInfo.Text = ( items - foldedItems ).ToString() + " unique chars, duplicates removed " + foldedItems;
      return false;
    }



    private bool CheckForHiResBitmapErrors()
    {
      bool  allGood = true;
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

          for ( int charY = 0; charY < CheckBlockHeight; ++charY )
          {
            for ( int charX = 0; charX < CheckBlockWidth; ++charX )
            {
              byte  colorIndex = (byte)m_GraphicScreenProject.Image.GetPixel( x * CheckBlockWidth + charX, y * CheckBlockHeight + charY );

              if ( colorIndex >= 16 )
              {
                m_ErrornousChars[x, y] = true;
                m_Chars[x + y * BlockWidth].Error = "Encountered color index >= 16 at " + ( x * CheckBlockWidth + charX ) + "," + ( y * CheckBlockHeight + charY );
                allGood = false;
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
            m_Chars[x + y * BlockWidth].Error = "Uses more than two colors at " + ( x * CheckBlockWidth ) + "," + ( y * CheckBlockHeight );
            allGood = false;
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
            m_Chars[x + y * BlockWidth].Error = "Looks like single color, but doesn't use the set background color " + ( x * CheckBlockWidth ) + "," + ( y * CheckBlockHeight );
            allGood = false;
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
                  m_Chars[x + y * BlockWidth].Error = "Uses more than one free color " + ( x * CheckBlockWidth ) + "," + ( y * CheckBlockHeight );
                  allGood = false;
                  continue;
                }
                usedFreeColor = i;
              }
            }
          }
        }
      }
      return allGood;
    }



    private bool CheckForMCBitmapErrors()
    {
      bool allGood = true;

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
          for ( int charY = 0; charY < CheckBlockHeight; ++charY )
          {
            for ( int charX = 0; charX < CheckBlockWidth / 2; ++charX )
            {
              byte  colorIndex = (byte)m_GraphicScreenProject.Image.GetPixel( x * CheckBlockWidth + charX * 2, y * CheckBlockHeight + charY );
              if ( colorIndex >= 16 )
              {
                m_Chars[x + y * BlockWidth].Error = "Color index >= 16 at " + ( x * CheckBlockWidth + charX * 2 ).ToString() + ", " + ( y * CheckBlockHeight + charY ).ToString() + " (" + charX + "," + charY + ")";
                m_ErrornousChars[x, y] = true;
                allGood = false;
              }
              byte  colorIndex2 = (byte)m_GraphicScreenProject.Image.GetPixel( x * CheckBlockWidth + charX * 2 + 1, y * CheckBlockHeight + charY );
              if ( colorIndex != colorIndex2 )
              {
                m_Chars[x + y * BlockWidth].Error = "Used HiRes pixel >= 16 at " + ( x * CheckBlockWidth + charX * 2 ).ToString() + ", " + ( y * CheckBlockHeight + charY ).ToString() + " (" + charX + "," + charY + ")";
                m_ErrornousChars[x, y] = true;
                allGood = false;
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
            allGood = false;
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
            for ( int charY = 0; charY < CheckBlockHeight; ++charY )
            {
              for ( int charX = 0; charX < CheckBlockWidth / 2; ++charX )
              {
                byte colorIndex = (byte)m_GraphicScreenProject.Image.GetPixel( x * CheckBlockWidth + charX * 2, y * CheckBlockHeight + charY );
                if ( colorIndex != m_GraphicScreenProject.Colors.BackgroundColor )
                {
                  // other color
                  byte colorValue = usedColors[colorIndex];
                  int bitmapIndex = x * CheckBlockWidth + y * 320 + charY;

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
      return allGood;
    }



    private void btnCheck_Click( DecentForms.ControlBase Sender )
    {
      Debug.Log( "CheckExportType a" );
      CheckExportType();
      Debug.Log( "CheckExportType b" );
    }



    public bool CheckExportType()
    {
      Debug.Log( "CheckExportType inside a" );
      foreach ( Formats.CharData aChar in m_Chars )
      {
        aChar.Error = "";
        aChar.Tile.CustomColor = 0;
        aChar.Replacement = null;
      }

      Debug.Log( "CheckExportType inside b" );
      bool  hasErrors = false;

      switch ( (Formats.GraphicScreenProject.CheckType)comboCheckType.SelectedIndex )
      {
        case RetroDevStudio.Formats.GraphicScreenProject.CheckType.HIRES_BITMAP:
          hasErrors = CheckForHiResBitmapErrors();
          break;
        case RetroDevStudio.Formats.GraphicScreenProject.CheckType.MULTICOLOR_BITMAP:
          hasErrors = CheckForMCBitmapErrors();
          break;
        case RetroDevStudio.Formats.GraphicScreenProject.CheckType.HIRES_CHARSET:
          hasErrors = CheckForHiResCharsetErrors();
          break;
        case RetroDevStudio.Formats.GraphicScreenProject.CheckType.MULTICOLOR_CHARSET:
          hasErrors = CheckForMCCharsetErrors();
          break;
        case RetroDevStudio.Formats.GraphicScreenProject.CheckType.MEGA65_FCM_CHARSET:
          hasErrors = CheckForFCMCharsetErrors( 256 );
          break;
        case RetroDevStudio.Formats.GraphicScreenProject.CheckType.MEGA65_FCM_CHARSET_16BIT:
          hasErrors = CheckForFCMCharsetErrors( 8192 );
          break;
        case RetroDevStudio.Formats.GraphicScreenProject.CheckType.VIC20_CHARSET:
        case RetroDevStudio.Formats.GraphicScreenProject.CheckType.VIC20_CHARSET_8X16:
          hasErrors = CheckForVIC20CharsetErrors();
          break;
        case GraphicScreenProject.CheckType.MEGA65_BITMAP:
          // always fine
          hasErrors = true;
          break;
        default:
          Debug.Log( "Unsupported CheckType: " + (Formats.GraphicScreenProject.CheckType)comboCheckType.SelectedIndex );
          break;
      }
      Redraw();

      return !hasErrors;
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

      int     charX = X / ( charEditor.ClientRectangle.Width / CheckBlockWidth );
      int     charY = Y / ( charEditor.ClientRectangle.Height / CheckBlockHeight );

      if ( ( Buttons & MouseButtons.Left ) != 0 )
      {
        if ( m_ButtonReleased )
        {
          m_ButtonReleased = false;
          DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoGraphicScreenImageChange( m_GraphicScreenProject, this, m_SelectedChar.X * CheckBlockWidth, m_SelectedChar.Y * CheckBlockHeight, CheckBlockWidth, CheckBlockHeight ) );
        }
        if ( ( m_GraphicScreenProject.SelectedCheckType == Formats.GraphicScreenProject.CheckType.MULTICOLOR_BITMAP )
        ||   ( m_GraphicScreenProject.SelectedCheckType == Formats.GraphicScreenProject.CheckType.MULTICOLOR_CHARSET ) )
        {
          byte    colorToSet = m_CurrentColor;

          charX /= 2;
          m_GraphicScreenProject.Image.SetPixel( m_SelectedChar.X * CheckBlockWidth + charX * 2, m_SelectedChar.Y * CheckBlockHeight + charY, colorToSet );
          m_GraphicScreenProject.Image.SetPixel( m_SelectedChar.X * CheckBlockWidth + charX * 2 + 1, m_SelectedChar.Y * CheckBlockHeight + charY, colorToSet );

          charEditor.DisplayPage.SetPixel( charX * 2, charY, colorToSet );
          charEditor.DisplayPage.SetPixel( charX * 2 + 1, charY, colorToSet );
          charEditor.Invalidate();
          Modified = true;
          Redraw();
        }
        else
        {
          byte    colorToSet = m_CurrentColor;

          m_GraphicScreenProject.Image.SetPixel( m_SelectedChar.X * CheckBlockWidth + charX, m_SelectedChar.Y * CheckBlockHeight + charY, colorToSet );

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
        if ( !Core.Settings.BehaviourRightClickIsBGColorPaint )
        {
          m_CurrentColor = (byte)m_GraphicScreenProject.Image.GetPixel( m_SelectedChar.X * CheckBlockWidth + charX, m_SelectedChar.Y * CheckBlockHeight + charY );
          _colorPickerDlg.SelectedColor = m_CurrentColor;
        }
        else
        {
          // TODO - optional paint only possible pixels?
          /*
          if ( ( m_GraphicScreenProject.SelectedCheckType == Formats.GraphicScreenProject.CheckType.MULTICOLOR_BITMAP )
          ||   ( m_GraphicScreenProject.SelectedCheckType == Formats.GraphicScreenProject.CheckType.MULTICOLOR_CHARSET ) )
          {
            byte    colorToSet = (byte)m_GraphicScreenProject.Colors.BackgroundColor;
            charX /= 2;
            m_GraphicScreenProject.Image.SetPixel( m_SelectedChar.X * CheckBlockWidth + charX * 2, m_SelectedChar.Y * CheckBlockHeight + charY, colorToSet );
            m_GraphicScreenProject.Image.SetPixel( m_SelectedChar.X * CheckBlockWidth + charX * 2 + 1, m_SelectedChar.Y * CheckBlockHeight + charY, colorToSet );

            charEditor.DisplayPage.SetPixel( charX * 2, charY, colorToSet );
            charEditor.DisplayPage.SetPixel( charX * 2 + 1, charY, colorToSet );
            charEditor.Invalidate();
            Modified = true;
            Redraw();
          }
          else*/
          {
            byte  colorToSet = (byte)m_GraphicScreenProject.Colors.BackgroundColor;

            m_GraphicScreenProject.Image.SetPixel( m_SelectedChar.X * CheckBlockWidth + charX, m_SelectedChar.Y * CheckBlockHeight + charY, colorToSet );

            charEditor.DisplayPage.SetPixel( charX, charY, colorToSet );
            charEditor.Invalidate();
            Modified = true;
            Redraw();
          }
        }
      }

    }



    private void btnExportAsCharset_Click( DecentForms.ControlBase Sender )
    {
      /*
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
      }*/
    }



    public static Formats.CharsetProject ExportToCharset( GraphicScreenProject Project, List<CharData> Chars )
    {
      if ( Chars.Any( c => !string.IsNullOrEmpty( c.Error ) ) )
      {
        return null;
      }
      // export possible
      Formats.CharsetProject projectToExport = new RetroDevStudio.Formats.CharsetProject();
      int curCharIndex = 0;

      // TODO - set mode accordingly!
      projectToExport.Colors.MultiColor1      = Project.Colors.MultiColor1;
      projectToExport.Colors.MultiColor2      = Project.Colors.MultiColor2;
      projectToExport.Colors.BackgroundColor  = Project.Colors.BackgroundColor;
      foreach ( Formats.CharData charData in Chars )
      {
        if ( charData.Replacement == null )
        {
          if ( charData.Index >= projectToExport.TotalNumberOfCharacters )
          {
            return null;
          }
          projectToExport.Characters[charData.Index] = charData.Clone();
          ++curCharIndex;
        }
      }
      return projectToExport;
    }


    public static bool ExportToCharScreen( List<CharData> Chars, int BlockWidth, int BlockHeight, out ByteBuffer ScreenCharData, out ByteBuffer ScreenColorData )
    {
      ScreenCharData   = new GR.Memory.ByteBuffer( (uint)( BlockWidth * BlockHeight ) );
      ScreenColorData  = new GR.Memory.ByteBuffer( (uint)( BlockWidth * BlockHeight ) );

      for ( int y = 0; y < BlockHeight; ++y )
      {
        for ( int x = 0; x < BlockWidth; ++x )
        {
          RetroDevStudio.Formats.CharData  charUsed = Chars[x + y * BlockWidth];
          while ( charUsed.Replacement != null )
          {
            charUsed = charUsed.Replacement;
          }
          ScreenCharData.SetU8At( x + y * BlockWidth, (byte)charUsed.Index );
          ScreenColorData.SetU8At( x + y * BlockWidth, (byte)charUsed.Tile.CustomColor );
        }
      }
      return true;
    }



    private void UsedCharsToClipboard()
    {
      // export possible
      Formats.CharsetProject projectToExport = new RetroDevStudio.Formats.CharsetProject();
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
            Core.Notification.MessageBox( "Too many characters", "Too many characters found (more than 256)!" );
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
        dataSelection.AppendI32( CheckBlockWidth );
        dataSelection.AppendI32( CheckBlockHeight );
        dataSelection.AppendU32( projectToExport.Characters[i].Tile.Data.Length );
        dataSelection.Append( projectToExport.Characters[i].Tile.Data );
        dataSelection.AppendI32( i );
      }

      DataObject dataObj = new DataObject();

      dataObj.SetData( "RetroDevStudio.ImageList", false, dataSelection.MemoryStream() );

      Clipboard.SetDataObject( dataObj, true );
    }



    private void comboCheckType_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( m_GraphicScreenProject.SelectedCheckType == (RetroDevStudio.Formats.GraphicScreenProject.CheckType)comboCheckType.SelectedIndex )
      {
        return;
      }
      m_GraphicScreenProject.SelectedCheckType  = (RetroDevStudio.Formats.GraphicScreenProject.CheckType)comboCheckType.SelectedIndex;
      m_GraphicScreenProject.Colors.Palette     = PaletteManager.PaletteFromMode( Lookup.CharacterModeFromCheckType( m_GraphicScreenProject.SelectedCheckType ) );

      charEditor.DisplayPage.Create( CheckBlockWidth, CheckBlockHeight, GR.Drawing.PixelFormat.Format8bppIndexed );

      m_SelectedChar.X %= BlockWidth;
      m_SelectedChar.Y %= BlockHeight;

      PaletteManager.ApplyPalette( pictureEditor.DisplayPage, m_GraphicScreenProject.Colors.Palette );
      PaletteManager.ApplyPalette( charEditor.DisplayPage, m_GraphicScreenProject.Colors.Palette );
      charEditor.Invalidate();

      ClearErrorFlags();
      ChangeColorChooserDialog();
      ChangeColorPickerDialog();
      SetCharCheckList();
      UpdateColorMappingOptions();

      int     numBytes = Lookup.NumBytesOfSingleCharacterBitmap( Lookup.CharacterModeFromCheckType( m_GraphicScreenProject.SelectedCheckType ) );
      if ( ( m_Chars.Count > 0 )
      &&   ( numBytes != m_Chars[0].Tile.Data.Length ) )
      {
        foreach ( var character in m_Chars )
        {
          character.Tile.Data.Resize( (uint)numBytes );
        }
      }
      // redraw the selected character
      charEditor.DisplayPage.DrawImage( m_GraphicScreenProject.Image, 0, 0, m_SelectedChar.X * CheckBlockWidth, m_SelectedChar.Y * CheckBlockHeight, CheckBlockWidth, CheckBlockHeight );

      pictureEditor.Invalidate();
    }



    private void UpdateColorMappingOptions()
    {
      comboColorMappingTargets.BeginUpdate();
      comboColorMappingTargets.Items.Clear();
      switch ( m_GraphicScreenProject.SelectedCheckType )
      {
        case GraphicScreenProject.CheckType.HIRES_BITMAP:
          comboColorMappingTargets.Items.Add( UtilForms.ComboItem( GraphicScreenProject.ColorMappingTarget.COLOR_1 ) );
          comboColorMappingTargets.Items.Add( UtilForms.ComboItem( GraphicScreenProject.ColorMappingTarget.COLOR_2 ) );
          comboColorMappingTargets.Items.Add( UtilForms.ComboItem( GraphicScreenProject.ColorMappingTarget.ANY ) );
          break;
        case GraphicScreenProject.CheckType.MULTICOLOR_BITMAP:
          comboColorMappingTargets.Items.Add( UtilForms.ComboItem( GraphicScreenProject.ColorMappingTarget.BITS_00 ) );
          comboColorMappingTargets.Items.Add( UtilForms.ComboItem( GraphicScreenProject.ColorMappingTarget.BITS_01 ) );
          comboColorMappingTargets.Items.Add( UtilForms.ComboItem( GraphicScreenProject.ColorMappingTarget.BITS_10 ) );
          comboColorMappingTargets.Items.Add( UtilForms.ComboItem( GraphicScreenProject.ColorMappingTarget.BITS_11 ) );
          comboColorMappingTargets.Items.Add( UtilForms.ComboItem( GraphicScreenProject.ColorMappingTarget.ANY ) );
          break;
      }
      comboColorMappingTargets.EndUpdate();
    }



    private void AdjustScrollbars()
    {
      screenHScroll.Minimum = 0;
      screenHScroll.SmallChange = m_ZoomFactor;
      screenHScroll.LargeChange = m_ZoomFactor;
      screenVScroll.SmallChange = m_ZoomFactor;
      screenVScroll.LargeChange = m_ZoomFactor;

      int   trueWidth = m_GraphicScreenProject.Image.Width * m_ZoomFactor;
      int   trueHeight = m_GraphicScreenProject.Image.Height * m_ZoomFactor;

      if ( trueWidth <= 320 )
      {
        screenHScroll.Maximum = 0;
        screenHScroll.Enabled = false;
        m_GraphicScreenProject.ScreenOffsetX = 0;
      }
      else
      {
        screenHScroll.Maximum = trueWidth - 320;
        screenHScroll.Enabled = true;
        if ( m_GraphicScreenProject.ScreenOffsetX > screenHScroll.Maximum )
        {
          m_GraphicScreenProject.ScreenOffsetX = screenHScroll.Maximum;
        }
      }

      screenVScroll.Minimum = 0;
      if ( trueHeight <= 200 )
      {
        screenVScroll.Maximum = 0;
        screenVScroll.Enabled = false;
        m_GraphicScreenProject.ScreenOffsetY = 0;
      }
      else
      {
        screenVScroll.Maximum = trueHeight - 200;
        screenVScroll.Enabled = true;
        if ( m_GraphicScreenProject.ScreenOffsetY > screenVScroll.Maximum )
        {
          m_GraphicScreenProject.ScreenOffsetY = screenVScroll.Maximum;
        }
      }
    }



    private void screenHScroll_Scroll( DecentForms.ControlBase Sender )
    {
      if ( m_GraphicScreenProject.ScreenOffsetX != screenHScroll.Value )
      {
        m_GraphicScreenProject.ScreenOffsetX = screenHScroll.Value;
        Redraw();
      }
    }



    private void screenVScroll_Scroll( DecentForms.ControlBase Sender )
    {
      if ( m_GraphicScreenProject.ScreenOffsetY != screenVScroll.Value )
      {
        m_GraphicScreenProject.ScreenOffsetY = screenVScroll.Value;
        Redraw();
      }
    }



    void MainForm_ApplicationEvent( Types.ApplicationEvent Event )
    {
      // TODO
      /*
      if ( ( Event.EventType == Types.ApplicationEvent.Type.DOCUMENT_INFO_CREATED )
      &&   ( Event.Doc.Type == ProjectElement.ElementType.CHARACTER_SCREEN ) )
      {
        string    nameToUse = Event.Doc.DocumentFilename ?? "New File";
        comboCharScreens.Items.Add( new Types.ComboItem( nameToUse, Event.Doc ) );
      }
      if ( ( Event.EventType == Types.ApplicationEvent.Type.DOCUMENT_INFO_REMOVED )
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
      }*/
    }



    private void btnApplyScreenSize_Click( DecentForms.ControlBase Sender )
    {
      int     newWidth  = GR.Convert.ToI32( editScreenWidth.Text );
      int     newHeight = GR.Convert.ToI32( editScreenHeight.Text );

      if ( ( newWidth > 0 )
      &&   ( newHeight > 0 ) )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoGraphicScreenSizeChange( m_GraphicScreenProject, this, m_GraphicScreenProject.ScreenWidth, m_GraphicScreenProject.ScreenHeight ) );
        SetScreenSize( newWidth, newHeight );
        SetModified();
        Redraw();
      }
    }



    private void editScreenWidth_TextChanged( object sender, EventArgs e )
    {
      int     newWidth  = GR.Convert.ToI32( editScreenWidth.Text );
      int     newHeight = GR.Convert.ToI32( editScreenHeight.Text );

      btnApplyScreenSize.Enabled = ( ( newWidth > 0 )
                                && ( newHeight > 0 ) );
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
      charEditor.DisplayPage.DrawImage( m_GraphicScreenProject.Image, 0, 0, m_SelectedChar.X * CheckBlockWidth, m_SelectedChar.Y * CheckBlockHeight, CheckBlockWidth, CheckBlockHeight );
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
      RetroDevStudio.Formats.GraphicScreenProject.ColorMappingTarget   targetIndex = (RetroDevStudio.Formats.GraphicScreenProject.ColorMappingTarget)comboColorMappingTargets.SelectedIndex;

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
      RetroDevStudio.Formats.GraphicScreenProject.ColorMappingTarget   targetIndex = (RetroDevStudio.Formats.GraphicScreenProject.ColorMappingTarget)( (ComboItem)comboColorMappingTargets.SelectedItem ).Tag;

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
      foreach ( RetroDevStudio.Formats.GraphicScreenProject.ColorMappingTarget entry in System.Enum.GetValues( typeof( RetroDevStudio.Formats.GraphicScreenProject.ColorMappingTarget ) ) )
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



    private RetroDevStudio.Formats.GraphicScreenProject.ColorMappingTarget ColorMappingFromItem( ArrangedItemEntry Item )
    {
      foreach ( RetroDevStudio.Formats.GraphicScreenProject.ColorMappingTarget entry in System.Enum.GetValues( typeof( RetroDevStudio.Formats.GraphicScreenProject.ColorMappingTarget ) ) )
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



    private void btnFullCopyToClipboard_Click( DecentForms.ControlBase Sender )
    {
      CopyImageToClipboard();
    }



    private void btnToolPaint_CheckedChanged( DecentForms.ControlBase Sender )
    {
      m_PaintTool = PaintTool.DRAW_PIXEL;
      Redraw();
    }



    private void btnToolLine_CheckedChanged( DecentForms.ControlBase Sender )
    {
      m_PaintTool = PaintTool.DRAW_FREE_LINE;
      Redraw();
    }



    private void btnToolLineDrag_CheckedChanged( DecentForms.ControlBase Sender )
    {
      m_PaintTool = PaintTool.DRAW_LINE;
      Redraw();
    }



    private void btnToolRect_CheckedChanged( DecentForms.ControlBase Sender )
    {
      m_PaintTool = PaintTool.DRAW_RECTANGLE;
      Redraw();
    }



    private void btnToolQuad_CheckedChanged( DecentForms.ControlBase Sender )
    {
      m_PaintTool = PaintTool.DRAW_BOX;
      Redraw();
    }



    private void btnToolFill_CheckedChanged( DecentForms.ControlBase Sender )
    {
      m_PaintTool = PaintTool.FLOOD_FILL;
      Redraw();
    }



    private void btnToolSelect_CheckedChanged( DecentForms.ControlBase Sender )
    {
      m_PaintTool = PaintTool.SELECT;
      Redraw();
    }



    private void btnToolValidate_CheckedChanged( DecentForms.ControlBase Sender )
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



    private void btnMirrorX_Click( DecentForms.ControlBase Sender )
    {
      MirrorH();
    }



    private void MirrorH()
    {
      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoGraphicScreenImageChange( m_GraphicScreenProject, this, m_SelectedChar.X * CheckBlockWidth, m_SelectedChar.Y * CheckBlockHeight, CheckBlockWidth, CheckBlockHeight ) );

      for ( int j = 0; j < CheckBlockHeight; ++j )
      {
        for ( int i = 0; i < 4; ++i )
        {
          uint  curColor = m_GraphicScreenProject.Image.GetPixel( m_SelectedChar.X * CheckBlockWidth + i, m_SelectedChar.Y * CheckBlockHeight + j );
          m_GraphicScreenProject.Image.SetPixel( m_SelectedChar.X * CheckBlockWidth + i, m_SelectedChar.Y * CheckBlockHeight + j, 
            m_GraphicScreenProject.Image.GetPixel( m_SelectedChar.X * CheckBlockWidth + CheckBlockWidth - 1 - i, m_SelectedChar.Y * CheckBlockHeight + j ) );
          m_GraphicScreenProject.Image.SetPixel( m_SelectedChar.X * CheckBlockWidth + CheckBlockWidth - 1 - i, m_SelectedChar.Y * CheckBlockHeight + j, curColor );
        }
      }

      charEditor.DisplayPage.DrawImage( m_GraphicScreenProject.Image, 0, 0, m_SelectedChar.X * CheckBlockWidth, m_SelectedChar.Y * CheckBlockHeight, CheckBlockWidth, CheckBlockHeight );
      charEditor.Invalidate();

      Redraw();
      pictureEditor.Invalidate();
      Modified = true;
    }



    private void btnMirrorY_Click( DecentForms.ControlBase Sender )
    {
      MirrorV();
    }



    private void MirrorV()
    {
      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoGraphicScreenImageChange( m_GraphicScreenProject, this, m_SelectedChar.X * CheckBlockWidth, m_SelectedChar.Y * CheckBlockHeight, CheckBlockWidth, CheckBlockHeight ) );

      for ( int j = 0; j < CheckBlockHeight / 2; ++j )
      {
        for ( int i = 0; i < CheckBlockWidth; ++i )
        {
          uint  curColor = m_GraphicScreenProject.Image.GetPixel( m_SelectedChar.X * CheckBlockWidth + i, m_SelectedChar.Y * CheckBlockHeight + j );
          m_GraphicScreenProject.Image.SetPixel( m_SelectedChar.X * CheckBlockWidth + i, m_SelectedChar.Y * CheckBlockHeight + j,
            m_GraphicScreenProject.Image.GetPixel( m_SelectedChar.X * CheckBlockWidth + i, m_SelectedChar.Y * CheckBlockHeight + CheckBlockHeight - 1 - j ) );
          m_GraphicScreenProject.Image.SetPixel( m_SelectedChar.X * CheckBlockWidth + i, m_SelectedChar.Y * CheckBlockHeight + CheckBlockHeight - 1 - j, curColor );
        }
      }

      charEditor.DisplayPage.DrawImage( m_GraphicScreenProject.Image, 0, 0, m_SelectedChar.X * CheckBlockWidth, m_SelectedChar.Y * CheckBlockHeight, CheckBlockWidth, CheckBlockHeight );
      charEditor.Invalidate();

      Redraw();
      pictureEditor.Invalidate();
      Modified = true;
    }



    private void btnShiftUp_Click( DecentForms.ControlBase Sender )
    {
      ShiftUp();
    }



    private void ShiftUp()
    {
      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoGraphicScreenImageChange( m_GraphicScreenProject, this, m_SelectedChar.X * CheckBlockWidth, m_SelectedChar.Y * CheckBlockHeight, CheckBlockWidth, 8 ) );

      for ( int i = 0; i < CheckBlockWidth; ++i )
      {
        uint  curColor = m_GraphicScreenProject.Image.GetPixel( m_SelectedChar.X * CheckBlockWidth + i, m_SelectedChar.Y * CheckBlockHeight );
        for ( int j = 0; j < CheckBlockHeight - 1; ++j )
        {
          
          m_GraphicScreenProject.Image.SetPixel( m_SelectedChar.X * CheckBlockWidth + i, m_SelectedChar.Y * CheckBlockHeight + j,
            m_GraphicScreenProject.Image.GetPixel( m_SelectedChar.X * CheckBlockWidth + i, m_SelectedChar.Y * CheckBlockHeight + j + 1 ) );
        }
        m_GraphicScreenProject.Image.SetPixel( m_SelectedChar.X * CheckBlockWidth + i, m_SelectedChar.Y * CheckBlockHeight + CheckBlockHeight - 1, curColor );
      }

      charEditor.DisplayPage.DrawImage( m_GraphicScreenProject.Image, 0, 0, m_SelectedChar.X * CheckBlockWidth, m_SelectedChar.Y * CheckBlockHeight, CheckBlockWidth, CheckBlockHeight );
      charEditor.Invalidate();

      Redraw();
      pictureEditor.Invalidate();
      Modified = true;
    }



    private void btnShiftDown_Click( DecentForms.ControlBase Sender )
    {
      ShiftDown();
    }



    private void ShiftDown()
    {
      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoGraphicScreenImageChange( m_GraphicScreenProject, this, m_SelectedChar.X * CheckBlockWidth, m_SelectedChar.Y * CheckBlockHeight, CheckBlockWidth, CheckBlockWidth ) );

      for ( int i = 0; i < CheckBlockWidth; ++i )
      {
        uint  curColor = m_GraphicScreenProject.Image.GetPixel( m_SelectedChar.X * CheckBlockWidth + i, m_SelectedChar.Y * CheckBlockHeight + CheckBlockHeight - 1 );
        for ( int j = 0; j < CheckBlockHeight - 1; ++j )
        {
          m_GraphicScreenProject.Image.SetPixel( m_SelectedChar.X * CheckBlockWidth + i, m_SelectedChar.Y * CheckBlockHeight + CheckBlockHeight - 1 - j,
            m_GraphicScreenProject.Image.GetPixel( m_SelectedChar.X * CheckBlockWidth + i, m_SelectedChar.Y * CheckBlockHeight + CheckBlockHeight - 2 - j ) );
        }
        m_GraphicScreenProject.Image.SetPixel( m_SelectedChar.X * CheckBlockWidth + i, m_SelectedChar.Y * CheckBlockHeight, curColor );
      }

      charEditor.DisplayPage.DrawImage( m_GraphicScreenProject.Image, 0, 0, m_SelectedChar.X * CheckBlockWidth, m_SelectedChar.Y * CheckBlockHeight, CheckBlockWidth, CheckBlockHeight );
      charEditor.Invalidate();

      Redraw();
      pictureEditor.Invalidate();
      Modified = true;
    }



    private void btnShiftRight_Click( DecentForms.ControlBase Sender )
    {
      ShiftRight();
    }



    private void ShiftRight()
    {
      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoGraphicScreenImageChange( m_GraphicScreenProject, this, m_SelectedChar.X * CheckBlockWidth, m_SelectedChar.Y * CheckBlockHeight, CheckBlockWidth, CheckBlockWidth ) );

      for ( int i = 0; i < CheckBlockHeight; ++i )
      {
        uint  curColor = m_GraphicScreenProject.Image.GetPixel( m_SelectedChar.X * CheckBlockWidth + CheckBlockWidth - 1, m_SelectedChar.Y * CheckBlockHeight + i );
        for ( int j = 0; j < CheckBlockWidth - 1; ++j )
        {
          m_GraphicScreenProject.Image.SetPixel( m_SelectedChar.X * CheckBlockWidth + CheckBlockWidth - 1 - j, m_SelectedChar.Y * CheckBlockHeight + i,
            m_GraphicScreenProject.Image.GetPixel( m_SelectedChar.X * CheckBlockWidth + CheckBlockWidth - 2 - j, m_SelectedChar.Y * CheckBlockHeight + i ) );
        }
        m_GraphicScreenProject.Image.SetPixel( m_SelectedChar.X * CheckBlockWidth, m_SelectedChar.Y * CheckBlockHeight + i, curColor );
      }

      charEditor.DisplayPage.DrawImage( m_GraphicScreenProject.Image, 0, 0, m_SelectedChar.X * CheckBlockWidth, m_SelectedChar.Y * CheckBlockHeight, CheckBlockWidth, CheckBlockHeight );
      charEditor.Invalidate();

      Redraw();
      pictureEditor.Invalidate();
      Modified = true;
    }



    private void btnShiftLeft_Click( DecentForms.ControlBase Sender )
    {
      ShiftLeft();
    }



    private void ShiftLeft()
    {
      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoGraphicScreenImageChange( m_GraphicScreenProject, this, m_SelectedChar.X * CheckBlockWidth, m_SelectedChar.Y * CheckBlockHeight, CheckBlockWidth, CheckBlockWidth ) );

      for ( int i = 0; i < CheckBlockHeight; ++i )
      {
        uint  curColor = m_GraphicScreenProject.Image.GetPixel( m_SelectedChar.X * CheckBlockWidth, m_SelectedChar.Y * CheckBlockHeight + i );
        for ( int j = 0; j < CheckBlockWidth - 1; ++j )
        {
          m_GraphicScreenProject.Image.SetPixel( m_SelectedChar.X * CheckBlockWidth + j, m_SelectedChar.Y * CheckBlockHeight + i,
            m_GraphicScreenProject.Image.GetPixel( m_SelectedChar.X * CheckBlockWidth + j + 1, m_SelectedChar.Y * CheckBlockHeight + i ) );
        }
        m_GraphicScreenProject.Image.SetPixel( m_SelectedChar.X * CheckBlockWidth + CheckBlockWidth - 1, m_SelectedChar.Y * CheckBlockHeight + i, curColor );
      }

      charEditor.DisplayPage.DrawImage( m_GraphicScreenProject.Image, 0, 0, m_SelectedChar.X * CheckBlockWidth, m_SelectedChar.Y * CheckBlockHeight, CheckBlockWidth, CheckBlockHeight );
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
        case Function.COPY:
          CopySelectedImageToClipboard();
          return true;
        case Function.PASTE:
          PasteClipboardImageToSelectedChar();
          return true;
      }
      return base.ApplyFunction( Function );
    }



    private void pictureEditor_PostPaint( GR.Image.FastImage TargetBuffer )
    {
      uint  selColor = Core.Settings.FGColor( ColorableElement.SELECTION_FRAME );

      int charOffsetX = m_SelectedChar.X - m_GraphicScreenProject.ScreenOffsetX / m_ZoomFactor;
      int charOffsetY = m_SelectedChar.Y - m_GraphicScreenProject.ScreenOffsetY / m_ZoomFactor;
      int dx = ( pictureEditor.ClientRectangle.Width * m_ZoomFactor ) / 40;
      int dy = ( pictureEditor.ClientRectangle.Height * m_ZoomFactor ) / 25;

      int pixelOffsetX = ( m_GraphicScreenProject.ScreenOffsetX % m_ZoomFactor ) * dx / m_ZoomFactor;
      int pixelOffsetY = ( m_GraphicScreenProject.ScreenOffsetY % m_ZoomFactor ) * dy / m_ZoomFactor;

      switch ( m_PaintTool )
      {
        case PaintTool.VALIDATE:
          for ( int j = 0; j < BlockHeight; ++j )
          {
            for ( int i = 0; i < BlockWidth; ++i )
            {
              if ( m_ErrornousChars[i, j] )
              {
                int sx1 = ToScreenX( i * CheckBlockWidth );
                int sx2 = ToScreenX( ( i + 1 ) * CheckBlockWidth ) - 1;
                int sy1 = ToScreenY( j * CheckBlockHeight );
                int sy2 = ToScreenY( ( j + 1 ) * CheckBlockHeight ) - 1;

                for ( int x = sx1; x <= sx2; ++x )
                {
                  TargetBuffer.SetPixel( x, sy1, (uint)( ( x & 1 ) * selColor ) );
                  TargetBuffer.SetPixel( x, sy2, (uint)( ( x & 1 ) * selColor ) );
                }
                for ( int y = sy1; y <= sy2; ++y )
                {
                  TargetBuffer.SetPixel( sx1, y, (uint)( ( y & 1 ) * selColor ) );
                  TargetBuffer.SetPixel( sx2, y, (uint)( ( y & 1 ) * selColor ) );
                }
              }
            }
          }
          if ( m_SelectedChar.X != -1 )
          {
            int sx1 = ToScreenX( m_SelectedChar.X * CheckBlockWidth );
            int sx2 = ToScreenX( ( m_SelectedChar.X + 1 ) * CheckBlockWidth );
            int sy1 = ToScreenY( m_SelectedChar.Y * CheckBlockHeight );
            int sy2 = ToScreenY( ( m_SelectedChar.Y + 1 ) * CheckBlockHeight );

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

            int sx1 = ToScreenX( x1 );
            int sx2 = ToScreenX( x2 + 1 );
            int sy1 = ToScreenY( y1 );
            int sy2 = ToScreenY( y2 + 1 );

            TargetBuffer.Rectangle( sx1, sy1, sx2 - sx1, sy2 - sy1, selColor );
          }
          if ( m_Selection.Width > 0 )
          {
            int sx1 = ToScreenX( m_Selection.X );
            int sx2 = ToScreenX( m_Selection.X + m_Selection.Width );
            int sy1 = ToScreenY( m_Selection.Y );
            int sy2 = ToScreenY( m_Selection.Y + m_Selection.Height );

            TargetBuffer.Rectangle( sx1, sy1, sx2 - sx1, sy2 - sy1, selColor );
          }
          break;
      }

      // draw outside area
      int     fillWidth = 0;
      int     fillHeight = 0;

      //if ( m_GraphicScreenProject.Image.Width < pictureEditor.DisplayPage.Width )

      if ( m_GraphicScreenProject.ScreenWidth - m_GraphicScreenProject.ScreenOffsetX / m_ZoomFactor < pictureEditor.DisplayPage.Width )
      {
        fillWidth = pictureEditor.DisplayPage.Width - ( m_GraphicScreenProject.ScreenWidth - m_GraphicScreenProject.ScreenOffsetX / m_ZoomFactor );
      }
      if ( m_GraphicScreenProject.ScreenHeight - m_GraphicScreenProject.ScreenOffsetY / m_ZoomFactor < pictureEditor.DisplayPage.Height )
      {
        fillHeight = pictureEditor.DisplayPage.Height - ( m_GraphicScreenProject.ScreenHeight - m_GraphicScreenProject.ScreenOffsetY / m_ZoomFactor );
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



    private int ToScreenX( int X )
    {
      int   leftX = m_GraphicScreenProject.ScreenOffsetX / m_ZoomFactor;

      return ( ( X - leftX ) * pictureEditor.ClientRectangle.Width * m_ZoomFactor ) / 320;
    }



    private int ToScreenY( int Y )
    {
      int   leftY = m_GraphicScreenProject.ScreenOffsetY / m_ZoomFactor;

      return ( ( Y - leftY ) * pictureEditor.ClientRectangle.Height * m_ZoomFactor ) / 200;
    }



    private int ToLocalX( int X )
    {
      return ( ( X / m_ZoomFactor ) / ( pictureEditor.ClientRectangle.Width / pictureEditor.DisplayPage.Width / m_ZoomFactor ) ) + m_GraphicScreenProject.ScreenOffsetX / m_ZoomFactor;
    }



    private int ToLocalY( int Y )
    {
      return ( ( Y / m_ZoomFactor ) / ( pictureEditor.ClientRectangle.Height / pictureEditor.DisplayPage.Height / m_ZoomFactor ) ) + m_GraphicScreenProject.ScreenOffsetY / m_ZoomFactor;
    }



    private void btnClearScreen_Click( DecentForms.ControlBase Sender )
    {
      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoGraphicScreenImageChange( m_GraphicScreenProject, this, 0, 0, m_GraphicScreenProject.ScreenWidth, m_GraphicScreenProject.ScreenHeight ) );
      m_GraphicScreenProject.Image.Box( 0, 0, m_GraphicScreenProject.ScreenWidth, m_GraphicScreenProject.ScreenHeight, 0 );
      Redraw();
      SetModified();
    }



    public void ColorValuesChanged()
    {
      UpdateCurrentColorMapping();

      _ColorChooserDlg.ColorChanged( Types.ColorType.BACKGROUND, m_GraphicScreenProject.Colors.BackgroundColor );
      _ColorChooserDlg.ColorChanged( Types.ColorType.MULTICOLOR_1, m_GraphicScreenProject.Colors.MultiColor1 );
      _ColorChooserDlg.ColorChanged( Types.ColorType.MULTICOLOR_2, m_GraphicScreenProject.Colors.MultiColor2 );

      PaletteManager.ApplyPalette( m_GraphicScreenProject.Image, m_GraphicScreenProject.Colors.Palette );
      PaletteManager.ApplyPalette( pictureEditor.DisplayPage, m_GraphicScreenProject.Colors.Palette );
      PaletteManager.ApplyPalette( charEditor.DisplayPage, m_GraphicScreenProject.Colors.Palette );
      _colorPickerDlg.UpdatePalette( m_GraphicScreenProject.Colors.Palette );

      Redraw();
    }



    public override bool CopyPossible
    {
      get
      {
        return pictureEditor.Focused;
      }
    }



    public override bool PastePossible
    {
      get
      {
        return pictureEditor.Focused;
      }
    }



    private void btnZoomIn_Click( DecentForms.ControlBase Sender )
    {
      ZoomIn();
    }



    private void ZoomIn()
    {
      if ( m_ZoomFactor < 8 )
      {
        int   totalWidth = Math.Max( 320, m_GraphicScreenProject.ScreenWidth );
        int   totalHeight = Math.Max( 200, m_GraphicScreenProject.ScreenHeight );

        int   centerX = ToLocalX( totalWidth );
        int   centerY = ToLocalY( totalHeight );

        int   leftX = ToLocalX( 0 );
        int   leftY = ToLocalY( 0 );

        m_ZoomFactor *= 2;

        int   screenX = ToScreenX( centerX );
        int   screenY = ToScreenY( centerY );

        btnZoomIn.Enabled = ( m_ZoomFactor < 8 );
        btnZoomOut.Enabled = true;

        pictureEditor.SetImageSize( totalWidth / m_ZoomFactor, totalHeight / m_ZoomFactor );
        AdjustScrollbars();

        int   newLeftX = ( centerX - leftX ) / m_ZoomFactor;

        int   oldValueH = newLeftX / m_ZoomFactor;
        oldValueH = Math.Max( 0, oldValueH );
        oldValueH = Math.Min( screenHScroll.Maximum, oldValueH );
        screenHScroll.Value = oldValueH;
        screenHScroll_Scroll( screenHScroll );

        //screenVScroll.Value += centerY * m_ZoomFactor;

        Redraw();
      }
    }



    private void btnZoomOut_Click( DecentForms.ControlBase Sender )
    {
      ZoomOut();
    }



    private void ZoomOut()
    {
      if ( m_ZoomFactor > 1 )
      {
        m_ZoomFactor /= 2;
        btnZoomIn.Enabled = true;
        btnZoomOut.Enabled = ( m_ZoomFactor > 1 );
        int   totalWidth = Math.Max( 320, m_GraphicScreenProject.ScreenWidth );
        int   totalHeight = Math.Max( 200, m_GraphicScreenProject.ScreenHeight );
        pictureEditor.SetImageSize( totalWidth / m_ZoomFactor, totalHeight / m_ZoomFactor );
        AdjustScrollbars();
        Redraw();
      }
    }



    private void pictureEditor_PreviewKeyDown( object sender, PreviewKeyDownEventArgs e )
    {
      if ( e.KeyData == Keys.Space )
      {
        if ( !m_DragViewModeActiveBySpace )
        {
          m_DragViewModeActiveBySpace = true;
          pictureEditor.Cursor = Core.MainForm.CursorHand;
        }
      }
    }



    private void PictureEditor_KeyUp( object sender, KeyEventArgs e )
    {
      if ( ( e.KeyData == Keys.Space )
      &&   ( m_DragViewModeActiveBySpace ) )
      {
        m_DragViewModeActiveBySpace = false;
        pictureEditor.Cursor = Cursors.Default;
      }
    }



    private void PictureEditor_LostFocus( object sender, EventArgs e )
    {
      if ( m_DragViewModeActiveBySpace )
      {
        m_DragViewModeActiveBySpace = false;
        pictureEditor.Cursor = Cursors.Default;
      }
    }



    public override void OnApplicationEvent( ApplicationEvent Event )
    {
      switch ( Event.EventType )
      {
        case ApplicationEvent.Type.DEFAULT_PALETTE_CHANGED:
          {
            bool  prevModified = Modified;

            var palType = GR.EnumHelper.GetAttributeOfType<PaletteTypeAttribute>( m_GraphicScreenProject.SelectedCheckType ).PalType;
            var pal     = Core.Settings.Palettes[palType][0];

            if ( !string.IsNullOrEmpty( Event.OriginalValue ) )
            {
              Core.Imaging.ApplyPalette( (PaletteType)Enum.Parse( typeof( PaletteType ), Event.OriginalValue, true ),
                                         palType,
                                         m_GraphicScreenProject.Colors );
            }
            else
            {
              Core.Imaging.ApplyPalette( palType,
                                         palType,
                                         m_GraphicScreenProject.Colors );

            }

            PaletteManager.ApplyPalette( m_GraphicScreenProject.Image, pal );
            PaletteManager.ApplyPalette( pictureEditor.DisplayPage, pal );
            PaletteManager.ApplyPalette( charEditor.DisplayPage, pal );
            _colorPickerDlg.UpdatePalette( pal );

            Redraw();

            Modified = prevModified;
          }
          break;
      }
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
      _ImportForm         = (ImportGraphicScreenFormBase)Activator.CreateInstance( item.second, new object[] { Core } );
      _ImportForm.Parent  = panelImport;
      _ImportForm.Size    = panelImport.ClientSize;
      _ImportForm.CreateControl();
    }



    public void DataImported()
    {
      // force refresh of color indices
      charEditor.Invalidate();

      ChangeColorChooserDialog();
      ChangeColorPickerDialog();
      Redraw();
      SetModified();
    }



    private void btnImport_Click( DecentForms.ControlBase Sender )
    {
      var undo = new Undo.UndoGraphicScreenImageChange( m_GraphicScreenProject, this, 0, 0, m_GraphicScreenProject.ScreenWidth, m_GraphicScreenProject.ScreenHeight );
      var undo2 = new Undo.UndoGraphicScreenValuesChange( m_GraphicScreenProject, this );

      int numUndos = DocumentInfo.UndoManager.NumUndos;
      if ( _ImportForm.HandleImport( m_GraphicScreenProject, this ) )
      {
        DocumentInfo.UndoManager.AddUndoTask( undo, numUndos == DocumentInfo.UndoManager.NumUndos );
        DocumentInfo.UndoManager.AddGroupedUndoTask( undo2 );
        SetModified();
      }
    }



    private void btnExport_Click( DecentForms.ControlBase Sender )
    {
      Debug.Log( "CheckExportType a" );
      CheckExportType();
      Debug.Log( "CheckExportType b" );

      var exportInfo = new ExportGraphicScreenInfo()
      {
        Project           = m_GraphicScreenProject,
        BlockWidth        = BlockWidth,
        BlockHeight       = BlockHeight,
        CheckBlockWidth   = CheckBlockWidth,
        CheckBlockHeight  = CheckBlockHeight,
        Image             = m_GraphicScreenProject.Image,
        Chars             = m_Chars,
        ErrornousChars    = m_ErrornousChars
      };

      editExportOutput.Text = "";
      editExportOutput.Font = m_DefaultOutputFont;
      Debug.Log( "CheckExportType c" );
      if ( !_ExportForm.HandleExport( this, exportInfo, editExportOutput, DocumentInfo ) )
      {
        // if export as charset was chosen and failed with checks, redraw
        Redraw();
      }
    }



    private void comboExportMethod_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( _ExportForm != null )
      {
        _ExportForm.Dispose();
        _ExportForm = null;
      }

      editExportOutput.Text = "";
      editExportOutput.Font = m_DefaultOutputFont;

      var item = (GR.Generic.Tupel<string, Type>)comboExportMethod.SelectedItem;
      if ( ( item == null )
      ||   ( item.second == null ) )
      {
        return;
      }
      _ExportForm = (ExportGraphicScreenFormBase)Activator.CreateInstance( item.second, new object[] { Core } );
      _ExportForm.Parent = panelExport;
      _ExportForm.CreateControl();
    }



    private void editExportOutput_PreviewKeyDown( object sender, PreviewKeyDownEventArgs e )
    {
      if ( e.KeyData == ( Keys.A | Keys.Control ) )
      {
        editExportOutput.SelectAll();
      }
    }



  }
}

