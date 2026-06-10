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
    public List<GR.Memory.ByteBuffer> ScreenCharData = new List<GR.Memory.ByteBuffer>();
    public List<GR.Memory.ByteBuffer> ScreenColorData = new List<GR.Memory.ByteBuffer>();
    public GR.Memory.ByteBuffer       CombinedCharData;
    public GR.Memory.ByteBuffer       CombinedColorData;
    public GR.Memory.ByteBuffer       CharsetData;
    public GR.Math.Rectangle          Area;
    public List<int>                  SelectedCharactersInCharset = new List<int>();
    public ExportData                 Data = ExportData.CHAR_THEN_COLOR;
    public bool                       RowByRow = true;
    public List<int>                  ScreensToExport = new List<int>();
  }
}
