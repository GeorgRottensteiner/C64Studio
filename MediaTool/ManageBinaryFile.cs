using System;
using System.Collections.Generic;
using System.Text;

namespace MediaTool
{
  public partial class Manager
  {
    private int HandleBinaryFile( GR.Text.ArgumentParser ArgParser )
    {
      if ( !ValidateExportType( "Binary file", ArgParser.Parameter( "TYPE" ), new string[] { "BYTES" } ) )
      {
        return 1;
      }

      GR.Memory.ByteBuffer    data = GR.IO.File.ReadAllBytes( ArgParser.Parameter( "BINARY" ) );
      if ( data == null )
      {
        System.Console.WriteLine( "Couldn't read binary file " + ArgParser.Parameter( "BINARY" ) );
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
        count = (int)data.Length;
      }

      if ( ( firstUnit < 0 )
      ||   ( firstUnit >= (int)data.Length ) )
      {
        System.Console.WriteLine( "OFFSET is invalid" );
        return 1;
      }
      if ( ( count <= 0 )
      ||   ( firstUnit + count > (int)data.Length ) )
      {
        System.Console.WriteLine( "COUNT is invalid" );
        return 1;
      }

      GR.Memory.ByteBuffer    resultData = new GR.Memory.ByteBuffer( (uint)count );
      data.CopyTo( resultData, firstUnit, count );

      if ( !GR.IO.File.WriteAllBytes( ArgParser.Parameter( "EXPORT" ), resultData ) )
      {
        Console.WriteLine( "Could not write to file " + ArgParser.Parameter( "EXPORT" ) );
        return 1;
      }
      return 0;
    }

  }
}
