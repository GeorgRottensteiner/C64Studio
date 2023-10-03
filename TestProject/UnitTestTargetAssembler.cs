using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
  [TestClass]
  public class UnitTestTargetAssembler
  {
    [TestMethod]
    public void TestCart16KBin()
    {
      string      source = @"* = $8000
                              !byte 0,1,2,3,4,5,6,7";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.crt";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.CARTRIDGE_16K_BIN;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( GR.IO.File.ReadAllBytes( "Compare Files/Cartridge16kBin.bin" ), assembly.Assembly );
    }



    [TestMethod]
    public void TestCart16KCrt()
    {
      string      source = @"* = $8000
                              !byte 0,1,2,3,4,5,6,7";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.crt";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.CARTRIDGE_16K_CRT;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( GR.IO.File.ReadAllBytes( "Compare Files/Cartridge16kCrt.bin" ), assembly.Assembly );
    }



    [TestMethod]
    public void TestCart8KBin()
    {
      string      source = @"* = $8000
                              !byte 0,1,2,3,4,5,6,7";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.crt";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.CARTRIDGE_8K_BIN;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( GR.IO.File.ReadAllBytes( "Compare Files/Cartridge8kBin.bin" ), assembly.Assembly );
    }



    [TestMethod]
    public void TestCart8KCrt()
    {
      string      source = @"* = $8000
                              !byte 0,1,2,3,4,5,6,7";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.crt";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.CARTRIDGE_8K_CRT;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( GR.IO.File.ReadAllBytes( "Compare Files/Cartridge8kCrt.bin" ), assembly.Assembly );
    }



    [TestMethod]
    public void TestCartUltimax4KBin()
    {
      string      source = @"* = $8000
                              !byte 0,1,2,3,4,5,6,7";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.crt";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.CARTRIDGE_ULTIMAX_4K_BIN;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( GR.IO.File.ReadAllBytes( "Compare Files/Ultimax4kBin.bin" ), assembly.Assembly );
    }



    [TestMethod]
    public void TestCartUltimax4KCrt()
    {
      string      source = @"* = $8000
                              !byte 0,1,2,3,4,5,6,7";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.crt";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.CARTRIDGE_ULTIMAX_4K_CRT;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( GR.IO.File.ReadAllBytes( "Compare Files/Ultimax4kCrt.bin" ), assembly.Assembly );
    }



    [TestMethod]
    public void TestCartUltimax8KBin()
    {
      string      source = @"* = $8000
                              !byte 0,1,2,3,4,5,6,7";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.crt";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.CARTRIDGE_ULTIMAX_8K_BIN;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( GR.IO.File.ReadAllBytes( "Compare Files/Ultimax8kBin.bin" ), assembly.Assembly );
    }



    [TestMethod]
    public void TestCartUltimax8KCrt()
    {
      string      source = @"* = $8000
                              !byte 0,1,2,3,4,5,6,7";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.crt";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.CARTRIDGE_ULTIMAX_8K_CRT;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( GR.IO.File.ReadAllBytes( "Compare Files/Ultimax8kCrt.bin" ), assembly.Assembly );
    }



    [TestMethod]
    public void TestCartUltimax16KBin()
    {
      string      source = @"* = $8000
                              !byte 0,1,2,3,4,5,6,7";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.crt";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.CARTRIDGE_ULTIMAX_16K_BIN;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( GR.IO.File.ReadAllBytes( "Compare Files/Ultimax16kBin.bin" ), assembly.Assembly );
    }



    [TestMethod]
    public void TestCartUltimax16KCrt()
    {
      string      source = @"* = $8000
                              !byte 0,1,2,3,4,5,6,7";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.crt";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.CARTRIDGE_ULTIMAX_16K_CRT;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( GR.IO.File.ReadAllBytes( "Compare Files/Ultimax16kCrt.bin" ), assembly.Assembly );
    }



    [TestMethod]
    public void TestCartGMOD2Bin()
    {
      string      source = @"* = $8000
                              !byte 0,1,2,3,4,5,6,7";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.crt";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.CARTRIDGE_GMOD2_BIN;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( GR.IO.File.ReadAllBytes( "Compare Files/GMOD2Bin.bin" ), assembly.Assembly );
    }



    [TestMethod]
    public void TestCartGMOD2Crt()
    {
      string      source = @"* = $8000
                              !byte 0,1,2,3,4,5,6,7";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.crt";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.CARTRIDGE_GMOD2_CRT;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( GR.IO.File.ReadAllBytes( "Compare Files/GMOD2Crt.bin" ), assembly.Assembly );
    }



    [TestMethod]
    public void TestCartEasyflashBin()
    {
      string      source = @"* = $8000
                              !byte 0,1,2,3,4,5,6,7";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.crt";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.CARTRIDGE_EASYFLASH_BIN;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( GR.IO.File.ReadAllBytes( "Compare Files/easyflashBin.bin" ), assembly.Assembly );
    }



    [TestMethod]
    public void TestCartEasyflashCrt()
    {
      string      source = @"* = $8000
                              !byte 0,1,2,3,4,5,6,7";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.crt";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.CARTRIDGE_EASYFLASH_CRT;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( GR.IO.File.ReadAllBytes( "Compare Files/easyflashCrt.bin" ), assembly.Assembly );
    }



    [TestMethod]
    public void TestCartMagicDeskBin32k()
    {
      string      source = @"* = $8000
                              !byte 0,1,2,3,4,5,6,7";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.crt";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.CARTRIDGE_MAGICDESK_BIN_32K;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( GR.IO.File.ReadAllBytes( "Compare Files/MagicDesk32kBin.bin" ), assembly.Assembly );
    }



    [TestMethod]
    public void TestCartMagicDeskCrt32k()
    {
      string      source = @"* = $8000
                              !byte 0,1,2,3,4,5,6,7";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.crt";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.CARTRIDGE_MAGICDESK_CRT_32K;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( GR.IO.File.ReadAllBytes( "Compare Files/MagicDesk32kCrt.bin" ), assembly.Assembly );
    }



    [TestMethod]
    public void TestCartMagicDeskBin64k()
    {
      string      source = @"* = $8000
                              !byte 0,1,2,3,4,5,6,7";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.crt";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.CARTRIDGE_MAGICDESK_BIN_64K;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( GR.IO.File.ReadAllBytes( "Compare Files/MagicDesk64kBin.bin" ), assembly.Assembly );
    }



    [TestMethod]
    public void TestCartMagicDeskCrt64k()
    {
      string      source = @"* = $8000
                              !byte 0,1,2,3,4,5,6,7";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.crt";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.CARTRIDGE_MAGICDESK_CRT_64K;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( GR.IO.File.ReadAllBytes( "Compare Files/MagicDesk64kCrt.bin" ), assembly.Assembly );
    }



    [TestMethod]
    public void TestCartMagicDeskBin128k()
    {
      string      source = @"* = $8000
                              !byte 0,1,2,3,4,5,6,7";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.crt";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.CARTRIDGE_MAGICDESK_BIN_128K;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( GR.IO.File.ReadAllBytes( "Compare Files/MagicDesk128kBin.bin" ), assembly.Assembly );
    }



    [TestMethod]
    public void TestCartMagicDeskCrt128k()
    {
      string      source = @"* = $8000
                              !byte 0,1,2,3,4,5,6,7";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.crt";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.CARTRIDGE_MAGICDESK_CRT_128K;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( GR.IO.File.ReadAllBytes( "Compare Files/MagicDesk128kCrt.bin" ), assembly.Assembly );
    }



    [TestMethod]
    public void TestCartMagicDeskBin256k()
    {
      string      source = @"* = $8000
                              !byte 0,1,2,3,4,5,6,7";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.crt";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.CARTRIDGE_MAGICDESK_BIN_256K;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( GR.IO.File.ReadAllBytes( "Compare Files/MagicDesk256kBin.bin" ), assembly.Assembly );
    }



    [TestMethod]
    public void TestCartMagicDeskCrt256k()
    {
      string      source = @"* = $8000
                              !byte 0,1,2,3,4,5,6,7";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.crt";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.CARTRIDGE_MAGICDESK_CRT_256K;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( GR.IO.File.ReadAllBytes( "Compare Files/MagicDesk256kCrt.bin" ), assembly.Assembly );
    }



    [TestMethod]
    public void TestCartMagicDeskBin512k()
    {
      string      source = @"* = $8000
                              !byte 0,1,2,3,4,5,6,7";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.crt";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.CARTRIDGE_MAGICDESK_BIN_512K;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( GR.IO.File.ReadAllBytes( "Compare Files/MagicDesk512kBin.bin" ), assembly.Assembly );
    }



    [TestMethod]
    public void TestCartMagicDeskCrt512k()
    {
      string      source = @"* = $8000
                              !byte 0,1,2,3,4,5,6,7";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.crt";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.CARTRIDGE_MAGICDESK_CRT_512K;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( GR.IO.File.ReadAllBytes( "Compare Files/MagicDesk512kCrt.bin" ), assembly.Assembly );
    }



    [TestMethod]
    public void TestCartMagicDeskBin1M()
    {
      string      source = @"* = $8000
                              !byte 0,1,2,3,4,5,6,7";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.crt";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.CARTRIDGE_MAGICDESK_BIN_1M;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( GR.IO.File.ReadAllBytes( "Compare Files/MagicDeskBin1MB.bin" ), assembly.Assembly );
    }



    [TestMethod]
    public void TestCartMagicDeskCrt1M()
    {
      string      source = @"* = $8000
                              !byte 0,1,2,3,4,5,6,7";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.crt";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.CARTRIDGE_MAGICDESK_CRT_1M;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( GR.IO.File.ReadAllBytes( "Compare Files/MagicDeskCrt1MB.bin" ), assembly.Assembly );
    }



    [TestMethod]
    public void TestCartRGCDBin()
    {
      string      source = @"* = $8000
                              !byte 0,1,2,3,4,5,6,7";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.crt";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.CARTRIDGE_RGCD_BIN;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( GR.IO.File.ReadAllBytes( "Compare Files/RGCDBin.bin" ), assembly.Assembly );
    }



    [TestMethod]
    public void TestCartRGCDCrt()
    {
      string      source = @"* = $8000
                              !byte 0,1,2,3,4,5,6,7";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.crt";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.CARTRIDGE_RGCD_CRT;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( GR.IO.File.ReadAllBytes( "Compare Files/RGCDCrt.bin" ), assembly.Assembly );
    }

    
  }
}
