using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using GR.Memory;

namespace GR
{
  public class OS
  {
    private static readonly bool     _IsWindows = false;
    private static readonly bool     _IsLinux = false;
    private static readonly bool     _IsMac = false;


    public static bool IsWindows
    {
      get 
      { 
        return _IsWindows; 
      } 
    }



    public static bool IsLinux
    {
      get
      {
        return _IsLinux;
      }
    }



    public static bool IsMac
    {
      get
      {
        return _IsMac;
      }
    }



    static OS()
    {
      string windir = Environment.GetEnvironmentVariable( "windir" );
      if ( ( !string.IsNullOrEmpty( windir ) )
      &&   ( windir.Contains( @"\" ) )
      &&   ( System.IO.Directory.Exists( windir ) ) )
      {
        _IsWindows = true;
      }
      if ( System.IO.File.Exists( @"/proc/sys/kernel/ostype" ) )
      {
        string osType = System.IO.File.ReadAllText( @"/proc/sys/kernel/ostype" );
        if ( osType.StartsWith( "Linux", StringComparison.OrdinalIgnoreCase ) )
        {
          // Note: Android gets here too
          _IsLinux = true;
        }
      }
      if ( System.IO.File.Exists( @"/System/Library/CoreServices/SystemVersion.plist" ) )
      {
        // Note: iOS gets here too
        _IsMac = true;
      }
    }



  }

}

