using LibGit2Sharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RetroDevStudio.Types;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace TestProject
{
  public class TestAssemblerBase
  {
    public static GR.Memory.ByteBuffer TestAssemble( AssemblerType assemblerType, string Source, out RetroDevStudio.Types.ASM.FileInfo Info )
    {
      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( assemblerType );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = CompileTargetType.NONE;
      config.Assembler = assemblerType;

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



    public static GR.Memory.ByteBuffer TestAssemble( AssemblerType assemblerType, string Source, out GR.Collections.MultiMap<int, RetroDevStudio.Parser.ParserBase.ParseMessage> Messages )
    {
      return TestAssemble( assemblerType, Source, CompileTargetType.PRG, null, out Messages, out RetroDevStudio.Types.ASM.FileInfo info );
    }



    public static GR.Memory.ByteBuffer TestAssemble( AssemblerType assemblerType, string Source, CompileTargetType TargetType, RetroDevStudio.Parser.CompileConfig config, out GR.Collections.MultiMap<int, RetroDevStudio.Parser.ParserBase.ParseMessage> Messages, out RetroDevStudio.Types.ASM.FileInfo Info )
    {
      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( assemblerType );

      if ( config == null )
      {
        config = new RetroDevStudio.Parser.CompileConfig();
      }
      config.OutputFile = "test.prg";
      config.TargetType = TargetType;
      config.Assembler = assemblerType;

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



    public static GR.Memory.ByteBuffer TestAssemble( AssemblerType assemblerType, string Source )
    {
      return TestAssemble( assemblerType, Source, out GR.Collections.MultiMap<int, RetroDevStudio.Parser.ParserBase.ParseMessage> Messages );
    }



    public static string GetProjectDirectory()
    {
      // Start from the output directory (where the test assembly runs)  
      string currentDir = AppContext.BaseDirectory;

      // Search for the marker file to find the project root  
      while ( currentDir != null )
      {
        string markerPath = Path.Combine( currentDir, "System Tests");
        if ( Directory.Exists( markerPath ) )
        {
          return markerPath;
        }
        currentDir = Directory.GetParent( currentDir )?.FullName;
      }
      throw new FileNotFoundException( "Marker file not found. Ensure sub folder 'System Tests' exists in the project directory." );
    }



    public static GR.Memory.ByteBuffer AssembleFromFile( AssemblerType assemblerType, string filename, out GR.Collections.MultiMap<int, RetroDevStudio.Parser.ParserBase.ParseMessage> messages, out RetroDevStudio.Types.ASM.FileInfo info )
    {
      filename = Path.Combine( GetProjectDirectory(), filename );
      var config = new RetroDevStudio.Parser.CompileConfig()
      {
        InputFile = filename
      };
      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( assemblerType );

      config.OutputFile = "test.prg";
      config.TargetType = CompileTargetType.PLAIN;
      config.Assembler  = assemblerType;
      config.LibraryFiles.Add( GetProjectDirectory() );

      var source = GR.IO.File.ReadAllText( config.InputFile );

      bool parseResult= parser.ParseFile( config.InputFile, source, null, config, null, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo );
      messages = asmFileInfo.Messages;
      if ( !parseResult )
      {
        Debug.Log( "Testassemble failed:" );
        foreach ( var msg in asmFileInfo.Messages )
        {
          asmFileInfo.FindTrueLineSource( msg.Key, out var documentFile, out var documentLine );

          Debug.Log( $"  {documentFile}({documentLine}): {msg.Value.Message}" );
        }
      }


      Assert.IsTrue( parseResult );
      Assert.IsTrue( parser.Assemble( config ) );

      info = asmFileInfo;

      return parser.AssembledOutput.Assembly;
    }



  }
}
