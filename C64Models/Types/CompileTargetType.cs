using System.ComponentModel;

namespace RetroDevStudio.Types
{
  public enum CompileTargetType
  {
    [Description( "None" )]
    NONE,
    [Description( "Plain" )]
    [DefaultFileExtension( ".bin" )]
    PLAIN,
    [Description( "PRG (CBM)" )]
    [DefaultFileExtension( ".prg" )]
    PRG,
    [Description( "T64 Tape Image" )]
    [DefaultFileExtension( ".t64" )]
    T64,
    [Description( "8k Cartridge Binary" )]
    [DefaultFileExtension( ".bin" )]
    CARTRIDGE_8K_BIN,
    [Description( "8k Cartridge CRT" )]
    [DefaultFileExtension( ".crt" )]
    CARTRIDGE_8K_CRT,
    [Description( "16k Cartridge Binary" )]
    [DefaultFileExtension( ".bin" )]
    CARTRIDGE_16K_BIN,
    [Description( "16k Cartridge CRT" )]
    [DefaultFileExtension( ".crt" )]
    CARTRIDGE_16K_CRT,
    [Description( "D64 Disk Image" )]
    [AdditionalArgumentCount( 2, 0 )]
    [DefaultFileExtension( ".d64" )]
    D64,
    [Description( "Magic Desk Cartridge 64k Binary" )]
    [DefaultFileExtension( ".bin" )]
    CARTRIDGE_MAGICDESK_BIN_64K,
    [Description( "Magic Desk Cartridge 64k CRT" )]
    [DefaultFileExtension( ".crt" )]
    CARTRIDGE_MAGICDESK_CRT_64K,
    [Description( "TAP Tape Image" )]
    [DefaultFileExtension( ".tap" )]
    TAP,
    [Description( "Easyflash Cartridge Binary" )]
    [DefaultFileExtension( ".bin" )]
    CARTRIDGE_EASYFLASH_BIN,
    [Description( "Easyflash Cartridge CRT" )]
    [DefaultFileExtension( ".crt" )]
    CARTRIDGE_EASYFLASH_CRT,
    [Description( "RGCD Cartridge Binary" )]
    [DefaultFileExtension( ".bin" )]
    CARTRIDGE_RGCD_BIN,
    [Description( "RGCD Cartridge CRT" )]
    [DefaultFileExtension( ".crt" )]
    CARTRIDGE_RGCD_CRT,
    [Description( "GMOD2 Cartridge Binary" )]
    [DefaultFileExtension( ".bin" )]
    CARTRIDGE_GMOD2_BIN,
    [Description( "GMOD2 Cartridge CRT" )]
    [DefaultFileExtension( ".crt" )]
    CARTRIDGE_GMOD2_CRT,
    [Description( "D81 Disk Image" )]
    [AdditionalArgumentCount( 2, 0 )]
    [DefaultFileExtension( ".d81" )]
    D81,
    [Description( "Ultimax Cartridge 4k Binary" )]
    [DefaultFileExtension( ".bin" )]
    CARTRIDGE_ULTIMAX_4K_BIN,
    [Description( "Ultimax Cartridge 4k CRT" )]
    [DefaultFileExtension( ".crt" )]
    CARTRIDGE_ULTIMAX_4K_CRT,
    [Description( "Ultimax Cartridge 8k Binary" )]
    [DefaultFileExtension( ".bin" )]
    CARTRIDGE_ULTIMAX_8K_BIN,
    [Description( "Ultimax Cartridge 8k CRT" )]
    [DefaultFileExtension( ".crt" )]
    CARTRIDGE_ULTIMAX_8K_CRT,
    [Description( "Ultimax Cartridge 16k Binary" )]
    [DefaultFileExtension( ".bin" )]
    CARTRIDGE_ULTIMAX_16K_BIN,
    [Description( "Ultimax Cartridge 16k CRT" )]
    [DefaultFileExtension( ".crt" )]
    CARTRIDGE_ULTIMAX_16K_CRT,
    [Description( "Magic Desk Cartridge 32k Binary" )]
    [DefaultFileExtension( ".bin" )]
    CARTRIDGE_MAGICDESK_BIN_32K,
    [Description( "Magic Desk Cartridge 32k CRT" )]
    [DefaultFileExtension( ".crt" )]
    CARTRIDGE_MAGICDESK_CRT_32K,
    [Description( "Magic Desk Cartridge 128k Binary" )]
    [DefaultFileExtension( ".bin" )]
    CARTRIDGE_MAGICDESK_BIN_128K,
    [Description( "Magic Desk Cartridge 128k CRT" )]
    [DefaultFileExtension( ".crt" )]
    CARTRIDGE_MAGICDESK_CRT_128K,
    [Description( "Magic Desk Cartridge 256k Binary" )]
    [DefaultFileExtension( ".bin" )]
    CARTRIDGE_MAGICDESK_BIN_256K,
    [Description( "Magic Desk Cartridge 256k CRT" )]
    [DefaultFileExtension( ".crt" )]
    CARTRIDGE_MAGICDESK_CRT_256K,
    [Description( "Magic Desk Cartridge 512k Binary" )]
    [DefaultFileExtension( ".bin" )]
    CARTRIDGE_MAGICDESK_BIN_512K,
    [Description( "Magic Desk Cartridge 512k CRT" )]
    [DefaultFileExtension( ".crt" )]
    CARTRIDGE_MAGICDESK_CRT_512K,
    [Description( "Magic Desk Cartridge 1M Binary" )]
    [DefaultFileExtension( ".bin" )]
    CARTRIDGE_MAGICDESK_BIN_1M,
    [Description( "Magic Desk Cartridge 1M CRT" )]
    [DefaultFileExtension( ".crt" )]
    CARTRIDGE_MAGICDESK_CRT_1M,
    [Description( "CPC DSK Disk Image" )]
    [DefaultFileExtension( ".dsk" )]
    DSK,
    [Description( "NES Cartridge (iNES)" )]
    [AdditionalArgumentCount( 0, 4 )]
    [DefaultFileExtension( ".nes" )]
    CARTRIDGE_NES,
    [Description( "P (ZX81)" )]
    [DefaultFileExtension( ".p81" )]
    P_ZX81
  }



}