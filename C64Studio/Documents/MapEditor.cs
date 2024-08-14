using RetroDevStudio.Types;
using RetroDevStudio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using RetroDevStudio.Formats;
using RetroDevStudio.Controls;
using System.Runtime.InteropServices;

namespace RetroDevStudio.Documents
{
  public partial class MapEditor : BaseDocument
  {
    private enum ToolMode
    {
      SINGLE_TILE,
      RECTANGLE,
      FILLED_RECTANGLE,
      FILL,
      SELECT
    };



    private Formats.MapProject          m_MapProject = new RetroDevStudio.Formats.MapProject();

    private Formats.MapProject.Map      m_CurrentMap = null;

    private Formats.MapProject.Tile     m_CurrentEditedTile = null;
    private Formats.MapProject.TileChar m_CurrentTileChar = null;

    private Formats.MapProject.Tile     m_CurrentEditorTile = null;

    private byte                        m_CurrentChar = 0;
    private byte                        m_CurrentColor = 1;

    private GR.Image.MemoryImage        m_Image = new GR.Image.MemoryImage( 320, 200, GR.Drawing.PixelFormat.Format32bppRgb );

    private int                         m_CurEditorOffsetX = 0;
    private int                         m_CurEditorOffsetY = 0;

    private ToolMode                    m_ToolMode = ToolMode.SINGLE_TILE;

    private bool[,]                     m_SelectedTiles = new bool[20, 12];

    private bool                        m_MouseButtonReleased = false;
    private System.Drawing.Point        m_MousePos;

    private System.Drawing.Point        m_DragStartPos = new System.Drawing.Point();
    private System.Drawing.Point        m_DragEndPos = new System.Drawing.Point();
    private System.Drawing.Point        m_LastDragEndPos = new System.Drawing.Point( -1, -1 );

    private List<GR.Generic.Tupel<bool,int>>          m_FloatingSelection = null;
    private System.Drawing.Size                       m_FloatingSelectionSize;
    private System.Drawing.Point                      m_FloatingSelectionPos;

    private ExportMapFormBase           m_ExportForm = null;
    private ImportMapFormBase           m_ImportForm = null;



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



    public MapEditor( StudioCore Core )
    {
      this.Core = Core;

      DocumentInfo.Type = ProjectElement.ElementType.MAP_EDITOR;
      DocumentInfo.UndoManager.MainForm = Core.MainForm;

      m_IsSaveable = true;
      InitializeComponent();
      SuspendLayout();

      characterEditor.Core = Core;

      GR.Image.DPIHandler.ResizeControlsForDPI( this );

      characterEditor.UndoManager = DocumentInfo.UndoManager;
      characterEditor.Core = Core;

      comboExportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "as assembly", typeof( ExportMapAsAssembly ) ) );
      comboExportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "to binary file", typeof( ExportMapAsBinaryFile ) ) );
      comboExportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "charset to charset project", typeof( ExportMapCharsetAsCharset ) ) );
      comboExportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "charset to binary file", typeof( ExportMapCharsetAsBinaryFile ) ) );
      comboExportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "map to char screen project", typeof( ExportMapAsCharscreen ) ) );
      comboExportMethod.SelectedIndex = 0;

      comboImportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "charset from character set file", typeof( ImportMapCharsetFromCharsetFile ) ) );
      comboImportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "map/charset from binary/charpad file", typeof( ImportMapFromBinaryFile ) ) );
      comboImportMethod.SelectedIndex = 0;

      foreach ( MapExportType exportType in Enum.GetValues( typeof( MapExportType ) ) )
      {
        comboExportData.Items.Add( new GR.Generic.Tupel<string, MapExportType>( GR.EnumHelper.GetDescription( exportType ), exportType ) );
      }

      pictureEditor.MouseWheel += pictureEditor_MouseWheel;
      pictureEditor.DisplayPage.Create( 320, 200, GR.Drawing.PixelFormat.Format32bppRgb );
      pictureEditor.PostPaint += PictureEditor_PostPaint;
      pictureTileDisplay.ClientSize = new System.Drawing.Size( 256, 256 );
      pictureTileDisplay.DisplayPage.Create( 128, 128, GR.Drawing.PixelFormat.Format32bppRgb );
      panelCharacters.PixelFormat = GR.Drawing.PixelFormat.Format32bppRgb;
      panelCharacters.SetDisplaySize( 128, 128 );
      panelCharacters.DisplayPage.Create( 128, 128, GR.Drawing.PixelFormat.Format32bppRgb );

      panelCharColors.DisplayPage.Create( 128, 8, GR.Drawing.PixelFormat.Format32bppRgb );

      m_Image.Create( 320, 200, GR.Drawing.PixelFormat.Format32bppRgb );

      Palette   pal = Core.MainForm.ActivePalette;

      PaletteManager.ApplyPalette( pictureEditor.DisplayPage );
      PaletteManager.ApplyPalette( pictureTileDisplay.DisplayPage );
      PaletteManager.ApplyPalette( panelCharacters.DisplayPage );
      PaletteManager.ApplyPalette( m_Image );
      PaletteManager.ApplyPalette( panelCharColors.DisplayPage );

      comboMapMultiColor1.Items.Add( "From charset" );
      comboMapMultiColor2.Items.Add( "From charset" );
      comboMapBGColor.Items.Add( "Project" );
      comboMapAlternativeBGColor4.Items.Add( "Project" );
      for ( int i = 0; i < 16; ++i )
      {
        comboTileBackground.Items.Add( i.ToString( "d2" ) );
        comboTileMulticolor1.Items.Add( i.ToString( "d2" ) );
        comboTileMulticolor2.Items.Add( i.ToString( "d2" ) );
        comboTileBGColor4.Items.Add( i.ToString( "d2" ) );
        comboMapMultiColor1.Items.Add( i.ToString( "d2" ) );
        comboMapMultiColor2.Items.Add( i.ToString( "d2" ) );
        comboMapBGColor.Items.Add( i.ToString( "d2" ) );
        comboMapAlternativeBGColor4.Items.Add( i.ToString( "d2" ) );
      }
      comboTileBackground.SelectedIndex = 0;
      comboTileMulticolor1.SelectedIndex = 0;
      comboTileMulticolor2.SelectedIndex = 0;
      comboTileBGColor4.SelectedIndex = 0;
      comboMapMultiColor1.SelectedIndex = 0;
      comboMapMultiColor2.SelectedIndex = 0;
      comboMapBGColor.SelectedIndex = 0;
      comboMapAlternativeBGColor4.SelectedIndex = 0;

      comboExportOrientation.SelectedIndex = 0;
      comboExportData.SelectedIndex = 0;

      foreach ( TextMode mode in Enum.GetValues( typeof( TextMode ) ) )
      {
        if ( mode != TextMode.UNKNOWN )
        {
          comboMapProjectMode.Items.Add( GR.EnumHelper.GetDescription( mode ) );
        }
      }
      comboMapProjectMode.SelectedIndex = 0;

      comboMapAlternativeMode.Items.Add( "From Project" );
      foreach ( TextCharMode mode in Enum.GetValues( typeof( TextCharMode ) ) )
      {
        if ( mode != TextCharMode.UNKNOWN )
        {
          comboMapAlternativeMode.Items.Add( GR.EnumHelper.GetDescription( mode ) );
        }
      }
      comboMapAlternativeMode.SelectedIndex = 0;

      Core.MainForm.ApplicationEvent += new MainForm.ApplicationEventHandler( MainForm_ApplicationEvent );

      for ( int i = 0; i < 256; ++i )
      {
        RebuildCharImage( i );
        panelCharacters.Items.Add( i.ToString(), m_MapProject.Charset.Characters[i].Tile.Image );
      }

      characterEditor.CharsetUpdated( m_MapProject.Charset );
      Modified = false;

      ResumeLayout();
    }



    private int ScreenCharWidth
    {
      get
      {
        return Lookup.ScreenWidthInCharacters( m_MapProject.Mode );
      }
    }



    private int ScreenCharHeight
    {
      get
      {
        return Lookup.ScreenHeightInCharacters( m_MapProject.Mode );
      }
    }



    public override bool ApplyFunction( Function Function )
    {
      if ( characterEditor.EditorFocused )
      {
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
      }
      else if ( ( pictureEditor.Focused )
      ||        ( mapHScroll.Focused )
      ||        ( mapVScroll.Focused ) )
      {
        switch ( Function )
        {
          case Function.COPY:
            if ( m_ToolMode == ToolMode.SELECT )
            {
              CopyToClipboard();
              return true;
            }
            break;
          case Function.PASTE:
            PasteFromClipboard();
            return true;
        }
      }
      return base.ApplyFunction( Function );
    }



    public override bool CopyPossible
    {
      get
      {
        return ( ( characterEditor.EditorFocused )
        ||       ( ( m_ToolMode == ToolMode.SELECT )
        &&         ( ( pictureEditor.Focused )
        ||           ( mapHScroll.Focused )
        ||           ( mapVScroll.Focused ) ) ) );
      }
    }



    public override bool PastePossible
    {
      get
      {
        return ( ( characterEditor.EditorFocused )
        ||       ( pictureEditor.Focused )
        ||       ( mapHScroll.Focused )
        ||       ( mapVScroll.Focused ) );
      }
    }



    private void PictureEditor_PostPaint( GR.Image.FastImage TargetBuffer )
    {
      if ( m_MapProject.ShowGrid )
      {
        if ( m_CurrentMap == null )
        {
          pictureEditor.Invalidate();
          return;
        }

        int offsetX = m_CurEditorOffsetX;
        int offsetY = m_CurEditorOffsetY;

        int x1 = offsetX;
        int x2 = offsetX + m_CurrentMap.TileSpacingX * m_CurrentMap.Tiles.Width;
        int y1 = offsetY;
        int y2 = offsetY + m_CurrentMap.TileSpacingY * m_CurrentMap.Tiles.Height;

        if ( x2 - offsetX > TargetBuffer.Width / ( m_CurrentMap.TileSpacingX * 16 ) )
        {
          x2 = TargetBuffer.Width / ( m_CurrentMap.TileSpacingX * 16 ) + offsetX;
        }
        if ( y2 - offsetY > TargetBuffer.Height / ( m_CurrentMap.TileSpacingY * 16 ) )
        {
          y2 = TargetBuffer.Height / ( m_CurrentMap.TileSpacingY * 16 ) + offsetY;
        }

        for ( int y = y1; y <= y2; ++y )
        {
          for ( int x = x1; x <= x2; ++x )
          {
            TargetBuffer.Rectangle( ( x - offsetX ) * m_CurrentMap.TileSpacingX * 16, ( y - offsetY ) * m_CurrentMap.TileSpacingY * 16, m_CurrentMap.TileSpacingX * 16, m_CurrentMap.TileSpacingY * 16, 0xffffffff );
          }
        }
      }

      if ( ( m_CurrentMap == null )
      ||   ( m_ToolMode != ToolMode.SELECT ) )
      {
        return;
      }

      // draw selection
      uint    selectionColor = Core.Settings.FGColor( ColorableElement.SELECTION_FRAME );
      for ( int x = 0; x < m_CurrentMap.Tiles.Width; ++x )
      {
        for ( int y = 0; y < m_CurrentMap.Tiles.Height; ++y )
        {
          if ( m_SelectedTiles[x, y] )
          {
            int  sx1 = ( x - m_CurEditorOffsetX ) * m_CurrentMap.TileSpacingX * pictureEditor.ClientRectangle.Width / ScreenCharWidth;
            int  sx2 = ( x + 1 - m_CurEditorOffsetX ) * m_CurrentMap.TileSpacingX * pictureEditor.ClientRectangle.Width / ScreenCharWidth;
            int  sy1 = ( y - m_CurEditorOffsetY ) * m_CurrentMap.TileSpacingY * pictureEditor.ClientRectangle.Height / ScreenCharHeight;
            int  sy2 = ( y + 1 - m_CurEditorOffsetY ) * m_CurrentMap.TileSpacingY * pictureEditor.ClientRectangle.Height / ScreenCharHeight;

            --sx2;
            --sy2;

            if ( ( y == 0 )
            ||   ( !m_SelectedTiles[x, y - 1] ) )
            {
              for ( int i = sx1; i <= sx2; ++i )
              {
                TargetBuffer.SetPixel( i, sy1, selectionColor );
              }
            }
            if ( ( y == m_SelectedTiles.GetUpperBound( 1 ) )
            ||   ( !m_SelectedTiles[x, y + 1] ) )
            {
              for ( int i = sx1; i <= sx2; ++i )
              {
                TargetBuffer.SetPixel( i, sy2, selectionColor );
              }
            }
            if ( ( x == 0 )
            ||   ( !m_SelectedTiles[x - 1, y] ) )
            {
              for ( int i = sy1; i <= sy2; ++i )
              {
                TargetBuffer.SetPixel( sx1, i, selectionColor );
              }
            }
            if ( ( x == m_SelectedTiles.GetUpperBound( 0 ) )
            ||   ( !m_SelectedTiles[x + 1, y] ) )
            {
              for ( int i = sy1; i <= sy2; ++i )
              {
                TargetBuffer.SetPixel( sx2, i, selectionColor );
              }
            }
          }
        }
      }

      // current dragged selection
      if ( ( m_ToolMode == ToolMode.SELECT )
      &&   ( m_LastDragEndPos.X != -1 ) )
      {
        System.Drawing.Point    o1, o2;

        CalcRect( m_DragStartPos, m_LastDragEndPos, out o1, out o2 );

        TargetBuffer.Rectangle( ( o1.X - m_CurEditorOffsetX ) * m_CurrentMap.TileSpacingX * pictureEditor.ClientRectangle.Width / ScreenCharWidth,
                                ( o1.Y - m_CurEditorOffsetY ) * m_CurrentMap.TileSpacingY * pictureEditor.ClientRectangle.Height / ScreenCharHeight,
                                ( o2.X - o1.X + 1 ) * m_CurrentMap.TileSpacingX * pictureEditor.ClientRectangle.Width / ScreenCharWidth, 
                                ( o2.Y - o1.Y + 1 ) * m_CurrentMap.TileSpacingY * pictureEditor.ClientRectangle.Height / ScreenCharHeight,
                                selectionColor );
      }

    }



    void pictureEditor_MouseWheel( object sender, MouseEventArgs e )
    {
      int numberOfLinesToMove = e.Delta * SystemInformation.MouseWheelScrollLines / 120;

      if ( mapVScroll.Enabled )
      {
        int     oldValue = mapVScroll.Value;
        int     newValue = oldValue - numberOfLinesToMove;
        if ( newValue < 0 )
        {
          newValue = 0;
        }
        if ( newValue > mapVScroll.Maximum )
        {
          newValue = mapVScroll.Maximum;
        }
        if ( oldValue != newValue )
        {
          mapVScroll.Value = newValue;
          mapVScroll_Scroll( mapVScroll );
        }
      }
    }



    void MainForm_ApplicationEvent( RetroDevStudio.Types.ApplicationEvent Event )
    {
    }



    protected override void OnClosed( EventArgs e )
    {
      Core.MainForm.ApplicationEvent -= MainForm_ApplicationEvent;
      base.OnClosed( e );
    }



    void RebuildCharImage( int CharIndex )
    {
      Displayer.CharacterDisplayer.DisplayChar( m_MapProject.Charset,
                                                m_MapProject.Charset.Colors.Palette,
                                                CharIndex, m_MapProject.Charset.Characters[CharIndex].Tile.Image, 0, 0,
                                                m_MapProject.Charset.Characters[CharIndex].Tile.CustomColor,
                                                m_MapProject.BackgroundColor,
                                                m_MapProject.MultiColor1,
                                                m_MapProject.MultiColor2,
                                                m_MapProject.BGColor4 );

      if ( CharIndex < panelCharacters.Items.Count )
      {
        panelCharacters.Items[CharIndex].MemoryImage = m_MapProject.Charset.Characters[CharIndex].Tile.Image;
      }
    }



    void DrawCharImage( GR.Image.FastImage TargetImage, int X, int Y, byte Char, byte Color )
    {
      int bgColor = m_MapProject.BackgroundColor;
      int mColor1 = m_MapProject.MultiColor1;
      int mColor2 = m_MapProject.MultiColor2;
      int bgColor4 = m_MapProject.BGColor4;
      if ( m_CurrentMap != null )
      {
        if ( m_CurrentMap.AlternativeBackgroundColor != -1 )
        {
          bgColor = m_CurrentMap.AlternativeBackgroundColor;
        }
        if ( m_CurrentMap.AlternativeMultiColor1 != -1 )
        {
          mColor1 = m_CurrentMap.AlternativeMultiColor1;
        }
        if ( m_CurrentMap.AlternativeMultiColor2 != -1 )
        {
          mColor2 = m_CurrentMap.AlternativeMultiColor2;
        }
        if ( m_CurrentMap.AlternativeBGColor4 != -1 )
        {
          bgColor4 = m_CurrentMap.AlternativeBGColor4;
        }
      }
      Displayer.CharacterDisplayer.DisplayChar( m_MapProject.Charset, m_MapProject.Charset.Colors.Palette, Char, TargetImage, X, Y, Color, bgColor, mColor1, mColor2, bgColor4 );
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

      Core.Theming.DrawSingleColorComboBox( combo, e, m_MapProject.Charset.Colors.Palette );
    }



    private void comboMulticolor_DrawItem( object sender, DrawItemEventArgs e )
    {
      ComboBox combo = (ComboBox)sender;

      Core.Theming.DrawMultiColorComboBox( combo, e, m_MapProject.Charset.Colors.Palette );
    }



    private void pictureEditor_MouseDown( object sender, MouseEventArgs e )
    {
      pictureEditor.Focus();
      HandleMouseOnEditor( e.X, e.Y, e.Button );
    }



    private void CalcRect( System.Drawing.Point In1, System.Drawing.Point In2, out System.Drawing.Point P1, out System.Drawing.Point P2 )
    {
      P1 = new System.Drawing.Point();
      P2 = new System.Drawing.Point();

      if ( In1.X <= In2.X )
      {
        P1.X = In1.X;
        P2.X = In2.X;
      }
      else
      {
        P1.X = In2.X;
        P2.X = In1.X;
      }
      if ( In1.Y <= In2.Y )
      {
        P1.Y = In1.Y;
        P2.Y = In2.Y;
      }
      else
      {
        P1.Y = In2.Y;
        P2.Y = In1.Y;
      }
    }



    private void InsertFloatingSelection()
    {
      if ( m_FloatingSelection == null )
      {
        return;
      }

      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoMapTilesChange( this, m_CurrentMap, m_MousePos.X + m_CurEditorOffsetX, m_MousePos.Y + m_CurEditorOffsetY, m_FloatingSelectionSize.Width, m_FloatingSelectionSize.Height ) );

      for ( int j = 0; j < m_FloatingSelectionSize.Height; ++j )
      {
        for ( int i = 0; i < m_FloatingSelectionSize.Width; ++i )
        {
          var selectionChar = m_FloatingSelection[i + j * m_FloatingSelectionSize.Width];
          if ( selectionChar.first )
          {
            m_CurrentMap.Tiles[m_MousePos.X + m_CurEditorOffsetX + i, m_MousePos.Y + m_CurEditorOffsetY + j] = selectionChar.second;

            DrawTile( ( m_MousePos.X + i ) * 8 * m_CurrentMap.TileSpacingX,
                      ( m_MousePos.Y + j ) * 8 * m_CurrentMap.TileSpacingY,
                      selectionChar.second );

            pictureEditor.DisplayPage.DrawTo( m_Image,
                                              ( m_MousePos.X + i ) * 8 * m_CurrentMap.TileSpacingX,
                                              ( m_MousePos.Y + j ) * 8 * m_CurrentMap.TileSpacingY,
                                              ( m_MousePos.X + i ) * 8 * m_CurrentMap.TileSpacingX,
                                              ( m_MousePos.Y + j ) * 8 * m_CurrentMap.TileSpacingY,
                                              8 * m_CurrentMap.TileSpacingX, 8 * m_CurrentMap.TileSpacingY );
            pictureEditor.Invalidate( new System.Drawing.Rectangle( ( m_MousePos.X + i ) * 8 * m_CurrentMap.TileSpacingX,
                                                                    ( m_MousePos.Y + j ) * 8 * m_CurrentMap.TileSpacingY,
                                                                    8 * m_CurrentMap.TileSpacingX, 8 * m_CurrentMap.TileSpacingY ) );
          }
        }
      }
      m_FloatingSelection = null;
      Redraw();
      Modified = true;
    }



    private void FillContent( int X, int Y )
    {
      List<System.Drawing.Point>      pointsToCheck = new List<System.Drawing.Point>();

      pointsToCheck.Add( new System.Drawing.Point( X, Y ) );

      int     tileToFill = m_CurrentMap.Tiles[X,Y];
      if ( tileToFill == m_CurrentEditorTile.Index )
      {
        return;
      }

      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoMapTilesChange( this, m_CurrentMap, 0, 0, m_CurrentMap.Tiles.Width, m_CurrentMap.Tiles.Height ) );

      while ( pointsToCheck.Count != 0 )
      {
        System.Drawing.Point    point = pointsToCheck[pointsToCheck.Count - 1];
        pointsToCheck.RemoveAt( pointsToCheck.Count - 1 );

        if ( m_CurrentMap.Tiles[point.X, point.Y] != m_CurrentEditorTile.Index )
        {
          DrawTile( point.X, point.Y, m_CurrentEditorTile.Index );
          m_CurrentMap.Tiles[point.X, point.Y] = m_CurrentEditorTile.Index;

          if ( ( point.X > 0 )
          &&   ( m_CurrentMap.Tiles[point.X - 1, point.Y] == tileToFill ) )
          {
            pointsToCheck.Add( new System.Drawing.Point( point.X - 1, point.Y ) );
          }
          if ( ( point.X + 1 < m_CurrentMap.Tiles.Width )
          &&   ( m_CurrentMap.Tiles[point.X + 1, point.Y] == tileToFill ) )
          {
            pointsToCheck.Add( new System.Drawing.Point( point.X + 1, point.Y ) );
          }
          if ( ( point.Y > 0 )
          &&   ( m_CurrentMap.Tiles[point.X, point.Y - 1] == tileToFill ) )
          {
            pointsToCheck.Add( new System.Drawing.Point( point.X, point.Y - 1 ) );
          }
          if ( ( point.Y + 1 < m_CurrentMap.Tiles.Height )
          &&   ( m_CurrentMap.Tiles[point.X, point.Y + 1] == tileToFill ) )
          {
            pointsToCheck.Add( new System.Drawing.Point( point.X, point.Y + 1 ) );
          }
        }
      }
      Modified = true;
      RedrawMap();
      Redraw();
    }



    private void HandleMouseOnEditor( int X, int Y, MouseButtons Buttons )
    {
      if ( m_CurrentMap == null )
      {
        labelEditInfo.Text = "";
        return;
      }
      int     charX = X / ( pictureEditor.ClientRectangle.Width / ScreenCharWidth );
      int     charY = Y / ( pictureEditor.ClientRectangle.Height / ScreenCharHeight );

      m_MousePos.X = charX / m_CurrentMap.TileSpacingX;
      m_MousePos.Y = charY / m_CurrentMap.TileSpacingY;
      if ( m_FloatingSelection != null )
      {
        if ( m_MousePos != m_FloatingSelectionPos )
        {
          m_FloatingSelectionPos = m_MousePos;
          Redraw();
          pictureEditor.Invalidate();
        }
      }

      int offsetX = m_CurEditorOffsetX;
      int offsetY = m_CurEditorOffsetY;

      if ( ( charX < 0 )
      ||   ( charX >= ScreenCharWidth )
      ||   ( charY < 0 )
      ||   ( charY >= ScreenCharHeight ) )
      {
        return;
      }

      int trueX = charX / m_CurrentMap.TileSpacingX;
      int trueY = charY / m_CurrentMap.TileSpacingY;

      if ( ( trueX + offsetX < 0 )
      ||   ( trueX + offsetX >= m_CurrentMap.Tiles.Width )
      ||   ( trueY + offsetY < 0 )
      ||   ( trueY + offsetY >= m_CurrentMap.Tiles.Height ) )
      {
        return;
      }

      int mapPos = trueX + offsetX + ( trueY + offsetY ) * m_CurrentMap.Tiles.Width;
      labelEditInfo.Text = "X: " + ( trueX + offsetX ).ToString() + " Y:" + ( trueY + offsetY ).ToString() + " Abs:" + mapPos.ToString() + "/$" + mapPos.ToString( "X2" );

      if ( ( Buttons & MouseButtons.Left ) == 0 )
      {
        m_MouseButtonReleased = true;

        switch ( m_ToolMode )
        {
          case ToolMode.RECTANGLE:
          case ToolMode.FILLED_RECTANGLE:
            if ( m_LastDragEndPos.X != -1 )
            {
              m_LastDragEndPos.X = -1;
              m_LastDragEndPos.Y = -1;

              System.Drawing.Point    p1, p2;

              CalcRect( m_DragStartPos, m_DragEndPos, out p1, out p2 );

              DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoMapTilesChange( this, m_CurrentMap, p1.X, p1.Y, p2.X - p1.X + 1, p2.Y - p1.Y + 1 ) );

              if ( m_ToolMode == ToolMode.RECTANGLE )
              {
                for ( int x = p1.X; x <= p2.X; ++x )
                {
                  DrawTile( x - m_CurEditorOffsetX, p1.Y - m_CurEditorOffsetY, m_CurrentEditorTile.Index );
                  DrawTile( x - m_CurEditorOffsetX, p2.Y - m_CurEditorOffsetY, m_CurrentEditorTile.Index );
                  m_CurrentMap.Tiles[x, p1.Y] = m_CurrentEditorTile.Index;
                  m_CurrentMap.Tiles[x, p2.Y] = m_CurrentEditorTile.Index;

                  pictureEditor.DisplayPage.DrawTo( m_Image,
                                  ( x - m_CurEditorOffsetX ) * 8 * m_CurrentMap.TileSpacingX,
                                  ( p1.Y - m_CurEditorOffsetY ) * 8 * m_CurrentMap.TileSpacingY,
                                  ( x - m_CurEditorOffsetX ) * 8 * m_CurrentMap.TileSpacingX,
                                  ( p1.Y - m_CurEditorOffsetY ) * 8 * m_CurrentMap.TileSpacingY,
                                  8 * m_CurrentMap.TileSpacingX, 8 * m_CurrentMap.TileSpacingY );
                  pictureEditor.DisplayPage.DrawTo( m_Image,
                                  ( x - m_CurEditorOffsetX ) * 8 * m_CurrentMap.TileSpacingX,
                                  ( p2.Y - m_CurEditorOffsetY ) * 8 * m_CurrentMap.TileSpacingY,
                                  ( x - m_CurEditorOffsetX ) * 8 * m_CurrentMap.TileSpacingX,
                                  ( p2.Y - m_CurEditorOffsetY ) * 8 * m_CurrentMap.TileSpacingY,
                                  8 * m_CurrentMap.TileSpacingX, 8 * m_CurrentMap.TileSpacingY );
                }
                for ( int y = p1.Y + 1; y <= p2.Y - 1; ++y )
                {
                  DrawTile( p1.X - m_CurEditorOffsetX, y - m_CurEditorOffsetY, m_CurrentEditorTile.Index );
                  DrawTile( p2.X - m_CurEditorOffsetX, y - m_CurEditorOffsetY, m_CurrentEditorTile.Index );
                  m_CurrentMap.Tiles[p1.X, y] = m_CurrentEditorTile.Index;
                  m_CurrentMap.Tiles[p2.X, y] = m_CurrentEditorTile.Index;

                  pictureEditor.DisplayPage.DrawTo( m_Image,
                                  ( p1.X - m_CurEditorOffsetX ) * 8 * m_CurrentMap.TileSpacingX,
                                  ( y - m_CurEditorOffsetY ) * 8 * m_CurrentMap.TileSpacingY,
                                  ( p1.X - m_CurEditorOffsetX ) * 8 * m_CurrentMap.TileSpacingX,
                                  ( y - m_CurEditorOffsetY ) * 8 * m_CurrentMap.TileSpacingY,
                                  8 * m_CurrentMap.TileSpacingX, 8 * m_CurrentMap.TileSpacingY );
                  pictureEditor.DisplayPage.DrawTo( m_Image,
                                  ( p2.X - m_CurEditorOffsetX ) * 8 * m_CurrentMap.TileSpacingX,
                                  ( y - m_CurEditorOffsetY ) * 8 * m_CurrentMap.TileSpacingY,
                                  ( p2.X - m_CurEditorOffsetX ) * 8 * m_CurrentMap.TileSpacingX,
                                  ( y - m_CurEditorOffsetY ) * 8 * m_CurrentMap.TileSpacingY,
                                  8 * m_CurrentMap.TileSpacingX, 8 * m_CurrentMap.TileSpacingY );
                }
              }
              else
              {
                for ( int x = p1.X; x <= p2.X; ++x )
                {
                  for ( int y = p1.Y; y <= p2.Y; ++y )
                  {
                    DrawTile( x - m_CurEditorOffsetX, y - m_CurEditorOffsetY, m_CurrentEditorTile.Index );
                    m_CurrentMap.Tiles[x, y] = m_CurrentEditorTile.Index;

                    pictureEditor.DisplayPage.DrawTo( m_Image,
                                    ( x - m_CurEditorOffsetX ) * 8 * m_CurrentMap.TileSpacingX,
                                    ( y - m_CurEditorOffsetY ) * 8 * m_CurrentMap.TileSpacingY,
                                    ( x - m_CurEditorOffsetX ) * 8 * m_CurrentMap.TileSpacingX,
                                    ( y - m_CurEditorOffsetY ) * 8 * m_CurrentMap.TileSpacingY,
                                    8 * m_CurrentMap.TileSpacingX, 8 * m_CurrentMap.TileSpacingY );
                  }
                }
              }
              pictureEditor.DisplayPage.DrawTo( m_Image,
                                                ( p1.X - m_CurEditorOffsetX ) * m_CurrentMap.TileSpacingX, ( p1.Y - m_CurEditorOffsetY ) * m_CurrentMap.TileSpacingY,
                                                ( p1.X - m_CurEditorOffsetX ) * m_CurrentMap.TileSpacingX, ( p1.Y - m_CurEditorOffsetY ) * m_CurrentMap.TileSpacingY,
                                                ( p2.X - p1.X + 1 ) * m_CurrentMap.TileSpacingX, ( p2.Y - p1.Y + 1 ) * m_CurrentMap.TileSpacingY );
              pictureEditor.Invalidate( new System.Drawing.Rectangle( p1.X * m_CurrentMap.TileSpacingX, p1.Y * m_CurrentMap.TileSpacingY, ( p2.X - p1.X + 1 ) * m_CurrentMap.TileSpacingX, ( p2.Y - p1.Y + 1 ) * m_CurrentMap.TileSpacingY ) );
              Modified = true;
            }
            break;
          case ToolMode.SELECT:
            if ( m_LastDragEndPos.X != -1 )
            {
              m_LastDragEndPos.X = -1;
              m_LastDragEndPos.Y = -1;

              System.Drawing.Point    p1, p2;

              CalcRect( m_DragStartPos, m_DragEndPos, out p1, out p2 );

              bool shiftPressed = ( ( ModifierKeys & Keys.Shift ) == Keys.Shift );

              if ( ( !shiftPressed )
              && ( ( ModifierKeys & Keys.Control ) == Keys.None ) )
              {
                // not ctrl-Click, remove previous selection
                for ( int x = 0; x < m_CurrentMap.Tiles.Width; ++x )
                {
                  for ( int y = 0; y < m_CurrentMap.Tiles.Height; ++y )
                  {
                    m_SelectedTiles[x, y] = false;
                  }
                }
              }

              for ( int x = p1.X; x <= p2.X; ++x )
              {
                for ( int y = p1.Y; y <= p2.Y; ++y )
                {
                  if ( shiftPressed )
                  {
                    m_SelectedTiles[x, y] = false;
                  }
                  else
                  {
                    m_SelectedTiles[x, y] = true;
                  }
                }
              }
              pictureEditor.Invalidate();
              Redraw();
            }
            break;
        }
      }

      if ( ( Buttons & MouseButtons.Left ) != 0 )
      {
        if ( m_FloatingSelection != null )
        {
          if ( m_MouseButtonReleased )
          {
            InsertFloatingSelection();
            m_MouseButtonReleased = false;
          }
          return;
        }

        switch ( m_ToolMode )
        {
          case ToolMode.SINGLE_TILE:
            if ( m_CurrentEditorTile != null )
            {
              if ( m_CurrentMap.Tiles[trueX + offsetX, trueY + offsetY] != m_CurrentEditorTile.Index )
              {
                if ( m_MouseButtonReleased )
                {
                  m_MouseButtonReleased = false;
                  DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoMapTilesChange( this, m_CurrentMap, 0, 0, m_CurrentMap.Tiles.Width, m_CurrentMap.Tiles.Height ) );
                }
                m_CurrentMap.Tiles[trueX + offsetX, trueY + offsetY] = m_CurrentEditorTile.Index;
                SetModified();

                DrawTile( trueX, trueY, m_CurrentEditorTile.Index );
                // copy to image cache
                pictureEditor.DisplayPage.DrawTo( m_Image,
                                trueX * 8 * m_CurrentMap.TileSpacingX,
                                trueY * 8 * m_CurrentMap.TileSpacingY,
                                trueX * 8 * m_CurrentMap.TileSpacingX,
                                trueY * 8 * m_CurrentMap.TileSpacingY,
                                m_MapProject.Tiles[m_CurrentEditorTile.Index].Chars.Width * 8,
                                m_MapProject.Tiles[m_CurrentEditorTile.Index].Chars.Height * 8 );

                pictureEditor.Invalidate( new System.Drawing.Rectangle( ( trueX * m_CurrentMap.TileSpacingX ) * 8,
                                                                        ( trueY * m_CurrentMap.TileSpacingY ) * 8,
                                                                        m_CurrentEditorTile.Chars.Width * 8,
                                                                        m_CurrentEditorTile.Chars.Height * 8 ) );
              }
            }
            break;
          case ToolMode.FILL:
            if ( m_MouseButtonReleased )
            {
              m_MouseButtonReleased = false;

              FillContent( trueX + m_CurEditorOffsetX, trueY + m_CurEditorOffsetY );
            }
            break;
          case ToolMode.RECTANGLE:
          case ToolMode.FILLED_RECTANGLE:
            if ( m_MouseButtonReleased )
            {
              m_MouseButtonReleased = false;

              // first point
              m_DragStartPos.X = trueX + m_CurEditorOffsetX;
              m_DragStartPos.Y = trueY + m_CurEditorOffsetY;
              m_LastDragEndPos = new System.Drawing.Point( -1, -1 );
            }
            // draw other point
            m_DragEndPos.X = trueX + m_CurEditorOffsetX;
            m_DragEndPos.Y = trueY + m_CurEditorOffsetY;

            if ( m_DragEndPos != m_LastDragEndPos )
            {
              // restore background
              if ( m_LastDragEndPos.X != -1 )
              {
                System.Drawing.Point    o1, o2;

                CalcRect( m_DragStartPos, m_LastDragEndPos, out o1, out o2 );

                m_Image.DrawTo( pictureEditor.DisplayPage,
                                ( o1.X - m_CurEditorOffsetX ) * 8 * m_CurrentMap.TileSpacingX, ( o1.Y - m_CurEditorOffsetY ) * 8 * m_CurrentMap.TileSpacingY,
                                ( o1.X - m_CurEditorOffsetX ) * 8 * m_CurrentMap.TileSpacingX, ( o1.Y - m_CurEditorOffsetY ) * 8 * m_CurrentMap.TileSpacingY,
                                ( o2.X - o1.X + 1 ) * 8 * m_CurrentMap.TileSpacingX, ( o2.Y - o1.Y + 1 ) * 8 * m_CurrentMap.TileSpacingY );

                pictureEditor.Invalidate( new System.Drawing.Rectangle( o1.X * 8 * m_CurrentMap.TileSpacingX, o1.Y * 8 * m_CurrentMap.TileSpacingY, ( o2.X - o1.X + 1 ) * 8 * m_CurrentMap.TileSpacingX, ( o2.Y - o1.Y + 1 ) * 8 * m_CurrentMap.TileSpacingY ) );
              }
              m_LastDragEndPos = m_DragEndPos;

              System.Drawing.Point    p1, p2;

              CalcRect( m_DragStartPos, m_DragEndPos, out p1, out p2 );

              if ( m_ToolMode == ToolMode.RECTANGLE )
              {
                for ( int x = p1.X; x <= p2.X; ++x )
                {
                  DrawTile( x - m_CurEditorOffsetX, 
                            p1.Y - m_CurEditorOffsetY, 
                            m_CurrentEditorTile.Index );
                  DrawTile( x - m_CurEditorOffsetX,
                            p2.Y - m_CurEditorOffsetY,
                            m_CurrentEditorTile.Index );
                }
                for ( int y = p1.Y + 1; y <= p2.Y - 1; ++y )
                {
                  DrawTile( p1.X - m_CurEditorOffsetX,
                            y - m_CurEditorOffsetY,
                            m_CurrentEditorTile.Index );
                  DrawTile( p2.X - m_CurEditorOffsetX,
                            y - m_CurEditorOffsetY,
                            m_CurrentEditorTile.Index );
                }
              }
              else
              {
                for ( int x = p1.X; x <= p2.X; ++x )
                {
                  for ( int y = p1.Y; y <= p2.Y; ++y )
                  {
                    DrawTile( x - m_CurEditorOffsetX,
                              y - m_CurEditorOffsetY,
                              m_CurrentEditorTile.Index );
                  }
                }
              }
              pictureEditor.Invalidate( new System.Drawing.Rectangle( p1.X * m_CurrentMap.TileSpacingX, 
                                                                      p1.Y * m_CurrentMap.TileSpacingY,
                                                                      ( p2.X - p1.X + 1 ) * m_CurrentMap.TileSpacingX, 
                                                                      ( p2.Y - p1.Y + 1 ) * m_CurrentMap.TileSpacingY ) );
            }
            break;
          case ToolMode.SELECT:
            if ( m_MouseButtonReleased )
            {
              m_MouseButtonReleased = false;

              // first point
              m_DragStartPos.X = trueX + m_CurEditorOffsetX;
              m_DragStartPos.Y = trueY + m_CurEditorOffsetY;
              m_LastDragEndPos = new System.Drawing.Point( -1, -1 );
            }
            // draw other point
            m_DragEndPos.X = trueX + m_CurEditorOffsetX;
            m_DragEndPos.Y = trueY + m_CurEditorOffsetY;

            if ( m_DragEndPos != m_LastDragEndPos )
            {
              // restore background
              if ( m_LastDragEndPos.X != -1 )
              {
                System.Drawing.Point    o1, o2;

                CalcRect( m_DragStartPos, m_LastDragEndPos, out o1, out o2 );

                pictureEditor.Invalidate( new System.Drawing.Rectangle( o1.X * 8, o1.Y * 8, ( o2.X - o1.X + 1 ) * 8, ( o2.Y - o1.Y + 1 ) * 8 ) );
              }
              m_LastDragEndPos = m_DragEndPos;

              System.Drawing.Point    p1, p2;

              CalcRect( m_DragStartPos, m_DragEndPos, out p1, out p2 );

              pictureEditor.Invalidate( new System.Drawing.Rectangle( p1.X * 8, p1.Y * 8, ( p2.X - p1.X + 1 ) * 8, ( p2.Y - p1.Y + 1 ) * 8 ) );
              Redraw();
            }
            break;
        }
      }
      if ( ( Buttons & MouseButtons.Right ) != 0 )
      {
        int tileIndex = m_CurrentMap.Tiles[trueX + offsetX, trueY + offsetY];
        if ( tileIndex < m_MapProject.Tiles.Count )
        {
          m_CurrentEditorTile = m_MapProject.Tiles[tileIndex];
          if ( ( tileIndex >= 0 )
          &&   ( tileIndex < comboTiles.Items.Count ) )
          {
            comboTiles.SelectedIndex = tileIndex;
          }
        }
      }
    }



    private void DrawTile( int trueX, int trueY, int TileIndex )
    {
      if ( ( TileIndex < 0 )
      ||   ( TileIndex >= m_MapProject.Tiles.Count ) )
      {
        return;
      }
      for ( int j = 0; j < m_MapProject.Tiles[TileIndex].Chars.Height; ++j )
      {
        for ( int i = 0; i < m_MapProject.Tiles[TileIndex].Chars.Width; ++i )
        {
          DrawCharImage( pictureEditor.DisplayPage,
                         ( trueX * m_CurrentMap.TileSpacingX + i ) * 8,
                         ( trueY * m_CurrentMap.TileSpacingY + j ) * 8,
                         m_MapProject.Tiles[TileIndex].Chars[i, j].Character,
                         m_MapProject.Tiles[TileIndex].Chars[i, j].Color );
        }
      }
    }



    private void pictureEditor_MouseMove( object sender, MouseEventArgs e )
    {
      MouseButtons    buttons = e.Button;
      if ( !pictureEditor.Focused )
      {
        buttons = 0;
      }
      HandleMouseOnEditor( e.X, e.Y, buttons );
    }



    private void checkMulticolor_CheckedChanged( object sender, EventArgs e )
    {
    }



    private void RedrawMap()
    {
      uint    bgColor = (uint)m_MapProject.BackgroundColor;
      if ( ( m_CurrentMap != null )
      &&   ( m_CurrentMap.AlternativeBackgroundColor != -1 ) )
      {
        bgColor = (uint)m_CurrentMap.AlternativeBackgroundColor;
      }

      int     fillWidth = pictureEditor.DisplayPage.Width;
      int     fillHeight = pictureEditor.DisplayPage.Height;

      if ( m_CurrentMap != null )
      {
        fillWidth = m_CurrentMap.TileSpacingX * 8 * m_CurrentMap.Tiles.Width;
        fillHeight = m_CurrentMap.TileSpacingY * 8 * m_CurrentMap.Tiles.Height;
        if ( ( m_CurrentMap.Tiles.Width - m_CurEditorOffsetX ) * 8 * m_CurrentMap.TileSpacingX < pictureEditor.DisplayPage.Width )
        {
          fillWidth = ( m_CurrentMap.Tiles.Width - m_CurEditorOffsetX ) * 8 * m_CurrentMap.TileSpacingX;
        }
        if ( ( m_CurrentMap.Tiles.Height - m_CurEditorOffsetY ) * 8 * m_CurrentMap.TileSpacingY < pictureEditor.DisplayPage.Height )
        {
          fillHeight = ( m_CurrentMap.Tiles.Height - m_CurEditorOffsetY ) * 8 * m_CurrentMap.TileSpacingY;
        }
      }

      pictureEditor.DisplayPage.Box( 0, 0, fillWidth, fillHeight, m_MapProject.Charset.Colors.Palette.ColorValues[bgColor] );

      if ( fillWidth < pictureEditor.DisplayPage.Width )
      {
        pictureEditor.DisplayPage.Box( fillWidth, 0, pictureEditor.DisplayPage.Width - fillWidth, pictureEditor.DisplayPage.Height, Core.Settings.FGColor( ColorableElement.SELECTION_FRAME ) );
      }
      if ( fillHeight < pictureEditor.DisplayPage.Height )
      {
        pictureEditor.DisplayPage.Box( 0, fillHeight, pictureEditor.DisplayPage.Width, pictureEditor.DisplayPage.Height - fillHeight, Core.Settings.FGColor( ColorableElement.SELECTION_FRAME ) );
      }

      if ( m_CurrentMap == null )
      {
        pictureEditor.Invalidate();
        return;
      }

      int offsetX = m_CurEditorOffsetX;
      int offsetY = m_CurEditorOffsetY;

      int x1 = offsetX;
      int x2 = offsetX + ( pictureEditor.DisplayPage.Width / ( 8 * m_CurrentMap.TileSpacingX ) );
      int y1 = offsetY;
      int y2 = offsetY + ( pictureEditor.DisplayPage.Height / ( 8 * m_CurrentMap.TileSpacingY ) );

      for ( int y = y1; y <= y2; ++y )
      {
        for ( int x = x1; x <= x2; ++x )
        {
          if ( ( x >= m_CurrentMap.Tiles.Width )
          ||   ( y >= m_CurrentMap.Tiles.Height ) )
          {
            continue;
          }
          int tileIndex = m_CurrentMap.Tiles[x, y];
          if ( tileIndex < m_MapProject.Tiles.Count )
          {
            // a real tile
            var tile = m_MapProject.Tiles[tileIndex];

            for ( int j = 0; j < tile.Chars.Height; ++j )
            {
              for ( int i = 0; i < tile.Chars.Width; ++i )
              {
                Displayer.CharacterDisplayer.DisplayChar( m_MapProject.Charset,
                                                          m_MapProject.Charset.Colors.Palette,
                                                          tile.Chars[i, j].Character,
                                                          pictureEditor.DisplayPage, 
                                                          ( ( x - offsetX ) * m_CurrentMap.TileSpacingX + i ) * 8, 
                                                          ( ( y - offsetY ) * m_CurrentMap.TileSpacingY + j ) * 8,
                                                          tile.Chars[i,j].Color,
                                                          ( m_CurrentMap.AlternativeBackgroundColor != -1 ) ? m_CurrentMap.AlternativeBackgroundColor : m_MapProject.BackgroundColor,
                                                          ( m_CurrentMap.AlternativeMultiColor1 != -1 ) ? m_CurrentMap.AlternativeMultiColor1 : m_MapProject.MultiColor1,
                                                          ( m_CurrentMap.AlternativeMultiColor2 != -1 ) ? m_CurrentMap.AlternativeMultiColor2 : m_MapProject.MultiColor2,
                                                          ( m_CurrentMap.AlternativeBGColor4 != -1 ) ? m_CurrentMap.AlternativeBGColor4 : m_MapProject.BGColor4,
                                                          ( m_CurrentMap.AlternativeMode != TextCharMode.UNKNOWN ) ? m_CurrentMap.AlternativeMode : Lookup.TextCharModeFromTextMode( m_MapProject.Mode ) );
              }
            }
          }
        }
      }
      pictureEditor.DisplayPage.DrawTo( m_Image, 0, 0, 0, 0, 320, 200 );
      pictureEditor.Invalidate();
    }



    private void comboBackground_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( m_MapProject.BackgroundColor != comboTileBackground.SelectedIndex )
      {
        m_MapProject.BackgroundColor = comboTileBackground.SelectedIndex;
        m_MapProject.Charset.Colors.BackgroundColor = m_MapProject.BackgroundColor;
        for ( int i = 0; i < m_MapProject.Charset.TotalNumberOfCharacters; ++i )
        {
          RebuildCharImage( i );
        }
        Modified = true;
        RedrawMap();
        pictureEditor.Invalidate();
        panelCharacters.Invalidate();
      }
    }



    private void comboMulticolor1_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( m_MapProject.MultiColor1 != comboTileMulticolor1.SelectedIndex )
      {
        m_MapProject.MultiColor1 = comboTileMulticolor1.SelectedIndex;
        m_MapProject.Charset.Colors.MultiColor1 = m_MapProject.MultiColor1;
        SetModified();
        FullRebuild();
      }
    }



    private void comboMulticolor2_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( m_MapProject.MultiColor2 != comboTileMulticolor2.SelectedIndex )
      {
        m_MapProject.MultiColor2 = comboTileMulticolor2.SelectedIndex;
        m_MapProject.Charset.Colors.MultiColor2 = m_MapProject.MultiColor2;
        SetModified();
        FullRebuild();
      }
    }



    public void Clear()
    {
      DocumentInfo.DocumentFilename = "";

      m_MapProject.Clear();
    }



    public bool OpenProject( string File )
    {
      Clear();
      comboMaps.Items.Clear();
      comboMaps.Enabled = false;
      comboTiles.Items.Clear();

      GR.Memory.ByteBuffer projectFile = GR.IO.File.ReadAllBytes( File );
      if ( projectFile == null )
      {
        return false;
      }

      if ( !m_MapProject.ReadFromBuffer( projectFile ) )
      {
        return false;
      }
      foreach ( var tile in m_MapProject.Tiles )
      {
        comboTiles.Items.Add( new GR.Generic.Tupel<string, Formats.MapProject.Tile>( tile.Name, tile ) );

        ListViewItem item = new ListViewItem();

        item.Text = tile.Index.ToString();
        item.SubItems.Add( tile.Name );
        item.SubItems.Add( tile.Chars.Width.ToString() + "x" + tile.Chars.Height.ToString() );
        item.SubItems.Add( "0" );
        item.Tag = tile;

        listTileInfo.Items.Add( item );
      }

      int index = 0;
      foreach ( var map in m_MapProject.Maps )
      {
        comboMaps.Items.Add( new GR.Generic.Tupel<string, Formats.MapProject.Map>( index.ToString() + ": " + map.Name, map ) );
        comboMaps.Enabled = true;
        ++index;
      }


      comboTileBackground.SelectedIndex   = m_MapProject.BackgroundColor;
      comboTileMulticolor1.SelectedIndex = m_MapProject.MultiColor1;
      comboTileMulticolor2.SelectedIndex = m_MapProject.MultiColor2;
      comboTileBGColor4.SelectedIndex = m_MapProject.BGColor4;
      comboMapProjectMode.SelectedIndex = (int)m_MapProject.Mode;
      checkShowGrid.Checked = m_MapProject.ShowGrid;

      for ( int i = 0; i < m_MapProject.Charset.TotalNumberOfCharacters; ++i )
      {
        RebuildCharImage( i );
      }
      RedrawMap();
      RedrawColorChooser();
      characterEditor.CharsetUpdated( m_MapProject.Charset );
      Modified = false;
      DocumentInfo.DocumentFilename = File;

      if ( ( comboMaps.Items.Count > 0 )
      &&   ( comboMaps.SelectedIndex == -1 ) )
      {
        comboMaps.SelectedIndex = 0;
      }
      if ( ( comboTiles.Items.Count > 0 )
      &&   ( comboTiles.SelectedIndex == -1 ) )
      {
        comboTiles.SelectedIndex = 0;
      }
      if ( ( listTileInfo.Items.Count > 0 )
      &&   ( listTileInfo.SelectedIndices.Count == 0 ) )
      {
        listTileInfo.SelectedIndices.Add( 0 );
      }

      EnableFileWatcher();
      return true;
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
        System.Windows.Forms.MessageBox.Show( "Could not load map project file " + DocumentInfo.FullPath + ".\r\n" + ex.Message, "Could not load file" );
        return false;
      }
      SetUnmodified();
      return true;
    }



    public override GR.Memory.ByteBuffer SaveToBuffer()
    {
      return m_MapProject.SaveToBuffer();
    }



    protected override bool QueryFilename( string PreviousFilename, out string Filename )
    {
      Filename = "";

      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Save Map Editor Project as";
      saveDlg.Filter = "Map Editor Projects|*.mapproject|All Files|*.*";
      saveDlg.FileName = System.IO.Path.GetFileName( PreviousFilename );
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
        DialogResult doSave = MessageBox.Show( "There are unsaved changes in your map project. Save now?", "Save changes?", endButtons );
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



    private bool ImportCharset( string Filename )
    {
      GR.Memory.ByteBuffer charData = GR.IO.File.ReadAllBytes( Filename );
      if ( charData == null )
      {
        return false;
      }

      int charsToImport = (int)charData.Length / 8;
      if ( charsToImport > 256 )
      {
        charsToImport = 256;
      }
      for ( int i = 0; i < charsToImport; ++i )
      {
        for ( int j = 0; j < 8; ++j )
        {
          m_MapProject.Charset.Characters[i].Tile.Data.SetU8At( j, charData.ByteAt( i * 8 + j ) );
        }
        RebuildCharImage( i );
      }
      characterEditor.CharsetUpdated( m_MapProject.Charset );
      return true;
    }



    private void Redraw()
    {
      pictureEditor.DisplayPage.DrawImage( m_Image, 0, 0 );

      if ( m_CurrentMap == null )
      {
        return;
      }

      if ( m_FloatingSelection != null )
      {
        for ( int j = 0; j < m_FloatingSelectionSize.Height; ++j )
        {
          for ( int i = 0; i < m_FloatingSelectionSize.Width; ++i )
          {
            var selectionChar = m_FloatingSelection[i + j * m_FloatingSelectionSize.Width];
            if ( selectionChar.first )
            {
              DrawTile( ( m_MousePos.X + i ),
                        ( m_MousePos.Y + j ),
                        selectionChar.second );
            }
          }
        }
      }

      pictureEditor.Invalidate();
    }



    private void pictureEditor_Paint( object sender, PaintEventArgs e )
    {
    }



    private void CopyToClipboard()
    {
      // not only rectangular pieces
      int     x1 = m_CurrentMap.Tiles.Width;
      int     x2 = 0;
      int     y1 = m_CurrentMap.Tiles.Height;
      int     y2 = 0;
      bool    selectAll = false;

      for ( int i = 0; i < m_CurrentMap.Tiles.Width; ++i )
      {
        for ( int j = 0; j < m_CurrentMap.Tiles.Height; ++j )
        {
          if ( m_SelectedTiles[i, j] )
          {
            if ( i < x1 )
            {
              x1 = i;
            }
            if ( i > x2 )
            {
              x2 = i;
            }
            if ( j < y1 )
            {
              y1 = j;
            }
            if ( j > y2 )
            {
              y2 = j;
            }
          }
        }
      }
      if ( x1 == m_CurrentMap.Tiles.Width )
      {
        // no selection, select all
        x1 = 0;
        y1 = 0;
        x2 = m_CurrentMap.Tiles.Width - 1;
        y2 = m_CurrentMap.Tiles.Height - 1;
        selectAll = true;
      }

      GR.Memory.ByteBuffer dataSelection = new GR.Memory.ByteBuffer();

      dataSelection.Reserve( ( y2 - y1 + 1 ) * ( x2 - x1 + 1 ) + 8 );
      dataSelection.AppendI32( x2 - x1 + 1 );
      dataSelection.AppendI32( y2 - y1 + 1 );

      for ( int y = 0; y < y2 - y1 + 1; ++y )
      {
        for ( int x = 0; x < x2 - x1 + 1; ++x )
        {
          if ( ( selectAll )
          ||   ( m_SelectedTiles[x1 + x, y1 + y] ) )
          {
            dataSelection.AppendU8( 1 );
            dataSelection.AppendI32( m_CurrentMap.Tiles[x1 + x, y1 + y] );
          }
          else
          {
            dataSelection.AppendU8( 0 );
          }
        }
      }

      DataObject dataObj = new DataObject();

      dataObj.SetData( "RetroDevStudio.MapEditorSelection", false, dataSelection.MemoryStream() );

      // TODO - Grafik?
      /*
      GR.Memory.ByteBuffer      dibData = m_Charset.Characters[m_CurrentChar].Image.CreateHDIBAsBuffer();

      System.IO.MemoryStream    ms = dibData.MemoryStream();

      // WTF - SetData requires streams, NOT global data (HGLOBAL)
      dataObj.SetData( "DeviceIndependentBitmap", ms );
      */
      Clipboard.SetDataObject( dataObj, true );
    }



    private void PasteFromClipboard()
    {
      IDataObject dataObj = Clipboard.GetDataObject();
      if ( dataObj == null )
      {
        System.Windows.Forms.MessageBox.Show( "The clipboard is empty" );
        return;
      }
      if ( dataObj.GetDataPresent( "RetroDevStudio.MapEditorSelection" ) )
      {
        System.IO.MemoryStream ms = (System.IO.MemoryStream)dataObj.GetData( "RetroDevStudio.MapEditorSelection" );

        GR.Memory.ByteBuffer data = new GR.Memory.ByteBuffer( (uint)ms.Length );

        ms.Read( data.Data(), 0, (int)ms.Length );

        GR.IO.MemoryReader memIn = data.MemoryReader();

        int   selectionWidth  = memIn.ReadInt32();
        int   selectionHeight = memIn.ReadInt32();

        m_FloatingSelection = new List<GR.Generic.Tupel<bool, int>>();
        m_FloatingSelectionSize = new System.Drawing.Size( selectionWidth, selectionHeight );

        for ( int y = 0; y < selectionHeight; ++y )
        {
          for ( int x = 0; x < selectionWidth; ++x )
          {
            bool  isCharSet = ( memIn.ReadUInt8() != 0 );
            if ( isCharSet )
            {
              m_FloatingSelection.Add( new GR.Generic.Tupel<bool, int>( true, memIn.ReadInt32() ) );
            }
            else
            {
              m_FloatingSelection.Add( new GR.Generic.Tupel<bool, int>( false, 0 ) );
            }
          }
        }
        m_FloatingSelectionPos = m_MousePos;
        Redraw();
        pictureEditor.Invalidate();
        return;
      }
    }



    private void panelCharacters_SelectedIndexChanged( object sender, EventArgs e )
    {
      m_CurrentChar = (byte)panelCharacters.SelectedIndex;
      RedrawColorChooser();

      if ( ( m_CurrentTileChar != null )
      &&   ( m_CurrentTileChar.Character != m_CurrentChar ) )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoMapTileModified( this, m_MapProject, listTileInfo.SelectedIndices[0] ) );

        m_CurrentTileChar.Character = m_CurrentChar;

        listTileChars.SelectedItems[0].SubItems[1].Text = m_CurrentChar.ToString();
        RedrawTile();
        RedrawMap();
        SetModified();
      }
    }



    private void RedrawColorChooser()
    {
      for ( byte i = 0; i < 16; ++i )
      {
        DrawCharImage( panelCharColors.DisplayPage, i * 8, 0, m_CurrentChar, i );
      }
      panelCharColors.DisplayPage.Rectangle( m_CurrentColor * 8, 0, 8, 8, Core.Settings.FGColor( ColorableElement.SELECTION_FRAME ) );
      panelCharColors.Invalidate();
    }



    private void panelCharColors_MouseDown( object sender, MouseEventArgs e )
    {
      HandleMouseOnColorChooser( e.X, e.Y, e.Button );
    }



    private void panelCharColors_MouseMove( object sender, MouseEventArgs e )
    {
      MouseButtons    buttons = e.Button;
      if ( !panelCharColors.Focused )
      {
        buttons = 0;
      }
      HandleMouseOnColorChooser( e.X, e.Y, buttons );
    }



    private void HandleMouseOnColorChooser( int X, int Y, MouseButtons Buttons )
    {
      if ( ( Buttons & MouseButtons.Left ) == MouseButtons.Left )
      {
        int colorIndex = (int)( ( 16 * X ) / panelCharColors.ClientSize.Width );
        m_CurrentColor = (byte)colorIndex;
        RedrawColorChooser();

        if ( ( m_CurrentTileChar != null )
        &&   ( m_CurrentTileChar.Color != m_CurrentColor ) )
        {
          DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoMapTileModified( this, m_MapProject, listTileInfo.SelectedIndices[0] ) );

          m_CurrentTileChar.Color = m_CurrentColor;

          listTileChars.SelectedItems[0].SubItems[2].Text = m_CurrentColor.ToString();
          RedrawTile();
          RedrawMap();
          SetModified();
        }
      }
    }



    private void importCharsetToolStripMenuItem_Click( object sender, EventArgs e )
    {
      ImportCharset();
    }



    public bool OpenExternalCharset( string Filename )
    {
      string extension = System.IO.Path.GetExtension( Filename ).ToUpper();

      if ( extension == ".CHARSETPROJECT" )
      {
        GR.Memory.ByteBuffer charSetProject = GR.IO.File.ReadAllBytes( Filename );
        if ( charSetProject == null )
        {
          return false;
        }
        if ( !m_MapProject.Charset.ReadFromBuffer( charSetProject ) )
        {
          return false;
        }
        m_MapProject.Mode = Lookup.TextModeFromTextCharMode( m_MapProject.Charset.Mode );
        comboMapProjectMode.SelectedIndex = (int)m_MapProject.Mode;

        FullRebuild();
        characterEditor.CharsetUpdated( m_MapProject.Charset );
        RedrawMap();
        Modified = true;
        return true;
      }
      // treat as .chr
      if ( !ImportCharset( Filename ) )
      {
        return false;
      }
      pictureEditor.Invalidate();
      Modified = true;
      return true;
    }



    public void ImportCharset()
    {
      string filename;

      if ( !OpenFile( "Open charset or charset project", Types.Constants.FILEFILTER_CHARSET + Types.Constants.FILEFILTER_ALL, out filename ) )
      {
        return;
      }
      OpenExternalCharset( filename );
      if ( ( DocumentInfo.Project == null )
      ||   ( string.IsNullOrEmpty( DocumentInfo.Project.Settings.BasePath ) ) )
      {
        m_MapProject.ExternalCharset = filename;
      }
      else
      {
        m_MapProject.ExternalCharset = GR.Path.RelativePathTo( filename, false, System.IO.Path.GetFullPath( DocumentInfo.Project.Settings.BasePath ), true );
      }
      Modified = true;
    }



    private void btnExportToFile_Click( object sender, EventArgs e )
    {
      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Save data as";
      saveDlg.Filter = "Map Data|*.map|Binary Data|*.bin|All Files|*.*";
      if ( DocumentInfo.Project != null )
      {
        saveDlg.InitialDirectory = DocumentInfo.Project.Settings.BasePath;
      }
      if ( saveDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
      {
        return;
      }

      // prepare data
      GR.Memory.ByteBuffer tileData = new GR.Memory.ByteBuffer();
      GR.Memory.ByteBuffer mapData = new GR.Memory.ByteBuffer();

      GR.Memory.ByteBuffer finalData = null;

      switch ( (MapExportType)comboExportData.SelectedIndex )
      {
        case MapExportType.TILE_DATA:
          m_MapProject.ExportTilesAsBuffer( comboExportOrientation.SelectedIndex == 0, out tileData );
          finalData = tileData;
          break;
        case MapExportType.TILE_AND_MAP_DATA:
          m_MapProject.ExportTilesAsBuffer( comboExportOrientation.SelectedIndex == 0, out tileData );
          mapData = m_MapProject.ExportMapsAsBuffer( comboExportOrientation.SelectedIndex == 0 );
          finalData = tileData + mapData;
          break;
        case MapExportType.MAP_DATA:
          {
            bool    vertical = ( comboExportOrientation.SelectedIndex != 0 );

            if ( m_CurrentMap != null )
            {
              GR.Memory.ByteBuffer      selectionData = new GR.Memory.ByteBuffer();

              if ( vertical )
              {
                // select all
                for ( int i = 0; i < m_CurrentMap.Tiles.Width; ++i )
                {
                  for ( int j = 0; j < m_CurrentMap.Tiles.Height; ++j )
                  {
                    selectionData.AppendU8( (byte)m_CurrentMap.Tiles[i, j] );
                  }
                }
              }
              else
              {
                // select all
                for ( int j = 0; j < m_CurrentMap.Tiles.Height; ++j )
                {
                  for ( int i = 0; i < m_CurrentMap.Tiles.Width; ++i )
                  {
                    selectionData.AppendU8( (byte)m_CurrentMap.Tiles[i, j] );
                  }
                }
              }
              finalData = selectionData;
            }
          }
          break;
        case MapExportType.MAP_DATA_SELECTION:
          {
            bool    vertical = ( comboExportOrientation.SelectedIndex != 0 );

            if ( m_CurrentMap != null )
            {
              GR.Memory.ByteBuffer      selectionData = new GR.Memory.ByteBuffer();
              bool                      hasSelection = false;

              if ( vertical )
              {
                for ( int i = 0; i < m_CurrentMap.Tiles.Width; ++i )
                {
                  for ( int j = 0; j < m_CurrentMap.Tiles.Height; ++j )
                  {
                    if ( m_SelectedTiles[i, j] )
                    {
                      selectionData.AppendU8( (byte)m_CurrentMap.Tiles[i, j] );
                      hasSelection = true;
                    }
                  }
                }
                if ( !hasSelection )
                {
                  // select all
                  for ( int i = 0; i < m_CurrentMap.Tiles.Width; ++i )
                  {
                    for ( int j = 0; j < m_CurrentMap.Tiles.Height; ++j )
                    {
                      selectionData.AppendU8( (byte)m_CurrentMap.Tiles[i, j] );
                    }
                  }
                }
              }
              else
              {
                for ( int j = 0; j < m_CurrentMap.Tiles.Height; ++j )
                {
                  for ( int i = 0; i < m_CurrentMap.Tiles.Width; ++i )
                  {
                    if ( m_SelectedTiles[i, j] )
                    {
                      selectionData.AppendU8( (byte)m_CurrentMap.Tiles[i, j] );
                      hasSelection = true;
                    }
                  }
                }
                if ( !hasSelection )
                {
                  // select all
                  for ( int j = 0; j < m_CurrentMap.Tiles.Height; ++j )
                  {
                    for ( int i = 0; i < m_CurrentMap.Tiles.Width; ++i )
                    {
                      selectionData.AppendU8( (byte)m_CurrentMap.Tiles[i, j] );
                    }
                  }
                }
              }
              finalData = selectionData;
            }
          }
          break;
        default:
          MessageBox.Show( "The export type " + (MapExportType)comboExportData.SelectedIndex + " is not supported for binary export.", "Export type not supported" );
          return;
      }
      if ( finalData != null )
      {
        GR.IO.File.WriteAllBytes( saveDlg.FileName, finalData );
      }
    }



    private void AdjustScrollbars()
    {
      mapHScroll.Minimum = 0;
      mapHScroll.SmallChange = 1;
      mapHScroll.LargeChange = 1;
      mapVScroll.SmallChange = 1;
      mapVScroll.LargeChange = 1;
      if ( m_CurrentMap == null )
      {
        mapHScroll.Maximum = 0;
        mapVScroll.Maximum = 0;
        return;
      }

      if ( m_CurrentMap.TileSpacingX * m_CurrentMap.Tiles.Width <= ScreenCharWidth )
      {
        mapHScroll.Maximum = 0;
        mapHScroll.Enabled = false;
        m_CurEditorOffsetX = 0;
      }
      else
      {
        mapHScroll.Maximum = ( m_CurrentMap.TileSpacingX * m_CurrentMap.Tiles.Width - ScreenCharWidth ) / m_CurrentMap.TileSpacingX + 1;
        mapHScroll.Enabled = true;
      }
      if ( m_CurEditorOffsetX > mapHScroll.Maximum )
      {
        m_CurEditorOffsetX = mapHScroll.Maximum;
      }

      mapVScroll.Minimum = 0;
      if ( m_CurrentMap.TileSpacingY * m_CurrentMap.Tiles.Height <= ScreenCharHeight )
      {
        mapVScroll.Maximum = 0;
        mapVScroll.Enabled = false;
        m_CurEditorOffsetY = 0;
      }
      else
      {
        mapVScroll.Maximum = ( m_CurrentMap.TileSpacingY * m_CurrentMap.Tiles.Height - ScreenCharHeight ) / m_CurrentMap.TileSpacingY + 1;
        mapVScroll.Enabled = true;
      }
      if ( m_CurEditorOffsetY > mapVScroll.Maximum )
      {
        m_CurEditorOffsetY = mapVScroll.Maximum;
      }
    }



    private string FormatExtraData( GR.Memory.ByteBuffer Data )
    {
      if ( Data.Length == 0 )
      {
        return "";
      }
      StringBuilder sb = new StringBuilder();

      for ( int i = 0; i < ( Data.Length + 7 ) / 8; ++i )
      {
        int     len = 8;
        if ( i * 8 + 8 > Data.Length )
        {
          len = (int)Data.Length - i * 8;
        }
        sb.AppendLine( Data.ToString( i * 8, len ) );
      }
      return sb.ToString();
    }



    private void comboMaps_SelectedIndexChanged( object sender, EventArgs e )
    {
      m_CurrentMap = null;

      btnMapApply.Enabled = ( comboMaps.SelectedIndex != -1 );
      btnMapDelete.Enabled = ( comboMaps.SelectedIndex != -1 );

      if ( comboMaps.SelectedIndex == -1 )
      {
        comboTiles.Items.Clear();
        btnCopy.Enabled = false;
        btnMoveMapDown.Enabled = false;
        btnMoveMapUp.Enabled = false;
        return;
      }
      m_CurrentMap = ( (GR.Generic.Tupel<string, Formats.MapProject.Map>)comboMaps.SelectedItem ).second;
      btnCopy.Enabled = true;

      btnMoveMapDown.Enabled  = ( ( comboMaps.Items.Count >= 2 ) && ( comboMaps.SelectedIndex + 1 < comboMaps.Items.Count ) );
      btnMoveMapUp.Enabled    = ( ( comboMaps.Items.Count >= 2 ) && ( comboMaps.SelectedIndex > 0 ) );

      m_SelectedTiles = new bool[m_CurrentMap.Tiles.Width, m_CurrentMap.Tiles.Height];

      editMapName.Text = m_CurrentMap.Name;
      editTileSpacingW.Text = m_CurrentMap.TileSpacingX.ToString();
      editTileSpacingH.Text = m_CurrentMap.TileSpacingY.ToString();
      editMapWidth.Text = m_CurrentMap.Tiles.Width.ToString();
      editMapHeight.Text = m_CurrentMap.Tiles.Height.ToString();
      comboTiles.ItemHeight = m_CurrentMap.TileSpacingY * 8 * 2;
      //editMapExtraData.Text = FormatExtraData( m_CurrentMap.ExtraData );
      editMapExtraData.Text = m_CurrentMap.ExtraDataText;
      comboMapMultiColor1.SelectedIndex = m_CurrentMap.AlternativeMultiColor1 + 1;
      comboMapMultiColor2.SelectedIndex = m_CurrentMap.AlternativeMultiColor2 + 1;
      comboMapBGColor.SelectedIndex = m_CurrentMap.AlternativeBackgroundColor + 1;
      comboMapAlternativeMode.SelectedIndex = (int)m_CurrentMap.AlternativeMode + 1;

      AdjustScrollbars();
      RedrawMap();
    }



    private void btnAddTile_Click( DecentForms.ControlBase Sender )
    {
      int w = GR.Convert.ToI32( editTileWidth.Text );
      int h = GR.Convert.ToI32( editTileHeight.Text );

      if ( ( w == 0 )
      ||   ( h == 0 ) )
      {
        return;
      }
      Formats.MapProject.Tile tile = new Formats.MapProject.Tile();
      tile.Chars.Resize( w, h );
      tile.Name = editTileName.Text;

      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoMapTileAdd( this, m_MapProject, m_MapProject.Tiles.Count ) );

      AddTile( m_MapProject.Tiles.Count, tile );
    }



    public void AddTile( int TileIndex, Formats.MapProject.Tile Tile )
    {
      m_MapProject.Tiles.Insert( TileIndex, Tile );
      Tile.Index = TileIndex;
      comboTiles.Items.Insert( TileIndex, new GR.Generic.Tupel<string, Formats.MapProject.Tile>( Tile.Name, Tile ) );

      ListViewItem item = new ListViewItem();

      item.Text = Tile.Index.ToString();
      item.SubItems.Add( Tile.Name );
      item.SubItems.Add( Tile.Chars.Width.ToString() + "x" + Tile.Chars.Height.ToString() );
      item.SubItems.Add( "0" );
      item.Tag = Tile;

      listTileInfo.Items.Insert( TileIndex, item );

      for ( int i = 0; i < m_MapProject.Tiles.Count; ++i )
      {
        m_MapProject.Tiles[i].Index = i;
      }
      for ( int i = TileIndex; i < listTileInfo.Items.Count; ++i )
      {
        listTileInfo.Items[i].Text = i.ToString();
      }
      RedrawMap();
      RedrawTile();
      SetModified();
    }



    private void listTileInfo_SelectedIndexChanged( object sender, EventArgs e )
    {
      m_CurrentEditedTile = null;
      m_CurrentTileChar = null;
      listTileChars.Items.Clear();

      btnTileApply.Enabled = ( listTileInfo.SelectedIndices.Count != 0 );
      btnTileDelete.Enabled = ( listTileInfo.SelectedIndices.Count != 0 );
      btnTileClone.Enabled = ( listTileInfo.SelectedIndices.Count != 0 );

      btnMoveTileUp.Enabled = ( ( listTileInfo.Items.Count > 1 ) && ( listTileInfo.SelectedIndices.Count > 0 ) && ( listTileInfo.SelectedIndices[0] > 0 ) );
      btnMoveTileDown.Enabled = ( ( listTileInfo.Items.Count > 1 ) && ( listTileInfo.SelectedIndices.Count > 0 ) && ( listTileInfo.SelectedIndices[0] + 1 < listTileInfo.Items.Count ) );

      if ( listTileInfo.SelectedIndices.Count == 0 )
      {
        RedrawTile();
        return;
      }

      m_CurrentEditedTile = (Formats.MapProject.Tile)listTileInfo.SelectedItems[0].Tag;

      editTileWidth.Text = m_CurrentEditedTile.Chars.Width.ToString();
      editTileHeight.Text = m_CurrentEditedTile.Chars.Height.ToString();
      editTileName.Text = m_CurrentEditedTile.Name;

      for ( int j = 0; j < m_CurrentEditedTile.Chars.Height; ++j )
      {
        for ( int i = 0; i < m_CurrentEditedTile.Chars.Width; ++i )
        {
          Formats.MapProject.TileChar character = m_CurrentEditedTile.Chars[i, j];

          ListViewItem item = new ListViewItem( ( i + j * m_CurrentEditedTile.Chars.Width ).ToString() );
          item.SubItems.Add( character.Character.ToString() );
          item.SubItems.Add( character.Color.ToString() );
          item.Tag = character;

          listTileChars.Items.Add( item );
        }
      }

      RedrawTile();
    }



    private void RedrawTile()
    {
      pictureTileDisplay.DisplayPage.Box( 0, 0, pictureTileDisplay.DisplayPage.Width, pictureTileDisplay.DisplayPage.Height, (uint)comboTileBackground.SelectedIndex );
      if ( m_CurrentEditedTile != null )
      {
        for ( int j = 0; j < m_CurrentEditedTile.Chars.Height; ++j )
        {
          for ( int i = 0; i < m_CurrentEditedTile.Chars.Width; ++i )
          {
            Formats.MapProject.TileChar character = m_CurrentEditedTile.Chars[i, j];

            DrawCharImage( pictureTileDisplay.DisplayPage, i * 8, j * 8, character.Character, character.Color );
          }
        }
      }
      pictureTileDisplay.Invalidate();
    }



    private void listTileChars_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( m_CurrentEditedTile == null )
      {
        btnCopyTileCharToNextIncreased.Enabled  = false;
        btnSetNextTileChar.Enabled              = false;
        return;
      }
      m_CurrentTileChar = null;
      if ( listTileChars.SelectedItems.Count == 0 )
      {
        btnCopyTileCharToNextIncreased.Enabled  = false;
        btnSetNextTileChar.Enabled              = false;
        return;
      }
      m_CurrentTileChar = (Formats.MapProject.TileChar)listTileChars.SelectedItems[0].Tag;

      btnCopyTileCharToNextIncreased.Enabled  = ( listTileChars.SelectedIndices[0] + 1 < listTileChars.Items.Count );
      btnSetNextTileChar.Enabled              = ( listTileChars.SelectedIndices[0] + 1 < listTileChars.Items.Count );

      panelCharacters.SelectedIndex = m_CurrentTileChar.Character;
      m_CurrentColor = m_CurrentTileChar.Color;
      RedrawColorChooser();
      panelCharColors.Invalidate();
    }



    private void btnMapAdd_Click( DecentForms.ControlBase Sender )
    {
      int w = GR.Convert.ToI32( editMapWidth.Text );
      int h = GR.Convert.ToI32( editMapHeight.Text );
      int tw = GR.Convert.ToI32( editTileSpacingW.Text );
      int th = GR.Convert.ToI32( editTileSpacingH.Text );

      if ( ( w == 0 )
      ||   ( h == 0 )
      ||   ( tw == 0 )
      ||   ( th == 0 ) )
      {
        return;
      }

      Formats.MapProject.Map map = new Formats.MapProject.Map();

      map.TileSpacingX = tw;
      map.TileSpacingY = th;
      map.Tiles.Resize( w, h );
      map.Name = editMapName.Text;

      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoMapAdd( this, m_MapProject, m_MapProject.Maps.Count ) );

      AddMap( m_MapProject.Maps.Count, map );
    }



    public void AddMap( int MapIndex, Formats.MapProject.Map Map )
    {
      m_MapProject.Maps.Insert( MapIndex, Map );

      int   mapIndex = MapIndex;
      comboMaps.Items.Insert( MapIndex, new GR.Generic.Tupel<string, Formats.MapProject.Map>( mapIndex.ToString() + ": " + Map.Name, Map ) );
      comboMaps.Enabled = true;

      for ( int i = 0; i < comboMaps.Items.Count; ++i )
      {
        GR.Generic.Tupel<string, Formats.MapProject.Map>    mapPair = (GR.Generic.Tupel<string, Formats.MapProject.Map>)comboMaps.Items[i];

        mapPair.first = i.ToString() + ": " + mapPair.second.Name;

        // force name update
        comboMaps.Items[i] = comboMaps.Items[i];
      }

      SetModified();

      AdjustScrollbars();
      RedrawMap();
    }



    private void btnMapApply_Click( DecentForms.ControlBase Sender )
    {
      if ( m_CurrentMap == null )
      {
        return;
      }

      int w = GR.Convert.ToI32( editMapWidth.Text );
      int h = GR.Convert.ToI32( editMapHeight.Text );
      int tw = GR.Convert.ToI32( editTileSpacingW.Text );
      int th = GR.Convert.ToI32( editTileSpacingH.Text );

      if ( ( w == 0 )
      ||   ( h == 0 )
      ||   ( tw == 0 )
      ||   ( th == 0 ) )
      {
        return;
      }

      if ( ( w == m_CurrentMap.Tiles.Width )
      &&   ( h == m_CurrentMap.Tiles.Height )
      &&   ( tw == m_CurrentMap.TileSpacingX )
      &&   ( th == m_CurrentMap.TileSpacingY )
      &&   ( editMapName.Text == m_CurrentMap.Name ) )
      {
        return;
      }

      // Unterschied!
      bool  firstUndo = true;
      if ( ( w != m_CurrentMap.Tiles.Width )
      ||   ( h != m_CurrentMap.Tiles.Height ) )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoMapSizeChange( this, m_CurrentMap, m_CurrentMap.Tiles.Width, m_CurrentMap.Tiles.Height ) );
        firstUndo = false;
      }
      if ( ( tw != m_CurrentMap.TileSpacingX )
      ||   ( th != m_CurrentMap.TileSpacingY )
      ||   ( editMapName.Text != m_CurrentMap.Name ) )
      {
        if ( firstUndo )
        {
          DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoMapValueChange( this, m_CurrentMap ) );
        }
        else
        {
          DocumentInfo.UndoManager.AddGroupedUndoTask( new Undo.UndoMapValueChange( this, m_CurrentMap ) );
        }
      }



      m_CurrentMap.TileSpacingX = tw;
      m_CurrentMap.TileSpacingY = th;
      m_CurrentMap.Tiles.Resize( w, h );
      m_CurrentMap.Name = editMapName.Text;

      m_SelectedTiles = new bool[w, h];

      // update name in combo
      int index = 0;
      foreach ( GR.Generic.Tupel<string, Formats.MapProject.Map> mapInfo in comboMaps.Items )
      {
        if ( mapInfo.second == m_CurrentMap )
        {
          mapInfo.first = index.ToString() + ": " + m_CurrentMap.Name;
          comboMaps.Items[index] = comboMaps.Items[index];
          break;
        }
        ++index;
      }
      AdjustScrollbars();
      RedrawMap();
      SetModified();
    }



    private void mapHScroll_Scroll( DecentForms.ControlBase Sender )
    {
      if ( m_CurEditorOffsetX != mapHScroll.Value )
      {
        m_CurEditorOffsetX = mapHScroll.Value;
        RedrawMap();
        Redraw();
      }
    }



    private void mapVScroll_Scroll( DecentForms.ControlBase Sender )
    {
      if ( m_CurEditorOffsetY != mapVScroll.Value )
      {
        m_CurEditorOffsetY = mapVScroll.Value;
        RedrawMap();
        Redraw();
      }
    }



    private void comboTiles_DrawItem( object sender, DrawItemEventArgs e )
    {
      e.DrawBackground();
      if ( ( m_CurrentMap == null )
      ||   ( e.Index == -1 ) )
      {
        e.DrawFocusRectangle();
        return;
      }

      Formats.MapProject.Tile tile = ( (GR.Generic.Tupel<string, Formats.MapProject.Tile>)comboTiles.Items[e.Index] ).second;

      int tileW = tile.Chars.Width * 8 * 2;
      int tileH = tile.Chars.Height * 8 * 2;
      System.Drawing.Rectangle itemRect = new System.Drawing.Rectangle( e.Bounds.Left + ( e.Bounds.Width - tileW ), e.Bounds.Top, tileW, e.Bounds.Height );



      GR.Image.FastImage memImage = new GR.Image.FastImage( tile.Chars.Width * 8, tile.Chars.Height * 8, GR.Drawing.PixelFormat.Format32bppRgb );
      PaletteManager.ApplyPalette( memImage );

      for ( int j = 0; j < tile.Chars.Height; ++j )
      {
        for ( int i = 0; i < tile.Chars.Width; ++i )
        {
          Formats.MapProject.TileChar character = tile.Chars[i, j];

          DrawCharImage( memImage, i * 8, j * 8, character.Character, character.Color );
        }
      }
      memImage.DrawToHDC( e.Graphics.GetHdc(), itemRect );
      memImage.Dispose();
      e.Graphics.ReleaseHdc();

      if ( ( e.State & DrawItemState.Selected ) != 0 )
      {
        e.Graphics.DrawString( e.Index.ToString() + ":" + comboTiles.Items[e.Index].ToString(), comboTiles.Font, new System.Drawing.SolidBrush( System.Drawing.Color.White ), 3.0f, e.Bounds.Top + 1.0f );
      }
      else
      {
        e.Graphics.DrawString( e.Index.ToString() + ":" + comboTiles.Items[e.Index].ToString(), comboTiles.Font, new System.Drawing.SolidBrush( System.Drawing.Color.Black ), 3.0f, e.Bounds.Top + 1.0f );
      }

      e.DrawFocusRectangle();
    }



    private void comboTiles_SelectedIndexChanged( object sender, EventArgs e )
    {
      m_CurrentEditorTile = null;
      if ( comboTiles.SelectedIndex == -1 )
      {
        return;
      }
      m_CurrentEditorTile = ( (GR.Generic.Tupel<string, Formats.MapProject.Tile>)comboTiles.SelectedItem ).second;
    }



    private void comboBackground_SelectedIndexChanged_1( object sender, EventArgs e )
    {
      m_MapProject.BackgroundColor = comboTileBackground.SelectedIndex;
      FullRebuild();
    }



    private void FullRebuild()
    {
      for ( int i = 0; i < m_MapProject.Charset.TotalNumberOfCharacters; ++i )
      {
        RebuildCharImage( i );
        if ( i < panelCharacters.Items.Count )
        {
          panelCharacters.Items[i].MemoryImage = m_MapProject.Charset.Characters[i].Tile.Image;
        }
      }
      panelCharacters.Invalidate();

      SetModified();
      RedrawTile();
      RedrawColorChooser();
      RedrawMap();
    }



    private void btnTileApply_Click( DecentForms.ControlBase Sender )
    {
      if ( m_CurrentEditedTile == null )
      {
        return;
      }
      bool    modified = false;
      if ( m_CurrentEditedTile.Name != editTileName.Text )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoMapTileModified( this, m_MapProject, listTileInfo.SelectedIndices[0] ) );
        modified = true;

        m_CurrentEditedTile.Name = editTileName.Text;

        listTileInfo.SelectedItems[0].SubItems[1].Text = m_CurrentEditedTile.Name;
        GR.Generic.Tupel<string, Formats.MapProject.Tile>      comboItem = (GR.Generic.Tupel<string, Formats.MapProject.Tile>)comboTiles.Items[listTileInfo.SelectedIndices[0]];
        comboItem.first = m_CurrentEditedTile.Name;
        SetModified();
      }

      int w = GR.Convert.ToI32( editTileWidth.Text );
      int h = GR.Convert.ToI32( editTileHeight.Text );

      if ( ( m_CurrentEditedTile.Chars.Width != w )
      ||   ( m_CurrentEditedTile.Chars.Height != h ) )
      {
        if ( !modified )
        {
          DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoMapTileModified( this, m_MapProject, listTileInfo.SelectedIndices[0] ) );
          modified = true;
        }

        m_CurrentEditedTile.Chars.Resize( w, h );
        listTileInfo.SelectedItems[0].SubItems[2].Text = w.ToString() + "x" + h.ToString();
        listTileInfo_SelectedIndexChanged( null, null );
        SetModified();
      }
    }



    private void btnTileDelete_Click( DecentForms.ControlBase Sender )
    {
      if ( listTileInfo.SelectedIndices.Count == 0 )
      {
        return;
      }
      var selectedIndices = listTileInfo.SelectedIndices;
      var indicesToRemove = new List<int>();
      for ( int i = 0; i < selectedIndices.Count; ++i )
      {
        indicesToRemove.Add( selectedIndices[i] );
      }
      if ( indicesToRemove.Count > 0 )
      {
        for ( int i = 0; i < indicesToRemove.Count; ++i )
        {
          int   indexToRemove = indicesToRemove[indicesToRemove.Count - 1 - i];

          DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoMapTileRemove( this, m_MapProject, indexToRemove ), i == 0 );
          RemoveTile( indexToRemove );
        }
      }
    }



    public void RemoveTile( int TileIndex )
    {
      // remove from all maps
      foreach ( var map in m_MapProject.Maps )
      {
        for ( int i = 0; i < map.Tiles.Width; ++i )
        {
          for ( int j = 0; j < map.Tiles.Height; ++j )
          {
            int tile = map.Tiles[i, j];
            if ( tile > TileIndex )
            {
              DocumentInfo.UndoManager.AddGroupedUndoTask( new Undo.UndoMapTilesChange( this, map, i, j, 1, 1 ) );
              map.Tiles[i, j] = tile - 1;
            }
            else if ( tile == TileIndex )
            {
              DocumentInfo.UndoManager.AddGroupedUndoTask( new Undo.UndoMapTilesChange( this, map, i, j, 1, 1 ) );
              map.Tiles[i, j] = 0;
            }
          }
        }
      }

      m_MapProject.Tiles.RemoveAt( TileIndex );
      for ( int i = 0; i < m_MapProject.Tiles.Count; ++i )
      {
        m_MapProject.Tiles[i].Index = i;
      }
      listTileInfo.Items.RemoveAt( TileIndex );
      for ( int i = TileIndex; i < listTileInfo.Items.Count; ++i )
      {
        listTileInfo.Items[i].Text = i.ToString();
      }
      listTileInfo_SelectedIndexChanged( null, null );
      comboTiles.Items.RemoveAt( TileIndex );
      m_CurrentEditedTile = null;
      listTileChars.Items.Clear();
      RedrawMap();
      SetModified();
    }



    private void btnMapDelete_Click( DecentForms.ControlBase Sender )
    {
      if ( m_CurrentMap == null )
      {
        return;
      }

      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoMapRemove( this, m_MapProject, comboMaps.SelectedIndex ) );

      RemoveMap( comboMaps.SelectedIndex );
    }



    private void btnMoveTileUp_Click( DecentForms.ControlBase Sender )
    {
      int index1 = listTileInfo.SelectedIndices[0] - 1;
      int index2 = listTileInfo.SelectedIndices[0];

      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoMapTileExchange( this, m_MapProject, index1, index2 ) );

      SwapTiles( index1, index2 );

      listTileInfo.SelectedIndices.Clear();
      listTileInfo.SelectedIndices.Add( index1 );
    }



    public void SwapTiles( int Index1, int Index2 )
    {
      Formats.MapProject.Tile tile1 = m_MapProject.Tiles[Index1];
      Formats.MapProject.Tile tile2 = m_MapProject.Tiles[Index2];

      m_MapProject.Tiles[Index1] = tile2;
      m_MapProject.Tiles[Index2] = tile1;

      m_MapProject.Tiles[Index1].Index = Index1;
      m_MapProject.Tiles[Index2].Index = Index2;

      // swap in list
      listTileInfo.Items[Index1].SubItems[1].Text = tile2.Name;
      listTileInfo.Items[Index1].SubItems[2].Text = tile2.Chars.Width.ToString() + "x" + tile2.Chars.Height.ToString();
      listTileInfo.Items[Index1].SubItems[3].Text = "0";
      listTileInfo.Items[Index1].Tag = tile2;

      listTileInfo.Items[Index2].SubItems[1].Text = tile1.Name;
      listTileInfo.Items[Index2].SubItems[2].Text = tile1.Chars.Width.ToString() + "x" + tile1.Chars.Height.ToString();
      listTileInfo.Items[Index2].SubItems[3].Text = "0";
      listTileInfo.Items[Index2].Tag = tile1;

      // swap in tile combo
      GR.Generic.Tupel<string, Formats.MapProject.Tile>    tupel1 = (GR.Generic.Tupel<string, Formats.MapProject.Tile>)comboTiles.Items[Index1];
      GR.Generic.Tupel<string, Formats.MapProject.Tile>    tupel2 = (GR.Generic.Tupel<string, Formats.MapProject.Tile>)comboTiles.Items[Index2];

      string    temp = tupel1.first;
      tupel1.first = tupel2.first;
      tupel2.first = temp;

      tupel1.second = tile2;
      tupel2.second = tile1;
         
      comboTiles.Items[Index1] = tupel1;
      comboTiles.Items[Index2] = tupel2;

      foreach ( var map in m_MapProject.Maps )
      {
        for ( int i = 0; i < map.Tiles.Width; ++i )
        {
          for ( int j = 0; j < map.Tiles.Height; ++j )
          {
            if ( map.Tiles[i, j] == Index1 )
            {
              map.Tiles[i, j] = Index2;
            }
            else if ( map.Tiles[i, j] == Index2 )
            {
              map.Tiles[i, j] = Index1;
            }
          }
        }
      }
      RedrawMap();
      SetModified();
    }



    private void btnMoveTileDown_Click( DecentForms.ControlBase Sender )
    {
      int index1 = listTileInfo.SelectedIndices[0];
      int index2 = listTileInfo.SelectedIndices[0] + 1;

      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoMapTileExchange( this, m_MapProject, index1, index2 ) );

      SwapTiles( index1, index2 );

      listTileInfo.SelectedIndices.Clear();
      listTileInfo.SelectedIndices.Add( index2 );
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



    private void editMapExtraData_TextChanged( object sender, EventArgs e )
    {
      if ( m_CurrentMap == null )
      {
        return;
      }
      if ( editMapExtraData.Text != m_CurrentMap.ExtraDataText )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoMapValueChange( this, m_CurrentMap ) );  

        Modified = true;
        m_CurrentMap.ExtraDataText = editMapExtraData.Text;
      }
    }



    private void editMapExtraData_KeyPress( object sender, KeyPressEventArgs e )
    {
      if ( ( System.Windows.Forms.Control.ModifierKeys == Keys.Control )
      &&   ( e.KeyChar == 1 ) )
      {
        editMapExtraData.SelectAll();
        e.Handled = true;
      }
    }



    public bool OpenCharpadFile( string filename )
    {
      GR.Memory.ByteBuffer projectFile = GR.IO.File.ReadAllBytes( filename );

      Formats.CharpadProject    cpProject = new RetroDevStudio.Formats.CharpadProject();
      if ( !cpProject.LoadFromFile( projectFile ) )
      {
        return false;
      }

      m_MapProject.Charset.Colors.BackgroundColor = cpProject.BackgroundColor;
      m_MapProject.Charset.Colors.MultiColor1 = cpProject.MultiColor1;
      m_MapProject.Charset.Colors.MultiColor2 = cpProject.MultiColor2;
      m_MapProject.Charset.Colors.BGColor4 = cpProject.BackgroundColor4;

      int maxChars = cpProject.NumChars;
      if ( maxChars > 256 )
      {
        maxChars = 256;
      }

      m_MapProject.Charset.ExportNumCharacters = maxChars;
      for ( int charIndex = 0; charIndex < m_MapProject.Charset.ExportNumCharacters; ++charIndex )
      {
        m_MapProject.Charset.Characters[charIndex].Tile.Data = cpProject.Characters[charIndex].Data;
        m_MapProject.Charset.Characters[charIndex].Tile.CustomColor = cpProject.Characters[charIndex].Color;

        RebuildCharImage( charIndex );
      }

      // import tiles
      m_MapProject.Maps.Clear();
      comboMaps.Items.Clear();

      m_MapProject.Tiles.Clear();
      comboTiles.Items.Clear();
      listTileInfo.Items.Clear();

      switch ( cpProject.DisplayModeFile )
      {
        case Formats.CharpadProject.DisplayMode.HIRES:
          comboMapProjectMode.SelectedIndex = (int)TextMode.COMMODORE_40_X_25_HIRES;
          m_MapProject.Charset.Mode = TextCharMode.COMMODORE_HIRES;
          break;
        case Formats.CharpadProject.DisplayMode.MULTICOLOR:
          comboMapProjectMode.SelectedIndex = (int)TextMode.COMMODORE_40_X_25_MULTICOLOR;
          m_MapProject.Charset.Mode = TextCharMode.COMMODORE_MULTICOLOR;
          break;
        case Formats.CharpadProject.DisplayMode.ECM:
          comboMapProjectMode.SelectedIndex = (int)TextMode.COMMODORE_40_X_25_ECM;
          m_MapProject.Charset.Mode = TextCharMode.COMMODORE_ECM;
          break;
      }
      characterEditor.CharsetUpdated( m_MapProject.Charset );

      for ( int i = 0; i < cpProject.NumTiles; ++i )
      {
        Formats.MapProject.Tile tile = new Formats.MapProject.Tile();

        tile.Name = "Tile " + ( i + 1 ).ToString();
        tile.Chars.Resize( cpProject.TileWidth, cpProject.TileHeight );
        tile.Index = i;

        for ( int y = 0; y < tile.Chars.Height; ++y )
        {
          for ( int x = 0; x < tile.Chars.Width; ++x )
          {
            tile.Chars[x, y].Character = (byte)cpProject.Tiles[i].CharData.UInt16At( 2 * ( x + y * tile.Chars.Width ) );
            tile.Chars[x, y].Color = cpProject.Tiles[i].ColorData.ByteAt( x + y * tile.Chars.Width );
          }
        }
        m_MapProject.Tiles.Add( tile );
        comboTiles.Items.Add( new GR.Generic.Tupel<string, Formats.MapProject.Tile>( tile.Name, tile ) );

        ListViewItem item = new ListViewItem();

        item.Text = tile.Index.ToString();
        item.SubItems.Add( tile.Name );
        item.SubItems.Add( tile.Chars.Width.ToString() + "x" + tile.Chars.Height.ToString() );
        item.SubItems.Add( "0" );
        item.Tag = tile;

        listTileInfo.Items.Add( item );
      }

      var map = new Formats.MapProject.Map();
      map.Tiles.Resize( cpProject.MapWidth, cpProject.MapHeight );
      for ( int j = 0; j < cpProject.MapHeight; ++j )
      {
        for ( int i = 0; i < cpProject.MapWidth; ++i )
        {
          map.Tiles[i, j] = cpProject.MapData.ByteAt( i + j * cpProject.MapWidth );
        }
      }
      map.TileSpacingX = cpProject.TileWidth;
      map.TileSpacingY = cpProject.TileHeight;
      if ( map.TileSpacingX <= 0 )
      {
        map.TileSpacingX = 1;
      }
      if ( map.TileSpacingY <= 0 )
      {
        map.TileSpacingY = 1;
      }
      map.Name = "Imported Map";
      m_MapProject.Maps.Add( map );
      comboMaps.Items.Add( new GR.Generic.Tupel<string, Formats.MapProject.Map>( map.Name, map ) );
      comboMaps.Enabled = true;

      comboTileBackground.SelectedIndex = m_MapProject.Charset.Colors.BackgroundColor;
      comboTileMulticolor1.SelectedIndex = m_MapProject.Charset.Colors.MultiColor1;
      comboTileMulticolor2.SelectedIndex = m_MapProject.Charset.Colors.MultiColor2;
      comboTileBGColor4.SelectedIndex = m_MapProject.Charset.Colors.BGColor4;

      RedrawMap();
      SetModified();
      return true;
    }



    private void RemoveFloatingSelection()
    {
      if ( m_FloatingSelection != null )
      {
        m_FloatingSelection = null;
        Redraw();
      }
    }



    private void btnToolEdit_CheckedChanged( DecentForms.ControlBase Sender )
    {
      HideSelection();
      RemoveFloatingSelection();
      m_ToolMode = ToolMode.SINGLE_TILE;
    }



    private void HideSelection()
    {
      if ( m_CurrentMap != null )
      {
        for ( int i = 0; i < m_CurrentMap.Tiles.Width; ++i )
        {
          for ( int j = 0; j < m_CurrentMap.Tiles.Height; ++j )
          {
            m_SelectedTiles[i, j] = false;
          }
        }
        Redraw();
      }
    }



    private void btnToolRect_CheckedChanged( DecentForms.ControlBase Sender )
    {
      HideSelection();
      RemoveFloatingSelection();
      m_ToolMode = ToolMode.RECTANGLE;
    }



    private void btnToolQuad_CheckedChanged( DecentForms.ControlBase Sender )
    {
      HideSelection();
      RemoveFloatingSelection();
      m_ToolMode = ToolMode.FILLED_RECTANGLE;
    }



    private void btnToolFill_CheckedChanged( DecentForms.ControlBase Sender )
    {
      HideSelection();
      RemoveFloatingSelection();
      m_ToolMode = ToolMode.FILL;
    }



    private void btnToolSelect_CheckedChanged( DecentForms.ControlBase Sender )
    {
      m_ToolMode = ToolMode.SELECT;
    }



    private void pictureEditor_PreviewKeyDown( object sender, PreviewKeyDownEventArgs e )
    {
      if ( e.KeyCode == Keys.Escape )
      {
        RemoveFloatingSelection();
      }
    }



    private void btnMapCopy_Click( DecentForms.ControlBase Sender )
    {
      if ( m_CurrentMap == null )
      {
        return;
      }

      var newMap = new RetroDevStudio.Formats.MapProject.Map();
      newMap.ExtraDataOld = new GR.Memory.ByteBuffer( m_CurrentMap.ExtraDataOld );
      newMap.ExtraDataText = m_CurrentMap.ExtraDataText;
      newMap.Name = m_CurrentMap.Name;
      newMap.Tiles = new GR.Game.Layer<int>();
      newMap.Tiles.Resize( m_CurrentMap.Tiles.Width, m_CurrentMap.Tiles.Height );
      for ( int i = 0; i < m_CurrentMap.Tiles.Width; ++i )
      {
        for ( int j = 0; j < m_CurrentMap.Tiles.Height; ++j )
        {
          newMap.Tiles[i,j] =  m_CurrentMap.Tiles[i,j];
        }
      }
      newMap.TileSpacingX = m_CurrentMap.TileSpacingX;
      newMap.TileSpacingY = m_CurrentMap.TileSpacingY;
      newMap.AlternativeBackgroundColor = m_CurrentMap.AlternativeBackgroundColor;
      newMap.AlternativeMultiColor1     = m_CurrentMap.AlternativeMultiColor1;
      newMap.AlternativeMultiColor2     = m_CurrentMap.AlternativeMultiColor2;


      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoMapAdd( this, m_MapProject, m_MapProject.Maps.Count ) );

      AddMap( m_MapProject.Maps.Count, newMap );
    }



    private void comboAlternativeColor_DrawItem( object sender, DrawItemEventArgs e )
    {
      ComboBox combo = (ComboBox)sender;

      e.DrawBackground();
      System.Drawing.Rectangle itemRect = new System.Drawing.Rectangle( e.Bounds.Left + 20, e.Bounds.Top, e.Bounds.Width - 20, e.Bounds.Height );

      int colorToUse = e.Index - 1;
      if ( colorToUse == -1 )
      {
        if ( combo == comboMapMultiColor1 )
        {
          colorToUse = m_MapProject.Charset.Colors.MultiColor1;
        }
        else if ( combo == comboMapMultiColor2 )
        {
          colorToUse = m_MapProject.Charset.Colors.MultiColor2;
        }
        else
        {
          colorToUse = m_MapProject.BackgroundColor;
        }
        itemRect = new System.Drawing.Rectangle( e.Bounds.Left + 80, e.Bounds.Top, e.Bounds.Width - 80, e.Bounds.Height );
      }
      e.Graphics.FillRectangle( ConstantData.Palette.ColorBrushes[colorToUse], itemRect );
      if ( ( e.State & DrawItemState.Selected ) != 0 )
      {
        e.Graphics.DrawString( combo.Items[e.Index].ToString(), combo.Font, new System.Drawing.SolidBrush( System.Drawing.Color.White ), 3.0f, e.Bounds.Top + 1.0f );
      }
      else
      {
        e.Graphics.DrawString( combo.Items[e.Index].ToString(), combo.Font, new System.Drawing.SolidBrush( System.Drawing.Color.Black ), 3.0f, e.Bounds.Top + 1.0f );
      }

    }



    private void comboMapMultiColor1_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( ( m_CurrentMap != null )
      &&   ( m_CurrentMap.AlternativeMultiColor1 + 1 != comboMapMultiColor1.SelectedIndex ) )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoMapValueChange( this, m_CurrentMap ) );  

        m_CurrentMap.AlternativeMultiColor1 = comboMapMultiColor1.SelectedIndex - 1;
        RedrawMap();
        Modified = true;
      }
    }



    private void comboMapMultiColor2_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( ( m_CurrentMap != null )
      &&   ( m_CurrentMap.AlternativeMultiColor2 + 1 != comboMapMultiColor2.SelectedIndex ) )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoMapValueChange( this, m_CurrentMap ) );  

        m_CurrentMap.AlternativeMultiColor2 = comboMapMultiColor2.SelectedIndex - 1;
        RedrawMap();
        Modified = true;
      }
    }



    private void comboMapBGColor_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( ( m_CurrentMap != null )
      &&   ( m_CurrentMap.AlternativeBackgroundColor + 1 != comboMapBGColor.SelectedIndex ) )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoMapValueChange( this, m_CurrentMap ) );  

        m_CurrentMap.AlternativeBackgroundColor = comboMapBGColor.SelectedIndex - 1;
        RedrawMap();
        Modified = true;
      }
    }



    public void InvalidateCurrentMap()
    {
      comboMaps_SelectedIndexChanged( null, null );
    }



    public void UpdateArea( int X, int Y, int Width, int Height )
    {
      for ( int i = 0; i < Width; ++i )
      {
        for ( int j = 0; j < Height; ++j )
        {
          DrawTile( X + i - m_CurEditorOffsetX, Y + j - m_CurEditorOffsetY, m_CurrentMap.Tiles[X + i, Y + j] );
        }
      }
      pictureEditor.DisplayPage.DrawTo( m_Image,
                      ( X - m_CurEditorOffsetX ) * 8 * m_CurrentMap.TileSpacingX,
                      ( Y - m_CurEditorOffsetY ) * 8 * m_CurrentMap.TileSpacingY,
                      ( X - m_CurEditorOffsetX ) * 8 * m_CurrentMap.TileSpacingX,
                      ( Y - m_CurEditorOffsetY ) * 8 * m_CurrentMap.TileSpacingY,
                      Width * 8 * m_CurrentMap.TileSpacingX, Height * 8 * m_CurrentMap.TileSpacingY );

      pictureEditor.Invalidate( new System.Drawing.Rectangle( ( X - m_CurEditorOffsetX ) * m_CurrentMap.TileSpacingX * 8,
                                                              ( Y - m_CurEditorOffsetY ) * m_CurrentMap.TileSpacingY * 8,
                                                              Width * m_CurrentMap.TileSpacingY * 8,
                                                              Height * m_CurrentMap.TileSpacingY * 8 ) );
      RedrawMap();
    }



    public void RemoveMap( int MapIndex )
    {
      if ( ( MapIndex >= 0 )
      &&   ( MapIndex < m_MapProject.Maps.Count ) )
      {
        m_MapProject.Maps.RemoveAt( MapIndex );
        comboMaps.Items.RemoveAt( MapIndex );

        for ( int i = 0; i < comboMaps.Items.Count; ++i )
        {
          GR.Generic.Tupel<string, Formats.MapProject.Map>    mapPair = (GR.Generic.Tupel<string, Formats.MapProject.Map>)comboMaps.Items[i];

          mapPair.first = i.ToString() + ": " + mapPair.second.Name;

          // force name update
          comboMaps.Items[i] = comboMaps.Items[i];
        }
        SetModified();
      }
      else
      {
        Debug.Log( "remove invalid map index" );
      }
    }



    public void TileModified( int TileIndex )
    {
      // force refresh
      listTileInfo_SelectedIndexChanged( null, null );
      listTileChars_SelectedIndexChanged( null, null );
    }



    private void comboBGColor4_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( m_MapProject.BGColor4 != comboTileBGColor4.SelectedIndex )
      {
        m_MapProject.BGColor4 = comboTileBGColor4.SelectedIndex;
        m_MapProject.Charset.Colors.BGColor4 = m_MapProject.BGColor4;
        SetModified();
        FullRebuild();
      }

    }



    private void comboMapBGColor4_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( ( m_CurrentMap != null )
      &&   ( m_CurrentMap.AlternativeBGColor4 + 1 != comboMapAlternativeBGColor4.SelectedIndex ) )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoMapValueChange( this, m_CurrentMap ) );

        m_CurrentMap.AlternativeBGColor4 = comboMapAlternativeBGColor4.SelectedIndex - 1;
        RedrawMap();
        Modified = true;
      }
    }



    private void comboMapAlternativeMode_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( ( m_CurrentMap != null )
      &&   ( (int)m_CurrentMap.AlternativeMode + 1 != comboMapAlternativeMode.SelectedIndex ) )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoMapValueChange( this, m_CurrentMap ) );

        m_CurrentMap.AlternativeMode = (TextCharMode)( comboMapAlternativeMode.SelectedIndex - 1 );

        switch ( m_CurrentMap.AlternativeMode )
        {
          case TextCharMode.COMMODORE_ECM:
          case TextCharMode.COMMODORE_HIRES:
          case TextCharMode.COMMODORE_MULTICOLOR:
            m_MapProject.Charset.Colors.Palettes[0] = Core.Imaging.PaletteFromMachine( MachineType.C64 );
            break;
          case TextCharMode.VIC20:
            m_MapProject.Charset.Colors.Palettes[0] = Core.Imaging.PaletteFromMachine( MachineType.VIC20 );
            break;
        }
        RedrawMap();
        Modified = true;
      }
    }



    private void btnSetNextTileChar_Click( DecentForms.ControlBase Sender )
    {
      if ( ( m_CurrentEditedTile == null )
      ||   ( m_CurrentTileChar == null )
      ||   ( listTileChars.SelectedIndices.Count == 0 ) )
      {
        return;
      }
      int     currentTileCharIndex = listTileChars.SelectedIndices[0];
      if ( currentTileCharIndex + 1 >= listTileChars.Items.Count )
      {
        return;
      }
      var nextChar = m_CurrentEditedTile.Chars[( currentTileCharIndex + 1 ) % m_CurrentEditedTile.Chars.Width, ( currentTileCharIndex + 1 ) / m_CurrentEditedTile.Chars.Width];

      if ( ( nextChar.Character != m_CurrentTileChar.Character )
      ||   ( nextChar.Color != m_CurrentTileChar.Color ) )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoMapTileModified( this, m_MapProject, m_CurrentEditedTile.Index ) );

        nextChar.Character = m_CurrentTileChar.Character;
        nextChar.Color = m_CurrentTileChar.Color;

        listTileChars.Items[currentTileCharIndex + 1].SubItems[1].Text = nextChar.Character.ToString();
        listTileChars.Items[currentTileCharIndex + 1].SubItems[2].Text = nextChar.Color.ToString();
        RedrawTile();
        RedrawMap();
        SetModified();
      }
      // move selection to next in any case
      listTileChars.SelectedIndices.Clear();
      listTileChars.SelectedIndices.Add( currentTileCharIndex + 1 );
    }



    private void btnCopyTileCharToNextIncreased_Click( DecentForms.ControlBase Sender )
    {
      if ( ( m_CurrentEditedTile == null )
      ||   ( m_CurrentTileChar == null )
      ||   ( listTileChars.SelectedIndices.Count == 0 ) )
      {
        return;
      }
      int     currentTileCharIndex = listTileChars.SelectedIndices[0];
      if ( currentTileCharIndex + 1 >= listTileChars.Items.Count )
      {
        return;
      }
      var nextChar = m_CurrentEditedTile.Chars[( currentTileCharIndex + 1 ) % m_CurrentEditedTile.Chars.Width, ( currentTileCharIndex + 1 ) / m_CurrentEditedTile.Chars.Width];

      if ( ( nextChar.Character != (byte)( m_CurrentTileChar.Character + 1 ) )
      ||   ( nextChar.Color != m_CurrentColor ) )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoMapTileModified( this, m_MapProject, m_CurrentEditedTile.Index ) );

        nextChar.Character  = (byte)( m_CurrentTileChar.Character + 1 );
        nextChar.Color      = m_CurrentTileChar.Color;

        listTileChars.Items[currentTileCharIndex + 1].SubItems[1].Text = nextChar.Character.ToString();
        listTileChars.Items[currentTileCharIndex + 1].SubItems[2].Text = nextChar.Color.ToString();
        RedrawTile();
        RedrawMap();
        SetModified();
      }
      // move selection to next in any case
      listTileChars.SelectedIndices.Clear();
      listTileChars.SelectedIndices.Add( currentTileCharIndex + 1 );
    }



    private void pictureTileDisplay_MouseDown( object sender, MouseEventArgs e )
    {
      if ( ( e.Button & System.Windows.Forms.MouseButtons.Left ) != 0 )
      {
        PaintTileChar( e );
      }
      if ( ( e.Button & System.Windows.Forms.MouseButtons.Right ) != 0 )
      {
        FetchTileChar( e );
      }
    }



    private void PaintTileChar( MouseEventArgs e )
    {
      if ( m_CurrentEditedTile == null )
      {
        return;
      }

      int     tx = e.X / 16;
      int     ty = e.Y / 16;

      if ( ( tx < 0 )
      ||   ( tx >= m_CurrentEditedTile.Chars.Width )
      ||   ( ty < 0 )
      ||   ( ty >= m_CurrentEditedTile.Chars.Height ) )
      {
        return;
      }

      int     currentTileCharIndex = tx + ty * m_CurrentEditedTile.Chars.Width;

      var curChar = m_CurrentEditedTile.Chars[tx, ty];

      if ( ( curChar.Character != m_CurrentChar )
      ||   ( curChar.Color != m_CurrentColor ) )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoMapTileModified( this, m_MapProject, m_CurrentEditedTile.Index ) );

        curChar.Character = m_CurrentChar;
        curChar.Color = m_CurrentColor;
        Modified = true;

        listTileChars.Items[currentTileCharIndex].SubItems[1].Text = curChar.Character.ToString();
        listTileChars.Items[currentTileCharIndex].SubItems[2].Text = curChar.Color.ToString();

        RedrawTile();
        RedrawMap();
        pictureTileDisplay.Invalidate();
      }
    }



    private void FetchTileChar( MouseEventArgs e )
    {
      if ( m_CurrentEditedTile == null )
      {
        return;
      }

      int     tx = e.X / 16;
      int     ty = e.Y / 16;

      if ( ( tx < 0 )
      ||   ( tx >= m_CurrentEditedTile.Chars.Width )
      ||   ( ty < 0 )
      ||   ( ty >= m_CurrentEditedTile.Chars.Height ) )
      {
        return;
      }

      int     currentTileCharIndex = tx + ty * m_CurrentEditedTile.Chars.Width;

      if ( ( listTileChars.SelectedIndices.Count == 0 )
      ||   ( listTileChars.SelectedIndices[0] != currentTileCharIndex ) )
      {
        listTileChars.SelectedIndices.Clear();
        listTileChars.SelectedIndices.Add( currentTileCharIndex );
      }
    }



    private void pictureTileDisplay_MouseMove( object sender, MouseEventArgs e )
    {
      if ( ( e.Button & System.Windows.Forms.MouseButtons.Left ) != 0 )
      {
        PaintTileChar( e );
      }
      if ( ( e.Button & System.Windows.Forms.MouseButtons.Right ) != 0 )
      {
        FetchTileChar( e );
      }
    }



    private void checkShowGrid_CheckedChanged( object sender, EventArgs e )
    {
      m_MapProject.ShowGrid = checkShowGrid.Checked;
      Redraw();
    }



    private void btnCloneTile_Click( DecentForms.ControlBase Sender )
    {
      if ( m_CurrentEditedTile == null )
      {
        return;
      }
      var     clonedTile = new Formats.MapProject.Tile();
      clonedTile.Name = m_CurrentEditedTile.Name + " cloned";

      clonedTile.Chars.Resize( m_CurrentEditedTile.Chars.Width, m_CurrentEditedTile.Chars.Height );

      for ( int i = 0; i < m_CurrentEditedTile.Chars.Width; ++i )
      {
        for ( int j = 0; j < m_CurrentEditedTile.Chars.Height; ++j )
        {
          var origChar = m_CurrentEditedTile.Chars[i,j];
          clonedTile.Chars[i, j].Character  = origChar.Character;
          clonedTile.Chars[i,j].Color       = origChar.Color;
        }
      }

      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoMapTileAdd( this, m_MapProject, m_MapProject.Tiles.Count ) );

      AddTile( m_MapProject.Tiles.Count, clonedTile );
    }



    private void characterEditor_Modified( List<int> AffectedChars )
    {
      RedrawMap();
      RedrawColorChooser();
      RedrawTile();
      SetModified();
    }



    private void btnCopyImage_Click( DecentForms.ControlBase Sender )
    {
      Clipboard.SetImage( m_Image.GetAsBitmap() );
    }



    private void btnExportCharset_Click( object sender, EventArgs e )
    {
      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Save Charset Project as";
      saveDlg.Filter = FilterString( Constants.FILEFILTER_CHARSET );

      if ( saveDlg.ShowDialog() != DialogResult.OK )
      {
        return;
      }
      string    extension = System.IO.Path.GetExtension( saveDlg.FileName );

      if ( extension.ToUpper() == ".CHARSETPROJECT" )
      {
        GR.IO.File.WriteAllBytes( saveDlg.FileName, m_MapProject.Charset.SaveToBuffer() );
      }
      else
      {
        GR.IO.File.WriteAllBytes( saveDlg.FileName, m_MapProject.Charset.SaveCharsetToBuffer() );
      }
    }



    private string FilterString( string Source )
    {
      return Source.Substring( 0, Source.Length - 1 );
    }



    private void btnMoveMapDown_Click( DecentForms.ControlBase Sender )
    {
      if ( ( comboMaps.SelectedIndex == -1 )
      ||   ( comboMaps.Items.Count < 2 )
      ||   ( comboMaps.SelectedIndex + 1 >= comboMaps.Items.Count ) )
      {
        return;
      }
      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoMapSwap( this, m_MapProject, comboMaps.SelectedIndex, comboMaps.SelectedIndex + 1 ) );

      int   curIndex = comboMaps.SelectedIndex;
      SwapMap( comboMaps.SelectedIndex, comboMaps.SelectedIndex + 1 );
      comboMaps.SelectedIndex = curIndex + 1;
      SetModified();
    }

    

    private void btnMoveMapUp_Click( DecentForms.ControlBase Sender )
    {
      if ( ( comboMaps.SelectedIndex <= 0 )
      ||   ( comboMaps.Items.Count < 2 ) )
      {
        return;
      }
      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoMapSwap( this, m_MapProject, comboMaps.SelectedIndex - 1, comboMaps.SelectedIndex ) );

      int   curIndex = comboMaps.SelectedIndex;
      SwapMap( comboMaps.SelectedIndex - 1, comboMaps.SelectedIndex );
      comboMaps.SelectedIndex = curIndex - 1;
      SetModified();
    }



    public void SwapMap( int MapIndex1, int MapIndex2 )
    {
      if ( ( MapIndex1 < 0 )
      ||   ( MapIndex1 >= m_MapProject.Maps.Count )
      ||   ( MapIndex2 < 0 )
      ||   ( MapIndex2 >= m_MapProject.Maps.Count ) )
      {
        return;
      }
      if ( MapIndex1 > MapIndex2 )
      {
        var map1 = m_MapProject.Maps[MapIndex1];
        m_MapProject.Maps.RemoveAt( MapIndex1 );
        m_MapProject.Maps.Insert( MapIndex2, map1 );

        var old1 = comboMaps.Items[MapIndex1];
        comboMaps.Items.RemoveAt( MapIndex1 );
        comboMaps.Items.Insert( MapIndex2, old1 );
      }
      else
      {
        var map2 = m_MapProject.Maps[MapIndex2];
        m_MapProject.Maps.RemoveAt( MapIndex2 );
        m_MapProject.Maps.Insert( MapIndex1, map2 );

        var old2 = comboMaps.Items[MapIndex2];
        comboMaps.Items.RemoveAt( MapIndex2 );
        comboMaps.Items.Insert( MapIndex1, old2 );
      }


      var item1 = (GR.Generic.Tupel<string, Formats.MapProject.Map>)comboMaps.Items[MapIndex1];
      var item2 = (GR.Generic.Tupel<string, Formats.MapProject.Map>)comboMaps.Items[MapIndex2];

      item1.first = MapIndex1.ToString() + ": " + item1.second.Name;
      item2.first = MapIndex2.ToString() + ": " + item2.second.Name;

      comboMaps.Items.RemoveAt( MapIndex2 );
      comboMaps.Items.Insert( MapIndex2, item2 );
      comboMaps.Items.RemoveAt( MapIndex1 );
      comboMaps.Items.Insert( MapIndex1, item1 );
    }



    private void characterEditor_CharactersShifted( int[] OldToNew, int[] NewToOld )
    {
      foreach ( var tile in m_MapProject.Tiles )
      {
        for ( int i = 0; i < tile.Chars.Width; ++i )
        {
          for ( int j = 0; j < tile.Chars.Height; ++j )
          {
            tile.Chars[i,j].Character = (byte)OldToNew[tile.Chars[i, j].Character];
          }
        }
      }
      for ( int i = 0; i < m_MapProject.Charset.TotalNumberOfCharacters; ++i )
      {
        panelCharacters.Items[i].MemoryImage = m_MapProject.Charset.Characters[i].Tile.Image;
      }
      RedrawMap();
      RedrawTile();
    }



    private void comboMapProjectMode_SelectedIndexChanged( object sender, EventArgs e )
    {
      //TODO Undo!

      m_MapProject.Mode = (TextMode)comboMapProjectMode.SelectedIndex;

      // TODO - that should change all kind of values inside the charset! (TotalNumberOfCharacters!)
      m_MapProject.Charset.Mode         = Lookup.TextCharModeFromTextMode( m_MapProject.Mode );
      characterEditor.CharsetUpdated( m_MapProject.Charset );

      m_MapProject.Charset.Colors.Palettes[0] = Core.Imaging.PaletteFromMachine( Lookup.MachineTypeFromTextMode( m_MapProject.Mode ) );

      for ( int i = 0; i < m_MapProject.Charset.TotalNumberOfCharacters; ++i )
      {
        RebuildCharImage( i );
      }
      Modified = true;
      panelCharacters.Invalidate();
      RedrawColorChooser();
      RedrawMap();
    }



    private void btnExport_Click( DecentForms.ControlBase Sender )
    {
      var exportInfo = new ExportMapInfo()
      {
        Map             = m_MapProject,
        RowByRow        = ( comboExportOrientation.SelectedIndex == 0 ),
        ExportType      = (MapExportType)comboExportData.SelectedIndex,
        SelectedTiles   = m_SelectedTiles,
        CurrentMap      = m_CurrentMap
      };

      editDataExport.Text = "";
      m_ExportForm.HandleExport( exportInfo, editDataExport, DocumentInfo );
    }



    private void comboExportMethod_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( m_ExportForm != null )
      {
        m_ExportForm.Dispose();
        m_ExportForm = null;
      }

      editDataExport.Text = "";

      var item = (GR.Generic.Tupel<string, Type>)comboExportMethod.SelectedItem;
      if ( ( item == null )
      ||   ( item.second == null ) )
      {
        return;
      }
      m_ExportForm = (ExportMapFormBase)Activator.CreateInstance( item.second, new object[] { Core } );
      m_ExportForm.Parent = panelExport;
      m_ExportForm.CreateControl();
    }



    private void btnImport_Click( DecentForms.ControlBase Sender )
    {
      // Undo?
      var undo = new Undo.UndoMapCharsetChange( m_MapProject, this );

      if ( m_ImportForm.HandleImport( m_MapProject, this ) )
      {
        Modified = true;
        DocumentInfo.UndoManager.AddUndoTask( undo );
        RedrawMap();
        RedrawColorChooser();
        RedrawTile();
      }
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
      ||   ( item.second == null ) )
      {
        return;
      }
      m_ImportForm = (ImportMapFormBase)Activator.CreateInstance( item.second, new object[] { Core } );
      m_ImportForm.Parent = panelImport;
      m_ImportForm.Size = panelImport.ClientSize;
      m_ImportForm.CreateControl();
    }



    internal void CharsetChanged()
    {
      characterEditor.CharsetUpdated( m_MapProject.Charset );
      RedrawMap();
      RedrawColorChooser();
      RedrawTile();
    }



    public override void OnApplicationEvent( ApplicationEvent Event )
    {
      switch ( Event.EventType )
      {
        case ApplicationEvent.Type.DEFAULT_PALETTE_CHANGED:
          {
            bool  prevModified = Modified;

            if ( !string.IsNullOrEmpty( Event.OriginalValue ) )
            {
              Core.Imaging.ApplyPalette( (PaletteType)Enum.Parse( typeof( PaletteType ), Event.OriginalValue, true ),
                                         Lookup.PaletteTypeFromTextCharMode( m_MapProject.Charset.Mode ),
                                         m_MapProject.Charset.Colors );
            }
            else
            {
              Core.Imaging.ApplyPalette( Lookup.PaletteTypeFromTextCharMode( m_MapProject.Charset.Mode ),
                                         Lookup.PaletteTypeFromTextCharMode( m_MapProject.Charset.Mode ),
                                         m_MapProject.Charset.Colors );

            }
            characterEditor.ColorsChanged();
            RedrawMap();
            RedrawColorChooser();
            RedrawTile();

            Modified = prevModified;
          }
          break;
      }
    }



    private void mapVScroll_PreviewKeyDown( object sender, PreviewKeyDownEventArgs e )
    {
      if ( e.KeyCode == Keys.Escape )
      {
        RemoveFloatingSelection();
      }
    }



    private void mapHScroll_PreviewKeyDown( object sender, PreviewKeyDownEventArgs e )
    {
      if ( e.KeyCode == Keys.Escape )
      {
        RemoveFloatingSelection();
      }
    }



  }
}


