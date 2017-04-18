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
    }



    void WriteHelp()
    {
      System.Console.WriteLine( "Call with C64Ass [options] [file]" );
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
