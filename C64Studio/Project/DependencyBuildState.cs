using System;
using System.Collections.Generic;
using System.Text;

namespace RetroDevStudio
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
