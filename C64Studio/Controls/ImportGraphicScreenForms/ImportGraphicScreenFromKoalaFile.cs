using RetroDevStudio.Formats;
using RetroDevStudio.Types;
using GR.Memory;
using System;
using RetroDevStudio.Documents;



namespace RetroDevStudio.Controls
{
  public partial class ImportGraphicScreenFromKoalaFile : ImportGraphicScreenFormBase
  {
    public ImportGraphicScreenFromKoalaFile() :
      base( null )
    { 
    }



    public ImportGraphicScreenFromKoalaFile( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();
    }



    public override bool HandleImport( GraphicScreenProject Project, GraphicScreenEditor Editor )
    {
      string    filename;
      if ( Editor.OpenFile( "Open Koala file", Types.Constants.FILEFILTER_ALL, out filename ) )
      {
        GR.Memory.ByteBuffer imageData = GR.IO.File.ReadAllBytes( filename );

        // 0000 - 1F3F : Bitmap 8000 Bytes
        // 1F40 - 2327 : Bildschirmspeicher 1000 Bytes
        // 2328 - 270F : Farb-RAM 1000 Bytes
        // 2710        : Hintergrundfarbe 1 Byte

        if ( imageData.Length == 10003 )
        {
          // could be a Koala painter image
          if ( imageData.UInt16At( 0 ) == 0x6000 )
          {
            // background color
            Project.Colors.BackgroundColor = imageData.ByteAt( 10002 ) % 16;

            int blockWidth  = ( Project.ScreenWidth + 7 ) / 8;
            int blockHeight = ( Project.ScreenHeight + 7 ) / 8;

            for ( int i = 0; i < blockWidth * blockHeight; ++i )
            {
              byte screenByte = imageData.ByteAt( 2 + 8000 + i );
              byte colorByte = imageData.ByteAt( 2 + 8000 + 1000 + i );
              for ( int j = 0; j < 8; ++j )
              {
                byte pixelData = imageData.ByteAt( 2 + i * 8 + j );

                byte pixelMask = 0xc0;
                for ( int k = 0; k < 4; ++k )
                {
                  byte byteValue = (byte)( pixelData & pixelMask );

                  byteValue >>= 6 - 2 * k;

                  int     colorIndex = Project.Colors.BackgroundColor;

                  switch ( byteValue )
                  {
                    case 0:
                      // background
                      break;
                    case 0x01:
                      colorIndex = ( screenByte >> 4 );
                      break;
                    case 0x02:
                      colorIndex = screenByte & 0x0f;
                      break;
                    case 0x03:
                      colorIndex = colorByte % 16;
                      break;
                  }
                  Project.Image.SetPixel( ( i % blockWidth ) * 8 + k * 2, ( i / blockWidth ) * 8 + j, (uint)colorIndex );
                  Project.Image.SetPixel( ( i % blockWidth ) * 8 + k * 2 + 1, ( i / blockWidth ) * 8 + j, (uint)colorIndex );
                  pixelMask >>= 2;
                }
              }
            }
            Editor.DataImported();
          }
        }
      }

      return true;
    }



  }
}
