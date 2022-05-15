using RetroDevStudio;
using System;
using System.Collections.Generic;
using System.Text;

namespace RetroDevStudio
{
  public class WatchEntry
  {
    public enum DisplayType
    {
      HEX,
      PETSCII,
      DEZ,
      BINARY
    };

    public string               Name = "";
    public int                  Address = 0;
    public GR.Memory.ByteBuffer CurrentValue = new GR.Memory.ByteBuffer();
    public int                  SizeInBytes = 0;
    public DisplayType          Type = DisplayType.HEX;
    public bool                 DisplayMemory = false;
    public bool                 IndexedX = false;
    public bool                 IndexedY = false;
    public bool                 LiteralValue = false;
    public bool                 BigEndian = true;



    public GR.Memory.ByteBuffer Save()
    {
      GR.IO.FileChunk   chunkWatch = new GR.IO.FileChunk( FileChunkConstants.PROJECT_WATCH_ENTRY );

      chunkWatch.AppendString( Name );
      chunkWatch.AppendI32( SizeInBytes );
      chunkWatch.AppendU32( (uint)Type );
      chunkWatch.AppendU8( (byte)( DisplayMemory ? 1 : 0 ) );
      chunkWatch.AppendU8( (byte)( IndexedX ? 1 : 0 ) );
      chunkWatch.AppendU8( (byte)( IndexedY ? 1 : 0 ) );
      chunkWatch.AppendU8( (byte)( LiteralValue ? 1 : 0 ) );
      chunkWatch.AppendI32( Address );
      chunkWatch.AppendU8( (byte)( BigEndian ? 1 : 0 ) );

      return chunkWatch.ToBuffer();
    }



    public void Load( GR.IO.MemoryReader MemIn )
    {
      Name          = MemIn.ReadString();
      SizeInBytes   = MemIn.ReadInt32();
      Type          = (DisplayType)MemIn.ReadUInt32();
      DisplayMemory = ( MemIn.ReadUInt8() != 0 );
      IndexedX      = ( MemIn.ReadUInt8() != 0 );
      IndexedY      = ( MemIn.ReadUInt8() != 0 );
      LiteralValue  = ( MemIn.ReadUInt8() != 0 );
      Address       = MemIn.ReadInt32();
      BigEndian     = ( MemIn.ReadUInt8() != 0 );
    }
  }
}
