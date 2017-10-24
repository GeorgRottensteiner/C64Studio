using System;
using System.Collections.Generic;
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
      _Args.AddParameter( "-FORMAT", true );
      _Args.AddParameter( "F", true );
      _Args.AddParameter( "-SETPC", true );
      _Args.AddSwitch( "-AUTOTRUNCATELITERALS", true );
    }



    void WriteHelp()
    {
      System.Console.WriteLine( "Call with C64Ass [options] [file]" );
      System.Console.WriteLine();
      System.Console.WriteLine( "-H, --HELP                 - Display this help" );
      System.Console.WriteLine( "-O, --OUTFILE [Filename]   - Determine output file name" );
      System.Console.WriteLine( "-L, --LABELDUMP [Filename] - Write a label file" );
      System.Console.WriteLine( "-F, --FORMAT [PLAIN/CBM]   - Sets the output file format" );
      System.Console.WriteLine( "                             PLAIN is a raw binary, CBM a .prg file" );
      //System.Console.WriteLine( "--SETPC                    - Forces " );
      System.Console.WriteLine( "-AUTOTRUNCATELITERALS      - Clamps literal values to bytes/words without error" );
    }



    public C64Studio.Parser.CompileConfig CheckParams( string[] Args )
    {
      C64Studio.Parser.CompileConfig config = null;

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

      // last arg is filename?
      if ( ( Args[Args.Length - 1].StartsWith( "-" ) )
      ||   ( Args[Args.Length - 1].StartsWith( "/" ) ) )
      {
        WriteHelp();
        return config;
      }

      config = new C64Studio.Parser.CompileConfig();
      config.InputFile = Args[Args.Length - 1];

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
      config.AutoTruncateLiteralValues = _Args.IsParameterSet( "-AUTOTRUNCATELITERALS" );

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

  }
}
