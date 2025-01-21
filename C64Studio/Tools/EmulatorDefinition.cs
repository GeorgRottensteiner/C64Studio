using System;
using System.Collections.Generic;
using System.Text;



namespace RetroDevStudio
{
  public class EmulatorDefinition
  {
    public string                         Name = "";

    // order also defines order of passing arguments!
    public List<ToolInfo.DynamicArgument> SupportedArguments = new List<ToolInfo.DynamicArgument>();

    public Dictionary<ToolInfo.DynamicArgument,string>   DefaultArguments = new Dictionary<ToolInfo.DynamicArgument, string>();

  }



}
