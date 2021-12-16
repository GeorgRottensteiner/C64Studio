using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C64Studio.Converter
{
  public class ColorQuantizer
  {
    const int MaxTreeDepth          = 8;
    const int NodesInAList          = 1536;
    const int ExceptionQueueLength  = 16;
    const int MaxNodes              = 266817;
    const int QuantumDepth          = 8;

    int                       m_Depth;
    int                       m_Colors;
    CubeInfo                  m_pCubeInfo;
    QuantizeInfo              m_pQuantizeInfo;


    class NodeInfo
    {
      public NodeInfo       parent = null;
      public NodeInfo[]     child = new NodeInfo[8];

      public double         number_unique = 0;
      public double         total_red = 0;
      public double         total_green = 0;
      public double         total_blue = 0;
      public double         quantize_error = 0;

      public int            color_number = 0;

      public byte           id = 0;
      public byte           level = 0;
      public byte           census = 0;
    }



    class Nodes
    {
      public NodeInfo[]  nodes = new NodeInfo[NodesInAList];
      public Nodes    next = null;
    }



    struct PixelPacket
    {
      public byte   red;
      public byte   green;
      public byte   blue;
    }



    public class QuantizeInfo
    {
      public ulong      signature;
      public int        number_colors;
      public uint       tree_depth;
      public uint       dither;
      public uint       measure_error;


      public void Clear()
      {
        signature = 0;
        number_colors = 0;
        tree_depth = 0;
        dither = 0;
        measure_error = 0;
      }
    }



    class CubeInfo
    {
      public NodeInfo         root;

      public int              colors;

      public PixelPacket      color = new PixelPacket();
      public PixelPacket[]    colormap = new PixelPacket[256];

      public double           distance;
      public double           pruning_threshold;
      public double           next_threshold;

      public int              nodes;
      public int              free_nodes;
      public int              color_number;

      public int              next_node;
      public Nodes            node_queue;

      public List<long>       cache;

      public double[]         weights = new double[ExceptionQueueLength];

      public QuantizeInfo     quantize_info;

      public int              depth;
    }



    public ColorQuantizer( int NumColors )
    {
      m_pCubeInfo       = null;
      m_pQuantizeInfo   = new QuantizeInfo();
      m_Depth           = 8;
      m_Colors          = NumColors;

      m_pQuantizeInfo.Clear();
      m_pQuantizeInfo.number_colors = NumColors;
      m_pQuantizeInfo.dither        = 0;


      // Depth of color tree is: Log4(colormap size)+2.
      int colors = NumColors;
      for ( m_Depth = 1; colors != 0; m_Depth++ )
      {
        colors >>= 2;
      }
      if ( m_pQuantizeInfo.dither > 0 )
      {
        m_Depth--;
      }
      m_Depth += 2;

      m_pCubeInfo = GetCubeInfo( m_pQuantizeInfo, m_Depth );
    }



    void PruneChild( CubeInfo cube_info, NodeInfo node_info )
    {
      // Traverse any children.
      if ( node_info.census != 0 )
      {
        for ( int id = 0; id < MaxTreeDepth; id++ )
        {
          if ( ( node_info.census & ( 1 << id ) ) != 0 )
          {
            PruneChild( cube_info, node_info.child[id] );
          }
        }
      }

      // Merge color statistics into parent.
      NodeInfo       parent = node_info.parent;

      parent.census         &= (byte)~( 1 << node_info.id );
      parent.number_unique  += node_info.number_unique;
      parent.total_red      += node_info.total_red;
      parent.total_green    += node_info.total_green;
      parent.total_blue     += node_info.total_blue;
      --cube_info.nodes;
    }



    void PruneLevel( CubeInfo cube_info, NodeInfo node_info )
    {
      // Traverse any children.
      if ( node_info.census != 0 )
      {
        for ( int id = 0; id < MaxTreeDepth; id++ )
        {
          if ( ( node_info.census & ( 1 << id ) ) != 0 )
          {
            PruneLevel( cube_info, node_info.child[id] );
          }
        }
      }
      if ( node_info.level == cube_info.depth )
      {
        PruneChild( cube_info, node_info );
      }
    }



    NodeInfo GetNodeInfo( CubeInfo cube_info, byte id, byte level, NodeInfo parent )
    {
      NodeInfo   node_info;

      if ( cube_info.free_nodes == 0 )
      {
        Nodes        nodes = new Nodes();

        nodes.next            = cube_info.node_queue;
        cube_info.node_queue  = nodes;
        cube_info.next_node   = 0;
        cube_info.free_nodes  = NodesInAList;
      }
      ++cube_info.nodes;
      --cube_info.free_nodes;

      cube_info.node_queue.nodes[cube_info.next_node] = new NodeInfo();

      node_info         = cube_info.node_queue.nodes[cube_info.next_node];
      node_info.parent  = parent;
      node_info.id      = id;
      node_info.level   = level;

      ++cube_info.next_node;

      return node_info;
    }



    public void AddSourceToColorCube( GR.Image.IImage Image )
    {
      Classification( m_pCubeInfo, Image );
    }



    bool Classification( CubeInfo cube_info, GR.Image.IImage Image )
    {
      double        bisect,
                    mid_red,
                    mid_green,
                    mid_blue;

      int           count;

      NodeInfo      node_info;

      double        blue,
                    green,
                    red;

      uint          index,
                    level,
                    id;


      for ( int y = 0; y < Image.Height; ++y )
      {
        if ( cube_info.nodes > MaxNodes )
        {
          // Prune one level if the color tree is too large.
          PruneLevel( cube_info, cube_info.root );
          --cube_info.depth;
        }

        for ( int x = 0; x < Image.Width; x += count )
        {
          // Start at the root and descend the color cube tree.
          count       = 1;
          index       = MaxTreeDepth - 1;
          bisect      = ( MaxRGB() + 1 ) / 2.0;
          mid_red     = MaxRGB() / 2.0;
          mid_green   = MaxRGB() / 2.0;
          mid_blue    = MaxRGB() / 2.0;
          node_info   = cube_info.root;

          uint pixel  = Image.GetPixel( x, y );
          int     r   = (int)( ( pixel & 0xff0000 ) >> 16 );
          int     g   = (int)( ( pixel & 0xff00 ) >> 8 );
          int     b   = (int)( pixel & 0xff );

          for ( level = 1; level <= cube_info.depth; level++ )
          {
            bisect *= 0.5;
            id = (uint)( ( ( Downscale( r ) >> (byte)index ) & 0x01 ) << 2 |
                         ( ( Downscale( g ) >> (byte)index ) & 0x01 ) << 1 |
                         ( ( Downscale( b ) >> (byte)index ) & 0x01 ) );
            mid_red   += ( id & 4 ) != 0 ? bisect : -bisect;
            mid_green += ( id & 2 ) != 0 ? bisect : -bisect;
            mid_blue  += ( id & 1 ) != 0 ? bisect : -bisect;
            if ( node_info.child[id] == null )
            {
              // Set colors of new node to contain pixel.
              node_info.census |= (byte)( 1 << (byte)id );
              node_info.child[id] = GetNodeInfo( cube_info, (byte)id, (byte)level, node_info );
              if ( node_info.child[id] == null )
              {
                Debug.Log( "this should never happen!" );
              }
              if ( level == cube_info.depth )
              {
                ++cube_info.colors;
              }
            }

            // Approximate the quantization error represented by this node.
            node_info = node_info.child[id];
            red       = (double)r - mid_red;
            green     = (double)g - mid_green;
            blue      = (double)b - mid_blue;
            node_info.quantize_error      += count * red * red + count * green * green + count * blue * blue;
            cube_info.root.quantize_error += node_info.quantize_error;
            --index;
          }

          // Sum RGB for this leaf for later derivation of the mean cube color.
          node_info.number_unique += count;
          node_info.total_red     += count * r;
          node_info.total_green   += count * g;
          node_info.total_blue    += count * b;
        }
      }
      return true;
    }



    void Reduce( CubeInfo cube_info, NodeInfo node_info )
    {
      // Traverse any children.
      if ( node_info.census != 0 )
      {
        for ( int id = 0; id < MaxTreeDepth; id++ )
        {
          if ( ( node_info.census & ( 1 << id ) ) != 0 )
          {
            Reduce( cube_info, node_info.child[id] );
          }
        }
      }
      if ( node_info.quantize_error <= cube_info.pruning_threshold )
      {
        PruneChild( cube_info, node_info );
      }
      else
      {
        // Find minimum pruning threshold.
        if ( node_info.number_unique > 0 )
        {
          cube_info.colors++;
        }
        if ( node_info.quantize_error < cube_info.next_threshold )
        {
          cube_info.next_threshold = node_info.quantize_error;
        }
      }
    }



    void Reduction( CubeInfo cube_info, int number_colors )
    {
      cube_info.next_threshold = 0.0;
      while ( cube_info.colors > number_colors )
      {
        cube_info.pruning_threshold = cube_info.next_threshold;
        cube_info.next_threshold = cube_info.root.quantize_error - 1;
        cube_info.colors = 0;
        Reduce( cube_info, cube_info.root );
      }
    }



    void DefineColormap( CubeInfo cube_info, NodeInfo node_info )
    {
      // Traverse any children.
      if ( node_info.census != 0 )
      {
        for ( int id = 0; id < MaxTreeDepth; id++ )
        {
          if ( ( node_info.census & ( 1 << id ) ) != 0 )
          {
            DefineColormap( cube_info, node_info.child[id] );
          }
        }
      }
      if ( node_info.number_unique != 0 )
      {
        // Colormap entry is defined by the mean color in this cube.

        double      number_unique = node_info.number_unique;
        cube_info.colormap[cube_info.colors].red    = (byte)( ( node_info.total_red + 0.5 * number_unique ) / number_unique );
        cube_info.colormap[cube_info.colors].green  = (byte)( ( node_info.total_green + 0.5 * number_unique ) / number_unique );
        cube_info.colormap[cube_info.colors].blue   = (byte)( ( node_info.total_blue + 0.5 * number_unique ) / number_unique );
        node_info.color_number = cube_info.colors++;
      }
    }



    void ClosestColor( CubeInfo cube_info, NodeInfo node_info )
    {
      if ( cube_info.distance != 0.0 )
      {
        // Traverse any children.
        if ( node_info.census != 0 )
        {
          for ( int id = 0; id < MaxTreeDepth; id++ )
          {
            if ( ( node_info.census & ( 1 << id ) ) != 0 )
            {
              ClosestColor( cube_info, node_info.child[id] );
            }
          }
        }
        if ( node_info.number_unique != 0 )
        {
          double          distance;
          double          blue;
          double          green;
          double          red;

          PixelPacket     color;

          // Determine if this color is "closest".
          color     = cube_info.colormap[node_info.color_number];
          red       = (double)( color.red - cube_info.color.red );
          green     = (double)( color.green - cube_info.color.green );
          blue      = (double)( color.blue - cube_info.color.blue );
          distance  = red * red + green * green + blue * blue;
          if ( distance < cube_info.distance )
          {
            cube_info.distance      = distance;
            cube_info.color_number  = node_info.color_number;
          }
        }
      }
    }



    GR.Image.MemoryImage Assignment( CubeInfo cube_info, GR.Image.IImage pCD )
    {
      System.Drawing.Imaging.PixelFormat format = System.Drawing.Imaging.PixelFormat.Format8bppIndexed;

      if ( m_Colors <= 2 )
      {
        format = System.Drawing.Imaging.PixelFormat.Format1bppIndexed;
      }
      /*
      else if ( m_Colors <= 4 )
      {
        format = System.Drawing.Imaging.PixelFormat.Format2bppIndexed;
      }*/
      else if ( m_Colors <= 16 )
      {
        format = System.Drawing.Imaging.PixelFormat.Format4bppIndexed;
      }

      var       pImage = new GR.Image.MemoryImage( pCD.Width, pCD.Height, format );

      var       pal = new RetroDevStudio.Palette( m_Colors );

      byte      index;

      int       count,
                i;

      NodeInfo  node_info;

      uint  dither,
                id;


      // Allocate image colormap.
      cube_info.colormap = new PixelPacket[256];
      cube_info.colors = 0;
      DefineColormap( cube_info, cube_info.root );

      // Create a reduced color image.
      dither = cube_info.quantize_info.dither;
      for ( int y = 0; y < pCD.Height; y++ )
      {
        //indexes=GetIndexes(image);
        for ( int x = 0; x < pCD.Width; x += count )
        {
          // Identify the deepest node containing the pixel's color.
          count = 1;

          uint pixel = pCD.GetPixel( x, y );
          int r = (byte)( ( pixel & 0xff0000 ) >> 16 ),
              g = (byte)( ( pixel & 0xff00 ) >> 8 ),
              b = (byte)( pixel & 0xff );

          node_info = cube_info.root;
          for ( index = MaxTreeDepth - 1; (int)index > 0; index-- )
          {
            id = (uint)( ( ( Downscale( r ) >> index ) & 0x01 ) << 2 |
                                  ( ( Downscale( g ) >> index ) & 0x01 ) << 1 |
                                  ( ( Downscale( b ) >> index ) & 0x01 ) );
            if ( ( node_info.census & ( 1 << (byte)id ) ) == 0 )
            {
              break;
            }
            node_info = node_info.child[id];
          }

          // Find closest color among siblings and their children.
          cube_info.color.red = (byte)r;
          cube_info.color.green = (byte)g;
          cube_info.color.blue = (byte)b;
          cube_info.distance = 3.0 * ( MaxRGB() + 1 ) * ( MaxRGB() + 1 );
          ClosestColor( cube_info, node_info.parent );
          index = (byte)cube_info.color_number;
          for ( i = 0; i < count; i++ )
          {
            if ( cube_info.quantize_info.measure_error == 0 )
            {
              pImage.SetPixel( x, y, index );
            }
          }
        }
      }

      for ( i = 0; i < cube_info.colors; i++ )
      {
        pal.ColorValues[i] = (uint)( 0xff000000
          | (uint)( cube_info.colormap[i].red << 16 )
          | (uint)( cube_info.colormap[i].green << 8 )
          | (uint)cube_info.colormap[i].blue );

        pImage.SetPaletteColor( i, cube_info.colormap[i].red, cube_info.colormap[i].green, cube_info.colormap[i].blue );
      }


      return pImage;
    }



    CubeInfo GetCubeInfo( QuantizeInfo quantize_info, int depth )
    {
      CubeInfo        cube_info;

      double          sum,
                      weight;


      // Initialize tree to describe color cube_info.
      cube_info = new CubeInfo();
      if ( depth > MaxTreeDepth )
      {
        depth = MaxTreeDepth;
      }
      if ( depth < 2 )
      {
        depth = 2;
      }
      cube_info.depth = depth;

      // Initialize root node.
      cube_info.root = GetNodeInfo( cube_info, 0, 0, null );
      if ( cube_info.root == null )
      {
        return null;
      }

      cube_info.root.parent = cube_info.root;
      cube_info.quantize_info = quantize_info;
      if ( cube_info.quantize_info.dither == 0 )
      {
        return cube_info;
      }

      // Initialize dither resources.
      cube_info.cache = new List<long>( ( 1 << 18 ) );

      // Initialize color cache.
      for ( int i = 0; i < ( 1 << 18 ); i++ )
      {
        cube_info.cache[i] = ( -1 );
      }

      // Distribute weights along a curve of exponential decay.
      weight = 1.0;
      for ( int i = 0; i < ExceptionQueueLength; i++ )
      {
        cube_info.weights[ExceptionQueueLength - i - 1] = 1.0 / weight;
        weight *= Math.Exp( Math.Log( (double)( MaxRGB() + 1 ) ) / ( ExceptionQueueLength - 1.0 ) );
      }

      // Normalize the weighting factors.
      weight = 0.0;
      for ( int i = 0; i < ExceptionQueueLength; i++ )
      {
        weight += cube_info.weights[i];
      }

      sum = 0.0;
      for ( int i = 0; i < ExceptionQueueLength; i++ )
      {
        cube_info.weights[i] /= weight;
        sum += cube_info.weights[i];
      }
      cube_info.weights[0] += 1.0 - sum;
      return cube_info;
    }



    /*
    void PalFromCube( GR::Graphic::Palette* pPal, CubeInfo* pCI )
    {
      Nodes* pNode = pCI.node_queue;

      GR::u32 dwGlobal = 0;
      while ( pNode != NULL )
      {
        NodeInfo* pNI = pNode.nodes;

        GR::u32   dwNr = 0;
        while ( pNI != NULL )
        {
          dwNr++;
          if ( pNI.number_unique )
          {
            pPal.SetColor( pNI.color_number,
                (int)( pNI.total_red / pNI.number_unique ),
                (int)( pNI.total_green / pNI.number_unique ),
                (int)( pNI.total_blue / pNI.number_unique ) );

            dwGlobal++;
            if ( dwGlobal >= pCI.colors )
            {
              break;
            }
          }

          pNI++;
          if ( dwNr >= 1536 )
          {
            pNI = NULL;
          }
        }
        pNode = pNode.next;
      }
    }
    */


    /*
    GR::Graphic::Palette* CreatePalette()
    {
      GR::Graphic::Palette*   pPal = new GR::Graphic::Palette();

      pPal.Create( m_Colors );
      PalFromCube( pPal, m_pCubeInfo );

      return pPal;
    }
    */


    public void Calculate()
    {
      Reduction( m_pCubeInfo, m_Colors );
    }



    public GR.Image.IImage Reduce( GR.Image.IImage pPackSource )
    {
      return Assignment( m_pCubeInfo, pPackSource );
    }



    byte MaxRGB()
    {
      return (byte)( ( 1L << QuantumDepth ) - 1L );
    }



    byte Downscale( int Value )
    {
      return (byte)Value;
    }



    byte Upscale( int Value )
    {
      return (byte)Value;
    }

  }
}
