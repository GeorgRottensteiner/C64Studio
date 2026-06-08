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
    private bool POIncludeMediaSpriteProject( int lineIndex, bool binary, string subFilename, string method, List<List<TokenInfo>> paramTokens, string labelPrefix, out ByteBuffer dataToInclude, out string[] replacementLines )
    {
      replacementLines  = null;
      dataToInclude     = null;

      if ( !binary )
      {
        AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Assembly include for sprite projects is not supported" );
        return false;
      }

      if ( ( method != "SPRITE" )
      &&   ( method != "SPRITEDATA" ) 
      &&   ( method != "SPRITEOPTIMIZE" )
      &&   ( method != "SPRITEDATAOPTIMIZE" )
      &&   ( method != "PALETTE" )
      &&   ( method != "PALETTESWIZZLED" )
      &&   ( method != "PALETTERGB" )
      &&   ( method != "PALETTERGBSWIZZLED" ) )
      {
        AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Unknown method '" + method + "', supported values for this file name are SPRITE, SPRITEOPTIMIZE, SPRITEDATA, SPRITEDATAOPTIMIZE, PALETTE, PALETTERGB, PALETTESWIZZLED or PALETTERGBSWIZZLED" );
        return false;
      }

      if ( ( method == "PALETTE" )
      ||   ( method == "PALETTESWIZZLED" )
      ||   ( method == "PALETTERGB" )
      ||   ( method == "PALETTERGBSWIZZLED" ) )
      {
        if ( !binary )
        {
          AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Export as palette is only supported for binary" );
          return false;
        }

        Formats.SpriteProject   spriteProject = new RetroDevStudio.Formats.SpriteProject();

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

        int totalNumColors = spriteProject.Colors.Palettes.Count * spriteProject.Colors.Palettes[0].ColorValues.Length;
        int startIndex = 0;
        int numColors = totalNumColors;

        if ( ( paramTokens.Count >= 3 )
        &&   ( EvaluateTokens( lineIndex, paramTokens[2], out SymbolInfo startIndexSymbol ) ) )
        {
          startIndex = (int)startIndexSymbol.ToInteger();
        }
        if ( ( paramTokens.Count >= 4 )
        &&   ( EvaluateTokens( lineIndex, paramTokens[3], out SymbolInfo numColorsSymbol ) ) )
        {
          numColors = (int)numColorsSymbol.ToInteger();
        }
        if ( ( startIndex < 0 )
        ||   ( startIndex >= totalNumColors ) )
        {
          AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Invalid start index" );
          return false;
        }
        if ( numColors <= 0 )
        {
          AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Invalid number of colors, must be >= 1" );
          return false;
        }
        if ( startIndex + numColors > totalNumColors )
        {
          AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Invalid number of colors, sprite project has "
                + totalNumColors + " colors, but we're trying to fetch up to " + ( startIndex + numColors ) );
          return false;
        }
        dataToInclude = spriteProject.GetPaletteExportData( startIndex, numColors, method.EndsWith( "SWIZZLED" ), !method.Contains( "RGB" ) );
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
      if ( ( method == "SPRITE" )
      ||   ( method == "SPRITEOPTIMIZE" ) )
      {
        int   startIndex = 0;
        int   numSprites = -1; 

        if ( ( paramTokens.Count >= 3 )
        &&   ( EvaluateTokens( lineIndex, paramTokens[2], out SymbolInfo startIndexSymbol ) ) )
        {
          startIndex = (int)startIndexSymbol.ToInteger();
        }
        if ( ( paramTokens.Count >= 4 )
        &&   ( EvaluateTokens( lineIndex, paramTokens[3], out SymbolInfo numSpritesSymbol ) ) )
        {
          numSprites = (int)numSpritesSymbol.ToInteger();
        }
        Formats.SpriteProject   spriteProject = new RetroDevStudio.Formats.SpriteProject();

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
          numSprites = spriteProject.TotalNumberOfSprites;
        }
        if ( ( startIndex < 0 )
        ||   ( startIndex >= spriteProject.TotalNumberOfSprites ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid start index " + startIndex );
          return false;
        }
        if ( ( numSprites <= 0 )
        ||   ( ( startIndex + numSprites ) > spriteProject.TotalNumberOfSprites ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid sprite count " + numSprites );
          return false;
        }

        bool  optimizePadding = ( method == "SPRITEOPTIMIZE" );
        bool  addColor = Lookup.HaveCustomSpriteColor( spriteProject.Mode );

        GR.Memory.ByteBuffer    spriteData;

        if ( optimizePadding )
        {
          int paddedLength        = Lookup.NumPaddedBytesOfSingleSprite( spriteProject.Mode );
          int singleSpriteLength  = (int)spriteProject.Sprites[startIndex].Tile.Data.Length;

          uint  bufferSize = (uint)( singleSpriteLength + ( paddedLength * ( numSprites - 1 ) ) );
          if ( addColor )
          {
            ++bufferSize;
          }

          spriteData = new GR.Memory.ByteBuffer( bufferSize );

          for ( int i = 0; i < numSprites; ++i )
          {
            spriteProject.Sprites[startIndex + i].Tile.Data.CopyTo( spriteData,
                                                                    0,
                                                                    (int)spriteProject.Sprites[startIndex + i].Tile.Data.Length,
                                                                    i * paddedLength );
            if ( addColor )
            {
              spriteData.SetU8At( i * singleSpriteLength, (byte)spriteProject.Sprites[startIndex + i].Tile.CustomColor );
            }
          }
        }
        else
        {
          int paddedLength        = Lookup.NumPaddedBytesOfSingleSprite( spriteProject.Mode );
          int singleSpriteLength  = (int)spriteProject.Sprites[startIndex].Tile.Data.Length;

          spriteData = new GR.Memory.ByteBuffer( (uint)( paddedLength * numSprites ) );

          for ( int i = 0; i < numSprites; ++i )
          {
            spriteProject.Sprites[startIndex + i].Tile.Data.CopyTo( spriteData,
                                                                    0,
                                                                    (int)spriteProject.Sprites[startIndex + i].Tile.Data.Length,
                                                                    i * paddedLength );
            if ( addColor )
            {
              spriteData.SetU8At( i * paddedLength + singleSpriteLength, (byte)spriteProject.Sprites[startIndex + i].Tile.CustomColor );
            }
          }
        }
        dataToInclude = spriteData;
      }
      else if ( ( method == "SPRITEDATA" )
      ||        ( method == "SPRITEDATAOPTIMIZE" ) )
      {
        int   startIndex = 0;
        int   numSprites = 256;
        int   offsetBytes = 0;
        int   numBytes = numSprites * 64;

        if ( EvaluateTokens( lineIndex, paramTokens[2], out SymbolInfo startIndexSymbol ) )
        {
          startIndex = (int)startIndexSymbol.ToInteger();
        }
        if ( EvaluateTokens( lineIndex, paramTokens[3], out SymbolInfo numSpritesSymbol ) )
        {
          numSprites = (int)numSpritesSymbol.ToInteger();
        }
        if ( EvaluateTokens( lineIndex, paramTokens[4], out SymbolInfo offsetBytesSymbol ) )
        {
          offsetBytes = (int)offsetBytesSymbol.ToInteger();
        }
        if ( EvaluateTokens( lineIndex, paramTokens[5], out SymbolInfo numBytesSymbol ) )
        {
          numBytes = (int)numBytesSymbol.ToInteger();
        }

        Formats.SpriteProject   spriteProject = new RetroDevStudio.Formats.SpriteProject();

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
          numSprites = spriteProject.TotalNumberOfSprites;
        }
        if ( ( startIndex < 0 )
        ||   ( startIndex >= spriteProject.TotalNumberOfSprites ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid start index " + startIndex );
          return false;
        }
        if ( ( numSprites <= 0 )
        ||   ( ( startIndex + numSprites ) > spriteProject.TotalNumberOfSprites ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid sprite count " + numSprites );
          return false;
        }
        bool  optimizePadding = ( method == "SPRITEDATAOPTIMIZE" );
        bool  addColor = Lookup.HaveCustomSpriteColor( spriteProject.Mode );

        GR.Memory.ByteBuffer    spriteData;

        if ( optimizePadding )
        {
          int paddedLength        = Lookup.NumPaddedBytesOfSingleSprite( spriteProject.Mode );
          int singleSpriteLength  = (int)spriteProject.Sprites[startIndex].Tile.Data.Length;

          uint  bufferSize = (uint)( singleSpriteLength + ( paddedLength * ( numSprites - 1 ) ) );
          if ( addColor )
          {
            ++bufferSize;
          }

          spriteData = new GR.Memory.ByteBuffer( bufferSize );

          for ( int i = 0; i < numSprites; ++i )
          {
            spriteProject.Sprites[startIndex + i].Tile.Data.CopyTo( spriteData,
                                                                    0,
                                                                    (int)spriteProject.Sprites[startIndex + i].Tile.Data.Length,
                                                                    i * paddedLength );
            if ( addColor )
            {
              spriteData.SetU8At( i * paddedLength + singleSpriteLength, (byte)spriteProject.Sprites[startIndex + i].Tile.CustomColor );
            }
          }
        }
        else
        {
          int paddedLength        = Lookup.NumPaddedBytesOfSingleSprite( spriteProject.Mode );
          int singleSpriteLength  = (int)spriteProject.Sprites[startIndex].Tile.Data.Length;

          spriteData = new GR.Memory.ByteBuffer( (uint)( paddedLength * numSprites ) );

          for ( int i = 0; i < numSprites; ++i )
          {
            spriteProject.Sprites[startIndex + i].Tile.Data.CopyTo( spriteData,
                                                                    0,
                                                                    (int)spriteProject.Sprites[startIndex + i].Tile.Data.Length,
                                                                    i * paddedLength );
            if ( addColor )
            {
              spriteData.SetU8At( i * paddedLength + singleSpriteLength, (byte)spriteProject.Sprites[startIndex + i].Tile.CustomColor );
            }
          }
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
