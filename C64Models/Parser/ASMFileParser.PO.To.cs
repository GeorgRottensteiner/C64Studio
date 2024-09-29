using GR.Collections;
using GR.Memory;
using RetroDevStudio.Formats;
using RetroDevStudio.Parser;
using RetroDevStudio.Types;
using RetroDevStudio.Types.ASM;
using System;
using System.Collections.Generic;
using System.Linq;
using Tiny64;

namespace RetroDevStudio.Parser
{
  public partial class ASMFileParser : ParserBase
  {
    private ParseLineResult POTo( List<TokenInfo> lineTokenInfos )
    {
      // !to targetfilename,outputtype[,type specific options]
      if ( ( !ParseLineInParameters( lineTokenInfos, 0, lineTokenInfos.Count, _ParseContext.LineIndex, false, out List<List<TokenInfo>> parms ) )
      ||   ( parms.Count < 2 )
      ||   ( parms[0].Count != 2 )
      ||   ( parms[1].Count != 1 ) )
      {
        AddError( _ParseContext.LineIndex,
                  Types.ErrorCode.E1302_MALFORMED_MACRO,
                  "Expected !to <Filename>,<Type = " + ListKeys( Lookup.CompileTargetModeToKeyword.Values ) + ">[,<Type specific options>]" );
        return ParseLineResult.ERROR_ABORT;
      }

      if ( !string.IsNullOrEmpty( m_CompileTargetFile ) )
      {
        AddWarning( _ParseContext.LineIndex,
                    RetroDevStudio.Types.ErrorCode.W0004_TARGET_FILENAME_ALREADY_PROVIDED,
                    "A target file name was already provided, ignoring this one",
                    -1,
                    0 );
      }
      else
      {
        //int   numArgs = 1 + 

        if ( parms[0][1].Type != Types.TokenInfo.TokenType.LITERAL_STRING )
        {
          AddError( _ParseContext.LineIndex,
                    Types.ErrorCode.E1307_FILENAME_INCOMPLETE,
                    "String as file name expected",
                    parms[0][1].StartPos,
                    parms[0][1].Length );
          return ParseLineResult.ERROR_ABORT;
        }

        if ( !InsertLiteralTextMacros( lineTokenInfos ) )
        {
          return ParseLineResult.ERROR_ABORT;
        }

        string    targetType = parms[1][0].Content.ToUpper();
        if ( !Lookup.CompileTargetModeToKeyword.ContainsValue( targetType ) )
        {
          AddError( _ParseContext.LineIndex,
                    Types.ErrorCode.E1304_UNSUPPORTED_TARGET_TYPE,
                    "Unsupported target type " + lineTokenInfos[2].Content + ", only " + ListKeys( Lookup.CompileTargetModeToKeyword.Values ) + " supported",
                    parms[1][0].StartPos,
                    parms[1][0].Length );
          return ParseLineResult.ERROR_ABORT;
        }
        string filename = parms[1][0].Content.Substring( 1, lineTokenInfos[0].Length - 2 );
        // do not append to absolute path!
        if ( GR.Path.IsPathRooted( filename ) )
        {
          m_CompileTargetFile = filename;
        }
        else
        {
          m_CompileTargetFile = GR.Path.Append( m_DocBasePath, filename );
        }

        m_CompileTarget.Type = Lookup.CompileTargetModeToKeyword.Where( c => c.Value == targetType ).Select( c => c.Key ).First();

        int numExtraArgs = 0;

        var memberInfo = typeof( CompileTargetType ).GetMember( m_CompileTarget.Type.ToString() ).FirstOrDefault();
        if ( memberInfo != null )
        {
          var att = (AdditionalArgumentCountAttribute)memberInfo.GetCustomAttributes( typeof( AdditionalArgumentCountAttribute ), false ).FirstOrDefault();
          if ( att != null )
          {
            numExtraArgs = att.NumArguments;
          }
        }

        if ( parms.Count != 2 + numExtraArgs )
        {
          AddError( _ParseContext.LineIndex,
                    Types.ErrorCode.E1302_MALFORMED_MACRO,
                    $"Expected {numExtraArgs} additional parameters, but found {parms.Count - 2}" );
          return ParseLineResult.ERROR_ABORT;
        }
      }
      return ParseLineResult.OK;
    }



  }
}
