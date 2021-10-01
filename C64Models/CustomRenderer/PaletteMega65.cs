namespace RetroDevStudio
{
  public partial class ConstantData
  {
    public static Palette PaletteMega65()
    {
      var pal = new Palette( 256 );

      for ( int i = 0; i < 16; ++i )
      {
        // hard coded c64 colors
        pal.ColorValues[i * 16 + 0] = 0xff000000;
        pal.ColorValues[i * 16 + 1] = 0xffffffff;
        pal.ColorValues[i * 16 + 2] = 0xff8B4131;
        pal.ColorValues[i * 16 + 3] = 0xff7BBDC5;
        pal.ColorValues[i * 16 + 4] = 0xff8B41AC;
        pal.ColorValues[i * 16 + 5] = 0xff6AAC41;
        pal.ColorValues[i * 16 + 6] = 0xff3931A4;
        pal.ColorValues[i * 16 + 7] = 0xffD5DE73;
        pal.ColorValues[i * 16 + 8] = 0xff945A20;
        pal.ColorValues[i * 16 + 9] = 0xff5A4100;
        pal.ColorValues[i * 16 + 10] = 0xffBD736A;
        pal.ColorValues[i * 16 + 11] = 0xff525252;
        pal.ColorValues[i * 16 + 12] = 0xff838383;
        pal.ColorValues[i * 16 + 13] = 0xffACEE8B;
        pal.ColorValues[i * 16 + 14] = 0xff7B73DE;
        pal.ColorValues[i * 16 + 15] = 0xffACACAC;
      }

      pal.CreateBrushes();

      return pal;
    }



  }
}
