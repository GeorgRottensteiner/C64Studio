namespace RetroDevStudio
{
  public partial class ConstantData
  {
    public static Palette PaletteMega65_256()
    {
      var pal = new Palette( 256 );

      // hard coded c64 colors
      pal.ColorValues[0] =  0xff000000;
      pal.ColorValues[1] =  0xfff0f0f0;
      pal.ColorValues[2] =  0xfff00000;
      pal.ColorValues[3] =  0xff00f0f0;
      pal.ColorValues[4] =  0xfff000f0;
      pal.ColorValues[5] =  0xff00f000;
      pal.ColorValues[6] =  0xff0000f0;
      pal.ColorValues[7] =  0xfff0f000;
      pal.ColorValues[8] =  0xfff06000;
      pal.ColorValues[9] =  0xffa04000;
      pal.ColorValues[10] = 0xfff07070;
      pal.ColorValues[11] = 0xff505050;
      pal.ColorValues[12] = 0xff808080;
      pal.ColorValues[13] = 0xff90f090;
      pal.ColorValues[14] = 0xff9090f0;
      pal.ColorValues[15] = 0xffb0b0b0;

      // second 16 colors
      pal.ColorValues[16] = 0xffe00000;
      pal.ColorValues[17] = 0xffF05000;
      pal.ColorValues[18] = 0xffF0B000;
      pal.ColorValues[19] = 0xffE0E000;
      pal.ColorValues[20] = 0xff70F000;
      pal.ColorValues[21] = 0xff60E060;
      pal.ColorValues[22] = 0xff00E030;
      pal.ColorValues[23] = 0xff00F090;
      pal.ColorValues[24] = 0xff00D0D0;
      pal.ColorValues[25] = 0xff0090F0;
      pal.ColorValues[26] = 0xff0030F0;
      pal.ColorValues[27] = 0xff0000E0;
      pal.ColorValues[28] = 0xff7000F0;
      pal.ColorValues[29] = 0xffC000F0;
      pal.ColorValues[30] = 0xffF000B0;
      pal.ColorValues[31] = 0xffF03060;

      // almost random colors? (show up in background)
      pal.ColorValues[32] = 0xff7c7c7c;
      pal.ColorValues[33] = 0xff6c6c6c;
      pal.ColorValues[34] = 0xffa2a2a2;
      pal.ColorValues[35] = 0xff9a9a9a;
      pal.ColorValues[36] = 0xffababab;
      pal.ColorValues[37] = 0xffbcbcbc;
      pal.ColorValues[38] = 0xffcdcdcd;
      pal.ColorValues[39] = 0xffd4d4d4;
      pal.ColorValues[40] = 0xffdbdbdb;
      pal.ColorValues[41] = 0xffe5e5e5;
      pal.ColorValues[42] = 0xffc4c4c4;
      pal.ColorValues[43] = 0xff939393;
      pal.ColorValues[44] = 0xff8c8c8c;
      pal.ColorValues[45] = 0xff3a4665;
      pal.ColorValues[46] = 0xff394b76;
      pal.ColorValues[47] = 0xff3a486f;
      pal.ColorValues[48] = 0xff727272;
      pal.ColorValues[49] = 0xffb3b3b3;
      pal.ColorValues[50] = 0xffffffff;
      pal.ColorValues[51] = 0xff5c5c5c;
      pal.ColorValues[52] = 0xff545454;
      pal.ColorValues[53] = 0xfff4f4f4;
      pal.ColorValues[54] = 0xff385190;
      pal.ColorValues[55] = 0xff347cff;
      pal.ColorValues[56] = 0xff3574ff;
      pal.ColorValues[57] = 0xffececec;
      pal.ColorValues[58] = 0xff3667e7;
      pal.ColorValues[59] = 0xff366fff;
      pal.ColorValues[60] = 0xff366ad9;
      pal.ColorValues[61] = 0xff3764d9;
      pal.ColorValues[62] = 0xff402334;
      pal.ColorValues[63] = 0xff335c39;
      pal.ColorValues[64] = 0xff39443b;
      pal.ColorValues[65] = 0xff411d3b;
      pal.ColorValues[66] = 0xff2a8139;
      pal.ColorValues[67] = 0xff0cff36;
      pal.ColorValues[68] = 0xff12f337;
      pal.ColorValues[69] = 0xff306d3a;
      pal.ColorValues[70] = 0xff18d437;
      pal.ColorValues[71] = 0xff14e937;
      pal.ColorValues[72] = 0xff10fb37;
      pal.ColorValues[73] = 0xff16e037;
      pal.ColorValues[74] = 0xff2f1e3d;
      pal.ColorValues[75] = 0xff4d4d38;
      pal.ColorValues[76] = 0xff454539;
      pal.ColorValues[77] = 0xff1c1b3f;
      pal.ColorValues[78] = 0xff929630;
      pal.ColorValues[79] = 0xfffdff24;
      pal.ColorValues[80] = 0xffeef425;
      pal.ColorValues[81] = 0xfff3f624;
      pal.ColorValues[82] = 0xff696a35;
      pal.ColorValues[83] = 0xff342d35;
      pal.ColorValues[84] = 0xff111040;
      pal.ColorValues[85] = 0xffffff1f;
      pal.ColorValues[86] = 0xff5f6036;
      pal.ColorValues[87] = 0xff1d2140;
      pal.ColorValues[88] = 0xff323a3e;
      pal.ColorValues[89] = 0xff213945; // 89
      pal.ColorValues[90] = 0xff713f27;
      pal.ColorValues[91] = 0xffcc460a;
      pal.ColorValues[92] = 0xffb8440b;
      pal.ColorValues[93] = 0xff673e2b;
      pal.ColorValues[94] = 0xff2a3a41;
      pal.ColorValues[95] = 0xff4f3c34;
      pal.ColorValues[96] = 0xffe84700;
      pal.ColorValues[97] = 0xfffc4900;
      pal.ColorValues[98] = 0xffea4700;
      pal.ColorValues[99] = 0xff193848;
      pal.ColorValues[100] = 0xff89411d;  // 100
      pal.ColorValues[101] = 0xffd64600;
      pal.ColorValues[102] = 0xffd84600;

      for ( int i = 103; i < 256; ++i )
      {
        pal.ColorValues[i] = 0xff000000;
      }

      pal.CreateBrushes();

      pal.Name = "Default Mega65 (256)";

      return pal;
    }



  }
}
