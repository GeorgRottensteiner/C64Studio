using GR.Memory;
using System;

namespace RetroDevStudioModels
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
        return NumColorsUsed - 1;
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



    internal ByteBuffer GetExportData( int StartIndex, int CountColors )
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
      return result;
    }



  }
}
