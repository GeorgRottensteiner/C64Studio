namespace RetroDevStudio
{
  public partial class ConstantData
  {
    public static Palette PaletteC64()
    {
      var pal = new Palette();

      // hard coded c64 colors
       pal.ColorValues[0] = 0xff000000;
       pal.ColorValues[1] = 0xffffffff;
       pal.ColorValues[2] = 0xff8B4131;
       pal.ColorValues[3] = 0xff7BBDC5;
       pal.ColorValues[4] = 0xff8B41AC;
       pal.ColorValues[5] = 0xff6AAC41;
       pal.ColorValues[6] = 0xff3931A4;
       pal.ColorValues[7] = 0xffD5DE73;
       pal.ColorValues[8] = 0xff945A20;
       pal.ColorValues[9] = 0xff5A4100;
      pal.ColorValues[10] = 0xffBD736A;
      pal.ColorValues[11] = 0xff525252;
      pal.ColorValues[12] = 0xff838383;
      pal.ColorValues[13] = 0xffACEE8B;
      pal.ColorValues[14] = 0xff7B73DE;
      pal.ColorValues[15] = 0xffACACAC;

      // selection color
      pal.ColorValues[16] = 0xff80ff80;

      pal.CreateBrushes();

      return pal;
    }



  }
}
