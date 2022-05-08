using C64Studio.Formats;
using GR.Memory;
using System.Collections.Generic;


namespace RetroDevStudio.Formats
{
  public class ExportSpriteInfo
  {
    public SpriteProject      Project;
    public ByteBuffer         ExportData;
    public List<int>          ExportIndices;
  }
}
