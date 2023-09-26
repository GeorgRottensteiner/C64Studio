using GR.Math;



namespace GR.Math
{
  public struct Rectangle
  {
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



  }

}