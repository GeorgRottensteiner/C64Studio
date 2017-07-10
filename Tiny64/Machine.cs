using System;
using System.Collections.Generic;
using System.Text;
using Tiny64.data;



namespace Tiny64
{
  public class Machine
  {
    Memory          Memory = new Memory();
    Processor       CPU = new Processor();

    byte            IODirection = 0x2f;   // RAM 0000
    byte            PortRegister = 55;    // RAM 0001

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

      IODirection = 0x2f;
      PortRegister = 55;

      //Beim Hard-Reset startet die CPU bei der in $FFFC/$FFFD abgelegten Adresse (springt nach $FCE2, RESET
      CPU.PC = Memory.ReadWordDirect( 0xfffc );
    }



    public void RunCycle()
    {
      // TODO - split opcodes for number of cycles!!
      byte    opCode = Memory.ReadByte( CPU.PC );

      switch ( opCode )
      {
        case 0x02:
          // JAM        1b, ?c
          {
            throw new NotSupportedException( "JAM" );
          }
          break;
        case 0x09:
          // ORA #$FF   2b, 2c
          {
            byte    operand = Memory.ReadByte( CPU.PC + 1 );

            CPU.Accu = (byte)( CPU.Accu | operand );

            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();
            CPU.PC += 2;
          }
          break;
        case 0x10:
          // BPL $FFFF           2b, 2*c
          {
            if ( !CPU.FlagNegative )
            {
              CPU.PC = CalcRelativeAddress();
            }
            else
            {
              CPU.PC += 2;
            }
          }
          break;
        case 0x18:
          // CLC        1b, 2c
          {
            CPU.FlagCarry = false;

            CPU.PC += 1;
          }
          break;
        case 0x20:
          // JSR        3 bytes, 6 cycles
          {
            ushort    address = Memory.ReadWord( CPU.PC + 1 );

            ushort    returnAddress = (ushort)( CPU.PC + 2 );

            PushStack( (byte)( returnAddress & 0xff ) );
            PushStack( (byte)( ( returnAddress >> 8 ) & 0xff ) );

            CPU.PC = address;
          }
          break;
        case 0x29:
          // AND #$FF            2b, 2c
          {
            byte    operand = Memory.ReadByte( CPU.PC + 1 );

            CPU.Accu = (byte)( CPU.Accu & operand );

            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();
            CPU.PC += 2;
          }
          break;
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
          break;
        case 0x4c:
          // JMP $FFFF           3b, 3c
          {
            ushort    address = Memory.ReadWord( CPU.PC + 1 );

            CPU.PC = address;
          }
          break;
        case 0x60:
          // RTS        1 byte, 6 cycles
          {
            // PC from Stack, PC + 1 -> PC               
            ushort    returnAddress = (ushort)( PopStack() << 8 );
            returnAddress = (ushort)( returnAddress | PopStack() );

            CPU.PC = (ushort)( returnAddress + 1 );
          }
          break;
        case 0x69:
          // ADC #$FF            2b, 2c
          {
            byte    operand = Memory.ReadByte( CPU.PC + 1 );

            byte    startValue = CPU.Accu;
            CPU.Accu = (byte)( CPU.Accu + operand );
            if ( CPU.Accu < startValue )
            {
              CPU.FlagOverflow = true;
            }
            if ( CPU.FlagCarry )
            {
              CPU.Accu = (byte)( CPU.Accu + 1 );
            }
            CPU.CheckFlagNegative();
            CPU.CheckFlagZero();
            CPU.FlagOverflow = ( startValue & 0x80 ) != ( CPU.Accu & 0x80 );

            CPU.PC += 2;
          }
          break;
        case 0x78:
          // SEI        1 byte, 2 cycles
          {
            CPU.FlagIRQ = true;
            CPU.PC += 1;
          }
          break;
        case 0x84:
          // STY $FF             2b, 3c
          {
            ushort    address = Memory.ReadByte( CPU.PC + 1 );

            Memory.WriteByte( address, CPU.Y );

            CPU.PC += 2;
          }
          break;
        case 0x85:
          // STA $FF             2b 3c
          {
            ushort    address = Memory.ReadByte( CPU.PC + 1 );

            Memory.WriteByte( address, CPU.Accu );

            CPU.PC += 2;
          }
          break;
        case 0x86:
          // STX $FF             2b, 3c
          {
            ushort    address = Memory.ReadByte( CPU.PC + 1 );

            Memory.WriteByte( address, CPU.X );

            CPU.PC += 2;
          }
          break;
        case 0x88:
          // DEY                  1b, 2c
          {
            CPU.Y = (byte)( CPU.Y - 1 );

            CPU.FlagZero = ( CPU.Y == 0 );
            CPU.FlagNegative = ( ( CPU.Y & 0x80 ) != 0 );

            CPU.PC += 1;
          }
          break;
        case 0x8a:
          // TXA                  1b, 2c
          {
            CPU.Accu = CPU.X;

            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();

            CPU.PC += 1;
          }
          break;
        case 0x8c:
          // STY $FFFF           3b, 4c
          {
            ushort    address = Memory.ReadWord( CPU.PC + 1 );

            Memory.WriteByte( address, CPU.Y );

            CPU.PC += 3;
          }
          break;
        case 0x8d:
          // STA $FFFF           3b 4c
          {
            ushort    address = Memory.ReadWord( CPU.PC + 1 );

            Memory.WriteByte( address, CPU.Accu );

            CPU.PC += 3;
          }
          break;
        case 0x8e:
          {
            // STX $FFFF             3 bytes, 4 cycles
            ushort    address = Memory.ReadWord( CPU.PC + 1 );

            Memory.WriteByte( address, CPU.X );

            CPU.PC += 3;
          }
          break;
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
          break;
        case 0x94:
          // STY $FF,X      2b, 4c
          {
            ushort    address = Memory.ReadByte( CPU.PC + 1 );
            ushort    finalAddress = CalcZeropageX( address, CPU.X );

            Memory.WriteByte( finalAddress, CPU.Y );

            CPU.PC += 2;
          }
          break;
        case 0x98:
          // TYA            1b, 2c
          {
            CPU.Accu = CPU.Y;

            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();

            CPU.PC += 1;
            break;
          }
        case 0x99:
          // STA $FFFF,Y    3b, 5c
          {
            ushort    address = Memory.ReadWord( CPU.PC + 1 );
            ushort    finalAddress = CalcAbsoluteY( address, CPU.Y );

            Memory.WriteByte( finalAddress, CPU.Accu );

            CPU.PC += 3;
          }
          break;
        case 0x9a:
          // TXS        1 byte, 2 cycles
          {
            PushStack( CPU.X );

            CPU.PC += 1;
          }
          break;
        case 0x9d:
          // STA $FFFF,X    3b, 5c
          {
            ushort    address = Memory.ReadWord( CPU.PC + 1 );
            ushort    finalAddress = CalcAbsoluteX( address, CPU.X );

            Memory.WriteByte( finalAddress, CPU.Accu );

            CPU.PC += 3;
          }
          break;
        case 0xa0:
          // LDY #$FF   2b, 2c
          {
            byte    operand = Memory.ReadByte( CPU.PC + 1 );

            CPU.Y = operand;
            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();
            CPU.PC += 2;
          }
          break;
        case 0xa2:
          // LDX #$FF   2 bytes, 2 cycles
          {
            byte    operand = Memory.ReadByte( CPU.PC + 1 );

            CPU.X = operand;
            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();
            CPU.PC += 2;
          }
          break;
        case 0xa4:
          // LDY $FF   2b, 3c
          {
            ushort  address = Memory.ReadByte( CPU.PC + 1 );

            CPU.Y = Memory.ReadByte( address );
            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();
            CPU.PC += 2;
          }
          break;
        case 0xa8:
          // TAY       1b, 2c
          {
            CPU.Y = CPU.Accu;
            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();
            CPU.PC += 1;
          }
          break;
        case 0xa9:
          // LDA #$FF  2b, 2c
          {
            byte    operand = Memory.ReadByte( CPU.PC + 1 );

            CPU.Accu = operand;
            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();
            CPU.PC += 2;
          }
          break;
        case 0xaa:
          // TAX            1b, 2c
          {
            CPU.X = CPU.Accu;

            CPU.FlagZero = ( CPU.X == 0 );
            CPU.FlagNegative = ( ( CPU.X & 0x80 ) != 0 );

            CPU.PC += 1;
          }
          break;
        case 0xad:
          // LDA $FFFF     3b, 4c
          {
            ushort    address = Memory.ReadWord( CPU.PC + 1 );

            CPU.Accu = Memory.ReadByte( address );
            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();

            CPU.PC += 3;
          }
          break;
        case 0xb0:
          // BCS $FF        2b, 2*
          {
            if ( CPU.FlagCarry )
            {
              ushort  newAddress = CalcRelativeAddress();

              CPU.PC = newAddress;
            }
            else
            {
              CPU.PC += 2;
            }
          }
          break;
        case 0xb1:
          // LDA ($FF),Y    2b, 5*c
          {
            ushort    address = Memory.ReadByte( CPU.PC + 1 );
            ushort    finalAddress = CalcIndirectY( address, CPU.Y );

            CPU.Accu = Memory.ReadByte( finalAddress );
            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();

            CPU.PC += 2;
          }
          break;
        case 0xb9:
          // LDA $FFFF,Y   3b, 4*c
          {
            ushort    address = Memory.ReadWord( CPU.PC + 1 );
            ushort    finalAddress = CalcAbsoluteY( address, CPU.Y );

            CPU.Accu = Memory.ReadByte( finalAddress );
            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();

            CPU.PC += 3;
          }
          break;
        case 0xbd:
          // LDA $FFFF,X   3 bytes, 4* cycles
          {
            ushort    address = Memory.ReadWord( CPU.PC + 1 );
            ushort    finalAddress = CalcAbsoluteX( address, CPU.X );

            CPU.Accu = Memory.ReadByte( finalAddress );
            CPU.CheckFlagZero();
            CPU.CheckFlagNegative();

            CPU.PC += 3;
          }
          break;
        case 0xc2:
          // NOP #$FF            2b, 2c
          {
            CPU.PC += 2;
          }
          break;
        case 0xc8:
          // INY        1b, 2c
          {
            CPU.Y = (byte)( CPU.Y + 1 );

            CPU.FlagZero = ( CPU.Y == 0 );
            CPU.FlagNegative = ( ( CPU.Y & 0x80 ) != 0 );

            CPU.PC += 1;
          }
          break;
        case 0xca:
          // DEX        1b 2c
          {
            CPU.X = (byte)( CPU.X - 1 );

            CPU.FlagZero      = ( CPU.X == 0 );
            CPU.FlagNegative  = ( ( CPU.X & 0x80 ) != 0 );

            CPU.PC += 1;
          }
          break;
        case 0xd0:
          // BNE $FF    2 bytes, 2* cycles
          {
            if ( !CPU.FlagZero )
            {
              ushort  newAddress = CalcRelativeAddress();
              CPU.PC = newAddress;
            }
            else
            {
              CPU.PC += 2;
            }
          }
          break;
        case 0xd1:
          // CMP ($FF),Y    2b, 5*c
          {
            ushort    address = Memory.ReadByte( CPU.PC + 1 );
            ushort    finalAddress = CalcIndirectY( address, CPU.Y );

            byte  compareWith = Memory.ReadByte( finalAddress );

            CPU.FlagZero = ( compareWith == CPU.Accu );
            CPU.FlagCarry = ( compareWith >= CPU.Accu );
            CPU.FlagNegative = ( ( ( compareWith - CPU.Accu ) & 0x80 ) != 0 );

            CPU.PC += 2;
          }
          break;
        case 0xd8:
          // CLD    1 byte, 2 cycles
          {
            CPU.FlagDecimal = false;

            CPU.PC += 1;
          }
          break;
        case 0xdd:
          // CMP $FFFF,X    3 bytes, 4* cycles
          {
            ushort    address = Memory.ReadWord( CPU.PC + 1 );
            ushort    finalAddress = CalcAbsoluteX( address, CPU.X );

            byte  compareWith = Memory.ReadByte( finalAddress );

            CPU.FlagZero      = ( compareWith == CPU.Accu );
            CPU.FlagCarry     = ( compareWith >= CPU.Accu );
            CPU.FlagNegative  = ( ( ( compareWith - CPU.Accu ) & 0x80 ) != 0 );

            CPU.PC += 3;
          }
          break;
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
          break;
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
          break;
        default:
          throw new NotSupportedException( "Unsupported opcode " + opCode.ToString( "X2" ) );
      }
    }



    private ushort CalcZeropageX( ushort Address, byte X )
    {
      byte    lo = Memory.ReadByte( Address );
      byte    hi = Memory.ReadByte( ( Address + 1 ) % 256 );
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
    }



    private byte PopStack()
    {
      CPU.StackPointer = (byte)( CPU.StackPointer + 1 );
      return Memory.RAM[0x100 + CPU.StackPointer];
    }

  }
}
