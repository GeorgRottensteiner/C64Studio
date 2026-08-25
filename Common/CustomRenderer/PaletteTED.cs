namespace RetroDevStudio
{
  public partial class ConstantData
  {
    public static GR.Image.Palette DefaultPaletteTED()
    {
      var pal = new GR.Image.Palette( 128 );

      // hard coded TED colors
      pal.ColorValues[0] =  0xff000000;
      pal.ColorValues[1] =  0xff2c2c2c;
      pal.ColorValues[2] =  0xff621307;
      pal.ColorValues[3] =  0xff00424c;
      pal.ColorValues[4] =  0xff510378;
      pal.ColorValues[5] =  0xff004e00;
      pal.ColorValues[6] =  0xff27188e;
      pal.ColorValues[7] =  0xff303e00;
      pal.ColorValues[8] =  0xff582100;
      pal.ColorValues[9] =  0xff463000;
      pal.ColorValues[10] = 0xff244400;
      pal.ColorValues[11] = 0xff630448;
      pal.ColorValues[12] = 0xff004e0c;
      pal.ColorValues[13] = 0xff0e2784;
      pal.ColorValues[14] = 0xff33118e;
      pal.ColorValues[15] = 0xff184800;

      // second 16 colors
      pal.ColorValues[16] = 0xff000000;
      pal.ColorValues[17] = 0xff3b3b3b;
      pal.ColorValues[18] = 0xff702419;
      pal.ColorValues[19] = 0xff00505a;
      pal.ColorValues[20] = 0xff601685;
      pal.ColorValues[21] = 0xff125d00;
      pal.ColorValues[22] = 0xff36289b;
      pal.ColorValues[23] = 0xff3f4c00;
      pal.ColorValues[24] = 0xff663100;
      pal.ColorValues[25] = 0xff553f00;
      pal.ColorValues[26] = 0xff345200;
      pal.ColorValues[27] = 0xff711656;
      pal.ColorValues[28] = 0xff005c1d;
      pal.ColorValues[29] = 0xff1f3691;
      pal.ColorValues[30] = 0xff42229b;
      pal.ColorValues[31] = 0xff285700;

      // 3rd 16 colors
      pal.ColorValues[32] = 0xff000000;
      pal.ColorValues[33] = 0xff424242;
      pal.ColorValues[34] = 0xff772c21;
      pal.ColorValues[35] = 0xff055861;
      pal.ColorValues[36] = 0xff661e8c;
      pal.ColorValues[37] = 0xff1b6400;
      pal.ColorValues[38] = 0xff3e30a2;
      pal.ColorValues[39] = 0xff475400;
      pal.ColorValues[40] = 0xff6d3900;
      pal.ColorValues[41] = 0xff5c4700;
      pal.ColorValues[42] = 0xff3b5900;
      pal.ColorValues[43] = 0xff771f5d;
      pal.ColorValues[44] = 0xff046325;
      pal.ColorValues[45] = 0xff273e98;
      pal.ColorValues[46] = 0xff492aa1;
      pal.ColorValues[47] = 0xff305e00;

      // 4th 16 colors
      pal.ColorValues[48] = 0xff000000;
      pal.ColorValues[49] = 0xff515151;
      pal.ColorValues[50] = 0xff843b31;
      pal.ColorValues[51] = 0xff17656f;
      pal.ColorValues[52] = 0xff742e99;
      pal.ColorValues[53] = 0xff2b7100;
      pal.ColorValues[54] = 0xff4c3faf;
      pal.ColorValues[55] = 0xff556200;
      pal.ColorValues[56] = 0xff7a4709;
      pal.ColorValues[57] = 0xff6a5500;
      pal.ColorValues[58] = 0xff4a6700;
      pal.ColorValues[59] = 0xff852f6b;
      pal.ColorValues[60] = 0xff177135;
      pal.ColorValues[61] = 0xff364ca5;
      pal.ColorValues[62] = 0xff5739ae;
      pal.ColorValues[63] = 0xff3f6b00;

      // 5th 16 colors
      pal.ColorValues[64] = 0xff000000;
      pal.ColorValues[65] = 0xff7a7a7a;
      pal.ColorValues[66] = 0xffac665c;
      pal.ColorValues[67] = 0xff468e97;
      pal.ColorValues[68] = 0xff9c5ac0;
      pal.ColorValues[69] = 0xff57992e;
      pal.ColorValues[70] = 0xff766ad5;
      pal.ColorValues[71] = 0xff7e8a13;
      pal.ColorValues[72] = 0xffa2713a;
      pal.ColorValues[73] = 0xff927e20;
      pal.ColorValues[74] = 0xff748f14;
      pal.ColorValues[75] = 0xffac5a93;
      pal.ColorValues[76] = 0xff459960;
      pal.ColorValues[77] = 0xff6276cb;
      pal.ColorValues[78] = 0xff8064d4;
      pal.ColorValues[79] = 0xff6a9419;

      // 6th 16 colors
      pal.ColorValues[80] = 0xff000000;
      pal.ColorValues[81] = 0xff959595;
      pal.ColorValues[82] = 0xffc58178;
      pal.ColorValues[83] = 0xff62a8b1;
      pal.ColorValues[84] = 0xffb675d9;
      pal.ColorValues[85] = 0xff73b34c;
      pal.ColorValues[86] = 0xff9185ed;
      pal.ColorValues[87] = 0xff99a433;
      pal.ColorValues[88] = 0xffbb8c57;
      pal.ColorValues[89] = 0xffac993e;
      pal.ColorValues[90] = 0xff8faa34;
      pal.ColorValues[91] = 0xffc676ad;
      pal.ColorValues[92] = 0xff62b37b;
      pal.ColorValues[93] = 0xff7d91e4;
      pal.ColorValues[94] = 0xff9b80ed;
      pal.ColorValues[95] = 0xff85ae38;

      // 7th 16 colors
      pal.ColorValues[ 96] = 0xff000000;
      pal.ColorValues[ 97] = 0xffafafaf;
      pal.ColorValues[ 98] = 0xffde9b93;
      pal.ColorValues[ 99] = 0xff7dc2ca;
      pal.ColorValues[100] = 0xffcf90f2;
      pal.ColorValues[101] = 0xff8dcd68;
      pal.ColorValues[102] = 0xffab9fff;
      pal.ColorValues[103] = 0xffb3be51;
      pal.ColorValues[104] = 0xffd5a673;
      pal.ColorValues[105] = 0xffc6b35b;
      pal.ColorValues[106] = 0xffa9c351;
      pal.ColorValues[107] = 0xffdf91c7;
      pal.ColorValues[108] = 0xff7dcc96;
      pal.ColorValues[109] = 0xff97abfd;
      pal.ColorValues[110] = 0xffb59aff;
      pal.ColorValues[111] = 0xff9fc755;

      // 8th 16 colors
      pal.ColorValues[112] = 0xff000000;
      pal.ColorValues[113] = 0xffffffff;
      pal.ColorValues[114] = 0xffffcfc6;
      pal.ColorValues[115] = 0xffb2f4fc;
      pal.ColorValues[116] = 0xffffc4ff;
      pal.ColorValues[117] = 0xffc1fe9d;
      pal.ColorValues[118] = 0xffddd2ff;
      pal.ColorValues[119] = 0xffe5f088;
      pal.ColorValues[120] = 0xffffd9a8;
      pal.ColorValues[121] = 0xfff7e591;
      pal.ColorValues[122] = 0xffdbf588;
      pal.ColorValues[123] = 0xffffc4f9;
      pal.ColorValues[124] = 0xffb1fec9;
      pal.ColorValues[125] = 0xffcbddff;
      pal.ColorValues[126] = 0xffe7cdff;
      pal.ColorValues[127] = 0xffd2f98c;

      pal.CreateBrushes();

      pal.Name = "Default Commodore TED";

      return pal;
    }



  }
}
