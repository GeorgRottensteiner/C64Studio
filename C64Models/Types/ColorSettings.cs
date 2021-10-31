using System.Collections.Generic;



namespace RetroDevStudio.Types
{
  public class ColorSettings
  {
    public int            BackgroundColor = 0;
    public int            MultiColor1     = 0;
    public int            MultiColor2     = 0;
    public int            BGColor4        = 0;

    public int            ActivePalette   = 0;

    public List<Palette>  Palettes = new List<Palette>();



    public Palette Palette
    {
      get
      {
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
    }



    public ColorSettings( ColorSettings OtherSettings )
    {
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
    }



  }
}