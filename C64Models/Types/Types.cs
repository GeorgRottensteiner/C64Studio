using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Windows.Forms;

namespace C64Studio.Types
{
  public enum MachineType
  {
    UNKNOWN,
    C64,
    VC20,
    C128,
    PLUS4,
    PET,
    CBM,
    ATARI2600
  };

  public class Machine
  {
    public MachineType    Type;
    public int            InitialBreakpointAddress;           // common initial breakpoint address before jumping to program start (inside Kernal)
    public int            InitialBreakpointAddressCartridge;  // initial breakpoint address before jumping to cartridge start



    internal static Machine FromType( MachineType Type )
    {
      switch ( Type )
      {
        case MachineType.C64:
          return C64Studio.Lookup.Machines.C64;
        case MachineType.VC20:
          return C64Studio.Lookup.Machines.VC20;
        case MachineType.C128:
          return C64Studio.Lookup.Machines.C128;
        default:
          // fallback to C64
          return C64Studio.Lookup.Machines.C64;
      }
    }
  }



  public enum AssemblerType
  {
    AUTO,
    C64_STUDIO,
    PDS,
    DASM,
    C64ASM,
    CBMPRGSTUDIO,
    TASM
  };

  public enum CharsetMode
  {
    INVALID     = -1,
    HIRES       = 0,
    MULTICOLOR  = 1,
    ECM         = 2
  };

  public class MacroInfo
  {
    public enum PseudoOpType
    {
      UNKNOWN,
      BYTE,
      WORD,
      WORD_BE,
      DWORD,
      DWORD_BE,
      TEXT,
      SCREEN_TAB,
      CONVERSION_TAB,
      PSEUDO_PC,
      REAL_PC,
      BANK,
      INCLUDE_BINARY,
      INCLUDE_SOURCE,
      COMPILE_TARGET,
      ZONE,
      ERROR,
      WARN,
      IFDEF,
      IFNDEF,
      IF,
      FILL,
      ALIGN,
      END_OF_FILE,
      IGNORE,
      ORG,
      NO_WARNING,
      FOR,
      END,
      MACRO,
      MESSAGE,
      HEX,            // ACME: !HEX, !H  f0f1f2 or !hex f0 f1 f2
      TEXT_SCREEN,
      LOW_BYTE,
      HIGH_BYTE,
      LOOP_START,     // PDS: DO, DASM: REPEAT
      LOOP_END,       // PDS: LOOP, DASM: REPEND
      SEG,            // DASM: SEG
      SEG_VIRTUAL,    // DASM: SEG.U
      TEXT_PET,
      TRACE,
      TEXT_RAW,
      INCLUDE_MEDIA,
      INCLUDE_MEDIA_SOURCE,
      ELSE,
      END_IF,
      LABEL_FILE,     // ACME: !sl
      SET,
      ALIGN_DASM,
      BASIC,
      ADDRESS,              // ACME: !ADDR with or without braces
      REPEAT,               // PDS: Repeat <x> times next line
      ADD_INCLUDE_SOURCE,   // DASM: Add directory to source code paths (custom library paths)
      CPU                   // Set the procesor type
    }

    public PseudoOpType      Type = PseudoOpType.UNKNOWN;
    public string         Keyword = "";
  };


  public enum Function
  {
    NONE = 0,
    BUILD,
    BUILD_AND_RUN,
    BUILD_AND_DEBUG,
    DEBUG_STEP,
    DEBUG_STEP_OVER,
    DEBUG_STEP_OUT,
    DEBUG_GO,
    DEBUG_BREAK,
    DEBUG_STOP,
    DEBUG_RUN_TO,
    SAVE_DOCUMENT,
    GO_TO_DECLARATION,
    DELETE_LINE,
    CENTER_ON_CURSOR,
    FIND,
    FIND_NEXT,
    COMPILE,
    FIND_REPLACE,
    PRINT,
    SAVE_ALL,
    SAVE_DOCUMENT_AS,
    FIND_IN_PROJECT,
    REPLACE_IN_PROJECT,
    HELP,
    REBUILD,
    TOGGLE_BREAKPOINT,
    MOVE_LINE_UP,
    MOVE_LINE_DOWN,
    COPY_LINE_UP,
    COPY_LINE_DOWN,
    COMMENT_SELECTION,
    UNCOMMENT_SELECTION,
    OPEN_FILES,
    FIND_NEXT_MESSAGE,
    UNDO,
    REDO,
    BUILD_TO_PREPROCESSED_FILE,
    COLLAPSE_ALL_FOLDING_BLOCKS,
    EXPAND_ALL_FOLDING_BLOCKS,
    COPY,
    PASTE,
    CUT,
    JUMP_TO_LINE,
    GRAPHIC_ELEMENT_MIRROR_H,
    GRAPHIC_ELEMENT_MIRROR_V,
    GRAPHIC_ELEMENT_SHIFT_L,
    GRAPHIC_ELEMENT_SHIFT_R,
    GRAPHIC_ELEMENT_SHIFT_U,
    GRAPHIC_ELEMENT_SHIFT_D,
    GRAPHIC_ELEMENT_ROTATE_L,
    GRAPHIC_ELEMENT_ROTATE_R,
    GRAPHIC_ELEMENT_INVERT,
    GRAPHIC_ELEMENT_NEXT,
    GRAPHIC_ELEMENT_PREVIOUS,
    GRAPHIC_ELEMENT_CUSTOM_COLOR,
    GRAPHIC_ELEMENT_MULTI_COLOR_1,
    GRAPHIC_ELEMENT_MULTI_COLOR_2,
    BOOKMARK_ADD,
    BOOKMARK_DELETE,
    BOOKMARK_DELETE_ALL,
    BOOKMARK_NEXT,
    BOOKMARK_PREVIOUS,
    NAVIGATE_BACK,
    NAVIGATE_FORWARD
  }

  public enum StudioState
  {
    NORMAL,
    COMPILE,
    BUILD,
    BUILD_AND_RUN,
    BUILD_AND_DEBUG,
    DEBUGGING_RUN,
    DEBUGGING_BROKEN,
    BUILD_PRE_PROCESSED_FILE
  };

  [Flags]
  public enum FunctionStudioState
  {
    [Description("Normal Editing")]
    NORMAL            = 0x00000001,
    [Description( "Broken in Debugger" )]
    DEBUGGER_BROKEN   = 0x00000002,
    [Description( "Running in Debug Mode" )]
    DEBUGGER_RUNNING  = 0x00000004,
    [Description( "Debug Mode" )]
    DEBUGGER = DEBUGGER_BROKEN | DEBUGGER_RUNNING,
    [Description( "Broken in Debug Mode" )]
    DEBUGGER_EDITING = NORMAL | DEBUGGER_BROKEN,
    [Description( "Building" )]
    BUILDING          = 0x00000008,
    [Description( "Any" )]
    ANY               = 0x0000000f
  };

  public enum ErrorCode
  {
    OK                                      = 0,

    E0001_NO_OUTPUT_FILENAME                = 0x0001,
    E0002_CODE_WITHOUT_START_ADDRESS        = 0x0002,

    E1000_SYNTAX_ERROR                      = 0x1000,
    E1001_FAILED_TO_EVALUATE_EXPRESSION     = 0x1001,
    E1002_VALUE_OUT_OF_BOUNDS_BYTE          = 0x1002,
    E1003_VALUE_OUT_OF_BOUNDS_WORD          = 0x1003,
    E1004_MISSING_OPENING_BRACKET           = 0x1004,
    E1005_MISSING_CLOSING_BRACKET           = 0x1005,
    E1006_MALFORMED_BLOCK_CLOSE_STATEMENT   = 0x1006,
    E1007_MISSING_LOOP_START                = 0x1007,
    E1008_MISSING_LOOP_END                  = 0x1008,
    E1009_INVALID_VALUE                     = 0x1009,
    E1010_UNKNOWN_LABEL                     = 0x1010,

    E1100_RELATIVE_JUMP_TOO_FAR             = 0x1100,
    E1101_BANK_TOO_BIG                      = 0x1101,
    E1102_PROGRAM_TOO_LARGE                 = 0x1102,
    E1103_SEGMENT_OVERLAP                   = 0x1103,
    E1104_BANK_SIZE_INVALID                 = 0x1104,
    E1105_INVALID_OPCODE                    = 0x1105,
    E1106_SEGMENT_OUT_OF_BOUNDS             = 0x1106,
    E1107_ARGUMENT_OUT_OF_BOUNDS            = 0x1107,
    E1108_SAFETY_BREAK                      = 0x1108,

    E1200_REDEFINITION_OF_LABEL             = 0x1200,
    E1201_REDEFINITION_OF_PREPROCESSOR_DEFINE = 0x1201,
    E1202_REDEFINITION_OF_ZONE              = 0x1202,
    E1203_REDEFINITION_OF_CONSTANT          = 0x1203,

    E1300_OPCODE_AMBIGIOUS                  = 0x1300,
    E1301_PSEUDO_OPERATION                     = 0x1301,
    E1302_MALFORMED_MACRO                   = 0x1302,
    E1303_MALFORMED_ZONE_DESCRIPTOR         = 0x1303,
    E1304_UNSUPPORTED_TARGET_TYPE           = 0x1304,
    E1305_EXPECTED_TRAILING_SYMBOL          = 0x1305,
    E1306_EXPECTED_BRACKETS_AND_TRAILING_SYMBOL = 0x1306,
    E1307_FILENAME_INCOMPLETE               = 0x1307,
    E1308_USER_ERROR                        = 0x1308,
    E1309_ELSE_WITHOUT_IF                   = 0x1309,
    E1310_END_IF_WITHOUT_SCOPE              = 0x1310,
    E1311_UNSUPPORTED_CPU                   = 0x1311,

    E1400_CIRCULAR_INCLUSION                = 0x1400,
    E1401_INTERNAL_ERROR                    = 0x1401,

    E2000_FILE_OPEN_ERROR                   = 0x2000,
    E2001_FILE_READ_ERROR                   = 0x2001,
    E2002_UNSUPPORTED_FILE_TYPE             = 0x2002,

    E3000_BASIC_MISSING_LINE_NUMBER         = 0x3000,
    E3001_BASIC_INVALID_LINE_NUMBER         = 0x3001,
    E3002_BASIC_UNSUPPORTED_CHARACTER       = 0x3002,
    E3003_BASIC_LABEL_MALFORMED             = 0x3003,
    E3004_BASIC_MISSING_LABEL               = 0x3004,
    E3005_BASIC_UNKNOWN_MACRO               = 0x3005,
    E3006_BASIC_LINE_TOO_LONG               = 0x3006,


    WARNING_START                           = 0x7FFF,

    [Description( "W0001: Segment overlaps" )]
    W0001_SEGMENT_OVERLAP                   = 0x8000,
    [Description( "W0002: Unknown macro" )]
    W0002_UNKNOWN_MACRO                     = 0x8001,
    [Description( "W0003: Bank index already used" )]
    W0003_BANK_INDEX_ALREADY_USED           = 0x8002,
    [Description( "W0004: Target file name already provided" )]
    W0004_TARGET_FILENAME_ALREADY_PROVIDED  = 0x8003,
    [Description( "W0005: Warning trigger by pseudo operation" )]
    W0005_USER_WARNING                      = 0x8004,
    [Description( "W0006: Label dump file name already provided" )]
    W0006_LABEL_DUMP_FILE_ALREADY_GIVEN     = 0x8005,
    [Description( "W0007: Potential problem detected" )]
    W0007_POTENTIAL_PROBLEM                 = 0x8006,
    [Description( "W1000: Unused label" )]
    W1000_UNUSED_LABEL                      = 0x9003,
    [Description( "W1001: BASIC line is too long for manual entry" )]
    W1001_BASIC_LINE_TOO_LONG_FOR_MANUAL_ENTRY = 0x9004,

    WARNING_LAST_PLUS_ONE
  };



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
    D64,
    [Description( "Magic Desk Cartridge Binary" )]
    CARTRIDGE_MAGICDESK_BIN,
    [Description( "Magic Desk Cartridge CRT" )]
    CARTRIDGE_MAGICDESK_CRT,
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
    CARTRIDGE_ULTIMAX_16K_CRT
  };



  public enum GraphicType
  {
    [Description( "Sprites" )]
    SPRITES = 0,
    [Description( "Characters" )]
    CHARACTERS,
    [Description( "HiRes Characters" )]
    CHARACTERS_HIRES,
    [Description( "Multicolor Characters" )]
    CHARACTERS_MULTICOLOR,
    [Description( "Bitmap" )]
    BITMAP,
    [Description( "HiRes Bitmap" )]
    BITMAP_HIRES,
    [Description( "Multicolor Bitmap" )]
    BITMAP_MULTICOLOR
  };



  public class MulticolorSettings
  {
    public int      BackgroundColor = 0;
    public int      MultiColor1     = 0;
    public int      MultiColor2     = 0;
  };



  public enum ColorableElement
  {
    [Description( "Text" )] 
    NONE = 0,
    [Description( "Code" )] 
    CODE,
    [Description( "String Literal" )] 
    LITERAL_STRING,
    [Description( "Numeric Literal" )] 
    LITERAL_NUMBER,
    [Description( "Label" )] 
    LABEL,
    [Description( "Comment" )] 
    COMMENT,
    [Description( "Pseudo Operator" )] 
    PSEUDO_OP,
    [Description( "Current Debug Position" )] 
    CURRENT_DEBUG_LINE,
    [Description( "Empty Space" )]
    EMPTY_SPACE,
    [Description( "Operator" )]
    OPERATOR,
    [Description( "Highlighted Search Results" )]
    HIGHLIGHTED_SEARCH_RESULTS,

    [Description( "Error Underline" )]
    ERROR_UNDERLINE,

    [Description( "Selected Text" )]
    SELECTED_TEXT,

    [Description( "Control Text" )]
    CONTROL_TEXT,

    //FIRST_GUI_ELEMENT = CONTROL_TEXT,

    [Description( "Control Background" )]
    BACKGROUND_CONTROL,

    [Description( "Debug Changed Element" )]
    CHANGED_DEBUG_ELEMENT,

    LAST_ENTRY
  }

  public class ColorSetting
  {
    public uint         FGColor = 0;
    public uint         BGColor = 0xffffffff;
    public bool         BGColorAuto = false;
    public string       Name = "";


    public ColorSetting( string Name )
    {
      this.Name = Name;
    }

    public ColorSetting( string Name, uint FGColor, uint BGColor )
    {
      this.Name = Name;
      this.FGColor = FGColor;
      this.BGColor = BGColor;
      this.BGColorAuto = false;
    }

    public ColorSetting( string Name, uint FGColor )
    {
      this.Name = Name;
      this.FGColor = FGColor;
      this.BGColorAuto = true;
    }

    public override string ToString()
    {
      return Name;
    }
  }

  public class ComboItem
  {
    public object     Tag = null;
    public string     Desc = "";


    public ComboItem( string Desc )
    {
      this.Desc = Desc;
    }

    public ComboItem( string Desc, object Tag )
    {
      this.Desc = Desc;
      this.Tag = Tag;
    }

    public override string  ToString()
    {
      return Desc;
    }
  }

  public class Constants
  {
    public static string FILEFILTER_ALL = "All Files|*.*|";
    public static string FILEFILTER_ASM = "ASM File|*.asm;*.a|";
    public static string FILEFILTER_BASIC = "Basic File|*.bas|";
    public static string FILEFILTER_CHARSET_FILE = "Charset File|*.chr|";
    public static string FILEFILTER_CHARSET_PROJECT = "Charset Project|*.charsetproject|";
    public static string FILEFILTER_CHARSET = "Charset Project or File|*.charsetproject;*.chr|";
    public static string FILEFILTER_CHARSET_CHARPAD = "Charpad Project|*.ctm|";
    public static string FILEFILTER_MARCS_PETSCII = "Marc's PETSCII Editor File|*.c|";
    public static string FILEFILTER_CHARSET_SCREEN = "Charset Screen Project|*.charscreen|";
    public static string FILEFILTER_SPRITE_PROJECT = "Sprite Project|*.spriteproject|";
    public static string FILEFILTER_SPRITE_FILE = "Sprite File|*.spr|";
    public static string FILEFILTER_SPRITE = "Sprite Project or File|*.spriteproject;*.spr|";
    public static string FILEFILTER_SPRITE_SPRITEPAD = "Spritepad Project|*.spd|";
    public static string FILEFILTER_GRAPHIC_SCREEN = "Graphic Screen Project|*.graphicscreen|";
    public static string FILEFILTER_MAP = "Map Project|*.mapproject|";
    public static string FILEFILTER_EXECUTABLE = "Executable Files|*.exe|";
    public static string FILEFILTER_ACME = "ACME|acme.exe|";
    public static string FILEFILTER_PRG = "PRG File|*.prg|";
    public static string FILEFILTER_VICE = "Vice|x64.exe;x64sc.exe|";
    public static string FILEFILTER_PROJECT = "C64 Studio Project Files|*.c64|";
    public static string FILEFILTER_SOLUTION = "C64 Studio Solution Files|*.s64|";
    public static string FILEFILTER_SOLUTION_OR_PROJECTS = "C64 Studio Solution or Project Files|*.s64;*.c64|";
    public static string FILEFILTER_ALL_SUPPORTED_FILES = "Supported Files|*.s64;*.c64;*.asm;*.a;*.charsetproject;*.spriteproject;*.bas;*.chr;*.spr;*.charscreen;*.graphicscreen;*.mapproject;*.bin|";
    public static string FILEFILTER_SOURCE_FILES = "Source Files|*.asm;*.bas|";
    public static string FILEFILTER_BINARY_FILES = "Binary Files|*.bin|";
    public static string FILEFILTER_MEDIA_FILES = "Tape/Disk Files|*.t64;*.prg;*.d64;*.d71;*.d81|";
    public static string FILEFILTER_IMAGE_FILES = "Image Files|*.png;*.bmp;*.gif|PNG Files|*.png|BMP Files|*.bmp|GIF Files|*.gif|Koala Files|*.koa;*.kla|";
    public static string FILEFILTER_TAPE = "Tape Files|*.t64,*.prg|";
    public static string FILEFILTER_DISK = "Disk Files|*.d64;*.d71;*.d81|";
    public static string FILEFILTER_VALUE_TABLE_PROJECT = "Value Table Project Files|*.valuetableproject|";
    public static string FILEFILTER_VALUE_TABLE_DATA = "Value Table Data Files|*.dat|";
  }

  public class FileChunk
  {
    public const System.UInt16    RESTART_INFO          = 0x0100;
    public const System.UInt16    RESTART_DATA          = 0x0101;
    public const System.UInt16    RESTART_DOC_INFO      = 0x0102;

    public const System.UInt16    SOLUTION              = 0x0400;
    public const System.UInt16    SOLUTION_INFO         = 0x0401;
    public const System.UInt16    SOLUTION_PROJECT      = 0x0402;
    public const System.UInt16    SOLUTION_NODES        = 0x0403;

    public const System.UInt16    PROJECT               = 0x1000;
    public const System.UInt16    PROJECT_ELEMENT       = 0x1001;
    public const System.UInt16    PROJECT_ELEMENT_DATA  = 0x1002;
    public const System.UInt16    PROJECT_ELEMENT_DISPLAY_DATA        = 0x1003;
    public const System.UInt16    PROJECT_ELEMENT_PER_CONFIG_SETTING  = 0x1004;
    public const System.UInt16    PROJECT_ELEMENT_FOLDED_BLOCKS       = 0x1005;
    public const System.UInt16    PROJECT_CONFIG        = 0x1100;
    public const System.UInt16    PROJECT_WATCH_ENTRY   = 0x1101;

    public const System.UInt16    CHARSET_SCREEN_INFO   = 0x1200;
    public const System.UInt16    SCREEN_CHAR_DATA      = 0x1300;
    public const System.UInt16    SCREEN_COLOR_DATA     = 0x1301;
    public const System.UInt16    GRAPHIC_SCREEN_INFO   = 0x1310;
    public const System.UInt16    GRAPHIC_DATA          = 0x1311;   // uint width, uint height, uint image type, uint palette entry count, byte r,g,b, uint data size, data
    public const System.UInt16    GRAPHIC_COLOR_MAPPING = 0x1312;   // Dictionary<int,List<byte>>

    public const System.UInt16    MAP_PROJECT_INFO      = 0x1320;
    public const System.UInt16    MAP_PROJECT_DATA      = 0x1321;
    public const System.UInt16    MAP_TILE              = 0x1322;
    public const System.UInt16    MAP                   = 0x1324;
    public const System.UInt16    MAP_INFO              = 0x1325;
    public const System.UInt16    MAP_DATA              = 0x1326;
    public const System.UInt16    MAP_EXTRA_DATA        = 0x1327;
    public const System.UInt16    MAP_CHARSET           = 0x1328;
    public const System.UInt16    MAP_EXTRA_DATA_TEXT   = 0x1329;   // replaces MAP_EXTRA_DATA

    public const System.UInt16    SOURCE_ASM            = 0x1330;
    public const System.UInt16    SOURCE_BASIC          = 0x1331;

    public const System.UInt16    SPRITESET_LAYER       = 0x1400;
    public const System.UInt16    SPRITESET_LAYER_ENTRY = 0x1401;
    public const System.UInt16    SPRITESET_LAYER_INFO  = 0x1402;

    public const System.UInt16    MULTICOLOR_DATA       = 0x1500;
    public const System.UInt16    CHARSET_DATA          = 0x1501;   // multicolor-data und binary data

    public const System.UInt16    DISASSEMBLY_INFO      = 0x1600;
    public const System.UInt16    DISASSEMBLY_DATA      = 0x1601;
    public const System.UInt16    DISASSEMBLY_JUMP_ADDRESSES = 0x1602;
    public const System.UInt16    DISASSEMBLY_NAMED_LABELS = 0x1603;

    public const System.UInt16    SETTINGS_TOOL         = 0x2000;
    public const System.UInt16    SETTINGS_ACCELERATOR  = 0x2001;
    public const System.UInt16    SETTINGS_SOUND        = 0x2002;
    public const System.UInt16    SETTINGS_WINDOW       = 0x2003;
    public const System.UInt16    SETTINGS_TEXT_EDITOR  = 0x2004;
    public const System.UInt16    SETTINGS_FONT         = 0x2005;
    public const System.UInt16    SETTINGS_SYNTAX_COLORING = 0x2006;
    public const System.UInt16    SETTINGS_UI           = 0x2007;
    public const System.UInt16    SETTINGS_DEFAULTS     = 0x2008;
    public const System.UInt16    SETTINGS_FIND_REPLACE = 0x2009;
    public const System.UInt16    SETTINGS_IGNORED_WARNINGS = 0x200A;
    public const System.UInt16    SETTINGS_LAYOUT       = 0x200B;   // do not use anymore!
    public const System.UInt16    SETTINGS_PANEL_DISPLAY_DETAILS = 0x200C;
    public const System.UInt16    SETTINGS_DPS_LAYOUT   = 0x200D;
    public const System.UInt16    SETTINGS_RUN_EMULATOR = 0x200E;
    public const System.UInt16    SETTINGS_BASIC_KEYMAP = 0x200F;
    public const System.UInt16    SETTINGS_BASIC_PARSER = 0x2010;
    public const System.UInt16    SETTINGS_ASSEMBLER_EDITOR = 0x2011;
    public const System.UInt16    SETTINGS_ENVIRONMENT  = 0x2012;
    public const System.UInt16    SETTINGS_PERSPECTIVES = 0x2013;
    public const System.UInt16    SETTINGS_PERSPECTIVE  = 0x2014;
    public const System.UInt16    SETTINGS_OUTLINE      = 0x2015;
    public const System.UInt16    SETTINGS_HEX_VIEW     = 0x2016;
    public const System.UInt16    SETTINGS_MRU_PROJECTS = 0x2017;
    public const System.UInt16    SETTINGS_MRU_FILES    = 0x2018;
    public const System.UInt16    SETTINGS_WARNINGS_AS_ERRORS = 0x2019;
    public const System.UInt16    SETTINGS_C64STUDIO_HACKS    = 0x201A;
  }

  public enum KeyboardKey
  {
    UNDEFINED = 0,
    KEY_ARROW_LEFT,
    KEY_1,
    KEY_2,
    KEY_3,
    KEY_4,
    KEY_5,
    KEY_6,
    KEY_7,
    KEY_8,
    KEY_9,
    KEY_0,
    KEY_PLUS,
    KEY_MINUS,
    KEY_POUND,
    KEY_CLR_HOME,
    KEY_INST_DEL,
    KEY_CTRL,
    KEY_Q,
    KEY_W,
    KEY_E,
    KEY_R,
    KEY_T,
    KEY_Y,
    KEY_U,
    KEY_I,
    KEY_O,
    KEY_P,
    KEY_AT,
    KEY_STAR,
    KEY_ARROW_UP,
    KEY_RESTORE,
    KEY_RUN_STOP,
    KEY_SHIFT_LOCK,
    KEY_A,
    KEY_S,
    KEY_D,
    KEY_F,
    KEY_G,
    KEY_H,
    KEY_J,
    KEY_K,
    KEY_L,
    KEY_COLON,
    KEY_SEMI_COLON,
    KEY_EQUAL,
    KEY_RETURN,
    KEY_COMMODORE,
    KEY_SHIFT_LEFT,
    KEY_Z,
    KEY_X,
    KEY_C,
    KEY_V,
    KEY_B,
    KEY_N,
    KEY_M,
    KEY_COMMA,
    KEY_DOT,
    KEY_SLASH,
    KEY_SHIFT_RIGHT,
    KEY_CURSOR_UP_DOWN,
    KEY_CURSOR_LEFT_RIGHT,
    KEY_SPACE,
    KEY_F1,
    KEY_F3,
    KEY_F5,
    KEY_F7,

    LAST_ENTRY
  }

  [Flags]
  public enum KeyType
  {
    NORMAL              = 0,
    CONTROL_CODE        = 0x00000001,     // can only be shown/entered inside string mode (colors, cursor keys)
    GRAPHIC_SYMBOL      = 0x00000002,     // only usefully useage inside string mode (important for BASIC editor)
    EDITOR_CONTROL_CODE = 0x00000004      // could be shown but is always not inserted as token
  }

  public enum KeyModifier
  {
    NORMAL,
    SHIFT,
    CONTROL,
    COMMODORE
  }

  public enum FileType
  {
    [Description( "SCR" )]
    SCRATCHED = 0x00,
    [Description( "DEL" )]
    DEL = 0x80,
    [Description( "SEQ" )]
    SEQ = 0x81,
    [Description( "PRG" )]
    PRG = 0x82,
    [Description( "USR" )]
    USR = 0x83,
    [Description( "REL" )]
    REL = 0x84,

    LOCKED = 0x40,
    CLOSED = 0x80
  }

  public class FileInfo
  {
    public GR.Memory.ByteBuffer Filename = new GR.Memory.ByteBuffer( 16 );
    public GR.Memory.ByteBuffer Data = new GR.Memory.ByteBuffer();
    public int StartTrack = -1;
    public int StartSector = -1;
    public int Blocks = 0;

    /* Bit 0-3: The actual filetype
                          000 (0) - DEL
                          001 (1) - SEQ
                          010 (2) - PRG
                          011 (3) - USR
                          100 (4) - REL
                          Values 5-15 are illegal, but if used will produce
                          very strange results. The 1541 is inconsistent in
                          how it treats these bits. Some routines use all 4
                          bits, others ignore bit 3,  resulting  in  values
                          from 0-7.
       Bit   5: Used only during SAVE-@ replacement
       Bit   6: Locked flag (Set produces ">" locked files)
       Bit   7: Closed flag  (Not  set  produces  "*", or "splat"
                  files)
     */
    public FileType Type = FileType.SCRATCHED;

    public int DirEntryIndex = -1;
  }

  public class SymbolInfo
  {
    public enum Types
    {
      UNKNOWN = 0,
      LABEL,
      PREPROCESSOR_LABEL,
      PREPROCESSOR_CONSTANT_1,
      PREPROCESSOR_CONSTANT_2,
      CONSTANT_1,
      CONSTANT_2,
      ZONE,
      CONSTANT_F
    };

    public Types      Type = Types.UNKNOWN;
    public string     Name = "";
    public int        LineIndex = 0;            // global
    public int        LineCount = -1;           // global (-1 is for complete file)
    public string     DocumentFilename = "";
    public int        LocalLineIndex = 0;
    public int        AddressOrValue = -1;
    public double     RealValue = 0;
    public bool       Used = false;
    public string     Zone = "";
    public bool       FromDependency = false;
    public string     Info = "";
    public int        CharIndex = -1;
    public int        Length = 0;
    public ASM.SourceInfo SourceInfo = null;



    public override string ToString()
    {
      return Name;
    }
  }


  public class C64Character
  {
    public byte           ScreenCodeValue = 0;
    public bool           HasScreenCode = false;
    public byte           PetSCIIValue = 0;
    public bool           HasPetSCII = false;
    public char           CharValue = ' ';
    public bool           HasChar = false;
    public char           LowerCaseDisplayChar = 'X';
    public char           LowerCaseInputChar = ' ';
    public byte           LowerCasePETSCII = 0;
    public string         Desc = "";
    public string         ShortDesc = "";
    public KeyboardKey    PhysicalKey = KeyboardKey.UNDEFINED;
    public KeyModifier    Modifier = KeyModifier.NORMAL;
    public KeyType        Type = KeyType.NORMAL;
    public List<string>   Replacements = new List<string>();


    public C64Character( KeyboardKey PhysicalKey, byte ScreenCodeValue, bool HasScreenCode, byte PetSCIIValue, bool HasPetSCII, char CharValue, bool HasChar, string Desc, string ShortDesc )
    {
      this.PhysicalKey = PhysicalKey;
      this.ScreenCodeValue = ScreenCodeValue;
      this.HasScreenCode = HasScreenCode;
      this.PetSCIIValue = PetSCIIValue;
      this.HasPetSCII = HasPetSCII;
      this.CharValue = CharValue;
      this.HasChar = HasChar;
      this.Desc = Desc;
      this.ShortDesc = ShortDesc;
    }
  };

  public class C64Key
  {
    public C64Character      Normal = null;
    public C64Character      WithShift = null;
    public C64Character      WithControl = null;
    public C64Character      WithCommodore = null;
  };

  public class ConstantData
  {
    public static GR.Memory.ByteBuffer UpperCaseCharset = new GR.Memory.ByteBuffer( "3C666E6E60623C00183C667E666666007C66667C66667C003C66606060663C00786C6666666C78007E60607860607E007E606078606060003C66606E66663C006666667E666666003C18181818183C001E0C0C0C0C6C3800666C7870786C66006060606060607E0063777F6B6363630066767E7E6E6666003C66666666663C007C66667C606060003C666666663C0E007C66667C786C66003C66603C06663C007E181818181818006666666666663C0066666666663C18006363636B7F77630066663C183C6666006666663C181818007E060C1830607E003C30303030303C000C12307C3062FC003C0C0C0C0C0C3C0000183C7E181818180010307F7F3010000000000000000000181818180000180066666600000000006666FF66FF666600183E603C067C180062660C18306646003C663C3867663F00060C1800000000000C18303030180C0030180C0C0C18300000663CFF3C6600000018187E1818000000000000001818300000007E0000000000000000001818000003060C183060003C666E7666663C001818381818187E003C66060C30607E003C66061C06663C00060E1E667F0606007E607C0606663C003C66607C66663C007E660C18181818003C66663C66663C003C66663E06663C00000018000018000000001800001818300E18306030180E0000007E007E00000070180C060C1870003C66060C18001800000000FFFF000000081C3E7F7F1C3E001818181818181818000000FFFF0000000000FFFF0000000000FFFF000000000000000000FFFF000030303030303030300C0C0C0C0C0C0C0C000000E0F038181818181C0F07000000181838F0E0000000C0C0C0C0C0C0FFFFC0E070381C0E070303070E1C3870E0C0FFFFC0C0C0C0C0C0FFFF030303030303003C7E7E7E7E3C000000000000FFFF00367F7F7F3E1C08006060606060606060000000070F1C1818C3E77E3C3C7EE7C3003C7E66667E3C001818666618183C000606060606060606081C3E7F3E1C0800181818FFFF181818C0C03030C0C0303018181818181818180000033E76363600FF7F3F1F0F0703010000000000000000F0F0F0F0F0F0F0F000000000FFFFFFFFFF0000000000000000000000000000FFC0C0C0C0C0C0C0C0CCCC3333CCCC3333030303030303030300000000CCCC3333FFFEFCF8F0E0C08003030303030303031818181F1F181818000000000F0F0F0F1818181F1F000000000000F8F8181818000000000000FFFF0000001F1F181818181818FFFF000000000000FFFF181818181818F8F8181818C0C0C0C0C0C0C0C0E0E0E0E0E0E0E0E00707070707070707FFFF000000000000FFFFFF00000000000000000000FFFFFF030303030303FFFF00000000F0F0F0F00F0F0F0F00000000181818F8F8000000F0F0F0F000000000F0F0F0F00F0F0F0FC39991919F99C3FFE7C39981999999FF83999983999983FFC3999F9F9F99C3FF87939999999387FF819F9F879F9F81FF819F9F879F9F9FFFC3999F919999C3FF99999981999999FFC3E7E7E7E7E7C3FFE1F3F3F3F393C7FF9993878F879399FF9F9F9F9F9F9F81FF9C8880949C9C9CFF99898181919999FFC39999999999C3FF839999839F9F9FFFC399999999C3F1FF83999983879399FFC3999FC3F999C3FF81E7E7E7E7E7E7FF999999999999C3FF9999999999C3E7FF9C9C9C9480889CFF9999C3E7C39999FF999999C3E7E7E7FF81F9F3E7CF9F81FFC3CFCFCFCFCFC3FFF3EDCF83CF9D03FFC3F3F3F3F3F3C3FFFFE7C381E7E7E7E7FFEFCF8080CFEFFFFFFFFFFFFFFFFFFFE7E7E7E7FFFFE7FF999999FFFFFFFFFF99990099009999FFE7C19FC3F983E7FF9D99F3E7CF99B9FFC399C3C79899C0FFF9F3E7FFFFFFFFFFF3E7CFCFCFE7F3FFCFE7F3F3F3E7CFFFFF99C300C399FFFFFFE7E781E7E7FFFFFFFFFFFFFFE7E7CFFFFFFF81FFFFFFFFFFFFFFFFFFE7E7FFFFFCF9F3E7CF9FFFC39991899999C3FFE7E7C7E7E7E781FFC399F9F3CF9F81FFC399F9E3F999C3FFF9F1E19980F9F9FF819F83F9F999C3FFC3999F839999C3FF8199F3E7E7E7E7FFC39999C39999C3FFC39999C1F999C3FFFFFFE7FFFFE7FFFFFFFFE7FFFFE7E7CFF1E7CF9FCFE7F1FFFFFF81FF81FFFFFF8FE7F3F9F3E78FFFC399F9F3E7FFE7FFFFFFFF0000FFFFFFF7E3C18080E3C1FFE7E7E7E7E7E7E7E7FFFFFF0000FFFFFFFFFF0000FFFFFFFFFF0000FFFFFFFFFFFFFFFFFF0000FFFFCFCFCFCFCFCFCFCFF3F3F3F3F3F3F3F3FFFFFF1F0FC7E7E7E7E7E3F0F8FFFFFFE7E7C70F1FFFFFFF3F3F3F3F3F3F00003F1F8FC7E3F1F8FCFCF8F1E3C78F1F3F00003F3F3F3F3F3F0000FCFCFCFCFCFCFFC381818181C3FFFFFFFFFFFF0000FFC9808080C1E3F7FF9F9F9F9F9F9F9F9FFFFFFFF8F0E3E7E73C1881C3C381183CFFC381999981C3FFE7E79999E7E7C3FFF9F9F9F9F9F9F9F9F7E3C180C1E3F7FFE7E7E70000E7E7E73F3FCFCF3F3FCFCFE7E7E7E7E7E7E7E7FFFFFCC189C9C9FF0080C0E0F0F8FCFEFFFFFFFFFFFFFFFF0F0F0F0F0F0F0F0FFFFFFFFF0000000000FFFFFFFFFFFFFFFFFFFFFFFFFFFF003F3F3F3F3F3F3F3F3333CCCC3333CCCCFCFCFCFCFCFCFCFCFFFFFFFF3333CCCC000103070F1F3F7FFCFCFCFCFCFCFCFCE7E7E7E0E0E7E7E7FFFFFFFFF0F0F0F0E7E7E7E0E0FFFFFFFFFFFF0707E7E7E7FFFFFFFFFFFF0000FFFFFFE0E0E7E7E7E7E7E70000FFFFFFFFFFFF0000E7E7E7E7E7E70707E7E7E73F3F3F3F3F3F3F3F1F1F1F1F1F1F1F1FF8F8F8F8F8F8F8F80000FFFFFFFFFFFF000000FFFFFFFFFFFFFFFFFFFF000000FCFCFCFCFCFC0000FFFFFFFF0F0F0F0FF0F0F0F0FFFFFFFFE7E7E70707FFFFFF0F0F0F0FFFFFFFFF0F0F0F0FF0F0F0F0" );
    public static GR.Memory.ByteBuffer LowerCaseCharset = new GR.Memory.ByteBuffer( "3C666E6E60623C0000003C063E663E000060607C66667C0000003C6060603C000006063E66663E0000003C667E603C00000E183E1818180000003E66663E067C0060607C666666000018003818183C00000600060606063C0060606C786C66000038181818183C000000667F7F6B630000007C666666660000003C6666663C0000007C66667C606000003E66663E060600007C666060600000003E603C067C0000187E1818180E000000666666663E0000006666663C18000000636B7F3E36000000663C183C660000006666663E0C7800007E0C18307E003C30303030303C000C12307C3062FC003C0C0C0C0C0C3C0000183C7E181818180010307F7F3010000000000000000000181818180000180066666600000000006666FF66FF666600183E603C067C180062660C18306646003C663C3867663F00060C1800000000000C18303030180C0030180C0C0C18300000663CFF3C6600000018187E1818000000000000001818300000007E0000000000000000001818000003060C183060003C666E7666663C001818381818187E003C66060C30607E003C66061C06663C00060E1E667F0606007E607C0606663C003C66607C66663C007E660C18181818003C66663C66663C003C66663E06663C00000018000018000000001800001818300E18306030180E0000007E007E00000070180C060C1870003C66060C18001800000000FFFF000000183C667E666666007C66667C66667C003C66606060663C00786C6666666C78007E60607860607E007E606078606060003C66606E66663C006666667E666666003C18181818183C001E0C0C0C0C6C3800666C7870786C66006060606060607E0063777F6B6363630066767E7E6E6666003C66666666663C007C66667C606060003C666666663C0E007C66667C786C66003C66603C06663C007E181818181818006666666666663C0066666666663C18006363636B7F77630066663C183C6666006666663C181818007E060C1830607E00181818FFFF181818C0C03030C0C0303018181818181818183333CCCC3333CCCC3399CC663399CC660000000000000000F0F0F0F0F0F0F0F000000000FFFFFFFFFF0000000000000000000000000000FFC0C0C0C0C0C0C0C0CCCC3333CCCC3333030303030303030300000000CCCC3333CC993366CC99336603030303030303031818181F1F181818000000000F0F0F0F1818181F1F000000000000F8F8181818000000000000FFFF0000001F1F181818181818FFFF000000000000FFFF181818181818F8F8181818C0C0C0C0C0C0C0C0E0E0E0E0E0E0E0E00707070707070707FFFF000000000000FFFFFF00000000000000000000FFFFFF0103066C7870600000000000F0F0F0F00F0F0F0F00000000181818F8F8000000F0F0F0F000000000F0F0F0F00F0F0F0FC39991919F99C3FFFFFFC3F9C199C1FFFF9F9F83999983FFFFFFC39F9F9FC3FFFFF9F9C19999C1FFFFFFC399819FC3FFFFF1E7C1E7E7E7FFFFFFC19999C1F983FF9F9F83999999FFFFE7FFC7E7E7C3FFFFF9FFF9F9F9F9C3FF9F9F93879399FFFFC7E7E7E7E7C3FFFFFF998080949CFFFFFF8399999999FFFFFFC3999999C3FFFFFF839999839F9FFFFFC19999C1F9F9FFFF83999F9F9FFFFFFFC19FC3F983FFFFE781E7E7E7F1FFFFFF99999999C1FFFFFF999999C3E7FFFFFF9C9480C1C9FFFFFF99C3E7C399FFFFFF999999C1F387FFFF81F3E7CF81FFC3CFCFCFCFCFC3FFF3EDCF83CF9D03FFC3F3F3F3F3F3C3FFFFE7C381E7E7E7E7FFEFCF8080CFEFFFFFFFFFFFFFFFFFFFE7E7E7E7FFFFE7FF999999FFFFFFFFFF99990099009999FFE7C19FC3F983E7FF9D99F3E7CF99B9FFC399C3C79899C0FFF9F3E7FFFFFFFFFFF3E7CFCFCFE7F3FFCFE7F3F3F3E7CFFFFF99C300C399FFFFFFE7E781E7E7FFFFFFFFFFFFFFE7E7CFFFFFFF81FFFFFFFFFFFFFFFFFFE7E7FFFFFCF9F3E7CF9FFFC39991899999C3FFE7E7C7E7E7E781FFC399F9F3CF9F81FFC399F9E3F999C3FFF9F1E19980F9F9FF819F83F9F999C3FFC3999F839999C3FF8199F3E7E7E7E7FFC39999C39999C3FFC39999C1F999C3FFFFFFE7FFFFE7FFFFFFFFE7FFFFE7E7CFF1E7CF9FCFE7F1FFFFFF81FF81FFFFFF8FE7F3F9F3E78FFFC399F9F3E7FFE7FFFFFFFF0000FFFFFFE7C39981999999FF83999983999983FFC3999F9F9F99C3FF87939999999387FF819F9F879F9F81FF819F9F879F9F9FFFC3999F919999C3FF99999981999999FFC3E7E7E7E7E7C3FFE1F3F3F3F393C7FF9993878F879399FF9F9F9F9F9F9F81FF9C8880949C9C9CFF99898181919999FFC39999999999C3FF839999839F9F9FFFC399999999C3F1FF83999983879399FFC3999FC3F999C3FF81E7E7E7E7E7E7FF999999999999C3FF9999999999C3E7FF9C9C9C9480889CFF9999C3E7C39999FF999999C3E7E7E7FF81F9F3E7CF9F81FFE7E7E70000E7E7E73F3FCFCF3F3FCFCFE7E7E7E7E7E7E7E7CCCC3333CCCC3333CC663399CC663399FFFFFFFFFFFFFFFF0F0F0F0F0F0F0F0FFFFFFFFF0000000000FFFFFFFFFFFFFFFFFFFFFFFFFFFF003F3F3F3F3F3F3F3F3333CCCC3333CCCCFCFCFCFCFCFCFCFCFFFFFFFF3333CCCC3366CC993366CC99FCFCFCFCFCFCFCFCE7E7E7E0E0E7E7E7FFFFFFFFF0F0F0F0E7E7E7E0E0FFFFFFFFFFFF0707E7E7E7FFFFFFFFFFFF0000FFFFFFE0E0E7E7E7E7E7E70000FFFFFFFFFFFF0000E7E7E7E7E7E70707E7E7E73F3F3F3F3F3F3F3F1F1F1F1F1F1F1F1FF8F8F8F8F8F8F8F80000FFFFFFFFFFFF000000FFFFFFFFFFFFFFFFFFFF000000FEFCF993878F9FFFFFFFFFFF0F0F0F0FF0F0F0F0FFFFFFFFE7E7E70707FFFFFF0F0F0F0FFFFFFFFF0F0F0F0FF0F0F0F0" );

    public static Palette Palette           = new Palette();

    public static GR.Collections.Map<char, byte> PETSCII = new GR.Collections.Map<char, byte>();
    public static GR.Collections.Map<byte, char> PETSCIIToUnicode = new GR.Collections.Map<byte, char>();

    public static GR.Collections.Map<byte, C64Character>    ScreenCodeToChar = new GR.Collections.Map<byte, C64Character>();
    public static GR.Collections.Map<byte, C64Character>    PetSCIIToChar = new GR.Collections.Map<byte, C64Character>();
    public static GR.Collections.Map<char, C64Character>    CharToC64Char = new GR.Collections.Map<char,C64Character>();
    public static GR.Collections.Map<char, C64Character>    LowerCaseCharTo64Char = new GR.Collections.Map<char, C64Character>();
    public static GR.Collections.Map<byte, byte>            ColorToPetSCIIChar = new GR.Collections.Map<byte, byte>();
    public static GR.Collections.Map<KeyboardKey, C64Key>   PhysicalKeyInfo = new GR.Collections.Map<KeyboardKey,C64Key>();
    public static List<C64Character>                        AllPhysicalKeyInfos = new List<C64Character>();



    static ConstantData()
    {
      Palette.ColorValues[0] = 0xff000000;
      Palette.ColorValues[1] = 0xffffffff;
      Palette.ColorValues[2] = 0xff8B4131;
      Palette.ColorValues[3] = 0xff7BBDC5;
      Palette.ColorValues[4] = 0xff8B41AC;
      Palette.ColorValues[5] = 0xff6AAC41;
      Palette.ColorValues[6] = 0xff3931A4;
      Palette.ColorValues[7] = 0xffD5DE73;
      Palette.ColorValues[8] = 0xff945A20;
      Palette.ColorValues[9] = 0xff5A4100;
      Palette.ColorValues[10] = 0xffBD736A;
      Palette.ColorValues[11] = 0xff525252;
      Palette.ColorValues[12] = 0xff838383;
      Palette.ColorValues[13] = 0xffACEE8B;
      Palette.ColorValues[14] = 0xff7B73DE;
      Palette.ColorValues[15] = 0xffACACAC;
      Palette.ColorValues[16] = 0xff80ff80;
      for ( int i = 0; i < 16; ++i )
      {
        Palette.Colors[i] = GR.Color.Helper.FromARGB( Palette.ColorValues[i] );
        Palette.ColorBrushes[i] = new System.Drawing.SolidBrush( Palette.Colors[i] );
      }

      ColorToPetSCIIChar[0] = 144;
      ColorToPetSCIIChar[1] = 5;
      ColorToPetSCIIChar[2] = 28;
      ColorToPetSCIIChar[3] = 159;
      ColorToPetSCIIChar[4] = 156;
      ColorToPetSCIIChar[5] = 30;
      ColorToPetSCIIChar[6] = 31;
      ColorToPetSCIIChar[7] = 158;
      ColorToPetSCIIChar[8] = 129;
      ColorToPetSCIIChar[9] = 149;
      ColorToPetSCIIChar[10] = 150;
      ColorToPetSCIIChar[11] = 151;
      ColorToPetSCIIChar[12] = 152;
      ColorToPetSCIIChar[13] = 153;
      ColorToPetSCIIChar[14] = 154;
      ColorToPetSCIIChar[15] = 155;

      PETSCII[' '] = 32;
      PETSCII['!'] = 33;
      PETSCII['"'] = 34;
      PETSCII['#'] = 35;
      PETSCII['$'] = 36;
      PETSCII['%'] = 37;
      PETSCII['&'] = 38;
      PETSCII['\''] = 39;
      PETSCII['('] = 40;
      PETSCII[')'] = 41;
      PETSCII['*'] = 42;
      PETSCII['+'] = 43;
      PETSCII[','] = 44;
      PETSCII['-'] = 45;
      PETSCII['.'] = 46;
      PETSCII['/'] = 47;
      PETSCII['0'] = 48;
      PETSCII['1'] = 49;
      PETSCII['2'] = 50;
      PETSCII['3'] = 51;
      PETSCII['4'] = 52;
      PETSCII['5'] = 53;
      PETSCII['6'] = 54;
      PETSCII['7'] = 55;
      PETSCII['8'] = 56;
      PETSCII['9'] = 57;
      PETSCII[':'] = 58;
      PETSCII[';'] = 59;
      PETSCII['<'] = 60;
      PETSCII['='] = 61;
      PETSCII['>'] = 62;
      PETSCII['?'] = 63;
      PETSCII['@'] = 64;
      PETSCII['A'] = 65;
      PETSCII['B'] = 66;
      PETSCII['C'] = 67;
      PETSCII['D'] = 68;
      PETSCII['E'] = 69;
      PETSCII['F'] = 70;
      PETSCII['G'] = 71;
      PETSCII['H'] = 72;
      PETSCII['I'] = 73;
      PETSCII['J'] = 74;
      PETSCII['K'] = 75;
      PETSCII['L'] = 76;
      PETSCII['M'] = 77;
      PETSCII['N'] = 78;
      PETSCII['O'] = 79;
      PETSCII['P'] = 80;
      PETSCII['Q'] = 81;
      PETSCII['R'] = 82;
      PETSCII['S'] = 83;
      PETSCII['T'] = 84;
      PETSCII['U'] = 85;
      PETSCII['V'] = 86;
      PETSCII['W'] = 87;
      PETSCII['X'] = 88;
      PETSCII['Y'] = 89;
      PETSCII['Z'] = 90;
      PETSCII['['] = 91;
      PETSCII['£'] = 92;
      PETSCII[']'] = 93;
      //PETSCII['_'] = 127;
      //PETSCII['\''] = 95;
      //PETSCII['{'] = 123;
      PETSCII['|'] = 166;
      //PETSCII['}'] = 125;
      PETSCII[''] = 127;

      foreach ( KeyValuePair<char,byte> unicode in PETSCII )
      {
        PETSCIIToUnicode[unicode.Value] = unicode.Key;
      }

      // Commodore 64 PETSCII code to screen code conversion
      // PETSCII code
      // (dec, hex)       Change          (dec, hex) Screen code (dec, hex)  
      //   0- 31 $00-$1F    +128 $80        128-159 $80-$9F  
      //  32- 63 $20-$3F       0 $00         32- 63 $20-$3F  
      //  64- 95 $40-$5F     -64 $C0          0- 31 $00-$1F  
      //  96-127 $60-$7F     -32 $E0         64- 95 $40-$5F  
      // 128-159 $80-$9F     +64 $40        192-223 $C0-$DF  
      // 160-191 $A0-$BF     -64 $C0         96-127 $60-$7F  
      // 192-223 $C0-$DF    -128 $80         64- 95 $40-$5F  
      // 224-254 $E0-$FE    -128 $80         96-126 $60-$7E  
      // 255 $FF                                 94 $5E  


      //   0- 31 $00-$1F    +128 $80        128-159 $80-$9F  
      // TODO - Screencodes!!
      AddC64Key( KeyboardKey.KEY_AT, KeyModifier.CONTROL, KeyType.CONTROL_CODE, 128, true, 0, false, (char)0xee80, true, "REVERSE @" ).Replacements.Add( "CTRL-@" );
      AddC64Key( KeyboardKey.KEY_A, KeyModifier.CONTROL, KeyType.CONTROL_CODE, 129, true, 1, true, (char)0xee81, true, "REVERSE A" ).Replacements.Add( "CTRL-A" );
      AddC64Key( KeyboardKey.KEY_B, KeyModifier.CONTROL, KeyType.CONTROL_CODE, 130, true, 2, true, (char)0xee82, true, "REVERSE B" ).Replacements.Add( "CTRL-B" );

      AddC64Key( KeyboardKey.KEY_RUN_STOP, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 131, true, 3, true, (char)0xee83, true, "RUN STOP" ).Replacements.Add( "CTRL-C" ); ;   // run stop

      AddC64Key( KeyboardKey.KEY_D, KeyModifier.CONTROL, KeyType.CONTROL_CODE, 132, true, 4, true, (char)0xee84, true, "REVERSE D" ).Replacements.Add( "CTRL-D" );

      AddC64Key( KeyboardKey .KEY_2, KeyModifier.CONTROL, KeyType.CONTROL_CODE, 133, true, 5, true, (char)0xee85, -1, 0, true, "WHITE", "WHI" ).Replacements.AddRange( new string[] { "WHITE", "WHT" } );

      AddC64Key( KeyboardKey.KEY_F, KeyModifier.CONTROL, KeyType.CONTROL_CODE, 134, true, 6, true, (char)0xee86, true, "REVERSE F" ).Replacements.Add( "CTRL-F" );
      AddC64Key( KeyboardKey.KEY_G, KeyModifier.CONTROL, KeyType.CONTROL_CODE, 135, true, 7, true, (char)0xee87, true, "REVERSE G" ).Replacements.Add( "CTRL-G" );

      AddC64Key( KeyboardKey.KEY_H, KeyModifier.CONTROL, KeyType.CONTROL_CODE, 136, true, 8, true, (char)0xee88, -1, 0, true, "SHIFT C= OFF", "SH C= OFF" ).Replacements.AddRange( new string[] { "CTRL-H", "DISH" } );   // Shift-C= aus
      AddC64Key( KeyboardKey.KEY_I, KeyModifier.CONTROL, KeyType.CONTROL_CODE, 137, true, 9, true, (char)0xee89, -1, 0, true, "SHIFT C= ON", "SH C= ON" ).Replacements.AddRange( new string[] { "CTRL-I", "ENSH" } );   // Shift-C= an

      AddC64Key( KeyboardKey.KEY_J, KeyModifier.CONTROL, KeyType.CONTROL_CODE, 138, true, 10, true, (char)0xee8a, true, "REVERSE J" ).Replacements.Add( "CTRL-J" );
      AddC64Key( KeyboardKey.KEY_K, KeyModifier.CONTROL, KeyType.CONTROL_CODE, 139, true, 11, true, (char)0xee8b, true, "REVERSE K" ).Replacements.Add( "CTRL-K" );
      AddC64Key( KeyboardKey.KEY_L, KeyModifier.CONTROL, KeyType.CONTROL_CODE, 140, true, 12, true, (char)0xee8c, true, "REVERSE L" ).Replacements.Add( "CTRL-L" );

      AddC64Key( KeyboardKey.KEY_RETURN, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 141, true, 13, true, (char)0xee8d, -1, 0, true, "RETURN", "RET" ).Replacements.Add( "CTRL-M" );  // return
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 142, true, 14, true, (char)0xee8e, -1, 0, true, "SHIFT C=", "SH C=" ).Replacements.AddRange( new string[] { "SWLC" } );  // toggle upper/lower

      AddC64Key( KeyboardKey.KEY_O, KeyModifier.CONTROL, KeyType.CONTROL_CODE, 143, true, 15, true, (char)0xee8f, true, "REVERSE O" ).Replacements.Add( "CTRL-O" );
      AddC64Key( KeyboardKey.KEY_P, KeyModifier.CONTROL, KeyType.CONTROL_CODE, 144, true, 16, true, (char)0xee90, true, "REVERSE P" ).Replacements.Add( "CTRL-P" );

      AddC64Key( KeyboardKey.KEY_CURSOR_UP_DOWN, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 128 + 17, true, 17, true, (char)0xee91, -1, 0, true, "CURSOR DOWN", "CUR DOWN" ).Replacements.Add( "DOWN" );
      AddC64Key( KeyboardKey.KEY_9, KeyModifier.CONTROL, KeyType.CONTROL_CODE, 128 + 18, true, 18, true, (char)0xee92, -1, 0, true, "REVERSE ON", "RVS ON" ).Replacements.AddRange( new string[] { "RVSON", "RVON", "RVS", "REVERSE ON" } );
      AddC64Key( KeyboardKey.KEY_CLR_HOME, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 128 + 19, true, 19, true, (char)0xee93, -1, 0, true, "HOME", "HOM" ).Replacements.AddRange( new string[] { "HOME", "HOM" } );
      AddC64Key( KeyboardKey.KEY_INST_DEL, KeyModifier.NORMAL, KeyType.EDITOR_CONTROL_CODE, 128 + 20, true, 20, true, (char)0xee94, true, "DEL" ).Replacements.AddRange( new string[]{ "DEL", "DELETE" } );

      AddC64Key( KeyboardKey.KEY_U, KeyModifier.CONTROL, KeyType.CONTROL_CODE, 149, true, 21, true, (char)0xee95, true, "REVERSE U" ).Replacements.Add( "CTRL-U" );
      AddC64Key( KeyboardKey.KEY_V, KeyModifier.CONTROL, KeyType.CONTROL_CODE, 150, true, 22, true, (char)0xee96, true, "REVERSE V" ).Replacements.Add( "CTRL-V" );
      AddC64Key( KeyboardKey.KEY_W, KeyModifier.CONTROL, KeyType.CONTROL_CODE, 151, true, 23, true, (char)0xee97, true, "REVERSE W" ).Replacements.Add( "CTRL-W" );
      AddC64Key( KeyboardKey.KEY_X, KeyModifier.CONTROL, KeyType.CONTROL_CODE, 152, true, 24, true, (char)0xee98, true, "REVERSE X" ).Replacements.Add( "CTRL-X" );
      AddC64Key( KeyboardKey.KEY_Y, KeyModifier.CONTROL, KeyType.CONTROL_CODE, 153, true, 25, true, (char)0xee99, true, "REVERSE Y" ).Replacements.Add( "CTRL-Y" );
      AddC64Key( KeyboardKey.KEY_Z, KeyModifier.CONTROL, KeyType.CONTROL_CODE, 154, true, 26, true, (char)0xee9a, true, "REVERSE Z" ).Replacements.Add( "CTRL-Z" );
      AddC64Key( KeyboardKey.KEY_COLON, KeyModifier.SHIFT, KeyType.NORMAL, 155, true, 27, true, (char)0xee9b, true, "REVERSE [" );

      AddC64Key( KeyboardKey.KEY_3, KeyModifier.CONTROL, KeyType.CONTROL_CODE, 128 + 28, true, 28, true, (char)0xee9c, true, "RED" ).Replacements.Add( "RED" );
      AddC64Key( KeyboardKey.KEY_CURSOR_LEFT_RIGHT, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 128 + 29, true, 29, true, (char)0xee9d, -1, 0, true, "CURSOR RIGHT", "CUR RIGHT" ).Replacements.AddRange( new string[] { "RIGHT", "RGHT" } );
      AddC64Key( KeyboardKey.KEY_6, KeyModifier.CONTROL, KeyType.CONTROL_CODE, 128 + 30, true, 30, true, (char)0xee9e, -1, 0, true, "GREEN", "GRN" ).Replacements.AddRange( new string[] { "GREEN", "GRN" } );
      AddC64Key( KeyboardKey.KEY_7, KeyModifier.CONTROL, KeyType.CONTROL_CODE, 128 + 31, true, 31, true, (char)0xee9f, -1, 0, true, "BLUE", "BLU" ).Replacements.AddRange( new string[] { "BLUE", "BLU" } );
      //  32- 63 $20-$3F       0 $00         32- 63 $20-$3F  
      AddC64Key( KeyboardKey.KEY_SPACE, KeyModifier.NORMAL, KeyType.NORMAL, 32, true, 32, true, ' ', -1, 0, true, "SPACE", "SPC" ).Replacements.AddRange( new string[] { "SPACE", "SPACES" } );
      AddC64Key( KeyboardKey.KEY_1, KeyModifier.SHIFT, KeyType.NORMAL, 33, true, 33, true, '!', true );
      AddC64Key( KeyboardKey.KEY_2, KeyModifier.SHIFT, KeyType.NORMAL, 34, true, 34, true, '"', true );
      AddC64Key( KeyboardKey.KEY_3, KeyModifier.SHIFT, KeyType.NORMAL, 35, true, 35, true, '#', true );
      AddC64Key( KeyboardKey.KEY_4, KeyModifier.SHIFT, KeyType.NORMAL, 36, true, 36, true, '$', true );
      AddC64Key( KeyboardKey.KEY_5, KeyModifier.SHIFT, KeyType.NORMAL, 37, true, 37, true, '%', true );
      AddC64Key( KeyboardKey.KEY_6, KeyModifier.SHIFT, KeyType.NORMAL, 38, true, 38, true, '&', true );
      AddC64Key( KeyboardKey.KEY_7, KeyModifier.SHIFT, KeyType.NORMAL, 39, true, 39, true, '\'', true );
      AddC64Key( KeyboardKey.KEY_8, KeyModifier.SHIFT, KeyType.NORMAL, 40, true, 40, true, '(', true );
      AddC64Key( KeyboardKey.KEY_9, KeyModifier.SHIFT, KeyType.NORMAL, 41, true, 41, true, ')', true );
      AddC64Key( KeyboardKey.KEY_STAR, KeyModifier.NORMAL, KeyType.NORMAL, 42, true, 42, true, '*', true );
      AddC64Key( KeyboardKey.KEY_PLUS, KeyModifier.NORMAL, KeyType.NORMAL, 43, true, 43, true, '+', true );
      AddC64Key( KeyboardKey.KEY_COMMA, KeyModifier.NORMAL, KeyType.NORMAL, 44, true, 44, true, ',', true );
      AddC64Key( KeyboardKey.KEY_MINUS, KeyModifier.NORMAL, KeyType.NORMAL, 45, true, 45, true, '-', true );
      AddC64Key( KeyboardKey.KEY_DOT, KeyModifier.NORMAL, KeyType.NORMAL, 46, true, 46, true, '.', true );
      AddC64Key( KeyboardKey.KEY_SLASH, KeyModifier.NORMAL, KeyType.NORMAL, 47, true, 47, true, '/', true );
      AddC64Key( KeyboardKey.KEY_0, KeyModifier.NORMAL, KeyType.NORMAL, 48, true, 48, true, '0', true );
      AddC64Key( KeyboardKey.KEY_1, KeyModifier.NORMAL, KeyType.NORMAL, 49, true, 49, true, '1', true );
      AddC64Key( KeyboardKey.KEY_2, KeyModifier.NORMAL, KeyType.NORMAL, 50, true, 50, true, '2', true );
      AddC64Key( KeyboardKey.KEY_3, KeyModifier.NORMAL, KeyType.NORMAL, 51, true, 51, true, '3', true );
      AddC64Key( KeyboardKey.KEY_4, KeyModifier.NORMAL, KeyType.NORMAL, 52, true, 52, true, '4', true );
      AddC64Key( KeyboardKey.KEY_5, KeyModifier.NORMAL, KeyType.NORMAL, 53, true, 53, true, '5', true );
      AddC64Key( KeyboardKey.KEY_6, KeyModifier.NORMAL, KeyType.NORMAL, 54, true, 54, true, '6', true );
      AddC64Key( KeyboardKey.KEY_7, KeyModifier.NORMAL, KeyType.NORMAL, 55, true, 55, true, '7', true );
      AddC64Key( KeyboardKey.KEY_8, KeyModifier.NORMAL, KeyType.NORMAL, 56, true, 56, true, '8', true );
      AddC64Key( KeyboardKey.KEY_9, KeyModifier.NORMAL, KeyType.NORMAL, 57, true, 57, true, '9', true );
      AddC64Key( KeyboardKey.KEY_COLON, KeyModifier.NORMAL, KeyType.NORMAL, 58, true, 58, true, ':', true );
      AddC64Key( KeyboardKey.KEY_SEMI_COLON, KeyModifier.NORMAL, KeyType.NORMAL, 59, true, 59, true, ';', true );
      AddC64Key( KeyboardKey.KEY_COMMA, KeyModifier.SHIFT, KeyType.NORMAL, 60, true, 60, true, '<', true );
      AddC64Key( KeyboardKey.KEY_EQUAL, KeyModifier.NORMAL, KeyType.NORMAL, 61, true, 61, true, '=', true );
      AddC64Key( KeyboardKey.KEY_DOT, KeyModifier.SHIFT, KeyType.NORMAL, 62, true, 62, true, '>', true );
      AddC64Key( KeyboardKey.KEY_SLASH, KeyModifier.SHIFT, KeyType.NORMAL, 63, true, 63, true, '?', true );
      //  64- 95 $40-$5F     -64 $C0          0- 31 $00-$1F  
      AddC64Key( KeyboardKey.KEY_AT, KeyModifier.NORMAL, KeyType.NORMAL, 64 - 64, true, 64, true, '@', true );

      AddC64KeyLC( KeyboardKey.KEY_A, KeyModifier.NORMAL, KeyType.NORMAL, 65 - 64, true, 65, true, 'A', 'a', 0x41 );
      AddC64KeyLC( KeyboardKey.KEY_B, KeyModifier.NORMAL, KeyType.NORMAL, 66 - 64, true, 66, true, 'B', 'b', 0x42 );
      AddC64KeyLC( KeyboardKey.KEY_C, KeyModifier.NORMAL, KeyType.NORMAL, 67 - 64, true, 67, true, 'C', 'c', 0x43 );
      AddC64KeyLC( KeyboardKey.KEY_D, KeyModifier.NORMAL, KeyType.NORMAL, 68 - 64, true, 68, true, 'D', 'd', 0x44 );
      AddC64KeyLC( KeyboardKey.KEY_E, KeyModifier.NORMAL, KeyType.NORMAL, 69 - 64, true, 69, true, 'E', 'e', 0x45 );
      AddC64KeyLC( KeyboardKey.KEY_F, KeyModifier.NORMAL, KeyType.NORMAL, 70 - 64, true, 70, true, 'F', 'f', 0x46 );
      AddC64KeyLC( KeyboardKey.KEY_G, KeyModifier.NORMAL, KeyType.NORMAL, 71 - 64, true, 71, true, 'G', 'g', 0x47 );
      AddC64KeyLC( KeyboardKey.KEY_H, KeyModifier.NORMAL, KeyType.NORMAL, 72 - 64, true, 72, true, 'H', 'h', 0x48 );
      AddC64KeyLC( KeyboardKey.KEY_I, KeyModifier.NORMAL, KeyType.NORMAL, 73 - 64, true, 73, true, 'I', 'i', 0x49  );
      AddC64KeyLC( KeyboardKey.KEY_J, KeyModifier.NORMAL, KeyType.NORMAL, 74 - 64, true, 74, true, 'J', 'j', 0x4a  );
      AddC64KeyLC( KeyboardKey.KEY_K, KeyModifier.NORMAL, KeyType.NORMAL, 75 - 64, true, 75, true, 'K', 'k', 0x4b  );
      AddC64KeyLC( KeyboardKey.KEY_L, KeyModifier.NORMAL, KeyType.NORMAL, 76 - 64, true, 76, true, 'L', 'l', 0x4c  );
      AddC64KeyLC( KeyboardKey.KEY_M, KeyModifier.NORMAL, KeyType.NORMAL, 77 - 64, true, 77, true, 'M', 'm', 0x4d  );
      AddC64KeyLC( KeyboardKey.KEY_N, KeyModifier.NORMAL, KeyType.NORMAL, 78 - 64, true, 78, true, 'N', 'n', 0x4e  );
      AddC64KeyLC( KeyboardKey.KEY_O, KeyModifier.NORMAL, KeyType.NORMAL, 79 - 64, true, 79, true, 'O', 'o', 0x4f  );
      AddC64KeyLC( KeyboardKey.KEY_P, KeyModifier.NORMAL, KeyType.NORMAL, 80 - 64, true, 80, true, 'P', 'p', 0x50  );
      AddC64KeyLC( KeyboardKey.KEY_Q, KeyModifier.NORMAL, KeyType.NORMAL, 81 - 64, true, 81, true, 'Q', 'q', 0x51  );
      AddC64KeyLC( KeyboardKey.KEY_R, KeyModifier.NORMAL, KeyType.NORMAL, 82 - 64, true, 82, true, 'R', 'r', 0x52  );
      AddC64KeyLC( KeyboardKey.KEY_S, KeyModifier.NORMAL, KeyType.NORMAL, 83 - 64, true, 83, true, 'S', 's', 0x53  );
      AddC64KeyLC( KeyboardKey.KEY_T, KeyModifier.NORMAL, KeyType.NORMAL, 84 - 64, true, 84, true, 'T', 't', 0x54  );
      AddC64KeyLC( KeyboardKey.KEY_U, KeyModifier.NORMAL, KeyType.NORMAL, 85 - 64, true, 85, true, 'U', 'u', 0x55  );
      AddC64KeyLC( KeyboardKey.KEY_V, KeyModifier.NORMAL, KeyType.NORMAL, 86 - 64, true, 86, true, 'V', 'v', 0x56 );
      AddC64KeyLC( KeyboardKey.KEY_W, KeyModifier.NORMAL, KeyType.NORMAL, 87 - 64, true, 87, true, 'W', 'w', 0x57 );
      AddC64KeyLC( KeyboardKey.KEY_X, KeyModifier.NORMAL, KeyType.NORMAL, 88 - 64, true, 88, true, 'X', 'x', 0x58  );
      AddC64KeyLC( KeyboardKey.KEY_Y, KeyModifier.NORMAL, KeyType.NORMAL, 89 - 64, true, 89, true, 'Y', 'y', 0x59  );
      AddC64KeyLC( KeyboardKey.KEY_Z, KeyModifier.NORMAL, KeyType.NORMAL, 90 - 64, true, 90, true, 'Z', 'z', 0x5a );

      AddC64KeyLC( KeyboardKey.KEY_A, KeyModifier.NORMAL, KeyType.NORMAL, 65 - 64, true, 65, true, (char)0xe041, (char)0xe141, 0x41 );
      AddC64KeyLC( KeyboardKey.KEY_B, KeyModifier.NORMAL, KeyType.NORMAL, 66 - 64, true, 66, true, (char)0xe042, (char)0xe142, 0x42 );
      AddC64KeyLC( KeyboardKey.KEY_C, KeyModifier.NORMAL, KeyType.NORMAL, 67 - 64, true, 67, true, (char)0xe043, (char)0xe143, 0x43 );
      AddC64KeyLC( KeyboardKey.KEY_D, KeyModifier.NORMAL, KeyType.NORMAL, 68 - 64, true, 68, true, (char)0xe044, (char)0xe144, 0x44 );
      AddC64KeyLC( KeyboardKey.KEY_E, KeyModifier.NORMAL, KeyType.NORMAL, 69 - 64, true, 69, true, (char)0xe045, (char)0xe145, 0x45 );
      AddC64KeyLC( KeyboardKey.KEY_F, KeyModifier.NORMAL, KeyType.NORMAL, 70 - 64, true, 70, true, (char)0xe046, (char)0xe146, 0x46 );
      AddC64KeyLC( KeyboardKey.KEY_G, KeyModifier.NORMAL, KeyType.NORMAL, 71 - 64, true, 71, true, (char)0xe047, (char)0xe147, 0x47 );
      AddC64KeyLC( KeyboardKey.KEY_H, KeyModifier.NORMAL, KeyType.NORMAL, 72 - 64, true, 72, true, (char)0xe048, (char)0xe148, 0x48 );
      AddC64KeyLC( KeyboardKey.KEY_I, KeyModifier.NORMAL, KeyType.NORMAL, 73 - 64, true, 73, true, (char)0xe049, (char)0xe149, 0x49 );
      AddC64KeyLC( KeyboardKey.KEY_J, KeyModifier.NORMAL, KeyType.NORMAL, 74 - 64, true, 74, true, (char)0xe04a, (char)0xe14a, 0x4a );
      AddC64KeyLC( KeyboardKey.KEY_K, KeyModifier.NORMAL, KeyType.NORMAL, 75 - 64, true, 75, true, (char)0xe04b, (char)0xe14b, 0x4b );
      AddC64KeyLC( KeyboardKey.KEY_L, KeyModifier.NORMAL, KeyType.NORMAL, 76 - 64, true, 76, true, (char)0xe04c, (char)0xe14c, 0x4c );
      AddC64KeyLC( KeyboardKey.KEY_M, KeyModifier.NORMAL, KeyType.NORMAL, 77 - 64, true, 77, true, (char)0xe04d, (char)0xe14d, 0x4d );
      AddC64KeyLC( KeyboardKey.KEY_N, KeyModifier.NORMAL, KeyType.NORMAL, 78 - 64, true, 78, true, (char)0xe04e, (char)0xe14e, 0x4e );
      AddC64KeyLC( KeyboardKey.KEY_O, KeyModifier.NORMAL, KeyType.NORMAL, 79 - 64, true, 79, true, (char)0xe04f, (char)0xe14f, 0x4f );
      AddC64KeyLC( KeyboardKey.KEY_P, KeyModifier.NORMAL, KeyType.NORMAL, 80 - 64, true, 80, true, (char)0xe050, (char)0xe150, 0x50 );
      AddC64KeyLC( KeyboardKey.KEY_Q, KeyModifier.NORMAL, KeyType.NORMAL, 81 - 64, true, 81, true, (char)0xe051, (char)0xe151, 0x51 );
      AddC64KeyLC( KeyboardKey.KEY_R, KeyModifier.NORMAL, KeyType.NORMAL, 82 - 64, true, 82, true, (char)0xe052, (char)0xe152, 0x52 );
      AddC64KeyLC( KeyboardKey.KEY_S, KeyModifier.NORMAL, KeyType.NORMAL, 83 - 64, true, 83, true, (char)0xe053, (char)0xe153, 0x53 );
      AddC64KeyLC( KeyboardKey.KEY_T, KeyModifier.NORMAL, KeyType.NORMAL, 84 - 64, true, 84, true, (char)0xe054, (char)0xe154, 0x54 );
      AddC64KeyLC( KeyboardKey.KEY_U, KeyModifier.NORMAL, KeyType.NORMAL, 85 - 64, true, 85, true, (char)0xe055, (char)0xe155, 0x55 );
      AddC64KeyLC( KeyboardKey.KEY_V, KeyModifier.NORMAL, KeyType.NORMAL, 86 - 64, true, 86, true, (char)0xe056, (char)0xe156, 0x56 );
      AddC64KeyLC( KeyboardKey.KEY_W, KeyModifier.NORMAL, KeyType.NORMAL, 87 - 64, true, 87, true, (char)0xe057, (char)0xe157, 0x57 );
      AddC64KeyLC( KeyboardKey.KEY_X, KeyModifier.NORMAL, KeyType.NORMAL, 88 - 64, true, 88, true, (char)0xe058, (char)0xe158, 0x58 );
      AddC64KeyLC( KeyboardKey.KEY_Y, KeyModifier.NORMAL, KeyType.NORMAL, 89 - 64, true, 89, true, (char)0xe059, (char)0xe159, 0x59 );
      AddC64KeyLC( KeyboardKey.KEY_Z, KeyModifier.NORMAL, KeyType.NORMAL, 90 - 64, true, 90, true, (char)0xe05a, (char)0xe15a, 0x5a );

      AddC64Key( KeyboardKey.KEY_COLON, KeyModifier.SHIFT, KeyType.NORMAL, 91 - 64, true, 91, true, '[', true );//(char)0xee1b, true );//'[', true );
      AddC64Key( KeyboardKey.KEY_POUND, KeyModifier.NORMAL, KeyType.NORMAL, 92 - 64, true, 92, true, '£', true ).Replacements.Add( "POUND" );
      AddC64Key( KeyboardKey.KEY_SEMI_COLON, KeyModifier.SHIFT, KeyType.NORMAL, 93 - 64, true, 93, true, ']', true );//(char)0xee1d, true );// ']', true );
      AddC64Key( KeyboardKey.KEY_ARROW_UP, KeyModifier.NORMAL, KeyType.NORMAL, 94 - 64, true, 94, true, '^', true, "ARROW UP" ).Replacements.Add( "ARROW UP" );
      AddC64Key( KeyboardKey.KEY_ARROW_UP, KeyModifier.NORMAL, KeyType.NORMAL, 94 - 64, true, 94, true, (char)0xee1e, true, "ARROW UP" ).Replacements.Add( "ARROW UP" );
      AddC64Key( KeyboardKey.KEY_ARROW_LEFT, KeyModifier.NORMAL, KeyType.NORMAL, 95 - 64, true, 95, true, (char)0xee1f, true, "ARROW LEFT" ).Replacements.Add( "ARROW LEFT" ); // arrow left
      //  96-127 $60-$7F     -32 $E0         64- 95 $40-$5F  
      AddC64Key( KeyboardKey.KEY_STAR, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 96 - 32, true, 96, true, (char)0xee40, true, "SHIFT *" ).Replacements.Add( "SHIFT-*" );   // Shift *
      AddC64Key( KeyboardKey.KEY_A, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 97 - 32, true, 97, true, (char)0xee41, true, "SHIFT A" ).Replacements.Add( "SHIFT-A" );   // Shift-A
      AddC64Key( KeyboardKey.KEY_B, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 98 - 32, true, 98, true, (char)0xee42, true, "SHIFT B" ).Replacements.Add( "SHIFT-B" );   // Shift-B
      AddC64Key( KeyboardKey.KEY_C, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 99 - 32, true, 99, true, (char)0xee43, true, "SHIFT C" ).Replacements.Add( "SHIFT-C" );   // Shift-C
      AddC64Key( KeyboardKey.KEY_D, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 100 - 32, true, 100, true, (char)0xee44, true, "SHIFT D" ).Replacements.Add( "SHIFT-D" );   // Shift-D
      AddC64Key( KeyboardKey.KEY_E, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 101 - 32, true, 101, true, (char)0xee45, true, "SHIFT E" ).Replacements.Add( "SHIFT-E" );   // Shift-E
      AddC64Key( KeyboardKey.KEY_F, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 102 - 32, true, 102, true, (char)0xee46, true, "SHIFT F" ).Replacements.Add( "SHIFT-F" );   // Shift-F
      AddC64Key( KeyboardKey.KEY_G, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 103 - 32, true, 103, true, (char)0xee47, true, "SHIFT G" ).Replacements.Add( "SHIFT-G" );   // Shift-G
      AddC64Key( KeyboardKey.KEY_H, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 104 - 32, true, 104, true, (char)0xee48, true, "SHIFT H" ).Replacements.Add( "SHIFT-H" );   // Shift-H
      AddC64Key( KeyboardKey.KEY_I, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 105 - 32, true, 105, true, (char)0xee49, true, "SHIFT I" ).Replacements.Add( "SHIFT-I" );   // Shift-I
      AddC64Key( KeyboardKey.KEY_J, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 106 - 32, true, 106, true, (char)0xee4a, true, "SHIFT J" ).Replacements.Add( "SHIFT-J" );   // Shift-J
      AddC64Key( KeyboardKey.KEY_K, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 107 - 32, true, 107, true, (char)0xee4b, true, "SHIFT K" ).Replacements.Add( "SHIFT-K" );   // Shift-K
      AddC64Key( KeyboardKey.KEY_L, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 108 - 32, true, 108, true, (char)0xee4c, true, "SHIFT L" ).Replacements.Add( "SHIFT-L" );   // Shift-L
      AddC64Key( KeyboardKey.KEY_M, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 109 - 32, true, 109, true, (char)0xee4d, true, "SHIFT M" ).Replacements.Add( "SHIFT-M" );   // Shift-M
      AddC64Key( KeyboardKey.KEY_N, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 110 - 32, true, 110, true, (char)0xee4e, true, "SHIFT N" ).Replacements.Add( "SHIFT-N" );   // Shift-N
      AddC64Key( KeyboardKey.KEY_O, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 111 - 32, true, 111, true, (char)0xee4f, true, "SHIFT O" ).Replacements.Add( "SHIFT-O" );   // Shift-O
      AddC64Key( KeyboardKey.KEY_P, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 112 - 32, true, 112, true, (char)0xee50, true, "SHIFT P" ).Replacements.Add( "SHIFT-P" );   // Shift-P
      AddC64Key( KeyboardKey.KEY_Q, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 113 - 32, true, 113, true, (char)0xee51, true, "SHIFT Q" ).Replacements.Add( "SHIFT-Q" );   // Shift-Q
      AddC64Key( KeyboardKey.KEY_R, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 114 - 32, true, 114, true, (char)0xee52, true, "SHIFT R" ).Replacements.Add( "SHIFT-R" );   // Shift-R
      AddC64Key( KeyboardKey.KEY_S, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 115 - 32, true, 115, true, (char)0xee53, true, "SHIFT S" ).Replacements.Add( "SHIFT-S" );   // Shift-S
      AddC64Key( KeyboardKey.KEY_T, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 116 - 32, true, 116, true, (char)0xee54, true, "SHIFT T" ).Replacements.Add( "SHIFT-T" );   // Shift-T
      AddC64Key( KeyboardKey.KEY_U, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 117 - 32, true, 117, true, (char)0xee55, true, "SHIFT U" ).Replacements.Add( "SHIFT-U" );   // Shift-U
      AddC64Key( KeyboardKey.KEY_V, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 118 - 32, true, 118, true, (char)0xee56, true, "SHIFT V" ).Replacements.Add( "SHIFT-V" );   // Shift-V
      AddC64Key( KeyboardKey.KEY_W, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 119 - 32, true, 119, true, (char)0xee57, true, "SHIFT W" ).Replacements.Add( "SHIFT-W" );   // Shift-W
      AddC64Key( KeyboardKey.KEY_X, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 120 - 32, true, 120, true, (char)0xee58, true, "SHIFT X" ).Replacements.Add( "SHIFT-X" );   // Shift-X
      AddC64Key( KeyboardKey.KEY_Y, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 121 - 32, true, 121, true, (char)0xee59, true, "SHIFT Y" ).Replacements.Add( "SHIFT-Y" );   // Shift-Y
      AddC64Key( KeyboardKey.KEY_Z, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 122 - 32, true, 122, true, (char)0xee5a, true, "SHIFT Z" ).Replacements.Add( "SHIFT-Z" );   // Shift-Z
      AddC64Key( KeyboardKey.KEY_PLUS, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 123 - 32, true, 123, true, (char)0xee5b, true, "SHIFT +" );   // Shift +
      AddC64Key( KeyboardKey.KEY_MINUS, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 124 - 32, true, 124, true, (char)0xee5c, true, "CBM -" ).Replacements.Add( "CBM--" );   // C= -
      AddC64Key( KeyboardKey.KEY_MINUS, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 125 - 32, true, 125, true, (char)0xee5d, true, "SHIFT -" ).Replacements.Add( "SHIFT--" );   // Shift -
      AddC64Key( KeyboardKey.KEY_ARROW_UP, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 126 - 32, true, 0xff, true, (char)0xee5e, true, "PI" ).Replacements.Add( "PI" );   // PI (Shift and Commodore)
      AddC64Key( KeyboardKey.KEY_STAR, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 127 - 32, true, 127, true, (char)0xee5f, true, "CBM *" );   // C= *

      // 128-159 $80-$9F     +64 $40        192-223 $C0-$DF  
      AddC64Key( KeyboardKey.KEY_AT, KeyModifier.SHIFT, KeyType.CONTROL_CODE, 192, true, 192, true, (char)0xeec0, true, "REVERSE SHIFT @" );

      AddC64Key( KeyboardKey.KEY_1, KeyModifier.COMMODORE, KeyType.CONTROL_CODE, 129 + 64, true, 129, true, (char)0xeec1, -1, 0, true, "ORANGE", "ORN" ).Replacements.AddRange( new string[] { "ORANGE", "ORN", "ORNG" } );   // orange

      AddC64Key( KeyboardKey.KEY_B, KeyModifier.SHIFT, KeyType.CONTROL_CODE, 194, true, 157, true, (char)0xeec2, true, "REVERSE SHIFT B" );

      AddC64Key( KeyboardKey.KEY_RUN_STOP, KeyModifier.SHIFT, KeyType.CONTROL_CODE, 131 + 64, true, 131, true, (char)0xeec3, -1, 0, true, "LOAD+RUN", "L+R" );   // TODO load+run

      AddC64Key( KeyboardKey.KEY_D, KeyModifier.SHIFT, KeyType.CONTROL_CODE, 196, true, 196, true, (char)0xeec4, true, "REVERSE SHIFT D" );

      AddC64Key( KeyboardKey.KEY_F1, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 133 + 64, true, 133, true, (char)0xeec5, true, "F1" ).Replacements.Add( "F1" );   // TODO F1
      AddC64Key( KeyboardKey.KEY_F3, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 134 + 64, true, 134, true, (char)0xeec6, true, "F3" ).Replacements.Add( "F3" );   // TODO F3
      AddC64Key( KeyboardKey.KEY_F5, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 135 + 64, true, 135, true, (char)0xeec7, true, "F5" ).Replacements.Add( "F5" );   // TODO F5
      AddC64Key( KeyboardKey.KEY_F7, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 136 + 64, true, 136, true, (char)0xeec8, true, "F7" ).Replacements.Add( "F7" );   // TODO F7
      AddC64Key( KeyboardKey.KEY_F1, KeyModifier.SHIFT, KeyType.CONTROL_CODE, 137 + 64, true, 137, true, (char)0xeec9, true, "F2" ).Replacements.Add( "F2" );   // TODO F2
      AddC64Key( KeyboardKey.KEY_F3, KeyModifier.SHIFT, KeyType.CONTROL_CODE, 138 + 64, true, 138, true, (char)0xeeca, true, "F4" ).Replacements.Add( "F4" );   // TODO F4
      AddC64Key( KeyboardKey.KEY_F5, KeyModifier.SHIFT, KeyType.CONTROL_CODE, 139 + 64, true, 139, true, (char)0xeecb, true, "F6" ).Replacements.Add( "F6" );   // TODO F6
      AddC64Key( KeyboardKey.KEY_F7, KeyModifier.SHIFT, KeyType.CONTROL_CODE, 140 + 64, true, 140, true, (char)0xeecc, true, "F8" ).Replacements.Add( "F8" );   // TODO F8
      AddC64Key( KeyboardKey.KEY_RETURN, KeyModifier.SHIFT, KeyType.CONTROL_CODE, 141 + 64, true, 141, true, (char)0xeecd, -1, 0, true, "SHIFT RETURN", "SH RET" ).Replacements.AddRange( new string[] { "SHIFT-RETURN", "SRET" } );   // TODO Shift-Return
      AddC64Key( KeyboardKey.KEY_N, KeyModifier.CONTROL, KeyType.CONTROL_CODE, 142 + 64, true, 142, true, (char)0xeece, -1, 0, true, "UPPERCASE", "UPCASE" ).Replacements.AddRange( new string[] { "CTRL-N", "SWUC" } );   // TODO Uppercase

      AddC64Key( KeyboardKey.KEY_O, KeyModifier.SHIFT, KeyType.CONTROL_CODE, 207, true, 0, false, (char)0xeecf, true, "REVERSE SHIFT O" );

      AddC64Key( KeyboardKey.KEY_1, KeyModifier.CONTROL, KeyType.CONTROL_CODE, 144 + 64, true, 144, true, (char)0xeed0, -1, 0, true, "BLACK", "BLK" ).Replacements.AddRange( new string[] { "BLACK", "BLK" } );   // black
      AddC64Key( KeyboardKey.KEY_CURSOR_UP_DOWN, KeyModifier.SHIFT, KeyType.CONTROL_CODE, 145 + 64, true, 145, true, (char)0xeed1, -1, 0, true, "CURSOR UP", "CUR UP" ).Replacements.Add( "UP" );   // TODO cursor up
      AddC64Key( KeyboardKey.KEY_0, KeyModifier.CONTROL, KeyType.CONTROL_CODE, 146 + 64, true, 146, true, (char)0xeed2, -1, 0, true, "REVERSE OFF", "RVS OFF" ).Replacements.AddRange( new string[] { "RVSOFF", "RVOF", "OFF", "REVERSE OFF" } );   // TODO rvs off
      AddC64Key( KeyboardKey.KEY_CLR_HOME, KeyModifier.SHIFT, KeyType.CONTROL_CODE, 147 + 64, true, 147, true, (char)0xeed3, -1, 0, true, "CLEAR", "CLR" ).Replacements.AddRange( new string[] { "CLR", "CLEAR" } );   // clr (with shift)
      AddC64Key( KeyboardKey.KEY_CLR_HOME, KeyModifier.COMMODORE, KeyType.CONTROL_CODE, 147 + 64, true, 147, true, (char)0xeed3, -1, 0, true, "CLEAR", "CLR" ).Replacements.AddRange( new string[] { "CLR", "CLEAR" } );   // clr (with commodore)
      AddC64Key( KeyboardKey.KEY_2, KeyModifier.COMMODORE, KeyType.CONTROL_CODE, 149 + 64, true, 149, true, (char)0xeed5, -1, 0, true, "BROWN", "BRN" ).Replacements.AddRange( new string[] { "BROWN", "BRN" } );   // brown
      AddC64Key( KeyboardKey.KEY_3, KeyModifier.COMMODORE, KeyType.CONTROL_CODE, 150 + 64, true, 150, true, (char)0xeed6, -1, 0, true, "LIGHT RED", "LRD" ).Replacements.AddRange( new string[] { "LIGHT RED", "PINK", "LRED" } );   // light red
      AddC64Key( KeyboardKey.KEY_4, KeyModifier.COMMODORE, KeyType.CONTROL_CODE, 151 + 64, true, 151, true, (char)0xeed7, -1, 0, true, "GREY 1", "GR1" ).Replacements.AddRange( new string[]{ "GREY 1", "GRAY 1", "DARK GRAY", "DARK GREY", "GRY1" } );      // grey 1
      AddC64Key( KeyboardKey.KEY_5, KeyModifier.COMMODORE, KeyType.CONTROL_CODE, 152 + 64, true, 152, true, (char)0xeed8, -1, 0, true, "GREY 2", "GR2" ).Replacements.AddRange( new string[]{ "GREY 2", "GRAY 2", "GRY2" } );      // grey 2
      AddC64Key( KeyboardKey.KEY_6, KeyModifier.COMMODORE, KeyType.CONTROL_CODE, 153 + 64, true, 153, true, (char)0xeed9, -1, 0, true, "LIGHT GREEN", "LGR" ).Replacements.AddRange( new string[] { "LIGHT GREEN", "LGRN" } ); // light green
      AddC64Key( KeyboardKey.KEY_7, KeyModifier.COMMODORE, KeyType.CONTROL_CODE, 154 + 64, true, 154, true, (char)0xeeda, -1, 0, true, "LIGHT BLUE", "LBL" ).Replacements.AddRange( new string[] { "LIGHT BLUE", "LBL", "LBLU" } );  // light blue
      AddC64Key( KeyboardKey.KEY_8, KeyModifier.COMMODORE, KeyType.CONTROL_CODE, 155 + 64, true, 155, true, (char)0xeedb, -1, 0, true, "GREY 3", "GR3" ).Replacements.AddRange( new string[] { "GREY 3", "GRAY 3", "LIGHT GREY", "LIGHT GRAY", "GRY3" } );      // grey 3
      AddC64Key( KeyboardKey.KEY_5, KeyModifier.CONTROL, KeyType.CONTROL_CODE, 156 + 64, true, 156, true, (char)0xeedc, -1, 0, true, "PURPLE", "PUR" ).Replacements.AddRange( new string[] { "PURPLE", "PUR" } );      // purple
      AddC64Key( KeyboardKey.KEY_CURSOR_LEFT_RIGHT, KeyModifier.SHIFT, KeyType.CONTROL_CODE, 157 + 64, true, 157, true, (char)0xeedd, -1, 0, true, "CURSOR LEFT", "CUR LEFT" ).Replacements.Add( "LEFT" );   // TODO cursor left
      AddC64Key( KeyboardKey.KEY_8, KeyModifier.CONTROL, KeyType.CONTROL_CODE, 158 + 64, true, 158, true, (char)0xeede, -1, 0, true, "YELLOW", "YEL" ).Replacements.AddRange( new string[] { "YELLOW", "YEL" } );    // yellow
      AddC64Key( KeyboardKey.KEY_4, KeyModifier.CONTROL, KeyType.CONTROL_CODE, 159 + 64, true, 159, true, (char)0xeedf, -1, 0, true, "CYAN", "CYN" ).Replacements.AddRange( new string[] { "CYAN", "CYN" }  );      // TODO cyan

      // 160-191 $A0-$BF     -64 $C0         96-127 $60-$7F  
      AddC64Key( KeyboardKey.KEY_SPACE, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 160 - 64, true, 160, true, (char)0xee60, true, "SHIFT SPACE" ).Replacements.AddRange( new string[]{ "SHIFT SPACE", "SH SPACE" } );   // shift-space
      AddC64Key( KeyboardKey.KEY_K, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 161 - 64, true, 161, true, (char)0xee61, true, "CBM K" ).Replacements.Add( "CBM-K" );   // C= K
      AddC64Key( KeyboardKey.KEY_I, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 162 - 64, true, 162, true, (char)0xee62, true, "CBM I" ).Replacements.Add( "CBM-I" );   // C= I
      AddC64Key( KeyboardKey.KEY_T, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 163 - 64, true, 163, true, (char)0xee63, true, "CBM T" ).Replacements.Add( "CBM-T" );   // C= T
      AddC64Key( KeyboardKey.KEY_AT, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 164 - 64, true, 164, true, (char)0xee64, true, "CBM @" ).Replacements.Add( "CBM-@" );   // C= @
      AddC64Key( KeyboardKey.KEY_G, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 165 - 64, true, 165, true, (char)0xee65, true, "CBM G" ).Replacements.Add( "CBM-G" );   // C= G
      AddC64Key( KeyboardKey.KEY_PLUS, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 166 - 64, true, 166, true, (char)0xee66, true, "CBM +" ).Replacements.Add( "CBM-+" );   // C= +
      AddC64Key( KeyboardKey.KEY_N, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 167 - 64, true, 167, true, (char)0xee67, true, "CBM N" ).Replacements.Add( "CBM-N" );   // C= N
      AddC64Key( KeyboardKey.KEY_POUND, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 168 - 64, true, 168, true, (char)0xee68, true, "CBM £" ).Replacements.Add( "CBM-POUND" );   // C= Insert 
      AddC64Key( KeyboardKey.KEY_POUND, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 169 - 64, true, 169, true, (char)0xee69, true, "SHIFT £" ).Replacements.Add( "SHIFT-POUND" );   // Shift Insert 
      AddC64Key( KeyboardKey.KEY_M, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 170 - 64, true, 170, true, (char)0xee6a, true, "CBM M" ).Replacements.Add( "CBM-M" );   // C= M
      AddC64Key( KeyboardKey.KEY_Q, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 171 - 64, true, 171, true, (char)0xee6b, true, "CBM Q" ).Replacements.Add( "CBM-Q" );   // C= Q
      AddC64Key( KeyboardKey.KEY_D, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 172 - 64, true, 172, true, (char)0xee6c, true, "CBM D" ).Replacements.Add( "CBM-D" );   // C= D
      AddC64Key( KeyboardKey.KEY_Z, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 173 - 64, true, 173, true, (char)0xee6d, true, "CBM Z" ).Replacements.Add( "CBM-Z" );   // C= Y
      AddC64Key( KeyboardKey.KEY_S, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 174 - 64, true, 174, true, (char)0xee6e, true, "CBM S" ).Replacements.Add( "CBM-S" );   // C= S
      AddC64Key( KeyboardKey.KEY_P, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 175 - 64, true, 175, true, (char)0xee6f, true, "CBM P" ).Replacements.Add( "CBM-P" );   // C= P
      AddC64Key( KeyboardKey.KEY_A, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 176 - 64, true, 176, true, (char)0xee70, true, "CBM A" ).Replacements.Add( "CBM-A" );   // C= A 
      AddC64Key( KeyboardKey.KEY_E, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 177 - 64, true, 177, true, (char)0xee71, true, "CBM E" ).Replacements.Add( "CBM-E" );   // C= E
      AddC64Key( KeyboardKey.KEY_R, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 178 - 64, true, 178, true, (char)0xee72, true, "CBM R" ).Replacements.Add( "CBM-R" );   // C= R
      AddC64Key( KeyboardKey.KEY_W, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 179 - 64, true, 179, true, (char)0xee73, true, "CBM W" ).Replacements.Add( "CBM-W" );   // C= W
      AddC64Key( KeyboardKey.KEY_H, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 180 - 64, true, 180, true, (char)0xee74, true, "CBM H" ).Replacements.Add( "CBM-H" );   // C= H
      AddC64Key( KeyboardKey.KEY_J, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 181 - 64, true, 181, true, (char)0xee75, true, "CBM J" ).Replacements.Add( "CBM-J" );   // C= J
      AddC64Key( KeyboardKey.KEY_L, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 182 - 64, true, 182, true, (char)0xee76, true, "CBM L" ).Replacements.Add( "CBM-L" );   // C= L
      AddC64Key( KeyboardKey.KEY_Y, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 183 - 64, true, 183, true, (char)0xee77, true, "CBM Y" ).Replacements.Add( "CBM-Y" );   // C= Z
      AddC64Key( KeyboardKey.KEY_U, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 184 - 64, true, 184, true, (char)0xee78, true, "CBM U" ).Replacements.Add( "CBM-U" );   // C= U
      AddC64Key( KeyboardKey.KEY_O, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 185 - 64, true, 185, true, (char)0xee79, true, "CBM O" ).Replacements.Add( "CBM-O" );   // C= O
      AddC64Key( KeyboardKey.KEY_AT, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 186 - 64, true, 186, true, (char)0xee7a, true, "SHIFT @" ).Replacements.Add( "SHIFT-@" );   // Shift @
      AddC64Key( KeyboardKey.KEY_F, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 187 - 64, true, 187, true, (char)0xee7b, true, "CBM F" ).Replacements.Add( "CBM-F" );   // C= F
      AddC64Key( KeyboardKey.KEY_C, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 188 - 64, true, 188, true, (char)0xee7c, true, "CBM C" ).Replacements.Add( "CBM-C" );   // C= C
      AddC64Key( KeyboardKey.KEY_X, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 189 - 64, true, 189, true, (char)0xee7d, true, "CBM X" ).Replacements.Add( "CBM-X" );   // C= X
      AddC64Key( KeyboardKey.KEY_V, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 190 - 64, true, 190, true, (char)0xee7e, true, "CBM V" ).Replacements.Add( "CBM-V" );   // C= V
      AddC64Key( KeyboardKey.KEY_B, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 191 - 64, true, 191, true, (char)0xee7f, true, "CBM B" ).Replacements.Add( "CBM-B" );   // C= B

      // 192-223 $C0-$DF    -128 $80         64- 95 $40-$5F  
      AddC64Key( KeyboardKey.KEY_STAR, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 192 - 128, true, 192, true, (char)0xee40, true, "SHIFT *" ).Replacements.Add( "SHIFT-*" );   // mittelstrich
      AddC64Key( KeyboardKey.KEY_A, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 193 - 128, true, 193, true, (char)0xee41, true, "SHIFT A" ).Replacements.Add( "SHIFT-A" );   // Shift-A
      AddC64Key( KeyboardKey.KEY_B, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 194 - 128, true, 194, true, (char)0xee42, true, "SHIFT B" ).Replacements.Add( "SHIFT-B" );   // Shift-B
      AddC64Key( KeyboardKey.KEY_C, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 195 - 128, true, 195, true, (char)0xee43, true, "SHIFT C" ).Replacements.Add( "SHIFT-C" );   // Shift-C
      AddC64Key( KeyboardKey.KEY_D, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 196 - 128, true, 196, true, (char)0xee44, true, "SHIFT D" ).Replacements.Add( "SHIFT-D" );   // Shift-D
      AddC64Key( KeyboardKey.KEY_E, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 197 - 128, true, 197, true, (char)0xee45, true, "SHIFT E" ).Replacements.Add( "SHIFT-E" );   // Shift-E
      AddC64Key( KeyboardKey.KEY_F, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 198 - 128, true, 198, true, (char)0xee46, true, "SHIFT F" ).Replacements.Add( "SHIFT-F" );   // Shift-F
      AddC64Key( KeyboardKey.KEY_G, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 199 - 128, true, 199, true, (char)0xee47, true, "SHIFT G" ).Replacements.Add( "SHIFT-G" );   // Shift-G
      AddC64Key( KeyboardKey.KEY_H, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 200 - 128, true, 200, true, (char)0xee48, true, "SHIFT H" ).Replacements.Add( "SHIFT-H" );   // Shift-H
      AddC64Key( KeyboardKey.KEY_I, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 201 - 128, true, 201, true, (char)0xee49, true, "SHIFT I" ).Replacements.Add( "SHIFT-I" );   // Shift-I
      AddC64Key( KeyboardKey.KEY_J, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 202 - 128, true, 202, true, (char)0xee4a, true, "SHIFT J" ).Replacements.Add( "SHIFT-J" );   // Shift-J
      AddC64Key( KeyboardKey.KEY_K, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 203 - 128, true, 203, true, (char)0xee4b, true, "SHIFT K" ).Replacements.Add( "SHIFT-K" );   // Shift-K
      AddC64Key( KeyboardKey.KEY_L, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 204 - 128, true, 204, true, (char)0xee4c, true, "SHIFT L" ).Replacements.Add( "SHIFT-L" );   // Shift-L
      AddC64Key( KeyboardKey.KEY_M, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 205 - 128, true, 205, true, (char)0xee4d, true, "SHIFT M" ).Replacements.Add( "SHIFT-M" );   // Shift-M
      AddC64Key( KeyboardKey.KEY_N, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 206 - 128, true, 206, true, (char)0xee4e, true, "SHIFT N" ).Replacements.Add( "SHIFT-N" );   // Shift-N
      AddC64Key( KeyboardKey.KEY_O, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 207 - 128, true, 207, true, (char)0xee4f, true, "SHIFT O" ).Replacements.Add( "SHIFT-O" );   // Shift-O
      AddC64Key( KeyboardKey.KEY_P, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 208 - 128, true, 208, true, (char)0xee50, true, "SHIFT P" ).Replacements.Add( "SHIFT-P" );   // Shift-P
      AddC64Key( KeyboardKey.KEY_Q, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 209 - 128, true, 209, true, (char)0xee51, true, "SHIFT Q" ).Replacements.Add( "SHIFT-Q" );   // Shift-Q
      AddC64Key( KeyboardKey.KEY_R, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 210 - 128, true, 210, true, (char)0xee52, true, "SHIFT R" ).Replacements.Add( "SHIFT-R" );   // Shift-R
      AddC64Key( KeyboardKey.KEY_S, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 211 - 128, true, 211, true, (char)0xee53, true, "SHIFT S" ).Replacements.Add( "SHIFT-S" );   // Shift-S
      AddC64Key( KeyboardKey.KEY_T, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 212 - 128, true, 212, true, (char)0xee54, true, "SHIFT T" ).Replacements.Add( "SHIFT-T" );   // Shift-T
      AddC64Key( KeyboardKey.KEY_U, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 213 - 128, true, 213, true, (char)0xee55, true, "SHIFT U" ).Replacements.Add( "SHIFT-U" );   // Shift-U
      AddC64Key( KeyboardKey.KEY_V, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 214 - 128, true, 214, true, (char)0xee56, true, "SHIFT V" ).Replacements.Add( "SHIFT-V" );   // Shift-V
      AddC64Key( KeyboardKey.KEY_W, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 215 - 128, true, 215, true, (char)0xee57, true, "SHIFT W" ).Replacements.Add( "SHIFT-W" );   // Shift-W
      AddC64Key( KeyboardKey.KEY_X, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 216 - 128, true, 216, true, (char)0xee58, true, "SHIFT X" ).Replacements.Add( "SHIFT-X" );   // Shift-X
      AddC64Key( KeyboardKey.KEY_Y, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 217 - 128, true, 217, true, (char)0xee59, true, "SHIFT Y" ).Replacements.Add( "SHIFT-Y" );   // Shift-Y
      AddC64Key( KeyboardKey.KEY_Z, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 218 - 128, true, 218, true, (char)0xee5a, true, "SHIFT Z" ).Replacements.Add( "SHIFT-Z" );   // Shift-Z
      AddC64Key( KeyboardKey.KEY_PLUS, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 219 - 128, true, 219, true, (char)0xee5b, true, "SHIFT +" ).Replacements.Add( "SHIFT-+" );
      AddC64Key( KeyboardKey.KEY_MINUS, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 220 - 128, true, 220, true, (char)0xee5c, true, "CBM A" ).Replacements.Add( "CBM--" );
      AddC64Key( KeyboardKey.KEY_MINUS, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 221 - 128, true, 221, true, (char)0xee5d, true, "SHIFT -" ).Replacements.Add( "SHIFT--" );
      AddC64Key( KeyboardKey.KEY_ARROW_UP, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 222 - 128, true, 0xff, true, (char)0xee5e, true, "PI" ).Replacements.Add( "PI" );   // TODO
      AddC64Key( KeyboardKey.KEY_STAR, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 223 - 128, true, 223, true, (char)0xee5f, true, "CBM *" ).Replacements.Add( "CBM-*" );   // C= *

      // 224-254 $E0-$FE    -128 $80         96-126 $60-$7E  
      AddC64Key( KeyboardKey.KEY_SPACE, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 224 - 128, true, 224, true, (char)0xee60, true, "SHIFT SPACE" ).Replacements.Add( "SHIFT-SPACE" );   // shift-space
      AddC64Key( KeyboardKey.KEY_K, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 225 - 128, true, 225, true, (char)0xee61, true, "CBM K" ).Replacements.Add( "CBM-K" );   // C= K
      AddC64Key( KeyboardKey.KEY_I, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 226 - 128, true, 226, true, (char)0xee62, true, "CBM I" ).Replacements.Add( "CBM-I" );   // C= I
      AddC64Key( KeyboardKey.KEY_T, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 227 - 128, true, 227, true, (char)0xee63, true, "CBM T" ).Replacements.Add( "CBM-T" );   // C= T
      AddC64Key( KeyboardKey.KEY_AT, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 228 - 128, true, 228, true, (char)0xee64, true, "CBM @" ).Replacements.Add( "CBM-@" );   // C= @
      AddC64Key( KeyboardKey.KEY_G, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 229 - 128, true, 229, true, (char)0xee65, true, "CBM G" ).Replacements.Add( "CBM-G" );   // C= G
      AddC64Key( KeyboardKey.KEY_PLUS, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 230 - 128, true, 230, true, (char)0xee66, true, "CBM +" ).Replacements.Add( "CBM-+" );   // C= +
      AddC64Key( KeyboardKey.KEY_N, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 231 - 128, true, 231, true, (char)0xee67, true, "CBM N" ).Replacements.Add( "CBM-N" );   // C= N
      //AddC64Key( KeyboardKey.KEY_INST_DEL, KeyModifier.COMMODORE, KeyType.CONTROL_CODE, 232 - 128, true, 148, true, (char)0xeed4, true, "INSERT" ).Replacements.AddRange( new string[] { "INS", "INSERT", "INST" } );  // C= Insert 
      //AddC64Key( KeyboardKey.KEY_INST_DEL, KeyModifier.SHIFT, KeyType.CONTROL_CODE, 233 - 128, true, 148, true, (char)0xeed4, true, "INSERT" ).Replacements.AddRange( new string[] { "INS", "INSERT", "INST" } );   // Shift Insert 
      AddC64Key( KeyboardKey.KEY_INST_DEL, KeyModifier.COMMODORE, KeyType.CONTROL_CODE, 212, true, 148, true, (char)0xeed4, true, "INSERT" ).Replacements.AddRange( new string[] { "INS", "INSERT", "INST" } );  // C= Insert 
      AddC64Key( KeyboardKey.KEY_INST_DEL, KeyModifier.SHIFT, KeyType.CONTROL_CODE, 212, true, 148, true, (char)0xeed4, true, "INSERT" ).Replacements.AddRange( new string[] { "INS", "INSERT", "INST" } );   // Shift Insert 
      AddC64Key( KeyboardKey.KEY_M, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 234 - 128, true, 234, true, (char)0xee6a, true, "CBM M" ).Replacements.Add( "CBM-M" );   // C= M
      AddC64Key( KeyboardKey.KEY_Q, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 235 - 128, true, 235, true, (char)0xee6b, true, "CBM Q" ).Replacements.Add( "CBM-Q" );   // C= Q
      AddC64Key( KeyboardKey.KEY_D, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 236 - 128, true, 236, true, (char)0xee6c, true, "CBM D" ).Replacements.Add( "CBM-D" );   // C= D
      AddC64Key( KeyboardKey.KEY_Z, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 237 - 128, true, 237, true, (char)0xee6d, true, "CBM Z" ).Replacements.Add( "CBM-Z" );   // C= Y
      AddC64Key( KeyboardKey.KEY_S, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 238 - 128, true, 238, true, (char)0xee6e, true, "CBM S" ).Replacements.Add( "CBM-S" );   // C= S
      AddC64Key( KeyboardKey.KEY_P, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 239 - 128, true, 239, true, (char)0xee6f, true, "CBM P" ).Replacements.Add( "CBM-P" );   // C= P
      AddC64Key( KeyboardKey.KEY_A, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 240 - 128, true, 240, true, (char)0xee70, true, "CBM A" ).Replacements.Add( "CBM-A" );   // C= A 
      AddC64Key( KeyboardKey.KEY_E, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 241 - 128, true, 241, true, (char)0xee71, true, "CBM E" ).Replacements.Add( "CBM-E" );   // C= E
      AddC64Key( KeyboardKey.KEY_R, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 242 - 128, true, 242, true, (char)0xee72, true, "CBM R" ).Replacements.Add( "CBM-R" );   // C= R
      AddC64Key( KeyboardKey.KEY_W, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 243 - 128, true, 243, true, (char)0xee73, true, "CBM W" ).Replacements.Add( "CBM-W" );   // C= W
      AddC64Key( KeyboardKey.KEY_H, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 244 - 128, true, 244, true, (char)0xee74, true, "CBM H" ).Replacements.Add( "CBM-H" );   // C= H
      AddC64Key( KeyboardKey.KEY_J, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 245 - 128, true, 245, true, (char)0xee75, true, "CBM J" ).Replacements.Add( "CBM-J" );   // C= J
      AddC64Key( KeyboardKey.KEY_L, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 246 - 128, true, 246, true, (char)0xee76, true, "CBM L" ).Replacements.Add( "CBM-L" );   // C= L
      AddC64Key( KeyboardKey.KEY_Y, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 247 - 128, true, 247, true, (char)0xee77, true, "CBM Y" ).Replacements.Add( "CBM-Y" );   // C= Z
      AddC64Key( KeyboardKey.KEY_U, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 248 - 128, true, 248, true, (char)0xee78, true, "CBM U" ).Replacements.Add( "CBM-U" );   // C= U
      AddC64Key( KeyboardKey.KEY_O, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 249 - 128, true, 249, true, (char)0xee79, true, "CBM O" ).Replacements.Add( "CBM-O" );   // C= O
      AddC64Key( KeyboardKey.KEY_AT, KeyModifier.SHIFT, KeyType.GRAPHIC_SYMBOL, 250 - 128, true, 250, true, (char)0xee7a, true, "SHIFT @" ).Replacements.Add( "SHIFT-@" );   // Shift @
      AddC64Key( KeyboardKey.KEY_F, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 251 - 128, true, 251, true, (char)0xee7b, true, "CBM F" ).Replacements.Add( "CBM-F" );   // C= F
      AddC64Key( KeyboardKey.KEY_C, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 252 - 128, true, 252, true, (char)0xee7c, true, "CBM C" ).Replacements.Add( "CBM-C" );   // C= C
      AddC64Key( KeyboardKey.KEY_X, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 253 - 128, true, 253, true, (char)0xee7d, true, "CBM X" ).Replacements.Add( "CBM-X" );   // C= X
      AddC64Key( KeyboardKey.KEY_V, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 254 - 128, true, 254, true, (char)0xee7e, true, "CBM V" ).Replacements.Add( "CBM-V" );   // C= V

      // 255 $FF                                 94 $5E  
      AddC64Key( KeyboardKey.KEY_ARROW_UP, KeyModifier.COMMODORE, KeyType.GRAPHIC_SYMBOL, 158, true, 0xff, true, (char)0xee5e, true, "PI" ).Replacements.Add( "PI" );   // PI

      // reverse keys!
      AddC64Key( KeyboardKey.KEY_SPACE, KeyModifier.SHIFT, KeyType.CONTROL_CODE, 160, true, 0, false, (char)0xeee0, true );   // reverse space
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 161, true, 0, false, (char)0xeea1, true, "REVERSE !" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 162, true, 0, false, (char)0xeea2, true, "REVERSE \"" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 163, true, 0, false, (char)0xeea3, true, "REVERSE #" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 164, true, 0, false, (char)0xeea4, true, "REVERSE $" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 165, true, 0, false, (char)0xeea5, true, "REVERSE %" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 166, true, 0, false, (char)0xeea6, true, "REVERSE &" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 167, true, 0, false, (char)0xeea7, true, "REVERSE '" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 168, true, 0, false, (char)0xeea8, true, "REVERSE (" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 169, true, 0, false, (char)0xeea9, true, "REVERSE )" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 170, true, 0, false, (char)0xeeaa, true, "REVERSE *" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 171, true, 0, false, (char)0xeeab, true, "REVERSE +" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 172, true, 0, false, (char)0xeeac, true, "REVERSE ," );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 173, true, 0, false, (char)0xeead, true, "REVERSE -" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 174, true, 0, false, (char)0xeeae, true, "REVERSE ." );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 175, true, 0, false, (char)0xeeaf, true, "REVERSE /" );

      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 176, true, 0, false, (char)0xeeb0, true, "REVERSE 0" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 177, true, 0, false, (char)0xeeb1, true, "REVERSE 1" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 178, true, 0, false, (char)0xeeb2, true, "REVERSE 2" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 179, true, 0, false, (char)0xeeb3, true, "REVERSE 3" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 180, true, 0, false, (char)0xeeb4, true, "REVERSE 4" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 181, true, 0, false, (char)0xeeb5, true, "REVERSE 5" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 182, true, 0, false, (char)0xeeb6, true, "REVERSE 6" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 183, true, 0, false, (char)0xeeb7, true, "REVERSE 7" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 184, true, 0, false, (char)0xeeb8, true, "REVERSE 8" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 185, true, 0, false, (char)0xeeb9, true, "REVERSE 9" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 186, true, 0, false, (char)0xeeba, true, "REVERSE :" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 187, true, 0, false, (char)0xeebb, true, "REVERSE ;" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 188, true, 0, false, (char)0xeebc, true, "REVERSE <" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 189, true, 0, false, (char)0xeebd, true, "REVERSE =" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 190, true, 0, false, (char)0xeebe, true, "REVERSE >" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 191, true, 0, false, (char)0xeebf, true, "REVERSE ?" );

      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 224, true, 0, false, (char)0xeee0, true, "" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 225, true, 0, false, (char)0xeee1, true, "" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 226, true, 0, false, (char)0xeee2, true, "" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 227, true, 0, false, (char)0xeee3, true, "" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 228, true, 0, false, (char)0xeee4, true, "" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 229, true, 0, false, (char)0xeee5, true, "" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 230, true, 0, false, (char)0xeee6, true, "" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 231, true, 0, false, (char)0xeee7, true, "" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 232, true, 0, false, (char)0xeee8, true, "" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 233, true, 0, false, (char)0xeee9, true, "" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 234, true, 0, false, (char)0xeeea, true, "" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 235, true, 0, false, (char)0xeeeb, true, "" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 236, true, 0, false, (char)0xeeec, true, "" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 237, true, 0, false, (char)0xeeed, true, "" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 238, true, 0, false, (char)0xeeee, true, "" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 239, true, 0, false, (char)0xeeef, true, "" );

      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 240, true, 0, false, (char)0xeef0, true, "" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 241, true, 0, false, (char)0xeef1, true, "" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 242, true, 0, false, (char)0xeef2, true, "" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 243, true, 0, false, (char)0xeef3, true, "" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 244, true, 0, false, (char)0xeef4, true, "" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 245, true, 0, false, (char)0xeef5, true, "" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 246, true, 0, false, (char)0xeef6, true, "" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 247, true, 0, false, (char)0xeef7, true, "" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 248, true, 0, false, (char)0xeef8, true, "" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 249, true, 0, false, (char)0xeef9, true, "" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 250, true, 0, false, (char)0xeefa, true, "" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 251, true, 0, false, (char)0xeefb, true, "" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 252, true, 0, false, (char)0xeefc, true, "" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 253, true, 0, false, (char)0xeefd, true, "" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 254, true, 0, false, (char)0xeefe, true, "" );
      AddC64Key( KeyboardKey.UNDEFINED, KeyModifier.NORMAL, KeyType.CONTROL_CODE, 255, true, 0, false, (char)0xeeff, true, "" );
    }



    static C64Character AddC64KeyLC( KeyboardKey Key, KeyModifier Modifier, KeyType Type, byte ScreenCodeValue, bool HasScreenCode, byte PetSCIIValue, bool HasPetSCII, char CharValue, char LowerCaseCharValue, byte LowerCasePETSCII )
    {
      return AddC64Key( Key, Modifier, Type, ScreenCodeValue, HasScreenCode, PetSCIIValue, HasPetSCII, CharValue, LowerCaseCharValue, LowerCasePETSCII, true, "" + (char)PetSCIIValue, "" );
    }



    static C64Character AddC64Key( KeyboardKey Key, KeyModifier Modifier, KeyType Type, byte ScreenCodeValue, bool HasScreenCode, byte PetSCIIValue, bool HasPetSCII, char CharValue, bool HasChar )
    {
      return AddC64Key( Key, Modifier, Type, ScreenCodeValue, HasScreenCode, PetSCIIValue, HasPetSCII, CharValue, -1, 0, HasChar, "" + CharValue, "" );
    }



    static C64Character AddC64Key( KeyboardKey Key, KeyModifier Modifier, KeyType Type, byte ScreenCodeValue, bool HasScreenCode, byte PetSCIIValue, bool HasPetSCII, char CharValue, bool HasChar, string Desc )
    {
      return AddC64Key( Key, Modifier, Type, ScreenCodeValue, HasScreenCode, PetSCIIValue, HasPetSCII, CharValue, -1, 0, HasChar, Desc, "" );
    }



    static C64Character AddC64Key( KeyboardKey Key, KeyModifier Modifier, KeyType Type, byte ScreenCodeValue, bool HasScreenCode, byte PetSCIIValue, bool HasPetSCII, char CharValue, int LowerCaseCharValue, byte LowerCasePETSCII, bool HasChar, string Desc, string ShortDesc )
    {
      C64Character  c64Char = new C64Character( Key, ScreenCodeValue, HasScreenCode, PetSCIIValue, HasPetSCII, CharValue, HasChar, Desc, ShortDesc );
      c64Char.Modifier = Modifier;
      c64Char.Type = Type;
      c64Char.LowerCasePETSCII = LowerCasePETSCII;
      if ( c64Char.HasChar )
      {
        if ( ( c64Char.CharValue & 0xff00 ) == 0xee00 )
        {
          c64Char.LowerCaseDisplayChar = (char)( 0x100 + c64Char.CharValue );
        }
        else if ( ( c64Char.CharValue >= 'A' )
        &&        ( c64Char.CharValue <= 'Z' ) )
        {
          c64Char.LowerCaseDisplayChar = (char)( 0xef00 + ( c64Char.CharValue - 'A' + 1 ) );
        }
        else
        {
          c64Char.LowerCaseDisplayChar = c64Char.CharValue;
        }
      }

      AllPhysicalKeyInfos.Add( c64Char );

      if ( !PhysicalKeyInfo.ContainsKey( Key ) )
      {
        PhysicalKeyInfo.Add( Key, new Types.C64Key() );
      }

      if ( Modifier == KeyModifier.NORMAL )
      {
        PhysicalKeyInfo[Key].Normal = c64Char;
      }
      if ( Modifier == KeyModifier.SHIFT )
      {
        PhysicalKeyInfo[Key].WithShift = c64Char;
      }
      if ( Modifier == KeyModifier.COMMODORE )
      {
        PhysicalKeyInfo[Key].WithCommodore = c64Char;
      }
      if ( Modifier == KeyModifier.CONTROL )
      {
        PhysicalKeyInfo[Key].WithControl = c64Char;
      }



      if ( HasScreenCode )
      {
        if ( !ScreenCodeToChar.ContainsKey( ScreenCodeValue ) )
        {
          ScreenCodeToChar[ScreenCodeValue] = c64Char;
        }
      }
      if ( HasPetSCII )
      {
        if ( !PetSCIIToChar.ContainsKey( PetSCIIValue ) )
        {
          PetSCIIToChar[PetSCIIValue] = c64Char;
        }
      }
      if ( HasChar )
      {
        if ( !CharToC64Char.ContainsKey( CharValue ) )
        {
          CharToC64Char[CharValue] = c64Char;
        }
        if ( LowerCaseCharValue != -1 )
        {
          LowerCaseCharTo64Char[(char)LowerCaseCharValue] = c64Char;
        }
      }
      if ( ( HasPetSCII )
      &&   ( HasChar ) )
      {
        if ( !PETSCIIToUnicode.ContainsKey( PetSCIIValue ) )
        {
          PETSCIIToUnicode[PetSCIIValue] = CharValue;
        }
      }
      return c64Char;
    }



    public static C64Character FindC64KeyByUnicode( char UnicodeValue )
    {
      foreach ( var c64Key in Types.ConstantData.PhysicalKeyInfo )
      {
        if ( ( c64Key.Value.WithShift != null )
        &&   ( c64Key.Value.WithShift.CharValue == UnicodeValue ) )
        {
          return c64Key.Value.WithShift;
        }
        if ( ( c64Key.Value.WithControl != null )
        &&   ( c64Key.Value.WithControl.CharValue == UnicodeValue ) )
        {
          return c64Key.Value.WithControl;
        }
        if ( ( c64Key.Value.WithCommodore != null )
        &&   ( c64Key.Value.WithCommodore.CharValue == UnicodeValue ) )
        {
          return c64Key.Value.WithCommodore;
        }
        if ( ( c64Key.Value.Normal != null )
        &&   ( c64Key.Value.Normal.CharValue == UnicodeValue ) )
        {
          return c64Key.Value.Normal;
        }
      }
      return null;
    }



    public static C64Character FindC64KeyByPETSCII( byte PETSCIIValue )
    {
      if ( ( PETSCIIValue >= 192 )
      &&   ( PETSCIIValue <= 223 ) )
      {
        PETSCIIValue -= 96;
      }
      else if ( ( PETSCIIValue >= 224 )
      &&        ( PETSCIIValue <= 254 ) )
      {
        PETSCIIValue -= 64;
      }
      else if ( PETSCIIValue == 255 )
      {
        PETSCIIValue = 126;
      }
      foreach ( var c64Key in Types.ConstantData.AllPhysicalKeyInfos )
      {
        if ( c64Key.PetSCIIValue == PETSCIIValue )
        {
          return c64Key;
        }
      }
      return null;
    }

  }



  public class TokenInfo
  {
    public enum TokenType
    {
      UNKNOWN,
      BLANK,
      LABEL_GLOBAL,
      LABEL_LOCAL,
      OPCODE,
      OPERATOR,
      LITERAL_NUMBER,
      LITERAL_CHAR,
      LITERAL_STRING,
      COMMENT,
      PSEUDO_OP,
      SEPARATOR,
      LABEL_INTERNAL,
      CALL_MACRO,
      COMMENT_IF_FIRST_CHAR,
      OPCODE_FIXED_ZP,
      OPCODE_FIXED_NON_ZP,
      OPCODE_DIRECT_VALUE,
      LABEL_CHEAP_LOCAL,
      LITERAL_REAL_NUMBER,
      SINGLE_CHAR,
      MACRO_PARAMETER
    }

    public string     OriginatingString;
    public int        StartPos = 0;
    public int        Length = 0;
    public TokenType  Type = TokenType.UNKNOWN;

    private string _Content = null;

    public string Content
    {
      get
      {
        if ( _Content != null )
        {
          return _Content;
        }
        return OriginatingString.Substring( StartPos, Length );
      }

      set
      {
        _Content = value;
      }
    }

    public int EndPos
    {
      get
      {
        return StartPos + Length - 1;
      }
    }
  };



  public class LoopInfo
  {
    public int                LineIndex = 0;
    public int                LoopLength = -1;
    public string             Label = "";
    public int                StartValue = 0;
    public int                EndValue = 0;
    public int                StepValue = 1;
    public int                CurrentValue = 0;
    public string[]           Content = null;
    public bool               FirstLoop = true;
  };



  public class RepeatUntilInfo
  {
    public int                LineIndex = 0;
    public int                LoopLength = -1;
    public int                NumRepeats = 0;
    public string[]           Content = null;
  };



  public class ScopeInfo
  {
    public enum ScopeType
    {
      UNKNOWN,
      IF_OR_IFDEF,
      LOOP,
      MACRO_FUNCTION,
      PSEUDO_PC,
      ZONE,
      ADDRESS,
      DO_UNTIL,
      REPEAT
    };

    public int                      StartIndex = 0;
    public bool                     Active = true;
    public bool                     IfChainHadActiveEntry = false;    // used for if/ifelse/else..chains
    public Types.LoopInfo           Loop = null;
    public Types.MacroFunctionInfo  Macro = null;
    public RepeatUntilInfo          RepeatUntil = null;
    public ScopeType                Type = ScopeType.LOOP;

    public ScopeInfo( ScopeType Type )
    {
      this.Type = Type;
    }
  };



  public class MacroFunctionInfo
  {
    public int      LineIndex = 0;
    public int      LineEnd = -1;
    public string   Name = "";
    public List<string> ParameterNames = new List<string>();
    public List<bool> ParametersAreReferences = new List<bool>();
    public string[] Content = null;
    public bool     UsesBracket = false;
    public string   ParentFileName = "";
  };



  public class BuildInfo
  {
    public string             TargetFile = "";
    public CompileTargetType  TargetType = CompileTargetType.NONE;
  };



  public class FunctionInfo
  {
    public Function             Function = Function.NONE;
    public string               Description = "";
    public FunctionStudioState  State = FunctionStudioState.ANY;
    public ToolStripMenuItem    MenuItem = null;
    public ToolStripButton      ToolBarButton = null;

    public FunctionInfo( Function Func, string Desc, FunctionStudioState State )
    {
      Function = Func;
      Description = Desc;
      this.State = State;
    }
  };



  public class AutoCompleteItemInfo
  {
    public string       Token = "";
    public string       ToolTipTitle = null;
    public string       ToolTipText = null;

    public SymbolInfo   Symbol = null;
  };

}
