using GR.Memory;
using RetroDevStudio.Formats;
using RetroDevStudio.Parser;
using RetroDevStudio.Types;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Security.Policy;

namespace RetroDevStudio.Parser
{
  public partial class BasicFileParser : ParserBase
  {
    private bool MetaDataSpriteData( int LineIndex, string MetaData, string MetaDataParams )
    {
      var  parms = PureTokenizeLine( MetaDataParams );

      if ( !ParseLineInParameters( parms.Tokens, 0, parms.Tokens.Count, LineIndex, false, out List<List<Token>> cleanedParms ) )
      {
        return false;
      }

      // followed by string literal for file name
      if ( ( cleanedParms.Count != 5 )
      ||   ( cleanedParms[0].Count != 1 )
      ||   ( cleanedParms[1].Count > 1 )
      ||   ( cleanedParms[2].Count > 1 )
      ||   ( cleanedParms[3].Count != 1 )
      ||   ( cleanedParms[4].Count != 1 )
      ||   ( !cleanedParms[0][0].Content.StartsWith( "\"" ) )
      ||   ( !cleanedParms[0][0].Content.EndsWith( "\"" ) ) )
      {
        AddError( LineIndex, Types.ErrorCode.E3007_BASIC_MALFORMED_METADATA, "SpriteData expects <Filename>,<Offset>,<Count>,<Start Line No>,<Line Step>" );
        return false;
      }

      var filename = cleanedParms[0][0].Content;
      string    includeFile = filename.Substring( 1, filename.Length - 2 );
      string    subFilenameFull   = GR.Path.RenameFile( m_CompileConfig.InputFile, includeFile );
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

      var spriteProject = new SpriteProject();

      try
      {
        var subFile = GR.IO.File.ReadAllBytes( subFilenameFull );
        spriteProject.ReadFromBuffer( subFile );
      }
      catch ( System.IO.IOException )
      {
        AddError( LineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read file " + subFilenameFull );
        return false;
      }

      int offset    = GR.Convert.ToI32( cleanedParms[1][0].Content );
      int count     = GR.Convert.ToI32( cleanedParms[2][0].Content );
      int startLine = GR.Convert.ToI32( cleanedParms[3][0].Content );
      int stepLine  = GR.Convert.ToI32( cleanedParms[4][0].Content );

      if ( ( count <= 0 )
      ||   ( offset >= spriteProject.TotalNumberOfSprites )
      ||   ( offset + count <= 0 ) )
      {
        return true;
      }

      if ( offset < 0 )
      {
        count += offset;
        offset = 0;
      }
      if ( offset + count > spriteProject.TotalNumberOfSprites )
      {
        count = spriteProject.TotalNumberOfSprites - offset;
      }

      int spriteSize = Lookup.NumPaddedBytesOfSingleSprite( spriteProject.Mode );
      int paddingSize = spriteSize - Lookup.NumBytesOfSingleSprite( spriteProject.Mode );

      var spriteData = new GR.Memory.ByteBuffer( (uint)( count * spriteSize - paddingSize ) );

      for ( int i = 0; i < count; ++i )
      {
        spriteProject.Sprites[offset + i].Tile.Data.CopyTo( spriteData,
                                                            0,
                                                            (int)spriteProject.Sprites[offset + i].Tile.Data.Length,
                                                            i * spriteSize );
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
