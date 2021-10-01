using RetroDevStudio;
using System;



namespace C64Studio.Converter
{
  class KoalaToBitmap
  {
    /*
        Koalafile http://www.c64-wiki.de/index.php?title=Koala_Painter#Koala-Dateiformat
        Koalaformat has an Loading address (first two bytes) [00 60]
        ====================================================================
        Offset (hex): Content
        --------------------------------------------------------------------
        0000 - 1F3F : Bitmap 8000 Bytes
        1F40 - 2327 : Screen-RAM 1000 Bytes
        2328 - 270F : Color-RAM 1000 Bytes
        2710        : Background 1 Byte


        C64 MC Mode http://www.c64-wiki.de/index.php/Multicolor
        ====================================================================
        Color-Bits	Corresponding-Color	                        Address
        --------------------------------------------------------------------
        00	        Background	                                53281
        01	        Upper (four Bits/Nibble)...                 1024-2023
        10	        Lower (four Bits/Nibble) of Screen-RAM	    1024-2023
        11	        Color-RAM                                   55296-56295
      */


    public static GR.Memory.ByteBuffer KoalaFromBitmap( GR.Memory.ByteBuffer BitmapData, GR.Memory.ByteBuffer ScreenRAM, GR.Memory.ByteBuffer ColorRAM, byte BackgroundColor )
    {
      GR.Memory.ByteBuffer      result = new GR.Memory.ByteBuffer();
      result.AppendU16( 0x6000 );
      result.Append( BitmapData );
      result.Append( ScreenRAM );
      result.Append( ColorRAM );
      result.AppendU8( BackgroundColor );

      return result;
    }



    public static GR.Image.MemoryImage BitmapFromKoala( GR.Memory.ByteBuffer koala )
    {
      byte fullValue;
      byte value;
      byte currentColor;

      int xCooBase = 0;
      int yCooBase = 0;

      GR.Image.MemoryImage Image = new GR.Image.MemoryImage( 320, 200, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );

      // Set the palette to the C64 one
      PaletteManager.ApplyPalette( Image );

      if ( koala.Length >= 10002 )
      {
        // Last Byte is Background Color and global
        Byte backgroundColor = koala.ByteAt( 10002 );

        for ( int y = 0; y < 1000; y++ )
        {
          // koala has 3 colors which can be selected for ech "cell" (8x8 pixel) 
          byte lowerNibbleColor = (byte)( ( ( ( ( koala.ByteAt( y + 8002 ) ) << 4 ) & 0xFF ) >> 4 ) );
          byte upperNibbleColor = (byte)( koala.ByteAt( y + 8002 ) >> 4 );
          byte colorRamColor    = koala.ByteAt( y + 9002 );

          for ( int x = 0; x < 8; x++ )
          {
            fullValue = koala.ByteAt( ( 8 * y ) + x + 2 );

            for ( int z = 0; z < 4; z++ )
            {
              value = (byte)( ( ( fullValue << ( z * 2 ) ) & 0xFF ) >> 6 );

              switch ( value )
              {
                case 0:
                  currentColor = backgroundColor;
                  break;
                case 1:
                  currentColor = upperNibbleColor;
                  break;
                case 2:
                  currentColor = lowerNibbleColor;
                  break;
                default:
                  currentColor = colorRamColor;
                  break;
              }
              Image.SetPixel( ( ( xCooBase * 4 ) * 2 + ( z * 2 ) ), yCooBase * 8 + x, currentColor );
              // koala is doublepixel, so we repeat it for the right neighbour
              Image.SetPixel( ( ( xCooBase * 4) * 2 + ( z * 2 ) + 1 ), yCooBase * 8 + x, currentColor );
            }
          }

          xCooBase++;
          if ( xCooBase == 40 )
          {
            xCooBase = 0;
            yCooBase++;
          }
        }
      }
      return Image;
    }



    public static GR.Image.MemoryImage BitmapFromKoala( string Filename )
    {
      GR.Memory.ByteBuffer byteBuffer = GR.IO.File.ReadAllBytes( Filename );
      return BitmapFromKoala( byteBuffer );
    }
  }
}
