using GR.IO;
using GR.Memory;
using System;



namespace RetroDevStudio
{
  public class Palette
  {
    public string                   Name = "";
    private int                     NumColorsUsed = 16;
    public System.Drawing.Color[]   Colors = new System.Drawing.Color[256]; 
    public System.Drawing.Brush[]   ColorBrushes = new System.Drawing.Brush[256];
    public uint[]                   ColorValues = new uint[256];



    public Palette()
    {
    }



    public Palette( int NumColors )
    {
      if ( NumColors < 16 )
      {
        Debug.Log( "< 16 colors" );
        NumColors = 16;
      }
      else if ( ( NumColors > 16 )
      &&        ( NumColors < 256 ) )
      {
        Debug.Log( "< 256 colors" );
        NumColors = 256;
      }
      if ( NumColors == 257 )
      {
        NumColors = 256;
      }
      if ( NumColors == 17 )
      {
        NumColors = 16;
      }

      NumColorsUsed = NumColors;
    }



    public Palette( Palette OtherPal )
    {
      Name          = OtherPal.Name;
      NumColorsUsed = OtherPal.NumColorsUsed;

      for ( int i = 0; i < NumColorsUsed; ++i )
      {
        ColorValues[i] = OtherPal.ColorValues[i];
      }
      CreateBrushes();
    }



    public int NumColors
    {
      get
      {
        return NumColorsUsed;
      }
    }



    internal void CreateBrushes()
    {
      for ( int i = 0; i < NumColorsUsed; ++i )
      {
        Colors[i]       = GR.Color.Helper.FromARGB( ColorValues[i] );
        ColorBrushes[i] = new System.Drawing.SolidBrush( Colors[i] );
      }
    }



    internal ByteBuffer GetExportData( int StartIndex, int CountColors, bool Swizzled )
    {
      if ( ( StartIndex < 0 )
      ||   ( StartIndex + CountColors > NumColors ) )
      {
        Debug.Log( "Palette.GetExportData, trying to access out of bounds data" );
        return new ByteBuffer();
      }
      var result = new ByteBuffer();
      result.Reserve( CountColors * 3 );
      for ( int i = 0; i < CountColors; ++i )
      {
        result.AppendU8( (byte)( ( ColorValues[i] & 0x00ff0000 ) >> 16 ) );
      }
      for ( int i = 0; i < CountColors; ++i )
      {
        result.AppendU8( (byte)( ( ColorValues[i] & 0x0000ff00 ) >> 8 ) );
      }
      for ( int i = 0; i < CountColors; ++i )
      {
        result.AppendU8( (byte)( ( ColorValues[i] & 0x000000ff ) ) );
      }
      if ( Swizzled )
      {
        for ( int i = 0; i < result.Length; ++i )
        {
          result.SetU8At( i, SwizzleByte( result.ByteAt( i ) ) );
        }
      }
      return result;
    }


    private byte SwizzleByte( byte Value )
    {
      return (byte)( ( Value >> 4 ) | ( Value << 4 ) );
    }



    public static Palette Read( IReader Reader )
    {
      int     numColors = Reader.ReadInt32();

      var pal = new Palette( numColors );

      for ( int i = 0; i < pal.NumColors; ++i )
      {
        pal.ColorValues[i] = Reader.ReadUInt32();
      }
      pal.CreateBrushes();

      pal.Name = Reader.ReadString();
      return pal;
    }



    public ByteBuffer ToBuffer()
    {
      var chunkPalette = new GR.IO.FileChunk( RetroDevStudio.FileChunkConstants.PALETTE );
      chunkPalette.AppendI32( NumColors );
      for ( int i = 0; i < NumColors; ++i )
      {
        chunkPalette.AppendU32( ColorValues[i] );
      }
      chunkPalette.AppendString( Name );

      return chunkPalette.ToBuffer();
    }



  }
}
