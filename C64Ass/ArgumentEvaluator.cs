using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace C64Ass
{
  class ArgumentEvaluator
  {
    GR.Text.ArgumentParser      _Args = new GR.Text.ArgumentParser();

    public ArgumentEvaluator()
    {
      // [options] [file]
      _Args.AddSwitch( "H", true );
      _Args.AddSwitch( "-HELP", true );
      _Args.AddParameter( "O", true );
      _Args.AddParameter( "-OUTFILE", true );
      _Args.AddParameter( "L", true );
      _Args.AddParameter( "-LABELDUMP", true );
      _Args.AddParameter( "LIB", true );
      _Args.AddParameter( "-LIBRARY", true );
      _Args.AddParameter( "-FORMAT", true );
      _Args.AddParameter( "F", true );
      _Args.AddParameter( "-SETPC", true );
      _Args.AddSwitch( "N", true );
      _Args.AddSwitch( "-NOWARNINGS", true );
      _Args.AddSwitch( "-AUTOTRUNCATELITERALS", true );
      _Args.AddParameter( "I", true );
      _Args.AddParameter( "-IGNORE", true );
      _Args.AddSwitch( "V", true );
      _Args.AddSwitch( "-VERSION", true );
    }



    void WriteHelp()
    {
      System.Console.WriteLine( "Call with C64Ass [options] [file]" );
      System.Console.WriteLine();
      System.Console.WriteLine( "-H, --HELP                     - Display this help" );
      System.Console.WriteLine( "-O, --OUTFILE [Filename]       - Determine output file name" );
      System.Console.WriteLine( "-L, --LABELDUMP [Filename]     - Write a label file" );
      System.Console.WriteLine( "-LIB, --LIBRARY [Library Path] - Add path(s) to library paths" );
      System.Console.WriteLine( "                                 Multiple paths are separated by comma" );
      System.Console.WriteLine( "-F, --FORMAT [PLAIN/CBM]       - Sets the output file format" );
      System.Console.WriteLine( "                                 PLAIN is a raw binary, CBM a .prg file" );
      //System.Console.WriteLine( "--SETPC                    - Forces " );
      System.Console.WriteLine( "-AUTOTRUNCATELITERALS          - Clamps literal values to bytes/words without error" );
      System.Console.WriteLine( "-D, --DEFINE [Key=Value]       - Add Predefines" );
      System.Console.WriteLine( "-N, --NOWARNINGS               - Show no warnings" );
      System.Console.WriteLine( "-I, --IGNORE [WarningNo]       - Ignore specific Warnings" );
      System.Console.WriteLine( "                                 Multiple warnings separated by comma" );
      System.Console.WriteLine( "-V, --VERSION                  - Display version" );
    }



    public C64Studio.Parser.CompileConfig CheckParams( string[] Args, out string AdditionalDefines, out bool ShowNoWarnings, out List<string> WarningsToIgnore )
    {
      C64Studio.Parser.CompileConfig config = null;
      AdditionalDefines = null;
      ShowNoWarnings = false;
      WarningsToIgnore = new List<string>();

      if ( Args.Length == 0 )
      {
        WriteHelp();
        return config;
      }

      if ( !_Args.CheckParameters( Args ) )
      {
        WriteHelp();
        return config;
      }

      if ( ( _Args.IsParameterSet( "V" ) )
      ||   ( _Args.IsParameterSet( "-VERSION" ) ) )
      {
        Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

        Console.WriteLine( $"C64Ass Assembler V{version}" ); 
        return null;
      }

      // last arg is filename?
      if ( _Args.UnknownArgumentCount() < 1 )
      {
        // last arg should be file name!
        WriteHelp();
        return config;
      }

      config = new C64Studio.Parser.CompileConfig();
      config.InputFile = _Args.UnknownArgument( _Args.UnknownArgumentCount() - 1 );

      if ( _Args.IsParameterSet( "O" ) )
      {
        config.OutputFile = _Args.Parameter( "O" );
      }
      if ( _Args.IsParameterSet( "OUTFILE" ) )
      {
        config.OutputFile = _Args.Parameter( "OUTFILE" );
      }
      if ( _Args.IsParameterSet( "L" ) )
      {
        config.LabelDumpFile = _Args.Parameter( "L" );
      }
      if ( _Args.IsParameterSet( "-LABELDUMP" ) )
      {
        config.LabelDumpFile = _Args.Parameter( "LABELDUMP" );
      }
      if ( _Args.IsParameterSet( "LIB" ) )
      {
        config.LibraryFiles = _Args.Parameter( "LIB" ).Split( ',' ).ToList();
      }
      if ( _Args.IsParameterSet( "LIBRARY" ) )
      {
        config.LibraryFiles = _Args.Parameter( "LIBRARY" ).Split( ',' ).ToList();
      }
      if ( _Args.IsParameterSet( "N" ) )
      {
        ShowNoWarnings = true;
      }
      if ( _Args.IsParameterSet( "-NOWARNINGS" ) )
      {
        ShowNoWarnings = true;
      }
      if ( _Args.IsParameterSet( "I" ) )
      {
        ParseWarnings( WarningsToIgnore, _Args.Parameter( "I" ) );
      }
      if ( _Args.IsParameterSet( "-IGNORE" ) )
      {
        ParseWarnings( WarningsToIgnore, _Args.Parameter( "-IGNORE" ) );
      }
      config.AutoTruncateLiteralValues = _Args.IsParameterSet( "-AUTOTRUNCATELITERALS" );

      for ( int i = 0; i < _Args.UnknownArgumentCount() - 1; ++i )
      {
        var curArg = _Args.UnknownArgument( i ).ToUpper();
        if ( ( curArg == "-D" )
        ||   ( curArg == "--DEFINE" ) )
        {
          if ( i + 1 >= _Args.UnknownArgumentCount() - 1 )
          {
            // no argument for define
            WriteHelp();
            return null;
          }
          AdditionalDefines += _Args.UnknownArgument( i + 1 ) + "\r\n";
          ++i;
        }
        else
        {
          Console.WriteLine( "Unsupported argument: " + _Args.UnknownArgument( i ) );
          WriteHelp();
          return null;
        }
      }

      if ( ( _Args.IsParameterSet( "F" ) )
      ||   ( _Args.IsParameterSet( "-FORMAT" ) ) )
      {
        string    outputFormat = _Args.IsParameterSet( "F" ) ? _Args.Parameter( "F" ) : _Args.Parameter( "-FORMAT" );

        if ( string.Compare( outputFormat, "PLAIN", true ) == 0 )
        {
          config.TargetType = C64Studio.Types.CompileTargetType.PLAIN;
        }
        else if ( string.Compare( outputFormat, "CBM", true ) == 0 )
        {
          config.TargetType = C64Studio.Types.CompileTargetType.PRG;
        }
        else if ( string.Compare( outputFormat, "D64", true ) == 0 )
        {
          config.TargetType = C64Studio.Types.CompileTargetType.D64;
        }
      }

      if ( _Args.IsParameterSet( "-SETPC" ) )
      {
        int     startPC = GR.Convert.ToI32( _Args.Parameter( "-SETPC" ) );
        if ( ( startPC >= 0 )
        &&   ( startPC < 65536 ) )
        {
          config.StartAddress = startPC;
        }
      }
      return config;
    }



    private void ParseWarnings( List<string> WarningsToIgnore, string Args )
    {
      string[]  parts = Args.Split( ',' );
      for ( int i = 0; i < parts.Length; ++i )
      {
        WarningsToIgnore.Add( parts[i].ToUpper() );
      }
    }



  }
}
