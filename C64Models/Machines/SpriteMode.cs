using System.ComponentModel;

namespace RetroDevStudioModels
{
  public enum SpriteMode
  {
    [Description( "Unknown" )]
    UNKNOWN = -1,
    [Description( "Commodore 24x21 HiRes" )]
    COMMODORE_24_X_21_HIRES = 0,
    [Description( "Commodore 24x21 Multicolor" )]
    COMMODORE_24_X_21_MULTICOLOR,
    [Description( "Mega65 16x21 16 colors" )]
    MEGA65_16_X_21_16_COLORS
  }



}
