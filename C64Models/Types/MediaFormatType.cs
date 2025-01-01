using System;
using System.ComponentModel;

namespace RetroDevStudio
{
  public enum MediaFormatType
  {
    UNKNOWN = 0,
    [Description( "D64" )]
    [MediaType( MediaType.DISK )]
    [DefaultFileExtension( ".d64" )]
    D64,
    [Description( "D64 (40 Tracks)" )]
    [MediaType( MediaType.DISK )]
    [DefaultFileExtension( ".d64" )]
    D64_40,
    [Description( "D71" )]
    [MediaType( MediaType.DISK )]
    [DefaultFileExtension( ".d71" )]
    D71,
    [Description( "D81" )]
    [MediaType( MediaType.DISK )]
    [DefaultFileExtension( ".d81" )]
    D81,
    [Description( "ADF" )]
    [MediaType( MediaType.DISK )]
    [DefaultFileExtension( ".adf" )]
    ADF,
    [Description( "DSK" )]
    [MediaType( MediaType.DISK )]
    [DefaultFileExtension( ".dsk" )]
    DSK,

    [Description( "PRG" )]
    [MediaType( MediaType.TAPE )]
    [DefaultFileExtension( ".prg" )]
    PRG,
    [Description( "T64" )]
    [MediaType( MediaType.TAPE )]
    [DefaultFileExtension( ".t64" )]
    T64,
    [Description( "TAP" )]
    [MediaType( MediaType.TAPE )]
    [DefaultFileExtension( ".tap" )]
    TAP,
    [Description( "TZX" )]
    [MediaType( MediaType.TAPE )]
    [DefaultFileExtension( ".tzx" )]
    TZX
  }

}