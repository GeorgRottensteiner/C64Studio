using System.ComponentModel;

namespace RetroDevStudio
{
  public enum GraphicTileMode
  {
    [Description( "Unknown" )]
    UNKNOWN = -1,
    [Description( "Commodore HiRes" )]
    COMMODORE_HIRES = 0,
    [Description( "Commodore Multicolor Sprites" )]
    COMMODORE_MULTICOLOR_SPRITES,
    [Description( "Commodore Multicolor Chars" )]
    COMMODORE_MULTICOLOR_CHARACTERS,
    [Description( "Commodore ECM" )]
    COMMODORE_ECM,
    [Description( "Mega65 Nibble Color Mode 16 colors" )]
    MEGA65_NCM_CHARACTERS,
    [Description( "Mega65 Sprites 16 colors" )]
    MEGA65_NCM_SPRITES,
    [Description( "Mega65 Full Color Mode 256 colors" )]
    MEGA65_FCM_256_COLORS,
    [Description( "Commander X16 HiRes" )]
    COMMANDERX16_HIRES,
    [Description( "Commander X16 16 colors" )]
    COMMANDERX16_16_COLORS,
    [Description( "Commander X16 256 colors" )]
    COMMANDERX16_256_COLORS,
    [Description( "Commodore 128 VDC HiRes" )]
    COMMODORE_128_VDC_HIRES,
    [Description( "NES" )]
    NES,
    [Description( "Commodore HiRes VIC20 8x16" )]
    COMMODORE_HIRES_8X16,
    [Description( "Commodore Multicolor VIC20 8x16" )]
    COMMODORE_MULTICOLOR_CHARACTERS_8X16
  }

}
