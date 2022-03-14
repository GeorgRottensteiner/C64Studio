using System;



namespace RetroDevStudio
{
  public class ColorSystem
  {
    public class RGB
    {
      // R,G,B  0..255
      public byte         R = 0;
      public byte         G = 0;
      public byte         B = 0;



      public RGB()
      {
      }

      public RGB( byte R, byte G, byte B )
      {
        this.R = R;
        this.G = G;
        this.B = B;
      }
    }



    public class XYZ
    { 
      // X,Y,Z 0..1
      public float         X = 0.0f;
      public float         Y = 0.0f;
      public float         Z = 0.0f;



      public XYZ()
      {
      }

      public XYZ( float X, float Y, float Z )
      {
        this.X = X;
        this.Y = Y;
        this.Z = Z;
      }
    }



    public class CIELab
    {
      public float L = 0.0f;
      public float a = 0.0f;
      public float b = 0.0f;



      public CIELab()
      {
      }

      public CIELab( float L, float a, float b )
      {
        this.L = L;
        this.a = a;
        this.b = b;
      }
    }



    public class HSV
    {
      // H    0..360
      // S,V  0..1

      public float H = 0.0f;
      public float S = 1.0f;
      public float V = 0.0f;


      public HSV()
      {
      }


      public HSV( float H, float S, float V )
      {
        this.H = H;
        this.S = S;
        this.V = V;
      }
    }



    public static XYZ RGBToXYZ( RGB Color )
    {
      float var_R = ( Color.R / 255.0f );        //R from 0 to 255
      float var_G = ( Color.G / 255.0f );        //G from 0 to 255
      float var_B = ( Color.B / 255.0f );        //B from 0 to 255

      if ( var_R > 0.04045f ) 
      {
        var_R = (float)Math.Pow( ( ( var_R + 0.055f ) / 1.055f ), 2.4f );
      }
      else                   
      {
        var_R = var_R / 12.92f;
      }
      if ( var_G > 0.04045f ) 
      {
        var_G = (float)Math.Pow( ( ( var_G + 0.055f ) / 1.055f ), 2.4f );
      }
      else
      {
        var_G = var_G / 12.92f;
      }
      if ( var_B > 0.04045f ) 
      {
        var_B = (float)Math.Pow( ( ( var_B + 0.055f ) / 1.055f ), 2.4f );
      }
      else
      {
        var_B = var_B / 12.92f;
      }
        
      var_R = var_R * 100;
      var_G = var_G * 100;
      var_B = var_B * 100;

      //Observer. = 2°, Illuminant = D65
      float X = var_R * 0.412453f + var_G * 0.357580f + var_B * 0.180423f;
      float Y = var_R * 0.212671f + var_G * 0.715160f + var_B * 0.072169f;
      float Z = var_R * 0.019334f + var_G * 0.119193f + var_B * 0.950227f;

      return new XYZ( X, Y, Z );
    }



    public static RGB XYZToRGB( XYZ Color )
    {
      float var_X = Color.X / 100;       //X from 0 to  95.047      (Observer = 2°, Illuminant = D65)
      float var_Y = Color.Y / 100;        //Y from 0 to 100.000
      float var_Z = Color.Z / 100;       //Z from 0 to 108.883

      float var_R = var_X *  3.240479f + var_Y * -1.537150f + var_Z * -0.498535f;
      float var_G = var_X * -0.969256f + var_Y *  1.875992f + var_Z *  0.041556f;
      float var_B = var_X *  0.055658f + var_Y * -0.204043f + var_Z *  1.057311f;

      if ( var_R > 0.0031308 ) 
      {
        var_R = (float)( 1.055f * Math.Pow( var_R, ( 1 / 2.4 ) ) - 0.055f );
      }
      else
      {
        var_R = 12.92f * var_R;
      }
      if ( var_G > 0.0031308 )
      {
        var_G = (float)( 1.055f * Math.Pow( var_G, ( 1 / 2.4 ) ) - 0.055f );
      }
      else
      {
        var_G = 12.92f * var_G;
      }
      if ( var_B > 0.0031308 )
      {
        var_B = (float)( 1.055f * Math.Pow( var_B, ( 1 / 2.4 ) ) - 0.055f );
      }
      else
      {
        var_B = 12.92f * var_B;
      }
      return new RGB( (byte)( var_R * 255 ), (byte)( var_G * 255 ), (byte)( var_B * 255 ) ); 
    }



    public static HSV RGBToHSV( RGB Color )
    {
      float r = Color.R / 255.0f;
      float g = Color.G / 255.0f;
      float b = Color.B / 255.0f;

      HSV result = new HSV();

      float min, max, delta;
      min = Math.Min( Math.Min( r, g ), b );
      max = Math.Max( Math.Max( r, g ), b );
      result.V = max;				// v
      delta = max - min;
      if ( max != 0 )
      {
        result.S = delta / max;		// s
      }
      else
      {
        // r = g = b = 0		// s = 0, v is undefined
        result.S = 0;
        result.H = 0;
        return result;
      }
      if ( delta == 0 )
      {
        // a grey value
        result.H = 0;
        result.S = 0;
        result.V = r;
        return result;
      }
      if ( r == max )
      {
        result.H = ( g - b ) / delta;		// between yellow & magenta
      }
      else if ( g == max )
      {
        result.H = 2 + ( b - r ) / delta;	// between cyan & yellow
      }
      else
      {
        result.H = 4 + ( r - g ) / delta;	// between magenta & cyan
      }
      result.H *= 60;				// degrees
      if ( result.H < 0 )
      {
        result.H += 360;
      }
      return result;
    }



    public static RGB HSVToRGB( HSV Color )
    {
      int i;
      float f, p, q, t;
      RGB result = new RGB();
      if ( Color.S == 0 )
      {
        // achromatic (grey)
        result.R = result.G = result.B = (byte)( Color.V * 255.0f );
        return result;
      }
      float h = Color.H / 60; // sector 0 to 5
      i = (int)Math.Floor( h );
      f = h - i;			// factorial part of h
      p = Color.V * ( 1 - Color.S );
      q = Color.V * ( 1 - Color.S * f );
      t = Color.V * ( 1 - Color.S * ( 1 - f ) );
      switch ( i )
      {
        case 0:
          result.R = (byte)( Color.V * 255.0f );
          result.G = (byte)( t * 255.0f );
          result.B = (byte)( p * 255.0f );
          break;
        case 1:
          result.R = (byte)( q * 255.0f );
          result.G = (byte)( Color.V * 255.0f );
          result.B = (byte)( p * 255.0f );
          break;
        case 2:
          result.R = (byte)( p * 255.0f );
          result.G = (byte)( Color.V * 255.0f );
          result.B = (byte)( t * 255.0f );
          break;
        case 3:
          result.R = (byte)( p * 255.0f );
          result.G = (byte)( q * 255.0f );
          result.B = (byte)( Color.V * 255.0f );
          break;
        case 4:
          result.R = (byte)( t * 255.0f );
          result.G = (byte)( p * 255.0f );
          result.B = (byte)( Color.V * 255.0f );
          break;
        default:		// case 5:
          result.R = (byte)( Color.V * 255.0f );
          result.G = (byte)( p * 255.0f );
          result.B = (byte)( q * 255.0f );
          break;
      }
      return result;
    }



    public static CIELab XYZToCIELab( XYZ Color )
    {
      CIELab lab = new CIELab();

      // from wikipedia, the only source that actually states useable values for those "constants"
      float     Xn = 0.95f;
      float     Yn = 1.0f;
      float     Zn = 1.09f;
      float     xFactor = Color.X / Xn;
      float     yFactor = Color.Y / Yn;
      float     zFactor = Color.Z / Zn;

      if ( yFactor > 0.008856 )
      {
        yFactor = (float)Math.Pow( yFactor, 1.0 / 3.0 );
        lab.L = (float)( 116 * yFactor - 16 );
      }
      else
      {
        lab.L = 903.3f * yFactor;
        yFactor = 7.787f * yFactor + 16.0f / 116.0f;
      }
      if ( xFactor > 0.008856f )
      {
        xFactor = (float)Math.Pow( xFactor, 1.0 / 3.0 );
      }
      else
      {
        xFactor = 7.787f * xFactor + 16.0f / 116.0f;
      }
      if ( zFactor > 0.008856f )
      {
        zFactor = (float)Math.Pow( zFactor, 1.0 / 3.0 );
      }
      else
      {
        zFactor = 7.787f * zFactor + 16.0f / 116.0f;
      }

      lab.a = 500.0f * ( xFactor - yFactor );
      lab.b = 200.0f * ( yFactor - zFactor );

      return lab;
    }



    public static XYZ CIELabToXYZ( CIELab Color )
    {
      float P = ( Color.L + 16 ) / 116.0f;

      float Xn = 0.95f;
      float Yn = 1.0f;
      float Zn = 1.09f;

      XYZ result = new XYZ( Xn * (float)Math.Pow( P + Color.a / 500.0f, 3.0f ),
                            Yn * (float)Math.Pow( P, 3.0f ),
                            Zn * (float)Math.Pow( P - Color.b / 200.0f, 3.0f ) );
      return result;
    }



    public static CIELab RGBToCIELab( RGB Color )
    {
      return XYZToCIELab( RGBToXYZ( Color ) );
    }



    public static RGB CIELabToRGB( CIELab Color )
    {
      return XYZToRGB( CIELabToXYZ( Color ) );
    }





  }
}
