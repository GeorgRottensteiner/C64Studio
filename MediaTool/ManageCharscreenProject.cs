using System;
using System.Collections.Generic;
using System.Text;

namespace MediaTool
{
  public partial class Manager
  {
    private int HandleCharscreenFile( GR.Text.ArgumentParser ArgParser )
    {
      if ( !ValidateExportType( "charscreen file", ArgParser.Parameter( "TYPE" ), new string[] { "CHARS", "CHARSCOLORS", "COLORS", "CHARSET" } ) )
      {
        return 1;
      }

      GR.Memory.ByteBuffer    data = GR.IO.File.ReadAllBytes( ArgParser.Parameter( "CHARSCREEN" ) );
      if ( data == null )
      {
        System.Console.WriteLine( "Couldn't read binary char file " + ArgParser.Parameter( "CHARSCREEN" ) );
        return 1;
      }

      var charScreenProject = new RetroDevStudio.Formats.CharsetScreenProject();
      if ( !charScreenProject.ReadFromBuffer( data ) )
      {
        System.Console.WriteLine( "Couldn't read charscreen project from file " + ArgParser.Parameter( "CHARSCREEN" ) );
        return 1;
      }

      int     firstScreen = 0;
      int     count = -1;
      if ( ArgParser.IsParameterSet( "OFFSET" ) )
      {
        firstScreen = GR.Convert.ToI32( ArgParser.Parameter( "OFFSET" ) );
        if ( ( firstScreen < 0 )
        ||   ( firstScreen >= charScreenProject.Screens.Count ) )
        {
          System.Console.WriteLine( "OFFSET is out of bounds" );
          return 1;
        }
      }
      if ( ArgParser.IsParameterSet( "COUNT" ) )
      {
        count = GR.Convert.ToI32( ArgParser.Parameter( "COUNT" ) );
      }
      if ( count == -1 )
      {
        count = charScreenProject.Screens.Count;
      }
      if ( ( count < 1 )
      ||   ( firstScreen + count >= charScreenProject.Screens.Count ) )
      {
        System.Console.WriteLine( "COUNT is out of bounds" );
        return 1;
      }

      var resultingData = new GR.Memory.ByteBuffer();

      for ( int s = 0; s < count; ++s )
      {
        if ( ( firstScreen + s < 0 )
        ||   ( firstScreen + s >= charScreenProject.Screens.Count ) )
        {
          System.Console.WriteLine( "OFFSET and COUNT are out of bounds" );
          return 1;
        }
        var     screen = charScreenProject.Screens[firstScreen + s];
        int     x = 0;
        int     y = 0;
        int     width = -1;
        int     height = -1;
        if ( ArgParser.IsParameterSet( "AREA" ) )
        {
          string      rangeInfo = ArgParser.Parameter( "AREA" );
          string[]    rangeParts = rangeInfo.Split( ',' );
          if ( rangeParts.Length != 4 )
          {
            System.Console.WriteLine( "AREA is invalid, expected four values separated by comma: x,y,width,height" );
            return 1;
          }
          x       = GR.Convert.ToI32( rangeParts[0] );
          y       = GR.Convert.ToI32( rangeParts[1] );
          width   = GR.Convert.ToI32( rangeParts[2] );
          height  = GR.Convert.ToI32( rangeParts[3] );

          if ( ( width <= 0 )
          ||   ( height <= 0 )
          ||   ( x < 0 )
          ||   ( y < 0 )
          ||   ( x + width > screen.Width )
          ||   ( y + height > screen.Height ) )
          {
            System.Console.WriteLine( $"AREA values are out of bounds or invalid for screen {screen.Name}, expected four values separated by comma: x,y,width,height" );
            return 1;
          }
        }
        else
        {
          width = screen.Width;
          height = screen.Height;
        }

        GR.Memory.ByteBuffer    resultingDataPerScreen = new GR.Memory.ByteBuffer();

        if ( ( ArgParser.Parameter( "TYPE" ) == "CHARS" )
        ||   ( ArgParser.Parameter( "TYPE" ) == "CHARSCOLORS" ) )
        {
          for ( int j = y; j < y + height; ++j )
          {
            for ( int i = x; i < x + width; ++i )
            {
              resultingDataPerScreen.AppendU8( (byte)screen.CharacterAt( i, j ) );
            }
          }
        }
        if ( ( ArgParser.Parameter( "TYPE" ) == "COLORS" )
        ||   ( ArgParser.Parameter( "TYPE" ) == "CHARSCOLORS" ) )
        {
          for ( int j = y; j < y + height; ++j )
          {
            for ( int i = x; i < x + width; ++i )
            {
              resultingDataPerScreen.AppendU8( (byte)( screen.ColorAt( i, j ) ) );
            }
          }
        }
        if ( ArgParser.Parameter( "TYPE" ) == "CHARSET" )
        {
          count = -1;
          int     firstChar = 0;
          if ( ArgParser.IsParameterSet( "OFFSET" ) )
          {
            firstChar = GR.Convert.ToI32( ArgParser.Parameter( "OFFSET" ) );
          }
          if ( ArgParser.IsParameterSet( "COUNT" ) )
          {
            count = GR.Convert.ToI32( ArgParser.Parameter( "COUNT" ) );
          }
          if ( count == -1 )
          {
            count = charScreenProject.CharSet.Characters.Count;
          }
          if ( ( firstChar < 0 )
          ||   ( firstChar >= charScreenProject.CharSet.Characters.Count ) )
          {
            System.Console.WriteLine( "OFFSET is invalid" );
            return 1;
          }
          if ( ( count <= 0 )
          ||   ( firstChar + count > charScreenProject.CharSet.Characters.Count ) )
          {
            System.Console.WriteLine( "COUNT is invalid" );
            return 1;
          }
          resultingDataPerScreen = new GR.Memory.ByteBuffer( (uint)( count * 8 ) );
          for ( int j = 0; j < count; ++j )
          {
            charScreenProject.CharSet.Characters[firstChar + j].Tile.Data.CopyTo( resultingDataPerScreen, 0, 8, j * 8 );
          }
        }
        resultingData.Append( resultingDataPerScreen );
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
