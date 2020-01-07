using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio
{
  public class ProjectConfig
  {
    public string               Name = "";
    public string               Defines = "";
    public string               DebugStartAddressLabel = "";


    public void Load( GR.IO.IReader Reader )
    {
      Name = Reader.ReadString();
      Defines = Reader.ReadString();
      int unusedValue = Reader.ReadInt32();
      DebugStartAddressLabel = Reader.ReadString();

      if ( string.IsNullOrEmpty( DebugStartAddressLabel ) )
      {
        DebugStartAddressLabel = unusedValue.ToString();
      }
    }
    
    

    public GR.Memory.ByteBuffer Save()
    {
      GR.IO.FileChunk   chunk = new GR.IO.FileChunk( Types.FileChunk.PROJECT_CONFIG );

      chunk.AppendString( Name );
      chunk.AppendString( Defines );
      chunk.AppendI32( 0 );
      chunk.AppendString( DebugStartAddressLabel );
      return chunk.ToBuffer();
    }

  }
}
