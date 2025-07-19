using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;



namespace RetroDevStudio.Types
{
  public class ColorSettings
  {
    public TextCharMode   CharMode = RetroDevStudio.TextCharMode.UNKNOWN;

    // Commodore
    public int            CustomColor     = 1;
    public int            BackgroundColor = 0;
    public int            MultiColor1     = 0;
    public int            MultiColor2     = 0;
    public int            BGColor4        = 0;

    // index in Palettes list
    public int            ActivePalette   = 0;
    // several palettes (Mega65, etc.)
    private List<Palette>  _palettes = new List<Palette>();

    // offset inside palette (e.g. 256 colors, 16 per sprite)
    public int            PaletteOffset   = 0;

    // used for e.g. NES
    public List<List<int>>PaletteIndexMapping = new List<List<int>>();
    // index in PaletteIndexMapping
    public int            PaletteMappingIndex = 0;




    public List<Palette> Palettes
    {
      get
      {
        return _palettes;
      }
      set
      {
        _palettes = value;
        if ( ActivePalette >= _palettes.Count )
        {
          ActivePalette = _palettes.Count - 1;
        }
      }
    }



    public Palette Palette
    {
      get
      {
        if ( ActivePalette >= Palettes.Count )
        {
          return Palettes[0];
        }
        return Palettes[ActivePalette];
      }
      set
      {
        Palettes[ActivePalette] = value;
      }
    }



    public ColorSettings()
    {
      Palettes.Add( new Palette() );

      SetDefaultPaletteIndexMapping();
    }



    public ColorSettings( ColorSettings OtherSettings )
    {
      CustomColor     = OtherSettings.CustomColor;
      BackgroundColor = OtherSettings.BackgroundColor;
      MultiColor1     = OtherSettings.MultiColor1;
      MultiColor2     = OtherSettings.MultiColor2;
      BGColor4        = OtherSettings.BGColor4;

      Palettes = new List<Palette>();
      foreach ( var pal in OtherSettings.Palettes )
      {
        Palettes.Add( new Palette( pal ) );
      }
      ActivePalette = OtherSettings.ActivePalette;
      PaletteOffset = OtherSettings.PaletteOffset;

      PaletteIndexMapping = new List<List<int>>();
      foreach ( var palEntry in OtherSettings.PaletteIndexMapping )
      {
        PaletteIndexMapping.Add( new List<int>( palEntry ) );
      }
      PaletteMappingIndex = OtherSettings.PaletteMappingIndex;
    }



    public void SetDefaultPaletteIndexMapping()
    {
      // set for NES
      for ( int i = 0; i < 16; ++i )
      {
        var list = new List<int>();
        PaletteIndexMapping.Add( list );
      }
      // presets from NEXXT
      PaletteIndexMapping[0].Add( 15 );
      PaletteIndexMapping[0].Add( 0 );
      PaletteIndexMapping[0].Add( 16 );
      PaletteIndexMapping[0].Add( 48 );
      PaletteIndexMapping[1].Add( 15 );
      PaletteIndexMapping[1].Add( 1 );
      PaletteIndexMapping[1].Add( 33 );
      PaletteIndexMapping[1].Add( 49 );
      PaletteIndexMapping[2].Add( 15 );
      PaletteIndexMapping[2].Add( 6 );
      PaletteIndexMapping[2].Add( 22 );
      PaletteIndexMapping[2].Add( 38 );
      PaletteIndexMapping[3].Add( 15 );
      PaletteIndexMapping[3].Add( 9 );
      PaletteIndexMapping[3].Add( 25 );
      PaletteIndexMapping[3].Add( 41 );

      PaletteIndexMapping[4].Add( 15 );
      PaletteIndexMapping[4].Add( 28 );
      PaletteIndexMapping[4].Add( 33 );
      PaletteIndexMapping[4].Add( 50 );
      PaletteIndexMapping[5].Add( 15 );
      PaletteIndexMapping[5].Add( 17 );
      PaletteIndexMapping[5].Add( 34 );
      PaletteIndexMapping[5].Add( 51 );
      PaletteIndexMapping[6].Add( 15 );
      PaletteIndexMapping[6].Add( 18 );
      PaletteIndexMapping[6].Add( 35 );
      PaletteIndexMapping[6].Add( 52 );
      PaletteIndexMapping[7].Add( 15 );
      PaletteIndexMapping[7].Add( 19 );
      PaletteIndexMapping[7].Add( 36 );
      PaletteIndexMapping[7].Add( 53 );

      PaletteIndexMapping[8].Add( 15 );
      PaletteIndexMapping[8].Add( 19 );
      PaletteIndexMapping[8].Add( 36 );
      PaletteIndexMapping[8].Add( 54 );
      PaletteIndexMapping[9].Add( 15 );
      PaletteIndexMapping[9].Add( 21 );
      PaletteIndexMapping[9].Add( 38 );
      PaletteIndexMapping[9].Add( 55 );
      PaletteIndexMapping[10].Add( 15 );
      PaletteIndexMapping[10].Add( 22 );
      PaletteIndexMapping[10].Add( 39 );
      PaletteIndexMapping[10].Add( 55 );
      PaletteIndexMapping[11].Add( 15 );
      PaletteIndexMapping[11].Add( 25 );
      PaletteIndexMapping[11].Add( 40 );
      PaletteIndexMapping[11].Add( 56 );

      PaletteIndexMapping[12].Add( 15 );
      PaletteIndexMapping[12].Add( 24 );
      PaletteIndexMapping[12].Add( 41 );
      PaletteIndexMapping[12].Add( 56 );
      PaletteIndexMapping[13].Add( 15 );
      PaletteIndexMapping[13].Add( 27 );
      PaletteIndexMapping[13].Add( 42 );
      PaletteIndexMapping[13].Add( 57 );
      PaletteIndexMapping[14].Add( 15 );
      PaletteIndexMapping[14].Add( 27 );
      PaletteIndexMapping[14].Add( 43 );
      PaletteIndexMapping[14].Add( 60 );
      PaletteIndexMapping[15].Add( 15 );
      PaletteIndexMapping[15].Add( 28 );
      PaletteIndexMapping[15].Add( 44 );
      PaletteIndexMapping[15].Add( 59 );
    }



  }

}