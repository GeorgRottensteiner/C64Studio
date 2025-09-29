using GR.Math;
using System;
using System.Diagnostics;



namespace GR.Math
{
  [DebuggerDisplay( "X={X}, Y={Y}, Width={Width}, Height={Height}" )]
  public struct Rectangle
  {
    public static readonly Rectangle Empty = new Rectangle();



    Point     Position;
    Point     Size;



    public Rectangle( int X, int Y, int Width, int Height )
    {
      Position  = new Point( X, Y );
      Size      = new Point( Width, Height );
    }



    public int Left
    {
      get
      {
        return Position.X;
      }
      set
      {
        Position.X = value;
      }
    }



    public int Top
    {
      get
      {
        return Position.Y;
      }
      set
      {
        Position.Y = value;
      }
    }



    public int Right
    {
      get
      {
        return Position.X + Size.X;
      }
    }



    public int Bottom
    {
      get
      {
        return Position.Y + Size.Y;
      }
    }



    public int X
    {
      get
      {
        return Position.X;
      }
      set
      {
        Position.X = value;
      }
    }



    public int Y
    {
      get
      {
        return Position.Y;
      }
      set
      {
        Position.Y = value;
      }
    }



    public int Width
    {
      get
      {
        return Size.X;
      }
      set
      {
        Size.X = value;
      }
    }



    public int Height
    {
      get
      {
        return Size.Y;
      }
      set
      {
        Size.Y = value;
      }
    }



    public void Offset( int dx, int dy )
    {
      Position.Offset( dx, dy );
    }
  }

}