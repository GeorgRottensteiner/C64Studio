using System;
using System.Collections.Generic;
using System.Text;
using Tiny64.data;



namespace Tiny64
{
  public class Machine
  {
    public Memory     Memory = new Memory();
    public Processor  CPU = new Processor();

    byte            IODirection = 0x2f;   // RAM 0000
    byte            PortRegister = 55;    // RAM 0001

    int             RasterPos = 0;
    int             CyclesInLine = 0;
    int             TotalCycles = 0;

    bool            Game = false;
    bool            ExRom = false;



    public Machine()
    {
      HardReset();
    }



    public bool LORAMActive
    {
      get
      {
        return ( PortRegister & 0x01 ) == 0x01;
      }
    }



    public bool HIRAMActive
    {
      get
      {
        return ( PortRegister & 0x02 ) == 0x02;
      }
    }



    public bool CHARENActive
    {
      get
      {
        return ( PortRegister & 0x04 ) == 0x04;
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
      Memory.Initialize();
      CPU.Initialize();

      IODirection   = 0x2f;
      PortRegister  = 55;
      RasterPos     = 0;
      CyclesInLine  = 0;
      TotalCycles   = 0;

      //Beim Hard-Reset startet die CPU bei der in $FFFC/$FFFD abgelegten Adresse (springt nach $FCE2, RESET
      CPU.PC = Memory.ReadWordDirect( 0xfffc );
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
        CyclesInLine += curCycles;
        if ( CyclesInLine >= 63 )
        {
          CyclesInLine -= 63;

          ++RasterPos;
          if ( RasterPos >= 312 )
          {
            RasterPos = 0;
          }

          Memory.VIC.SetRasterPos( RasterPos );
        }
      }
      cyclesUsed -= MaxCycles;
      return cyclesUsed;
    }



    // returns numbers of cycles used
    public int RunCycle()
    {
      // TODO - split opcodes for number of cycles!!
      byte    opCode = Memory.ReadByte( CPU.PC );

      //Debug.Log( CPU.PC.ToString( "X4" ) + ":" + opCode.ToString( "X2" ) + " A:" + CPU.Accu.ToString( "X2" )  + " X:" + CPU.X.ToString( "X2" ) + " Y:" + CPU.Y.ToString( "X2" ) + " " + ( Memory.RAM[0xc1] + ( Memory.RAM[0xc2] << 8 ) ).ToString( "X4" ) );

      //Debug.Log( "PC: " + CPU.PC.ToString( "X4" ) );
      if ( CPU.PC == 0xbcc0 )
      {
        Debug.Log( CPU.PC.ToString( "X4" ) + ":" + opCode.ToString( "X2" ) + " A:" + CPU.Accu.ToString( "X2" ) + " X:" + CPU.X.ToString( "X2" ) + " Y:" + CPU.Y.ToString( "X2" ) + " " + ( Memory.RAM[0xc1] + ( Memory.RAM[0xc2] << 8 ) ).ToString( "X4" ) );
      }

      switch ( opCode )
      {
        case 0x00:
          // BRK        1b, 7c
          {
            Memory.ReadByte( CPU.PC + 1 );
            PushStack( (byte)( ( CPU.PC + 2 ) >> 8 ) );
            PushStack( (byte)( ( CPU.PC + 2 ) & 0xff ) );
            PushStack( CPU.Flags );
            CPU.FlagIRQ = true;

            byte    lo = Memory.ReadByte( 0xfffe );
            byte    hi = Memory.ReadByte( 0xffff );

            CPU.PC = (ushort)( lo + ( hi << 8 ) );
            
          }
          return 7;
        case 0x02:
          // JAM        1b, ?c
          {
            throw new NotSupportedException( "JAM" );
          }
          return 0;
        case 0x05:
          // ORA $FF             2b, 3c
          {
            ushort    address = Memory.ReadByte( CPU.PC + 1 );
            byte    operand = Memory.ReadByte( address );

            CPU.Accu = (byte)( CPU.Accu | operand );

            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();
            CPU.PC += 2;
          }
          return 3;
        case 0x06:
          // ASL $FF             2b, 5c
          {
            ushort    address = Memory.ReadByte( CPU.PC + 1 );
            byte    operand = Memory.ReadByte( address );

            CPU.FlagCarry = ( ( operand & 0x80 ) != 0 );
            operand = (byte)( operand << 1 );

            Memory.WriteByte( address, operand );

            CPU.FlagZero = ( operand == 0 );
            CPU.FlagNegative = ( ( operand & 0x80 ) != 0 );

            CPU.PC += 2;
          }
          return 5;
        case 0x08:
          // PHP                 1b, 3c
          {
            PushStack( CPU.Flags );

            CPU.PC += 1;
          }
          return 3;
        case 0x09:
          // ORA #$FF   2b, 2c
          {
            byte    operand = Memory.ReadByte( CPU.PC + 1 );

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
            ushort    address = Memory.ReadWord( CPU.PC + 1 );

            byte    operand = Memory.ReadByte( address );

            CPU.Accu = (byte)( CPU.Accu | operand );

            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();
            CPU.PC += 3;
          }
          return 4;
        case 0x10:
          // BPL $FFFF           2b, 2*c
          return HandleBranch( !CPU.FlagNegative );
        case 0x16:
          // ASL $FF,X           2b, 6c
          {
            ushort    address = Memory.ReadByte( CPU.PC + 1 );
            ushort    finalAddress = CalcZeropageX( address, CPU.X );
            byte      operand = Memory.ReadByte( finalAddress );

            CPU.FlagCarry = ( ( operand & 0x80 ) != 0 );
            operand = (byte)( operand << 1 );

            Memory.WriteByte( finalAddress, operand );

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
        case 0x20:
          // JSR        3 bytes, 6 cycles
          {
            ushort    address = Memory.ReadWord( CPU.PC + 1 );

            ushort    returnAddress = (ushort)( CPU.PC + 2 );

            PushStack( (byte)( ( returnAddress >> 8 ) & 0xff ) );
            PushStack( (byte)( returnAddress & 0xff ) );

            CPU.PC = address;

            //Debug.Log( "JSR $" + CPU.PC.ToString( "X4" ) );
          }
          return 6;
        case 0x24:
          // BIT $FF             2b, 3c
          {
            ushort    address = Memory.ReadByte( CPU.PC + 1 );
            byte      operand = Memory.ReadByte( address );

            byte      result = (byte)( operand & CPU.Accu );

            CPU.FlagNegative = ( ( result & 0x80 ) != 0 );
            CPU.FlagOverflow = ( ( result & 0x40 ) != 0 );
            CPU.FlagZero = ( result == 0 );
            CPU.PC += 2;
          }
          return 3;
        case 0x28:
          // PLP                 1b, 4c
          {
            CPU.Flags = PopStack();

            CPU.PC += 1;
          }
          return 4;
        case 0x29:
          // AND #$FF            2b, 2c
          {
            byte    operand = Memory.ReadByte( CPU.PC + 1 );

            CPU.Accu = (byte)( CPU.Accu & operand );

            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();
            CPU.PC += 2;
          }
          return 2;
        case 0x2a:
          // ROL A                1b, 2c
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
          return 2;
        case 0x2c:
          // BIT $FFFF           3b, 4c
          {
            ushort    address = Memory.ReadWord( CPU.PC + 1 );
            byte      operand = Memory.ReadByte( address );

            byte      result = (byte)( operand & CPU.Accu );

            CPU.FlagNegative = ( ( result & 0x80 ) != 0 );
            CPU.FlagOverflow = ( ( result & 0x40 ) != 0 );
            CPU.FlagZero = ( result == 0 );

            CPU.PC += 3;
          }
          return 4;
        case 0x30:
          // BMI $FFFF           2b, 2*c
          return HandleBranch( CPU.FlagNegative );
        case 0x38:
          // SEC                1b, 2c
          {
            CPU.FlagCarry = true;
            CPU.PC += 1;
          }
          return 2;
        case 0x40:
          // RTI                 1b, 6c
          {
            CPU.Flags = PopStack();

            byte lo = PopStack();
            byte hi = PopStack();

            ushort  address = (ushort)( lo + ( hi << 8 ) );

            CPU.PC = address;
          }
          return 6;
        case 0x45:
          // EOR $FF             2b, 3c
          {
            ushort    address = Memory.ReadByte( CPU.PC + 1 );
            byte    operand = Memory.ReadByte( address );

            CPU.Accu = (byte)( CPU.Accu ^ operand );

            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();
            CPU.PC += 2;
          }
          return 3;
        case 0x46:
          // LSR $FF             2b, 5c
          {
            ushort    address = Memory.ReadByte( CPU.PC + 1 );
            byte    operand = Memory.ReadByte( address );

            CPU.FlagCarry = ( ( operand & 0x01 ) != 0 );

            byte  result = (byte)( operand >> 1 );
            Memory.WriteByte( address, result );

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
            byte operand = Memory.ReadByte( CPU.PC + 1 );

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
            ushort    address = Memory.ReadWord( CPU.PC + 1 );

            CPU.PC = address;

            //Debug.Log( "JMP $" + CPU.PC.ToString( "X4" ) );
          }
          return 3;
        case 0x56:
          // LSR $FF,X           2b, 6c
          {
            ushort    address = Memory.ReadByte( CPU.PC + 1 );
            ushort    finalAddress = CalcZeropageX( address, CPU.X );
            byte    operand = Memory.ReadByte( finalAddress );

            CPU.FlagCarry = ( ( operand & 0x01 ) != 0 );

            operand = (byte)( operand >> 1 );

            Memory.WriteByte( finalAddress, operand );

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
        case 0x60:
          // RTS        1 byte, 6 cycles
          {
            // PC from Stack, PC + 1 -> PC               
            ushort    returnAddress = PopStack();
            returnAddress |= (ushort)( PopStack() << 8 );
            
            //Debug.Log( "RTS from " + CPU.PC.ToString( "X4" ) + " to " + ( returnAddress + 1 ).ToString( "X4" ) );

            //TODO - $e421 has broken return address!  (0x0199)
            CPU.PC = (ushort)( returnAddress + 1 );
          }
          return 6;
        case 0x65:
          // ADC $FF             2b, 3c
          {
            ushort  address = Memory.ReadByte( CPU.PC + 1 );
            byte    operand = Memory.ReadByte( address );
            byte    origOperand = operand;
            byte    startValue = CPU.Accu;
            int     result = startValue + operand;

            if ( CPU.FlagCarry )
            {
              ++result;
            }

            CPU.Accu = (byte)( result );

            CPU.CheckFlagNegative();
            CPU.CheckFlagZero();

            // from VICE
            // (!((reg_a_read ^ tmp_value) & 0x80)  && ((reg_a_read ^ tmp) & 0x80)); 
            CPU.FlagOverflow = ( ( ( startValue ^ origOperand ) & 0x80 ) == 0 ) && ( ( ( startValue ^ CPU.Accu ) & 0x80 ) != 0 );
            //CPU.FlagOverflow = ( startValue & 0x80 ) != ( CPU.Accu & 0x80 );

            CPU.FlagCarry = ( result > 255 );

            CPU.PC += 2;
          }
          return 3;
        case 0x66:
          // ROR $FF             2b, 5c
          {
            ushort    address = Memory.ReadByte( CPU.PC + 1 );

            byte      operand = Memory.ReadByte( address );
            int       temp = operand & 0x01;
            operand = (byte)( operand >> 1 );
            if ( CPU.FlagCarry )
            {
              operand = (byte)( operand | 0x80 );
            }

            Memory.WriteByte( address, operand );

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
            byte    operand = Memory.ReadByte( CPU.PC + 1 );
            byte    startValue = CPU.Accu;
            byte    origOperand = operand;
            int     result = startValue + operand;

            if ( CPU.FlagCarry )
            {
              ++result;
            }

            CPU.FlagCarry = ( result > 255 );
            CPU.Accu = (byte)( result );

            CPU.CheckFlagNegative();
            CPU.CheckFlagZero();

            // from VICE
            // ( ! ( ( reg_a_read ^ tmp_value ) & 0x80 )  && ( ( reg_a_read ^ tmp ) & 0x80 ) ); 
            CPU.FlagOverflow = ( ( ( startValue ^ origOperand ) & 0x80 ) == 0 ) && ( ( ( startValue ^ CPU.Accu ) & 0x80 ) != 0 );
            //CPU.FlagOverflow = ( startValue & 0x80 ) != ( CPU.Accu & 0x80 );

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
            ushort    address = Memory.ReadWord( CPU.PC + 1 );
            ushort    finalAddress = Memory.ReadWord( address );

            CPU.PC = finalAddress;

            //Debug.Log( "JMP indirect to $" + CPU.PC.ToString( "X4" ) );
          }
          return 5;
        case 0x70:
          // BVS $FFFF           2b, 2*c
          return HandleBranch( CPU.FlagOverflow );
        case 0x76:
          // ROR $FF,X           2b, 6c
          {
            ushort    address = Memory.ReadByte( CPU.PC + 1 );
            ushort    finalAddress = CalcZeropageX( address, CPU.X );

            byte      operand = Memory.ReadByte( finalAddress );
            int       temp = operand & 0x01;
            operand = (byte)( operand >> 1 );
            if ( CPU.FlagCarry )
            {
              operand = (byte)( operand | 0x80 );
            }

            Memory.WriteByte( finalAddress, operand );

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
            // SOLL Carry und Overflow setzen
            //.C:be7b  79 17 BF    ADC $BF17,Y    - A:8B X:80 Y:08 SP:f9 N.-.....    2227475

            int numCycles = 4;

            ushort    address = Memory.ReadWord( CPU.PC + 1 );

            if ( ( address & 0xff00 ) != ( ( address + CPU.Y ) & 0xff00 ) )
            {
              ++numCycles;
            }

            ushort    finalAddress = CalcAbsoluteY( address, CPU.Y );

            byte operand = Memory.ReadByte( finalAddress );
            byte origOperand = operand;
            byte startValue = CPU.Accu;
            int  result = operand + startValue;

            if ( CPU.FlagCarry )
            {
              ++result;
            }

            CPU.FlagCarry = ( result > 255 );
            CPU.Accu = (byte)( result );

            CPU.CheckFlagNegative();
            CPU.CheckFlagZero();

            // from VICE
            // (!((reg_a_read ^ tmp_value) & 0x80)  && ((reg_a_read ^ tmp) & 0x80)); 
            CPU.FlagOverflow = ( ( ( startValue ^ origOperand ) & 0x80 ) == 0 ) && ( ( ( startValue ^ CPU.Accu ) & 0x80 ) != 0 );
            //CPU.FlagOverflow = ( startValue & 0x80 ) != ( CPU.Accu & 0x80 );


            CPU.PC += 3;

            return numCycles;
          }
        case 0x84:
          // STY $FF             2b, 3c
          {
            ushort    address = Memory.ReadByte( CPU.PC + 1 );

            Memory.WriteByte( address, CPU.Y );

            CPU.PC += 2;
          }
          return 3;
        case 0x85:
          // STA $FF             2b 3c
          {
            ushort    address = Memory.ReadByte( CPU.PC + 1 );

            Memory.WriteByte( address, CPU.Accu );

            CPU.PC += 2;
          }
          return 3;
        case 0x86:
          // STX $FF             2b, 3c
          {
            ushort    address = Memory.ReadByte( CPU.PC + 1 );

            Memory.WriteByte( address, CPU.X );

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
            ushort    address = Memory.ReadWord( CPU.PC + 1 );

            Memory.WriteByte( address, CPU.Y );

            CPU.PC += 3;
          }
          return 4;
        case 0x8d:
          // STA $FFFF           3b 4c
          {
            ushort    address = Memory.ReadWord( CPU.PC + 1 );

            Memory.WriteByte( address, CPU.Accu );

            CPU.PC += 3;
          }
          return 4;
        case 0x8e:
          {
            // STX $FFFF             3 bytes, 4 cycles
            ushort    address = Memory.ReadWord( CPU.PC + 1 );

            Memory.WriteByte( address, CPU.X );

            CPU.PC += 3;
          }
          return 4;
        case 0x90:
          // BCC $FFFF      2b, 2*c
          return HandleBranch( !CPU.FlagCarry );
        case 0x91:
          // STA ($FF),Y    2b, 6c
          {
            ushort    address = Memory.ReadByte( CPU.PC + 1 );
            ushort    finalAddress = CalcIndirectY( address, CPU.Y );

            Memory.WriteByte( finalAddress, CPU.Accu );
            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();

            CPU.PC += 2;
          }
          return 6;
        case 0x94:
          // STY $FF,X      2b, 4c
          {
            ushort    address = Memory.ReadByte( CPU.PC + 1 );
            ushort    finalAddress = CalcZeropageX( address, CPU.X );

            Memory.WriteByte( finalAddress, CPU.Y );

            CPU.PC += 2;
          }
          return 4;
        case 0x95:
          // STA $FF,X      2b, 4c
          {
            ushort    address = Memory.ReadByte( CPU.PC + 1 );
            ushort    finalAddress = CalcZeropageX( address, CPU.X );

            Memory.WriteByte( finalAddress, CPU.Accu );

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
            ushort    address = Memory.ReadWord( CPU.PC + 1 );
            ushort    finalAddress = CalcAbsoluteY( address, CPU.Y );

            Memory.WriteByte( finalAddress, CPU.Accu );

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
            ushort    address = Memory.ReadWord( CPU.PC + 1 );
            ushort    finalAddress = CalcAbsoluteX( address, CPU.X );

            Memory.WriteByte( finalAddress, CPU.Accu );

            CPU.PC += 3;
          }
          return 5;
        case 0xa0:
          // LDY #$FF   2b, 2c
          {
            byte    operand = Memory.ReadByte( CPU.PC + 1 );

            CPU.Y = operand;
            CPU.CheckFlagZeroY();
            CPU.CheckFlagNegativeY();
            CPU.PC += 2;
          }
          return 2;
        case 0xa2:
          // LDX #$FF   2 bytes, 2 cycles
          {
            byte    operand = Memory.ReadByte( CPU.PC + 1 );

            CPU.X = operand;
            CPU.CheckFlagZeroX();
            CPU.CheckFlagNegativeX();
            CPU.PC += 2;
          }
          return 2;
        case 0xa4:
          // LDY $FF   2b, 3c
          {
            ushort  address = Memory.ReadByte( CPU.PC + 1 );

            CPU.Y = Memory.ReadByte( address );
            CPU.CheckFlagZeroY();
            CPU.CheckFlagNegativeY();
            CPU.PC += 2;
          }
          return 3;
        case 0xa5:
          // LDA $FF             2b, 3c
          {
            ushort  address = Memory.ReadByte( CPU.PC + 1 );

            CPU.Accu = Memory.ReadByte( address );
            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();
            CPU.PC += 2;
          }
          return 3;
        case 0xa6:
          // LDX $FF             2b, 3c
          {
            ushort  address = Memory.ReadByte( CPU.PC + 1 );

            CPU.X = Memory.ReadByte( address );
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
            byte    operand = Memory.ReadByte( CPU.PC + 1 );

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
            ushort    address = Memory.ReadWord( CPU.PC + 1 );

            CPU.Y = Memory.ReadByte( address );
            CPU.CheckFlagZeroY();
            CPU.CheckFlagNegativeY();

            CPU.PC += 3;
          }
          return 4;
        case 0xad:
          // LDA $FFFF     3b, 4c
          {
            ushort    address = Memory.ReadWord( CPU.PC + 1 );

            CPU.Accu = Memory.ReadByte( address );
            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();

            CPU.PC += 3;
          }
          return 4;
        case 0xae:
          // LDX $FFFF           3b, 4c
          {
            ushort    address = Memory.ReadWord( CPU.PC + 1 );

            CPU.X = Memory.ReadByte( address );
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

            ushort    address = Memory.ReadByte( CPU.PC + 1 );
            if ( address + CPU.Y >= 0x100 )
            {
              ++numCycles;
            }
            ushort    finalAddress = CalcIndirectY( address, CPU.Y );

            CPU.Accu = Memory.ReadByte( finalAddress );
            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();

            CPU.PC += 2;

            return numCycles;
          }
        case 0xb4:
          // LDY $FF,X      2b, 4c
          {
            ushort    address = Memory.ReadByte( CPU.PC + 1 );
            ushort    finalAddress = CalcZeropageX( address, CPU.X );

            CPU.Y = Memory.ReadByte( finalAddress );
            CPU.CheckFlagZeroY();
            CPU.CheckFlagNegativeY();

            CPU.PC += 2;
          }
          return 4;
        case 0xb5:
          // LDA $FF,X      2b, 4c
          {
            ushort    address = Memory.ReadByte( CPU.PC + 1 );
            ushort    finalAddress = CalcZeropageX( address, CPU.X );

            CPU.Accu = Memory.ReadByte( finalAddress );
            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();

            CPU.PC += 2;
          }
          return 4;
        case 0xb9:
          // LDA $FFFF,Y   3b, 4*c
          {
            int numCycles = 4;
            ushort    address = Memory.ReadWord( CPU.PC + 1 );

            if ( ( address & 0xff00 ) != ( ( address + CPU.Y ) & 0xff00 ) )
            {
              ++numCycles;
            }

            ushort    finalAddress = CalcAbsoluteY( address, CPU.Y );

            CPU.Accu = Memory.ReadByte( finalAddress );
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
            ushort    address = Memory.ReadWord( CPU.PC + 1 );

            if ( ( address & 0xff00 ) != ( ( address + CPU.X ) & 0xff00 ) )
            {
              ++numCycles;
            }

            ushort    finalAddress = CalcAbsoluteX( address, CPU.X );

            CPU.Accu = Memory.ReadByte( finalAddress );
            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();

            CPU.PC += 3;

            return numCycles;
          }
        case 0xc0:
          // CPY #$FF            2b, 2c
          {
            byte  compareWith = Memory.ReadByte( CPU.PC + 1 );

            int   result = CPU.Y - compareWith;

            CPU.FlagZero = ( compareWith == CPU.Y );
            CPU.FlagCarry = ( result >= 0 );
            CPU.FlagNegative = ( ( ( (byte)( result ) ) & 0x80 ) != 0 );

            CPU.PC += 2;
          }
          return 2;
        case 0xc2:
          // NOP #$FF            2b, 2c
          {
            CPU.PC += 2;
          }
          return 2;
        case 0xc4:
          // CPY $FF             2b, 3c
          {
            ushort  address = Memory.ReadByte( CPU.PC + 1 );
            byte  compareWith = Memory.ReadByte( address );

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
            ushort  address = Memory.ReadByte( CPU.PC + 1 );
            byte  compareWith = Memory.ReadByte( address );

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
            ushort  address = Memory.ReadByte( CPU.PC + 1 );
            byte  operand = Memory.ReadByte( address );
            operand = (byte)( operand - 1 );

            Memory.WriteByte( address, operand );

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
            byte  compareWith = Memory.ReadByte( CPU.PC + 1 );
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
        case 0xcd:
          // CMP $FFFF           3b, 4c
          {
            ushort    address = Memory.ReadWord( CPU.PC + 1 );

            byte  compareWith = Memory.ReadByte( address );

            int result = CPU.Accu - compareWith;

            CPU.FlagZero = ( compareWith == CPU.Accu );
            CPU.FlagCarry = ( result >= 0 );
            CPU.FlagNegative = ( ( ( (byte)( result ) ) & 0x80 ) != 0 );

            CPU.PC += 3;
          }
          return 4;
        case 0xd0:
          // BNE $FF    2 bytes, 2* cycles
          return HandleBranch( !CPU.FlagZero );
        case 0xd1:
          // CMP ($FF),Y    2b, 5*c
          {
            int       numCycles = 5;
            ushort    address = Memory.ReadByte( CPU.PC + 1 );
            if ( ( address & 0xff00 ) != ( ( address + CPU.Y ) & 0xff00 ) )
            {
              ++numCycles;
            }

            ushort    finalAddress = CalcIndirectY( address, CPU.Y );

            byte  compareWith = Memory.ReadByte( finalAddress );
            int result = CPU.Accu - compareWith;

            CPU.FlagZero = ( compareWith == CPU.Accu );
            CPU.FlagCarry = ( result >= 0 );
            CPU.FlagNegative = ( ( ( (byte)( result ) ) & 0x80 ) != 0 );

            CPU.PC += 2;

            return numCycles;
          }
        case 0xd8:
          // CLD    1 byte, 2 cycles
          {
            CPU.FlagDecimal = false;

            CPU.PC += 1;
          }
          return 2;
        case 0xdd:
          // CMP $FFFF,X    3 bytes, 4* cycles
          {
            int       numCycles = 4;
            ushort    address = Memory.ReadWord( CPU.PC + 1 );
            ushort    finalAddress = CalcAbsoluteX( address, CPU.X );

            if ( ( address & 0xff00 ) != ( finalAddress & 0xff00 ) )
            {
              ++numCycles;
            }

            byte  compareWith = Memory.ReadByte( finalAddress );
            int result = CPU.Accu - compareWith;

            CPU.FlagZero      = ( compareWith == CPU.Accu );
            CPU.FlagCarry = ( result >= 0 );
            CPU.FlagNegative = ( ( ( (byte)( result ) ) & 0x80 ) != 0 );

            CPU.PC += 3;

            return numCycles;
          }
        case 0xe0:
          // CPX #$FF            2b, 2c
          {
            byte  compareWith = Memory.ReadByte( CPU.PC + 1 );

            int result = CPU.X - compareWith;

            CPU.FlagZero = ( compareWith == CPU.X );
            CPU.FlagCarry = ( result >= 0 );
            CPU.FlagNegative = ( ( ( (byte)( result ) ) & 0x80 ) != 0 );

            CPU.PC += 2;
          }
          return 2;
        case 0xe4:
          // CPX $FF             2b, 3c
          {
            ushort  address = Memory.ReadByte( CPU.PC + 1 );
            byte    compareWith = Memory.ReadByte( address );

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
            ushort  address = Memory.ReadByte( CPU.PC + 1 );
            byte    operand = Memory.ReadByte( address );

            byte    startValue = CPU.Accu;

            if ( !CPU.FlagCarry )
            {
              operand = (byte)( operand + 1 );
            }
            CPU.FlagCarry = ( operand <= CPU.Accu );

            int result = CPU.Accu - operand;

            CPU.Accu = (byte)( CPU.Accu - operand );

            CPU.CheckFlagNegative();
            CPU.CheckFlagZero();
            //CPU.FlagOverflow = ( ( startValue & 0x80 ) != ( CPU.Accu & 0x80 ) );
            CPU.FlagOverflow = ( ( startValue & 0x80 ) == ( result & 0x80 ) );

            CPU.PC += 2;
          }
          return 3;
        case 0xe6:
          // INC $FF             2b, 5c
          {
            ushort  address = Memory.ReadByte( CPU.PC + 1 );
            byte    value = Memory.ReadByte( address );
            value = (byte)( value + 1 );
            Memory.WriteByte( address, value );

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
            byte    operand = Memory.ReadByte( CPU.PC + 1 );

            byte    startValue = CPU.Accu;

            if ( !CPU.FlagCarry )
            {
              operand = (byte)( operand + 1 );
            }
            CPU.FlagCarry = ( operand <= CPU.Accu );

            CPU.Accu = (byte)( CPU.Accu - operand );

            CPU.CheckFlagNegative();
            CPU.CheckFlagZero();
            CPU.FlagOverflow = ( ( startValue & 0x80 ) != ( CPU.Accu & 0x80 ) );

            CPU.PC += 2;
          }
          return 2;
        case 0xec:
          // CPX $FFFF           3b, 4c
          {
            ushort  address = Memory.ReadWord( CPU.PC + 1 );
            byte  compareWith = Memory.ReadByte( address );

            int result = CPU.X - compareWith;

            CPU.FlagZero = ( compareWith == CPU.X );
            CPU.FlagCarry = ( result >= 0 );
            CPU.FlagNegative = ( ( ( (byte)( result ) ) & 0x80 ) != 0 );

            CPU.PC += 3;
          }
          return 4;
        case 0xf0:
          // BEQ $FFFF           2b, 2c
          {
            if ( CPU.FlagZero )
            {
              ushort  newAddress = CalcRelativeAddress();
              CPU.PC = newAddress;
            }
            else
            {
              CPU.PC += 2;
            }
          }
          return 2;
        case 0xf6:
          // INC $FF,X           2b, 6c
          {
            ushort  address = Memory.ReadByte( CPU.PC + 1 );
            ushort  finalAddress = CalcZeropageX( address, CPU.X );
            byte    value = Memory.ReadByte( finalAddress );
            value = (byte)( value + 1 );
            Memory.WriteByte( finalAddress, value );

            CPU.FlagZero = ( value == 0 );
            CPU.FlagNegative = ( ( value & 0x80 ) != 0 );

            CPU.PC += 2;
          }
          return 6;
        default:
          throw new NotSupportedException( "Unsupported opcode " + opCode.ToString( "X2" ) );
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
      byte    hi = 0;
      return (ushort)( lo + ( hi << 8 ) + X );
    }



    private ushort CalcRelativeAddress()
    {
      byte    delta = Memory.ReadByte( CPU.PC + 1 );
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



    private ushort CalcIndirectY( ushort Address, byte Y )
    {
      byte    lo = Memory.ReadByte( Address );
      byte    hi = Memory.ReadByte( ( Address + 1 ) % 256 );
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

  }
}
