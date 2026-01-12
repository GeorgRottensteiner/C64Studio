using GR.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RetroDevStudio.Audio
{
  internal class SFXPlayerDescriptor
  {
    public ByteBuffer             PlayerCode = new ByteBuffer();
    public Audio.SFXPlayer        Player = SFXPlayer.RETRO_DEV_STUDIO_SID;  
    public string                 Name = "";
    public List<ValueDescriptor>  Parameters = new List<ValueDescriptor>();

    public int                    PlayerCodeAddress = -1;
    // start player routine
    public int                    AddressToStartPlayer = -1;

    // trigger a replay of the effect
    public int                    AddressToTriggerPlaying = -1;
    public byte                   ValueToTriggerPlaying = 1;


  }
}
