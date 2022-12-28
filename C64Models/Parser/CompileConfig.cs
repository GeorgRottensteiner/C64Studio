using GR.Collections;
using System;
using System.Collections.Generic;
using System.Text;



namespace RetroDevStudio.Parser
{
  public class CompileConfig
  {
    public string                                 InputFile = "";
    public string                                 OutputFile = null;
    public string                                 LabelDumpFile = null;
    public int                                    StartAddress = -1;
    public RetroDevStudio.Types.CompileTargetType TargetType = RetroDevStudio.Types.CompileTargetType.NONE;
    public Types.AssemblerType                    Assembler = Types.AssemblerType.AUTO;
    public bool                                   AutoTruncateLiteralValues = false;
    public bool                                   CreatePreProcesseFile = false;
    public bool                                   CreateRelocationFile = false;
    public bool                                   DoNotExpandStringLiterals = false;
    public List<string>                           LibraryFiles = new List<string>();
    public Set<Types.ErrorCode>                   WarningsToTreatAsError = new Set<Types.ErrorCode>();
    public Set<AssemblerSettings.Hacks>           EnabledHacks = new Set<AssemblerSettings.Hacks>();
    public Encoding                               Encoding = new System.Text.UTF8Encoding( false );
  }
}
