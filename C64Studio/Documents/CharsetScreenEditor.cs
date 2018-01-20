using C64Studio.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

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

    private GR.Image.MemoryImage        m_Image = new GR.Image.MemoryImage( 320, 200, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );

    private bool[,]                     m_ErrornousChars = new bool[40, 25];
    private bool[,]                     m_SelectedChars = new bool[40, 25];

    private System.Drawing.Point        m_SelectedChar = new System.Drawing.Point( -1, -1 );

    private Formats.CharsetScreenProject    m_CharsetScreen = new C64Studio.Formats.CharsetScreenProject();

    private ToolMode                    m_ToolMode = ToolMode.SINGLE_CHAR;

    private bool                        m_MouseButtonReleased = false;
    private System.Drawing.Point        m_MousePos;

    private System.Drawing.Point        m_DragStartPos = new System.Drawing.Point();
    private System.Drawing.Point        m_DragEndPos = new System.Drawing.Point();
    private System.Drawing.Point        m_LastDragEndPos = new System.Drawing.Point( -1, -1 );

    private List<GR.Generic.Tupel<bool,ushort>>       m_FloatingSelection = null;
    private System.Drawing.Size                       m_FloatingSelectionSize;
    private System.Drawing.Point                      m_FloatingSelectionPos;

    private bool                        m_AffectChars = true;
    private bool                        m_AffectColors = true;




    public CharsetScreenEditor( StudioCore Core )
    {
      this.Core = Core;
      DocumentInfo.Type = ProjectElement.ElementType.CHARACTER_SCREEN;
      DocumentInfo.UndoManager.MainForm = Core.MainForm;
      m_IsSaveable = true;
      InitializeComponent();

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
          comboBasicFiles.Items.Add( new Types.ComboItem( doc.Name, doc ) );
        }
        else if ( doc.DocumentInfo.Type == ProjectElement.ElementType.CHARACTER_SET )
        {
          comboCharsetFiles.Items.Add( new Types.ComboItem( doc.Name, doc ) );
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
    }



    void MainForm_ApplicationEvent( C64Studio.Types.ApplicationEvent Event )
    {
      if ( Event.EventType == C64Studio.Types.ApplicationEvent.Type.ELEMENT_CREATED )
      {
        if ( Event.Doc.Type == ProjectElement.ElementType.BASIC_SOURCE )
        {
          string    nameToUse = Event.Doc.DocumentFilename ?? "New File";
          comboBasicFiles.Items.Add( new Types.ComboItem( nameToUse, Event.Doc ) );
        }
        else if ( Event.Doc.Type == ProjectElement.ElementType.CHARACTER_SET )
        {
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

      Displayer.CharacterDisplayer.DisplayChar( m_CharsetScreen.CharSet, CharIndex, Char.Image, 0, 0 );
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

      e.DrawBackground();
      System.Drawing.Rectangle itemRect = new System.Drawing.Rectangle( e.Bounds.Left + 20, e.Bounds.Top, e.Bounds.Width - 20, e.Bounds.Height );
      if ( ( e.State & DrawItemState.Disabled ) != 0 )
      {
        e.Graphics.FillRectangle( System.Drawing.SystemBrushes.GrayText, itemRect );
        if ( e.Index >= 0 )
        {
          e.Graphics.DrawString( combo.Items[e.Index].ToString(), combo.Font, new System.Drawing.SolidBrush( System.Drawing.Color.Gray ), 3.0f, e.Bounds.Top + 1.0f );
        }
      }
      else if ( ( e.State & DrawItemState.Selected ) != 0 )
      {
        if ( e.Index >= 0 )
        {
          e.Graphics.FillRectangle( Types.ConstantData.Palette.ColorBrushes[e.Index], itemRect );
          e.Graphics.DrawString( combo.Items[e.Index].ToString(), combo.Font, new System.Drawing.SolidBrush( System.Drawing.Color.White ), 3.0f, e.Bounds.Top + 1.0f );
        }
      }
      else
      {
        if ( e.Index >= 0 )
        {
          e.Graphics.FillRectangle( Types.ConstantData.Palette.ColorBrushes[e.Index], itemRect );
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
      else if ( e.Index >= 0 )
      {
        e.Graphics.FillRectangle( Types.ConstantData.Palette.ColorBrushes[e.Index], itemRect );
      }
      if ( e.Index >= 0 )
      {
        if ( ( e.State & DrawItemState.Disabled ) != 0 )
        {
          e.Graphics.DrawString( combo.Items[e.Index].ToString(), combo.Font, new System.Drawing.SolidBrush( System.Drawing.Color.Gray ), 3.0f, e.Bounds.Top + 1.0f );
        }
        else if ( ( e.State & DrawItemState.Selected ) != 0 )
        {
          e.Graphics.DrawString( combo.Items[e.Index].ToString(), combo.Font, new System.Drawing.SolidBrush( System.Drawing.Color.White ), 3.0f, e.Bounds.Top + 1.0f );
        }
        else
        {
          e.Graphics.DrawString( combo.Items[e.Index].ToString(), combo.Font, new System.Drawing.SolidBrush( System.Drawing.Color.Black ), 3.0f, e.Bounds.Top + 1.0f );
        }
      }
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

      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenCharChange( m_CharsetScreen, this, m_MousePos.X, m_MousePos.Y, m_FloatingSelectionSize.Width, m_FloatingSelectionSize.Height ) );

      for ( int j = 0; j < m_FloatingSelectionSize.Height; ++j )
      {
        for ( int i = 0; i < m_FloatingSelectionSize.Width; ++i )
        {
          var selectionChar = m_FloatingSelection[i + j * m_FloatingSelectionSize.Width];
          if ( selectionChar.first )
          {
            m_CharsetScreen.Chars[m_MousePos.X + i + m_CharsetScreen.ScreenOffsetX + ( m_MousePos.Y + j + m_CharsetScreen.ScreenOffsetY ) * m_CharsetScreen.ScreenWidth] =
                    selectionChar.second;

            DrawCharImage( pictureEditor.DisplayPage,
               ( m_MousePos.X + i ) * 8,
               ( m_MousePos.Y + j ) * 8,
               (byte)( selectionChar.second & 0xff ),
               (byte)( selectionChar.second >> 8 ) );

            pictureEditor.DisplayPage.DrawTo( m_Image,
                                              ( m_CharsetScreen.ScreenOffsetX + m_MousePos.X + i ) * 8,
                                              ( m_CharsetScreen.ScreenOffsetY + m_MousePos.Y + j ) * 8,
                                              ( m_MousePos.X + i ) * 8,
                                              ( m_MousePos.Y + j ) * 8,
                                              8, 8 );
            pictureEditor.Invalidate( new System.Drawing.Rectangle( ( m_MousePos.X + i ) * 8,
                                                                    ( m_MousePos.Y + j ) * 8,
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
          &&   ( m_CharsetScreen.Chars[point.X - 1 + m_CharsetScreen.ScreenWidth * point.Y] == charToFill ) )
          {
            pointsToCheck.Add( new System.Drawing.Point( point.X - 1, point.Y ) );
          }
          if ( ( point.X + 1 < m_CharsetScreen.ScreenWidth )
          &&   ( m_CharsetScreen.Chars[point.X + 1 + m_CharsetScreen.ScreenWidth * point.Y] == charToFill ) )
          {
            pointsToCheck.Add( new System.Drawing.Point( point.X + 1, point.Y ) );
          }
          if ( ( point.Y > 0 )
          &&   ( m_CharsetScreen.Chars[point.X + m_CharsetScreen.ScreenWidth * ( point.Y - 1 )] == charToFill ) )
          {
            pointsToCheck.Add( new System.Drawing.Point( point.X, point.Y - 1 ) );
          }
          if ( ( point.Y + 1 < m_CharsetScreen.ScreenHeight )
          &&   ( m_CharsetScreen.Chars[point.X + m_CharsetScreen.ScreenWidth * ( point.Y + 1 )] == charToFill ) )
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
            }
            break;
          case ToolMode.SINGLE_CHAR:
            if ( m_CharsetScreen.Chars[charX + charY * m_CharsetScreen.ScreenWidth] != (ushort)( m_CurrentChar | ( m_CurrentColor << 8 ) ) )
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

              // first point
              m_DragStartPos.X = charX;
              m_DragStartPos.Y = charY;
              m_LastDragEndPos = new System.Drawing.Point( -1, -1 );
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
                                o1.X * 8, o1.Y * 8,
                                ( o1.X - m_CharsetScreen.ScreenOffsetX ) * 8, ( o1.Y - m_CharsetScreen.ScreenOffsetY ) * 8,
                                ( o2.X - o1.X + 1 ) * 8, ( o2.Y - o1.Y + 1 ) * 8 );

                pictureEditor.Invalidate( new System.Drawing.Rectangle( o1.X * 8, o1.Y * 8, ( o2.X - o1.X + 1 ) * 8, ( o2.Y - o1.Y + 1 ) * 8 ) );
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
                  DrawCharacter( p1.Y, y );
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
              pictureEditor.Invalidate( new System.Drawing.Rectangle( p1.X * 8, p1.Y * 8, ( p2.X - p1.X + 1 ) * 8, ( p2.Y - p1.Y + 1 ) * 8 ) );
            }

            break;
          case ToolMode.SELECT:
            if ( m_MouseButtonReleased )
            {
              m_MouseButtonReleased = false;

              // first point
              m_DragStartPos.X = charX;
              m_DragStartPos.Y = charY;
              m_LastDragEndPos = new System.Drawing.Point( -1, -1 );
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
                /*
                for ( int x = o1.X; x <= o2.X; ++x )
                {
                  for ( int y = o1.Y; y <= o2.Y; ++y )
                  {
                    m_SelectedChars[x, y] = false;
                  }
                }*/
              }
              m_LastDragEndPos = m_DragEndPos;

              System.Drawing.Point    p1, p2;

              CalcRect( m_DragStartPos, m_DragEndPos, out p1, out p2 );

              /*
              for ( int x = p1.X; x <= p2.X; ++x )
              {
                for ( int y = p1.Y; y <= p2.Y; ++y )
                {
                  m_SelectedChars[x, y] = true;
                }
              }*/

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
      pictureEditor.DisplayPage.Box( 0, 0, 320, 200, 16 );
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

      for ( int i = x1; i <= x2; ++i )
      {
        for ( int j = y1; j <= y2; ++j )
        {
          DrawCharImage( pictureEditor.DisplayPage, 
                         ( i - x1 ) * 8, 
                         ( j - y1 ) * 8, 
                         (byte)( m_CharsetScreen.Chars[i + j * m_CharsetScreen.ScreenWidth] & 0xff ), 
                         (byte)( m_CharsetScreen.Chars[i + j * m_CharsetScreen.ScreenWidth] >> 8 ) );
          pictureEditor.DisplayPage.DrawTo( m_Image,
                                            ( i - x1 ) * 8 + m_CharsetScreen.ScreenOffsetX, ( j - y1 ) * 8 + m_CharsetScreen.ScreenOffsetY,
                                            ( i - x1 ) * 8, ( j - y1 ) * 8,
                                            8, 8 );
        }
      }
      pictureEditor.Invalidate();
    }



    private void comboBackground_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( m_CharsetScreen.BackgroundColor != comboBackground.SelectedIndex )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenValuesChange( m_CharsetScreen, this ) );

        m_CharsetScreen.BackgroundColor = comboBackground.SelectedIndex;
        m_CharsetScreen.CharSet.BackgroundColor = m_CharsetScreen.BackgroundColor;
        for ( int i = 0; i < 256; ++i )
        {
          RebuildCharImage( i );
        }
        Modified = true;
        RedrawFullScreen();
        pictureEditor.Invalidate();
        panelCharacters.Invalidate();
      }
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

      RedrawColorChooser();
      RedrawFullScreen();
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



    private bool SaveProject( bool SaveAs )
    {
      string    saveFilename = DocumentInfo.FullPath;

      if ( ( String.IsNullOrEmpty( DocumentInfo.DocumentFilename ) )
      ||   ( SaveAs ) )
      {
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
        if ( SaveAs )
        {
          saveFilename = saveDlg.FileName;
        }
        else
        {
          SetDocumentFilename( saveDlg.FileName );
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


      GR.Memory.ByteBuffer projectFile = SaveToBuffer();

      if ( !GR.IO.File.WriteAllBytes( saveFilename, projectFile ) )
      {
        return false;
      }
      Modified = false;
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
      pictureEditor.DisplayPage.Box( 0, 0, 320, 200, 16 );
      pictureEditor.DisplayPage.DrawFromMemoryImage( m_Image, -m_CharsetScreen.ScreenOffsetX * 8, -m_CharsetScreen.ScreenOffsetY * 8 );

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
            if ( ( y - m_CharsetScreen.ScreenOffsetY == 0 )
            ||   ( !m_SelectedChars[x, y - 1] ) )
            {
              for ( int i = 0; i < 8; ++i )
              {
                pictureEditor.DisplayPage.SetPixel( ( x - m_CharsetScreen.ScreenOffsetX ) * 8 + i, 
                                                    ( y - m_CharsetScreen.ScreenOffsetY ) * 8, 
                                                    16 );
              }
            }
            if ( ( y - m_CharsetScreen.ScreenOffsetY == m_SelectedChars.GetUpperBound( 1 ) )
            ||   ( !m_SelectedChars[x, y + 1] ) )
            {
              for ( int i = 0; i < 8; ++i )
              {
                pictureEditor.DisplayPage.SetPixel( ( x - m_CharsetScreen.ScreenOffsetX ) * 8 + i,
                                                    ( y - m_CharsetScreen.ScreenOffsetY ) * 8 + 7,
                                                    16 );
              }
            }
            if ( ( x - m_CharsetScreen.ScreenOffsetX == 0 )
            ||   ( !m_SelectedChars[x - 1, y] ) )
            {
              for ( int i = 0; i < 8; ++i )
              {
                pictureEditor.DisplayPage.SetPixel( ( x - m_CharsetScreen.ScreenOffsetX ) * 8,
                                                    ( y - m_CharsetScreen.ScreenOffsetY ) * 8 + i,
                                                    16 );
              }
            }
            if ( ( x - m_CharsetScreen.ScreenOffsetX == m_SelectedChars.GetUpperBound( 0 ) )
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
      &&   ( m_LastDragEndPos.X != -1 ) )
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
      }
    }



    private void importCharsetToolStripMenuItem_Click( object sender, EventArgs e )
    {
      OpenExternalCharset();
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
              // no selection, select all
              return new System.Drawing.Rectangle( 0, 0, m_CharsetScreen.ScreenWidth, m_CharsetScreen.ScreenHeight );
            }
            return new System.Drawing.Rectangle( minX, minY, maxX - minX + 1, maxY - minY + 1 );
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

      sb.Append( "10 PRINT\"" + Types.ConstantData.PetSCIIToChar[147].CharValue + "\";\n" );
      sb.Append( "11 POKE53280," + m_CharsetScreen.BackgroundColor.ToString() + ":POKE53281," + m_CharsetScreen.BackgroundColor.ToString() + "\n" );

      System.Drawing.Rectangle    exportRect = DetermineExportRectangle();

      for ( int i = exportRect.Top; i < exportRect.Bottom; ++i )
      {
        sb.Append( 20 + i );
        sb.Append( " PRINT\"" );
        for ( int x = exportRect.Left; x < exportRect.Right; ++x )
        {
          byte newColor = (byte)( ( ( m_CharsetScreen.Chars[i * m_CharsetScreen.ScreenWidth + x] & 0xff00 ) >> 8 ) & 0x0f );
          byte newChar = (byte)( m_CharsetScreen.Chars[i * m_CharsetScreen.ScreenWidth + x] & 0xff );

          if ( newColor != curColor )
          {
            sb.Append( Types.ConstantData.PetSCIIToChar[Types.ConstantData.ColorToPetSCIIChar[newColor]].CharValue );
            curColor = newColor;
          }
          if ( newChar >= 128 )
          {
            if ( !isReverse )
            {
              isReverse = true;
              sb.Append( Types.ConstantData.PetSCIIToChar[18].CharValue );
            }
          }
          else if ( isReverse )
          {
            isReverse = false;
            sb.Append( Types.ConstantData.PetSCIIToChar[146].CharValue );
          }
          if ( isReverse )
          {
            sb.Append( Types.ConstantData.ScreenCodeToChar[(byte)( newChar - 128 )].CharValue );
          }
          else
          {
            if ( newChar == 34 )
            {
              // an apostrohpe!
              string    replacement = "\";CHR$(34);\"";

              for ( int t = 0; t < replacement.Length; ++t )
              {
                sb.Append( Types.ConstantData.CharToC64Char[replacement[t]].CharValue );
              }
            }
            else
            {
              sb.Append( Types.ConstantData.ScreenCodeToChar[newChar].CharValue );
            }
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
        document.FillContent( sb.ToString() );
        document.SetModified();
        document.Save();
      }
      else
      {
        BaseDocument document = (BaseDocument)comboItem.Tag;
        document.InsertText( sb.ToString() );
        document.SetModified();
      }
    }



    private void btnExportToData_Click( object sender, EventArgs e )
    {
      // prepare data
      GR.Memory.ByteBuffer screenCharData;
      GR.Memory.ByteBuffer screenColorData;
      GR.Memory.ByteBuffer charsetData;

      var exportRect = DetermineExportRectangle();

      m_CharsetScreen.ExportToBuffer( out screenCharData, out screenColorData, out charsetData, exportRect.Left, exportRect.Top, exportRect.Width, exportRect.Height, ( comboExportOrientation.SelectedIndex == 0 ) );

      string screenData = Util.ToASMData( screenCharData, checkExportToDataWrap.Checked, GR.Convert.ToI32( editWrapByteCount.Text ), editPrefix.Text, checkExportHex.Checked );
      string colorData = Util.ToASMData( screenColorData, checkExportToDataWrap.Checked, GR.Convert.ToI32( editWrapByteCount.Text ), editPrefix.Text, checkExportHex.Checked );

      switch ( comboExportData.SelectedIndex )
      {
        case 0:
          editDataExport.Text = ";screen char data" + System.Environment.NewLine + screenData + System.Environment.NewLine + ";screen color data" + System.Environment.NewLine + colorData;
          break;
        case 1:
          editDataExport.Text = ";screen char data" + System.Environment.NewLine + screenData + System.Environment.NewLine;
          break;
        case 2:
          editDataExport.Text = ";screen color data" + System.Environment.NewLine + colorData;
          break;
        case 3:
          editDataExport.Text = ";screen color data" + System.Environment.NewLine + colorData + System.Environment.NewLine + ";screen char data" + System.Environment.NewLine + screenData;
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

      if ( OpenFile( "Open binary data", C64Studio.Types.Constants.FILEFILTER_ALL, out filename ) )
      {
        GR.Memory.ByteBuffer data = GR.IO.File.ReadAllBytes( filename );

        if ( data.Length == 1000 )
        {
          SetScreenSize( 40, 25 );
        }

        if ( data.Length >= m_CharsetScreen.ScreenWidth * m_CharsetScreen.ScreenHeight )
        {
          // chars first
          for ( int j = 0; j < m_CharsetScreen.ScreenHeight; ++j )
          {
            for ( int i = 0; i < m_CharsetScreen.ScreenWidth; ++i )
            {
              m_CharsetScreen.Chars[i + j * m_CharsetScreen.ScreenWidth] = (ushort)( data.ByteAt( i + j * m_CharsetScreen.ScreenWidth ) | ( m_CharsetScreen.Chars[i + j * m_CharsetScreen.ScreenWidth] & 0xff00 ) );
            }
          }
        }
        if ( data.Length >= 2 * m_CharsetScreen.ScreenWidth * m_CharsetScreen.ScreenHeight )
        {
          // colors
          for ( int j = 0; j < m_CharsetScreen.ScreenHeight; ++j )
          {
            for ( int i = 0; i < m_CharsetScreen.ScreenWidth; ++i )
            {
              m_CharsetScreen.Chars[i + j * m_CharsetScreen.ScreenWidth] = (ushort)( ( data.ByteAt( m_CharsetScreen.ScreenWidth * m_CharsetScreen.ScreenHeight + i + j * m_CharsetScreen.ScreenWidth ) << 8 ) | ( m_CharsetScreen.Chars[i + j * m_CharsetScreen.ScreenWidth] & 0xff ) );
            }
          }
        }
        Modified = true;
        RedrawFullScreen();
      }
    }



    public void SetScreenSize( int Width, int Height )
    {
      m_ErrornousChars = new bool[Width, Height];
      m_SelectedChars = new bool[Width, Height];

      m_CharsetScreen.SetScreenSize( Width, Height );
      m_Image.Create( Width * 8, Height * 8, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );
      CustomRenderer.PaletteManager.ApplyPalette( m_Image );

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



    private void btnApplyScreenSize_Click( object sender, EventArgs e )
    {
      int     newWidth = GR.Convert.ToI32( editScreenWidth.Text );
      int     newHeight = GR.Convert.ToI32( editScreenHeight.Text );

      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenSizeChange( m_CharsetScreen, this, m_CharsetScreen.ScreenWidth, m_CharsetScreen.ScreenHeight ) );

      SetScreenSize( newWidth, newHeight );
    }



    public void InjectProjects( Formats.CharsetScreenProject CharScreen, Formats.CharsetProject CharSet )
    {
      m_CharsetScreen = CharScreen;
      m_CharsetScreen.CharSet = CharSet;

      comboBackground.SelectedIndex   = m_CharsetScreen.CharSet.BackgroundColor;
      comboMulticolor1.SelectedIndex  = m_CharsetScreen.CharSet.MultiColor1;
      comboMulticolor2.SelectedIndex  = m_CharsetScreen.CharSet.MultiColor2;
      comboCharsetMode.SelectedIndex  = (int)m_CharsetScreen.Mode;
      comboBGColor4.SelectedIndex     = m_CharsetScreen.CharSet.BGColor4;

      for ( int i = 0; i < m_CharsetScreen.CharSet.NumCharacters; ++i )
      {
        RebuildCharImage( i );

        panelCharacters.Items[i].MemoryImage = m_CharsetScreen.CharSet.Characters[i].Image;
      }

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
      m_ToolMode = ToolMode.SINGLE_CHAR;
    }



    private void btnToolRect_CheckedChanged( object sender, EventArgs e )
    {
      m_ToolMode = ToolMode.RECTANGLE;
    }



    private void btnToolFill_CheckedChanged( object sender, EventArgs e )
    {
      m_ToolMode = ToolMode.FILL;
    }



    private void btnToolSelect_CheckedChanged( object sender, EventArgs e )
    {
      m_ToolMode = ToolMode.SELECT;
    }



    private void btnToolQuad_CheckedChanged( object sender, EventArgs e )
    {
      m_ToolMode = ToolMode.FILLED_RECTANGLE;
    }



    private void btnToolText_CheckedChanged( object sender, EventArgs e )
    {
      m_ToolMode = ToolMode.TEXT;
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
            DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharscreenCharChange( m_CharsetScreen, this, charX, charY, 1, 1 ) );

            if ( m_SelectedChar.X >= 39 )
            {
              m_SelectedChar.X = 0;
              ++m_SelectedChar.Y;
              if ( m_SelectedChar.Y >= 24 )
              {
                m_SelectedChar.Y = 0;
              }
            }
            else
            {
              ++m_SelectedChar.X;
            }
            SetCharacter( charX, charY, charIndex, m_CurrentColor );
            pictureEditor.DisplayPage.DrawTo( m_Image,
                                              charX * 8, charY * 8,
                                              ( charX - m_CharsetScreen.ScreenOffsetX ) * 8, ( charY - m_CharsetScreen.ScreenOffsetY ) * 8,
                                              8, 8 );

            Redraw();
            Modified = true;
            pictureEditor.Invalidate( new System.Drawing.Rectangle( charX * 8, charY * 8, 8, 8 ) );
            pictureEditor.Invalidate( new System.Drawing.Rectangle( m_SelectedChar.X * 8, m_SelectedChar.Y * 8, 8, 8 ) );
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
      &&   ( e.KeyCode == Keys.V ) )
      {
        PasteFromClipboard();
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
      comboBackground.SelectedIndex   = m_CharsetScreen.BackgroundColor;
      comboMulticolor1.SelectedIndex  = m_CharsetScreen.MultiColor1;
      comboMulticolor2.SelectedIndex  = m_CharsetScreen.MultiColor2;
      comboCharsetMode.SelectedIndex  = (int)m_CharsetScreen.Mode;
      comboBGColor4.SelectedIndex     = m_CharsetScreen.BGColor4;

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
        RedrawColorChooser();
        RedrawFullScreen();
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
        ( (CharsetEditor)document).OpenProject( charSetData );
        document.SetModified();
        document.Save();
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


      var exportRect = DetermineExportRectangle();

      m_CharsetScreen.ExportToBuffer( out screenCharData, out screenColorData, out charsetData, exportRect.Left, exportRect.Top, exportRect.Width, exportRect.Height, ( comboExportOrientation.SelectedIndex == 0 ) );

      switch ( comboExportData.SelectedIndex )
      {
        case 0:
          editDataExport.Text = Util.ToBASICData( screenCharData + screenColorData, startLine, lineOffset );
          break;
        case 1:
          editDataExport.Text = Util.ToBASICData( screenCharData, startLine, lineOffset );
          break;
        case 2:
          editDataExport.Text = Util.ToBASICData( screenColorData, startLine, lineOffset );
          break;
        case 3:
          editDataExport.Text = Util.ToBASICData( screenColorData + screenCharData, startLine, lineOffset );
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



  }
}
