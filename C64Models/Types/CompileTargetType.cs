using System.ComponentModel;

namespace RetroDevStudio.Types
{
  public enum CompileTargetType
  {
    [Description( "None" )]
    NONE,
    [Description( "Plain" )]
    PLAIN,
    [Description( "PRG (CBM)" )]
    PRG,
    [Description( "T64 Tape Image" )]
    T64,
    [Description( "8k Cartridge Binary" )]
    CARTRIDGE_8K_BIN,
    [Description( "8k Cartridge CRT" )]
    CARTRIDGE_8K_CRT,
    [Description( "16k Cartridge Binary" )]
    CARTRIDGE_16K_BIN,
    [Description( "16k Cartridge CRT" )]
    CARTRIDGE_16K_CRT,
    [Description( "D64 Disk Image" )]
    [AdditionalArgumentCount( 2, 0 )]
    D64,
    [Description( "Magic Desk Cartridge 64k Binary" )]
    CARTRIDGE_MAGICDESK_BIN_64K,
    [Description( "Magic Desk Cartridge 64k CRT" )]
    CARTRIDGE_MAGICDESK_CRT_64K,
    [Description( "TAP Tape Image" )]
    TAP,
    [Description( "Easyflash Cartridge Binary" )]
    CARTRIDGE_EASYFLASH_BIN,
    [Description( "Easyflash Cartridge CRT" )]
    CARTRIDGE_EASYFLASH_CRT,
    [Description( "RGCD Cartridge Binary" )]
    CARTRIDGE_RGCD_BIN,
    [Description( "RGCD Cartridge CRT" )]
    CARTRIDGE_RGCD_CRT,
    [Description( "GMOD2 Cartridge Binary" )]
    CARTRIDGE_GMOD2_BIN,
    [Description( "GMOD2 Cartridge CRT" )]
    CARTRIDGE_GMOD2_CRT,
    [Description( "D81 Disk Image" )]
    [AdditionalArgumentCount( 2, 0 )]
    D81,
    [Description( "Ultimax Cartridge 4k Binary" )]
    CARTRIDGE_ULTIMAX_4K_BIN,
    [Description( "Ultimax Cartridge 4k CRT" )]
    CARTRIDGE_ULTIMAX_4K_CRT,
    [Description( "Ultimax Cartridge 8k Binary" )]
    CARTRIDGE_ULTIMAX_8K_BIN,
    [Description( "Ultimax Cartridge 8k CRT" )]
    CARTRIDGE_ULTIMAX_8K_CRT,
    [Description( "Ultimax Cartridge 16k Binary" )]
    CARTRIDGE_ULTIMAX_16K_BIN,
    [Description( "Ultimax Cartridge 16k CRT" )]
    CARTRIDGE_ULTIMAX_16K_CRT,
    [Description( "Magic Desk Cartridge 32k Binary" )]
    CARTRIDGE_MAGICDESK_BIN_32K,
    [Description( "Magic Desk Cartridge 32k CRT" )]
    CARTRIDGE_MAGICDESK_CRT_32K,
    [Description( "Magic Desk Cartridge 128k Binary" )]
    CARTRIDGE_MAGICDESK_BIN_128K,
    [Description( "Magic Desk Cartridge 128k CRT" )]
    CARTRIDGE_MAGICDESK_CRT_128K,
    [Description( "Magic Desk Cartridge 256k Binary" )]
    CARTRIDGE_MAGICDESK_BIN_256K,
    [Description( "Magic Desk Cartridge 256k CRT" )]
    CARTRIDGE_MAGICDESK_CRT_256K,
    [Description( "Magic Desk Cartridge 512k Binary" )]
    CARTRIDGE_MAGICDESK_BIN_512K,
    [Description( "Magic Desk Cartridge 512k CRT" )]
    CARTRIDGE_MAGICDESK_CRT_512K,
    [Description( "Magic Desk Cartridge 1M Binary" )]
    CARTRIDGE_MAGICDESK_BIN_1M,
    [Description( "Magic Desk Cartridge 1M CRT" )]
    CARTRIDGE_MAGICDESK_CRT_1M,
    [Description( "CPC DSK Disk Image" )]
    DSK,
    [Description( "NES Cartridge (iNES)" )]
    [AdditionalArgumentCount( 0, 4 )]
    CARTRIDGE_NES,
    [Description( "P (ZX81)" )]
    P_ZX81
  }



}