using C64Studio;
using C64Studio.Parser;
using System;
using System.Collections.Generic;
using System.Text;



namespace C64Ass
{
  class Program
  {
    static int Main( string[] args )
    {
      var configReader = new ArgumentEvaluator();
      var config = configReader.CheckParams( args );
      if ( config == null )
      {
        return 1;
      }
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;

      var parser = new ASMFileParser();

      var docInfo = new DocumentInfo() 
        { 
          Type = ProjectElement.ElementType.ASM_SOURCE 
        };

      var projectConfig = new ProjectConfig();
      // TODO - add defines if given

      docInfo.DocumentFilename = System.IO.Path.GetFullPath( config.InputFile );

      bool result = parser.ParseFile( docInfo, projectConfig, config );
      if ( !result )
      {
        System.Console.WriteLine( "Parsing the file failed" );
        return 1;
      }

      // default to plain
      C64Studio.Types.CompileTargetType compileTargetType = C64Studio.Types.CompileTargetType.PLAIN;
      // command line given target type overrides everything
      if ( config.TargetType != C64Studio.Types.CompileTargetType.NONE )
      {
        compileTargetType = config.TargetType;
      }
      else if ( parser.CompileTarget != C64Studio.Types.CompileTargetType.NONE )
      {
        compileTargetType = parser.CompileTarget;
      }
      config.TargetType = compileTargetType;

      if ( !parser.Assemble( config ) )
      {
        System.Console.WriteLine( "Assembling the output failed" );
        return 1;
      }

      if ( !GR.IO.File.WriteAllBytes( config.OutputFile, parser.Assembly ) )
      {
        System.Console.WriteLine( "Failed to write output file" );
        return 1;
      }
      return 0;
    }
  }
}
