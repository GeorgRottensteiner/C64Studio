using System.ComponentModel;

namespace RetroDevStudio.Types
{
  public enum FileTypeNative
  {
    [Description( "???" )]
    NONE = 0,

    [Description( "DEL" )]
    [DefaultFileExtension( ".del" )]
    COMMODORE_DEL,
    [Description( "SEQ" )]
    [DefaultFileExtension( ".seq" )]
    COMMODORE_SEQ,
    [Description( "PRG" )]
    [DefaultFileExtension( ".prg" )]
    [MachineType( MachineType.CBM )]
    [MachineType( MachineType.C128 )]
    [MachineType( MachineType.C64 )]
    [MachineType( MachineType.COMMANDER_X16 )]
    [MachineType( MachineType.MEGA65 )]
    [MachineType( MachineType.PET )]
    [MachineType( MachineType.PLUS4 )]
    [MachineType( MachineType.VIC20 )]
    COMMODORE_PRG,
    [Description( "USR" )]
    [DefaultFileExtension( ".usr" )]
    COMMODORE_USR,
    [Description( "REL" )]
    [DefaultFileExtension( ".rel" )]
    COMMODORE_REL,

    // ADF format
    [Description( "FIL" )]
    [MachineType( MachineType.AMIGA )]
    ADF_FILE,
    [Description( "DIR" )]
    ADF_DIR,

    // DSK format
    [Description( "FIL" )]
    [MachineType( MachineType.CPC )]
    CPC_FILE,
    [Description( "SCR" )]
    CPC_SCRATCHED,

    TZX_FILE,

    // P format
    [Description( "Spectrum P" )]
    [MachineType( MachineType.ZX80 )]
    [MachineType( MachineType.ZX81 )]
    [MachineType( MachineType.ZX_SPECTRUM )]
    SPECTRUM_P
    /*
    [Description( "Program Header" )]
    P_PROGRAM_HEADER              = 0x10,
    [Description( "Turbo Speed Data Block" )]
    TZX_TURBO_SPEED_DATA_BLOCK    = 0x11,
    [Description( "Pure Tone" )]
    TZX_PURE_TONE                 = 0x12,
    [Description( "Pulse Sequence" )]
    TZX_PULSE_SEQUENCE            = 0x13,
    [Description( "Pure Data Block" )]
    TZX_PURE_DATA_BLOCK           = 0x14,
    [Description( "Direct Recording Block" )]
    TZX_DIRECT_RECORDING_BLOCK    = 0x15,
    [Description( "C64 ROM Block" )]
    TZX_C64_ROM_BLOCK             = 0x16,
    [Description( "C64 Turbo Block" )]
    TZX_C64_TURBO_BLOCK           = 0x17,
    [Description( "CSW Recording Block" )]
    TZX_CSW_RECORDING_BLOCK       = 0x18,
    [Description( "Generalized Data Block" )]
    TZX_GENERALIZED_DATA_BLOCK    = 0x19,
    [Description( "Pause/Stop Tape" )]
    TZX_PAUSE_OR_STOP_TAPE        = 0x20,
    [Description( "Group Start" )]
    TZX_GROUP_START               = 0x21,
    [Description( "Group End" )]
    TZX_GROUP_END                 = 0x22,
    [Description( "Jump to Block" )]
    TZX_JUMP_TO_BLOCK             = 0x23,
    [Description( "Loop Start" )]
    TZX_LOOP_START                = 0x24,
    [Description( "Loop End" )]
    TZX_LOOP_END                  = 0x25,
    [Description( "Call Sequence" )]
    TZX_CALL_SEQUENCE             = 0x26,
    [Description( "Return from Sequence" )]
    TZX_RETURN_FROM_SEQUENCE      = 0x27,
    [Description( "Select Block" )]
    TZX_SELECT_BLOCK              = 0x28,
    [Description( "Stop Tape 48k" )]
    TZX_STOP_TAPE_48K             = 0x2A,
    [Description( "Set Signal" )]
    TZX_SET_SIGNAL                = 0x2B,
    [Description( "Text Description" )]
    TZX_TEXT_DESCRIPTION          = 0x30,
    [Description( "Message" )]
    TZX_MESSAGE                   = 0x31,
    [Description( "Archive Info" )]
    TZX_ARCHIVE_INFO              = 0x32,
    [Description( "Hardware Type" )]
    TZX_HARDWARE_TYPE             = 0x33,
    [Description( "Emulation Info" )]
    TZX_EMULATION_INFO            = 0x34,
    [Description( "Custom Info" )]
    TZX_CUSTOM_INFO               = 0x35,
    [Description( "Screen Block" )]
    TZX_SCREEN_BLOCK              = 0x40,
    [Description( "Skip (Glue) Block" )]
    TZX_SKIP_GLUE_BLOCK           = 0x5A*/
  }

}
