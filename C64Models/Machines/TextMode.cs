using System.ComponentModel;

namespace RetroDevStudioModels
{
  public enum TextMode
  {
    [Description( "Unknown" )]
    UNKNOWN = -1,
    [Description( "Commodore 40x25 HiRes" )]
    COMMODORE_40_X_25_HIRES = 0,
    [Description( "Commodore 40x25 Multicolor" )]
    COMMODORE_40_X_25_MULTICOLOR,
    [Description( "Commodore 40x25 ECM" )]
    COMMODORE_40_X_25_ECM,
    [Description( "Mega 65 80x25 HiRes" )]
    MEGA65_80_X_25_HIRES,
    [Description( "Mega 65 80x25 Multicolor" )]
    MEGA65_80_X_25_MULTICOLOR
  }



}
