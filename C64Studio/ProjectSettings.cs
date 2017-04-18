using C64Studio.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio
{
  public class ProjectSettings
  {
    public string       Name = "";
    public string       Filename = null;
    public string       BasePath = null;
    public ushort       DebugPort = 6510;
    public ushort       DebugStartAddress = 0x0801;
    public string       BuildTool = "";
    public string       RunTool = "";
    public string       MainDocument = "";
    public GR.Collections.Map<string,ProjectConfig>         Configs = new GR.Collections.Map<string, ProjectConfig>();
    public ProjectConfig    CurrentConfig = null;
  }
}
