using GR.Memory;
using System;
using System.Collections.Generic;
using System.Text;



namespace Tiny64
{
  public class Machine
  {
    public enum IRQSource
    {
      VIC,
      CIA,
      NMI
    };




    public Memory     Memory;
    public Processor  CPU = Processor.Create6510();
    public VIC        VIC;
    public CIA1       CIA1;
    public CIA2       CIA2;
    public SID        SID = new SID();

    public Display    Display = new Display();

    public bool       SkipNextBreakpointCheck = false;

    //byte            IODirection = 0x2f;   // RAM 0000

    public int        TotalCycles = 0;

    bool              Game = true;
    bool              ExRom = true;
    bool              CharEn = true;  // Bit 2
    bool              HiRAM = true;   // Bit 1
    bool              LoRAM = true;   // Bit 0

    private bool      IRQCIARaised = false;
    private bool      IRQVICRaised = false;
    private bool      IRQNMIRaised = false;
    private bool      IRQResetRaised = false;

    private bool      FullDebug = false;
    //private bool      TempDebug = false;
    private bool      _ReadDebug = false;

    private bool      StartupCompleted = false;
    public ByteBuffer InjectFileAfterStartup = null;



    GR.Collections.MultiMap<ushort,Breakpoint>        Breakpoints = new GR.Collections.MultiMap<ushort, Breakpoint>();
    public List<Breakpoint>                           TriggeredBreakpoints = new List<Breakpoint>();



    public Machine()
    {
      Memory  = new Memory( this );
      VIC     = new VIC( this );
      CIA1    = new CIA1( this );
      CIA2    = new CIA2( this );

      HardReset();
    }



    public void RaiseNMI()
    {
      IRQNMIRaised = true;
    }



    public void LowerNMI()
    {
      IRQNMIRaised = false;
    }



    public void RaiseIRQ( IRQSource Source )
    {
      switch ( Source )
      {
        case IRQSource.CIA:
          IRQCIARaised = true;
          break;
        case IRQSource.VIC:
          IRQVICRaised = true;
          break;
        default:
          throw new Exception( "Unsupported IRQ source " + Source );
      }
    }



    public void LowerIRQ( IRQSource Source )
    {
      switch ( Source )
      {
        case IRQSource.CIA:
          if ( IRQCIARaised )
          {
            IRQCIARaised = false;
          }
          break;
        case IRQSource.VIC:
          if ( IRQVICRaised )
          {
            IRQVICRaised = true;
          }
          break;
        default:
          throw new Exception( "Unsupported IRQ source " + Source );
      }
    }



    public bool LORAMActive
    {
      get
      {
        return LoRAM;
      }
    }



    public bool HIRAMActive
    {
      get
      {
        return HiRAM;
      }
    }



    public bool CHARENActive
    {
      get
      {
        return CharEn;
      }
    }



    public bool IOVisible
    {
      get
      {
        if ( ( Game )
        &&   ( ExRom )
        &&   ( LORAMActive )
        &&   ( HIRAMActive )
        &&   ( CHARENActive ) )
        {
          return true;
        }
        if ( ( Game )
        &&   ( !LORAMActive )
        &&   ( HIRAMActive )
        &&   ( CHARENActive ) )
        {
          return true;
        }
        if ( ( Game )
        &&   ( LORAMActive )
        &&   ( !HIRAMActive )
        &&   ( CHARENActive ) )
        {
          return true;
        }
        if ( ( Game )
        &&   ( !ExRom )
        &&   ( LORAMActive )
        &&   ( HIRAMActive )
        &&   ( CHARENActive ) )
        {
          return true;
        }

        return false;
      }
    }



    public void HardReset()
    {
      SkipNextBreakpointCheck = false;

      IRQVICRaised = false;
      IRQCIARaised = false;
      IRQNMIRaised = false;
      IRQResetRaised = false;

      VIC.Initialize();
      SID.Init();
      CIA1.Init();
      CIA2.Init();

      Memory.Initialize();
      CPU.Initialize();

      //IODirection   = 0x2f;
      TotalCycles   = 6;

      CharEn  = true;  // Bit 2
      HiRAM   = true;   // Bit 1
      LoRAM   = true;   // Bit 0

      //Beim Hard-Reset startet die CPU bei der in $FFFC/$FFFD abgelegten Adresse (springt nach $FCE2, RESET)
      CPU.PC        = Memory.ReadWordDirect( 0xfffc );

      StartupCompleted = false;
    }



    public int RunCycles( int MaxCycles )
    {
      int cyclesUsed = MaxCycles;
      while ( MaxCycles >= 0 )
      {
        ushort    oldPC = CPU.PC;
        int curCycles = RunCycle();

        TotalCycles += curCycles;

        MaxCycles -= curCycles;

        // "update" VIC raster pos
        for ( int i = 0; i < curCycles; ++i )
        {
          VIC.RunCycle( Memory, Display );
        }
      }
      cyclesUsed -= MaxCycles;
      return cyclesUsed;
    }



    // returns numbers of cycles used
    public int RunCycle()
    {
      // cycle has two phases, usually first VIC, then CPU (if not stalled by VIC)
      CIA1.RunCycle();
      CIA2.RunCycle();
      VIC.RunCycle( Memory, Display );

      // TODO check for interrupts!
      if ( !CPU.FlagIRQ )
      {
        if ( IRQResetRaised )
        {
          HardReset();
        }
        if ( IRQNMIRaised )
        {
          //Debug.Log( "NMIRaised" );

          // push PC
          PushStack( (byte)( CPU.PC >> 8 ) );
          PushStack( (byte)( CPU.PC & 0xff ) );
          // push state
          PushStack( CPU.Flags );

          ushort  jumpAddress = (ushort)( Memory.ReadByteDirect( 0xfffa ) | ( Memory.ReadByteDirect( 0xfffb ) << 8 ) );

          CPU.PC = jumpAddress;
        }
        else if ( ( IRQCIARaised )
        ||        ( IRQVICRaised ) )
        {
          //Debug.Log( "IRQRaised" );
          //FullDebug = true;

          // push PC
          PushStack( (byte)( CPU.PC >> 8 ));
          PushStack( (byte)( CPU.PC & 0xff ) );
          // push state
          PushStack( CPU.Flags );

          // really, do it like that?
          CPU.FlagIRQ = true;

          ushort  jumpAddress = (ushort)( Memory.ReadByteDirect( 0xfffe ) | ( Memory.ReadByteDirect( 0xffff ) << 8 ) );

          //Debug.Log( "Enter IRQ at " + TotalCycles + " at address " + CPU.PC.ToString( "X4" ) + ", IRQ at " + jumpAddress.ToString( "X4" ) );

          /*
          for ( int i = 0x1f0; i <= 0x1ff; ++i )
          {
            Debug.Log( "Stack " + i.ToString( "X4" ) + ": " + Memory.ReadByteDirect( (ushort)i ).ToString( "X" ) );
          }*/

          CPU.PC = jumpAddress;
        }
      }

      // TODO - should run opcode in pieces!!

      int numCycles = RunOpcode();

      // catch up missed cycles 
      for ( int i = 1; i < numCycles; ++i )
      {
        CIA1.RunCycle();
        CIA2.RunCycle();
        VIC.RunCycle( Memory, Display );
      }

      // TODO - NOT clean!!

      TotalCycles += numCycles;

      return numCycles;
    }



    int RunOpcode()
    {
      // TODO - split opcodes for number of cycles!!
      byte    opCode = Memory.ReadByteDirect( CPU.PC );

      if ( FullDebug )
      {
        string display = Disassembler.DisassembleMnemonicToString( CPU.OpcodeByValue[opCode], Memory, CPU.PC, new GR.Collections.Set<ushort>(), new GR.Collections.Map<int, string>() );
        Debug.Log( $"{CPU.PC.ToString( "X4" )}: "
          + "A:" + CPU.Accu.ToString( "X2" ) + " X:" + CPU.X.ToString( "X2" ) + " Y:" + CPU.Y.ToString( "X2" ) 
          + " F: " + CPU.Flags.ToString ( "X2" )
          + $" -- {display}"
          );
      }

      //Debug.Log( $"Exec {CPU.PC.ToString( "X4" )} A:{CPU.Accu.ToString( "X2" )} X:{CPU.X.ToString( "X2" )} Y:{CPU.Y.ToString( "X2" )} P:{CPU.Flags.ToString( "X2" )}" );      

      if ( SkipNextBreakpointCheck )
      {
        SkipNextBreakpointCheck = false;
      }
      else
      {
        OnExecAddress( CPU.PC );
        if ( TriggeredBreakpoints.Count > 0 )
        {
          foreach ( var bp in TriggeredBreakpoints )
          {
            if ( bp.Temporary )
            {
              RemoveBreakpoint( bp );
            }
          }
          return 0;
        }
      }

      //Debug.Log( CPU.PC.ToString( "X4" ) + ":" + opCode.ToString( "X2" ) + " A:" + CPU.Accu.ToString( "X2" )  + " X:" + CPU.X.ToString( "X2" ) + " Y:" + CPU.Y.ToString( "X2" ) + " " + ( Memory.RAM[0xc1] + ( Memory.RAM[0xc2] << 8 ) ).ToString( "X4" ) );

      switch ( opCode )
      {
        case 0x00:
          // BRK        1b, 7c
          {
            OnReadAddress( (ushort)( CPU.PC + 1 ) );
            PushStack( (byte)( ( CPU.PC + 2 ) >> 8 ) );
            PushStack( (byte)( ( CPU.PC + 2 ) & 0xff ) );

            byte    flags = (byte)0x30;
            if ( CPU.FlagNegative )
            {
              flags |= 0x80;
            }
            if ( CPU.FlagOverflow )
            {
              flags |= 0x40;
            }
            if ( CPU.FlagDecimal )
            {
              flags |= 0x08;
            }
            if ( CPU.FlagIRQ )
            {
              flags |= 0x04;
            }
            if ( CPU.FlagZero )
            {
              flags |= 0x02;
            }
            if ( CPU.FlagCarry )
            {
              flags |= 0x01;
            }
            PushStack( flags );
            CPU.FlagIRQ = true;
            CPU.Flags |= 0x10;

            byte    lo = OnReadAddress( 0xfffe );
            byte    hi = OnReadAddress( 0xffff );

            CPU.PC = (ushort)( lo + ( hi << 8 ) );
            //FullDebug = true;
          }
          return 7;
        case 0x01:
          // ORA ($FF,X)    2b, 6c
          {
            ushort    zpBaseAddress = OnReadAddress( (ushort)( CPU.PC + 1 ) );

            ushort    addressLo = OnReadAddress( (ushort)( ( zpBaseAddress + CPU.X ) % 256 ) );
            ushort    addressHi = OnReadAddress( (ushort)( ( zpBaseAddress + CPU.X + 1 ) % 256 ) );
            ushort    finalAddress = (ushort)( addressLo + ( addressHi << 8 ) );

            byte operand = OnReadAddress( finalAddress );

            CPU.Accu = (byte)( CPU.Accu | operand );

            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();

            CPU.PC += 2;
          }
          return 6;
        case 0x02:
          // JAM        1b, ?c
          {
            throw new NotSupportedException( "JAM" );
          }
          // TODO?
          //return 0;
        case 0x05:
          // ORA $FF             2b, 3c
          {
            ushort    address = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            byte      operand = OnReadAddress( address );

            CPU.Accu = (byte)( CPU.Accu | operand );

            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();
            CPU.PC += 2;
          }
          return 3;
        case 0x06:
          // ASL $FF             2b, 5c
          {
            ushort    address = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            byte    operand = OnReadAddress( address );

            CPU.FlagCarry = ( ( operand & 0x80 ) != 0 );
            operand = (byte)( operand << 1 );

            OnWriteAddress( address, operand );

            CPU.FlagZero = ( operand == 0 );
            CPU.FlagNegative = ( ( operand & 0x80 ) != 0 );

            CPU.PC += 2;
          }
          return 5;
        case 0x08:
          // PHP                 1b, 3c
          {
            // all flags, break and unused bit is always set
            byte    flags = (byte)0x30;
            if ( CPU.FlagNegative )
            {
              flags |= 0x80;
            }
            if ( CPU.FlagOverflow )
            {
              flags |= 0x40;
            }
            if ( CPU.FlagDecimal )
            {
              flags |= 0x08;
            }
            if ( CPU.FlagIRQ )
            {
              flags |= 0x04;
            }
            if ( CPU.FlagZero )
            {
              flags |= 0x02;
            }
            if ( CPU.FlagCarry )
            {
              flags |= 0x01;
            }

            //Debug.Log( CPU.PC.ToString( "X4" ) + ": PHP " + flags.ToString( "X2" ) );

            PushStack( flags );

            CPU.PC += 1;
          }
          return 3;
        case 0x09:
          // ORA #$FF   2b, 2c
          {
            byte    operand = OnReadAddress( (ushort)( CPU.PC + 1 ) );

            CPU.Accu = (byte)( CPU.Accu | operand );

            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();
            CPU.PC += 2;
          }
          return 2;
        case 0x0a:
          // ASL A                1b, 2c
          {
            CPU.FlagCarry = ( ( CPU.Accu & 0x80 ) != 0 );
            CPU.Accu = (byte)( CPU.Accu << 1 );

            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();

            CPU.PC += 1;
          }
          return 2;
        case 0x0d:
          // ORA $FFFF           3b, 4c
          {
            ushort    address = OnReadWord( (ushort)( CPU.PC + 1 ) );

            byte    operand = OnReadAddress( address );

            CPU.Accu = (byte)( CPU.Accu | operand );

            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();
            CPU.PC += 3;
          }
          return 4;
        case 0x0e:
          // ASL $FFFF           3b, 6c
          {
            ushort    address = OnReadWord( (ushort)( CPU.PC + 1 ) );
            byte    operand = OnReadAddress( address );

            CPU.FlagCarry = ( ( operand & 0x80 ) != 0 );
            operand = (byte)( operand << 1 );

            OnWriteAddress( address, operand );

            CPU.FlagZero = ( operand == 0 );
            CPU.FlagNegative = ( ( operand & 0x80 ) != 0 );

            CPU.PC += 3;
          }
          return 6;
        case 0x10:
          // BPL $FFFF           2b, 2*c
          return HandleBranch( !CPU.FlagNegative );
        case 0x11:
          // ORA ($FF),Y    2b, 5*c
          {
            int numCycles = 5;

            ushort    address = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            ushort    finalAddress = CalcIndirectY( address, CPU.Y );

            if ( ( address & 0xff00 ) != ( finalAddress & 0xff00 ) )
            {
              ++numCycles;
            }

            byte operand = OnReadAddress( finalAddress );

            CPU.Accu = (byte)( CPU.Accu | operand );

            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();

            CPU.PC += 2;

            return numCycles;
          }
        case 0x15:
          // ORA $FF,X           2b, 4c
          {
            ushort    address = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            address = CalcZeropageX( address, CPU.X );
            byte      operand = OnReadAddress( address );

            CPU.Accu = (byte)( CPU.Accu | operand );

            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();
            CPU.PC += 2;
          }
          return 4;
        case 0x16:
          // ASL $FF,X           2b, 6c
          {
            ushort    address = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            ushort    finalAddress = CalcZeropageX( address, CPU.X );
            byte      operand = OnReadAddress( finalAddress );

            CPU.FlagCarry = ( ( operand & 0x80 ) != 0 );
            operand = (byte)( operand << 1 );

            OnWriteAddress( finalAddress, operand );

            CPU.FlagZero = ( operand == 0 );
            CPU.FlagNegative = ( ( operand & 0x80 ) != 0 );

            CPU.PC += 2;
          }
          return 6;
        case 0x18:
          // CLC        1b, 2c
          {
            CPU.FlagCarry = false;

            CPU.PC += 1;
          }
          return 2;
        case 0x19:
          // ORA $FFFF,Y           3b, 4*c
          {
            int       numCycles = 4;
            ushort    address = OnReadWord( (ushort)( CPU.PC + 1 ) );
            ushort    finalAddress = CalcAbsoluteY( address, CPU.Y );

            if ( ( address & 0xff00 ) != ( finalAddress & 0xff00 ) )
            {
              ++numCycles;
            }

            byte    operand = OnReadAddress( finalAddress );

            CPU.Accu = (byte)( CPU.Accu | operand );

            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();
            CPU.PC += 3;

            return numCycles;
          }
        case 0x1d:
          // ORA $FFFF,X           3b, 4*c
          {
            int       numCycles = 4;
            ushort    address = OnReadWord( (ushort)( CPU.PC + 1 ) );
            ushort    finalAddress = CalcAbsoluteX( address, CPU.X );

            if ( ( address & 0xff00 ) != ( finalAddress & 0xff00 ) )
            {
              ++numCycles;
            }

            byte    operand = OnReadAddress( finalAddress );

            CPU.Accu = (byte)( CPU.Accu | operand );

            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();
            CPU.PC += 3;

            return numCycles;
          }
        case 0x1e:
          // ASL $FFFF,X         3b, 7c
          {
            ushort    address = OnReadWord( (ushort)( CPU.PC + 1 ) );
            ushort    finalAddress = CalcAbsoluteX( address, CPU.X );
            byte      operand = OnReadAddress( finalAddress );

            CPU.FlagCarry = ( ( operand & 0x80 ) != 0 );
            operand = (byte)( operand << 1 );

            OnWriteAddress( finalAddress, operand );

            CPU.FlagZero = ( operand == 0 );
            CPU.FlagNegative = ( ( operand & 0x80 ) != 0 );

            CPU.PC += 3;
          }
          return 7;
        case 0x20:
          // JSR        3 bytes, 6 cycles
          {
            ushort    address = OnReadWord( (ushort)( CPU.PC + 1 ) );

            ushort    returnAddress = (ushort)( CPU.PC + 2 );

            PushStack( (byte)( ( returnAddress >> 8 ) & 0xff ) );
            PushStack( (byte)( returnAddress & 0xff ) );

            CPU.PC = address;

            //Debug.Log( "JSR $" + CPU.PC.ToString( "X4" ) );
          }
          return 6;
        case 0x21:
          // AND ($FF,X)    2b, 6c
          {
            ushort    zpBaseAddress = OnReadAddress( (ushort)( CPU.PC + 1 ) );

            ushort    addressLo = OnReadAddress( (ushort)( ( zpBaseAddress + CPU.X ) % 256 ) );
            ushort    addressHi = OnReadAddress( (ushort)( ( zpBaseAddress + CPU.X + 1 ) % 256 ) );
            ushort    finalAddress = (ushort)( addressLo + ( addressHi << 8 ) );

            byte operand = OnReadAddress( finalAddress );

            CPU.Accu = (byte)( CPU.Accu & operand );

            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();

            CPU.PC += 2;
          }
          return 6;
        case 0x24:
          // BIT $FF             2b, 3c
          {
            ushort    address = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            byte      result = OnReadAddress( address );

            CPU.FlagNegative  = ( ( result & 0x80 ) != 0 );
            CPU.FlagOverflow  = ( ( result & 0x40 ) != 0 );
            CPU.FlagZero      = ( ( result & CPU.Accu ) == 0 );
            CPU.PC += 2;
          }
          return 3;
        case 0x25:
          // AND $FF            2b, 3c
          {
            byte      address = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            byte      operand = OnReadAddress( address );

            CPU.Accu = (byte)( CPU.Accu & operand );

            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();
            CPU.PC += 2;
          }
          return 3;
        case 0x26:
          // ROL $ff                2b, 5c
          {
            ushort    address = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            byte      operand = OnReadAddress( address );

            uint     newValue = (uint)( operand * 2 );
            if ( CPU.FlagCarry )
            {
              newValue |= 0x01;
            }
            CPU.FlagCarry = ( ( newValue & 0x100 ) != 0 );

            byte result = (Byte)newValue;

            OnWriteAddress( address, result );

            CPU.FlagZero = ( result == 0 );
            CPU.FlagNegative = ( ( result & 0x80 ) != 0 );

            CPU.PC += 2;
          }
          return 5;
        case 0x28:
          // PLP                 1b, 4c
          {
            byte    flags = PopStack();

            if ( FullDebug )
            {
              Debug.Log( CPU.PC.ToString( "X4") + ": PLP " + flags.ToString( "X2" ) );
            }

            CPU.FlagNegative  = ( ( flags & 0x80 ) != 0 );
            CPU.FlagOverflow  = ( ( flags & 0x40 ) != 0 );
            CPU.FlagDecimal   = ( ( flags & 0x08 ) != 0 );
            CPU.FlagIRQ       = ( ( flags & 0x04 ) != 0 );
            CPU.FlagZero      = ( ( flags & 0x02 ) != 0 );
            CPU.FlagCarry     = ( ( flags & 0x01 ) != 0 );

            CPU.PC += 1;
          }
          return 4;
        case 0x29:
          // AND #$FF            2b, 2c
          {
            byte    operand = OnReadAddress( (ushort)( CPU.PC + 1 ) );

            CPU.Accu = (byte)( CPU.Accu & operand );

            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();
            CPU.PC += 2;
          }
          return 2;
        case 0x2a:
          // ROL A                1b, 2c
          {
            uint     newValue = (uint)( CPU.Accu * 2 );
            if ( CPU.FlagCarry )
            {
              newValue |= 0x01;
            }
            CPU.FlagCarry = ( ( newValue & 0x100 ) != 0 );
            CPU.Accu = (byte)newValue;

            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();

            CPU.PC += 1;
          }
          return 2;
        case 0x2c:
          // BIT $FFFF           3b, 4c
          {
            ushort    address = OnReadWord( (ushort)( CPU.PC + 1 ) );
            byte      operand = OnReadAddress( address );

            byte      result = (byte)( operand & CPU.Accu );

            CPU.FlagNegative = ( ( operand & 0x80 ) != 0 );
            CPU.FlagOverflow = ( ( operand & 0x40 ) != 0 );
            CPU.FlagZero = ( result == 0 );

            CPU.PC += 3;
          }
          return 4;
        case 0x2d:
          // AND $FFFF          3b, 4c
          {
            ushort address = OnReadWord( (ushort)( CPU.PC + 1 ) );
            byte      operand = OnReadAddress( address );

            CPU.Accu = (byte)( CPU.Accu & operand );

            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();
            CPU.PC += 3;
          }
          return 4;
        case 0x2e:
          // ROL $ffff                3b, 6c
          {
            ushort    address = OnReadWord( (ushort)( CPU.PC + 1 ) );
            byte      operand = OnReadAddress( address );

            uint     newValue = (uint)( operand * 2 );
            if ( CPU.FlagCarry )
            {
              newValue |= 0x01;
            }
            CPU.FlagCarry = ( ( newValue & 0x100 ) != 0 );

            byte result = (Byte)newValue;

            OnWriteAddress( address, result );

            CPU.FlagZero = ( result == 0 );
            CPU.FlagNegative = ( ( result & 0x80 ) != 0 );

            CPU.PC += 3;
          }
          return 6;
        /*
      case 0x2f:
        // RLA ROL memory, AND memory       3b, 6c
        //  A <- (M << 1) /\ (A)
        {
          int     newValue = CPU.Accu * 2;
          if ( CPU.FlagCarry )
          {
            newValue |= 0x01;
          }
          CPU.FlagCarry = ( ( newValue & 0x100 ) != 0 );
          CPU.Accu = (byte)newValue;

          CPU.CheckFlagZero();
          CPU.CheckFlagNegative();

          CPU.PC += 1;
        }
        return 2;*/
        case 0x30:
          // BMI $FFFF           2b, 2*c
          return HandleBranch( CPU.FlagNegative );
        case 0x31:
          // AND ($FF),Y    2b, 5*c
          {
            int numCycles = 5;

            ushort    address = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            ushort    finalAddress = CalcIndirectY( address, CPU.Y );

            if ( ( address & 0xff00 ) != ( finalAddress & 0xff00 ) )
            {
              ++numCycles;
            }

            byte operand = OnReadAddress( finalAddress );

            CPU.Accu = (byte)( CPU.Accu & operand );

            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();

            CPU.PC += 2;

            return numCycles;
          }
        case 0x35:
          // AND $FF,X          2b, 4c
          {
            ushort address = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            address = CalcZeropageX( address, CPU.X );
            byte      operand = OnReadAddress( address );

            CPU.Accu = (byte)( CPU.Accu & operand );

            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();
            CPU.PC += 2;
          }
          return 4;
        case 0x36:
          // ROL $ff,X              2b, 6c
          {
            ushort    address = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            ushort    finalAddress = CalcZeropageX( address, CPU.X );
            byte      operand = OnReadAddress( finalAddress );

            uint     newValue = (uint)( operand * 2 );
            if ( CPU.FlagCarry )
            {
              newValue |= 0x01;
            }
            CPU.FlagCarry = ( ( newValue & 0x100 ) != 0 );

            byte result = (Byte)newValue;

            OnWriteAddress( finalAddress, result );

            CPU.FlagZero = ( result == 0 );
            CPU.FlagNegative = ( ( result & 0x80 ) != 0 );

            CPU.PC += 2;
          }
          return 6;
        case 0x38:
          // SEC                1b, 2c
          {
            CPU.FlagCarry = true;
            CPU.PC += 1;
          }
          return 2;
        case 0x39:
          // AND $FFFF,Y          3b, 4*c
          {
            int       numCycles = 4;
            ushort    address = OnReadWord( (ushort)( CPU.PC + 1 ) );
            ushort    finalAddress = CalcAbsoluteY( address, CPU.Y );

            if ( ( address & 0xff00 ) != ( finalAddress & 0xff00 ) )
            {
              ++numCycles;
            }

            byte      operand = OnReadAddress( finalAddress );

            CPU.Accu = (byte)( CPU.Accu & operand );

            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();
            CPU.PC += 3;

            return numCycles;
          }
        case 0x3d:
          // AND $FFFF,X          3b, 4*c
          {
            int       numCycles = 4;
            ushort    address = OnReadWord( (ushort)( CPU.PC + 1 ) );
            ushort    finalAddress = CalcAbsoluteX( address, CPU.X );

            if ( ( address & 0xff00 ) != ( finalAddress & 0xff00 ) )
            {
              ++numCycles;
            }

            byte      operand = OnReadAddress( finalAddress );

            CPU.Accu = (byte)( CPU.Accu & operand );

            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();
            CPU.PC += 3;

            return numCycles;
          }
        case 0x3e:
          // ROL $ffFF,X              3b, 7c
          {
            ushort    address = OnReadWord( (ushort)( CPU.PC + 1 ) );
            ushort    finalAddress = CalcAbsoluteX( address, CPU.X );
            byte      operand = OnReadAddress( finalAddress );

            uint     newValue = (uint)( operand * 2 );
            if ( CPU.FlagCarry )
            {
              newValue |= 0x01;
            }
            CPU.FlagCarry = ( ( newValue & 0x100 ) != 0 );

            byte result = (Byte)newValue;

            OnWriteAddress( finalAddress, result );

            CPU.FlagZero = ( result == 0 );
            CPU.FlagNegative = ( ( result & 0x80 ) != 0 );

            CPU.PC += 3;
          }
          return 7;
        case 0x40:
          // RTI                 1b, 6c
          {
            CPU.Flags = PopStack();

            byte lo = PopStack();
            byte hi = PopStack();

            ushort  address = (ushort)( lo + ( hi << 8 ) );

            CPU.PC = address;

            if ( FullDebug )
            {
              Debug.Log( "Leave IRQ, return to " + address.ToString( "X" ) );
            }
          }
          return 6;
        case 0x41:
          // EOR ($FF,X)    2b, 6c
          {
            ushort    zpBaseAddress = OnReadAddress( (ushort)( CPU.PC + 1 ) );

            ushort    addressLo = OnReadAddress( (ushort)( ( zpBaseAddress + CPU.X ) % 256 ) );
            ushort    addressHi = OnReadAddress( (ushort)( ( zpBaseAddress + CPU.X + 1 ) % 256 ) );
            ushort    finalAddress = (ushort)( addressLo + ( addressHi << 8 ) );

            byte operand = OnReadAddress( finalAddress );

            CPU.Accu = (byte)( CPU.Accu ^ operand );

            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();

            CPU.PC += 2;
          }
          return 6;
        case 0x45:
          // EOR $FF             2b, 3c
          {
            ushort    address = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            byte    operand = OnReadAddress( address );

            CPU.Accu = (byte)( CPU.Accu ^ operand );

            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();
            CPU.PC += 2;
          }
          return 3;
        case 0x46:
          // LSR $FF             2b, 5c
          {
            ushort    address = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            byte    operand = OnReadAddress( address );

            CPU.FlagCarry = ( ( operand & 0x01 ) != 0 );

            byte  result = (byte)( operand >> 1 );
            OnWriteAddress( address, result );

            CPU.FlagZero = ( result == 0 );
            CPU.FlagNegative = ( ( result & 0x80 ) != 0 );
            CPU.PC += 2;
          }
          return 5;
        case 0x48:
          // PHA              1b, 3c
          {
            PushStack( CPU.Accu );

            CPU.PC += 1;
          }
          return 3;
        case 0x49:
          // EOR #$FF            2b, 2c
          {
            byte operand = OnReadAddress( (ushort)( CPU.PC + 1 ) );

            CPU.Accu = (byte)( CPU.Accu ^ operand );

            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();
            CPU.PC += 2;
          }
          return 2;
        case 0x4a:
          // LSR A               1b, 2c
          {
            CPU.FlagCarry = ( ( CPU.Accu & 0x01 ) != 0 );
            CPU.Accu = (byte)( CPU.Accu >> 1 );

            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();

            CPU.PC += 1;
          }
          return 2;
        case 0x4c:
          // JMP $FFFF           3b, 3c
          {
            ushort    address = OnReadWord( (ushort)( CPU.PC + 1 ) );

            CPU.PC = address;

            //Debug.Log( "JMP $" + CPU.PC.ToString( "X4" ) );
          }
          return 3;
        case 0x4d:
          // EOR $FFFF           3b, 4c
          {
            ushort    address = OnReadWord( (ushort)( CPU.PC + 1 ) );
            byte    operand = OnReadAddress( address );

            CPU.Accu = (byte)( CPU.Accu ^ operand );

            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();
            CPU.PC += 3;
          }
          return 4;
        case 0x4e:
          // LSR $FFFF           3b, 6c
          {
            ushort    address = OnReadWord( (ushort)( CPU.PC + 1 ) );
            byte    operand = OnReadAddress( address );

            CPU.FlagCarry = ( ( operand & 0x01 ) != 0 );

            byte  result = (byte)( operand >> 1 );
            OnWriteAddress( address, result );

            CPU.FlagZero = ( result == 0 );
            CPU.FlagNegative = ( ( result & 0x80 ) != 0 );
            CPU.PC += 3;
          }
          return 6;
        case 0x50:
          // BVC $FFFF           2b, 2*c
          return HandleBranch( !CPU.FlagOverflow );
        case 0x51:
          // EOR ($FF),Y    2b, 5*c
          {
            int numCycles = 5;

            ushort    address = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            ushort    finalAddress = CalcIndirectY( address, CPU.Y );

            if ( ( address & 0xff00 ) != ( finalAddress & 0xff00 ) )
            {
              ++numCycles;
            }

            byte operand = OnReadAddress( finalAddress );

            CPU.Accu = (byte)( CPU.Accu ^ operand );

            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();

            CPU.PC += 2;

            return numCycles;
          }
        case 0x55:
          // EOR $FF,X           2b, 4c
          {
            ushort    address = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            ushort    finalAddress = CalcZeropageX( address, CPU.X );
            byte    operand = OnReadAddress( finalAddress );

            CPU.Accu = (byte)( CPU.Accu ^ operand );

            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();
            CPU.PC += 2;
          }
          return 4;
        case 0x56:
          // LSR $FF,X           2b, 6c
          {
            ushort    address = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            ushort    finalAddress = CalcZeropageX( address, CPU.X );
            byte    operand = OnReadAddress( finalAddress );

            CPU.FlagCarry = ( ( operand & 0x01 ) != 0 );

            operand = (byte)( operand >> 1 );

            OnWriteAddress( finalAddress, operand );

            CPU.FlagZero = ( operand == 0 );
            CPU.FlagNegative = ( ( operand & 0x80 ) != 0 );
            CPU.PC += 2;
          }
          return 6;
        case 0x58:
          // CLI        1b, 2c
          {
            CPU.FlagIRQ = false;

            CPU.PC += 1;
          }
          return 2;
        case 0x59:
          // EOR $FFFF,Y           3b, 4*c
          {
            int numCycles = 4;

            ushort    address = OnReadWord( (ushort)( CPU.PC + 1 ) );
            ushort    finalAddress = CalcAbsoluteY( address, CPU.Y );
            if ( ( address & 0xff00 ) != ( finalAddress & 0xff00 ) )
            {
              ++numCycles;
            }
            byte    operand = OnReadAddress( finalAddress );

            CPU.Accu = (byte)( CPU.Accu ^ operand );

            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();
            CPU.PC += 3;

            return numCycles;
          }
        case 0x5d:
          // EOR $FFFF,X           3b, 4*c
          {
            int numCycles = 4;

            ushort    address = OnReadWord( (ushort)( CPU.PC + 1 ) );
            ushort    finalAddress = CalcAbsoluteX( address, CPU.X );
            if ( ( address & 0xff00 ) != ( finalAddress & 0xff00 ) )
            {
              ++numCycles;
            }
            byte    operand = OnReadAddress( finalAddress );

            CPU.Accu = (byte)( CPU.Accu ^ operand );

            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();
            CPU.PC += 3;

            return numCycles;
          }
        case 0x5e:
          // LSR $FFFF,X           3b, 7c
          {
            ushort    address = OnReadWord( (ushort)( CPU.PC + 1 ) );
            ushort    finalAddress = CalcAbsoluteX( address, CPU.X );
            byte    operand = OnReadAddress( finalAddress );

            CPU.FlagCarry = ( ( operand & 0x01 ) != 0 );

            operand = (byte)( operand >> 1 );

            OnWriteAddress( finalAddress, operand );

            CPU.FlagZero = ( operand == 0 );
            CPU.FlagNegative = ( ( operand & 0x80 ) != 0 );
            CPU.PC += 3;
          }
          return 7;
        case 0x60:
          // RTS        1 byte, 6 cycles
          {
            /*
            if ( TempDebug )
            {
              Debug.Log( $"Run RTS {opCode.ToString( "X2" )}/{CPU.OpcodeByValue[opCode].Mnemonic} at {CPU.PC.ToString( "X4" )} "
                + "A: " + CPU.Accu.ToString( "X2" ) + " X: " + CPU.X.ToString( "X2" ) + " Y: " + CPU.Y.ToString( "X2" )
                + " F: " + CPU.Flags.ToString( "X2" )

                + " $61-66:" + Memory.RAM[0x61].ToString( "X2" )
                             + Memory.RAM[0x62].ToString( "X2" )
                             + Memory.RAM[0x63].ToString( "X2" )
                             + Memory.RAM[0x64].ToString( "X2" )
                             + Memory.RAM[0x65].ToString( "X2" )
                             + Memory.RAM[0x66].ToString( "X2" )
                             );
            }*/

            // PC from Stack, PC + 1 -> PC               
            ushort    returnAddress = PopStack();
            returnAddress |= (ushort)( PopStack() << 8 );
            
            //Debug.Log( "RTS from " + CPU.PC.ToString( "X4" ) + " to " + ( returnAddress + 1 ).ToString( "X4" ) );

            //TODO - $e421 has broken return address!  (0x0199)
            CPU.PC = (ushort)( returnAddress + 1 );
          }
          return 6;
        case 0x61:
          // ADC ($FF,X)             2b, 6c
          {
            ushort    zpBaseAddress = OnReadAddress( (ushort)( CPU.PC + 1 ) );

            ushort    addressLo = OnReadAddress( (ushort)( ( zpBaseAddress + CPU.X ) % 256 ) );
            ushort    addressHi = OnReadAddress( (ushort)( ( zpBaseAddress + CPU.X + 1 ) % 256 ) );
            ushort    finalAddress = (ushort)( addressLo + ( addressHi << 8 ) );

            byte    operand = OnReadAddress( finalAddress );
            HandleADC( operand );

            CPU.PC += 2;
          }
          return 6;
        case 0x65:
          // ADC $FF             2b, 3c
          {
            ushort  address = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            byte    operand = OnReadAddress( address );
            HandleADC( operand );

            CPU.PC += 2;
          }
          return 3;
        case 0x66:
          // ROR $FF             2b, 5c
          {
            ushort    address = OnReadAddress( (ushort)( CPU.PC + 1 ) );

            byte      operand = OnReadAddress( address );
            int       temp = operand & 0x01;
            operand = (byte)( operand >> 1 );
            if ( CPU.FlagCarry )
            {
              operand = (byte)( operand | 0x80 );
            }

            OnWriteAddress( address, operand );

            CPU.FlagCarry = ( temp == 1 );

            CPU.FlagZero = ( operand == 0 );
            CPU.FlagNegative = ( ( operand & 0x80 ) != 0 );

            CPU.PC += 2;
          }
          return 5;
        case 0x68:
          // PLA                 1b, 4c
          {
            CPU.Accu = PopStack();

            CPU.CheckFlagNegative();
            CPU.CheckFlagZero();

            CPU.PC += 1;
          }
          return 4;
        case 0x69:
          // ADC #$FF            2b, 2c
          {
            byte    operand = OnReadAddress( (ushort)( CPU.PC + 1 ) );

            HandleADC( operand );

            CPU.PC += 2;
          }
          return 2;
        case 0x6a:
          // ROR A               1b, 2c
          {
            byte      operand = CPU.Accu;
            int       temp = operand & 0x01;
            operand = (byte)( operand >> 1 );
            if ( CPU.FlagCarry )
            {
              operand = (byte)( operand | 0x80 );
            }
            CPU.FlagCarry = ( temp == 1 );

            CPU.Accu = operand;

            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();

            CPU.PC += 1;
          }
          return 2;
        case 0x6c:
          // JMP ($FFFF)      3b, 5c
          {
            ushort    address = OnReadWord( (ushort)( CPU.PC + 1 ) );

            ushort    finalAddress = OnReadAddress( address );
            finalAddress |= (ushort)( OnReadAddress( (ushort)( ( address & 0xff00 ) | ( ( ( address & 0xff ) + 1 ) % 256 ) ) ) << 8 );

            CPU.PC = finalAddress;

            //Debug.Log( "JMP indirect to $" + CPU.PC.ToString( "X4" ) );
          }
          return 5;
        case 0x6D:
          // ADC $FFFF             3b, 4c
          {
            ushort  address = OnReadWord( (ushort)( CPU.PC + 1 ) );
            byte    operand = OnReadAddress( address );
            HandleADC( operand );

            CPU.PC += 3;
          }
          return 4;
        case 0x6e:
          // ROR $FFFF           3b, 6c
          {
            ushort    address = OnReadWord( (ushort)( CPU.PC + 1 ) );
            byte      operand = OnReadAddress( address );
            int       temp = operand & 0x01;
            operand = (byte)( operand >> 1 );
            if ( CPU.FlagCarry )
            {
              operand = (byte)( operand | 0x80 );
            }

            OnWriteAddress( address, operand );

            CPU.FlagCarry = ( temp == 1 );

            CPU.FlagZero = ( operand == 0 );
            CPU.FlagNegative = ( ( operand & 0x80 ) != 0 );

            CPU.PC += 3;
          }
          return 6;
        case 0x70:
          // BVS $FFFF           2b, 2*c
          return HandleBranch( CPU.FlagOverflow );
        case 0x71:
          // ADC ($FF),Y             2b, 5*c
          {
            int numCycles = 5;

            ushort    address = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            ushort    finalAddress = CalcIndirectY( address, CPU.Y );

            if ( ( address & 0xff00 ) != ( finalAddress & 0xff00 ) )
            {
              ++numCycles;
            }

            byte    operand = OnReadAddress( finalAddress );
            HandleADC( operand );

            CPU.PC += 2;

            return numCycles;
          }
        case 0x75:
          // ADC $FF,X             2b, 4c
          {
            ushort  address = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            ushort  finalAddress = CalcZeropageX( address, CPU.X );
               
            byte    operand = OnReadAddress( finalAddress );
            HandleADC( operand );

            CPU.PC += 2;
          }
          return 4;
        case 0x76:
          // ROR $FF,X           2b, 6c
          {
            ushort    address = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            ushort    finalAddress = CalcZeropageX( address, CPU.X );

            byte      operand = OnReadAddress( finalAddress );
            int       temp = operand & 0x01;
            operand = (byte)( operand >> 1 );
            if ( CPU.FlagCarry )
            {
              operand = (byte)( operand | 0x80 );
            }

            OnWriteAddress( finalAddress, operand );

            CPU.FlagCarry = ( temp == 1 );

            CPU.FlagZero = ( operand == 0 );
            CPU.FlagNegative = ( ( operand & 0x80 ) != 0 );

            CPU.PC += 2;
          }
          return 6;
        case 0x78:
          // SEI        1 byte, 2 cycles
          {
            CPU.FlagIRQ = true;
            CPU.PC += 1;
          }
          return 2;
        case 0x79:
          // ADC $FFFF,Y         3b, 4*c
          {
            int numCycles = 4;

            ushort    address = OnReadWord( (ushort)( CPU.PC + 1 ) );
            ushort    finalAddress = CalcAbsoluteY( address, CPU.Y );

            if ( ( address & 0xff00 ) != ( finalAddress & 0xff00 ) )
            {
              ++numCycles;
            }

            byte operand = OnReadAddress( finalAddress );
            HandleADC( operand );

            CPU.PC += 3;

            return numCycles;
          }
        case 0x7D:
          // ADC $FFFF,X             3b, 4*c
          {
            int numCycles = 4;

            ushort    address = OnReadWord( (ushort)( CPU.PC + 1 ) );
            ushort    finalAddress = CalcAbsoluteX( address, CPU.X );

            if ( ( address & 0xff00 ) != ( finalAddress & 0xff00 ) )
            {
              ++numCycles;
            }

            byte    operand = OnReadAddress( finalAddress );
            HandleADC( operand );

            CPU.PC += 3;

            return numCycles;
          }
        case 0x7e:
          // ROR $FFFF,X           3b, 7c
          {
            ushort    address = OnReadWord( (ushort)( CPU.PC + 1 ) );
            ushort    finalAddress = CalcAbsoluteX( address, CPU.X );

            byte      operand = OnReadAddress( finalAddress );
            int       temp = operand & 0x01;
            operand = (byte)( operand >> 1 );
            if ( CPU.FlagCarry )
            {
              operand = (byte)( operand | 0x80 );
            }

            OnWriteAddress( finalAddress, operand );

            CPU.FlagCarry = ( temp == 1 );

            CPU.FlagZero = ( operand == 0 );
            CPU.FlagNegative = ( ( operand & 0x80 ) != 0 );

            CPU.PC += 3;
          }
          return 7;
        case 0x81:
          // STA ($FF,X)    2b, 6c
          {
            ushort    zpBaseAddress = OnReadAddress( (ushort)( CPU.PC + 1 ) );

            ushort    addressLo = OnReadAddress( (ushort)( ( zpBaseAddress + CPU.X ) % 256 ) );
            ushort    addressHi = OnReadAddress( (ushort)( ( zpBaseAddress + CPU.X + 1 ) % 256 ) );
            ushort    finalAddress = (ushort)( addressLo + ( addressHi << 8 ) );

            OnWriteAddress( finalAddress, CPU.Accu );

            CPU.PC += 2;
          }
          return 6;
        case 0x84:
          // STY $FF             2b, 3c
          {
            ushort    address = OnReadAddress( (ushort)( CPU.PC + 1 ) );

            OnWriteAddress( address, CPU.Y );

            CPU.PC += 2;
          }
          return 3;
        case 0x85:
          // STA $FF             2b 3c
          {
            ushort    address = OnReadAddress( (ushort)( CPU.PC + 1 ) );

            OnWriteAddress( address, CPU.Accu );

            CPU.PC += 2;
          }
          return 3;
        case 0x86:
          // STX $FF             2b, 3c
          {
            ushort    address = OnReadAddress( (ushort)( CPU.PC + 1 ) );

            OnWriteAddress( address, CPU.X );

            CPU.PC += 2;
          }
          return 3;
        case 0x88:
          // DEY                  1b, 2c
          {
            CPU.Y = (byte)( CPU.Y - 1 );

            CPU.FlagZero = ( CPU.Y == 0 );
            CPU.FlagNegative = ( ( CPU.Y & 0x80 ) != 0 );

            CPU.PC += 1;
          }
          return 2;
        case 0x8a:
          // TXA                  1b, 2c
          {
            CPU.Accu = CPU.X;

            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();

            CPU.PC += 1;
          }
          return 2;
        case 0x8c:
          // STY $FFFF           3b, 4c
          {
            ushort    address = OnReadWord( (ushort)( CPU.PC + 1 ) );

            OnWriteAddress( address, CPU.Y );

            CPU.PC += 3;
          }
          return 4;
        case 0x8d:
          // STA $FFFF           3b 4c
          {
            ushort    address = OnReadWord( (ushort)( CPU.PC + 1 ) );

            OnWriteAddress( address, CPU.Accu );

            CPU.PC += 3;
          }
          return 4;
        case 0x8e:
          {
            // STX $FFFF             3 bytes, 4 cycles
            ushort    address = OnReadWord( (ushort)( CPU.PC + 1 ) );

            OnWriteAddress( address, CPU.X );

            CPU.PC += 3;
          }
          return 4;
        case 0x90:
          // BCC $FFFF      2b, 2*c
          return HandleBranch( !CPU.FlagCarry );
        case 0x91:
          // STA ($FF),Y    2b, 6c
          {
            ushort    address = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            ushort    finalAddress = CalcIndirectY( address, CPU.Y );

            OnWriteAddress( finalAddress, CPU.Accu );

            CPU.PC += 2;
          }
          return 6;
        case 0x94:
          // STY $FF,X      2b, 4c
          {
            ushort    address = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            ushort    finalAddress = CalcZeropageX( address, CPU.X );

            OnWriteAddress( finalAddress, CPU.Y );

            CPU.PC += 2;
          }
          return 4;
        case 0x95:
          // STA $FF,X      2b, 4c
          {
            ushort    address = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            ushort    finalAddress = CalcZeropageX( address, CPU.X );

            OnWriteAddress( finalAddress, CPU.Accu );

            CPU.PC += 2;
          }
          return 4;
        case 0x96:
          // STX $FF,Y           2b, 4c
          {
            ushort address = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            ushort finalAddress = CalcZeropageX( address, CPU.Y );

            OnWriteAddress( finalAddress, CPU.X );

            CPU.PC += 2;
          }
          return 4;
        case 0x98:
          // TYA            1b, 2c
          {
            CPU.Accu = CPU.Y;

            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();

            CPU.PC += 1;
          }
          return 2;
        case 0x99:
          // STA $FFFF,Y    3b, 5c
          {
            ushort    address = OnReadWord( (ushort)( CPU.PC + 1 ) );
            ushort    finalAddress = CalcAbsoluteY( address, CPU.Y );

            OnWriteAddress( finalAddress, CPU.Accu );

            CPU.PC += 3;
          }
          return 5;
        case 0x9a:
          // TXS        1 byte, 2 cycles
          {
            CPU.StackPointer = CPU.X;

            CPU.PC += 1;
          }
          return 2;
        case 0x9d:
          // STA $FFFF,X    3b, 5c
          {
            ushort    address = OnReadWord( (ushort)( CPU.PC + 1 ) );
            ushort    finalAddress = CalcAbsoluteX( address, CPU.X );

            OnWriteAddress( finalAddress, CPU.Accu );

            CPU.PC += 3;
          }
          return 5;
        case 0xa0:
          // LDY #$FF   2b, 2c
          {
            byte    operand = OnReadAddress( (ushort)( CPU.PC + 1 ) );

            CPU.Y = operand;
            CPU.CheckFlagZeroY();
            CPU.CheckFlagNegativeY();
            CPU.PC += 2;
          }
          return 2;
        case 0xa1:
          // LDA ($FF,X)      2b, 6c
          {
            ushort    zpBaseAddress = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            ushort    address = zpBaseAddress;

            ushort    addressLo = OnReadAddress( (ushort)( ( zpBaseAddress + CPU.X ) % 256 ) );
            ushort    addressHi = OnReadAddress( (ushort)( ( zpBaseAddress + CPU.X + 1 ) % 256 ) );
            ushort    finalAddress = (ushort)( addressLo + ( addressHi << 8 ) );

            CPU.Accu = OnReadAddress( finalAddress );

            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();

            CPU.PC += 2;
          }
          return 6;
        case 0xa2:
          // LDX #$FF   2 bytes, 2 cycles
          {
            byte    operand = OnReadAddress( (ushort)( CPU.PC + 1 ) );

            CPU.X = operand;
            CPU.CheckFlagZeroX();
            CPU.CheckFlagNegativeX();
            CPU.PC += 2;
          }
          return 2;
        case 0xa4:
          // LDY $FF   2b, 3c
          {
            ushort  address = OnReadAddress( (ushort)( CPU.PC + 1 ) );

            CPU.Y = OnReadAddress( address );
            CPU.CheckFlagZeroY();
            CPU.CheckFlagNegativeY();
            CPU.PC += 2;
          }
          return 3;
        case 0xa5:
          // LDA $FF             2b, 3c
          {
            ushort  address = OnReadAddress( (ushort)( CPU.PC + 1 ) );

            CPU.Accu = OnReadAddress( address );
            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();
            CPU.PC += 2;
          }
          return 3;
        case 0xa6:
          // LDX $FF             2b, 3c
          {
            ushort  address = OnReadAddress( (ushort)( CPU.PC + 1 ) );

            CPU.X = OnReadAddress( address );
            CPU.CheckFlagZeroX();
            CPU.CheckFlagNegativeX();
            CPU.PC += 2;
          }
          return 3;
        case 0xa8:
          // TAY       1b, 2c
          {
            CPU.Y = CPU.Accu;
            CPU.CheckFlagZeroY();
            CPU.CheckFlagNegativeY();
            CPU.PC += 1;
          }
          return 2;
        case 0xa9:
          // LDA #$FF  2b, 2c
          {
            byte    operand = OnReadAddress( (ushort)( CPU.PC + 1 ) );

            CPU.Accu = operand;
            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();
            CPU.PC += 2;
          }
          return 2;
        case 0xaa:
          // TAX            1b, 2c
          {
            CPU.X = CPU.Accu;

            CPU.CheckFlagZeroX();
            CPU.CheckFlagNegativeX();

            CPU.PC += 1;
          }
          return 2;
        case 0xac:
          // LDY $FFFF           3b, 4c
          {
            ushort    address = OnReadWord( (ushort)( CPU.PC + 1 ) );

            CPU.Y = OnReadAddress( address );
            CPU.CheckFlagZeroY();
            CPU.CheckFlagNegativeY();

            CPU.PC += 3;
          }
          return 4;
        case 0xad:
          // LDA $FFFF     3b, 4c
          {
            ushort    address = OnReadWord( (ushort)( CPU.PC + 1 ) );

            CPU.Accu = OnReadAddress( address );
            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();

            CPU.PC += 3;
          }
          return 4;
        case 0xae:
          // LDX $FFFF           3b, 4c
          {
            ushort    address = OnReadWord( (ushort)( CPU.PC + 1 ) );

            CPU.X = OnReadAddress( address );
            CPU.CheckFlagZeroX();
            CPU.CheckFlagNegativeX();

            CPU.PC += 3;
          }
          return 4;
        case 0xb0:
          // BCS $FF        2b, 2*
          return HandleBranch( CPU.FlagCarry );
        case 0xb1:
          // LDA ($FF),Y    2b, 5*c
          {
            int numCycles = 5;

            ushort    address = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            ushort    finalAddress = CalcIndirectY( address, CPU.Y );
            if ( ( address & 0xff00 ) != ( finalAddress & 0xff00 ) )
            {
              ++numCycles;
            }

            CPU.Accu = OnReadAddress( finalAddress );
            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();

            CPU.PC += 2;

            return numCycles;
          }
        case 0xb4:
          // LDY $FF,X      2b, 4c
          {
            ushort    address = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            ushort    finalAddress = CalcZeropageX( address, CPU.X );

            CPU.Y = OnReadAddress( finalAddress );
            CPU.CheckFlagZeroY();
            CPU.CheckFlagNegativeY();

            CPU.PC += 2;
          }
          return 4;
        case 0xb5:
          // LDA $FF,X      2b, 4c
          {
            ushort    address = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            ushort    finalAddress = CalcZeropageX( address, CPU.X );

            CPU.Accu = OnReadAddress( finalAddress );

            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();

            CPU.PC += 2;
          }
          return 4;
        case 0xb6:
          // LDX $FF,y           2b, 4c
          {
            ushort  address = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            ushort  finalAddress = CalcZeropageX( address, CPU.Y );

            CPU.X = OnReadAddress( finalAddress );
            CPU.CheckFlagZeroX();
            CPU.CheckFlagNegativeX();
            CPU.PC += 2;
          }
          return 4;
        case 0xb8:
          // CLV        1b, 2c
          {
            CPU.FlagOverflow = false;

            CPU.PC += 1;
          }
          return 2;
        case 0xb9:
          // LDA $FFFF,Y   3b, 4*c
          {
            int numCycles = 4;
            ushort    address = OnReadWord( (ushort)( CPU.PC + 1 ) );

            if ( ( address & 0xff00 ) != ( ( address + CPU.Y ) & 0xff00 ) )
            {
              ++numCycles;
            }

            ushort    finalAddress = CalcAbsoluteY( address, CPU.Y );

            CPU.Accu = OnReadAddress( finalAddress );
            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();

            CPU.PC += 3;

            return numCycles;
          }
        case 0xba:
          // TSX            1b, 2c
          {
            CPU.X = CPU.StackPointer;

            CPU.CheckFlagZeroX();
            CPU.CheckFlagNegativeX();

            CPU.PC += 1;
          }
          return 2;
        case 0xbd:
          // LDA $FFFF,X   3 bytes, 4* cycles
          {
            int       numCycles = 4;
            ushort    address = OnReadWord( (ushort)( CPU.PC + 1 ) );
            ushort    finalAddress = CalcAbsoluteX( address, CPU.X );

            if ( ( address & 0xff00 ) != ( finalAddress & 0xff00 ) )
            {
              ++numCycles;
            }

            /*
            if ( ( CPU.PC >= 0x0801 )
            &&   ( CPU.PC < 0x2000 ) )
            {
              Debug.Log( "ldaax from address " + address.ToString( "X2" ) + ", X:" + CPU.X.ToString( "X2" ) + " = finalAddress: " + finalAddress.ToString( "X4" ) );

              Debug.Log( "  RAM is " + Memory.RAM[finalAddress].ToString( "X2" ) );

              Debug.Log( $"Run {opCode.ToString( "X2" )}/{CPU.OpcodeByValue[opCode].Mnemonic} at {CPU.PC.ToString( "X4" )} "
                          + "A: " + CPU.Accu.ToString( "X2" ) + " X: " + CPU.X.ToString( "X2" ) + " Y: " + CPU.Y.ToString( "X2" )
                          + " F: " + CPU.Flags.ToString( "X2" ) );
            }*/

            CPU.Accu = OnReadAddress( finalAddress );
            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();

            /*
            if ( ( CPU.PC >= 0x0801 )
            &&   ( CPU.PC < 0x2000 ) )
            {
              Debug.Log( "A = " + CPU.Accu.ToString( "X2" ) + ", FlagZero = " + CPU.FlagZero );
            }*/

            CPU.PC += 3;

            return numCycles;
          }
        case 0xbc:
          // LDY $FFFF,X         3b, 4*c
          {
            int       numCycles = 4;
            ushort    address = OnReadWord( (ushort)( CPU.PC + 1 ) );
            ushort    finalAddress = CalcAbsoluteX( address, CPU.X );

            if ( ( address & 0xff00 ) != ( finalAddress & 0xff00 ) )
            {
              ++numCycles;
            }

            CPU.Y = OnReadAddress( finalAddress );
            CPU.CheckFlagZeroY();
            CPU.CheckFlagNegativeY();

            CPU.PC += 3;
            return numCycles;
          }
        case 0xbe:
          // LDX $FFFF,y           3b, 4*c
          {
            int       numCycles = 4;

            ushort    address = OnReadWord( (ushort)( CPU.PC + 1 ) );
            ushort    finalAddress = CalcAbsoluteY( address, CPU.Y );

            if ( ( address & 0xff00 ) != ( finalAddress & 0xff00 ) )
            {
              ++numCycles;
            }

            CPU.X = OnReadAddress( finalAddress );
            CPU.CheckFlagZeroX();
            CPU.CheckFlagNegativeX();
            CPU.PC += 3;

            return numCycles;
          }
        case 0xc0:
          // CPY #$FF            2b, 2c
          {
            byte  compareWith = OnReadAddress( (ushort)( CPU.PC + 1 ) );

            int   result = CPU.Y - compareWith;

            CPU.FlagZero = ( compareWith == CPU.Y );
            CPU.FlagCarry = ( result >= 0 );
            CPU.FlagNegative = ( ( ( (byte)( result ) ) & 0x80 ) != 0 );

            CPU.PC += 2;
          }
          return 2;
        case 0xc1:
          // CMP ($FF,X)             2b, 6c
          {
            ushort    zpBaseAddress = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            ushort    addressLo = OnReadAddress( (ushort)( ( zpBaseAddress + CPU.X ) % 256 ) );
            ushort    addressHi = OnReadAddress( (ushort)( ( zpBaseAddress + CPU.X + 1 ) % 256 ) );
            ushort    finalAddress = (ushort)( addressLo + ( addressHi << 8 ) );
            byte  compareWith = OnReadAddress( finalAddress );

            int   result = CPU.Accu - compareWith;

            CPU.FlagZero = ( compareWith == CPU.Accu );
            CPU.FlagCarry = ( result >= 0 );
            CPU.FlagNegative = ( ( ( (byte)( result ) ) & 0x80 ) != 0 );

            CPU.PC += 2;
          }
          return 6;
        case 0xc2:
          // NOP #$FF            2b, 2c
          {
            CPU.PC += 2;
          }
          return 2;
        case 0xc4:
          // CPY $FF             2b, 3c
          {
            ushort  address = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            byte  compareWith = OnReadAddress( address );

            int   result = CPU.Y - compareWith;

            CPU.FlagZero = ( compareWith == CPU.Y );
            CPU.FlagCarry = ( result >= 0 );
            CPU.FlagNegative = ( ( ( (byte)( result ) ) & 0x80 ) != 0 );

            CPU.PC += 2;
          }
          return 3;
        case 0xc5:
          // CMP $FF             2b, 3c
          {
            ushort  address = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            byte  compareWith = OnReadAddress( address );

            int   result = CPU.Accu - compareWith;

            CPU.FlagZero = ( compareWith == CPU.Accu );
            CPU.FlagCarry = ( result >= 0 );
            CPU.FlagNegative = ( ( ( (byte)( result ) ) & 0x80 ) != 0 );

            CPU.PC += 2;
          }
          return 3;
        case 0xc6:
          // DEC $FF             2b, 5c
          {
            ushort  address = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            byte  operand = OnReadAddress( address );
            operand = (byte)( operand - 1 );

            OnWriteAddress( address, operand );

            CPU.FlagZero = ( operand == 0 );
            CPU.FlagNegative = ( ( operand & 0x80 ) != 0 );

            CPU.PC += 2;
          }
          return 5;
        case 0xc8:
          // INY        1b, 2c
          {
            CPU.Y = (byte)( CPU.Y + 1 );

            CPU.FlagZero = ( CPU.Y == 0 );
            CPU.FlagNegative = ( ( CPU.Y & 0x80 ) != 0 );

            CPU.PC += 1;
          }
          return 2;
        case 0xc9:
          // CMP #$FF   2b, 2c
          {
            byte  compareWith = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            int result = CPU.Accu - compareWith;

            CPU.FlagZero = ( compareWith == CPU.Accu );
            CPU.FlagCarry = ( result >= 0 );
            CPU.FlagNegative = ( ( ( (byte)( result ) ) & 0x80 ) != 0 );

            CPU.PC += 2;
          }
          return 2;
        case 0xca:
          // DEX        1b 2c
          {
            CPU.X = (byte)( CPU.X - 1 );

            CPU.CheckFlagZeroX();
            CPU.CheckFlagNegativeX();

            CPU.PC += 1;
          }
          return 2;
        case 0xcc:
          // CPY $FFFF             3b, 4c
          {
            ushort  address = OnReadWord( (ushort)( CPU.PC + 1 ) );
            byte  compareWith = OnReadAddress( address );

            int   result = CPU.Y - compareWith;

            CPU.FlagZero = ( compareWith == CPU.Y );
            CPU.FlagCarry = ( result >= 0 );
            CPU.FlagNegative = ( ( ( (byte)( result ) ) & 0x80 ) != 0 );

            CPU.PC += 3;
          }
          return 4;
        case 0xcd:
          // CMP $FFFF           3b, 4c
          {
            ushort    address = OnReadWord( (ushort)( CPU.PC + 1 ) );

            byte  compareWith = OnReadAddress( address );

            int result = CPU.Accu - compareWith;

            CPU.FlagZero = ( compareWith == CPU.Accu );
            CPU.FlagCarry = ( result >= 0 );
            CPU.FlagNegative = ( ( ( (byte)( result ) ) & 0x80 ) != 0 );

            CPU.PC += 3;
          }
          return 4;
        case 0xce:
          // DEC $FFFF           3b, 6
          {
            ushort  address = OnReadWord( (ushort)( CPU.PC + 1 ) );

            byte    value = OnReadAddress( address );

            value = (byte)( value - 1 );
            //value = (byte)( ( value + 255 ) & 0xff );

            OnWriteAddress( address, value );

            CPU.FlagZero = ( value == 0 );
            CPU.FlagNegative = ( ( value & 0x80 ) != 0 );

            CPU.PC += 3;
          }
          return 5;
        case 0xd0:
          // BNE $FF    2 bytes, 2* cycles
          return HandleBranch( !CPU.FlagZero );
        case 0xd1:
          // CMP ($FF),Y    2b, 5*c
          {
            int       numCycles = 5;
            ushort    address = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            if ( ( address & 0xff00 ) != ( ( address + CPU.Y ) & 0xff00 ) )
            {
              ++numCycles;
            }

            ushort    finalAddress = CalcIndirectY( address, CPU.Y );

            byte  compareWith = OnReadAddress( finalAddress );
            int result = CPU.Accu - compareWith;

            CPU.FlagZero = ( compareWith == CPU.Accu );
            CPU.FlagCarry = ( result >= 0 );
            CPU.FlagNegative = ( ( ( (byte)( result ) ) & 0x80 ) != 0 );

            CPU.PC += 2;

            return numCycles;
          }
        case 0xd5:
          // CMP $FF,X    2 bytes, 4 cycles
          {
            ushort    address = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            ushort    finalAddress = CalcZeropageX( address, CPU.X );

            byte  compareWith = OnReadAddress( finalAddress );

            int   result = CPU.Accu - compareWith;

            CPU.FlagZero = ( compareWith == CPU.Accu );
            CPU.FlagCarry = ( result >= 0 );
            CPU.FlagNegative = ( ( ( (byte)( result ) ) & 0x80 ) != 0 );

            CPU.PC += 2;

            return 4;
          }
        case 0xd6:
          // DEC $FF,X         2b, 6
          {
            ushort  address = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            ushort  finalAddress = CalcZeropageX( address, CPU.X );
            byte    value = OnReadAddress( finalAddress );

            value = (byte)( value - 1 );

            OnWriteAddress( finalAddress, value );

            CPU.FlagZero = ( value == 0 );
            CPU.FlagNegative = ( ( value & 0x80 ) != 0 );

            CPU.PC += 2;
          }
          return 6;
        case 0xd8:
          // CLD    1 byte, 2 cycles
          {
            CPU.FlagDecimal = false;

            CPU.PC += 1;
          }
          return 2;
        case 0xd9:
          // CMP $FFFF,Y    3 bytes, 4* cycles
          {
            int       numCycles = 4;
            ushort    address = OnReadWord( (ushort)( CPU.PC + 1 ) );
            ushort    finalAddress = CalcAbsoluteY( address, CPU.Y );

            if ( ( address & 0xff00 ) != ( finalAddress & 0xff00 ) )
            {
              ++numCycles;
            }

            byte  compareWith = OnReadAddress( finalAddress );
            int result = CPU.Accu - compareWith;

            CPU.FlagZero = ( compareWith == CPU.Accu );
            CPU.FlagCarry = ( result >= 0 );
            CPU.FlagNegative = ( ( ( (byte)( result ) ) & 0x80 ) != 0 );

            CPU.PC += 3;

            return numCycles;
          }
        case 0xdd:
          // CMP $FFFF,X    3 bytes, 4* cycles
          {
            int       numCycles = 4;
            ushort    address = OnReadWord( (ushort)( CPU.PC + 1 ) );
            ushort    finalAddress = CalcAbsoluteX( address, CPU.X );

            if ( ( address & 0xff00 ) != ( finalAddress & 0xff00 ) )
            {
              ++numCycles;
            }

            byte  compareWith = OnReadAddress( finalAddress );
            int result = CPU.Accu - compareWith;

            CPU.FlagZero      = ( compareWith == CPU.Accu );
            CPU.FlagCarry = ( result >= 0 );
            CPU.FlagNegative = ( ( ( (byte)( result ) ) & 0x80 ) != 0 );

            CPU.PC += 3;

            return numCycles;
          }
        case 0xde:
          // DEC $FFFF,X         3b, 7
          {
            ushort  address = OnReadWord( (ushort)( CPU.PC + 1 ) );
            ushort finalAddress = CalcAbsoluteX( address, CPU.X );

            byte    value = OnReadAddress( finalAddress );

            value = (byte)( value - 1 );
            //value = (byte)( ( value + 255 ) & 0xff );

            OnWriteAddress( finalAddress, value );

            CPU.FlagZero = ( value == 0 );
            CPU.FlagNegative = ( ( value & 0x80 ) != 0 );

            CPU.PC += 3;
          }
          return 7;
        case 0xe0:
          // CPX #$FF            2b, 2c
          {
            byte  compareWith = OnReadAddress( (ushort)( CPU.PC + 1 ) );

            int result = CPU.X - compareWith;

            CPU.FlagZero = ( compareWith == CPU.X );
            CPU.FlagCarry = ( result >= 0 );
            CPU.FlagNegative = ( ( ( (byte)( result ) ) & 0x80 ) != 0 );

            CPU.PC += 2;
          }
          return 2;
        case 0xe1:
          // SBC ($FF,x)           2b, 6c
          {
            ushort    zpBaseAddress = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            ushort    addressLo = OnReadAddress( (ushort)( ( zpBaseAddress + CPU.X ) % 256 ) );
            ushort    addressHi = OnReadAddress( (ushort)( ( zpBaseAddress + CPU.X + 1 ) % 256 ) );
            ushort    finalAddress = (ushort)( addressLo + ( addressHi << 8 ) );

            byte    operand = OnReadAddress( finalAddress );

            HandleSBC( operand );

            CPU.PC += 2;

            return 6;
          }
        case 0xe4:
          // CPX $FF             2b, 3c
          {
            ushort  address = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            byte    compareWith = OnReadAddress( address );

            int result = CPU.X - compareWith;

            CPU.FlagZero = ( compareWith == CPU.X );
            CPU.FlagCarry = ( result >= 0 );
            CPU.FlagNegative = ( ( ( (byte)( result ) ) & 0x80 ) != 0 );

            CPU.PC += 2;
          }
          return 3;
        case 0xe5:
          // SBC $FF             2b, 3c
          {
            ushort  address = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            byte    operand = OnReadAddress( address );

            HandleSBC( operand );

            CPU.PC += 2;
          }
          return 3;
        case 0xe6:
          // INC $FF             2b, 5c
          {
            ushort  address = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            byte    value = OnReadAddress( address );
            value = (byte)( value + 1 );
            OnWriteAddress( address, value );

            CPU.FlagZero = ( value == 0 );
            CPU.FlagNegative = ( ( value & 0x80 ) != 0 );

            CPU.PC += 2;
          }
          return 5;
        case 0xe8:
          // INX                 1b, 2c
          {
            CPU.X = (byte)( CPU.X + 1 );

            CPU.FlagZero = ( CPU.X == 0 );
            CPU.FlagNegative = ( ( CPU.X & 0x80 ) != 0 );

            CPU.PC += 1;
          }
          return 2;
        case 0xe9:
          // SBC #$FF            2b, 2c
          {
            byte    operand = OnReadAddress( (ushort)( CPU.PC + 1 ) );

            HandleSBC( operand );

            CPU.PC += 2;
          }
          return 2;
        case 0xea:
          // NOP                 1b, 2c
          ++CPU.PC;
          return 2;
        case 0xec:
          // CPX $FFFF           3b, 4c
          {
            ushort  address = OnReadWord( (ushort)( CPU.PC + 1 ) );
            byte  compareWith = OnReadAddress( address );

            int result = CPU.X - compareWith;

            CPU.FlagZero = ( compareWith == CPU.X );
            CPU.FlagCarry = ( result >= 0 );
            CPU.FlagNegative = ( ( ( (byte)( result ) ) & 0x80 ) != 0 );

            CPU.PC += 3;
          }
          return 4;
        case 0xed:
          // SBC $FFFF           3b, 4c
          {
            ushort  address = OnReadWord( (ushort)( CPU.PC + 1 ) );
            byte    operand = OnReadAddress( address );

            HandleSBC( operand );

            CPU.PC += 3;
          }
          return 4;
        case 0xee:
          // INC $FFFF             3b, 6c
          {
            ushort  address = OnReadWord( (ushort)( CPU.PC + 1 ) );
            byte    value = OnReadAddress( address );
            value = (byte)( value + 1 );
            OnWriteAddress( address, value );

            CPU.FlagZero = ( value == 0 );
            CPU.FlagNegative = ( ( value & 0x80 ) != 0 );

            CPU.PC += 3;
          }
          return 6;
        case 0xf0:
          // BEQ $FFFF           2b, 2c
          return HandleBranch( CPU.FlagZero );
        case 0xf1:
          // SBC ($FF),y             2b, 5*c
          {
            int numCycles = 5;

            ushort    address = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            if ( address + CPU.Y >= 0x100 )
            {
              ++numCycles;
            }
            ushort    finalAddress = CalcIndirectY( address, CPU.Y );
            byte    operand = OnReadAddress( finalAddress );

            HandleSBC( operand );

            CPU.PC += 2;
            return numCycles;
          }
        case 0xf5:
          // SBC $FF,X             2b, 4c
          {
            ushort  address = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            ushort  finalAddress = CalcZeropageX( address, CPU.X );
            byte    operand = OnReadAddress( finalAddress );

            HandleSBC( operand );

            CPU.PC += 2;
          }
          return 4;
        case 0xf6:
          // INC $FF,X           2b, 6c
          {
            ushort  address = OnReadAddress( (ushort)( CPU.PC + 1 ) );
            ushort  finalAddress = CalcZeropageX( address, CPU.X );
            byte    value = OnReadAddress( finalAddress );
            value = (byte)( value + 1 );
            OnWriteAddress( finalAddress, value );

            CPU.FlagZero = ( value == 0 );
            CPU.FlagNegative = ( ( value & 0x80 ) != 0 );

            CPU.PC += 2;
          }
          return 6;
        case 0xf8:
          // SED                1b, 2c
          {
            CPU.FlagDecimal = true;
            CPU.PC += 1;
          }
          return 2;
        case 0xf9:
          // SBC  $FFFF,Y             3b, 4c
          {
            int numCycles = 4;

            ushort    address = OnReadWord( (ushort)( CPU.PC + 1 ) );
            ushort    finalAddress = CalcAbsoluteY( address, CPU.Y );
            if ( ( address & 0xff00 ) != ( finalAddress & 0xff00 ) )
            {
              ++numCycles;
            }
            
            byte operand = OnReadAddress( finalAddress );

            HandleSBC( operand );

            CPU.PC += 3;

            return numCycles;
          }
        case 0xfd:
          // SBC $FFFF,x           3b, 4*c
          {
            int       numCycles = 4;
            ushort    address = OnReadWord( (ushort)( CPU.PC + 1 ) );
            ushort    finalAddress = CalcAbsoluteX( address, CPU.X );
            if ( ( address & 0xff00 ) != ( finalAddress & 0xff00 ) )
            {
              ++numCycles;
            }

            byte    operand = OnReadAddress( finalAddress );

            HandleSBC( operand );
            CPU.PC += 3;

            return numCycles;
          }
        case 0xfe:
          // INC $FFFF,X           2b, 7c
          {
            ushort  address = OnReadWord( (ushort)( CPU.PC + 1 ) );
            ushort  finalAddress = CalcAbsoluteX( address, CPU.X );
            byte    value = OnReadAddress( finalAddress );
            value = (byte)( value + 1 );
            OnWriteAddress( finalAddress, value );

            CPU.FlagZero = ( value == 0 );
            CPU.FlagNegative = ( ( value & 0x80 ) != 0 );

            CPU.PC += 3;
          }
          return 7;
        default:
          throw new NotSupportedException( "Unsupported opcode " + opCode.ToString( "X2" ) + " at " + CPU.PC.ToString( "X4" ) );
      }
    }



    private void HandleADC( byte Operand )
    {
      uint     result = (uint)( CPU.Accu + Operand + ( CPU.FlagCarry ? 1 : 0 ) );

      if ( CPU.FlagDecimal )
      {
        ushort tmp = (ushort)( ( CPU.Accu & 0xf ) + ( Operand & 0xf ) + ( CPU.FlagCarry ? 1 : 0 ) );
        if ( tmp > 0x9 )
        {
          tmp += 0x6;
        }
        if ( tmp <= 0x0f )
        {
          tmp = (ushort)( ( tmp & 0xf ) + ( CPU.Accu & 0xf0 ) + ( Operand & 0xf0 ) );
        }
        else
        {
          tmp = (ushort)( ( tmp & 0xf ) + ( CPU.Accu & 0xf0 ) + ( Operand & 0xf0 ) + 0x10 );
        }

        CPU.FlagZero = ( ( result & 0xff ) == 0 );
        CPU.FlagNegative = ( ( tmp & 0x80 ) != 0 );
        CPU.FlagOverflow = ( ( ( ( CPU.Accu ^ tmp ) & 0x80 ) != 0 ) && ( ( ( CPU.Accu ^ Operand ) & 0x80 ) == 0 ) );
        if ( ( tmp & 0x1f0 ) > 0x90 )
        {
          tmp += 0x60;
        }

        CPU.FlagCarry = ( ( tmp & 0xff0 ) > 0xf0 );
        CPU.Accu = (byte)tmp;
      }
      else
      {
        CPU.FlagCarry = ( result > 255 );
        CPU.FlagOverflow = !( ( ( CPU.Accu ^ Operand ) & 0x80 ) != 0 ) && ( ( CPU.Accu ^ result ) & 0x80 ) != 0;
        CPU.Accu = (byte)result;
        CPU.CheckFlagNegative();
        CPU.CheckFlagZero();
      }
    }



    private void HandleSBC( byte Operand )
    {
      ushort result = (ushort)( CPU.Accu - Operand - ( CPU.FlagCarry ? 0 : 1 ) );

      if ( CPU.FlagDecimal )
      {
        ushort decResult = (ushort)( ( CPU.Accu & 0xf ) - ( Operand & 0xf ) - ( CPU.FlagCarry ? 0 : 1 ) );
        if ( ( decResult & 0x10 ) != 0 )
        {
          decResult = (ushort)( ( ( decResult - 6 ) & 0xf ) | ( ( CPU.Accu & 0xf0 ) - ( Operand & 0xf0 ) - 0x10 ) );
        }
        else
        {
          decResult = (ushort)( ( decResult & 0xf ) | ( ( CPU.Accu & 0xf0 ) - ( Operand & 0xf0 ) ) );
        }
        if ( ( decResult & 0x100 ) != 0 )
        {
          decResult -= 0x60;
        }

        // flag are set as if no decimal was set
        CPU.FlagCarry = ( result < 256 );
        CPU.FlagNegative = ( ( result & 0x80 ) != 0 );
        CPU.FlagZero = ( ( result & 0xff ) == 0 );
        CPU.FlagOverflow = ( ( ( CPU.Accu ^ result ) & 0x80 ) != 0 ) && ( ( ( CPU.Accu ^ Operand ) & 0x80 ) != 0 );

        CPU.Accu = (byte)decResult;
      }
      else
      {
        CPU.FlagOverflow = ( ( ( CPU.Accu ^ result ) & 0x80 ) != 0 ) && ( ( ( CPU.Accu ^ Operand ) & 0x80 ) != 0 );

        CPU.Accu = (byte)result;
        CPU.FlagCarry = ( result < 256 );

        CPU.CheckFlagNegative();
        CPU.CheckFlagZero();
      }
    }



    private void RemoveBreakpoint( Breakpoint bp )
    {
      var bpList = Breakpoints.GetValues( (ushort)bp.Address, true );
      bpList.Remove( bp );
    }



    private ushort OnReadWord( ushort Address )
    {
      //TODO - breakpoints
      byte  lo = OnReadAddress( Address );
      byte  hi = OnReadAddress( (ushort)( Address + 1 ) );

      return (ushort)( lo + ( hi << 8 ) );
    }



    private byte OnReadAddress( ushort Address )
    {
      byte  value = Memory.ReadByte( Address );

      if ( _ReadDebug )
      {
        Debug.Log( "Read from " + Address.ToString( "X4" ) + ": " + value.ToString( "X2" ) );
      }
      /*
      if ( CPU.PC == 0xe63c )
      {
        Debug.Log( "read " + value.ToString( "X2" ) + " from address " + Address.ToString( "X4" ) );
        if ( Address == 0x4f4 )
        {
          FullDebug = true;
        }
      }*/

      CheckBreakpoints( Address, true, false, false );

      return value;
    }



    private void OnExecAddress( ushort Address )
    {
      if ( Address == 0xE5CD )
      {
        if ( !StartupCompleted )
        {
          StartupCompleted = true;
          if ( InjectFileAfterStartup != null )
          {
            ushort    startAddress = InjectFileAfterStartup.UInt16At( 0 );

            System.Array.Copy( InjectFileAfterStartup.Data(), 2, Memory.RAM, startAddress, InjectFileAfterStartup.Length - 2 );

            InjectKey( PhysicalKey.KEY_R );
            InjectKey( PhysicalKey.KEY_U );
            InjectKey( PhysicalKey.KEY_N );
            InjectKey( PhysicalKey.KEY_RETURN );
          }
        }
      }
      //TODO - breakpoints
      CheckBreakpoints( Address, false, false, true );

      /*
      if ( ( Address == 0xb8d2 )
      &&   ( Memory.RAM[0x61] == 0x88 ) )
      {
        FullDebug = true;
      }*/
      /*
      if ( Address == 0xb7ee )
      {
        //Debug.Log( "POKE" );
        FullDebug = true;
      }*/
    }



    private void InjectKey( PhysicalKey Key )
    {
    }



    private void OnWriteAddress( ushort Address, byte Value )
    {
      /*
      if ( ( Address == 0x897 )
      ||   ( Address == 0x898 ) )
      {
        Debug.Log( "Write " + Value.ToString( "X2" ) + " to " + Address.ToString( "X4" ) );
      }*/

      Memory.WriteByte( Address, Value );
      CheckBreakpoints( Address, false, true, false );

      if ( Address == 0x01 )
      {
        // 1 = Output, 0 = Input( see $01 for description) |
        //| Bit  5 | Cassette Motor Control(0 = On, 1 = Off)        |
        //| Bit  4 | Cassette Switch Sense: 1 = Switch Closed |
        //| Bit  3 | Cassette Data Output Line |
        //| Bit  2 |   / CharEn - Signal( see Memory Configuration ) |
        //| Bit  1 |   / HiRam - Signal( see Memory Configuration ) |
        //| Bit  0 |   / LoRam - Signal( see Memory Configuration )

        if ( ( Memory.RAM[0] & 0x01 ) != 0 )
        {
          LoRAM = ( ( Value & 0x01 ) != 0 );
        }
        if ( ( Memory.RAM[0] & 0x02 ) != 0 )
        {
          HiRAM = ( ( Value & 0x02 ) != 0 );
        }
        if ( ( Memory.RAM[0] & 0x04 ) != 0 )
        {
          CharEn = ( ( Value & 0x04 ) != 0 );
        }

        Memory.SetupMapping( Game, ExRom, CharEn, HiRAM, LoRAM );
      }
    }



    private void CheckBreakpoints( ushort Address, bool Read, bool Write, bool Exec )
    {
      TriggeredBreakpoints.Clear();

      var potentialBreakPoints = Breakpoints.GetValues( Address, true );
      foreach ( var bp in potentialBreakPoints )
      {
        if ( ( bp.OnExecute == Exec )
        ||   ( bp.OnRead == Read )
        ||   ( bp.OnWrite == Write ) )
        {
          TriggeredBreakpoints.Add( bp );

          FullDebug = true;
          Breakpoints.Clear();
          TriggeredBreakpoints.Clear();
        }
      }
    }



    private int HandleBranch( bool Condition )
    {
      if ( !Condition )
      {
        CPU.PC += 2;
        return 2;
      }

      int     numCycles = 2;
      ushort  address = CalcRelativeAddress();

      if ( ( CPU.PC & 0xff00 ) == ( address & 0xff00 ) )
      {
        // branch to same page
        ++numCycles;
      }
      else
      {
        numCycles += 2;
      }
      CPU.PC = address;

      return numCycles;
    }



    private ushort CalcZeropageX( ushort Address, byte X )
    {
      byte    lo = (byte)Address;
      return (ushort)( ( lo + X ) % 256 );
    }



    private ushort CalcRelativeAddress()
    {
      byte    delta = OnReadAddress( (ushort)( CPU.PC + 1 ) );
      ushort  newAddress = (ushort)( CPU.PC + 2 );
      if ( ( delta & 0x80 ) != 0 )
      {
        newAddress = (ushort)( newAddress - ( 256 - delta ) );
      }
      else
      {
        newAddress += delta;
      }
      return newAddress;
    }



    private ushort CalcIndirectX( ushort Address, byte X )
    {
      byte    lo = OnReadAddress( (ushort)( ( Address + X ) % 256 ) );
      byte    hi = OnReadAddress( (ushort)( ( Address + X + 1 ) % 256 ) );
      return (ushort)( lo + ( hi << 8 ) );
    }



    private ushort CalcIndirectY( ushort Address, byte Y )
    {
      byte    lo = OnReadAddress( Address );
      byte    hi = OnReadAddress( (ushort)( ( Address + 1 ) % 256 ) );
      return (ushort)( lo + ( hi << 8 ) + Y );
    }



    private ushort CalcAbsoluteY( ushort Address, byte Y )
    {
      return (ushort)( Address + Y );
    }



    private ushort CalcAbsoluteX( ushort Address, byte X )
    {
      return (ushort)( Address + X );
    }



    private void PushStack( byte Value )
    {
      Memory.RAM[0x100 + CPU.StackPointer] = Value;
      CPU.StackPointer = (byte)( CPU.StackPointer - 1 );

      //Debug.Log( "PUSH " + Value.ToString( "X2" ) );
    }



    private byte PopStack()
    {
      CPU.StackPointer = (byte)( CPU.StackPointer + 1 );

      //Debug.Log( "POP " + Memory.RAM[0x100 + CPU.StackPointer].ToString( "X2" ) );
      return Memory.RAM[0x100 + CPU.StackPointer];
    }



    internal void AddBreakpoint( ushort Address, bool Read, bool Write, bool Execute )
    {
      Breakpoints.Add( Address, new Breakpoint() { Address = Address, OnRead = Read, OnWrite = Write, OnExecute = Execute } );
    }



    internal void AddTemporaryBreakpoint( ushort Address, bool Read, bool Write, bool Execute )
    {
      Breakpoints.Add( Address, new Breakpoint() { Address = Address, OnRead = Read, OnWrite = Write, OnExecute = Execute, Temporary = true } );
    }

  }
}
