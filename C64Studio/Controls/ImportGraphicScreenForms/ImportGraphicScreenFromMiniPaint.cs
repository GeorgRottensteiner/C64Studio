using RetroDevStudio.Formats;
using RetroDevStudio.Types;
using GR.Memory;
using System;
using RetroDevStudio.Documents;
using GR.Image;



namespace RetroDevStudio.Controls
{
  public partial class ImportGraphicScreenFromMiniPaint : ImportGraphicScreenFormBase
  {
    public ImportGraphicScreenFromMiniPaint() :
      base( null )
    { 
    }



    public ImportGraphicScreenFromMiniPaint( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();
    }



    public override bool HandleImport( GraphicScreenProject Project, GraphicScreenEditor Editor )
    {
      if ( !Editor.OpenFile( "Open MiniPaint .prg file", Constants.FILEFILTER_PRG + Constants.FILEFILTER_ALL, out string filename ) )
      {
        return false;
      }
      GR.Memory.ByteBuffer data = GR.IO.File.ReadAllBytes( filename );

      if ( ( data.Length != 4097 )
      ||   ( data.UInt16At( 0 ) != 0x10f1 ) )
      {
        // not a valid MiniPaint file
        return false;
      }
      // A MG picture format file is defined as follows: 
      // • CBM DOS file type: PRG
      // • length:
      //       exactly 4097 bytes, including load address, comprised of: 
      // • load address: 2 bytes, $10F1, written in little - endian.
      // • BASIC stub: 13 bytes, a BASIC program with one line: '2008 SYS 
      // 8584'. This is supposed to call the display routine, when the file is LOADed to the 
      // normal BASIC start at $1201, and then RUN.
      // • VIC register values:
      //       2 bytes, contents of $900E and $900F, stored in that
      // order. The lower nibble of $900E, containing the volume, is stored as zero.
      // MINIPAINT forces bit 3 of $900F(‘inverse video’) to 1 on load, i.e.normal, noninverse video. 
      // • bitmap:
      //       3840 bytes, contents of addresses $1100 to $1FFF, 
      // stored in column - major order:
      //       The first 192 bytes define the first 8 hires pixel
      // columns. Within each byte, the left - most pixel is in the most significant bit. 
      // • compressed colour RAM:
      //       120 bytes, compressed contents of addresses $9400 to
      // $94EF in row - major order. The lower nibbles of two consecutive bytes in the colour
      // RAM are combined into one byte.Of the byte pair, that one with the lower address
      // ( left ) is stored in the lower nibble.
      // • display routine: 120 bytes, saved from ‘addresses’ $2078.. $20EF, but
      // assumed to be run at address $2188 when the file is loaded to the normal BASIC start
      // at $1201.Reconstructs the address generating text screen, reprograms all necessary
      // VIC registers, and restores the bitmap and colour RAM at the correct place. After a
      // key press, the VIC - 20 then resets. 

      var image = new MemoryImage( 160, 192, GR.Drawing.PixelFormat.Format8bppIndexed );
      PaletteManager.ApplyPalette( image, Core.Imaging.PaletteFromMachine( MachineType.VIC20 ) );
      int widthInBytes = 160 / 8;
      byte auxColor     = (byte)( data.ByteAt( 15 ) >> 4 );
      byte borderColor  = (byte)( data.ByteAt( 16 ) & 0x07 );
      byte bgColor      = (byte)( data.ByteAt( 16 ) >> 4 );
      for ( int x = 0; x < widthInBytes; ++x )
      {
        for ( int y = 0; y < 192; ++y )
        {
          var nextByte = data.ByteAt( 17 + x * 192 + y );
          var colorByte = data.ByteAt( 17 + 3840 + ( y / 16 ) * 10 + x / 2 );
          var colorValue = 0;
          if ( ( x % 2 ) == 0 )
          {
            colorValue = colorByte & 0x0f;
          }
          else
          {
            colorValue = ( colorByte >> 4 ) & 0x0f;
          }
          if ( colorValue >= 8 )
          {
            // multicolor
            for ( int i = 0; i < 4; ++i )
            {
              var bitPattern = ( ( nextByte & ( 3 << ( ( 3 - i ) * 2 ) ) ) >> ( ( 3 - i ) * 2 ) ) & 0x03;

              switch ( bitPattern )
              {
                case 2:
                  image.SetPixel( x * 8 + i * 2, y, (uint)( colorValue - 8 ) );
                  image.SetPixel( x * 8 + i * 2 + 1, y, (uint)( colorValue - 8 ) );
                  break;
                case 3:
                  image.SetPixel( x * 8 + i * 2, y, auxColor );
                  image.SetPixel( x * 8 + i * 2 + 1, y, auxColor );
                  break;
                case 1:
                  image.SetPixel( x * 8 + i * 2, y, borderColor );
                  image.SetPixel( x * 8 + i * 2 + 1, y, borderColor );
                  break;
                case 0:
                  image.SetPixel( x * 8 + i * 2, y, bgColor );
                  image.SetPixel( x * 8 + i * 2 + 1, y, bgColor );
                  break;
              }
            }
          }
          else
          {
            for ( int i = 0; i < 8; ++i )
            {
              if ( ( nextByte & ( 1 << ( 7 - i ) ) ) != 0 )
              {
                image.SetPixel( x * 8 + i, y, (uint)colorValue );
              }
              else
              {
                image.SetPixel( x * 8 + i, y, bgColor );
              }
            }
          }
        }
      }

      return Editor.ImportImage( null, image, GraphicScreenEditor.ImageInsertionMode.AS_FULL_SCREEN );
    }



  }
}
