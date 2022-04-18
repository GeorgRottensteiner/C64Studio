using C64Studio.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio
{
  public class SingleBuildInfo
  {
    public string             TargetFile = "";
    public CompileTargetType  TargetType = CompileTargetType.NONE;
    public DateTime           TimeStampOfSourceFile = default( DateTime );
    public DateTime           TimeStampOfTargetFile = default( DateTime );
  }
}
