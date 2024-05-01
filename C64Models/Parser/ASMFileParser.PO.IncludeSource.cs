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
    private ParseLineResult POIncludeSource( string ParentFilename, List<TokenInfo> lineTokenInfos, ref int lineIndex, ref string[] Lines )
    {
      string          subFilename = "";
      PathResolving   resolving = PathResolving.FROM_FILE;
      bool            singleInclude = false;

      if ( !ParseLineInParameters( lineTokenInfos, 1, lineTokenInfos.Count - 1, lineIndex, false, out List<List<TokenInfo>> parms ) )
      {
        return ParseLineResult.RETURN_NULL;
      }

      if ( m_AssemblerSettings.IncludeHasOnlyFilename )
      {
        // PDS style
        if ( parms.Count != 1 )
        {
          AddError( lineIndex,
                  Types.ErrorCode.E1302_MALFORMED_MACRO,
                  "Expecting valid file name",
                  lineTokenInfos[0].StartPos,
                  lineTokenInfos[0].Length );
          return ParseLineResult.RETURN_NULL;
        }
        else
        {
          subFilename = TokensToExpression( lineTokenInfos, 1, lineTokenInfos.Count - 1 );
        }
      }
      else if ( ( parms.Count >= 1 )
      &&        ( parms.Count <= 2 )
      &&        ( parms[0].Count == 1 )
      &&        ( parms[0][0].Type == Types.TokenInfo.TokenType.LITERAL_STRING ) )
      {
        // regular include
        subFilename = lineTokenInfos[1].Content.Substring( 1, lineTokenInfos[1].Length - 2 );
        if ( ( parms.Count == 2 )
        &&   ( ( parms[1].Count != 1 )
        ||     ( parms[1][0].Type != TokenInfo.TokenType.LABEL_GLOBAL )
        ||     ( parms[1][0].Content.ToUpper() != "ONCE" ) ) )
        {
          AddError( lineIndex,
                  Types.ErrorCode.E1302_MALFORMED_MACRO,
                  "Trailing argument must be 'ONCE'",
                  parms[1][0].StartPos,
                  parms[1].Last().EndPos - parms[1][0].StartPos + 1 );
          return ParseLineResult.RETURN_NULL;
        }
        singleInclude = ( parms.Count == 2 );
      }
      else if ( ( parms.Count >= 1 )
      &&        ( parms.Count <= 2 )
      &&        ( parms[0].Count >= 3 )
      &&        ( parms[0][0].Content == "<" )
      &&        ( parms[0].Last().Content == ">" ) )
      {
        // library include
        subFilename = TokensToExpression( lineTokenInfos, 2, lineTokenInfos.Count - 3 );
        resolving = PathResolving.FROM_LIBRARIES_PATH;
        if ( ( parms.Count == 2 )
        &&   ( ( parms[1].Count != 1 )
        ||     ( parms[1][0].Type != TokenInfo.TokenType.LABEL_GLOBAL )
        ||     ( parms[1][0].Content.ToUpper() != "ONCE" ) ) )
        {
          AddError( lineIndex,
                  Types.ErrorCode.E1302_MALFORMED_MACRO,
                  "Trailing argument must be 'ONCE'",
                  parms[1][0].StartPos,
                  parms[1].Last().EndPos - parms[1][0].StartPos + 1 );
          return ParseLineResult.RETURN_NULL;
        }
        singleInclude = ( parms.Count == 2 );
      }
      else
      {
        AddError( lineIndex,
                  Types.ErrorCode.E1302_MALFORMED_MACRO,
                  "Expecting file name, either \"filename\" or <library filename>[,once]",
                  lineTokenInfos[0].StartPos,
                  lineTokenInfos.Last().EndPos - lineTokenInfos[0].StartPos + 1 );
        return ParseLineResult.RETURN_NULL;
      }

      if ( !m_ASMFileInfo.FindTrueLineSource( lineIndex, out string filename, out int localIndex ) )
      {
        DumpSourceInfos( OrigLines );
        AddError( lineIndex, Types.ErrorCode.E1401_INTERNAL_ERROR, "Includes caused a problem" );
        return ParseLineResult.RETURN_NULL;
      }

      if ( m_AssemblerSettings.IncludeSourceIsAlwaysUsingLibraryPathAndFile )
      {
        resolving = PathResolving.FROM_FILE_AND_LIBRARIES_PATH;
      }

      SourceInfoLog( "Include file " + subFilename + ", resolving " + resolving );
      if ( m_LoadedFiles[ParentFilename] == null )
      {
        m_LoadedFiles[ParentFilename] = new GR.Collections.Set<string>();
      }

      if ( GR.Path.IsPathEqual( ParentFilename, subFilename ) )
      {
        AddError( lineIndex, Types.ErrorCode.E1400_CIRCULAR_INCLUSION, "Circular inclusion in line " + lineIndex );
        return ParseLineResult.RETURN_NULL;
      }

      if ( DoLogSourceInfo )
      {
        string subFilenameFull2 = GR.Path.Append( System.IO.Path.GetDirectoryName( ParentFilename ), subFilename );
        if ( !OrigLines.ContainsKey( subFilenameFull2 ) )
        {
          OrigLines.Add( subFilenameFull2, new string[Lines.Length] );
          Array.Copy( Lines, OrigLines[subFilenameFull2], Lines.Length );
        }
      }

      if ( ( singleInclude )
      &&   ( m_AlreadyIncludedSingleIncludeFiles.Contains( subFilename ) ) )
      {
        // do not re-include single include files
        return ParseLineResult.CALL_CONTINUE;
      }
      if ( singleInclude )
      {
        m_AlreadyIncludedSingleIncludeFiles.Add( subFilename );
      }

      if ( m_LoadedFiles[ParentFilename].Contains( subFilename ) )
      {
        AddError( lineIndex, Types.ErrorCode.E1400_CIRCULAR_INCLUSION, "Circular inclusion in line " + lineIndex );
        return ParseLineResult.RETURN_NULL;
      }
      m_LoadedFiles[ParentFilename].Add( subFilename );

      string[]  subFile = null;
      string    subFilenameFull = subFilename;
      bool      foundAFile = false;

      if ( ( resolving == PathResolving.FROM_FILE )
      ||   ( resolving == PathResolving.FROM_FILE_AND_LIBRARIES_PATH ) )
      {
        subFilenameFull = BuildFullPath( System.IO.Path.GetDirectoryName( ParentFilename ), subFilename );
        foundAFile = System.IO.File.Exists( subFilenameFull );
      }

      if ( ( !foundAFile )
      &&   ( ( resolving == PathResolving.FROM_LIBRARIES_PATH )
      ||     ( resolving == PathResolving.FROM_FILE_AND_LIBRARIES_PATH ) ) )
      {
        subFilenameFull = DetermineFullLibraryFilePath( subFilename );
        if ( string.IsNullOrEmpty( subFilenameFull ) )
        {
          if ( ( resolving == PathResolving.FROM_FILE )
          ||   ( resolving == PathResolving.FROM_FILE_AND_LIBRARIES_PATH ) )
          {
            var msg = AddError( lineIndex, Types.ErrorCode.E1307_FILENAME_INCOMPLETE, "Can't find matching file for '" + subFilename + "' to include in line " + ( lineIndex + 1 ) );

            foreach ( var lib in m_CompileConfig.LibraryFiles )
            {
              msg.AddMessage( "Tried with " + lib, null, -1 );
            }
            return ParseLineResult.RETURN_NULL;
          }

          AddError( lineIndex, Types.ErrorCode.E1307_FILENAME_INCOMPLETE, "Can't find matching library file for '" + subFilename + "' in line " + ( lineIndex + 1 ) );
          return ParseLineResult.RETURN_NULL;
        }
      }

      if ( GR.Path.IsPathEqual( ParentFilename, subFilenameFull ) )
      {
        AddError( lineIndex, Types.ErrorCode.E1400_CIRCULAR_INCLUSION, "Circular inclusion in line " + lineIndex );
        return ParseLineResult.RETURN_NULL;
      }

      ExternallyIncludedFiles.Add( subFilenameFull );
      //Debug.Log( "Read subfile " + subFilename );
      try
      {
        subFile = System.IO.File.ReadAllLines( subFilenameFull, m_CompileConfig.Encoding );
      }
      catch ( System.IO.IOException )
      {
        AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read file " + subFilenameFull );
        return ParseLineResult.RETURN_NULL;
      }

      CleanLines( subFile );
      if ( subFile.Length == 0 )
      {
        // included empty file messes up source info, skip to adding it
        return ParseLineResult.CALL_CONTINUE;
      }

      Types.ASM.SourceInfo sourceInfo = new Types.ASM.SourceInfo();
      sourceInfo.Filename = subFilenameFull;
      sourceInfo.FullPath = subFilenameFull;
      sourceInfo.GlobalStartLine = lineIndex;
      sourceInfo.LineCount = subFile.Length;
      sourceInfo.FilenameParent = ParentFilename;

      SourceInfoLog( "-include at global index " + lineIndex );
      SourceInfoLog( "-has " + subFile.Length + " lines" );

      sourceInfo.GlobalStartLine = lineIndex + 1;
      InsertSourceInfo( sourceInfo );

      // keep !src line on top
      string[] result = new string[Lines.Length + subFile.Length];

      System.Array.Copy( Lines, 0, result, 0, lineIndex + 1 );
      System.Array.Copy( subFile, 0, result, lineIndex + 1, subFile.Length );

      // this keeps the !source line in the final code, makes working with source infos easier though
      System.Array.Copy( Lines, lineIndex + 1, result, lineIndex + subFile.Length + 1, Lines.Length - lineIndex - 1 );

      // replace !source with empty line (otherwise source infos would have one line more!)
      result[lineIndex] = "";

      Lines = result;

      m_ASMFileInfo.LineInfo.Remove( lineIndex );

      --lineIndex;
      return ParseLineResult.CALL_CONTINUE;
    }



  }
}
