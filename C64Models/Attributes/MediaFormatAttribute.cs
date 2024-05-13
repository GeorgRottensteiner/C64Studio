using System;
using System.ComponentModel;

namespace RetroDevStudio
{
  [AttributeUsageAttribute( AttributeTargets.Class, AllowMultiple = true )]
  public class MediaFormatAttribute : Attribute
  {
    public MediaFormatType Type = MediaFormatType.UNKNOWN;



    public MediaFormatAttribute( MediaFormatType Type )
    {
      this.Type = Type;
    }



  }

}