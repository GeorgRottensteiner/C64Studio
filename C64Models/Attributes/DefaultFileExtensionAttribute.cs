using System;
using System.ComponentModel;



namespace RetroDevStudio
{
  [AttributeUsageAttribute( AttributeTargets.Field | AttributeTargets.Class )]
  public class DefaultFileExtensionAttribute : Attribute
  {
    private readonly string _Extension = "";



    public string Extension
    {
      get
      {
        return _Extension;
      }
    }



    public DefaultFileExtensionAttribute( string extension )
    {
      _Extension = extension;
    }



  }
}