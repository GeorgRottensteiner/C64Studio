using RetroDevStudio.Types;
using System;
using System.ComponentModel;



namespace RetroDevStudio
{
  [AttributeUsage( AttributeTargets.Field | AttributeTargets.Class )]
  public class FileTypeNativeAttribute : Attribute
  {
    public FileTypeNative Type = FileTypeNative.NONE;



    public FileTypeNativeAttribute( FileTypeNative type )
    {
      Type = type;
    }



  }

}