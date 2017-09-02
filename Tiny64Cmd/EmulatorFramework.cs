using System;
using System.Collections.Generic;
using System.Text;
using Tiny64;

namespace Tiny64Cmd
{
  class EmulatorFramework
  {
    Emulator        m_Emulator = new Emulator();
    bool            m_EmulatorIsRunning = true;



    public int Run()
    {
      m_Emulator.AddBreakpoint( 0xA47D, false, false, true );

      while ( m_EmulatorIsRunning )
      {
        if ( m_Emulator.State == EmulatorState.RUNNING )
        {
          m_Emulator.Run();
        }

        // we returned from emulator
        if ( m_Emulator.State != EmulatorState.RUNNING )
        {
          EmulatorMenu();
        }
      }

      return 0;
    }



    private string FlagByteToString( byte Flags )
    {
      StringBuilder   sb = new StringBuilder();

      if ( ( Flags & 0x80 ) != 0 )
      {
        sb.Append( 'N' );
      }
      else
      {
        sb.Append( '.' );
      }
      if ( ( Flags & 0x40 ) != 0 )
      {
        sb.Append( 'V' );
      }
      else
      {
        sb.Append( '.' );
      }
      sb.Append( '-' );
      if ( ( Flags & 0x10 ) != 0 )
      {
        sb.Append( 'B' );
      }
      else
      {
        sb.Append( '.' );
      }
      if ( ( Flags & 0x08 ) != 0 )
      {
        sb.Append( 'D' );
      }
      else
      {
        sb.Append( '.' );
      }
      if ( ( Flags & 0x04 ) != 0 )
      {
        sb.Append( 'I' );
      }
      else
      {
        sb.Append( '.' );
      }
      if ( ( Flags & 0x02 ) != 0 )
      {
        sb.Append( 'Z' );
      }
      else
      {
        sb.Append( '.' );
      }
      if ( ( Flags & 0x01 ) != 0 )
      {
        sb.Append( 'C' );
      }
      else
      {
        sb.Append( '.' );
      }
      return sb.ToString();
    }



    public void EmulatorMenu()
    {
      StringBuilder     sb = new StringBuilder();

      sb.Append( m_Emulator.Machine.CPU.PC.ToString( "X4" ) );
      sb.Append( ": " );
      sb.Append( Disassembler.DisassembleMnemonicToString( m_Emulator.Machine.CPU.OpcodeByValue[m_Emulator.Machine.Memory.ReadByteDirect( m_Emulator.Machine.CPU.PC )],
                      m_Emulator.Machine.Memory,
                      m_Emulator.Machine.CPU.PC,
                      new GR.Collections.Set<ushort>(),
                      new GR.Collections.Map<int, string>() ) );
      if ( sb.Length < 18 )
      {
        sb.Append( ' ', 18 - sb.Length );
      }
      sb.Append( "A:" );
      sb.Append( m_Emulator.Machine.CPU.Accu.ToString( "X2" ) );
      sb.Append( " X:" );
      sb.Append( m_Emulator.Machine.CPU.X.ToString( "X2" ) );
      sb.Append( " Y:" );
      sb.Append( m_Emulator.Machine.CPU.Y.ToString( "X2" ) );
      sb.Append( " SP:" );
      sb.Append( m_Emulator.Machine.CPU.StackPointer.ToString( "X2" ) );
      sb.Append( " F:" );
      sb.Append( FlagByteToString( m_Emulator.Machine.CPU.Flags ) );
      sb.Append( " CYC:" );
      sb.Append( m_Emulator.Machine.TotalCycles );

      System.Console.WriteLine( sb.ToString() );

      System.Console.WriteLine( "s step over   d step into    Esc quit" );
      var key = System.Console.ReadKey( true );

      switch ( key.Key )
      {
        case ConsoleKey.Escape:
          m_EmulatorIsRunning = false;
          break;
        case ConsoleKey.S:
          m_Emulator.StepOver();
          break;
        case ConsoleKey.D:
          m_Emulator.StepInto();
          break;
      }
    }
  }
}
