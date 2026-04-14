using RetroDevStudio.Types;
using RetroDevStudio;
using RetroDevStudio.Formats;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;



namespace RetroDevStudio.Controls
{
  public partial class ExportCharscreenAsImage : ExportCharscreenFormBase
  {
    public ExportCharscreenAsImage() :
      base( null )
    { 
    }



    public ExportCharscreenAsImage( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();
    }



    public override bool HandleExport( ExportCharsetScreenInfo Info, TextBox EditOutput, DocumentInfo DocInfo )
    {
      int     neededWidth   = 0;
      int     neededHeight  = 0;

      int pixelsPerCharacterW = Lookup.CharacterWidthInPixel( Lookup.GraphicTileModeFromTextCharMode( Lookup.TextCharModeFromTextMode( Info.Charscreen.Mode ), 0 ) );
      int pixelsPerCharacterH = Lookup.CharacterHeightInPixel( Lookup.GraphicTileModeFromTextCharMode( Lookup.TextCharModeFromTextMode( Info.Charscreen.Mode ), 0 ) );

      foreach ( var screenIndex in Info.ScreensToExport )
      {
        neededWidth = Math.Max( Info.Charscreen.Screens[screenIndex].Width * pixelsPerCharacterW, neededWidth );
        neededHeight += Info.Charscreen.Screens[screenIndex].Height * pixelsPerCharacterH;
      }
      if ( neededWidth == 0 )
      {
        return false;
      }

      GR.Image.MemoryImage targetImg = new GR.Image.MemoryImage( neededWidth, neededHeight, GR.Drawing.PixelFormat.Format32bppRgb );
      int yOffset = 0;
      var altSettings = new AlternativeColorSettings( Info.Charscreen.CharSet.Colors );

      foreach ( var screenIndex in Info.ScreensToExport )
      {
        var screen = Info.Charscreen.Screens[screenIndex];

        for ( int x = 0; x < screen.Width; ++x )
        {
          for ( int y = 0; y < screen.Height; ++y )
          {
            altSettings.CustomColor = screen.ColorAt( x, y );
            altSettings.PaletteMappingIndex = screen.PaletteMappingAt( x, y );

            Displayer.CharacterDisplayer.DisplayChar( Info.Charscreen.CharSet,
                                                      screen.CharacterAt( x, y ),
                                                      targetImg,
                                                      x * pixelsPerCharacterW,
                                                      yOffset + y * pixelsPerCharacterH,
                                                      altSettings );
          }
        }
        yOffset += screen.Height * pixelsPerCharacterH;
      }

      System.Drawing.Bitmap bmpTarget = targetImg.GetAsBitmap();

      Clipboard.SetImage( bmpTarget );
      bmpTarget.Dispose();

      return true;
    }



  }
}
