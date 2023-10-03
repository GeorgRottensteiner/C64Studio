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
    private ParseLineResult PO65816Assume16BitAccu( int lineIndex )
    {
      if ( m_ASMFileInfo.Processor.Name != "65816" )
      {
        AddError( lineIndex, Types.ErrorCode.E1311_UNSUPPORTED_CPU, "Unsupported CPU type for !al, only allowed with 65816." );
        return ParseLineResult.RETURN_NULL;
      }
      m_Assume16BitAccu = true;
      return ParseLineResult.OK;
    }



    private ParseLineResult PO65816Assume8BitAccu( int lineIndex )
    {
      if ( m_ASMFileInfo.Processor.Name != "65816" )
      {
        AddError( lineIndex, Types.ErrorCode.E1311_UNSUPPORTED_CPU, "Unsupported CPU type for !as, only allowed with 65816." );
        return ParseLineResult.RETURN_NULL;
      }
      m_Assume16BitAccu = false;
      return ParseLineResult.OK;
    }



    private ParseLineResult PO65816Assume16BitRegisters( int lineIndex )
    {
      if ( m_ASMFileInfo.Processor.Name != "65816" )
      {
        AddError( lineIndex, Types.ErrorCode.E1311_UNSUPPORTED_CPU, "Unsupported CPU type for !rl, only allowed with 65816." );
        return ParseLineResult.RETURN_NULL;
      }
      m_Assume16BitRegisters = true;
      return ParseLineResult.OK;
    }



    private ParseLineResult PO65816Assume8BitRegisters( int lineIndex )
    {
      if ( m_ASMFileInfo.Processor.Name != "65816" )
      {
        AddError( lineIndex, Types.ErrorCode.E1311_UNSUPPORTED_CPU, "Unsupported CPU type for !rs, only allowed with 65816." );
        return ParseLineResult.RETURN_NULL;
      }
      m_Assume16BitRegisters = false;
      return ParseLineResult.OK;
    }



  }
}
