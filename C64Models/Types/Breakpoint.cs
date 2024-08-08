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



    public GR.Memory.ByteBuffer Save()
    {
      GR.IO.FileChunk chunkBreakPoint = new GR.IO.FileChunk( FileChunkConstants.SETTINGS_BREAKPOINT );

      chunkBreakPoint.AppendString( DocumentFilename );
      chunkBreakPoint.AppendI32( Address );
      chunkBreakPoint.AppendI32( LineIndex );
      chunkBreakPoint.AppendString( AddressSource );
      chunkBreakPoint.AppendString( Conditions );
      chunkBreakPoint.AppendString( Expression );
      chunkBreakPoint.AppendU32( (uint)( ( IsVirtual ? 1 : 0 )
                                       | ( TriggerOnExec ? 2 : 0 )
                                       | ( TriggerOnLoad ? 4 : 0 )
                                       | ( TriggerOnStore ? 8 : 0 ) ) );

      return chunkBreakPoint.ToBuffer();
    }



    public void Load( GR.IO.MemoryReader MemIn )
    {
      DocumentFilename  = MemIn.ReadString();
      Address           = MemIn.ReadInt32();
      LineIndex         = MemIn.ReadInt32();
      AddressSource     = MemIn.ReadString();
      Conditions        = MemIn.ReadString();
      Expression        = MemIn.ReadString();

      uint flags = MemIn.ReadUInt32();

      IsVirtual       = ( flags & 1 ) != 0;
      TriggerOnExec   = ( flags & 2 ) != 0;
      TriggerOnLoad   = ( flags & 4 ) != 0;
      TriggerOnStore  = ( flags & 8 ) != 0;
    }



  }
}
