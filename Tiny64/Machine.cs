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
        case 0x78:
          // SEI        1 byte, 2 cycles
          {
            CPU.FlagIRQ = true;
            CPU.PC += 1;
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
        case 0x9a:
          // TXS        1 byte, 2 cycles
          {
            PushStack( CPU.X );

            CPU.PC += 1;
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
              byte    delta = Memory.ReadByte( CPU.PC + 1 );
              ushort  newAddress = (ushort)( CPU.PC + 2 );
              if ( ( delta & 0x80 ) != 0 )
              {
                newAddress = (ushort)( newAddress - ( 255 - delta ) );
              }
              else
              {
                newAddress += delta;
              }
              CPU.PC = newAddress;
            }
            else
            {
              CPU.PC += 2;
            }
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
        case 0xf0:
          // BEQ $FFFF           2b, 2c
          {
            if ( CPU.FlagZero )
            {
              byte    delta = Memory.ReadByte( CPU.PC + 1 );
              ushort  newAddress = (ushort)( CPU.PC + 2 );
              if ( ( delta & 0x80 ) != 0 )
              {
                newAddress = (ushort)( newAddress - ( 255 - delta ) );
              }
              else
              {
                newAddress += delta;
              }
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
