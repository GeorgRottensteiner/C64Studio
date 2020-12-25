using System;
using System.Collections.Generic;
using System.Text;

namespace Tiny64
{
  public class CIA
  {
    public static class Register
    {
      public const int PRA_DATA_PORT_A      = 0;
      public const int PRB_DATA_PORT_B      = 1;
      public const int DDRA_DATA_DIR_PORT_A = 2;
      public const int DDRB_DATA_DIR_PORT_B = 3;
      public const int TIMER_A_LO           = 4;
      public const int TIMER_A_HI           = 5;
      public const int TIMER_B_LO           = 6;
      public const int TIMER_B_HI           = 7;
      public const int TOD_10TH_SECONDS     = 8;
      public const int TOD_SECONDS          = 9;
      public const int TOD_MINUTES          = 10;
      public const int TOD_HOURS            = 11;
      public const int SDR                  = 12;
      public const int IRQ_CONTROL_STATE    = 13;
      public const int TIMER_A_CONTROL      = 14;
      public const int TIMER_B_CONTROL      = 15;
    }

    public enum IRQSource
    {
      TIMER_A           = 1,
      TIMER_B           = 2,
      TIME_OF_DAY       = 4,
      SERIAL_REGISTER   = 8,
      FLAG              = 16
    };

    protected byte[]        Registers = new byte[16];

    protected int           TimerAValue = 0;
    protected int           TimerALatch = 0;

    protected int           TimerBValue = 0;
    protected int           TimerBLatch = 0;



    protected bool TimerAActive
    {
      get
      {
        return ( Registers[Register.TIMER_A_CONTROL] & 0x01 ) != 0;
      }
      set
      {
        Registers[Register.TIMER_A_CONTROL] |= 0x01;
      }
    }



    protected bool TimerAModeContinuous
    {
      get
      {
        return ( Registers[Register.TIMER_A_CONTROL] & 0x08 ) == 0;
      }
      set
      {
        Registers[Register.TIMER_A_CONTROL] |= 0x08;
      }
    }



    protected bool TimerAInputModeCountCycles
    {
      get
      {
        return ( Registers[Register.TIMER_A_CONTROL] & 0x20 ) == 0;
      }
      set
      {
        Registers[Register.TIMER_A_CONTROL] |= 0x20;
      }
    }



    protected bool TimerBActive
    {
      get
      {
        return ( Registers[Register.TIMER_B_CONTROL] & 0x01 ) != 0;
      }
      set
      {
        Registers[Register.TIMER_B_CONTROL] |= 0x01;
      }
    }



    protected bool TimerBModeContinuous
    {
      get
      {
        return ( Registers[Register.TIMER_B_CONTROL] & 0x08 ) != 0;
      }
      set
      {
        Registers[Register.TIMER_B_CONTROL] |= 0x08;
      }
    }



    protected byte TimerBInputMode
    {
      get
      {
        return (byte)( ( Registers[Register.TIMER_B_CONTROL] & 0x60 ) >> 5 );
      }
      set
      {
        Registers[Register.TIMER_B_CONTROL] &= 0x9f;  // ~0x60
        Registers[Register.TIMER_B_CONTROL] |= (byte)( ( value & 0x03 ) << 5 );
      }
    }
    
    
    
    public void Init()
    {
      Registers = new byte[16];
    }



    public void RunCycle()
    {
      if ( TimerAActive )
      {
        if ( TimerAInputModeCountCycles )
        {
          --TimerAValue;
        }
        if ( TimerAValue == 0 )
        {
          TimerAValue = TimerALatch;
          if ( !TimerAModeContinuous )
          {
            TimerAActive = false;
          }

          RaiseIRQ( IRQSource.TIMER_A );

          // underflow, trigger timer B?
          if ( ( TimerBActive )
          &&   ( TimerBInputMode == 0x02 ) )
          {
            RaiseIRQ( IRQSource.TIMER_B );
          }
        }
      }

      if ( TimerBActive )
      {
        switch ( TimerBInputMode )
        {
          case 0x00:
            // per cycle
            --TimerBValue;
            break;
          case 0x01:
            // positive edge of CNT pin
            break;
          case 0x02:
            // underflow of timer A
            break;
          case 0x03:
            // underflow of timer A when positive edge of CNT pin
            break;
        }
        if ( TimerBValue == 0 )
        {
          TimerBValue = TimerBLatch;
          if ( !TimerBModeContinuous )
          {
            TimerBActive = false;
          }
          RaiseIRQ( IRQSource.TIMER_B );
        }
      }
    }



    private void RaiseIRQ( IRQSource Source )
    {
      Registers[Register.IRQ_CONTROL_STATE] |= (byte)Source;

      // TODO - only if IRQ flag is enabled!
      // set the IRQ flag
      if ( ( Registers[Register.IRQ_CONTROL_STATE] & 0x80 ) == 0 )
      {
        Registers[Register.IRQ_CONTROL_STATE] |= 0x80;
      }
    }



    public bool IsIRQRaised
    {
      get
      {
        return ( Registers[Register.IRQ_CONTROL_STATE] & 0x80 ) != 0;
      }
    }
    
    
    
    public void WriteByte( byte Address, byte Value )
    {
      Address &= 0x0f;

      switch ( Address )
      {
        case Register.TIMER_A_LO:
          TimerALatch = ( TimerALatch & 0xff00 ) | Value;
          return;
        case Register.TIMER_A_HI:
          TimerALatch = ( TimerALatch & 0x00ff ) | ( Value << 8 );
          return;
        case Register.TIMER_B_LO:
          TimerBLatch = ( TimerBLatch & 0xff00 ) | Value;
          return;
        case Register.TIMER_B_HI:
          TimerBLatch = ( TimerBLatch & 0x00ff ) | ( Value << 8 );
          return;
        case Register.TIMER_A_CONTROL:
          // Bit 0:  Start Timer A (1=start, 0=stop)
          // Bit 1:  Select Timer A output on Port B (1=Timer A output appears on Bit 6 of
          //         Port B)
          // Bit 2:  Port B output mode (1=toggle Bit 6, 0=pulse Bit 6 for one cycle)
          // Bit 3:  Timer A run mode (1=one-shot, 0=continuous)
          // Bit 4:  Force latched value to be loaded to Timer A counter (1=force load
          //         strobe)
          // Bit 5:  Timer A input mode (0=count microprocessor cycles, 1=count signals on
          //         CNT line at pin 4 of User Port)
          // Bit 6:  Serial Port (56332, $DC0C) mode (1=output, 0=input)
          // Bit 7:  Time of Day Clock frequency (1=50 Hz required on TOD pin, 0=60 Hz)
          //Debug.Log( "TIMER A set to " + Value.ToString( "X2" ) );
          TimerAActive = ( ( Value & 0x01 ) != 0 );
          if ( ( Value & 0x10 ) != 0 )
          {
            TimerAValue = TimerALatch;
          }
          TimerAInputModeCountCycles  = ( ( Value & 0x20 ) == 0 );
          TimerAModeContinuous        = ( ( Value & 0x08 ) == 0 );
          break;
        case Register.TIMER_B_CONTROL:
          // Bit 0:  Start Timer B (1=start, 0=stop)
          // Bit 1:  Select Timer B output on Port B (1=Timer B output appears on
          //         Bit 7 of Port B)
          // Bit 2:  Port B output mode (1=toggle Bit 7, 0=pulse Bit 7 for one
          //         cycle)
          // Bit 3:  Timer B run mode (1=one-shot, 0=continuous)
          // Bit 4:  Force latched value to be loaded to Timer B counter (1=force
          //         load strobe)
          // Bits 5-6:  Timer B input mode
          //            00 = Timer B counts microprocessor cycles
          //            01 = Count signals on CNT line at pin 4 of User Port
          //            10 = Count each time that Timer A counts down to 0
          //            11 = Count Timer A 0's when CNT pulses are also present
          // Bit 7:  Select Time of Day write (0=writing to TOD registers sets
          //         alarm, 1=writing to TOD registers sets clock)
          //Debug.Log( "TIMER B set to " + Value.ToString( "X2" ) );
          TimerBActive = ( ( Value & 0x01 ) != 0 );
          if ( ( Value & 0x10 ) != 0 )
          {
            TimerBValue = TimerBLatch;
          }
          TimerBInputMode             = (byte)( ( Value & 0x60 ) >> 5 );
          TimerBModeContinuous        = ( ( Value & 0x08 ) == 0 );
          break;
      }

      Registers[Address] = Value;
    }



    public byte ReadByte( byte Address )
    {
      Address &= 0x0f;

      switch ( Address )
      {
        case Register.TIMER_A_LO:
          return (byte)( TimerAValue & 0xff );
        case Register.TIMER_A_HI:
          return (byte)( TimerAValue >> 8 );
        case Register.TIMER_B_LO:
          return (byte)( TimerBValue & 0xff );
        case Register.TIMER_B_HI:
          return (byte)( TimerBValue >> 8 );
        case Register.IRQ_CONTROL_STATE:
          {
            byte returnValue = Registers[Address];

            Registers[Address] = 0;

            return returnValue;
          }
      }

      return Registers[Address];
    }



  }
}
