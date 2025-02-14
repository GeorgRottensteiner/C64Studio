using System;

namespace RetroDevStudio
{
  [AttributeUsage( AttributeTargets.Field | AttributeTargets.Class, AllowMultiple = true )]
  internal class MachineTypeAttribute : Attribute
  {
    public MachineType MachineType = MachineType.ANY;


    public MachineTypeAttribute( MachineType machineType )
    {
      MachineType = machineType;
    }


  }
}