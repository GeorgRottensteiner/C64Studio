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
    private bool POIncludeMediaSpritePad( int lineIndex, bool binary, string subFilename, string method, List<List<TokenInfo>> paramTokens, string labelPrefix, out ByteBuffer dataToInclude, out string[] replacementLines )
    {
      replacementLines  = null;
      dataToInclude     = null;

      if ( !binary )
      {
        AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Assembly include for sprite projects is not supported" );
        return false;
      }

      if ( ( method != "SPRITE" )
      &&   ( method != "SPRITEDATA" ) )
      {
        AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Unknown method '" + method + "', supported values for this file name are SPRITE and SPRITEDATA" );
        return false;
      }

      // sprite set file
      // sprites,index,count
      if ( ( method == "SPRITE" )
      &&   ( paramTokens.Count > 4 ) )
      {
        AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected <Sprite>[,<Index>[,<Count>]]" );
        return false;
      }
      if ( ( method == "SPRITEDATA" )
      &&   ( paramTokens.Count != 6 ) )
      {
        AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected <SpriteData>,<Index>,<Count>,<Offset>,<NumBytes>" );
        return false;
      }
      if ( method == "SPRITE" )
      {
        int   startIndex = 0;
        int   numSprites = -1;

        if ( ( paramTokens.Count >= 3 )
        &&   ( EvaluateTokens( lineIndex, paramTokens[2], out SymbolInfo startIndexSymbol ) ) )
        {
          startIndex = startIndexSymbol.ToInt32();
        }
        if ( ( paramTokens.Count >= 4 )
        &&   ( EvaluateTokens( lineIndex, paramTokens[3], out SymbolInfo numSpritesSymbol ) ) )
        {
          numSprites = numSpritesSymbol.ToInt32();
        }
        Formats.SpritePadProject    spriteProject = new Formats.SpritePadProject();

        try
        {
          dataToInclude = GR.IO.File.ReadAllBytes( subFilename );
          if ( dataToInclude == null )
          {
            AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read file " + subFilename );
            return false;
          }
        }
        catch ( System.IO.IOException )
        {
          AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read file " + subFilename );
          return false;
        }
        if ( !spriteProject.ReadFromBuffer( dataToInclude ) )
        {
          AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read sprite project file " + subFilename );
          return false;
        }
        if ( numSprites == -1 )
        {
          numSprites = spriteProject.NumSprites;
        }
        if ( ( startIndex < 0 )
        ||   ( startIndex >= spriteProject.NumSprites ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid start index " + startIndex );
          return false;
        }
        if ( ( numSprites <= 0 )
        ||   ( ( startIndex + numSprites ) > spriteProject.NumSprites ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid sprite count " + numSprites );
          return false;
        }
        GR.Memory.ByteBuffer    spriteData = new GR.Memory.ByteBuffer( (uint)( numSprites * 64 ) );

        for ( int i = 0; i < numSprites; ++i )
        {
          spriteProject.Sprites[startIndex + i].Data.CopyTo( spriteData, 0, 63, i * 64 );
        }
        dataToInclude = spriteData;
      }
      else if ( method == "SPRITEDATA" )
      {
        int   startIndex = 0;
        int   numSprites = 256;
        int   offsetBytes = 0;
        int   numBytes = numSprites * 64;

        if ( EvaluateTokens( lineIndex, paramTokens[2], out SymbolInfo startIndexSymbol ) )
        {
          startIndex = startIndexSymbol.ToInt32();
        }
        if ( EvaluateTokens( lineIndex, paramTokens[3], out SymbolInfo numSpritesSymbol ) )
        {
          numSprites = numSpritesSymbol.ToInt32();
        }
        if ( EvaluateTokens( lineIndex, paramTokens[4], out SymbolInfo offsetBytesSymbol ) )
        {
          offsetBytes = offsetBytesSymbol.ToInt32();
        }
        if ( EvaluateTokens( lineIndex, paramTokens[5], out SymbolInfo numBytesSymbol ) )
        {
          numBytes = numBytesSymbol.ToInt32();
        }

        Formats.SpritePadProject    spriteProject = new Formats.SpritePadProject();

        try
        {
          dataToInclude = GR.IO.File.ReadAllBytes( subFilename );
          if ( dataToInclude == null )
          {
            AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read file " + subFilename );
            return false;
          }
        }
        catch ( System.IO.IOException )
        {
          AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read file " + subFilename );
          return false;
        }
        if ( !spriteProject.ReadFromBuffer( dataToInclude ) )
        {
          AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read sprite project file " + subFilename );
          return false;
        }
        if ( ( startIndex < 0 )
        ||   ( startIndex >= spriteProject.NumSprites ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid start index " + startIndex );
          return false;
        }
        if ( ( numSprites <= 0 )
        ||   ( ( startIndex + numSprites ) > spriteProject.NumSprites ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid sprite count " + numSprites );
          return false;
        }
        GR.Memory.ByteBuffer    spriteData = new GR.Memory.ByteBuffer( (uint)( numSprites * 64 ) );

        for ( int i = 0; i < numSprites; ++i )
        {
          spriteProject.Sprites[startIndex + i].Data.CopyTo( spriteData, 0, 63, i * 64 );
        }

        if ( ( offsetBytes >= spriteData.Length )
        ||   ( offsetBytes < 0 ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid data offset " + offsetBytes );
          return false;
        }
        if ( ( offsetBytes + numBytes > spriteData.Length )
        ||   ( numBytes <= 0 ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid data size " + numBytes );
          return false;
        }
        dataToInclude = spriteData.SubBuffer( offsetBytes, numBytes );
      }
      return true;
    }


  }
}
