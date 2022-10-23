using GR.Memory;
using RetroDevStudio.Formats;
using RetroDevStudio.Parser;
using RetroDevStudio.Types;
using RetroDevStudio.Types.ASM;
using System;
using System.Collections.Generic;
using Tiny64;

namespace RetroDevStudio.Parser
{
  public partial class ASMFileParser : ParserBase
  {
    private ParseLineResult POBasic( string Line, List<Types.TokenInfo> lineTokenInfos, int lineIndex, Types.ASM.LineInfo info, GR.Collections.Map<byte,byte> textCodeMapping, bool AllowLaterEvaluation, bool HideInPreprocessedOutput, out int lineSizeInBytes )
    {
      // !basic behaves differently for Mega65!
      lineSizeInBytes = 13;
      info.NumBytes   = 13;

      info.LineData = new GR.Memory.ByteBuffer();

      int       basicLineNumber = 10;
      int       startAddress = info.AddressStart;
      int       secondLineStartAddressOffset = 0;

      if ( m_Processor.Name == "M65" )
      {
        if ( info.AddressStart >= 65536 )
        {
          AddError( lineIndex, Types.ErrorCode.E1003_VALUE_OUT_OF_BOUNDS_WORD, "BASIC upstart address is too high (>65535)" );
          return ParseLineResult.RETURN_NULL;
        }

        info.LineData.AppendU16( (ushort)( info.AddressStart + 8 ) );
        info.LineData.AppendU16( (ushort)basicLineNumber );
        info.LineData.AppendHex( "FE023000" );    // bank 0


        lineSizeInBytes += 8;
        info.NumBytes += 8;
        secondLineStartAddressOffset = 8;

        basicLineNumber += 10;
        startAddress += 8;
      }

      GR.Memory.ByteBuffer    commentData = new ByteBuffer();

      List<int> tokenParams = new List<int>();
      bool      paramsValid = false;
      int       jumpAddress = -1;

      int       realNumParams = 1;
      int       numDigits = -1;

      List<List<Types.TokenInfo>>   poParams = new List<List<Types.TokenInfo>>();

      int     firstTokenIndex = 1;
      int     secondTokenIndex = -1;
      for ( int i = 1; i < lineTokenInfos.Count; ++i )
      {
        if ( ( lineTokenInfos[i].Type == Types.TokenInfo.TokenType.SEPARATOR )
        &&   ( lineTokenInfos[i].Content == "," ) )
        {
          if ( i - firstTokenIndex > 0 )
          {
            poParams.Add( lineTokenInfos.GetRange( firstTokenIndex, i - firstTokenIndex ) );
          }
          else
          {
            poParams.Add( new List<Types.TokenInfo>() );
          }
          
          ++realNumParams;
          firstTokenIndex = i + 1;
        }
        else if ( ( firstTokenIndex > 1 )
        &&        ( secondTokenIndex == -1 ) )
        {
          secondTokenIndex = i;
        }

      }
      if ( firstTokenIndex < lineTokenInfos.Count )
      {
        poParams.Add( lineTokenInfos.GetRange( firstTokenIndex, lineTokenInfos.Count - firstTokenIndex ) );
      }

      if ( lineTokenInfos.Count == 1 )
      {
        // !basic
        paramsValid = true;
      }
      else if ( ( lineTokenInfos.Count > 1 )
      &&        ( poParams.Count == 1 ) )
      {
        // !basic <jump address>
        if ( !EvaluateTokens( lineIndex, poParams[0], 0, poParams[0].Count, info.LineCodeMapping, out SymbolInfo jumpAddressSymbol ) )
        {
          // could not fully parse
          info.NeededParsedExpression = lineTokenInfos;
          info.Line = Line;
          // can we use 4 digits?
          if ( info.AddressStart + lineSizeInBytes - 1 < 10000 )
          {
            --lineSizeInBytes;
            --info.NumBytes;
          }
          if ( !AllowLaterEvaluation )
          {
            info.NeededParsedExpression = poParams[0];
          }
          return ParseLineResult.OK_PARSE_EXPRESSION_LATER;
        }
        jumpAddress = jumpAddressSymbol.ToInt32();
        if ( ( jumpAddress < 0 )
        ||   ( jumpAddress >= 65536 ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1003_VALUE_OUT_OF_BOUNDS_WORD, "Jump target address is out of bounds" );
          return ParseLineResult.RETURN_NULL;
        }
        paramsValid = true;
      }
      else if ( ( lineTokenInfos.Count > 1 )
      &&        ( realNumParams == 2 ) )
      {
        // !basic <line number>,<jump address>
        if ( !EvaluateTokens( lineIndex, poParams[0], 0, poParams[0].Count, info.LineCodeMapping, out SymbolInfo basicLineNumberSymbol ) )
        {
          // could not fully parse
          info.NeededParsedExpression = lineTokenInfos;
          info.Line = Line;
          // can we use 4 digits?
          if ( info.AddressStart + lineSizeInBytes - 1 < 10000 )
          {
            --lineSizeInBytes;
            --info.NumBytes;
          }
          if ( !AllowLaterEvaluation )
          {
            info.NeededParsedExpression = poParams[0];
          }
          return ParseLineResult.OK_PARSE_EXPRESSION_LATER;
        }
        basicLineNumber = basicLineNumberSymbol.ToInt32();
        if ( ( basicLineNumber < 0 )
        ||   ( basicLineNumber > 63999 ) )
        {
          AddError( lineIndex, Types.ErrorCode.E3001_BASIC_INVALID_LINE_NUMBER, "Unsupported line number, must be in the range 0 to 63999" );
          return ParseLineResult.RETURN_NULL;
        }

        if ( !EvaluateTokens( lineIndex, poParams[1], 0, poParams[1].Count, info.LineCodeMapping, out SymbolInfo jumpAddressSymbol ) )
        {
          // could not fully parse
          info.NeededParsedExpression = lineTokenInfos;
          info.Line = Line;
          // can we use 4 digits?
          if ( info.AddressStart + lineSizeInBytes - 1 < 10000 )
          {
            --lineSizeInBytes;
            --info.NumBytes;
          }
          if ( !AllowLaterEvaluation )
          {
            info.NeededParsedExpression = poParams[1];
          }
          return ParseLineResult.OK_PARSE_EXPRESSION_LATER;
        }
        jumpAddress = jumpAddressSymbol.ToInt32();
        if ( ( jumpAddress < 0 )
        ||   ( jumpAddress >= 65536 ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1003_VALUE_OUT_OF_BOUNDS_WORD, "Jump target address is out of bounds" );
          return ParseLineResult.RETURN_NULL;
        }
        paramsValid = true;
      }
      else if ( ( lineTokenInfos.Count > 1 )
      &&        ( realNumParams >= 3 ) )
      {
        info.NeededParsedExpression = null;
        // !basic <line number>,<comment>[,<comment-bytes>],<jump address>
        if ( !EvaluateTokens( lineIndex, poParams[0], 0, poParams[0].Count, info.LineCodeMapping, out SymbolInfo basicLineNumberSymbol ) )
        {
          // could not fully parse
          info.NeededParsedExpression = lineTokenInfos;
          info.Line = Line;
          // can we use 4 digits?
          if ( info.AddressStart + lineSizeInBytes - 1 < 10000 )
          {
            --lineSizeInBytes;
            --info.NumBytes;
          }
          if ( !AllowLaterEvaluation )
          {
            info.NeededParsedExpression = poParams[0];
          }
          return ParseLineResult.OK_PARSE_EXPRESSION_LATER;
        }
        basicLineNumber = basicLineNumberSymbol.ToInt32();
        if ( ( basicLineNumber < 0 )
        ||   ( basicLineNumber > 63999 ) )
        {
          AddError( lineIndex, Types.ErrorCode.E3001_BASIC_INVALID_LINE_NUMBER, "Unsupported line number, must be in the range 0 to 63999" );
          return ParseLineResult.RETURN_NULL;
        }

        var dummyLineInfo = new Types.ASM.LineInfo();
        dummyLineInfo.HideInPreprocessedOutput = HideInPreprocessedOutput;

        var subRange = lineTokenInfos.GetRange( secondTokenIndex, lineTokenInfos.Count - secondTokenIndex - 2 );

        for ( int i = 1; i < poParams.Count - 1; ++i )
        {
          GR.Memory.ByteBuffer    dataOut;

          if ( EvaluateTokensBinary( lineIndex, poParams[i], textCodeMapping, out dataOut ) )
          {
            if ( dummyLineInfo.LineData == null )
            {
              dummyLineInfo.LineData = new ByteBuffer();
            }
            dummyLineInfo.LineData.Append( dataOut );
          }
          else
          {
            AddError( info.LineIndex,
                      Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION,
                      "Failed to evaluate expression " + TokensToExpression( poParams[i], 0, poParams[i].Count ),
                      poParams[i][0].StartPos,
                      poParams[i][poParams[i].Count - 1].EndPos - poParams[i][0].StartPos + 1 );
            return ParseLineResult.ERROR_ABORT;
          }
        }

        var commentDataTemp = dummyLineInfo.LineData;
        if ( commentDataTemp == null )
        {
          commentDataTemp = new GR.Memory.ByteBuffer();
        }

        int   lengthOfCommentData = (int)commentDataTemp.Length;

        if ( !EvaluateTokens( lineIndex, poParams[poParams.Count - 1], 0, poParams[poParams.Count - 1].Count, info.LineCodeMapping, out SymbolInfo jumpAddressSymbol ) )
        {
          // could not fully parse
          info.NeededParsedExpression = lineTokenInfos;// poParams[poParams.Count - 1];
          info.Line = Line;
          // can we use 4 digits?
          if ( info.AddressStart + lineSizeInBytes - 1 + lengthOfCommentData < 10000 )
          {
            lineSizeInBytes = lineSizeInBytes - 1 + lengthOfCommentData;
            info.NumBytes = lineSizeInBytes - 1 + lengthOfCommentData;
          }
          if ( !AllowLaterEvaluation )
          {
            info.NeededParsedExpression = poParams[poParams.Count - 1];
          }
          return ParseLineResult.OK_PARSE_EXPRESSION_LATER;
        }
        jumpAddress = jumpAddressSymbol.ToInt32();
        if ( ( jumpAddress < 0 )
        ||   ( jumpAddress >= 65536 ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1003_VALUE_OUT_OF_BOUNDS_WORD, "Jump target address is out of bounds" );
          return ParseLineResult.RETURN_NULL;
        }
        commentData = commentDataTemp;
        paramsValid = true;
      }

      if ( ( lineTokenInfos.Count < 1 )
      ||   ( !paramsValid ) )
      {
        AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected !basic [<jump address>] or !basic <line number>,<jump address> or !basic <line number>,<comment>[,<more comment-bytes>],<jump address>" );
        return ParseLineResult.RETURN_NULL;
      }

      if ( jumpAddress == -1 )
      {
        int   endAddress = info.AddressStart + 8 + 5 + lineSizeInBytes - 13;
        jumpAddress = endAddress - 5 + CalcNumDigits( endAddress );
      }

      numDigits = CalcNumDigits( jumpAddress );

      // Startadresse der folgenden Programmzeile in der Reihenfolge Low - Byte, High - Byte($0000 kennzeichnet das Programmende ).
      // Zeilennummer in der Form Low - Byte, High - Byte.
      // Der eigentliche Programmcode (bis zu 250 Bytes) im Token-Format.
      // Ein Null - Byte kennzeichnet das Ende der Programmzeile.

      info.LineData.AppendU8( (byte)( 0x0b - 5 + numDigits ) );
      // 0x08   high byte of address of next line
      // 0xa000 line number 160 ?
      // 0x9e   SYS
      info.LineData.AppendHex( "08" );
      info.LineData.AppendU16( (ushort)basicLineNumber );
      info.LineData.AppendHex( "9E" );
      int     lineLength = (int)( 2 + 3 + numDigits + commentData.Length + 1 );
      info.LineData.SetU16At( secondLineStartAddressOffset, (ushort)( startAddress + lineLength ) );

      lineSizeInBytes = (int)info.LineData.Length + numDigits + 1 + (int)commentData.Length;

      if ( jumpAddress == -1 )
      {
        jumpAddress = info.AddressStart + lineSizeInBytes + 2;
      }

      if ( numDigits >= 5 )
      {
        info.LineData.AppendU8( (byte)( 0x30 + jumpAddress / 10000 ) );
      }
      if ( numDigits >= 4 )
      {
        info.LineData.AppendU8( (byte)( 0x30 + ( ( jumpAddress / 1000 ) % 10 ) ) );
      }
      if ( numDigits >= 3 )
      {
        info.LineData.AppendU8( (byte)( 0x30 + ( ( jumpAddress / 100 ) % 10 ) ) );
      }
      if ( numDigits >= 2 )
      {
        info.LineData.AppendU8( (byte)( 0x30 + ( ( jumpAddress / 10 ) % 10 ) ) );
      }
      info.LineData.AppendU8( (byte)( 0x30 + ( jumpAddress % 10 ) ) );

      info.LineData.Append( commentData );

      info.LineData.AppendHex( "000000" );

      lineSizeInBytes = (int)info.LineData.Length;
      info.NumBytes = lineSizeInBytes;

      return ParseLineResult.OK;
    }



  }
}
