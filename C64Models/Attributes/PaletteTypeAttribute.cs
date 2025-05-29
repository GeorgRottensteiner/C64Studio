using System;

namespace RetroDevStudio
{
  [AttributeUsage( AttributeTargets.Field | AttributeTargets.Class )]
  internal class PaletteTypeAttribute : Attribute
  {
    public PaletteType  PalType = PaletteType.C64;


    public PaletteTypeAttribute( PaletteType palType )
    {
      PalType = palType;
    }


  }
}