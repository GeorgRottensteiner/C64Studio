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
    private ParseLineResult HandleOpcode( LineInfo info, int lineIndex, List<TokenInfo> lineTokenInfos, List<List<TokenInfo>> opcodeExpressions, ulong resultingOpcodePatchValue, GR.Collections.Map<byte, byte> textCodeMapping )
    {
      if ( info.LineData == null )
      {
        info.LineData = new GR.Memory.ByteBuffer();
      }
      AppendOpcodeValue( info, resultingOpcodePatchValue );
      long byteValue = -1;

      // strip prefixed #
      if ( ( info.Opcode.Addressing == Opcode.AddressingType.IMMEDIATE_ACCU )
      ||   ( info.Opcode.Addressing == Opcode.AddressingType.IMMEDIATE_8BIT )
      ||   ( info.Opcode.Addressing == Opcode.AddressingType.IMMEDIATE_16BIT )
      ||   ( info.Opcode.Addressing == Opcode.AddressingType.IMMEDIATE_REGISTER ) )
      {
        if ( ( lineTokenInfos.Count > 1 )
        &&   ( lineTokenInfos[1].Content.StartsWith( "#" ) ) )
        {
          if ( lineTokenInfos[1].Length == 1 )
          {
            lineTokenInfos.RemoveAt( 1 );
          }
          else
          {
            lineTokenInfos[1].Content = lineTokenInfos[1].Content.Substring( 1 );
          }
        }
      }
      if ( ScopeInsideMacroDefinition() )
      {
        return ParseLineResult.CALL_CONTINUE;
      }

      int   rounds = 0;
      if ( ( opcodeExpressions != null )
      &&   ( opcodeExpressions.Count > 0 ) )
      {
        rounds = opcodeExpressions.Count;
      }

      for ( int round = 0; round < rounds; ++round )
      {
        int               startIndex = 0;
        int               count = opcodeExpressions[round].Count;
        List<TokenInfo>   tokensToEvaluate = opcodeExpressions[round];

        int   parserExpressionIndex = MatchRoundToParserExpression( info.Opcode, round );

        if ( EvaluateTokens( lineIndex, tokensToEvaluate, startIndex, count, out SymbolInfo byteValueSymbol ) )
        {
          byteValue = byteValueSymbol.ToInteger();
          if ( !ValidateExpressionValueRange( ref byteValue, info, round, out int valueRangeListIndex ) )
          {
            AddError( lineIndex,
                      Types.ErrorCode.E1014_VALUE_OUT_OF_BOUNDS_RANGE,
                      "Value $" + byteValue.ToString( "X" ) + $" ({byteValue}) is not in the range of {ListValidValues( info.Opcode.ParserExpressions[round].ValidValues[valueRangeListIndex].ValidValues )}.",
                      tokensToEvaluate[startIndex].StartPos,
                      tokensToEvaluate[count - 1].EndPos + 1 - tokensToEvaluate[startIndex].StartPos );
          }
          else
          {
            if ( VerifyOperandSize( info.Opcode, info.Opcode.ParserExpressions[parserExpressionIndex], info.AddressStart, ref byteValue, lineIndex, tokensToEvaluate ) )
            {
              ApplyOpcodePatch( info, (uint)byteValue, round );
            }
          }
          if ( round == 0 )
          {
            info.NeededParsedExpression = null;
          }
          else
          {
            info.NeededParsedExpression2 = null;
          }
        }
        else
        {
          if ( round == 0 )
          {
            info.NeededParsedExpression = tokensToEvaluate;
          }
          else
          {
            info.NeededParsedExpression2 = tokensToEvaluate;
          }
        }
      }
      return ParseLineResult.OK;
    }



    private int MatchRoundToParserExpression( Opcode Opcode, int Round )
    {
      int index = 0;
      foreach ( var exp in Opcode.ParserExpressions )
      {
        if ( ( exp.Type == Opcode.OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT )
        ||   ( exp.Type == Opcode.OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT )
        ||   ( exp.Type == Opcode.OpcodePartialExpression.ENCAPSULATED_EXPRESSION_24BIT )
        ||   ( exp.Type == Opcode.OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT )
        ||   ( exp.Type == Opcode.OpcodePartialExpression.EXPRESSION_7BIT )
        ||   ( exp.Type == Opcode.OpcodePartialExpression.EXPRESSION_8BIT )
        ||   ( exp.Type == Opcode.OpcodePartialExpression.EXPRESSION_15BIT )
        ||   ( exp.Type == Opcode.OpcodePartialExpression.EXPRESSION_16BIT )
        ||   ( exp.Type == Opcode.OpcodePartialExpression.EXPRESSION_24BIT )
        ||   ( exp.Type == Opcode.OpcodePartialExpression.EXPRESSION_32BIT )
        ||   ( exp.Type == Opcode.OpcodePartialExpression.EXPRESSION_8BIT_RELATIVE )
        ||   ( exp.Type == Opcode.OpcodePartialExpression.EXPRESSION_16BIT_RELATIVE ) )
        {
          if ( Round == 0 )
          {
            return index;
          }
          --Round;
        }
        else if ( exp.Type == Opcode.OpcodePartialExpression.COMPLEX )
        {
          foreach ( var entry in exp.ValidValues )
          {
            if ( ( entry.Expression == Opcode.OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT )
            ||   ( entry.Expression == Opcode.OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT )
            ||   ( entry.Expression == Opcode.OpcodePartialExpression.ENCAPSULATED_EXPRESSION_24BIT )
            ||   ( entry.Expression == Opcode.OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT )
            ||   ( entry.Expression == Opcode.OpcodePartialExpression.EXPRESSION_7BIT )
            ||   ( entry.Expression == Opcode.OpcodePartialExpression.EXPRESSION_8BIT )
            ||   ( entry.Expression == Opcode.OpcodePartialExpression.EXPRESSION_15BIT )
            ||   ( entry.Expression == Opcode.OpcodePartialExpression.EXPRESSION_16BIT )
            ||   ( entry.Expression == Opcode.OpcodePartialExpression.EXPRESSION_24BIT )
            ||   ( entry.Expression == Opcode.OpcodePartialExpression.EXPRESSION_32BIT )
            ||   ( entry.Expression == Opcode.OpcodePartialExpression.EXPRESSION_8BIT_RELATIVE )
            ||   ( entry.Expression == Opcode.OpcodePartialExpression.EXPRESSION_16BIT_RELATIVE ) )
            {
              if ( Round == 0 )
              {
                return index;
              }
              --Round;
            }
          }
        }
        ++index;
      }
      Debug.Log( "Could not deduce parser expression index!!" );
      return -1;
    }



    private bool VerifyOperandSize( Opcode Opcode, Opcode.OpcodeExpression OpcodeExpression, long AddressStart, ref long ByteValue, int lineIndex, List<TokenInfo> Tokens )
    {
      if ( OpcodeExpression.Type == Opcode.OpcodePartialExpression.COMPLEX )
      {
        foreach ( var valueGroup in OpcodeExpression.ValidValues )
        {
          if ( valueGroup.Expression == Opcode.OpcodePartialExpression.EXPRESSION_7BIT )
          {
            if ( !Valid7BitValue( ByteValue ) )
            {
              AddError( lineIndex,
                        Types.ErrorCode.E1016_VALUE_OUT_OF_BOUNDS_7BIT,
                        "Value out of bounds for 7 bit, needs to be >= -64 and <= 127. Expression:" + TokensToExpression( Tokens ),
                        Tokens[0].StartPos,
                        Tokens[Tokens.Count - 1].EndPos + 1 - Tokens[0].StartPos );
              ByteValue = ( (byte)ByteValue ) & 0x7f;
              return false;
            }
            ByteValue = ( (byte)ByteValue ) & 0x7f;
          }
          else if ( ( valueGroup.Expression == Opcode.OpcodePartialExpression.EXPRESSION_8BIT )
          ||        ( valueGroup.Expression == Opcode.OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT )
          ||        ( valueGroup.Expression == Opcode.OpcodePartialExpression.EXPRESSION_8BIT_RELATIVE ) )
          {
            if ( valueGroup.Expression == Opcode.OpcodePartialExpression.EXPRESSION_8BIT_RELATIVE )
            {
              ByteValue = ByteValue - AddressStart - 2;
            }

            if ( !ValidByteValue( ByteValue ) )
            {
              AddError( lineIndex,
                        Types.ErrorCode.E1002_VALUE_OUT_OF_BOUNDS_BYTE,
                        "Value out of bounds for byte, needs to be >= -128 and <= 255. Expression:" + TokensToExpression( Tokens ),
                        Tokens[0].StartPos,
                        Tokens[Tokens.Count - 1].EndPos + 1 - Tokens[0].StartPos );
              ByteValue = (byte)ByteValue;
              return false;
            }
            ByteValue = (byte)ByteValue;
          }
          else if ( valueGroup.Expression == Opcode.OpcodePartialExpression.EXPRESSION_15BIT )
          {
            if ( !Valid15BitValue( ByteValue ) )
            {
              AddError( lineIndex,
                        Types.ErrorCode.E1017_VALUE_OUT_OF_BOUNDS_15BIT,
                        $"Value {ByteValue.ToString( "X" )} ({ByteValue}) is out of bounds for 15bit, needs to be >= -16384 and <= {0x7FFF}. Expression:{TokensToExpression( Tokens )}",
                        Tokens[0].StartPos,
                        Tokens[Tokens.Count - 1].EndPos + 1 - Tokens[0].StartPos );
              ByteValue = ( (ushort)ByteValue ) & 0x7fff;
              return false;
            }
            ByteValue = ( (ushort)ByteValue ) & 0x7fff;
          }
          else if ( ( valueGroup.Expression == Opcode.OpcodePartialExpression.EXPRESSION_16BIT )
          ||        ( valueGroup.Expression == Opcode.OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT )
          ||        ( valueGroup.Expression == Opcode.OpcodePartialExpression.EXPRESSION_16BIT_RELATIVE ) )
          {
            if ( valueGroup.Expression == Opcode.OpcodePartialExpression.EXPRESSION_16BIT_RELATIVE )
            {
              ByteValue = ByteValue - AddressStart - 2;
            }

            if ( !ValidWordValue( ByteValue ) )
            {
              AddError( lineIndex,
                        Types.ErrorCode.E1003_VALUE_OUT_OF_BOUNDS_WORD,
                        $"Value {ByteValue.ToString( "X" )} ({ByteValue}) is out of bounds for 16bit, needs to be >= {short.MinValue} and <= {ushort.MaxValue}. Expression:{TokensToExpression( Tokens )}",
                        Tokens[0].StartPos,
                        Tokens[Tokens.Count - 1].EndPos + 1 - Tokens[0].StartPos );
              ByteValue = (ushort)ByteValue;
              return false;
            }
            ByteValue = (ushort)ByteValue;
          }
          else if ( ( valueGroup.Expression == Opcode.OpcodePartialExpression.EXPRESSION_24BIT )
          ||        ( valueGroup.Expression == Opcode.OpcodePartialExpression.ENCAPSULATED_EXPRESSION_24BIT ) )
          {
            if ( !Valid24BitValue( ByteValue ) )
            {
              AddError( lineIndex,
                        Types.ErrorCode.E1013_VALUE_OUT_OF_BOUNDS_24BIT,
                        "Value out of bounds for 24bit, needs to be >= -8388608 and <= 0x00ffffff. Expression:" + TokensToExpression( Tokens ),
                        Tokens[0].StartPos,
                        Tokens[Tokens.Count - 1].EndPos + 1 - Tokens[0].StartPos );
              ByteValue = ByteValue & 0x00ffffff;
              return false;
            }
            ByteValue = ByteValue & 0x00ffffff;
          }
          else if ( ( valueGroup.Expression == Opcode.OpcodePartialExpression.EXPRESSION_32BIT )
          ||        ( valueGroup.Expression == Opcode.OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT ) )
          {
            if ( !ValidDWordValue( ByteValue ) )
            {
              AddError( lineIndex,
                        Types.ErrorCode.E1015_VALUE_OUT_OF_BOUNDS_32BIT,
                        $"Value out of bounds for 32bit, needs to be >= {int.MinValue} and <= {uint.MaxValue}. Expression:{TokensToExpression( Tokens )}",
                        Tokens[0].StartPos,
                        Tokens[Tokens.Count - 1].EndPos + 1 - Tokens[0].StartPos );
              ByteValue = ByteValue & 0xffffffff;
              return false;
            }
            ByteValue = ByteValue & 0xffffffff;
          }
        }
      }
      else if ( OpcodeExpression.Type == Opcode.OpcodePartialExpression.EXPRESSION_7BIT )
      {
        if ( !Valid7BitValue( ByteValue ) )
        {
          AddError( lineIndex,
                    Types.ErrorCode.E1016_VALUE_OUT_OF_BOUNDS_7BIT,
                    "Value out of bounds for 7 bit, needs to be >= -64 and <= 127. Expression:" + TokensToExpression( Tokens ),
                    Tokens[0].StartPos,
                    Tokens[Tokens.Count - 1].EndPos + 1 - Tokens[0].StartPos );
          ByteValue = ( (byte)ByteValue ) & 0x7f;
          return false;
        }
        ByteValue = ( (byte)ByteValue ) & 0x7f;
      }
      else if ( ( OpcodeExpression.Type == Opcode.OpcodePartialExpression.EXPRESSION_8BIT )
      ||        ( OpcodeExpression.Type == Opcode.OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT )
      ||        ( OpcodeExpression.Type == Opcode.OpcodePartialExpression.EXPRESSION_8BIT_RELATIVE ) )
      {
        if ( OpcodeExpression.Type == Opcode.OpcodePartialExpression.EXPRESSION_8BIT_RELATIVE )
        {
          ByteValue = ByteValue - AddressStart - 2;
        }

        if ( !ValidByteValue( ByteValue ) )
        {
          AddError( lineIndex,
                    Types.ErrorCode.E1002_VALUE_OUT_OF_BOUNDS_BYTE,
                    "Value out of bounds for byte, needs to be >= -128 and <= 255. Expression:" + TokensToExpression( Tokens ),
                    Tokens[0].StartPos,
                    Tokens[Tokens.Count - 1].EndPos + 1 - Tokens[0].StartPos );
          ByteValue = (byte)ByteValue;
          return false;
        }
        ByteValue = (byte)ByteValue;
      }
      else if ( OpcodeExpression.Type == Opcode.OpcodePartialExpression.EXPRESSION_15BIT )
      {
        if ( !Valid15BitValue( ByteValue ) )
        {
          AddError( lineIndex,
                    Types.ErrorCode.E1017_VALUE_OUT_OF_BOUNDS_15BIT,
                    $"Value {ByteValue.ToString( "X" )} ({ByteValue}) is out of bounds for 15bit, needs to be >= -16384 and <= {0x7FFF}. Expression:{TokensToExpression( Tokens )}",
                    Tokens[0].StartPos,
                    Tokens[Tokens.Count - 1].EndPos + 1 - Tokens[0].StartPos );
          ByteValue = ( (ushort)ByteValue ) & 0x7fff;
          return false;
        }
        ByteValue = ( (ushort)ByteValue ) & 0x7fff;
      }
      else if ( ( OpcodeExpression.Type == Opcode.OpcodePartialExpression.EXPRESSION_16BIT )
      ||        ( OpcodeExpression.Type == Opcode.OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT )
      ||        ( OpcodeExpression.Type == Opcode.OpcodePartialExpression.EXPRESSION_16BIT_RELATIVE ) )
      {
        if ( OpcodeExpression.Type == Opcode.OpcodePartialExpression.EXPRESSION_16BIT_RELATIVE )
        {
          ByteValue = ByteValue - AddressStart - 2;
        }

        if ( !ValidWordValue( ByteValue ) )
        {
          AddError( lineIndex,
                    Types.ErrorCode.E1003_VALUE_OUT_OF_BOUNDS_WORD,
                    $"Value {ByteValue.ToString( "X" )} ({ByteValue}) is out of bounds for 16bit, needs to be >= {short.MinValue} and <= {ushort.MaxValue}. Expression:{TokensToExpression( Tokens )}",
                    Tokens[0].StartPos,
                    Tokens[Tokens.Count - 1].EndPos + 1 - Tokens[0].StartPos );
          ByteValue = (ushort)ByteValue;
          return false;
        }
        ByteValue = (ushort)ByteValue;
      }
      else if ( ( OpcodeExpression.Type == Opcode.OpcodePartialExpression.EXPRESSION_24BIT )
      ||        ( OpcodeExpression.Type == Opcode.OpcodePartialExpression.ENCAPSULATED_EXPRESSION_24BIT ) )
      {
        if ( !Valid24BitValue( ByteValue ) )
        {
          AddError( lineIndex,
                    Types.ErrorCode.E1013_VALUE_OUT_OF_BOUNDS_24BIT,
                    "Value out of bounds for 24bit, needs to be >= -8388608 and <= 0x00ffffff. Expression:" + TokensToExpression( Tokens ),
                    Tokens[0].StartPos,
                    Tokens[Tokens.Count - 1].EndPos + 1 - Tokens[0].StartPos );
          ByteValue = ByteValue & 0x00ffffff;
          return false;
        }
        ByteValue = ByteValue & 0x00ffffff;
      }
      else if ( ( OpcodeExpression.Type == Opcode.OpcodePartialExpression.EXPRESSION_32BIT )
      ||        ( OpcodeExpression.Type == Opcode.OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT ) )
      {
        if ( !ValidDWordValue( ByteValue ) )
        {
          AddError( lineIndex,
                    Types.ErrorCode.E1015_VALUE_OUT_OF_BOUNDS_32BIT,
                    $"Value out of bounds for 32bit, needs to be >= {int.MinValue} and <= {uint.MaxValue}. Expression:{TokensToExpression( Tokens )}",
                    Tokens[0].StartPos,
                    Tokens[Tokens.Count - 1].EndPos + 1 - Tokens[0].StartPos );
          ByteValue = ByteValue & 0xffffffff;
          return false;
        }
        ByteValue = ByteValue & 0xffffffff;
      }
      return true;
    }



  }
}
