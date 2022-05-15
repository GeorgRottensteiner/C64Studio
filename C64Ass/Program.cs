using RetroDevStudio;
using RetroDevStudio.Parser;
using RetroDevStudio.Types.ASM;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;



namespace C64Ass
{
  class Program
  {

    static int Main( string[] args )
    {
      var configReader = new ArgumentEvaluator();
      var config = configReader.CheckParams( args, out string additionalDefines, out bool showNoWarnings, out List<string> WarningsToIgnore );
      if ( config == null )
      {
        return 1;
      }
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      var parser = new ASMFileParser();

      var projectConfig = new ProjectConfig();
      // TODO - add defines if given

      string  fullPath = System.IO.Path.GetFullPath( config.InputFile );

      if ( string.IsNullOrEmpty( config.OutputFile ) )
      {
        // provide a default
        config.OutputFile = GR.Path.RenameExtension( config.InputFile, ".prg" );
        config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      }

      bool result = parser.ParseFile( fullPath, "", projectConfig, config, additionalDefines );
      if ( !result )
      {
        System.Console.WriteLine( "Parsing the file failed:" );

        DisplayOutput( parser, WarningsToIgnore, showNoWarnings );
        return 1;
      }

      // default to plain
      RetroDevStudio.Types.CompileTargetType compileTargetType = RetroDevStudio.Types.CompileTargetType.PLAIN;
      // command line given target type overrides everything
      if ( config.TargetType != RetroDevStudio.Types.CompileTargetType.NONE )
      {
        compileTargetType = config.TargetType;
      }
      else if ( parser.CompileTarget != RetroDevStudio.Types.CompileTargetType.NONE )
      {
        compileTargetType = parser.CompileTarget;
      }
      config.TargetType = compileTargetType;

      if ( !parser.Assemble( config ) )
      {
        System.Console.WriteLine( "Assembling the output failed" );
        DisplayOutput( parser, WarningsToIgnore, showNoWarnings );
        return 1;
      }
      DisplayOutput( parser, WarningsToIgnore, showNoWarnings );
      if ( !GR.IO.File.WriteAllBytes( config.OutputFile, parser.AssembledOutput.Assembly ) )
      {
        System.Console.WriteLine( "Failed to write output file" );
        return 1;
      }
      if ( !string.IsNullOrEmpty( config.LabelDumpFile ) )
      {
        DumpLabelFile( parser.ASMFileInfo );
      }

      return 0;
    }



    private static void DumpLabelFile( FileInfo FileInfo )
    {
      StringBuilder sb = new StringBuilder();

      foreach ( var labelInfo in FileInfo.Labels )
      {
        sb.Append( labelInfo.Value.Name );
        sb.Append( " =$" );
        if ( labelInfo.Value.AddressOrValue > 255 )
        {
          sb.Append( labelInfo.Value.AddressOrValue.ToString( "X4" ) );
        }
        else
        {
          sb.Append( labelInfo.Value.AddressOrValue.ToString( "X2" ) );
        }
        sb.Append( "; " );
        if ( labelInfo.Value.References.Count == 0 )
        {
          sb.AppendLine( "unused" );
        }
        else
        {
          sb.AppendLine();
        }
      }
      GR.IO.File.WriteAllText( FileInfo.LabelDumpFile, sb.ToString() );
    }



    private static void DisplayOutput( ASMFileParser Parser, List<string> WarningsToIgnore, bool ShowNoWarnings )
    {
      foreach ( var entry in Parser.Messages )
      {
        string    file;
        int       lineIndex;

        if ( ( ShowNoWarnings )
        &&   ( entry.Value.Type == ParserBase.ParseMessage.LineType.WARNING ) )
        {
          continue;
        }
        string  warningCode = entry.Value.Code.ToString();
        if ( warningCode.Length >= 5 )
        {
          warningCode = warningCode.Substring( 0, 5 );
          if ( WarningsToIgnore.Contains( warningCode ) )
          {
            continue;
          }
        }

        if ( Parser.ASMFileInfo.FindTrueLineSource( entry.Key, out file, out lineIndex ) )
        {
          System.Console.WriteLine( file + "(" + lineIndex + "): " + entry.Value.Code.ToString() + " - " + entry.Value.Message );
        }
        else
        {
          System.Console.WriteLine( entry.Value.Code.ToString() + " - " + entry.Value.Message );
        }
      }
    }
  }
}
