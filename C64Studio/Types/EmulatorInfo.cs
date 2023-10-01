using RetroDevStudio.Types.ASM;
using RetroDevStudio;
using System;
using GR.Memory;
using System.Text;

namespace RetroDevStudio.Types
{
  public static class EmulatorInfo
  {
    public static bool IsMega65Family( ToolInfo Tool )
    {
      string    uppercaseFilename = System.IO.Path.GetFileNameWithoutExtension( Tool.Filename ).ToUpper();

      return uppercaseFilename.StartsWith( "XMEGA65" );
    }



    public static bool IsVICEFamily( ToolInfo Tool )
    {
      string    uppercaseFilename = System.IO.Path.GetFileNameWithoutExtension( Tool.Filename ).ToUpper();

      return ( ( uppercaseFilename.StartsWith( "X64" ) )
            || ( uppercaseFilename.StartsWith( "XSCPU64" ) )
            || ( uppercaseFilename.StartsWith( "X128" ) )
            || ( uppercaseFilename.StartsWith( "XCBM" ) )
            || ( uppercaseFilename.StartsWith( "XPET" ) )
            || ( uppercaseFilename.StartsWith( "XPLUS4" ) )
            || ( uppercaseFilename.StartsWith( "XVIC" ) ) );
    }



    public static bool SupportsDebugging( ToolInfo Tool )
    {
      if ( Tool.IsInternal )
      {
        return true;
      }

      // currently only the VICE family is supported
      if ( ( IsVICEFamily( Tool ) )
      ||   ( IsMega65Family( Tool ) ) )
      {
        return true;
      }
      return false;
    }



    public static void SetDefaultRunArguments( ToolInfo Tool )
    {
      Tool.WorkPath               = "\"$(RunPath)\"";
      Tool.Type                   = ToolInfo.ToolType.EMULATOR;
      Tool.PRGArguments           = "\"$(RunFilename)\"";
      Tool.CartArguments          = "";
      Tool.DebugArguments         = "";
      Tool.TrueDriveOnArguments   = "";
      Tool.TrueDriveOffArguments  = "";
      Tool.PassLabelsToEmulator   = false;

      string upperCaseFilename = System.IO.Path.GetFileNameWithoutExtension( Tool.Filename ).ToUpper();

      if ( IsVICEFamily( Tool ) )
      {
        // VICE
        Tool.Name                   = "WinVICE";
        Tool.PRGArguments           = "\"$(RunFilename)\"";
        Tool.CartArguments          = "-cartcrt \"$(RunFilename)\"";
        Tool.DebugArguments         = "-initbreak 0x$(DebugStartAddressHex) -remotemonitor";
        Tool.PassLabelsToEmulator   = true;

        if ( IsVICEVersionOldTrueDrive( Tool ) )
        {
          Tool.TrueDriveOnArguments   = "-truedrive +virtualdev";
          Tool.TrueDriveOffArguments  = "+truedrive -virtualdev";
        }
        else
        {
          Tool.TrueDriveOnArguments   = "-drive8truedrive +virtualdev";
          Tool.TrueDriveOffArguments  = "+drive8truedrive -virtualdev";
        }
      }
      else if ( upperCaseFilename.StartsWith( "CCS64" ) )
      {
        // CCS64
        Tool.Name                 = "CCS64";
        Tool.PassLabelsToEmulator = false;
      }
      else if ( IsMega65Family( Tool ) )
      {
        // XMEGA65
        Tool.Name         = "XMEGA65";
        Tool.PRGArguments = "-prg \"$(RunFilename)\"";
      }
      else if ( upperCaseFilename.StartsWith( "STELLA" ) )
      {
        // Atari Stella
        Tool.Name         = "Stella";
      }
      else
      {
        // fallback
        Tool.Name   = upperCaseFilename;
      }
    }



    private static bool IsVICEVersionOldTrueDrive( ToolInfo Tool )
    {
      try
      {
        var executable = GR.IO.File.ReadAllBytes( Tool.Filename );

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



    public static LabelFileFormat LabelFormat( ToolInfo Tool )
    {
      string upperCaseFilename = System.IO.Path.GetFileNameWithoutExtension( Tool.Filename ).ToUpper();

      if ( upperCaseFilename.StartsWith( "C64DEBUGGER" ) )
      {
        return LabelFileFormat.C64DEBUGGER;
      }
      return LabelFileFormat.VICE;
    }



    public static MachineType DetectMachineType( ToolInfo Tool )
    {
      string    filename = System.IO.Path.GetFileNameWithoutExtension( Tool.Filename ).ToUpper();

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
      return MachineType.UNKNOWN;
    }



  }
}
