using RetroDevStudioModels;
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
      Key           = Keys.None;
      SecondaryKey  = Keys.None;
      Function      = C64Studio.Types.Function.NONE;
    }

    public AcceleratorKey( Keys Key, Types.Function Function )
    {
      this.Key      = Key;
      this.Function = Function;
    }

    public AcceleratorKey( Keys Key, Keys Key2, Types.Function Function )
    {
      this.Key          = Key;
      this.SecondaryKey = Key2;
      this.Function     = Function;
    }

    public Keys Key
    {
      get; 
      set;
    }

    public Keys SecondaryKey
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
      GR.IO.FileChunk chunk = new GR.IO.FileChunk( FileChunkConstants.SETTINGS_ACCELERATOR );

      chunk.AppendU32( (uint)Key );
      chunk.AppendU32( (uint)Function );
      chunk.AppendU32( (uint)SecondaryKey );

      return chunk;
    }



    public bool FromChunk( GR.IO.FileChunk Chunk )
    {
      if ( Chunk.Type != FileChunkConstants.SETTINGS_ACCELERATOR )
      {
        return false;
      }

      GR.IO.IReader reader = Chunk.MemoryReader();

      Key           = (Keys)reader.ReadUInt32();
      Function      = (C64Studio.Types.Function)reader.ReadUInt32();
      SecondaryKey  = (Keys)reader.ReadUInt32();
      return true;
    }

  }
}
