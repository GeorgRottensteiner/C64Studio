using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MediaTool
{
  public partial class Manager
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



    private bool ValidateFirstAndCount( int firstEntry, ref int count, int containerCount )
    {
      if ( count == -1 )
      {
        count = containerCount;
      }
      if ( ( firstEntry < 0 )
      ||   ( firstEntry >= containerCount ) )
      {
        System.Console.WriteLine( "OFFSET is invalid" );
        return false;
      }
      if ( ( count <= 0 )
      ||   ( firstEntry + count > containerCount ) )
      {
        System.Console.WriteLine( "COUNT is invalid" );
        return false;
      }
      return true;
    }



    public int Handle( string[] args )
    {
      var argParser = new GR.Text.ArgumentParser();

      argParser.AddOptionalParameter( "SPRITEPROJECT" );
      argParser.AddOptionalParameter( "SPRITES" );
      argParser.AddOptionalParameter( "CHARSETPROJECT" );
      argParser.AddOptionalParameter( "CHARS" );
      argParser.AddOptionalParameter( "CHARSCREEN" );
      argParser.AddOptionalParameter( "GRAPHICSCREEN" );
      argParser.AddOptionalParameter( "MAPPROJECT" );
      argParser.AddOptionalParameter( "OFFSET" );
      argParser.AddOptionalParameter( "COUNT" );
      argParser.AddOptionalParameter( "AREA" );
      argParser.AddOptionalParameter( "BINARY" );
      argParser.AddOptionalParameter( "IMPORTIMAGE" );
      argParser.AddParameter( "EXPORT" );
      argParser.AddSwitch( "TYPE", false );
      argParser.AddSwitchValue( "TYPE", "SPRITES" );
      argParser.AddSwitchValue( "TYPE", "CHARS" );
      argParser.AddSwitchValue( "TYPE", "CHARSCOLORS" );
      argParser.AddSwitchValue( "TYPE", "COLORS" );
      argParser.AddSwitchValue( "TYPE", "CHARSET" );
      argParser.AddSwitchValue( "TYPE", "BYTES" );
      argParser.AddSwitchValue( "TYPE", "MULTICOLORBITMAP" );
      argParser.AddSwitchValue( "TYPE", "MULTICOLORBITMAPCOLOR" );
      argParser.AddSwitchValue( "TYPE", "MULTICOLORBITMAPSCREEN" );
      argParser.AddSwitchValue( "TYPE", "MULTICOLORBITMAPSCREENCOLOR" );
      argParser.AddSwitchValue( "TYPE", "MULTICOLORBITMAPCOLORSCREEN" );
      argParser.AddSwitchValue( "TYPE", "MULTICOLORSCREEN" );
      argParser.AddSwitchValue( "TYPE", "MULTICOLORCOLOR" );
      argParser.AddSwitchValue( "TYPE", "HIRESBITMAP" );
      argParser.AddSwitchValue( "TYPE", "HIRESCOLOR" );
      argParser.AddSwitchValue( "TYPE", "HIRESSCREEN" );
      argParser.AddSwitchValue( "TYPE", "HIRESBITMAPCOLOR" );
      argParser.AddSwitchValue( "TYPE", "HIRESBITMAPSCREEN" );
      argParser.AddSwitchValue( "TYPE", "HIRESCOLORBITMAP" );
      argParser.AddSwitchValue( "TYPE", "HIRESCOLORSCREEN" );
      argParser.AddSwitchValue( "TYPE", "HIRESSCREENBITMAP" );
      argParser.AddSwitchValue( "TYPE", "HIRESSCREENCOLOR" );
      argParser.AddSwitchValue( "TYPE", "HIRESBITMAPSCREENCOLOR" );
      argParser.AddSwitchValue( "TYPE", "HIRESBITMAPCOLORSCREEN" );
      argParser.AddSwitchValue( "TYPE", "MAPDATA" );
      argParser.AddSwitchValue( "TYPE", "MAPDATAVERT" );
      argParser.AddSwitchValue( "TYPE", "MAPDATAASM" );
      argParser.AddSwitchValue( "TYPE", "MAPDATAVERTASM" );

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
        System.Console.WriteLine( "  [-graphicscreen <graphicscreen project file>]" );
        System.Console.WriteLine( "  [-mapproject <map project file>]" );
        System.Console.WriteLine( "  [-binary <file>]" );
        System.Console.WriteLine( "  [-type <export format>]" );
        System.Console.WriteLine( "  [-export <file name>]" );
        System.Console.WriteLine( "  [-importimage <image name>]" );
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
      else if ( argParser.IsParameterSet( "GRAPHICSCREEN" ) )
      {
        return HandleGraphicscreenFile( argParser );
      }
      else if ( argParser.IsParameterSet( "MAPPROJECT" ) )
      {
        return HandleMapProject( argParser );
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
