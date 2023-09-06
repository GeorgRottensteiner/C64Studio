using GR.Memory;
using RetroDevStudio.Formats;
using RetroDevStudio.Parser;
using RetroDevStudio.Types;
using RetroDevStudio.Types.ASM;
using System;
using System.Collections.Generic;
using Tiny64;

namespace RetroDevStudio.Parser
{
  public partial class ASMFileParser : ParserBase
  {
    private ParseLineResult POCPU( List<TokenInfo> lineTokenInfos, int lineIndex )
    {
      if ( lineTokenInfos.Count != 2 )
      {
        AddError( lineIndex, Types.ErrorCode.E1311_UNSUPPORTED_CPU, "Unsupported CPU type, currently only 6510, 65C02, R65C02, W65C02, 65CE02, 4502, M65 and 65816 are supported" );
        return ParseLineResult.RETURN_NULL;
      }

      string  cpuType = lineTokenInfos[1].Content.ToUpper();

      switch ( cpuType )
      {
        case "6502":
          m_Processor = Processor.Create6502();
          break;
        case "nmos6502":
        case "6510":
          m_Processor = Processor.Create6510();
          break;
        case "65C02":
          m_Processor = Processor.Create65C02();
          break;
        case "R65C02":
          m_Processor = Processor.CreateR65C02();
          break;
        case "W65C02":
          m_Processor = Processor.CreateWDC65C02();
          break;
        case "65CE02":
          m_Processor = Processor.Create65CE02();
          break;
        case "4502":
          m_Processor = Processor.Create4502();
          break;
        case "M65":
          m_Processor = Processor.CreateM65();
          break;
        case "65816":
          m_Processor = Processor.Create65816();
          break;
        default:
          AddError( lineIndex, Types.ErrorCode.E1311_UNSUPPORTED_CPU, "Unsupported CPU type, currently only 6510, 65C02, R65C02, W65C02, 65CE02, 4502, M65 and 65816 are supported" );
          return ParseLineResult.RETURN_NULL;
      }

      ASMFileInfo.Processor = m_Processor;
      return ParseLineResult.OK;
    }



  }
}
