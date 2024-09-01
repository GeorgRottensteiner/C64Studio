using RetroDevStudio.Converter;
using RetroDevStudio;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;



namespace RetroDevStudio.Dialogs
{
  public partial class DlgGraphicImport : Form
  {
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
    private int                     m_ItemWidth = 8;
    private int                     m_ItemHeight = 8;

    private Point                   m_ImageDisplayOffset = new Point();
    private Point                   m_ImageDragOffset = new Point();
    private bool                    m_IsDragging = false;

    private GR.Image.MemoryImage    m_OriginalImage = new GR.Image.MemoryImage( 20, 20, GR.Drawing.PixelFormat.Format8bppIndexed );
    private GR.Image.MemoryImage    m_ImportImage = new GR.Image.MemoryImage( 20, 20, GR.Drawing.PixelFormat.Format8bppIndexed );
    private Palette                 m_ImportPalette = null;      

    private string                  m_OrigFilename;

    private uint                    m_OrigReplacementColor = 0;

    public ColorSettings            MultiColorSettings = new ColorSettings();
    public bool                     PasteAsBlock = false;
    public Types.GraphicType        SelectedImportAsType = GraphicType.SPRITES;

    private Types.GraphicType       m_ImportType;



    public DlgGraphicImport( StudioCore Core, 
                             Types.GraphicType ImportType, 
                             GR.Image.IImage IncomingImage, 
                             string Filename, 
                             ColorSettings MCSettings,
                             int ItemWidth, int ItemHeight )
    {
      this.Core = Core;
      m_ImportType        = ImportType;
      m_ItemWidth         = ItemWidth;
      m_ItemHeight        = ItemHeight;
      m_CurPalette        = MCSettings.Palette;
      MultiColorSettings  = MCSettings;

      InitializeComponent();

      // TODO - adjust to used machine type!!
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
      ||   ( ImportType == Types.GraphicType.SPRITES_256_COLORS )
      ||   ( ImportType == Types.GraphicType.CHARACTERS_FCM )
      ||   ( ImportType == Types.GraphicType.BITMAP ) )
      {
        comboTargetPalette.Items.Add( "Use incoming palette" );
      }
      comboTargetPalette.SelectedIndex = 0;

      picOriginal.DisplayPage.Create( picOriginal.ClientSize.Width, picOriginal.ClientSize.Height, GR.Drawing.PixelFormat.Format32bppRgb );
      picOriginal.MouseWheel += Preview_MouseWheel;

      picPreview.DisplayPage.Create( picPreview.ClientSize.Width, picPreview.ClientSize.Height, GR.Drawing.PixelFormat.Format8bppIndexed );
      picPreview.MouseWheel += Preview_MouseWheel;

      // autosize preview panels
      AutoSizePreviewPanels();

      PaletteManager.ApplyPalette( picPreview.DisplayPage, m_CurPalette );

      switch ( ImportType )
      {
        case RetroDevStudio.Types.GraphicType.BITMAP:
          comboImportType.Items.Add( new GR.Generic.Tupel<string,RetroDevStudio.Types.GraphicType>( GR.EnumHelper.GetDescription( RetroDevStudio.Types.GraphicType.BITMAP_HIRES ), RetroDevStudio.Types.GraphicType.BITMAP_HIRES ) );
          comboImportType.Items.Add( new GR.Generic.Tupel<string,RetroDevStudio.Types.GraphicType>( GR.EnumHelper.GetDescription( RetroDevStudio.Types.GraphicType.BITMAP_MULTICOLOR ), RetroDevStudio.Types.GraphicType.BITMAP_MULTICOLOR ) );
          comboImportType.SelectedIndex = 1;
          break;
        case RetroDevStudio.Types.GraphicType.CHARACTERS:
          comboImportType.Items.Add( new GR.Generic.Tupel<string, RetroDevStudio.Types.GraphicType>( GR.EnumHelper.GetDescription( RetroDevStudio.Types.GraphicType.CHARACTERS_HIRES ), RetroDevStudio.Types.GraphicType.CHARACTERS_HIRES ) );
          comboImportType.Items.Add( new GR.Generic.Tupel<string, RetroDevStudio.Types.GraphicType>( GR.EnumHelper.GetDescription( RetroDevStudio.Types.GraphicType.CHARACTERS_MULTICOLOR ), RetroDevStudio.Types.GraphicType.CHARACTERS_MULTICOLOR ) );
          comboImportType.Items.Add( new GR.Generic.Tupel<string, RetroDevStudio.Types.GraphicType>( GR.EnumHelper.GetDescription( RetroDevStudio.Types.GraphicType.CHARACTERS_FCM ), RetroDevStudio.Types.GraphicType.CHARACTERS_FCM ) );
          comboImportType.SelectedIndex = 1;
          break;
        default:
          comboImportType.Items.Add( new GR.Generic.Tupel<string,RetroDevStudio.Types.GraphicType>( GR.EnumHelper.GetDescription( ImportType ), ImportType ) );
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

      SelectedImportAsType = m_ImportType;

      OpenImage( IncomingImage );

      comboBackground.SelectedIndex   = MCSettings.BackgroundColor + 1;
      comboMulticolor1.SelectedIndex  = MCSettings.MultiColor1 + 1;
      comboMulticolor2.SelectedIndex  = MCSettings.MultiColor2 + 1;

      Core.Theming.ApplyTheme( this );
    }



    private void Preview_MouseWheel( object sender, MouseEventArgs e )
    {
      if ( e.Delta > 0 )
      {
        btnZoomIn_Click( new DecentForms.ControlBase() );
      }
      else
      {
        btnZoomOut_Click( new DecentForms.ControlBase() );
      }
    }



    private void AutoSizePreviewPanels()
    {
      int   fullHeight = ClientSize.Height / 2;

      picOriginal.Size = new Size( picOriginal.Width, fullHeight - 8 );

      picPreview.Top = fullHeight;
      picPreview.Size = new Size( picPreview.Width, fullHeight - 8 );

      picOriginal.SetImageSize( picOriginal.ClientSize.Width * m_Zoom / 1024, picOriginal.ClientSize.Height * m_Zoom / 1024 );
      picPreview.SetImageSize( picPreview.ClientSize.Width * m_Zoom / 1024, picPreview.ClientSize.Height * m_Zoom / 1024 );
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
      openDlg.Filter = Core.MainForm.FilterString( RetroDevStudio.Types.Constants.FILEFILTER_IMAGE_FILES );
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



    private void OpenImage( GR.Image.IImage newImage )
    {
      if ( newImage == null )
      {
        return;
      }
      m_OriginalImage = new GR.Image.MemoryImage( newImage.Width, newImage.Height, newImage.PixelFormat );
      m_ImportImage = new GR.Image.MemoryImage( newImage.Width, newImage.Height, GR.Drawing.PixelFormat.Format8bppIndexed );

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
        // calculate a new palette
        var quant = new RetroDevStudio.Converter.ColorQuantizer( 256 );

        quant.AddSourceToColorCube( newImage );
        quant.Calculate();

        var resultingImage = quant.Reduce( newImage );
        m_ImportPalette = new Palette( 1 << resultingImage.BitsPerPixel );
        for ( int i = 0; i < m_ImportPalette.NumColors; ++i )
        {
          m_ImportPalette.ColorValues[i] = resultingImage.PaletteColor( i ) | 0xff000000;
        }
        m_ImportPalette.CreateBrushes();
        PaletteManager.ApplyPalette( m_OriginalImage, m_ImportPalette );
      }
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

      picOriginal.DisplayPage.DrawImage( m_OriginalImage, m_ImageDisplayOffset.X, m_ImageDisplayOffset.Y );

      m_OrigSize.Width  = m_OriginalImage.Width;
      m_OrigSize.Height = m_OriginalImage.Height;

      RecalcImport();
    }



    bool CheckColors()
    {
      // too early?
      if ( m_OrigSize.Width == 0 )
      {
        return true;
      }

      // can all colors be matched to the palette?
      GR.Collections.Map<uint, byte> matchedColors = new GR.Collections.Map<uint, byte>();

      var curPalColors = m_CurPalette.ColorValues.ToList();
      var importPalColors = curPalColors;
      if ( m_ImportPalette != null )
      {
        importPalColors = m_ImportPalette.ColorValues.ToList();
      }
      int   numPalcolorsToConsider = m_CurPalette.NumColors;
      if ( m_ImportType == GraphicType.SPRITES_16_COLORS )
      {
        // palette has 256 colors (for bg), but only the first 16 available for sprites
        numPalcolorsToConsider = 16;
      }
      if ( numPalcolorsToConsider < curPalColors.Count )
      {
        curPalColors.RemoveRange( numPalcolorsToConsider, curPalColors.Count - numPalcolorsToConsider );
      }

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

            if ( ( picOriginal.DisplayPage.PixelFormat == GR.Drawing.PixelFormat.Format4bppIndexed )
            ||   ( picOriginal.DisplayPage.PixelFormat == GR.Drawing.PixelFormat.Format1bppIndexed )
            ||   ( picOriginal.DisplayPage.PixelFormat == GR.Drawing.PixelFormat.Format8bppIndexed ) )
            {
              red   = picOriginal.DisplayPage.PaletteRed( (int)pixelValue );
              green = picOriginal.DisplayPage.PaletteGreen( (int)pixelValue );
              blue  = picOriginal.DisplayPage.PaletteBlue( (int)pixelValue );
            }

            int bestMatch = -1;

            if ( comboTargetPalette.SelectedIndex < MultiColorSettings.Palettes.Count )
            {
              bestMatch = ColorMatcher.MatchColor( (ColorMatchType)comboColorMatching.SelectedIndex, red, green, blue, curPalColors );
            }
            else
            {
              bestMatch = ColorMatcher.MatchColor( (ColorMatchType)comboColorMatching.SelectedIndex, red, green, blue, importPalColors );
            }

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
      picPreview.DisplayPage.DrawImage( m_ImportImage, m_ImageDisplayOffset.X, m_ImageDisplayOffset.Y );
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
      for ( int sy = 0; sy < ( m_OrigSize.Height + m_ItemWidth - 1 ) / m_ItemWidth; ++sy )
      {
        for ( int sx = 0; sx < ( m_OrigSize.Width + m_ItemHeight - 1 ) / m_ItemHeight; ++sx )
        {
          // determine single/multi color
          bool[] usedColor = new bool[16];
          int numColors = 0;
          bool hasSinglePixel = false;
          bool usedBackgroundColor = false;
          int determinedBackgroundColor = comboBackground.SelectedIndex - 1;
          int determinedMultiColor1 = comboMulticolor1.SelectedIndex - 1;
          int determinedMultiColor2 = comboMulticolor2.SelectedIndex - 1;

          for ( int y = 0; y < m_ItemHeight; ++y )
          {
            for ( int x = 0; x < m_ItemWidth; ++x )
            {
              int colorIndex = (int)picPreview.DisplayPage.GetPixelData( sx * m_ItemWidth + x, sy * m_ItemHeight + y );
              if ( colorIndex >= 16 )
              {
                AddError( "Encountered color index >= 16 at " + x + "," + y, sx * m_ItemWidth + x, sy * m_ItemHeight + y, 1, 1 );
              }
              else
              {
                if ( ( x % 2 ) == 0 )
                {
                  if ( colorIndex != (int)picPreview.DisplayPage.GetPixelData( sx * m_ItemWidth + x + 1, sy * m_ItemHeight + y ) )
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
              AddError( "Chosen multicolor, but has a single pixel", sx * m_ItemWidth, sy * m_ItemHeight, m_ItemWidth, m_ItemHeight );
              continue;
            }
          }
          if ( ( hasSinglePixel )
          &&   ( numColors > 2 ) )
          {
            AddError( "Has a single pixel, but more than two colors", sx * m_ItemWidth, sy * m_ItemHeight, m_ItemWidth, m_ItemHeight );
            continue;
          }
          if ( ( !MultiColor )
          &&   ( numColors > 2 ) )
          {
            AddError( "Chosen HiRes, but more than two colors", sx * m_ItemWidth, sy * m_ItemHeight, m_ItemWidth, m_ItemHeight );
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
                AddError( "Has a single pixel, but two colors, and none is the background color", sx * m_ItemWidth, sy * m_ItemHeight, m_ItemWidth, m_ItemHeight );
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
            AddError( "Looks like single color, but doesn't use the set background color and there are no more free custom colors.", sx * m_ItemWidth, sy * m_ItemHeight, m_ItemWidth, m_ItemHeight );
            return;
          }
          if ( ( !hasSinglePixel )
          &&   ( numColors > 4 ) )
          {
            AddError( "Uses more than 4 colors", sx * m_ItemWidth, sy * m_ItemHeight, m_ItemWidth, m_ItemHeight );
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
            AddError( "Uses 4 colors, but doesn't use the set background color", sx * m_ItemWidth, sy * m_ItemHeight, m_ItemWidth, m_ItemHeight );
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
                      AddError( "Uses more than one free color", sx * m_ItemWidth, sy * m_ItemHeight, m_ItemWidth, m_ItemHeight );
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
              AddError( "Chosen multi color but used free color with index >= 8", sx * m_ItemWidth, sy * m_ItemHeight, m_ItemWidth, m_ItemHeight );
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
              AddError( "Chosen multi color but used free color with index >= 8", sx * m_ItemWidth, sy * m_ItemHeight, m_ItemWidth, m_ItemHeight );
              continue;
            }

            if ( numColors - usedMultiColors > 1 )
            {
              // only one free color allowed
              AddError( "Uses more than one free color", sx * m_ItemWidth, sy * m_ItemHeight, m_ItemWidth, m_ItemHeight );
              continue;
            }
          }
        }
      }

    }



    private void CheckAsHiResBitmap()
    {
      for ( int sy = 0; sy < ( m_OrigSize.Height + m_ItemWidth - 1 ) / m_ItemWidth; ++sy )
      {
        for ( int sx = 0; sx < ( m_OrigSize.Width + m_ItemHeight - 1 ) / m_ItemHeight; ++sx )
        {
          // determine single/multi color
          bool[] usedColor = new bool[16];
          int numColors = 0;
          bool usedBackgroundColor = false;
          int determinedBackgroundColor = comboBackground.SelectedIndex - 1;

          for ( int y = 0; y < m_ItemHeight; ++y )
          {
            for ( int x = 0; x < m_ItemWidth; ++x )
            {
              int colorIndex = (int)picPreview.DisplayPage.GetPixelData( sx * m_ItemWidth + x, sy * m_ItemHeight + y );
              if ( colorIndex >= 16 )
              {
                AddError( "Encountered color index >= 16 at " + x + "," + y, sx * m_ItemWidth + x, sy * m_ItemHeight + y, 1, 1 );
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
            AddError( "Uses more than two colors", sx * m_ItemWidth, sy * m_ItemHeight, m_ItemWidth, m_ItemHeight );
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
            AddError( "Looks like single color, but doesn't use the set background color", sx * m_ItemWidth, sy * m_ItemHeight, m_ItemWidth, m_ItemHeight );
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
                  AddError( "Uses more than one free color", sx * m_ItemWidth, sy * m_ItemHeight, m_ItemWidth, m_ItemHeight );
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
      for ( int sy = 0; sy < ( m_OrigSize.Height + m_ItemWidth - 1 ) / m_ItemWidth; ++sy )
      {
        for ( int sx = 0; sx < ( m_OrigSize.Width + m_ItemHeight - 1 ) / m_ItemHeight; ++sx )
        {
          // determine single/multi color
          bool[] usedColor = new bool[16];
          int numColors = 0;
          bool hasSinglePixel = false;
          bool usedBackgroundColor = false;
          int determinedBackgroundColor = comboBackground.SelectedIndex - 1;
          int determinedMultiColor1 = comboMulticolor1.SelectedIndex - 1;
          int determinedMultiColor2 = comboMulticolor2.SelectedIndex - 1;

          for ( int y = 0; y < m_ItemHeight; ++y )
          {
            for ( int x = 0; x < m_ItemWidth; ++x )
            {
              int colorIndex = (int)picPreview.DisplayPage.GetPixelData( sx * m_ItemWidth + x, sy * m_ItemHeight + y );
              if ( colorIndex >= 16 )
              {
                AddError( "Encountered color index >= 16 at " + x + "," + y, sx * m_ItemWidth + x, sy * m_ItemHeight + y, 1, 1 );
              }
              else
              {
                if ( ( x % 2 ) == 0 )
                {
                  if ( colorIndex != (int)picPreview.DisplayPage.GetPixelData( sx * m_ItemWidth + x + 1, sy * m_ItemHeight + y ) )
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
            AddError( "Has a single pixel", sx * m_ItemWidth, sy * m_ItemHeight, m_ItemWidth, m_ItemHeight );
            continue;
          }
          if ( ( determinedBackgroundColor != -1 )
          &&   ( !usedBackgroundColor )
          &&   ( numColors >= 4 ) )
          {
            AddError( "Uses more than 3 colors, but doesn't use the set background color and there are no more free custom colors.", sx * m_ItemWidth, sy * m_ItemHeight, m_ItemWidth, m_ItemHeight );
            continue;
          }
          if ( numColors > 4 )
          {
            AddError( "Uses more than 4 colors", sx * m_ItemWidth, sy * m_ItemHeight, m_ItemWidth, m_ItemHeight );
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
            AddError( "Uses 4 colors, but doesn't use the set background color", sx * m_ItemWidth, sy * m_ItemHeight, m_ItemWidth, m_ItemHeight );
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
            AddError( "Uses more than one free color", sx * m_ItemWidth, sy * m_ItemHeight, m_ItemWidth, m_ItemHeight );
            continue;
          }
        }
      }
    }



    private void CheckAsSprites()
    {
      if ( m_ImportType == GraphicType.SPRITES_256_COLORS )
      {
        return;
      }

      for ( int sy = 0; sy < ( m_OrigSize.Height + m_ItemHeight - 1 ) / m_ItemHeight; ++sy )
      {
        for ( int sx = 0; sx < ( m_OrigSize.Width + m_ItemWidth - 1 ) / m_ItemWidth; ++sx )
        {
          // determine single/multi color
          bool[] usedColor = new bool[16];
          int numColors = 0;
          bool hasSinglePixel = false;
          bool usedBackgroundColor = false;
          int determinedBackgroundColor = comboBackground.SelectedIndex - 1;
          int determinedMultiColor1 = comboMulticolor1.SelectedIndex - 1;
          int determinedMultiColor2 = comboMulticolor2.SelectedIndex - 1;

          for ( int y = 0; y < m_ItemHeight; ++y )
          {
            for ( int x = 0; x < m_ItemWidth; ++x )
            {
              int colorIndex = (int)picPreview.DisplayPage.GetPixelData( sx * m_ItemWidth + x, sy * m_ItemHeight + y );

              if ( m_ImportType == GraphicType.SPRITES_16_COLORS )
              {
                if ( colorIndex >= 16 )
                {
                  AddError( $"Encountered color index >= 16 ({colorIndex}) at " + x + "," + y, sx * m_ItemWidth + x, sy * m_ItemHeight + y, 1, 1 );
                }
                continue;
              }
              else if ( colorIndex >= 16 )
              {
                AddError( $"Encountered color index >= 16 at ({colorIndex}) at " + x + "," + y, sx * m_ItemWidth + x, sy * m_ItemHeight + y, 1, 1 );
              }
              else
              {
                if ( ( x % 2 ) == 0 )
                {
                  if ( colorIndex != (int)picPreview.DisplayPage.GetPixelData( sx * m_ItemWidth + x + 1, sy * m_ItemHeight + y ) )
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
            AddError( "Has a single pixel, but more than two colors", sx * m_ItemWidth, sy * m_ItemHeight, m_ItemWidth, m_ItemHeight );
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
            AddError( "Looks like single color, but doesn't use the set background color", sx * m_ItemWidth, sy * m_ItemHeight, m_ItemWidth, m_ItemHeight );
            return;
          }*/
          if ( ( !hasSinglePixel )
          &&   ( numColors > 4 ) )
          {
            AddError( "Uses more than 4 colors", sx * m_ItemWidth, sy * m_ItemHeight, m_ItemWidth, m_ItemHeight );
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
            AddError( "Uses 4 colors, but doesn't use the set background color", sx * m_ItemWidth, sy * m_ItemHeight, m_ItemWidth, m_ItemHeight );
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
                    AddError( "Uses more than one free color", sx * m_ItemWidth, sy * m_ItemHeight, m_ItemWidth, m_ItemHeight );
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
              AddError( "Uses more than one free color", sx * m_ItemWidth, sy * m_ItemHeight, m_ItemWidth, m_ItemHeight );
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
        case RetroDevStudio.Types.GraphicType.CHARACTERS:
        case RetroDevStudio.Types.GraphicType.CHARACTERS_HIRES:
          CheckAsCharacters( false );
          break;
        case RetroDevStudio.Types.GraphicType.CHARACTERS_MULTICOLOR:
          CheckAsCharacters( true );
          break;
        case RetroDevStudio.Types.GraphicType.CHARACTERS_FCM:
          // nothing to do, there are no limits
          break;
        case RetroDevStudio.Types.GraphicType.SPRITES:
          CheckAsSprites();
          break;
        case RetroDevStudio.Types.GraphicType.BITMAP_HIRES:
          CheckAsHiResBitmap();
          break;
        case RetroDevStudio.Types.GraphicType.BITMAP_MULTICOLOR:
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
        StartDrag( e.X, e.Y );
      }
    }



    private void StartDrag( int X, int Y )
    {
      m_ImageDragOffset = new Point( X * m_Zoom / 1024 - m_ImageDisplayOffset.X,
                                     Y * m_Zoom / 1024 - m_ImageDisplayOffset.Y );
      m_IsDragging = true;
    }



    private void picOriginal_MouseMove( object sender, MouseEventArgs e )
    {
      if ( ( e.Button & MouseButtons.Left ) != 0 )
      {
        if ( m_IsDragging )
        {
          HandleDrag( e.X, e.Y );
        }
      }
    }



    private void HandleDrag( int X, int Y )
    {
      int   newX = X * m_Zoom / 1024 - m_ImageDragOffset.X;
      int   newY = Y * m_Zoom / 1024 - m_ImageDragOffset.Y;

      if ( ( m_ImageDisplayOffset.X != newX )
      ||   ( m_ImageDisplayOffset.Y != newY ) )
      {
        m_ImageDisplayOffset.X = newX;
        m_ImageDisplayOffset.Y = newY;

        SanitizeImageOffset();

        picOriginal.DisplayPage.Box( 0, 0, picOriginal.DisplayPage.Width, picOriginal.DisplayPage.Height, 0 );
        picOriginal.DisplayPage.DrawImage( m_OriginalImage, m_ImageDisplayOffset.X, m_ImageDisplayOffset.Y );
        picPreview.DisplayPage.Box( 0, 0, picPreview.DisplayPage.Width, picPreview.DisplayPage.Height, 0 );
        picPreview.DisplayPage.DrawImage( m_ImportImage, m_ImageDisplayOffset.X, m_ImageDisplayOffset.Y );

        picOriginal.Invalidate();
        picPreview.Invalidate();
      }
    }



    private void SanitizeImageOffset()
    {
      int     rightEnd = ( m_ImageDisplayOffset.X + m_OriginalImage.Width ) * 1024 / m_Zoom;
      int     bottomEnd = ( m_ImageDisplayOffset.Y + m_OriginalImage.Height ) * 1024 / m_Zoom;

      if ( m_OriginalImage.Width * 1024 / m_Zoom <= picOriginal.ClientSize.Width )
      {
        if ( m_ImageDisplayOffset.X < 0 )
        {
          m_ImageDisplayOffset.X = 0;
        }
        if ( m_ImageDisplayOffset.X > ( ( picOriginal.ClientSize.Width * m_Zoom ) / 1024 - m_OriginalImage.Width ) )
        {
          m_ImageDisplayOffset.X = ( ( picOriginal.ClientSize.Width * m_Zoom ) / 1024 - m_OriginalImage.Width );
        }
      }
      else
      {
        if ( rightEnd < picOriginal.ClientSize.Width )
        {
          m_ImageDisplayOffset.X = ( picOriginal.ClientSize.Width * m_Zoom / 1024 ) - m_OriginalImage.Width;
        }
        if ( m_ImageDisplayOffset.X > 0 )
        {
          m_ImageDisplayOffset.X = 0;
        }
      }
      if ( m_OriginalImage.Height * 1024 / m_Zoom <= picOriginal.ClientSize.Height )
      {
        if ( m_ImageDisplayOffset.Y < 0 )
        {
          m_ImageDisplayOffset.Y = 0;
        }
        if ( m_ImageDisplayOffset.Y > ( ( picOriginal.ClientSize.Height * m_Zoom ) / 1024 - m_OriginalImage.Height ) )
        {
          m_ImageDisplayOffset.Y = ( ( picOriginal.ClientSize.Height * m_Zoom ) / 1024 - m_OriginalImage.Height );
        }
      }
      else
      {
        if ( bottomEnd < picOriginal.ClientSize.Height )
        {
          m_ImageDisplayOffset.Y = ( picOriginal.ClientSize.Height * m_Zoom / 1024 ) - m_OriginalImage.Height;
        }
        if ( m_ImageDisplayOffset.Y > 0 )
        {
          m_ImageDisplayOffset.Y = 0;
        }
      }
    }



    private void picOriginal_MouseUp( object sender, MouseEventArgs e )
    {
      if ( ( e.Button & MouseButtons.Left ) != 0 )
      {
        picOriginal.Capture = false;
        m_IsDragging = false;
      }
    }



    private void picPreview_MouseDown( object sender, MouseEventArgs e )
    {
      if ( ( e.Button & MouseButtons.Left ) != 0 )
      {
        picPreview.Capture = true;
        StartDrag( e.X, e.Y );
      }
    }



    private void picPreview_MouseMove( object sender, MouseEventArgs e )
    {
      if ( ( e.Button & MouseButtons.Left ) != 0 )
      {
        if ( m_IsDragging )
        {
          HandleDrag( e.X, e.Y );
        }
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



    private void btnOK_Click( DecentForms.ControlBase Sender )
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



    private void btnZoomIn_Click( DecentForms.ControlBase Sender )
    {
      if ( m_Zoom > 1 )
      {
        m_Zoom /= 2;

        SanitizeImageOffset();

        picOriginal.SetImageSize( picOriginal.ClientSize.Width * m_Zoom / 1024, picOriginal.ClientSize.Height * m_Zoom / 1024 );
        picOriginal.DisplayPage.Box( 0, 0, picOriginal.DisplayPage.Width, picOriginal.DisplayPage.Height, 0 );
        picOriginal.DisplayPage.DrawImage( m_OriginalImage, m_ImageDisplayOffset.X, m_ImageDisplayOffset.Y );
        picPreview.SetImageSize( picPreview.ClientSize.Width * m_Zoom / 1024, picPreview.ClientSize.Height * m_Zoom / 1024 );
        picPreview.DisplayPage.Box( 0, 0, picPreview.DisplayPage.Width, picPreview.DisplayPage.Height, 0 );
        picPreview.DisplayPage.DrawImage( m_ImportImage, m_ImageDisplayOffset.X, m_ImageDisplayOffset.Y );
      }
    }



    private void btnZoomOut_Click( DecentForms.ControlBase Sender )
    {
      if ( m_Zoom < 65536 )
      {
        m_Zoom *= 2;

        SanitizeImageOffset();

        picOriginal.SetImageSize( picOriginal.ClientSize.Width * m_Zoom / 1024, picOriginal.ClientSize.Height * m_Zoom / 1024 );
        picOriginal.DisplayPage.Box( 0, 0, picOriginal.DisplayPage.Width, picOriginal.DisplayPage.Height, 0 );
        picOriginal.DisplayPage.DrawImage( m_OriginalImage, m_ImageDisplayOffset.X, m_ImageDisplayOffset.Y );
        picPreview.SetImageSize( picPreview.ClientSize.Width * m_Zoom / 1024, picPreview.ClientSize.Height * m_Zoom / 1024 );
        picPreview.DisplayPage.Box( 0, 0, picPreview.DisplayPage.Width, picPreview.DisplayPage.Height, 0 );
        picPreview.DisplayPage.DrawImage( m_ImportImage,  m_ImageDisplayOffset.X, m_ImageDisplayOffset.Y );
      }
    }



    private void comboImportType_SelectedIndexChanged( object sender, EventArgs e )
    {
      SelectedImportAsType = ( (GR.Generic.Tupel<string, Types.GraphicType>)comboImportType.SelectedItem ).second;
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



    private void btnReload_Click( DecentForms.ControlBase Sender )
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
      /*
      int     height = ClientSize.Height;

      picOriginal.Height = height / 2 - 8;
      picPreview.Location = new Point( picPreview.Location.X, picOriginal.Location.Y + picOriginal.Height + 4 );
      picPreview.Height = height / 2 - 8;*/

      AutoSizePreviewPanels();
    }



    private void DlgGraphicImport_ResizeEnd( object sender, EventArgs e )
    {
      picOriginal.DisplayPage.DrawImage( m_OriginalImage, m_ImageDisplayOffset.X, m_ImageDisplayOffset.Y );

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



    private void btnCancel_Click( DecentForms.ControlBase Sender )
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }



  }
}
