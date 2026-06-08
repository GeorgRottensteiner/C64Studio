using System;
using System.ComponentModel;



namespace RetroDevStudio
{
  [AttributeUsageAttribute( AttributeTargets.Field | AttributeTargets.Class )]
  public class MediaTypeAttribute : Attribute
  {
    public MediaType Type = MediaType.UNKNOWN;



    public MediaTypeAttribute( MediaType Type )
    {
      this.Type = Type;
    }



  }

}