using System;
using System.Runtime.Intrinsics.Arm;
using C64Models.BASIC;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
  [TestClass]
  public class UnitTestBASICParser
  {
    private RetroDevStudio.Parser.BasicFileParser CreateParser( string BASICDialectName )
    {
      var parser = new RetroDevStudio.Parser.BasicFileParser( new RetroDevStudio.Parser.BasicFileParser.ParserSettings() );

      parser.Settings.StripSpaces = false;
      parser.Settings.StripREM = false;
      if ( BASICDialectName == "BASIC V2" )
      {
        parser.SetBasicDialect( Dialect.BASICV2 );
      }
      else
      {
        string error;

        var temp = AppDomain.CurrentDomain.BaseDirectory;
        var dialect = Dialect.ReadBASICDialectForUnitTest( "BASIC Dialects/" + BASICDialectName + ".txt", out error );
        if ( dialect == null )
        {
          Assert.Fail( error );
        }
        parser.SetBasicDialect( dialect );
      }

      return parser;
    }



    private GR.Memory.ByteBuffer TestCompile( string Source, string BASICDialectName, bool StripREM = false )
    {
      return TestCompile( Source, BASICDialectName, 2049, StripREM );
    }



    private GR.Memory.ByteBuffer TestCompile( string Source, string BASICDialectName, bool StripREM, bool StripSpaces )
    {
      return TestCompile( Source, BASICDialectName, 2049, StripREM, StripSpaces );
    }



    private GR.Memory.ByteBuffer TestCompile( string Source, string BASICDialectName, ushort StartAddress, bool StripREM = false, bool StripSpaces = false )
    {
      var parser = CreateParser( BASICDialectName );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile   = "test.prg";
      config.TargetType   = RetroDevStudio.Types.CompileTargetType.PRG;
      config.StartAddress = StartAddress;

      parser.Settings.StripREM    = StripREM;
      parser.Settings.StripSpaces = StripSpaces;

      bool parseResult = parser.Parse( Source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo );
      if ( !parseResult )
      {
        Debug.Log( "Testassemble failed:" );
        foreach ( var msg in asmFileInfo.Messages.Values )
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

      var parser = CreateParser( "BASIC V2" );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;
      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      string result = parser.Renumber( 10, 3, 0, 64000 );

      Assert.AreEqual( "10 IFA=1THEN 10", result );
    }



    [TestMethod]
    public void TestRenumberWithSpacesNoStripSpaces()
    {
      string      source = @"20 IFA=1THEN 20";

      var parser = CreateParser( "BASIC V2" );
      parser.Settings.StripSpaces = false;

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;
      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      string result = parser.Renumber( 10, 3, 0, 64000 );

      Assert.AreEqual( "10 IFA=1THEN 10", result );
    }



    [TestMethod]
    public void TestRenumberOverLines()
    {
      string      source = @"10 GOTO 300
                          300 GOTO 10";

      var parser = CreateParser( "BASIC V2" );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;
      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      string result = parser.Renumber( 10, 3, 0, 64000 );

      Assert.AreEqual( @"10 GOTO 13
13 GOTO 10", result );
    }



    [TestMethod]
    public void TestRenumberOnGosub()
    {
      string      source = @"10 ONXGOSUB100,400,700
                          100PRINTA
                          400 PRINTB
                          700 PRINTC";

      var parser = CreateParser( "BASIC V2" );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;
      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      string result = parser.Renumber( 10, 3, 0, 64000 );

      Assert.AreEqual( @"10 ONXGOSUB13,16,19
13PRINTA
16 PRINTB
19 PRINTC", result );
    }



    [TestMethod]
    public void TestRenumberWithStatementAfterGotoGosub()
    {
      string      source = @"10 ONXGOSUB100,400,700:GOTO2000
                          100PRINTA
                          400 PRINTB
                          700 PRINTC
                          2000 PRINTD";

      var parser = CreateParser( "BASIC V2" );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;
      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      string result = parser.Renumber( 10, 3, 0, 64000 );

      Assert.AreEqual( @"10 ONXGOSUB13,16,19:GOTO22
13PRINTA
16 PRINTB
19 PRINTC
22 PRINTD", result );
    }



    [TestMethod]
    public void TestEncodeToLabels()
    {
      string      source = @"10 PRINT ""HALLO""
                          15 GET#2,A$
                          20 GOTO 10";

      var parser = CreateParser( "BASIC V2" );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      string  encoded = parser.EncodeToLabels();
      Assert.AreEqual( @"LABEL10
PRINT ""HALLO""
GET#2,A$
GOTO LABEL10
", encoded );
    }



    [TestMethod]
    public void TestDecodeFromLabels()
    {
      string      source = @"LABEL10
PRINT ""HALLO""
GET#2,A$
GOTO LABEL10";
      var parser = CreateParser( "BASIC V2" );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;
      parser.LabelMode = true;

      bool parseResult = parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo );
      if ( !parseResult )
      {
        Debug.Log( "Testassemble failed:" );
        foreach ( var msg in asmFileInfo.Messages.Values )
        {
          Debug.Log( msg.Message );
        }
      }

      string  decoded = parser.DecodeFromLabels( 10, 5 );
      Assert.AreEqual( @"10 PRINT ""HALLO""
15 GET#2,A$
20 GOTO 10
", decoded );
    }



    [TestMethod]
    public void TestEncodeAndDecode()
    {
      string      source = @"10 PRINT ""HALLO""
15 GET#2,A$
20 GOTO 10
";
      var parser = CreateParser( "BASIC V2" );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;
      config.DoNotExpandStringLiterals = true;

      bool parseResult = parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo );
      if ( !parseResult )
      {
        Debug.Log( "Testassemble failed:" );
        foreach ( var msg in asmFileInfo.Messages.Values )
        {
          Debug.Log( msg.Message );
        }
      }

      string  encoded = parser.EncodeToLabels();
      Assert.AreEqual( @"LABEL10
PRINT ""HALLO""
GET#2,A$
GOTO LABEL10
", encoded );

      parser.LabelMode = true;

      parseResult = parser.Parse( encoded, null, config, null, out asmFileInfo );
      if ( !parseResult )
      {
        Debug.Log( "Testassemble failed:" );
        foreach ( var msg in asmFileInfo.Messages.Values )
        {
          Debug.Log( msg.Message );
        }
      }

      string  decoded = parser.DecodeFromLabels( 10, 5 );
      Assert.AreEqual( source, decoded );
    }



    [TestMethod]
    public void TestEncodeAndDecodeToLabelsWithREM()
    {
      string      source = @"10 PRINT ""HALLO""
15 GET#2, A$:REM HURZ
20 GOTO 10
";
      var parser = CreateParser( "BASIC V2" );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;
      config.DoNotExpandStringLiterals = true;

      bool parseResult = parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo );
      if ( !parseResult )
      {
        Debug.Log( "Testassemble failed:" );
        foreach ( var msg in asmFileInfo.Messages.Values )
        {
          Debug.Log( msg.Message );
        }
      }

      string  encoded = parser.EncodeToLabels();
      Assert.AreEqual( @"LABEL10
PRINT ""HALLO""
GET#2, A$:REM HURZ
GOTO LABEL10
", encoded );

      parser.LabelMode = true;

      parseResult = parser.Parse( encoded, null, config, null, out asmFileInfo );
      if ( !parseResult )
      {
        Debug.Log( "Testassemble failed:" );
        foreach ( var msg in asmFileInfo.Messages.Values )
        {
          Debug.Log( msg.Message );
        }
      }

      string  decoded = parser.DecodeFromLabels( 10, 5 );
      Assert.AreEqual( source, decoded );
    }



    [TestMethod]
    public void TestEncodeAndDecodeToLabelsWithOnGoto()
    {
      string      source = @"10 PRINT ""HALLO""
15 GET#2, A$:REM HURZ
20 ON X GOTO 10, 15, 20
";
      var parser = CreateParser( "BASIC V2" );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;
      config.DoNotExpandStringLiterals = true;

      bool parseResult = parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo );
      if ( !parseResult )
      {
        Debug.Log( "Testassemble failed:" );
        foreach ( var msg in asmFileInfo.Messages.Values )
        {
          Debug.Log( msg.Message );
        }
      }

      string  encoded = parser.EncodeToLabels();
      Assert.AreEqual( @"LABEL10
PRINT ""HALLO""

LABEL15
GET#2, A$:REM HURZ

LABEL20
ON X GOTO LABEL10, LABEL15, LABEL20
", encoded );

      parser.LabelMode = true;

      parseResult = parser.Parse( encoded, null, config, null, out asmFileInfo );
      if ( !parseResult )
      {
        Debug.Log( "Testassemble failed:" );
        foreach ( var msg in asmFileInfo.Messages.Values )
        {
          Debug.Log( msg.Message );
        }
      }

      string  decoded = parser.DecodeFromLabels( 10, 5 );
      Assert.AreEqual( source, decoded );
    }



    [TestMethod]
    public void TestEncodeAndDecodeToLabelsWithLabelIn2ndArgument()
    {
      string      source = @"10 COLLISION 2, 50
30 GET#2, A$:REM HURZ
50 PRINT ""HALLO"":GOTO 10
";
      var parser = CreateParser( "BASIC V7.0" );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;
      config.DoNotExpandStringLiterals = true;

      bool parseResult = parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo );
      if ( !parseResult )
      {
        Debug.Log( "Testassemble failed:" );
        foreach ( var msg in asmFileInfo.Messages.Values )
        {
          Debug.Log( msg.Message );
        }
      }

      string  encoded = parser.EncodeToLabels();
      Assert.AreEqual( @"LABEL10
COLLISION 2, LABEL50
GET#2, A$:REM HURZ

LABEL50
PRINT ""HALLO"":GOTO LABEL10
", encoded );

      // and reverse...
      parser.LabelMode = true;

      parseResult = parser.Parse( encoded, null, config, null, out asmFileInfo );
      if ( !parseResult )
      {
        Debug.Log( "Testassemble failed:" );
        foreach ( var msg in asmFileInfo.Messages.Values )
        {
          Debug.Log( msg.Message );
        }
      }

      string  decoded = parser.DecodeFromLabels( 10, 20 );
      Assert.AreEqual( source, decoded );
    }



    [TestMethod]
    public void TestEncodeAndDecodeToLabelsWithSplitGoTo()
    {
      string      source = @"10 GO TO 50:PRINT ""HURZ""
30 GET#2, A$:REM HURZ
50 PRINT ""HALLO"":GOTO 10
";
      var parser = CreateParser( "BASIC V7.0" );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;
      config.DoNotExpandStringLiterals = true;

      bool parseResult = parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo );
      if ( !parseResult )
      {
        Debug.Log( "Testassemble failed:" );
        foreach ( var msg in asmFileInfo.Messages.Values )
        {
          Debug.Log( msg.Message );
        }
      }

      string  encoded = parser.EncodeToLabels();
      Assert.AreEqual( @"LABEL10
GO TO LABEL50:PRINT ""HURZ""
GET#2, A$:REM HURZ

LABEL50
PRINT ""HALLO"":GOTO LABEL10
", encoded );

      // and reverse...
      parser.LabelMode = true;

      parseResult = parser.Parse( encoded, null, config, null, out asmFileInfo );
      if ( !parseResult )
      {
        Debug.Log( "Testassemble failed:" );
        foreach ( var msg in asmFileInfo.Messages.Values )
        {
          Debug.Log( msg.Message );
        }
      }

      string  decoded = parser.DecodeFromLabels( 10, 20 );
      Assert.AreEqual( source, decoded );
    }



    [TestMethod]
    public void TestEncodeToLabelsONGoto()
    {
      string      source = @"10 PRINT ""HALLO""
                          15 GET#2, A$
                          20 ON X GOTO 10,  15, 20";

      var parser = CreateParser( "BASIC V2" );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      string  encoded = parser.EncodeToLabels();
      Assert.AreEqual( @"LABEL10
PRINT ""HALLO""

LABEL15
GET#2, A$

LABEL20
ON X GOTO LABEL10,  LABEL15, LABEL20
", encoded );
    }



    [TestMethod]
    public void TestEncodeToLabelsNoStripSpaces()
    {
      string      source = @"10 PRINT ""HALLO""
                          20 GOTO 10";

      var parser = CreateParser( "BASIC V2" );

      parser.Settings.StripSpaces = false;

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      string  encoded = parser.EncodeToLabels();
      Assert.AreEqual( @"LABEL10
PRINT ""HALLO""
GOTO LABEL10
", encoded );
    }



    [TestMethod]
    public void TestLaserBASICNoTokenAfterTask()
    {
      // disabled for now
      /*
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

      var result = TestCompile( source, RetroDevStudio.Parser.BasicFileParser.BasicVersion.LASER_BASIC );

      Assert.AreEqual( "01082C0805008F20544553542057494E444F5742204C4142454C204E414D4520544F4B454E49534154494F4E0060080A008F2044454D4F4E535452415445204D554C54492D5441534B494E4720414E4420534F554E4420494E2041205441534B006708140002250082081E00F13130302C30274D454D4F525981E5F64941424C4553009F083200E5312C0122423A992241204C494E45204F4620544558542200AD083C00F957279241E92E2E2E00CA0846009922414E4F54484552204C494E45204F462054455854212200DA085000893630279281455645522100E0085A009000E60864002700EF086E00EE01224200F5087800E700010982008149B230A431350009098C000127490012099600021E3135001F09A0000213312C33303030003109AA00021B312C302C31322C352C3132003909B400021431004109BE00022130004B09C800021F313230005409D20002203132005F09DC00021A312C3230007B09E6008144B231A43735303A82442792414249544C4F4E474552008209F0008249008909FA00E8D3008F0904012700AE090E01EE573A544924B222303030303030223AE73AE85449B135303AF800B409180127000000", result.ToString() );*/
    }



    [TestMethod]
    public void TestBASICV2NoTokenAfterThen()
    {
      string    source = @"66 IFINT(B)<>BTHEN66";

      var result = TestCompile( source, "BASIC V2" );

      Assert.AreEqual( "0108110842008BB5284229B3B142A73636000000", result.ToString() );
    }



    [TestMethod]
    public void TestBASICStripREM()
    {
      string    source = @"10 PRINT""HALLO"":REM PRINT""HURZ""
                           20 PRINT""WELT""";

      var result = TestCompile( source, "BASIC V2", true );

      Assert.AreEqual( "01080E080A00992248414C4C4F22001A081400992257454C5422000000", result.ToString() );
    }



    [TestMethod]
    public void TestBASICStripREMNoStripInREM()
    {
      string    source = @"10 PRINT""HALLO"":REM PRINT""HURZ""
                           20 PRINT""WELT""";

      var result = TestCompile( source, "BASIC V2" );

      Assert.AreEqual( "01081C080A00992248414C4C4F223A8F205052494E54224855525A220028081400992257454C5422000000", result.ToString() );
    }



    [TestMethod]
    public void TestBASICStripREMNoStripInDATA()
    {
      string    source = @"10 PRINT""HALLO"":REM PRINT""HURZ""
                           20 DATA LSR X,, RTS, ADC (X,,,,ADC $,ROR$,,PLA,ADC #,ROR,,JMP (,ADC?,ROR?,";

      var result = TestCompile( source, "BASIC V2" );

      Assert.AreEqual( "01081C080A00992248414C4C4F223A8F205052494E54224855525A22006508140083204C535220582C2C205254532C204144432028582C2C2C2C41444320242C524F52242C2C504C412C41444320232C524F522C2C4A4D5020282C4144433F2C524F523F2C000000", result.ToString() );
    }



    [TestMethod]
    public void TestBASICStripREMNoStripInDATAButOnStartOfDATAEntry()
    {
      string    source = @"20 DATA A  B C,  D E  F   ,G H I   ";

      var result = TestCompile( source, "BASIC V2", false, true );

      Assert.AreEqual( "01081D081400834120204220432C4420452020462020202C4720482049000000", result.ToString() );
    }



    // we had a bug where GOTO/GOSUB in front of a REM was not added to references when REM stripping was active
    [TestMethod]
    public void TestBASICStripREMWithGOTOInFront()
    {
      string    source = @"10 GOTO 20:REM PRINT""HURZ""
                           20 PRINT""WELT""";

      var parser = CreateParser( "BASIC V2" );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;
      parser.Settings.StripREM = true;

      bool parseResult = parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo );
      if ( !parseResult )
      {
        foreach ( var msg in asmFileInfo.Messages.Values )
        {
          Debug.Log( msg.Message );
        }
        Assert.Fail( "Testassemble failed:" );
      }
      Assert.IsTrue( parser.Assemble( config ) );

      Assert.AreEqual( "01080A080A00892032300016081400992257454C5422000000", parser.AssembledOutput.Assembly.ToString() );

      var renumbered = parser.Renumber( 1, 1, 10, 20 );

      string    renumberedSource = @"1 GOTO 2:REM PRINT""HURZ""
2 PRINT""WELT""";

      Assert.AreEqual( renumberedSource, renumbered );
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
                      47 {ARROW UP}
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

      //source = "238 AND%AND";
      var result = TestCompile( source, "Laser BASIC", 0x4e01 );

      //Assert.AreEqual( "01080708010080000D08020081001308030082001908040083001F08050084002508060085002B08070086003108080087003708090088003D080A00890043080B008A0049080C008B004F080D008C0055080E008D005B080F008E00610810008F006708110090006D08120091007308130092007908140093007F08150094008508160095008C08170096A5009208180097009808190098009E081A009900A4081B009A00AA081C009B00B0081D009C00B6081E009D00BC081F009E00C20820009F00C8082100A000CE082200A100D4082300A200DA082400A300E0082500A400E6082600A500EC082700A600F2082800A700F8082900A800FE082A00A90004092B00AA000A092C00AB0010092D00AC0016092E00AD001C092F00AE0022093000AF0028093100B0002E093200B10034093300B2003A093400B30040093500B40046093600B5004C093700B60052093800B70058093900B8005E093A00B90064093B00BA006A093C00BB0070093D00BC0076093E00BD007C093F00BE0082094000BF0088094100C0008E094200C10094094300C2009A094400C300A0094500C400A6094600C500AC094700C600B2094800C700B8094900C800BE094A00C900C4094B00CA00CA094D00CC00D0094E00CD00D6094F00CE00DC095000CF00E2095100D000E8095200D100EE095300D200F4095400D300FA095500D400000A5600D500060A5700D6000C0A5800D700120A5900D800180A5A00D9001E0A5B00DA00240A5C00DB002A0A5D00DC00300A5E00DD00360A5F00DE003C0A6000DF00420A6100E000480A6200E1004E0A6300E200540A6400E3005A0A6500E400600A6600E500660A6700E6006C0A6800E700720A6900E800780A6A00E9007E0A6B00EA00840A6C00EB008A0A6D00EC00900A6E00ED00960A6F00EE009C0A7000EF00A20A7100F000A80A7200F100AE0A7300F200B40A7400F300BA0A7500F400C00A7600F500C60A7700F600CC0A7800F700D20A7900F800D80A7A00F900DE0A7B00FA00E40A7C00FB00EA0A7D00FC00F00A7E00FD00F60A7F00FE00FC0A8000FF00030B82000101000A0B8300010200110B8400010300180B85000104001F0B8600010500260B87000106002D0B8800010700340B89000108003B0B8A00010900420B8B00010A00490B8C00010B00500B8D00010C00570B8E00010D005E0B8F00010E00650B9000010F006C0B9100011000730B92000111007A0B9300011200810B9400011300880B95000114008F0B9600011500960B97000116009D0B9800011700A40B9900011800AB0B9A00011900B20B9B00011A00B90B9C00011B00C00B9D00011C00C70B9E00011D00CE0B9F00011E00D50BA000011F00DC0BA100012000E30BA200012100EA0BA300012200F10BA400012300F80BA500012400FF0BA600012500060CA7000126000D0CA800012700140CA9000128001B0CAA00012900220CAB00012A00290CAC00012B00300CAD00012C00370CAE00012D003E0CAF00012E00450CB000012F004C0CB100013000530CB2000131005A0CB300013200610CB400013300680CB5000134006F0CB600013500760CB7000136007D0CB800013700840CB9000138008B0CBA00013900920CBB00013A00990CBC00013B00A00CBD00013C00A90CBE00A1424C4B00B00CBF00013E00B70CC000013F00BF0CC100A158B000C60CC200014100CD0CC300014200D40CC400A1B000DB0CC500014400E20CC600014500E90CC700A1AF00F00CC800014700F70CC900014800FE0CCA00014900050DCB00014A000C0DCC00014B00130DCD00014C001A0DCE00014D00210DCF00014E00280DD000014F002F0DD100015000360DD2000151003D0DD300015200440DD4000153004B0DD500015400520DD600015500590DD700015600600DD800015700670DD9000158006E0DDA00015900750DDB00015A007C0DDC00015B00830DDD00015C008A0DDE00015D00910DDF00015E00980DE000015F009F0DE100016000A60DE200016100AD0DE300016200B40DE400016300BE0DE500B025424C4B00C80DE600AF25424C4B00CF0DE700016600D60DE800016700DE0DE900B025B000E60DEA00AF25B000ED0DEB00016A00F40DEC00016B00FC0DED00B025AF00040EEE00AF25AF000B0EEF00016E00120EF000016F001B0EF100B02558B000240EF200AF2558B0002B0EF300017200320EF400017300390EF500017400400EF600017500470EF7000176004E0EF800017700550EF9000178005C0EFA00017900630EFB00017A006A0EFC00017B00710EFD00017C00780EFE00017D007F0EFF00017E00860E0001017F008D0E0201020100940E03010202009B0E0401020300A20E0501020400A90E0601020500B00E0701020600B70E0801020700BE0E0901020800C80E0A01B0414E474500CF0E0B01020A00D60E0C01020B00DD0E0D01020C00E40E0E01020D00EB0E0F01020E00F20E1001020F00F90E1101021000000F1201021100070F13010212000E0F1401021300150F15010214001C0F1601021500230F17010216002A0F1801021700310F1901021800380F1A010219003F0F1B01021A00460F1C01021B004D0F1E01021D00540F1F01021E005B0F2001021F00620F2101022000690F2201022100700F2301022200770F24010223007E0F2501022400850F26010225008C0F2701022600930F28010227009A0F2901022800A10F2A01022900A80F2B01022A00AF0F2C01022B00B60F2D01022C00BD0F2E01022D00C40F2F01023200CB0F3001023300D20F3101023500D90F3201023600E00F3301023700E70F3401023800EE0F3501023900F50F3601023A00FC0F3701023B0003103801023C000A103901023D0011103A01023E0018103B01023F001F103C0102400026103D010241002D103E0102420034103F010243003B104001024400421041010245004910420102460050104301024700571044010248005E10450102490065104601024A006C104701024B0073104801024C007A104901024D0081104A01022F0088104B010230008F104C0102310096104D01022E000000", result.ToString() );

      // this output was generated via VICE, so should be correct
      Assert.AreEqual( "014E074E010080000D4E02008100134E03008200194E040083001F4E05008400254E060085002B4E07008600314E08008700374E090088003D4E0A008900434E0B008A00494E0C008B004F4E0D008C00554E0E008D005B4E0F008E00614E10008F00674E110090006D4E12009100734E13009200794E140093007F4E15009400854E160095008D4E17009620A500934E18009700994E190098009F4E1A009900A54E1B009A00AB4E1C009B00B14E1D009C00B74E1E009D00BD4E1F009E00C34E20009F00C94E2100A000CF4E2200A100D54E2300A200DB4E2400A300E14E2500A400E74E2600A500ED4E2700A600F34E2800A700F94E2900A800FF4E2A00A900054F2B00AA000B4F2C00AB00114F2D00AC00174F2E00AD001D4F2F00AE00234F3000AF00294F3100B0002F4F3200B100354F3300B2003B4F3400B300414F3500B400474F3600B5004D4F3700B600534F3800B700594F3900B8005F4F3A00B900654F3B00BA006B4F3C00BB00714F3D00BC00774F3E00BD007D4F3F00BE00834F4000BF00894F4100C0008F4F4200C100954F4300C2009B4F4400C300A14F4500C400A74F4600C500AD4F4700C600B34F4800C700B94F4900C800BF4F4A00C900C54F4B00CA00CB4F4D00CC00D14F4E00CD00D74F4F00CE00DD4F5000CF00E34F5100D000E94F5200D100EF4F5300D200F54F5400D300FB4F5500D40001505600D50007505700D6000D505800D70013505900D80019505A00D9001F505B00DA0025505C00DB002B505D00DC0031505E00DD0037505F00DE003D506000DF0043506100E00049506200E1004F506300E20055506400E3005B506500E40061506600E50067506700E6006D506800E70073506900E80079506A00E9007F506B00EA0085506C00EB008B506D00EC0091506E00ED0097506F00EE009D507000EF00A3507100F000A9507200F100AF507300F200B5507400F300BB507500F400C1507600F500C7507700F600CD507800F700D3507900F800D9507A00F900DF507B00FA00E5507C00FB00EB507D00FC00F1507E00FD00F7507F00FE00FD508000FF00045182000101000B518300010200125184000103001951850001040020518600010500275187000106002E518800010700355189000108003C518A0001090043518B00010A004A518C00010B0051518D00010C0058518E00010D005F518F00010E0066519000010F006D519100011000745192000111007B519300011200825194000113008951950001140090519600011500975197000116009E519800011700A5519900011800AC519A00011900B3519B00011A00BA519C00011B00C1519D00011C00C8519E00011D00CF519F00011E00D651A000011F00DD51A100012000E451A200012100EB51A300012200F251A400012300F951A5000124000052A6000125000752A7000126000E52A8000127001552A9000128001C52AA000129002352AB00012A002A52AC00012B003152AD00012C003852AE00012D003F52AF00012E004652B000012F004D52B1000130005452B2000131005B52B3000132006252B4000133006952B5000134007052B6000135007752B7000136007E52B8000137008552B9000138008C52BA000139009352BB00013A009A52BC00013B00A152BD00013C00A852BE00013D00AF52BF00013E00B652C000013F00BD52C100014000C452C200014100CB52C300014200D252C400014300D952C500014400E052C600014500E752C700014600EE52C800014700F552C900014800FC52CA000149000353CB00014A000A53CC00014B001153CD00014C001853CE00014D001F53CF00014E002653D000014F002D53D1000150003453D2000151003B53D3000152004253D4000153004953D5000154005053D6000155005753D7000156005E53D8000157006553D9000158006C53DA000159007353DB00015A007A53DC00015B008153DD00015C008853DE00015D008F53DF00015E009653E000015F009D53E100016000A453E200016100AB53E300016200B253E400016300B953E500016400C053E600016500C753E700016600CE53E800016700D553E900016800DC53EA00016900E353EB00016A00EA53EC00016B00F153ED00016C00F853EE00016D00FF53EF00016E000654F000016F000D54F1000170001454F2000171001B54F3000172002254F4000173002954F5000174003054F6000175003754F7000176003E54F8000177004554F9000178004C54FA000179005354FB00017A005A54FC00017B006154FD00017C006854FE00017D006F54FF00017E0076540001017F007D540201020100845403010202008B5404010203009254050102040099540601020500A0540701020600A7540801020700AE540901020800B5540A01020900BC540B01020A00C3540C01020B00CA540D01020C00D1540E01020D00D8540F01020E00DF541001020F00E6541101021000ED541201021100F4541301021200FB541401021300025515010214000955160102150010551701021600175518010217001E55190102180025551A010219002C551B01021A0033551C01021B003A551E01021D0041551F01021E0048552001021F004F552101022000565522010221005D552301022200645524010223006B552501022400725526010225007955270102260080552801022700875529010228008E552A0102290095552B01022A009C552C01022B00A3552D01022C00AA552E01022D00B1552F01023200B8553001023300BF553101023500C6553201023600CD553301023700D4553401023800DB553501023900E2553601023A00E9553701023B00F0553801023C00F7553901023D00FE553A01023E0005563B01023F000C563C0102400013563D010241001A563E0102420021563F01024300285640010244002F564101024500365642010246003D564301024700445644010248004B56450102490052564601024A0059564701024B0060564801024C0067564901024D006E564A01022F0075564B010230007C564C0102310083564D01022E000000", result.ToString() );
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

      var result = TestCompile( source, "Laser BASIC", 0x5b01 );

      // TODO - REM text stays uppercase!
      //Assert.AreEqual( "015B0F5B010080203A208F20C5CEC4001D5B020081203A208F20C6CFD2002C5B030082203A208F20CEC5D8D4003B5B040083203A208F20C4C1D4C1004C5B050084203A208F20C9CED0D5D423005A5B070086203A208F20C4C9CD00695B080087203A208F20D2C5C1C400775B090088203A208F20CCC5D400865B0A0089203A208F20C7CFD4CF00945B0B008A203A208F20D2D5CE00A65B0D008C203A208F20D2C5D3D4CFD2C500B65B0E008D203A208F20C7CFD3D5C200C75B0F008E203A208F20D2C5D4D5D2CE00D65B110090203A208F20D3D4CFD000E35B120091203A208F20CFCE00F25B140093203A208F20CCCFC1C400015C150094203A208F20D3C1D6C500125C160095203A208F20D6C5D2C9C6D900235C170096203A208F20C4C5C620C6CE00325C180097203A208F20D0CFCBC500435C190098203A208F20D0D2C9CED42300535C1A0099203A208F20D0D2C9CED400625C1B009A203A208F20C3CFCED400715C1C009B203A208F20CCC9D3D4007F5C1D009C203A208F20C3CCD2008D5C1E009D203A208F20C3CDC4009B5C1F009E203A208F20D3D9D300AA5C20009F203A208F20CFD0C5CE00BA5C2100A0203A208F20C3CCCFD3C500C85C2200A1203A208F20C7C5D400D75C2400A3203A208F20D4C1C22800E65C2700A6203A208F20D3D0C32800F55C2800A7203A208F20D4C8C5CE00035D2900A8203A208F20CECFD400125D2A0090203A208F20D3D4C5D000205D3000AF203A208F20C1CEC4002E5D3500B4203A208F20D3C7CE003C5D3700B6203A208F20C1C2D3004A5D3800B7203A208F20D5D3D200585D3900B8203A208F20C6D2C500665D3B00BA203A208F20D3D1D200745D3C00BB203A208F20D2CEC400825D3E00BD203A208F20C5D8D000905D4000BF203A208F20D3C9CE009E5D4200C1203A208F20C1D4CE00AD5D4300C2203A208F20D0C5C5CB00BC5D4500C4203A208F20D3D4D22400CA5D4600C5203A208F20D6C1CC00D85D4700C6203A208F20C1D3C300E75D4800C7203A208F20C3C8D22400F75D4900C8203A208F20CCC5C6D42400085E4A00C9203A208F20D2C9C7C8D42400175E4B00CA203A208F20CDC9C42400265E4D00CC203A208F20C5CCD3C500355E4E00CD203A208F20C8C5D82400445E4F00CE203A208F20C4C5C5CB00535E5000CF203A208F20D4D2D5C500645E5100D0203A208F20C9CDD0CFD2D400725E5200D1203A208F20C3C6CE00815E5300D2203A208F20D3C9DAC500915E5400D3203A208F20C6C1CCD3C500A05E5500D4203A208F20D3C6D2C500AE5E5600D5203A208F20CCD0D800C05E5800D7203A208F20C3CFCDCDCFCE2500CF5E5900D8203A208F20C3D2CFD700DE5E5A00D9203A208F20C3C3CFCC00EC5E5D00DC203A208F20CED5CD00FB5E5E00DD203A208F20D2CFD732000A5F5F00DE203A208F20C3CFCC3200195F6000DF203A208F20D3D0CE3200275F6100E0203A208F20C8C7D400355F6200E1203A208F20D7C9C400445F6600E5203A208F20D4C1D3CB00535F6700E6203A208F20C8C1CCD400645F6800E7203A208F20D2C5D0C5C1D400745F6900E8203A208F20D5CED4C9CC00845F6A00E9203A208F20D7C8C9CCC500935F6B00EA203A208F20D7C5CEC400A15F6C00EB203A208F20C3C9C600B15F6D00EC203A208F20C3C5CCD3C500C05F6E00ED203A208F20C3C5CEC400D05F6F00EE203A208F20CCC1C2C5CC00DF5F7000EF203A208F20C4CFCBC500EE5F7100F0203A208F20C5D8C9D40001607200F1203A208F20C1CCCCCFC3C1D4C50013607300F2203A208F20C4C9D3C1C2CCC50022607400F3203A208F20D0D5CCCC0032607500F4203A208F20C4CCCFC1C40042607600F5203A208F20C4D3C1D6C50052607800F7203A208F20CCCFC3C1CC0064607900F8203A208F20D0D2CFC3C5CEC40075607B00FA203A208F20C3C1D3C5CEC40083607E00FD203A208F20D2D0D40094607F00FE203A208F20D3C5D4C1D4D200A46082000101203A208F20D3C3CCD200B66083000102203A208F20D3D0D2C9D4C500C66084000103203A208F20D7C9D0C500D76085000104203A208F20D2C5D3C5D400E96086000105203A208F20C83338C3CFCC00FA6087000106203A208F20CCCFD2C5D3000B6188000107203A208F20C8C9D2C5D3001B6189000108203A208F20D0CCCFD4002A618A000109203A208F20C2CFD8003A618B00010A203A208F20D0CFCCD9004A618C00010B203A208F20C4D2C1D7005B618E00010D203A208F20D332C3CFCC006C618F00010E203A208F20D334C3CFCC007E619000010F203A208F20C83430C3CFCC008E6191000110203A208F20D3C3D2D8009E6192000111203A208F20D7D2D23100AE6193000112203A208F20D7D2CC3100BE619E00011D203A208F20C1D4D4D200CF61A000011F203A208F20C1D4D4D5D000E061A1000120203A208F20C1D4D4C4CE00F061A2000121203A208F20C3C8C1D2000262A3000122203A208F20D7C9CEC4CFD7001362A4000123203A208F20CDD5CCD4C9002362A5000124203A208F20CDCFCECF003662A6000125203A208F20D4C2CFD2C4C5D2004962A7000126203A208F20C8C2CFD2C4C5D2005B62A8000127203A208F20D4D0C1D0C5D2006D62A9000128203A208F20C8D0C1D0C5D2007D62AA000129203A208F20D7D2C1D0008F62AB00012A203A208F20D3C3D2CFCCCC00A162AE00012D203A208F20C1D4D4C7C5D400B362AF00012E203A208F20C1D4D432CFCE00C462B000012F203A208F20C1D4D4CFCE00D662B1000130203A208F20C1D4D4CFC6C600E562B3000132203A208F20CDC1D200F562B4000133203A208F20D7C3CCD2000563B6000135203A208F20D3D0C9CE001763B7000136203A208F20CDCFD6C2CCCB002963B8000137203A208F20CDCFD6D8CFD2003B63B9000138203A208F20CDCFD6C1CEC4004C63BA000139203A208F20CDCFD6CFD2005E63BB00013A203A208F20CDCFD6C1D4D4007063BE00013D203A208F20C7C5D4C2CCCB008263BF00013E203A208F20D0D5D4C2CCCB009463C000013F203A208F20C3D0D9C2CCCB00A663C1000140203A208F20C7C5D4D8CFD200B863C2000141203A208F20D0D5D4D8CFD200CA63C3000142203A208F20C3D0D9D8CFD200DB63C4000143203A208F20C7C5D4CFD200EC63C5000144203A208F20D0D5D4CFD200FD63C6000145203A208F20C3D0D9CFD2000F64C7000146203A208F20C7C5D4C1CEC4002164C8000147203A208F20D0D5D4C1CEC4003364C9000148203A208F20C3D0D9C1CEC4004564CA000149203A208F20C4C2CCC1CECB005664CB00014A203A208F20C4D3C8CFD7006864CC00013E203A208F20D0D5D4C3C8D2007964CD00014C203A208F20CCC3C1D3C5008A64CE00014D203A208F20D5C3C1D3C5009964D000014F203A208F20C8CFCE00A964D1000150203A208F20C8CFC6C600B964D2000151203A208F20C8D3C5D400CA64D3000152203A208F20C6CCC9D0C100DB64D4000153203A208F20C834C3CFCC00EC64D5000154203A208F20C832C3CFCC00FD64D6000155203A208F20C831C3CFCC000E65D7000156203A208F20C833C3CFCC001E65D9000158203A208F20C8D3C8D8002E65DE00015D203A208F20C8C3CFCC003E65DF00015E203A208F20CFD6C5D2004F65E000015F203A208F20D5CEC4C5D2006265E1000160203A208F20D3D7C1D0C1D4D4007465E2000161203A208F20C4D4C3D4CFCE008765E3000162203A208F20C4D4C3D4CFC6C6009A65E4000163203A208F20C2CCCB25C2CCCB00AC65E5000164203A208F20CFD225C2CCCB00BF65E6000165203A208F20C1CEC425C2CCCB00D265E7000166203A208F20D8CFD225C2CCCB00E465E8000167203A208F20C2CCCB25CFD200F565E9000168203A208F20CFD225CFD2000766EA000169203A208F20C1CEC425CFD2001966EB00016A203A208F20D8CFD225CFD2002C66EC00016B203A208F20C2CCCB25C1CEC4003E66ED00016C203A208F20CFD225C1CEC4005166EE00016D203A208F20C1CEC425C1CEC4006466EF00016E203A208F20D8CFD225C1CEC4007766F000016F203A208F20C2CCCB25D8CFD2008966F1000170203A208F20CFD225D8CFD2009C66F2000171203A208F20C1CEC425D8CFD200AF66F3000172203A208F20D8CFD225D8CFD200BF66F4000173203A208F20D4C5D8D400CF66F7000176203A208F20D3C3C1CE00E066F8000177203A208F20D0CFC9CED400EF66F9000178203A208F20C4C6C100FF66FA000179203A208F20C1C6C132001067FD00017C203A208F20C6C9D2C531001F67FF00017E203A208F20CAD33100306702010201203A208F20C2CCC1C3CB00416703010202203A208F20D7C8C9D4C500516705010204203A208F20C3D9C1CE00636706010205203A208F20D0D5D2D0CCC500746707010206203A208F20C7D2C5C5CE00846708010207203A208F20C2CCD5C500966709010208203A208F20D9C5CCCCCFD700A8670A010209203A208F20CFD2C1CEC7C500B9670B01020A203A208F20C2D2CFD7CE00C9670C01020B203A208F202ED2C5C400DA670D01020C203A208F20C7D2C1D93100EC670F01020E203A208F202EC7D2C5C5CE00FD671001020F203A208F202EC2CCD5C5000C6812010211203A208F20CFD3C3001D6815010214203A208F20CECFC9D3C5002E6816010215203A208F20D0D5CCD3C5003E6819010218203A208F20D2C9CEC7004E681A010219203A208F20D3D9CEC3005F681B01021A203A208F20CDD5D3C9C3006F681C01021B203A208F20C1C4D3D2007F681E01021D203A208F20CDD5D4C50091681F01021E203A208F20D6CFCCD5CDC500A3682001021F203A208F20C3D5D4CFC6C600B86821010220203A208F20D2C5D3CFCEC1CEC3C500C86822010221203A208F20D0C1D3D300D86823010110203A208F20D3C3D2D900EA6824010223203A208F20D2C5C3C1CCCC00FB6825010224203A208F20D3D4CFD2C5000D6926010225203A208F20D3C9C4C3CCD2001E6927010226203A208F20CDC5D2C7C500316929010228203A208F20D2C5D3C5D2D6C50040692A010229203A208F20CFCCC40052692C01022B203A208F20C4D3D4CFD2C50065692D01022C203A208F20C4D2C5C3C1CCCC0077692E01022D203A208F20C4CDC5D2C7C50087692F010232203A208F20C1D5D4CF00986930010233203A208F20D2C5CED5CD00AB6931010235203A208F20C3D3D0D2C9D4C500BB6932010236203A208F20C3D0D5D400CB6933010237203A208F20C3C7C5D400DC6934010238203A208F20C3D3D7C1D000EE693601023A203A208F20D2C1D3D4C5D200FF693701023B203A208F20C5C2C1C3CB000E6A3801023C203A208F20C2C73000206A3C010240203A208F20D3D7C9D4C3C800306A3D010241203A208F20CECFD2CD00426A3E010242203A208F20CBC5D9CFC6C600536A3F010242203A208F20CBC5D9CFCE00646A40010244203A208F20CDC3CFCC3100746A43010247203A208F20C6C7CEC400846A44010248203A208F20C2C7CEC400966A4701024B203A208F20D5CED3D9CEC300A76A4801024C203A208F20D2D3D9CEC300B76A4901024D203A208F20C9CEC9D400C76A4B010230203A208F20D0CCC1D900D86A4C010231203A208F20D2D0CCC1D900E96A4D01022E203A208F20D4D2C1C3CB000000", result.ToString() );
    }



  }

}
