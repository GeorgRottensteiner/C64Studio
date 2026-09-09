using GR.Memory;
using RetroDevStudio.Formats;
using System.Collections.Generic;

namespace RetroDevStudio.Formats
{
  public class ExportPathInfo
  {
    public List<GR.Generic.Tupel<string,ByteBuffer>> DataPerPath = new List<GR.Generic.Tupel<string, ByteBuffer>>();
  }
}
