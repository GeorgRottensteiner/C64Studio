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
    private bool POIncludeMediaMapProject( int lineIndex, bool binary, string subFilename, string method, List<List<TokenInfo>> paramTokens, string labelPrefix, out ByteBuffer dataToInclude, out string[] replacementLines )
    {
      replacementLines  = null;
      dataToInclude     = null;

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
      &&   ( method != "CHARLABEL" )
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

      if ( ( binary )
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
        map.ExportTileNamesAsAssembly( out string textToIncludeTiles, labelPrefix );
        textToInclude = textToIncludeTiles + textToInclude;
      }
      else if ( method == "MAP" )
      {
        map.ExportMapsAsAssembly( false, out textToInclude, labelPrefix, false, 0, MacroByType( RetroDevStudio.Types.MacroInfo.PseudoOpType.BYTE ) );
        map.ExportTileNamesAsAssembly( out string textToIncludeTiles, labelPrefix );
        textToInclude = textToIncludeTiles + textToInclude;
      }
      else if ( method == "MAPEXTRADATA" )
      {
        map.ExportMapExtraDataAsAssembly( out textToInclude, labelPrefix, false, 0, MacroByType( RetroDevStudio.Types.MacroInfo.PseudoOpType.BYTE ) );
      }
      else if ( method == "MAPVERTICAL" )
      {
        map.ExportMapsAsAssembly( true, out textToInclude, labelPrefix, false, 0, MacroByType( RetroDevStudio.Types.MacroInfo.PseudoOpType.BYTE ) );
        map.ExportTileNamesAsAssembly( out string textToIncludeTiles, labelPrefix );
        textToInclude = textToIncludeTiles + textToInclude;
      }
      else if ( method == "TILE" )
      {
        map.ExportTilesAsAssembly( out textToInclude, labelPrefix, false, 0, MacroByType( RetroDevStudio.Types.MacroInfo.PseudoOpType.BYTE ) );
        map.ExportTileNamesAsAssembly( out string textToIncludeTiles, labelPrefix );
        textToInclude = textToIncludeTiles + textToInclude;
      }
      else if ( method == "TILEDATA" )
      {
        map.ExportTileDataAsAssembly( out textToInclude, labelPrefix, false, 0, MacroByType( RetroDevStudio.Types.MacroInfo.PseudoOpType.BYTE ) );
        map.ExportTileNamesAsAssembly( out string textToIncludeTiles, labelPrefix );
        textToInclude = textToIncludeTiles + textToInclude;
      }
      else if ( method == "CHARLABEL" )
      {
        if ( binary )
        {
          AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Export as CHARLABEL is only supported for text" );
          return false;
        }
        int startIndex = 0;
        int numChars = map.Charset.TotalNumberOfCharacters;

        if ( ( paramTokens.Count >= 3 )
        &&   ( EvaluateTokens( lineIndex, paramTokens[2], out SymbolInfo startIndexSymbol ) ) )
        {
          startIndex = startIndexSymbol.ToInt32();
        }
        if ( ( paramTokens.Count >= 4 )
        &&   ( EvaluateTokens( lineIndex, paramTokens[3], out SymbolInfo numCharsSymbol ) ) )
        {
          numChars = numCharsSymbol.ToInt32();
        }
        if ( ( startIndex < 0 )
        ||   ( startIndex >= map.Charset.TotalNumberOfCharacters ) )
        {
          AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Invalid start index" );
          return false;
        }
        if ( numChars <= 0 )
        {
          AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Invalid number of characters, must be >= 1" );
          return false;
        }
        if ( startIndex + numChars > map.Charset.TotalNumberOfCharacters )
        {
          AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Invalid number of characters, charset has " 
                + map.Charset.TotalNumberOfCharacters + " characters, but we're trying to fetch up to " + ( startIndex + numChars ) );
          return false;
        }
        map.Charset.ExportCharacterNamesAsAssembly( startIndex, numChars, out textToInclude, labelPrefix );
      }
      else if ( method == "CHAR" )
      {
        int startIndex = 0;
        int numChars = 256;

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

        map.ExportTileNamesAsAssembly( out string textToIncludeTiles, labelPrefix );
        textToInclude = textToIncludeTiles + textToInclude;
      }
      else if ( method == "MAPVERTICALTILE" )
      {
        string  dummy;
        map.ExportTilesAsAssembly( out dummy, labelPrefix, false, 0, MacroByType( RetroDevStudio.Types.MacroInfo.PseudoOpType.BYTE ) );
        textToInclude += dummy;

        map.ExportMapsAsAssembly( true, out dummy, labelPrefix, false, 0, MacroByType( RetroDevStudio.Types.MacroInfo.PseudoOpType.BYTE ) );
        textToInclude += dummy;

        map.ExportTileNamesAsAssembly( out string textToIncludeTiles, labelPrefix );
        textToInclude = textToIncludeTiles + textToInclude;
      }

      if ( !binary )
      {
        replacementLines = textToInclude.Split( new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries );
      }
      return true;
    }


  }
}
