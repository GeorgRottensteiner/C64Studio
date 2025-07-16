using GR.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;



namespace Tiny64
{
  public class Emulator
  {
    public Tiny64.Machine     Machine = new Tiny64.Machine();

    public EmulatorState      State = EmulatorState.RUNNING;

    public delegate void BreakpointHitHandler( Breakpoint breakPoint );

    public event BreakpointHitHandler   BreakpointHit;



    public Emulator()
    {
    }



    public void Reset()
    {
      Machine.HardReset();
    }



    public int RunCycles( int MaxCycles )
    {
      int cyclesUsed = MaxCycles;
      while ( MaxCycles >= 0 )
      {
        ushort    oldPC = Machine.CPU.PC;
        int curCycles = Machine.RunCycle();
        if ( Machine.TriggeredBreakpoints.Count > 0 )
        {
          State = EmulatorState.STOPPED;
          BreakpointHit( Machine.TriggeredBreakpoints[0] );
          Machine.TriggeredBreakpoints.Clear();
        }

        if ( ( State == EmulatorState.PAUSED )
        ||   ( State == EmulatorState.STOPPED ) )
        {
          return cyclesUsed;
        }

        MaxCycles -= curCycles;
      }
      cyclesUsed -= MaxCycles;
      return cyclesUsed;
    }



    public void Run()
    {
      do
      {
        Run( 16384 );
      }
      while ( State == EmulatorState.RUNNING );
    }



    public void Run( int MaxCycles )
    {
      // TODO - reset?
      State = EmulatorState.RUNNING;
      int   usedCycles = 0;
      while ( usedCycles < MaxCycles )
      {
        // round about one frame
        //int     numCycles = 19656;

        ushort    oldPC = Machine.CPU.PC;
        // TODO - single cycle steps?
        int curCycles = Machine.RunCycle();
        if ( Machine.TriggeredBreakpoints.Count > 0 )
        {
          //Debug.Log( "Breakpoint triggered at $" + Machine.CPU.PC.ToString( "X4" ) );
          var bpHit = Machine.TriggeredBreakpoints[0];
          State = EmulatorState.PAUSED;
          Machine.TriggeredBreakpoints.RemoveAll( bp => bp.Temporary );

          BreakpointHit( bpHit );
          return;
        }
        usedCycles += curCycles;

        //Debug.Log( machine.CPU.PC.ToString( "X4" ) + ":" + opCode.ToString( "X2" ) + " A:" + CPU.Accu.ToString( "X2" ) + " X:" + CPU.X.ToString( "X2" ) + " Y:" + CPU.Y.ToString( "X2" ) + " " + ( Memory.RAM[0xc1] + ( Memory.RAM[0xc2] << 8 ) ).ToString( "X4" ) );
        //Debug.Log( machine.CPU.PC.ToString( "X4" ) + ": A:" + machine.CPU.Accu.ToString( "X2" ) + " X:" + machine.CPU.X.ToString( "X2" ) + " Y:" + machine.CPU.Y.ToString( "X2" ) + " " + ( machine.Memory.RAM[0xc1] + ( machine.Memory.RAM[0xc2] << 8 ) ).ToString( "X4" ) );

        //RenderFullImage( Machine, img );
      }
    }



    private void RenderFullImage( Tiny64.Machine machine, GR.Image.MemoryImage img )
    {
      // render image
      bool  vicActive = ( ( machine.VIC.ReadByte( 0x11 ) & 0x10 ) != 0 );
      if ( vicActive )
      {
        int   vicBank = ( machine.CIA2.ReadByte( 0 ) & 0x03 ) ^ 0x03;
        int   screenPos = ( ( machine.VIC.ReadByte( 0x18 ) & 0xf0 ) >> 4 ) * 1024 + vicBank * 16384;
        int   localCharDataPos = ( machine.VIC.ReadByte( 0x18 ) & 0x0e ) * 1024;
        int   charDataPos = localCharDataPos + vicBank * 16384;
        byte  bgColor = (byte)( machine.VIC.ReadByte( 0x21 ) & 0x0f );

        GR.Memory.ByteBuffer    charData = null;
        if ( ( ( vicBank == 0 )
        ||     ( vicBank == 2 ) )
        &&   ( localCharDataPos == 0x1000 ) )
        {
          // use default upper case chars
          charData = new GR.Memory.ByteBuffer();
          charData.Append( Machine.Memory.CharacterROM, 0, 2048 );
        }
        else if ( ( ( vicBank == 0 )
        ||          ( vicBank == 2 ) )
        &&        ( localCharDataPos == 0x2000 ) )
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



    public int AddBreakpoint( ushort Address, bool Read, bool Write, bool Execute, bool temporary )
    {
      return Machine.AddBreakpoint( Address, Read, Write, Execute, temporary );
    }



    public void RemoveBreakpoint( int BreakpointIndex )
    {
      Machine.RemoveBreakpoint( BreakpointIndex );
    }



    public void StepOver()
    {
      var opCode = Machine.CPU.OpcodeByValue[Machine.Memory.ReadByteDirect( Machine.CPU.PC )];

      if ( IsJSRType( opCode ) )
      {
        Machine.AddTemporaryBreakpoint( (ushort)( Machine.CPU.PC + opCode.OpcodeSize + 1 ), false, false, true );
        Machine.SkipNextBreakpointCheck = true;
        State = EmulatorState.RUNNING;
      }
      else
      {
        StepInto();
      }
    }



    private bool IsJSRType( Opcode Opcode )
    {
      if ( Opcode.Mnemonic == "jsr" )
      {
        return true;
      }

      return false;
    }



    public void StepOut()
    {
      // fetch return address from stack
      ushort    returnAddress = Machine.Memory.RAM[0x100 + (byte)( Machine.CPU.StackPointer + 1 )];
      returnAddress |= (ushort)( Machine.Memory.RAM[0x100 + (byte)( Machine.CPU.StackPointer + 2 )] << 8 );

      Machine.AddTemporaryBreakpoint( returnAddress, false, false, true );
      Machine.SkipNextBreakpointCheck = true;
      State = EmulatorState.RUNNING;
    }



    public void StepInto()
    {
      State = EmulatorState.PAUSED;
      Machine.SkipNextBreakpointCheck = true;
      // TODO - should run one opcode, not one cycle (but currently does)
      Machine.RunCycle();
    }



    public void UpdateKeyState( PhysicalKey physicalKey, bool shifted, int c64_key, bool KeyDown )
    {
      if ( physicalKey != PhysicalKey.NONE )
      {
        int c64Byte = ( (int)physicalKey >> 3 ) & 7;
        int c64Bit = (int)physicalKey & 7;
        if ( !KeyDown )
        {
          if ( shifted )
          {
            Machine.CIA1.KeyMatrix[6] |= 0x10;
            Machine.CIA1.RevMatrix[4] |= 0x40;
          }
          Machine.CIA1.KeyMatrix[c64Byte] |= (byte)( 1 << c64Bit );
          Machine.CIA1.RevMatrix[c64Bit] |= (byte)( 1 << c64Byte );
        }
        else
        {
          if ( shifted )
          {
            Machine.CIA1.KeyMatrix[6] &= 0xef;
            Machine.CIA1.RevMatrix[4] &= 0xbf;
          }
          Machine.CIA1.KeyMatrix[c64Byte] &= (byte)~( 1 << c64Bit );
          Machine.CIA1.RevMatrix[c64Bit] &= (byte)~( 1 << c64Byte );
        }
      }

      if ( c64_key < 0 )
      {
        return;
      }

      // Handle joystick emulation
      if ( ( c64_key & 0x40 ) != 0 )
      {
        c64_key &= 0x1f;
        if ( !KeyDown )
        {
          Machine.CIA1.Joystick2 |= (byte)c64_key;
        }
        else
        {
          Machine.CIA1.Joystick2 &= (byte)~c64_key;
        }
        return;
      }

      /*
      // Handle other keys
      //bool shifted = (c64_key & 0x80) != 0;
      int c64_byte = (c64_key >> 3) & 7;
      int c64_bit = c64_key & 7;
      if ( !KeyDown )
      {
        if ( shifted )
        {
          m_Emulator.Machine.CIA1.KeyMatrix[6] |= 0x10;
          m_Emulator.Machine.CIA1.RevMatrix[4] |= 0x40;
        }
        m_Emulator.Machine.CIA1.KeyMatrix[c64_byte] |= (byte)( 1 << c64_bit );
        m_Emulator.Machine.CIA1.RevMatrix[c64_bit] |= (byte)( 1 << c64_byte );
      }
      else
      {
        if ( shifted )
        {
          m_Emulator.Machine.CIA1.KeyMatrix[6] &= 0xef;
          m_Emulator.Machine.CIA1.RevMatrix[4] &= 0xbf;
        }
        m_Emulator.Machine.CIA1.KeyMatrix[c64_byte] &= (byte)~( 1 << c64_bit );
        m_Emulator.Machine.CIA1.RevMatrix[c64_bit] &= (byte)~( 1 << c64_byte );
      }*/
    }



  }
}
