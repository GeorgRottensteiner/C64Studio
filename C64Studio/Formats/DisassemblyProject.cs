using RetroDevStudio;
using System;
using System.Collections.Generic;
using System.Text;

namespace RetroDevStudio.Formats
{
  public class DisassemblyProject
  {
    public GR.Memory.ByteBuffer             Data = new GR.Memory.ByteBuffer( 8 );
    public int                              DataStartAddress = 0;
    public string                           Description = "";
    public GR.Collections.Set<int>          JumpedAtAddresses = new GR.Collections.Set<int>();
    public GR.Collections.Map<int,string>   NamedLabels = new GR.Collections.Map<int,string>();



    public DisassemblyProject()
    {
    }



    public GR.Memory.ByteBuffer SaveToBuffer()
    {
      GR.Memory.ByteBuffer projectFile = new GR.Memory.ByteBuffer();

      GR.IO.FileChunk   chunkProjectInfo = new GR.IO.FileChunk( FileChunkConstants.DISASSEMBLY_INFO );
      chunkProjectInfo.AppendString( Description );
      projectFile.Append( chunkProjectInfo.ToBuffer() );


      GR.IO.FileChunk   chunkProjectData = new GR.IO.FileChunk( FileChunkConstants.DISASSEMBLY_DATA );
      chunkProjectData.AppendI32( DataStartAddress );
      chunkProjectData.AppendU32( Data.Length );
      chunkProjectData.Append( Data );
      projectFile.Append( chunkProjectData.ToBuffer() );

      GR.IO.FileChunk   chunkJumpAddresses = new GR.IO.FileChunk( FileChunkConstants.DISASSEMBLY_JUMP_ADDRESSES );
      chunkJumpAddresses.AppendI32( JumpedAtAddresses.Count );
      foreach ( int jumpAddress in JumpedAtAddresses )
      {
        chunkJumpAddresses.AppendI32( jumpAddress );
      }
      projectFile.Append( chunkJumpAddresses.ToBuffer() );

      GR.IO.FileChunk   chunkNamedLabels = new GR.IO.FileChunk( FileChunkConstants.DISASSEMBLY_NAMED_LABELS );
      chunkNamedLabels.AppendI32( NamedLabels.Count );
      foreach ( var namedLabel in NamedLabels )
      {
        chunkNamedLabels.AppendI32( namedLabel.Key );
        chunkNamedLabels.AppendString( namedLabel.Value );
      }
      projectFile.Append( chunkNamedLabels.ToBuffer() );

      return projectFile;
    }



    public bool ReadFromBuffer( GR.Memory.ByteBuffer DataIn )
    {
      if ( DataIn == null )
      {
        return false;
      }
      Data.Clear();
      JumpedAtAddresses.Clear();
      NamedLabels.Clear();
      Description = "";

      GR.IO.MemoryReader memIn = DataIn.MemoryReader();

      GR.IO.FileChunk   chunk = new GR.IO.FileChunk();

      while ( chunk.ReadFromStream( memIn ) )
      {
        var chunkReader = chunk.MemoryReader();
        switch ( chunk.Type )
        {
          case FileChunkConstants.DISASSEMBLY_INFO:
            Description = chunkReader.ReadString();
            break;
          case FileChunkConstants.DISASSEMBLY_DATA:
            {
              DataStartAddress = chunkReader.ReadInt32();
              uint dataLength = chunkReader.ReadUInt32();

              chunkReader.ReadBlock( Data, dataLength );
            }
            break;
          case FileChunkConstants.DISASSEMBLY_JUMP_ADDRESSES:
            {
              int     numEntries = chunkReader.ReadInt32();

              for ( int i = 0; i < numEntries; ++i )
              {
                int value = chunkReader.ReadInt32();
                JumpedAtAddresses.Add( value );
              }
            }
            break;
          case FileChunkConstants.DISASSEMBLY_NAMED_LABELS:
            {
              int     numEntries = chunkReader.ReadInt32();

              for ( int i = 0; i < numEntries; ++i )
              {
                int address = chunkReader.ReadInt32();
                string name = chunkReader.ReadString();

                NamedLabels[address] = name;
              }
            }
            break;
        }
      }
      return true;
    }

  }
}
