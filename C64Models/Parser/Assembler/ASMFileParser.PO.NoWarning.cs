using GR.Memory;
using RetroDevStudio.Formats;
using RetroDevStudio.Parser;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;

namespace RetroDevStudio.Parser
{
  public partial class ASMFileParser : ParserBase
  {
    private ParseLineResult PONoWarning( List<TokenInfo> TokenInfos, ref int lineIndex, ref string[] Lines )
    {
      if ( ParseLineInParameters( TokenInfos, 1, TokenInfos.Count - 1, lineIndex, false, out List<List<TokenInfo>> lineParams ) )
      {
        return ParseLineResult.ERROR_ABORT;
      }
      foreach ( var warning in lineParams )
      {
        if ( ( warning.Count != 1 )
        ||   ( warning[0].Type != TokenInfo.TokenType.LABEL_GLOBAL )
        ||   ( warning[0].Length != 5 )
        ||   ( ( warning[0].Content[0] != 'W' )
        &&     ( warning[0].Content[0] != 'w' ) ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1000_SYNTAX_ERROR, "Malformed warning number, expect warning value in line " + ( lineIndex + 1 ) );
          return ParseLineResult.RETURN_NULL;
        }
        string warningText = warning[0].Content.ToUpper();

        var warningEnums = System.Enum.GetNames( typeof( ErrorCode ) );
        var warningValues = System.Enum.GetValues( typeof( ErrorCode ) );
        int index = 0;
        bool foundWarning = false;
        foreach ( var warningEnum in warningEnums )
        {
          if ( warningEnum.Length < 5 )
          {
            ++index;
            continue;
          }
          if ( warningText == warningEnum.Substring( 0, 5 ) )
          {
            var actualWarning = (ErrorCode)warningValues.GetValue( index );

            if ( ( (int)actualWarning <= (int)ErrorCode.WARNING_START )
            ||   ( (int)actualWarning >= (int)ErrorCode.WARNING_LAST_PLUS_ONE ) )
            {
              AddError( lineIndex, Types.ErrorCode.E1000_SYNTAX_ERROR, $"Malformed warning number, {warning[0].Content} is not a known warning code in line " + ( lineIndex + 1 ) );
              return ParseLineResult.RETURN_NULL;
            }
            m_WarningsToIgnore.Add( actualWarning );
            foundWarning = true;
            break;
          }
          ++index;
        }
        if ( !foundWarning )
        {
          AddError( lineIndex, Types.ErrorCode.E1000_SYNTAX_ERROR, $"Malformed warning number, {warning[0].Content} is not a known warning code in line " + ( lineIndex + 1 ) );
          return ParseLineResult.RETURN_NULL;
        }

      }
      return ParseLineResult.CALL_CONTINUE;
    }



  }
}
