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
      argParser.AddOptionalParameter( "OFFSET" );
      argParser.AddOptionalParameter( "COUNT" );
      argParser.AddParameter( "EXPORT" );
      argParser.AddSwitch( "TYPE", false );
      argParser.AddSwitchValue( "TYPE", "SPRITES" );
      argParser.AddSwitchValue( "TYPE", "CHARS" );

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
        System.Console.WriteLine( "  [-type <export format>]" );
        System.Console.WriteLine( "  [-export <file name>]" );
        System.Console.WriteLine( "  [-offset <first unit to affect, default 0>]" );
        System.Console.WriteLine( "  [-count <count of units to affect, default all>]" );
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
      System.Console.Error.WriteLine( "Missing medium" );
      return 1;
    }

  }
}
