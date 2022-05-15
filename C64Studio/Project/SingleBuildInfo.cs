using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace RetroDevStudio
{
  public class SingleBuildInfo
  {
    public string             TargetFile = "";
    public CompileTargetType  TargetType = CompileTargetType.NONE;
    public DateTime           TimeStampOfSourceFile = default( DateTime );
    public DateTime           TimeStampOfTargetFile = default( DateTime );
  }
}
