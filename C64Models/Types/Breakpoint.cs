using System;
using System.Collections.Generic;
using System.Text;

namespace RetroDevStudio.Types
{
  public class Breakpoint
  {
    public string             DocumentFilename = "";
    public int                LineIndex = -1;
    public int                Address = -1;
    public string             AddressSource = null;
    public int                RemoteIndex = -1;
    public bool               Temporary = false;
    public bool               IsVirtual = false;
    public List<Breakpoint>   Virtual = new List<Breakpoint>();      // for trace breakpoints

    public string             Conditions = "";
    public string             Expression = "";     // for trace breakpoints
    public bool               TriggerOnExec = true;
    public bool               TriggerOnLoad = false;
    public bool               TriggerOnStore = false;



    public bool HasNonVirtual()
    {
      if ( !IsVirtual )
      {
        return true;
      }
      foreach ( var bp in Virtual )
      {
        if ( !bp.IsVirtual )
        {
          return true;
        }
      }
      return false;
    }



    public bool HasVirtual()
    {
      if ( IsVirtual )
      {
        return true;
      }
      foreach ( var bp in Virtual )
      {
        if ( bp.IsVirtual )
        {
          return true;
        }
      }
      return false;
    }
  }
}
