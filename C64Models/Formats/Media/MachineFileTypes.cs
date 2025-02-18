using System.ComponentModel;

namespace RetroDevStudio.Types
{
  public enum CommodoreFileTypeNative
  {
    [FileTypeNative( FileTypeNative.COMMODORE_DEL )]
    [Description( "DEL" )]
    DEL       = 0x00,
    [FileTypeNative( FileTypeNative.COMMODORE_SEQ )]
    [Description( "SEQ" )]
    SEQ       = 0x01,
    [FileTypeNative( FileTypeNative.COMMODORE_PRG )]
    [Description( "PRG" )]
    PRG       = 0x02,
    [FileTypeNative( FileTypeNative.COMMODORE_USR )]
    [Description( "USR" )]
    USR       = 0x03,
    [FileTypeNative( FileTypeNative.COMMODORE_REL )]
    [Description( "REL" )]
    REL       = 0x04,

    SCRATCHED = 0x00,
    LOCKED    = 0x40,
    CLOSED    = 0x80
  }



  public enum AmigaFileTypeNative
  {
    [FileTypeNative( FileTypeNative.ADF_FILE )]
    [Description( "FILE" )]
    FILE      = -3,
    [FileTypeNative( FileTypeNative.ADF_DIR )]
    [Description( "DIR" )]
    DIR       = 2
  }



  public enum TZXFileTypeNative
  {
    // TZX format
    [Description( "Standard Speed Data Block" )]
    STANDARD_SPEED_DATA_BLOCK = 0x10,
    [Description( "Turbo Speed Data Block" )]
    TURBO_SPEED_DATA_BLOCK    = 0x11,
    [Description( "Pure Tone" )]
    PURE_TONE                 = 0x12,
    [Description( "Pulse Sequence" )]
    PULSE_SEQUENCE            = 0x13,
    [Description( "Pure Data Block" )]
    PURE_DATA_BLOCK           = 0x14,
    [Description( "Direct Recording Block" )]
    DIRECT_RECORDING_BLOCK    = 0x15,
    [Description( "C64 ROM Block" )]
    C64_ROM_BLOCK             = 0x16,
    [Description( "C64 Turbo Block" )]
    C64_TURBO_BLOCK           = 0x17,
    [Description( "CSW Recording Block" )]
    CSW_RECORDING_BLOCK       = 0x18,
    [Description( "Generalized Data Block" )]
    GENERALIZED_DATA_BLOCK    = 0x19,
    [Description( "Pause/Stop Tape" )]
    PAUSE_OR_STOP_TAPE        = 0x20,
    [Description( "Group Start" )]
    GROUP_START               = 0x21,
    [Description( "Group End" )]
    GROUP_END                 = 0x22,
    [Description( "Jump to Block" )]
    JUMP_TO_BLOCK             = 0x23,
    [Description( "Loop Start" )]
    LOOP_START                = 0x24,
    [Description( "Loop End" )]
    LOOP_END                  = 0x25,
    [Description( "Call Sequence" )]
    CALL_SEQUENCE             = 0x26,
    [Description( "Return from Sequence" )]
    RETURN_FROM_SEQUENCE      = 0x27,
    [Description( "Select Block" )]
    SELECT_BLOCK              = 0x28,
    [Description( "Stop Tape 48k" )]
    STOP_TAPE_48K             = 0x2A,
    [Description( "Set Signal" )]
    SET_SIGNAL                = 0x2B,
    [Description( "Text Description" )]
    TEXT_DESCRIPTION          = 0x30,
    [Description( "Message" )]
    MESSAGE                   = 0x31,
    [Description( "Archive Info" )]
    ARCHIVE_INFO              = 0x32,
    [Description( "Hardware Type" )]
    HARDWARE_TYPE             = 0x33,
    [Description( "Emulation Info" )]
    EMULATION_INFO            = 0x34,
    [Description( "Custom Info" )]
    CUSTOM_INFO               = 0x35,
    [Description( "Screen Block" )]
    SCREEN_BLOCK              = 0x40,
    [Description( "Skip (Glue) Block" )]
    SKIP_GLUE_BLOCK           = 0x5A,
  }

}
