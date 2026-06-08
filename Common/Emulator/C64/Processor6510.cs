using System.Collections.Generic;
using static Tiny64.Opcode;



namespace Tiny64
{
  public class Processor6510 : Processor
  {
    public byte           Accu = 0;
    public byte           X = 0;
    public byte           Y = 0;
    public byte           Flags = 0;

    public ushort         PC = 0;
    public byte           StackPointer = 0xff;



    public Processor6510() : base( "6510" )
    {
      Accu          = 0;
      X             = 0;
      Y             = 0;
      Flags         = 0;
      PC            = 0;
      StackPointer  = 0xff;

      LittleEndian  = true;
    }



    public bool FlagNegative 
    {
      get
      {
        return ( Flags & 0x80 ) == 0x80;
      }
      set
      {
        if ( value )
        {
          Flags |= 0x80;
        }
        else
        {
          Flags &= 0x7f;
        }
      }
    }



    public bool FlagOverflow
    {
      get
      {
        return ( Flags & 0x40 ) == 0x40;
      }
      set
      {
        if ( value )
        {
          Flags |= 0x40;
        }
        else
        {
          Flags &= 0xbf;
        }
      }
    }



    public bool FlagDecimal
    {
      get
      {
        return ( Flags & 0x08 ) == 0x08;
      }
      set
      {
        if ( value )
        {
          Flags |= 0x08;
        }
        else
        {
          Flags &= 0xf7;
        }
      }
    }



    public bool FlagIRQ
    {
      get
      {
        return ( Flags & 0x04 ) == 0x04;
      }
      set
      {
        if ( value )
        {
          Flags |= 0x04;
        }
        else
        {
          Flags &= 0xfb;
        }
      }
    }



    public bool FlagZero
    {
      get
      {
        return ( Flags & 0x02 ) == 0x02;
      }
      set
      {
        if ( value )
        {
          Flags |= 0x02;
        }
        else
        {
          Flags &= 0xfd;
        }
      }
    }

    public bool FlagCarry
    {
      get
      {
        return ( Flags & 0x01 ) == 0x01;
      }
      set
      {
        if ( value )
        {
          Flags |= 0x01;
        }
        else
        {
          Flags &= 0xfe;
        }
      }
    }



    internal void Initialize()
    {
      Accu          = 0;
      X             = 0;
      Y             = 0;
      Flags         = 0;
      PC            = 0;
      StackPointer  = 0xff;
    }




    internal void CheckFlagZero()
    {
      FlagZero = ( Accu == 0 );
    }



    internal void CheckFlagNegative()
    {
      FlagNegative = ( ( Accu & 0x80 ) != 0 );
    }



    internal void CheckFlagZeroY()
    {
      FlagZero = ( Y == 0 );
    }



    internal void CheckFlagNegativeY()
    {
      FlagNegative = ( ( Y & 0x80 ) != 0 );
    }



    internal void CheckFlagZeroX()
    {
      FlagZero = ( X == 0 );
    }



    internal void CheckFlagNegativeX()
    {
      FlagNegative = ( ( X & 0x80 ) != 0 );
    }



  }
}
