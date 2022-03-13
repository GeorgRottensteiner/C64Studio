using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio.Converter
{
  public class CombinedGraphicsToCharset
  {
    private List<Formats.CharData>      m_Chars = new List<Formats.CharData>();



    private bool CheckCharBox( Formats.GraphicScreenProject Project, Formats.CharData cd, int X, int Y, bool CheckForMC )
    {
      // Match image data
      int chosenCharColor = -1;

      cd.Replacement = null;
      cd.Index = 0;

      // clear data
      for ( int i = 0; i < cd.Tile.Data.Length; ++i )
      {
        cd.Tile.Data.SetU8At( i, 0 );
      }

      bool  isMultiColor = false;

      {
        // determine single/multi color
        bool[] usedColor = new bool[16];
        int numColors = 0;
        bool hasSinglePixel = false;
        bool usedBackgroundColor = false;

        for ( int y = 0; y < 8; ++y )
        {
          for ( int x = 0; x < 8; ++x )
          {
            int colorIndex = (int)Project.Image.GetPixel( X + x, Y + y ) % 16;
            if ( colorIndex >= 16 )
            {
              cd.Error = "Color index >= 16";
              return false;
            }
            if ( ( x % 2 ) == 0 )
            {
              if ( colorIndex != (int)Project.Image.GetPixel( X + x + 1, Y + y ) % 16 )
              {
                // not a double pixel, must be single color then
                hasSinglePixel = true;
              }
            }

            if ( !usedColor[colorIndex] )
            {
              if ( colorIndex == Project.Colors.BackgroundColor )
              {
                usedBackgroundColor = true;
              }
              usedColor[colorIndex] = true;
              numColors++;
            }
          }
        }
        if ( ( hasSinglePixel )
        && ( numColors > 2 ) )
        {
          cd.Error = "Has single pixel, but more than 2 colors";
          return false;
        }
        if ( numColors > 2 )
        {
          isMultiColor = true;
        }
        if ( ( !CheckForMC )
        && ( numColors > 2 ) )
        {
          cd.Error = "Has too many colors";
          return false;
        }
        if ( ( !CheckForMC )
        && ( numColors == 2 )
        && ( !usedBackgroundColor ) )
        {
          cd.Error = "Uses two colors different from background color";
          return false;
        }
        if ( ( hasSinglePixel )
        && ( numColors == 2 )
        && ( !usedBackgroundColor ) )
        {
          cd.Error = "Has single pixel, but more than 2 colors different from background color";
          return false;
        }
        if ( ( CheckForMC )
        && ( !hasSinglePixel )
        && ( numColors > 4 ) )
        {
          cd.Error = "Has more than 4 colors";
          return false;
        }
        if ( ( !hasSinglePixel )
        && ( numColors == 4 )
        && ( !usedBackgroundColor ) )
        {
          cd.Error = "Has more than 4 colors different from background color";
          return false;
        }
        int otherColorIndex = 16;
        if ( ( !hasSinglePixel )
        && ( numColors == 2 )
        && ( usedBackgroundColor ) )
        {
          for ( int i = 0; i < 16; ++i )
          {
            if ( ( usedColor[i] )
            && ( i != Project.Colors.BackgroundColor ) )
            {
              otherColorIndex = i;
              break;
            }
          }
        }
        if ( ( hasSinglePixel )
        || ( !CheckForMC )
        || ( ( numColors == 2 )
        && ( usedBackgroundColor )
        && ( otherColorIndex < 8 ) ) )
        //||   ( numColors == 2 ) )
        {
          // eligible for single color
          isMultiColor = false;
          int usedFreeColor = -1;
          for ( int i = 0; i < 16; ++i )
          {
            if ( usedColor[i] )
            {
              if ( i != Project.Colors.BackgroundColor )
              {
                if ( usedFreeColor != -1 )
                {
                  cd.Error = "More than 1 free color";
                  return false;
                }
                usedFreeColor = i;
              }
            }
          }

          if ( ( hasSinglePixel )
          && ( CheckForMC )
          && ( numColors == 2 )
          && ( usedFreeColor >= 8 ) )
          {
            cd.Error = "Hires char cannot use free color with index " + usedFreeColor;
            return false;
          }

          for ( int y = 0; y < 8; ++y )
          {
            for ( int x = 0; x < 8; ++x )
            {
              int ColorIndex = (int)Project.Image.GetPixel( X + x, Y + y ) % 16;

              int BitPattern = 0;

              if ( ColorIndex != Project.Colors.BackgroundColor )
              {
                BitPattern = 1;
              }

              // noch nicht verwendete Farbe
              if ( BitPattern == 1 )
              {
                chosenCharColor = ColorIndex;
              }
              cd.Tile.Data.SetU8At( y + x / 8, (byte)( cd.Tile.Data.ByteAt( y + x / 8 ) | ( BitPattern << ( ( 7 - ( x % 8 ) ) ) ) ) );
            }
          }
        }
        else
        {
          // multi color
          isMultiColor = true;
          int usedMultiColors = 0;
          int usedFreeColor = -1;
          for ( int i = 0; i < 16; ++i )
          {
            if ( usedColor[i] )
            {
              if ( ( i == Project.Colors.MultiColor1 )
              ||   ( i == Project.Colors.MultiColor2 )
              ||   ( i == Project.Colors.BackgroundColor ) )
              {
                ++usedMultiColors;
              }
              else
              {
                usedFreeColor = i;
              }
            }
          }
          if ( numColors - usedMultiColors > 1 )
          {
            // only one free color allowed
            cd.Error = "More than 1 free color";
            return false;
          }
          if ( usedFreeColor >= 8 )
          {
            cd.Error = "Free color must be of index < 8";
            return false;
          }
          for ( int y = 0; y < 8; ++y )
          {
            for ( int x = 0; x < 4; ++x )
            {
              int ColorIndex = (int)Project.Image.GetPixel( X + 2 * x, Y + y ) % 16;

              byte BitPattern = 0;

              if ( ColorIndex == Project.Colors.BackgroundColor )
              {
                BitPattern = 0x00;
              }
              else if ( ColorIndex == Project.Colors.MultiColor1 )
              {
                BitPattern = 0x01;
              }
              else if ( ColorIndex == Project.Colors.MultiColor2 )
              {
                BitPattern = 0x02;
              }
              else
              {
                // noch nicht verwendete Farbe
                chosenCharColor = usedFreeColor;
                BitPattern = 0x03;
              }
              cd.Tile.Data.SetU8At( y + x / 4, (byte)( cd.Tile.Data.ByteAt( y + x / 4 ) | ( BitPattern << ( ( 3 - ( x % 4 ) ) * 2 ) ) ) );
            }
          }
          if ( usedFreeColor == -1 )
          {
            // only the two multi colors were used, we need to force multi color index though
            chosenCharColor = 8;
          }
        }
      }
      if ( chosenCharColor == -1 )
      {
        chosenCharColor = 0;
      }
      cd.Tile.CustomColor = chosenCharColor;
      if ( ( isMultiColor )
      &&   ( chosenCharColor < 8 ) )
      {
        cd.Tile.CustomColor = chosenCharColor + 8;
      }
      return true;
    }




    private bool CheckForMCCharsetErrors( Formats.GraphicScreenProject Project, int CharIndexOffset )
    {
      bool      foundError = false;
      int blockWidth = ( ( Project.Image.Width + 7 ) / 8 );
      int blockHeight = ( ( Project.Image.Height + 7 ) / 8 );
      for ( int j = 0; j < blockHeight; ++j )
      {
        for ( int i = 0; i < blockWidth; ++i )
        {
          CheckCharBox( Project, m_Chars[CharIndexOffset + i + j * blockWidth], i * 8, j * 8, true );
          if ( m_Chars[CharIndexOffset + i + j * blockWidth].Error.Length > 0 )
          {
            foundError = true;
          }
        }
      }
      if ( foundError )
      {
        return true;
      }
      return false;
    }



    private bool CheckForDuplicates()
    {
      int items = m_Chars.Count;
      int foldedItems = 0;
      int curIndex = 0;

      for ( int index1 = 0; index1 < m_Chars.Count; ++index1 )
      {
        bool wasFolded = false;
        for ( int index2 = 0; index2 < index1; ++index2 )
        {
          if ( m_Chars[index1].Tile.Data.Compare( m_Chars[index2].Tile.Data ) == 0 )
          {
            // same data
            if ( m_Chars[index2].Replacement != null )
            {
              m_Chars[index1].Replacement = m_Chars[index2].Replacement;
            }
            else
            {
              m_Chars[index1].Replacement = m_Chars[index2];
            }
            ++foldedItems;
            wasFolded = true;
            break;
          }
        }
        if ( !wasFolded )
        {
          // item was not folded
          m_Chars[index1].Index = curIndex;
          ++curIndex;
        }
      }
      Debug.Log( "Total chars " + items );
      Debug.Log( "Found " + ( items - foldedItems ) + " unique chars, duplicates removed " + foldedItems );

      if ( items - foldedItems > 256 )
      {
        Debug.Log( "Too many unique chars" );
        return false;
      }
      return true;
    }



    public void ConvertScreens( string BasePath, List<string> ProjectFiles )
    {
      var projects = new List<C64Studio.Formats.GraphicScreenProject>();

      foreach ( var file in ProjectFiles )
      {
        var project = new C64Studio.Formats.GraphicScreenProject();

        project.ReadFromBuffer( GR.IO.File.ReadAllBytes( file ) );

        projects.Add( project );
      }


      int     numChars = 0;

      foreach ( var project in projects )
      {
        numChars += ( ( project.Image.Width + 7 ) / 8 ) * ( ( project.Image.Height + 7 ) / 8 );
      }


      for ( int i = 0; i < numChars; ++i )
      {
        m_Chars.Add( new C64Studio.Formats.CharData() );
      }

      int     curCharOffset = 0;
      int     projectIndex = 0;
      foreach ( var project in projects )
      {
        if ( CheckForMCCharsetErrors( project, curCharOffset ) )
        {
          Debug.Log( "Found error in " + ProjectFiles[projectIndex] );
          return;
        }
        curCharOffset += ( ( project.Image.Width + 7 ) / 8 ) * ( ( project.Image.Height + 7 ) / 8 );
        ++projectIndex;
      }

      if ( CheckForDuplicates() )
      {
        // charset
        GR.Memory.ByteBuffer charSet = new GR.Memory.ByteBuffer();
        foreach ( var charInfo in m_Chars )
        {
          if ( charInfo.Replacement == null )
          {
            charSet.Append( charInfo.Tile.Data );
          }
        }
        GR.IO.File.WriteAllBytes( System.IO.Path.Combine( BasePath, "combined.chr" ), charSet );

        // screens
        int   charIndexOffset = 0;
        projectIndex = 0;
        foreach ( var project in projects )
        {
          // create screens from graphic
          var screen = new C64Studio.Formats.CharsetScreenProject();

          int blockWidth = ( ( project.Image.Width + 7 ) / 8 );
          int blockHeight = ( ( project.Image.Height + 7 ) / 8 );

          screen.SetScreenSize( blockWidth, blockHeight );
          for ( int y = 0; y < blockHeight; ++y )
          {
            for ( int x = 0; x < blockWidth; ++x )
            {
              var charData = m_Chars[charIndexOffset + x + y * blockWidth];
              var origCharData = charData;
              while ( charData.Replacement != null )
              {
                charData = charData.Replacement;
              }

              screen.Chars[x + y * blockWidth] = (ushort)( ( origCharData.Tile.CustomColor << 8 ) + charData.Index );
              screen.Mode = project.MultiColor ? RetroDevStudio.TextMode.COMMODORE_40_X_25_MULTICOLOR : RetroDevStudio.TextMode.COMMODORE_40_X_25_HIRES;
              screen.CharSet.Colors.MultiColor1  = project.Colors.MultiColor1;
              screen.CharSet.Colors.MultiColor2  = project.Colors.MultiColor2;
              screen.CharSet.Colors.BackgroundColor = project.Colors.BackgroundColor;
            }
          }
          screen.CharSet = new C64Studio.Formats.CharsetProject();
          screen.CharSet.Colors.BackgroundColor = project.Colors.BackgroundColor;
          screen.CharSet.Colors.MultiColor1 = project.Colors.MultiColor1;
          screen.CharSet.Colors.MultiColor2 = project.Colors.MultiColor2;

          for ( uint c = 0; c < charSet.Length / 8; ++c )
          {
            screen.CharSet.Characters[(int)c].Tile.Data = charSet.SubBuffer( (int)c * 8, 8 );
            screen.CharSet.Characters[(int)c].Tile.CustomColor = 9;
          }

          string    origFile = System.IO.Path.GetFileNameWithoutExtension( ProjectFiles[projectIndex] );

          GR.IO.File.WriteAllBytes( System.IO.Path.Combine( BasePath, origFile + ".charscreen" ), screen.SaveToBuffer() );

          ++projectIndex;
          charIndexOffset += blockWidth * blockHeight;
        }
      }
    }

  }
}
