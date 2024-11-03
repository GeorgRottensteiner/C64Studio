using RetroDevStudio.Displayer;
using GR.Memory;
using RetroDevStudio;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;



namespace Be.Windows.Forms
{
  public class HexBoxCharViewer : ICustomHexViewer
  {
    public Palette    PaletteC64 = ConstantData.Palette;



    public byte BackgroundColor
    {
      get;
      set;
    }

    public byte CustomColor
    {
      get;
      set;
    }

    public byte MultiColor1
    {
      get;
      set;
    }

    public byte MultiColor2
    {
      get;
      set;
    }

    public byte MultiColor3
    {
      get;
      set;
    }

    public byte MultiColor4
    {
      get;
      set;
    }

    public TextMode Mode
    {
      get;
      set;
    }




    public void PaintHexData( HexBox Box, Graphics graphics, long _startByte, long intern_endByte, Rectangle _recHex )
    {
      if ( Box.BytesPerLine >= 8 )
      {
        var oldClip = graphics.Clip;
        graphics.SetClip( _recHex );

        int     boxSize = (int)Box.CharSize.Height - 2;
        int     charsPerLine = (Box.BytesPerLine + 7) >> 3;
        ByteBuffer    charData = new ByteBuffer( 8 );
        using ( GR.Image.FastImage charImage = new GR.Image.FastImage( charsPerLine * 8, 8, GR.Drawing.PixelFormat.Format32bppRgb ) )
        {
          PaletteManager.ApplyPalette( charImage, PaletteC64 );

          for ( int j = 0; j < ( intern_endByte - _startByte ) / charsPerLine / 8; ++j )
          {
            for (int c = 0; c < charsPerLine; ++c)
            {
              for (int i = 0; i < 8; ++i)
              {
                charData.SetU8At(i, Box.ByteProvider.ReadByte(_startByte + j * charsPerLine * 8 + c * 8 + i));
              }
              int x = 8 * c;
              switch ( Mode )
              {
                case TextMode.COMMODORE_40_X_25_HIRES:
                  CharacterDisplayer.DisplayHiResChar( charData, 8, 8, PaletteC64, BackgroundColor, CustomColor, charImage, x, 0 );
                  break;
                case TextMode.COMMODORE_40_X_25_MULTICOLOR:
                  CharacterDisplayer.DisplayMultiColorChar( charData, 8, 8, PaletteC64, BackgroundColor, CustomColor, MultiColor1, MultiColor2, charImage, x, 0 );
                  break;
                case TextMode.COMMODORE_40_X_25_ECM:
                  // TODO - not correct
                  CharacterDisplayer.DisplayHiResChar( charData, 8, 8, PaletteC64, BackgroundColor, CustomColor, charImage, x, 0 );
                  break;
                default:
                  Debug.Log( "PaintHexData: Missing mode displayer" );
                  break;
              }
            }

            charImage.DrawToHDC( graphics.GetHdc(),
                                 new Rectangle( _recHex.Left, (int)( _recHex.Top + Box.CharSize.Height * j + ( Box.CharSize.Height - boxSize ) / 2 ), boxSize * charsPerLine, boxSize ) );
            graphics.ReleaseHdc();
          }
        }

        graphics.Clip = oldClip;
      }
    }



    internal void ToggleViewMode()
    {
      switch ( Mode )
      {
        case TextMode.COMMODORE_40_X_25_HIRES:
          Mode = TextMode.COMMODORE_40_X_25_MULTICOLOR;
          break;
        case  TextMode.COMMODORE_40_X_25_MULTICOLOR:
          Mode = TextMode.COMMODORE_40_X_25_ECM;
          break;
        case TextMode.COMMODORE_40_X_25_ECM:
        default:
          Mode = TextMode.COMMODORE_40_X_25_HIRES;
          break;
      }
    }

  }
}
