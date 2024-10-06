using System;
using System.Collections.Generic;
using System.Linq;



namespace RetroDevStudio.Types
{
  public class AlternativeColorSettings
  {
    public TextCharMode   CharMode = RetroDevStudio.TextCharMode.UNKNOWN;

    // Commodore
    public int            CustomColor     = -1;
    public int            BackgroundColor = -1;
    public int            MultiColor1     = -1;
    public int            MultiColor2     = -1;
    public int            BGColor4        = -1;

    // index in Palettes list
    public int            ActivePalette   = 0;
    // several palettes (Mega65, etc.)
    public List<Palette>  Palettes = new List<Palette>();

    // offset inside palette (e.g. 256 colors, 16 per sprite)
    public int            PaletteOffset   = 0;

    // used for e.g. NES
    public List<List<int>>PaletteIndexMapping = new List<List<int>>();
    // index in PaletteIndexMapping
    public int            PaletteMappingIndex = -1;




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



    public AlternativeColorSettings()
    {
      Palettes.Add( new Palette() );
    }



    public AlternativeColorSettings( ColorSettings OtherSettings )
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



    public AlternativeColorSettings( AlternativeColorSettings OtherSettings )
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



  }

}