using System;
using System.ComponentModel;

namespace RetroDevStudio.Formats
{
  public enum MediaFormatType
  {
    UNKNOWN = 0,
    [Description( "D64" )]
    [MediaType( MediaType.DISK )]
    [DefaultFileExtension( ".d64" )]
    [MachineType( MachineType.C64 )]
    D64,
    [Description( "D64 (40 Tracks)" )]
    [MediaType( MediaType.DISK )]
    [DefaultFileExtension( ".d64" )]
    [MachineType( MachineType.C64 )]
    D64_40,
    [Description( "D71" )]
    [MediaType( MediaType.DISK )]
    [DefaultFileExtension( ".d71" )]
    [MachineType( MachineType.C64 )]
    D71,
    [Description( "D81" )]
    [MediaType( MediaType.DISK )]
    [DefaultFileExtension( ".d81" )]
    [MachineType( MachineType.C64 )]
    D81,
    [Description( "ADF" )]
    [MediaType( MediaType.DISK )]
    [DefaultFileExtension( ".adf" )]
    [MachineType( MachineType.AMIGA )]
    ADF,
    [Description( "DSK" )]
    [MediaType( MediaType.DISK )]
    [DefaultFileExtension( ".dsk" )]
    [MachineType( MachineType.CPC )]
    DSK,

    [Description( "PRG" )]
    [MediaType( MediaType.TAPE )]
    [DefaultFileExtension( ".prg" )]
    [MachineType( MachineType.C64 )]
    PRG,
    [Description( "T64" )]
    [MediaType( MediaType.TAPE )]
    [DefaultFileExtension( ".t64" )]
    [MachineType( MachineType.C64 )]
    T64,
    [Description( "TAP" )]
    [MediaType( MediaType.TAPE )]
    [DefaultFileExtension( ".tap" )]
    [MachineType( MachineType.C64 )]
    TAP,
    [Description( "TZX" )]
    [MediaType( MediaType.TAPE )]
    [DefaultFileExtension( ".tzx" )]
    [MachineType( MachineType.ZX81 )]
    TZX,
    [Description( "P" )]
    [MediaType( MediaType.TAPE )]
    [DefaultFileExtension( ".p" )]
    [MachineType( MachineType.ZX81 )]
    P
  }

}