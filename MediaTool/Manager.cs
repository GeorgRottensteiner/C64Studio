using System;
using System.Collections.Generic;
using System.Text;

namespace MediaTool
{
  public class Manager
  {
    private bool ValidateExportType( string MediaType, string ExportType, string[] ValidExportTypes )
    {
      foreach ( var exportType in ValidExportTypes )
      {
        if ( exportType == ExportType.ToUpper() )
        {
          return true;
        }
      }
      System.Console.WriteLine( ExportType + " is not supported for media " + MediaType );
      return false;
    }



    private int HandleSpriteProject( GR.Text.ArgumentParser ArgParser )
    {
      if ( !ValidateExportType( "sprite project", ArgParser.Parameter( "TYPE" ), new string[] { "SPRITES" } ) )
      {
        return 1;
      }

      var spriteProject = new C64Studio.Formats.SpriteProject();

      if ( !spriteProject.ReadFromBuffer( GR.IO.File.ReadAllBytes( ArgParser.Parameter( "SPRITEPROJECT" ) ) ) )
      {
        System.Console.WriteLine( "Couldn't read sprite project from file " + ArgParser.Parameter( "SPRITEPROJECT" ) );
        return 1;
      }

      int     firstSprite = 0;
      int     count = -1;
      if ( ArgParser.IsParameterSet( "OFFSET" ) )
      {
        firstSprite = GR.Convert.ToI32( ArgParser.Parameter( "OFFSET" ) );
      }
      if ( ArgParser.IsParameterSet( "COUNT" ) )
      {
        count = GR.Convert.ToI32( ArgParser.Parameter( "COUNT" ) );
      }
      if ( count == -1 )
      {
        count = spriteProject.NumSprites;
      }

      if ( ( firstSprite < 0 )
      ||   ( firstSprite >= spriteProject.NumSprites ) )
      {
        System.Console.WriteLine( "OFFSET is invalid" );
        return 1;
      }
      if ( ( count <= 0 )
      ||   ( firstSprite + count > spriteProject.NumSprites ) )
      {
        System.Console.WriteLine( "COUNT is invalid" );
        return 1;
      }

      GR.Memory.ByteBuffer    spriteData = new GR.Memory.ByteBuffer( (uint)( count * 64 ) );
      for ( int i = 0; i < count; ++i )
      {
        spriteProject.Sprites[firstSprite + i].Data.CopyTo( spriteData, 0, 63, i * 64 );
      }

      if ( !GR.IO.File.WriteAllBytes( ArgParser.Parameter( "EXPORT" ), spriteData ) )
      {
        Console.WriteLine( "Could not write to file " + ArgParser.Parameter( "EXPORT" ) );
        return 1;
      }
      return 0;
    }



    private int HandleSpriteFile( GR.Text.ArgumentParser ArgParser )
    {
      if ( !ValidateExportType( "sprite file", ArgParser.Parameter( "TYPE" ), new string[] { "SPRITES" } ) )
      {
        return 1;
      }

      GR.Memory.ByteBuffer    data = GR.IO.File.ReadAllBytes( ArgParser.Parameter( "SPRITES" ) );
      if ( data == null )
      {
        System.Console.WriteLine( "Couldn't read binary sprite file " + ArgParser.Parameter( "SPRITES" ) );
        return 1;
      }
      int     firstSprite = 0;
      int     count = -1;
      if ( ArgParser.IsParameterSet( "OFFSET" ) )
      {
        firstSprite = GR.Convert.ToI32( ArgParser.Parameter( "OFFSET" ) );
      }
      if ( ArgParser.IsParameterSet( "COUNT" ) )
      {
        count = GR.Convert.ToI32( ArgParser.Parameter( "COUNT" ) );
      }
      if ( count == -1 )
      {
        count = (int)data.Length / 64;
      }

      if ( ( firstSprite < 0 )
      || ( firstSprite >= (int)data.Length / 64 ) )
      {
        System.Console.WriteLine( "OFFSET is invalid" );
        return 1;
      }
      if ( ( count <= 0 )
      || ( firstSprite + count > (int)data.Length / 64 ) )
      {
        System.Console.WriteLine( "COUNT is invalid" );
        return 1;
      }

      GR.Memory.ByteBuffer    spriteData = new GR.Memory.ByteBuffer( (uint)( count * 64 ) );
      data.CopyTo( spriteData, firstSprite * 64, count * 64 );

      if ( !GR.IO.File.WriteAllBytes( ArgParser.Parameter( "EXPORT" ), spriteData ) )
      {
        Console.WriteLine( "Could not write to file " + ArgParser.Parameter( "EXPORT" ) );
        return 1;
      }
      return 0;
    }



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
        charsetProject.Characters[firstUnit + i].Data.CopyTo( exportData, 0, 8, i * 8 );
      }

      if ( !GR.IO.File.WriteAllBytes( ArgParser.Parameter( "EXPORT" ), exportData ) )
      {
        Console.WriteLine( "Could not write to file " + ArgParser.Parameter( "EXPORT" ) );
        return 1;
      }
      return 0;
    }



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



    private int HandleCharscreenFile( GR.Text.ArgumentParser ArgParser )
    {
      if ( !ValidateExportType( "charscreen file", ArgParser.Parameter( "TYPE" ), new string[] { "CHARS", "CHARSCOLORS", "COLORS" } ) )
      {
        return 1;
      }

      GR.Memory.ByteBuffer    data = GR.IO.File.ReadAllBytes( ArgParser.Parameter( "CHARSCREEN" ) );
      if ( data == null )
      {
        System.Console.WriteLine( "Couldn't read binary char file " + ArgParser.Parameter( "CHARSCREEN" ) );
        return 1;
      }

      var charScreenProject = new C64Studio.Formats.CharsetScreenProject();
      if ( !charScreenProject.ReadFromBuffer( data ) )
      {
        System.Console.WriteLine( "Couldn't read charscreen project from file " + ArgParser.Parameter( "CHARSCREEN" ) );
        return 1;
      }

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
        ||   ( x + width > charScreenProject.ScreenWidth )
        ||   ( y + height > charScreenProject.ScreenHeight ) )
        {
          System.Console.WriteLine( "AREA values are out of bounds or invalid, expected four values separated by comma: x,y,width,height" );
          return 1;
        }
      }
      else
      {
        width = charScreenProject.ScreenWidth;
        height = charScreenProject.ScreenHeight;
      }

      GR.Memory.ByteBuffer    resultingData = new GR.Memory.ByteBuffer();

      if ( ArgParser.Parameter( "TYPE" ).Contains( "CHARS" ) )
      {
        for ( int j = y; j < y + height; ++j )
        {
          for ( int i = x; i < x + width; ++i )
          {
            resultingData.AppendU8( (byte)charScreenProject.Chars[i + j * charScreenProject.ScreenWidth] );
          }
        }
      }
      if ( ArgParser.Parameter( "TYPE" ).Contains( "COLORS" ) )
      {
        for ( int j = y; j < y + height; ++j )
        {
          for ( int i = x; i < x + width; ++i )
          {
            resultingData.AppendU8( (byte)( charScreenProject.Chars[i + j * charScreenProject.ScreenWidth] >> 8 ) );
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



    public int Handle( string[] args )
    {
      var argParser = new GR.Text.ArgumentParser();

      argParser.AddOptionalParameter( "SPRITEPROJECT" );
      argParser.AddOptionalParameter( "SPRITES" );
      argParser.AddOptionalParameter( "CHARSETPROJECT" );
      argParser.AddOptionalParameter( "CHARS" );
      argParser.AddOptionalParameter( "CHARSCREEN" );
      argParser.AddOptionalParameter( "OFFSET" );
      argParser.AddOptionalParameter( "COUNT" );
      argParser.AddOptionalParameter( "AREA" );
      argParser.AddOptionalParameter( "BINARY" );
      argParser.AddParameter( "EXPORT" );
      argParser.AddSwitch( "TYPE", false );
      argParser.AddSwitchValue( "TYPE", "SPRITES" );
      argParser.AddSwitchValue( "TYPE", "CHARS" );
      argParser.AddSwitchValue( "TYPE", "CHARSCOLORS" );
      argParser.AddSwitchValue( "TYPE", "COLORS" );
      argParser.AddSwitchValue( "TYPE", "BYTES" );

      if ( !argParser.CheckParameters( args ) )
      {
        System.Console.WriteLine( "MediaTool V" + System.Windows.Forms.Application.ProductVersion );
        System.Console.WriteLine( "" );

        System.Console.WriteLine( argParser.ErrorInfo() );
        System.Console.WriteLine( "" );

        System.Console.WriteLine( "Call with mediatool" );
        System.Console.WriteLine( "  [-spriteproject <sprite project file>]" );
        System.Console.WriteLine( "  [-sprites <binary sprite file>]" );
        System.Console.WriteLine( "  [-charsetproject <charset project file>]" );
        System.Console.WriteLine( "  [-chars <binary charset file>]" );
        System.Console.WriteLine( "  [-charscreen <charscreen project file>]" );
        System.Console.WriteLine( "  [-binary <file>]" );
        System.Console.WriteLine( "  [-type <export format>]" );
        System.Console.WriteLine( "  [-export <file name>]" );
        System.Console.WriteLine( "  [-area <x,y,width,height>]" );
        System.Console.WriteLine( "  [-offset <first unit to affect, default 0>]" );
        System.Console.WriteLine( "  [-count <count of units to affect, default all>]" );
        System.Console.WriteLine( "" );
        System.Console.WriteLine( "  -area is only applicable for charscreen" );
        return 1;
      }

      if ( argParser.IsParameterSet( "SPRITEPROJECT" ) )
      {
        return HandleSpriteProject( argParser );
      }
      else if ( argParser.IsParameterSet( "SPRITES" ) )
      {
        return HandleSpriteFile( argParser );
      }
      else if ( argParser.IsParameterSet( "CHARSETPROJECT" ) )
      {
        return HandleCharsetProject( argParser );
      }
      else if ( argParser.IsParameterSet( "CHARS" ) )
      {
        return HandleCharFile( argParser );
      }
      else if ( argParser.IsParameterSet( "CHARSCREEN" ) )
      {
        return HandleCharscreenFile( argParser );
      }
      else if ( argParser.IsParameterSet( "BINARY" ) )
      {
        return HandleBinaryFile( argParser );
      }
      System.Console.Error.WriteLine( "Missing medium" );
      return 1;
    }

  }
}
