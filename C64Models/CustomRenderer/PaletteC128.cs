namespace RetroDevStudio
{
  public partial class ConstantData
  {
    public static Palette DefaultPaletteC128()
    {
      var pal = new Palette();

      // hard coded C128 colors
       pal.ColorValues[0] = 0xff000000;
       pal.ColorValues[1] = 0xff555555;
       pal.ColorValues[2] = 0xff0000aa;
       pal.ColorValues[3] = 0xff5555ff;
       pal.ColorValues[4] = 0xff00aa00;
       pal.ColorValues[5] = 0xff55ff55;
       pal.ColorValues[6] = 0xff00aaaa;
       pal.ColorValues[7] = 0xffaa0000;
       pal.ColorValues[8] = 0xff55ffff;
       pal.ColorValues[9] = 0xffff5555;
      pal.ColorValues[10] = 0xffaa00aa;
      pal.ColorValues[11] = 0xffff55ff;
      pal.ColorValues[12] = 0xffaa5500;
      pal.ColorValues[13] = 0xffffff55;
      pal.ColorValues[14] = 0xffaaaaaa;
      pal.ColorValues[15] = 0xffffffff;

      pal.CreateBrushes();

      pal.Name = "Default C128";

      return pal;
    }



  }
}
