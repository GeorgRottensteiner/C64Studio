using System;
using System.Collections.Generic;
using System.Text;

namespace MediaTool
{
  public partial class Manager
  {
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

  }
}
