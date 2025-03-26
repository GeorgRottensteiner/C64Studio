using RetroDevStudio.Formats;
using System.Collections.Generic;



namespace RetroDevStudio.Formats
{
  public class ExportCharsetScreenInfo
  {
    public enum ExportData
    {
      CHAR_THEN_COLOR,
      CHAR_ONLY,
      COLOR_ONLY,
      COLOR_THEN_CHAR,
      CHARSET,
      CHAR_AND_COLOR_INTERLEAVED
    }

    public CharsetScreenProject       Charscreen;
    public GR.Memory.ByteBuffer       ScreenCharData;
    public GR.Memory.ByteBuffer       ScreenColorData;
    public GR.Memory.ByteBuffer       CharsetData;
    public GR.Math.Rectangle          Area;
    public GR.Image.MemoryImage       Image;
    public List<int>                  SelectedCharactersInCharset = new List<int>();
    public ExportData                 Data = ExportData.CHAR_THEN_COLOR;
    public bool                       RowByRow = true;
  }
}
