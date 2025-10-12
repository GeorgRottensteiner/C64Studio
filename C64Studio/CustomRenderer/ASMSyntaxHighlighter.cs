using FastColoredTextBoxNS;
using RetroDevStudio.Documents;
using RetroDevStudio.Parser;
using RetroDevStudio.Parser.BASIC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace RetroDevStudio.CustomRenderer
{
  class ASMSyntaxHighlighter : FastColoredTextBoxNS.SyntaxHighlighter
  {
    public ASMFileParser    Parser = new ASMFileParser();

    private SourceASMEx     _SourceDoc = null;



    public ASMSyntaxHighlighter( SourceASMEx sourceDoc )
    {
      _SourceDoc = sourceDoc;
    }



    public override void HighlightSyntax( Language Language, FastColoredTextBoxNS.Range ChangedRange )
    {
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
        var     line = ChangedRange.tb.ReTabifyLine( ChangedRange.tb.Lines[i], ChangedRange.tb.TabLength );
        if ( line.Length == 0 )
        {
          continue;
        }

        var info = Parser.ParseTokenInfo( line, 0, line.Length );

        var lineRange = ChangedRange.tb.GetLine( i );
        lineRange.ClearStyle( StyleIndex.All );

        bool hadREM = false;

        foreach ( var token in info )
        {
          var subRange = new FastColoredTextBoxNS.Range( ChangedRange.tb, token.StartPos, i, token.StartPos + token.Content.Length, i );
          if ( hadREM )
          {
            subRange.SetStyle( StyleIndex.Style3 );
            continue;
          }
          switch ( token.Type )
          {
            case Types.TokenInfo.TokenType.OPCODE:
              subRange.SetStyle( StyleIndex.Style8 );
              /*
              if (  _Parser.IsComment( token ) )
              {
                hadREM = true;
              }*/
              break;
            case Types.TokenInfo.TokenType.SINGLE_CHAR:
              subRange.SetStyle( StyleIndex.Style6 );
              break;
              /*
            case BasicFileParser.Token.Type.EX_BASIC_TOKEN:
              subRange.SetStyle( StyleIndex.Style2 );
              break;
            case BasicFileParser.Token.Type.LINE_NUMBER:
              subRange.SetStyle( StyleIndex.Style5 );
              break;
              */
            case Types.TokenInfo.TokenType.LITERAL_NUMBER:
            case Types.TokenInfo.TokenType.LITERAL_REAL_NUMBER:
              subRange.SetStyle( StyleIndex.Style7 );
              break;
            case Types.TokenInfo.TokenType.LITERAL_STRING:
            case Types.TokenInfo.TokenType.LITERAL_CHAR:
              subRange.SetStyle( StyleIndex.Style4 );
              break;
            case Types.TokenInfo.TokenType.COMMENT:
            case Types.TokenInfo.TokenType.COMMENT_IF_FIRST_CHAR:
              subRange.SetStyle( StyleIndex.Style3 );
              break;
            case Types.TokenInfo.TokenType.PSEUDO_OP:
              subRange.SetStyle( StyleIndex.Style6 );
              break;
            case Types.TokenInfo.TokenType.LABEL_CHEAP_LOCAL:
            case Types.TokenInfo.TokenType.LABEL_GLOBAL:
            case Types.TokenInfo.TokenType.LABEL_INTERNAL:
            case Types.TokenInfo.TokenType.LABEL_LOCAL:
              subRange.SetStyle( StyleIndex.Style9 );
              break;
            case Types.TokenInfo.TokenType.OPERATOR:
              subRange.SetStyle( StyleIndex.Style5 );
              break;

          }
        }
      }
      Parser.Clear();
    }


  }
}
