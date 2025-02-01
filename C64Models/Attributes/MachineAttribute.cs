using System;



namespace RetroDevStudio
{
  [AttributeUsage( AttributeTargets.Field | AttributeTargets.Class )]
  public class MachineAttribute : Attribute
  {
    public MachineType  Machine = MachineType.ANY;



    public MachineAttribute( MachineType machine )
    {
      Machine = machine;
    }



  }

}