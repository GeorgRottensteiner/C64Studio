using C64Studio.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using GR.Image;
using GR.Memory;
using C64Studio.Formats;
using RetroDevStudio;

namespace C64Studio
{
  public partial class CharsetScreenEditor : BaseDocument
  {
    private enum ToolMode
    {
      SINGLE_CHAR,
      RECTANGLE,
      FILLED_RECTANGLE,
      FILL,
      SELECT,
      TEXT
    };



    private ushort                      m_CurrentChar = 0;
    private ushort                      m_CurrentColor = 1;
    private bool                        m_OverrideCharMode = false;
    private int                         m_CharsWidth = 40;
    private int                         m_CharsHeight = 25;
    private int                         m_NumColorsInColorChooser = 16;

    private GR.Image.MemoryImage        m_Image = new GR.Image.MemoryImage( 320, 200, System.Drawing.Imaging.PixelFormat.Format32bppRgb );

    private bool[,]                     m_ErrornousChars = new bool[40, 25];
    private bool[,]                     m_SelectedChars = new bool[40, 25];
    private bool[,]                     m_ReverseCache = new bool[40, 25];

    private System.Drawing.Rectangle    m_SelectionBounds = new System.Drawing.Rectangle();


    private System.Drawing.Point        m_SelectedChar = new System.Drawing.Point( -1, -1 );

    private Formats.CharsetScreenProject    m_CharsetScreen = new C64Studio.Formats.CharsetScreenProject();

    private ToolMode                    m_ToolMode = ToolMode.SINGLE_CHAR;

    private bool                        m_MouseButtonReleased = false;
    private System.Drawing.Point        m_MousePos;

    private bool                        m_ShowGrid = false;

    private bool                        m_IsDragging = false;
    private System.Drawing.Point        m_DragStartPos = new System.Drawing.Point();
    private System.Drawing.Point        m_DragEndPos = new System.Drawing.Point();
    private System.Drawing.Point        m_LastDragEndPos = new System.Drawing.Point( -1, -1 );

    private List<GR.Generic.Tupel<bool,uint>>         m_FloatingSelection = null;
    private System.Drawing.Size                       m_FloatingSelectionSize;
    private System.Drawing.Point                      m_FloatingSelectionPos;

    private bool                        m_AffectChars = true;
    private bool                        m_AffectColors = true;
    private bool                        m_AutoCenterText = false;
    private bool                        m_ReverseChars = false;

    private int                         m_TextEntryStartedInLine = -1;
    private List<uint>                  m_TextEntryCachedLine = new List<uint>();
    private List<uint>                  m_TextEntryEnteredText = new List<uint>();



    public CharsetScreenEditor( StudioCore Core )
    {
      this.Core = Core;
      DocumentInfo.Type = ProjectElement.ElementType.CHARACTER_SCREEN;
      DocumentInfo.UndoManager.MainForm = Core.MainForm;
      m_IsSaveable = true;
      InitializeComponent();
      charEditor.Core = Core;

      charEditor.UndoManager = DocumentInfo.UndoManager;

      pictureEditor.DisplayPage.Create( 320, 200, System.Drawing.Imaging.PixelFormat.Format32bppRgb );
      panelCharacters.PixelFormat = System.Drawing.Imaging.PixelFormat.Format32bppRgb;
      panelCharacters.SetDisplaySize( 128, 128 );
      panelCharColors.DisplayPage.Create( 128, 8, System.Drawing.Imaging.PixelFormat.Format32bppRgb );
      m_Image.Create( 320, 200, System.Drawing.Imaging.PixelFormat.Format32bppRgb );

      DPIHandler.ResizeControlsForDPI( this );
      ApplyPalette();
      for ( int i = 0; i < m_CharsetScreen.CharSet.Colors.Palette.NumColors; ++i )
      {
        comboBackground.Items.Add( i.ToString( "d2" ) );
        comboMulticolor1.Items.Add( i.ToString( "d2" ) );
        comboMulticolor2.Items.Add( i.ToString( "d2" ) );
        comboBGColor4.Items.Add( i.ToString( "d2" ) );
      }
      comboBackground.SelectedIndex = 0;
      comboMulticolor1.SelectedIndex = 0;
      comboMulticolor2.SelectedIndex = 0;
      comboBGColor4.SelectedIndex = 0;

      comboExportOrientation.SelectedIndex = 0;
      comboExportData.SelectedIndex = 0;

      comboExportArea.Items.Add( "All" );
      comboExportArea.Items.Add( "Selection" );
      comboExportArea.Items.Add( "Custom Area" );
      comboExportArea.SelectedIndex = 0;

      foreach ( TextMode mode in Enum.GetValues( typeof( TextMode ) ) )
      {
        comboCharsetMode.Items.Add( GR.EnumHelper.GetDescription( mode ) );
      }
      comboCharsetMode.SelectedIndex = 0;

      comboBasicFiles.Items.Add( new Types.ComboItem( "To new file" ) );
      comboCharsetFiles.Items.Add( new Types.ComboItem( "To new charset project" ) );
      foreach ( BaseDocument doc in Core.MainForm.panelMain.Documents )
      {
        if ( doc.DocumentInfo.Type == ProjectElement.ElementType.BASIC_SOURCE )
        {
          string    nameToUse = doc.DocumentFilename ?? "New File";
          comboBasicFiles.Items.Add( new Types.ComboItem( nameToUse, doc.DocumentInfo ) );
        }
        else if ( doc.DocumentInfo.Type == ProjectElement.ElementType.CHARACTER_SET )
        {
          string    nameToUse = doc.DocumentFilename ?? "New File";
          comboCharsetFiles.Items.Add( new Types.ComboItem( nameToUse, doc.DocumentInfo ) );
        }
      }
      comboBasicFiles.SelectedIndex = 0;
      comboCharsetFiles.SelectedIndex = 0;

      Core.MainForm.ApplicationEvent += new MainForm.ApplicationEventHandler( MainForm_ApplicationEvent );

      checkExportToDataIncludeRes.Checked = true;
      checkExportToDataWrap.Checked = true;

      for ( int i = 0; i < m_CharsetScreen.CharSet.TotalNumberOfCharacters; ++i )
      {
        for ( int j = 0; j < 8; ++j )
        {
          m_CharsetScreen.CharSet.Characters[i].Tile.CustomColor = 1;
          m_CharsetScreen.CharSet.Characters[i].Tile.Data.SetU8At( j, ConstantData.UpperCaseCharsetC64.ByteAt( i * 8 + j ) );
        }
      }

      editScreenWidth.Text = "40";
      editScreenHeight.Text = "25";

      AdjustScrollbars();

      for ( int i = 0; i < m_CharsetScreen.CharSet.TotalNumberOfCharacters; ++i )
      {
        RebuildCharImage( i );
        panelCharacters.Items.Add( i.ToString(), m_CharsetScreen.CharSet.Characters[i].Tile.Image );
      }
      charEditor.CharsetUpdated( m_CharsetScreen.CharSet );
    }



    private void ApplyPalette()
    {
      PaletteManager.ApplyPalette( pictureEditor.DisplayPage, m_CharsetScreen.CharSet.Colors.Palette );
      PaletteManager.ApplyPalette( panelCharacters.DisplayPage, m_CharsetScreen.CharSet.Colors.Palette );
      PaletteManager.ApplyPalette( m_Image, m_CharsetScreen.CharSet.Colors.Palette );
      PaletteManager.ApplyPalette( panelCharColors.DisplayPage, m_CharsetScreen.CharSet.Colors.Palette );
    }



    public override DocumentInfo DocumentInfo
    {
      get
      {
        return base.DocumentInfo;
      }
      set
      {
        base.DocumentInfo = value;
        charEditor.UndoManager = DocumentInfo.UndoManager;
      }
    }



    private void pictureEditor_PostPaint( FastImage TargetBuffer )
    {
      if ( m_ShowGrid )
      {
        for ( int i = 0; i < m_CharsetScreen.ScreenWidth; ++i )
        {
          for ( int j = 0; j < TargetBuffer.Height; ++j )
          {
            TargetBuffer.SetPixel( i * pictureEditor.ClientRectangle.Width / m_CharsWidth, j, 0xffc0c0c0 );
          }
        }
        for ( int i = 0; i < m_CharsetScreen.ScreenHeight; ++i )
        {
          for ( int j = 0; j < TargetBuffer.Width; ++j )
          {
            TargetBuffer.SetPixel( j, i * pictureEditor.ClientRectangle.Height / m_CharsHeight, 0xffc0c0c0 );
          }
        }
      }

      // draw outside area
      uint  selColor = Core.Settings.FGColor( ColorableElement.SELECTION_FRAME );

      int     fillWidth = 0;
      int     fillHeight = 0;

      if ( m_CharsetScreen.ScreenWidth - m_CharsetScreen.ScreenOffsetX < m_CharsWidth )
      {
        fillWidth = m_CharsWidth - ( m_CharsetScreen.ScreenWidth - m_CharsetScreen.ScreenOffsetX );
      }
      if ( m_CharsetScreen.ScreenHeight - m_CharsetScreen.ScreenOffsetY < m_CharsHeight )
      {
        fillHeight = m_CharsHeight - ( m_CharsetScreen.ScreenHeight - m_CharsetScreen.ScreenOffsetY );
      }
      if ( ( fillWidth > 0 )
      &&   ( fillHeight > 0 ) )
      {
        // bottom right
        TargetBuffer.Box( ( m_CharsWidth - fillWidth ) * ( pictureEditor.ClientRectangle.Width / m_CharsWidth ),
                          ( m_CharsHeight - fillHeight ) * ( pictureEditor.ClientRectangle.Height / m_CharsHeight ),
                          pictureEditor.ClientRectangle.Width - ( m_CharsWidth - fillWidth ) * ( pictureEditor.ClientRectangle.Width / m_CharsWidth ),
                          pictureEditor.ClientRectangle.Height - ( m_CharsHeight - fillHeight ) * ( pictureEditor.ClientRectangle.Height / m_CharsHeight ),
                          selColor );
      }
      if ( fillWidth > 0 )
      {
        // right
        TargetBuffer.Box( ( m_CharsWidth - fillWidth ) * ( pictureEditor.ClientRectangle.Width / m_CharsWidth ),
                          0,
                          pictureEditor.ClientRectangle.Width - ( m_CharsWidth - fillWidth ) * ( pictureEditor.ClientRectangle.Width / m_CharsWidth ),
                          pictureEditor.ClientRectangle.Height,
                          selColor );
      }
      if ( fillHeight > 0 )
      {
        // bottom
        TargetBuffer.Box( 0,
                          ( m_CharsHeight - fillHeight ) * ( pictureEditor.ClientRectangle.Height / m_CharsHeight ),
                          pictureEditor.ClientRectangle.Width,
                          pictureEditor.ClientRectangle.Height - ( m_CharsHeight - fillHeight ) * ( pictureEditor.ClientRectangle.Height / m_CharsHeight ),
                          selColor );
      }

      // mark selected char
      if ( m_SelectedChar.X != -1 )
      {
        int  sx1 = ( m_SelectedChar.X - m_CharsetScreen.ScreenOffsetX ) * ( pictureEditor.ClientRectangle.Width / m_CharsWidth );
        int  sx2 = ( m_SelectedChar.X + 1 - m_CharsetScreen.ScreenOffsetX ) * ( pictureEditor.ClientRectangle.Width / m_CharsWidth );
        int  sy1 = ( m_SelectedChar.Y - m_CharsetScreen.ScreenOffsetY ) * ( pictureEditor.ClientRectangle.Height / m_CharsHeight );
        int  sy2 = ( m_SelectedChar.Y + 1 - m_CharsetScreen.ScreenOffsetY ) * ( pictureEditor.ClientRectangle.Height / m_CharsHeight );

        if ( m_SelectedChar.X - m_CharsetScreen.ScreenOffsetX == m_CharsWidth - 1 )
        {
          --sx2;
        }
        if ( m_SelectedChar.Y - m_CharsetScreen.ScreenOffsetY == m_CharsHeight - 1 )
        {
          --sy2;
        }

        TargetBuffer.Rectangle( sx1, sy1, sx2 - sx1 + 1, sy2 - sy1 + 1, selColor );
      }

      // current dragged selection
      if ( ( m_ToolMode == ToolMode.SELECT )
      &&   ( m_LastDragEndPos.X != -1 ) )
      {
        System.Drawing.Point    o1, o2;

        CalcRect( m_DragStartPos, m_LastDragEndPos, out o1, out o2 );

        int  sx1 = ( o1.X - m_CharsetScreen.ScreenOffsetX ) * ( pictureEditor.ClientRectangle.Width / m_CharsWidth );
        int  sx2 = ( o2.X + 1 - m_CharsetScreen.ScreenOffsetX ) * ( pictureEditor.ClientRectangle.Width / m_CharsWidth );
        int  sy1 = ( o1.Y - m_CharsetScreen.ScreenOffsetY ) * ( pictureEditor.ClientRectangle.Height / m_CharsHeight );
        int  sy2 = ( o2.Y + 1 - m_CharsetScreen.ScreenOffsetY ) * ( pictureEditor.ClientRectangle.Height / m_CharsHeight );

        if ( m_SelectedChar.X - m_CharsetScreen.ScreenOffsetX == m_CharsWidth - 1 )
        {
          --sx2;
        }
        if ( m_SelectedChar.Y - m_CharsetScreen.ScreenOffsetY == m_CharsHeight - 1 )
        {
          --sy2;
        }

        TargetBuffer.Rectangle( sx1, sy1, sx2 - sx1 + 1, sy2 - sy1 + 1, selColor );
      }

      // draw selection
      int     x1 = m_CharsetScreen.ScreenOffsetX;
      int     y1 = m_CharsetScreen.ScreenOffsetY;
      int     x2 = x1 + m_CharsetScreen.ScreenWidth - 1;
      int     y2 = y1 + m_CharsetScreen.ScreenHeight - 1;

      if ( x1 < 0 )
      {
        x1 = 0;
      }
      if ( x2 >= m_CharsetScreen.ScreenWidth )
      {
        x2 = m_CharsetScreen.ScreenWidth - 1;
      }
      if ( x2 - x1 > m_CharsWidth )
      {
        x2 = x1 + m_CharsWidth - 1;
      }
      if ( y1 < 0 )
      {
        y1 = 0;
      }
      if ( y2 >= m_CharsetScreen.ScreenHeight )
      {
        y2 = m_CharsetScreen.ScreenHeight - 1;
      }
      if ( y2 - y1 > m_CharsHeight )
      {
        y2 = y1 + m_CharsHeight - 1;
      }

      for ( int x = x1; x <= x2; ++x )
      {
        for ( int y = y1; y <= y2; ++y )
        {
          if ( m_SelectedChars[x, y] )
          {
            int  sx1 = ( x - m_CharsetScreen.ScreenOffsetX ) * ( pictureEditor.ClientRectangle.Width / m_CharsWidth );
            int  sx2 = ( x + 1 - m_CharsetScreen.ScreenOffsetX ) * ( pictureEditor.ClientRectangle.Width / m_CharsWidth );
            int  sy1 = ( y - m_CharsetScreen.ScreenOffsetY ) * ( pictureEditor.ClientRectangle.Height / m_CharsHeight );
            int  sy2 = ( y + 1 - m_CharsetScreen.ScreenOffsetY ) * ( pictureEditor.ClientRectangle.Height / m_CharsHeight );

            if ( x == x2 )
            {
              --sx2;
            }
            if ( y == y2 )
            {
              --sy2;
            }

            if ( ( y == 0 )
            ||   ( !m_SelectedChars[x, y - 1] ) )
            {
              for ( int i = sx1; i <= sx2; ++i )
              {
                TargetBuffer.SetPixel( i, sy1, selColor );
              }
            }
            if ( ( y == m_SelectedChars.GetUpperBound( 1 ) )
            ||   ( !m_SelectedChars[x, y + 1] ) )
            {
              for ( int i = sx1; i <= sx2; ++i )
              {
                TargetBuffer.SetPixel( i, sy2, selColor );
              }
            }
            if ( ( x == 0 )
            ||   ( !m_SelectedChars[x - 1, y] ) )
            {
              for ( int i = sy1; i <= sy2; ++i )
              {
                TargetBuffer.SetPixel( sx1, i, selColor );
              }
            }
            if ( ( x == m_SelectedChars.GetUpperBound( 0 ) )
            ||   ( !m_SelectedChars[x + 1, y] ) )
            {
              for ( int i = sy1; i <= sy2; ++i )
              {
                TargetBuffer.SetPixel( sx2, i, selColor );
              }
            }
          }
        }
      }
    }



    void MainForm_ApplicationEvent( C64Studio.Types.ApplicationEvent Event )
    {
      if ( Event.EventType == ApplicationEvent.Type.ELEMENT_CREATED )
      {
        if ( Event.Doc.Type == ProjectElement.ElementType.BASIC_SOURCE )
        {
          foreach ( ComboItem item in comboBasicFiles.Items )
          {
            if ( (DocumentInfo)item.Tag == Event.Doc )
            {
              return;
            }
          }

          string    nameToUse = Event.Doc.DocumentFilename ?? "New File";
          comboBasicFiles.Items.Add( new Types.ComboItem( nameToUse, Event.Doc ) );
        }
        else if ( Event.Doc.Type == ProjectElement.ElementType.CHARACTER_SET )
        {
          foreach ( ComboItem item in comboCharsetFiles.Items )
          {
            if ( (DocumentInfo)item.Tag == Event.Doc )
            {
              return;
            }
          }

          string    nameToUse = Event.Doc.DocumentFilename ?? "New File";
          comboCharsetFiles.Items.Add( new Types.ComboItem( nameToUse, Event.Doc ) );
        }
      }
      if ( Event.EventType == ApplicationEvent.Type.ELEMENT_REMOVED )
      {
        if ( Event.Doc.Type == ProjectElement.ElementType.BASIC_SOURCE )
        {
          foreach ( Types.ComboItem comboItem in comboBasicFiles.Items )
          {
            if ( (DocumentInfo)comboItem.Tag == Event.Doc )
            {
              comboBasicFiles.Items.Remove( comboItem );
              if ( comboBasicFiles.SelectedIndex == -1 )
              {
                comboBasicFiles.SelectedIndex = 0;
              }
              break;
            }
          }
        }
        else if ( Event.Doc.Type == ProjectElement.ElementType.CHARACTER_SET )
        {
          foreach ( Types.ComboItem comboItem in comboCharsetFiles.Items )
          {
            if ( (DocumentInfo)comboItem.Tag == Event.Doc )
            {
              comboCharsetFiles.Items.Remove( comboItem );
              if ( comboCharsetFiles.SelectedIndex == -1 )
              {
                comboCharsetFiles.SelectedIndex = 0;
              }
              break;
            }
          }
        }
      }
    }



    protected override void OnClosed( EventArgs e )
    {
      Core.MainForm.ApplicationEvent -= MainForm_ApplicationEvent;
      base.OnClosed( e );
    }



    void RebuildCharImage( int CharIndex )
    {
      Formats.CharData Char = m_CharsetScreen.CharSet.Characters[CharIndex];

      if ( m_OverrideCharMode )
      {
        Displayer.CharacterDisplayer.DisplayChar( m_CharsetScreen.CharSet, m_CharsetScreen.CharSet.Colors.Palette, CharIndex, Char.Tile.Image, 0, 0, m_CurrentColor,
                m_CharsetScreen.CharSet.Colors.BackgroundColor,
                m_CharsetScreen.CharSet.Colors.MultiColor1,
                m_CharsetScreen.CharSet.Colors.MultiColor2,
                m_CharsetScreen.CharSet.Colors.BGColor4,
                Lookup.TextCharModeFromTextMode( m_CharsetScreen.Mode ) );
      }
      else
      {
        Displayer.CharacterDisplayer.DisplayChar( m_CharsetScreen.CharSet, m_CharsetScreen.CharSet.Colors.Palette, CharIndex, Char.Tile.Image, 0, 0 );
      }
    }



    void DrawCharImage( GR.Image.IImage TargetImage, int X, int Y, ushort Char, ushort Color )
    {
      Displayer.CharacterDisplayer.DisplayChar( m_CharsetScreen.CharSet, m_CharsetScreen.CharSet.Colors.Palette, Char, TargetImage, X, Y, Color );
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

      Core.Theming.DrawSingleColorComboBox( combo, e, m_CharsetScreen.CharSet.Colors.Palette );
    }



    private void comboMulticolor_DrawItem( object sender, DrawItemEventArgs e )
    {
      ComboBox combo = (ComboBox)sender;

      Core.Theming.DrawMultiColorComboBox( combo, e, m_CharsetScreen.CharSet.Colors.Palette );
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

      int     undoX = Math.Max( m_MousePos.X, 0 );
      int     undoY = Math.Max( m_MousePos.Y, 0 );
      int     offsetX = undoX - m_MousePos.X;
      int     offsetY = undoY - m_MousePos.Y;
      int     undoWidth = m_FloatingSelectionSize.Width - ( undoX - m_MousePos.X );
      int     undoHeight = m_FloatingSelectionSize.Height - ( undoY - m_MousePos.Y );
      if ( undoX + undoWidth > m_CharsetScreen.ScreenWidth )
      {
        undoWidth = m_CharsetScreen.ScreenWidth - undoX;
      }
      if ( undoY + undoHeight > m_CharsetScreen.ScreenHeight )
      {
        undoHeight = m_CharsetScreen.ScreenHeight - undoY;
      }
      if ( ( undoWidth <= 0 )
      ||   ( undoHeight <= 0 ) )
      {
        m_FloatingSelection = null;
        return;
      }
      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenCharChange( m_CharsetScreen, this, undoX, undoY, undoWidth, undoHeight ) );

      for ( int j = 0; j < undoHeight; ++j )
      {
        for ( int i = 0; i < undoWidth; ++i )
        {
          if ( ( offsetX + i >= 0 )
          &&   ( offsetX + i < m_CharsetScreen.ScreenWidth )
          &&   ( offsetY + j >= 0 )
          &&   ( offsetY + j < m_CharsetScreen.ScreenHeight ) )
          {
            var selectionChar = m_FloatingSelection[( offsetX + i ) + ( offsetY + j ) * m_FloatingSelectionSize.Width];
            if ( selectionChar.first )
            {
              m_CharsetScreen.Chars[undoX + i + m_CharsetScreen.ScreenOffsetX + ( undoY + j + m_CharsetScreen.ScreenOffsetY ) * m_CharsetScreen.ScreenWidth] = selectionChar.second;

              DrawCharImage( pictureEditor.DisplayPage,
                 ( undoX + i ) * 8,
                 ( undoY + j ) * 8,
                 (ushort)( selectionChar.second & 0xffff ),
                 (ushort)( selectionChar.second >> 16 ) );

              DrawCharImage( m_Image,
                 ( m_CharsetScreen.ScreenOffsetX + undoX + i ) * 8,
                 ( m_CharsetScreen.ScreenOffsetY + undoY + j ) * 8,
                 (ushort)( selectionChar.second & 0xffff ),
                 (ushort)( selectionChar.second >> 16 ) );

              pictureEditor.Invalidate( new System.Drawing.Rectangle( ( undoX + i ) * 8,
                                                                      ( undoY + j ) * 8,
                                                                      8, 8 ) );
            }
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

      uint charToFill = m_CharsetScreen.Chars[X + m_CharsetScreen.ScreenWidth * Y];
      uint charToInsert = (uint)( m_CurrentChar | ( m_CurrentColor << 16 ) );
      if ( charToFill == charToInsert )
      {
        return;
      }

      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenCharChange( m_CharsetScreen, this, 0, 0, m_CharsetScreen.ScreenWidth, m_CharsetScreen.ScreenHeight ) );

      while ( pointsToCheck.Count != 0 )
      {
        System.Drawing.Point    point = pointsToCheck[pointsToCheck.Count - 1];
        pointsToCheck.RemoveAt( pointsToCheck.Count - 1 );

        if ( m_CharsetScreen.Chars[point.X + m_CharsetScreen.ScreenWidth * point.Y] != charToInsert )
        {
          DrawCharImage( m_Image, point.X * 8, point.Y * 8, m_CurrentChar, m_CurrentColor );
          DrawCharImage( pictureEditor.DisplayPage, ( point.X - m_CharsetScreen.ScreenOffsetX ) * 8, ( point.Y - m_CharsetScreen.ScreenOffsetY ) * 8, m_CurrentChar, m_CurrentColor );

          m_CharsetScreen.Chars[point.X + m_CharsetScreen.ScreenWidth * point.Y] = charToInsert;

          if ( ( point.X > 0 )
          && ( m_CharsetScreen.Chars[point.X - 1 + m_CharsetScreen.ScreenWidth * point.Y] == charToFill ) )
          {
            pointsToCheck.Add( new System.Drawing.Point( point.X - 1, point.Y ) );
          }
          if ( ( point.X + 1 < m_CharsetScreen.ScreenWidth )
          && ( m_CharsetScreen.Chars[point.X + 1 + m_CharsetScreen.ScreenWidth * point.Y] == charToFill ) )
          {
            pointsToCheck.Add( new System.Drawing.Point( point.X + 1, point.Y ) );
          }
          if ( ( point.Y > 0 )
          && ( m_CharsetScreen.Chars[point.X + m_CharsetScreen.ScreenWidth * ( point.Y - 1 )] == charToFill ) )
          {
            pointsToCheck.Add( new System.Drawing.Point( point.X, point.Y - 1 ) );
          }
          if ( ( point.Y + 1 < m_CharsetScreen.ScreenHeight )
          && ( m_CharsetScreen.Chars[point.X + m_CharsetScreen.ScreenWidth * ( point.Y + 1 )] == charToFill ) )
          {
            pointsToCheck.Add( new System.Drawing.Point( point.X, point.Y + 1 ) );
          }
        }
      }
      Modified = true;
      Redraw();
    }



    private string InfoText()
    {
      StringBuilder   sb = new StringBuilder();

      int     charX = m_MousePos.X + m_CharsetScreen.ScreenOffsetX;
      int     charY = m_MousePos.Y + m_CharsetScreen.ScreenOffsetY;
      sb.Append( "Pos " );
      sb.Append( charX );
      sb.Append( ',' );
      sb.Append( charY );
      sb.Append( "  Offset $" );
      sb.Append( ( charX + charY * m_CharsetScreen.ScreenWidth ).ToString( "X4" ) );
      sb.Append( "  Char $" );
      sb.Append( m_CurrentChar.ToString( "X2" ) );
      sb.Append( ',' );
      sb.Append( m_CurrentChar );
      sb.Append( "  Color $" );
      sb.Append( m_CurrentColor.ToString( "X2" ) );
      sb.Append( ',' );
      sb.Append( m_CurrentColor );
      sb.AppendLine();
      sb.Append( "Sprite Pos $" );

      int spritePosX = charX * 8 + 24;
      int spritePosY = charY * 8 + 50;
      sb.Append( spritePosX.ToString( "X3" ) );
      sb.Append( '/' );
      sb.Append( spritePosX );
      sb.Append( ", $" );
      sb.Append( spritePosY.ToString( "X2" ) );
      sb.Append( '/' );
      sb.Append( spritePosY );
      sb.AppendLine();

      if ( m_SelectionBounds.Width > 0 )
      {
        sb.Append( "Selection " );
        sb.Append( m_SelectionBounds.X );
        sb.Append( ", " );
        sb.Append( m_SelectionBounds.Y );
        sb.Append( " " );
        sb.Append( m_SelectionBounds.Width );
        sb.Append( "*" );
        sb.Append( m_SelectionBounds.Height );
        sb.AppendLine();
      }

      return sb.ToString();
    }



    private void HandleMouseOnEditor( int X, int Y, MouseButtons Buttons )
    {
      int     charX = ( m_CharsWidth * X ) / pictureEditor.ClientRectangle.Width + m_CharsetScreen.ScreenOffsetX;
      int     charY = ( m_CharsHeight * Y ) / pictureEditor.ClientRectangle.Height + m_CharsetScreen.ScreenOffsetY;

      m_MousePos.X = charX - m_CharsetScreen.ScreenOffsetX;
      m_MousePos.Y = charY - m_CharsetScreen.ScreenOffsetY;
      if ( m_FloatingSelection != null )
      {
        if ( m_MousePos != m_FloatingSelectionPos )
        {
          m_FloatingSelectionPos = m_MousePos;
          Redraw();
          pictureEditor.Invalidate();
        }
      }

      labelInfo.Text = InfoText();

      if ( ( Buttons & MouseButtons.Left ) == 0 )
      {
        m_MouseButtonReleased = true;

        switch ( m_ToolMode )
        {
          case ToolMode.RECTANGLE:
          case ToolMode.FILLED_RECTANGLE:
            if ( m_IsDragging )
            {
              m_IsDragging = false;
              if ( m_LastDragEndPos.X != -1 )
              {
                m_LastDragEndPos.X = -1;
                m_LastDragEndPos.Y = -1;

                System.Drawing.Point    p1, p2;

                CalcRect( m_DragStartPos, m_DragEndPos, out p1, out p2 );

                DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenCharChange( m_CharsetScreen, this, p1.X, p1.Y, p2.X - p1.X + 1, p2.Y - p1.Y + 1 ) );

                if ( m_ToolMode == ToolMode.RECTANGLE )
                {
                  for ( int x = p1.X; x <= p2.X; ++x )
                  {
                    SetCharacter( x, p1.Y );
                    SetCharacter( x, p2.Y );
                  }
                  for ( int y = p1.Y + 1; y <= p2.Y - 1; ++y )
                  {
                    SetCharacter( p1.X, y );
                    SetCharacter( p2.X, y );
                  }
                }
                else
                {
                  for ( int x = p1.X; x <= p2.X; ++x )
                  {
                    for ( int y = p1.Y; y <= p2.Y; ++y )
                    {
                      SetCharacter( x, y );
                    }
                  }
                }
                RedrawFullScreen();
                pictureEditor.Invalidate();
                Modified = true;
              }
              return;
            }
            break;
          case ToolMode.SELECT:
            if ( m_IsDragging )
            {
              m_IsDragging = false;
              if ( m_LastDragEndPos.X != -1 )
              {
                m_LastDragEndPos.X = -1;
                m_LastDragEndPos.Y = -1;

                System.Drawing.Point    p1, p2;

                CalcRect( m_DragStartPos, m_DragEndPos, out p1, out p2 );

                bool shiftPressed = ( ( ModifierKeys & Keys.Shift ) == Keys.Shift );

                if ( ( !shiftPressed )
                &&   ( ( ModifierKeys & Keys.Control ) == Keys.None ) )
                {
                  // not ctrl-Click, remove previous selection
                  for ( int x = 0; x < m_CharsetScreen.ScreenWidth; ++x )
                  {
                    for ( int y = 0; y < m_CharsetScreen.ScreenHeight; ++y )
                    {
                      m_SelectedChars[x, y] = false;
                    }
                  }
                }

                for ( int x = p1.X; x <= p2.X; ++x )
                {
                  for ( int y = p1.Y; y <= p2.Y; ++y )
                  {
                    if ( shiftPressed )
                    {
                      m_SelectedChars[x, y] = false;
                    }
                    else
                    {
                      m_SelectedChars[x, y] = true;
                    }
                  }
                }
                RecalcSelectionBounds();
                labelInfo.Text = InfoText();
                pictureEditor.Invalidate();
                Redraw();
              }
            }
            break;
        }
      }

      if ( ( charX < 0 )
      ||   ( charX >= m_CharsetScreen.ScreenWidth )
      ||   ( charY < 0 )
      ||   ( charY >= m_CharsetScreen.ScreenHeight ) )
      {
        return;
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
          case ToolMode.TEXT:
            if ( ( m_SelectedChar.X != charX )
            ||   ( m_SelectedChar.Y != charY ) )
            {
              m_SelectedChar.X = charX;
              m_SelectedChar.Y = charY;

              Redraw();
              pictureEditor.Invalidate();

              if ( ( m_AutoCenterText )
              &&   ( m_SelectedChar.Y != m_TextEntryStartedInLine ) )
              {
                // clicked on different line
                m_TextEntryStartedInLine = m_SelectedChar.Y;
                CacheScreenLine( m_TextEntryStartedInLine );
                m_TextEntryEnteredText.Clear();
              }
            }
            break;
          case ToolMode.SINGLE_CHAR:
            if ( ( m_ReverseChars )
            ||   ( m_CharsetScreen.Chars[charX + charY * m_CharsetScreen.ScreenWidth] != (uint)( m_CurrentChar | ( m_CurrentColor << 16 ) ) ) )
            {
              DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenCharChange( m_CharsetScreen, this, charX, charY, 1, 1 ), m_MouseButtonReleased );
              m_MouseButtonReleased = false;

              SetCharacter( charX, charY );
              pictureEditor.DisplayPage.DrawTo( m_Image,
                                                charX * 8, charY * 8,
                                                ( charX - m_CharsetScreen.ScreenOffsetX ) * 8, ( charY - m_CharsetScreen.ScreenOffsetY ) * 8,
                                                8, 8 );

              pictureEditor.Invalidate( new System.Drawing.Rectangle( X, Y, 8, 8 ) );
              Modified = true;
            }
            break;
          case ToolMode.FILL:
            if ( m_MouseButtonReleased )
            {
              m_MouseButtonReleased = false;

              FillContent( charX, charY );
            }
            break;
          case ToolMode.RECTANGLE:
          case ToolMode.FILLED_RECTANGLE:
            if ( m_MouseButtonReleased )
            {
              m_MouseButtonReleased = false;
              m_IsDragging = true;

              // first point
              m_DragStartPos.X = charX;
              m_DragStartPos.Y = charY;
              m_LastDragEndPos = new System.Drawing.Point( -1, -1 );
            }
            if ( !m_IsDragging )
            {
              return;
            }

            // draw other point
            m_DragEndPos.X = charX;
            m_DragEndPos.Y = charY;

            if ( ( charX == m_CharsetScreen.ScreenOffsetX + m_CharsWidth - 1 )
            &&   ( screenHScroll.Value < screenHScroll.Maximum ) )
            {
              // autoscroll right
              ++screenHScroll.Value;
              ++m_CharsetScreen.ScreenOffsetX;
              ++charX;
              m_DragEndPos.X = charX;
            }
            if ( ( charX == m_CharsetScreen.ScreenOffsetX )
            &&   ( screenHScroll.Value > screenHScroll.Minimum ) )
            {
              // autoscroll left
              --screenHScroll.Value;
              --m_CharsetScreen.ScreenOffsetX;
              --charX;
              m_DragEndPos.X = charX;
            }
            if ( ( charY == m_CharsetScreen.ScreenOffsetY + m_CharsHeight - 1 )
            &&   ( screenVScroll.Value < screenVScroll.Maximum ) )
            {
              // autoscroll down
              ++screenVScroll.Value;
              ++m_CharsetScreen.ScreenOffsetY;
              ++charY;
              m_DragEndPos.Y = charY;
            }
            if ( ( charY == m_CharsetScreen.ScreenOffsetY )
            &&   ( screenVScroll.Value > screenVScroll.Minimum ) )
            {
              // autoscroll up
              --screenVScroll.Value;
              --m_CharsetScreen.ScreenOffsetY;
              --charY;
              m_DragEndPos.Y = charY;
            }

            if ( m_DragEndPos != m_LastDragEndPos )
            {
              m_LastDragEndPos = m_DragEndPos;

              Redraw();
              pictureEditor.Invalidate();
            }
            break;
          case ToolMode.SELECT:
            if ( m_MouseButtonReleased )
            {
              m_MouseButtonReleased = false;
              m_IsDragging = true;

              // first point
              m_DragStartPos.X = charX;
              m_DragStartPos.Y = charY;
              m_LastDragEndPos = new System.Drawing.Point( -1, -1 );
            }
            if ( !m_IsDragging )
            {
              return;
            }
            // draw other point
            m_DragEndPos.X = charX;
            m_DragEndPos.Y = charY;

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
        m_CurrentChar = (ushort)( m_CharsetScreen.Chars[charX + charY * m_CharsetScreen.ScreenWidth] & 0xffff );
        m_CurrentColor = (ushort)( m_CharsetScreen.Chars[charX + charY * m_CharsetScreen.ScreenWidth] >> 16 );
        panelCharacters.SelectedIndex = m_CurrentChar;
        labelInfo.Text = InfoText();
        RedrawColorChooser();
      }
    }



    private void DrawCharacter( int X, int Y )
    {
      if ( m_ReverseChars )
      {
        DrawCharImage( pictureEditor.DisplayPage, ( X - m_CharsetScreen.ScreenOffsetX ) * 8, ( Y - m_CharsetScreen.ScreenOffsetY ) * 8, 
                       (ushort)( ( m_CharsetScreen.Chars[X + Y * m_CharsetScreen.ScreenWidth] & 0xffff ) ^ 0x80 ), 
                       (ushort)( m_CharsetScreen.Chars[X + Y * m_CharsetScreen.ScreenWidth] >> 16 ) );
        return;
      }

      if ( ( m_AffectChars )
      &&   ( m_AffectColors ) )
      {
        DrawCharImage( pictureEditor.DisplayPage, ( X - m_CharsetScreen.ScreenOffsetX ) * 8, ( Y - m_CharsetScreen.ScreenOffsetY ) * 8, m_CurrentChar, m_CurrentColor );
      }
      else if ( m_AffectChars )
      {
        DrawCharImage( pictureEditor.DisplayPage, ( X - m_CharsetScreen.ScreenOffsetX ) * 8, ( Y - m_CharsetScreen.ScreenOffsetY ) * 8, m_CurrentChar, (ushort)( m_CharsetScreen.Chars[X + Y * m_CharsetScreen.ScreenWidth] >> 16 ) );
      }
      else if ( m_AffectColors )
      {
        DrawCharImage( pictureEditor.DisplayPage, ( X - m_CharsetScreen.ScreenOffsetX ) * 8, ( Y - m_CharsetScreen.ScreenOffsetY ) * 8, (ushort)( m_CharsetScreen.Chars[X + Y * m_CharsetScreen.ScreenWidth] & 0xffff ), m_CurrentColor );
      }
    }



    private void SetCharacter( int X, int Y )
    {
      SetCharacter( X, Y, m_CurrentChar, m_CurrentColor );
    }



    private void SetCharacter( int X, int Y, ushort Char, ushort Color )
    {
      if ( m_ReverseChars )
      {
        byte  origChar = (byte)( m_CharsetScreen.Chars[X + Y * m_CharsetScreen.ScreenWidth] & 0xff );

        DrawCharImage( pictureEditor.DisplayPage, 
                       ( X - m_CharsetScreen.ScreenOffsetX ) * 8, 
                       ( Y - m_CharsetScreen.ScreenOffsetY ) * 8,
                       (ushort)( origChar ^ 0x80 ), Color );
        m_CharsetScreen.Chars[X + Y * m_CharsetScreen.ScreenWidth] = (uint)( ( origChar ^ 0x80 ) | ( Color << 16 ) );
        return;
      }

      if ( ( m_AffectChars )
      &&   ( m_AffectColors ) )
      {
        DrawCharImage( pictureEditor.DisplayPage, ( X - m_CharsetScreen.ScreenOffsetX ) * 8, ( Y - m_CharsetScreen.ScreenOffsetY ) * 8, Char, Color );
        m_CharsetScreen.Chars[X + Y * m_CharsetScreen.ScreenWidth] = (uint)( Char | ( Color << 16 ) );
      }
      else if ( m_AffectChars )
      {
        DrawCharImage( pictureEditor.DisplayPage, ( X - m_CharsetScreen.ScreenOffsetX ) * 8, ( Y - m_CharsetScreen.ScreenOffsetY ) * 8, Char, (ushort)( m_CharsetScreen.Chars[X + Y * m_CharsetScreen.ScreenWidth] >> 16 ) );
        m_CharsetScreen.Chars[X + Y * m_CharsetScreen.ScreenWidth] = (uint)( Char | ( m_CharsetScreen.Chars[X + Y * m_CharsetScreen.ScreenWidth] & 0xffff0000 ) );
      }
      else if ( m_AffectColors )
      {
        DrawCharImage( pictureEditor.DisplayPage, ( X - m_CharsetScreen.ScreenOffsetX ) * 8, ( Y - m_CharsetScreen.ScreenOffsetY ) * 8, (ushort)( m_CharsetScreen.Chars[X + Y * m_CharsetScreen.ScreenWidth] & 0xffff ), Color );
        m_CharsetScreen.Chars[X + Y * m_CharsetScreen.ScreenWidth] = (uint)( ( m_CharsetScreen.Chars[X + Y * m_CharsetScreen.ScreenWidth] & 0xffff ) | ( (uint)Color << 16 ) );
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



    private void RedrawFullScreen()
    {
      int     x1 = m_CharsetScreen.ScreenOffsetX;
      int     y1 = m_CharsetScreen.ScreenOffsetY;
      int     x2 = x1 + m_CharsetScreen.ScreenWidth - 1;
      int     y2 = y1 + m_CharsetScreen.ScreenHeight - 1;

      if ( x1 < 0 )
      {
        x1 = 0;
      }
      if ( x2 >= m_CharsetScreen.ScreenWidth )
      {
        x2 = m_CharsetScreen.ScreenWidth - 1;
      }
      if ( x2 - x1 > m_CharsetScreen.ScreenWidth )
      {
        x2 = x1 + m_CharsetScreen.ScreenWidth - 1;
      }
      if ( y1 < 0 )
      {
        y1 = 0;
      }
      if ( y2 >= m_CharsetScreen.ScreenHeight )
      {
        y2 = m_CharsetScreen.ScreenHeight - 1;
      }
      if ( y2 - y1 > m_CharsetScreen.ScreenHeight )
      {
        y2 = y1 + m_CharsetScreen.ScreenHeight - 1;
      }

      for ( int i = x1; i <= x2; ++i )
      {
        for ( int j = y1; j <= y2; ++j )
        {
          if ( ( j < 0 )
          ||   ( j >= m_CharsetScreen.ScreenHeight )
          ||   ( i < 0 )
          ||   ( i >= m_CharsetScreen.ScreenWidth ) )
          {
            continue;
          }
          DrawCharImage( pictureEditor.DisplayPage,
                         ( i - x1 ) * 8,
                         ( j - y1 ) * 8,
                         (ushort)( m_CharsetScreen.Chars[i + j * m_CharsetScreen.ScreenWidth] & 0xffff ),
                         (ushort)( m_CharsetScreen.Chars[i + j * m_CharsetScreen.ScreenWidth] >> 16 ) );
        }
      }
      for ( int i = 0; i < m_CharsetScreen.ScreenWidth; ++i )
      {
        for ( int j = 0; j < m_CharsetScreen.ScreenHeight; ++j )
        {
          DrawCharImage( m_Image, i * 8, j * 8,
                         (ushort)( m_CharsetScreen.Chars[i + j * m_CharsetScreen.ScreenWidth] & 0xffff ),
                         (ushort)( m_CharsetScreen.Chars[i + j * m_CharsetScreen.ScreenWidth] >> 16 ) );
        }
      }

      pictureEditor.Invalidate();
    }



    private void comboBackground_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( m_CharsetScreen.CharSet.Colors.BackgroundColor != comboBackground.SelectedIndex )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenValuesChange( m_CharsetScreen, this ) );

        SetBackgroundColor( comboBackground.SelectedIndex );
      }
    }



    private void SetBackgroundColor( int ColorIndex )
    {
      m_CharsetScreen.CharSet.Colors.BackgroundColor = ColorIndex;
      for ( int i = 0; i < m_CharsetScreen.CharSet.TotalNumberOfCharacters; ++i )
      {
        RebuildCharImage( i );
      }
      Modified = true;
      RedrawFullScreen();
      pictureEditor.Invalidate();
      panelCharacters.Invalidate();
      charEditor.CharsetUpdated( m_CharsetScreen.CharSet );
    }



    private void comboMulticolor1_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( m_CharsetScreen.CharSet.Colors.MultiColor1 != comboMulticolor1.SelectedIndex )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenValuesChange( m_CharsetScreen, this ) );

        m_CharsetScreen.CharSet.Colors.MultiColor1 = comboMulticolor1.SelectedIndex;
        for ( int i = 0; i < m_CharsetScreen.CharSet.TotalNumberOfCharacters; ++i )
        {
          RebuildCharImage( i );
        }
        Modified = true;
        RedrawFullScreen();
        panelCharacters.Invalidate();
        charEditor.CharsetUpdated( m_CharsetScreen.CharSet );
        RedrawColorChooser();
      }
    }



    private void comboMulticolor2_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( m_CharsetScreen.CharSet.Colors.MultiColor2 != comboMulticolor2.SelectedIndex )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenValuesChange( m_CharsetScreen, this ) );

        m_CharsetScreen.CharSet.Colors.MultiColor2 = comboMulticolor2.SelectedIndex;
        for ( int i = 0; i < m_CharsetScreen.CharSet.TotalNumberOfCharacters; ++i )
        {
          RebuildCharImage( i );
        }
        Modified = true;
        RedrawFullScreen();
        panelCharacters.Invalidate();
        charEditor.CharsetUpdated( m_CharsetScreen.CharSet );
        RedrawColorChooser();
      }
    }



    public void Clear()
    {
      DocumentInfo.DocumentFilename = "";
      // TODO - Clear
    }



    public bool OpenProject( string File )
    {
      GR.Memory.ByteBuffer projectFile = GR.IO.File.ReadAllBytes( File );
      if ( projectFile == null )
      {
        return false;
      }
      if ( !m_CharsetScreen.ReadFromBuffer( projectFile ) )
      {
        return false;
      }

      ApplyPalette();

      SetScreenSize( m_CharsetScreen.ScreenWidth, m_CharsetScreen.ScreenHeight );

      UpdatePalette();

      comboBackground.SelectedIndex = m_CharsetScreen.CharSet.Colors.BackgroundColor;
      comboMulticolor1.SelectedIndex = m_CharsetScreen.CharSet.Colors.MultiColor1;
      comboMulticolor2.SelectedIndex = m_CharsetScreen.CharSet.Colors.MultiColor2;
      comboCharsetMode.SelectedIndex = (int)m_CharsetScreen.Mode;
      comboBGColor4.SelectedIndex = m_CharsetScreen.CharSet.Colors.BGColor4;
      editCharOffset.Text = m_CharsetScreen.CharOffset.ToString();

      Modified = false;
      if ( m_CharsetScreen.ExternalCharset.Length != 0 )
      {
        if ( DocumentInfo.Project != null )
        {
          ImportCharset( DocumentInfo.Project.FullPath( m_CharsetScreen.ExternalCharset ) );
        }
        else
        {
          ImportCharset( m_CharsetScreen.ExternalCharset );
        }
      }
      else
      {
        if ( panelCharacters.Items.Count > m_CharsetScreen.CharSet.TotalNumberOfCharacters )
        {
          panelCharacters.Items.RemoveRange( m_CharsetScreen.CharSet.TotalNumberOfCharacters,
                                             panelCharacters.Items.Count - m_CharsetScreen.CharSet.TotalNumberOfCharacters );
        }
        for ( int i = 0; i < m_CharsetScreen.CharSet.TotalNumberOfCharacters; ++i )
        {
          RebuildCharImage( i );
          if ( i >= panelCharacters.Items.Count )
          {
            panelCharacters.Items.Add( i.ToString(), m_CharsetScreen.CharSet.Characters[i].Tile.Image );
          }
          panelCharacters.Items[i].MemoryImage = m_CharsetScreen.CharSet.Characters[i].Tile.Image;
        }
      }
      editScreenWidth.Text = m_CharsetScreen.ScreenWidth.ToString();
      editScreenHeight.Text = m_CharsetScreen.ScreenHeight.ToString();

      AdjustScrollbars();

      screenHScroll.Value = m_CharsetScreen.ScreenOffsetX;
      screenVScroll.Value = m_CharsetScreen.ScreenOffsetY;

      for ( int i = 0; i < m_CharsetScreen.ScreenWidth; ++i )
      {
        for ( int j = 0; j < m_CharsetScreen.ScreenHeight; ++j )
        {
          DrawCharImage( m_Image, i * 8, j * 8,
                         (ushort)( m_CharsetScreen.Chars[i + j * m_CharsetScreen.ScreenWidth] & 0xffff ),
                         (ushort)( m_CharsetScreen.Chars[i + j * m_CharsetScreen.ScreenWidth] >> 16 ) );
        }
      }

      panelCharColors.Visible = Lookup.RequiresCustomColorForCharacter( Lookup.TextCharModeFromTextMode( m_CharsetScreen.Mode ) );
      charEditor.CharsetUpdated( m_CharsetScreen.CharSet );

      RedrawColorChooser();
      RedrawFullScreen();

      EnableFileWatcher();
      return true;
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
        MessageBox.Show( "Could not load charset screen project file " + DocumentInfo.FullPath + ".\r\n" + ex.Message, "Could not load file" );
        return false;
      }
      SetUnmodified();
      return true;
    }



    public override GR.Memory.ByteBuffer SaveToBuffer()
    {
      return m_CharsetScreen.SaveToBuffer();
    }



    protected override bool QueryFilename( out string Filename )
    {
      Filename = "";



      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Save Charset Screen Project as";
      saveDlg.Filter = "Charset Screen Projects|*.charscreen|All Files|*.*";
      if ( DocumentInfo.Project != null )
      {
        saveDlg.InitialDirectory = DocumentInfo.Project.Settings.BasePath;
      }
      if ( saveDlg.ShowDialog() != DialogResult.OK )
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
        DialogResult doSave = MessageBox.Show( "There are unsaved changes in your character set. Save now?", "Save changes?", MessageBoxButtons.YesNoCancel );
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



    private void checkExportToDataWrap_CheckedChanged( object sender, EventArgs e )
    {
      editWrapByteCount.Enabled = checkExportToDataWrap.Checked;
    }



    private void checkExportToDataIncludeRes_CheckedChanged( object sender, EventArgs e )
    {
      editPrefix.Enabled = checkExportToDataIncludeRes.Checked;
    }



    private bool ImportCharset( string Filename )
    {
      string extension = System.IO.Path.GetExtension( Filename ).ToUpper();

      if ( extension == ".CHARSETPROJECT" )
      {
        GR.Memory.ByteBuffer charSetProject = GR.IO.File.ReadAllBytes( Filename );
        if ( charSetProject == null )
        {
          return false;
        }

        if ( !m_CharsetScreen.CharSet.ReadFromBuffer( charSetProject ) )
        {
          return false;
        }
        CharsetChanged();

        for ( int i = 0; i < m_CharsetScreen.CharSet.TotalNumberOfCharacters; ++i )
        {
          panelCharacters.Items[i].MemoryImage = m_CharsetScreen.CharSet.Characters[i].Tile.Image;
        }
        Modified = true;
        return true;
      }
      // treat as .chr
      GR.Memory.ByteBuffer charData = GR.IO.File.ReadAllBytes( Filename );
      if ( charData == null )
      {
        return false;
      }

      int charsToImport = (int)charData.Length / 8;
      if ( charsToImport > m_CharsetScreen.CharSet.TotalNumberOfCharacters )
      {
        charsToImport = m_CharsetScreen.CharSet.TotalNumberOfCharacters;
      }
      for ( int i = 0; i < charsToImport; ++i )
      {
        for ( int j = 0; j < 8; ++j )
        {
          m_CharsetScreen.CharSet.Characters[i].Tile.Data.SetU8At( j, charData.ByteAt( i * 8 + j ) );
        }
        RebuildCharImage( i );
      }
      return true;
    }



    private void btnImportCharset_Click( object sender, EventArgs e )
    {
      OpenExternalCharset();
    }



    private void OpenExternalCharset()
    {
      string filename;

      if ( OpenFile( "Open charset or charset project", Constants.FILEFILTER_CHARSET + Constants.FILEFILTER_ALL, out filename ) )
      {
        var undo = new Undo.UndoCharscreenCharsetChange( m_CharsetScreen, this );

        if ( ImportCharset( filename ) )
        {
          DocumentInfo.UndoManager.AddUndoTask( undo );

          //CharsetChanged();

          RedrawFullScreen();
          pictureEditor.Invalidate();
          labelInfo.Text = InfoText();

          if ( ( DocumentInfo.Project == null )
          ||   ( string.IsNullOrEmpty( DocumentInfo.Project.Settings.BasePath ) ) )
          {
            m_CharsetScreen.ExternalCharset = filename;
          }
          else
          {
            m_CharsetScreen.ExternalCharset = GR.Path.RelativePathTo( filename, false, System.IO.Path.GetFullPath( DocumentInfo.Project.Settings.BasePath ), true );
          }
          m_CharsetScreen.ExternalCharset = "";
          Modified = true;
        }
      }
    }



    private void Redraw()
    {
      pictureEditor.DisplayPage.Box( 0, 0, pictureEditor.DisplayPage.Width, pictureEditor.DisplayPage.Height, 16 );
      pictureEditor.DisplayPage.DrawImage( m_Image, -m_CharsetScreen.ScreenOffsetX * 8, -m_CharsetScreen.ScreenOffsetY * 8 );

      if ( ( ( m_ToolMode == ToolMode.RECTANGLE )
      ||     ( m_ToolMode == ToolMode.FILLED_RECTANGLE ) )
      &&   ( m_IsDragging ) )
      {
        System.Drawing.Point    p1, p2;

        CalcRect( m_DragStartPos, m_DragEndPos, out p1, out p2 );

        if ( m_ToolMode == ToolMode.RECTANGLE )
        {
          for ( int x = p1.X; x <= p2.X; ++x )
          {
            DrawCharacter( x, p1.Y );
            DrawCharacter( x, p2.Y );
          }
          for ( int y = p1.Y + 1; y <= p2.Y - 1; ++y )
          {
            DrawCharacter( p1.X, y );
            DrawCharacter( p2.X, y );
          }
        }
        else
        {
          for ( int x = p1.X; x <= p2.X; ++x )
          {
            for ( int y = p1.Y; y <= p2.Y; ++y )
            {
              DrawCharacter( x, y );
            }
          }
        }
      }

      int     x1 = m_CharsetScreen.ScreenOffsetX;
      int     y1 = m_CharsetScreen.ScreenOffsetY;
      int     x2 = x1 + m_CharsetScreen.ScreenWidth - 1;
      int     y2 = y1 + m_CharsetScreen.ScreenHeight - 1;

      if ( x1 < 0 )
      {
        x1 = 0;
      }
      if ( x2 >= m_CharsetScreen.ScreenWidth )
      {
        x2 = m_CharsetScreen.ScreenWidth - 1;
      }
      if ( x2 - x1 > m_CharsWidth )
      {
        x2 = x1 + m_CharsWidth - 1;
      }
      if ( y1 < 0 )
      {
        y1 = 0;
      }
      if ( y2 >= m_CharsetScreen.ScreenHeight )
      {
        y2 = m_CharsetScreen.ScreenHeight - 1;
      }
      if ( y2 - y1 > m_CharsHeight )
      {
        y2 = y1 + m_CharsHeight - 1;
      }

      // mark errornous chars
      for ( int j = 0; j < m_CharsetScreen.ScreenHeight; ++j )
      {
        for ( int i = 0; i < m_CharsetScreen.ScreenWidth; ++i )
        {
          if ( m_ErrornousChars[i, j] )
          {
            for ( int x = 0; x < 8; ++x )
            {
              pictureEditor.DisplayPage.SetPixel( i * 8 + x - m_CharsetScreen.ScreenOffsetX * 8, j * 8 - m_CharsetScreen.ScreenOffsetY * 8, 1 );
              pictureEditor.DisplayPage.SetPixel( i * 8 - m_CharsetScreen.ScreenOffsetX * 8, j * 8 + x - m_CharsetScreen.ScreenOffsetY * 8, 1 );
            }
          }
        }
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
              DrawCharImage( pictureEditor.DisplayPage,
                 ( m_MousePos.X + i ) * 8,
                 ( m_MousePos.Y + j ) * 8,
                 (ushort)( selectionChar.second & 0xffff ),
                 (ushort)( selectionChar.second >> 16 ) );
            }
          }
        }
      }

      pictureEditor.Invalidate();
    }



    private void pictureEditor_Paint( object sender, PaintEventArgs e )
    {
      for ( int j = 0; j < m_CharsetScreen.ScreenHeight; ++j )
      {
        for ( int i = 0; i < m_CharsetScreen.ScreenWidth; ++i )
        {
          if ( m_ErrornousChars[i, j] )
          {
            e.Graphics.DrawRectangle( System.Drawing.SystemPens.ControlLight, i * 16, j * 16, 16, 16 );
          }
        }
      }
    }



    private void panelCharacters_SelectedIndexChanged( object sender, EventArgs e )
    {
      m_CurrentChar = (byte)panelCharacters.SelectedIndex;

      RedrawColorChooser();
      labelInfo.Text = InfoText();
    }



    private void RedrawColorChooser()
    {
      for ( byte i = 0; i < m_NumColorsInColorChooser; ++i )
      {
        DrawCharImage( panelCharColors.DisplayPage, i * 8, 0, m_CurrentChar, i );
      }
      panelCharColors.Invalidate();
    }



    private void pictureCharColor_MouseDown( object sender, MouseEventArgs e )
    {
      HandleMouseOnColorChooser( e.X, e.Y, e.Button );
    }



    private void pictureCharColor_MouseMove( object sender, MouseEventArgs e )
    {
      HandleMouseOnColorChooser( e.X, e.Y, e.Button );
    }



    private void HandleMouseOnColorChooser( int X, int Y, MouseButtons Buttons )
    {
      if ( ( X < 0 )
      ||   ( X >= panelCharColors.ClientSize.Width ) )
      {
        return;
      }
      if ( ( Buttons & MouseButtons.Left ) == MouseButtons.Left )
      {
        int colorIndex = (int)( ( m_NumColorsInColorChooser * X ) / panelCharColors.ClientSize.Width );
        m_CurrentColor = (byte)colorIndex;
        RedrawColorChooser();
        labelInfo.Text = InfoText();

        if ( m_OverrideCharMode )
        {
          RebuildCharPanelImages();
        }
      }
    }



    private void importCharsetToolStripMenuItem_Click( object sender, EventArgs e )
    {
      OpenExternalCharset();
    }



    private void RecalcSelectionBounds()
    {
      int     minX = m_CharsetScreen.ScreenWidth;
      int     maxX = 0;
      int     minY = m_CharsetScreen.ScreenHeight;
      int     maxY = 0;

      for ( int i = 0; i < m_CharsetScreen.ScreenWidth; ++i )
      {
        for ( int j = 0; j < m_CharsetScreen.ScreenHeight; ++j )
        {
          if ( m_SelectedChars[i, j] )
          {
            minX = Math.Min( minX, i );
            maxX = Math.Max( maxX, i );
            minY = Math.Min( minY, j );
            maxY = Math.Max( maxY, j );
          }
        }
      }
      if ( minX == m_CharsetScreen.ScreenWidth )
      {
        m_SelectionBounds = new System.Drawing.Rectangle();
        return;
      }
      m_SelectionBounds = new System.Drawing.Rectangle( minX, minY, maxX - minX + 1, maxY - minY + 1 );
    }



    private System.Drawing.Rectangle DetermineExportRectangle()
    {
      switch ( comboExportArea.SelectedIndex )
      {
        case 0:
          // all
          return new System.Drawing.Rectangle( 0, 0, m_CharsetScreen.ScreenWidth, m_CharsetScreen.ScreenHeight );
        case 1:
          // selection
          {
            if ( m_SelectionBounds.Width == 0 )
            {
              // no selection, select all
              return new System.Drawing.Rectangle( 0, 0, m_CharsetScreen.ScreenWidth, m_CharsetScreen.ScreenHeight );
            }
            return m_SelectionBounds;
          }
        case 2:
          // Area
          {
            int     minX = GR.Convert.ToI32( editExportX.Text );
            int     minY = GR.Convert.ToI32( editExportY.Text );
            int     width = GR.Convert.ToI32( editAreaWidth.Text );
            int     height = GR.Convert.ToI32( editAreaHeight.Text );

            minX = Math.Max( 0, minX );
            minY = Math.Max( 0, minY );
            if ( width < 0 )
            {
              width = 1;
            }
            if ( height < 0 )
            {
              height = 1;
            }
            if ( minX + width > m_CharsetScreen.ScreenWidth )
            {
              width = m_CharsetScreen.ScreenWidth - minX;
            }
            if ( minY + height > m_CharsetScreen.ScreenHeight )
            {
              height = m_CharsetScreen.ScreenHeight - minX;
            }
            return new System.Drawing.Rectangle( minX, minY, width, height );
          }
      }

      // should not happen
      return new System.Drawing.Rectangle( 0, 0, m_CharsetScreen.ScreenWidth, m_CharsetScreen.ScreenHeight );
    }



    private void btnExportToBasic_Click( object sender, EventArgs e )
    {
      StringBuilder sb = new StringBuilder();
      int     curColor = -1;
      bool isReverse = false;

      int   startLineNo = GR.Convert.ToI32( editExportBASICLineNo.Text );
      int   lineStep = GR.Convert.ToI32( editExportBASICLineOffset.Text );
      int   wrapByteCount = GetExportWrapCount();
      if ( wrapByteCount < 10 )
      {
        wrapByteCount = 10;
      }

      sb.Append( startLineNo );
      sb.Append( " PRINT\"" + ConstantData.PetSCIIToChar[147].CharValue + "\";\n" );
      startLineNo += lineStep;

      sb.Append( startLineNo );
      startLineNo += lineStep;
      sb.Append( " POKE53280," + m_CharsetScreen.CharSet.Colors.BackgroundColor.ToString() + ":POKE53281," + m_CharsetScreen.CharSet.Colors.BackgroundColor.ToString() + "\n" );

      System.Drawing.Rectangle    exportRect = DetermineExportRectangle();

      for ( int i = exportRect.Top; i < exportRect.Bottom; ++i )
      {
        int   startLength = sb.Length;
        sb.Append( startLineNo );
        startLineNo += lineStep;
        sb.Append( " PRINT\"" );
        for ( int x = exportRect.Left; x < exportRect.Right; ++x )
        {
          ushort newColor = (ushort)( ( ( m_CharsetScreen.Chars[i * m_CharsetScreen.ScreenWidth + x] & 0xffff0000 ) >> 16 ) & 0x0f );
          ushort newChar = (ushort)( m_CharsetScreen.Chars[i * m_CharsetScreen.ScreenWidth + x] & 0xffff );

          List<char>  charsToAppend = new List<char>();

          if ( newColor != curColor )
          {
            charsToAppend.Add( ConstantData.PetSCIIToChar[ConstantData.ColorToPetSCIIChar[(byte)newColor]].CharValue );
            curColor = newColor;
          }
          if ( newChar >= 128 )
          {
            if ( !isReverse )
            {
              isReverse = true;
              charsToAppend.Add( ConstantData.PetSCIIToChar[18].CharValue );
            }
          }
          else if ( isReverse )
          {
            isReverse = false;
            charsToAppend.Add( ConstantData.PetSCIIToChar[146].CharValue );
          }
          if ( isReverse )
          {
            if ( newChar == 128 + 34 )
            {
              // reverse apostrophe
              string    replacement = "\"CHR$(34)CHR$(20)CHR$(34)\"";

              for ( int t = 0; t < replacement.Length; ++t )
              {
                charsToAppend.Add( ConstantData.CharToC64Char[replacement[t]].CharValue );
              }
            }
            else
            {
              charsToAppend.Add( ConstantData.ScreenCodeToChar[(byte)( newChar - 128 )].CharValue );
            }
          }
          else
          {
            if ( newChar == 34 )
            {
              // a regular apostrophe
              string    replacement = "\"CHR$(34)CHR$(20)CHR$(34)\"";

              for ( int t = 0; t < replacement.Length; ++t )
              {
                charsToAppend.Add( ConstantData.CharToC64Char[replacement[t]].CharValue );
              }
            }
            else
            {
              charsToAppend.Add( ConstantData.ScreenCodeToChar[(byte)newChar].CharValue );
            }
          }

          // don't make lines too long!
          if ( sb.Length - startLength + charsToAppend.Count >= wrapByteCount - 1 )
          {
            // we need to break and start a new line
            sb.Append( "\";\n" );
            startLength = sb.Length;
            sb.Append( startLineNo );
            startLineNo += lineStep;
            sb.Append( " PRINT\"" );
          }
          foreach ( char toAppend in charsToAppend )
          {
            sb.Append( toAppend );
          }
        }
        sb.Append( "\";\n" );
      }

      Types.ComboItem comboItem = (Types.ComboItem)comboBasicFiles.SelectedItem;
      if ( comboItem.Tag == null )
      {
        // to new file
        BaseDocument document = null;
        if ( DocumentInfo.Project == null )
        {
          document = Core.MainForm.CreateNewDocument( ProjectElement.ElementType.BASIC_SOURCE, null );
        }
        else
        {
          document = Core.MainForm.CreateNewElement( ProjectElement.ElementType.BASIC_SOURCE, "BASIC Screen", DocumentInfo.Project ).Document;
        }
        if ( document.DocumentInfo.Element != null )
        {
          document.SetDocumentFilename( "New BASIC File.bas" );
          document.DocumentInfo.Element.Filename = document.DocumentInfo.DocumentFilename;
        }
        document.FillContent( sb.ToString(), false );
        document.SetModified();
        document.Save( SaveMethod.SAVE );
      }
      else
      {
        var document = (DocumentInfo)comboItem.Tag;
        if ( document.BaseDoc == null )
        {
          if ( document.Project == null )
          {
            return;
          }
          document.Project.ShowDocument( document.Element );
        }
        document.BaseDoc.InsertText( sb.ToString() );
        document.BaseDoc.SetModified();
      }
    }



    private int GetExportWrapCount()
    {
      if ( checkExportToDataWrap.Checked )
      {
        return GR.Convert.ToI32( editWrapByteCount.Text );
      }
      return 80;
    }



    private void btnExportToData_Click( object sender, EventArgs e )
    {
      // prepare data
      GR.Memory.ByteBuffer screenCharData;
      GR.Memory.ByteBuffer screenColorData;
      GR.Memory.ByteBuffer charsetData;

      var exportRect = DetermineExportRectangle();

      StringBuilder   sb = new StringBuilder();

      if ( checkExportASMAsPetSCII.Checked )
      {
        // pet export only exports chars, no color changes
        bool            isReverse = false;

        sb.Append( ";size " );
        sb.Append( exportRect.Width );
        sb.Append( "," );
        sb.Append( exportRect.Height );
        sb.AppendLine();

        for ( int i = exportRect.Top; i < exportRect.Bottom; ++i )
        {
          sb.Append( "!pet \"" );
          for ( int x = exportRect.Left; x < exportRect.Right; ++x )
          {
            byte newChar = (byte)( m_CharsetScreen.Chars[i * m_CharsetScreen.ScreenWidth + x] & 0xffff );
            byte charToAdd = newChar;

            if ( newChar >= 128 )
            {
              isReverse = true;
            }
            else if ( isReverse )
            {
              isReverse = false;
            }
            if ( isReverse )
            {
              charToAdd -= 128;
            }
            if ( ( ConstantData.ScreenCodeToChar[newChar].HasPetSCII )
            &&   ( ConstantData.ScreenCodeToChar[charToAdd].CharValue < 256 ) )
            {
              sb.Append( ConstantData.ScreenCodeToChar[charToAdd].CharValue );
            }
            else
            {
              sb.Append( "\", $" );
              sb.Append( newChar.ToString( "X2" ) );
              sb.Append( ", \"" );
            }
          }
          sb.AppendLine( "\"" );
        }
        editDataExport.Text = sb.ToString();
        return;
      }

      m_CharsetScreen.ExportToBuffer( out screenCharData, out screenColorData, out charsetData, exportRect.Left, exportRect.Top, exportRect.Width, exportRect.Height, ( comboExportOrientation.SelectedIndex == 0 ) );

      string screenData = Util.ToASMData( screenCharData, checkExportToDataWrap.Checked, GR.Convert.ToI32( editWrapByteCount.Text ), checkExportToDataIncludeRes.Checked ? editPrefix.Text : "", checkExportHex.Checked );
      string colorData = Util.ToASMData( screenColorData, checkExportToDataWrap.Checked, GR.Convert.ToI32( editWrapByteCount.Text ), checkExportToDataIncludeRes.Checked ? editPrefix.Text : "", checkExportHex.Checked );

      sb.Append( ";size " );
      sb.Append( exportRect.Width );
      sb.Append( "," );
      sb.Append( exportRect.Height );
      sb.AppendLine();

      switch ( comboExportData.SelectedIndex )
      {
        case 0:
          editDataExport.Text = sb + ";screen char data" + Environment.NewLine + screenData + Environment.NewLine + ";screen color data" + Environment.NewLine + colorData;
          break;
        case 1:
          editDataExport.Text = sb + ";screen char data" + Environment.NewLine + screenData + Environment.NewLine;
          break;
        case 2:
          editDataExport.Text = sb + ";screen color data" + Environment.NewLine + colorData;
          break;
        case 3:
          editDataExport.Text = sb + ";screen color data" + Environment.NewLine + colorData + Environment.NewLine + ";screen char data" + Environment.NewLine + screenData;
          break;
      }
    }



    private void btnExportToFile_Click( object sender, EventArgs e )
    {
      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Save data as";
      saveDlg.Filter = "Binary Data|*.bin|All Files|*.*";
      if ( DocumentInfo.Project != null )
      {
        saveDlg.InitialDirectory = DocumentInfo.Project.Settings.BasePath;
      }
      if ( saveDlg.ShowDialog() != DialogResult.OK )
      {
        return;
      }

      // prepare data
      GetExportData( out ByteBuffer screenCharData, out ByteBuffer screenColorData, out System.Drawing.Rectangle exportRect );

      GR.Memory.ByteBuffer finalData = null;

      switch ( comboExportData.SelectedIndex )
      {
        case 0:
          finalData = screenCharData + screenColorData;
          break;
        case 1:
          finalData = screenCharData;
          break;
        case 2:
          finalData = screenColorData;
          break;
        case 3:
          finalData = screenColorData + screenCharData;
          break;
      }
      if ( finalData != null )
      {
        GR.IO.File.WriteAllBytes( saveDlg.FileName, finalData );
      }
    }



    private void btnImportFromFile_Click( object sender, EventArgs e )
    {
      string filename;

      if ( OpenFile( "Open Charpad project, Marq's PETSCII editor files or binary data", Constants.FILEFILTER_CHARSET_CHARPAD + Constants.FILEFILTER_MARQS_PETSCII + Constants.FILEFILTER_ALL, out filename ) )
      {
        if ( System.IO.Path.GetExtension( filename ).ToUpper() == ".CTM" )
        {
          // a charpad project file
          GR.Memory.ByteBuffer projectFile = GR.IO.File.ReadAllBytes( filename );

          Formats.CharpadProject    cpProject = new C64Studio.Formats.CharpadProject();
          if ( !cpProject.LoadFromFile( projectFile ) )
          {
            return;
          }

          m_CharsetScreen.CharSet.Colors.BackgroundColor = cpProject.BackgroundColor;
          m_CharsetScreen.CharSet.Colors.MultiColor1 = cpProject.MultiColor1;
          m_CharsetScreen.CharSet.Colors.MultiColor2 = cpProject.MultiColor2;
          m_CharsetScreen.CharSet.Colors.BGColor4 = cpProject.BackgroundColor4;

          int maxChars = cpProject.NumChars;
          if ( maxChars > m_CharsetScreen.CharSet.TotalNumberOfCharacters )
          {
            maxChars = m_CharsetScreen.CharSet.TotalNumberOfCharacters;
          }

          m_CharsetScreen.CharSet.ExportNumCharacters = maxChars;
          for ( int charIndex = 0; charIndex < m_CharsetScreen.CharSet.ExportNumCharacters; ++charIndex )
          {
            m_CharsetScreen.CharSet.Characters[charIndex].Tile.Data = cpProject.Characters[charIndex].Data;
            m_CharsetScreen.CharSet.Characters[charIndex].Tile.CustomColor = cpProject.Characters[charIndex].Color;

            RebuildCharImage( charIndex );
          }

          // import tiles
          var mapProject = new MapProject();
          mapProject.MultiColor1 = cpProject.MultiColor1;
          mapProject.MultiColor2 = cpProject.MultiColor2;

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
            mapProject.Tiles.Add( tile );
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
          mapProject.Maps.Add( map );

          comboBackground.SelectedIndex = mapProject.Charset.Colors.BackgroundColor;
          comboMulticolor1.SelectedIndex = mapProject.Charset.Colors.MultiColor1;
          comboMulticolor2.SelectedIndex = mapProject.Charset.Colors.MultiColor2;
          comboBGColor4.SelectedIndex = mapProject.Charset.Colors.BGColor4;
          comboCharsetMode.SelectedIndex = (int)cpProject.DisplayModeFile;
            //( cpProject.MultiColor ? TextMode.COMMODORE_40_X_25_MULTICOLOR : TextMode.COMMODORE_40_X_25_HIRES );

          GR.Memory.ByteBuffer      charData = new GR.Memory.ByteBuffer( (uint)( map.Tiles.Width * map.TileSpacingX * map.Tiles.Height * map.TileSpacingY ) );
          GR.Memory.ByteBuffer      colorData = new GR.Memory.ByteBuffer( (uint)( map.Tiles.Width * map.TileSpacingX * map.Tiles.Height * map.TileSpacingY ) );

          for ( int y = 0; y < map.Tiles.Height; ++y )
          {
            for ( int x = 0; x < map.Tiles.Width; ++x )
            {
              int tileIndex = map.Tiles[x, y];
              if ( tileIndex < mapProject.Tiles.Count )
              {
                // a real tile
                var tile = mapProject.Tiles[tileIndex];

                for ( int j = 0; j < tile.Chars.Height; ++j )
                {
                  for ( int i = 0; i < tile.Chars.Width; ++i )
                  {
                    charData.SetU8At( x * map.TileSpacingX + i + ( y * map.TileSpacingY + j ) * ( map.Tiles.Width * map.TileSpacingX ), tile.Chars[i, j].Character );
                    colorData.SetU8At( x * map.TileSpacingX + i + ( y * map.TileSpacingY + j ) * ( map.Tiles.Width * map.TileSpacingX ), tile.Chars[i, j].Color );
                  }
                }
              }
            }
          }

          if ( cpProject.MapColorData != null )
          {
            // this charpad project has alternative color data
            for ( int i = 0; i < cpProject.MapHeight; ++i )
            {
              for ( int j = 0; j < cpProject.MapWidth; ++j )
              {
                colorData.SetU8At( j + i * cpProject.MapWidth, cpProject.MapColorData.ByteAt( j + i * cpProject.MapWidth ) );
              }
            }
          }

          ImportFromData( map.TileSpacingX * map.Tiles.Width,
                          map.TileSpacingY * map.Tiles.Height,
                          charData, colorData, m_CharsetScreen.CharSet );
        }
        else if ( System.IO.Path.GetExtension( filename ).ToUpper() == ".C" )
        {
          string cData = GR.IO.File.ReadAllText( filename );
          if ( !string.IsNullOrEmpty( cData ) )
          {
            int     dataStart = cData.IndexOf( '{' );
            if ( dataStart == -1 )
            {
              return;
            }
            int     dataEnd = cData.IndexOf( '}', dataStart );
            if ( dataEnd == -1 )
            {
              return;
            }
            string  actualData = cData.Substring( dataStart + 1, dataEnd - dataStart - 2 );

            var screenData = new ByteBuffer();

            var dataLines = actualData.Split( '\n' );
            for ( int i = 0; i < dataLines.Length; ++i )
            {
              var dataLine = dataLines[i].Trim();
              if ( dataLine.StartsWith( "//" ) )
              {
                continue;
              }
              int     pos = 0;
              int     commaPos = -1;

              while ( pos < dataLine.Length )
              {
                commaPos = dataLine.IndexOf( ',', pos );
                if ( commaPos == -1 )
                {
                  // end of line
                  byte    byteValue = GR.Convert.ToU8( dataLine.Substring( pos ) );

                  screenData.AppendU8( byteValue );
                  break;
                }
                else
                {
                  byte    byteValue = GR.Convert.ToU8( dataLine.Substring( pos, commaPos - pos ) );

                  screenData.AppendU8( byteValue );
                  pos = commaPos + 1;
                }
              }

              // border and BG first
              m_CharsetScreen.CharSet.Colors.BackgroundColor = screenData.ByteAt( 1 );
              screenData = screenData.SubBuffer( 2 );

              ImportFromData( screenData );
            }
          }
        }
        else
        {
          GR.Memory.ByteBuffer data = GR.IO.File.ReadAllBytes( filename );

          ImportFromData( data );
        }
      }
    }



    private void ImportFromData( ByteBuffer Data )
    {
      if ( Data.Length == 1000 )
      {
        SetScreenSize( 40, 25 );
      }

      if ( Data.Length >= m_CharsetScreen.ScreenWidth * m_CharsetScreen.ScreenHeight )
      {
        // chars first
        for ( int j = 0; j < m_CharsetScreen.ScreenHeight; ++j )
        {
          for ( int i = 0; i < m_CharsetScreen.ScreenWidth; ++i )
          {
            m_CharsetScreen.Chars[i + j * m_CharsetScreen.ScreenWidth] = (uint)( Data.ByteAt( i + j * m_CharsetScreen.ScreenWidth ) | ( m_CharsetScreen.Chars[i + j * m_CharsetScreen.ScreenWidth] & 0xff0000 ) );
          }
        }
      }
      if ( Data.Length >= 2 * m_CharsetScreen.ScreenWidth * m_CharsetScreen.ScreenHeight )
      {
        // colors
        for ( int j = 0; j < m_CharsetScreen.ScreenHeight; ++j )
        {
          for ( int i = 0; i < m_CharsetScreen.ScreenWidth; ++i )
          {
            ushort colorValue = Data.ByteAt( m_CharsetScreen.ScreenWidth * m_CharsetScreen.ScreenHeight + i + j * m_CharsetScreen.ScreenWidth );
            colorValue &= 0x4f;

            if ( ( m_CharsetScreen.Mode == TextMode.MEGA65_80_X_25_HIRES )
            ||   ( m_CharsetScreen.Mode == TextMode.MEGA65_40_X_25_HIRES ) )
            {
              // colors >= 16 and < 32 need to be shifted up
              if ( ( colorValue & 0x40 ) == 0x40 )
              {
                colorValue -= 64 - 16;
              }
            }
            else
            {
              colorValue &= 0x0f;
            }

            m_CharsetScreen.Chars[i + j * m_CharsetScreen.ScreenWidth] = (uint)( ( ( (uint)colorValue ) << 16 ) | ( m_CharsetScreen.Chars[i + j * m_CharsetScreen.ScreenWidth] & 0xffff ) );
          }
        }
      }
      Modified = true;
      RedrawFullScreen();
    }



    public void SetScreenSize( int Width, int Height )
    {
      m_ErrornousChars = new bool[Width, Height];
      m_SelectedChars = new bool[Width, Height];
      m_ReverseCache = new bool[Width, Height];

      m_CharsetScreen.SetScreenSize( Width, Height );
      m_Image.Create( Width * 8, Height * 8, System.Drawing.Imaging.PixelFormat.Format32bppRgb );
      PaletteManager.ApplyPalette( m_Image );

      m_TextEntryCachedLine.Clear();
      m_TextEntryEnteredText.Clear();
      m_TextEntryStartedInLine = -1;

      editScreenWidth.Text  = Width.ToString();
      editScreenHeight.Text = Height.ToString();

      AdjustScrollbars();
      RedrawFullScreen();
    }



    private void AdjustScrollbars()
    {
      screenHScroll.Minimum = 0;
      screenHScroll.SmallChange = 1;
      screenHScroll.LargeChange = 1;
      screenVScroll.SmallChange = 1;
      screenVScroll.LargeChange = 1;

      if ( m_CharsetScreen.ScreenWidth <= m_CharsWidth )
      {
        screenHScroll.Maximum = 0;
        screenHScroll.Enabled = false;
        m_CharsetScreen.ScreenOffsetX = 0;
      }
      else
      {
        screenHScroll.Maximum = m_CharsetScreen.ScreenWidth - m_CharsWidth;
        screenHScroll.Enabled = true;
      }
      if ( m_CharsetScreen.ScreenOffsetX > screenHScroll.Maximum )
      {
        m_CharsetScreen.ScreenOffsetX = screenHScroll.Maximum;
      }

      screenVScroll.Minimum = 0;
      if ( m_CharsetScreen.ScreenHeight <= m_CharsHeight )
      {
        screenVScroll.Maximum = 0;
        screenVScroll.Enabled = false;
        m_CharsetScreen.ScreenOffsetY = 0;
      }
      else
      {
        screenVScroll.Maximum = m_CharsetScreen.ScreenHeight - m_CharsHeight;
        screenVScroll.Enabled = true;
      }
      if ( m_CharsetScreen.ScreenOffsetY > screenVScroll.Maximum )
      {
        m_CharsetScreen.ScreenOffsetY = screenVScroll.Maximum;
      }
    }



    private void screenHScroll_Scroll( object sender, ScrollEventArgs e )
    {
      if ( m_CharsetScreen.ScreenOffsetX != e.NewValue )
      {
        m_CharsetScreen.ScreenOffsetX = e.NewValue;
        Redraw();
      }
    }



    private void screenVScroll_Scroll( object sender, ScrollEventArgs e )
    {
      if ( m_CharsetScreen.ScreenOffsetY != e.NewValue )
      {
        m_CharsetScreen.ScreenOffsetY = e.NewValue;
        Redraw();
      }
    }



    private void editScreenWidth_TextChanged( object sender, EventArgs e )
    {
      int     newWidth = GR.Convert.ToI32( editScreenWidth.Text );
      int     newHeight = GR.Convert.ToI32( editScreenHeight.Text );

      if ( ( newWidth >= 1 )
      && ( newWidth <= 1000 )
      && ( newHeight >= 1 )
      && ( newHeight <= 1000 ) )
      {
        btnApplyScreenSize.Enabled = true;
      }
      else
      {
        btnApplyScreenSize.Enabled = false;
      }
    }



    private void editScreenHeight_TextChanged( object sender, EventArgs e )
    {
      int     newWidth = GR.Convert.ToI32( editScreenWidth.Text );
      int     newHeight = GR.Convert.ToI32( editScreenHeight.Text );

      if ( ( newWidth >= 1 )
      && ( newWidth <= 1000 )
      && ( newHeight >= 1 )
      && ( newHeight <= 1000 ) )
      {
        btnApplyScreenSize.Enabled = true;
      }
      else
      {
        btnApplyScreenSize.Enabled = false;
      }
    }



    private void btnApplyScreenSize_Click( object sender, EventArgs e )
    {
      int     newWidth = GR.Convert.ToI32( editScreenWidth.Text );
      int     newHeight = GR.Convert.ToI32( editScreenHeight.Text );

      if ( ( ( newWidth != m_CharsetScreen.ScreenWidth )
      ||     ( newHeight !=  m_CharsetScreen.ScreenHeight ) )
      &&   ( newWidth > 0 )
      &&   ( newWidth <= 65535 )
      &&   ( newHeight > 0 )
      &&   ( newHeight <= 65535 ) )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenSizeChange( m_CharsetScreen, this, m_CharsetScreen.ScreenWidth, m_CharsetScreen.ScreenHeight ) );

        SetScreenSize( newWidth, newHeight );
        SetModified();
      }
    }



    public void InjectProjects( Formats.CharsetScreenProject CharScreen, Formats.CharsetProject CharSet )
    {
      m_CharsetScreen = CharScreen;
      m_CharsetScreen.CharSet = CharSet;

      comboBackground.SelectedIndex = m_CharsetScreen.CharSet.Colors.BackgroundColor;
      comboMulticolor1.SelectedIndex = m_CharsetScreen.CharSet.Colors.MultiColor1;
      comboMulticolor2.SelectedIndex = m_CharsetScreen.CharSet.Colors.MultiColor2;
      comboCharsetMode.SelectedIndex = (int)m_CharsetScreen.Mode;
      comboBGColor4.SelectedIndex = m_CharsetScreen.CharSet.Colors.BGColor4;

      OnCharsetScreenModeChanged();

      for ( int i = 0; i < m_CharsetScreen.CharSet.ExportNumCharacters; ++i )
      {
        RebuildCharImage( i );

        panelCharacters.Items[i].MemoryImage = m_CharsetScreen.CharSet.Characters[i].Tile.Image;
      }
      charEditor.CharsetUpdated( m_CharsetScreen.CharSet );

      Modified = false;
      editScreenWidth.Text = m_CharsetScreen.ScreenWidth.ToString();
      editScreenHeight.Text = m_CharsetScreen.ScreenHeight.ToString();

      SetScreenSize( CharScreen.ScreenWidth, CharScreen.ScreenHeight );

      AdjustScrollbars();

      for ( int i = 0; i < CharScreen.ScreenWidth; ++i )
      {
        for ( int j = 0; j < CharScreen.ScreenHeight; ++j )
        {
          DrawCharImage( m_Image, i * 8, j * 8,
                         (ushort)( m_CharsetScreen.Chars[i + j * m_CharsetScreen.ScreenWidth] & 0xffff ),
                         (ushort)( m_CharsetScreen.Chars[i + j * m_CharsetScreen.ScreenWidth] >> 16 ) );
        }
      }
      pictureEditor.Invalidate();

      RedrawColorChooser();
    }



    private void btnToolEdit_CheckedChanged( object sender, EventArgs e )
    {
      HideSelection();
      HideTextCursor();
      RemoveFloatingSelection();
      m_ToolMode = ToolMode.SINGLE_CHAR;
      OnToolModeChanged();
    }



    private void HideTextCursor()
    {
      m_SelectedChar.X = -1;
      m_SelectedChar.Y = -1;
      Redraw();
    }



    private void btnToolRect_CheckedChanged( object sender, EventArgs e )
    {
      HideSelection();
      HideTextCursor();
      RemoveFloatingSelection();
      m_ToolMode = ToolMode.RECTANGLE;
    }



    private void btnToolFill_CheckedChanged( object sender, EventArgs e )
    {
      HideSelection();
      HideTextCursor();
      RemoveFloatingSelection();
      m_ToolMode = ToolMode.FILL;
      OnToolModeChanged();
    }



    private void OnToolModeChanged()
    {
      btnCopy.Enabled = ( m_ToolMode == ToolMode.SELECT );
      btnPaste.Enabled = ( m_ToolMode == ToolMode.SELECT );
    }



    private void btnToolSelect_CheckedChanged( object sender, EventArgs e )
    {
      HideTextCursor();
      m_ToolMode = ToolMode.SELECT;
      OnToolModeChanged();
    }



    private void btnToolQuad_CheckedChanged( object sender, EventArgs e )
    {
      HideSelection();
      HideTextCursor();
      RemoveFloatingSelection();
      m_ToolMode = ToolMode.FILLED_RECTANGLE;
      OnToolModeChanged();
    }



    private void btnToolText_CheckedChanged( object sender, EventArgs e )
    {
      HideSelection();
      RemoveFloatingSelection();
      m_ToolMode = ToolMode.TEXT;
      m_TextEntryStartedInLine = -1;
      OnToolModeChanged();
    }



    private void HideSelection()
    {
      for ( int i = 0; i < m_CharsetScreen.ScreenWidth; ++i )
      {
        for ( int j = 0; j < m_CharsetScreen.ScreenHeight; ++j )
        {
          m_SelectedChars[i, j] = false;
        }
      }
      m_SelectionBounds = new System.Drawing.Rectangle();
      Redraw();
    }



    private void CopyToClipboard()
    {
      // not only rectangular pieces
      int     x1 = m_CharsetScreen.ScreenWidth;
      int     x2 = 0;
      int     y1 = m_CharsetScreen.ScreenHeight;
      int     y2 = 0;


      for ( int i = 0; i < m_CharsetScreen.ScreenWidth; ++i )
      {
        for ( int j = 0; j < m_CharsetScreen.ScreenHeight; ++j )
        {
          if ( m_SelectedChars[i, j] )
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
      if ( x1 == m_CharsetScreen.ScreenWidth )
      {
        // no selection
        return;
      }

      GR.Memory.ByteBuffer dataSelection = new GR.Memory.ByteBuffer();

      dataSelection.Reserve( ( y2 - y1 + 1 ) * ( x2 - x1 + 1 ) + 8 );
      dataSelection.AppendI32( x2 - x1 + 1 );
      dataSelection.AppendI32( y2 - y1 + 1 );

      for ( int y = 0; y < y2 - y1 + 1; ++y )
      {
        for ( int x = 0; x < x2 - x1 + 1; ++x )
        {
          if ( m_SelectedChars[x1 + x, y1 + y] )
          {
            dataSelection.AppendU8( 1 );
            dataSelection.AppendU32( m_CharsetScreen.Chars[( y1 + y ) * m_CharsetScreen.ScreenWidth + x1 + x] );
          }
          else
          {
            dataSelection.AppendU8( 0 );
          }
        }
      }

      DataObject dataObj = new DataObject();

      dataObj.SetData( "C64Studio.CharacterScreenSelection", false, dataSelection.MemoryStream() );

      Core.Imaging.ImageToClipboardData( pictureEditor.DisplayPage, x1 * 8, y1 * 8, ( x2 - x1 + 1 ) * 8, ( y2 - y1 + 1 ) * 8, dataObj );

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
        MessageBox.Show( "The clipboard is empty" );
        return;
      }
      if ( dataObj.GetDataPresent( "C64Studio.CharacterScreenSelection" ) )
      {
        System.IO.MemoryStream ms = (System.IO.MemoryStream)dataObj.GetData( "C64Studio.CharacterScreenSelection" );

        GR.Memory.ByteBuffer data = new GR.Memory.ByteBuffer( (uint)ms.Length );

        ms.Read( data.Data(), 0, (int)ms.Length );

        GR.IO.MemoryReader memIn = data.MemoryReader();

        int   selectionWidth  = memIn.ReadInt32();
        int   selectionHeight = memIn.ReadInt32();

        m_FloatingSelection = new List<GR.Generic.Tupel<bool, uint>>();
        m_FloatingSelectionSize = new System.Drawing.Size( selectionWidth, selectionHeight );

        for ( int y = 0; y < selectionHeight; ++y )
        {
          for ( int x = 0; x < selectionWidth; ++x )
          {
            bool  isCharSet = ( memIn.ReadUInt8() != 0 );
            if ( isCharSet )
            {
              m_FloatingSelection.Add( new GR.Generic.Tupel<bool, uint>( true, memIn.ReadUInt32() ) );
            }
            else
            {
              m_FloatingSelection.Add( new GR.Generic.Tupel<bool, uint>( false, 0 ) );
            }
          }
        }
        m_FloatingSelectionPos = m_MousePos;
        Redraw();
        pictureEditor.Invalidate();
        return;
      }
    }



    private void pictureEditor_PreviewKeyDown( object sender, PreviewKeyDownEventArgs e )
    {
      if ( e.KeyCode == Keys.Escape )
      {
        if ( m_FloatingSelection != null )
        {
          RemoveFloatingSelection();

          if ( m_LastDragEndPos.X != -1 )
          {
            m_LastDragEndPos.X = -1;
            m_IsDragging = false;
            Redraw();
            return;
          }
        }
      }

      if ( m_ToolMode == ToolMode.TEXT )
      {
        System.Windows.Forms.Keys bareKey = e.KeyData & ~( Keys.Control | Keys.Shift | Keys.ShiftKey | Keys.Alt );
        bareKey = e.KeyData;

        bool    controlPushed = false;
        bool    commodorePushed = false;
        bool    shiftPushed = false;
        if ( ( bareKey & Keys.Shift ) == Keys.Shift )
        {
          bareKey &= ~Keys.Shift;
          shiftPushed = true;
        }
        if ( ( bareKey & Keys.Control ) == Keys.Control )
        {
          bareKey &= ~Keys.Control;
          commodorePushed = true;
        }
        if ( GR.Win32.KeyboardInfo.GetKeyState( Keys.Tab ).IsPressed )
        {
          controlPushed = true;
        }
        if ( Core.Settings.BASICKeyMap.KeymapEntryExists( bareKey ) )
        {
          //Debug.Log( "KeyData " + bareKey );

          var key = Core.Settings.BASICKeyMap.GetKeymapEntry( bareKey );

          if ( !ConstantData.PhysicalKeyInfo.ContainsKey( key.KeyboardKey ) )
          {
            Debug.Log( "No physical key info for " + key.KeyboardKey );
          }
          var physKey = ConstantData.PhysicalKeyInfo[key.KeyboardKey];

          C64Character    c64Key = physKey.Normal;
          if ( shiftPushed )
          {
            c64Key = physKey.WithShift;
            if ( c64Key == null )
            {
              c64Key = physKey.Normal;
            }
          }
          if ( controlPushed )
          {
            c64Key = physKey.WithControl;
            if ( c64Key == null )
            {
              c64Key = physKey.Normal;
            }
          }
          if ( commodorePushed )
          {
            c64Key = physKey.WithCommodore;
            if ( c64Key == null )
            {
              c64Key = physKey.Normal;
            }
          }

          if ( c64Key != null )
          {
            byte    charIndex = c64Key.ScreenCodeValue;
            int     charX = m_SelectedChar.X;
            int     charY = m_SelectedChar.Y;

            if ( m_TextEntryStartedInLine == -1 )
            {
              m_TextEntryStartedInLine = charY;
              m_TextEntryEnteredText.Clear();
              CacheScreenLine( m_TextEntryStartedInLine );
            }

            if ( m_AutoCenterText )
            {
              DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenCharChange( m_CharsetScreen, this, 0, charY, m_CharsetScreen.ScreenWidth, 1 ) );

              // restore old line
              for ( int i = 0; i < m_TextEntryCachedLine.Count; ++i )
              {
                ushort origChar = (ushort)( m_TextEntryCachedLine[i] & 0xffff );
                ushort origColor = (ushort)( m_TextEntryCachedLine[i] >> 16 );
                SetCharacter( i, charY, origChar, origColor );
              }
              pictureEditor.DisplayPage.DrawTo( m_Image,
                                                0, m_SelectedChar.Y * 8,
                                                ( 0 - m_CharsetScreen.ScreenOffsetY ) * 8, ( m_SelectedChar.Y - m_CharsetScreen.ScreenOffsetY ) * 8,
                                                m_TextEntryCachedLine.Count * 8, 8 );
            }

            if ( bareKey == Keys.Back )
            {
              if ( m_AutoCenterText )
              {
                if ( m_TextEntryEnteredText.Count > 0 )
                {
                  m_TextEntryEnteredText.RemoveAt( m_TextEntryEnteredText.Count - 1 );
                }
              }
              else
              {
                // blank out char to the left
                if ( charX > 0 )
                {
                  --m_SelectedChar.X;
                  // blank with space
                  --charX;
                }
                DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenCharChange( m_CharsetScreen, this, charX, charY, 1, 1 ) );
                charIndex = 32;
              }
            }
            else if ( m_AutoCenterText )
            {
              if ( m_TextEntryEnteredText.Count >= m_CharsWidth )
              {
                ++m_SelectedChar.Y;
                if ( m_SelectedChar.Y >= m_CharsHeight )
                {
                  m_SelectedChar.Y = 0;
                }
                m_TextEntryStartedInLine = m_SelectedChar.Y;
                m_TextEntryEnteredText.Clear();
                CacheScreenLine( m_TextEntryStartedInLine );
              }
              m_TextEntryEnteredText.Add( (uint)( charIndex | ( m_CurrentColor << 16 ) ) );
            }
            else
            {
              DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenCharChange( m_CharsetScreen, this, charX, charY, 1, 1 ) );
              if ( m_SelectedChar.X >= m_CharsWidth - 1 )
              {
                m_SelectedChar.X = 0;
                ++m_SelectedChar.Y;
                if ( m_SelectedChar.Y >= m_CharsHeight - 1 )
                {
                  m_SelectedChar.Y = 0;
                }
                m_TextEntryStartedInLine = m_SelectedChar.Y;
                m_TextEntryEnteredText.Clear();
                CacheScreenLine( m_TextEntryStartedInLine );
              }
              else
              {
                ++m_SelectedChar.X;
              }
            }

            if ( m_AutoCenterText )
            {
              int     newX = ( m_CharsetScreen.ScreenWidth - m_TextEntryEnteredText.Count ) / 2;
              for ( int i = 0; i < m_TextEntryEnteredText.Count; ++i )
              {
                ushort  origChar = (ushort)( m_TextEntryEnteredText[i] & 0xffff );
                ushort  origColor = (ushort)( m_TextEntryEnteredText[i] >> 16 );
                SetCharacter( newX + i, m_SelectedChar.Y, origChar, origColor );
              }
              pictureEditor.DisplayPage.DrawTo( m_Image,
                                                newX * 8, m_SelectedChar.Y * 8,
                                                ( newX - m_CharsetScreen.ScreenOffsetY ) * 8, ( m_SelectedChar.Y - m_CharsetScreen.ScreenOffsetY ) * 8,
                                                m_TextEntryCachedLine.Count * 8, 8 );
              pictureEditor.Invalidate( new System.Drawing.Rectangle( charX * 8, charY * 8, 8, 8 ) );
              pictureEditor.Invalidate( new System.Drawing.Rectangle( 0, m_SelectedChar.Y * 8, m_CharsetScreen.ScreenWidth * 8, 8 ) );
            }
            else
            {
              SetCharacter( charX, charY, charIndex, m_CurrentColor );
              pictureEditor.DisplayPage.DrawTo( m_Image,
                                                charX * 8, charY * 8,
                                                ( charX - m_CharsetScreen.ScreenOffsetX ) * 8, ( charY - m_CharsetScreen.ScreenOffsetY ) * 8,
                                                8, 8 );
              pictureEditor.Invalidate( new System.Drawing.Rectangle( charX * 8, charY * 8, 8, 8 ) );
              pictureEditor.Invalidate( new System.Drawing.Rectangle( m_SelectedChar.X * 8, m_SelectedChar.Y * 8, 8, 8 ) );
            }
            Redraw();
            Modified = true;
          }
        }
      }

      if ( m_ToolMode == ToolMode.SELECT )
      {
        if ( ( e.Modifiers == Keys.Control )
        &&   ( e.KeyCode == Keys.C ) )
        {
          CopyToClipboard();
        }
      }

      if ( ( m_ToolMode == ToolMode.RECTANGLE )
      ||   ( m_ToolMode == ToolMode.FILLED_RECTANGLE ) )
      {
        if ( e.KeyCode == Keys.Escape )
        {
          if ( m_IsDragging )
          {
            m_IsDragging = false;
            Redraw();
          }
        }
      }

      if ( ( e.Modifiers == Keys.Control )
      &&   ( e.KeyCode == Keys.V ) )
      {
        PasteFromClipboard();
      }
    }



    private void RemoveFloatingSelection()
    {
      if ( m_FloatingSelection != null )
      {
        m_FloatingSelection = null;
        Redraw();
      }
    }



    private void CacheScreenLine( int LineIndex )
    {
      m_TextEntryCachedLine.Clear();
      for ( int i = 0; i < m_CharsetScreen.ScreenWidth; ++i )
      {
        m_TextEntryCachedLine.Add( m_CharsetScreen.Chars[i + LineIndex * m_CharsetScreen.ScreenWidth] );
      }
    }



    public void UpdateArea( int X, int Y, int Width, int Height )
    {
      for ( int x = X; x < X + Width; ++x )
      {
        for ( int y = Y; y < Y + Height; ++y )
        {
          DrawCharImage( pictureEditor.DisplayPage,
                         ( x - m_CharsetScreen.ScreenOffsetX ) * 8,
                         ( y - m_CharsetScreen.ScreenOffsetY ) * 8,
                         (ushort)( m_CharsetScreen.Chars[x + y * m_CharsetScreen.ScreenWidth] & 0xffff ),
                         (ushort)( m_CharsetScreen.Chars[x + y * m_CharsetScreen.ScreenWidth] >> 16 ) );

          DrawCharImage( m_Image,
                         x * 8,
                         y * 8,
                         (ushort)( m_CharsetScreen.Chars[x + y * m_CharsetScreen.ScreenWidth] & 0xffff ),
                         (ushort)( m_CharsetScreen.Chars[x + y * m_CharsetScreen.ScreenWidth] >> 16 ) );
        }
      }
      pictureEditor.Invalidate( new System.Drawing.Rectangle( ( X - m_CharsetScreen.ScreenOffsetX ) * 8, 
                                                              ( Y - m_CharsetScreen.ScreenOffsetY ) * 8, Width * 8, Height * 8 ) );
    }



    public void ValuesChanged()
    {
      comboBackground.SelectedIndex = m_CharsetScreen.CharSet.Colors.BackgroundColor;
      comboMulticolor1.SelectedIndex = m_CharsetScreen.CharSet.Colors.MultiColor1;
      comboMulticolor2.SelectedIndex = m_CharsetScreen.CharSet.Colors.MultiColor2;
      comboCharsetMode.SelectedIndex = (int)m_CharsetScreen.Mode;
      comboBGColor4.SelectedIndex = m_CharsetScreen.CharSet.Colors.BGColor4;

      editCharOffset.Text = m_CharsetScreen.CharOffset.ToString();

      for ( int i = 0; i < m_CharsetScreen.CharSet.TotalNumberOfCharacters; ++i )
      {
        RebuildCharImage( i );
      }
      Modified = true;
      RedrawFullScreen();
      panelCharacters.Invalidate();
      charEditor.CharsetUpdated( m_CharsetScreen.CharSet );
      RedrawColorChooser();
    }



    private void comboExportArea_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( comboExportArea.SelectedIndex != 2 )
      {
        labelAreaX.Enabled = false;
        editExportX.Enabled = false;
        labelAreaY.Enabled = false;
        editExportY.Enabled = false;
        labelAreaWidth.Enabled = false;
        editAreaWidth.Enabled = false;
        labelAreaHeight.Enabled = false;
        editAreaHeight.Enabled = false;
      }
      else
      {
        labelAreaX.Enabled = true;
        editExportX.Enabled = true;
        labelAreaY.Enabled = true;
        editExportY.Enabled = true;
        labelAreaWidth.Enabled = true;
        editAreaWidth.Enabled = true;
        labelAreaHeight.Enabled = true;
        editAreaHeight.Enabled = true;
      }
    }



    private void comboCharsetMode_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( m_CharsetScreen.Mode != (TextMode)comboCharsetMode.SelectedIndex )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenValuesChange( m_CharsetScreen, this ) );
        m_CharsetScreen.Mode = (TextMode)comboCharsetMode.SelectedIndex;

        OnCharsetScreenModeChanged();
      }

      if ( m_OverrideCharMode )
      {
        RebuildCharPanelImages();
      }

      switch ( Lookup.TextCharModeFromTextMode( m_CharsetScreen.Mode ) )
      {
        case TextCharMode.COMMODORE_HIRES:
        case TextCharMode.MEGA65_HIRES:
          labelMColor1.Enabled = false;
          labelMColor2.Enabled = false;
          labelBGColor4.Enabled = false;
          comboMulticolor1.Enabled = false;
          comboMulticolor2.Enabled = false;
          comboBGColor4.Enabled = false;
          break;
        case TextCharMode.VIC20:
          labelMColor1.Enabled = true;
          labelMColor1.Text = "Border Color";
          labelMColor2.Enabled = true;
          labelMColor2.Text = "Aux. Color";
          labelBGColor4.Enabled = false;
          comboMulticolor1.Enabled = true;
          comboMulticolor2.Enabled = true;
          comboBGColor4.Enabled = false;
          break;
        case TextCharMode.COMMODORE_MULTICOLOR:
          labelMColor1.Enabled = true;
          labelMColor1.Text = "Multicolor 1";
          labelMColor2.Enabled = true;
          labelMColor2.Text = "Multicolor 2";
          labelBGColor4.Enabled = false;
          comboMulticolor1.Enabled = true;
          comboMulticolor2.Enabled = true;
          comboBGColor4.Enabled = false;
          break;
        case TextCharMode.COMMODORE_ECM:
          labelMColor1.Enabled = true;
          labelMColor1.Text = "BGColor 2";
          labelMColor2.Enabled = true;
          labelMColor2.Text = "BGColor 3";
          labelBGColor4.Enabled = true;
          comboMulticolor1.Enabled = true;
          comboMulticolor2.Enabled = true;
          comboBGColor4.Enabled = true;
          break;
        case TextCharMode.MEGA65_FCM:
        case TextCharMode.MEGA65_FCM_16BIT:
          labelMColor1.Enabled = false;
          labelMColor2.Enabled = false;
          labelBGColor4.Enabled = false;
          comboMulticolor1.Enabled = false;
          comboMulticolor2.Enabled = false;
          comboBGColor4.Enabled = false;
          break;
        default:
          Debug.Log( "comboCharsetMode_SelectedIndexChanged unsupported mode!" );
          break;
      }

      switch ( m_CharsetScreen.Mode )
      {
        case TextMode.COMMODORE_40_X_25_HIRES:
        case TextMode.COMMODORE_40_X_25_MULTICOLOR:
        case TextMode.COMMODORE_40_X_25_ECM:
        case TextMode.MEGA65_40_X_25_FCM:
        case TextMode.MEGA65_40_X_25_FCM_16BIT:
        case TextMode.MEGA65_40_X_25_ECM:
        case TextMode.MEGA65_40_X_25_HIRES:
          m_CharsWidth = 40;
          m_CharsHeight = 25;
          pictureEditor.DisplayPage.Create( 320, 200, System.Drawing.Imaging.PixelFormat.Format32bppRgb );
          break;
        case TextMode.MEGA65_80_X_25_HIRES:
        case TextMode.MEGA65_80_X_25_MULTICOLOR:
        case TextMode.MEGA65_80_X_25_FCM:
        case TextMode.MEGA65_80_X_25_FCM_16BIT:
        case TextMode.MEGA65_80_X_25_ECM:
          m_CharsWidth = 80;
          m_CharsHeight = 25;
          pictureEditor.DisplayPage.Create( 640, 200, System.Drawing.Imaging.PixelFormat.Format32bppRgb );
          break;
        case TextMode.COMMODORE_VIC20_22_X_23:
          m_CharsWidth = 22;
          m_CharsHeight = 23;
          pictureEditor.DisplayPage.Create( 176, 184, System.Drawing.Imaging.PixelFormat.Format32bppRgb );
          break;
        default:
          Debug.Log( "comboCharsetMode_SelectedIndexChanged unsupported mode!" );
          break;
      }
      RedrawFullScreen();
    }



    private void OnCharsetScreenModeChanged()
    {
      UpdatePalette();

      for ( int i = 0; i < m_CharsetScreen.CharSet.TotalNumberOfCharacters; ++i )
      {
        RebuildCharImage( i );
      }
      Modified = true;
      panelCharacters.Invalidate();
      charEditor.CharsetUpdated( m_CharsetScreen.CharSet );

      // TODO - change palette to machine type

      RedrawColorChooser();
      RedrawFullScreen();

      panelCharColors.Visible = Lookup.RequiresCustomColorForCharacter( Lookup.TextCharModeFromTextMode( m_CharsetScreen.Mode ) );
    }



    private void UpdatePalette()
    {
      int numColors = Lookup.NumberOfColorsInCharacter( Lookup.TextCharModeFromTextMode( m_CharsetScreen.Mode ) );

      // hard coded palettes
      int     numColorsInChooser = 16;

      switch ( m_CharsetScreen.Mode )
      {
        case TextMode.MEGA65_40_X_25_HIRES:
        case TextMode.MEGA65_80_X_25_HIRES:
          numColorsInChooser = 32;
          break;
        case TextMode.COMMODORE_40_X_25_ECM:
        case TextMode.COMMODORE_40_X_25_HIRES:
        case TextMode.COMMODORE_40_X_25_MULTICOLOR:
          m_CharsetScreen.CharSet.Colors.Palettes[0] = PaletteManager.PaletteFromMachine( MachineType.C64 );
          return;
        case TextMode.COMMODORE_VIC20_22_X_23:
          m_CharsetScreen.CharSet.Colors.Palettes[0] = PaletteManager.PaletteFromMachine( MachineType.VIC20 );
          return;
      }

      if ( m_NumColorsInColorChooser != numColorsInChooser )
      {
        m_NumColorsInColorChooser = numColorsInChooser;

        panelCharColors.DisplayPage.Create( 8 * m_NumColorsInColorChooser, 8, System.Drawing.Imaging.PixelFormat.Format32bppRgb );
        RedrawColorChooser();
      }

      if ( comboBackground.Items.Count != numColors )
      {
        comboBackground.BeginUpdate();

        while ( comboBackground.Items.Count < numColors )
        {
          comboBackground.Items.Add( comboBackground.Items.Count.ToString( "d2" ) );
        }
        while ( comboBackground.Items.Count > numColors )
        {
          comboBackground.Items.RemoveAt( numColors );
        }

        comboBackground.EndUpdate();
      }

      if ( m_CharsetScreen.CharSet.Colors.Palette.NumColors == numColors )
      {
        // palette is already matching, keep existing
        return;
      }
      m_CharsetScreen.CharSet.Colors.Palette = PaletteManager.PaletteFromNumColors( numColors );
    }



    private void comboBGColor4_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( m_CharsetScreen.CharSet.Colors.BGColor4 != comboBGColor4.SelectedIndex )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenValuesChange( m_CharsetScreen, this ) );

        m_CharsetScreen.CharSet.Colors.BGColor4 = comboBGColor4.SelectedIndex;
        for ( int i = 0; i < m_CharsetScreen.CharSet.TotalNumberOfCharacters; ++i )
        {
          RebuildCharImage( i );
        }
        Modified = true;
        RedrawFullScreen();
        panelCharacters.Invalidate();
        charEditor.CharsetUpdated( m_CharsetScreen.CharSet );
        RedrawColorChooser();
      }
    }



    private void btnExportToCharset_Click( object sender, EventArgs e )
    {
      var charSetData = m_CharsetScreen.CharSet.SaveToBuffer();

      Types.ComboItem comboItem = (Types.ComboItem)comboCharsetFiles.SelectedItem;
      if ( comboItem.Tag == null )
      {
        // to new file
        BaseDocument document = null;
        if ( DocumentInfo.Project == null )
        {
          document = Core.MainForm.CreateNewDocument( ProjectElement.ElementType.CHARACTER_SET, null );
        }
        else
        {
          document = Core.MainForm.CreateNewElement( ProjectElement.ElementType.CHARACTER_SET, "Character Set", DocumentInfo.Project ).Document;
        }
        if ( document.DocumentInfo.Element != null )
        {
          document.SetDocumentFilename( "New Character Set.charsetproject" );
          document.DocumentInfo.Element.Filename = document.DocumentInfo.DocumentFilename;
        }
        ( (CharsetEditor)document ).OpenProject( charSetData );
        document.SetModified();
        document.Save( SaveMethod.SAVE );
      }
      else
      {
        DocumentInfo    docInfo = (DocumentInfo)comboItem.Tag;
        CharsetEditor document = (CharsetEditor)docInfo.BaseDoc;
        if ( document == null )
        {
          if ( docInfo.Project != null )
          {
            docInfo.Project.ShowDocument( docInfo.Element );
            document = (CharsetEditor)docInfo.BaseDoc;
          }
        }
        if ( document != null )
        {
          document.OpenProject( charSetData );
          document.SetModified();
        }
      }
    }



    private void editDataExport_PreviewKeyDown( object sender, PreviewKeyDownEventArgs e )
    {
      if ( e.KeyData == ( Keys.A | Keys.Control ) )
      {
        editDataExport.SelectAll();
      }
    }



    private void btnDefaultUppercase_Click( object sender, EventArgs e )
    {
      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenCharsetChange( m_CharsetScreen, this ) );

      for ( int i = 0; i < 256; ++i )
      {
        for ( int j = 0; j < 8; ++j )
        {
          m_CharsetScreen.CharSet.Characters[i].Tile.Data.SetU8At( j, ConstantData.UpperCaseCharsetC64.ByteAt( i * 8 + j ) );
        }
        m_CharsetScreen.CharSet.Characters[i].Tile.CustomColor = 1;
      }
      Modified = true;
      CharsetChanged();
    }



    private void btnDefaultLowerCase_Click( object sender, EventArgs e )
    {
      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenCharsetChange( m_CharsetScreen, this ) );

      for ( int i = 0; i < 256; ++i )
      {
        for ( int j = 0; j < 8; ++j )
        {
          m_CharsetScreen.CharSet.Characters[i].Tile.Data.SetU8At( j, ConstantData.LowerCaseCharsetC64.ByteAt( i * 8 + j ) );
        }
        m_CharsetScreen.CharSet.Characters[i].Tile.CustomColor = 1;
      }
      Modified = true;
      CharsetChanged();
    }



    internal void CharsetChanged()
    {
      for ( int i = 0; i < m_CharsetScreen.CharSet.TotalNumberOfCharacters; ++i )
      {
        RebuildCharImage( i );
      }
      panelCharacters.Invalidate();
      charEditor.CharsetUpdated( m_CharsetScreen.CharSet );
      pictureEditor.Invalidate();
      RedrawFullScreen();
    }



    private void btnExportToBASICData_Click( object sender, EventArgs e )
    {
      // prepare data
      GR.Memory.ByteBuffer screenCharData;
      GR.Memory.ByteBuffer screenColorData;
      GR.Memory.ByteBuffer charsetData;

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

      var exportRect = DetermineExportRectangle();

      m_CharsetScreen.ExportToBuffer( out screenCharData, out screenColorData, out charsetData, exportRect.Left, exportRect.Top, exportRect.Width, exportRect.Height, ( comboExportOrientation.SelectedIndex == 0 ) );

      switch ( comboExportData.SelectedIndex )
      {
        case 0:
          editDataExport.Text = Util.ToBASICData( screenCharData + screenColorData, startLine, lineOffset, wrapByteCount );
          break;
        case 1:
          editDataExport.Text = Util.ToBASICData( screenCharData, startLine, lineOffset, wrapByteCount );
          break;
        case 2:
          editDataExport.Text = Util.ToBASICData( screenColorData, startLine, lineOffset, wrapByteCount );
          break;
        case 3:
          editDataExport.Text = Util.ToBASICData( screenColorData + screenCharData, startLine, lineOffset, wrapByteCount );
          break;
      }
    }



    private void checkApplyCharacter_CheckedChanged( object sender, EventArgs e )
    {
      m_AffectChars = checkApplyCharacter.Checked;
      if ( m_AffectChars )
      {
        checkApplyCharacter.Image = Properties.Resources.charscreen_chars;
      }
      else
      {
        checkApplyCharacter.Image = Properties.Resources.charscreen_chars_off.ToBitmap();
      }
    }



    private void checkApplyColors_CheckedChanged( object sender, EventArgs e )
    {
      m_AffectColors = checkApplyColors.Checked;
      if ( m_AffectColors )
      {
        checkApplyColors.Image = Properties.Resources.charscreen_colors;
      }
      else
      {
        checkApplyColors.Image = Properties.Resources.charscreen_colors_off.ToBitmap();
      }
    }



    private void checkShowGrid_CheckedChanged( object sender, EventArgs e )
    {
      m_ShowGrid = checkShowGrid.Checked;
      pictureEditor.Invalidate();
    }



    private void btnImportFromASM_Click( object sender, EventArgs e )
    {
      ImportFromData( Util.FromASMData( editDataImport.Text ) );
    }



    private void btnClearImportData_Click( object sender, EventArgs e )
    {
      editDataImport.Text = "";
    }



    private void editDataExport_KeyPress( object sender, KeyPressEventArgs e )
    {
      if ( ( ModifierKeys == Keys.Control )
      &&   ( e.KeyChar == 1 ) )
      {
        editDataExport.SelectAll();
        e.Handled = true;
      }
    }



    private void editDataImport_KeyPress( object sender, KeyPressEventArgs e )
    {
      if ( ( ModifierKeys == Keys.Control )
      &&   ( e.KeyChar == 1 ) )
      {
        editDataImport.SelectAll();
        e.Handled = true;
      }
    }



    public override bool ApplyFunction( Function Function )
    {
      if ( !charEditor.EditorFocused )
      {
        return false;
      }
      switch ( Function )
      {
        case Function.GRAPHIC_ELEMENT_MIRROR_H:
          charEditor.MirrorX();
          return true;
        case Function.GRAPHIC_ELEMENT_MIRROR_V:
          charEditor.MirrorY();
          return true;
        case Function.GRAPHIC_ELEMENT_SHIFT_D:
          charEditor.ShiftDown();
          return true;
        case Function.GRAPHIC_ELEMENT_SHIFT_U:
          charEditor.ShiftUp();
          return true;
        case Function.GRAPHIC_ELEMENT_SHIFT_L:
          charEditor.ShiftLeft();
          return true;
        case Function.GRAPHIC_ELEMENT_SHIFT_R:
          charEditor.ShiftRight();
          return true;
        case Function.GRAPHIC_ELEMENT_ROTATE_L:
          charEditor.RotateLeft();
          return true;
        case Function.GRAPHIC_ELEMENT_ROTATE_R:
          charEditor.RotateRight();
          return true;
        case Function.GRAPHIC_ELEMENT_INVERT:
          charEditor.Invert();
          return true;
        case Function.GRAPHIC_ELEMENT_PREVIOUS:
          charEditor.Previous();
          return true;
        case Function.GRAPHIC_ELEMENT_NEXT:
          charEditor.Next();
          return true;
        case Function.GRAPHIC_ELEMENT_CUSTOM_COLOR:
          charEditor.CustomColor();
          return true;
        case Function.GRAPHIC_ELEMENT_MULTI_COLOR_1:
          charEditor.MultiColor1();
          return true;
        case Function.GRAPHIC_ELEMENT_MULTI_COLOR_2:
          charEditor.MultiColor2();
          return true;
        case Function.GRAPHIC_ELEMENT_BACKGROUND_COLOR:
          charEditor.BackgroundColor();
          return true;
      }
      return base.ApplyFunction( Function );
    }
    
    
    
    public void ImportFromData( int Width, int Height, ByteBuffer CharData, ByteBuffer ColorData, CharsetProject Charset )
    {
      SetScreenSize( Width, Height );
      m_CharsetScreen.SetScreenSize( Width, Height );
      AdjustScrollbars();

      screenHScroll.Value = m_CharsetScreen.ScreenOffsetX;
      screenVScroll.Value = m_CharsetScreen.ScreenOffsetY;

      for ( int j = 0; j < Height; ++j )
      {
        for ( int i = 0; i < Width; ++i )
        {
          int     bufferIndex = i + j * Width;
          m_CharsetScreen.Chars[bufferIndex] = (uint)( CharData.ByteAt( bufferIndex ) + ( ColorData.ByteAt( bufferIndex ) << 16 ) );
        }
      }

      ByteBuffer    CharsetProject = Charset.SaveToBuffer();
      m_CharsetScreen.CharSet.ReadFromBuffer( CharsetProject );

      comboBackground.SelectedIndex = m_CharsetScreen.CharSet.Colors.BackgroundColor;
      comboMulticolor1.SelectedIndex = m_CharsetScreen.CharSet.Colors.MultiColor1;
      comboMulticolor2.SelectedIndex = m_CharsetScreen.CharSet.Colors.MultiColor2;
      comboCharsetMode.SelectedIndex = (int)m_CharsetScreen.Mode;
      comboBGColor4.SelectedIndex = m_CharsetScreen.CharSet.Colors.BGColor4;
      editScreenWidth.Text = m_CharsetScreen.ScreenWidth.ToString();
      editScreenHeight.Text = m_CharsetScreen.ScreenHeight.ToString();

      for ( int i = 0; i < m_CharsetScreen.CharSet.ExportNumCharacters; ++i )
      {
        RebuildCharImage( i );
      }
      RedrawFullScreen();
      RedrawColorChooser();
      Modified = true;
    }



    private void checkOverrideMode_CheckedChanged( object sender, EventArgs e )
    {
      m_OverrideCharMode = checkOverrideOriginalColorSettings.Checked;

      RebuildCharPanelImages();
    }



    private void RebuildCharPanelImages()
    {
      for ( int i = 0; i < m_CharsetScreen.CharSet.TotalNumberOfCharacters; ++i )
      {
        RebuildCharImage( i );

        panelCharacters.Items[i].MemoryImage = m_CharsetScreen.CharSet.Characters[i].Tile.Image;
      }
      panelCharacters.Invalidate();
      charEditor.CharsetUpdated( m_CharsetScreen.CharSet );
    }



    private void checkAutoCenterText_CheckedChanged( object sender, EventArgs e )
    {
      m_AutoCenterText = checkAutoCenter.Checked;
      if ( m_AutoCenterText )
      {
        m_TextEntryCachedLine.Clear();
        m_TextEntryEnteredText.Clear();
        m_TextEntryStartedInLine = -1;

        checkAutoCenter.Image = Properties.Resources.charscreen_autocenter.ToBitmap();
      }
      else
      {
        checkAutoCenter.Image = Properties.Resources.charscreen_autocenter_off.ToBitmap();
      }
    }



    private void checkReverse_CheckedChanged( object sender, EventArgs e )
    {
      m_ReverseChars = checkReverse.Checked;
      if ( m_ReverseChars )
      {
        for ( int x = 0; x < m_CharsetScreen.ScreenWidth; ++x )
        {
          for ( int y = 0; y < m_CharsetScreen.ScreenHeight; ++y )
          {
            m_ReverseCache[x, y] = false;
          }
        }
        checkApplyCharacter.Checked = false;
        checkApplyColors.Checked    = false;
        checkReverse.Image = Properties.Resources.charscreen_reverse_on;
      }
      else
      {
        checkReverse.Image = Properties.Resources.charscreen_reverse_off;
      }
    }



    private void btnExportToBASICDataHex_Click( object sender, EventArgs e )
    {
      // prepare data
      GR.Memory.ByteBuffer screenCharData;
      GR.Memory.ByteBuffer screenColorData;
      GR.Memory.ByteBuffer charsetData;

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


      var exportRect = DetermineExportRectangle();

      m_CharsetScreen.ExportToBuffer( out screenCharData, out screenColorData, out charsetData, exportRect.Left, exportRect.Top, exportRect.Width, exportRect.Height, ( comboExportOrientation.SelectedIndex == 0 ) );

      switch ( comboExportData.SelectedIndex )
      {
        case 0:
          editDataExport.Text = Util.ToBASICHexData( screenCharData + screenColorData, startLine, lineOffset );
          break;
        case 1:
          editDataExport.Text = Util.ToBASICHexData( screenCharData, startLine, lineOffset );
          break;
        case 2:
          editDataExport.Text = Util.ToBASICHexData( screenColorData, startLine, lineOffset );
          break;
        case 3:
          editDataExport.Text = Util.ToBASICHexData( screenColorData + screenCharData, startLine, lineOffset );
          break;
      }
    }



    private void btnExportToImageFile_Click( object sender, EventArgs e )
    {
      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Export Screen to Image";
      saveDlg.Filter = "PNG File|*.png";
      if ( saveDlg.ShowDialog() != DialogResult.OK )
      {
        return;
      }

      int     neededWidth   = m_CharsetScreen.ScreenWidth * 8;
      int     neededHeight  = m_CharsetScreen.ScreenHeight * 8;

      GR.Image.MemoryImage targetImg = new GR.Image.MemoryImage( neededWidth, neededHeight, System.Drawing.Imaging.PixelFormat.Format32bppRgb );

      PaletteManager.ApplyPalette( targetImg );

      m_Image.DrawTo( targetImg, 0, 0 );

      System.Drawing.Bitmap bmpTarget = targetImg.GetAsBitmap();
      bmpTarget.Save( saveDlg.FileName, System.Drawing.Imaging.ImageFormat.Png );
    }



    private void charEditor_CharactersShifted( int[] OldToNew, int[] NewToOld )
    {
      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenCharChange( m_CharsetScreen, this, 0, 0, m_CharsetScreen.ScreenWidth, m_CharsetScreen.ScreenHeight ), false );

      // now shift all characters
      for ( int j = 0; j < m_CharsetScreen.ScreenHeight; ++j )
      {
        for ( int i = 0; i < m_CharsetScreen.ScreenWidth; ++i )
        {
          uint  origChar = m_CharsetScreen.Chars[i + j * m_CharsetScreen.ScreenWidth];
          uint  origColor = ( origChar & 0xffff0000 );
          m_CharsetScreen.Chars[i + j * m_CharsetScreen.ScreenWidth] = (uint)( (uint)OldToNew[(int)( origChar & 0xffff )] | origColor );
        }
      }

      // ..and charset
      List<GR.Forms.ImageListbox.ImageListItem>    origListItems = new List<GR.Forms.ImageListbox.ImageListItem>();

      for ( int i = 0; i < m_CharsetScreen.CharSet.TotalNumberOfCharacters; ++i )
      {
        origListItems.Add( panelCharacters.Items[i] );
      }

      for ( int i = 0; i < m_CharsetScreen.CharSet.TotalNumberOfCharacters; ++i )
      {
        panelCharacters.Items[i] = origListItems[NewToOld[i]];
      }
      panelCharacters.Invalidate();

      RedrawFullScreen();
      RedrawColorChooser();
      Modified = true;
    }



    private void btnClearScreen_Click( object sender, EventArgs e )
    {
      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenCharChange( m_CharsetScreen, this, 0, 0, m_CharsetScreen.ScreenWidth, m_CharsetScreen.ScreenHeight ), false );

      // now shift all characters
      for ( int j = 0; j < m_CharsetScreen.ScreenHeight; ++j )
      {
        for ( int i = 0; i < m_CharsetScreen.ScreenWidth; ++i )
        {
          SetCharacter( i, j, 32, 1 );
        }
      }

      RedrawFullScreen();
      Modified = true;
    }



    private void charEditor_Modified()
    {
      // update charscreen charset from chareditor
      m_CharsetScreen.CharSet = charEditor.CharacterSet;
      ApplyPalette();

      for ( int i = 0; i < m_CharsetScreen.CharSet.TotalNumberOfCharacters; ++i )
      {
        RebuildCharImage( i );
      }
      panelCharacters.Invalidate();
      pictureEditor.Invalidate();
      RedrawFullScreen();

      SetModified();
    }



    private void btnExportToImage_Click( object sender, EventArgs e )
    {
      int     neededWidth   = m_CharsetScreen.ScreenWidth * 8;
      int     neededHeight  = m_CharsetScreen.ScreenHeight * 8;

      GR.Image.MemoryImage targetImg = new GR.Image.MemoryImage( neededWidth, neededHeight, System.Drawing.Imaging.PixelFormat.Format32bppRgb );

      PaletteManager.ApplyPalette( targetImg );

      m_Image.DrawTo( targetImg, 0, 0 );

      System.Drawing.Bitmap bmpTarget = targetImg.GetAsBitmap();

      Clipboard.SetImage( bmpTarget );
      bmpTarget.Dispose();
    }



    private void btnShiftLeft_Click( object sender, EventArgs e )
    {
      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenCharChange( m_CharsetScreen, this, 0, 0, m_CharsetScreen.ScreenWidth, m_CharsetScreen.ScreenHeight ) );

      for ( int j = 0; j < m_CharsetScreen.ScreenHeight; ++j )
      {
        uint  oldChar = m_CharsetScreen.Chars[0 + j * m_CharsetScreen.ScreenWidth];
        for ( int i = 1; i < m_CharsetScreen.ScreenWidth; ++i )
        {
          m_CharsetScreen.Chars[i + j * m_CharsetScreen.ScreenWidth - 1] = m_CharsetScreen.Chars[i + j * m_CharsetScreen.ScreenWidth];
        }
        m_CharsetScreen.Chars[j * m_CharsetScreen.ScreenWidth + m_CharsetScreen.ScreenWidth - 1] = oldChar;
      }
      SetModified();
      RedrawFullScreen();
    }



    private void btnShiftRight_Click( object sender, EventArgs e )
    {
      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenCharChange( m_CharsetScreen, this, 0, 0, m_CharsetScreen.ScreenWidth, m_CharsetScreen.ScreenHeight ) );

      for ( int j = 0; j < m_CharsetScreen.ScreenHeight; ++j )
      {
        uint  oldChar = m_CharsetScreen.Chars[m_CharsetScreen.ScreenWidth - 1 + j * m_CharsetScreen.ScreenWidth];
        for ( int i = m_CharsetScreen.ScreenWidth - 1; i >= 1; --i )
        {
          m_CharsetScreen.Chars[i + j * m_CharsetScreen.ScreenWidth] = m_CharsetScreen.Chars[i + j * m_CharsetScreen.ScreenWidth - 1];
        }
        m_CharsetScreen.Chars[j * m_CharsetScreen.ScreenWidth] = oldChar;
      }
      SetModified();
      RedrawFullScreen();
    }



    private void btnShiftUp_Click( object sender, EventArgs e )
    {
      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenCharChange( m_CharsetScreen, this, 0, 0, m_CharsetScreen.ScreenWidth, m_CharsetScreen.ScreenHeight ) );

      for ( int i = 0; i < m_CharsetScreen.ScreenWidth; ++i )
      {
        uint  oldChar = m_CharsetScreen.Chars[i];
        for ( int j = 0; j < m_CharsetScreen.ScreenHeight - 1; ++j )
        {
          m_CharsetScreen.Chars[i + j * m_CharsetScreen.ScreenWidth] = m_CharsetScreen.Chars[i + ( j + 1 ) * m_CharsetScreen.ScreenWidth];
        }
        m_CharsetScreen.Chars[i + ( m_CharsetScreen.ScreenHeight - 1 ) * m_CharsetScreen.ScreenWidth] = oldChar;
      }
      SetModified();
      RedrawFullScreen();
    }



    private void btnShiftDown_Click( object sender, EventArgs e )
    {
      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenCharChange( m_CharsetScreen, this, 0, 0, m_CharsetScreen.ScreenWidth, m_CharsetScreen.ScreenHeight ) );

      for ( int i = 0; i < m_CharsetScreen.ScreenWidth; ++i )
      {
        uint  oldChar = m_CharsetScreen.Chars[i + ( m_CharsetScreen.ScreenHeight - 1 ) * m_CharsetScreen.ScreenWidth];
        for ( int j = m_CharsetScreen.ScreenHeight - 1; j >= 1; --j )
        {
          m_CharsetScreen.Chars[i + j * m_CharsetScreen.ScreenWidth] = m_CharsetScreen.Chars[i + ( j - 1 ) * m_CharsetScreen.ScreenWidth];
        }
        m_CharsetScreen.Chars[i] = oldChar;
      }
      SetModified();
      RedrawFullScreen();
    }



    private void btnImportCharsetFromFile_Click( object sender, EventArgs e )
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

        int charsToImport = (int)charData.Length / 8;
        if ( charsToImport > m_CharsetScreen.CharSet.TotalNumberOfCharacters )
        {
          charsToImport = m_CharsetScreen.CharSet.TotalNumberOfCharacters;
        }

        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenCharsetChange( m_CharsetScreen, this ) );

        for ( int i = 0; i < charsToImport; ++i )
        {
          charData.CopyTo( m_CharsetScreen.CharSet.Characters[i].Tile.Data, i * 8, 8 );
          RebuildCharImage( i );
        }
        SetModified();
        RedrawFullScreen();
      }
    }



    private void panelCharColors_PostPaint( FastImage TargetBuffer )
    {
      int     x1 = m_CurrentColor * TargetBuffer.Width / m_NumColorsInColorChooser;
      int     x2 = ( m_CurrentColor + 1 ) * TargetBuffer.Width / m_NumColorsInColorChooser;

      uint  selColor = Core.Settings.FGColor( ColorableElement.SELECTION_FRAME );

      TargetBuffer.Rectangle( x1, 0, x2 - x1, TargetBuffer.Height, selColor );
    }



    private void editCharOffset_TextChanged( object sender, EventArgs e )
    {
      if ( m_CharsetScreen.CharOffset != GR.Convert.ToI32( editCharOffset.Text ) )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenValuesChange( m_CharsetScreen, this ) );

        m_CharsetScreen.CharOffset = GR.Convert.ToI32( editCharOffset.Text );
        SetModified();
      }
    }



    private void btnImportFromBASIC_Click( object sender, EventArgs e )
    {
      var settings = new C64Studio.Parser.BasicFileParser.ParserSettings();
      settings.StripREM     = true;
      settings.StripSpaces  = true;
      settings.BASICDialect = C64Models.BASIC.Dialect.BASICV2;

      var parser = new C64Studio.Parser.BasicFileParser( settings );

      string[] lines = editDataImport.Text.Split( new char[]{ '\n' }, StringSplitOptions.RemoveEmptyEntries );
      int lastLineNumber = -1;

      int     cursorX = 0;
      int     cursorY = 0;
      byte    curColor = 14;
      bool    reverseMode = false;

      foreach ( var line in lines )
      {
        var  cleanLine = line.Trim();

        var lineInfo = parser.TokenizeLine( cleanLine, 1, ref lastLineNumber );

        for ( int i = 0; i < lineInfo.Tokens.Count; ++i )
        {
          var token = lineInfo.Tokens[i];

          if ( ( token.TokenType == Parser.BasicFileParser.Token.Type.BASIC_TOKEN )
          &&   ( token.ByteValue == 0x99 ) )
          {
            // a PRINT statement
            bool    hasSemicolonAtEnd = false;
            while ( ( i + 1 < lineInfo.Tokens.Count )
            &&      ( lineInfo.Tokens[i + 1].Content != ":" ) )
            {
              ++i;

              var nextToken = lineInfo.Tokens[i];

              if ( nextToken.TokenType == Parser.BasicFileParser.Token.Type.STRING_LITERAL )
              {
                // handle incoming PETSCII plus control codes!
                bool hadError = false;
                var  actualString = Parser.BasicFileParser.ReplaceAllMacrosBySymbols( nextToken.Content.Substring( 1, nextToken.Content.Length - 2 ), out hadError );
                foreach ( var singleChar in actualString )
                {
                  var key = ConstantData.AllPhysicalKeyInfos.Find( x => x.CharValue == singleChar );
                  if ( key != null )
                  {
                    if ( ( key.Type == KeyType.GRAPHIC_SYMBOL )
                    ||   ( key.Type == KeyType.NORMAL ) )
                    {
                      if ( reverseMode )
                      {
                        SetCharacter( cursorX, cursorY, (ushort)( key.ScreenCodeValue + 128 ), curColor );
                      }
                      else
                      {
                        SetCharacter( cursorX, cursorY, key.ScreenCodeValue, curColor );
                      }
                      ++cursorX;
                      if ( cursorX >= m_CharsetScreen.ScreenWidth )
                      {
                        cursorX = 0;
                        ++cursorY;
                      }
                    }
                    else if ( key.Type == KeyType.CONTROL_CODE )
                    {
                      switch ( key.PetSCIIValue )
                      {
                        case 5:
                          curColor = 1;
                          break;
                        case 17:
                          ++cursorY;
                          break;
                        case 18:
                          reverseMode = true;
                          break;
                        case 28:
                          curColor = 2;
                          break;
                        case 29:
                          ++cursorX;
                          break;
                        case 30:
                          curColor = 5;
                          break;
                        case 31:
                          curColor = 6;
                          break;
                        case 129:
                          curColor = 8;
                          break;
                        case 144:
                          curColor = 0;
                          break;
                        case 145:
                          --cursorY;
                          break;
                        case 146:
                          reverseMode = false;
                          break;
                        case 149:
                          curColor = 9;
                          break;
                        case 150:
                          curColor = 10;
                          break;
                        case 151:
                          curColor = 11;
                          break;
                        case 152:
                          curColor = 12;
                          break;
                        case 153:
                          curColor = 13;
                          break;
                        case 154:
                          curColor = 14;
                          break;
                        case 155:
                          curColor = 15;
                          break;
                        case 156:
                          curColor = 4;
                          break;
                        case 157:
                          --cursorX;
                          if ( cursorX < 0 )
                          {
                            cursorX += m_CharsetScreen.ScreenWidth;
                            --cursorY;
                          }
                          break;
                        case 158:
                          curColor = 7;
                          break;
                        case 159:
                          curColor = 3;
                          break;
                      }
                    }
                  }
                }
                continue;
              }
              else if ( nextToken.Content == ";" )
              {
                hasSemicolonAtEnd = true;
              }
              else
              {
                hasSemicolonAtEnd = false;
                reverseMode = false;
              }
            }
            if ( !hasSemicolonAtEnd )
            {
              cursorX = 0;
              ++cursorY;

              // TODO - Scroll up
            }
          }
        }
      }
    }



    private void btnCopy_Click( object sender, EventArgs e )
    {
      CopyToClipboard();
    }



    private void btnPaste_Click( object sender, EventArgs e )
    {
      PasteFromClipboard();
    }



    private void btnExportToMarqsPETSCII_Click( object sender, EventArgs e )
    {
      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Save data as";
      saveDlg.Filter = "Marq's PETSCII File|*.c|All Files|*.*";
      if ( DocumentInfo.Project != null )
      {
        saveDlg.InitialDirectory = DocumentInfo.Project.Settings.BasePath;
      }
      if ( saveDlg.ShowDialog() != DialogResult.OK )
      {
        return;
      }

      // prepare data
      GetExportData( out ByteBuffer screenCharData, out ByteBuffer screenColorData, out System.Drawing.Rectangle exportRect );

      StringBuilder   sb = new StringBuilder();

      sb.Append( "unsigned char frame0000[]={// border,bg,chars,colors" );
      sb.Append( (char)10 );
      sb.Append( "0," );
      sb.Append( m_CharsetScreen.CharSet.Colors.BackgroundColor );
      sb.Append( ',' );
      sb.Append( (char)10 );

      int     bytePos = 0;
      for ( int j = 0; j < exportRect.Height; ++j )
      {
        for ( int i = 0; i < exportRect.Width; ++i )
        {
          sb.Append( screenCharData.ByteAt( bytePos ) );
          sb.Append( ',' );
          ++bytePos;
        }
        sb.Append( (char)10 );
      }

      bytePos = 0;
      for ( int j = 0; j < exportRect.Height; ++j )
      {
        for ( int i = 0; i < exportRect.Width; ++i )
        {
          sb.Append( screenColorData.ByteAt( bytePos ) );
          ++bytePos;
          if ( ( j < exportRect.Height - 1 )
          ||   ( i + 1 < exportRect.Width ) )
          {
            sb.Append( ',' );
          }
        }
        sb.Append( (char)10 );
      }
      sb.Append( "};" );
      sb.Append( (char)10 );

      sb.Append( "// META: " );
      sb.Append( exportRect.Width );
      sb.Append( ' ' );
      sb.Append( exportRect.Height );
      sb.Append( ' ' );
      switch ( m_CharsetScreen.Mode )
      {
        case TextMode.COMMODORE_40_X_25_ECM:
        case TextMode.COMMODORE_40_X_25_HIRES:
        case TextMode.COMMODORE_40_X_25_MULTICOLOR:
        default:
          sb.Append( "C64" );
          break;
        case TextMode.COMMODORE_VIC20_22_X_23:
          sb.Append( "VIC20" );
          break;
      }
      sb.Append( " upper" );
      sb.Append( (char)10 );

      GR.IO.File.WriteAllText( saveDlg.FileName, sb.ToString() );
    }



    private void GetExportData( out ByteBuffer ScreenCharData, out ByteBuffer ScreenColorData, out System.Drawing.Rectangle ExportRect )
    {
      ScreenCharData = new GR.Memory.ByteBuffer();
      ScreenColorData = new GR.Memory.ByteBuffer();

      ExportRect = DetermineExportRectangle();

      bool singleBytes = ( Lookup.NumBytesOfSingleCharacter( Lookup.TextCharModeFromTextMode( m_CharsetScreen.Mode ) ) == 1 );

      if ( comboExportOrientation.SelectedIndex == 0 )
      {
        // row by row
        for ( int i = ExportRect.Top; i < ExportRect.Bottom; ++i )
        {
          for ( int x = ExportRect.Left; x < ExportRect.Right; ++x )
          {
            ushort newColor = (ushort)( ( m_CharsetScreen.Chars[i * m_CharsetScreen.ScreenWidth + x] & 0xffff0000 ) >> 16 );
            ushort newChar = (ushort)( m_CharsetScreen.Chars[i * m_CharsetScreen.ScreenWidth + x] & 0xffff );


            if ( singleBytes )
            {
              ScreenCharData.AppendU8( (byte)newChar );
              ScreenColorData.AppendU8( (byte)newColor );
            }
            else
            {
              ScreenCharData.AppendU16( newChar );
              ScreenColorData.AppendU16( newColor );
            }
          }
        }
      }
      else
      {
        for ( int x = ExportRect.Left; x < ExportRect.Right; ++x )
        {
          for ( int i = ExportRect.Top; i < ExportRect.Bottom; ++i )
          {
            ushort newColor = (ushort)( ( m_CharsetScreen.Chars[i * m_CharsetScreen.ScreenWidth + x] & 0xffff0000 ) >> 16 );
            ushort newChar = (ushort)( m_CharsetScreen.Chars[i * m_CharsetScreen.ScreenWidth + x] & 0xffff );


            if ( singleBytes )
            {
              ScreenCharData.AppendU8( (byte)newChar );
              ScreenColorData.AppendU8( (byte)newColor );
            }
            else
            {
              ScreenCharData.AppendU16( newChar );
              ScreenColorData.AppendU16( newColor );
            }
          }
        }
      }
    }



  } 
}
