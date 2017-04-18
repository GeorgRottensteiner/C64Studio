using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio
{
  public class Debugging
  {
    public RemoteDebugger   Debugger              = null;
    public BaseDocument     MarkedDocument        = null;
    public int              MarkedDocumentLine    = -1;
    public int              CurrentCodePosition   = -1;
    public Project          DebuggedProject       = null;
    public int              OverrideDebugStart    = -1;
    public int              LateBreakpointOverrideDebugStart = -1;
    public bool             FirstActionAfterBreak = false;
    public string           TempDebuggerStartupFilename = "";
    public DocumentInfo     DebuggedASMBase       = null;
    public DocumentInfo     DebugBaseDocumentRun  = null;
    public Disassembly      DebugDisassembly      = null;

    public GR.Collections.Map<string, List<Types.Breakpoint>> BreakPoints = new GR.Collections.Map<string, List<C64Studio.Types.Breakpoint>>();
    public List<Types.Breakpoint>                             BreakpointsToAddAfterStartup = new List<C64Studio.Types.Breakpoint>();


  }
}
