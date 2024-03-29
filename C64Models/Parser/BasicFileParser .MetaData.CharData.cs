using GR.Memory;
using RetroDevStudio.Formats;
using RetroDevStudio.Parser;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Security.Policy;

namespace RetroDevStudio.Parser
{
  public partial class BasicFileParser : ParserBase
  {
    private bool MetaDataCharData( int LineIndex, string MetaData, string MetaDataParams )
    {
      var  parms = PureTokenizeLine( MetaDataParams );

      var cleanedParms = new List<List<string>>();
      var curList = new List<string>();
      for ( int i = 0; i < parms.Tokens.Count; ++i )
      {
        if ( parms.Tokens[i].Content == "," )
        {
          cleanedParms.Add( curList );
          curList = new List<string>();
        }
        else
        {
          curList.Add( parms.Tokens[i].Content );
        }
      }
      if ( curList.Count > 0 )
      {
        cleanedParms.Add( curList );
      }

      // followed by string literal for file name
      if ( ( cleanedParms.Count != 5 )
      ||   ( cleanedParms[0].Count != 1 )
      ||   ( cleanedParms[1].Count > 1 )
      ||   ( cleanedParms[2].Count > 1 )
      ||   ( cleanedParms[3].Count != 1 )
      ||   ( cleanedParms[4].Count != 1 )
      ||   ( !cleanedParms[0][0].StartsWith( "\"" ) )
      ||   ( !cleanedParms[0][0].EndsWith( "\"" ) ) )
      {
        AddError( LineIndex, Types.ErrorCode.E3007_BASIC_MALFORMED_METADATA, "CharData expects <Filename>,<Offset>,<Count>,<Start Line No>,<Line Step>" );
        return false;
      }

      var filename = cleanedParms[0][0];
      string    includeFile = filename.Substring( 1, filename.Length - 2 );
      string    subFilenameFull   = System.IO.Path.Combine( System.IO.Path.GetDirectoryName( m_CompileConfig.InputFile ), includeFile );
      if ( ( string.IsNullOrEmpty( includeFile ) )
      ||   ( !System.IO.File.Exists( subFilenameFull ) ) )
      {
        AddError( LineIndex, Types.ErrorCode.E2000_FILE_OPEN_ERROR, $"Could not open {subFilenameFull}" );
        return false;
      }

      int localIndex = 0;
      string dummyFilename = "";
      if ( !m_ASMFileInfo.FindTrueLineSource( LineIndex, out dummyFilename, out localIndex ) )
      {
        AddError( LineIndex, Types.ErrorCode.E1401_INTERNAL_ERROR, "Includes caused a problem" );
        return false;
      }

      var charsetProject = new CharsetProject();

      try
      {
        var subFile = GR.IO.File.ReadAllBytes( subFilenameFull );
        charsetProject.ReadFromBuffer( subFile );
      }
      catch ( System.IO.IOException )
      {
        AddError( LineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read file " + subFilenameFull );
        return false;
      }

      int offset    = GR.Convert.ToI32( cleanedParms[1][0] );
      int count     = GR.Convert.ToI32( cleanedParms[2][0] );
      int startLine = GR.Convert.ToI32( cleanedParms[3][0] );
      int stepLine  = GR.Convert.ToI32( cleanedParms[4][0] );

      if ( ( count <= 0 )
      ||   ( offset >= charsetProject.TotalNumberOfCharacters )
      ||   ( offset + count <= 0 ) )
      {
        return true;
      }

      if ( offset < 0 )
      {
        count += offset;
        offset = 0;
      }
      if ( offset + count > charsetProject.TotalNumberOfCharacters )
      {
        count = charsetProject.TotalNumberOfCharacters - offset;
      }

      int characterSize = Lookup.NumBytesOfSingleCharacterBitmap( charsetProject.Mode );

      var spriteData = new GR.Memory.ByteBuffer( (uint)( count * characterSize ) );

      for ( int i = 0; i < count; ++i )
      {
        charsetProject.Characters[offset + i].Tile.Data.CopyTo( spriteData,
                                                            0,
                                                            (int)charsetProject.Characters[offset + i].Tile.Data.Length,
                                                            i * characterSize );
      }

      string basicDatas = Util.ToBASICData( spriteData, startLine, stepLine, 0, Settings.BASICDialect.SafeLineLength, false );

      string[]  newLines = basicDatas.Split( new string[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries );

      Types.ASM.SourceInfo sourceInfo = new Types.ASM.SourceInfo();
      sourceInfo.Filename         = subFilenameFull;
      sourceInfo.FullPath         = subFilenameFull;
      sourceInfo.GlobalStartLine  = LineIndex;
      sourceInfo.LineCount        = newLines.Length;
      sourceInfo.FilenameParent   = filename;
      sourceInfo.Source           = Types.ASM.SourceInfo.SourceInfoSource.MEDIA_INCLUDE;

      //SourceInfoLog( "-include at global index " + LineIndex );
      //SourceInfoLog( "-has " + subFile.Length + " lines" );

      sourceInfo.GlobalStartLine = LineIndex + 1;
      InsertSourceInfo( sourceInfo );

      // keep !src line on top
      string[] result = new string[_Lines.Length + newLines.Length];

      System.Array.Copy( _Lines, 0, result, 0, LineIndex + 1 );
      System.Array.Copy( newLines, 0, result, LineIndex + 1, newLines.Length );

      // this keeps the !source line in the final code, makes working with source infos easier though
      System.Array.Copy( _Lines, LineIndex + 1, result, LineIndex + newLines.Length + 1, _Lines.Length - LineIndex - 1 );

      // replace !source with empty line (otherwise source infos would have one line more!)
      result[LineIndex] = "";

      _Lines = result;

      m_ASMFileInfo.LineInfo.Remove( LineIndex );

      //--LineIndex;*/

      return true;
    }



  }
}
