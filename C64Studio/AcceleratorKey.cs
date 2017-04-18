using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace C64Studio
{
  public class AcceleratorKey
  {
    public AcceleratorKey()
    {
      Key       = Keys.None;
      Function  = C64Studio.Types.Function.NONE;
    }

    public AcceleratorKey( Keys Key, Types.Function Function )
    {
      this.Key      = Key;
      this.Function = Function;
    }

    public Keys Key
    {
      get; 
      set;
    }

    public Types.Function Function
    {
      get;
      set;
    }

    public GR.IO.FileChunk ToChunk()
    {
      GR.IO.FileChunk chunk = new GR.IO.FileChunk( Types.FileChunk.SETTINGS_ACCELERATOR );

      chunk.AppendU32( (uint)Key );
      chunk.AppendU32( (uint)Function );

      return chunk;
    }

    public bool FromChunk( GR.IO.FileChunk Chunk )
    {
      if ( Chunk.Type != Types.FileChunk.SETTINGS_ACCELERATOR )
      {
        return false;
      }

      GR.IO.IReader reader = Chunk.MemoryReader();

      Key = (Keys)reader.ReadUInt32();
      Function = (C64Studio.Types.Function)reader.ReadUInt32();
      return true;
    }

  }
}
