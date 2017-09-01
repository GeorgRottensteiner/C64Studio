using System;
using System.Collections.Generic;
using System.Text;



namespace Tiny64Cmd
{
  public class Emulator
  {
    Tiny64.Machine    Machine = new Tiny64.Machine();

    GR.Collections.MultiMap<ushort,Breakpoint>        Breakpoints = new GR.Collections.MultiMap<ushort,Breakpoint>();



    public int RunCycles( int MaxCycles )
    {
      int cyclesUsed = MaxCycles;
      while ( MaxCycles >= 0 )
      {
        ushort    oldPC = Machine.CPU.PC;
        int curCycles = Machine.RunCycle();

        MaxCycles -= curCycles;
      }
      cyclesUsed -= MaxCycles;
      return cyclesUsed;
    }



    public void Run()
    {
      Machine = new Tiny64.Machine();

      /*
      GR.Image.MemoryImage    img = new GR.Image.MemoryImage( 320, 200, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );
      C64Studio.CustomRenderer.PaletteManager.ApplyPalette( img );*/

      while ( true )
      {
        // round about one frame
        int     numCycles = 19656;

        RunCycles( numCycles );

        //Debug.Log( machine.CPU.PC.ToString( "X4" ) + ":" + opCode.ToString( "X2" ) + " A:" + CPU.Accu.ToString( "X2" ) + " X:" + CPU.X.ToString( "X2" ) + " Y:" + CPU.Y.ToString( "X2" ) + " " + ( Memory.RAM[0xc1] + ( Memory.RAM[0xc2] << 8 ) ).ToString( "X4" ) );
        //Debug.Log( machine.CPU.PC.ToString( "X4" ) + ": A:" + machine.CPU.Accu.ToString( "X2" ) + " X:" + machine.CPU.X.ToString( "X2" ) + " Y:" + machine.CPU.Y.ToString( "X2" ) + " " + ( machine.Memory.RAM[0xc1] + ( machine.Memory.RAM[0xc2] << 8 ) ).ToString( "X4" ) );

        //RenderFullImage( Machine, img );
      }
    }



    private void RenderFullImage( Tiny64.Machine machine, GR.Image.MemoryImage img )
    {
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
        || ( vicBank == 2 ) )
        && ( localCharDataPos == 0x1000 ) )
        {
          // use default upper case chars
          charData = new GR.Memory.ByteBuffer();
          charData.Append( Machine.Memory.CharacterROM, 0, 2048 );
        }
        else if ( ( ( vicBank == 0 )
        || ( vicBank == 2 ) )
        && ( localCharDataPos == 0x2000 ) )
        {
          // use default lower case chars
          charData = new GR.Memory.ByteBuffer();
          charData.Append( Machine.Memory.CharacterROM, 2048, 2048 );
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

            //CharacterDisplayer.DisplayHiResChar( charData.SubBuffer( charIndex * 8, 8 ), bgColor, charColor, img, x * 8, y * 8 );
          }
        }
        /*
        DataObject dataObj = new DataObject();

        GR.Memory.ByteBuffer      dibData = img.CreateHDIBAsBuffer();

        System.IO.MemoryStream    ms = dibData.MemoryStream();

        // WTF - SetData requires streams, NOT global data (HGLOBAL)
        dataObj.SetData( "DeviceIndependentBitmap", ms );
        Clipboard.SetDataObject( dataObj, true );*/
      }
    }



  }
}
