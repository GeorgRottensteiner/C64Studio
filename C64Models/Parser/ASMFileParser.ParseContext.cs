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
      // whether ParseValue adds references during evaluation
      public bool         DoNotAddReferences    = false;

      // used for 65816 assembly
      public bool         Assume16BitAccu       = false;
      public bool         Assume16BitRegisters  = false;

      public bool         IsScopingActive       = true;

      public int          LineIndex             = -1;

      public string       ParentFilename        = "";

      // only set to true during DetermineUnparsedLabels
      public bool         DuringExpressionEvaluation = false;


      public GR.Collections.Map<string,GR.Collections.Map<byte, byte>>    TextMappings = new GR.Collections.Map<string, GR.Collections.Map<byte, byte>>();

      public SortedList<int,string>     ForwardLabelStacked = new SortedList<int, string>();

      public List<ScopeInfo>            Scopes = new List<ScopeInfo>();
      

      public void Clear()
      {
        LineIndex           = -1;

        ForwardLabelStacked.Clear();
        TextMappings.Clear();
        Scopes.Clear();
      }

    }



  }
}
