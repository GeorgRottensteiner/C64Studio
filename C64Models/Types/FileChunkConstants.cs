namespace RetroDevStudio
{
  public class FileChunkConstants
  {
    public const ushort    RESTART_INFO          = 0x0100;
    public const ushort    RESTART_DATA          = 0x0101;
    public const ushort    RESTART_DOC_INFO      = 0x0102;

    public const ushort    SOLUTION              = 0x0400;
    public const ushort    SOLUTION_INFO         = 0x0401;
    public const ushort    SOLUTION_PROJECT      = 0x0402;
    public const ushort    SOLUTION_NODES        = 0x0403;

    public const ushort    PROJECT               = 0x1000;
    public const ushort    PROJECT_ELEMENT       = 0x1001;
    public const ushort    PROJECT_ELEMENT_DATA  = 0x1002;
    public const ushort    PROJECT_ELEMENT_DISPLAY_DATA        = 0x1003;
    public const ushort    PROJECT_ELEMENT_PER_CONFIG_SETTING  = 0x1004;
    public const ushort    PROJECT_ELEMENT_FOLDED_BLOCKS       = 0x1005;
    public const ushort    PROJECT_CONFIG        = 0x1100;
    public const ushort    PROJECT_WATCH_ENTRY   = 0x1101;

    public const ushort    CHARSET_SCREEN_INFO   = 0x1200;
    public const ushort    SCREEN_CHAR_DATA      = 0x1300;
    public const ushort    SCREEN_COLOR_DATA     = 0x1301;
    public const ushort    GRAPHIC_SCREEN_INFO   = 0x1310;
    public const ushort    GRAPHIC_DATA          = 0x1311;   // uint width, uint height, uint image type, uint palette entry count, byte r,g,b, uint data size, data
    public const ushort    GRAPHIC_COLOR_MAPPING = 0x1312;   // Dictionary<int,List<byte>>

    public const ushort    MAP_PROJECT_INFO      = 0x1320;
    public const ushort    MAP_PROJECT_DATA      = 0x1321;
    public const ushort    MAP_TILE              = 0x1322;
    public const ushort    MAP                   = 0x1324;
    public const ushort    MAP_INFO              = 0x1325;
    public const ushort    MAP_DATA              = 0x1326;
    public const ushort    MAP_EXTRA_DATA        = 0x1327;
    public const ushort    MAP_CHARSET           = 0x1328;
    public const ushort    MAP_EXTRA_DATA_TEXT   = 0x1329;   // replaces MAP_EXTRA_DATA

    public const ushort    SOURCE_ASM            = 0x1330;
    public const ushort    SOURCE_BASIC          = 0x1331;

    public const ushort    CHARSET_PROJECT       = 0x1340;
    public const ushort    CHARSET_INFO          = 0x1341;
    public const ushort    CHARSET_CHAR          = 0x1342;
    public const ushort    CHARSET_COLOR_SETTINGS= 0x1343;
    public const ushort    CHARSET_PLAYGROUND    = 0x1344;
    public const ushort    CHARSET_EXPORT        = 0x1345;
    public const ushort    CHARSET_CATEGORY      = 0x1346;

    public const ushort    SPRITESET_PROJECT     = 0x13E0;
    public const ushort    SPRITESET_INFO        = 0x13E1;
    public const ushort    SPRITESET_SPRITE      = 0x13E2;
    public const ushort    SPRITESET_LAYER       = 0x1400;
    public const ushort    SPRITESET_LAYER_ENTRY = 0x1401;
    public const ushort    SPRITESET_LAYER_INFO  = 0x1402;

    public const ushort    MULTICOLOR_DATA       = 0x1500;
    public const ushort    CHARSET_DATA          = 0x1501;   // multicolor-data und binary data
    public const ushort    PALETTE                = 0x1502;   // int num entries, n * ARGB (uint)

    public const ushort    DISASSEMBLY_INFO      = 0x1600;
    public const ushort    DISASSEMBLY_DATA      = 0x1601;
    public const ushort    DISASSEMBLY_JUMP_ADDRESSES = 0x1602;
    public const ushort    DISASSEMBLY_NAMED_LABELS = 0x1603;

    public const ushort    SETTINGS_TOOL         = 0x2000;
    public const ushort    SETTINGS_ACCELERATOR  = 0x2001;
    public const ushort    SETTINGS_SOUND        = 0x2002;
    public const ushort    SETTINGS_WINDOW       = 0x2003;
    public const ushort    SETTINGS_TEXT_EDITOR  = 0x2004;
    public const ushort    SETTINGS_FONT         = 0x2005;
    public const ushort    SETTINGS_SYNTAX_COLORING = 0x2006;
    public const ushort    SETTINGS_UI           = 0x2007;
    public const ushort    SETTINGS_DEFAULTS     = 0x2008;
    public const ushort    SETTINGS_FIND_REPLACE = 0x2009;
    public const ushort    SETTINGS_IGNORED_WARNINGS = 0x200A;
    public const ushort    SETTINGS_LAYOUT       = 0x200B;   // do not use anymore!
    public const ushort    SETTINGS_PANEL_DISPLAY_DETAILS = 0x200C;
    public const ushort    SETTINGS_DPS_LAYOUT   = 0x200D;
    public const ushort    SETTINGS_RUN_EMULATOR = 0x200E;
    public const ushort    SETTINGS_BASIC_KEYMAP = 0x200F;
    public const ushort    SETTINGS_BASIC_PARSER = 0x2010;
    public const ushort    SETTINGS_ASSEMBLER_EDITOR = 0x2011;
    public const ushort    SETTINGS_ENVIRONMENT  = 0x2012;
    public const ushort    SETTINGS_PERSPECTIVES = 0x2013;
    public const ushort    SETTINGS_PERSPECTIVE  = 0x2014;
    public const ushort    SETTINGS_OUTLINE      = 0x2015;
    public const ushort    SETTINGS_HEX_VIEW     = 0x2016;
    public const ushort    SETTINGS_MRU_PROJECTS = 0x2017;
    public const ushort    SETTINGS_MRU_FILES    = 0x2018;
    public const ushort    SETTINGS_WARNINGS_AS_ERRORS = 0x2019;
    public const ushort    SETTINGS_C64STUDIO_HACKS    = 0x201A;
  }

}