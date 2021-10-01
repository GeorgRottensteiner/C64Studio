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
    protected List<Types.ASM.LineInfo>                  m_LineInfos = new List<Types.ASM.LineInfo>();



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
      // several warnings/errors in one line, mark the full line
      if ( ( LineIndex < m_LineInfos.Count )
      &&   ( m_LineInfos[LineIndex].HasCollapsedContent ) )
      {
        string    lineText = SourceControl[LineIndex].Text;

        // find first non white space
        CharPosStart = 0;
        while ( ( CharPosStart < lineText.Length )
        &&      ( ( lineText[CharPosStart] == ' ' )
        ||        ( lineText[CharPosStart] == '\t' ) ) )
        {
          ++CharPosStart;
        }
        CharLength = lineText.TrimEnd().Length - CharPosStart;
      }

      int     startPos = SourceControl.VirtualPositionToPositionInLine( LineIndex, CharPosStart );

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
