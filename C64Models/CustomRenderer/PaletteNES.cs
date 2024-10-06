namespace RetroDevStudio
{
  public partial class ConstantData
  {
    public static Palette DefaultPaletteNES()
    {
      var pal = new Palette( 64 );

      // hard coded NES colors (NES Classic (FBX).pal)
       pal.ColorValues[0] = 0xff616161;
       pal.ColorValues[1] = 0xff000088;
       pal.ColorValues[2] = 0xff1f0d99;
       pal.ColorValues[3] = 0xff371379;
       pal.ColorValues[4] = 0xff561260;
       pal.ColorValues[5] = 0xff5d0010;
       pal.ColorValues[6] = 0xff520e00;
       pal.ColorValues[7] = 0xff3a2308;
       pal.ColorValues[8] = 0xff21350c;
       pal.ColorValues[9] = 0xff0d410e;
      pal.ColorValues[10] = 0xff174417;
      pal.ColorValues[11] = 0xff003a1f;
      pal.ColorValues[12] = 0xff002f57;
      pal.ColorValues[13] = 0xff000000;
      pal.ColorValues[14] = 0xff000000;
      pal.ColorValues[15] = 0xff000000;

      pal.ColorValues[16] = 0xffaaaaaa;
      pal.ColorValues[17] = 0xff0d4dc4;
      pal.ColorValues[18] = 0xff4b24de;
      pal.ColorValues[19] = 0xff6912cf;
      pal.ColorValues[20] = 0xff9014ad;
      pal.ColorValues[21] = 0xff9d1c48;
      pal.ColorValues[22] = 0xff923404;
      pal.ColorValues[23] = 0xff735005;
      pal.ColorValues[24] = 0xff5d6913;
      pal.ColorValues[25] = 0xff167a11;
      pal.ColorValues[26] = 0xff138008;
      pal.ColorValues[27] = 0xff127649;
      pal.ColorValues[28] = 0xff1c6691;
      pal.ColorValues[29] = 0xff000000;
      pal.ColorValues[30] = 0xff000000;
      pal.ColorValues[31] = 0xff000000;

      pal.ColorValues[32] = 0xfffcfcfc;
      pal.ColorValues[33] = 0xff639afc;
      pal.ColorValues[34] = 0xff8a7efc;
      pal.ColorValues[35] = 0xffb06afc;
      pal.ColorValues[36] = 0xffdd6df2;
      pal.ColorValues[37] = 0xffe771ab;
      pal.ColorValues[38] = 0xffe38658;
      pal.ColorValues[39] = 0xffcc9e22;
      pal.ColorValues[40] = 0xffa8b100;
      pal.ColorValues[41] = 0xff72c100;
      pal.ColorValues[42] = 0xff5acd4e;
      pal.ColorValues[43] = 0xff34c28e;
      pal.ColorValues[44] = 0xff4fbece;
      pal.ColorValues[45] = 0xff424242;
      pal.ColorValues[46] = 0xff000000;
      pal.ColorValues[47] = 0xff000000;

      pal.ColorValues[48] = 0xfffcfcfc;
      pal.ColorValues[49] = 0xffbed4fc;
      pal.ColorValues[50] = 0xffcacafc;
      pal.ColorValues[51] = 0xffd9c4fc;
      pal.ColorValues[52] = 0xffecc1fc;
      pal.ColorValues[53] = 0xfffac3e7;
      pal.ColorValues[54] = 0xfff7cec3;
      pal.ColorValues[55] = 0xffe2cda7;
      pal.ColorValues[56] = 0xffdadb9c;
      pal.ColorValues[57] = 0xffc8e39e;
      pal.ColorValues[58] = 0xffbfe5b8;
      pal.ColorValues[59] = 0xffb2ebc8;
      pal.ColorValues[60] = 0xffb7e5eb;
      pal.ColorValues[61] = 0xffacacac;
      pal.ColorValues[62] = 0xff000000;
      pal.ColorValues[63] = 0xff000000;
      pal.CreateBrushes();

      pal.Name = "Default NES";

      return pal;
    }



  }
}
