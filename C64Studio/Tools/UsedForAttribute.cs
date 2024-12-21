using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RetroDevStudio
{
  [AttributeUsage( AttributeTargets.All, AllowMultiple = true )]
  internal class UsedForAttribute : Attribute
  {
    private List<MachineType>   _machines = new List<MachineType>();



    public bool UsedInMachine( MachineType machine )
    {
      return _machines.Contains( machine );
    }



    public ReadOnlyCollection<MachineType> Machines
    {
      get
      {
        return new ReadOnlyCollection<MachineType>( _machines );
      }
    }



    public UsedForAttribute( MachineType machine )
    {
      if ( !_machines.Contains( machine ) )
      {
        _machines.Add( machine );
      }
    }



  }
}