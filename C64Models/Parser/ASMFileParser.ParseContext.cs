using GR.Memory;
using RetroDevStudio.Formats;
using RetroDevStudio.Parser;
using RetroDevStudio.Types;
using RetroDevStudio.Types.ASM;
using System;
using System.Collections.Generic;



namespace RetroDevStudio.Parser
{
  public partial class ASMFileParser : ParserBase
  {
    public class ParseContext
    {
      public GR.Collections.Map<string,GR.Collections.Map<byte, byte>>    TextMappings = new GR.Collections.Map<string, GR.Collections.Map<byte, byte>>();

      public SortedList<int,string>    ForwardLabelStacked = new SortedList<int, string>();



      public void Clear()
      {
        ForwardLabelStacked.Clear();
        TextMappings.Clear();
      }

    }



  }
}
