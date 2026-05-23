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
    private bool POIncludeMediaCharscreen( int lineIndex, bool binary, string subFilename, string method, List<List<TokenInfo>> paramTokens, string labelPrefix, out ByteBuffer dataToInclude, out string[] replacementLines )
    {
      replacementLines  = null;
      dataToInclude     = null;

      // screen
      // char,x,y,width,height[,start,count]
      // color,x,y,width,height[,start,count]
      // charcolor,x,y,width,height[,start,count]
      // colorchar,x,y,width,height[,start,count]
      // charvert,x,y,width,height[,start,count]
      // colorvert,x,y,width,height[,start,count]
      // charcolorvert,x,y,width,height[,start,count]
      // colorcharvert,x,y,width,height[,start,count]
      // char[,start,count]
      // color[,start,count]
      // charcolor[,start,count]
      // colorchar[,start,count]
      // charvert[,start,count]
      // colorvert[,start,count]
      // charcolorvert[,start,count]
      // colorcharvert[,start,count]
      // palette[,start,count]
      if ( paramTokens.Count > 8 )
      {
        AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected <Char|CharVert|Color|ColorVert|CharColor|CharColorVert|ColorChar|ColorCharVert|CharSet|CharLabels>[,<X>[,<Y>[,<Width>[,<Height>[,<First Screen Index>[,<Screen Count>]]]]]" );
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
      &&   ( method != "CHARLABEL" )
      &&   ( method != "PALETTE" )
      &&   ( method != "PALETTESWIZZLED" )
      &&   ( method != "PALETTERGB" )
      &&   ( method != "PALETTERGBSWIZZLED" ) )
      {
        AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Unknown method '" + method + "', supported values for this file name are CHAR, COLOR, CHARCOLOR, COLORCHAR, CHARVERT, COLORVERT, CHARCOLORVERT, COLORCHARVERT, CHARCOLORINTERLEAVED, CHARCOLORINTERLEAVEDVERT, CHARSET, CHARLABEL, PALETTE, PALETTERGB, PALETTESWIZZLED and PALETTERGBSWIZZLED" );
        return false;
      }

      var screenProject = new RetroDevStudio.Formats.CharsetScreenProject();

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
        if ( !binary )
        {
          AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Export as CHARSET is only supported for binary" );
          return false;
        }

        int startIndex = 0;
        int numChars = screenProject.CharSet.TotalNumberOfCharacters;

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
        ||   ( startIndex >= screenProject.CharSet.TotalNumberOfCharacters ) )
        {
          AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Invalid start index" );
          return false;
        }
        if ( numChars <= 0 )
        {
          AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Invalid number of characters, must be >= 1" );
          return false;
        }
        if ( startIndex + numChars > screenProject.CharSet.TotalNumberOfCharacters )
        {
          AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Invalid number of characters, charset has " 
                + screenProject.CharSet.TotalNumberOfCharacters + " characters, but we're trying to fetch up to " + ( startIndex + numChars ) );
          return false;
        }
        dataToInclude = screenProject.CharSet.CharacterData( startIndex, numChars );
      }
      else if ( ( method == "PALETTE" )
      ||        ( method == "PALETTESWIZZLED" )
      ||        ( method == "PALETTERGB" )
      ||        ( method == "PALETTERGBSWIZZLED" ) )
      {
        if ( !binary )
        {
          AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Export as palette is only supported for binary" );
          return false;
        }

        int startIndex = 0;
        int numColors = screenProject.CharSet.Colors.Palette.NumColors;

        if ( ( paramTokens.Count >= 3 )
        &&   ( EvaluateTokens( lineIndex, paramTokens[2], out SymbolInfo startIndexSymbol ) ) )
        {
          startIndex = startIndexSymbol.ToInt32();
        }
        if ( ( paramTokens.Count >= 4 )
        &&   ( EvaluateTokens( lineIndex, paramTokens[3], out SymbolInfo numColorsSymbol ) ) )
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
      else if ( method == "CHARLABEL" )
      {
        if ( binary )
        {
          AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Export as CHARLABEL is only supported for text" );
          return false;
        }
        int startIndex = 0;
        int numChars = screenProject.CharSet.TotalNumberOfCharacters;

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
        ||   ( startIndex >= screenProject.CharSet.TotalNumberOfCharacters ) )
        {
          AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Invalid start index" );
          return false;
        }
        if ( numChars <= 0 )
        {
          AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Invalid number of characters, must be >= 1" );
          return false;
        }
        if ( startIndex + numChars > screenProject.CharSet.TotalNumberOfCharacters )
        {
          AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Invalid number of characters, charset has " 
                + screenProject.CharSet.TotalNumberOfCharacters + " characters, but we're trying to fetch up to " + ( startIndex + numChars ) );
          return false;
        }
        screenProject.CharSet.ExportCharacterNamesAsAssembly( startIndex, numChars, out var textToInclude, labelPrefix );
        replacementLines = textToInclude.Split( new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries );
      }
      else
      {
        int   x = 0;
        int   y = 0;
        int   w = -1;
        int   h = -1;
        int   startIndex = 0;
        int   count = screenProject.Screens.Count;

        if ( paramTokens.Count == 4 )
        {
          if ( ( EvaluateTokens( lineIndex, paramTokens[2], out SymbolInfo startSymbol ) )
          &&   ( EvaluateTokens( lineIndex, paramTokens[3], out SymbolInfo countSymbol ) ) )
          {
            startIndex = startSymbol.ToInt32();
            count = countSymbol.ToInt32();
          }
        }
        else
        {
          if ( ( paramTokens.Count >= 3 )
          &&   ( EvaluateTokens( lineIndex, paramTokens[2], out SymbolInfo xSymbol ) ) )
          {
            x = xSymbol.ToInt32();
          }
          if ( ( paramTokens.Count >= 4 )
          &&   ( EvaluateTokens( lineIndex, paramTokens[3], out SymbolInfo ySymbol ) ) )
          {
            y = ySymbol.ToInt32();
          }
          if ( ( paramTokens.Count >= 5 )
          &&   ( EvaluateTokens( lineIndex, paramTokens[4], out SymbolInfo wSymbol ) ) )
          {
            w = wSymbol.ToInt32();
          }
          if ( ( paramTokens.Count >= 6 )
          &&   ( EvaluateTokens( lineIndex, paramTokens[5], out SymbolInfo hSymbol ) ) )
          {
            h = hSymbol.ToInt32();
          }
          if ( ( paramTokens.Count >= 7 )
          &&   ( EvaluateTokens( lineIndex, paramTokens[6], out SymbolInfo sSymbol ) ) )
          {
            startIndex = sSymbol.ToInt32();
          }
          if ( ( paramTokens.Count >= 8 )
          &&   ( EvaluateTokens( lineIndex, paramTokens[7], out SymbolInfo cSymbol ) ) )
          {
            count = cSymbol.ToInt32();
          }
          else
          {
            count = screenProject.Screens.Count - startIndex;
          }
        }
        if ( ( startIndex + count > screenProject.Screens.Count )
        ||   ( startIndex < 0 )
        ||   ( count <= 0 ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Start Index/Count out of bounds" );
          return false;
        }

        dataToInclude.Clear();

        string    textToInclude = "";

        var combinedReplacementLines = new List<string>();

        for ( int screenIndex = 0; screenIndex < count; ++screenIndex )
        {
          var screen = screenProject.Screens[startIndex + screenIndex];

          int widthToUse = w;
          int heightToUse = h;

          if ( widthToUse == -1 )
          {
            widthToUse = screen.Width;
          }
          if ( heightToUse == -1 )
          {
            heightToUse = screen.Height;
          }
          if ( ( x < 0 )
          ||   ( x >= screen.Width )
          ||   ( y < 0 )
          ||   ( y >= screen.Height )
          ||   ( widthToUse < 0 )
          ||   ( x + widthToUse > screen.Width )
          ||   ( heightToUse < 0 )
          ||   ( y + heightToUse > screen.Height ) )
          {
            AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid coordinates" );
            return false;
          }

          var exportInfo = new ExportCharsetScreenInfo();
          exportInfo.RowByRow = !method.EndsWith( "VERT" );
          exportInfo.Area = new GR.Math.Rectangle( x, y, widthToUse, heightToUse );
          exportInfo.ScreensToExport.Add( screenIndex );

          if ( !screenProject.ExportToBuffer( exportInfo ) )
          {
            AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Failed to export data from " + subFilename );
            return false;
          }

          if ( ( method == "CHAR" )
          ||   ( method == "CHARVERT" ) )
          {
            if ( !binary )
            {
              textToInclude += OptionalPrefix( labelPrefix, "_", "CHARS" ) + System.Environment.NewLine;
              textToInclude += Util.ToASMData( exportInfo.ScreenCharData, false, 0, MacroByType( Types.MacroInfo.PseudoOpType.BYTE ) );

              combinedReplacementLines.AddRange( textToInclude.Split( new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries ) );
            }
            else
            {
              dataToInclude += exportInfo.ScreenCharData;
            }
          }
          else if ( ( method == "COLOR" )
          ||        ( method == "COLORVERT" ) )
          {
            if ( !binary )
            {
              textToInclude += OptionalPrefix( labelPrefix, "_", "COLOR" ) + System.Environment.NewLine;
              textToInclude += Util.ToASMData( exportInfo.ScreenColorData, false, 0, MacroByType( Types.MacroInfo.PseudoOpType.BYTE ) );
              combinedReplacementLines.AddRange( textToInclude.Split( new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries ) );
            }
            else
            {
              dataToInclude += exportInfo.ScreenColorData;
            }
          }
          else if ( ( method == "CHARCOLOR" )
          ||        ( method == "CHARCOLORVERT" ) )
          {
            if ( !binary )
            {
              textToInclude += OptionalPrefix( labelPrefix, "_", "CHARS" ) + System.Environment.NewLine;
              textToInclude += Util.ToASMData( exportInfo.ScreenCharData, false, 0, MacroByType( Types.MacroInfo.PseudoOpType.BYTE ) ) + System.Environment.NewLine;
              textToInclude += OptionalPrefix( labelPrefix, "_", "COLOR" ) + System.Environment.NewLine;
              textToInclude += Util.ToASMData( exportInfo.ScreenColorData, false, 0, MacroByType( Types.MacroInfo.PseudoOpType.BYTE ) );

              combinedReplacementLines.AddRange( textToInclude.Split( new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries ) );
            }
            else
            {
              dataToInclude += exportInfo.ScreenCharData + exportInfo.ScreenColorData;
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

            if ( !binary )
            {
              textToInclude += OptionalPrefix( labelPrefix, "_", "DATA" ) + System.Environment.NewLine;
              textToInclude += Util.ToASMData( interleavedBuffer, false, 0, MacroByType( Types.MacroInfo.PseudoOpType.BYTE ) ) + System.Environment.NewLine;

              combinedReplacementLines.AddRange( textToInclude.Split( new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries ) );
            }
            else
            {
              dataToInclude += interleavedBuffer;
            }
          }
          else if ( ( method == "COLORCHAR" )
          ||        ( method == "COLORCHARVERT" ) )
          {
            if ( !binary )
            {
              textToInclude = OptionalPrefix( labelPrefix, "_", "COLOR" ) + System.Environment.NewLine;
              textToInclude += Util.ToASMData( exportInfo.ScreenColorData, false, 0, MacroByType( Types.MacroInfo.PseudoOpType.BYTE ) ) + System.Environment.NewLine;
              textToInclude += OptionalPrefix( labelPrefix, "_", "CHARS" ) + System.Environment.NewLine;
              textToInclude += Util.ToASMData( exportInfo.ScreenCharData, false, 0, MacroByType( Types.MacroInfo.PseudoOpType.BYTE ) );
              combinedReplacementLines.AddRange( textToInclude.Split( new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries ) );
            }
            else
            {
              dataToInclude += exportInfo.ScreenColorData + exportInfo.ScreenCharData;
            }
          }
        }
        replacementLines = combinedReplacementLines.ToArray();
      }
      return true;
    }


  }
}
