using RetroDevStudio.Types.ASM;
using System;
using GR.Memory;



namespace RetroDevStudio.Emulators
{
  public static class EmulatorInfo
  {
    public static bool IsMega65Family( string emulatorFilename )
    {
      string    uppercaseFilename = GR.Path.GetFileNameWithoutExtension( emulatorFilename ).ToUpper();

      return uppercaseFilename.StartsWith( "XMEGA65" );
    }



    public static bool IsVICEFamily( string emulatorFilename )
    {
      string    uppercaseFilename = GR.Path.GetFileNameWithoutExtension( emulatorFilename ).ToUpper();

      return ( ( uppercaseFilename.StartsWith( "X64" ) )
            || ( uppercaseFilename.StartsWith( "XSCPU64" ) )
            || ( uppercaseFilename.StartsWith( "X128" ) )
            || ( uppercaseFilename.StartsWith( "XCBM" ) )
            || ( uppercaseFilename.StartsWith( "XPET" ) )
            || ( uppercaseFilename.StartsWith( "XPLUS4" ) )
            || ( uppercaseFilename.StartsWith( "XVIC" ) ) );
    }



    public static bool SupportsDebugging( string emulatorFilename )
    {
      // currently only the VICE family is supported
      if ( ( IsVICEFamily( emulatorFilename ) )
      ||   ( IsMega65Family( emulatorFilename ) ) )
      {
        return true;
      }
      return false;
    }



    public static bool IsVICEVersionOldTrueDrive( string emulatorFilename )
    {
      try
      {
        var executable = GR.IO.File.ReadAllBytes( emulatorFilename );

        // look for "vice-logo-black.svg00About VICE00"
        var searchKey = new ByteBuffer( "766963652d6c6f676f2d626c61636b2e7376670041626f7574205649434500" );
        int versionPos = executable.Find( searchKey );
        if ( versionPos == -1 )
        {
          // does not even have the version info
          return true;
        }
        int zeroPos = executable.Find( 0, versionPos + (int)searchKey.Length );
        if ( zeroPos == -1 )
        {
          return false;
        }
        // found the action version string
        string    versionString = executable.SubBuffer( versionPos + (int)searchKey.Length, zeroPos - versionPos - (int)searchKey.Length ).ToAsciiString();
        int   spacePos = versionString.IndexOf( ' ' );
        if ( spacePos != -1 )
        {
          versionString = versionString.Substring( 0, spacePos );
        }
        string[]  versionParts = versionString.Split( '.' );
        if ( ( versionParts.Length >= 1 )
        &&   ( string.Compare( versionParts[0], "3" ) > 0 ) )
        {
          return false;
        }
        if ( ( versionParts.Length >= 1 )
        &&   ( string.Compare( versionParts[0], "3" ) < 0 ) )
        {
          return true;
        }
        // < 3.6
        if ( ( versionParts.Length >= 2 )
        &&   ( string.Compare( versionParts[1], "6" ) < 0 ) )
        {
          return true;
        }
        return false;
      }
      catch ( Exception )
      {
        return false;
      }
    }



    public static LabelFileFormat LabelFormat( string emulatorFilename )
    {
      string upperCaseFilename = GR.Path.GetFileNameWithoutExtension( emulatorFilename ).ToUpper();

      if ( upperCaseFilename.StartsWith( "C64DEBUGGER" ) )
      {
        return LabelFileFormat.C64DEBUGGER;
      }
      return LabelFileFormat.VICE;
    }



    public static MachineType DetectMachineType( string emulatorFilename )
    {
      string    filename = GR.Path.GetFileNameWithoutExtension( emulatorFilename ).ToUpper();

      if ( ( filename.StartsWith( "X64" ) )
      ||   ( filename.StartsWith( "XSCPU64" ) )
      ||   ( filename.StartsWith( "CCS64" ) )
      ||   ( filename.StartsWith( "C64DEBUGGER" ) ) )
      {
        return MachineType.C64;
      }
      else if ( filename.StartsWith( "XVIC" ) )
      {
        return MachineType.VIC20;
      }
      else if ( filename.StartsWith( "X128" ) )
      {
        return MachineType.C128;
      }
      else if ( filename.StartsWith( "XCBM" ) )
      {
        return MachineType.CBM;
      }
      else if ( filename.StartsWith( "XPET" ) )
      {
        return MachineType.PET;
      }
      else if ( filename.StartsWith( "XPLUS4" ) )
      {
        return MachineType.PLUS4;
      }
      else if ( filename.StartsWith( "STELLA" ) )
      {
        return MachineType.ATARI2600;
      }
      else if ( filename.StartsWith( "XMEGA65" ) )
      {
        return MachineType.MEGA65;
      }
      return MachineType.ANY;
    }



  }
}
