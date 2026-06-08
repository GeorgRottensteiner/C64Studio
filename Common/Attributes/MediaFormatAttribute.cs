using System;
using System.ComponentModel;

namespace RetroDevStudio
{
  [AttributeUsage( AttributeTargets.Class, AllowMultiple = true )]
  public class MediaFormatAttribute : Attribute
  {
    public Formats.MediaFormatType Type = Formats.MediaFormatType.UNKNOWN;



    public MediaFormatAttribute( Formats.MediaFormatType Type )
    {
      this.Type = Type;
    }



  }

}