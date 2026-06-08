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
    private bool POIncludeMediaCharsetProject( int lineIndex, bool binary, string subFilename, string method, List<List<TokenInfo>> paramTokens, string labelPrefix, out ByteBuffer dataToInclude, out string[] replacementLines )
    {
      replacementLines  = null;
      dataToInclude     = null;

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
      string textToInclude = "";

      if ( ( paramTokens.Count >= 3 )
      &&   ( EvaluateTokens( lineIndex, paramTokens[2], out SymbolInfo startIndexSymbol ) ) )
      {
        startIndex = (int)startIndexSymbol.ToInteger();
      }
      if ( paramTokens.Count >= 4 )
      {
        if ( !EvaluateTokens( lineIndex, paramTokens[3], out SymbolInfo numCharsSymbol ) )
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
        if ( !binary )
        {
          AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Export as palette is only supported for binary" );
          return false;
        }

        long numColors = numChars;

        if ( charProject.Mode == TextCharMode.NES )
        {
          if ( method != "PALETTE" )
          {
            AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Export for NES mode supports only palette" );
            return false;
          }

          // NES uses palette mapping instead of palette
          if ( ( startIndex < 0 )
          ||   ( startIndex >= charProject.Colors.PaletteIndexMapping.Count ) )
          {
            AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Invalid start index" );
            return false;
          }
          if ( numColors <= 0 )
          {
            AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Invalid number of palette mappings, must be >= 1" );
            return false;
          }
          if ( startIndex + numColors > charProject.Colors.PaletteIndexMapping.Count )
          {
            AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Invalid number of palette mappings, charset has "
                  + charProject.Colors.PaletteIndexMapping.Count + " palette mappings, but we're trying to fetch up to " + ( startIndex + numColors ) );
            return false;
          }
          dataToInclude = new ByteBuffer( (uint)( 4 * numColors ) );

          for ( int i = 0; i < numColors; ++i )
          {
            for ( int j = 0; j < 4; ++j )
            {
              dataToInclude.SetU8At( i * 4 + j, (byte)charProject.Colors.PaletteIndexMapping[startIndex + i][j] );
            }
          }
        }
        else
        {
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
      }
      else
      {
        if ( ( startIndex < 0 )
        ||   ( startIndex >= charProject.TotalNumberOfCharacters ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid start index " + startIndex );
          return false;
        }
        if ( ( numChars <= 0 )
        ||   ( ( startIndex + numChars ) > charProject.TotalNumberOfCharacters ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid char count " + numChars );
          return false;
        }

        if ( !binary )
        {
          if ( method != "CHAR" )
          {
            AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Export as assembly is only supported for 'CHAR'" );
            return false;
          }
          charProject.ExportCharacterNamesAsAssembly( startIndex, numChars, out textToInclude, labelPrefix );
        }
        else
        {
          dataToInclude = charProject.CharacterData( startIndex, numChars );
          if ( method == "CHARCOLOR" )
          {
            dataToInclude += charProject.ColorData( startIndex, numChars );
          }
        }
      }

      if ( !binary )
      {
        replacementLines = textToInclude.Split( new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries );
      }
      return true;
    }


  }
}
