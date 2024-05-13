using System.ComponentModel;

namespace RetroDevStudio
{
  public enum MediaType
  {
    [Description( "Unknown" )]
    UNKNOWN = 0,
    [Description( "Tape Image" )]
    TAPE,
    [Description( "Disk Image" )]
    DISK,
    [Description( "Cartridge" )]
    CARTRIDGE
  }

}