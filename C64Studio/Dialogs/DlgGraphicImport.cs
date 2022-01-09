using RetroDevStudio;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;



namespace C64Studio
{
  public partial class DlgGraphicImport : Form
  {
    private enum ColorMatchType
    {
      [Description( "RGB distance" )]
      RGB_DISTANCE = 0,
      [Description( "HUE distance" )]
      HUE_DISTANCE,
      [Description( "CIE76 distance" )]
      CIE76_DISTANCE
    };

    private class ErrorRange
    {
      public int X = 0;
      public int Y = 0;
      public int W = 8;
      public int H = 8;
      public string Message = "";



      public ErrorRange( string Text )
      {
        Message = Text;
      }



      public ErrorRange( string Text, int X, int Y, int W, int H )
      {
        Message = Text;
        this.X = X;
        this.Y = Y;
        this.W = W;
        this.H = H;
      }
    };



    private StudioCore              Core = null;
    private System.Drawing.Size     m_OrigSize = new Size();

    private GR.Collections.Map<uint,byte>     m_ForcedReplacementColors = new GR.Collections.Map<uint, byte>();

    private Palette                 m_CurPalette = ConstantData.Palette;

    private List<ErrorRange>        m_Errors = new List<ErrorRange>();

    private int                     m_Zoom = 1024;

    private GR.Image.MemoryImage    m_OriginalImage = new GR.Image.MemoryImage( 20, 20, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );
    private GR.Image.MemoryImage    m_ImportImage = new GR.Image.MemoryImage( 20, 20, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );
    private Palette                 m_ImportPalette = null;      

    private string                  m_OrigFilename;

    private uint                    m_OrigReplacementColor = 0;

    public ColorSettings            MultiColorSettings = new ColorSettings();
    public bool                     PasteAsBlock = false;

    private Types.GraphicType       m_ImportType;



    public DlgGraphicImport( StudioCore Core, Types.GraphicType ImportType, GR.Image.FastImage IncomingImage, string Filename, ColorSettings MCSettings )
    {
      this.Core = Core;
      m_ImportType = ImportType;
      m_CurPalette = MCSettings.Palette;
      MultiColorSettings = MCSettings;

      InitializeComponent();

      comboBackground.Items.Add( "[Any]" );
      comboMulticolor1.Items.Add( "[Any]" );
      comboMulticolor2.Items.Add( "[Any]" );
      for ( int i = 0; i < m_CurPalette.NumColors; ++i )
      {
        comboBackground.Items.Add( i.ToString( "d2" ) );
        comboMulticolor1.Items.Add( i.ToString( "d2" ) );
        comboMulticolor2.Items.Add( i.ToString( "d2" ) );
      }

      for ( int i = 0; i < MCSettings.Palettes.Count; ++i )
      {
        comboTargetPalette.Items.Add( MCSettings.Palettes[i].Name );
      }
      if ( ( ImportType == Types.GraphicType.SPRITES_16_COLORS )
      ||   ( ImportType == Types.GraphicType.CHARACTERS_FCM )
      ||   ( ImportType == Types.GraphicType.BITMAP ) )
      {
        comboTargetPalette.Items.Add( "Use incoming palette" );
      }
      comboTargetPalette.SelectedIndex = 0;

      picOriginal.DisplayPage.Create( picOriginal.ClientSize.Width, picOriginal.ClientSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb );

      picPreview.DisplayPage.Create( picPreview.ClientSize.Width, picPreview.ClientSize.Height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );

      PaletteManager.ApplyPalette( picPreview.DisplayPage, m_CurPalette );

      switch ( ImportType )
      {
        case C64Studio.Types.GraphicType.BITMAP:
          comboImportType.Items.Add( new GR.Generic.Tupel<string,C64Studio.Types.GraphicType>( GR.EnumHelper.GetDescription( C64Studio.Types.GraphicType.BITMAP_HIRES ), C64Studio.Types.GraphicType.BITMAP_HIRES ) );
          comboImportType.Items.Add( new GR.Generic.Tupel<string,C64Studio.Types.GraphicType>( GR.EnumHelper.GetDescription( C64Studio.Types.GraphicType.BITMAP_MULTICOLOR ), C64Studio.Types.GraphicType.BITMAP_MULTICOLOR ) );
          comboImportType.SelectedIndex = 1;
          break;
        case C64Studio.Types.GraphicType.CHARACTERS:
          comboImportType.Items.Add( new GR.Generic.Tupel<string, C64Studio.Types.GraphicType>( GR.EnumHelper.GetDescription( C64Studio.Types.GraphicType.CHARACTERS_HIRES ), C64Studio.Types.GraphicType.CHARACTERS_HIRES ) );
          comboImportType.Items.Add( new GR.Generic.Tupel<string, C64Studio.Types.GraphicType>( GR.EnumHelper.GetDescription( C64Studio.Types.GraphicType.CHARACTERS_MULTICOLOR ), C64Studio.Types.GraphicType.CHARACTERS_MULTICOLOR ) );
          comboImportType.Items.Add( new GR.Generic.Tupel<string, C64Studio.Types.GraphicType>( GR.EnumHelper.GetDescription( C64Studio.Types.GraphicType.CHARACTERS_FCM ), C64Studio.Types.GraphicType.CHARACTERS_FCM ) );
          comboImportType.SelectedIndex = 1;
          break;
        default:
          comboImportType.Items.Add( new GR.Generic.Tupel<string,C64Studio.Types.GraphicType>( GR.EnumHelper.GetDescription( ImportType ), ImportType ) );
          comboImportType.SelectedIndex = 0;
          break;
      }
      foreach ( ColorMatchType matchType in System.Enum.GetValues( typeof( ColorMatchType ) ) )
      {
        comboColorMatching.Items.Add( GR.EnumHelper.GetDescription( matchType ) );
      }
      comboColorMatching.SelectedIndex = (int)ColorMatchType.CIE76_DISTANCE;

      m_OrigFilename = Filename;

      if ( string.IsNullOrEmpty( m_OrigFilename ) )
      {
        btnReload.Visible = false;
      }

      OpenImage( IncomingImage );

      comboBackground.SelectedIndex   = MCSettings.BackgroundColor + 1;
      comboMulticolor1.SelectedIndex  = MCSettings.MultiColor1 + 1;
      comboMulticolor2.SelectedIndex  = MCSettings.MultiColor2 + 1;

      Core.Theming.ApplyTheme( this );
    }



    private void ReloadImage()
    {
      if ( string.IsNullOrEmpty( m_OrigFilename ) )
      {
        return;
      }

      GR.Image.FastImage newImage = Core.Imaging.LoadImageFromFile( m_OrigFilename );

      OpenImage( newImage );
    }



    private void openToolStripMenuItem_Click( object sender, EventArgs e )
    {
      OpenFileDialog openDlg = new OpenFileDialog();

      openDlg.Title = "Import image";
      openDlg.Filter = Core.MainForm.FilterString( C64Studio.Types.Constants.FILEFILTER_IMAGE_FILES );
      if ( Core.MainForm.CurrentProject != null )
      {
        openDlg.InitialDirectory = Core.MainForm.CurrentProject.Settings.BasePath;
      }
      if ( ( openDlg.ShowDialog() != DialogResult.OK ) 
      ||   ( string.IsNullOrEmpty( openDlg.FileName ) ) )
      {
        return;
      }

      GR.Image.FastImage newImage = Core.Imaging.LoadImageFromFile( openDlg.FileName );

      OpenImage( newImage );
    }



    private void OpenImage( GR.Image.FastImage newImage )
    {
      if ( newImage == null )
      {
        return;
      }
      m_OriginalImage = new GR.Image.MemoryImage( newImage.Width, newImage.Height, newImage.PixelFormat );
      m_ImportImage = new GR.Image.MemoryImage( newImage.Width, newImage.Height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );

      m_ImportPalette = null;
      if ( newImage.BitsPerPixel <= 8 )
      {
        // an image with palette
        m_ImportPalette = new Palette( 1 << newImage.BitsPerPixel );
        for ( int i = 0; i < m_ImportPalette.NumColors; ++i )
        {
          m_ImportPalette.ColorValues[i] = (uint)( 0xff000000
                  + ( newImage.PaletteRed( i ) << 16 )
                  + ( newImage.PaletteGreen( i ) << 8 )
                  + newImage.PaletteBlue( i ) );
        }
        m_ImportPalette.CreateBrushes();
        PaletteManager.ApplyPalette( m_OriginalImage, m_ImportPalette );
      }
      else
      {
        var quant = new C64Studio.Converter.ColorQuantizer( 256 );

        quant.AddSourceToColorCube( newImage );
        quant.Calculate();

        var resultingImage = quant.Reduce( newImage );
        m_ImportPalette = new Palette( 1 << resultingImage.BitsPerPixel );
        for ( int i = 0; i < m_ImportPalette.NumColors; ++i )
        {
          m_ImportPalette.ColorValues[i] = resultingImage.PaletteColor( i );
        }
        //PaletteManager.ApplyPalette( m_OriginalImage, m_ImportPalette );
      }
      //PaletteManager.ApplyPalette( m_ImportImage );
      //PaletteManager.ApplyPalette( m_OriginalImage );
      newImage.DrawTo( m_OriginalImage, 0, 0 );
      newImage.Dispose();

      if ( m_OriginalImage.PixelFormat != picOriginal.DisplayPage.PixelFormat )
      {
        picOriginal.DisplayPage.Create( picOriginal.ClientSize.Width, picOriginal.ClientSize.Height, m_OriginalImage.PixelFormat );

        if ( m_ImportPalette != null )
        {
          PaletteManager.ApplyPalette( picOriginal.DisplayPage, m_ImportPalette );
        }
      }
      picOriginal.DisplayPage.Box( 0, 0, picOriginal.DisplayPage.Width, picOriginal.DisplayPage.Height, 0 );

      // determine optimal zoom size
      m_Zoom = 1024;
      while ( ( m_OriginalImage.Width * 1024 / m_Zoom < 64 )
      &&      ( m_OriginalImage.Height * 1024 / m_Zoom < 64 ) )
      {
        m_Zoom /= 2;
      }
      picOriginal.SetImageSize( picOriginal.ClientSize.Width * m_Zoom / 1024, picOriginal.ClientSize.Height * m_Zoom / 1024 );
      picPreview.SetImageSize( picPreview.ClientSize.Width * m_Zoom / 1024, picPreview.ClientSize.Height * m_Zoom / 1024 );

      picOriginal.DisplayPage.DrawImage( m_OriginalImage, 0, 0 );

      m_OrigSize.Width  = m_OriginalImage.Width;
      m_OrigSize.Height = m_OriginalImage.Height;

      RecalcImport();
    }



    int MatchColor( byte R, byte G, byte B, Palette Palette )
    {
      int bestMatchDistance = 50000000;
      int bestMatch = -1;

      ColorMatchType    matchType = (ColorMatchType)comboColorMatching.SelectedIndex;

      ColorSystem.RGB   origColor = new ColorSystem.RGB( R, G, B );
      
      for ( int k = 0; k < Palette.NumColors; ++k )
      {
        switch ( matchType )
        {
          case ColorMatchType.RGB_DISTANCE:
            {
              int distR = R - (int)( ( Palette.ColorValues[k] & 0xff0000 ) >> 16 );
              int distG = G - (int)( ( Palette.ColorValues[k] & 0x00ff00 ) >> 8 );
              int distB = B - (int)( Palette.ColorValues[k] & 0xff );
              //int distance = (int)( distR * distR * 0.3f + distG * distG * 0.6f + distB * distB * 0.1f );
              int distance = (int)( distR * distR + distG * distG + distB * distB );

              if ( distance < bestMatchDistance )
              {
                bestMatchDistance = distance;
                bestMatch = k;
              }
            }
            break;
          case ColorMatchType.HUE_DISTANCE:
            {
              ColorSystem.HSV myHSV = ColorSystem.RGBToHSV( origColor );
              ColorSystem.HSV otherHSV = ColorSystem.RGBToHSV( new ColorSystem.RGB( (byte)( ( Palette.ColorValues[k] & 0xff0000 ) >> 16 ), (byte)( ( Palette.ColorValues[k] & 0x00ff00 ) >> 8 ), (byte)( Palette.ColorValues[k] & 0xff ) ) );

              int distance = Math.Abs( (int)( otherHSV.H - myHSV.H ) );
              distance = Math.Min( distance, Math.Abs( (int)( otherHSV.H + 360.0 - myHSV.H ) ) );
              distance = Math.Min( distance, Math.Abs( (int)( otherHSV.H - 360.0 - myHSV.H ) ) );

              distance *= distance;
              distance += (int)( 255.0f * Math.Abs( myHSV.V - otherHSV.V ) ) * (int)( 255.0f * Math.Abs( myHSV.V - otherHSV.V ) );
              distance += (int)( 255.0f * Math.Abs( myHSV.S - otherHSV.S ) ) * (int)( 255.0f * Math.Abs( myHSV.S - otherHSV.S ) );

              if ( distance < bestMatchDistance )
              {
                bestMatchDistance = distance;
                bestMatch = k;
              }
            }
            break;
          case ColorMatchType.CIE76_DISTANCE:
            {
              ColorSystem.CIELab myLab = ColorSystem.RGBToCIELab( origColor );
              ColorSystem.CIELab otherLab = ColorSystem.RGBToCIELab( new ColorSystem.RGB( (byte)( ( Palette.ColorValues[k] & 0xff0000 ) >> 16 ), (byte)( ( Palette.ColorValues[k] & 0x00ff00 ) >> 8 ), (byte)( Palette.ColorValues[k] & 0xff ) ) );

              float distL = ( myLab.L - otherLab.L ) * ( myLab.L - otherLab.L );
              float dista = ( myLab.a - otherLab.a ) * ( myLab.a - otherLab.a );
              float distb = ( myLab.b - otherLab.b ) * ( myLab.b - otherLab.b );

              int distance = (int)( distL + dista + distb );

              if ( distance < bestMatchDistance )
              {
                bestMatchDistance = distance;
                bestMatch = k;
              }
            }
            break;
        }
      }
      return bestMatch;
    }



    bool CheckColors()
    {
      // can all colors be matched to the palette?
      GR.Collections.Map<uint, byte> matchedColors = new GR.Collections.Map<uint, byte>();

      for ( int i = 0; i < m_OrigSize.Width; ++i )
      {
        for ( int j = 0; j < m_OrigSize.Height; ++j )
        {
          uint    pixelValue = m_OriginalImage.GetPixel( i, j );

          byte    matchedColor = 0;

          if ( matchedColors.ContainsKey( pixelValue ) )
          {
            matchedColor = matchedColors[pixelValue];
            m_ImportImage.SetPixel( i, j, matchedColor );
          }
          else
          {
            // forced matches
            if ( m_ForcedReplacementColors.ContainsKey( pixelValue ) )
            {
              matchedColor                = m_ForcedReplacementColors[pixelValue];
              matchedColors[pixelValue]   = matchedColor;
              m_ImportImage.SetPixel( i, j, matchedColor );
              continue;
            }

            // match
            byte red    = (byte)( ( pixelValue & 0xff0000 ) >> 16 );
            byte green  = (byte)( ( pixelValue & 0x00ff00 ) >> 8 );
            byte blue   = (byte)( pixelValue & 0xff );

            if ( ( picOriginal.DisplayPage.PixelFormat == System.Drawing.Imaging.PixelFormat.Format4bppIndexed )
            ||   ( picOriginal.DisplayPage.PixelFormat == System.Drawing.Imaging.PixelFormat.Format1bppIndexed )
            ||   ( picOriginal.DisplayPage.PixelFormat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed ) )
            {
              red   = picOriginal.DisplayPage.PaletteRed( (int)pixelValue );
              green = picOriginal.DisplayPage.PaletteGreen( (int)pixelValue );
              blue  = picOriginal.DisplayPage.PaletteBlue( (int)pixelValue );
            }


            //ColorSystem.RGB rgb = new ColorSystem.RGB( red, green, blue );

            // HSV-system (painter!)
            int bestMatch = (byte)pixelValue;
            if ( comboTargetPalette.SelectedIndex < MultiColorSettings.Palettes.Count )
            {
              bestMatch = MatchColor( red, green, blue, m_CurPalette );
            }
            /*
            int bestMatchDistance = 50000000;
            int bestMatch = -1;
            for ( int k = 0; k < 16; ++k )
            {
              int distR = red - (int)( ( m_CurPalette.ColorValues[k] & 0xff0000 ) >> 16 );
              int distG = green - (int)( ( m_CurPalette.ColorValues[k] & 0x00ff00 ) >> 8 );
              int distB = blue - (int)( m_CurPalette.ColorValues[k] & 0xff );
              //int distance = (int)( distR * distR * 0.3f + distG * distG * 0.6f + distB * distB * 0.1f );
              int distance = (int)( distR * distR + distG * distG + distB * distB );

              if ( distance < bestMatchDistance )
              {
                bestMatchDistance = distance;
                bestMatch = k;
              }
            }*/


            if ( bestMatch == -1 )
            {
              m_ImportImage.SetPixel( i, j, 16 );
            }
            else
            {
              matchedColor              = (byte)bestMatch;
              matchedColors[pixelValue] = matchedColor;

              m_ImportImage.SetPixel( i, j, matchedColor );
            }
          }
        }
      }
      picPreview.DisplayPage.DrawImage( m_ImportImage, 0, 0 );
      picPreview.Invalidate();
      return true;
    }



    public GR.Image.FastImage ConvertedImage
    {
      get
      {
        return new GR.Image.FastImage( m_ImportImage );
      }
    }



    private void AddError( string Message )
    {
      m_Errors.Add( new ErrorRange( Message ) );

      ListViewItem item = new ListViewItem( "" );
      item.SubItems.Add( Message );
      listProblems.Items.Add( item );
    }



    private void AddError( string Message, int X, int Y, int W, int H )
    {
      m_Errors.Add( new ErrorRange( Message, X, Y, W, H ) );

      ListViewItem item = new ListViewItem( X + "," + Y + "," + W + "x" + H );
      item.SubItems.Add( Message );
      listProblems.Items.Add( item );
    }



    private void AddError( string Message, int X, int Y )
    {
      m_Errors.Add( new ErrorRange( Message, X, Y, -1, -1 ) );

      ListViewItem item = new ListViewItem( X + "," + Y );
      item.SubItems.Add( Message );
      listProblems.Items.Add( item );
    }



    private void CheckAsCharacters( bool MultiColor )
    {
      for ( int sy = 0; sy < ( m_OrigSize.Height + 7 ) / 8; ++sy )
      {
        for ( int sx = 0; sx < ( m_OrigSize.Width + 7 ) / 8; ++sx )
        {
          // determine single/multi color
          bool[] usedColor = new bool[16];
          int numColors = 0;
          bool hasSinglePixel = false;
          bool usedBackgroundColor = false;
          int determinedBackgroundColor = comboBackground.SelectedIndex - 1;
          int determinedMultiColor1 = comboMulticolor1.SelectedIndex - 1;
          int determinedMultiColor2 = comboMulticolor2.SelectedIndex - 1;

          for ( int y = 0; y < 8; ++y )
          {
            for ( int x = 0; x < 8; ++x )
            {
              int colorIndex = (int)picPreview.DisplayPage.GetPixelData( sx * 8 + x, sy * 8 + y );
              if ( colorIndex >= 16 )
              {
                AddError( "Encountered color index >= 16 at " + x + "," + y, sx * 8 + x, sy * 8 + y, 1, 1 );
              }
              else
              {
                if ( ( x % 2 ) == 0 )
                {
                  if ( colorIndex != (int)picPreview.DisplayPage.GetPixelData( sx * 8 + x + 1, sy * 8 + y ) )
                  {
                    // not a double pixel, must be single color then
                    hasSinglePixel = true;
                  }
                }

                if ( !usedColor[colorIndex] )
                {
                  if ( ( determinedBackgroundColor != -1 )
                  &&   ( colorIndex == determinedBackgroundColor ) )
                  {
                    usedBackgroundColor = true;
                  }
                  usedColor[colorIndex] = true;
                  numColors++;
                }
              }
            }
          }
          //bool couldBeHiresInMC = false;
          if ( ( MultiColor )
          &&   ( hasSinglePixel ) )
          {
            bool  safeUseOfHires = false;

            if ( ( numColors == 2 )
            &&   ( usedBackgroundColor ) )
            {
              // using a hires color is ok
              for ( int i = 0; i < 8; ++i )
              {
                if ( ( usedColor[i] )
                &&   ( i != determinedBackgroundColor ) )
                {
                  safeUseOfHires = true;
                  break;
                }
              }
            }
            if ( !safeUseOfHires )
            {
              AddError( "Chosen multicolor, but has a single pixel", sx * 8, sy * 8, 8, 8 );
              continue;
            }
          }
          if ( ( hasSinglePixel )
          &&   ( numColors > 2 ) )
          {
            AddError( "Has a single pixel, but more than two colors", sx * 8, sy * 8, 8, 8 );
            continue;
          }
          if ( ( !MultiColor )
          &&   ( numColors > 2 ) )
          {
            AddError( "Chosen HiRes, but more than two colors", sx * 8, sy * 8, 8, 8 );
            continue;
          }
          if ( ( hasSinglePixel )
          &&   ( numColors <= 2 ) )
          {
            if ( determinedBackgroundColor == -1 )
            {
              // set background color as one of the 2 found, prefer 0, than the lower one
              if ( usedColor[0] )
              {
                determinedBackgroundColor = 0;
              }
              else
              {
                for ( int i = 0; i < 16; ++i )
                {
                  if ( usedColor[i] )
                  {
                    determinedBackgroundColor = i;
                    break;
                  }
                }
              }
            }
            else if ( numColors == 2 )
            {
              bool    usedBGColor = false;
              for ( int i = 0; i < 16; ++i )
              {
                if ( ( usedColor[i] )
                &&   ( i == determinedBackgroundColor ) )
                {
                  usedBGColor = true;
                  break;
                }
              }
              if ( !usedBGColor )
              {
                AddError( "Has a single pixel, but two colors, and none is the background color", sx * 8, sy * 8, 8, 8 );
                continue;
              }
            }
            // is hires in multicolor?
            for ( int i = 0; i < 16; ++i )
            {
              if ( ( usedColor[i] )
              &&   ( i != determinedBackgroundColor ) )
              {
                if ( i < 8 )
                {
                  //couldBeHiresInMC = true;
                  break;
                }
              }
            }
          }
          if ( ( determinedBackgroundColor != -1 )
          &&   ( !usedBackgroundColor )
          &&   ( numColors >= 4 ) )
          {
            AddError( "Looks like single color, but doesn't use the set background color and there are no more free custom colors.", sx * 8, sy * 8, 8, 8 );
            return;
          }
          if ( ( !hasSinglePixel )
          &&   ( numColors > 4 ) )
          {
            AddError( "Uses more than 4 colors", sx * 8, sy * 8, 8, 8 );
            continue;
          }
          if ( ( !hasSinglePixel )
          &&   ( numColors <= 4 ) )
          {
            if ( ( usedColor[0] )
            &&   ( determinedBackgroundColor == -1 ) )
            {
              determinedBackgroundColor = 0;
            }
            else
            {
              for ( int i = 0; i < 16; ++i )
              {
                if ( usedColor[i] )
                {
                  if ( determinedBackgroundColor == -1 )
                  {
                    determinedBackgroundColor = i;
                  }
                  else if ( determinedMultiColor1 == -1 )
                  {
                    determinedMultiColor1 = i;
                  }
                  else if ( determinedMultiColor2 == -1 )
                  {
                    determinedMultiColor2 = i;
                  }
                }
              }
            }
          }
          if ( ( determinedBackgroundColor != -1 )
          &&   ( numColors == 4 )
          &&   ( !usedBackgroundColor ) )
          {
            AddError( "Uses 4 colors, but doesn't use the set background color", sx * 8, sy * 8, 8, 8 );
            return;
          }
          if ( ( hasSinglePixel )
          ||   ( numColors == 2 ) )
          {
            // eligible for single color
            int usedFreeColor = -1;
            for ( int i = 0; i < 16; ++i )
            {
              if ( usedColor[i] )
              {
                if ( i != determinedBackgroundColor )
                {
                  if ( ( ( MultiColor )
                  &&     ( i != determinedMultiColor1 )
                  &&     ( i != determinedMultiColor2 ) )
                  ||   ( !MultiColor ) )
                  {
                    if ( usedFreeColor != -1 )
                    {
                      AddError( "Uses more than one free color", sx * 8, sy * 8, 8, 8 );
                      continue;
                    }
                    usedFreeColor = i;
                  }
                }
              }
            }
            if ( ( MultiColor )
            &&   ( usedFreeColor >= 8 ) )
            {
              AddError( "Chosen multi color but used free color with index >= 8", sx * 8, sy * 8, 8, 8 );
              continue;
            }
          }
          else
          {
            // multi color
            int usedMultiColors = 0;
            int usedFreeColor = -1;
            for ( int i = 0; i < 16; ++i )
            {
              if ( usedColor[i] )
              {
                if ( ( i == determinedMultiColor1 )
                || ( i == determinedMultiColor2 )
                || ( i == determinedBackgroundColor ) )
                {
                  ++usedMultiColors;
                }
                else
                {
                  usedFreeColor = i;
                }
              }
            }
            if ( ( MultiColor )
            &&   ( usedFreeColor >= 8 ) )
            {
              AddError( "Chosen multi color but used free color with index >= 8", sx * 8, sy * 8, 8, 8 );
              continue;
            }

            if ( numColors - usedMultiColors > 1 )
            {
              // only one free color allowed
              AddError( "Uses more than one free color", sx * 8, sy * 8, 8, 8 );
              continue;
            }
          }
        }
      }

    }



    private void CheckAsHiResBitmap()
    {
      for ( int sy = 0; sy < ( m_OrigSize.Height + 7 ) / 8; ++sy )
      {
        for ( int sx = 0; sx < ( m_OrigSize.Width + 7 ) / 8; ++sx )
        {
          // determine single/multi color
          bool[] usedColor = new bool[16];
          int numColors = 0;
          bool usedBackgroundColor = false;
          int determinedBackgroundColor = comboBackground.SelectedIndex - 1;

          for ( int y = 0; y < 8; ++y )
          {
            for ( int x = 0; x < 8; ++x )
            {
              int colorIndex = (int)picPreview.DisplayPage.GetPixelData( sx * 8 + x, sy * 8 + y );
              if ( colorIndex >= 16 )
              {
                AddError( "Encountered color index >= 16 at " + x + "," + y, sx * 8 + x, sy * 8 + y, 1, 1 );
              }
              else
              {
                if ( !usedColor[colorIndex] )
                {
                  if ( ( determinedBackgroundColor != -1 )
                  &&   ( colorIndex == determinedBackgroundColor ) )
                  {
                    usedBackgroundColor = true;
                  }
                  usedColor[colorIndex] = true;
                  numColors++;
                }
              }
            }
          }
          if ( numColors > 2 )
          {
            AddError( "Uses more than two colors", sx * 8, sy * 8, 8, 8 );
            continue;
          }
          /*
          if ( determinedBackgroundColor == -1 )
          {
            // set background color as one of the 2 found, prefer 0, then the lower one
            if ( usedColor[0] )
            {
              determinedBackgroundColor = 0;
            }
            else
            {
              for ( int i = 0; i < 16; ++i )
              {
                if ( usedColor[i] )
                {
                  determinedBackgroundColor = i;
                  break;
                }
              }
            }
          }*/
          if ( ( determinedBackgroundColor != -1 )
          &&   ( !usedBackgroundColor )
          &&   ( numColors >= 2 ) )
          {
            AddError( "Looks like single color, but doesn't use the set background color", sx * 8, sy * 8, 8, 8 );
            continue;
          }
          int usedFreeColor = -1;
          for ( int i = 0; i < 16; ++i )
          {
            if ( usedColor[i] )
            {
              if ( ( i != determinedBackgroundColor )
              &&   ( determinedBackgroundColor != -1 ) )
              {
                if ( usedFreeColor != -1 )
                {
                  AddError( "Uses more than one free color", sx * 8, sy * 8, 8, 8 );
                  continue;
                }
                usedFreeColor = i;
              }
            }
          }
        }
      }
    }



    private void CheckAsMCBitmap()
    {
      for ( int sy = 0; sy < ( m_OrigSize.Height + 7 ) / 8; ++sy )
      {
        for ( int sx = 0; sx < ( m_OrigSize.Width + 7 ) / 8; ++sx )
        {
          // determine single/multi color
          bool[] usedColor = new bool[16];
          int numColors = 0;
          bool hasSinglePixel = false;
          bool usedBackgroundColor = false;
          int determinedBackgroundColor = comboBackground.SelectedIndex - 1;
          int determinedMultiColor1 = comboMulticolor1.SelectedIndex - 1;
          int determinedMultiColor2 = comboMulticolor2.SelectedIndex - 1;

          for ( int y = 0; y < 8; ++y )
          {
            for ( int x = 0; x < 8; ++x )
            {
              int colorIndex = (int)picPreview.DisplayPage.GetPixelData( sx * 8 + x, sy * 8 + y );
              if ( colorIndex >= 16 )
              {
                AddError( "Encountered color index >= 16 at " + x + "," + y, sx * 8 + x, sy * 8 + y, 1, 1 );
              }
              else
              {
                if ( ( x % 2 ) == 0 )
                {
                  if ( colorIndex != (int)picPreview.DisplayPage.GetPixelData( sx * 8 + x + 1, sy * 8 + y ) )
                  {
                    // not a double pixel, must be single color then
                    hasSinglePixel = true;
                  }
                }

                if ( !usedColor[colorIndex] )
                {
                  if ( ( determinedBackgroundColor != -1 )
                  && ( colorIndex == determinedBackgroundColor ) )
                  {
                    usedBackgroundColor = true;
                  }
                  usedColor[colorIndex] = true;
                  numColors++;
                }
              }
            }
          }
          //bool couldBeHiresInMC = false;
          if ( hasSinglePixel )
          {
            AddError( "Has a single pixel", sx * 8, sy * 8, 8, 8 );
            continue;
          }
          if ( ( determinedBackgroundColor != -1 )
          &&   ( !usedBackgroundColor )
          &&   ( numColors >= 4 ) )
          {
            AddError( "Uses more than 3 colors, but doesn't use the set background color and there are no more free custom colors.", sx * 8, sy * 8, 8, 8 );
            continue;
          }
          if ( numColors > 4 )
          {
            AddError( "Uses more than 4 colors", sx * 8, sy * 8, 8, 8 );
            continue;
          }
          if ( ( determinedBackgroundColor == -1 )
          &&   ( usedColor[0] ) )
          {
            // prefer 0 as back ground color
            determinedBackgroundColor = 0;
          }
          else
          {
            int     foundColorIndex = 0;

            for ( int i = 0; i < 16; ++i )
            {
              if ( usedColor[i] )
              {
                switch ( foundColorIndex )
                {
                  case 0:
                    determinedBackgroundColor = i;
                    break;
                  case 1:
                    determinedMultiColor1 = i;
                    break;
                  case 2:
                    determinedMultiColor2 = i;
                    break;
                }
              }
            }
          }
          if ( ( determinedBackgroundColor != -1 )
          &&   ( numColors == 4 )
          &&   ( !usedBackgroundColor ) )
          {
            AddError( "Uses 4 colors, but doesn't use the set background color", sx * 8, sy * 8, 8, 8 );
            return;
          }
          // multi color
          int usedMultiColors = 0;
          int usedFreeColor = -1;
          for ( int i = 0; i < 16; ++i )
          {
            if ( usedColor[i] )
            {
              if ( ( i == determinedMultiColor1 )
              ||   ( i == determinedMultiColor2 )
              ||   ( i == determinedBackgroundColor ) )
              {
                ++usedMultiColors;
              }
              else if ( determinedMultiColor1 == -1 )
              {
                determinedMultiColor1 = i;
                ++usedMultiColors;
              }
              else if ( determinedMultiColor2 == -1 )
              {
                determinedMultiColor2 = i;
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
            AddError( "Uses more than one free color", sx * 8, sy * 8, 8, 8 );
            continue;
          }
        }
      }
    }



    private void CheckAsSprites()
    {
      for ( int sy = 0; sy < ( m_OrigSize.Height + 20 ) / 21; ++sy )
      {
        for ( int sx = 0; sx < ( m_OrigSize.Width + 23 ) / 24; ++sx )
        {
          // determine single/multi color
          bool[] usedColor = new bool[16];
          int numColors = 0;
          bool hasSinglePixel = false;
          bool usedBackgroundColor = false;
          int determinedBackgroundColor = comboBackground.SelectedIndex - 1;
          int determinedMultiColor1 = comboMulticolor1.SelectedIndex - 1;
          int determinedMultiColor2 = comboMulticolor2.SelectedIndex - 1;

          for ( int y = 0; y < 21; ++y )
          {
            for ( int x = 0; x < 24; ++x )
            {
              int colorIndex = (int)picPreview.DisplayPage.GetPixelData( sx * 24 + x, sy * 21 + y );
              if ( colorIndex >= 16 )
              {
                AddError( "Encountered color index >= 16 at " + x + "," + y, sx * 24 + x, sy * 21 + y, 1, 1 );
              }
              else
              {
                if ( ( x % 2 ) == 0 )
                {
                  if ( colorIndex != (int)picPreview.DisplayPage.GetPixelData( sx * 24 + x + 1, sy * 21 + y ) )
                  {
                    // not a double pixel, must be single color then
                    hasSinglePixel = true;
                  }
                }

                if ( !usedColor[colorIndex] )
                {
                  if ( ( determinedBackgroundColor != -1 )
                  &&   ( colorIndex == determinedBackgroundColor ) )
                  {
                    usedBackgroundColor = true;
                  }
                  usedColor[colorIndex] = true;
                  numColors++;
                }
              }
            }
          }
          if ( ( hasSinglePixel )
          &&   ( numColors > 2 ) )
          {
            AddError( "Has a single pixel, but more than two colors", sx * 24, sy * 21, 24, 21 );
            continue;
          }
          if ( ( hasSinglePixel )
          &&   ( numColors <= 2 ) )
          {
            if ( determinedBackgroundColor == -1 )
            {
              // set background color as one of the 2 found, prefer 0, than the lower one
              if ( usedColor[0] )
              {
                determinedBackgroundColor = 0;
                usedBackgroundColor = true;
              }
              else
              {
                for ( int i = 0; i < 16; ++i )
                {
                  if ( usedColor[i] )
                  {
                    determinedBackgroundColor = i;
                    usedBackgroundColor = true;
                    break;
                  }
                }
              }
            }
          }
          /*
          // this check is only useful for hires sprites
          if ( ( determinedBackgroundColor > -1 )
          &&   ( numColors > 1 )
          &&   ( !usedBackgroundColor ) )
          {
            AddError( "Looks like single color, but doesn't use the set background color", sx * 24, sy * 21, 24, 21 );
            return;
          }*/
          if ( ( !hasSinglePixel )
          &&   ( numColors > 4 ) )
          {
            AddError( "Uses more than 4 colors", sx * 24, sy * 21, 24, 21 );
            continue;
          }
          if ( ( !hasSinglePixel )
          &&   ( numColors <= 4 ) )
          {
            int     foundColorIndex = 0;

            for ( int i = 0; i < 16; ++i )
            {
              if ( usedColor[i] )
              {
                while ( foundColorIndex < 3 )
                {
                  if ( ( foundColorIndex == 0 )
                  &&   ( determinedBackgroundColor == -1 ) )
                  {
                    determinedBackgroundColor = i;
                    usedBackgroundColor = true;
                    break;
                  }
                  if ( ( foundColorIndex == 0 )
                  &&   ( determinedMultiColor1 == -1 ) )
                  {
                    determinedMultiColor1 = i;
                    break;
                  }
                  if ( ( foundColorIndex == 0 )
                  &&   ( determinedMultiColor2 == -1 ) )
                  {
                    determinedMultiColor2 = i;
                    break;
                  }
                  ++foundColorIndex;
                }
              }
            }
          }
          if ( ( determinedBackgroundColor != -1 )
          &&   ( numColors == 4 )
          &&   ( !usedBackgroundColor ) )
          {
            AddError( "Uses 4 colors, but doesn't use the set background color", sx * 24, sy * 21, 24, 21 );
            return;
          }
          if ( ( hasSinglePixel )
          ||   ( numColors == 2 ) )
          {
            // eligible for single color
            int usedFreeColor = -1;
            int usedMultiColors = 0;
            for ( int i = 0; i < 16; ++i )
            {
              if ( usedColor[i] )
              {
                if ( ( i == determinedMultiColor1 )
                ||   ( i == determinedMultiColor2 ) )
                {
                  ++usedMultiColors;
                  continue;
                }
              }
            }
            for ( int i = 0; i < 16; ++i )
            {
              if ( usedColor[i] )
              {
                if ( ( i != determinedBackgroundColor )
                &&   ( i != determinedMultiColor1 )
                &&   ( i != determinedMultiColor2 ) )
                {
                  if ( usedFreeColor != -1 )
                  {
                    AddError( "Uses more than one free color", sx * 24, sy * 21, 24, 21 );
                    continue;
                  }
                  usedFreeColor = i;
                }
              }
            }
          }
          else
          {
            // multi color
            int usedMultiColors = 0;
            int usedFreeColor = -1;
            for ( int i = 0; i < 16; ++i )
            {
              if ( usedColor[i] )
              {
                if ( ( i == determinedMultiColor1 )
                ||   ( i == determinedMultiColor2 )
                ||   ( i == determinedBackgroundColor ) )
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
              AddError( "Uses more than one free color", sx * 24, sy * 21, 24, 21 );
              continue;
            }
          }
        }
      }
    }



    private void RecalcImport()
    {
      if ( comboImportType.SelectedItem == null )
      {
        return;
      }
      if ( !CheckColors() )
      {
        return;
      }
      m_Errors.Clear();
      listProblems.Items.Clear();
      listProblems.BeginUpdate();
      switch ( ( (GR.Generic.Tupel<string,Types.GraphicType>)comboImportType.SelectedItem ).second )
      { 
        case C64Studio.Types.GraphicType.CHARACTERS:
        case C64Studio.Types.GraphicType.CHARACTERS_HIRES:
          CheckAsCharacters( false );
          break;
        case C64Studio.Types.GraphicType.CHARACTERS_MULTICOLOR:
          CheckAsCharacters( true );
          break;
        case C64Studio.Types.GraphicType.CHARACTERS_FCM:
          // nothing to do, there are no limits
          break;
        case C64Studio.Types.GraphicType.SPRITES:
          CheckAsSprites();
          break;
        case C64Studio.Types.GraphicType.BITMAP_HIRES:
          CheckAsHiResBitmap();
          break;
        case C64Studio.Types.GraphicType.BITMAP_MULTICOLOR:
          CheckAsMCBitmap();
          break;
      }
      listProblems.EndUpdate();
    }



    private void exitToolStripMenuItem_Click( object sender, EventArgs e )
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }



    private void combo_DrawItem( object sender, DrawItemEventArgs e )
    {
      ComboBox combo = (ComboBox)sender;

      e.DrawBackground();
      if ( e.Index == -1 )
      {
        return;
      }
      System.Drawing.Rectangle itemRect = new System.Drawing.Rectangle( e.Bounds.Left + 20, e.Bounds.Top, e.Bounds.Width - 20, e.Bounds.Height );
      if ( e.Index == 0 )
      {
        // any
        int colorIndex = e.Index;
        if ( ( e.State & DrawItemState.Selected ) != 0 )
        {
          e.Graphics.DrawString( combo.Items[colorIndex].ToString(), combo.Font, new System.Drawing.SolidBrush( System.Drawing.Color.White ), 3.0f, e.Bounds.Top + 1.0f );
        }
        else
        {
          e.Graphics.DrawString( combo.Items[colorIndex].ToString(), combo.Font, new System.Drawing.SolidBrush( System.Drawing.Color.Black ), 3.0f, e.Bounds.Top + 1.0f );
        }
      }
      else
      {
        int colorIndex = e.Index - 1;
        e.Graphics.FillRectangle( m_CurPalette.ColorBrushes[colorIndex], itemRect );
        if ( ( e.State & DrawItemState.Selected ) != 0 )
        {
          e.Graphics.DrawString( combo.Items[colorIndex + 1].ToString(), combo.Font, new System.Drawing.SolidBrush( System.Drawing.Color.White ), 3.0f, e.Bounds.Top + 1.0f );
        }
        else
        {
          e.Graphics.DrawString( combo.Items[colorIndex + 1].ToString(), combo.Font, new System.Drawing.SolidBrush( System.Drawing.Color.Black ), 3.0f, e.Bounds.Top + 1.0f );
        }
      }
    }



    private void picOriginal_MouseDown( object sender, MouseEventArgs e )
    {
      if ( ( e.Button & MouseButtons.Left ) != 0 )
      {
        picOriginal.Capture = true;
      }
    }



    private void picOriginal_MouseMove( object sender, MouseEventArgs e )
    {
      if ( ( e.Button & MouseButtons.Left ) != 0 )
      {
        // TODO drag image
      }
    }



    private void picOriginal_MouseUp( object sender, MouseEventArgs e )
    {
      if ( ( e.Button & MouseButtons.Left ) != 0 )
      {
        picOriginal.Capture = false;
      }

    }



    private void picPreview_MouseDown( object sender, MouseEventArgs e )
    {
      if ( ( e.Button & MouseButtons.Left ) != 0 )
      {
        picPreview.Capture = true;
      }
    }



    private void picPreview_MouseMove( object sender, MouseEventArgs e )
    {
      if ( ( e.Button & MouseButtons.Left ) != 0 )
      {
        // TODO drag image
      }
    }



    private void picPreview_MouseUp( object sender, MouseEventArgs e )
    {
      if ( ( e.Button & MouseButtons.Left ) != 0 )
      {
        picPreview.Capture = false;
      }
    }



    private void comboColorMatching_SelectedIndexChanged( object sender, EventArgs e )
    {
      RecalcImport();
    }



    private void btnOK_Click( object sender, EventArgs e )
    {
      DialogResult = DialogResult.OK;

      if ( comboTargetPalette.SelectedIndex >= MultiColorSettings.Palettes.Count )
      {
        // we could have more colors in the source palette, truncate
        var newPal = new Palette( MultiColorSettings.Palettes[0].NumColors );
        for ( int i = 0; i < newPal.NumColors; ++i )
        {
          newPal.ColorValues[i] = m_ImportPalette.ColorValues[i];
        }
        newPal.CreateBrushes();
        newPal.Name = "Imported Palette";
        MultiColorSettings.Palettes.Add( newPal );

        MultiColorSettings.ActivePalette = MultiColorSettings.Palettes.Count - 1;
      }
      Close();
    }



    private void btnZoomIn_Click( object sender, EventArgs e )
    {
      if ( m_Zoom > 1 )
      {
        m_Zoom /= 2;
        picOriginal.SetImageSize( picOriginal.ClientSize.Width * m_Zoom / 1024, picOriginal.ClientSize.Height * m_Zoom / 1024 );
        picOriginal.DisplayPage.DrawImage( m_OriginalImage, 0, 0 );
        picPreview.SetImageSize( picPreview.ClientSize.Width * m_Zoom / 1024, picPreview.ClientSize.Height * m_Zoom / 1024 );
        picPreview.DisplayPage.DrawImage( m_ImportImage, 0, 0 );
      }
    }



    private void btnZoomOut_Click( object sender, EventArgs e )
    {
      if ( m_Zoom < 65536 )
      {
        m_Zoom *= 2;
        picOriginal.SetImageSize( picOriginal.ClientSize.Width * m_Zoom / 1024, picOriginal.ClientSize.Height * m_Zoom / 1024 );
        picOriginal.DisplayPage.DrawImage( m_OriginalImage, 0, 0 );
        picPreview.SetImageSize( picPreview.ClientSize.Width * m_Zoom / 1024, picPreview.ClientSize.Height * m_Zoom / 1024 );
        picPreview.DisplayPage.DrawImage( m_ImportImage, 0, 0 );
      }
    }



    private void comboImportType_SelectedIndexChanged( object sender, EventArgs e )
    {
      RecalcImport();
    }



    private void comboBackground_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( comboBackground.SelectedIndex >= 0 )
      {
        MultiColorSettings.BackgroundColor = comboBackground.SelectedIndex - 1;
      }
      RecalcImport();
    }



    private void comboMulticolor1_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( comboMulticolor1.SelectedIndex >= 0 )
      {
        MultiColorSettings.MultiColor1 = comboMulticolor1.SelectedIndex - 1;
      }
      RecalcImport();
    }



    private void comboMulticolor2_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( comboMulticolor2.SelectedIndex >= 0 )
      {
        MultiColorSettings.MultiColor2 = comboMulticolor2.SelectedIndex - 1;
      }
      RecalcImport();
    }



    private void btnReload_Click( object sender, EventArgs e )
    {
      ReloadImage();
    }



    private void contextMenuOrigPic_Opening( object sender, CancelEventArgs e )
    {
      // determine point under mouse

      System.Drawing.Point    ptMouse = Cursor.Position;

      ptMouse = picOriginal.PointToClient( ptMouse );

      m_OrigReplacementColor = m_OriginalImage.GetPixel( ptMouse.X * m_Zoom / 1024, ptMouse.Y * m_Zoom / 1024 );

      forceTargetColorToolStripMenuItem.Text = "Force replace color #" + m_OrigReplacementColor.ToString( "X6" ) + ":";
    }



    private void replaceColorMenuItem_Click( object sender, EventArgs e )
    {
      ToolStripItem    menuItem = (ToolStripItem)sender;

      bool    entryExisted = m_ForcedReplacementColors.ContainsKey( m_OrigReplacementColor );

      byte    colorValue = GR.Convert.ToU8( (string)menuItem.Tag );

      m_ForcedReplacementColors[m_OrigReplacementColor] = colorValue;

      if ( entryExisted )
      {
        foreach ( ListViewItem oldItem in listDirectReplaceColors.Items )
        {
          if ( (uint)oldItem.Tag == m_OrigReplacementColor )
          {
            oldItem.SubItems[1].Text = colorValue.ToString();
            break;
          }
        }
      }
      else
      {
        ListViewItem item = new ListViewItem( "#" + m_OrigReplacementColor.ToString( "X6" ) );
        item.SubItems.Add( colorValue.ToString() );
        item.Tag = m_OrigReplacementColor;

        listDirectReplaceColors.Items.Add( item );
      }

      RecalcImport();
    }



    private void checkPasteAsBlock_CheckedChanged( object sender, EventArgs e )
    {
      PasteAsBlock = checkPasteAsBlock.Checked;
    }



    private void DlgGraphicImport_SizeChanged( object sender, EventArgs e )
    {
      int     height = ClientSize.Height;

      picOriginal.Height = height / 2 - 8;
      picPreview.Location = new Point( picPreview.Location.X, picOriginal.Location.Y + picOriginal.Height + 4 );
      picPreview.Height = height / 2 - 8;
    }



    private void DlgGraphicImport_ResizeEnd( object sender, EventArgs e )
    {
      picOriginal.DisplayPage.DrawImage( m_OriginalImage, 0, 0 );

      m_OrigSize.Width = m_OriginalImage.Width;
      m_OrigSize.Height = m_OriginalImage.Height;

      RecalcImport();

      picPreview.Invalidate();
      picOriginal.Invalidate();
    }



    private void comboTargetPalette_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( comboTargetPalette.SelectedIndex >= MultiColorSettings.Palettes.Count )
      {
        m_CurPalette = m_ImportPalette;
      }
      else
      {
        m_CurPalette = MultiColorSettings.Palettes[comboTargetPalette.SelectedIndex];
      }
      PaletteManager.ApplyPalette( picPreview.DisplayPage, m_CurPalette );
      RecalcImport();

      picPreview.Invalidate();
      picOriginal.Invalidate();
    }


  }
}
