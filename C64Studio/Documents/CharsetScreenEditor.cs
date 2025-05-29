using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using GR.Image;
using GR.Memory;
using RetroDevStudio.Formats;
using RetroDevStudio;
using RetroDevStudio.Controls;
using GR.Forms;
using System.Drawing;
using System.Linq;
using GR.Color;



namespace RetroDevStudio.Documents
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

    private enum CharlistLayout
    { 
      PLAIN,
      PETSCII_EDITOR
    };



    private ushort                      m_CurrentChar = 0;
    private ushort                      m_CurrentColor = 1;
    private int                         m_CurrentPaletteMapping = 0;
    private int                         m_CharsWidth = 40;
    private int                         m_CharsHeight = 25;
    private int                         m_CharacterWidth = 8;
    private int                         m_CharacterHeight = 8;
    private int                         m_NumColorsInColorChooser = 16;

    private GR.Image.MemoryImage        m_Image = new GR.Image.MemoryImage( 320, 200, GR.Drawing.PixelFormat.Format32bppRgb );

    private bool[,]                     m_ErrornousChars = new bool[40, 25];
    private bool[,]                     m_SelectedChars = new bool[40, 25];
    private bool[,]                     m_ReverseCache = new bool[40, 25];
    private ushort[]                    m_CharlistLayout = new ushort[256];
    private CharlistLayout              m_CurrentLayout = CharlistLayout.PLAIN;

    private GR.Math.Rectangle           m_SelectionBounds = new GR.Math.Rectangle();


    private System.Drawing.Point        m_SelectedChar = new System.Drawing.Point( -1, -1 );

    private Formats.CharsetScreenProject    m_CharsetScreen = new RetroDevStudio.Formats.CharsetScreenProject();

    private ToolMode                    m_ToolMode = ToolMode.SINGLE_CHAR;

    private bool                        m_MouseButtonReleased = false;
    private System.Drawing.Point        m_MousePos;

    private bool                        _ShowGrid = false;
    private bool                        _HighlightUsedChars = false;

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

    private System.Drawing.Font         m_DefaultOutputFont = null;
    private ExportCharscreenFormBase    m_ExportForm = null;
    private ImportCharscreenFormBase    m_ImportForm = null;

    private ColorPickerCharsBase             _colorPickerDlg = null;

    private Dictionary<Control,int>     m_ControlsToTheRight = new Dictionary<Control, int>();
    private Dictionary<Control,int>     m_ControlsBelow = new Dictionary<Control, int>();



    public CharsetScreenEditor( StudioCore Core )
    {
      this.Core = Core;
      DocumentInfo.Type = ProjectElement.ElementType.CHARACTER_SCREEN;
      DocumentInfo.UndoManager.MainForm = Core.MainForm;
      m_IsSaveable = true;
      InitializeComponent();
      SuspendLayout();
      charEditor.Core = Core;
      charEditor.UndoManager = DocumentInfo.UndoManager;

      m_DefaultOutputFont = editDataExport.Font;

      comboExportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "as assembly", typeof( ExportCharscreenAsAssembly ) ) );
      comboExportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "as BASIC Data statements", typeof( ExportCharscreenAsBASICData ) ) );
      comboExportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "as BASIC code", typeof( ExportCharsetAsBASIC ) ) );
      comboExportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "to binary file", typeof( ExportCharscreenAsBinaryFile ) ) );
      comboExportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "to charset project file", typeof( ExportCharscreenAsCharset ) ) );
      comboExportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "to image file", typeof( ExportCharscreenAsImageFile ) ) );
      comboExportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "to image (clipboard)", typeof( ExportCharscreenAsImage ) ) );
      comboExportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "to Marq's PETSCII editor", typeof( ExportCharscreenAsMarqSPETSCII ) ) );
      comboExportMethod.SelectedIndex = 0;
      comboExportOrientation.SelectedIndex = 0;

      comboImportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "from binary file", typeof( ImportCharscreenFromBinaryFile ) ) );
      comboImportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "from character set file", typeof( ImportCharscreenCharsetFromCharsetFile ) ) );
      comboImportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "character set from assembly", typeof( ImportCharscreenCharsetFromASM ) ) );
      comboImportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "from assembly", typeof( ImportCharscreenFromASM ) ) );
      comboImportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "from BASIC PRINT statements", typeof( ImportCharscreenFromBASIC ) ) );
      comboImportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "set default character sets", typeof( ImportCharscreenCharsetFromDefault ) ) );
      comboImportMethod.SelectedIndex = 0;

      pictureEditor.DisplayPage.Create( 320, 200, GR.Drawing.PixelFormat.Format32bppRgb );
      panelCharacters.PixelFormat = GR.Drawing.PixelFormat.Format32bppRgb;
      panelCharacters.SetDisplaySize( 128, 128 );
      m_Image.Create( 320, 200, GR.Drawing.PixelFormat.Format32bppRgb );

      DPIHandler.ResizeControlsForDPI( this );
      ApplyPalette();

      comboExportData.SelectedIndex = 0;

      comboExportArea.Items.Add( "All" );
      comboExportArea.Items.Add( "Selection" );
      comboExportArea.Items.Add( "Custom Area" );
      comboExportArea.SelectedIndex = 0;

      for ( int i = 0; i < 256; ++i )
      {
        m_CharlistLayout[i] = (ushort)i;
      }
      comboCharlistLayout.Items.Add( "Default" );
      comboCharlistLayout.Items.Add( "PETSCII Editor" );
      comboCharlistLayout.SelectedIndex = 0;

      foreach ( TextMode mode in Enum.GetValues( typeof( TextMode ) ) )
      {
        if ( mode != TextMode.UNKNOWN )
        {
          comboCharsetMode.Items.Add( GR.EnumHelper.GetDescription( mode ) );
        }
      }
      comboCharsetMode.SelectedIndex = 0;

      Core.MainForm.ApplicationEvent += new MainForm.ApplicationEventHandler( MainForm_ApplicationEvent );

      int numBytesPerChar = Lookup.NumBytesOfSingleCharacterBitmap( TextCharMode.COMMODORE_HIRES );
      for ( int i = 0; i < m_CharsetScreen.CharSet.TotalNumberOfCharacters; ++i )
      {
        m_CharsetScreen.CharSet.Characters[i].Tile.CustomColor = 1;
        for ( int j = 0; j < numBytesPerChar; ++j )
        {
          m_CharsetScreen.CharSet.Characters[i].Tile.Data.SetU8At( j, ConstantData.UpperCaseCharsetC64.ByteAt( i * numBytesPerChar + j ) );
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
      SetupColorPickerDialog();
      ResumeLayout();

      // remember controls to the right and below the editor
      int     rightEnd = pictureEditor.Bounds.Right;
      int     bottomEnd = pictureEditor.Bounds.Bottom;

      foreach ( Control control in tabEditor.Controls )
      {
        if ( control == pictureEditor )
        {
          continue;
        }
        if ( control.Location.X >= rightEnd )
        {
          m_ControlsToTheRight.Add( control, control.Location.X - rightEnd );
        }
        else if ( control.Location.Y >= bottomEnd )
        {
          m_ControlsBelow.Add( control, control.Location.Y - bottomEnd );
        }
      }
    }



    private void ApplyPalette()
    {
      PaletteManager.ApplyPalette( pictureEditor.DisplayPage, m_CharsetScreen.CharSet.Colors.Palette );
      PaletteManager.ApplyPalette( panelCharacters.DisplayPage, m_CharsetScreen.CharSet.Colors.Palette );
      PaletteManager.ApplyPalette( m_Image, m_CharsetScreen.CharSet.Colors.Palette );
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
      // visible chars
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

      if ( _ShowGrid )
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
      if ( _HighlightUsedChars )
      {
        uint  highlightColor = Core.Settings.FGColor( ColorableElement.SELECTED_TEXT );

        for ( int x = x1; x <= x2; ++x )
        {
          for ( int y = y1; y <= y2; ++y )
          {
            if ( m_CharsetScreen.CharacterAt( x, y ) == m_CurrentChar )
            {
              int  sx1 = ( ( x - m_CharsetScreen.ScreenOffsetX ) * pictureEditor.ClientRectangle.Width ) / m_CharsWidth;
              int  sx2 = ( ( x + 1 - m_CharsetScreen.ScreenOffsetX ) * pictureEditor.ClientRectangle.Width ) / m_CharsWidth - 1;
              int  sy1 = ( ( y - m_CharsetScreen.ScreenOffsetY ) * pictureEditor.ClientRectangle.Height ) / m_CharsHeight;
              int  sy2 = ( ( y + 1 - m_CharsetScreen.ScreenOffsetY ) * pictureEditor.ClientRectangle.Height ) / m_CharsHeight - 1;

              TargetBuffer.BoxAlpha( sx1, sy1, sx2 - sx1 + 1, sy2 - sy1 + 1, highlightColor );
            }
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
        int  sx1 = ( ( m_SelectedChar.X - m_CharsetScreen.ScreenOffsetX ) * pictureEditor.ClientRectangle.Width ) / m_CharsWidth;
        int  sx2 = ( ( m_SelectedChar.X + 1 - m_CharsetScreen.ScreenOffsetX ) * pictureEditor.ClientRectangle.Width ) / m_CharsWidth - 1;
        int  sy1 = ( ( m_SelectedChar.Y - m_CharsetScreen.ScreenOffsetY ) * pictureEditor.ClientRectangle.Height ) / m_CharsHeight;
        int  sy2 = ( ( m_SelectedChar.Y + 1 - m_CharsetScreen.ScreenOffsetY ) * pictureEditor.ClientRectangle.Height ) / m_CharsHeight - 1;

        /*
        if ( m_SelectedChar.X - m_CharsetScreen.ScreenOffsetX == m_CharsWidth - 1 )
        {
          --sx2;
        }
        if ( m_SelectedChar.Y - m_CharsetScreen.ScreenOffsetY == m_CharsHeight - 1 )
        {
          --sy2;
        }*/

        TargetBuffer.Rectangle( sx1, sy1, sx2 - sx1 + 1, sy2 - sy1 + 1, selColor );
      }

      // current dragged selection
      if ( ( m_ToolMode == ToolMode.SELECT )
      &&   ( m_LastDragEndPos.X != -1 ) )
      {
        System.Drawing.Point    o1, o2;

        CalcRect( m_DragStartPos, m_LastDragEndPos, out o1, out o2 );

        int  sx1 = ( ( o1.X - m_CharsetScreen.ScreenOffsetX ) * pictureEditor.ClientRectangle.Width ) / m_CharsWidth;
        int  sx2 = ( ( o2.X + 1 - m_CharsetScreen.ScreenOffsetX ) * pictureEditor.ClientRectangle.Width ) / m_CharsWidth - 1;
        int  sy1 = ( ( o1.Y - m_CharsetScreen.ScreenOffsetY ) * pictureEditor.ClientRectangle.Height ) / m_CharsHeight;
        int  sy2 = ( ( o2.Y + 1 - m_CharsetScreen.ScreenOffsetY ) * pictureEditor.ClientRectangle.Height ) / m_CharsHeight - 1;

        TargetBuffer.Rectangle( sx1, sy1, sx2 - sx1 + 1, sy2 - sy1 + 1, selColor );
      }

      // draw selection
      for ( int x = x1; x <= x2; ++x )
      {
        for ( int y = y1; y <= y2; ++y )
        {
          if ( m_SelectedChars[x, y] )
          {
            int  sx1 = ( ( x - m_CharsetScreen.ScreenOffsetX ) * pictureEditor.ClientRectangle.Width ) / m_CharsWidth;
            int  sx2 = ( ( x + 1 - m_CharsetScreen.ScreenOffsetX ) * pictureEditor.ClientRectangle.Width ) / m_CharsWidth - 1;
            int  sy1 = ( ( y - m_CharsetScreen.ScreenOffsetY ) * pictureEditor.ClientRectangle.Height ) / m_CharsHeight;
            int  sy2 = ( ( y + 1 - m_CharsetScreen.ScreenOffsetY ) * pictureEditor.ClientRectangle.Height ) / m_CharsHeight - 1;

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



    void MainForm_ApplicationEvent( Types.ApplicationEvent Event )
    {
      
    }



    protected override void OnClosed( EventArgs e )
    {
      Core.MainForm.ApplicationEvent -= MainForm_ApplicationEvent;
      base.OnClosed( e );
    }



    public void RebuildCharImage( int CharIndex )
    {
      Formats.CharData Char = m_CharsetScreen.CharSet.Characters[CharIndex];

      Displayer.CharacterDisplayer.DisplayChar( m_CharsetScreen.CharSet, CharIndex, Char.Tile.Image, 0, 0 );
    }



    void DrawCharImage( GR.Image.IImage TargetImage, int X, int Y, ushort Char, ushort Color, int PaletteMappingIndex )
    {
      var altSettings = new AlternativeColorSettings( m_CharsetScreen.CharSet.Colors );
      altSettings.CustomColor         = Color;
      altSettings.PaletteMappingIndex = PaletteMappingIndex;
      Displayer.CharacterDisplayer.DisplayChar( m_CharsetScreen.CharSet, Char, TargetImage, X, Y, altSettings );
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
      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenCharChange( m_CharsetScreen, this, m_CharsetScreen.ScreenOffsetX + undoX, m_CharsetScreen.ScreenOffsetY + undoY, undoWidth, undoHeight ) );

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
                 ( undoX + i ) * m_CharacterWidth,
                 ( undoY + j ) * m_CharacterHeight,
                 (ushort)( selectionChar.second & 0xffff ),
                 (ushort)( selectionChar.second >> 16 ),
                 (ushort)( selectionChar.second >> 16 ) );

              DrawCharImage( m_Image,
                 ( m_CharsetScreen.ScreenOffsetX + undoX + i ) * m_CharacterWidth,
                 ( m_CharsetScreen.ScreenOffsetY + undoY + j ) * m_CharacterHeight,
                 (ushort)( selectionChar.second & 0xffff ),
                 (ushort)( selectionChar.second >> 16 ),
                 (ushort)( selectionChar.second >> 16 ) );

              pictureEditor.Invalidate( new System.Drawing.Rectangle( ( undoX + i ) * m_CharacterWidth,
                                                                      ( undoY + j ) * m_CharacterHeight,
                                                                      m_CharacterWidth, m_CharacterHeight ) );
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

      uint charToFill   = m_CharsetScreen.CompleteCharAt( X, Y );
      uint charToInsert = m_CharsetScreen.AssembleChar( m_CurrentChar, m_CurrentColor, m_CurrentPaletteMapping );
      if ( charToFill == charToInsert )
      {
        return;
      }

      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenCharChange( m_CharsetScreen, this, 0, 0, m_CharsetScreen.ScreenWidth, m_CharsetScreen.ScreenHeight ) );

      // for NES style filling to work we need to remember the original positions and replant later
      var changedChars = new GR.Collections.Set<System.Drawing.Point>();

      while ( pointsToCheck.Count != 0 )
      {
        System.Drawing.Point    point = pointsToCheck[pointsToCheck.Count - 1];
        pointsToCheck.RemoveAt( pointsToCheck.Count - 1 );

        if ( m_CharsetScreen.CompleteCharAt( point.X, point.Y ) != charToInsert )
        {
          DrawCharImage( m_Image, point.X * m_CharacterWidth, point.Y * m_CharacterHeight, m_CurrentChar, m_CurrentColor, m_CurrentPaletteMapping );
          m_CharsetScreen.SetCompleteCharAt( point.X, point.Y, charToInsert );
          changedChars.Add( point );

          if ( ( point.X > 0 )
          &&   ( m_CharsetScreen.CompleteCharAt( point.X - 1, point.Y ) == charToFill ) )
          {
            pointsToCheck.Add( new System.Drawing.Point( point.X - 1, point.Y ) );
          }
          if ( ( point.X + 1 < m_CharsetScreen.ScreenWidth )
          &&   ( m_CharsetScreen.CompleteCharAt( point.X + 1, point.Y ) == charToFill ) )
          {
            pointsToCheck.Add( new System.Drawing.Point( point.X + 1, point.Y ) );
          }
          if ( ( point.Y > 0 )
          &&   ( m_CharsetScreen.CompleteCharAt( point.X, point.Y - 1 ) == charToFill ) )
          {
            pointsToCheck.Add( new System.Drawing.Point( point.X, point.Y - 1 ) );
          }
          if ( ( point.Y + 1 < m_CharsetScreen.ScreenHeight )
          &&   ( m_CharsetScreen.CompleteCharAt( point.X, point.Y + 1 ) == charToFill ) )
          {
            pointsToCheck.Add( new System.Drawing.Point( point.X, point.Y + 1 ) );
          }
        }
      }
      // for NES this fixes the 2x2 block palette mapping
      if ( m_CharsetScreen.Mode == TextMode.NES )
      {
        foreach ( var point in changedChars )
        {
          // two times to force a change
          SetCharacter( point.X, point.Y, m_CurrentChar, m_CurrentColor, ( m_CurrentPaletteMapping + 1 ) % 4 );
          SetCharacter( point.X, point.Y, m_CurrentChar, m_CurrentColor, m_CurrentPaletteMapping );
        }
        RedrawFullScreen();
      }
      Modified = true;
      Redraw();
    }



    private string InfoText()
    {
      StringBuilder   sb = new StringBuilder();

      int     charX = m_MousePos.X + m_CharsetScreen.ScreenOffsetX;
      int     charY = m_MousePos.Y + m_CharsetScreen.ScreenOffsetY;
      int     numBytesOfSingleChar = Lookup.NumBytesOfSingleCharacter( Lookup.TextCharModeFromTextMode( m_CharsetScreen.Mode ) );

      sb.Append( "Pos " );
      sb.Append( charX );
      sb.Append( ',' );
      sb.Append( charY );
      sb.Append( "  Offset $" );
      sb.Append( ( charX + charY * m_CharsetScreen.ScreenWidth ).ToString( "X4" ) );

      int     screenOffset = PreferredScreenOffset( PreferredMachineType );
      if ( screenOffset != -1 )
      {
        sb.Append( "/$" );
        sb.Append( ( screenOffset + charX + charY * m_CharsetScreen.ScreenWidth ).ToString( "X4" ) );
      }
      int     colorOffset = PreferredColorOffset( PreferredMachineType );
      if ( colorOffset != -1 )
      {
        sb.Append( "/$" );
        sb.Append( ( colorOffset + charX + charY * m_CharsetScreen.ScreenWidth ).ToString( "X4" ) );
      }
      sb.AppendLine();

      sb.Append( "Char $" );
      if ( numBytesOfSingleChar == 2 )
      {
        sb.Append( m_CurrentChar.ToString( "X4" ) );
      }
      else
      {
        sb.Append( m_CurrentChar.ToString( "X2" ) );
      }
      sb.Append( ',' );
      sb.Append( m_CurrentChar );
      sb.Append( "  Color $" );
      if ( numBytesOfSingleChar == 2 )
      {
        sb.Append( m_CurrentColor.ToString( "X4" ) );
      }
      else
      {
        sb.Append( m_CurrentColor.ToString( "X2" ) );
      }
      sb.Append( ',' );
      sb.Append( m_CurrentColor );
      sb.AppendLine();
      sb.Append( "Sprite Pos $" );

      int spritePosX = charX * m_CharacterWidth + 24;
      int spritePosY = charY * m_CharacterHeight + 50;
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



    private int PreferredScreenOffset( MachineType MachineType )
    { 
      //             BASIC        SCREEN       COLOR
      // Unexpanded  $1000 -$1DFF $1E00 -$1FFF $9600 -$97FF
      //       + 3K  $0400 -$1DFF $1E00 -$1FFF $9600 -$97FF
      //       + 8K  $1200 -$3FFF $1000 -$11FF $9400 -$95FF
      //       + 16K $1200 -$5FFF $1000 -$11FF $9400 -$95FF
      //       + 24K $1200 -$7FFF $1000 -$11FF $9400 -$95FF

      switch ( MachineType )
      {
        case MachineType.C64:
          return 0x0400;
        case MachineType.PLUS4:
          return 0x0c00;
        case MachineType.VIC20:
          return 0x1e00;
        default:
          return -1;
      }
    }



    private int PreferredColorOffset( MachineType MachineType )
    {
      switch ( MachineType )
      {
        case MachineType.C64:
        case MachineType.C128:
          return 0xd800;
        case MachineType.PLUS4:
          return 0x0800;
        case MachineType.VIC20:
          return 0x9e00;
        default:
          return -1;
      }
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

        // clear reverse cache
        if ( m_ReverseChars )
        {
          for ( int x = 0; x < m_CharsetScreen.ScreenWidth; ++x )
          {
            for ( int y = 0; y < m_CharsetScreen.ScreenHeight; ++y )
            {
              m_ReverseCache[x, y] = false;
            }
          }
        }

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

                var affectedArea = DetermineAffectedArea( p1.X, p1.Y, p2.X - p1.X + 1, p2.Y - p1.Y + 1 );

                DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenCharChange( m_CharsetScreen, this, affectedArea ) );

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
            ||   ( m_CharsetScreen.Chars[charX + charY * m_CharsetScreen.ScreenWidth] != CombineChar( m_CurrentChar, m_CurrentChar, m_CurrentPaletteMapping ) ) )
            {
              if ( m_ReverseChars )
              {
                if ( m_ReverseCache[charX, charY] )
                {
                  return;
                }
                m_ReverseCache[charX, charY] = true;
              }

              var affectedArea = DetermineAffectedArea( charX, charY, 1, 1 );
              DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenCharChange( m_CharsetScreen, this, affectedArea ), m_MouseButtonReleased );
              m_MouseButtonReleased = false;

              SetCharacter( charX, charY );
              UpdateArea( affectedArea.X, affectedArea.Y, affectedArea.Width, affectedArea.Height );
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


                pictureEditor.Invalidate( new System.Drawing.Rectangle( o1.X * m_CharacterWidth, o1.Y * m_CharacterHeight, ( o2.X - o1.X + 1 ) * m_CharacterWidth, ( o2.Y - o1.Y + 1 ) * m_CharacterHeight ) );
              }
              m_LastDragEndPos = m_DragEndPos;

              System.Drawing.Point    p1, p2;

              CalcRect( m_DragStartPos, m_DragEndPos, out p1, out p2 );

              pictureEditor.Invalidate( new System.Drawing.Rectangle( p1.X * m_CharacterWidth, p1.Y * m_CharacterHeight, ( p2.X - p1.X + 1 ) * m_CharacterWidth, ( p2.Y - p1.Y + 1 ) * m_CharacterHeight ) );
              Redraw();
            }
            break;
        }
      }
      if ( ( Buttons & MouseButtons.Right ) != 0 )
      {
        m_CurrentChar           = m_CharsetScreen.CharacterAt( charX, charY );
        m_CurrentColor          = m_CharsetScreen.ColorAt( charX, charY );
        m_CurrentPaletteMapping = m_CharsetScreen.PaletteMappingAt( charX, charY );
        for ( int i = 0; i < m_CharsetScreen.CharSet.TotalNumberOfCharacters; ++i )
        {
          if ( m_CharlistLayout[i] == m_CurrentChar )
          {
            panelCharacters.SelectedIndex = i;
            break;
          }
        }
        _colorPickerDlg.SelectedColor          = m_CurrentColor;
        _colorPickerDlg.SelectedChar           = m_CurrentChar;
        _colorPickerDlg.SelectedPaletteMapping = m_CurrentPaletteMapping;
        labelInfo.Text = InfoText();
        RedrawColorChooser();
      }
    }



    private Rectangle DetermineAffectedArea( int X, int Y, int Width, int Height )
    {
      int x = ( m_CharsetScreen.Mode == TextMode.NES ) ? ( X & ~0x01 ) : X;
      int y = ( m_CharsetScreen.Mode == TextMode.NES ) ? ( Y & ~0x01 ) : Y;
      int x2 = ( m_CharsetScreen.Mode == TextMode.NES ) ? ( ( ( X + Width - 1 ) & ~0x01 ) + 1 ) : ( X + Width - 1 );
      int y2 = ( m_CharsetScreen.Mode == TextMode.NES ) ? ( ( ( Y + Height - 1 ) & ~0x01 ) + 1 ) : ( Y + Height - 1 );

      return new Rectangle( x, y, x2 - x + 1, y2 - y + 1 );
    }



    private uint CombineChar( ushort Char, ushort Color, int PaletteMapping )
    {
      if ( m_CharsetScreen.Mode == TextMode.NES )
      {
        return (uint)( Char | ( PaletteMapping << 16 ) );
      }
      return (uint)( Char | ( Color << 16 ) );
    }



    private void DrawCharacter( int X, int Y )
    {
      if ( m_ReverseChars )
      {
        DrawCharImage( pictureEditor.DisplayPage, ( X - m_CharsetScreen.ScreenOffsetX ) * m_CharacterWidth, ( Y - m_CharsetScreen.ScreenOffsetY ) * m_CharacterHeight, 
                       (ushort)( ( m_CharsetScreen.Chars[X + Y * m_CharsetScreen.ScreenWidth] & 0xffff ) ^ 0x80 ), 
                       (ushort)( m_CharsetScreen.Chars[X + Y * m_CharsetScreen.ScreenWidth] >> 16 ),
                       (ushort)( m_CharsetScreen.Chars[X + Y * m_CharsetScreen.ScreenWidth] >> 16 ) );
        return;
      }

      if ( ( m_AffectChars )
      &&   ( m_AffectColors ) )
      {
        DrawCharImage( pictureEditor.DisplayPage, ( X - m_CharsetScreen.ScreenOffsetX ) * m_CharacterWidth, ( Y - m_CharsetScreen.ScreenOffsetY ) * m_CharacterHeight, m_CurrentChar, m_CurrentColor, m_CurrentPaletteMapping );
      }
      else if ( m_AffectChars )
      {
        DrawCharImage( pictureEditor.DisplayPage, ( X - m_CharsetScreen.ScreenOffsetX ) * m_CharacterWidth, ( Y - m_CharsetScreen.ScreenOffsetY ) * m_CharacterHeight, m_CurrentChar, (ushort)( m_CharsetScreen.Chars[X + Y * m_CharsetScreen.ScreenWidth] >> 16 ), (ushort)( m_CharsetScreen.Chars[X + Y * m_CharsetScreen.ScreenWidth] >> 16 ) );
      }
      else if ( m_AffectColors )
      {
        DrawCharImage( pictureEditor.DisplayPage, ( X - m_CharsetScreen.ScreenOffsetX ) * m_CharacterWidth, ( Y - m_CharsetScreen.ScreenOffsetY ) * m_CharacterHeight, (ushort)( m_CharsetScreen.Chars[X + Y * m_CharsetScreen.ScreenWidth] & 0xffff ), m_CurrentColor, m_CurrentPaletteMapping );
      }
    }



    private void SetCharacter( int X, int Y )
    {
      SetCharacter( X, Y, m_CurrentChar, m_CurrentColor, m_CurrentPaletteMapping );
    }



    public void SetCharacter( int X, int Y, ushort Char, ushort Color, int PaletteMappingIndex )
    {
      if ( m_ReverseChars )
      {
        byte  origChar = (byte)( m_CharsetScreen.Chars[X + Y * m_CharsetScreen.ScreenWidth] & 0xff );

        SetCharacterWithoutReverse( X, Y, (byte)( Char ^ 0x80 ), Color, PaletteMappingIndex );
        return;
      }
      SetCharacterWithoutReverse( X, Y, Char, Color, PaletteMappingIndex );
    }



    public void SetCharacterWithoutReverse( int X, int Y, ushort Char, ushort Color, int PaletteMappingIndex )
    {
      if ( ( m_AffectChars )
      &&   ( m_AffectColors ) )
      {
        DrawCharImage( pictureEditor.DisplayPage, ( X - m_CharsetScreen.ScreenOffsetX ) * m_CharacterWidth, ( Y - m_CharsetScreen.ScreenOffsetY ) * m_CharacterHeight, 
                       Char, Color, PaletteMappingIndex );
        m_CharsetScreen.SetAt( X, Y, Char, Color, PaletteMappingIndex );
      }
      else if ( m_AffectChars )
      {
        DrawCharImage( pictureEditor.DisplayPage,
                       ( X - m_CharsetScreen.ScreenOffsetX ) * m_CharacterWidth,
                       ( Y - m_CharsetScreen.ScreenOffsetY ) * m_CharacterHeight,
                       Char,
                       m_CharsetScreen.ColorAt( X, Y ),
                       m_CharsetScreen.ColorAt( X, Y ) );
        m_CharsetScreen.SetCharacterAt( X, Y, Char );
      }
      else if ( m_AffectColors )
      {
        DrawCharImage( pictureEditor.DisplayPage, 
                       ( X - m_CharsetScreen.ScreenOffsetX ) * m_CharacterWidth, 
                       ( Y - m_CharsetScreen.ScreenOffsetY ) * m_CharacterHeight, 
                       m_CharsetScreen.CharacterAt( X, Y ),
                       Color,
                       PaletteMappingIndex );
        m_CharsetScreen.SetColorAt( X, Y, Color, PaletteMappingIndex );
      }

      if ( ( m_AffectColors )
      &&   ( m_CharsetScreen.Mode == TextMode.NES ) )
      {
        // NES allows one palette mapping per 2x2 char block
        int     xx = X & ~0x01;
        int     yy = Y & ~0x01;

        for ( int i = 0; i < 2; ++i )
        {
          for ( int j = 0; j < 2; ++j )
          {
            if ( ( xx + i == X )
            &&   ( yy + j == Y ) )
            {
              continue;
            }
            if ( m_CharsetScreen.PaletteMappingAt( xx + i, yy + j ) != PaletteMappingIndex )
            {
              m_CharsetScreen.SetColorAt( xx + i, yy + j, Color, PaletteMappingIndex );
              DrawCharImage( pictureEditor.DisplayPage,
                       ( xx + i - m_CharsetScreen.ScreenOffsetX ) * m_CharacterWidth,
                       ( yy + j - m_CharsetScreen.ScreenOffsetY ) * m_CharacterHeight,
                       m_CharsetScreen.CharacterAt( xx + i, yy + j ),
                       Color,
                       PaletteMappingIndex );
            }
          }
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



    private void RedrawFullScreen()
    {
      int     x1 = m_CharsetScreen.ScreenOffsetX;
      int     y1 = m_CharsetScreen.ScreenOffsetY;
      int     x2 = x1 + m_CharsetScreen.ScreenWidth - 1;
      int     y2 = y1 + m_CharsetScreen.ScreenHeight - 1;

      int     charWidth = Lookup.CharacterWidthInPixel( Lookup.GraphicTileModeFromTextCharMode( Lookup.TextCharModeFromTextMode( m_CharsetScreen.Mode ), 0 ) );
      int     charHeight = Lookup.CharacterHeightInPixel( Lookup.GraphicTileModeFromTextCharMode( Lookup.TextCharModeFromTextMode( m_CharsetScreen.Mode ), 0 ) );

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
                         ( i - x1 ) * charWidth,
                         ( j - y1 ) * charHeight,
                         m_CharsetScreen.CharacterAt( i, j ),
                         m_CharsetScreen.ColorAt( i, j ),
                         m_CharsetScreen.PaletteMappingAt( i, j ) );
        }
      }
      for ( int i = 0; i < m_CharsetScreen.ScreenWidth; ++i )
      {
        for ( int j = 0; j < m_CharsetScreen.ScreenHeight; ++j )
        {
          DrawCharImage( m_Image, i * charWidth, j * charHeight,
                         m_CharsetScreen.CharacterAt( i, j ),
                         m_CharsetScreen.ColorAt( i, j ),
                         m_CharsetScreen.PaletteMappingAt( i, j ) );
        }
      }

      pictureEditor.Invalidate();
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
      SetupColorPickerDialog();

      comboCharsetMode.SelectedIndex = (int)m_CharsetScreen.Mode;
      editCharOffset.Text = m_CharsetScreen.CharOffset.ToString();
      SetScreenSize( m_CharsetScreen.ScreenWidth, m_CharsetScreen.ScreenHeight );

      SetupColorPickerDialog();
      OnCharsetScreenModeChanged();

      panelCharacters.ItemWidth   = Lookup.CharacterWidthInPixel( Lookup.GraphicTileModeFromTextCharMode( m_CharsetScreen.CharSet.Mode, 0 ) );
      panelCharacters.ItemHeight  = Lookup.CharacterHeightInPixel( Lookup.GraphicTileModeFromTextCharMode( m_CharsetScreen.CharSet.Mode, 0 ) );

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
          DrawCharImage( m_Image, i * m_CharacterWidth, j * m_CharacterHeight,
                         m_CharsetScreen.CharacterAt( i, j ),
                         m_CharsetScreen.ColorAt( i, j ),
                         m_CharsetScreen.PaletteMappingAt( i, j ) );
        }
      }

      charEditor.CharsetUpdated( m_CharsetScreen.CharSet );

      RedrawColorChooser();
      RedrawFullScreen();

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
        Core.Notification.MessageBox( "Could not load file", "Could not load charset screen project file " + DocumentInfo.FullPath + ".\r\n" + ex.Message );
        return false;
      }
      SetUnmodified();
      return true;
    }



    public override GR.Memory.ByteBuffer SaveToBuffer()
    {
      return m_CharsetScreen.SaveToBuffer();
    }



    protected override bool QueryFilename( string PreviousFilename, out string Filename )
    {
      Filename = "";

      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Save Charset Screen Project as";
      saveDlg.Filter = "Charset Screen Projects|*.charscreen|All Files|*.*";
      saveDlg.FileName = GR.Path.GetFileName( PreviousFilename );
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
        var endButtons = MessageBoxButtons.YesNoCancel;
        if ( Core.ShuttingDown )
        {
          endButtons = MessageBoxButtons.YesNo;
        }
        DialogResult doSave = MessageBox.Show( "There are unsaved changes in your character screen project. Save now?", "Save changes?", endButtons );
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
      string extension = GR.Path.GetExtension( Filename ).ToUpper();

      if ( extension == ".CHARSETPROJECT" )
      {
        GR.Memory.ByteBuffer charSetProject = GR.IO.File.ReadAllBytes( Filename );
        if ( charSetProject == null )
        {
          return false;
        }
        var importProject = new CharsetProject();
        if ( !importProject.ReadFromBuffer( charSetProject ) )
        {
          return false;
        }

        if ( importProject.Mode != Lookup.TextCharModeFromTextMode( m_CharsetScreen.Mode ) )
        {
          Core.Notification.MessageBox( "Character set modes are different!",
                                        $"The mode of the imported charset ({importProject.Mode}) is not compatible with the current screen mode ({Lookup.TextCharModeFromTextMode( m_CharsetScreen.Mode )})!" );
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

      int numBytesOfSingleChar = Lookup.NumBytesOfSingleCharacterBitmap( Lookup.TextCharModeFromTextMode( m_CharsetScreen.Mode ) );
      int charsToImport = (int)charData.Length / numBytesOfSingleChar;
      if ( charsToImport > m_CharsetScreen.CharSet.TotalNumberOfCharacters )
      {
        charsToImport = m_CharsetScreen.CharSet.TotalNumberOfCharacters;
      }
      for ( int i = 0; i < charsToImport; ++i )
      {
        for ( int j = 0; j < numBytesOfSingleChar; ++j )
        {
          m_CharsetScreen.CharSet.Characters[i].Tile.Data.SetU8At( j, charData.ByteAt( i * numBytesOfSingleChar + j ) );
        }
        RebuildCharImage( i );
      }
      return true;
    }



    public bool OpenExternalCharset()
    {
      string filename;

      if ( OpenFile( "Open charset or charset project", Constants.FILEFILTER_CHARSET + Constants.FILEFILTER_ALL, out filename ) )
      {
        var undo = new Undo.UndoCharscreenCharsetChange( m_CharsetScreen, this );

        if ( ImportCharset( filename ) )
        {
          DocumentInfo.UndoManager.AddUndoTask( undo );

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
          return true;
        }
      }
      return false;
    }



    private void Redraw()
    {
      pictureEditor.DisplayPage.Box( 0, 0, pictureEditor.DisplayPage.Width, pictureEditor.DisplayPage.Height, 16 );
      pictureEditor.DisplayPage.DrawImage( m_Image, -m_CharsetScreen.ScreenOffsetX * m_CharacterWidth, -m_CharsetScreen.ScreenOffsetY * m_CharacterHeight );

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
            for ( int x = 0; x < m_CharacterWidth; ++x )
            {
              pictureEditor.DisplayPage.SetPixel( i * m_CharacterWidth + x - m_CharsetScreen.ScreenOffsetX * m_CharacterWidth, j * m_CharacterHeight - m_CharsetScreen.ScreenOffsetY * m_CharacterHeight, 1 );
              pictureEditor.DisplayPage.SetPixel( i * m_CharacterWidth - m_CharsetScreen.ScreenOffsetX * m_CharacterWidth, j * m_CharacterHeight + x - m_CharsetScreen.ScreenOffsetY * m_CharacterHeight, 1 );
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
                 ( m_MousePos.X + i ) * m_CharacterWidth,
                 ( m_MousePos.Y + j ) * m_CharacterHeight,
                 (ushort)( selectionChar.second & 0xffff ),
                 (ushort)( selectionChar.second >> 16 ),
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
      m_CurrentChar = (ushort)m_CharlistLayout[(ushort)panelCharacters.SelectedIndex];
      _colorPickerDlg.SelectedChar = m_CurrentChar;

      RedrawColorChooser();
      labelInfo.Text = InfoText();

      if ( _HighlightUsedChars )
      {
        pictureEditor.Invalidate();
      }
    }



    private void RedrawColorChooser()
    {
      _colorPickerDlg.Redraw();
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
        m_SelectionBounds = new GR.Math.Rectangle();
        return;
      }
      m_SelectionBounds = new GR.Math.Rectangle( minX, minY, maxX - minX + 1, maxY - minY + 1 );
    }



    private GR.Math.Rectangle DetermineExportRectangle()
    {
      switch ( comboExportArea.SelectedIndex )
      {
        case 0:
          // all
          return new GR.Math.Rectangle( 0, 0, m_CharsetScreen.ScreenWidth, m_CharsetScreen.ScreenHeight );
        case 1:
          // selection
          {
            if ( m_SelectionBounds.Width == 0 )
            {
              // no selection, select all
              return new GR.Math.Rectangle( 0, 0, m_CharsetScreen.ScreenWidth, m_CharsetScreen.ScreenHeight );
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
            return new GR.Math.Rectangle( minX, minY, width, height );
          }
      }

      // should not happen
      return new GR.Math.Rectangle( 0, 0, m_CharsetScreen.ScreenWidth, m_CharsetScreen.ScreenHeight );
    }



    public bool ImportFromData( ByteBuffer Data )
    {
      if ( Data == null )
      {
        return false;
      }
      if ( Data.Length == 1000 )
      {
        SetScreenSize( 40, 25 );
      }
      // update bg color
      charEditor.CharsetUpdated( m_CharsetScreen.CharSet );

      int   curBytePos = 0;
      if ( curBytePos < Data.Length )
      {
        // chars first
        for ( int j = 0; j < m_CharsetScreen.ScreenHeight; ++j )
        {
          for ( int i = 0; i < m_CharsetScreen.ScreenWidth; ++i )
          {
            m_CharsetScreen.SetCharacterAt( i, j, Data.ByteAt( curBytePos ) );
            ++curBytePos;
            if ( curBytePos >= Data.Length )
            {
              break;
            }
          }
          if ( curBytePos >= Data.Length )
          {
            break;
          }
        }
      }
      if ( curBytePos < Data.Length )
      {
        // sanity/shuffle colors
        for ( int j = 0; j < m_CharsetScreen.ScreenHeight; ++j )
        {
          for ( int i = 0; i < m_CharsetScreen.ScreenWidth; ++i )
          {
            ushort colorValue = Data.ByteAt( curBytePos );
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

            m_CharsetScreen.SetColorAt( i, j, colorValue, colorValue );
            ++curBytePos;
            if ( curBytePos >= Data.Length )
            {
              break;
            }
          }
          if ( curBytePos >= Data.Length )
          {
            break;
          }
        }
      }
      return true;
    }



    public void SetScreenSize( int Width, int Height )
    {
      m_ErrornousChars = new bool[Width, Height];
      m_SelectedChars = new bool[Width, Height];
      m_ReverseCache = new bool[Width, Height];

      m_CharsetScreen.SetScreenSize( Width, Height );
      m_Image.Create( Width * m_CharacterWidth, Height * m_CharacterHeight, GR.Drawing.PixelFormat.Format32bppRgb );

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



    private void screenHScroll_Scroll( DecentForms.ControlBase Sender )
    {
      if ( m_CharsetScreen.ScreenOffsetX != screenHScroll.Value )
      {
        m_CharsetScreen.ScreenOffsetX = screenHScroll.Value;
        Redraw();
      }
    }



    private void screenVScroll_Scroll( DecentForms.ControlBase Sender )
    {
      if ( m_CharsetScreen.ScreenOffsetY != screenVScroll.Value )
      {
        m_CharsetScreen.ScreenOffsetY = screenVScroll.Value;
        Redraw();
      }
    }



    private void editScreenWidth_TextChanged( object sender, EventArgs e )
    {
      int     newWidth = GR.Convert.ToI32( editScreenWidth.Text );
      int     newHeight = GR.Convert.ToI32( editScreenHeight.Text );

      if ( ( newWidth >= 1 )
      &&   ( newWidth <= 1000 )
      &&   ( newHeight >= 1 )
      &&   ( newHeight <= 1000 ) )
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
      &&   ( newWidth <= 1000 )
      &&   ( newHeight >= 1 )
      &&   ( newHeight <= 1000 ) )
      {
        btnApplyScreenSize.Enabled = true;
      }
      else
      {
        btnApplyScreenSize.Enabled = false;
      }
    }



    private void btnApplyScreenSize_Click( DecentForms.ControlBase Sender )
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
        Modified = true;
      }
    }



    public void InjectProjects( Formats.CharsetScreenProject CharScreen, Formats.CharsetProject CharSet )
    {
      m_CharsetScreen = CharScreen;
      m_CharsetScreen.CharSet = CharSet;

      comboCharsetMode.SelectedIndex = (int)m_CharsetScreen.Mode;

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
          DrawCharImage( m_Image, i * m_CharacterWidth, j * m_CharacterHeight,
                         m_CharsetScreen.CharacterAt( i, j ),
                         m_CharsetScreen.ColorAt( i, j ),
                         m_CharsetScreen.PaletteMappingAt( i, j ) );
        }
      }
      pictureEditor.Invalidate();

      SetupColorPickerDialog();
      Modified = true;
    }



    private void btnToolEdit_CheckedChanged( DecentForms.ControlBase Sender )
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



    private void btnToolRect_CheckedChanged( DecentForms.ControlBase Sender )
    {
      HideSelection();
      HideTextCursor();
      RemoveFloatingSelection();
      m_ToolMode = ToolMode.RECTANGLE;
    }



    private void btnToolFill_CheckedChanged( DecentForms.ControlBase Sender )
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



    private void btnToolSelect_CheckedChanged( DecentForms.ControlBase Sender )
    {
      HideTextCursor();
      m_ToolMode = ToolMode.SELECT;
      OnToolModeChanged();
    }



    private void btnToolQuad_CheckedChanged( DecentForms.ControlBase Sender )
    {
      HideSelection();
      HideTextCursor();
      RemoveFloatingSelection();
      m_ToolMode = ToolMode.FILLED_RECTANGLE;
      OnToolModeChanged();
    }



    private void btnToolText_CheckedChanged( DecentForms.ControlBase Sender )
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
      m_SelectionBounds = new GR.Math.Rectangle();
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

      var dataObj = new DataObject();
      dataObj.SetData( "RetroDevStudio.CharacterScreenSelection", false, dataSelection.MemoryStream() );

      Core.Imaging.ImageToClipboardData( pictureEditor.DisplayPage, x1 * m_CharacterWidth, y1 * m_CharacterHeight, ( x2 - x1 + 1 ) * m_CharacterWidth, ( y2 - y1 + 1 ) * m_CharacterHeight, dataObj );
      
      Clipboard.SetDataObject( dataObj, true );
    }



    private void PasteFromClipboard()
    {
      IDataObject dataObj = Clipboard.GetDataObject();
      if ( dataObj == null )
      {
        Core.Notification.MessageBox( "Clipboard empty", "The clipboard is empty" );
        return;
      }
      if ( dataObj.GetDataPresent( "RetroDevStudio.CharacterScreenSelection" ) )
      {
        System.IO.MemoryStream ms = (System.IO.MemoryStream)dataObj.GetData( "RetroDevStudio.CharacterScreenSelection" );
        if ( ms == null )
        {
          Core.Notification.MessageBox( "Clipboard empty", "The clipboard is empty" );
          return;
        }

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
          var key = Core.Settings.BASICKeyMap.GetKeymapEntry( bareKey );
          SingleKeyInfo    c64Key = null;
          if ( !ConstantData.PhysicalKeyInfo[MachineType.C64].ContainsKey( key.KeyboardKey ) )
          {
            Debug.Log( "No physical key info for " + key.KeyboardKey );
          }
          else
          {
            var physKey = ConstantData.PhysicalKeyInfo[MachineType.C64][key.KeyboardKey];

            c64Key = physKey.Keys[Types.KeyModifier.NORMAL];
            if ( ( shiftPushed )
            &&   ( physKey.Keys.ContainsKey( KeyModifier.SHIFT ) ) )
            {
              c64Key = physKey.Keys[KeyModifier.SHIFT];
            }
            if ( ( controlPushed )
            &&   ( physKey.Keys.ContainsKey( KeyModifier.CONTROL ) ) )
            {
              c64Key = physKey.Keys[KeyModifier.CONTROL];
            }
            if ( ( commodorePushed )
            &&   ( physKey.Keys.ContainsKey( KeyModifier.COMMODORE ) ) )
            {
              c64Key = physKey.Keys[KeyModifier.COMMODORE];
            }
          }

          if ( c64Key != null )
          {
            byte    charIndex = c64Key.ScreenCodeValue;
            int     charX = m_SelectedChar.X;
            int     charY = m_SelectedChar.Y;

            if ( m_ReverseChars )
            {
              charIndex ^= 0x80;
            }

            if ( m_TextEntryStartedInLine == -1 )
            {
              m_TextEntryStartedInLine = charY;
              m_TextEntryEnteredText.Clear();
              CacheScreenLine( m_TextEntryStartedInLine );
            }

            if ( m_AutoCenterText )
            {
              var affectedArea = DetermineAffectedArea( 0, charY, m_CharsetScreen.ScreenWidth, 1 );
              DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenCharChange( m_CharsetScreen, this, affectedArea ) );

              // restore old line
              for ( int i = 0; i < m_TextEntryCachedLine.Count; ++i )
              {
                ushort origChar = (ushort)( m_TextEntryCachedLine[i] & 0xffff );
                ushort origColor = (ushort)( m_TextEntryCachedLine[i] >> 16 );

                SetCharacter( i, charY, origChar, origColor, origColor );
              }
              pictureEditor.DisplayPage.DrawTo( m_Image,
                                                0, m_SelectedChar.Y * m_CharacterHeight,
                                                ( 0 - m_CharsetScreen.ScreenOffsetY ) * m_CharacterHeight, ( m_SelectedChar.Y - m_CharsetScreen.ScreenOffsetY ) * m_CharacterHeight,
                                                m_TextEntryCachedLine.Count * m_CharacterWidth, m_CharacterHeight );
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
                var affectedArea = DetermineAffectedArea( charX, charY, 1, 1 );
                DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenCharChange( m_CharsetScreen, this, affectedArea ) );
                charIndex = 32;
                if ( m_ReverseChars )
                {
                  charIndex ^= 0x80;
                }
              }
            }
            else if ( m_AutoCenterText )
            {
              if ( m_TextEntryEnteredText.Count >= m_CharsetScreen.ScreenWidth )
              {
                ++m_SelectedChar.Y;
                if ( m_SelectedChar.Y >= m_CharsetScreen.ScreenHeight )
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
              var affectedArea = DetermineAffectedArea( charX, charY, 1, 1 );
              DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenCharChange( m_CharsetScreen, this, affectedArea ) );
              if ( m_SelectedChar.X >= m_CharsetScreen.ScreenWidth - 1 )
              {
                m_SelectedChar.X = 0;
                ++m_SelectedChar.Y;
                if ( m_SelectedChar.Y >= m_CharsetScreen.ScreenHeight - 1 )
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
                SetCharacterWithoutReverse( newX + i, m_SelectedChar.Y, origChar, origColor, origColor );
              }
              pictureEditor.DisplayPage.DrawTo( m_Image,
                                                newX * m_CharacterWidth, m_SelectedChar.Y * m_CharacterHeight,
                                                ( newX - m_CharsetScreen.ScreenOffsetY ) * m_CharacterWidth, ( m_SelectedChar.Y - m_CharsetScreen.ScreenOffsetY ) * m_CharacterHeight,
                                                m_TextEntryCachedLine.Count * m_CharacterWidth, m_CharacterHeight );
              pictureEditor.Invalidate( new System.Drawing.Rectangle( charX * m_CharacterWidth, charY * m_CharacterHeight, m_CharacterWidth, m_CharacterHeight ) );
              pictureEditor.Invalidate( new System.Drawing.Rectangle( 0, m_SelectedChar.Y * m_CharacterHeight, m_CharsetScreen.ScreenWidth * m_CharacterWidth, m_CharacterHeight ) );
            }
            else
            {
              SetCharacterWithoutReverse( charX, charY, charIndex, m_CurrentColor, m_CurrentPaletteMapping );

              var affectedArea = DetermineAffectedArea( charX, charY, 1, 1 );
              pictureEditor.DisplayPage.DrawTo( m_Image,
                                                affectedArea.X * m_CharacterWidth, 
                                                affectedArea.Y * m_CharacterHeight,
                                                ( affectedArea.X - m_CharsetScreen.ScreenOffsetX ) * m_CharacterWidth, 
                                                ( affectedArea.Y - m_CharsetScreen.ScreenOffsetY ) * m_CharacterHeight,
                                                affectedArea.Width * m_CharacterWidth, affectedArea.Height * m_CharacterHeight );
              pictureEditor.Invalidate( new System.Drawing.Rectangle( affectedArea.X * m_CharacterWidth, affectedArea.Y * m_CharacterHeight,
                                                                      affectedArea.Width * m_CharacterWidth, affectedArea.Height * m_CharacterHeight ) );
              pictureEditor.Invalidate( new System.Drawing.Rectangle( m_SelectedChar.X * m_CharacterWidth, 
                                                                      m_SelectedChar.Y * m_CharacterHeight, 
                                                                      m_CharacterWidth, m_CharacterHeight ) );
            }
            Redraw();
            Modified = true;
          }
        }
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
                         ( x - m_CharsetScreen.ScreenOffsetX ) * m_CharacterWidth,
                         ( y - m_CharsetScreen.ScreenOffsetY ) * m_CharacterHeight,
                         m_CharsetScreen.CharacterAt( x, y ),
                         m_CharsetScreen.ColorAt( x, y ),
                         m_CharsetScreen.PaletteMappingAt( x, y ) );

          DrawCharImage( m_Image,
                         x * m_CharacterWidth,
                         y * m_CharacterHeight,
                         m_CharsetScreen.CharacterAt( x, y ),
                         m_CharsetScreen.ColorAt( x, y ),
                         m_CharsetScreen.PaletteMappingAt( x, y ) );
        }
      }
      pictureEditor.Invalidate( new System.Drawing.Rectangle( ( X - m_CharsetScreen.ScreenOffsetX ) * m_CharacterWidth, 
                                                              ( Y - m_CharsetScreen.ScreenOffsetY ) * m_CharacterHeight, Width * m_CharacterWidth, Height * m_CharacterHeight ) );
    }



    public void ValuesChanged()
    {
      editCharOffset.Text = m_CharsetScreen.CharOffset.ToString();

      if ( comboCharsetMode.SelectedIndex != (int)m_CharsetScreen.Mode )
      {
        comboCharsetMode.SelectedIndex = (int)m_CharsetScreen.Mode;
        OnCharsetScreenModeChanged();
      }

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
      bool changedMode = false;
      if ( m_CharsetScreen.Mode != (TextMode)comboCharsetMode.SelectedIndex )
      {
        changedMode = true;
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenValuesChange( m_CharsetScreen, this ) );
        m_CharsetScreen.Mode = (TextMode)comboCharsetMode.SelectedIndex;
        if ( m_CharsetScreen.Mode != TextMode.NES )
        {
          m_CurrentPaletteMapping = 0;
        }
        Modified = true;
      }
      m_CharacterWidth = Lookup.CharacterWidthInPixel( Lookup.GraphicTileModeFromTextCharMode( Lookup.TextCharModeFromTextMode( m_CharsetScreen.Mode ), 0 ) );
      m_CharacterHeight = Lookup.CharacterHeightInPixel( Lookup.GraphicTileModeFromTextCharMode( Lookup.TextCharModeFromTextMode( m_CharsetScreen.Mode ), 0 ) );

      m_CharsWidth = Lookup.ScreenWidthInCharacters( m_CharsetScreen.Mode );
      m_CharsHeight = Lookup.ScreenHeightInCharacters( m_CharsetScreen.Mode );

      SetupColorPickerDialog();
      if ( changedMode )
      {
        OnCharsetScreenModeChanged();
        SetScreenSize( m_CharsetScreen.ScreenWidth, m_CharsetScreen.ScreenHeight );
        RedrawFullScreen();
        AdjustScrollbars();
      }
    }



    private void SetupColorPickerDialog()
    {
      if ( _colorPickerDlg != null )
      {
        panelColorChooser.Controls.Remove( _colorPickerDlg );
        _colorPickerDlg.Dispose();
        _colorPickerDlg = null;
      }

      switch ( Lookup.TextCharModeFromTextMode( m_CharsetScreen.Mode ) )
      {
        case TextCharMode.X16_HIRES:
          _colorPickerDlg = new ColorPickerCharsX16( Core, m_CharsetScreen.CharSet, m_CurrentChar, (byte)m_CurrentColor );
          break;
        case TextCharMode.NES:
          _colorPickerDlg = new ColorPickerCharsNES( Core, m_CharsetScreen.CharSet, m_CurrentChar, (byte)m_CurrentColor );
          break;
        case TextCharMode.VIC20_8X16:
          _colorPickerDlg = new ColorPickerCharsCommodoreVIC20X16( Core, m_CharsetScreen.CharSet, m_CurrentChar, (byte)m_CurrentColor );
          break;
        case TextCharMode.MEGA65_HIRES:
        case TextCharMode.MEGA65_NCM:
        case TextCharMode.MEGA65_FCM:
        case TextCharMode.MEGA65_ECM:
        case TextCharMode.MEGA65_FCM_16BIT:
          _colorPickerDlg = new ColorPickerCharsMega65_32( Core, m_CharsetScreen.CharSet, m_CurrentChar, (byte)m_CurrentColor );
          break;
        default:
          _colorPickerDlg = new ColorPickerCharsCommodore( Core, m_CharsetScreen.CharSet, m_CurrentChar, (byte)m_CurrentColor );
          break;
      }
      _colorPickerDlg.SelectedColorChanged += _ColorChooserDlg_SelectedColorChanged;
      _colorPickerDlg.PaletteMappingSelected += _ColorChooserDlg_PaletteMappingSelected;
      _colorPickerDlg.Redraw();
      panelColorChooser.Controls.Add( _colorPickerDlg );
    }



    private void _ColorChooserDlg_PaletteMappingSelected( int PaletteMapping )
    {
      m_CurrentPaletteMapping = PaletteMapping;
    }



    private void _ColorChooserDlg_SelectedColorChanged( ushort Color )
    {
      m_CurrentColor = Color;
      labelInfo.Text = InfoText();
    }



    private void OnCharsetScreenModeChanged()
    {
      var clientSize = new Size( 640, 400 );
      var displaySize = new Size( 320, 200 );

      switch ( m_CharsetScreen.Mode )
      {
        case TextMode.NES:
          clientSize = new Size( 512, 480 );
          displaySize = new Size( 256, 240 );
          break;
        case TextMode.X16_20_X_15:
        case TextMode.X16_80_X_60:
        case TextMode.X16_20_X_30:
        case TextMode.X16_40_X_15:
        case TextMode.X16_40_X_30:
        case TextMode.X16_40_X_60:
        case TextMode.X16_80_X_30:
          clientSize = new Size( 640, 480 );
          // these modes adapt aspect ratio!
          displaySize = new Size( Lookup.ScreenWidthInCharacters( m_CharsetScreen.Mode ) * 8, Lookup.ScreenHeightInCharacters( m_CharsetScreen.Mode ) * 8 );
          break;
        case TextMode.COMMODORE_VIC20_8_X_8:
          displaySize = new Size( 176, 184 );
          clientSize = new Size( displaySize.Width * 4, displaySize.Height * 2 );
          break;
        case TextMode.COMMODORE_VIC20_8_X_16:
          displaySize = new Size( 160, 192 );
          clientSize = new Size( displaySize.Width * 4, displaySize.Height * 2 );
          break;
        case TextMode.MEGA65_80_X_25_ECM:
        case TextMode.MEGA65_80_X_25_FCM:
        case TextMode.MEGA65_80_X_25_FCM_16BIT:
        case TextMode.MEGA65_80_X_25_HIRES:
        case TextMode.MEGA65_80_X_25_MULTICOLOR:
        case TextMode.MEGA65_80_X_25_NCM:
          displaySize = new Size( Lookup.ScreenWidthInCharacters( m_CharsetScreen.Mode ) * 8, Lookup.ScreenHeightInCharacters( m_CharsetScreen.Mode ) * 8 );
          break;
        default:
          // all others use 16:10 (do they?)
          break;
      }

      pictureEditor.ClientSize = new Size( clientSize.Width, clientSize.Height );
      pictureEditor.DisplayPage.Create( displaySize.Width, displaySize.Height, GR.Drawing.PixelFormat.Format32bppRgb );

      int     rightEnd = pictureEditor.Bounds.Right;
      int     bottomEnd = pictureEditor.Bounds.Bottom;

      screenHScroll.Width = pictureEditor.Width;
      screenVScroll.Height = pictureEditor.Height;

      foreach ( var controlEntry in m_ControlsToTheRight )
      {
        controlEntry.Key.Location = new Point( rightEnd + controlEntry.Value, controlEntry.Key.Location.Y );
      }
      foreach ( var controlEntry in m_ControlsBelow )
      {
        controlEntry.Key.Location = new Point( controlEntry.Key.Location.X, bottomEnd + controlEntry.Value );
      }

      panelCharacters.ItemWidth = Lookup.CharacterWidthInPixel( Lookup.GraphicTileModeFromTextCharMode( Lookup.TextCharModeFromTextMode( m_CharsetScreen.Mode ), 0 ) );
      panelCharacters.ItemHeight = Lookup.CharacterHeightInPixel( Lookup.GraphicTileModeFromTextCharMode( Lookup.TextCharModeFromTextMode( m_CharsetScreen.Mode ), 0 ) );
      panelCharacters.Invalidate();
      UpdatePalette();

      charEditor.CharsetUpdated( m_CharsetScreen.CharSet );

      if ( panelCharacters.Items.Count > m_CharsetScreen.CharSet.TotalNumberOfCharacters )
      {
        panelCharacters.Items.RemoveRange( m_CharsetScreen.CharSet.TotalNumberOfCharacters,
                                           panelCharacters.Items.Count - m_CharsetScreen.CharSet.TotalNumberOfCharacters );
      }
      panelCharacters.BeginUpdate();
      for ( int i = 0; i < m_CharsetScreen.CharSet.TotalNumberOfCharacters; ++i )
      {
        if ( i >= panelCharacters.Items.Count )
        {
          panelCharacters.Items.Add( i.ToString(), m_CharsetScreen.CharSet.Characters[i].Tile.Image );
        }
        panelCharacters.Items[i].MemoryImage = m_CharsetScreen.CharSet.Characters[i].Tile.Image;
      }
      panelCharacters.EndUpdate();

      for ( int i = 0; i < m_CharsetScreen.CharSet.TotalNumberOfCharacters; ++i )
      {
        RebuildCharImage( i );
      }

      // TODO - change palette to machine type

      RedrawColorChooser();
      RedrawFullScreen();
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
        case TextMode.MEGA65_40_X_25_ECM:
        case TextMode.MEGA65_40_X_25_NCM:
        case TextMode.MEGA65_40_X_25_FCM:
        case TextMode.MEGA65_40_X_25_FCM_16BIT:
        case TextMode.MEGA65_40_X_25_MULTICOLOR:
        case TextMode.MEGA65_80_X_25_ECM:
        case TextMode.MEGA65_80_X_25_NCM:
        case TextMode.MEGA65_80_X_25_FCM:
        case TextMode.MEGA65_80_X_25_FCM_16BIT:
        case TextMode.MEGA65_80_X_25_MULTICOLOR:
          break;
        case TextMode.COMMODORE_128_VDC_80_X_25_HIRES:
          m_CharsetScreen.CharSet.Colors.Palettes[0] = Core.Imaging.PaletteFromMachine( MachineType.C128 );
          break;
        case TextMode.COMMODORE_40_X_25_ECM:
        case TextMode.COMMODORE_40_X_25_HIRES:
        case TextMode.COMMODORE_40_X_25_MULTICOLOR:
          m_CharsetScreen.CharSet.Colors.Palettes[0] = Core.Imaging.PaletteFromMachine( MachineType.C64 );
          break;
        case TextMode.COMMODORE_VIC20_8_X_8:
          m_CharsetScreen.CharSet.Colors.Palettes[0] = Core.Imaging.PaletteFromMachine( MachineType.VIC20 );
          break;
      }

      if ( m_NumColorsInColorChooser != numColorsInChooser )
      {
        m_NumColorsInColorChooser = numColorsInChooser;
        RedrawColorChooser();
      }

      m_CharsetScreen.CharSet.Colors.Palette = PaletteManager.PaletteFromMode( Lookup.TextCharModeFromTextMode( m_CharsetScreen.Mode ) );
    }



    private void editDataExport_PreviewKeyDown( object sender, PreviewKeyDownEventArgs e )
    {
      if ( e.KeyData == ( Keys.A | Keys.Control ) )
      {
        editDataExport.SelectAll();
      }
    }



    internal void CharsetChanged()
    {
      // fix up number of items in panelCharacters
      if ( panelCharacters.Items.Count > m_CharsetScreen.CharSet.TotalNumberOfCharacters )
      {
        panelCharacters.Items.RemoveRange( m_CharsetScreen.CharSet.TotalNumberOfCharacters,
                                           panelCharacters.Items.Count - m_CharsetScreen.CharSet.TotalNumberOfCharacters );
      }
      for ( int i = 0; i < m_CharsetScreen.CharSet.TotalNumberOfCharacters; ++i )
      {
        if ( i >= panelCharacters.Items.Count )
        {
          panelCharacters.Items.Add( i.ToString(), m_CharsetScreen.CharSet.Characters[i].Tile.Image );
        }
        panelCharacters.Items[i].MemoryImage = m_CharsetScreen.CharSet.Characters[i].Tile.Image;
      }

      panelCharacters.ItemWidth   = Lookup.CharacterWidthInPixel( Lookup.GraphicTileModeFromTextCharMode( m_CharsetScreen.CharSet.Mode, 0 ) );
      panelCharacters.ItemHeight  = Lookup.CharacterHeightInPixel( Lookup.GraphicTileModeFromTextCharMode( m_CharsetScreen.CharSet.Mode, 0 ) );

      for ( int i = 0; i < m_CharsetScreen.CharSet.TotalNumberOfCharacters; ++i )
      {
        RebuildCharImage( i );
      }
      panelCharacters.Invalidate();
      charEditor.CharsetUpdated( m_CharsetScreen.CharSet );
      pictureEditor.Invalidate();
      RedrawFullScreen();
    }



    private void checkApplyCharacter_CheckedChanged( DecentForms.ControlBase Sender )
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



    private void checkApplyColors_CheckedChanged( DecentForms.ControlBase Sender )
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



    private void checkShowGrid_CheckedChanged( DecentForms.ControlBase Sender )
    {
      _ShowGrid = checkShowGrid.Checked;
      checkShowGrid.Image = _ShowGrid ? Properties.Resources.charscreen_grid_on.ToBitmap() : Properties.Resources.charscreen_grid_off.ToBitmap();
      pictureEditor.Invalidate();
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



    public override bool ApplyFunction( Function Function )
    {
      if ( charEditor.EditorFocused )
      {
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
          case Function.COPY:
            charEditor.Copy();
            return true;
          case Function.PASTE:
            charEditor.Paste();
            return true;
        }
      }
      else if ( pictureEditor.Focused )
      {
        switch ( Function )
        {
          case Function.COPY:
            if ( m_ToolMode == ToolMode.SELECT )
            {
              if ( !FocusSupport.IsFocusOnChildOfAndCouldAffectReason( tabEditor, FocusSupport.FocusControlReason.COPY_PASTE ) )
              {
                CopyToClipboard();
                return true;
              }
            }
            break;
          case Function.PASTE:
            if ( !FocusSupport.IsFocusOnChildOfAndCouldAffectReason( tabEditor, FocusSupport.FocusControlReason.COPY_PASTE ) )
            {
              PasteFromClipboard();
              return true;
            }
            break;
        }
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

      comboCharsetMode.SelectedIndex = (int)m_CharsetScreen.Mode;
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



    private void checkAutoCenterText_CheckedChanged( DecentForms.ControlBase Sender )
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



    private void checkReverse_CheckedChanged( DecentForms.ControlBase Sender )
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
        checkReverse.Image = Properties.Resources.charscreen_reverse_on;
      }
      else
      {
        checkReverse.Image = Properties.Resources.charscreen_reverse_off;
      }
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



    private void btnClearScreen_Click( DecentForms.ControlBase Sender )
    {
      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenCharChange( m_CharsetScreen, this, 0, 0, m_CharsetScreen.ScreenWidth, m_CharsetScreen.ScreenHeight ), false );

      // now shift all characters
      for ( int j = 0; j < m_CharsetScreen.ScreenHeight; ++j )
      {
        for ( int i = 0; i < m_CharsetScreen.ScreenWidth; ++i )
        {
          SetCharacter( i, j, 32, 1, 0 );
        }
      }

      RedrawFullScreen();
      Modified = true;
    }



    private void charEditor_Modified( List<int> AffectedChars )
    {
      // update charscreen charset from chareditor
      m_CharsetScreen.CharSet = charEditor.CharacterSet;
      ApplyPalette();

      panelCharacters.Invalidate();
      pictureEditor.Invalidate();
      RedrawFullScreen();

      Modified = true;
    }



    private void btnShiftLeft_Click( DecentForms.ControlBase Sender )
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
      Modified = true;
      RedrawFullScreen();
    }



    private void btnShiftRight_Click( DecentForms.ControlBase Sender )
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
      Modified = true;
      RedrawFullScreen();
    }



    private void btnShiftUp_Click( DecentForms.ControlBase Sender )
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
      Modified = true;
      RedrawFullScreen();
    }



    private void btnShiftDown_Click( DecentForms.ControlBase Sender )
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
      Modified = true;
      RedrawFullScreen();
    }



    private void panelCharColors_PostPaint( FastImage TargetBuffer )
    {
      if ( Lookup.TextCharModeFromTextMode( m_CharsetScreen.Mode ) == TextCharMode.X16_HIRES )
      {
        return;
      }

      int     x1 = m_CurrentColor * TargetBuffer.Width / m_NumColorsInColorChooser;
      int     x2 = ( m_CurrentColor + 1 ) * TargetBuffer.Width / m_NumColorsInColorChooser;

      if ( Core != null )
      {
        uint  selColor = Core.Settings.FGColor( ColorableElement.SELECTION_FRAME );

        TargetBuffer.Rectangle( x1, 0, x2 - x1, TargetBuffer.Height, selColor );
      }
    }



    private void editCharOffset_TextChanged( object sender, EventArgs e )
    {
      if ( m_CharsetScreen.CharOffset != GR.Convert.ToI32( editCharOffset.Text ) )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenValuesChange( m_CharsetScreen, this ) );

        m_CharsetScreen.CharOffset = GR.Convert.ToI32( editCharOffset.Text );
        Modified = true;
      }
    }



    private void btnCopy_Click( DecentForms.ControlBase Sender )
    {
      CopyToClipboard();
    }



    private void btnPaste_Click( DecentForms.ControlBase Sender )
    {
      PasteFromClipboard();
    }



    private void comboExportMethod_SelectedIndexChanged( object sender, EventArgs e )
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
      ||   ( item.second == null ) )
      {
        return;
      }
      m_ExportForm = (ExportCharscreenFormBase)Activator.CreateInstance( item.second, new object[]{ Core } );
      m_ExportForm.Parent = panelExport;
      m_ExportForm.CreateControl();
    }



    private void btnExport_Click( DecentForms.ControlBase Sender )
    {
      var exportInfo        = new ExportCharsetScreenInfo()
      {
        Charscreen  = m_CharsetScreen,
        Area        = DetermineExportRectangle(),
        RowByRow    = ( comboExportOrientation.SelectedIndex == 0 ),
        Data        = (ExportCharsetScreenInfo.ExportData)comboExportData.SelectedIndex,
        Image       = m_Image,
      };

      if ( ( exportInfo.Data == ExportCharsetScreenInfo.ExportData.CHARSET )
      &&   ( comboExportArea.SelectedIndex == 1 ) )
      {
        // export selection (use selection from charset control)
        exportInfo.SelectedCharactersInCharset = charEditor.SelectedIndices;
      }


      editDataExport.Text = "";
      editDataExport.Font = m_DefaultOutputFont;
      m_CharsetScreen.ExportToBuffer( exportInfo );
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
      ||   ( item.second == null ) )
      {
        return;
      }
      m_ImportForm = (ImportCharscreenFormBase)Activator.CreateInstance( item.second, new object[] { Core } );
      m_ImportForm.Parent = panelImport;
      m_ImportForm.Size = panelImport.ClientSize;
      m_ImportForm.CreateControl();
    }



    private void btnImport_Click( DecentForms.ControlBase Sender )
    {
      var undo1 = new Undo.UndoCharscreenCharChange( m_CharsetScreen, this, 0, 0, m_CharsetScreen.ScreenWidth, m_CharsetScreen.ScreenHeight );
      var undo2 = new Undo.UndoCharscreenValuesChange( m_CharsetScreen, this );
      var undo3 = new Undo.UndoCharscreenCharsetChange( m_CharsetScreen, this );
      var undo4 = new Undo.UndoCharscreenSizeChange( m_CharsetScreen, this, m_CharsetScreen.ScreenWidth, m_CharsetScreen.ScreenHeight );

      if ( m_ImportForm.HandleImport( m_CharsetScreen, this ) )
      {
        DocumentInfo.UndoManager.StartUndoGroup();
        DocumentInfo.UndoManager.AddUndoTask( undo1, false );
        DocumentInfo.UndoManager.AddUndoTask( undo2, false );
        DocumentInfo.UndoManager.AddUndoTask( undo3, false );
        DocumentInfo.UndoManager.AddUndoTask( undo4, false );
        Modified = true;
        RedrawFullScreen();
      }
    }



    public override bool CopyPossible
    {
      get
      {
        return ( ( charEditor.EditorFocused )
        ||       ( ( m_ToolMode == ToolMode.SELECT )
        &&         ( !FocusSupport.IsFocusOnChildOfAndCouldAffectReason( tabEditor, FocusSupport.FocusControlReason.COPY_PASTE ) ) ) );
      }
    }



    public override bool PastePossible
    {
      get
      {
        return ( ( charEditor.EditorFocused )
        ||       ( !FocusSupport.IsFocusOnChildOfAndCouldAffectReason( tabEditor, FocusSupport.FocusControlReason.COPY_PASTE ) ) );
      }
    }




    private void comboCharlistLayout_SelectedIndexChanged( object sender, EventArgs e )
    {
      SetCharlistLayout( (CharlistLayout)comboCharlistLayout.SelectedIndex );
    }



    private void SetCharlistLayout( CharlistLayout Layout )
    {
      if ( ( Layout == m_CurrentLayout )
      &&   ( m_CharlistLayout.Length == m_CharsetScreen.CharSet.TotalNumberOfCharacters ) )
      {
        return;
      }

      m_CharlistLayout = new ushort[m_CharsetScreen.CharSet.TotalNumberOfCharacters];

      switch ( Layout )
      {
        case CharlistLayout.PLAIN:
          for ( int i = 0; i < m_CharsetScreen.CharSet.TotalNumberOfCharacters; ++i )
          {
            m_CharlistLayout[i] = (ushort)i;
          }
          break;
        case CharlistLayout.PETSCII_EDITOR:
          {
            // PETSCII editor
            var petsciiList = new ushort[]
            {
              32, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15,
              16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 46, 44, 59, 33, 63,
              48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 34, 35, 36, 37, 38, 39,
              112, 110, 85, 73, 79, 80, 108, 123, 78, 77, 40, 64, 41, 27, 29, 58,
              109, 125, 74, 75, 76, 122, 124, 126, 95, 105, 114, 91, 113, 104, 102, 92,
              100, 111, 82, 70, 67, 68, 69, 119, 99, 86, 106, 89, 96, 66, 101, 93,
              118, 103, 107, 72, 115, 116, 71, 117, 84, 121, 47, 45, 127, 98, 120, 97,
              60, 62, 61, 42, 43, 30, 31, 65, 88, 83, 87, 81, 90, 94, 28, 0
            };

            for ( int i = 0; i < 128; ++i )
            {
              m_CharlistLayout[i] = petsciiList[i];
              m_CharlistLayout[i + 128] = (ushort)( 128 + petsciiList[i] );
            }
            for ( int i = 256; i < m_CharsetScreen.CharSet.TotalNumberOfCharacters; ++i )
            {
              m_CharlistLayout[i] = (ushort)i;
            }
          }
          break;
      }
      m_CurrentLayout = Layout;
      for ( int i = 0; i < 256; ++i )
      {
        panelCharacters.Items[i].MemoryImage = m_CharsetScreen.CharSet.Characters[m_CharlistLayout[i]].Tile.Image;
      }
      panelCharacters.Invalidate();
    }



    public override void OnApplicationEvent( ApplicationEvent Event )
    {
      switch ( Event.EventType )
      {
        case ApplicationEvent.Type.DEFAULT_PALETTE_CHANGED:
          {
            bool prevModified = Modified;
            if ( !string.IsNullOrEmpty( Event.OriginalValue ) )
            {
              Core.Imaging.ApplyPalette( (PaletteType)Enum.Parse( typeof( PaletteType ), Event.OriginalValue, true ),
                                         Lookup.PaletteTypeFromTextCharMode( m_CharsetScreen.CharSet.Mode ),
                                         m_CharsetScreen.CharSet.Colors );
            }
            else
            {
              Core.Imaging.ApplyPalette( Lookup.PaletteTypeFromTextCharMode( m_CharsetScreen.CharSet.Mode ),
                                         Lookup.PaletteTypeFromTextCharMode( m_CharsetScreen.CharSet.Mode ),
                                         m_CharsetScreen.CharSet.Colors );

            }
            charEditor.ColorsChanged();
            Modified = prevModified;
          }
          break;
      }
    }



    protected override bool ProcessCmdKey( ref Message msg, Keys keyData )
    {
      if ( keyData == Keys.Escape )
      {
        if ( !FocusSupport.IsFocusOnChildOfAndCouldAffectReason( tabEditor, FocusSupport.FocusControlReason.ESCAPE ) )
        {
          if ( ( m_ToolMode == ToolMode.RECTANGLE )
          ||   ( m_ToolMode == ToolMode.FILLED_RECTANGLE ) )
          {
            if ( m_IsDragging )
            {
              m_IsDragging = false;
              Redraw();
            }
          }

          if ( m_FloatingSelection != null )
          {
            RemoveFloatingSelection();

            if ( m_LastDragEndPos.X != -1 )
            {
              m_LastDragEndPos.X = -1;
              m_IsDragging = false;
              Redraw();
            }
          }
          return true;
        }
      }
      return base.ProcessCmdKey( ref msg, keyData );
    }



    public void ImportCharsetFromImage( GR.Image.IImage Image )
    {
      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharacterEditorCharChange( charEditor, m_CharsetScreen.CharSet, 0, m_CharsetScreen.CharSet.TotalNumberOfCharacters ) );

      int   charWidth   = Lookup.CharacterWidthInPixel( m_CharsetScreen.CharSet.Characters[0].Tile.Mode );
      int   charHeight  = Lookup.CharacterHeightInPixel( m_CharsetScreen.CharSet.Characters[0].Tile.Mode );
      int   numCharsX   = Image.Width / charWidth;
      int   numCharsY   = Image.Height / charHeight;

      var   customColor = new GR.Generic.Tupel<ColorType, byte>( ColorType.CUSTOM_COLOR, 1 );
      var   bgColor = new GR.Generic.Tupel<ColorType, byte>( ColorType.BACKGROUND, 0 );

      for ( int i = 0; i < m_CharsetScreen.CharSet.TotalNumberOfCharacters; ++i )
      {
        int x = ( i % numCharsX );
        int y = i / numCharsX;

        if ( ( x >= Image.Width )
        ||   ( y >= Image.Height ) )
        {
          break;
        }
        for ( int xx = 0; xx < charWidth; ++xx )
        {
          for ( int yy = 0; yy < charHeight; ++yy )
          {
            if ( Image.GetPixel( x * charWidth + xx, y * charHeight + yy ) != 0 )
            {
              m_CharsetScreen.CharSet.Characters[i].Tile.SetPixel( xx, yy, customColor );
            }
            else
            {
              m_CharsetScreen.CharSet.Characters[i].Tile.SetPixel( xx, yy, bgColor );
            }
          }
        }
        RebuildCharImage( i );
        panelCharacters.Items[i].MemoryImage = m_CharsetScreen.CharSet.Characters[i].Tile.Image;
      }
      RedrawFullScreen();
      charEditor.CharsetUpdated( m_CharsetScreen.CharSet );
    }



    private void checkHighlightUsedChars_CheckedChanged( DecentForms.ControlBase Sender )
    {
      _HighlightUsedChars = checkHighlightUsedChars.Checked;
      checkHighlightUsedChars.Image = _HighlightUsedChars ? Properties.Resources.charscreen_highlightusedchars_on.ToBitmap() : Properties.Resources.charscreen_highlightusedchars_off.ToBitmap();
      pictureEditor.Invalidate();
    }



  } 
}
