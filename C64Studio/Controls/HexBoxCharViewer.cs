using C64Studio.Displayer;
using GR.Memory;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Be.Windows.Forms
{
  public class HexBoxCharViewer : ICustomHexViewer
  {
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

    public C64Studio.Types.CharsetMode Mode
    {
      get;
      set;
    }




    public void PaintHexData( HexBox Box, Graphics graphics, long _startByte, long intern_endByte, Rectangle _recHex )
    {
      if ( Box.BytesPerLine == 8 )
      {
        var oldClip = graphics.Clip;
        graphics.SetClip( _recHex );

        int     boxSize = (int)Box.CharSize.Height - 2;
        ByteBuffer    charData = new ByteBuffer( 8 );
        for ( int j = 0; j < ( intern_endByte - _startByte ) / 8; ++j )
        {
          for ( int i = 0; i < 8; ++i )
          {
            charData.SetU8At( i, Box.ByteProvider.ReadByte( _startByte + j * 8 + i ) );
          }
          GR.Image.FastImage  charImage = new GR.Image.FastImage( 8, 8, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );
          C64Studio.CustomRenderer.PaletteManager.ApplyPalette( charImage );

          switch ( Mode )
          {
            case C64Studio.Types.CharsetMode.HIRES:
              CharacterDisplayer.DisplayHiResChar( charData, BackgroundColor, CustomColor, charImage, 0, 0 );
              break;
            case C64Studio.Types.CharsetMode.MULTICOLOR:
              CharacterDisplayer.DisplayMultiColorChar( charData, BackgroundColor, CustomColor, MultiColor1, MultiColor2, charImage, 0, 0 );
              break;
            case C64Studio.Types.CharsetMode.ECM:
              // TODO!
              CharacterDisplayer.DisplayHiResChar( charData, BackgroundColor, CustomColor, charImage, 0, 0 );
              break;
          }

          charImage.DrawToHDC( graphics.GetHdc(),
                               new Rectangle( _recHex.Left, (int)( _recHex.Top + Box.CharSize.Height * j + ( Box.CharSize.Height - boxSize ) / 2 ), boxSize, boxSize ) );
          graphics.ReleaseHdc();
          charImage.Dispose();
        }

        graphics.Clip = oldClip;
      }
    }



    internal void ToggleViewMode()
    {
      switch ( Mode )
      {
        case C64Studio.Types.CharsetMode.HIRES:
          Mode = C64Studio.Types.CharsetMode.MULTICOLOR;
          break;
        case C64Studio.Types.CharsetMode.MULTICOLOR:
          Mode = C64Studio.Types.CharsetMode.ECM;
          break;
        case C64Studio.Types.CharsetMode.ECM:
        default:
          Mode = C64Studio.Types.CharsetMode.HIRES;
          break;
      }
    }

  }
}
