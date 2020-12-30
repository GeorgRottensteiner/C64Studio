using System;
using System.Collections.Generic;
using System.Text;



namespace Tiny64
{
  public class VIC
  {
    enum Register
    {
      CONTROL_1         = 0x11,   // 17| $d011 |RST8| ECM| BMM| DEN|RSEL|    YSCROLL   | Control register 1
      RASTER            = 0x12,   // 18| $d012 |                 RASTER                | Raster counter
      CONTROL_2         = 0x16,   // 22| $d016 |  - |  - | RES| MCM|CSEL|    XSCROLL   | Control register 2
      MEMORY_CONTROL    = 0x18,   // 24| $d018 |VM13|VM12|VM11|VM10|CB13|CB12|CB11|  - | Memory pointers
      BORDER_COLOR      = 0x20,   // 32
      BACKGROUND_COLOR  = 0x21
    };

    enum GraphicMode
    {
      UNDEFINED,
      CHAR_HIRES,
      CHAR_ECM,
      CHAR_MC,
      BITMAP_HIRES,
      BITMAP_MC
    };

    //          | Video  | # of  | Visible | Cycles/ |  Visible
    //   Type   | system | lines |  lines  |  line   | pixels/line
    // ---------+--------+-------+---------+---------+------------
    // 6567R56A | NTSC-M |  262  |   234   |   64    |    411
    //  6567R8  | NTSC-M |  263  |   235   |   65    |    418
    //   6569   |  PAL-B |  312  |   284   |   63    |    403
    //
    //          | First  |  Last  |              |   First    |   Last
    //          | vblank | vblank | First X coo. |  visible   |  visible
    //   Type   |  line  |  line  |  of a line   |   X coo.   |   X coo.
    // ---------+--------+--------+--------------+------------+-----------
    // 6567R56A |   13   |   40   |  412 ($19c)  | 488 ($1e8) | 388 ($184)
    //  6567R8  |   13   |   40   |  412 ($19c)  | 489 ($1e9) | 396 ($18c)
    //   6569   |  300   |   15   |  404 ($194)  | 480 ($1e0) | 380 ($17c)

    // RSEL|  Display window height   | First line  | Last line
    // ----+--------------------------+-------------+----------
    //   0 | 24 text lines/192 pixels |   55 ($37)  | 246 ($f6)
    //   1 | 25 text lines/200 pixels |   51 ($33)  | 250 ($fa)
    // 
    // CSEL|   Display window width   | First X coo. | Last X coo.
    // ----+--------------------------+--------------+------------
    //   0 | 38 characters/304 pixels |   31 ($1f)   |  334 ($14e)
    //   1 | 40 characters/320 pixels |   24 ($18)   |  343 ($157)

    byte[]        Registers = new byte[64];
    int           RasterLinePos = 0;
    int           IRQRasterLinePos = 0;
    int           CycleInLinePos = 0;

    int           CyclesPerLine = 63;     // PAL
    int           NumberOfLines = 312;    // PAL

    int           MemoryBank = 3;

    int           TopBorder = 51;     // pixel;
    int           BottomBorder = 250; // pixel;
    int           LeftBorder  = 24;   // pixel;
    int           RightBorder = 343;  // pixel;

    bool          SideBorderActive = true;
    bool          TopBottomBorderActive = true;

    byte[]        VideoMatrixChars = new byte[40];
    byte[]        VideoMatrixColor = new byte[40];

    int           VideoCounter = 0;
    int           VideoCounterBase = 0;
    int           RowCounter = 0;
    int           VMLIRead = 0;
    int           VMLIWrite = 0;
    //int           XPos = 0;

    GraphicMode   CurrentMode = GraphicMode.CHAR_HIRES;
    bool          IdleState = false;

    int[]         MemoryBankBase = new int[4]{ 0xc000, 0x8000, 0x4000, 0x0000 };

    Machine       Machine = null;



    byte XScroll 
    { 
      get 
      { 
        return (byte)( Registers[(int)Register.CONTROL_2] & 0x07 ); 
      } 
    }



    byte YScroll
    {
      get
      {
        return (byte)( Registers[(int)Register.CONTROL_1] & 0x07 );
      }
    }



    bool RSEL
    {
      get
      {
        return ( Registers[(int)Register.CONTROL_1] & 0x08 ) != 0;
      }
    }



    bool BMM
    {
      get
      {
        return ( Registers[(int)Register.CONTROL_1] & 0x20 ) != 0;
      }
    }



    bool ECM
    {
      get
      {
        return ( Registers[(int)Register.CONTROL_1] & 0x40 ) != 0;
      }
    }



    bool CSEL
    {
      get
      {
        return ( Registers[(int)Register.CONTROL_2] & 0x08 ) != 0;
      }
    }



    bool MCM
    {
      get
      {
        return ( Registers[(int)Register.CONTROL_2] & 0x10 ) != 0;
      }
    }



    public bool IsIRQRaised
    {
      get
      {
        // TODO - raster, collision, etc.
        return false;
      }
    }



    public VIC( Machine Machine )
    {
      this.Machine = Machine;
    }



    public void Initialize()
    {
      Registers = new byte[64];

      for ( int i = 47; i < 64; ++i )
      {
        Registers[i] = 0xff;
      }

      RasterLinePos = 0;
      IRQRasterLinePos = 0;
      CycleInLinePos = 0;

      CyclesPerLine = 63;     // PAL
      NumberOfLines = 312;    // PAL

      TopBorder = 51;
      BottomBorder = 250;
      LeftBorder  = 24;
      RightBorder = 343;

      SideBorderActive = true;
      TopBottomBorderActive = true;

      VideoCounterBase  = 0;
      RowCounter        = 0;
      VideoCounter      = 0;
      VMLIRead          = 0;
      VMLIWrite         = 0;
      MemoryBank        = 3;
      IdleState         = true;
    }



    public ushort GetScreenAddress( ushort Address ) 
    {
      ushort    screenBaseAddress = (ushort)( ( Registers[(int)Register.MEMORY_CONTROL] & 0xf0 ) << 6 );
      
      return (ushort)( screenBaseAddress + ( Address & 0x3ff ) + MemoryBankBase[MemoryBank] ); 
    }
    
    
    
    public ushort GetCharsetAddress( ushort Address ) 
    {
      ushort  charDataAddress = (ushort)( ( Registers[(int)Register.MEMORY_CONTROL] & 0x0e ) << 10 );

      return (ushort)( charDataAddress + ( Address & 0x7ff ) + MemoryBankBase[MemoryBank] );
    }
    
    
    
    public ushort GetBitmapAddress( ushort Address ) 
    {
      ushort bitmapDataAddress = (ushort)( ( Registers[(int)Register.MEMORY_CONTROL] & 0x08 ) << 10 );

      return (ushort)( bitmapDataAddress + ( Address & 0x7ff ) + MemoryBankBase[MemoryBank] );
    }



    public void RunCycle( Memory Memory, Display Display )
    {
      // toggle border state
      if ( !TopBottomBorderActive )
      {
        // side border can only be toggled if the vertical border is not active

        // TODO - on real x pixel pos basis!
        int     cycleOffset = 13;
        if ( ( ( CycleInLinePos - cycleOffset ) * 8 < LeftBorder )
        &&   ( ( CycleInLinePos - cycleOffset ) * 8 + 8 >= LeftBorder ) )
        {
          SideBorderActive = false;
        }
      }

      if ( CycleInLinePos == 14 )
      {
        VideoCounter = VideoCounterBase;
        VMLIRead = 0;
        VMLIWrite = 0;

        if ( IsBadLine() )
        {
          RowCounter = 0;
          IdleState = false;
        }
      }

      if ( CycleInLinePos == 58 )
      {
        // 5.In the first phase of cycle 58, the VIC checks if RC = 7.If so, the video
        // logic goes to idle state and VCBASE is loaded from VC( VC->VCBASE ).If
        // the video logic is in display state afterwards( this is always the case
        // if there is a Bad Line Condition), RC is incremented.
        if ( RowCounter == 7 )
        {
          VideoCounterBase = VideoCounter;
          //Debug.Log( $"Raster {RasterLinePos} -> next video line {VideoCounterBase}" );
          IdleState = true;
        }
        if ( !IdleState )
        {
          RowCounter = ( RowCounter + 1 ) & 7;
        }
      }

      if ( ( CycleInLinePos >= 15 )
      &&   ( CycleInLinePos <= 54 ) )
      {
        // fetch graphic data
        if ( IsBadLine() )
        {
          ushort    readMemAddress      = GetScreenAddress( (ushort)VideoCounter );
          ushort    readColorMemAddress = (ushort)VideoCounter;
          CacheInVideoMatrix( Memory.ReadByteDirectAsVIC( readMemAddress ), Memory.ColorRAM[readColorMemAddress] );
        }
        if ( ( !IdleState )
        ||   ( IsBadLine() ) )
        {
          ++VideoCounter;
        }
      }

      UpdateNextPixelSlice( Display, Memory );
      ++CycleInLinePos;
      if ( CycleInLinePos == CyclesPerLine )
      {
        CycleInLinePos = 0;
        ++RasterLinePos;
        if ( RasterLinePos == NumberOfLines )
        {
          RasterLinePos = 0;
          Display.Flush();

          VideoCounterBase = 0;
        }
      }

      // toggle border state
      if ( !TopBottomBorderActive )
      {
        // side border can only be toggled if the vertical border is not active

        // TODO - on real x pixel pos basis!
        int     cycleOffset = 11;
        /*
        if ( ( ( CycleInLinePos - cycleOffset ) * 8 - 8 < LeftBorder )
        &&   ( ( CycleInLinePos - cycleOffset ) * 8 >= LeftBorder ) )
        {
          SideBorderActive = false;
        }*/
        if ( ( ( CycleInLinePos - cycleOffset ) * 8 - 2 * 8 < RightBorder )
        &&   ( ( CycleInLinePos - cycleOffset ) * 8 - 8 >= RightBorder ) )
        {
          SideBorderActive = true;
        }
      }
      if ( RasterLinePos == TopBorder )
      {
        TopBottomBorderActive = false;
      }
      if ( RasterLinePos == BottomBorder )
      {
        TopBottomBorderActive = true;
      }
    }



    private void CacheInVideoMatrix( byte CharData, byte ColorData )
    {
      VideoMatrixChars[VMLIWrite] = CharData;
      VideoMatrixColor[VMLIWrite] = ColorData;
      ++VMLIWrite;

      //Debug.Log( $"Cache char {CharData} at {VMLIWrite}" );
    }



    private void ReadVideoMatrix( out byte CharData, out byte ColorData )
    {
      if ( VMLIRead >= 40 )
      {
        //Debug.Log( "ReadVideoMatrix out of bounds!" );

        CharData = 0;
        ColorData = 16;
        return;
      }
      CharData  = VideoMatrixChars[VMLIRead];
      ColorData = VideoMatrixColor[VMLIRead];
      ++VMLIRead;

      //Debug.Log( $"Read char {CharData} at {VMLIRead}" );
    }



    private void UpdateNextPixelSlice( Display Display, Memory Memory )
    {
      // TODO - display disabled

      if ( ( SideBorderActive )
      ||   ( TopBottomBorderActive ) )
      {
        for ( int i = 0; i < 8; ++i )
        {
          Display.SetPixel( CycleInLinePos * 8 + i, RasterLinePos, Registers[(int)Register.BORDER_COLOR] );
        }
        return;
      }

      var graphicMode = DetermineGraphicMode();
      
      RenderGraphicModeSlice( Display, Memory, graphicMode );
    }



    private void RenderGraphicModeSlice( Display Display, Memory Memory, GraphicMode Mode )
    {
      // TODO !
      switch ( CurrentMode )
      {
        case GraphicMode.CHAR_HIRES:
          {
            byte  charData;
            byte  color;
            int   shift = 0;
            int   xscroll = 0;
            int   x = CycleInLinePos * 8 + xscroll;

            ReadVideoMatrix( out charData, out color );
            //Debug.Log( "Read cached byte " + VMLIRead + " in line " + RasterLinePos + "/cycle " + CycleInLinePos );

            byte bgColor  = Registers[(int)Register.BACKGROUND_COLOR];
            uint bits     = (uint)( Memory.ReadByteDirectAsVIC( GetCharsetAddress( (ushort)( ( charData * 8 ) + RowCounter ) ) ) << shift );

            uint pos = (uint)( RasterLinePos * 504 + x );
            for ( uint lim = pos + 8; pos < lim; pos++ )
            {
              bits <<= 1;
              uint pixel = bits & 0x100;

              Display.SetPixel( x + 8 - (int)( lim - pos ), RasterLinePos, pixel == 0 ? bgColor : color );
              //collision[pos] = (ushort)pixel;
            }
          }
          break;
        default:
          for ( int i = 0; i < 8; ++i )
          {
            Display.SetPixel( CycleInLinePos * 8 + i, RasterLinePos, Registers[(int)Register.BACKGROUND_COLOR] );
          }
          break;
      }
    }



    private GraphicMode DetermineGraphicMode()
    {
      byte    modeBits = 0;
      if ( MCM )
      {
        modeBits |= 1;
      }
      if ( BMM )
      {
        modeBits |= 2;
      }
      if ( ECM )
      {
        modeBits |= 4;
      }
      switch ( modeBits )
      {
        case 0:
        default:
          return GraphicMode.CHAR_HIRES;
        case 1:
          return GraphicMode.CHAR_MC;
        case 2:
          return GraphicMode.BITMAP_HIRES;
        case 3:
          return GraphicMode.BITMAP_MC;
        case 4:
          return GraphicMode.CHAR_ECM;
        case 5:
          return GraphicMode.UNDEFINED;
      }
    }



    public void WriteByte( byte Address, byte Value )
    {
      if ( Address >= 0x40 )
      {
        throw new NotImplementedException( "VIC only supports 0x40 registers!" );
      }

      if ( Address >= 47 )
      {
        // unused addresses can not be set
        return;
      }

      var register = (Register)Address;

      if ( register == Register.RASTER )
      {
        // raster pos for IRQ (lower 8 bits)
        IRQRasterLinePos = ( IRQRasterLinePos & 0x100 ) | Value;
      }
      else
      {
        Registers[Address] = Value;
        switch ( register )
        {
          case Register.CONTROL_1:
            // msb is msb of IRQ raster pos
            IRQRasterLinePos = ( IRQRasterLinePos & 0xff );
            if ( ( Value & 0x80 ) != 0 )
            {
              IRQRasterLinePos |= 0x100;
            }

            //       |   CSEL=0   |   CSEL=1
            // ------+------------+-----------
            // Left  |  31 ($1f)  |  24 ($18)
            // Right | 335 ($14f) | 344 ($158)
            //
            //        |   RSEL=0  |  RSEL=1
            // -------+-----------+----------
            // Top    |  55 ($37) |  51 ($33)
            // Bottom | 247 ($f7) | 251 ($fb)
            Registers[Address] = Value;
            if ( RSEL )
            {
              TopBorder     = 51;
              BottomBorder  = 251;
            }
            else
            {
              TopBorder     = 55;
              BottomBorder  = 247;
            }
            if ( CSEL )
            {
              LeftBorder  = 24;
              RightBorder = 344;
            }
            else
            {
              LeftBorder  = 31;
              RightBorder = 335;
            }
            CurrentMode = DetermineGraphicMode();
            break;
          case Register.CONTROL_2:
            CurrentMode = DetermineGraphicMode();
            break;
          case Register.MEMORY_CONTROL:
            break;
        }
        //Debug.Log( "VIC 0xd0" + Address.ToString( "X2" ) + "=" + Value.ToString( "X2" ) );
      }
    }



    public byte ReadByte( byte Address )
    {
      if ( Address >= 0x40 )
      {
        throw new NotImplementedException( "VIC only supports 0x40 registers!" );
      }
      return Registers[Address];
    }



    int RasterPos()
    {
      return RasterLinePos;
    }



    internal void SetRasterPos( int RasterPos )
    {
      RasterLinePos = RasterPos;

      Registers[(int)Register.RASTER]     = (byte)( RasterPos & 0xff );
      Registers[(int)Register.CONTROL_1]  = (byte)( ( Registers[(int)Register.CONTROL_1] & 0x7f ) + ( RasterPos >= 256 ? 0x80 : 0 ) );
    }



    public bool IsBadLine()
    {
      // bad line 

      // TODO - not if display is off!
      if ( ( RasterLinePos >= 0x30 )
      &&   ( RasterLinePos <= 0xf7 )
      &&   ( ( RasterLinePos & 0x07 ) == YScroll ) )
      {
        return true;
      }
      return false;
    }


  }
}
