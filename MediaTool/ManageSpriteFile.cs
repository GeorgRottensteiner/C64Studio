using System;
using System.Collections.Generic;
using System.Text;

namespace MediaTool
{
  public partial class Manager
  {
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




  }
}
