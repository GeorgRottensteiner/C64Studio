using System.ComponentModel;

namespace RetroDevStudio
{
  public enum MachineType
  {
    [Description( "Unknown" )]
    ANY,
    [Description( "Commodore 64" )]
    C64,
    [Description( "VIC 20" )]
    VIC20,
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
    NES,
    [Description( "Commander X16" )]
    COMMANDER_X16,
    [Description( "Amstrad CPC" )]
    CPC,
    [Description( "Sinclair ZX80" )]
    ZX80,
    [Description( "Sinclair ZX81" )]
    ZX81,
    [Description( "Sinclair ZX Spectrum" )]
    ZX_SPECTRUM,
    [Description( "Commodore Amiga" )]
    AMIGA
  }



}
