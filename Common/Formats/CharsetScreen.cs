using RetroDevStudio;
using RetroDevStudio.Formats;
using System;
using System.Collections.Generic;
using System.Text;



namespace RetroDevStudio.Formats
{
  public class CharsetScreen
  {
    public string         Name = "Screen"; 
    public int            Width = 40;
    public int            Height = 25;

    public List<uint>     Chars = new List<uint>( 40 * 25 );

    public TextMode       Mode = TextMode.COMMODORE_40_X_25_HIRES;



    public CharsetScreen()
    {
      for ( int j = 0; j < Height * Width; ++j )
      {
        // spaces with white color
        Chars.Add( 0x010020 );
      }
    }



    public ushort CharacterAt( int X, int Y )
    {
      if ( ( X < 0 )
      ||   ( X >= Width )
      ||   ( Y < 0 )
      ||   ( Y >= Height ) )
      {
        return 0;
      }

      return (ushort)( Chars[Y * Width + X] & 0xffff );
    }



    public ushort ColorAt( int X, int Y )
    {
      if ( ( X < 0 )
      ||   ( X >= Width )
      ||   ( Y < 0 )
      ||   ( Y >= Height )
      ||   ( Mode == TextMode.NES ) )
      {
        return 0;
      }

      return (ushort)( Chars[Y * Width + X] >> 16 );
    }



    public int PaletteMappingAt( int X, int Y )
    {
      if ( ( X < 0 )
      ||   ( X >= Width )
      ||   ( Y < 0 )
      ||   ( Y >= Height )
      ||   ( Mode != TextMode.NES ) )
      {
        return 0;
      }

      return (ushort)( ( Chars[Y * Width + X] >> 16 ) & 0x03 );
    }



    public uint CompleteCharAt( int X, int Y )
    {
      if ( ( X < 0 )
      ||   ( X >= Width )
      ||   ( Y < 0 )
      ||   ( Y >= Height )
      ||   ( Mode == TextMode.NES ) )
      {
        return 0;
      }

      return Chars[Y * Width + X];
    }



    public bool SetCharacterAt( int X, int Y, ushort CharValue )
    {
      if ( ( X < 0 )
      ||   ( X >= Width )
      ||   ( Y < 0 )
      ||   ( Y >= Height ) )
      {
        return false;
      }

      Chars[Y * Width + X] = ( Chars[Y * Width + X] & 0xffff0000 ) | CharValue;

      return true;
    }



    public bool SetColorAt( int X, int Y, ushort ColorValue, int PaletteMappingIndex )
    {
      if ( ( X < 0 )
      ||   ( X >= Width )
      ||   ( Y < 0 )
      ||   ( Y >= Height ) )
      {
        return false;
      }

      if ( Mode == TextMode.NES )
      {
        // uses palette mapping index inside color 
        Chars[Y * Width + X] = (uint)( ( Chars[Y * Width + X] & 0xffff ) | (uint)( (uint)PaletteMappingIndex << 16 ) );
      }
      else
      {
        Chars[Y * Width + X] = (uint)( ( Chars[Y * Width + X] & 0xffff ) | (uint)( ColorValue << 16 ) );
      }

      return true;
    }



    public bool SetCompleteCharAt( int X, int Y, uint CompleteCharValue )
    {
      if ( ( X < 0 )
      ||   ( X >= Width )
      ||   ( Y < 0 )
      ||   ( Y >= Height ) )
      {
        return false;
      }

      Chars[Y * Width + X] = CompleteCharValue;
      return true;
    }



    public void SetScreenSize( int Width, int Height )
    {
      if ( ( Width == this.Width )
      &&   ( Height == this.Height ) )
      {
        // nothing to do
        return;
      }
      int     oldWidth = this.Width;
      int     oldHeight = this.Height;

      this.Width = Width;
      this.Height = Height;

      List<uint>    newChars = new List<uint>();

      int     copyWidth = Math.Min( oldWidth, Width );
      int     copyHeight = Math.Min( oldHeight, Height );

      newChars = new List<uint>();
      for ( int i = 0; i < this.Width * this.Height; ++i )
      {
        newChars.Add( (uint)0x010020 );
      }
      // copy over orig chars if possible
      for ( int i = 0; i < copyWidth; ++i )
      {
        for ( int j = 0; j < copyHeight; ++j )
        {
          newChars[i + Width * j] = Chars[i + oldWidth * j];
        }
      }
      Chars = newChars;
    }



    public bool SetAt( int X, int Y, ushort Char, ushort Color, int PaletteMappingIndex )
    {
      if ( ( X < 0 )
      ||   ( X >= Width )
      ||   ( Y < 0 )
      ||   ( Y >= Height ) )
      {
        return false;
      }

      if ( Mode == TextMode.NES )
      {
        // uses palette mapping index inside color 
        Chars[Y * Width + X] = (uint)( (uint)( Char & 0xffff ) | (uint)( (uint)PaletteMappingIndex << 16 ) );
      }
      else
      {
        Chars[Y * Width + X] = (uint)( (uint)( Char & 0xffff ) | (uint)( Color << 16 ) );
      }

      return true;
        
    }



    public uint AssembleChar( ushort Char, ushort Color, int PaletteMappingIndex )
    {
      if ( Mode == TextMode.NES )
      {
        return (uint)( (uint)( Char & 0xffff ) | (uint)( (uint)PaletteMappingIndex << 16 ) );
      }
      return (uint)( (uint)( Char & 0xffff ) | (uint)( (uint)Color << 16 ) );
    }



  }
}
