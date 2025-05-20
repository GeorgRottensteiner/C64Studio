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

    public ToolType                             Type = ToolType.UNKNOWN;
    public string                               Name = "";
    public string                               Filename = "";
    public string                               WorkPath = "";
    public string                               PRGArguments = "";
    public string                               CartArguments = "";
    public string                               DebugArguments = "";
    public string                               TrueDriveOnArguments = "";
    public string                               TrueDriveOffArguments = "";
    public bool                                 PassLabelsToEmulator = true;
    public bool                                 IsInternal = false;
    public List<DynamicArgument>                ArgumentOrder = new List<DynamicArgument>();   
    private Dictionary<DynamicArgument,string>  _dynamicArguments = new Dictionary<DynamicArgument, string>();


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
      chunk.AppendI32( _dynamicArguments.Count );
      foreach ( var dynArg in _dynamicArguments )
      {
        chunk.AppendI32( (int)dynArg.Key );
        chunk.AppendString( dynArg.Value );
      }
      chunk.AppendI32( ArgumentOrder.Count );
      foreach ( var dynArg in ArgumentOrder )
      {
        chunk.AppendI32( (int)dynArg );
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

      _dynamicArguments.Clear();
      int   numDynamicArgs = reader.ReadInt32();
      for ( int i = 0; i < numDynamicArgs; ++i )
      {
        DynamicArgument   dynArg = (DynamicArgument)reader.ReadInt32();
        string    arg = reader.ReadString();

        if ( !_dynamicArguments.ContainsKey( dynArg ) )
        {
          _dynamicArguments.Add( dynArg, arg );
        }
        else
        {
          _dynamicArguments[dynArg] = arg;
        }
      }
      int   countArgOrder = reader.ReadInt32();
      for ( int i = 0; i < countArgOrder; ++i )
      {
        DynamicArgument   dynArg = (DynamicArgument)reader.ReadInt32();
        ArgumentOrder.Add( dynArg );
      }
      if ( countArgOrder == 0 )
      {
        // is that a good idea?
        ArgumentOrder.AddRange( _dynamicArguments.Keys );
      }

      MoveArgument( PRGArguments, DynamicArgument.CALL_SINGLE_FILE_TAPE );
      MoveArgument( DebugArguments, DynamicArgument.CALL_DEBUG );
      MoveArgument( CartArguments, DynamicArgument.CALL_CARTRIDGE_ROM );
      MoveArgument( TrueDriveOnArguments, DynamicArgument.CALL_VICE_TRUE_DRIVE_ON );
      MoveArgument( TrueDriveOffArguments, DynamicArgument.CALL_VICE_TRUE_DRIVE_OFF );

      return true;
    }



    public string Argument( DynamicArgument arg )
    {
      if ( _dynamicArguments.TryGetValue( arg, out string value ) )
      {
        return value;
      }
      return "";
    }



    public void Argument( DynamicArgument arg, string argValue )
    {
      _dynamicArguments[arg] = argValue;
    }



    private void MoveArgument( string Arguments, DynamicArgument DynArg )
    {
      if ( string.IsNullOrEmpty( Arguments ) )
      {
        return;
      }
      if ( !_dynamicArguments.ContainsKey( DynArg ) )
      {
        _dynamicArguments.Add( DynArg, Arguments );
      }
      else
      {
        _dynamicArguments[DynArg] = Arguments;
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
