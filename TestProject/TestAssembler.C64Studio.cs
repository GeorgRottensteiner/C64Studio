using System;
using System.Diagnostics;
using System.Linq;
using LibGit2Sharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RetroDevStudio.Types;

namespace TestProject
{
  public class TestAssemblerC64Studio
  {
    public static GR.Memory.ByteBuffer TestAssembleC64Studio( string Source, out RetroDevStudio.Types.ASM.FileInfo Info )
    {
      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      bool parseResult = parser.Parse( Source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo );
      if ( !parseResult )
      {
        Debug.Log( "Testassemble failed:" );
        foreach ( var msg in asmFileInfo.Messages )
        {
          Debug.Log( msg.Value.Message + " in line " + asmFileInfo.LineInfo[msg.Key].Line );
        }
      }


      Assert.IsTrue( parseResult );
      Assert.IsTrue( parser.Assemble( config ) );

      Info = asmFileInfo;

      return parser.AssembledOutput.Assembly;
    }



    public static GR.Memory.ByteBuffer TestAssembleC64Studio( string Source, out GR.Collections.MultiMap<int, RetroDevStudio.Parser.ParserBase.ParseMessage> Messages )
    {
      return TestAssembleC64Studio( Source, out Messages, out RetroDevStudio.Types.ASM.FileInfo info );
    }



    public static GR.Memory.ByteBuffer TestAssembleC64Studio( string Source, out GR.Collections.MultiMap<int, RetroDevStudio.Parser.ParserBase.ParseMessage> Messages, out RetroDevStudio.Types.ASM.FileInfo Info )
    {
      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      bool parseResult = parser.Parse( Source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo );
      Messages = asmFileInfo.Messages;
      if ( !parseResult )
      {
        Debug.Log( "Testassemble failed:" );
        foreach ( var msg in asmFileInfo.Messages.Values )
        {
          Debug.Log( msg.Message );
        }
      }


      Assert.IsTrue( parseResult );
      Assert.IsTrue( parser.Assemble( config ) );

      Info = asmFileInfo;

      return parser.AssembledOutput.Assembly;
    }



    public static GR.Memory.ByteBuffer TestAssembleC64Studio( string Source )
    {
      return TestAssembleC64Studio( Source, out RetroDevStudio.Types.ASM.FileInfo Info );
    }



  }
}
