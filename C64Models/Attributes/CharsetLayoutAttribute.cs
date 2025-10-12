using RetroDevStudio.Types;
using System;

namespace RetroDevStudio
{
  [AttributeUsage( AttributeTargets.Field )]
  public class CharsetLayoutAttribute : Attribute
  {
    public CharlistLayout Layout = CharlistLayout.PLAIN;



    public CharsetLayoutAttribute( CharlistLayout layout )
    {
      Layout = layout;
    }



  }
}