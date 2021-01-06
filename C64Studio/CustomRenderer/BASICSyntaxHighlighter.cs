using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C64Studio.CustomRenderer
{
  class BASICSyntaxHighlighter : FastColoredTextBoxNS.SyntaxHighlighter
  {
    static Parser.BasicFileParser     _Parser = new Parser.BasicFileParser( new Parser.BasicFileParser.ParserSettings() );
    static C64Models.BASIC.Dialect    _Dialect = null;



    public void SetBASICDialect( C64Models.BASIC.Dialect Dialect )
    {
      _Parser.Settings.BASICDialect = Dialect;
    }



    public override void HighlightSyntax( Language Language, Range ChangedRange )
    {
      if ( _Parser.Settings.BASICDialect == null )
      {
        _Dialect = C64Models.BASIC.Dialect.BASICV2;
        _Parser.Settings.BASICDialect = _Dialect;
      }
      // get full lines in covered range
      int     firstLine = ChangedRange.Start.iLine;
      int     lastLine = ChangedRange.End.iLine;

      if ( firstLine > lastLine )
      {
        int   dummy = firstLine;
        firstLine = lastLine;
        lastLine = dummy;
      }

      for ( int i = firstLine; i <= lastLine; ++i )
      {
        int     lastLineNo = i;
        var     line = ChangedRange.tb.Lines[i];
        var info = _Parser.PureTokenizeLine( line, i );

        var lineRange = ChangedRange.tb.GetLine( i );
        lineRange.ClearStyle();

        foreach ( var token in info.Tokens )
        {
          var subRange = new Range( ChangedRange.tb, token.StartIndex, i, token.StartIndex + token.Content.Length, i );
          switch ( token.TokenType )
          {
            case Parser.BasicFileParser.Token.Type.BASIC_TOKEN:
              subRange.SetStyle( StyleIndex.Style8 );
              break;
            case Parser.BasicFileParser.Token.Type.DIRECT_TOKEN:
              subRange.SetStyle( StyleIndex.Style6 );
              break;
            case Parser.BasicFileParser.Token.Type.EX_BASIC_TOKEN:
              subRange.SetStyle( StyleIndex.Style2 );
              break;
            case Parser.BasicFileParser.Token.Type.LINE_NUMBER:
              subRange.SetStyle( StyleIndex.Style5 );
              break;
            case Parser.BasicFileParser.Token.Type.NUMERIC_LITERAL:
              subRange.SetStyle( StyleIndex.Style5 );
              break;
            case Parser.BasicFileParser.Token.Type.STRING_LITERAL:
              subRange.SetStyle( StyleIndex.Style4 );
              break;
            case Parser.BasicFileParser.Token.Type.COMMENT:
              subRange.SetStyle( StyleIndex.Style3 );
              break;
            case Parser.BasicFileParser.Token.Type.MACRO:
              subRange.SetStyle( StyleIndex.Style7 );
              break;
            case Parser.BasicFileParser.Token.Type.VARIABLE:
              subRange.SetStyle( StyleIndex.Style9 );
              break;
          }
        }
      }
      _Parser.Clear();
    }


  }
}
