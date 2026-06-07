using GR.Math;
using System;
using System.Diagnostics;
using System.Drawing;



namespace GR.Math
{
  [DebuggerDisplay( "X={X}, Y={Y}, Width={Width}, Height={Height}" )]
  public struct Rectangle
  {
    public static readonly Rectangle Empty = new Rectangle();



    private Point     _Position;
    private Point     _Size;



    public Rectangle( int X, int Y, int Width, int Height )
    {
      _Position  = new Point( X, Y );
      _Size      = new Point( Width, Height );
    }



    public Point Size 
    { 
      get 
      { 
        return _Size; 
      } 
    }



    public int Left
    {
      get
      {
        return _Position.X;
      }
      set
      {
        _Position.X = value;
      }
    }



    public int Top
    {
      get
      {
        return _Position.Y;
      }
      set
      {
        _Position.Y = value;
      }
    }



    public int Right
    {
      get
      {
        return _Position.X + _Size.X;
      }
    }



    public int Bottom
    {
      get
      {
        return _Position.Y + _Size.Y;
      }
    }



    public int X
    {
      get
      {
        return _Position.X;
      }
      set
      {
        _Position.X = value;
      }
    }



    public int Y
    {
      get
      {
        return _Position.Y;
      }
      set
      {
        _Position.Y = value;
      }
    }



    public int Width
    {
      get
      {
        return _Size.X;
      }
      set
      {
        _Size.X = value;
      }
    }



    public int Height
    {
      get
      {
        return _Size.Y;
      }
      set
      {
        _Size.Y = value;
      }
    }



    public void Offset( int dx, int dy )
    {
      _Position.Offset( dx, dy );
    }



    public bool Contains( int x, int y )
    {
      return ( ( x >= Left ) 
        &&     ( x < Right ) 
        &&     ( y >= Top ) 
        &&     ( y < Bottom ) );
    }
  }

}