namespace RetroDevStudio
{
  public partial class ConstantData
  {
    public static Palette PaletteVC20()
    {
      var pal = new Palette();

      // hard coded VC20 colors (nabbed from Forum64 thread)
       pal.ColorValues[0] = 0xff000000;
       pal.ColorValues[1] = 0xffffffff;
       pal.ColorValues[2] = 0xff9e3135;
       pal.ColorValues[3] = 0xff67e7e1;
       pal.ColorValues[4] = 0xffb140c3;
       pal.ColorValues[5] = 0xff62e35b;
       pal.ColorValues[6] = 0xff4637bf;
       pal.ColorValues[7] = 0xffd9e847;
       pal.ColorValues[8] = 0xffbd6d26;
       pal.ColorValues[9] = 0xffe3bb98;
      pal.ColorValues[10] = 0xffdca6a8;
      pal.ColorValues[11] = 0xffb7f7f4;
      pal.ColorValues[12] = 0xffdfa7e8;
      pal.ColorValues[13] = 0xffb4f4b0;
      pal.ColorValues[14] = 0xffa69ee2;
      pal.ColorValues[15] = 0xfff5fcac;

      // selection color
      pal.ColorValues[16] = 0xff80ff80;

      pal.CreateBrushes();

      return pal;
    }



  }
}
