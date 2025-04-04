﻿using FastColoredTextBoxNS;
using RetroDevStudio.Documents;
using RetroDevStudio.Parser;
using RetroDevStudio.Parser.BASIC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace RetroDevStudio.CustomRenderer
{
  class BASICSyntaxHighlighter : FastColoredTextBoxNS.SyntaxHighlighter
  {
    static BasicFileParser    _Parser = new BasicFileParser( new BasicFileParser.ParserSettings() );
    static Dialect            _Dialect = null;


    private SourceBasicEx     _SourceDoc = null;



    public BASICSyntaxHighlighter( SourceBasicEx SourceDoc )
    {
      _SourceDoc = SourceDoc;
    }



    public void SetBASICDialect( Dialect dialect )
    {
      _Parser.Settings.BASICDialect = dialect;
    }



    public override void HighlightSyntax( Language Language, FastColoredTextBoxNS.Range ChangedRange )
    {
      if ( _Parser.Settings.BASICDialect == null )
      {
        _Dialect = Dialect.BASICV2;
        // _Parser.Settings.BASICDialect = _Dialect;
        _Parser.SetBasicDialect( _Dialect );
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
        if ( line.Length == 0 )
        {
          continue;
        }
        if ( _SourceDoc.m_LowerCaseMode )
        {
          line = BasicFileParser.MakeUpperCase( line, _SourceDoc.Core.Settings.BASICUseNonC64Font );
        }

        var info = _Parser.PureTokenizeLine( line );

        var lineRange = ChangedRange.tb.GetLine( i );
        lineRange.ClearStyle( StyleIndex.All );

        bool hadREM = false;

        foreach ( var token in info.Tokens )
        {
          var subRange = new FastColoredTextBoxNS.Range( ChangedRange.tb, token.StartIndex, i, token.StartIndex + token.Content.Length, i );
          if ( hadREM )
          {
            subRange.SetStyle( StyleIndex.Style3 );
            continue;
          }
          switch ( token.TokenType )
          {
            case BasicFileParser.Token.Type.BASIC_TOKEN:
              subRange.SetStyle( StyleIndex.Style8 );
              if (  _Parser.IsComment( token ) )
              {
                hadREM = true;
              }
              break;
            case BasicFileParser.Token.Type.DIRECT_TOKEN:
              subRange.SetStyle( StyleIndex.Style6 );
              break;
            case BasicFileParser.Token.Type.EX_BASIC_TOKEN:
              subRange.SetStyle( StyleIndex.Style2 );
              break;
            case BasicFileParser.Token.Type.LINE_NUMBER:
              subRange.SetStyle( StyleIndex.Style5 );
              break;
            case BasicFileParser.Token.Type.NUMERIC_LITERAL:
              subRange.SetStyle( StyleIndex.Style5 );
              break;
            case BasicFileParser.Token.Type.STRING_LITERAL:
              subRange.SetStyle( StyleIndex.Style4 );
              break;
            case BasicFileParser.Token.Type.COMMENT:
            case BasicFileParser.Token.Type.HARD_COMMENT:
              subRange.SetStyle( StyleIndex.Style3 );
              break;
            case BasicFileParser.Token.Type.MACRO:
              subRange.SetStyle( StyleIndex.Style7 );
              break;
            case BasicFileParser.Token.Type.VARIABLE:
              subRange.SetStyle( StyleIndex.Style9 );
              break;
          }
        }
      }
      _Parser.Clear();
    }


  }
}
