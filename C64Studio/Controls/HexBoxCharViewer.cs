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
    enum ViewMode
    {
      HIRES,
      MULTI_COLOR,
      ECM
    };


    private ViewMode          m_ViewMode = ViewMode.HIRES;



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

          switch ( m_ViewMode )
          {
            case ViewMode.HIRES:
              CharacterDisplayer.DisplayHiResChar( charData, 1, 0, charImage, 0, 0 );
              break;
            case ViewMode.MULTI_COLOR:
              CharacterDisplayer.DisplayMultiColorChar( charData, 1, 0, 2, 14, charImage, 0, 0 );
              break;
            case ViewMode.ECM:
              // TODO!
              CharacterDisplayer.DisplayHiResChar( charData, 1, 0, charImage, 0, 0 );
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
      switch ( m_ViewMode )
      {
        case ViewMode.HIRES:
          m_ViewMode = ViewMode.MULTI_COLOR;
          break;
        case ViewMode.MULTI_COLOR:
          m_ViewMode = ViewMode.ECM;
          break;
        case ViewMode.ECM:
        default:
          m_ViewMode = ViewMode.HIRES;
          break;
      }
    }

  }
}
