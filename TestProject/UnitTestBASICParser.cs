using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
  [TestClass]
  public class UnitTestBASICParser
  {
    private C64Studio.Parser.BasicFileParser CreateParser( C64Studio.Parser.BasicFileParser.BasicVersion Version )
    {
      var parser= new C64Studio.Parser.BasicFileParser( new C64Studio.Parser.BasicFileParser.ParserSettings() );

      parser.SetBasicVersion( Version );

      return parser;
    }



    private GR.Memory.ByteBuffer TestCompile( string Source, C64Studio.Parser.BasicFileParser.BasicVersion Version )
    {
      var parser = CreateParser( Version );

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = C64Studio.Types.CompileTargetType.PRG;

      bool parseResult = parser.Parse( Source, null, config, null );
      if ( !parseResult )
      {
        Debug.Log( "Testassemble failed:" );
        foreach ( var msg in parser.Messages.Values )
        {
          Debug.Log( msg.Message );
        }
      }
      Assert.IsTrue( parser.Assemble( config ) );

      return parser.AssembledOutput.Assembly;
    }



    [TestMethod]
    public void TestRenumberWithSpaces()
    {
      string      source = @"20 IFA=1THEN 20";

      var parser = CreateParser( C64Studio.Parser.BasicFileParser.BasicVersion.C64_BASIC_V2 );

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = C64Studio.Types.CompileTargetType.PRG;
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;
      Assert.IsTrue( parser.Parse( source, null, config, null ) );

      string result = parser.Renumber( 10, 3, 0, 64000 );

      Assert.AreEqual( "10IFA=1THEN10", result );
    }



    [TestMethod]
    public void TestRenumberWithSpacesNoStripSpaces()
    {
      string      source = @"20 IFA=1THEN 20";

      var parser = CreateParser( C64Studio.Parser.BasicFileParser.BasicVersion.C64_BASIC_V2 );
      parser.Settings.StripSpaces = false;

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = C64Studio.Types.CompileTargetType.PRG;
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;
      Assert.IsTrue( parser.Parse( source, null, config, null ) );

      string result = parser.Renumber( 10, 3, 0, 64000 );

      Assert.AreEqual( "10IFA=1THEN 10", result );
    }



    [TestMethod]
    public void TestRenumberOverLines()
    {
      string      source = @"10 GOTO 300
                          300 GOTO 10";

      var parser = CreateParser( C64Studio.Parser.BasicFileParser.BasicVersion.C64_BASIC_V2 );

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = C64Studio.Types.CompileTargetType.PRG;
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;
      Assert.IsTrue( parser.Parse( source, null, config, null ) );

      string result = parser.Renumber( 10, 3, 0, 64000 );

      Assert.AreEqual( @"10GOTO13
13GOTO10", result );
    }



    [TestMethod]
    public void TestRenumberOnGosub()
    {
      string      source = @"10 ONXGOSUB100,400,700
                          100PRINTA
                          400 PRINTB
                          700 PRINTC";

      var parser = CreateParser( C64Studio.Parser.BasicFileParser.BasicVersion.C64_BASIC_V2 );

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = C64Studio.Types.CompileTargetType.PRG;
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;
      Assert.IsTrue( parser.Parse( source, null, config, null ) );

      string result = parser.Renumber( 10, 3, 0, 64000 );

      Assert.AreEqual( @"10ONXGOSUB13,16,19
13PRINTA
16PRINTB
19PRINTC", result );
    }



    [TestMethod]
    public void TestRenumberWithStatementAfterGotoGosub()
    {
      string      source = @"10 ONXGOSUB100,400,700:GOTO2000
                          100PRINTA
                          400 PRINTB
                          700 PRINTC
                          2000 PRINTD";

      var parser = CreateParser( C64Studio.Parser.BasicFileParser.BasicVersion.C64_BASIC_V2 );

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = C64Studio.Types.CompileTargetType.PRG;
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;
      Assert.IsTrue( parser.Parse( source, null, config, null ) );

      string result = parser.Renumber( 10, 3, 0, 64000 );

      Assert.AreEqual( @"10ONXGOSUB13,16,19:GOTO22
13PRINTA
16PRINTB
19PRINTC
22PRINTD", result );
    }



    [TestMethod]
    public void TestEncodeToLabels()
    {
      string      source = @"10 PRINT ""HALLO""
                          20 GOTO 10";

      var parser = CreateParser( C64Studio.Parser.BasicFileParser.BasicVersion.C64_BASIC_V2 );

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = C64Studio.Types.CompileTargetType.PRG;
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null ) );

      string  encoded = parser.EncodeToLabels();
      Assert.AreEqual( @"
LABEL10
PRINT""HALLO""
GOTOLABEL10
", encoded );
    }



    [TestMethod]
    public void TestEncodeToLabelsNoStripSpaces()
    {
      string      source = @"10 PRINT ""HALLO""
                          20 GOTO 10";

      var parser = CreateParser( C64Studio.Parser.BasicFileParser.BasicVersion.C64_BASIC_V2 );

      parser.Settings.StripSpaces = false;

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = C64Studio.Types.CompileTargetType.PRG;
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null ) );

      string  encoded = parser.EncodeToLabels();
      Assert.AreEqual( @"
LABEL10
PRINT ""HALLO""
GOTO LABEL10
", encoded );
    }



    [TestMethod]
    public void TestLaserBASICNoTokenAfterTask()
    {
      string    source = @"5 REM Test windowb label name tokenisation
                          10 REM Demonstrate multi-tasking and sound in a task
                          20 SIDCLR
                          30 ALLOCATE 100,0 'memory for task variables
                          50 TASK1,windowb:PRINT""A line of text""
                          60 PROCw   'wait a while...
                          70 PRINT ""Another line of text!""
                          80 GOTO 60 'wait forever!
                          90 STOP
                          100 '
                          110 LABELwindowb
                          120 REPEAT
                          130   FOR I=0TO15
                          140     TPAPER I
                          150   VOLUME 15
                          160   FRQ 1,3000
                          170  ADSR 1,0,12,5,12
                          180  NOISE 1
                          190  PASS 0
                          200  CUTOFF 120
                          210  RESONANCE 12
                          220     MUSIC 1,20
                          230     FOR D = 1 to 750 : NEXT D 'wait a bit longer
                          240   NEXT I
                          250 UNTIL FALSE
                          260 '
                          270 LABELw: ti$= ""000000"":REPEAT: UNTILti > 50:PROCEND
                          280 '";

      var result = TestCompile( source, C64Studio.Parser.BasicFileParser.BasicVersion.LASER_BASIC );

      Assert.AreEqual( "01082C0805008F20544553542057494E444F5742204C4142454C204E414D4520544F4B454E49534154494F4E0060080A008F2044454D4F4E535452415445204D554C54492D5441534B494E4720414E4420534F554E4420494E2041205441534B006708140002250082081E00F13130302C30274D454D4F525981E5F64941424C4553009F083200E5312C0122423A992241204C494E45204F4620544558542200AD083C00F957279241E92E2E2E00CA0846009922414E4F54484552204C494E45204F462054455854212200DA085000893630279281455645522100E0085A009000E60864002700EF086E00EE01224200F5087800E700010982008149B230A431350009098C000127490012099600021E3135001F09A0000213312C33303030003109AA00021B312C302C31322C352C3132003909B400021431004109BE00022130004B09C800021F313230005409D20002203132005F09DC00021A312C3230007B09E6008144B231A43735303A82442792414249544C4F4E474552008209F0008249008909FA00E8D3008F0904012700AE090E01EE573A544924B222303030303030223AE73AE85449B135303AF800B409180127000000", result.ToString() );
    }



    [TestMethod]
    public void TestBASICV2NoTokenAfterTask()
    {
      string    source = @"66 IFINT(B)<>BTHEN66";

      var result = TestCompile( source, C64Studio.Parser.BasicFileParser.BasicVersion.C64_BASIC_V2 );

      Assert.AreEqual( "0108110842008BB5284229B3B142A73636000000", result.ToString() );
    }



    [TestMethod]
    public void TestLaserBASICKeywords()
    {
      string  source=@"1 END
                      2 FOR
                      3 NEXT
                      4 DATA
                      5 INPUT#
                      6 INPUT
                      7 DIM
                      8 READ
                      9 LET
                      10 GOTO
                      11 RUN
                      12 IF
                      13 RESTORE
                      14 GOSUB
                      15 RETURN
                      16 REM
                      17 STOP
                      18 ON
                      19 WAIT
                      20 LOAD
                      21 SAVE
                      22 VERIFY
                      23 DEF FN
                      24 POKE
                      25 PRINT#
                      26 PRINT
                      27 CONT
                      28 LIST
                      29 CLR
                      30 CMD
                      31 SYS
                      32 OPEN
                      33 CLOSE
                      34 GET
                      35 NEW
                      36 TAB(
                      37 TO
                      38 FN
                      39 SPC(
                      40 THEN
                      41 NOT
                      42 STEP
                      43 +
                      44 -
                      45 *
                      46 /
                      47 ^
                      48 AND
                      49 OR
                      50 >
                      51 =
                      52 <
                      53 SGN
                      54 INT
                      55 ABS
                      56 USR
                      57 FRE
                      58 POS
                      59 SQR
                      60 RND
                      61 LOG
                      62 EXP
                      63 COS
                      64 SIN
                      65 TAN
                      66 ATN
                      67 PEEK
                      68 LEN
                      69 STR$
                      70 VAL
                      71 ASC
                      72 CHR$
                      73 LEFT$
                      74 RIGHT$
                      75 MID$
                      77 ELSE
                      78 HEX$
                      79 DEEK
                      80 TRUE
                      81 IMPORT
                      82 CFN
                      83 SIZE
                      84 FALSE
                      85 SFRE
                      86 LPX
                      87 LPY
                      88 COMMON%
                      89 CROW
                      90 CCOL
                      91 ATR
                      92 INC
                      93 NUM
                      94 ROW2
                      95 COL2
                      96 SPN2
                      97 HGT
                      98 WID
                      99 ROW
                      100 COL
                      101 SPN
                      102 TASK
                      103 HALT
                      104 REPEAT
                      105 UNTIL
                      106 WHILE
                      107 WEND
                      108 CIF
                      109 CELSE
                      110 CEND
                      111 LABEL
                      112 DOKE
                      113 EXIT
                      114 ALLOCATE
                      115 DISABLE
                      116 PULL
                      117 DLOAD
                      118 DSAVE
                      119 VAR
                      120 LOCAL
                      121 PROCEND
                      122 PROC
                      123 CASEND
                      124 OF
                      125 CASE
                      126 RPT
                      127 SETATR
                      128 PI
                      130 SCLR
                      131 SPRITE
                      132 WIPE
                      133 RESET
                      134 H38COL
                      135 LORES
                      136 HIRES
                      137 PLOT
                      138 BOX
                      139 POLY
                      140 DRAW
                      141 MODE
                      142 S2COL
                      143 S4COL
                      144 H40COL
                      145 SCRX
                      146 WRR1
                      147 WRL1
                      148 SCR1
                      149 SCL1
                      150 WRR2
                      151 WRL2
                      152 SCR2
                      153 SCL2
                      154 WRR8
                      155 WRL8
                      156 SCR8
                      157 SCL8
                      158 ATTR
                      159 ATTL
                      160 ATTUP
                      161 ATTDN
                      162 CHAR
                      163 WINDOW
                      164 MULTI
                      165 MONO
                      166 TBORDER
                      167 HBORDER
                      168 TPAPER
                      169 HPAPER
                      170 WRAP
                      171 SCROLL
                      172 INK
                      173 SETA
                      174 ATTGET
                      175 ATT2ON
                      176 ATTON
                      177 ATTOFF
                      178 MIR
                      179 MAR
                      180 WCLR
                      181 INV
                      182 SPIN
                      183 MOVBLK
                      184 MOVXOR
                      185 MOVAND
                      186 MOVOR
                      187 MOVATT
                      188 EXX
                      189 EXY
                      190 GETBLK
                      191 PUTBLK
                      192 CPYBLK
                      193 GETXOR
                      194 PUTXOR
                      195 CPYXOR
                      196 GETOR
                      197 PUTOR
                      198 CPYOR
                      199 GETAND
                      200 PUTAND
                      201 CPYAND
                      202 DBLANK
                      203 DSHOW
                      204 PUTCHR
                      205 LCASE
                      206 UCASE
                      207 CONV
                      208 HON
                      209 HOFF
                      210 HSET
                      211 FLIPA
                      212 H4COL
                      213 H2COL
                      214 H1COL
                      215 H3COL
                      216 HEXX
                      217 HSHX
                      218 HEXY
                      219 HSHY
                      220 HX
                      221 HY
                      222 HCOL
                      223 OVER
                      224 UNDER
                      225 SWAPATT
                      226 DTCTON
                      227 DTCTOFF
                      228 BLK%BLK
                      229 OR%BLK
                      230 AND%BLK
                      231 XOR%BLK
                      232 BLK%OR
                      233 OR%OR
                      234 AND%OR
                      235 XOR%OR
                      236 BLK%AND
                      237 OR%AND
                      238 AND%AND
                      239 XOR%AND
                      240 BLK%XOR
                      241 OR%XOR
                      242 AND%XOR
                      243 XOR%XOR
                      244 TEXT
                      245 FLIP
                      246 HIT
                      247 SCAN
                      248 POINT
                      249 DFA
                      250 AFA2
                      251 AFA
                      252 KB
                      253 FIRE1
                      254 FIRE2
                      255 JS1
                      256 JS2
                      258 BLACK
                      259 WHITE
                      260 RED
                      261 CYAN
                      262 PURPLE
                      263 GREEN
                      264 BLUE
                      265 YELLOW
                      266 ORANGE
                      267 BROWN
                      268 .RED
                      269 GRAY1
                      270 GRAY2
                      271 .GREEN
                      272 .BLUE
                      273 GRAY3
                      274 OSC
                      275 ENV
                      276 FRQ
                      277 NOISE
                      278 PULSE
                      279 SAW
                      280 TRI
                      281 RING
                      282 SYNC
                      283 MUSIC
                      284 ADSR
                      286 MUTE
                      287 VOLUME
                      288 CUTOFF
                      289 RESONANCE
                      290 PASS
                      291 SCRY
                      292 RECALL
                      293 STORE
                      294 SIDCLR
                      295 MERGE
                      296 RESEQ
                      297 RESERVE
                      298 OLD
                      299 DIR
                      300 DSTORE
                      301 DRECALL
                      302 DMERGE
                      303 AUTO
                      304 RENUM
                      305 CSPRITE
                      306 CPUT
                      307 CGET
                      308 CSWAP
                      309 FILL
                      310 RASTER
                      311 EBACK
                      312 BG0
                      313 BG1
                      314 BG2
                      315 BG3
                      316 SWITCH
                      317 NORM
                      318 KEYOFF
                      319 KEYON
                      320 MCOL1
                      321 MCOL2
                      322 MCOL3
                      323 FGND
                      324 BGND
                      325 EI
                      326 DI
                      327 UNSYNC
                      328 RSYNC
                      329 INIT
                      330 MOVE
                      331 PLAY
                      332 RPLAY
                      333 TRACK";

      var result = TestCompile( source, C64Studio.Parser.BasicFileParser.BasicVersion.LASER_BASIC );

      Assert.AreEqual( "01080708010080000D08020081001308030082001908040083001F08050084002508060085002B08070086003108080087003708090088003D080A00890043080B008A0049080C008B004F080D008C0055080E008D005B080F008E00610810008F006708110090006D08120091007308130092007908140093007F08150094008508160095008C08170096A5009208180097009808190098009E081A009900A4081B009A00AA081C009B00B0081D009C00B6081E009D00BC081F009E00C20820009F00C8082100A000CE082200A100D4082300A200DA082400A300E0082500A400E6082600A500EC082700A600F2082800A700F8082900A800FE082A00A90004092B00AA000A092C00AB0010092D00AC0016092E00AD001C092F00AE0022093000AF0028093100B0002E093200B10034093300B2003A093400B30040093500B40046093600B5004C093700B60052093800B70058093900B8005E093A00B90064093B00BA006A093C00BB0070093D00BC0076093E00BD007C093F00BE0082094000BF0088094100C0008E094200C10094094300C2009A094400C300A0094500C400A6094600C500AC094700C600B2094800C700B8094900C800BE094A00C900C4094B00CA00CA094D00CC00D0094E00CD00D6094F00CE00DC095000CF00E2095100D000E8095200D100EE095300D200F4095400D300FA095500D400000A5600D500060A5700D6000C0A5800D700120A5900D800180A5A00D9001E0A5B00DA00240A5C00DB002A0A5D00DC00300A5E00DD00360A5F00DE003C0A6000DF00420A6100E000480A6200E1004E0A6300E200540A6400E3005A0A6500E400600A6600E500660A6700E6006C0A6800E700720A6900E800780A6A00E9007E0A6B00EA00840A6C00EB008A0A6D00EC00900A6E00ED00960A6F00EE009C0A7000EF00A20A7100F000A80A7200F100AE0A7300F200B40A7400F300BA0A7500F400C00A7600F500C60A7700F600CC0A7800F700D20A7900F800D80A7A00F900DE0A7B00FA00E40A7C00FB00EA0A7D00FC00F00A7E00FD00F60A7F00FE00FC0A8000FF00030B82000101000A0B8300010200110B8400010300180B85000104001F0B8600010500260B87000106002D0B8800010700340B89000108003B0B8A00010900420B8B00010A00490B8C00010B00500B8D00010C00570B8E00010D005E0B8F00010E00650B9000010F006C0B9100011000730B92000111007A0B9300011200810B9400011300880B95000114008F0B9600011500960B97000116009D0B9800011700A40B9900011800AB0B9A00011900B20B9B00011A00B90B9C00011B00C00B9D00011C00C70B9E00011D00CE0B9F00011E00D50BA000011F00DC0BA100012000E30BA200012100EA0BA300012200F10BA400012300F80BA500012400FF0BA600012500060CA7000126000D0CA800012700140CA9000128001B0CAA00012900220CAB00012A00290CAC00012B00300CAD00012C00370CAE00012D003E0CAF00012E00450CB000012F004C0CB100013000530CB2000131005A0CB300013200610CB400013300680CB5000134006F0CB600013500760CB7000136007D0CB800013700840CB9000138008B0CBA00013900920CBB00013A00990CBC00013B00A00CBD00013C00A90CBE00A1424C4B00B00CBF00013E00B70CC000013F00BF0CC100A158B000C60CC200014100CD0CC300014200D40CC400A1B000DB0CC500014400E20CC600014500E90CC700A1AF00F00CC800014700F70CC900014800FE0CCA00014900050DCB00014A000C0DCC00014B00130DCD00014C001A0DCE00014D00210DCF00014E00280DD000014F002F0DD100015000360DD2000151003D0DD300015200440DD4000153004B0DD500015400520DD600015500590DD700015600600DD800015700670DD9000158006E0DDA00015900750DDB00015A007C0DDC00015B00830DDD00015C008A0DDE00015D00910DDF00015E00980DE000015F009F0DE100016000A60DE200016100AD0DE300016200B40DE400016300BE0DE500B025424C4B00C80DE600AF25424C4B00CF0DE700016600D60DE800016700DE0DE900B025B000E60DEA00AF25B000ED0DEB00016A00F40DEC00016B00FC0DED00B025AF00040EEE00AF25AF000B0EEF00016E00120EF000016F001B0EF100B02558B000240EF200AF2558B0002B0EF300017200320EF400017300390EF500017400400EF600017500470EF7000176004E0EF800017700550EF9000178005C0EFA00017900630EFB00017A006A0EFC00017B00710EFD00017C00780EFE00017D007F0EFF00017E00860E0001017F008D0E0201020100940E03010202009B0E0401020300A20E0501020400A90E0601020500B00E0701020600B70E0801020700BE0E0901020800C80E0A01B0414E474500CF0E0B01020A00D60E0C01020B00DD0E0D01020C00E40E0E01020D00EB0E0F01020E00F20E1001020F00F90E1101021000000F1201021100070F13010212000E0F1401021300150F15010214001C0F1601021500230F17010216002A0F1801021700310F1901021800380F1A010219003F0F1B01021A00460F1C01021B004D0F1E01021D00540F1F01021E005B0F2001021F00620F2101022000690F2201022100700F2301022200770F24010223007E0F2501022400850F26010225008C0F2701022600930F28010227009A0F2901022800A10F2A01022900A80F2B01022A00AF0F2C01022B00B60F2D01022C00BD0F2E01022D00C40F2F01023200CB0F3001023300D20F3101023500D90F3201023600E00F3301023700E70F3401023800EE0F3501023900F50F3601023A00FC0F3701023B0003103801023C000A103901023D0011103A01023E0018103B01023F001F103C0102400026103D010241002D103E0102420034103F010243003B104001024400421041010245004910420102460050104301024700571044010248005E10450102490065104601024A006C104701024B0073104801024C007A104901024D0081104A01022F0088104B010230008F104C0102310096104D01022E000000", result.ToString() );
    }



    [TestMethod]
    public void TestLaserBASICKeywordsAbbreviations()
    {
      string source = @"1 E. : REM END
                      2 F. : REM FOR
                      3 N. : REM NEXT
                      4 D. : REM DATA
                      5 I. : REM INPUT#
                      7 DI. : REM DIM
                      8 R. : REM READ
                      9 L. : REM LET
                      10 G. : REM GOTO
                      11 RU. : REM RUN
                      13 RES. : REM RESTORE
                      14 GOS. : REM GOSUB
                      15 RET. : REM RETURN
                      17 S. : REM STOP
                      18 O. : REM ON
                      20 LO. : REM LOAD
                      21 SA. : REM SAVE
                      22 V. : REM VERIFY
                      23 DE. : REM DEF FN
                      24 P. : REM POKE
                      25 PR. : REM PRINT#
                      26 ? : REM PRINT
                      27 C. : REM CONT
                      28 LI. : REM LIST
                      29 CL. : REM CLR
                      30 CM. : REM CMD
                      31 SY. : REM SYS
                      32 OP. : REM OPEN
                      33 CLO. : REM CLOSE
                      34 GE. : REM GET
                      36 T. : REM TAB(
                      39 SP. : REM SPC(
                      40 TH. : REM THEN
                      41 NO. : REM NOT
                      42 S. : REM STEP
                      48 A. : REM AND
                      53 SG. : REM SGN
                      55 AB. : REM ABS
                      56 U. : REM USR
                      57 FR. : REM FRE
                      59 SQ. : REM SQR
                      60 RN. : REM RND
                      62 EX. : REM EXP
                      64 SI. : REM SIN
                      66 AT. : REM ATN
                      67 PE. : REM PEEK
                      69 STR. : REM STR$
                      70 VA. : REM VAL
                      71 AS. : REM ASC
                      72 CH. : REM CHR$
                      73 LEF. : REM LEFT$
                      74 RI. : REM RIGHT$
                      75 M. : REM MID$
                      77 EL. : REM ELSE
                      78 H. : REM HEX$
                      79 DEE. : REM DEEK
                      80 TR. : REM TRUE
                      81 IM. : REM IMPORT
                      82 CF. : REM CFN
                      83 SIZ. : REM SIZE
                      84 FA. : REM FALSE
                      85 SF. : REM SFRE
                      86 LP. : REM LPX
                      88 COM. : REM COMMON%
                      89 CR. : REM CROW
                      90 CC. : REM CCOL
                      93 NU. : REM NUM
                      94 RO. : REM ROW2
                      95 COL. : REM COL2
                      96 SPN. : REM SPN2
                      97 HG. : REM HGT
                      98 WI. : REM WID
                      102 TAS. : REM TASK
                      103 HA. : REM HALT
                      104 REP. : REM REPEAT
                      105 UN. : REM UNTIL
                      106 WH. : REM WHILE
                      107 WE. : REM WEND
                      108 CI. : REM CIF
                      109 CE. : REM CELSE
                      110 CEN. : REM CEND
                      111 LA. : REM LABEL
                      112 DO. : REM DOKE
                      113 EXI. : REM EXIT
                      114 AL. : REM ALLOCATE
                      115 DIS. : REM DISABLE
                      116 PU. : REM PULL
                      117 DL. : REM DLOAD
                      118 DS. : REM DSAVE
                      120 LOC. : REM LOCAL
                      121 PRO. : REM PROCEND
                      123 CA. : REM CASEND
                      126 RP. : REM RPT
                      127 SE. : REM SETATR
                      130 SC. : REM SCLR
                      131 SPR. : REM SPRITE
                      132 WIP. : REM WIPE
                      133 RESE. : REM RESET
                      134 H3. : REM H38COL
                      135 LOR. : REM LORES
                      136 HI. : REM HIRES
                      137 PL. : REM PLOT
                      138 B. : REM BOX
                      139 POL. : REM POLY
                      140 DR. : REM DRAW
                      142 S2. : REM S2COL
                      143 S4. : REM S4COL
                      144 H4. : REM H40COL
                      145 SCR. : REM SCRX
                      146 WR. : REM WRR1
                      147 WRL. : REM WRL1
                      158 ATT. : REM ATTR
                      160 ATTU. : REM ATTUP
                      161 ATTD. : REM ATTDN
                      162 CHA. : REM CHAR
                      163 WIN. : REM WINDOW
                      164 MU. : REM MULTI
                      165 MON. : REM MONO
                      166 TB. : REM TBORDER
                      167 HB. : REM HBORDER
                      168 TP. : REM TPAPER
                      169 HP. : REM HPAPER
                      170 WRA. : REM WRAP
                      171 SCRO. : REM SCROLL
                      174 ATTG. : REM ATTGET
                      175 ATT2. : REM ATT2ON
                      176 ATTO. : REM ATTON
                      177 ATTOF. : REM ATTOFF
                      179 MA. : REM MAR
                      180 WC. : REM WCLR
                      182 SPI. : REM SPIN
                      183 MOV. : REM MOVBLK
                      184 MOVX. : REM MOVXOR
                      185 MOVA. : REM MOVAND
                      186 MOVO. : REM MOVOR
                      187 MOVAT. : REM MOVATT
                      190 GETB. : REM GETBLK
                      191 PUT. : REM PUTBLK
                      192 CP. : REM CPYBLK
                      193 GETX. : REM GETXOR
                      194 PUTX. : REM PUTXOR
                      195 CPYX. : REM CPYXOR
                      196 GETO. : REM GETOR
                      197 PUTO. : REM PUTOR
                      198 CPYO. : REM CPYOR
                      199 GETA. : REM GETAND
                      200 PUTA. : REM PUTAND
                      201 CPYA. : REM CPYAND
                      202 DB. : REM DBLANK
                      203 DSH. : REM DSHOW
                      204 PUT. : REM PUTCHR
                      205 LC. : REM LCASE
                      206 UC. : REM UCASE
                      208 HO. : REM HON
                      209 HOF. : REM HOFF
                      210 HS. : REM HSET
                      211 FL. : REM FLIPA
                      212 H4C. : REM H4COL
                      213 H2. : REM H2COL
                      214 H1. : REM H1COL
                      215 H3C. : REM H3COL
                      217 HSH. : REM HSHX
                      222 HC. : REM HCOL
                      223 OV. : REM OVER
                      224 UND. : REM UNDER
                      225 SW. : REM SWAPATT
                      226 DT. : REM DTCTON
                      227 DTCTOF. : REM DTCTOFF
                      228 BL. : REM BLK%BLK
                      229 OR%. : REM OR%BLK
                      230 AND%. : REM AND%BLK
                      231 XO. : REM XOR%BLK
                      232 BLK%O. : REM BLK%OR
                      233 OR%O. : REM OR%OR
                      234 AND%O. : REM AND%OR
                      235 XOR%O. : REM XOR%OR
                      236 BLK%A. : REM BLK%AND
                      237 OR%A. : REM OR%AND
                      238 AND%A. : REM AND%AND
                      239 XOR%A. : REM XOR%AND
                      240 BLK%X. : REM BLK%XOR
                      241 OR%X. : REM OR%XOR
                      242 AND%X. : REM AND%XOR
                      243 XOR%X. : REM XOR%XOR
                      244 TE. : REM TEXT
                      247 SCA. : REM SCAN
                      248 POI. : REM POINT
                      249 DF. : REM DFA
                      250 AF. : REM AFA2
                      253 FI. : REM FIRE1
                      255 J. : REM JS1
                      258 BLA. : REM BLACK
                      259 WHIT. : REM WHITE
                      261 CY. : REM CYAN
                      262 PUR. : REM PURPLE
                      263 GR. : REM GREEN
                      264 BLU. : REM BLUE
                      265 Y. : REM YELLOW
                      266 ORA. : REM ORANGE
                      267 BR. : REM BROWN
                      268 .R. : REM .RED
                      269 GRA. : REM GRAY1
                      271 .G. : REM .GREEN
                      272 .B. : REM .BLUE
                      274 OS. : REM OSC
                      277 NOI. : REM NOISE
                      278 PULS. : REM PULSE
                      281 RIN. : REM RING
                      282 SYN. : REM SYNC
                      283 MUS. : REM MUSIC
                      284 AD. : REM ADSR
                      286 MUT. : REM MUTE
                      287 VO. : REM VOLUME
                      288 CU. : REM CUTOFF
                      289 RESO. : REM RESONANCE
                      290 PA. : REM PASS
                      291 SCR. : REM SCRY
                      292 REC. : REM RECALL
                      293 STOR. : REM STORE
                      294 SID. : REM SIDCLR
                      295 ME. : REM MERGE
                      297 RESER. : REM RESERVE
                      298 OL. : REM OLD
                      300 DST. : REM DSTORE
                      301 DRE. : REM DRECALL
                      302 DM. : REM DMERGE
                      303 AU. : REM AUTO
                      304 REN. : REM RENUM
                      305 CS. : REM CSPRITE
                      306 CPU. : REM CPUT
                      307 CG. : REM CGET
                      308 CSW. : REM CSWAP
                      310 RA. : REM RASTER
                      311 EB. : REM EBACK
                      312 BG. : REM BG0
                      316 SWI. : REM SWITCH
                      317 NOR. : REM NORM
                      318 KEYOF. : REM KEYOFF
                      319 KE. : REM KEYON
                      320 MC. : REM MCOL1
                      323 FG. : REM FGND
                      324 BGN. : REM BGND
                      327 UNS. : REM UNSYNC
                      328 RS. : REM RSYNC
                      329 INI. : REM INIT
                      331 PLA. : REM PLAY
                      332 RPL. : REM RPLAY
                      333 TRA. : REM TRACK";

      var result = TestCompile( source, C64Studio.Parser.BasicFileParser.BasicVersion.LASER_BASIC );
      Assert.AreEqual( "01080D080100803A8F20454E440019080200813A8F20464F520026080300823A8F204E4558540033080400833A8F20444154410042080500843A8F20494E50555423004E080700863A8F2044494D005B080800873A8F20524541440067080900883A8F204C45540074080A00893A8F20474F544F0080080B008A3A8F2052554E0090080D008C3A8F20524553544F5245009F080E00CBA93A8F20474F53554200AE080F008E3A8F2052455455524E00BB081100A93A8F2053544F5000C6081200913A8F204F4E00D3081400933A8F204C4F414400E0081500943A8F205341564500EF081600953A8F2056455249465900FE081700963A8F2044454620464E000B091800973A8F20504F4B45001A091900983A8F205052494E54230028091A00993A8F205052494E540035091B009A3A8F20434F4E540042091C009B3A8F204C495354004E091D009C3A8F20434C52005A091E009D3A8F20434D440066091F009E3A8F2053595300730920009F3A8F204F50454E0081092100A03A8F20434C4F5345008D092200A13A8F20474554009A092400A33A8F205441422800A7092700A63A8F205350432800B4092800A73A8F205448454E00C0092900A83A8F204E4F5400CD092A00A93A8F205354455000D9093000AF3A8F20414E4400E5093500B43A8F2053474E00F1093700B63A8F2041425300FD093800B73A8F2055535200090A3900B83A8F2046524500150A3B00BA3A8F2053515200210A3C00BB3A8F20524E44002D0A3E00BD3A8F2045585000390A4000BF3A8F2053494E00450A4200C13A8F2041544E00520A4300C23A8F205045454B005F0A4500C43A8F2053545224006B0A4600C53A8F2056414C00770A4700C63A8F2041534300840A4800C73A8F204348522400920A4900C83A8F204C4546542400A10A4A00C93A8F2052494748542400AE0A4B00CA3A8F204D49442400BB0A4D00CC3A8F20454C534500C80A4E00CD3A8F204845582400D50A4F00CE3A8F204445454B00E20A5000CF3A8F205452554500F10A5100D03A8F20494D504F525400FD0A5200D13A8F2043464E000A0B5300D23A8F2053495A4500180B5400D33A8F2046414C534500250B5500D43A8F205346524500310B5600D53A8F204C505800410B5800D73A8F20434F4D4D4F4E25004E0B5900D83A8F2043524F57005B0B5A00D93A8F2043434F4C00670B5D00DC3A8F204E554D00740B5E00DD3A8F20524F573200810B5F00DE3A8F20434F4C32008E0B6000DF3A8F2053504E32009A0B6100E03A8F2048475400A60B6200E13A8F2057494400B30B6600E53A8F205441534B00C00B6700E63A8F2048414C5400CF0B6800E73A8F2052455045415400DD0B6900E83A8F20554E54494C00EB0B6A00E93A8F205748494C4500F80B6B00EA3A8F2057454E4400040C6C00EB3A8F2043494600120C6D00EC3A8F2043454C5345001F0C6E00ED3A8F2043454E44002D0C6F00EE3A8F204C4142454C003A0C7000EF3A8F20444F4B4500470C7100F03A8F204558495400580C7200F13A8F20414C4C4F4341544500680C7300F23A8F2044495341424C4500750C7400F33A8F2050554C4C00830C7500F43A8F20444C4F414400910C7600F53A8F204453415645009F0C7800F73A8F204C4F43414C00AF0C7900F83A8F2050524F43454E4400BE0C7B00FA3A8F20434153454E4400CA0C7E00FD3A8F2052505400D90C7F00FE3A8F2053455441545200E70C820001013A8F2053434C5200F70C830001023A8F2053505249544500050D840001033A8F205749504500140D850001043A8F20524553455400240D860001053A8F20483338434F4C00330D870001063A8F204C4F52455300420D880001073A8F20484952455300500D890001083A8F20504C4F54005D0D8A0001093A8F20424F58006B0D8B00010A3A8F20504F4C5900790D8C00010B3A8F204452415700880D8E00010D3A8F205332434F4C00970D8F00010E3A8F205334434F4C00A70D9000010F3A8F20483430434F4C00B50D910002223A8F205343525800C30D920001113A8F205752523100D10D930001123A8F2057524C3100DF0D9E00011D3A8F204154545200EE0DA000011F3A8F20415454555000FD0DA10001203A8F20415454444E000B0EA20001213A8F2043484152001B0EA30001223A8F2057494E444F57002A0EA40001233A8F204D554C544900380EA50001243A8F204D4F4E4F00490EA60001253A8F2054424F52444552005A0EA70001263A8F2048424F52444552006A0EA80001273A8F20545041504552007A0EA90001283A8F2048504150455200880EAA0001293A8F205752415000980EAB00012A3A8F205343524F4C4C00A80EAE00012D3A8F2041545447455400B80EAF00012E3A8F20415454324F4E00C70EB000012F3A8F204154544F4E00D70EB10001303A8F204154544F464600E40EB30001323A8F204D415200F20EB40001333A8F2057434C5200000FB60001353A8F205350494E00100FB70001363A8F204D4F56424C4B00200FB80001373A8F204D4F56584F5200300FB90001383A8F204D4F56414E44003F0FBA0001393A8F204D4F564F52004F0FBB00013A3A8F204D4F5641545400600FBE00A101093A8F20474554424C4B00700FBF00014B3A8F20505554424C4B00800FC000013F3A8F20435059424C4B00910FC100A1582E3A8F20474554584F5200A10FC20001413A8F20505554584F5200B10FC30001423A8F20435059584F5200C00FC400A1913A8F204745544F5200CF0FC50001443A8F205055544F5200DE0FC60001453A8F204350594F5200EE0FC700A1AF3A8F20474554414E4400FE0FC80001473A8F20505554414E44000E10C90001483A8F20435059414E44001E10CA0001493A8F2044424C414E4B002D10CB00014A3A8F204453484F57003D10CC00014B3A8F20505554434852004C10CD00014C3A8F204C43415345005B10CE00014D3A8F205543415345006810D000014F3A8F20484F4E007610D10001503A8F20484F4646008410D20001513A8F2048534554009310D30001523A8F20464C49504100A210D40001533A8F204834434F4C00B110D50001543A8F204832434F4C00C010D60001553A8F204831434F4C00CF10D70001563A8F204833434F4C00DD10D90001583A8F204853485800EB10DE00015D3A8F2048434F4C00F910DF00015E3A8F204F564552000811E000015F3A8F20554E444552001911E10001603A8F2053574150415454002911E20001613A8F20445443544F4E003A11E30001623A8F20445443544F4646004B11E40001633A8F20424C4B25424C4B005C11E500B0252E3A8F204F5225424C4B006E11E600AF252E3A8F20414E4425424C4B007F11E70001663A8F20584F5225424C4B008F11E80001673A8F20424C4B254F52009F11E900B025913A8F204F52254F5200B011EA00AF25913A8F20414E44254F5200C011EB00016A3A8F20584F52254F5200D111EC00016B3A8F20424C4B25414E4400E211ED00B025AF3A8F204F5225414E4400F411EE00AF25AF3A8F20414E4425414E44000512EF00016E3A8F20584F5225414E44001612F000016F3A8F20424C4B25584F52002812F100B025582E3A8F204F5225584F52003B12F200AF25582E3A8F20414E4425584F52004C12F30001723A8F20584F5225584F52005A12F40001733A8F2054455854006812F70001763A8F205343414E007712F80001773A8F20504F494E54008412F90001783A8F20444641009212FA0001793A8F204146413200A112FD00017C3A8F20464952453100AE12FF00017E3A8F204A533100BD12020102013A8F20424C41434B00CC12030102023A8F20574849544500DA12050102043A8F204359414E00EA12060102053A8F20505552504C4500F912070102063A8F20475245454E000713080102073A8F20424C5545001713090102083A8F2059454C4C4F570027130A01B0AF3A8F204F52414E47450036130B01020A3A8F2042524F574E0044130C01020B3A8F202E5245440053130D01020C3A8F2047524159310063130F01020E3A8F202E475245454E0072131001020F3A8F202E424C5545007F13120102113A8F204F5343008E13150102143A8F204E4F495345009D13160102153A8F2050554C534500AB13190102183A8F2052494E4700B9131A0102193A8F2053594E4300C8131B01021A3A8F204D5553494300D6131C01021B3A8F204144535200E4131E01021D3A8F204D55544500F4131F01021E3A8F20564F4C554D450004142001021F3A8F204355544F4646001714210102203A8F205245534F4E414E4345002514220102213A8F2050415353003314230102223A8F2053435259004314240102233A8F20524543414C4C005214250102243A8F2053544F5245006214260102253A8F20534944434C52007114270102263A8F204D45524745008214290102283A8F2052455345525645008F142A0102293A8F204F4C44009F142C01022B3A8F204453544F524500B0142D01022C3A8F2044524543414C4C00C0142E01022D3A8F20444D4552474500CE142F0102323A8F204155544F00DD14300102333A8F2052454E554D00EE14310102353A8F204353505249544500FC14320102363A8F2043505554000A15330102373A8F2043474554001915340102383A8F2043535741500029153601023A3A8F205241535445520038153701023B3A8F20454241434B0045153801023C3A8F204247300055153C0102403A8F205357495443480063153D0102413A8F204E4F524D0073153E0102423A8F204B45594F46460082153F0102433A8F204B45594F4E009115400102443A8F204D434F4C31009F15430102473A8F2046474E4400AD15440102483A8F2042474E4400BD154701024B3A8F20554E53594E4300CC154801024C3A8F205253594E4300DA154901024D3A8F20494E495400E8154B0102303A8F20504C415900F7154C0102313A8F2052504C41590006164D01022E3A8F20545241434B000000", result.ToString() );
    }

  }
}
