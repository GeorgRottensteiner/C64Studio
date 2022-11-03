using System;
using System.Data;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;



namespace RetroDevStudio.Controls
{
  public partial class EditC64Filename : Control
  {
    [DllImport( "user32.dll", EntryPoint = "ShowCaret", ExactSpelling = true )]
    static extern UInt32 ShowCaret( IntPtr Handle );

    [DllImport( "user32.dll", EntryPoint = "CreateCaret", ExactSpelling = true )]
    static extern UInt32 CreateCaret( IntPtr Handle, IntPtr Bitmap, int Width, int Height );

    [DllImport( "user32.dll", EntryPoint = "DestroyCaret", ExactSpelling = true )]
    static extern UInt32 DestroyCaret();

    [DllImport( "user32.dll", EntryPoint = "SetCaretPos", ExactSpelling = true )]
    static extern UInt32 SetCaretPos( int X, int Y );

    private System.Drawing.Font fixedFont = new System.Drawing.Font( "Courier New", 9 );
    private int m_CursorPos = 0;
    private int m_SelectionStartPos = -1;
    private int m_SelectionEndPos = -1;
    private int maxCharacters = 5;
    private int letterWidth = 14;
    private const char    EMPTY_CHAR = '◄';   // replaces 0xa0



    public EditC64Filename()
    {
      InitializeComponent();
      this.Font = fixedFont;
      this.DoubleBuffered = true;
      this.TabStop = true;
    }



    public int MaxLength
    {
      get
      {
        return maxCharacters;
      }
      set
      {
        maxCharacters = value;
        if ( maxCharacters >= 0 )
        {
          Width = maxCharacters * letterWidth;
        }
      }
    }



    public int LetterWidth
    {
      get
      {
        return letterWidth;
      }
      set
      {
        letterWidth = value;
      }
    }



    public int CursorPos
    {
      get
      {
        return m_CursorPos;
      }
      set
      {
        int   cursorPos = value;
        if ( cursorPos >= Text.Length )
        {
          cursorPos = Text.Length;
        }
        m_CursorPos = cursorPos;
        SetCaretPos( m_CursorPos * letterWidth, 0 );
        ClearSelection();
      }
    }



    public char CurrentChar
    {
      get
      {
        if ( ( m_CursorPos < 0 )
        ||   ( m_CursorPos >= Text.Length ) )
        {
          return ' ';
        }
        return Text[m_CursorPos];
      }
      set
      {
        if ( ( m_CursorPos < 0 )
        ||   ( m_CursorPos >= Text.Length ) )
        {
          return;
        }
        string NewText = Text.Substring( 0, m_CursorPos ) + value + Text.Substring( m_CursorPos + 1 );

        NewText = NewText.TrimEnd( (char)160 );
        NewText = NewText.PadRight( MaxLength, EMPTY_CHAR );
        base.Text = NewText;
        Invalidate();
      }
    }



    public bool HasSelection
    {
      get
      {
        if ( ( m_SelectionStartPos != -1 )
        &&   ( m_SelectionEndPos != -1 ) )
        {
          return true;
        }
        return false;
      }
    }



    public string Selection
    {
      get
      {
        if ( !HasSelection )
        {
          return "";
        }
        int Start = m_SelectionStartPos;
        int End = m_SelectionEndPos;
        if ( End < Start )
        {
          Start = m_SelectionEndPos;
          End = m_SelectionStartPos;
        }
        string Select = Text.Substring( Start, End - Start );
        return Select;
      }
      set
      {
        ReplaceSelection( value );
      }
    }



    public override string Text
    {
      get
      {
        return base.Text.Replace( EMPTY_CHAR, (char)0xa0 );
      }
      set
      {
        string NewText = value.Replace( (char)0xa0, EMPTY_CHAR );
        if ( NewText == null )
        {
          NewText = "";
        }
        NewText = NewText.PadRight( this.MaxLength, EMPTY_CHAR );
        if ( NewText.Length > this.MaxLength )
        {
          NewText = NewText.Substring( 0, this.MaxLength );
        }
        base.Text = NewText;
        Invalidate();
      }
    }



    protected override void OnPaint( PaintEventArgs pe )
    {
      // TODO - eigentlich soll schon das ClipRectangle benutzt werden, aber dann muss auch mit dem Offset umgegangen werden!
      System.Drawing.Rectangle drawRect = new Rectangle( 0, 0, Width, Height );

      // drawRect = pe.ClipRectangle;

      if ( !Enabled )
      {
        pe.Graphics.FillRectangle( new SolidBrush( SystemColors.ButtonFace ), pe.ClipRectangle );
        return;
      }
      // OnPaint-Basisklasse wird aufgerufen
      //base.OnPaint( pe );

      Rectangle rect = new Rectangle( drawRect.Location, drawRect.Size );
      if ( ( rect.Width % letterWidth ) > 0 )
      {
        rect.Width -= ( rect.Width % letterWidth );
      }
      pe.Graphics.FillRectangle( new SolidBrush( SystemColors.Window ), pe.ClipRectangle );
      if ( HasSelection )
      {
        int  L,R,T,B;

        T = 0;
        B = this.Height - 1;
        if ( m_SelectionStartPos > m_SelectionEndPos )
        {
          L = m_SelectionEndPos * letterWidth;
          R = m_SelectionStartPos * letterWidth - 1;
        }
        else
        {
          L = m_SelectionStartPos * letterWidth;
          R = m_SelectionEndPos * letterWidth - 1;
        }
        Rectangle selRect = new Rectangle( L, T, R - L, B - T );
        pe.Graphics.FillRectangle( new SolidBrush( SystemColors.Highlight ), selRect );
      }
      StringFormat sf = new StringFormat();
      sf.LineAlignment = StringAlignment.Center;
      sf.Alignment = StringAlignment.Center;

      for ( int i = 0; i < maxCharacters; ++i )
      {
        pe.Graphics.DrawRectangle( new Pen( SystemColors.ControlLight ), new Rectangle( i * letterWidth, 0, letterWidth - 1, this.Height - 1 ) );
        if ( i < Text.Length )
        {
          RectangleF  letterBounds = new RectangleF( (float)i * letterWidth, 0.0f, (float)letterWidth, (float)Height );

          if ( HasSelection )
          {
            if ( ( m_SelectionStartPos > m_SelectionEndPos )
            &&   ( i >= m_SelectionEndPos )
            &&   ( i < m_SelectionStartPos ) )
            {
              pe.Graphics.DrawString( base.Text.Substring( i, 1 ), this.Font, new SolidBrush( SystemColors.HighlightText ), letterBounds, sf );
            }
            else if ( ( i >= m_SelectionStartPos )
            &&        ( i < m_SelectionEndPos ) )
            {
              pe.Graphics.DrawString( base.Text.Substring( i, 1 ), this.Font, new SolidBrush( SystemColors.HighlightText ), letterBounds, sf );
            }
            else
            {
              pe.Graphics.DrawString( base.Text.Substring( i, 1 ), this.Font, new SolidBrush( SystemColors.WindowText ), letterBounds, sf );
            }
          }
          else
          {
            pe.Graphics.DrawString( base.Text.Substring( i, 1 ), this.Font, new SolidBrush( SystemColors.WindowText ), letterBounds, sf );
          }
        }
      }
      if ( Focused )
      {
        System.Windows.Forms.ControlPaint.DrawFocusRectangle( pe.Graphics, new Rectangle( 0, 0, rect.Width, rect.Height ) );
      }
    }



    private void FormTextBox_SizeChanged( object sender, EventArgs e )
    {
      if ( maxCharacters == -1 )
      {
        maxCharacters = this.Size.Width / letterWidth;
      }
      else
      {
        Width = maxCharacters * letterWidth;
      }
    }



    private void FormTextBox_Enter( object sender, EventArgs e )
    {
      CreateCaret( this.Handle, System.IntPtr.Zero, letterWidth, this.Height );
      Invalidate();
      SetCaretPos( m_CursorPos * letterWidth, 0 );
      ShowCaret( this.Handle );
    }



    private void FormTextBox_Leave( object sender, EventArgs e )
    {
      DestroyCaret();
      Invalidate();
    }

    private void FormTextBox_MouseClick( object sender, MouseEventArgs e )
    {
      Focus();
    }

    public void ClearSelection()
    {
      if ( HasSelection )
      {
        m_SelectionStartPos = -1;
        m_SelectionEndPos = -1;
        Invalidate();
      }
    }

    private void FormTextBox_KeyDown( object sender, KeyEventArgs e )
    {
      switch ( e.KeyCode )
      {
        case Keys.Insert:
          if ( e.Control )
          {
            // copy
            if ( HasSelection )
            {
              Clipboard.SetText( Selection );
            }
          }
          else if ( e.Shift )
          {
            // paste
            if ( Clipboard.ContainsText() )
            {
              if ( HasSelection )
              {
                ReplaceSelection( Clipboard.GetText() );
              }
              else
              {
                string ClipText = Clipboard.GetText();
                for ( int i = 0; i < ClipText.Length; ++i )
                {
                  FormTextBox_KeyPress( this, new KeyPressEventArgs( ClipText[i] ) );
                }
              }
            }
          }
          break;
        case Keys.C:
          if ( e.Control )
          {
            if ( HasSelection )
            {
              Clipboard.SetText( Selection );
            }
          }
          break;
        case Keys.X:
          if ( e.Control )
          {
            if ( HasSelection )
            {
              Clipboard.SetText( Selection );
              Selection = "";
            }
          }
          break;
        case Keys.V:
          if ( e.Control )
          {
            if ( Clipboard.ContainsText() )
            {
              if ( HasSelection )
              {
                ReplaceSelection( Clipboard.GetText() );
              }
              else
              {
                string ClipText = Clipboard.GetText();
                for ( int i = 0; i < ClipText.Length; ++i )
                {
                  FormTextBox_KeyPress( this, new KeyPressEventArgs( ClipText[i] ) );
                }
              }
            }
          }
          break;
        case Keys.Delete:
          if ( e.Shift )
          {
            if ( HasSelection )
            {
              Clipboard.SetText( Selection );
              Selection = "";
            }
          }
          else if ( HasSelection )
          {
            ReplaceSelection( "" );
          }
          else
          {
            if ( m_CursorPos < Text.Length )
            {
              Text = Text.Substring( 0, m_CursorPos ) + Text.Substring( m_CursorPos + 1 );
              Text = Text.PadRight( maxCharacters, EMPTY_CHAR );
              Invalidate();
            }
          }
          break;
        case Keys.Left:
          if ( e.Shift )
          {
            if ( m_SelectionStartPos == -1 )
            {
              m_SelectionStartPos = m_CursorPos;
            }
          }
          if ( m_CursorPos > 0 )
          {
            m_CursorPos--;
            SetCaretPos( m_CursorPos * letterWidth, 0 );
          }
          if ( e.Shift )
          {
            m_SelectionEndPos = m_CursorPos;
            Invalidate();
          }
          else
          {
            ClearSelection();
          }
          e.Handled = true;
          break;
        case Keys.Right:
          if ( e.Shift )
          {
            if ( m_SelectionStartPos == -1 )
            {
              m_SelectionStartPos = m_CursorPos;
            }
          }
          if ( m_CursorPos < Text.Length )
          {
            m_CursorPos++;
            SetCaretPos( m_CursorPos * letterWidth, 0 );
          }
          if ( e.Shift )
          {
            m_SelectionEndPos = m_CursorPos;
            Invalidate();
          }
          else
          {
            ClearSelection();
          }
          e.Handled = true;
          break;
        case Keys.Home:
          if ( e.Shift )
          {
            if ( m_SelectionStartPos == -1 )
            {
              m_SelectionStartPos = m_CursorPos;
            }
          }
          if ( m_CursorPos > 0 )
          {
            m_CursorPos = 0;
            SetCaretPos( m_CursorPos * letterWidth, 0 );
          }
          if ( e.Shift )
          {
            m_SelectionEndPos = m_CursorPos;
            Invalidate();
          }
          else
          {
            ClearSelection();
          }
          break;
        case Keys.End:
          if ( e.Shift )
          {
            if ( m_SelectionStartPos == -1 )
            {
              m_SelectionStartPos = m_CursorPos;
            }
          }
          if ( m_CursorPos < Text.Length )
          {
            m_CursorPos = Text.Length;
            SetCaretPos( m_CursorPos * letterWidth, 0 );
          }
          if ( e.Shift )
          {
            m_SelectionEndPos = m_CursorPos;
            Invalidate();
          }
          else
          {
            ClearSelection();
          }
          break;
      }
      if ( e.KeyCode == Keys.Tab )
      {
        e.Handled = false;
      }
    }



    public void ReplaceSelection( string Replacement )
    {
      if ( !HasSelection )
      {
        return;
      }
      string NewText = "";
      int   Start = m_SelectionStartPos;
      int   End = m_SelectionEndPos;
      if ( End < Start )
      {
        Start = m_SelectionEndPos;
        End = m_SelectionStartPos;
      }
      NewText = Text.Substring( 0, Start );
      NewText += Replacement;
      NewText += Text.Substring( End );
      NewText = NewText.PadRight( maxCharacters, EMPTY_CHAR );
      Text = NewText;
      m_CursorPos = Start;
      SetCaretPos( m_CursorPos * letterWidth, 0 );
      m_SelectionStartPos = -1;
      m_SelectionEndPos = -1;
      Invalidate();
    }



    private void FormTextBox_KeyPress( object sender, KeyPressEventArgs e )
    {
      if ( e.KeyChar == 8 )
      {
        if ( HasSelection )
        {
          ReplaceSelection( "" );
        }
        else if ( m_CursorPos > 0 )
        {
          string NewText = "";
          if ( m_CursorPos > 1 )
          {
            NewText = Text.Substring( 0, m_CursorPos - 1 );
          }
          NewText += Text.Substring( m_CursorPos );
          NewText = NewText.PadRight( maxCharacters, EMPTY_CHAR );
          Text = NewText;
          m_CursorPos--;
          SetCaretPos( m_CursorPos * letterWidth, 0 );
          Invalidate();
        }
      }
      else if ( m_CursorPos < maxCharacters )
      {
        if ( (int)e.KeyChar >= 32 )
        {
          if ( HasSelection )
          {
            ReplaceSelection( e.KeyChar.ToString() );
          }
          else
          {
            if ( m_CursorPos < Text.Length )
            {
              string NewText = "";
              if ( m_CursorPos > 0 )
              {
                NewText = Text.Substring( 0, m_CursorPos );
              }
              NewText += e.KeyChar + Text.Substring( m_CursorPos + 1 );
              Text = NewText;
              OnTextChanged( new EventArgs() );
            }
            m_CursorPos++;
            SetCaretPos( m_CursorPos * letterWidth, 0 );
            Invalidate();
          }
        }
      }
    }



    protected override void WndProc( ref Message msg )
    {
      if ( msg.Msg == 0x0087 )
      {
        //msg.Result = (IntPtr)0x0004;
        // want arrows and chars
        msg.Result = (IntPtr)0x0081;
        return;
      }
      else if ( msg.Msg == 0x0100 ) // WM_KEYDOWN
      {
        // Convert key code to a .NET Keys structure
        Keys keyData = ( (Keys)(int)msg.WParam ) | ModifierKeys;
        Keys keyCode = ( (Keys)(int)msg.WParam );

        FormTextBox_KeyDown( this, new KeyEventArgs( keyData ) );
      }
      base.WndProc( ref msg );
    }



    private void FormTextBox_MouseDown( object sender, MouseEventArgs e )
    {
      if ( ( e.X >= 0 )
      &&   ( e.X < maxCharacters * letterWidth ) )
      {
        int LetterOffset = e.X / letterWidth;
        if ( LetterOffset > Text.Length )
        {
          LetterOffset = Text.Length;
        }
        m_CursorPos = LetterOffset;
        SetCaretPos( m_CursorPos * letterWidth, 0 );
      }
    }

  }
}
