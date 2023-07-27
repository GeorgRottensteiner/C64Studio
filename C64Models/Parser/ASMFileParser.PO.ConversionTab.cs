using GR.Collections;
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
    private Map<byte, byte> POConversionTab( string PO, Map<byte, byte> textCodeMapping, int lineIndex, List<TokenInfo> lineTokenInfos )
    {
      if ( lineTokenInfos.Count < 2 )
      {
        AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, $"Expected {PO} <Type = raw or scr or pet or mapping list>" );
      }
      else if ( lineTokenInfos[1].Content.ToUpper() == "RAW" )
      {
        textCodeMapping = m_TextCodeMappingRaw;
      }
      else if ( lineTokenInfos[1].Content.ToUpper() == "SCR" )
      {
        textCodeMapping = m_TextCodeMappingScr;
      }
      else if ( lineTokenInfos[1].Content.ToUpper() == "PET" )
      {
        textCodeMapping = m_TextCodeMappingPet;
      }
      else
      {
        // expecting mapping table
        if ( ( textCodeMapping == m_TextCodeMappingPet )
        ||   ( textCodeMapping == m_TextCodeMappingRaw )
        ||   ( textCodeMapping == m_TextCodeMappingScr ) )
        {
          // only reset mapping if previously mapping was a predefined one
          textCodeMapping = new GR.Collections.Map<byte, byte>();
        }
        else
        {
          // create new instance to avoid modifying previously stored mappings
          textCodeMapping = new GR.Collections.Map<byte, byte>( textCodeMapping );
        }

        GR.Memory.ByteBuffer data = new GR.Memory.ByteBuffer();

        int commaCount = 0;
        int startTokenIndex = 1;
        for ( int tokenIndex = 1; tokenIndex < lineTokenInfos.Count; ++tokenIndex )
        {
          string token = lineTokenInfos[tokenIndex].Content;

          if ( ( tokenIndex == 1 )
          &&   ( token == "#" ) )
          {
            // direct value?
            if ( ( lineTokenInfos.Count > 2 )
            &&   ( lineTokenInfos[2].Content != "#" )
            &&   ( lineTokenInfos[2].Content != "." ) )
            {
              // not a binary value
              continue;
            }
          }

          if ( token == "," )
          {
            if ( startTokenIndex < tokenIndex )
            {
              if ( EvaluateTokens( lineIndex, lineTokenInfos, startTokenIndex, tokenIndex - startTokenIndex, textCodeMapping, out SymbolInfo aByte ) )
              {
                data.AppendU8( (byte)aByte.ToInteger() );
              }
              else
              {
                // could not fully parse
                AddError( lineIndex, Types.ErrorCode.E1000_SYNTAX_ERROR, "Could not parse " + TokensToExpression( lineTokenInfos, startTokenIndex, lineTokenInfos.Count - startTokenIndex ) );
              }
            }
            ++commaCount;
            startTokenIndex = tokenIndex + 1;
          }
        }
        if ( startTokenIndex < lineTokenInfos.Count )
        {
          if ( EvaluateTokens( lineIndex, lineTokenInfos, startTokenIndex, lineTokenInfos.Count - startTokenIndex, textCodeMapping, out SymbolInfo aByte ) )
          {
            data.AppendU8( (byte)aByte.ToInteger() );
          }
          else
          {
            // could not fully parse
            AddError( lineIndex, Types.ErrorCode.E1000_SYNTAX_ERROR, "Could not parse " + TokensToExpression( lineTokenInfos, startTokenIndex, lineTokenInfos.Count - startTokenIndex ) );
          }
        }
        if ( ( data.Length % 2 ) != 0 )
        {
          AddError( lineIndex, Types.ErrorCode.E1000_SYNTAX_ERROR, "Mapping table must have pairs of bytes, found " + data.Length + " bytes" );
        }
        else
        {
          for ( int mapping = 0; mapping < data.Length / 2; ++mapping )
          {
            textCodeMapping[data.ByteAt( mapping * 2 )] = data.ByteAt( mapping * 2 + 1 );
          }
        }
      }

      return textCodeMapping;
    }



    private Map<byte, byte> POConversionTabTASS( string PO, Map<byte, byte> textCodeMapping, int lineIndex, List<TokenInfo> lineTokenInfos )
    {
      if ( lineTokenInfos.Count < 2 )
      {
        AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, $@"Expected {PO} ""screen"", ""none""" );
      }
      else if ( lineTokenInfos[1].Content.ToUpper() == "\"NONE\"" )
      {
        textCodeMapping = m_TextCodeMappingRaw;
      }
      else if ( lineTokenInfos[1].Content.ToUpper() == "\"SCREEN\"" )
      {
        textCodeMapping = m_TextCodeMappingScr;
      }
      else
      {
        // expecting custom mapping table
        var encodingName = lineTokenInfos[1].Content.Substring( 1, lineTokenInfos[1].Content.Length - 1 );
        if ( !_ParseContext.TextMappings.ContainsKey( encodingName ) )
        {
          _ParseContext.TextMappings[encodingName] = new Map<byte, byte>();
        }
        textCodeMapping = _ParseContext.TextMappings[encodingName];
      }

      return textCodeMapping;
    }



    private ParseLineResult POConversionTabTASSEntry( string PO, Map<byte, byte> textCodeMapping, int lineIndex, List<TokenInfo> lineTokenInfos, out Map<byte, byte> ResultingMapping )
    {
      ResultingMapping = textCodeMapping;
      var parseResult = ParseLineInParameters( lineTokenInfos, 1, lineTokenInfos.Count - 1, lineIndex, out List<List<TokenInfo>> lineParams );
      if ( parseResult != ParseLineResult.OK )
      {
        return parseResult;
      }
      int   firstIndex = 0;
      while ( firstIndex + 2 <= lineParams.Count )
      {
        if ( lineParams[firstIndex].Count == 1 )
        {
          // must be a string literal with two characters
          if ( ( lineParams[firstIndex][0].Type != TokenInfo.TokenType.LITERAL_STRING )
          ||   ( lineParams[firstIndex][0].Content.Length != 4 ) )
          {
            AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, $@"Expected a two character sized string literal" );
            return ParseLineResult.ERROR_ABORT;
          }
          // 2nd must be a byte value (or result in one)
          if ( !EvaluateTokens( lineIndex, lineParams[firstIndex + 1], 0, lineParams[firstIndex + 1].Count, textCodeMapping, out SymbolInfo aByte ) )
          {
            // could not fully parse
            AddError( lineIndex, Types.ErrorCode.E1000_SYNTAX_ERROR, "Could not parse " + TokensToExpression( lineParams[firstIndex + 1] ) );
            return ParseLineResult.ERROR_ABORT;
          }
          char  firstChar = lineParams[firstIndex][0].Content[1];
          char  lastChar = lineParams[firstIndex][0].Content[2];

          for ( char c = firstChar; c <= lastChar; ++c )
          {
            textCodeMapping[(byte)c] = (byte)( aByte.ToInt32() + c - firstChar );
          }
        }
        firstIndex += 2;
      }
      if ( firstIndex < lineParams.Count )
      {
        AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, $@"{PO} has incomplete mapping at the end" );
        return ParseLineResult.ERROR_ABORT;
      }
      /*
      else
      {
        // expecting mapping table
        if ( ( textCodeMapping == m_TextCodeMappingPet )
        ||   ( textCodeMapping == m_TextCodeMappingRaw )
        ||   ( textCodeMapping == m_TextCodeMappingScr ) )
        {
          // only reset mapping if previously mapping was a predefined one
          textCodeMapping = new GR.Collections.Map<byte, byte>();
        }
        else
        {
          // create new instance to avoid modifying previously stored mappings
          textCodeMapping = new GR.Collections.Map<byte, byte>( textCodeMapping );
        }

        GR.Memory.ByteBuffer data = new GR.Memory.ByteBuffer();

        int commaCount = 0;
        int startTokenIndex = 1;
        for ( int tokenIndex = 1; tokenIndex < lineTokenInfos.Count; ++tokenIndex )
        {
          string token = lineTokenInfos[tokenIndex].Content;

          if ( ( tokenIndex == 1 )
          &&   ( token == "#" ) )
          {
            // direct value?
            if ( ( lineTokenInfos.Count > 2 )
            &&   ( lineTokenInfos[2].Content != "#" )
            &&   ( lineTokenInfos[2].Content != "." ) )
            {
              // not a binary value
              continue;
            }
          }

          if ( token == "," )
          {
            if ( startTokenIndex < tokenIndex )
            {
              if ( EvaluateTokens( lineIndex, lineTokenInfos, startTokenIndex, tokenIndex - startTokenIndex, textCodeMapping, out SymbolInfo aByte ) )
              {
                data.AppendU8( (byte)aByte.ToInteger() );
              }
              else
              {
                // could not fully parse
                AddError( lineIndex, Types.ErrorCode.E1000_SYNTAX_ERROR, "Could not parse " + TokensToExpression( lineTokenInfos, startTokenIndex, lineTokenInfos.Count - startTokenIndex ) );
                parseResult = ParseLineResult.ERROR_ABORT;
              }
            }
            ++commaCount;
            startTokenIndex = tokenIndex + 1;
          }
        }
        if ( startTokenIndex < lineTokenInfos.Count )
        {
          if ( EvaluateTokens( lineIndex, lineTokenInfos, startTokenIndex, lineTokenInfos.Count - startTokenIndex, textCodeMapping, out SymbolInfo aByte ) )
          {
            data.AppendU8( (byte)aByte.ToInteger() );
          }
          else
          {
            // could not fully parse
            AddError( lineIndex, Types.ErrorCode.E1000_SYNTAX_ERROR, "Could not parse " + TokensToExpression( lineTokenInfos, startTokenIndex, lineTokenInfos.Count - startTokenIndex ) );
            parseResult = ParseLineResult.ERROR_ABORT;
          }
        }
        if ( ( data.Length % 2 ) != 0 )
        {
          AddError( lineIndex, Types.ErrorCode.E1000_SYNTAX_ERROR, "Mapping table must have pairs of bytes, found " + data.Length + " bytes" );
          parseResult = ParseLineResult.ERROR_ABORT;
        }
        else
        {
          for ( int mapping = 0; mapping < data.Length / 2; ++mapping )
          {
            textCodeMapping[data.ByteAt( mapping * 2 )] = data.ByteAt( mapping * 2 + 1 );
          }
        }
      }*/

      ResultingMapping = textCodeMapping;
      return parseResult;
    }



  }
}
