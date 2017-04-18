using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio
{
  public class ProjectConfig
  {
    public string               Name = "";
    public string               Defines = "";


    public void Load( GR.IO.IReader Reader )
    {
      Name = Reader.ReadString();
      Defines = Reader.ReadString();
    }
    
    

    public GR.Memory.ByteBuffer Save()
    {
      GR.IO.FileChunk   chunk = new GR.IO.FileChunk( Types.FileChunk.PROJECT_CONFIG );

      chunk.AppendString( Name );
      chunk.AppendString( Defines );
      return chunk.ToBuffer();
    }

  }
}
