using C64Studio.Displayer;
using GR.Memory;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Be.Windows.Forms
{
  public class HexBoxSpriteViewer : ICustomHexViewer
  {
    enum ViewMode
    {
      HIRES,
      MULTI_COLOR
    };


    private ViewMode          m_ViewMode = ViewMode.HIRES;




    public void PaintHexData( HexBox Box, Graphics graphics, long _startByte, long intern_endByte, Rectangle _recHex )
    {
      //graphics.FillRectangle( new SolidBrush( Color.Beige ), _recHex );

      if ( Box.BytesPerLine == 8 )
      {
        int     spriteSize = (int)( Box.CharSize.Height * 8 ) - 2;
        ByteBuffer    spriteData = new ByteBuffer( 64 );
        int     firstSprite = (int)( _startByte / 64 );
        int     firstTrueByte = firstSprite * 64;
        int     firstLineOffset = (int)( _startByte - firstTrueByte ) / 8;
        int     lastTrueByte = (int)( intern_endByte - _startByte ) + firstTrueByte;
        int     lastSprite = (int)lastTrueByte / 64;
        if ( lastSprite * 64 > intern_endByte )
        {
          ++lastSprite;
        }

        var oldClip = graphics.Clip;
        graphics.SetClip( _recHex );

        for ( int j = firstSprite; j <= lastSprite; ++j )
        {
          for ( int i = 0; i < 64; ++i )
          {
            spriteData.SetU8At( i, Box.ByteProvider.ReadByte( firstTrueByte + ( j - firstSprite ) * 64 + i ) );
          }
          GR.Image.FastImage  spriteImage = new GR.Image.FastImage( 24, 21, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );
          spriteImage.Box( 0, 0, 24, 21, 1 );
          C64Studio.CustomRenderer.PaletteManager.ApplyPalette( spriteImage );

          switch ( m_ViewMode )
          {
            case ViewMode.HIRES:
              SpriteDisplayer.DisplayHiResSprite( spriteData, 1, 0, spriteImage, 0, 0 );
              break;
            case ViewMode.MULTI_COLOR:
              SpriteDisplayer.DisplayMultiColorSprite( spriteData, 1, 0, 2, 6, spriteImage, 0, 0 );
              break;
          }

          int     offsetY = (int)( Box.CharSize.Height * ( j - firstSprite ) * 8 + ( Box.CharSize.Height * 8 - spriteSize ) / 2 ) - (int)( Box.CharSize.Height * firstLineOffset );

          System.Drawing.Image    img = spriteImage.GetAsBitmap();

          graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

          graphics.DrawImage( img, new Rectangle( _recHex.Left, _recHex.Top + offsetY, spriteSize, spriteSize ) );

          img.Dispose();
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
        default:
          m_ViewMode = ViewMode.HIRES;
          break;
      }
    }

  }
}
