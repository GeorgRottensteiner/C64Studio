using System.Collections.Generic;



namespace RetroDevStudio.Formats
{
  public class ExportGraphicScreenInfo
  {
    public GraphicScreenProject       Project;
    public GR.Image.MemoryImage       Image;
    public int                        BlockWidth;
    public int                        BlockHeight;
    // sizes of single character for charset
    public int                        CheckBlockWidth;
    public int                        CheckBlockHeight;

    public List<Formats.CharData>     Chars;
    public bool[,]                    ErrornousChars;
  }
}
