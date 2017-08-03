using C64Studio.CustomRenderer;
using C64Studio.Displayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;



namespace Tiny64Cmd
{
  class Emulator
  {
    Tiny64.Machine    Machine = new Tiny64.Machine();




    public void Run()
    {
      Tiny64.Machine    machine = new Tiny64.Machine();

      GR.Image.MemoryImage    img = new GR.Image.MemoryImage( 320, 200, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );
      C64Studio.CustomRenderer.PaletteManager.ApplyPalette( img );

      while ( true )
      {
        // round about one frame
        int     numCycles = 19656;

        machine.RunCycles( numCycles );

        //Debug.Log( machine.CPU.PC.ToString( "X4" ) + ":" + opCode.ToString( "X2" ) + " A:" + CPU.Accu.ToString( "X2" ) + " X:" + CPU.X.ToString( "X2" ) + " Y:" + CPU.Y.ToString( "X2" ) + " " + ( Memory.RAM[0xc1] + ( Memory.RAM[0xc2] << 8 ) ).ToString( "X4" ) );
        //Debug.Log( machine.CPU.PC.ToString( "X4" ) + ": A:" + machine.CPU.Accu.ToString( "X2" ) + " X:" + machine.CPU.X.ToString( "X2" ) + " Y:" + machine.CPU.Y.ToString( "X2" ) + " " + ( machine.Memory.RAM[0xc1] + ( machine.Memory.RAM[0xc2] << 8 ) ).ToString( "X4" ) );

        // render image
        bool  vicActive = ( ( machine.Memory.VIC.ReadByte( 0x11 ) & 0x10 ) != 0 );
        if ( vicActive )
        {
          int   vicBank = ( machine.Memory.CIA2.ReadByte( 0 ) & 0x03 ) ^ 0x03;
          int   screenPos = ( ( machine.Memory.VIC.ReadByte( 0x18 ) & 0xf0 ) >> 4 ) * 1024 + vicBank * 16384;
          int   localCharDataPos = ( machine.Memory.VIC.ReadByte( 0x18 ) & 0x0e ) * 1024;
          int   charDataPos = localCharDataPos + vicBank * 16384;
          byte  bgColor = (byte)( machine.Memory.VIC.ReadByte( 0x21 ) & 0x0f );

          GR.Memory.ByteBuffer    charData = null;
          if ( ( ( vicBank == 0 )
          ||     ( vicBank == 2 ) )
          &&   ( localCharDataPos == 0x1000 ) )
          {
            // use default upper case chars
            charData = C64Studio.Types.ConstantData.UpperCaseCharset;
          }
          else if ( ( ( vicBank == 0 )
          ||          ( vicBank == 2 ) )
          &&        ( localCharDataPos == 0x2000 ) )
          {
            // use default lower case chars
            charData = C64Studio.Types.ConstantData.LowerCaseCharset;
          }
          else
          {
            // use RAM
            charData = new GR.Memory.ByteBuffer( machine.Memory.RAM );
            charData = charData.SubBuffer( charDataPos, 2048 );
          }
          for ( int y = 0; y < 25; ++y )
          {
            for ( int x = 0; x < 40; ++x )
            {
              byte    charIndex = machine.Memory.RAM[screenPos + x + y * 40];
              byte    charColor = machine.Memory.ColorRAM[x + y * 40];

              CharacterDisplayer.DisplayHiResChar( charData.SubBuffer( charIndex * 8, 8 ), bgColor, charColor, img, x * 8, y * 8 );
            }
          }
          DataObject dataObj = new DataObject();

          GR.Memory.ByteBuffer      dibData = img.CreateHDIBAsBuffer();

          System.IO.MemoryStream    ms = dibData.MemoryStream();

          // WTF - SetData requires streams, NOT global data (HGLOBAL)
          dataObj.SetData( "DeviceIndependentBitmap", ms );
          Clipboard.SetDataObject( dataObj, true );
        }
      }
    }



  }
}
