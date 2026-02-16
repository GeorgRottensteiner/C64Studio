using GR.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RetroDevStudio.Audio
{
  public class SFXPlayerDescriptor
  {
    public MachineType            Machine = MachineType.C64;

    public string                 PlayerCodeAssembly = "";
    public ByteBuffer             PlayerCode = new ByteBuffer();
    public string                 Name = "";
    public List<ValueDescriptor>  Parameters = new List<ValueDescriptor>();

    public int                    PlayerCodeAddress = -1;
    // start player routine
    public int                    AddressToStartPlayer = -1;

    // trigger a replay of the effect
    public int                    AddressToTriggerPlaying = -1;
    public byte                   ValueToTriggerPlaying = 1;

    public bool                   CanReplay = true; 


  }
}
