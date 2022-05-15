using System;
using System.Collections.Generic;
using System.Text;

namespace RetroDevStudio.Types
{
  public class BuildChain
  {
    public List<BuildChainEntry>    Entries = new List<BuildChainEntry>();
    public bool                     Active = false;



    internal void AddEntry( BuildChainEntry Entry )
    {
      Entries.Add( Entry );
    }
  }

}
