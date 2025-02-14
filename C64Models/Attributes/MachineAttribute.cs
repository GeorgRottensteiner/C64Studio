using System;



namespace RetroDevStudio
{
  [AttributeUsage( AttributeTargets.Field | AttributeTargets.Class, AllowMultiple = true )]
  public class MachineAttribute : Attribute
  {
    public MachineType  Machine = MachineType.ANY;



    public MachineAttribute( MachineType machine )
    {
      Machine = machine;
    }



  }

}