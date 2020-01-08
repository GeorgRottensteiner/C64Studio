using System;
using System.Collections.Generic;
using System.Text;

namespace MediaTool
{
  public partial class Manager
  {
    private int HandleMapProject( GR.Text.ArgumentParser ArgParser )
    {
      if ( !ValidateExportType( "map project file", ArgParser.Parameter( "TYPE" ), new string[] { "MAPDATA" } ) )
      {
        return 1;
      }

      GR.Memory.ByteBuffer    data = GR.IO.File.ReadAllBytes( ArgParser.Parameter( "MAPPROJECT" ) );
      if ( data == null )
      {
        System.Console.WriteLine( "Couldn't read binary char file " + ArgParser.Parameter( "MAPPROJECT" ) );
        return 1;
      }

      var mapProject = new C64Studio.Formats.MapProject();
      if ( !mapProject.ReadFromBuffer( data ) )
      {
        System.Console.WriteLine( "Couldn't read map project from file " + ArgParser.Parameter( "MAPPROJECT" ) );
        return 1;
      }

      GR.Memory.ByteBuffer    resultingData = new GR.Memory.ByteBuffer();

      if ( ArgParser.Parameter( "TYPE" ).Contains( "MAPDATA" ) )
      {
        foreach ( var map in mapProject.Maps )
        {
          for ( int j = 0; j < 0 + map.Tiles.Height; ++j )
          {
            for ( int i = 0; i < 0 + map.Tiles.Width; ++i )
            {
              resultingData.AppendU8( (byte)map.Tiles[i,j] );
            }
          }
        }
      }
      if ( !GR.IO.File.WriteAllBytes( ArgParser.Parameter( "EXPORT" ), resultingData ) )
      {
        Console.WriteLine( "Could not write to file " + ArgParser.Parameter( "EXPORT" ) );
        return 1;
      }
      return 0;
    }

  }
}
