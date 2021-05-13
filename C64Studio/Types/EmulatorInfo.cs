using C64Studio.Types.ASM;
using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio.Types
{
  public static class EmulatorInfo
  {
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
      // currently only the VICE family is supported
      return IsVICEFamily( Tool );
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
        Tool.TrueDriveOnArguments   = "-truedrive +virtualdev";
        Tool.TrueDriveOffArguments  = "+truedrive -virtualdev";
        Tool.PassLabelsToEmulator   = true;
      }
      else if ( upperCaseFilename.StartsWith( "CCS64" ) )
      {
        // CCS64
        Tool.Name                 = "CCS64";
        Tool.PassLabelsToEmulator = false;
      }
      else if ( upperCaseFilename.StartsWith( "XMEGA65" ) )
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
        return MachineType.VC20;
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
      else if ( filename.StartsWith( "STELLA" ) )
      {
        return MachineType.ATARI2600;
      }
      return MachineType.UNKNOWN;
    }



  }
}
