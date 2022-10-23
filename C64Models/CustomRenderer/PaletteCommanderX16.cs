namespace RetroDevStudio
{
  public partial class ConstantData
  {
    public static Palette PaletteCommanderX16()
    {
      var pal = new Palette( 256 );

      // hard coded c64 colors
      pal.ColorValues[0] =  0xff000000;
      pal.ColorValues[1] =  0xfff0f0f0;
      pal.ColorValues[2] =  0xff800000;
      pal.ColorValues[3] =  0xffa0f0e0;
      pal.ColorValues[4] =  0xffc040c0;
      pal.ColorValues[5] =  0xff00c050;
      pal.ColorValues[6] =  0xff0000a0;
      pal.ColorValues[7] =  0xffe0e070;
      pal.ColorValues[8] =  0xffd08050;
      pal.ColorValues[9] =  0xff604000;
      pal.ColorValues[10] = 0xfff07070;
      pal.ColorValues[11] = 0xff303030;
      pal.ColorValues[12] = 0xff707070;
      pal.ColorValues[13] = 0xffa0f060;
      pal.ColorValues[14] = 0xff0080f0;
      pal.ColorValues[15] = 0xffb0b0b0;

      // gray scale
      pal.ColorValues[16] = 0xff000000;
      pal.ColorValues[17] = 0xff101010;
      pal.ColorValues[18] = 0xff202020;
      pal.ColorValues[19] = 0xff303030;
      pal.ColorValues[20] = 0xff404040;
      pal.ColorValues[21] = 0xff505050;
      pal.ColorValues[22] = 0xff606060;
      pal.ColorValues[23] = 0xff707070;
      pal.ColorValues[24] = 0xff808080;
      pal.ColorValues[25] = 0xff909090;
      pal.ColorValues[26] = 0xffa0a0a0;
      pal.ColorValues[27] = 0xffb0b0b0;
      pal.ColorValues[28] = 0xffc0c0c0;
      pal.ColorValues[29] = 0xffd0d0d0;
      pal.ColorValues[30] = 0xffe0e0e0;
      pal.ColorValues[31] = 0xfff0f0f0;

      pal.ColorValues[32] = 0xff201010;
      pal.ColorValues[33] = 0xff403030;
      pal.ColorValues[34] = 0xff604040;
      pal.ColorValues[35] = 0xff806060;
      pal.ColorValues[36] = 0xffa08080;
      pal.ColorValues[37] = 0xffc09090;
      pal.ColorValues[38] = 0xfff0b0b0;
      pal.ColorValues[39] = 0xff201010;
      pal.ColorValues[40] = 0xff402020;
      pal.ColorValues[41] = 0xff603030;
      pal.ColorValues[42] = 0xff804040;
      pal.ColorValues[43] = 0xffa05050;
      pal.ColorValues[44] = 0xffc06060;
      pal.ColorValues[45] = 0xfff07070;
      pal.ColorValues[46] = 0xff200000;
      pal.ColorValues[47] = 0xff401010;

      pal.ColorValues[48] = 0xff601010;
      pal.ColorValues[49] = 0xff802020;
      pal.ColorValues[50] = 0xffa02020;
      pal.ColorValues[51] = 0xffc03030;
      pal.ColorValues[52] = 0xfff03030;
      pal.ColorValues[53] = 0xff200000;
      pal.ColorValues[54] = 0xff400000;
      pal.ColorValues[55] = 0xff600000;
      pal.ColorValues[56] = 0xff800000;
      pal.ColorValues[57] = 0xffa00000;
      pal.ColorValues[58] = 0xffc00000;
      pal.ColorValues[59] = 0xfff00000;
      pal.ColorValues[60] = 0xff202010;
      pal.ColorValues[61] = 0xff404030;
      pal.ColorValues[62] = 0xff606040;
      pal.ColorValues[63] = 0xff808060;

      pal.ColorValues[64] = 0xffa0a080;
      pal.ColorValues[65] = 0xffc0c090;
      pal.ColorValues[66] = 0xfff0e0b0;
      pal.ColorValues[67] = 0xff201010;
      pal.ColorValues[68] = 0xff403020;
      pal.ColorValues[69] = 0xff605030;
      pal.ColorValues[70] = 0xff807040;
      pal.ColorValues[71] = 0xffa09050;
      pal.ColorValues[72] = 0xffc0b060;
      pal.ColorValues[73] = 0xfff0d070;
      pal.ColorValues[74] = 0xff201000;
      pal.ColorValues[75] = 0xff403010;
      pal.ColorValues[76] = 0xff605010;
      pal.ColorValues[77] = 0xff806020;
      pal.ColorValues[78] = 0xffa08020;
      pal.ColorValues[79] = 0xffc0a030;

      pal.ColorValues[80] = 0xfff0c030;
      pal.ColorValues[81] = 0xff201000;
      pal.ColorValues[82] = 0xff403000;
      pal.ColorValues[83] = 0xff604000;
      pal.ColorValues[84] = 0xff806000;
      pal.ColorValues[85] = 0xffa08000;
      pal.ColorValues[86] = 0xffc09000;
      pal.ColorValues[87] = 0xfff0b000;
      pal.ColorValues[88] = 0xff102010;
      pal.ColorValues[89] = 0xff304030; // 89
      pal.ColorValues[90] = 0xff506040;
      pal.ColorValues[91] = 0xff708060;
      pal.ColorValues[92] = 0xff90a080;
      pal.ColorValues[93] = 0xffb0c090;
      pal.ColorValues[94] = 0xffd0f0b0;
      pal.ColorValues[95] = 0xff102010;

      pal.ColorValues[96] = 0xff304020;
      pal.ColorValues[97] = 0xff406030;
      pal.ColorValues[98] = 0xff608040;
      pal.ColorValues[99] = 0xff80a050;
      pal.ColorValues[100] = 0xff90c060;  // 100
      pal.ColorValues[101] = 0xffb0f070;
      pal.ColorValues[102] = 0xff102000;
      pal.ColorValues[103] = 0xff204010;
      pal.ColorValues[104] = 0xff406010;
      pal.ColorValues[105] = 0xff508020;
      pal.ColorValues[106] = 0xff60a020;
      pal.ColorValues[107] = 0xff80c030;
      pal.ColorValues[108] = 0xff90f030;
      pal.ColorValues[109] = 0xff102000;
      pal.ColorValues[110] = 0xff204000;
      pal.ColorValues[111] = 0xff306000;

      pal.ColorValues[112] = 0xff408000;
      pal.ColorValues[113] = 0xff50a000;
      pal.ColorValues[114] = 0xff60c000;
      pal.ColorValues[115] = 0xff70f000;
      pal.ColorValues[116] = 0xff102010;
      pal.ColorValues[117] = 0xff304030;
      pal.ColorValues[118] = 0xff406050;
      pal.ColorValues[119] = 0xff608060;
      pal.ColorValues[120] = 0xff80a080;
      pal.ColorValues[121] = 0xff90c0a0;
      pal.ColorValues[122] = 0xffb0f0c0;
      pal.ColorValues[123] = 0xff102010;
      pal.ColorValues[124] = 0xff204020;
      pal.ColorValues[125] = 0xff306040;
      pal.ColorValues[126] = 0xff408050;
      pal.ColorValues[127] = 0xff50a060;

      pal.ColorValues[128] = 0xff60c080;
      pal.ColorValues[129] = 0xff70f090;
      pal.ColorValues[130] = 0xff002000;
      pal.ColorValues[131] = 0xff104010;
      pal.ColorValues[132] = 0xff106020;
      pal.ColorValues[133] = 0xff208030;
      pal.ColorValues[134] = 0xff20a040;
      pal.ColorValues[135] = 0xff30c050;
      pal.ColorValues[136] = 0xff30f060;
      pal.ColorValues[137] = 0xff002000;
      pal.ColorValues[138] = 0xff004010;
      pal.ColorValues[139] = 0xff006010;
      pal.ColorValues[140] = 0xff008020;
      pal.ColorValues[141] = 0xff00a020;
      pal.ColorValues[142] = 0xff00c030;
      pal.ColorValues[143] = 0xff00f030;

      pal.ColorValues[144] = 0xff102020;
      pal.ColorValues[145] = 0xff304040;
      pal.ColorValues[146] = 0xff406060;
      pal.ColorValues[147] = 0xff608080;
      pal.ColorValues[148] = 0xff80a0a0;
      pal.ColorValues[149] = 0xff90c0c0;
      pal.ColorValues[150] = 0xffb0f0f0;
      pal.ColorValues[151] = 0xff102020;
      pal.ColorValues[152] = 0xff204040;
      pal.ColorValues[153] = 0xff306060;
      pal.ColorValues[154] = 0xff408080;
      pal.ColorValues[155] = 0xff50a0a0;
      pal.ColorValues[156] = 0xff60c0c0;
      pal.ColorValues[157] = 0xff70f0f0;
      pal.ColorValues[158] = 0xff002020;
      pal.ColorValues[159] = 0xff104040;

      pal.ColorValues[160] = 0xff106060;
      pal.ColorValues[161] = 0xff208080;
      pal.ColorValues[162] = 0xff20a0a0;
      pal.ColorValues[163] = 0xff30c0c0;
      pal.ColorValues[164] = 0xff30f0f0;
      pal.ColorValues[165] = 0xff002020;
      pal.ColorValues[166] = 0xff004040;
      pal.ColorValues[167] = 0xff006060;
      pal.ColorValues[168] = 0xff008080;
      pal.ColorValues[169] = 0xff00a0a0;
      pal.ColorValues[170] = 0xff00c0c0;
      pal.ColorValues[171] = 0xff00f0f0;
      pal.ColorValues[172] = 0xff101020;
      pal.ColorValues[173] = 0xff303040;
      pal.ColorValues[174] = 0xff405060;
      pal.ColorValues[175] = 0xff606080;

      pal.ColorValues[176] = 0xff8080a0;
      pal.ColorValues[177] = 0xff90a0c0;
      pal.ColorValues[178] = 0xffb0c0f0;
      pal.ColorValues[179] = 0xff101020;
      pal.ColorValues[180] = 0xff202040;
      pal.ColorValues[181] = 0xff304060;
      pal.ColorValues[182] = 0xff405080;
      pal.ColorValues[183] = 0xff5060a0;
      pal.ColorValues[184] = 0xff6080c0;
      pal.ColorValues[185] = 0xff7090f0;
      pal.ColorValues[186] = 0xff000020;
      pal.ColorValues[187] = 0xff101040;
      pal.ColorValues[188] = 0xff102060;
      pal.ColorValues[189] = 0xff203080;
      pal.ColorValues[190] = 0xff2040a0;
      pal.ColorValues[191] = 0xff3050c0;

      pal.ColorValues[192] = 0xff3060f0;
      pal.ColorValues[193] = 0xff000020;
      pal.ColorValues[194] = 0xff001040;
      pal.ColorValues[195] = 0xff001060;
      pal.ColorValues[196] = 0xff002080;
      pal.ColorValues[197] = 0xff0020a0;
      pal.ColorValues[198] = 0xff0030c0;
      pal.ColorValues[199] = 0xff0030f0;
      pal.ColorValues[200] = 0xff101020;
      pal.ColorValues[201] = 0xff303040;
      pal.ColorValues[202] = 0xff504060;
      pal.ColorValues[203] = 0xff706080;
      pal.ColorValues[204] = 0xff9080a0;
      pal.ColorValues[205] = 0xffb090c0;
      pal.ColorValues[206] = 0xffd0b0f0;
      pal.ColorValues[207] = 0xff101020;

      pal.ColorValues[208] = 0xff302040;
      pal.ColorValues[209] = 0xff403060;
      pal.ColorValues[210] = 0xff604080;
      pal.ColorValues[211] = 0xff8050a0;
      pal.ColorValues[212] = 0xff9060c0;
      pal.ColorValues[213] = 0xffb070f0;
      pal.ColorValues[214] = 0xff100020;
      pal.ColorValues[215] = 0xff201040;
      pal.ColorValues[216] = 0xff401060;
      pal.ColorValues[217] = 0xff502080;
      pal.ColorValues[218] = 0xff6020a0;
      pal.ColorValues[219] = 0xff8030c0;
      pal.ColorValues[220] = 0xff9030f0;
      pal.ColorValues[221] = 0xff100020;
      pal.ColorValues[222] = 0xff200040;
      pal.ColorValues[223] = 0xff300060;

      pal.ColorValues[224] = 0xff400080;
      pal.ColorValues[225] = 0xff5000a0;
      pal.ColorValues[226] = 0xff6000c0;
      pal.ColorValues[227] = 0xff7000f0;
      pal.ColorValues[228] = 0xff201020;
      pal.ColorValues[229] = 0xff403040;
      pal.ColorValues[230] = 0xff604060;
      pal.ColorValues[231] = 0xff806080;
      pal.ColorValues[232] = 0xffa080a0;
      pal.ColorValues[233] = 0xffc090c0;
      pal.ColorValues[234] = 0xfff0b0e0;
      pal.ColorValues[235] = 0xff201010;
      pal.ColorValues[236] = 0xff402030;
      pal.ColorValues[237] = 0xff603050;
      pal.ColorValues[238] = 0xff804070;
      pal.ColorValues[239] = 0xffa05090;

      pal.ColorValues[240] = 0xffc060b0;
      pal.ColorValues[241] = 0xfff070d0;
      pal.ColorValues[242] = 0xff200010;
      pal.ColorValues[243] = 0xff401030;
      pal.ColorValues[244] = 0xff601050;
      pal.ColorValues[245] = 0xff802060;
      pal.ColorValues[246] = 0xffa02080;
      pal.ColorValues[247] = 0xffc030a0;
      pal.ColorValues[248] = 0xfff030c0;
      pal.ColorValues[249] = 0xff200010;
      pal.ColorValues[250] = 0xff400030;
      pal.ColorValues[251] = 0xff600040;
      pal.ColorValues[252] = 0xff800060;
      pal.ColorValues[253] = 0xffa00080;
      pal.ColorValues[254] = 0xffc00090;
      pal.ColorValues[255] = 0xfff000b0;

      pal.CreateBrushes();

      pal.Name = "Default Commander X16 (256)";

      return pal;
    }



  }
}
