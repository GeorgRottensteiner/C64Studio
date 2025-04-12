using System.Collections.Generic;



namespace RetroDevStudio.Emulators
{
  public class EmulatorDefinition
  {
    public string                               Name = "";

    public List<MachineType>                    SupportedMachines = new List<MachineType>();

    // order also defines order of passing arguments!
    public List<DynamicArgument>                SupportedArguments = new List<DynamicArgument>();

    public Dictionary<DynamicArgument,string>   DefaultArguments = new Dictionary<DynamicArgument, string>();



  }
}