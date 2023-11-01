using RetroDevStudio.Displayer;
using RetroDevStudio;
using System.Drawing;



namespace Be.Windows.Forms
{
  public class HexBoxPETSCIIViewer : ICustomHexViewer
  {
    private enum PETSCIIDisplay
    {
      UPPER_CASE,
      LOWER_CASE
    };

    PETSCIIDisplay      Mode = PETSCIIDisplay.UPPER_CASE;



    public void PaintHexData( HexBox Box, Graphics graphics, long _startByte, long intern_endByte, Rectangle _recHex )
    {
      {
        var oldClip = graphics.Clip;
        graphics.SetClip( _recHex );

        GR.Image.FastImage  charImage = new GR.Image.FastImage( 8, 8, GR.Drawing.PixelFormat.Format32bppRgb );
        PaletteManager.ApplyPalette( charImage );
        int chars = Box.BytesPerLine;

        for ( int i = 0; i < intern_endByte - _startByte; ++i )
        {
          byte character = Box.ByteProvider.ReadByte( _startByte + i );
          int   displayColor = 0;
          int   bgColor = 1;

          if ( ( _startByte + i >= Box.SelectionStart )
          &&   ( _startByte + i < Box.SelectionStart + Box.SelectionLength ) )
          {
            displayColor = 1;
            bgColor = 14;
          }

          switch ( Mode )
          {
            case PETSCIIDisplay.UPPER_CASE:
              CharacterDisplayer.DisplayHiResChar( ConstantData.UpperCaseCharsetC64.SubBuffer( character * 8, 8 ),
                                                   ConstantData.Palette,
                                                   bgColor, displayColor,
                                                   charImage, 0, 0 );
              break;
            case PETSCIIDisplay.LOWER_CASE:
              CharacterDisplayer.DisplayHiResChar( ConstantData.LowerCaseCharsetC64.SubBuffer( character * 8, 8 ),
                                                   ConstantData.Palette,
                                                   bgColor, displayColor,
                                                   charImage, 0, 0 );
              break;
          }

          charImage.DrawToHDC( graphics.GetHdc(),
                               new Rectangle( _recHex.Left + (int)Box.CharSize.Width * ( i % chars ), 
                                              (int)( _recHex.Top + ( i / chars ) * (int)Box.CharSize.Height + ( Box.CharSize.Height - Box.CharSize.Width ) / 2 ), 
                                              (int)Box.CharSize.Width, 
                                              (int)Box.CharSize.Width ) );
          graphics.ReleaseHdc();
        }
        charImage.Dispose();

        graphics.Clip = oldClip;
      }
    }



    internal void ToggleViewMode()
    {
      switch ( Mode )
      {
        case PETSCIIDisplay.UPPER_CASE:
          Mode = PETSCIIDisplay.LOWER_CASE;
          break;
        case PETSCIIDisplay.LOWER_CASE:
        default:
          Mode = PETSCIIDisplay.UPPER_CASE;
          break;
      }
    }

  }
}
