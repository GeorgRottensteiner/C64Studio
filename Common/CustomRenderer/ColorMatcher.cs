using System;
using System.Collections.Generic;
using System.ComponentModel;



namespace RetroDevStudio.Converter
{
  public enum ColorMatchType
  {
    [Description( "RGB distance" )]
    RGB_DISTANCE = 0,
    [Description( "HUE distance" )]
    HUE_DISTANCE,
    [Description( "CIE76 distance" )]
    CIE76_DISTANCE
  };



  public class ColorMatcher
  {
    public static int MatchColor( ColorMatchType MatchType, byte R, byte G, byte B, List<uint> Colors )
    {
      int bestMatchDistance = 50000000;
      int bestMatch = -1;

      ColorSystem.RGB   origColor = new ColorSystem.RGB( R, G, B );

      for ( int k = 0; k < Colors.Count; ++k )
      {
        switch ( MatchType )
        {
          case ColorMatchType.RGB_DISTANCE:
            {
              int distR = R - (int)( ( Colors[k] & 0xff0000 ) >> 16 );
              int distG = G - (int)( ( Colors[k] & 0x00ff00 ) >> 8 );
              int distB = B - (int)  ( Colors[k] & 0xff );
              //int distance = (int)( distR * distR * 0.3f + distG * distG * 0.6f + distB * distB * 0.1f );
              int distance = (int)( distR * distR + distG * distG + distB * distB );

              if ( distance < bestMatchDistance )
              {
                bestMatchDistance = distance;
                bestMatch = k;
              }
            }
            break;
          case ColorMatchType.HUE_DISTANCE:
            {
              ColorSystem.HSV myHSV = ColorSystem.RGBToHSV( origColor );
              ColorSystem.HSV otherHSV = ColorSystem.RGBToHSV( new ColorSystem.RGB( (byte)( ( Colors[k] & 0xff0000 ) >> 16 ), (byte)( ( Colors[k] & 0x00ff00 ) >> 8 ), (byte)( Colors[k] & 0xff ) ) );

              int distance = Math.Abs( (int)( otherHSV.H - myHSV.H ) );
              distance = Math.Min( distance, Math.Abs( (int)( otherHSV.H + 360.0 - myHSV.H ) ) );
              distance = Math.Min( distance, Math.Abs( (int)( otherHSV.H - 360.0 - myHSV.H ) ) );

              distance *= distance;
              distance += (int)( 255.0f * Math.Abs( myHSV.V - otherHSV.V ) ) * (int)( 255.0f * Math.Abs( myHSV.V - otherHSV.V ) );
              distance += (int)( 255.0f * Math.Abs( myHSV.S - otherHSV.S ) ) * (int)( 255.0f * Math.Abs( myHSV.S - otherHSV.S ) );

              if ( distance < bestMatchDistance )
              {
                bestMatchDistance = distance;
                bestMatch = k;
              }
            }
            break;
          case ColorMatchType.CIE76_DISTANCE:
            {
              ColorSystem.CIELab myLab = ColorSystem.RGBToCIELab( origColor );
              ColorSystem.CIELab otherLab = ColorSystem.RGBToCIELab( new ColorSystem.RGB( (byte)( ( Colors[k] & 0xff0000 ) >> 16 ), (byte)( ( Colors[k] & 0x00ff00 ) >> 8 ), (byte)( Colors[k] & 0xff ) ) );

              float distL = ( myLab.L - otherLab.L ) * ( myLab.L - otherLab.L );
              float dista = ( myLab.a - otherLab.a ) * ( myLab.a - otherLab.a );
              float distb = ( myLab.b - otherLab.b ) * ( myLab.b - otherLab.b );

              int distance = (int)( distL + dista + distb );

              if ( distance < bestMatchDistance )
              {
                bestMatchDistance = distance;
                bestMatch = k;
              }
            }
            break;
        }
      }
      return bestMatch;
    }



  }
}
