using GR.Collections;
using GR.Memory;
using RetroDevStudio.Formats;
using RetroDevStudio.Parser;
using RetroDevStudio.Types;
using RetroDevStudio.Types.ASM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Tiny64;

namespace RetroDevStudio.Parser
{
  public partial class ASMFileParser : ParserBase
  {
    [Flags]
    [DefaultValue( LabelFileMode.DEFAULT )]
    private enum LabelFileMode
    {
      
      DEFAULT                       = 0,
      IGNORE_ASSEMBLER_ID_LABELS    = 0x00000001,
      IGNORE_UNUSED_LABELS          = 0x00000002,
      IGNORE_INTERNAL_LABELS        = 0x00000004    // any C64STUDIO_INTERNAL... are omitted
    }



    private ParseLineResult POLabelFile( List<TokenInfo> lineTokenInfos )
    {
      if ( !ParseLineInParameters( lineTokenInfos, 1, lineTokenInfos.Count - 1, _ParseContext.LineIndex, false, out List<List<TokenInfo>> parms ) )
      {
        return ParseLineResult.ERROR_ABORT;
      }

      if ( ( parms.Count > 2 )
      ||   ( parms.Count < 1 ) )
      {
        AddError( _ParseContext.LineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, $"Expected !sl <Filename>[, {ListFlagValues( typeof( LabelFileMode ) )}]" );

        return ParseLineResult.ERROR_ABORT;
      }


      lineTokenInfos.RemoveAt( 0 );

      if ( parms[0].Count != 1 )
      {
        AddError( _ParseContext.LineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, $"Expected !sl <Filename>[, {ListFlagValues( typeof( LabelFileMode ) )}]" );

        return ParseLineResult.ERROR_ABORT;
      }
      if ( parms[0][0].Type != Types.TokenInfo.TokenType.LITERAL_STRING )
      {
        AddError( _ParseContext.LineIndex,
                  Types.ErrorCode.E1307_FILENAME_INCOMPLETE,
                  "String as file name expected",
                  lineTokenInfos[0].StartPos,
                  lineTokenInfos[0].Length );

        return ParseLineResult.ERROR_ABORT;
      }
      if ( !string.IsNullOrEmpty( m_ASMFileInfo.LabelDumpFile ) )
      {
        AddWarning( _ParseContext.LineIndex,
                    RetroDevStudio.Types.ErrorCode.W0006_LABEL_DUMP_FILE_ALREADY_GIVEN,
                    "Label dump file name has already been provided",
                    lineTokenInfos[0].StartPos,
                    lineTokenInfos[lineTokenInfos.Count - 1].EndPos + 1 - lineTokenInfos[0].StartPos );
      }

      string fileName = parms[0][0].Content.Substring( 1, lineTokenInfos[0].Content.Length - 2 );
      // use path of source file
      fileName = GR.Path.Append( System.IO.Path.GetDirectoryName( _ParseContext.ParentFilename ), fileName );
      m_ASMFileInfo.LabelDumpFile = fileName;

      if ( parms.Count > 1 )
      {
        // parse mode
        if ( !ParseEnumFlags( parms[1], out LabelFileMode modeFlags ) )
        {
          AddError( _ParseContext.LineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, $"Expected !sl <Filename>[, {ListFlagValues( typeof( LabelFileMode ) )}]" );

          return ParseLineResult.ERROR_ABORT;
        }
      }

      return ParseLineResult.OK;
    }



    /// <summary>
    /// Parse flags from a token list, expect flag[ | flag]*
    /// </summary>
    /// <typeparam name="EnumType"></typeparam>
    /// <param name="Tokens"></param>
    /// <param name="ModeFlags"></param>
    /// <returns></returns>
    private bool ParseEnumFlags<EnumType>( List<TokenInfo> Tokens, out EnumType ModeFlags )
    {
      ModeFlags = default( EnumType );
      return false;
    }



  }
}
