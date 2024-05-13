using System;
using System.ComponentModel;

namespace RetroDevStudio
{
  public enum MediaFormatType
  {
    UNKNOWN = 0,
    [Description( "D64" )]
    [MediaType( MediaType.DISK )]
    D64,
    [Description( "D64 (40 Tracks)" )]
    [MediaType( MediaType.DISK )]
    D64_40,
    [Description( "D71" )]
    [MediaType( MediaType.DISK )]
    D71,
    [Description( "D81" )]
    [MediaType( MediaType.DISK )]
    D81,
    [Description( "ADF" )]
    [MediaType( MediaType.DISK )]
    ADF,
    [Description( "DSK" )]
    [MediaType( MediaType.DISK )]
    DSK,

    [Description( "PRG" )]
    [MediaType( MediaType.TAPE )]
    PRG,
    [Description( "T64" )]
    [MediaType( MediaType.TAPE )]
    T64,
    [Description( "TAP" )]
    [MediaType( MediaType.TAPE )]
    TAP
  }

}