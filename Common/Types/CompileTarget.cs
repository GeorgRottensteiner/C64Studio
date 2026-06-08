using System.ComponentModel;

namespace RetroDevStudio.Types
{
  public class CompileTarget
  {
    // default
    public CompileTargetType  Type  = CompileTargetType.NONE;

    // for tape/disk images (file name inside disk)
    public string             InternalFilename = "";

    // for disk images (disk name)
    public string             ContainerName = "";

    // for iNES-Header
    public byte               NESPrgROMUnits16k = 0;
    public byte               NESChrROMUnits8k  = 0;
    public byte               NESMapper         = 0;
    public byte               NESMirroring      = 0;


  }



}