﻿using GR.Memory;
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
    private ParseLineResult PO65816Assume16BitAccu()
    {
      if ( m_ASMFileInfo.Processor.Name != "65816" )
      {
        AddError( _ParseContext.LineIndex, Types.ErrorCode.E1311_UNSUPPORTED_CPU, "Unsupported CPU type for !al, only allowed with 65816." );
        return ParseLineResult.RETURN_NULL;
      }
      _ParseContext.Assume16BitAccu = true;
      return ParseLineResult.OK;
    }



    private ParseLineResult PO65816Assume8BitAccu()
    {
      if ( m_ASMFileInfo.Processor.Name != "65816" )
      {
        AddError( _ParseContext.LineIndex, Types.ErrorCode.E1311_UNSUPPORTED_CPU, "Unsupported CPU type for !as, only allowed with 65816." );
        return ParseLineResult.RETURN_NULL;
      }
      _ParseContext.Assume16BitAccu = false;
      return ParseLineResult.OK;
    }



    private ParseLineResult PO65816Assume16BitRegisters()
    {
      if ( m_ASMFileInfo.Processor.Name != "65816" )
      {
        AddError( _ParseContext.LineIndex, Types.ErrorCode.E1311_UNSUPPORTED_CPU, "Unsupported CPU type for !rl, only allowed with 65816." );
        return ParseLineResult.RETURN_NULL;
      }
      _ParseContext.Assume16BitRegisters = true;
      return ParseLineResult.OK;
    }



    private ParseLineResult PO65816Assume8BitRegisters()
    {
      if ( m_ASMFileInfo.Processor.Name != "65816" )
      {
        AddError( _ParseContext.LineIndex, Types.ErrorCode.E1311_UNSUPPORTED_CPU, "Unsupported CPU type for !rs, only allowed with 65816." );
        return ParseLineResult.RETURN_NULL;
      }
      _ParseContext.Assume16BitRegisters = false;
      return ParseLineResult.OK;
    }



  }
}
