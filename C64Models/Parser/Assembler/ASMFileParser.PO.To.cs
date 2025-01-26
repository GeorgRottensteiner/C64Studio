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
        string filename = parms[0][1].Content.Substring( 1, parms[0][1].Length - 2 );
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
        if ( numExtraArgs != 0 )
        {
          if ( !POToHandleExtraArguments( parms, numExtraArgs ) )
          {
            return ParseLineResult.ERROR_ABORT;
          }
        }
      }
      return ParseLineResult.OK;
    }



    private bool POToHandleExtraArguments( List<List<TokenInfo>> LineParams, int NumExtraArgs )
    {
      switch ( m_CompileTarget.Type )
      {
        case CompileTargetType.CARTRIDGE_NES:
          if ( ( !EvaluateTokens( _ParseContext.LineIndex, LineParams[2], out SymbolInfo nesPRGUnits ) )
          ||   ( !EvaluateTokens( _ParseContext.LineIndex, LineParams[3], out SymbolInfo nesChrUnits ) )
          ||   ( !EvaluateTokens( _ParseContext.LineIndex, LineParams[4], out SymbolInfo nesMapper ) )
          ||   ( !EvaluateTokens( _ParseContext.LineIndex, LineParams[5], out SymbolInfo nesMirroring ) ) )
          {
            AddError( _ParseContext.LineIndex,
                      Types.ErrorCode.E1302_MALFORMED_MACRO,
                      $"Failed to evaluate extra arguments for {m_CompileTarget.Type}" );
            return false;
          }
          {
            int   prgUnits  = nesPRGUnits.ToInt32();
            int   chrUnits  = nesChrUnits.ToInt32();
            int   mapper    = nesMapper.ToInt32();
            int   mirroring = nesMirroring.ToInt32();
            if ( ( !ValidateRange( prgUnits, 0, 64, $"Number of PRG units must be a value >= 0 and <= 64", nesPRGUnits ) )
            ||   ( !ValidateRange( chrUnits, 0, 64, $"Number of CHR units must be a value >= 0 and <= 64", nesChrUnits ) )
            ||   ( !ValidateRange( mapper, 0, 255, $"Mapper number must be a value >= 0 and <= 255", nesMapper ) )
            ||   ( !ValidateRange( mirroring, 0, 15, $"Mirroring value must be a value >= 0 and <= 15", nesMirroring ) ) )
            {
              return false;
            }
            m_CompileTarget.NESPrgROMUnits16k = (byte)prgUnits;
            m_CompileTarget.NESChrROMUnits8k  = (byte)chrUnits;
            m_CompileTarget.NESMapper         = (byte)mapper;
            m_CompileTarget.NESMirroring      = (byte)mirroring;
          }
          return true;
      }
      AddError( _ParseContext.LineIndex,
                Types.ErrorCode.E1302_MALFORMED_MACRO,
                $"Missing POTo extra argument implementation for {m_CompileTarget.Type}" );
      return false;
    }



    private bool ValidateRange( int Value, int Min, int Max, string ErrorMessage, SymbolInfo Symbol )
    {
      if ( ( Value < Min )
      ||   ( Value > Max ) )
      {
        AddError( _ParseContext.LineIndex,
                  Types.ErrorCode.E1302_MALFORMED_MACRO,
                  ErrorMessage,
                  Symbol.CharIndex, Symbol.Length );
        return false;
      }
      return true;
    }



  }
}
