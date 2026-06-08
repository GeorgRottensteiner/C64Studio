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
    private bool POIncludeMediaGraphicScreen( int lineIndex, bool binary, string subFilename, string method, List<List<TokenInfo>> paramTokens, string labelPrefix, out ByteBuffer dataToInclude, out string[] replacementLines )
    {
      replacementLines  = null;
      dataToInclude     = null;

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
      && ( EvaluateTokens( lineIndex, paramTokens[5], out SymbolInfo hSymbol ) ) )
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
        if ( !binary )
        {
          textToInclude += OptionalPrefix( labelPrefix, "_", "BITMAP_DATA" ) + System.Environment.NewLine;
          textToInclude += Util.ToASMData( bitmapClipped, false, 0, MacroByType( Types.MacroInfo.PseudoOpType.BYTE ) ) + System.Environment.NewLine;
        }
        else
        {
          dataToInclude.Append( bitmapClipped );
        }
      }
      if ( method.IndexOf( "SCREEN" ) != -1 )
      {
        if ( !binary )
        {
          textToInclude += OptionalPrefix( labelPrefix, "_", "SCREEN_DATA" ) + System.Environment.NewLine;
          textToInclude += Util.ToASMData( screenClipped, false, 0, MacroByType( Types.MacroInfo.PseudoOpType.BYTE ) ) + System.Environment.NewLine;
        }
        else
        {
          dataToInclude.Append( screenClipped );
        }
      }
      if ( method.IndexOf( "COLOR" ) != -1 )
      {
        if ( !binary )
        {
          textToInclude += OptionalPrefix( labelPrefix, "_", "COLOR_DATA" ) + System.Environment.NewLine;
          textToInclude += Util.ToASMData( colorClipped, false, 0, MacroByType( Types.MacroInfo.PseudoOpType.BYTE ) ) + System.Environment.NewLine;
        }
        else
        {
          dataToInclude.Append( colorClipped );
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
