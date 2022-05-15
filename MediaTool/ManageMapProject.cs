using GR.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaTool
{
  public partial class Manager
  {
    private int HandleMapProject( GR.Text.ArgumentParser ArgParser )
    {
      if ( !ValidateExportType( "map project file", ArgParser.Parameter( "TYPE" ), new string[] { "MAPDATA", "MAPDATAASM" } ) )
      {
        return 1;
      }

      GR.Memory.ByteBuffer    data = GR.IO.File.ReadAllBytes( ArgParser.Parameter( "MAPPROJECT" ) );
      if ( data == null )
      {
        System.Console.WriteLine( "Couldn't read binary char file " + ArgParser.Parameter( "MAPPROJECT" ) );
        return 1;
      }

      var mapProject = new RetroDevStudio.Formats.MapProject();
      if ( !mapProject.ReadFromBuffer( data ) )
      {
        System.Console.WriteLine( "Couldn't read map project from file " + ArgParser.Parameter( "MAPPROJECT" ) );
        return 1;
      }

      GR.Memory.ByteBuffer    resultingData = new GR.Memory.ByteBuffer();

      if ( ArgParser.Parameter( "TYPE" ).Contains( "MAPDATAASM" ) )
      {
        foreach ( var map in mapProject.Maps )
        {
          var     mapData = new ByteBuffer( (uint)( map.Tiles.Width * map.Tiles.Height ) );

          for ( int j = 0; j < 0 + map.Tiles.Height; ++j )
          {
            for ( int i = 0; i < 0 + map.Tiles.Width; ++i )
            {
              mapData.SetU8At( i + j * map.Tiles.Width, (byte)map.Tiles[i, j] );
            }
          }
          resultingData.Append( Encoding.ASCII.GetBytes( "MAP_" + map.Name + "\n!hex \"" + mapData.ToString() + "\"\n" ) );
        }
      }
      else if ( ArgParser.Parameter( "TYPE" ).Contains( "MAPDATA" ) )
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
