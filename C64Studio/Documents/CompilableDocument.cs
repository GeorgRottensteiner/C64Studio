using C64Studio.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace C64Studio
{
  public class CompilableDocument : BaseDocument
  {
    public virtual FastColoredTextBoxNS.FastColoredTextBox SourceControl
    {
      get;
    }


    public void CenterOnCaret()
    {
      // automatically centers
      SourceControl.DoSelectionVisible();
    }



    public void MarkTextAsError( int LineIndex, int CharPosStart, int CharLength )
    {
      if ( ( LineIndex < 0 )
      ||   ( LineIndex >= SourceControl.LinesCount ) )
      {
        Debug.Log( "MarkTextAsError lineindex out of bounds!" );
        return;
      }
      int     startPos = CharPosStart;
      if ( SourceControl.AllowTabs )
      {
        // adjust offset in case of tabs (butt ugly hackaround)
        string origText = SourceControl[LineIndex].Text;

        if ( CharPosStart < origText.Length )
        {
          origText = SourceControl[LineIndex].Text.Substring( 0, CharPosStart );
        }
        origText = SourceControl.ReTabifyLine( origText, SourceControl.TabLength );

        startPos = origText.Length;

        if ( ( startPos >= origText.Length )
        || ( startPos + CharLength > origText.Length ) )
        {
          startPos = 0;
          CharLength = origText.Length;
        }

        //startPos = rng.AdjustXPosForTabs( LineIndex, startPos );
      }

      var range = new FastColoredTextBoxNS.Range( SourceControl, new FastColoredTextBoxNS.Place( startPos, LineIndex ), new FastColoredTextBoxNS.Place( startPos + CharLength, LineIndex ) );

      range.SetStyle( FastColoredTextBoxNS.StyleIndex.Style10 );
    }



    internal void RemoveAllErrorMarkings()
    {
      //TODO das macht schon wieder die folding markers weg!!
      SourceControl.ClearStyleWithoutAffectingFoldingMarkers( FastColoredTextBoxNS.StyleIndex.Style10 );
    }




  }
}
