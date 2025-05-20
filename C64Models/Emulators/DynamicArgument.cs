using System.ComponentModel;



namespace RetroDevStudio.Emulators
{
  public enum DynamicArgument
  {
    [Description( "General Arguments" )]
    [UsedFor( MachineType.ANY )]
    GENERAL,
    [Description( "Single File/Tape" )]
    [UsedFor( MachineType.C64 )]
    [UsedFor( MachineType.VIC20 )]
    [UsedFor( MachineType.PET )]
    [UsedFor( MachineType.PLUS4 )]
    [UsedFor( MachineType.C128 )]
    [UsedFor( MachineType.CBM )]
    CALL_SINGLE_FILE_TAPE,
    [Description( "Disk" )]
    [UsedFor( MachineType.C64 )]
    [UsedFor( MachineType.VIC20 )]
    [UsedFor( MachineType.PET )]
    [UsedFor( MachineType.PLUS4 )]
    [UsedFor( MachineType.C128 )]
    [UsedFor( MachineType.CBM )]
    [UsedFor( MachineType.MEGA65 )]
    [UsedFor( MachineType.COMMANDER_X16 )]
    CALL_SINGLE_FILE_DISK,
    [UsedFor( MachineType.C64 )]
    [UsedFor( MachineType.VIC20 )]
    [UsedFor( MachineType.PET )]
    [UsedFor( MachineType.PLUS4 )]
    [UsedFor( MachineType.C128 )]
    [UsedFor( MachineType.CBM )]
    [UsedFor( MachineType.MEGA65 )]
    [UsedFor( MachineType.ATARI2600 )]
    [UsedFor( MachineType.NES )]
    [Description( "Cartridge/ROM" )]
    CALL_CARTRIDGE_ROM,
    [UsedFor( MachineType.C64 )]
    [UsedFor( MachineType.VIC20 )]
    [UsedFor( MachineType.PET )]
    [UsedFor( MachineType.PLUS4 )]
    [UsedFor( MachineType.C128 )]
    [UsedFor( MachineType.CBM )]
    [Description( "Debug" )]
    CALL_DEBUG,
    [UsedFor( MachineType.C64 )]
    [UsedFor( MachineType.VIC20 )]
    [UsedFor( MachineType.PET )]
    [UsedFor( MachineType.PLUS4 )]
    [UsedFor( MachineType.C128 )]
    [UsedFor( MachineType.CBM )]
    [Description( "True Drive On" )]
    CALL_VICE_TRUE_DRIVE_ON,
    [UsedFor( MachineType.C64 )]
    [UsedFor( MachineType.VIC20 )]
    [UsedFor( MachineType.PET )]
    [UsedFor( MachineType.PLUS4 )]
    [UsedFor( MachineType.C128 )]
    [UsedFor( MachineType.CBM )]
    [Description( "True Drive Off" )]
    CALL_VICE_TRUE_DRIVE_OFF,
    [Description( "Full Screen" )]
    [UsedFor( MachineType.ANY )]
    EXPLICIT_FULL_SCREEN,
    [Description( "Windowed" )]
    [UsedFor( MachineType.ANY )]
    EXPLICIT_WINDOW,
    [Description( "Very First Arguments" )]
    [UsedFor( MachineType.ANY )]
    FIRST_ARGUMENTS,
    [Description( "Very Last Arguments" )]
    [UsedFor( MachineType.ANY )]
    LAST_ARGUMENTS
  }


}