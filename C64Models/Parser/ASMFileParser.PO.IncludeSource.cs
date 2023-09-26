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
    private ParseLineResult POIncludeSource( PathResolving Resolving, string subFilename, string ParentFilename, ref int lineIndex, ref string[] Lines )
    {
      if ( m_AssemblerSettings.IncludeSourceIsAlwaysUsingLibraryPathAndFile )
      {
        Resolving = PathResolving.FROM_FILE_AND_LIBRARIES_PATH;
      }

      SourceInfoLog( "Include file " + subFilename + ", resolving " + Resolving );
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

      if ( m_LoadedFiles[ParentFilename].Contains( subFilename ) )
      {
        AddError( lineIndex, Types.ErrorCode.E1400_CIRCULAR_INCLUSION, "Circular inclusion in line " + lineIndex );
        return ParseLineResult.RETURN_NULL;
      }
      m_LoadedFiles[ParentFilename].Add( subFilename );

      string[]  subFile = null;
      string    subFilenameFull = subFilename;
      bool      foundAFile = false;

      if ( ( Resolving == PathResolving.FROM_FILE )
      ||   ( Resolving == PathResolving.FROM_FILE_AND_LIBRARIES_PATH ) )
      {
        subFilenameFull = BuildFullPath( System.IO.Path.GetDirectoryName( ParentFilename ), subFilename );
        foundAFile = System.IO.File.Exists( subFilenameFull );
      }

      if ( ( !foundAFile )
      &&   ( ( Resolving == PathResolving.FROM_LIBRARIES_PATH )
      ||     ( Resolving == PathResolving.FROM_FILE_AND_LIBRARIES_PATH ) ) )
      {
        subFilenameFull = DetermineFullLibraryFilePath( subFilename );
        if ( string.IsNullOrEmpty( subFilenameFull ) )
        {
          if ( ( Resolving == PathResolving.FROM_FILE )
          ||   ( Resolving == PathResolving.FROM_FILE_AND_LIBRARIES_PATH ) )
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

      ASMFileInfo.LineInfo.Remove( lineIndex );

      --lineIndex;
      return ParseLineResult.CALL_CONTINUE;
    }



  }
}
