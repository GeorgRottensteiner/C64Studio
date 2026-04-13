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
    public int            ScreenWidth = 40;
    public int            ScreenHeight = 25;

    public List<uint>     Chars = new List<uint>( 40 * 25 );

    public TextMode       Mode = TextMode.COMMODORE_40_X_25_HIRES;



    public CharsetScreen()
    {
      for ( int j = 0; j < ScreenHeight * ScreenWidth; ++j )
      {
        // spaces with white color
        Chars.Add( 0x010020 );
      }
    }



    public ushort CharacterAt( int X, int Y )
    {
      if ( ( X < 0 )
      ||   ( X >= ScreenWidth )
      ||   ( Y < 0 )
      ||   ( Y >= ScreenHeight ) )
      {
        return 0;
      }

      return (ushort)( Chars[Y * ScreenWidth + X] & 0xffff );
    }



    public ushort ColorAt( int X, int Y )
    {
      if ( ( X < 0 )
      ||   ( X >= ScreenWidth )
      ||   ( Y < 0 )
      ||   ( Y >= ScreenHeight )
      ||   ( Mode == TextMode.NES ) )
      {
        return 0;
      }

      return (ushort)( Chars[Y * ScreenWidth + X] >> 16 );
    }



    public int PaletteMappingAt( int X, int Y )
    {
      if ( ( X < 0 )
      ||   ( X >= ScreenWidth )
      ||   ( Y < 0 )
      ||   ( Y >= ScreenHeight )
      ||   ( Mode != TextMode.NES ) )
      {
        return 0;
      }

      return (ushort)( ( Chars[Y * ScreenWidth + X] >> 16 ) & 0x03 );
    }



    public uint CompleteCharAt( int X, int Y )
    {
      if ( ( X < 0 )
      ||   ( X >= ScreenWidth )
      ||   ( Y < 0 )
      ||   ( Y >= ScreenHeight )
      ||   ( Mode == TextMode.NES ) )
      {
        return 0;
      }

      return Chars[Y * ScreenWidth + X];
    }



    public bool SetCharacterAt( int X, int Y, ushort CharValue )
    {
      if ( ( X < 0 )
      ||   ( X >= ScreenWidth )
      ||   ( Y < 0 )
      ||   ( Y >= ScreenHeight ) )
      {
        return false;
      }

      Chars[Y * ScreenWidth + X] = ( Chars[Y * ScreenWidth + X] & 0xffff0000 ) | CharValue;

      return true;
    }



    public bool SetColorAt( int X, int Y, ushort ColorValue, int PaletteMappingIndex )
    {
      if ( ( X < 0 )
      ||   ( X >= ScreenWidth )
      ||   ( Y < 0 )
      ||   ( Y >= ScreenHeight ) )
      {
        return false;
      }

      if ( Mode == TextMode.NES )
      {
        // uses palette mapping index inside color 
        Chars[Y * ScreenWidth + X] = (uint)( ( Chars[Y * ScreenWidth + X] & 0xffff ) | (uint)( (uint)PaletteMappingIndex << 16 ) );
      }
      else
      {
        Chars[Y * ScreenWidth + X] = (uint)( ( Chars[Y * ScreenWidth + X] & 0xffff ) | (uint)( ColorValue << 16 ) );
      }

      return true;
    }



    public bool SetCompleteCharAt( int X, int Y, uint CompleteCharValue )
    {
      if ( ( X < 0 )
      ||   ( X >= ScreenWidth )
      ||   ( Y < 0 )
      ||   ( Y >= ScreenHeight ) )
      {
        return false;
      }

      Chars[Y * ScreenWidth + X] = CompleteCharValue;
      return true;
    }



    public void SetScreenSize( int Width, int Height )
    {
      if ( ( Width == ScreenWidth )
      &&   ( Height == ScreenHeight ) )
      {
        // nothing to do
        return;
      }
      int     oldWidth = ScreenWidth;
      int     oldHeight = ScreenHeight;

      ScreenWidth = Width;
      ScreenHeight = Height;

      List<uint>    newChars = new List<uint>();

      int     copyWidth = Math.Min( oldWidth, Width );
      int     copyHeight = Math.Min( oldHeight, Height );

      newChars = new List<uint>();
      for ( int i = 0; i < ScreenWidth * ScreenHeight; ++i )
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
      ||   ( X >= ScreenWidth )
      ||   ( Y < 0 )
      ||   ( Y >= ScreenHeight ) )
      {
        return false;
      }

      if ( Mode == TextMode.NES )
      {
        // uses palette mapping index inside color 
        Chars[Y * ScreenWidth + X] = (uint)( (uint)( Char & 0xffff ) | (uint)( (uint)PaletteMappingIndex << 16 ) );
      }
      else
      {
        Chars[Y * ScreenWidth + X] = (uint)( (uint)( Char & 0xffff ) | (uint)( Color << 16 ) );
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
