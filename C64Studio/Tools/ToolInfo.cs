using System;
using System.Collections.Generic;
using System.Text;
using RetroDevStudio.Types.ASM;
using RetroDevStudio;
using System.ComponentModel;

namespace RetroDevStudio
{
  public class ToolInfo
  {
    public enum ToolType
    {
      UNKNOWN,
      ASSEMBLER,
      EMULATOR
    }

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
      EXPLICIT_WINDOW
    }

    public ToolType         Type = ToolType.UNKNOWN;
    public string           Name = "";
    public string           Filename = "";
    public string           WorkPath = "";
    public string           PRGArguments = "";
    public string           CartArguments = "";
    public string           DebugArguments = "";
    public string           TrueDriveOnArguments = "";
    public string           TrueDriveOffArguments = "";
    public bool             PassLabelsToEmulator = true;
    public bool             IsInternal = false;
    public Dictionary<DynamicArgument,string>   DynamicArguments = new Dictionary<DynamicArgument, string>();


    public GR.IO.FileChunk ToChunk()
    {
      GR.IO.FileChunk chunk = new GR.IO.FileChunk( FileChunkConstants.SETTINGS_TOOL );

      chunk.AppendU32( (uint)Type );
      chunk.AppendString( Name );
      chunk.AppendString( Filename );
      chunk.AppendString( PRGArguments );
      chunk.AppendString( DebugArguments );
      chunk.AppendString( WorkPath );
      chunk.AppendString( CartArguments );
      chunk.AppendString( TrueDriveOnArguments );
      chunk.AppendString( TrueDriveOffArguments );
      chunk.AppendU8( PassLabelsToEmulator ? (byte)0 : (byte)1 );
      chunk.AppendI32( DynamicArguments.Count );
      foreach ( var dynArg in DynamicArguments )
      {
        chunk.AppendI32( (int)dynArg.Key );
        chunk.AppendString( dynArg.Value );
      }

      return chunk;
    }



    public bool FromChunk( GR.IO.FileChunk Chunk )
    {
      if ( Chunk.Type != FileChunkConstants.SETTINGS_TOOL )
      {
        return false;
      }

      GR.IO.IReader reader = Chunk.MemoryReader();

      Type            = (ToolType)reader.ReadUInt32();
      Name            = reader.ReadString();
      Filename        = reader.ReadString();
      PRGArguments    = reader.ReadString();
      DebugArguments  = reader.ReadString();
      WorkPath        = reader.ReadString();
      CartArguments   = reader.ReadString();
      TrueDriveOnArguments = reader.ReadString();
      TrueDriveOffArguments = reader.ReadString();
      PassLabelsToEmulator = ( reader.ReadUInt8() == 0 );

      DynamicArguments.Clear();
      int   numDynamicArgs = reader.ReadInt32();
      for ( int i = 0; i < numDynamicArgs; ++i )
      {
        DynamicArgument   dynArg = (DynamicArgument)reader.ReadInt32();
        string    arg = reader.ReadString();

        if ( !DynamicArguments.ContainsKey( dynArg ) )
        {
          DynamicArguments.Add( dynArg, arg );
        }
        else
        {
          DynamicArguments[dynArg] = arg;
        }
      }

      MoveArgument( PRGArguments, DynamicArgument.CALL_SINGLE_FILE_TAPE );
      MoveArgument( DebugArguments, DynamicArgument.CALL_DEBUG );
      MoveArgument( CartArguments, DynamicArgument.CALL_CARTRIDGE_ROM );
      MoveArgument( TrueDriveOnArguments, DynamicArgument.CALL_VICE_TRUE_DRIVE_ON );
      MoveArgument( TrueDriveOffArguments, DynamicArgument.CALL_VICE_TRUE_DRIVE_OFF );

      return true;
    }



    private void MoveArgument( string Arguments, DynamicArgument DynArg )
    {
      if ( string.IsNullOrEmpty( Arguments ) )
      {
        return;
      }
      if ( !DynamicArguments.ContainsKey( DynArg ) )
      {
        DynamicArguments.Add( DynArg, Arguments );
      }
      else
      {
        DynamicArguments[DynArg] = Arguments;
      }
    }



    public override string ToString()
    {
      return Name;
    }



  }
}
