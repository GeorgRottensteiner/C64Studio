using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace RetroDevStudio.Audio
{
  enum SFXPlayer
  {
    [Type( typeof( SIDPlayer ) )]
    [Description( "RetroDev Studio SID Player (C64)" )]
    [Machine( MachineType.C64 )]
    RETRO_DEV_STUDIO_SID
  }

}
