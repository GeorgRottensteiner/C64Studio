namespace RetroDevStudio.Parser
{
  public class LabelDumpSettings
  {
    public string       Filename = "";
    public bool         IncludeAssemblerIDLabels = true;  // include assembler ID labels
    public bool         IgnoreUnusedLabels = false;       // any unusused labels are omitted
    public bool         IgnoreInternalLabels = false;     // any C64STUDIO_INTERNAL... are omitted

  };
}
