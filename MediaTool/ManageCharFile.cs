using System;
using System.Collections.Generic;
using System.Text;

namespace MediaTool
{
  public partial class Manager
  {
    private int HandleCharFile( GR.Text.ArgumentParser ArgParser )
    {
      if ( !ValidateExportType( "charset file", ArgParser.Parameter( "TYPE" ), new string[] { "CHARS" } ) )
      {
        return 1;
      }

      GR.Memory.ByteBuffer    data = GR.IO.File.ReadAllBytes( ArgParser.Parameter( "CHARS" ) );
      if ( data == null )
      {
        System.Console.WriteLine( "Couldn't read binary char file " + ArgParser.Parameter( "CHARS" ) );
        return 1;
      }
      int     firstUnit = 0;
      int     count = -1;
      if ( ArgParser.IsParameterSet( "OFFSET" ) )
      {
        firstUnit = GR.Convert.ToI32( ArgParser.Parameter( "OFFSET" ) );
      }
      if ( ArgParser.IsParameterSet( "COUNT" ) )
      {
        count = GR.Convert.ToI32( ArgParser.Parameter( "COUNT" ) );
      }
      if ( count == -1 )
      {
        count = (int)data.Length / 8;
      }

      if ( ( firstUnit < 0 )
      ||   ( firstUnit >= (int)data.Length / 8 ) )
      {
        System.Console.WriteLine( "OFFSET is invalid" );
        return 1;
      }
      if ( ( count <= 0 )
      || ( firstUnit + count > (int)data.Length / 8 ) )
      {
        System.Console.WriteLine( "COUNT is invalid" );
        return 1;
      }

      GR.Memory.ByteBuffer    spriteData = new GR.Memory.ByteBuffer( (uint)( count * 8 ) );
      data.CopyTo( spriteData, firstUnit * 8, count * 8 );

      if ( !GR.IO.File.WriteAllBytes( ArgParser.Parameter( "EXPORT" ), spriteData ) )
      {
        Console.WriteLine( "Could not write to file " + ArgParser.Parameter( "EXPORT" ) );
        return 1;
      }
      return 0;
    }

  }
}
