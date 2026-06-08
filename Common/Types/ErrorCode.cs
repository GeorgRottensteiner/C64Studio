using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using RetroDevStudio;
using RetroDevStudio.Types;
using GR.Collections;
using RetroDevStudio.Emulators;

namespace RetroDevStudio.Types
{

  public enum ErrorCode
  {
    OK                                      = 0,

    E0001_NO_OUTPUT_FILENAME                = 0x0001,
    E0002_CODE_WITHOUT_START_ADDRESS        = 0x0002,

    E0100_FAILED_TO_WRITE_TO_FILE           = 0x0100,

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
    E1011_TYPE_MISMATCH                     = 0x1011,
    E1012_IMPLEMENTATION_MISSING            = 0x1012,
    E1013_VALUE_OUT_OF_BOUNDS_24BIT         = 0x1013,
    E1014_VALUE_OUT_OF_BOUNDS_RANGE         = 0x1014,
    E1015_VALUE_OUT_OF_BOUNDS_32BIT         = 0x1015,
    E1016_VALUE_OUT_OF_BOUNDS_7BIT          = 0x1016,
    E1017_VALUE_OUT_OF_BOUNDS_15BIT         = 0x1017,

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
    E1402_DUPLICATE_INCLUSION               = 0x1402,

    E2000_FILE_OPEN_ERROR                   = 0x2000,
    E2001_FILE_READ_ERROR                   = 0x2001,
    E2002_UNSUPPORTED_FILE_TYPE             = 0x2002,

    [UsedForBASIC]
    E3000_BASIC_MISSING_LINE_NUMBER         = 0x3000,
    [UsedForBASIC]
    E3001_BASIC_INVALID_LINE_NUMBER         = 0x3001,
    [UsedForBASIC]
    E3002_BASIC_UNSUPPORTED_CHARACTER       = 0x3002,
    [UsedForBASIC]
    E3003_BASIC_LABEL_MALFORMED             = 0x3003,
    [UsedForBASIC]
    E3004_BASIC_MISSING_LABEL               = 0x3004,
    [UsedForBASIC]
    E3005_BASIC_UNKNOWN_MACRO               = 0x3005,
    [UsedForBASIC]
    E3006_BASIC_LINE_TOO_LONG               = 0x3006,
    [UsedForBASIC]
    E3007_BASIC_MALFORMED_METADATA          = 0x3007,
    [UsedForBASIC]
    E3008_BASIC_DOES_NOT_MATCH_LABEL_MODE   = 0x3008,

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
    [Description( "W0008: Opcode used as a label" )]
    W0008_OPCODE_USED_AS_LABEL              = 0x8007,
    [Description( "W1000: Unused label" )]
    W1000_UNUSED_LABEL                      = 0x9003,

    [UsedForBASIC]
    [Description( "W1001: BASIC line is too long for manual entry" )]
    W1001_BASIC_LINE_TOO_LONG_FOR_MANUAL_ENTRY = 0x9004,
    [UsedForBASIC]
    [Description( "W1002: BASIC variable potentially ambiguous" )]
    W1002_BASIC_VARIABLE_POTENTIALLY_AMBIGUOUS = 0x9005,
    [UsedForBASIC]
    [Description( "W1003: BASIC referenced line number is missing" )]
    W1003_BASIC_REFERENCED_LINE_NUMBER_NOT_FOUND,

    [Description( "W1004: Unused Constant" )]
    W1004_UNUSED_CONSTANT,

    WARNING_LAST_PLUS_ONE
  }



}