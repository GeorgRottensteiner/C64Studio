using System;
using System.Collections.Generic;
using System.Text;

namespace MediaTool
{
  public partial class Manager
  {
    private int HandleCharsetProject( GR.Text.ArgumentParser ArgParser )
    {
      if ( !ValidateExportType( "charset project", ArgParser.Parameter( "TYPE" ), new string[] { "CHARS" } ) )
      {
        return 1;
      }

      var charsetProject = new C64Studio.Formats.CharsetProject();

      if ( !charsetProject.ReadFromBuffer( GR.IO.File.ReadAllBytes( ArgParser.Parameter( "CHARSETPROJECT" ) ) ) )
      {
        System.Console.WriteLine( "Couldn't read charset project from file " + ArgParser.Parameter( "CHARSETPROJECT" ) );
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
        count = charsetProject.Characters.Count;
      }

      if ( ( firstUnit < 0 )
      ||   ( firstUnit >= charsetProject.Characters.Count ) )
      {
        System.Console.WriteLine( "OFFSET is invalid" );
        return 1;
      }
      if ( ( count <= 0 )
      ||   ( firstUnit + count > charsetProject.Characters.Count ) )
      {
        System.Console.WriteLine( "COUNT is invalid" );
        return 1;
      }

      GR.Memory.ByteBuffer    exportData = new GR.Memory.ByteBuffer( (uint)( count * 8 ) );
      for ( int i = 0; i < count; ++i )
      {
        charsetProject.Characters[firstUnit + i].Tile.Data.CopyTo( exportData, 0, 8, i * 8 );
      }

      if ( !GR.IO.File.WriteAllBytes( ArgParser.Parameter( "EXPORT" ), exportData ) )
      {
        Console.WriteLine( "Could not write to file " + ArgParser.Parameter( "EXPORT" ) );
        return 1;
      }
      return 0;
    }

  }
}
