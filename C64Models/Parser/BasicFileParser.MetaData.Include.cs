using RetroDevStudio.Parser;



namespace RetroDevStudio.Parser
{
  public partial class BasicFileParser : ParserBase
  {
    private bool MetaDataInclude( int LineIndex, string MetaData, string MetaDataParams )
    {
      // followed by string literal for file name
      if ( ( !MetaDataParams.StartsWith( "\"" ) )
      ||   ( !MetaDataParams.EndsWith( "\"" ) )
      ||   ( MetaDataParams.Length < 2 ) )
      {
        AddError( LineIndex, Types.ErrorCode.E3007_BASIC_MALFORMED_METADATA, "Include expects a file name" );
        return false;
      }

      string    includeFile = MetaDataParams.Substring( 1, MetaDataParams.Length - 2 );
      string    subFilenameFull   = System.IO.Path.Combine( System.IO.Path.GetDirectoryName( m_CompileConfig.InputFile ), includeFile );
      if ( ( string.IsNullOrEmpty( includeFile ) )
      ||   ( !System.IO.File.Exists( subFilenameFull ) ) )
      {
        AddError( LineIndex, Types.ErrorCode.E2000_FILE_OPEN_ERROR, $"Could not open {subFilenameFull}" );
        return false;
      }

      int localIndex = 0;
      string filename = "";
      if ( !ASMFileInfo.FindTrueLineSource( LineIndex, out filename, out localIndex ) )
      {
        AddError( LineIndex, Types.ErrorCode.E1401_INTERNAL_ERROR, "Includes caused a problem" );
        return false;
      }

      string[]  subFile;

      try
      {
        subFile = System.IO.File.ReadAllLines( subFilenameFull, m_CompileConfig.Encoding );
      }
      catch ( System.IO.IOException )
      {
        AddError( LineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read file " + subFilenameFull );
        return false;
      }

      CleanLines( subFile );
      if ( subFile.Length == 0 )
      {
        // included empty file messes up source info, skip to adding it
        return true; // ParseLineResult.CALL_CONTINUE;
      }

      Types.ASM.SourceInfo sourceInfo = new Types.ASM.SourceInfo();
      sourceInfo.Filename = subFilenameFull;
      sourceInfo.FullPath = subFilenameFull;
      sourceInfo.GlobalStartLine = LineIndex;
      sourceInfo.LineCount = subFile.Length;
      sourceInfo.FilenameParent = filename;

      //SourceInfoLog( "-include at global index " + LineIndex );
      //SourceInfoLog( "-has " + subFile.Length + " lines" );

      sourceInfo.GlobalStartLine = LineIndex + 1;
      InsertSourceInfo( sourceInfo );

      // keep !src line on top
      string[] result = new string[_Lines.Length + subFile.Length];

      System.Array.Copy( _Lines, 0, result, 0, LineIndex + 1 );
      System.Array.Copy( subFile, 0, result, LineIndex + 1, subFile.Length );

      // this keeps the !source line in the final code, makes working with source infos easier though
      System.Array.Copy( _Lines, LineIndex + 1, result, LineIndex + subFile.Length + 1, _Lines.Length - LineIndex - 1 );

      // replace !source with empty line (otherwise source infos would have one line more!)
      result[LineIndex] = "";

      _Lines = result;

      ASMFileInfo.LineInfo.Remove( LineIndex );

      //--LineIndex;

      return true;
    }



  }
}
