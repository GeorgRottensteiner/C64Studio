using System;
using System.Diagnostics;

namespace GR
{
  namespace Math
  {
    [DebuggerDisplay( "X, Y = {X}, {Y}" )]
    public struct Point : IComparable
    {
      public int X;
      public int Y;



      public Point( int X, int Y )
      {
        this.X = X;
        this.Y = Y;
      }



      public Point( Point RHS )
      {
        X = RHS.X;
        Y = RHS.Y;
      }



      public Point( PointF RHS )
      {
        X = (int)RHS.X;
        Y = (int)RHS.Y;
      }


      public float Length
      {
        get
        {
          return (float)System.Math.Sqrt( LengthSquared() );
        }
      }



      public int LengthSquared()
      {
        return X * X + Y * Y;
      }



      public void Offset( int DX, int DY )
      {
        X += DX;
        Y += DY;
      }



      public int CompareTo( object Obj )
      {
        if ( ( Obj == null )
        ||   ( !( Obj is Point ) ) )
        {
          throw new ArgumentException( "Other object is not a Point" );
        }
        Point   otherPoint = (Point)Obj;

        if ( X < otherPoint.X )
        {
          return -1;
        }
        else if ( X > otherPoint.X )
        {
          return 1;
        }
        return otherPoint.Y - Y;
      }



      public static Point operator +( Point Point1, Point Point2 )
      {
        return new Point( Point1.X + Point2.X, Point1.Y + Point2.Y );
      }



      public static Point operator -( Point Point1, Point Point2 )
      {
        return new Point( Point1.X - Point2.X, Point1.Y - Point2.Y );
      }



      public static Point operator *( Point Point1, int Value )
      {
        return new Point( Point1.X * Value, Point1.Y * Value );
      }



      public static Point operator /( Point Point1, int Value )
      {
        return new Point( Point1.X / Value, Point1.Y / Value );
      }



      public float DistanceTo( Point V )
      {
        return ( V - this ).Length;
      }



      public float Angle()
      {
        return (float)( System.Math.Atan2( (double)Y, (double)X ) * 180.0 / System.Math.PI );
      }



      public float AngleTowards( Point V )
      { 
        return ( V - this ).Angle();
      }



      public static implicit operator PointF( Point P )
      {
        return new PointF( (float)P.X, (float)P.Y );
      }

    }



    public struct PointF : IComparable
    {
      public float X;
      public float Y;



      public PointF( float X, float Y )
      {
        this.X = X;
        this.Y = Y;
      }



      public PointF( PointF RHS )
      {
        X = RHS.X;
        Y = RHS.Y;
      }



      public PointF Unit
      {
        get
        {
          return new PointF( X, Y ) / Length;
        }
      }



      public float Length
      {
        get
        {
          return (float)System.Math.Sqrt( LengthSquared() );
        }
      }



      public float LengthSquared()
      {
        return X * X + Y * Y;
      }



      public void Offset( float DX, float DY )
      {
        X += DX;
        Y += DY;
      }



      public int CompareTo( object Obj )
      {
        if ( ( Obj == null )
        ||   ( !( Obj is PointF ) ) )
        {
          throw new ArgumentException( "Other object is not a Point" );
        }
        PointF   otherPoint = (PointF)Obj;

        if ( X < otherPoint.X )
        {
          return -1;
        }
        else if ( X > otherPoint.X )
        {
          return 1;
        }
        if ( Y < otherPoint.Y )
        {
          return -1;
        }
        else if ( Y > otherPoint.Y )
        {
          return 1;
        }
        return 0;
      }



      public static PointF operator +( PointF Point1, PointF Point2 )
      {
        return new PointF( Point1.X + Point2.X, Point1.Y + Point2.Y );
      }



      public static PointF operator -( PointF Point1, PointF Point2 )
      {
        return new PointF( Point1.X - Point2.X, Point1.Y - Point2.Y );
      }



      public static PointF operator *( PointF Point1, float Value )
      {
        return new PointF( Point1.X * Value, Point1.Y * Value );
      }



      public static PointF operator *( float Value, PointF Point1 )
      {
        return new PointF( Point1.X * Value, Point1.Y * Value );
      }



      public static PointF operator /( PointF Point1, float Value )
      {
        return new PointF( Point1.X / Value, Point1.Y / Value );
      }



      public float DistanceTo( PointF V )
      {
        return ( V - this ).Length;
      }



      public float Angle()
      {
        return (float)( System.Math.Atan2( (double)Y, (double)X ) * 180.0 / System.Math.PI );
      }



      public float AngleTowards( PointF V )
      { 
        return ( V - this ).Angle();
      }



      public void Normalize()
      {
        float   len = Length;
        if ( len == 0 )
        {
          return;
        }
        X /= len;
        Y /= len;
      }



      float Dot( PointF V )
      {
        return (float)X * (float)V.X + (float)Y * (float)V.Y;
      }



      PointF Normalized()
      {
        float len = Length;
        if ( len <= 0.0f )
        {
          return new PointF( this );
        }
        return new PointF( this ) / len;
      }



      public PointF ProjectOn( PointF V2 )
      {
        PointF     normV2 = V2.Normalized();

        return normV2 * normV2.Dot( this );
      }

    }

  }
}
