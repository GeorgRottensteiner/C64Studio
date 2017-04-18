using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio
{
  public class DependencyBuildState
  {
    public GR.Collections.Map<string,DateTime>      BuildState = new GR.Collections.Map<string,DateTime>();


    public void Clear()
    {
      BuildState.Clear();
    }
  }
}
