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
    private ParseLineResult POTo( List<TokenInfo> lineTokenInfos, int lineIndex )
    {
      // !to targetfilename,outputtype
      if ( !string.IsNullOrEmpty( m_CompileTargetFile ) )
      {
        AddWarning( lineIndex,
                    RetroDevStudio.Types.ErrorCode.W0004_TARGET_FILENAME_ALREADY_PROVIDED,
                    "A target file name was already provided, ignoring this one",
                    -1,
                    0 );
      }
      else
      {
        lineTokenInfos.RemoveAt( 0 );

        if ( ( lineTokenInfos.Count != 3 )
        ||   ( lineTokenInfos[1].Content != "," ) )
        {
          AddError( lineIndex,
                    Types.ErrorCode.E1302_MALFORMED_MACRO,
                    "Expected !to <Filename>,<Type = " + ListKeys( Lookup.CompileTargetModeToKeyword.Values ) + ">" );
          return ParseLineResult.ERROR_ABORT;
        }
        if ( lineTokenInfos[0].Type != Types.TokenInfo.TokenType.LITERAL_STRING )
        {
          AddError( lineIndex,
                    Types.ErrorCode.E1307_FILENAME_INCOMPLETE,
                    "String as file name expected",
                    lineTokenInfos[0].StartPos,
                    lineTokenInfos[0].Length );
          return ParseLineResult.ERROR_ABORT;
        }
        string    targetType = lineTokenInfos[2].Content.ToUpper();
        if ( !Lookup.CompileTargetModeToKeyword.ContainsValue( targetType ) )
        {
          AddError( lineIndex,
                    Types.ErrorCode.E1304_UNSUPPORTED_TARGET_TYPE,
                    "Unsupported target type " + lineTokenInfos[2].Content + ", only " + ListKeys( Lookup.CompileTargetModeToKeyword.Values ) + " supported",
                    lineTokenInfos[2].StartPos,
                    lineTokenInfos[2].Length );
          return ParseLineResult.ERROR_ABORT;
        }
        string filename = lineTokenInfos[0].Content.Substring( 1, lineTokenInfos[0].Length - 2 );
        // do not append to absolute path!
        if ( System.IO.Path.IsPathRooted( filename ) )
        {
          m_CompileTargetFile = filename;
        }
        else
        {
          m_CompileTargetFile = GR.Path.Append( m_DocBasePath, filename );
        }
        m_CompileTarget = Lookup.CompileTargetModeToKeyword.Where( c => c.Value == targetType ).Select( c => c.Key ).First();
      }
      return ParseLineResult.OK;
    }



  }
}
