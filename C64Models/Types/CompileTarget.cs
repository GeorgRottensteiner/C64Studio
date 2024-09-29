using System.ComponentModel;

namespace RetroDevStudio.Types
{
  public class CompileTarget
  {
    // default
    public CompileTargetType  Type  = CompileTargetType.PRG;

    // for tape/disk images
    public string             InternalFilename = "";

    // for iNES-Header
    public byte               NESPrgROMUnits16k = 0;
    public byte               NESChrROMUnits8k  = 0;
    public byte               NESMapper         = 0;
    public byte               NESMirroring      = 0;


  }



}