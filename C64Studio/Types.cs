using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace C64Studio.Types
{
  public enum AssemblerType
  {
    AUTO,
    C64_STUDIO,
    PDS,
    DASM
  };

  public class MacroInfo
  {
    public enum MacroType
    {
      UNKNOWN,
      BYTE,
      WORD,
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
      IFDEF,
      IFNDEF,
      IF,
      FILL,
      ALIGN,
      END_OF_FILE,
      PROCESSOR,
      ORG,
      NO_WARNING
    }

    public MacroType      Type = MacroType.UNKNOWN;
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
    SAVE_DOCUMENT_AS
  }

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

    E1100_RELATIVE_JUMP_TOO_FAR             = 0x1100,
    E1101_BANK_TOO_BIG                      = 0x1101,
    E1102_PROGRAM_TOO_LARGE                 = 0x1102,
    E1103_SEGMENT_OVERLAP                   = 0x1103,

    E1200_REDEFINITION_OF_LABEL             = 0x1200,
    E1201_REDEFINITION_OF_PREPROCESSOR_DEFINE = 0x1201,
    E1202_REDEFINITION_OF_ZONE              = 0x1202,
    E1203_REDEFINITION_OF_CONSTANT          = 0x1203,

    E1300_OPCODE_AMBIGIOUS                  = 0x1300,
    E1301_MACRO_UNKNOWN                     = 0x1301,
    E1302_MALFORMED_MACRO                   = 0x1302,
    E1303_MALFORMED_ZONE_DESCRIPTOR         = 0x1303,
    E1304_UNSUPPORTED_TARGET_TYPE           = 0x1304,
    E1305_EXPECTED_TRAILING_SYMBOL          = 0x1305,
    E1306_EXPECTED_BRACKETS_AND_TRAILING_SYMBOL = 0x1306,
    E1307_FILENAME_INCOMPLETE               = 0x1307,

    E1400_CIRCULAR_INCLUSION                = 0x1400,

    E2000_FILE_OPEN_ERROR                   = 0x2000,
    E2001_FILE_READ_ERROR                   = 0x2001,

    E3000_BASIC_MISSING_LINE_NUMBER         = 0x3000,
    E3001_BASIC_INVALID_LINE_NUMBER         = 0x3001,
    E3002_BASIC_UNSUPPORTED_CHARACTER       = 0x3002,
    E3003_BASIC_LABEL_MALFORMED             = 0x3003,
    E3004_BASIC_MISSING_LABEL               = 0x3004,
    E3005_BASIC_UNKNOWN_MACRO               = 0x3005,


    W0001_SEGMENT_OVERLAP                   = 0x8000,
    W0002_UNKNOWN_MACRO                     = 0x8001,
    W0003_BANK_INDEX_ALREADY_USED           = 0x8002,
    W1000_UNUSED_LABEL                      = 0x8003
  }


  public enum SyntaxElement
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
    [Description( "Macro" )] 
    MACRO,
    [Description( "Current Debug Position" )] 
    CURRENT_DEBUG_LINE,
    [Description( "Empty Space" )]
    EMPTY_SPACE,
    [Description( "Operator" )]
    OPERATOR,
  }

  public class SyntaxColor
  {
    public uint         FGColor = 0;
    public uint         BGColor = 0xffffffff;
    public bool         BGColorAuto = false;
    public string       Name = "";


    public SyntaxColor( string Name )
    {
      this.Name = Name;
    }

    public SyntaxColor( string Name, uint FGColor, uint BGColor )
    {
      this.Name = Name;
      this.FGColor = FGColor;
      this.BGColor = BGColor;
      this.BGColorAuto = false;
    }

    public SyntaxColor( string Name, uint FGColor )
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
    public static string FILEFILTER_ASM = "ASM File|*.asm|";
    public static string FILEFILTER_BASIC = "Basic File|*.bas|";
    public static string FILEFILTER_CHARSET = "Charset Project|*.charsetproject|Charset File|*.chr|";
    public static string FILEFILTER_SPRITE = "Sprite Project|*.spriteproject|Sprite File|*.spr|";
    public static string FILEFILTER_CHARSET_SCREEN = "Charset Screen Project|*.charscreen|";
    public static string FILEFILTER_GRAPHIC_SCREEN = "Graphic Screen Project|*.graphicscreen|";
    public static string FILEFILTER_EXECUTABLE = "Executable Files|*.exe|";
    public static string FILEFILTER_ACME = "ACME|acme.exe|";
    public static string FILEFILTER_PRG = "PRG File|*.prg|";
    public static string FILEFILTER_VICE = "Vice|x64.exe;x64sc.exe|";
    public static string FILEFILTER_STUDIO = "C64 Studio Files|*.c64|";
    public static string FILEFILTER_ALL_SUPPORTED_FILES = "Supported Files|*.c64;*.asm;*.charsetproject;*.spriteproject;*.bas;*.chr;*.spr;*.charscreen;*.graphicscreen|";
    public static string FILEFILTER_SOURCE_FILES = "Source Files|*.asm;*.bas|";
    public static string FILEFILTER_MEDIA_FILES = "Tape/Disk Files|*.t64;*.prg;*.d64|";
    public static string FILEFILTER_IMAGE_FILES = "Image Files|*.png;*.bmp;*.gif|PNG Files|*.png|BMP Files|*.bmp|GIF Files|*.gif|";
    public static string FILEFILTER_TAPE = "Tape Files|*.t64,*.prg|";
    public static string FILEFILTER_DISK = "Disk Files|*.d64|";
  }

  public class FileChunk
  {
    public const System.UInt16    PROJECT               = 0x1000;
    public const System.UInt16    PROJECT_ELEMENT       = 0x1001;
    public const System.UInt16    PROJECT_ELEMENT_DATA  = 0x1002;
    public const System.UInt16    PROJECT_ELEMENT_DISPLAY_DATA  = 0x1003;
    public const System.UInt16    PROJECT_ELEMENT_PER_CONFIG_SETTING = 0x1004;
    public const System.UInt16    PROJECT_CONFIG        = 0x1100;

    public const System.UInt16    CHARSET_SCREEN_INFO   = 0x1200;
    public const System.UInt16    SCREEN_CHAR_DATA      = 0x1300;
    public const System.UInt16    SCREEN_COLOR_DATA     = 0x1301;
    public const System.UInt16    GRAPHIC_SCREEN_INFO   = 0x1310;
    public const System.UInt16    GRAPHIC_DATA          = 0x1311;   // uint width, uint height, uint image type, uint palette entry count, byte r,g,b, uint data size, data

    public const System.UInt16    MULTICOLOR_DATA       = 0x1500;

    public const System.UInt16    SETTINGS_TOOL         = 0x2000;
    public const System.UInt16    SETTINGS_ACCELERATOR  = 0x2001;
    public const System.UInt16    SETTINGS_SOUND        = 0x2002;
    public const System.UInt16    SETTINGS_WINDOW       = 0x2003;
    public const System.UInt16    SETTINGS_TABS         = 0x2004;
    public const System.UInt16    SETTINGS_FONT         = 0x2005;
    public const System.UInt16    SETTINGS_SYNTAX_COLORING = 0x2006;
    public const System.UInt16    SETTINGS_UI           = 0x2007;
    public const System.UInt16    SETTINGS_DEFAULTS     = 0x2008;
  }

  public enum FileType
  {
    SCRATCHED = 0x00,
    DEL = 0x80,
    SEQ = 0x81,
    PRG = 0x82,
    USR = 0x83,
    REL = 0x84
  }

  public class FileInfo
  {
    public GR.Memory.ByteBuffer Filename = new GR.Memory.ByteBuffer( 16 );
    public int StartTrack = -1;
    public int StartSector = -1;
    public int Blocks = 0;
    public FileType Type = FileType.SCRATCHED;
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
      ZONE
    };

    public Types      Type = Types.UNKNOWN;
    public string     Name = "";
    public int        LineIndex = 0;            // global
    public string     DocumentFilename = "";
    public int        LocalLineIndex = 0;
    public int        AddressOrValue = -1;
    public bool       Used = false;
    public string     Zone = "";

    public override string ToString()
    {
      return Name;
    }
  }


  public class C64Character
  {
    public byte       ScreenCodeValue = 0;
    public bool       HasScreenCode = false;
    public byte       PetSCIIValue = 0;
    public bool       HasPetSCII = false;
    public char       CharValue = ' ';
    public bool       HasChar = false;
    public string     Desc = "";
    public string     ShortDesc = "";

    public C64Character( byte ScreenCodeValue, bool HasScreenCode, byte PetSCIIValue, bool HasPetSCII, char CharValue, bool HasChar, string Desc, string ShortDesc )
    {
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


  public class ConstantData
  {
    public static GR.Memory.ByteBuffer UpperCaseCharset = new GR.Memory.ByteBuffer( "3C666E6E60623C00183C667E666666007C66667C66667C003C66606060663C00786C6666666C78007E60607860607E007E606078606060003C66606E66663C006666667E666666003C18181818183C001E0C0C0C0C6C3800666C7870786C66006060606060607E0063777F6B6363630066767E7E6E6666003C66666666663C007C66667C606060003C666666663C0E007C66667C786C66003C66603C06663C007E181818181818006666666666663C0066666666663C18006363636B7F77630066663C183C6666006666663C181818007E060C1830607E003C30303030303C000C12307C3062FC003C0C0C0C0C0C3C0000183C7E181818180010307F7F3010000000000000000000181818180000180066666600000000006666FF66FF666600183E603C067C180062660C18306646003C663C3867663F00060C1800000000000C18303030180C0030180C0C0C18300000663CFF3C6600000018187E1818000000000000001818300000007E0000000000000000001818000003060C183060003C666E7666663C001818381818187E003C66060C30607E003C66061C06663C00060E1E667F0606007E607C0606663C003C66607C66663C007E660C18181818003C66663C66663C003C66663E06663C00000018000018000000001800001818300E18306030180E0000007E007E00000070180C060C1870003C66060C18001800000000FFFF000000081C3E7F7F1C3E001818181818181818000000FFFF0000000000FFFF0000000000FFFF000000000000000000FFFF000030303030303030300C0C0C0C0C0C0C0C000000E0F038181818181C0F07000000181838F0E0000000C0C0C0C0C0C0FFFFC0E070381C0E070303070E1C3870E0C0FFFFC0C0C0C0C0C0FFFF030303030303003C7E7E7E7E3C000000000000FFFF00367F7F7F3E1C08006060606060606060000000070F1C1818C3E77E3C3C7EE7C3003C7E66667E3C001818666618183C000606060606060606081C3E7F3E1C0800181818FFFF181818C0C03030C0C0303018181818181818180000033E76363600FF7F3F1F0F0703010000000000000000F0F0F0F0F0F0F0F000000000FFFFFFFFFF0000000000000000000000000000FFC0C0C0C0C0C0C0C0CCCC3333CCCC3333030303030303030300000000CCCC3333FFFEFCF8F0E0C08003030303030303031818181F1F181818000000000F0F0F0F1818181F1F000000000000F8F8181818000000000000FFFF0000001F1F181818181818FFFF000000000000FFFF181818181818F8F8181818C0C0C0C0C0C0C0C0E0E0E0E0E0E0E0E00707070707070707FFFF000000000000FFFFFF00000000000000000000FFFFFF030303030303FFFF00000000F0F0F0F00F0F0F0F00000000181818F8F8000000F0F0F0F000000000F0F0F0F00F0F0F0FC39991919F99C3FFE7C39981999999FF83999983999983FFC3999F9F9F99C3FF87939999999387FF819F9F879F9F81FF819F9F879F9F9FFFC3999F919999C3FF99999981999999FFC3E7E7E7E7E7C3FFE1F3F3F3F393C7FF9993878F879399FF9F9F9F9F9F9F81FF9C8880949C9C9CFF99898181919999FFC39999999999C3FF839999839F9F9FFFC399999999C3F1FF83999983879399FFC3999FC3F999C3FF81E7E7E7E7E7E7FF999999999999C3FF9999999999C3E7FF9C9C9C9480889CFF9999C3E7C39999FF999999C3E7E7E7FF81F9F3E7CF9F81FFC3CFCFCFCFCFC3FFF3EDCF83CF9D03FFC3F3F3F3F3F3C3FFFFE7C381E7E7E7E7FFEFCF8080CFEFFFFFFFFFFFFFFFFFFFE7E7E7E7FFFFE7FF999999FFFFFFFFFF99990099009999FFE7C19FC3F983E7FF9D99F3E7CF99B9FFC399C3C79899C0FFF9F3E7FFFFFFFFFFF3E7CFCFCFE7F3FFCFE7F3F3F3E7CFFFFF99C300C399FFFFFFE7E781E7E7FFFFFFFFFFFFFFE7E7CFFFFFFF81FFFFFFFFFFFFFFFFFFE7E7FFFFFCF9F3E7CF9FFFC39991899999C3FFE7E7C7E7E7E781FFC399F9F3CF9F81FFC399F9E3F999C3FFF9F1E19980F9F9FF819F83F9F999C3FFC3999F839999C3FF8199F3E7E7E7E7FFC39999C39999C3FFC39999C1F999C3FFFFFFE7FFFFE7FFFFFFFFE7FFFFE7E7CFF1E7CF9FCFE7F1FFFFFF81FF81FFFFFF8FE7F3F9F3E78FFFC399F9F3E7FFE7FFFFFFFF0000FFFFFFF7E3C18080E3C1FFE7E7E7E7E7E7E7E7FFFFFF0000FFFFFFFFFF0000FFFFFFFFFF0000FFFFFFFFFFFFFFFFFF0000FFFFCFCFCFCFCFCFCFCFF3F3F3F3F3F3F3F3FFFFFF1F0FC7E7E7E7E7E3F0F8FFFFFFE7E7C70F1FFFFFFF3F3F3F3F3F3F00003F1F8FC7E3F1F8FCFCF8F1E3C78F1F3F00003F3F3F3F3F3F0000FCFCFCFCFCFCFFC381818181C3FFFFFFFFFFFF0000FFC9808080C1E3F7FF9F9F9F9F9F9F9F9FFFFFFFF8F0E3E7E73C1881C3C381183CFFC381999981C3FFE7E79999E7E7C3FFF9F9F9F9F9F9F9F9F7E3C180C1E3F7FFE7E7E70000E7E7E73F3FCFCF3F3FCFCFE7E7E7E7E7E7E7E7FFFFFCC189C9C9FF0080C0E0F0F8FCFEFFFFFFFFFFFFFFFF0F0F0F0F0F0F0F0FFFFFFFFF0000000000FFFFFFFFFFFFFFFFFFFFFFFFFFFF003F3F3F3F3F3F3F3F3333CCCC3333CCCCFCFCFCFCFCFCFCFCFFFFFFFF3333CCCC000103070F1F3F7FFCFCFCFCFCFCFCFCE7E7E7E0E0E7E7E7FFFFFFFFF0F0F0F0E7E7E7E0E0FFFFFFFFFFFF0707E7E7E7FFFFFFFFFFFF0000FFFFFFE0E0E7E7E7E7E7E70000FFFFFFFFFFFF0000E7E7E7E7E7E70707E7E7E73F3F3F3F3F3F3F3F1F1F1F1F1F1F1F1FF8F8F8F8F8F8F8F80000FFFFFFFFFFFF000000FFFFFFFFFFFFFFFFFFFF000000FCFCFCFCFCFC0000FFFFFFFF0F0F0F0FF0F0F0F0FFFFFFFFE7E7E70707FFFFFF0F0F0F0FFFFFFFFF0F0F0F0FF0F0F0F0" );
    public static GR.Memory.ByteBuffer LowerCaseCharset = new GR.Memory.ByteBuffer( "3C666E6E60623C0000003C063E663E000060607C66667C0000003C6060603C000006063E66663E0000003C667E603C00000E183E1818180000003E66663E067C0060607C666666000018003818183C00000600060606063C0060606C786C66000038181818183C000000667F7F6B630000007C666666660000003C6666663C0000007C66667C606000003E66663E060600007C666060600000003E603C067C0000187E1818180E000000666666663E0000006666663C18000000636B7F3E36000000663C183C660000006666663E0C7800007E0C18307E003C30303030303C000C12307C3062FC003C0C0C0C0C0C3C0000183C7E181818180010307F7F3010000000000000000000181818180000180066666600000000006666FF66FF666600183E603C067C180062660C18306646003C663C3867663F00060C1800000000000C18303030180C0030180C0C0C18300000663CFF3C6600000018187E1818000000000000001818300000007E0000000000000000001818000003060C183060003C666E7666663C001818381818187E003C66060C30607E003C66061C06663C00060E1E667F0606007E607C0606663C003C66607C66663C007E660C18181818003C66663C66663C003C66663E06663C00000018000018000000001800001818300E18306030180E0000007E007E00000070180C060C1870003C66060C18001800000000FFFF000000183C667E666666007C66667C66667C003C66606060663C00786C6666666C78007E60607860607E007E606078606060003C66606E66663C006666667E666666003C18181818183C001E0C0C0C0C6C3800666C7870786C66006060606060607E0063777F6B6363630066767E7E6E6666003C66666666663C007C66667C606060003C666666663C0E007C66667C786C66003C66603C06663C007E181818181818006666666666663C0066666666663C18006363636B7F77630066663C183C6666006666663C181818007E060C1830607E00181818FFFF181818C0C03030C0C0303018181818181818183333CCCC3333CCCC3399CC663399CC660000000000000000F0F0F0F0F0F0F0F000000000FFFFFFFFFF0000000000000000000000000000FFC0C0C0C0C0C0C0C0CCCC3333CCCC3333030303030303030300000000CCCC3333CC993366CC99336603030303030303031818181F1F181818000000000F0F0F0F1818181F1F000000000000F8F8181818000000000000FFFF0000001F1F181818181818FFFF000000000000FFFF181818181818F8F8181818C0C0C0C0C0C0C0C0E0E0E0E0E0E0E0E00707070707070707FFFF000000000000FFFFFF00000000000000000000FFFFFF0103066C7870600000000000F0F0F0F00F0F0F0F00000000181818F8F8000000F0F0F0F000000000F0F0F0F00F0F0F0FC39991919F99C3FFFFFFC3F9C199C1FFFF9F9F83999983FFFFFFC39F9F9FC3FFFFF9F9C19999C1FFFFFFC399819FC3FFFFF1E7C1E7E7E7FFFFFFC19999C1F983FF9F9F83999999FFFFE7FFC7E7E7C3FFFFF9FFF9F9F9F9C3FF9F9F93879399FFFFC7E7E7E7E7C3FFFFFF998080949CFFFFFF8399999999FFFFFFC3999999C3FFFFFF839999839F9FFFFFC19999C1F9F9FFFF83999F9F9FFFFFFFC19FC3F983FFFFE781E7E7E7F1FFFFFF99999999C1FFFFFF999999C3E7FFFFFF9C9480C1C9FFFFFF99C3E7C399FFFFFF999999C1F387FFFF81F3E7CF81FFC3CFCFCFCFCFC3FFF3EDCF83CF9D03FFC3F3F3F3F3F3C3FFFFE7C381E7E7E7E7FFEFCF8080CFEFFFFFFFFFFFFFFFFFFFE7E7E7E7FFFFE7FF999999FFFFFFFFFF99990099009999FFE7C19FC3F983E7FF9D99F3E7CF99B9FFC399C3C79899C0FFF9F3E7FFFFFFFFFFF3E7CFCFCFE7F3FFCFE7F3F3F3E7CFFFFF99C300C399FFFFFFE7E781E7E7FFFFFFFFFFFFFFE7E7CFFFFFFF81FFFFFFFFFFFFFFFFFFE7E7FFFFFCF9F3E7CF9FFFC39991899999C3FFE7E7C7E7E7E781FFC399F9F3CF9F81FFC399F9E3F999C3FFF9F1E19980F9F9FF819F83F9F999C3FFC3999F839999C3FF8199F3E7E7E7E7FFC39999C39999C3FFC39999C1F999C3FFFFFFE7FFFFE7FFFFFFFFE7FFFFE7E7CFF1E7CF9FCFE7F1FFFFFF81FF81FFFFFF8FE7F3F9F3E78FFFC399F9F3E7FFE7FFFFFFFF0000FFFFFFE7C39981999999FF83999983999983FFC3999F9F9F99C3FF87939999999387FF819F9F879F9F81FF819F9F879F9F9FFFC3999F919999C3FF99999981999999FFC3E7E7E7E7E7C3FFE1F3F3F3F393C7FF9993878F879399FF9F9F9F9F9F9F81FF9C8880949C9C9CFF99898181919999FFC39999999999C3FF839999839F9F9FFFC399999999C3F1FF83999983879399FFC3999FC3F999C3FF81E7E7E7E7E7E7FF999999999999C3FF9999999999C3E7FF9C9C9C9480889CFF9999C3E7C39999FF999999C3E7E7E7FF81F9F3E7CF9F81FFE7E7E70000E7E7E73F3FCFCF3F3FCFCFE7E7E7E7E7E7E7E7CCCC3333CCCC3333CC663399CC663399FFFFFFFFFFFFFFFF0F0F0F0F0F0F0F0FFFFFFFFF0000000000FFFFFFFFFFFFFFFFFFFFFFFFFFFF003F3F3F3F3F3F3F3F3333CCCC3333CCCCFCFCFCFCFCFCFCFCFFFFFFFF3333CCCC3366CC993366CC99FCFCFCFCFCFCFCFCE7E7E7E0E0E7E7E7FFFFFFFFF0F0F0F0E7E7E7E0E0FFFFFFFFFFFF0707E7E7E7FFFFFFFFFFFF0000FFFFFFE0E0E7E7E7E7E7E70000FFFFFFFFFFFF0000E7E7E7E7E7E70707E7E7E73F3F3F3F3F3F3F3F1F1F1F1F1F1F1F1FF8F8F8F8F8F8F8F80000FFFFFFFFFFFF000000FFFFFFFFFFFFFFFFFFFF000000FEFCF993878F9FFFFFFFFFFF0F0F0F0FF0F0F0F0FFFFFFFFE7E7E70707FFFFFF0F0F0F0FFFFFFFFF0F0F0F0FF0F0F0F0" );

    public static System.Drawing.Color[]      m_Colors = new System.Drawing.Color[17];
    public static System.Drawing.Brush[]      m_ColorBrushes = new System.Drawing.Brush[17];
    public static uint[]                      m_ColorValues = new uint[17];

    public static GR.Collections.Map<char, byte> PETSCII = new GR.Collections.Map<char, byte>();
    public static GR.Collections.Map<byte, char> PETSCIIToUnicode = new GR.Collections.Map<byte, char>();

    public static GR.Collections.Map<byte, C64Character>  ScreenCodeToChar = new GR.Collections.Map<byte, C64Character>();
    public static GR.Collections.Map<byte, C64Character>  PetSCIIToChar = new GR.Collections.Map<byte, C64Character>();
    public static GR.Collections.Map<char, C64Character>  CharToC64Char = new GR.Collections.Map<char,C64Character>();
    public static GR.Collections.Map<byte, byte>          ColorToPetSCIIChar = new GR.Collections.Map<byte, byte>();


    static ConstantData()
    {
      m_ColorValues[0] = 0xff000000;
      m_ColorValues[1] = 0xffffffff;
      m_ColorValues[2] = 0xff8B4131;
      m_ColorValues[3] = 0xff7BBDC5;
      m_ColorValues[4] = 0xff8B41AC;
      m_ColorValues[5] = 0xff6AAC41;
      m_ColorValues[6] = 0xff3931A4;
      m_ColorValues[7] = 0xffD5DE73;
      m_ColorValues[8] = 0xff945A20;
      m_ColorValues[9] = 0xff5A4100;
      m_ColorValues[10] = 0xffBD736A;
      m_ColorValues[11] = 0xff525252;
      m_ColorValues[12] = 0xff838383;
      m_ColorValues[13] = 0xffACEE8B;
      m_ColorValues[14] = 0xff7B73DE;
      m_ColorValues[15] = 0xffACACAC;
      m_ColorValues[16] = 0xff80ff80;
      for ( int i = 0; i < 16; ++i )
      {
        m_Colors[i] = GR.Color.Helper.FromARGB( m_ColorValues[i] );
        m_ColorBrushes[i] = new System.Drawing.SolidBrush( m_Colors[i] );
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
      //PETSCII['_'] = 94;
      //PETSCII['\''] = 95;
      PETSCII['{'] = 123;
      PETSCII['|'] = 166;
      PETSCII['}'] = 125;
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
      AddC64Key( 128 + 3, true, 3, true, ' ', false, "RUN STOP" );   // run stop
      AddC64Key( 128 + 5, true, 5, true, (char)0xee85, true, "WHITE", "WHI" );   // white
      AddC64Key( 128 + 8, true, 8, true, ' ', false, "SHIFT C= OFF", "SH C= OFF" );   // Shift-C= aus
      AddC64Key( 128 + 9, true, 9, true, ' ', false, "SHIFT C= ON", "SH C= ON" );   // Shift-C= an
      AddC64Key( 128 + 13, true, 13, true, ' ', false, "RETURN", "RET" );  // return
      AddC64Key( 128 + 14, true, 14, true, ' ', false, "SHIFT C=", "SH C=" );  // toggle upper/lower
      AddC64Key( 128 + 17, true, 17, true, (char)0xee91, true, "CURSOR DOWN", "CUR DOWN" );  // cursor down
      AddC64Key( 128 + 18, true, 18, true, (char)0xee92, true, "REVERSE ON", "RVS ON" );  // RVS-On
      AddC64Key( 128 + 19, true, 19, true, (char)0xee93, true, "HOME", "HOM" );  // Home
      AddC64Key( 128 + 20, true, 20, true, (char)0xee94, true, "DEL" );  // Del
      AddC64Key( 128 + 28, true, 28, true, (char)0xee9c, true, "RED" );  // Red
      AddC64Key( 128 + 29, true, 29, true, (char)0xee9d, true, "CURSOR RIGHT", "CUR RIGHT" );  // cursor right
      AddC64Key( 128 + 30, true, 30, true, (char)0xee9e, true, "GREEN", "GRN" );  // Green
      AddC64Key( 128 + 31, true, 31, true, (char)0xee9f, true , "BLUE", "BLU" );  // blue
      //  32- 63 $20-$3F       0 $00         32- 63 $20-$3F  
      AddC64Key( 32, true, 32, true, ' ', true );
      AddC64Key( 33, true, 33, true, '!', true );
      AddC64Key( 34, true, 34, true, '"', true );
      AddC64Key( 35, true, 35, true, '#', true );
      AddC64Key( 36, true, 36, true, '$', true );
      AddC64Key( 37, true, 37, true, '%', true );
      AddC64Key( 38, true, 38, true, '&', true );
      AddC64Key( 39, true, 39, true, '\'', true );
      AddC64Key( 40, true, 40, true, '(', true );
      AddC64Key( 41, true, 41, true, ')', true );
      AddC64Key( 42, true, 42, true, '*', true );
      AddC64Key( 43, true, 43, true, '+', true );
      AddC64Key( 44, true, 44, true, ',', true );
      AddC64Key( 45, true, 45, true, '-', true );
      AddC64Key( 46, true, 46, true, '.', true );
      AddC64Key( 47, true, 47, true, '/', true );
      AddC64Key( 48, true, 48, true, '0', true );
      AddC64Key( 49, true, 49, true, '1', true );
      AddC64Key( 50, true, 50, true, '2', true );
      AddC64Key( 51, true, 51, true, '3', true );
      AddC64Key( 52, true, 52, true, '4', true );
      AddC64Key( 53, true, 53, true, '5', true );
      AddC64Key( 54, true, 54, true, '6', true );
      AddC64Key( 55, true, 55, true, '7', true );
      AddC64Key( 56, true, 56, true, '8', true );
      AddC64Key( 57, true, 57, true, '9', true );
      AddC64Key( 58, true, 58, true, ':', true );
      AddC64Key( 59, true, 59, true, ';', true );
      AddC64Key( 60, true, 60, true, '<', true );
      AddC64Key( 61, true, 61, true, '=', true );
      AddC64Key( 62, true, 62, true, '>', true );
      AddC64Key( 63, true, 63, true, '?', true );
      //  64- 95 $40-$5F     -64 $C0          0- 31 $00-$1F  
      AddC64Key( 64 - 64, true, 64, true, '@', true );
      AddC64Key( 65 - 64, true, 65, true, 'A', true );
      AddC64Key( 66 - 64, true, 66, true, 'B', true );
      AddC64Key( 67 - 64, true, 67, true, 'C', true );
      AddC64Key( 68 - 64, true, 68, true, 'D', true );
      AddC64Key( 69 - 64, true, 69, true, 'E', true );
      AddC64Key( 70 - 64, true, 70, true, 'F', true );
      AddC64Key( 71 - 64, true, 71, true, 'G', true );
      AddC64Key( 72 - 64, true, 72, true, 'H', true );
      AddC64Key( 73 - 64, true, 73, true, 'I', true );
      AddC64Key( 74 - 64, true, 74, true, 'J', true );
      AddC64Key( 75 - 64, true, 75, true, 'K', true );
      AddC64Key( 76 - 64, true, 76, true, 'L', true );
      AddC64Key( 77 - 64, true, 77, true, 'M', true );
      AddC64Key( 78 - 64, true, 78, true, 'N', true );
      AddC64Key( 79 - 64, true, 79, true, 'O', true );
      AddC64Key( 80 - 64, true, 80, true, 'P', true );
      AddC64Key( 81 - 64, true, 81, true, 'Q', true );
      AddC64Key( 82 - 64, true, 82, true, 'R', true );
      AddC64Key( 83 - 64, true, 83, true, 'S', true );
      AddC64Key( 84 - 64, true, 84, true, 'T', true );
      AddC64Key( 85 - 64, true, 85, true, 'U', true );
      AddC64Key( 86 - 64, true, 86, true, 'V', true );
      AddC64Key( 87 - 64, true, 87, true, 'W', true );
      AddC64Key( 88 - 64, true, 88, true, 'X', true );
      AddC64Key( 89 - 64, true, 89, true, 'Y', true );
      AddC64Key( 90 - 64, true, 90, true, 'Z', true );
      AddC64Key( 91 - 64, true, 91, true, '[', true );
      AddC64Key( 92 - 64, true, 92, true, '£', true );
      AddC64Key( 93 - 64, true, 93, true, ']', true );
      AddC64Key( 94 - 64, true, 94, true, (char)0xee1e, true );   // arrow up
      AddC64Key( 95 - 64, true, 95, true, (char)0xee1f, true );   // arrow left
      //  96-127 $60-$7F     -32 $E0         64- 95 $40-$5F  
      AddC64Key( 96 - 32, true, 96, true, (char)0xee40, true );   // Shift *
      AddC64Key( 97 - 32, true, 97, true,   (char)0xee41, true );   // Shift-A
      AddC64Key( 98 - 32, true, 98, true,   (char)0xee42, true );   // Shift-B
      AddC64Key( 99 - 32, true, 99, true,   (char)0xee43, true );   // Shift-C
      AddC64Key( 100 - 32, true, 100, true, (char)0xee44, true );   // Shift-D
      AddC64Key( 101 - 32, true, 101, true, (char)0xee45, true );   // Shift-E
      AddC64Key( 102 - 32, true, 102, true, (char)0xee46, true );   // Shift-F
      AddC64Key( 103 - 32, true, 103, true, (char)0xee47, true );   // Shift-G
      AddC64Key( 104 - 32, true, 104, true, (char)0xee48, true );   // Shift-H
      AddC64Key( 105 - 32, true, 105, true, (char)0xee49, true );   // Shift-I
      AddC64Key( 106 - 32, true, 106, true, (char)0xee4a, true );   // Shift-J
      AddC64Key( 107 - 32, true, 107, true, (char)0xee4b, true );   // Shift-K
      AddC64Key( 108 - 32, true, 108, true, (char)0xee4c, true );   // Shift-L
      AddC64Key( 109 - 32, true, 109, true, (char)0xee4d, true );   // Shift-M
      AddC64Key( 110 - 32, true, 110, true, (char)0xee4e, true );   // Shift-N
      AddC64Key( 111 - 32, true, 111, true, (char)0xee4f, true );   // Shift-O
      AddC64Key( 112 - 32, true, 112, true, (char)0xee50, true );   // Shift-P
      AddC64Key( 113 - 32, true, 113, true, (char)0xee51, true );   // Shift-Q
      AddC64Key( 114 - 32, true, 114, true, (char)0xee52, true );   // Shift-R
      AddC64Key( 115 - 32, true, 115, true, (char)0xee53, true );   // Shift-S
      AddC64Key( 116 - 32, true, 116, true, (char)0xee54, true );   // Shift-T
      AddC64Key( 117 - 32, true, 117, true, (char)0xee55, true );   // Shift-U
      AddC64Key( 118 - 32, true, 118, true, (char)0xee56, true );   // Shift-V
      AddC64Key( 119 - 32, true, 119, true, (char)0xee57, true );   // Shift-W
      AddC64Key( 120 - 32, true, 120, true, (char)0xee58, true );   // Shift-X
      AddC64Key( 121 - 32, true, 121, true, (char)0xee59, true );   // Shift-Y
      AddC64Key( 122 - 32, true, 122, true, (char)0xee5a, true );   // Shift-Z
      AddC64Key( 123 - 32, true, 123, true, (char)0xee5b, true );   // Shift +
      AddC64Key( 124 - 32, true, 124, true, (char)0xee5c, true );   // C= -
      AddC64Key( 125 - 32, true, 125, true, (char)0xee5d, true );   // Shift -
      AddC64Key( 126 - 32, true, 126, true, (char)0xee5e, true );   // PI
      AddC64Key( 127 - 32, true, 127, true, (char)0xee5f, true );   // C= *

      // 128-159 $80-$9F     +64 $40        192-223 $C0-$DF  
      AddC64Key( 129 + 64, true, 129, true, (char)0xeec1, true, "ORANGE", "ORN" );   // orange
      AddC64Key( 131 + 64, true, 131, true, ' ', false, "LOAD+RUN", "L+R" );   // TODO load+run
      AddC64Key( 133 + 64, true, 133, true, ' ', false, "F1" );   // TODO F1
      AddC64Key( 134 + 64, true, 134, true, ' ', false, "F3" );   // TODO F3
      AddC64Key( 135 + 64, true, 135, true, ' ', false, "F5" );   // TODO F5
      AddC64Key( 136 + 64, true, 136, true, ' ', false, "F7" );   // TODO F7
      AddC64Key( 137 + 64, true, 137, true, ' ', false, "F2" );   // TODO F2
      AddC64Key( 138 + 64, true, 138, true, ' ', false, "F4" );   // TODO F4
      AddC64Key( 139 + 64, true, 139, true, ' ', false, "F6" );   // TODO F6
      AddC64Key( 140 + 64, true, 140, true, ' ', false, "F8" );   // TODO F8
      AddC64Key( 141 + 64, true, 141, true, ' ', false, "SHIFT RETURN", "SH RET" );   // TODO Shift-Return
      AddC64Key( 142 + 64, true, 142, true, ' ', false, "UPPERCASE", "UPCASE" );   // TODO Uppercase
      AddC64Key( 144 + 64, true, 144, true, (char)0xeed0, true, "BLACK", "BLK" );   // black
      AddC64Key( 145 + 64, true, 145, true, ' ', false, "CURSOR UP", "CUR UP" );   // TODO cursor up
      AddC64Key( 146 + 64, true, 146, true, (char)0xeed2, true, "REVERSE OFF", "RVS OFF" );   // TODO rvs off
      AddC64Key( 147 + 64, true, 147, true, (char)0xeed3, true, "CLEAR", "CLR" );   // TODO clr
      AddC64Key( 148 + 64, true, 148, true, ' ', false, "INSERT", "INS" );   // TODO insert
      AddC64Key( 149 + 64, true, 149, true, (char)0xeed5, true, "BROWN", "BRN" );   // brown
      AddC64Key( 150 + 64, true, 150, true, (char)0xeed6, true, "LIGHT RED", "LRD" );   // light red
      AddC64Key( 151 + 64, true, 151, true, (char)0xeed7, true, "GREY 1", "GR1" );      // grey 1
      AddC64Key( 152 + 64, true, 152, true, (char)0xeed8, true, "GREY 2", "GR2" );      // grey 2
      AddC64Key( 153 + 64, true, 153, true, (char)0xeed9, true, "LIGHT GREEN", "LGR" ); // light green
      AddC64Key( 154 + 64, true, 154, true, (char)0xeeda, true, "LIGHT BLUE", "LBL" );  // light blue
      AddC64Key( 155 + 64, true, 155, true, (char)0xeedb, true, "GREY 3", "GR3" );      // grey 3
      AddC64Key( 156 + 64, true, 156, true, (char)0xeedc, true, "PURPLE", "PUR" );      // purple
      AddC64Key( 157 + 64, true, 157, true, ' ', false, "CURSOR LEFT", "CUR LEFT" );   // TODO cursor left
      AddC64Key( 158 + 64, true, 158, true, (char)0xeede, true, "YELLOW", "YEL" );    // yellow
      AddC64Key( 159 + 64, true, 159, true, (char)0xeedf, true, "CYAN", "CYN" );      // TODO cyan

      // 160-191 $A0-$BF     -64 $C0         96-127 $60-$7F  
      AddC64Key( 160 - 64, true, 160, true, (char)0xee60, true );   // shift-space
      AddC64Key( 161 - 64, true, 161, true, (char)0xee61, true );   // C= K
      AddC64Key( 162 - 64, true, 162, true, (char)0xee62, true );   // C= I
      AddC64Key( 163 - 64, true, 163, true, (char)0xee63, true );   // C= T
      AddC64Key( 164 - 64, true, 164, true, (char)0xee64, true );   // C= @
      AddC64Key( 165 - 64, true, 165, true, (char)0xee65, true );   // C= G
      AddC64Key( 166 - 64, true, 166, true, (char)0xee66, true );   // C= +
      AddC64Key( 167 - 64, true, 167, true, (char)0xee67, true );   // C= N
      AddC64Key( 168 - 64, true, 168, true, (char)0xee68, true );   // C= Insert 
      AddC64Key( 169 - 64, true, 169, true, (char)0xee69, true );   // Shift Insert 
      AddC64Key( 170 - 64, true, 170, true, (char)0xee6a, true );   // C= M
      AddC64Key( 171 - 64, true, 171, true, (char)0xee6b, true );   // C= Q
      AddC64Key( 172 - 64, true, 172, true, (char)0xee6c, true );   // C= D
      AddC64Key( 173 - 64, true, 173, true, (char)0xee6d, true );   // C= Y
      AddC64Key( 174 - 64, true, 174, true, (char)0xee6e, true );   // C= S
      AddC64Key( 175 - 64, true, 175, true, (char)0xee6f, true );   // C= P
      AddC64Key( 176 - 64, true, 176, true, (char)0xee70, true );   // C= A 
      AddC64Key( 177 - 64, true, 177, true, (char)0xee71, true );   // C= E
      AddC64Key( 178 - 64, true, 178, true, (char)0xee72, true );   // C= R
      AddC64Key( 179 - 64, true, 179, true, (char)0xee73, true );   // C= W
      AddC64Key( 180 - 64, true, 180, true, (char)0xee74, true );   // C= H
      AddC64Key( 181 - 64, true, 181, true, (char)0xee75, true );   // C= J
      AddC64Key( 182 - 64, true, 182, true, (char)0xee76, true );   // C= L
      AddC64Key( 183 - 64, true, 183, true, (char)0xee77, true );   // C= Z
      AddC64Key( 184 - 64, true, 184, true, (char)0xee78, true );   // C= U
      AddC64Key( 185 - 64, true, 185, true, (char)0xee79, true );   // C= O
      AddC64Key( 186 - 64, true, 186, true, (char)0xee7a, true );   // Shift @
      AddC64Key( 187 - 64, true, 187, true, (char)0xee7b, true );   // C= F
      AddC64Key( 188 - 64, true, 188, true, (char)0xee7c, true );   // C= C
      AddC64Key( 189 - 64, true, 189, true, (char)0xee7d, true );   // C= X
      AddC64Key( 190 - 64, true, 190, true, (char)0xee7e, true );   // C= V
      AddC64Key( 191 - 64, true, 191, true, (char)0xee7f, true );   // C= B

      // 192-223 $C0-$DF    -128 $80         64- 95 $40-$5F  
      AddC64Key( 192 - 128, true, 192, true, ' ', false );   // TODO mittelstrich
      AddC64Key( 193 - 128, true, 193, true, (char)0xee41, true );   // Shift-A
      AddC64Key( 194 - 128, true, 194, true, (char)0xee42, true );   // Shift-B
      AddC64Key( 195 - 128, true, 195, true, (char)0xee43, true );   // Shift-C
      AddC64Key( 196 - 128, true, 196, true, (char)0xee44, true );   // Shift-D
      AddC64Key( 197 - 128, true, 197, true, (char)0xee45, true );   // Shift-E
      AddC64Key( 198 - 128, true, 198, true, (char)0xee46, true );   // Shift-F
      AddC64Key( 199 - 128, true, 199, true, (char)0xee47, true );   // Shift-G
      AddC64Key( 200 - 128, true, 200, true, (char)0xee48, true );   // Shift-H
      AddC64Key( 201 - 128, true, 201, true, (char)0xee49, true );   // Shift-I
      AddC64Key( 202 - 128, true, 202, true, (char)0xee4a, true );   // Shift-J
      AddC64Key( 203 - 128, true, 203, true, (char)0xee4b, true );   // Shift-K
      AddC64Key( 204 - 128, true, 204, true, (char)0xee4c, true );   // Shift-L
      AddC64Key( 205 - 128, true, 205, true, (char)0xee4d, true );   // Shift-M
      AddC64Key( 206 - 128, true, 206, true, (char)0xee4e, true );   // Shift-N
      AddC64Key( 207 - 128, true, 207, true, (char)0xee4f, true );   // Shift-O
      AddC64Key( 208 - 128, true, 208, true, (char)0xee50, true );   // Shift-P
      AddC64Key( 209 - 128, true, 209, true, (char)0xee51, true );   // Shift-Q
      AddC64Key( 210 - 128, true, 210, true, (char)0xee52, true );   // Shift-R
      AddC64Key( 211 - 128, true, 211, true, (char)0xee53, true );   // Shift-S
      AddC64Key( 212 - 128, true, 212, true, (char)0xee54, true );   // Shift-T
      AddC64Key( 213 - 128, true, 213, true, (char)0xee55, true );   // Shift-U
      AddC64Key( 214 - 128, true, 214, true, (char)0xee56, true );   // Shift-V
      AddC64Key( 215 - 128, true, 215, true, (char)0xee57, true );   // Shift-W
      AddC64Key( 216 - 128, true, 216, true, (char)0xee58, true );   // Shift-X
      AddC64Key( 217 - 128, true, 217, true, (char)0xee5a, true );   // Shift-Y
      AddC64Key( 218 - 128, true, 218, true, (char)0xee59, true );   // Shift-Z
      AddC64Key( 219 - 128, true, 219, true, ' ', false );   // TODO
      AddC64Key( 220 - 128, true, 220, true, ' ', false );   // TODO
      AddC64Key( 221 - 128, true, 221, true, ' ', false );   // TODO
      AddC64Key( 222 - 128, true, 222, true, ' ', false );   // TODO
      AddC64Key( 223 - 128, true, 223, true, ' ', false );   // TODO

      // 224-254 $E0-$FE    -128 $80         96-126 $60-$7E  
      AddC64Key( 224 - 128, true, 224, true, (char)0xee60, true );   // shift-space
      AddC64Key( 225 - 128, true, 225, true, (char)0xee61, true );   // C= K
      AddC64Key( 226 - 128, true, 226, true, (char)0xee62, true );   // C= I
      AddC64Key( 227 - 128, true, 227, true, (char)0xee63, true );   // C= T
      AddC64Key( 228 - 128, true, 228, true, (char)0xee64, true );   // C= @
      AddC64Key( 229 - 128, true, 229, true, (char)0xee65, true );   // C= G
      AddC64Key( 230 - 128, true, 230, true, (char)0xee66, true );   // C= +
      AddC64Key( 231 - 128, true, 231, true, (char)0xee67, true );   // C= N
      AddC64Key( 232 - 128, true, 232, true, (char)0xee68, true );   // C= Insert 
      AddC64Key( 233 - 128, true, 233, true, (char)0xee69, true );   // Shift Insert 
      AddC64Key( 234 - 128, true, 234, true, (char)0xee6a, true );   // C= M
      AddC64Key( 235 - 128, true, 235, true, (char)0xee6b, true );   // C= Q
      AddC64Key( 236 - 128, true, 236, true, (char)0xee6c, true );   // C= D
      AddC64Key( 237 - 128, true, 237, true, (char)0xee6d, true );   // C= Y
      AddC64Key( 238 - 128, true, 238, true, (char)0xee6e, true );   // C= S
      AddC64Key( 239 - 128, true, 239, true, (char)0xee6f, true );   // C= P
      AddC64Key( 240 - 128, true, 240, true, (char)0xee70, true );   // C= A 
      AddC64Key( 241 - 128, true, 241, true, (char)0xee71, true );   // C= E
      AddC64Key( 242 - 128, true, 242, true, (char)0xee72, true );   // C= R
      AddC64Key( 243 - 128, true, 243, true, (char)0xee73, true );   // C= W
      AddC64Key( 244 - 128, true, 244, true, (char)0xee74, true );   // C= H
      AddC64Key( 245 - 128, true, 245, true, (char)0xee75, true );   // C= J
      AddC64Key( 246 - 128, true, 246, true, (char)0xee76, true );   // C= L
      AddC64Key( 247 - 128, true, 247, true, (char)0xee77, true );   // C= Z
      AddC64Key( 248 - 128, true, 248, true, (char)0xee78, true );   // C= U
      AddC64Key( 249 - 128, true, 249, true, (char)0xee79, true );   // C= O
      AddC64Key( 250 - 128, true, 250, true, (char)0xee7a, true );   // Shift @
      AddC64Key( 251 - 128, true, 251, true, (char)0xee7b, true );   // C= F
      AddC64Key( 252 - 128, true, 252, true, (char)0xee7c, true );   // C= C
      AddC64Key( 253 - 128, true, 253, true, (char)0xee7d, true );   // C= X
      AddC64Key( 254 - 128, true, 254, true, (char)0xee7e, true );   // C= V

      // 255 $FF                                 94 $5E  
      AddC64Key( 255, true, 126, true, (char)0xee5e, true );   // PI

      // reverse keys!
      AddC64Key( 160, true, 0, false, (char)0xeee0, true );   // reverse space
    }



    static void AddC64Key( byte ScreenCodeValue, bool HasScreenCode, byte PetSCIIValue, bool HasPetSCII, char CharValue, bool HasChar )
    {
      AddC64Key( ScreenCodeValue, HasScreenCode, PetSCIIValue, HasPetSCII, CharValue, HasChar, "" + CharValue, "" );
    }



    static void AddC64Key( byte ScreenCodeValue, bool HasScreenCode, byte PetSCIIValue, bool HasPetSCII, char CharValue, bool HasChar, string Desc )
    {
      AddC64Key( ScreenCodeValue, HasScreenCode, PetSCIIValue, HasPetSCII, CharValue, HasChar, Desc, "" );
    }



    static void AddC64Key( byte ScreenCodeValue, bool HasScreenCode, byte PetSCIIValue, bool HasPetSCII, char CharValue, bool HasChar, string Desc, string ShortDesc )
    {
      C64Character  c64Char = new C64Character( ScreenCodeValue, HasScreenCode, PetSCIIValue, HasPetSCII, CharValue, HasChar, Desc, ShortDesc );
      if ( HasScreenCode )
      {
        ScreenCodeToChar[ScreenCodeValue] = c64Char;
      }
      if ( HasPetSCII )
      {
        PetSCIIToChar[PetSCIIValue] = c64Char;
      }
      if ( HasChar )
      {
        CharToC64Char[CharValue] = c64Char;
      }
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
      MACRO,
      SEPARATOR,
      LABEL_INTERNAL
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
  };
}
