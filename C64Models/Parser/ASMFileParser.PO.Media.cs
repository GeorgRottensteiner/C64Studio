using GR.Memory;
using RetroDevStudio.Formats;
using RetroDevStudio.Parser;
using System;
using System.Collections.Generic;

namespace RetroDevStudio.Parser
{
  public partial class ASMFileParser : ParserBase
  {
    private bool POIncludeMedia( List<Types.TokenInfo> lineTokenInfos, int lineIndex, bool Binary, Types.ASM.LineInfo info, string ParentFilename, out int lineSizeInBytes, out string[] ReplacementLines )
    {
      ReplacementLines = null;
      lineSizeInBytes = 0;


      //List<List<Types.TokenInfo>>   paramTokens = new List<List<RetroDevStudio.Types.TokenInfo>>();
      //paramTokens.Add( new List<RetroDevStudio.Types.TokenInfo>() );

      ParseLineInParameters( lineTokenInfos, 1, lineTokenInfos.Count - 1, lineIndex, false, out List<List<Types.TokenInfo>> paramTokens );

      if ( ( paramTokens.Count >= 7 )
      ||   ( paramTokens.Count < 2 ) )
      {
        if ( Binary )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected !media <Filename>[,<MediaParams>...]" );
        }
        else
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected !mediasrc <Filename>,SrcLabelPrefix[,<MediaParams>...]" );
        }
        return false;
      }

      // validate filename
      if ( ( paramTokens[0].Count != 1 )
      ||   ( paramTokens[0][0].Length < 2 ) )
      {
        AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected string literal as filename" );
        return false;
      }
      if ( ( !paramTokens[0][0].Content.StartsWith( "\"" ) )
      ||   ( !paramTokens[0][0].Content.EndsWith( "\"" ) )
      ||   ( paramTokens[0][0].Length <= 2 ) )
      {
        AddError( lineIndex, Types.ErrorCode.E1307_FILENAME_INCOMPLETE, "Expected proper file name between apostrophes" );
        return false;
      }
      string    subFilename = paramTokens[0][0].Content.Substring( 1, paramTokens[0][0].Length - 2 );
      int       includeMethodParamIndex = 1;
      string    labelPrefix = "";

      subFilename = BuildFullPath( System.IO.Path.GetDirectoryName( ParentFilename ), subFilename );

      if ( !Binary )
      {
        if ( paramTokens.Count < 3 )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected !mediasrc <Filename>,SrcLabelPrefix[,<MediaParams>...]" );
          return false;
        }
        // label prefix 
        if ( ( paramTokens[1].Count != 1 )
        ||   ( ( paramTokens[1][0].Type != RetroDevStudio.Types.TokenInfo.TokenType.LABEL_GLOBAL )
        &&     ( paramTokens[1][0].Type != RetroDevStudio.Types.TokenInfo.TokenType.LABEL_LOCAL ) ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected proper global or local label prefix" );
          return false;
        }
        labelPrefix = paramTokens[1][0].Content;

        paramTokens.RemoveAt( 1 );
      }

      string    extension = System.IO.Path.GetExtension( subFilename ).ToUpper();
      if ( ( paramTokens[includeMethodParamIndex].Count != 1 )
      ||   ( paramTokens[includeMethodParamIndex][0].Type != RetroDevStudio.Types.TokenInfo.TokenType.LABEL_GLOBAL ) )
      {
        AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected known import method" );
        return false;
      }
      string    method = paramTokens[includeMethodParamIndex][0].Content.ToUpper();

      GR.Memory.ByteBuffer    dataToInclude = new GR.Memory.ByteBuffer();


      if ( extension == ".CHR" )
      {
        if ( !Binary )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Assembly include for charsets is not supported" );
          return false;
        }

        // char set file
        //  char,index,count
        if ( paramTokens.Count > 4 )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected <Char>[,<Index>[,<Count>]]" );
          return false;
        }
        if ( method != "CHAR" )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Unknown method '" + method + "', supported values for this file name are CHAR" );
          return false;
        }
        int startIndex = 0;
        int numChars = 256;

        if ( ( paramTokens.Count >= 3 )
        &&   ( EvaluateTokens( lineIndex, paramTokens[2], info.LineCodeMapping, out SymbolInfo startIndexSymbol ) ) )
        {
          startIndex = (int)startIndexSymbol.ToInteger();
        }
        if ( ( paramTokens.Count >= 4 )
        &&   ( EvaluateTokens( lineIndex, paramTokens[3], info.LineCodeMapping, out SymbolInfo numCharsSymbol ) ) )
        {
          numChars = (int)numCharsSymbol.ToInteger();
        }
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
        if ( ( startIndex < 0 )
        ||   ( startIndex * 8 >= dataToInclude.Length ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid start index " + startIndex );
          return false;
        }
        if ( ( numChars <= 0 )
        ||   ( ( startIndex + numChars ) * 8 > dataToInclude.Length ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid char count " + numChars );
          return false;
        }
        dataToInclude = dataToInclude.SubBuffer( startIndex * 8, numChars * 8 );
      }
      else if ( extension == ".CHARSETPROJECT" )
      {
        if ( !Binary )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Assembly include for charset projects is not supported" );
          return false;
        }

        // character project file
        // char,index,count
        if ( paramTokens.Count > 4 )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected <Char|CharColor>[,<Index>[,<Count>]]" );
          return false;
        }
        if ( ( method != "CHAR" )
        &&   ( method != "CHARCOLOR" )
        &&   ( method != "PALETTE" ) 
        &&   ( method != "PALETTESWIZZLED" )
        &&   ( method != "PALETTERGB" ) 
        &&   ( method != "PALETTERGBSWIZZLED" ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Unknown method '" + method + "', supported values for this file name are CHAR, CHARCOLOR, PALETTE, PALETTERGB, PALETTESWIZZLED or PALETTERGBSWIZZLED." );
          return false;
        }
        int startIndex = 0;
        int numChars = 256;

        if ( ( paramTokens.Count >= 3 )
        &&   ( EvaluateTokens( lineIndex, paramTokens[2], info.LineCodeMapping, out SymbolInfo startIndexSymbol ) ) )
        {
          startIndex = (int)startIndexSymbol.ToInteger();
        }
        if ( paramTokens.Count >= 4 )
        {
          if ( !EvaluateTokens( lineIndex, paramTokens[3], info.LineCodeMapping, out SymbolInfo numCharsSymbol ) )
          {
            AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Failed to evaluate expression " + TokensToExpression( paramTokens[3] ) );
            return false;
          }
          numChars = (int)numCharsSymbol.ToInteger();
        }
        Formats.CharsetProject    charProject = new RetroDevStudio.Formats.CharsetProject();

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
        if ( !charProject.ReadFromBuffer( dataToInclude ) )
        {
          AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read charset project file " + subFilename );
          return false;
        }

        if ( ( method == "PALETTE" )
        ||   ( method == "PALETTESWIZZLED" )
        ||   ( method == "PALETTERGB" )
        ||   ( method == "PALETTERGBSWIZZLED" ) )
        {
          if ( !Binary )
          {
            AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Export as palette is only supported for binary" );
            return false;
          }

          long numColors = numChars;

          if ( ( startIndex < 0 )
          ||   ( startIndex >= charProject.Colors.Palette.NumColors ) )
          {
            AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Invalid start index" );
            return false;
          }
          if ( numColors <= 0 )
          {
            AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Invalid number of colors, must be >= 1" );
            return false;
          }
          if ( startIndex + numColors > charProject.Colors.Palette.NumColors )
          {
            AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Invalid number of colors, charset has "
                  + charProject.Colors.Palette.NumColors + " colors, but we're trying to fetch up to " + ( startIndex + numColors ) );
            return false;
          }
          dataToInclude = charProject.Colors.Palette.GetExportData( (int)startIndex, (int)numColors, method.EndsWith( "SWIZZLED" ), !method.Contains( "RGB" ) );
        }
        else
        {
          if ( ( startIndex < 0 )
          ||   ( startIndex >= charProject.ExportNumCharacters ) )
          {
            AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid start index " + startIndex );
            return false;
          }
          if ( ( numChars <= 0 )
          || ( ( startIndex + numChars ) > charProject.ExportNumCharacters ) )
          {
            AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid char count " + numChars );
            return false;
          }

          dataToInclude = charProject.CharacterData( startIndex, numChars );
          if ( method == "CHARCOLOR" )
          {
            dataToInclude += charProject.ColorData( startIndex, numChars );
          }
        }
      }
      else if ( extension == ".VALUETABLEPROJECT" )
      {
        if ( !Binary )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Assembly include for value table projects is not supported" );
          return false;
        }

        // value table project file
        if ( paramTokens.Count > 4 )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected <Data>[,<Offset>[,<Bytes>]]" );
          return false;
        }
        if ( method != "DATA" )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Unknown method '" + method + "', supported values for this file name are DATA" );
          return false;
        }
        int   numBytes = -1;
        int   startIndex = 0;
        if ( ( paramTokens.Count >= 3 )
        &&   ( EvaluateTokens( lineIndex, paramTokens[2], info.LineCodeMapping, out SymbolInfo startIndexSymbol ) ) )
        {
          startIndex = (int)startIndexSymbol.ToInteger();
        }
        if ( ( paramTokens.Count >= 4 )
        &&   ( EvaluateTokens( lineIndex, paramTokens[3], info.LineCodeMapping, out SymbolInfo numBytesSymbol ) ) )
        {
          numBytes = (int)numBytesSymbol.ToInteger();
        }
        Formats.ValueTableProject   valueTableProject = new Formats.ValueTableProject();

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
        if ( !valueTableProject.ReadFromBuffer( dataToInclude ) )
        {
          AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read value table project file " + subFilename );
          return false;
        }
        dataToInclude = valueTableProject.GenerateTableData();
        if ( ( startIndex < 0 )
        ||   ( startIndex >= dataToInclude.Length ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid start index " + startIndex );
          return false;
        }
        if ( numBytes== -1 )
        {
          numBytes = (int)dataToInclude.Length;
        }
        if ( ( numBytes <= 0 )
        || ( ( startIndex + numBytes ) > dataToInclude.Length ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid byte count " + numBytes );
          return false;
        }
        dataToInclude = dataToInclude.SubBuffer( 0, numBytes );
      }
      else if ( extension == ".SPR" )
      {
        if ( !Binary )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Assembly include for sprites is not supported" );
          return false;
        }

        if ( ( method != "SPRITE" )
        &&   ( method != "SPRITEDATA" ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Unknown method '" + method + "', supported values for this file name are SPRITE or SPRITEDATA" );
          return false;
        }

        if ( ( method == "SPRITE" )
        &&   ( paramTokens.Count > 4 ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected <Sprite>[,<Index>[,<Count>]]" );
          return false;
        }
        if ( ( method == "SPRITEDATA" )
        &&   ( paramTokens.Count != 6 ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected <Spritedata>,<Index>,<Count>,<Offset>,<NumBytes>" );
          return false;
        }

        if ( method == "SPRITE" )
        {
          int startIndex = 0;
          int numSprites = -1;

          if ( ( paramTokens.Count >= 3 )
          &&   ( EvaluateTokens( lineIndex, paramTokens[2], info.LineCodeMapping, out SymbolInfo startIndexSymbol ) ) )
          {
            startIndex = (int)startIndexSymbol.ToInteger();
          }
          if ( ( paramTokens.Count >= 4 )
          &&   ( EvaluateTokens( lineIndex, paramTokens[3], info.LineCodeMapping, out SymbolInfo numSpritesSymbol ) ) )
          {
            numSprites = (int)numSpritesSymbol.ToInteger();
          }
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
          if ( numSprites == -1 )
          {
            numSprites = (int)dataToInclude.Length / 64;
          }
          if ( ( startIndex < 0 )
          ||   ( startIndex * 64 >= dataToInclude.Length ) )
          {
            AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid start index " + startIndex );
            return false;
          }
          if ( ( numSprites <= 0 )
          ||   ( ( startIndex + numSprites ) * 64 > dataToInclude.Length ) )
          {
            AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid sprite count " + numSprites );
            return false;
          }
          dataToInclude = dataToInclude.SubBuffer( startIndex * 64, numSprites * 64 );
        }
        else if ( method == "SPRITEDATA" )
        {
          int   startIndex = 0;
          int   numSprites = -1;
          int   offsetBytes = 0;
          int   numBytes = numSprites * 64;

          if ( EvaluateTokens( lineIndex, paramTokens[2], info.LineCodeMapping, out SymbolInfo startIndexSymbol ) )
          {
            startIndex = (int)startIndexSymbol.ToInteger();
          }
          if ( EvaluateTokens( lineIndex, paramTokens[3], info.LineCodeMapping, out SymbolInfo numSpritesSymbol ) )
          {
            numSprites = (int)numSpritesSymbol.ToInteger();
          }
          if ( EvaluateTokens( lineIndex, paramTokens[4], info.LineCodeMapping, out SymbolInfo offsetBytesSymbol ) )
          {
            offsetBytes = (int)offsetBytesSymbol.ToInteger();
          }
          if ( EvaluateTokens( lineIndex, paramTokens[5], info.LineCodeMapping, out SymbolInfo numBytesSymbol ) )
          {
            numBytes = (int)numBytesSymbol.ToInteger();
          }

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
          if ( numSprites == -1 )
          {
            numSprites = (int)dataToInclude.Length / 64;
          }
          if ( ( startIndex < 0 )
          ||   ( startIndex * 64 >= dataToInclude.Length ) )
          {
            AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid start index " + startIndex );
            return false;
          }
          if ( ( numSprites <= 0 )
          ||   ( ( startIndex + numSprites ) * 64 > dataToInclude.Length ) )
          {
            AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid sprite count " + numSprites );
            return false;
          }
          dataToInclude = dataToInclude.SubBuffer( startIndex * 64, numSprites * 64 );
          if ( ( offsetBytes >= dataToInclude.Length )
          ||   ( offsetBytes < 0 ) )
          {
            AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid data offset " + offsetBytes );
            return false;
          }
          if ( ( offsetBytes + numBytes > dataToInclude.Length )
          ||   ( numBytes <= 0 ) )
          {
            AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid data size " + numBytes );
            return false;
          }
          dataToInclude = dataToInclude.SubBuffer( offsetBytes, numBytes );
        }
      }
      else if ( extension == ".SPRITEPROJECT" )
      {
        if ( !Binary )
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
          if ( !Binary )
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
          &&   ( EvaluateTokens( lineIndex, paramTokens[2], info.LineCodeMapping, out SymbolInfo startIndexSymbol ) ) )
          {
            startIndex = (int)startIndexSymbol.ToInteger();
          }
          if ( ( paramTokens.Count >= 4 )
          &&   ( EvaluateTokens( lineIndex, paramTokens[3], info.LineCodeMapping, out SymbolInfo numColorsSymbol ) ) )
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
          &&   ( EvaluateTokens( lineIndex, paramTokens[2], info.LineCodeMapping, out SymbolInfo startIndexSymbol ) ) )
          {
            startIndex = (int)startIndexSymbol.ToInteger();
          }
          if ( ( paramTokens.Count >= 4 )
          &&   ( EvaluateTokens( lineIndex, paramTokens[3], info.LineCodeMapping, out SymbolInfo numSpritesSymbol ) ) )
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

          if ( EvaluateTokens( lineIndex, paramTokens[2], info.LineCodeMapping, out SymbolInfo startIndexSymbol ) )
          {
            startIndex = (int)startIndexSymbol.ToInteger();
          }
          if ( EvaluateTokens( lineIndex, paramTokens[3], info.LineCodeMapping, out SymbolInfo numSpritesSymbol ) )
          {
            numSprites = (int)numSpritesSymbol.ToInteger();
          }
          if ( EvaluateTokens( lineIndex, paramTokens[4], info.LineCodeMapping, out SymbolInfo offsetBytesSymbol ) )
          {
            offsetBytes = (int)offsetBytesSymbol.ToInteger();
          }
          if ( EvaluateTokens( lineIndex, paramTokens[5], info.LineCodeMapping, out SymbolInfo numBytesSymbol ) )
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
                spriteData.SetU8At( i * singleSpriteLength, (byte)spriteProject.Sprites[startIndex + i].Tile.CustomColor );
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
      }
      else if ( extension == ".SPD" )
      {
        if ( !Binary )
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
          &&   ( EvaluateTokens( lineIndex, paramTokens[2], info.LineCodeMapping, out SymbolInfo startIndexSymbol ) ) )
          {
            startIndex = startIndexSymbol.ToInt32();
          }
          if ( ( paramTokens.Count >= 4 )
          &&   ( EvaluateTokens( lineIndex, paramTokens[3], info.LineCodeMapping, out SymbolInfo numSpritesSymbol ) ) )
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

          if ( EvaluateTokens( lineIndex, paramTokens[2], info.LineCodeMapping, out SymbolInfo startIndexSymbol ) )
          {
            startIndex = startIndexSymbol.ToInt32();
          }
          if ( EvaluateTokens( lineIndex, paramTokens[3], info.LineCodeMapping, out SymbolInfo numSpritesSymbol ) )
          {
            numSprites = numSpritesSymbol.ToInt32();
          }
          if ( EvaluateTokens( lineIndex, paramTokens[4], info.LineCodeMapping, out SymbolInfo offsetBytesSymbol ) )
          {
            offsetBytes = offsetBytesSymbol.ToInt32();
          }
          if ( EvaluateTokens( lineIndex, paramTokens[5], info.LineCodeMapping, out SymbolInfo numBytesSymbol ) )
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
          || ( numBytes <= 0 ) )
          {
            AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid data size " + numBytes );
            return false;
          }
          dataToInclude = spriteData.SubBuffer( offsetBytes, numBytes );
        }
      }
      else if ( extension == ".CHARSCREEN" )
      {
        // screen
        // char,x,y,width,height
        // color,x,y,width,height
        // charcolor,x,y,width,height
        // colorchar,x,y,width,height
        // charvert,x,y,width,height
        // colorvert,x,y,width,height
        // charcolorvert,x,y,width,height
        // colorcharvert,x,y,width,height
        // palette[,start,count]
        if ( paramTokens.Count > 6 )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected <Char|CharVert|Color|ColorVert|CharColor|CharColorVert|ColorChar|ColorCharVert|CharSet>[,<X>[,<Y>[,<Width>[,<Height>]]]]" );
          return false;
        }
        if ( ( method != "CHAR" )
        &&   ( method != "COLOR" )
        &&   ( method != "CHARSET" ) 
        &&   ( method != "CHARCOLOR" )
        &&   ( method != "CHARCOLORINTERLEAVED" )
        &&   ( method != "CHARCOLORINTERLEAVEDVERT" )
        &&   ( method != "COLORCHAR" )
        &&   ( method != "CHARVERT" )
        &&   ( method != "COLORVERT" )
        &&   ( method != "CHARCOLORVERT" )
        &&   ( method != "COLORCHARVERT" )
        &&   ( method != "PALETTE" )
        &&   ( method != "PALETTESWIZZLED" )
        &&   ( method != "PALETTERGB" )
        &&   ( method != "PALETTERGBSWIZZLED" ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Unknown method '" + method + "', supported values for this file name are CHAR, COLOR, CHARCOLOR, COLORCHAR, CHARVERT, COLORVERT, CHARCOLORVERT, COLORCHARVERT, CHARCOLORINTERLEAVED, CHARCOLORINTERLEAVEDVERT, CHARSET, PALETTE, PALETTERGB, PALETTESWIZZLED and PALETTERGBSWIZZLED" );
          return false;
        }

        Formats.CharsetScreenProject    screenProject = new RetroDevStudio.Formats.CharsetScreenProject();

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
        if ( !screenProject.ReadFromBuffer( dataToInclude ) )
        {
          AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read charset screen project from " + subFilename );
          return false;
        }

        if ( method == "CHARSET" )
        {
          if ( !Binary )
          {
            AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Export as CHARSET is only supported for binary" );
            return false;
          }

          int startIndex = 0;
          int numChars = screenProject.CharSet.ExportNumCharacters;

          if ( ( paramTokens.Count >= 3 )
          &&   ( EvaluateTokens( lineIndex, paramTokens[2], info.LineCodeMapping, out SymbolInfo startIndexSymbol ) ) )
          {
            startIndex = startIndexSymbol.ToInt32();
          }
          if ( ( paramTokens.Count >= 4 )
          &&   ( EvaluateTokens( lineIndex, paramTokens[3], info.LineCodeMapping, out SymbolInfo numCharsSymbol ) ) )
          {
            numChars = numCharsSymbol.ToInt32();
          }
          if ( ( startIndex < 0 )
          ||   ( startIndex >= screenProject.CharSet.ExportNumCharacters ) )
          {
            AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Invalid start index" );
            return false;
          }
          if ( numChars <= 0 )
          {
            AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Invalid number of characters, must be >= 1" );
            return false;
          }
          if ( startIndex + numChars > screenProject.CharSet.ExportNumCharacters )
          {
            AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Invalid number of characters, charset has " 
                  + screenProject.CharSet.ExportNumCharacters + " characters, but we're trying to fetch up to " + ( startIndex + numChars ) );
            return false;
          }
          dataToInclude = screenProject.CharSet.CharacterData( startIndex, numChars );
        }
        else if ( ( method == "PALETTE" )
        ||        ( method == "PALETTESWIZZLED" )
        ||        ( method == "PALETTERGB" )
        ||        ( method == "PALETTERGBSWIZZLED" ) )
        {
          if ( !Binary )
          {
            AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Export as palette is only supported for binary" );
            return false;
          }

          int startIndex = 0;
          int numColors = screenProject.CharSet.Colors.Palette.NumColors;

          if ( ( paramTokens.Count >= 3 )
          &&   ( EvaluateTokens( lineIndex, paramTokens[2], info.LineCodeMapping, out SymbolInfo startIndexSymbol ) ) )
          {
            startIndex = startIndexSymbol.ToInt32();
          }
          if ( ( paramTokens.Count >= 4 )
          &&   ( EvaluateTokens( lineIndex, paramTokens[3], info.LineCodeMapping, out SymbolInfo numColorsSymbol ) ) )
          {
            numColors = numColorsSymbol.ToInt32();
          }
          if ( ( startIndex < 0 )
          ||   ( startIndex >= screenProject.CharSet.Colors.Palette.NumColors ) )
          {
            AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Invalid start index" );
            return false;
          }
          if ( numColors <= 0 )
          {
            AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Invalid number of colors, must be >= 1" );
            return false;
          }
          if ( startIndex + numColors > screenProject.CharSet.Colors.Palette.NumColors )
          {
            AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Invalid number of colors, charset has "
                  + screenProject.CharSet.Colors.Palette.NumColors + " colors, but we're trying to fetch up to " + ( startIndex + numColors ) );
            return false;
          }
          dataToInclude = screenProject.CharSet.Colors.Palette.GetExportData( startIndex, numColors, method.EndsWith( "SWIZZLED" ), !method.Contains( "RGB" ) );
        }
        else
        {
          int   x = 0;
          int   y = 0;
          int   w = screenProject.ScreenWidth;
          int   h = screenProject.ScreenHeight;

          if ( ( paramTokens.Count >= 3 )
          &&   ( EvaluateTokens( lineIndex, paramTokens[2], info.LineCodeMapping, out SymbolInfo xSymbol ) ) )
          {
            x = xSymbol.ToInt32();
          }
          if ( ( paramTokens.Count >= 4 )
          &&   ( EvaluateTokens( lineIndex, paramTokens[3], info.LineCodeMapping, out SymbolInfo ySymbol ) ) )
          {
            y = ySymbol.ToInt32();
          }
          if ( ( paramTokens.Count >= 5 )
          &&   ( EvaluateTokens( lineIndex, paramTokens[4], info.LineCodeMapping, out SymbolInfo wSymbol ) ) )
          {
            w = wSymbol.ToInt32();
          }
          if ( ( paramTokens.Count >= 6 )
          &&   ( EvaluateTokens( lineIndex, paramTokens[5], info.LineCodeMapping, out SymbolInfo hSymbol ) ) )
          {
            h = hSymbol.ToInt32();
          }

          if ( ( x < 0 )
          ||   ( x >= screenProject.ScreenWidth )
          ||   ( y < 0 )
          ||   ( y >= screenProject.ScreenHeight )
          ||   ( w < 0 )
          ||   ( x + w > screenProject.ScreenWidth )
          ||   ( h < 0 )
          ||   ( y + h > screenProject.ScreenHeight ) )
          {
            AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid coordinates" );
            return false;
          }

          dataToInclude.Clear();

          var exportInfo = new ExportCharsetScreenInfo();
          exportInfo.RowByRow = !method.EndsWith( "VERT" );
          exportInfo.Area = new GR.Math.Rectangle( x, y, w, h ); 

          if ( !screenProject.ExportToBuffer( exportInfo ) )
          {
            AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Failed to export data from " + subFilename );
            return false;
          }

          string    textToInclude = "";
          if ( ( method == "CHAR" )
          ||   ( method == "CHARVERT" ) )
          {
            if ( !Binary )
            {
              textToInclude = labelPrefix + "_CHARS" + System.Environment.NewLine;
              textToInclude += Util.ToASMData( exportInfo.ScreenCharData, false, 0, MacroByType( Types.MacroInfo.PseudoOpType.BYTE ) );

              ReplacementLines = textToInclude.Split( new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries );
            }
            else
            {
              dataToInclude = exportInfo.ScreenCharData;
            }
          }
          else if ( ( method == "COLOR" )
          ||        ( method == "COLORVERT" ) )
          {
            if ( !Binary )
            {
              textToInclude = labelPrefix + "_COLOR" + System.Environment.NewLine;
              textToInclude += Util.ToASMData( exportInfo.ScreenColorData, false, 0, MacroByType( Types.MacroInfo.PseudoOpType.BYTE ) );
              ReplacementLines = textToInclude.Split( new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries );
            }
            else
            {
              dataToInclude = exportInfo.ScreenColorData;
            }
          }
          else if ( ( method == "CHARCOLOR" )
          ||        ( method == "CHARCOLORVERT" ) )
          {
            if ( !Binary )
            {
              textToInclude = labelPrefix + "_CHARS" + System.Environment.NewLine;
              textToInclude += Util.ToASMData( exportInfo.ScreenCharData, false, 0, MacroByType( Types.MacroInfo.PseudoOpType.BYTE ) ) + System.Environment.NewLine;
              textToInclude += labelPrefix + "_COLOR" + System.Environment.NewLine;
              textToInclude += Util.ToASMData( exportInfo.ScreenColorData, false, 0, MacroByType( Types.MacroInfo.PseudoOpType.BYTE ) );

              ReplacementLines = textToInclude.Split( new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries );
            }
            else
            {
              dataToInclude = exportInfo.ScreenCharData + exportInfo.ScreenColorData;
            }
          }
          else if ( ( method == "CHARCOLORINTERLEAVED" )
          ||        ( method == "CHARCOLORINTERLEAVEDVERT" ) )
          {
            var interleavedBuffer = new ByteBuffer( exportInfo.ScreenCharData.Length + exportInfo.ScreenColorData.Length );
            for ( int i = 0; i < exportInfo.ScreenCharData.Length; ++i )
            {
              interleavedBuffer.SetU8At( i * 2, exportInfo.ScreenCharData.ByteAt( i ) );
              interleavedBuffer.SetU8At( i * 2 + 1, exportInfo.ScreenColorData.ByteAt( i ) );
            }

            if ( !Binary )
            {
              textToInclude = labelPrefix + "_DATA" + System.Environment.NewLine;
              textToInclude += Util.ToASMData( interleavedBuffer, false, 0, MacroByType( Types.MacroInfo.PseudoOpType.BYTE ) ) + System.Environment.NewLine;

              ReplacementLines = textToInclude.Split( new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries );
            }
            else
            {
              dataToInclude = interleavedBuffer;
            }
          }
          else if ( ( method == "COLORCHAR" )
          ||        ( method == "COLORCHARVERT" ) )
          {
            if ( !Binary )
            {
              textToInclude = labelPrefix + "_COLOR" + System.Environment.NewLine;
              textToInclude += Util.ToASMData( exportInfo.ScreenColorData, false, 0, MacroByType( Types.MacroInfo.PseudoOpType.BYTE ) ) + System.Environment.NewLine;
              textToInclude += labelPrefix + "_CHARS" + System.Environment.NewLine;
              textToInclude += Util.ToASMData( exportInfo.ScreenCharData, false, 0, MacroByType( Types.MacroInfo.PseudoOpType.BYTE ) );
              ReplacementLines = textToInclude.Split( new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries );
            }
            else
            {
              dataToInclude = exportInfo.ScreenColorData + exportInfo.ScreenCharData;
            }
          }
        }
      }
      else if ( extension == ".GRAPHICSCREEN" )
      {
        // graphic screen
        // bitmap,x,y,width,height
        // bitmapscreen,x,y,width,height
        // bitmapscreencolor,x,y,width,height
        // screen,x,y,width,height
        // color,x,y,width,height
        if ( paramTokens.Count > 6 )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected <Bitmap|BitmapScreen|BitmapScreenColor|BitmapHires|BitmapHiresScreen|BitmapHiresScreenColor|Screen|Color>[,<X>[,<Y>[,<Width>[,<Height>]]]]" );
          return false;
        }
        if ( ( method != "BITMAP" )
        &&   ( method != "BITMAPSCREEN" )
        &&   ( method != "BITMAPSCREENCOLOR" )
        &&   ( method != "BITMAPHIRES" )
        &&   ( method != "BITMAPHIRESSCREEN" )
        &&   ( method != "BITMAPHIRESSCREENCOLOR" )
        &&   ( method != "SCREEN" )
        &&   ( method != "COLOR" ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Unknown method '" + method + "', supported values for this file name are BITMAP, BITMAPSCREEN, BITMAPSCREENCOLOR, BITMAPHIRES, BITMAPHIRESSCREEN, BITMAPHIRESSCREENCOLOR, SCREEN and COLOR" );
          return false;
        }
        Formats.GraphicScreenProject screenProject = new RetroDevStudio.Formats.GraphicScreenProject();

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
        if ( !screenProject.ReadFromBuffer( dataToInclude ) )
        {
          AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read graphic screen project from " + subFilename );
          return false;
        }

        int   x = 0;
        int   y = 0;
        int   w = screenProject.ScreenWidth;
        int   h = screenProject.ScreenHeight;

        if ( ( paramTokens.Count >= 3 )
        &&   ( EvaluateTokens( lineIndex, paramTokens[2], info.LineCodeMapping, out SymbolInfo xSymbol ) ) )
        {
          x = xSymbol.ToInt32();
        }
        if ( ( paramTokens.Count >= 4 )
        &&   ( EvaluateTokens( lineIndex, paramTokens[3], info.LineCodeMapping, out SymbolInfo ySymbol ) ) )
        {
          y = ySymbol.ToInt32();
        }
        if ( ( paramTokens.Count >= 5 )
        &&   ( EvaluateTokens( lineIndex, paramTokens[4], info.LineCodeMapping, out SymbolInfo wSymbol ) ) )
        {
          w = wSymbol.ToInt32();
        }
        if ( ( paramTokens.Count >= 6 )
        && ( EvaluateTokens( lineIndex, paramTokens[5], info.LineCodeMapping, out SymbolInfo hSymbol ) ) )
        {
          h = hSymbol.ToInt32();
        }
        if ( ( x < 0 )
        ||   ( x >= screenProject.ScreenWidth )
        ||   ( y < 0 )
        ||   ( y >= screenProject.ScreenHeight )
        ||   ( w < 0 )
        ||   ( x + w > screenProject.ScreenWidth )
        ||   ( h < 0 )
        ||   ( y + h > screenProject.ScreenHeight )
        ||   ( x % 8 != 0 )
        ||   ( y % 8 != 0 )
        ||   ( w % 8 != 0 )
        ||   ( h % 8 != 0 ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid coordinates (x,y,width,height must be multiples of 8)" );
          return false;
        }

        dataToInclude.Clear();

        bool    importAsMC = !method.Contains( "BITMAPHIRES" );

        GR.Memory.ByteBuffer screenChar   = new GR.Memory.ByteBuffer();
        GR.Memory.ByteBuffer screenColor  = new GR.Memory.ByteBuffer();
        GR.Memory.ByteBuffer bitmapData   = new GR.Memory.ByteBuffer();

        if ( !method.Contains( "BITMAPHIRES" ) )
        {
          importAsMC = ( screenProject.SelectedCheckType == Formats.GraphicScreenProject.CheckType.MULTICOLOR_BITMAP );
        }

        if ( importAsMC )
        {
          screenProject.ImageToMCBitmapData( screenProject.ColorMapping, null, null, x, y, w / 8, h / 8, out bitmapData, out screenChar, out screenColor );
        }
        else
        {
          screenProject.ImageToHiresBitmapData( screenProject.ColorMapping, null, null, x, y, w / 8, h / 8, out bitmapData, out screenChar, out screenColor );
        }

        GR.Memory.ByteBuffer    bitmapClipped = new GR.Memory.ByteBuffer( (uint)( w / 8 * h / 8 * 8 ) );
        GR.Memory.ByteBuffer    colorClipped = new GR.Memory.ByteBuffer( (uint)( w / 8 * h / 8 ) );
        GR.Memory.ByteBuffer    screenClipped = new GR.Memory.ByteBuffer( (uint)( w / 8 * h / 8 ) );

        int numBytesCopiedBM = 0;
        int numBytesCopiedColor = 0;
        for ( int j = 0; j < h / 8; ++j )
        {
          bitmapData.CopyTo( bitmapClipped, ( y + j * 8 ) * screenProject.ScreenWidth / 8 + x, ( w / 8 ) * 8, numBytesCopiedBM );
          numBytesCopiedBM += w;

          screenChar.CopyTo( screenClipped, ( y / 8 + j ) * screenProject.ScreenWidth / 8 + x / 8, w / 8, numBytesCopiedColor );
          screenColor.CopyTo( colorClipped, ( y / 8 + j ) * screenProject.ScreenWidth / 8 + x / 8, w / 8, numBytesCopiedColor );
          numBytesCopiedColor += w / 8;
        }

        string textToInclude = "";

        if ( method.StartsWith( "BITMAP" ) )
        {
          if ( !Binary )
          {
            textToInclude += labelPrefix + "_BITMAP_DATA" + System.Environment.NewLine;
            textToInclude += Util.ToASMData( bitmapClipped, false, 0, MacroByType( Types.MacroInfo.PseudoOpType.BYTE ) ) + System.Environment.NewLine;
          }
          else
          {
            dataToInclude.Append( bitmapClipped );
          }
        }
        if ( method.IndexOf( "SCREEN" ) != -1 )
        {
          if ( !Binary )
          {
            textToInclude += labelPrefix + "_SCREEN_DATA" + System.Environment.NewLine;
            textToInclude += Util.ToASMData( screenClipped, false, 0, MacroByType( Types.MacroInfo.PseudoOpType.BYTE ) ) + System.Environment.NewLine;
          }
          else
          {
            dataToInclude.Append( screenClipped );
          }
        }
        if ( method.IndexOf( "COLOR" ) != -1 )
        {
          if ( !Binary )
          {
            textToInclude += labelPrefix + "_COLOR_DATA" + System.Environment.NewLine;
            textToInclude += Util.ToASMData( colorClipped, false, 0, MacroByType( Types.MacroInfo.PseudoOpType.BYTE ) ) + System.Environment.NewLine;
          }
          else
          {
            dataToInclude.Append( colorClipped );
          }
        }

        if ( !Binary )
        {
          ReplacementLines = textToInclude.Split( new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries );
        }
      }
      else if ( extension == ".MAPPROJECT" )
      {
        // map project
        // map,index,count
        // tile,index,count
        // maptile
        // char,index,count
        if ( paramTokens.Count > 4 )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected <Map|Tile|MapTile|TileElements|TileData>[,<Index>[,<Count>]]" );
          return false;
        }
        if ( ( method != "TILE" )
        &&   ( method != "TILEDATA" )
        &&   ( method != "MAP" )
        &&   ( method != "MAPEXTRADATA" )
        &&   ( method != "MAPVERTICAL" )
        &&   ( method != "TILEELEMENTS" )
        &&   ( method != "CHAR" )
        &&   ( method != "MAPTILE" )
        &&   ( method != "MAPVERTICALTILE" ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Unknown method '" + method + "', supported values for this file name are MAP, MAPVERTICAL, TILE, TILEDATA, TILEELEMENTS, MAPTILE, MAPVERTICALTILE, MAPEXTRADATA, CHAR" );
          return false;
        }

        if ( ( Binary )
        &&   ( method != "CHAR" ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Binary export from map project is only supported for method 'CHAR'" );
          return false;
        }

        Formats.MapProject map = new RetroDevStudio.Formats.MapProject();

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
        if ( !map.ReadFromBuffer( dataToInclude ) )
        {
          AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read map project from " + subFilename );
          return false;
        }

        string textToInclude = "";

        if ( method == "TILEELEMENTS" )
        {
          map.ExportTilesAsElements( out textToInclude, labelPrefix, false, 0, MacroByType( RetroDevStudio.Types.MacroInfo.PseudoOpType.BYTE ) );
        }
        else if ( method == "MAP" )
        {
          map.ExportMapsAsAssembly( false, out textToInclude, labelPrefix, false, 0, MacroByType( RetroDevStudio.Types.MacroInfo.PseudoOpType.BYTE ) );
        }
        else if ( method == "MAPEXTRADATA" )
        {
          map.ExportMapExtraDataAsAssembly( out textToInclude, labelPrefix, false, 0, MacroByType( RetroDevStudio.Types.MacroInfo.PseudoOpType.BYTE ) );
        }
        else if ( method == "MAPVERTICAL" )
        {
          map.ExportMapsAsAssembly( true, out textToInclude, labelPrefix, false, 0, MacroByType( RetroDevStudio.Types.MacroInfo.PseudoOpType.BYTE ) );
        }
        else if ( method == "TILE" )
        {
          map.ExportTilesAsAssembly( out textToInclude, labelPrefix, false, 0, MacroByType( RetroDevStudio.Types.MacroInfo.PseudoOpType.BYTE ) );
        }
        else if ( method == "TILEDATA" )
        {
          map.ExportTileDataAsAssembly( out textToInclude, labelPrefix, false, 0, MacroByType( RetroDevStudio.Types.MacroInfo.PseudoOpType.BYTE ) );
        }
        else if ( method == "CHAR" )
        {
          int startIndex = 0;
          int numChars = 256;

          if ( ( paramTokens.Count >= 3 )
          &&   ( EvaluateTokens( lineIndex, paramTokens[2], info.LineCodeMapping, out SymbolInfo startIndexSymbol ) ) )
          {
            startIndex = (int)startIndexSymbol.ToInteger();
          }
          if ( paramTokens.Count >= 4 )
          {
            if ( !EvaluateTokens( lineIndex, paramTokens[3], info.LineCodeMapping, out SymbolInfo numCharsSymbol ) )
            {
              AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Failed to evaluate expression " + TokensToExpression( paramTokens[3] ) );
              return false;
            }
            numChars = (int)numCharsSymbol.ToInteger();
          }

          if ( ( startIndex < 0 )
          ||   ( startIndex >= map.Charset.TotalNumberOfCharacters ) )
          {
            AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid start index " + startIndex );
            return false;
          }
          if ( ( numChars <= 0 )
          ||   ( ( startIndex + numChars ) > map.Charset.TotalNumberOfCharacters ) )
          {
            AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid char count " + numChars );
            return false;
          }

          dataToInclude = map.Charset.CharacterData( startIndex, numChars );
        }
        else if ( method == "MAPTILE" )
        {
          string  dummy;
          map.ExportTilesAsAssembly( out dummy, labelPrefix, false, 0, MacroByType( RetroDevStudio.Types.MacroInfo.PseudoOpType.BYTE ) );
          textToInclude += dummy;

          map.ExportMapsAsAssembly( false, out dummy, labelPrefix, false, 0, MacroByType( RetroDevStudio.Types.MacroInfo.PseudoOpType.BYTE ) );
          textToInclude += dummy;
          //Debug.Log( textToInclude );
        }
        else if ( method == "MAPVERTICALTILE" )
        {
          string  dummy;
          map.ExportTilesAsAssembly( out dummy, labelPrefix, false, 0, MacroByType( RetroDevStudio.Types.MacroInfo.PseudoOpType.BYTE ) );
          textToInclude += dummy;

          map.ExportMapsAsAssembly( true, out dummy, labelPrefix, false, 0, MacroByType( RetroDevStudio.Types.MacroInfo.PseudoOpType.BYTE ) );
          textToInclude += dummy;
          //Debug.Log( textToInclude );
        }

        if ( !Binary )
        {
          ReplacementLines = textToInclude.Split( new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries );
        }
        else
        {
        }
      }
      else
      {
        AddError( lineIndex, RetroDevStudio.Types.ErrorCode.E2002_UNSUPPORTED_FILE_TYPE, "Unknown file type" );
        return false;
      }

      ExternallyIncludedFiles.Add( subFilename );

      if ( !Binary )
      {
        return true;
      }

      info.LineData = dataToInclude;
      info.NumBytes = (int)info.LineData.Length;
      lineSizeInBytes = (int)dataToInclude.Length;
      return true;
    }






  }
}
