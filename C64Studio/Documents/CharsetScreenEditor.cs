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



    private byte                        m_CurrentChar = 0;
    private byte                        m_CurrentColor = 1;
    private bool                        m_OverrideCharMode = false;

    private GR.Image.MemoryImage        m_Image = new GR.Image.MemoryImage( 320, 200, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );

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

    private List<GR.Generic.Tupel<bool,ushort>>       m_FloatingSelection = null;
    private System.Drawing.Size                       m_FloatingSelectionSize;
    private System.Drawing.Point                      m_FloatingSelectionPos;

    private bool                        m_AffectChars = true;
    private bool                        m_AffectColors = true;
    private bool                        m_AutoCenterText = false;
    private bool                        m_ReverseChars = false;

    private int                         m_TextEntryStartedInLine = -1;
    private List<ushort>                m_TextEntryCachedLine = new List<ushort>();
    private List<ushort>                m_TextEntryEnteredText = new List<ushort>();




    public CharsetScreenEditor( StudioCore Core )
    {
      this.Core = Core;
      DocumentInfo.Type = ProjectElement.ElementType.CHARACTER_SCREEN;
      DocumentInfo.UndoManager.MainForm = Core.MainForm;
      m_IsSaveable = true;
      InitializeComponent();
      charEditor.Core = Core;

      GR.Image.DPIHandler.ResizeControlsForDPI( this );

      charEditor.UndoManager = DocumentInfo.UndoManager;

      pictureEditor.PostPaint += new GR.Forms.FastPictureBox.PostPaintCallback( pictureEditor_PostPaint );

      pictureEditor.DisplayPage.Create( 320, 200, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );
      panelCharacters.PixelFormat = System.Drawing.Imaging.PixelFormat.Format8bppIndexed;
      panelCharacters.SetDisplaySize( 128, 128 );
      panelCharColors.DisplayPage.Create( 128, 8, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );
      m_Image.Create( 320, 200, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );

      CustomRenderer.PaletteManager.ApplyPalette( pictureEditor.DisplayPage );
      CustomRenderer.PaletteManager.ApplyPalette( panelCharacters.DisplayPage );
      CustomRenderer.PaletteManager.ApplyPalette( m_Image );
      CustomRenderer.PaletteManager.ApplyPalette( panelCharColors.DisplayPage );
      for ( int i = 0; i < 16; ++i )
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

      comboCharsetMode.Items.Add( "HiRes" );
      comboCharsetMode.Items.Add( "MultiColor" );
      comboCharsetMode.Items.Add( "Enhanced Char Mode (ECM)" );
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

      for ( int i = 0; i < 256; ++i )
      {
        for ( int j = 0; j < 8; ++j )
        {
          m_CharsetScreen.CharSet.Characters[i].Color = 1;
          m_CharsetScreen.CharSet.Characters[i].Data.SetU8At( j, Types.ConstantData.UpperCaseCharset.ByteAt( i * 8 + j ) );
        }
      }

      editScreenWidth.Text = "40";
      editScreenHeight.Text = "25";

      AdjustScrollbars();

      for ( int i = 0; i < 256; ++i )
      {
        RebuildCharImage( i );
        panelCharacters.Items.Add( i.ToString(), m_CharsetScreen.CharSet.Characters[i].Image );
      }
      charEditor.CharsetUpdated( m_CharsetScreen.CharSet );
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
            TargetBuffer.SetPixel( i * ( pictureEditor.ClientRectangle.Width / 40 ), j, 0xffc0c0c0 );
          }
        }
        for ( int i = 0; i < m_CharsetScreen.ScreenHeight; ++i )
        {
          for ( int j = 0; j < TargetBuffer.Width; ++j )
          {
            TargetBuffer.SetPixel( j, i * ( pictureEditor.ClientRectangle.Height / 25 ), 0xffc0c0c0 );
          }
        }
      }
    }



    void MainForm_ApplicationEvent( C64Studio.Types.ApplicationEvent Event )
    {
      if ( Event.EventType == C64Studio.Types.ApplicationEvent.Type.ELEMENT_CREATED )
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
      if ( Event.EventType == C64Studio.Types.ApplicationEvent.Type.ELEMENT_REMOVED )
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
        Displayer.CharacterDisplayer.DisplayChar( m_CharsetScreen.CharSet, CharIndex, Char.Image, 0, 0, m_CurrentColor,
                m_CharsetScreen.BackgroundColor,
                m_CharsetScreen.MultiColor1,
                m_CharsetScreen.MultiColor2,
                m_CharsetScreen.BGColor4,
                m_CharsetScreen.Mode );
      }
      else
      {
        Displayer.CharacterDisplayer.DisplayChar( m_CharsetScreen.CharSet, CharIndex, Char.Image, 0, 0 );
      }
    }



    void DrawCharImage( GR.Image.IImage TargetImage, int X, int Y, byte Char, byte Color )
    {
      Displayer.CharacterDisplayer.DisplayChar( m_CharsetScreen.CharSet, Char, TargetImage, X, Y, Color );
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

      Core.Theming.DrawSingleColorComboBox( combo, e );
    }



    private void comboMulticolor_DrawItem( object sender, DrawItemEventArgs e )
    {
      ComboBox combo = (ComboBox)sender;

      Core.Theming.DrawMultiColorComboBox( combo, e );
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
          var selectionChar = m_FloatingSelection[( offsetX + i ) + ( offsetY + j ) * m_FloatingSelectionSize.Width];
          if ( selectionChar.first )
          {
            m_CharsetScreen.Chars[undoX + i + m_CharsetScreen.ScreenOffsetX + ( undoY + j + m_CharsetScreen.ScreenOffsetY ) * m_CharsetScreen.ScreenWidth] = selectionChar.second;

            DrawCharImage( pictureEditor.DisplayPage,
               ( undoX + i ) * 8,
               ( undoY + j ) * 8,
               (byte)( selectionChar.second & 0xff ),
               (byte)( selectionChar.second >> 8 ) );

            DrawCharImage( m_Image,
               ( m_CharsetScreen.ScreenOffsetX + undoX + i ) * 8,
               ( m_CharsetScreen.ScreenOffsetY + undoY + j ) * 8,
               (byte)( selectionChar.second & 0xff ),
               (byte)( selectionChar.second >> 8 ) );

            /*
            pictureEditor.DisplayPage.DrawTo( m_Image,
                                              ( m_CharsetScreen.ScreenOffsetX + undoX + i ) * 8,
                                              ( m_CharsetScreen.ScreenOffsetY + undoY + j ) * 8,
                                              ( undoX + i ) * 8,
                                              ( undoY + j ) * 8,
                                              8, 8 );*/
            pictureEditor.Invalidate( new System.Drawing.Rectangle( ( undoX + i ) * 8,
                                                                    ( undoY + j ) * 8,
                                                                    8, 8 ) );
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

      ushort charToFill = m_CharsetScreen.Chars[X + m_CharsetScreen.ScreenWidth * Y];
      ushort charToInsert = (ushort)( m_CurrentChar | ( m_CurrentColor << 8 ) );
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
          DrawCharImage( pictureEditor.DisplayPage, ( point.X - m_CharsetScreen.ScreenOffsetX ) * 8, ( point.Y - m_CharsetScreen.ScreenOffsetY ) * 8, m_CurrentChar, m_CurrentColor );
          pictureEditor.DisplayPage.DrawTo( m_Image,
                                            point.X * 8, point.Y * 8,
                                            ( point.X - m_CharsetScreen.ScreenOffsetX ) * 8, ( point.Y - m_CharsetScreen.ScreenOffsetY ) * 8,
                                            8, 8 );
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
      int     charX = X / ( pictureEditor.ClientRectangle.Width / 40 ) + m_CharsetScreen.ScreenOffsetX;
      int     charY = Y / ( pictureEditor.ClientRectangle.Height / 25 ) + m_CharsetScreen.ScreenOffsetY;

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
            if ( !m_IsDragging )
            {
              return;
            }
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
              pictureEditor.DisplayPage.DrawTo( m_Image,
                                                p1.X * 8, p1.Y * 8,
                                                ( p1.X - m_CharsetScreen.ScreenOffsetX ) * 8, ( p1.Y - m_CharsetScreen.ScreenOffsetY ) * 8,
                                                ( p2.X - p1.X + 1 ) * 8, ( p2.Y - p1.Y + 1 ) * 8 );
              pictureEditor.Invalidate( new System.Drawing.Rectangle( p1.X * 8, p1.Y * 8, ( p2.X - p1.X + 1 ) * 8, ( p2.Y - p1.Y + 1 ) * 8 ) );
              Modified = true;
            }
            break;
          case ToolMode.SELECT:
            if ( !m_IsDragging )
            {
              return;
            }
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
            ||   ( m_CharsetScreen.Chars[charX + charY * m_CharsetScreen.ScreenWidth] != (ushort)( m_CurrentChar | ( m_CurrentColor << 8 ) ) ) )
            {
              DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenCharChange( m_CharsetScreen, this, charX, charY, 1, 1 ) );

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

            if ( m_DragEndPos != m_LastDragEndPos )
            {
              // restore background
              if ( m_LastDragEndPos.X != -1 )
              {
                System.Drawing.Point    o1, o2;

                CalcRect( m_DragStartPos, m_LastDragEndPos, out o1, out o2 );

                m_Image.DrawTo( pictureEditor.DisplayPage,
                                ( o1.X - m_CharsetScreen.ScreenOffsetX ) * 8, ( o1.Y - m_CharsetScreen.ScreenOffsetY ) * 8,
                                o1.X * 8, o1.Y * 8,
                                ( o2.X - o1.X + 1 ) * 8, ( o2.Y - o1.Y + 1 ) * 8 );

                pictureEditor.Invalidate( new System.Drawing.Rectangle( ( o1.X - m_CharsetScreen.ScreenOffsetX ) * 8,
                                                                        ( o1.Y - m_CharsetScreen.ScreenOffsetY ) * 8, 
                                                                        ( o2.X - o1.X + 1 ) * 8, 
                                                                        ( o2.Y - o1.Y + 1 ) * 8 ) );
              }
              m_LastDragEndPos = m_DragEndPos;

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
              pictureEditor.Invalidate( new System.Drawing.Rectangle( ( p1.X - m_CharsetScreen.ScreenOffsetX ) * 8, 
                                                                      ( p1.Y - m_CharsetScreen.ScreenOffsetY ) * 8, 
                                                                      ( p2.X - p1.X + 1 ) * 8, 
                                                                      ( p2.Y - p1.Y + 1 ) * 8 ) );
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
        m_CurrentChar = (byte)( m_CharsetScreen.Chars[charX + charY * m_CharsetScreen.ScreenWidth] & 0x00ff );
        m_CurrentColor = (byte)( m_CharsetScreen.Chars[charX + charY * m_CharsetScreen.ScreenWidth] >> 8 );
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
                       (byte)( ( m_CharsetScreen.Chars[X + Y * m_CharsetScreen.ScreenWidth] & 0xff ) ^ 0x80 ), 
                       (byte)( m_CharsetScreen.Chars[X + Y * m_CharsetScreen.ScreenWidth] >> 8 ) );
        return;
      }

      if ( ( m_AffectChars )
      &&   ( m_AffectColors ) )
      {
        DrawCharImage( pictureEditor.DisplayPage, ( X - m_CharsetScreen.ScreenOffsetX ) * 8, ( Y - m_CharsetScreen.ScreenOffsetY ) * 8, m_CurrentChar, m_CurrentColor );
      }
      else if ( m_AffectChars )
      {
        DrawCharImage( pictureEditor.DisplayPage, ( X - m_CharsetScreen.ScreenOffsetX ) * 8, ( Y - m_CharsetScreen.ScreenOffsetY ) * 8, m_CurrentChar, (byte)( m_CharsetScreen.Chars[X + Y * m_CharsetScreen.ScreenWidth] >> 8 ) );
      }
      else if ( m_AffectColors )
      {
        DrawCharImage( pictureEditor.DisplayPage, ( X - m_CharsetScreen.ScreenOffsetX ) * 8, ( Y - m_CharsetScreen.ScreenOffsetY ) * 8, (byte)( m_CharsetScreen.Chars[X + Y * m_CharsetScreen.ScreenWidth] & 0xff ), m_CurrentColor );
      }
    }



    private void SetCharacter( int X, int Y )
    {
      SetCharacter( X, Y, m_CurrentChar, m_CurrentColor );
    }



    private void SetCharacter( int X, int Y, byte Char, byte Color )
    {
      if ( m_ReverseChars )
      {
        byte  origChar = (byte)( m_CharsetScreen.Chars[X + Y * m_CharsetScreen.ScreenWidth] & 0xff );

        DrawCharImage( pictureEditor.DisplayPage, 
                       ( X - m_CharsetScreen.ScreenOffsetX ) * 8, 
                       ( Y - m_CharsetScreen.ScreenOffsetY ) * 8,
                       (byte)( origChar ^ 0x80 ), Color );
        m_CharsetScreen.Chars[X + Y * m_CharsetScreen.ScreenWidth] = (ushort)( ( origChar ^ 0x80 ) | ( Color << 8 ) );
        return;
      }

      if ( ( m_AffectChars )
      &&   ( m_AffectColors ) )
      {
        DrawCharImage( pictureEditor.DisplayPage, ( X - m_CharsetScreen.ScreenOffsetX ) * 8, ( Y - m_CharsetScreen.ScreenOffsetY ) * 8, Char, Color );
        m_CharsetScreen.Chars[X + Y * m_CharsetScreen.ScreenWidth] = (ushort)( Char | ( Color << 8 ) );
      }
      else if ( m_AffectChars )
      {
        DrawCharImage( pictureEditor.DisplayPage, ( X - m_CharsetScreen.ScreenOffsetX ) * 8, ( Y - m_CharsetScreen.ScreenOffsetY ) * 8, Char, (byte)( m_CharsetScreen.Chars[X + Y * m_CharsetScreen.ScreenWidth] >> 8 ) );
        m_CharsetScreen.Chars[X + Y * m_CharsetScreen.ScreenWidth] = (ushort)( Char | ( m_CharsetScreen.Chars[X + Y * m_CharsetScreen.ScreenWidth] & 0xff00 ) );
      }
      else if ( m_AffectColors )
      {
        DrawCharImage( pictureEditor.DisplayPage, ( X - m_CharsetScreen.ScreenOffsetX ) * 8, ( Y - m_CharsetScreen.ScreenOffsetY ) * 8, (byte)( m_CharsetScreen.Chars[X + Y * m_CharsetScreen.ScreenWidth] & 0xff ), Color );
        m_CharsetScreen.Chars[X + Y * m_CharsetScreen.ScreenWidth] = (ushort)( ( m_CharsetScreen.Chars[X + Y * m_CharsetScreen.ScreenWidth] & 0xff ) | ( Color << 8 ) );
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
      pictureEditor.DisplayPage.Box( 0, 0, pictureEditor.DisplayPage.Width, pictureEditor.DisplayPage.Height, 16 );
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
                         (byte)( m_CharsetScreen.Chars[i + j * m_CharsetScreen.ScreenWidth] & 0xff ),
                         (byte)( m_CharsetScreen.Chars[i + j * m_CharsetScreen.ScreenWidth] >> 8 ) );
        }
      }
      for ( int i = 0; i < m_CharsetScreen.ScreenWidth; ++i )
      {
        for ( int j = 0; j < m_CharsetScreen.ScreenHeight; ++j )
        {
          DrawCharImage( m_Image, i * 8, j * 8,
                         (byte)( m_CharsetScreen.Chars[i + j * m_CharsetScreen.ScreenWidth] & 0xff ),
                         (byte)( m_CharsetScreen.Chars[i + j * m_CharsetScreen.ScreenWidth] >> 8 ) );
        }
      }

      pictureEditor.Invalidate();
    }



    private void comboBackground_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( m_CharsetScreen.BackgroundColor != comboBackground.SelectedIndex )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenValuesChange( m_CharsetScreen, this ) );

        SetBackgroundColor( comboBackground.SelectedIndex );
      }
    }



    private void SetBackgroundColor( int ColorIndex )
    {
      m_CharsetScreen.BackgroundColor = ColorIndex;
      m_CharsetScreen.CharSet.BackgroundColor = m_CharsetScreen.BackgroundColor;
      for ( int i = 0; i < 256; ++i )
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
      if ( m_CharsetScreen.MultiColor1 != comboMulticolor1.SelectedIndex )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenValuesChange( m_CharsetScreen, this ) );

        m_CharsetScreen.MultiColor1 = comboMulticolor1.SelectedIndex;
        m_CharsetScreen.CharSet.MultiColor1 = m_CharsetScreen.MultiColor1;
        for ( int i = 0; i < 256; ++i )
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
      if ( m_CharsetScreen.MultiColor2 != comboMulticolor2.SelectedIndex )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenValuesChange( m_CharsetScreen, this ) );

        m_CharsetScreen.MultiColor2 = comboMulticolor2.SelectedIndex;
        m_CharsetScreen.CharSet.MultiColor2 = m_CharsetScreen.MultiColor2;
        for ( int i = 0; i < 256; ++i )
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
      SetScreenSize( m_CharsetScreen.ScreenWidth, m_CharsetScreen.ScreenHeight );

      comboBackground.SelectedIndex = m_CharsetScreen.CharSet.BackgroundColor;
      comboMulticolor1.SelectedIndex = m_CharsetScreen.CharSet.MultiColor1;
      comboMulticolor2.SelectedIndex = m_CharsetScreen.CharSet.MultiColor2;
      comboCharsetMode.SelectedIndex = (int)m_CharsetScreen.Mode;
      comboBGColor4.SelectedIndex = m_CharsetScreen.CharSet.BGColor4;

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
        for ( int i = 0; i < 256; ++i )
        {
          RebuildCharImage( i );
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
                         (byte)( m_CharsetScreen.Chars[i + j * m_CharsetScreen.ScreenWidth] & 0xff ),
                         (byte)( m_CharsetScreen.Chars[i + j * m_CharsetScreen.ScreenWidth] >> 8 ) );
        }
      }

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
        System.Windows.Forms.MessageBox.Show( "Could not load charset screen project file " + DocumentInfo.FullPath + ".\r\n" + ex.Message, "Could not load file" );
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
        for ( int i = 0; i < m_CharsetScreen.CharSet.NumCharacters; ++i )
        {
          RebuildCharImage( i );
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
      if ( charsToImport > 256 )
      {
        charsToImport = 256;
      }
      for ( int i = 0; i < charsToImport; ++i )
      {
        for ( int j = 0; j < 8; ++j )
        {
          m_CharsetScreen.CharSet.Characters[i].Data.SetU8At( j, charData.ByteAt( i * 8 + j ) );
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

      if ( OpenFile( "Open charset or charset project", C64Studio.Types.Constants.FILEFILTER_CHARSET + C64Studio.Types.Constants.FILEFILTER_ALL, out filename ) )
      {
        if ( ImportCharset( filename ) )
        {
          RedrawFullScreen();
          pictureEditor.Invalidate();
          labelInfo.Text = InfoText();

          if ( ( DocumentInfo.Project == null )
          || ( string.IsNullOrEmpty( DocumentInfo.Project.Settings.BasePath ) ) )
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
      if ( x2 - x1 > 40 )
      {
        x2 = x1 + 39;
      }
      if ( y1 < 0 )
      {
        y1 = 0;
      }
      if ( y2 >= m_CharsetScreen.ScreenHeight )
      {
        y2 = m_CharsetScreen.ScreenHeight - 1;
      }
      if ( y2 - y1 > 25 )
      {
        y2 = y1 + 24;
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
      // mark selected char
      if ( m_SelectedChar.X != -1 )
      {
        for ( int x = 0; x < 8; ++x )
        {
          pictureEditor.DisplayPage.SetPixel( m_SelectedChar.X * 8 + x - m_CharsetScreen.ScreenOffsetX * 8, m_SelectedChar.Y * 8 - m_CharsetScreen.ScreenOffsetY * 8, 16 );
          pictureEditor.DisplayPage.SetPixel( m_SelectedChar.X * 8 - m_CharsetScreen.ScreenOffsetX * 8, m_SelectedChar.Y * 8 + x - m_CharsetScreen.ScreenOffsetY * 8, 16 );
        }
      }

      // draw selection
      for ( int x = x1; x <= x2; ++x )
      {
        for ( int y = y1; y <= y2; ++y )
        {
          if ( m_SelectedChars[x, y] )
          {
            if ( ( y == 0 )
            ||   ( !m_SelectedChars[x, y - 1] ) )
            {
              for ( int i = 0; i < 8; ++i )
              {
                pictureEditor.DisplayPage.SetPixel( ( x - m_CharsetScreen.ScreenOffsetX ) * 8 + i,
                                                    ( y - m_CharsetScreen.ScreenOffsetY ) * 8,
                                                    16 );
              }
            }
            if ( ( y == m_SelectedChars.GetUpperBound( 1 ) )
            ||   ( !m_SelectedChars[x, y + 1] ) )
            {
              for ( int i = 0; i < 8; ++i )
              {
                pictureEditor.DisplayPage.SetPixel( ( x - m_CharsetScreen.ScreenOffsetX ) * 8 + i,
                                                    ( y - m_CharsetScreen.ScreenOffsetY ) * 8 + 7,
                                                    16 );
              }
            }
            if ( ( x == 0 )
            ||   ( !m_SelectedChars[x - 1, y] ) )
            {
              for ( int i = 0; i < 8; ++i )
              {
                pictureEditor.DisplayPage.SetPixel( ( x - m_CharsetScreen.ScreenOffsetX ) * 8,
                                                    ( y - m_CharsetScreen.ScreenOffsetY ) * 8 + i,
                                                    16 );
              }
            }
            if ( ( x == m_SelectedChars.GetUpperBound( 0 ) )
            ||   ( !m_SelectedChars[x + 1, y] ) )
            {
              for ( int i = 0; i < 8; ++i )
              {
                pictureEditor.DisplayPage.SetPixel( ( x - m_CharsetScreen.ScreenOffsetX ) * 8 + 7,
                                                    ( y - m_CharsetScreen.ScreenOffsetY ) * 8 + i,
                                                    16 );
              }
            }
          }
        }
      }

      // current dragged selection
      if ( ( m_ToolMode == ToolMode.SELECT )
      && ( m_LastDragEndPos.X != -1 ) )
      {
        System.Drawing.Point    o1, o2;

        CalcRect( m_DragStartPos, m_LastDragEndPos, out o1, out o2 );

        o1.Offset( -m_CharsetScreen.ScreenOffsetX, -m_CharsetScreen.ScreenOffsetY );
        o2.Offset( -m_CharsetScreen.ScreenOffsetX, -m_CharsetScreen.ScreenOffsetY );

        pictureEditor.DisplayPage.Rectangle( o1.X * 8, o1.Y * 8, ( o2.X - o1.X + 1 ) * 8, ( o2.Y - o1.Y + 1 ) * 8, 16 );
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
                 (byte)( selectionChar.second & 0xff ),
                 (byte)( selectionChar.second >> 8 ) );
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
      for ( byte i = 0; i < 16; ++i )
      {
        DrawCharImage( panelCharColors.DisplayPage, i * 8, 0, m_CurrentChar, i );
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
      if ( ( Buttons & MouseButtons.Left ) == MouseButtons.Left )
      {
        int colorIndex = X / 16;
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
      sb.Append( " PRINT\"" + Types.ConstantData.PetSCIIToChar[147].CharValue + "\";\n" );
      startLineNo += lineStep;

      sb.Append( startLineNo );
      startLineNo += lineStep;
      sb.Append( " POKE53280," + m_CharsetScreen.BackgroundColor.ToString() + ":POKE53281," + m_CharsetScreen.BackgroundColor.ToString() + "\n" );

      System.Drawing.Rectangle    exportRect = DetermineExportRectangle();

      for ( int i = exportRect.Top; i < exportRect.Bottom; ++i )
      {
        int   startLength = sb.Length;
        sb.Append( startLineNo );
        startLineNo += lineStep;
        sb.Append( " PRINT\"" );
        for ( int x = exportRect.Left; x < exportRect.Right; ++x )
        {
          byte newColor = (byte)( ( ( m_CharsetScreen.Chars[i * m_CharsetScreen.ScreenWidth + x] & 0xff00 ) >> 8 ) & 0x0f );
          byte newChar = (byte)( m_CharsetScreen.Chars[i * m_CharsetScreen.ScreenWidth + x] & 0xff );

          List<char>  charsToAppend = new List<char>();

          if ( newColor != curColor )
          {
            charsToAppend.Add( Types.ConstantData.PetSCIIToChar[Types.ConstantData.ColorToPetSCIIChar[newColor]].CharValue );
            curColor = newColor;
          }
          if ( newChar >= 128 )
          {
            if ( !isReverse )
            {
              isReverse = true;
              charsToAppend.Add( Types.ConstantData.PetSCIIToChar[18].CharValue );
            }
          }
          else if ( isReverse )
          {
            isReverse = false;
            charsToAppend.Add( Types.ConstantData.PetSCIIToChar[146].CharValue );
          }
          if ( isReverse )
          {
            if ( newChar == 128 + 34 )
            {
              // reverse apostrophe
              string    replacement = "\"CHR$(34)CHR$(20)CHR$(34)\"";

              for ( int t = 0; t < replacement.Length; ++t )
              {
                charsToAppend.Add( Types.ConstantData.CharToC64Char[replacement[t]].CharValue );
              }
            }
            else
            {
              charsToAppend.Add( Types.ConstantData.ScreenCodeToChar[(byte)( newChar - 128 )].CharValue );
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
                charsToAppend.Add( Types.ConstantData.CharToC64Char[replacement[t]].CharValue );
              }
            }
            else
            {
              charsToAppend.Add( Types.ConstantData.ScreenCodeToChar[newChar].CharValue );
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
            byte newChar = (byte)( m_CharsetScreen.Chars[i * m_CharsetScreen.ScreenWidth + x] & 0xff );
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
            if ( Types.ConstantData.ScreenCodeToChar[newChar].HasPetSCII )
            {
              sb.Append( Types.ConstantData.ScreenCodeToChar[charToAdd].CharValue );
            }
            else
            {
              sb.Append( "\", $" );
              sb.Append( newChar.ToString( "X2" ) );
              sb.Append( "\"" );
            }
          }
          sb.AppendLine( "\"" );
        }
        editDataExport.Text = sb.ToString();
        return;
      }

      m_CharsetScreen.ExportToBuffer( out screenCharData, out screenColorData, out charsetData, exportRect.Left, exportRect.Top, exportRect.Width, exportRect.Height, ( comboExportOrientation.SelectedIndex == 0 ) );

      string screenData = Util.ToASMData( screenCharData, checkExportToDataWrap.Checked, GR.Convert.ToI32( editWrapByteCount.Text ), editPrefix.Text, checkExportHex.Checked );
      string colorData = Util.ToASMData( screenColorData, checkExportToDataWrap.Checked, GR.Convert.ToI32( editWrapByteCount.Text ), editPrefix.Text, checkExportHex.Checked );

      sb.Append( ";size " );
      sb.Append( exportRect.Width );
      sb.Append( "," );
      sb.Append( exportRect.Height );
      sb.AppendLine();

      switch ( comboExportData.SelectedIndex )
      {
        case 0:
          editDataExport.Text = sb + ";screen char data" + System.Environment.NewLine + screenData + System.Environment.NewLine + ";screen color data" + System.Environment.NewLine + colorData;
          break;
        case 1:
          editDataExport.Text = sb + ";screen char data" + System.Environment.NewLine + screenData + System.Environment.NewLine;
          break;
        case 2:
          editDataExport.Text = sb + ";screen color data" + System.Environment.NewLine + colorData;
          break;
        case 3:
          editDataExport.Text = sb + ";screen color data" + System.Environment.NewLine + colorData + System.Environment.NewLine + ";screen char data" + System.Environment.NewLine + screenData;
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
      if ( saveDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
      {
        return;
      }

      // prepare data
      GR.Memory.ByteBuffer screenCharData = new GR.Memory.ByteBuffer();
      GR.Memory.ByteBuffer screenColorData = new GR.Memory.ByteBuffer();

      var exportRect = DetermineExportRectangle();

      if ( comboExportOrientation.SelectedIndex == 0 )
      {
        // row by row
        for ( int i = exportRect.Top; i < exportRect.Bottom; ++i )
        {
          for ( int x = exportRect.Left; x < exportRect.Right; ++x )
          {
            byte newColor = (byte)( ( m_CharsetScreen.Chars[i * m_CharsetScreen.ScreenWidth + x] & 0xff00 ) >> 8 );
            byte newChar = (byte)( m_CharsetScreen.Chars[i * m_CharsetScreen.ScreenWidth + x] & 0xff );

            screenCharData.AppendU8( newChar );
            screenColorData.AppendU8( newColor );
          }
        }
      }
      else
      {
        for ( int x = exportRect.Left; x < exportRect.Right; ++x )
        {
          for ( int i = exportRect.Top; i < exportRect.Bottom; ++i )
          {
            byte newColor = (byte)( ( m_CharsetScreen.Chars[i * m_CharsetScreen.ScreenWidth + x] & 0xff00 ) >> 8 );
            byte newChar = (byte)( m_CharsetScreen.Chars[i * m_CharsetScreen.ScreenWidth + x] & 0xff );

            screenCharData.AppendU8( newChar );
            screenColorData.AppendU8( newColor );
          }
        }
      }

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

      if ( OpenFile( "Open Charpad project, Marc's PETSCII editor files or binary data", C64Studio.Types.Constants.FILEFILTER_CHARSET_CHARPAD + C64Studio.Types.Constants.FILEFILTER_MARCS_PETSCII + C64Studio.Types.Constants.FILEFILTER_ALL, out filename ) )
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

          m_CharsetScreen.CharSet.BackgroundColor = cpProject.BackgroundColor;
          m_CharsetScreen.CharSet.MultiColor1 = cpProject.MultiColor1;
          m_CharsetScreen.CharSet.MultiColor2 = cpProject.MultiColor2;
          m_CharsetScreen.MultiColor1 = cpProject.MultiColor1;
          m_CharsetScreen.MultiColor2 = cpProject.MultiColor2;

          int maxChars = cpProject.NumChars;
          if ( maxChars > 256 )
          {
            maxChars = 256;
          }

          m_CharsetScreen.CharSet.NumCharacters = maxChars;
          for ( int charIndex = 0; charIndex < m_CharsetScreen.CharSet.NumCharacters; ++charIndex )
          {
            m_CharsetScreen.CharSet.Characters[charIndex].Data = cpProject.Characters[charIndex].Data;
            m_CharsetScreen.CharSet.Characters[charIndex].Color = cpProject.Characters[charIndex].Color;
            m_CharsetScreen.CharSet.Characters[charIndex].Mode = cpProject.MultiColor ? Types.CharsetMode.MULTICOLOR : C64Studio.Types.CharsetMode.HIRES;

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

          comboBackground.SelectedIndex = mapProject.Charset.BackgroundColor;
          comboMulticolor1.SelectedIndex = mapProject.Charset.MultiColor1;
          comboMulticolor2.SelectedIndex = mapProject.Charset.MultiColor2;
          comboCharsetMode.SelectedIndex = (int)( cpProject.MultiColor ? Types.CharsetMode.MULTICOLOR : Types.CharsetMode.HIRES );

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

              if ( screenData.Length == 2002 )
              {
                // border and BG first
                m_CharsetScreen.BackgroundColor = screenData.ByteAt( 1 );
                screenData = screenData.SubBuffer( 2 );
              }
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
            m_CharsetScreen.Chars[i + j * m_CharsetScreen.ScreenWidth] = (ushort)( Data.ByteAt( i + j * m_CharsetScreen.ScreenWidth ) | ( m_CharsetScreen.Chars[i + j * m_CharsetScreen.ScreenWidth] & 0xff00 ) );
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
            m_CharsetScreen.Chars[i + j * m_CharsetScreen.ScreenWidth] = (ushort)( ( ( Data.ByteAt( m_CharsetScreen.ScreenWidth * m_CharsetScreen.ScreenHeight + i + j * m_CharsetScreen.ScreenWidth ) & 0x0f ) << 8 ) | ( m_CharsetScreen.Chars[i + j * m_CharsetScreen.ScreenWidth] & 0xff ) );
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
      m_Image.Create( Width * 8, Height * 8, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );
      CustomRenderer.PaletteManager.ApplyPalette( m_Image );

      m_TextEntryCachedLine.Clear();
      m_TextEntryEnteredText.Clear();
      m_TextEntryStartedInLine = -1;

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

      if ( m_CharsetScreen.ScreenWidth <= 40 )
      {
        screenHScroll.Maximum = 0;
        screenHScroll.Enabled = false;
        m_CharsetScreen.ScreenOffsetX = 0;
      }
      else
      {
        screenHScroll.Maximum = m_CharsetScreen.ScreenWidth - 40;
        screenHScroll.Enabled = true;
      }
      if ( m_CharsetScreen.ScreenOffsetX > screenHScroll.Maximum )
      {
        m_CharsetScreen.ScreenOffsetX = screenHScroll.Maximum;
      }

      screenVScroll.Minimum = 0;
      if ( m_CharsetScreen.ScreenHeight <= 25 )
      {
        screenVScroll.Maximum = 0;
        screenVScroll.Enabled = false;
        m_CharsetScreen.ScreenOffsetY = 0;
      }
      else
      {
        screenVScroll.Maximum = m_CharsetScreen.ScreenHeight - 25;
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

      comboBackground.SelectedIndex = m_CharsetScreen.CharSet.BackgroundColor;
      comboMulticolor1.SelectedIndex = m_CharsetScreen.CharSet.MultiColor1;
      comboMulticolor2.SelectedIndex = m_CharsetScreen.CharSet.MultiColor2;
      comboCharsetMode.SelectedIndex = (int)m_CharsetScreen.Mode;
      comboBGColor4.SelectedIndex = m_CharsetScreen.CharSet.BGColor4;

      for ( int i = 0; i < m_CharsetScreen.CharSet.NumCharacters; ++i )
      {
        RebuildCharImage( i );

        panelCharacters.Items[i].MemoryImage = m_CharsetScreen.CharSet.Characters[i].Image;
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
                         (byte)( m_CharsetScreen.Chars[i + j * m_CharsetScreen.ScreenWidth] & 0xff ),
                         (byte)( m_CharsetScreen.Chars[i + j * m_CharsetScreen.ScreenWidth] >> 8 ) );
        }
      }
      pictureEditor.Invalidate();

      RedrawColorChooser();
    }



    private void btnToolEdit_CheckedChanged( object sender, EventArgs e )
    {
      HideSelection();
      m_ToolMode = ToolMode.SINGLE_CHAR;
    }



    private void btnToolRect_CheckedChanged( object sender, EventArgs e )
    {
      HideSelection();
      m_ToolMode = ToolMode.RECTANGLE;
    }



    private void btnToolFill_CheckedChanged( object sender, EventArgs e )
    {
      HideSelection();
      m_ToolMode = ToolMode.FILL;
    }



    private void btnToolSelect_CheckedChanged( object sender, EventArgs e )
    {
      m_ToolMode = ToolMode.SELECT;
    }



    private void btnToolQuad_CheckedChanged( object sender, EventArgs e )
    {
      HideSelection();
      m_ToolMode = ToolMode.FILLED_RECTANGLE;
    }



    private void btnToolText_CheckedChanged( object sender, EventArgs e )
    {
      HideSelection();
      m_ToolMode = ToolMode.TEXT;
      m_TextEntryStartedInLine = -1;
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
            dataSelection.AppendU16( m_CharsetScreen.Chars[( y1 + y ) * m_CharsetScreen.ScreenWidth + x1 + x] );
          }
          else
          {
            dataSelection.AppendU8( 0 );
          }
        }
      }

      DataObject dataObj = new DataObject();

      dataObj.SetData( "C64Studio.CharacterScreenSelection", false, dataSelection.MemoryStream() );

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
      if ( dataObj.GetDataPresent( "C64Studio.CharacterScreenSelection" ) )
      {
        System.IO.MemoryStream ms = (System.IO.MemoryStream)dataObj.GetData( "C64Studio.CharacterScreenSelection" );

        GR.Memory.ByteBuffer data = new GR.Memory.ByteBuffer( (uint)ms.Length );

        ms.Read( data.Data(), 0, (int)ms.Length );

        GR.IO.MemoryReader memIn = data.MemoryReader();

        int   selectionWidth  = memIn.ReadInt32();
        int   selectionHeight = memIn.ReadInt32();

        m_FloatingSelection = new List<GR.Generic.Tupel<bool, ushort>>();
        m_FloatingSelectionSize = new System.Drawing.Size( selectionWidth, selectionHeight );

        for ( int y = 0; y < selectionHeight; ++y )
        {
          for ( int x = 0; x < selectionWidth; ++x )
          {
            bool  isCharSet = ( memIn.ReadUInt8() != 0 );
            if ( isCharSet )
            {
              m_FloatingSelection.Add( new GR.Generic.Tupel<bool, ushort>( true, memIn.ReadUInt16() ) );
            }
            else
            {
              m_FloatingSelection.Add( new GR.Generic.Tupel<bool, ushort>( false, 0 ) );
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
        System.Windows.Forms.Keys bareKey = e.KeyData & ~( System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.ShiftKey | System.Windows.Forms.Keys.Alt );
        bareKey = e.KeyData;

        bool    controlPushed = false;
        bool    commodorePushed = false;
        bool    shiftPushed = false;
        if ( ( bareKey & System.Windows.Forms.Keys.Shift ) == System.Windows.Forms.Keys.Shift )
        {
          bareKey &= ~System.Windows.Forms.Keys.Shift;
          shiftPushed = true;
        }
        if ( ( bareKey & System.Windows.Forms.Keys.Control ) == System.Windows.Forms.Keys.Control )
        {
          bareKey &= ~System.Windows.Forms.Keys.Control;
          commodorePushed = true;
        }
        if ( GR.Win32.KeyboardInfo.GetKeyState( System.Windows.Forms.Keys.Tab ).IsPressed )
        {
          controlPushed = true;
        }
        if ( Core.Settings.BASICKeyMap.KeymapEntryExists( bareKey ) )
        {
          //Debug.Log( "KeyData " + bareKey );

          var key = Core.Settings.BASICKeyMap.GetKeymapEntry( bareKey );

          if ( !Types.ConstantData.PhysicalKeyInfo.ContainsKey( key.KeyboardKey ) )
          {
            Debug.Log( "No physical key info for " + key.KeyboardKey );
          }
          var physKey = Types.ConstantData.PhysicalKeyInfo[key.KeyboardKey];

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
                byte  origChar = (byte)( m_TextEntryCachedLine[i] & 0xff );
                byte  origColor = (byte)( m_TextEntryCachedLine[i] >> 8 );
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
              if ( m_TextEntryEnteredText.Count >= 40 )
              {
                ++m_SelectedChar.Y;
                if ( m_SelectedChar.Y >= 24 )
                {
                  m_SelectedChar.Y = 0;
                }
                m_TextEntryStartedInLine = m_SelectedChar.Y;
                m_TextEntryEnteredText.Clear();
                CacheScreenLine( m_TextEntryStartedInLine );
              }
              m_TextEntryEnteredText.Add( (ushort)( charIndex | ( m_CurrentColor << 8 ) ) );
            }
            else
            {
              DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenCharChange( m_CharsetScreen, this, charX, charY, 1, 1 ) );
              if ( m_SelectedChar.X >= 39 )
              {
                m_SelectedChar.X = 0;
                ++m_SelectedChar.Y;
                if ( m_SelectedChar.Y >= 24 )
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
                byte  origChar = (byte)( m_TextEntryEnteredText[i] & 0xff );
                byte  origColor = (byte)( m_TextEntryEnteredText[i] >> 8 );
                SetCharacter( newX + i, m_SelectedChar.Y, origChar, origColor );
              }
              pictureEditor.DisplayPage.DrawTo( m_Image,
                                                newX * 8, m_SelectedChar.Y * 8,
                                                ( newX - m_CharsetScreen.ScreenOffsetY ) * 8, ( m_SelectedChar.Y - m_CharsetScreen.ScreenOffsetY ) * 8,
                                                m_TextEntryCachedLine.Count * 8, 8 );
              /*
              SetCharacter( charX, charY, charIndex, m_CurrentColor );
              pictureEditor.DisplayPage.DrawTo( m_Image,
                                                charX * 8, charY * 8,
                                                ( charX - m_CharsetScreen.ScreenOffsetX ) * 8, ( charY - m_CharsetScreen.ScreenOffsetY ) * 8,
                                                8, 8 );*/
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
        && ( e.KeyCode == Keys.C ) )
        {
          CopyToClipboard();
        }
        if ( e.KeyCode == Keys.Escape )
        {
          if ( m_FloatingSelection != null )
          {
            m_FloatingSelection = null;
            Redraw();
          }
        }
      }
      if ( ( e.Modifiers == Keys.Control )
      && ( e.KeyCode == Keys.V ) )
      {
        PasteFromClipboard();
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
                         (byte)( m_CharsetScreen.Chars[x + y * m_CharsetScreen.ScreenWidth] & 0xff ),
                         (byte)( m_CharsetScreen.Chars[x + y * m_CharsetScreen.ScreenWidth] >> 8 ) );
        }
      }
      pictureEditor.DisplayPage.DrawTo( m_Image,
                                        X * 8, Y * 8,
                                        ( X - m_CharsetScreen.ScreenOffsetX ) * 8, ( Y - m_CharsetScreen.ScreenOffsetY ) * 8,
                                        Width * 8, Height * 8 );
      pictureEditor.Invalidate( new System.Drawing.Rectangle( X * 8, Y * 8, Width * 8, Height * 8 ) );
    }



    public void ValuesChanged()
    {
      comboBackground.SelectedIndex = m_CharsetScreen.BackgroundColor;
      comboMulticolor1.SelectedIndex = m_CharsetScreen.MultiColor1;
      comboMulticolor2.SelectedIndex = m_CharsetScreen.MultiColor2;
      comboCharsetMode.SelectedIndex = (int)m_CharsetScreen.Mode;
      comboBGColor4.SelectedIndex = m_CharsetScreen.BGColor4;

      m_CharsetScreen.CharSet.MultiColor1 = m_CharsetScreen.MultiColor1;
      m_CharsetScreen.CharSet.MultiColor2 = m_CharsetScreen.MultiColor2;
      m_CharsetScreen.CharSet.BackgroundColor = m_CharsetScreen.BackgroundColor;
      m_CharsetScreen.CharSet.BGColor4 = m_CharsetScreen.BGColor4;
      for ( int i = 0; i < 256; ++i )
      {
        m_CharsetScreen.CharSet.Characters[i].Mode = m_CharsetScreen.Mode;
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
      if ( m_CharsetScreen.Mode != (C64Studio.Types.CharsetMode)comboCharsetMode.SelectedIndex )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenValuesChange( m_CharsetScreen, this ) );
        m_CharsetScreen.Mode = (C64Studio.Types.CharsetMode)comboCharsetMode.SelectedIndex;

        for ( int i = 0; i < 256; ++i )
        {
          m_CharsetScreen.CharSet.Characters[i].Mode = m_CharsetScreen.Mode;
          RebuildCharImage( i );
        }
        Modified = true;
        panelCharacters.Invalidate();
        charEditor.CharsetUpdated( m_CharsetScreen.CharSet );
        RedrawColorChooser();
        RedrawFullScreen();
      }

      if ( m_OverrideCharMode )
      {
        RebuildCharPanelImages();
      }

      switch ( m_CharsetScreen.Mode )
      {
        case C64Studio.Types.CharsetMode.HIRES:
          labelMColor1.Enabled = false;
          labelMColor2.Enabled = false;
          labelBGColor4.Enabled = false;
          comboMulticolor1.Enabled = false;
          comboMulticolor2.Enabled = false;
          comboBGColor4.Enabled = false;
          break;
        case C64Studio.Types.CharsetMode.MULTICOLOR:
          labelMColor1.Enabled = true;
          labelMColor1.Text = "Multicolor 1";
          labelMColor2.Enabled = true;
          labelMColor2.Text = "Multicolor 2";
          labelBGColor4.Enabled = false;
          comboMulticolor1.Enabled = true;
          comboMulticolor2.Enabled = true;
          comboBGColor4.Enabled = false;
          break;
        case C64Studio.Types.CharsetMode.ECM:
          labelMColor1.Enabled = true;
          labelMColor1.Text = "BGColor 2";
          labelMColor2.Enabled = true;
          labelMColor2.Text = "BGColor 3";
          labelBGColor4.Enabled = true;
          comboMulticolor1.Enabled = true;
          comboMulticolor2.Enabled = true;
          comboBGColor4.Enabled = true;
          break;
      }
    }



    private void comboBGColor4_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( m_CharsetScreen.BGColor4 != comboBGColor4.SelectedIndex )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenValuesChange( m_CharsetScreen, this ) );

        m_CharsetScreen.BGColor4 = comboBGColor4.SelectedIndex;
        m_CharsetScreen.CharSet.BGColor4 = m_CharsetScreen.BGColor4;
        for ( int i = 0; i < 256; ++i )
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

      //m_CharsetScreen.ExternalCharset.CopyTo

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
          m_CharsetScreen.CharSet.Characters[i].Data.SetU8At( j, Types.ConstantData.UpperCaseCharset.ByteAt( i * 8 + j ) );
        }
        m_CharsetScreen.CharSet.Characters[i].Mode = C64Studio.Types.CharsetMode.HIRES;
        m_CharsetScreen.CharSet.Characters[i].Color = 1;
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
          m_CharsetScreen.CharSet.Characters[i].Data.SetU8At( j, Types.ConstantData.LowerCaseCharset.ByteAt( i * 8 + j ) );
        }
        m_CharsetScreen.CharSet.Characters[i].Mode = C64Studio.Types.CharsetMode.HIRES;
        m_CharsetScreen.CharSet.Characters[i].Color = 1;
      }
      Modified = true;
      CharsetChanged();
    }



    internal void CharsetChanged()
    {
      for ( int i = 0; i < 256; ++i )
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
        checkApplyCharacter.Image = global::C64Studio.Properties.Resources.charscreen_chars;
      }
      else
      {
        checkApplyCharacter.Image = global::C64Studio.Properties.Resources.charscreen_chars_off.ToBitmap();
      }
    }



    private void checkApplyColors_CheckedChanged( object sender, EventArgs e )
    {
      m_AffectColors = checkApplyColors.Checked;
      if ( m_AffectColors )
      {
        checkApplyColors.Image = global::C64Studio.Properties.Resources.charscreen_colors;
      }
      else
      {
        checkApplyColors.Image = global::C64Studio.Properties.Resources.charscreen_colors_off.ToBitmap();
      }
    }



    private void checkShowGrid_CheckedChanged( object sender, EventArgs e )
    {
      m_ShowGrid = checkShowGrid.Checked;
      pictureEditor.Invalidate();
    }



    private void btnImportFromASM_Click( object sender, EventArgs e )
    {
      Parser.ASMFileParser asmParser = new C64Studio.Parser.ASMFileParser();

      Parser.CompileConfig config = new Parser.CompileConfig();
      config.TargetType = Types.CompileTargetType.PLAIN;
      config.OutputFile = "temp.bin";
      config.Assembler = Types.AssemblerType.C64_STUDIO;

      string    temp = "* = $0801\n" + editDataImport.Text;
      if ( ( asmParser.Parse( temp, null, config, null ) )
      && ( asmParser.Assemble( config ) ) )
      {
        GR.Memory.ByteBuffer data = asmParser.AssembledOutput.Assembly;
        ImportFromData( data );
      }
    }



    private void btnClearImportData_Click( object sender, EventArgs e )
    {
      editDataImport.Text = "";
    }



    private void editDataExport_KeyPress( object sender, KeyPressEventArgs e )
    {
      if ( ( System.Windows.Forms.Control.ModifierKeys == Keys.Control )
      && ( e.KeyChar == 1 ) )
      {
        editDataExport.SelectAll();
        e.Handled = true;
      }
    }



    private void editDataImport_KeyPress( object sender, KeyPressEventArgs e )
    {
      if ( ( System.Windows.Forms.Control.ModifierKeys == Keys.Control )
      && ( e.KeyChar == 1 ) )
      {
        editDataImport.SelectAll();
        e.Handled = true;
      }
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
          m_CharsetScreen.Chars[bufferIndex] = (ushort)( CharData.ByteAt( bufferIndex ) + ( ColorData.ByteAt( bufferIndex ) << 8 ) );
        }
      }

      ByteBuffer    CharsetProject = Charset.SaveToBuffer();
      m_CharsetScreen.CharSet.ReadFromBuffer( CharsetProject );

      comboBackground.SelectedIndex = m_CharsetScreen.CharSet.BackgroundColor;
      comboMulticolor1.SelectedIndex = m_CharsetScreen.CharSet.MultiColor1;
      comboMulticolor2.SelectedIndex = m_CharsetScreen.CharSet.MultiColor2;
      comboCharsetMode.SelectedIndex = (int)m_CharsetScreen.Mode;
      comboBGColor4.SelectedIndex = m_CharsetScreen.CharSet.BGColor4;
      editScreenWidth.Text = m_CharsetScreen.ScreenWidth.ToString();
      editScreenHeight.Text = m_CharsetScreen.ScreenHeight.ToString();

      for ( int i = 0; i < m_CharsetScreen.CharSet.NumCharacters; ++i )
      {
        RebuildCharImage( i );
      }
      RedrawFullScreen();
      RedrawColorChooser();
      Modified = true;
    }



    /*
    private void btnMoveSelectionToTarget_Click( object sender, EventArgs e )
    {
      int targetIndex = GR.Convert.ToI32( editMoveTargetIndex.Text );

      var selection = panelCharsetDetails.SelectedIndices;
      if ( selection.Count == 0 )
      {
        return;
      }
      if ( targetIndex + selection.Count > 256 )
      {
        MessageBox.Show( "Not enough chars for selection starting at the given index!", "Can't move selection" );
        return;
      }

      int[]   charMapNewToOld = new int[256];
      int[]   charMapOldToNew = new int[256];
      for ( int i = 0; i < 256; ++i )
      {
        charMapNewToOld[i] = -1;
        charMapOldToNew[i] = -1;
      }

      int     insertIndex = targetIndex;
      foreach ( var entry in selection )
      {
        charMapNewToOld[insertIndex] = entry;
        charMapOldToNew[entry] = insertIndex;
        ++insertIndex;
      }

      // now fill all other entries
      byte    insertCharIndex = 0;
      int     charPos = 0;
      while ( charPos < 256 )
      {
        // already inserted, skip
        if ( charMapNewToOld[charPos] != -1 )
        {
          ++charPos;
          continue;
        }
        while ( selection.Contains( insertCharIndex ) )
        {
          ++insertCharIndex;
        }
        charMapNewToOld[charPos] = insertCharIndex;
        charMapOldToNew[insertCharIndex] = charPos;
        ++charPos;
        ++insertCharIndex;
      }

      // TODO - undo!
      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenCharChange( m_CharsetScreen, this, 0, 0, m_CharsetScreen.ScreenWidth, m_CharsetScreen.ScreenHeight ) );
      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenCharsetChange( m_CharsetScreen, this ), false );


      // now shift all characters
      for ( int j = 0; j < m_CharsetScreen.ScreenHeight; ++j )
      {
        for ( int i = 0; i < m_CharsetScreen.ScreenWidth; ++i )
        {
          ushort    origChar = m_CharsetScreen.Chars[i + j * m_CharsetScreen.ScreenWidth];
          m_CharsetScreen.Chars[i + j * m_CharsetScreen.ScreenWidth] = (ushort)( charMapOldToNew[origChar & 0xff] | ( origChar & 0xff00 ) );
        }
      }

      // ..and charset
      List<CharData>    origCharData = new List<CharData>();
      List<GR.Forms.ImageListbox.ImageListItem>    origListItems = new List<GR.Forms.ImageListbox.ImageListItem>();
      List<GR.Forms.ImageListbox.ImageListItem>    origListItems2 = new List<GR.Forms.ImageListbox.ImageListItem>();

      for ( int i = 0; i < 256; ++i )
      {
        origCharData.Add( m_CharsetScreen.CharSet.Characters[i] );
        origListItems.Add( panelCharacters.Items[i] );
        origListItems2.Add( panelCharsetDetails.Items[i] );
      }

      for ( int i = 0; i < 256; ++i )
      {
        m_CharsetScreen.CharSet.Characters[i]  = origCharData[charMapNewToOld[i]];
        panelCharacters.Items[i]               = origListItems[charMapNewToOld[i]];
        panelCharsetDetails.Items[i]           = origListItems2[charMapNewToOld[i]];
      }
      panelCharacters.Invalidate();
      panelCharsetDetails.Invalidate();

      RedrawFullScreen();
      RedrawColorChooser();
      Modified = true;
    }
    */



    private void checkOverrideMode_CheckedChanged( object sender, EventArgs e )
    {
      m_OverrideCharMode = checkOverrideOriginalColorSettings.Checked;

      RebuildCharPanelImages();
    }



    private void RebuildCharPanelImages()
    {
      for ( int i = 0; i < 256; ++i )
      {
        RebuildCharImage( i );

        panelCharacters.Items[i].MemoryImage = m_CharsetScreen.CharSet.Characters[i].Image;
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

        checkAutoCenter.Image = global::C64Studio.Properties.Resources.charscreen_autocenter;
      }
      else
      {
        checkAutoCenter.Image = global::C64Studio.Properties.Resources.charscreen_autocenter_off;
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
        checkReverse.Image = global::C64Studio.Properties.Resources.charscreen_reverse_on;
      }
      else
      {
        checkReverse.Image = global::C64Studio.Properties.Resources.charscreen_reverse_off;
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
      if ( saveDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
      {
        return;
      }

      int     neededWidth   = m_CharsetScreen.ScreenWidth * 8;
      int     neededHeight  = m_CharsetScreen.ScreenHeight * 8;

      GR.Image.MemoryImage targetImg = new GR.Image.MemoryImage( neededWidth, neededHeight, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );

      CustomRenderer.PaletteManager.ApplyPalette( targetImg );

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
          ushort    origChar = m_CharsetScreen.Chars[i + j * m_CharsetScreen.ScreenWidth];
          m_CharsetScreen.Chars[i + j * m_CharsetScreen.ScreenWidth] = (ushort)( OldToNew[origChar & 0xff] | ( origChar & 0xff00 ) );
        }
      }

      // ..and charset
      List<GR.Forms.ImageListbox.ImageListItem>    origListItems = new List<GR.Forms.ImageListbox.ImageListItem>();

      for ( int i = 0; i < 256; ++i )
      {
        origListItems.Add( panelCharacters.Items[i] );
      }

      for ( int i = 0; i < 256; ++i )
      {
        panelCharacters.Items[i]              = origListItems[NewToOld[i]];
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

      for ( int i = 0; i < 256; ++i )
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

      GR.Image.MemoryImage targetImg = new GR.Image.MemoryImage( neededWidth, neededHeight, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );

      CustomRenderer.PaletteManager.ApplyPalette( targetImg );

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
        ushort  oldChar = m_CharsetScreen.Chars[0 + j * m_CharsetScreen.ScreenWidth];
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
        ushort  oldChar = m_CharsetScreen.Chars[m_CharsetScreen.ScreenWidth - 1 + j * m_CharsetScreen.ScreenWidth];
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
        ushort  oldChar = m_CharsetScreen.Chars[i];
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
        ushort  oldChar = m_CharsetScreen.Chars[i + ( m_CharsetScreen.ScreenHeight - 1 ) * m_CharsetScreen.ScreenWidth];
        for ( int j = m_CharsetScreen.ScreenHeight - 1; j >= 1; --j )
        {
          m_CharsetScreen.Chars[i + j * m_CharsetScreen.ScreenWidth] = m_CharsetScreen.Chars[i + ( j - 1 ) * m_CharsetScreen.ScreenWidth];
        }
        m_CharsetScreen.Chars[i] = oldChar;
      }
      SetModified();
      RedrawFullScreen();
    }



  } 
}
