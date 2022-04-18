using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio
{
  public class DependencyBuildState
  {
    public GR.Collections.Map<string,SingleBuildInfo>      BuildState = new GR.Collections.Map<string,SingleBuildInfo>();


    public void Clear()
    {
      BuildState.Clear();
    }
  }
}
