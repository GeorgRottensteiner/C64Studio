using GR.Memory;
using RetroDevStudio.Formats;
using RetroDevStudio.Parser;
using RetroDevStudio.Types;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Security.Policy;

namespace RetroDevStudio.Parser.BASIC
{
  public partial class BasicFileParser : ParserBase
  {
    private bool MetaCustomLabel( int lineIndex, string metaData, string metaDataParams, LineInfo lineInfo )
    {
      var  parms = PureTokenizeLine( metaDataParams );

      if ( !ParseLineInParameters( parms.Tokens, 0, parms.Tokens.Count, lineIndex, false, out List<List<Token>> cleanedParms ) )
      {
        return false;
      }

      // label texts may be split into multiple tokens (e.g. containing a BASIC command), so we need to combine them into a single token again
      foreach ( var group in cleanedParms )
      {
        bool hasInvalidTokenType = false;
        foreach ( var token in group ) 
        {
          if ( ( token.TokenType != Token.Type.VARIABLE )
          &&   ( token.TokenType != Token.Type.NUMERIC_LITERAL )
          &&   ( token.TokenType != Token.Type.BASIC_TOKEN ) )
          {
            hasInvalidTokenType = true;
            break;
          }
        }
        if ( hasInvalidTokenType )
        {
          AddError( lineIndex, Types.ErrorCode.E3007_BASIC_MALFORMED_METADATA, "LABEL expects a single label name" );
          return false;
        }
        var combinedLabel = TokensToExpression( group, 0, group.Count );
        group.Clear();

        // combine multiple variable tokens into a single token
        var combinedToken = new Token();
        combinedToken.TokenType = Token.Type.VARIABLE;
        combinedToken.Content = combinedLabel;
        group.Add( combinedToken );
      }

      if ( ( cleanedParms.Count != 1 )
      ||   ( cleanedParms[0].Count != 1 ) )
      {
        AddError( lineIndex, Types.ErrorCode.E3007_BASIC_MALFORMED_METADATA, "LABEL expects a single label name" );
        return false;
      }
      lineInfo.CustomLabelName = cleanedParms[0][0].Content;
      return true;
    }



  }
}
