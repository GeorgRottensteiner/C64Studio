using System;
using System.Collections.Generic;
using System.Text;
using RetroDevStudio.Types.ASM;
using RetroDevStudio;
using System.ComponentModel;
using RetroDevStudio.Types;
using RetroDevStudio.Emulators;

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



    public static void SetDefaultRunArguments( ToolInfo Tool )
    {
      Tool.WorkPath = "\"$(RunPath)\"";
      Tool.Type = ToolInfo.ToolType.EMULATOR;
      Tool.PRGArguments = "\"$(RunFilename)\"";
      Tool.CartArguments = "";
      Tool.DebugArguments = "";
      Tool.TrueDriveOnArguments = "";
      Tool.TrueDriveOffArguments = "";
      Tool.PassLabelsToEmulator = false;

      string upperCaseFilename = GR.Path.GetFileNameWithoutExtension( Tool.Filename ).ToUpper();

      if ( Emulators.EmulatorInfo.IsVICEFamily( Tool.Filename ) )
      {
        // VICE
        Tool.Name = "WinVICE";
        Tool.PRGArguments = "\"$(RunFilename)\"";
        Tool.CartArguments = "-cartcrt \"$(RunFilename)\"";
        Tool.DebugArguments = "-initbreak 0x$(DebugStartAddressHex) -remotemonitor";
        Tool.PassLabelsToEmulator = true;

        if ( Emulators.EmulatorInfo.IsVICEVersionOldTrueDrive( Tool.Filename ) )
        {
          Tool.TrueDriveOnArguments = "-truedrive +virtualdev";
          Tool.TrueDriveOffArguments = "+truedrive -virtualdev";
        }
        else
        {
          Tool.TrueDriveOnArguments = "-drive8truedrive +virtualdev8";
          Tool.TrueDriveOffArguments = "+drive8truedrive -virtualdev8";
        }
      }
      else if ( upperCaseFilename.StartsWith( "CCS64" ) )
      {
        // CCS64
        Tool.Name = "CCS64";
        Tool.PassLabelsToEmulator = false;
      }
      else if ( Emulators.EmulatorInfo.IsMega65Family( Tool.Filename ) )
      {
        // XMEGA65
        Tool.Name = "XMEGA65";
        Tool.PRGArguments = "-prg \"$(RunFilename)\"";
      }
      else if ( upperCaseFilename.StartsWith( "STELLA" ) )
      {
        // Atari Stella
        Tool.Name = "Stella";
      }
      else
      {
        // fallback
        Tool.Name = upperCaseFilename;
      }
    }




  }
}
