using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RetroDevStudio.Audio
{
  internal class ValueDescriptor
  {
    public string     Name = "";

    public int        AddressToWriteTo = 0;
    public int        MinValue = 0;
    public int        MaxValue = 0;
    public byte       Value = 0;

    // how to apply the value in the final byte
    public int        ShiftBitsLeft = 0;
    public int        ShiftBitsRight = 0;
    public int        RelevantBits = 0xff;

    public Dictionary<string,int>     ValidValues = new Dictionary<string, int>();
  }
}
