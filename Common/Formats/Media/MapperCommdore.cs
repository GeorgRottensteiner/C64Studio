using System;
using System.ComponentModel;

namespace RetroDevStudio.Types
{
  public static class MapperCommodore
  {
    public static FileTypeNative Map( CommodoreFileTypeNative commodoreType )
    {
      var attr = GR.EnumHelper.GetAttributeOfType<FileTypeNativeAttribute>( commodoreType );
      if ( attr != null )
      {
        return attr.Type;
      }
      return FileTypeNative.NONE;
    }



    public static CommodoreFileTypeNative Map( FileTypeNative fileType )
    {
      foreach ( CommodoreFileTypeNative commodoreType in Enum.GetValues( typeof( CommodoreFileTypeNative ) ) )
      {
        var attr = GR.EnumHelper.GetAttributeOfType<FileTypeNativeAttribute>( commodoreType );
        if ( ( attr != null )
        &&   ( attr.Type == fileType ) )
        {
          return commodoreType;
        }
      }
      return CommodoreFileTypeNative.PRG;
    }



  }

}
