namespace GR
{
  namespace Game
  {
    public class Layer<TileType> where TileType : new()
    {
      private int       m_Width  = 0;
      private int       m_Height = 0;


      public TileType InvalidTile
      {
        get;
        set;
      }

      public bool Visible
      {
        get;
        set;
      }

      public Layer()
      {
        Visible = true;
      }

      public int Width
      {
        get
        {
          return m_Width;
        }
      }

      public int Height
      {
        get
        {
          return m_Height;
        }
      }

      
      protected TileType[] m_Tiles = null;



      public TileType this[int x, int y]
      {
        get
        {
          if ( ( x < 0 )
          ||   ( x >= Width )
          ||   ( y < 0 )
          ||   ( y >= Height ) )
          {
            return InvalidTile;
          }
          return m_Tiles[x + y * Width];
        }
        set
        {
          if ( ( x < 0 )
          || ( x >= Width )
          || ( y < 0 )
          || ( y >= Height ) )
          {
            return;
          }
          m_Tiles[x + y * Width] = value;
        }
      }

      public void Resize( int Width, int Height )
      {
        if ( m_Tiles == null )
        {
          m_Tiles = new TileType[Width * Height];
          for ( int i = 0; i < Width; ++i )
          {
            for ( int j = 0; j < Height; ++j )
            {
              if ( InvalidTile == null )
              {
                m_Tiles[i + j * Width] = default( TileType );
              }
              else
              {
                if ( InvalidTile is System.ICloneable )
                {
                  m_Tiles[i + j * Width] = (TileType)( (System.ICloneable)InvalidTile ).Clone();
                }
                else if ( InvalidTile is System.ValueType )
                {
                  m_Tiles[i + j * Width] = InvalidTile;
                }
                else
                {
                  m_Tiles[i + j * Width] = new TileType();
                }
              }
            }
          }
        }
        else
        {
          TileType[]  tempTiles = new TileType[Width * Height];

          // copy matching values over
          int     copyWidth = System.Math.Min( Width, m_Width );
          int     copyHeight = System.Math.Min( Height, m_Height );

          for ( int i = 0; i < Width; ++i )
          {
            for ( int j = 0; j < Height; ++j )
            {
              if ( InvalidTile == null )
              {
                tempTiles[i + j * Width] = default( TileType );
              }
              else
              {
                if ( InvalidTile is System.ICloneable )
                {
                  tempTiles[i + j * Width] = (TileType)( (System.ICloneable)InvalidTile ).Clone();
                }
                else if ( InvalidTile is System.ValueType )
                {
                  tempTiles[i + j * Width] = InvalidTile;
                }
                else
                {
                  tempTiles[i + j * Width] = new TileType();
                }
              }
            }
          }
          for ( int i = 0; i < copyWidth; ++i )
          {
            for ( int j = 0; j < copyHeight; ++j )
            {
              tempTiles[i + j * Width] = m_Tiles[i + j * m_Width];
            }
          }

          m_Tiles = tempTiles;
        }
        m_Width = Width;
        m_Height = Height;
      }



      public void Fill( TileType Tile )
      {
        for ( int i = 0; i < m_Tiles.Length; ++i )
        {
          m_Tiles[i] = Tile;
        }
      }

    }

  }
}