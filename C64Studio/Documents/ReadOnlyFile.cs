using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using WeifenLuo.WinFormsUI.Docking;

namespace RetroDevStudio.Documents
{
  public partial class ReadOnlyFile : BaseDocument
  {
    int m_CurrentMarkedLineIndex = -1;



    public ReadOnlyFile()
    {
      InitializeComponent();
      editText.ReadOnly = true;
    }



    public ReadOnlyFile( StudioCore Core )
    {
      this.Core = Core;
      InitializeComponent();
      editText.ReadOnly = true;
    }


    public void SetText( string Text )
    {
      using ( var readOnlyBypass = new ReadOnlyBypass( editText ) )
      {
        editText.Text = Text;
      }
    }



    public void AppendText( string Text )
    {
      using ( var readOnlyBypass = new ReadOnlyBypass( editText ) )
      {
        editText.AppendText( Text );
      }
      editText.Selection.Start = new FastColoredTextBoxNS.Place( 0, editText.Lines.Count - 1 );
      editText.Navigate( editText.Selection.Start.iLine );
    }



    public override void SetLineMarked( int Line, bool Set )
    {
      if ( m_CurrentMarkedLineIndex != -1 )
      {
        editText[m_CurrentMarkedLineIndex].BackgroundBrush = null;
        m_CurrentMarkedLineIndex = -1;
      }
      if ( Set )
      {
        m_CurrentMarkedLineIndex = Line;
        editText[m_CurrentMarkedLineIndex].BackgroundBrush = new System.Drawing.SolidBrush( System.Drawing.Color.FromArgb( (int)Core.Settings.BGColor( Types.ColorableElement.CURRENT_DEBUG_LINE ) ) );
      }
    }



    public override void SetCursorToLine( int Line, int CharIndex, bool SetFocus )
    {
      if ( SetFocus )
      {
        editText.Focus();
      }

      editText.Navigate( Line, CharIndex );
    }

  }

  public class ReadOnlyBypass : IDisposable
  {
    private FastColoredTextBoxNS.FastColoredTextBox control;
    private bool originalIsReadOnly;

    public ReadOnlyBypass( FastColoredTextBoxNS.FastColoredTextBox control )
    {
      this.control = control;
      this.originalIsReadOnly = control.ReadOnly;
      control.ReadOnly = false;
    }

    void IDisposable.Dispose()
    {
      control.ReadOnly = originalIsReadOnly;
    }
  }

}
