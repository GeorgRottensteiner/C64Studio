namespace RetroDevStudio.Types
{
  public class ColorEntry
  {
    public ColorType  Type = ColorType.CUSTOM_COLOR;
    public byte       ColorIndex = 1;


    public ColorEntry( ColorType type = ColorType.CUSTOM_COLOR, byte colorIndex = 1 )
    {
      Type        = type;
      ColorIndex  = colorIndex;
    }



  }
}