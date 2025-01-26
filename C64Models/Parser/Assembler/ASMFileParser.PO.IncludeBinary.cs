using GR.Memory;
using RetroDevStudio.Formats;
using RetroDevStudio.Parser;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RetroDevStudio.Parser
{
  public partial class ASMFileParser : ParserBase
  {
    private ParseLineResult POIncludeBinary( MacroInfo.PseudoOpType OpType, List<Types.TokenInfo> lineTokenInfos, int lineIndex, Types.ASM.LineInfo info, out int lineSizeInBytes )
    {
      lineSizeInBytes = 0;

      List<Types.TokenInfo> paramsFile = new List<Types.TokenInfo>();
      List<Types.TokenInfo> paramsSize = new List<Types.TokenInfo>();
      List<Types.TokenInfo> paramsSkip = new List<Types.TokenInfo>();

      int maxParams = 3;
      if ( OpType == MacroInfo.PseudoOpType.INCLUDE_BINARY_TASM )
      {
        // TASM only has optional skip param
        maxParams = 2;
      }

      if ( !m_AssemblerSettings.IncludeHasOnlyFilename )
      {
        ParseLineInParameters( lineTokenInfos, 1, lineTokenInfos.Count - 1, lineIndex, true, out List<List<TokenInfo>> paramList );

        if ( ( paramList.Count > maxParams )
        ||   ( paramList.Count < 1 )
        ||   ( paramList[0].Count != 1 ) )
        {
          if ( OpType == MacroInfo.PseudoOpType.INCLUDE_BINARY_TASM )
          {
            AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Macro not formatted as expected. Expected " + MacroByType( MacroInfo.PseudoOpType.INCLUDE_BINARY ) + " <Filename>[,<Skip>]" );
          }
          else
          {
            AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Macro not formatted as expected. Expected " + MacroByType( MacroInfo.PseudoOpType.INCLUDE_BINARY ) + " <Filename>[,<Size>[,<Skip>]]" );
          }
          return ParseLineResult.RETURN_NULL;
        }
        paramsFile = paramList[0];
        if ( paramList.Count >= 2 )
        {
          paramsSize = paramList[1];
        }
        if ( ( OpType != MacroInfo.PseudoOpType.INCLUDE_BINARY_TASM )
        &&   ( paramList.Count >= 3 ) )
        {
          paramsSkip = paramList[2];
        }
      }
      else
      {
        for ( int i = 1; i < lineTokenInfos.Count; ++i )
        {
          paramsFile.Add( lineTokenInfos[i] );
        }
      }

      string subFilename = "";

      if ( m_AssemblerSettings.IncludeExpectsStringLiteral )
      {
        if ( ( !paramsFile[0].Content.StartsWith( "\"" ) )
        ||   ( !paramsFile[0].Content.EndsWith( "\"" ) )
        ||   ( paramsFile[0].Length <= 2 ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1307_FILENAME_INCOMPLETE, "Expected proper file name between apostrophes" );
          return ParseLineResult.RETURN_NULL;
        }
        subFilename = DeQuote( paramsFile[0].Content );
      }
      else
      {
        subFilename = TokensToExpression( paramsFile );
      }

      int     fileSize = -1;
      int     fileSkip = -1;
      bool    fileSizeValid = EvaluateTokens( lineIndex, paramsSize, out SymbolInfo fileSizeSymbol );
      bool    fileSkipValid = EvaluateTokens( lineIndex, paramsSkip, out SymbolInfo fileSkipSymbol );

      if ( OpType != MacroInfo.PseudoOpType.INCLUDE_BINARY_TASM )
      {
        if ( ( paramsSize.Count > 0 )
        &&   ( !fileSizeValid ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Cannot evaluate size argument" );
          return ParseLineResult.RETURN_NULL;
        }
      }
      else
      {
        fileSizeValid = false;
        fileSize      = -1;
      }
      if ( ( paramsSkip.Count > 0 )
      &&   ( !fileSkipValid ) )
      {
        AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Cannot evaluate skip argument" );
        return ParseLineResult.RETURN_NULL;
      }
      if ( OpType != MacroInfo.PseudoOpType.INCLUDE_BINARY_TASM )
      {
        if ( fileSizeValid )
        {
          fileSize = fileSizeSymbol.ToInt32();
        }
      }
      if ( fileSkipValid )
      {
        fileSkip = fileSkipSymbol.ToInt32();
      }
      // special case, allow 0 length as all bytes
      if ( ( fileSizeValid )
      &&   ( fileSize == 0 ) )
      {
        fileSizeValid = false;
      }

      GR.Memory.ByteBuffer    subFile = null;

      try
      {
        subFile = GR.IO.File.ReadAllBytes( BuildFullPath( m_DocBasePath, subFilename ) );
        if ( subFile == null )
        {
          AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read file " + GR.Path.Append( m_DocBasePath, subFilename ) );
          return ParseLineResult.RETURN_NULL;
        }
      }
      catch ( System.IO.IOException )
      {
        AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read file " + GR.Path.Append( m_DocBasePath, subFilename ) );
        return ParseLineResult.RETURN_NULL;
      }
      ExternallyIncludedFiles.Add( GR.Path.Append( m_DocBasePath, subFilename ) );

      int             maxBytes = (int)subFile.Length;
      if ( !fileSkipValid )
      {
        fileSkip = 0;
      }
      if ( !fileSizeValid )
      {
        fileSize = maxBytes - fileSkip;
      }

      if ( fileSkip + fileSize > maxBytes )
      {
        // more bytes requested than the file holds
        // as ACME fills up with zeroes we follow along
        uint  bytesToAdd = (uint)( fileSkip + fileSize - maxBytes );
        subFile.Append( new GR.Memory.ByteBuffer( bytesToAdd ) );
      }
      if ( fileSkip > maxBytes )
      {
        AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Trying to skip more bytes than the file " + GR.Path.Append( m_DocBasePath, subFilename ) + " holds" );
        return ParseLineResult.RETURN_NULL;
      }

      info.LineData = subFile.SubBuffer( fileSkip, fileSize );
      info.NumBytes = (int)info.LineData.Length;
      lineSizeInBytes = fileSize;

      return ParseLineResult.OK;
    }



  }
}
