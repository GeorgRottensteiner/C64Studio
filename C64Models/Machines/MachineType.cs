using System.ComponentModel;

namespace RetroDevStudio
{
  public enum MachineType
  {
    [Description( "Unknown" )]
    UNKNOWN,
    [Description( "Commodore 64" )]
    C64,
    [Description( "VC 20" )]
    VC20,
    [Description( "Commodore 128" )]
    C128,
    [Description( "Commodore Plus 4" )]
    PLUS4,
    [Description( "Commodore PET" )]
    PET,
    [Description( "Commodore CBM" )]
    CBM,
    [Description( "Atari 2600" )]
    ATARI2600,
    [Description( "Mega 65" )]
    MEGA65,
    [Description( "Nintendo NES" )]
    NES
  }



}
