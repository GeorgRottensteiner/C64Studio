namespace RetroDevStudio.Types
{
  public class ColorSettings
  {
    public int      BackgroundColor = 0;
    public int      MultiColor1     = 0;
    public int      MultiColor2     = 0;
    public int      BGColor4        = 0;

    public Palette  Palette = new Palette();



    public ColorSettings()
    {
    }



    public ColorSettings( ColorSettings OtherSettings )
    {
      BackgroundColor = OtherSettings.BackgroundColor;
      MultiColor1     = OtherSettings.MultiColor1;
      MultiColor2     = OtherSettings.MultiColor2;
      BGColor4        = OtherSettings.BGColor4;
      Palette         = new Palette( OtherSettings.Palette );
    }
  }
}