using GR.Image;
using GR.Memory;
using LibGit2Sharp;
using RetroDevStudio.Parser;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using WeifenLuo.WinFormsUI.Docking;
using static RetroDevStudio.Parser.AssemblerSettings;


namespace RetroDevStudio.Audio
{
  public class Audio
  {
    StudioCore                  Core;

    List<SFXPlayerDescriptor>   SFXPlayers = new List<SFXPlayerDescriptor>();



    public Audio( StudioCore Core )
    {
      this.Core = Core;
    }



    private string BasePath()
    {
#if DEBUG
      string    sfxPlayerBasePath = @"../../../../C64StudioRelease/shared content/SFXPlayers";
#else
      string    sfxPlayerBasePath = @"SFXPlayers";
#endif
      return sfxPlayerBasePath;
    }



    public void InitSFXEditors()
    {
      var sfxPlayerBasePath = System.IO.Path.GetFullPath( BasePath() );
      var folders = System.IO.Directory.GetDirectories( sfxPlayerBasePath );
      SFXPlayers = new List<SFXPlayerDescriptor>();
      foreach ( var systemfolder in folders )
      {
        var sysFolderName = GR.Path.GetFileName( systemfolder ).ToUpper();
        var machine = MachineType.ANY;
        if ( !Enum.GetNames( typeof( MachineType ) ).Contains( sysFolderName ) )
        {
          foreach ( MachineType machineType in Enum.GetValues( typeof( MachineType ) ) )
          {
            if ( GR.EnumHelper.GetDescription( machineType ).ToUpper() == sysFolderName )
            {
              machine = machineType;
              break;
            }
          }
          if ( machine == MachineType.ANY )
          {
            Debug.Log( $"Unsupported sample machine type {sysFolderName}" );
            continue;
          }
        }
        else
        {
          machine = (MachineType)System.Enum.Parse( typeof( MachineType ), sysFolderName, true );
        }

        var playerFolders = System.IO.Directory.GetDirectories( systemfolder );
        foreach ( var playerFolder in playerFolders )
        {
          var metaFile = GR.Path.Append( playerFolder, "metadata.xml" );

          if ( !System.IO.File.Exists( metaFile ) )
          {
            continue;
          }

          var parser = new GR.Strings.XMLParser();

          if ( parser.Parse( GR.IO.File.ReadAllText( metaFile ), false ) )
          {
            var xmlSFXPlayer = parser.FindByType( "SFXPlayer" );
            if ( xmlSFXPlayer != null )
            {
              var sfxPlayer = new SFXPlayerDescriptor()
              {
                Machine                 = machine,
                PlayerCodeAssembly      = xmlSFXPlayer.Attribute( "Assembly" ),
                Name                    = xmlSFXPlayer.Attribute( "Name" ),
                PlayerCodeAddress       = GR.Convert.ToI32( xmlSFXPlayer.Attribute( "CodeAddress" ) ),
                AddressToStartPlayer    = GR.Convert.ToI32( xmlSFXPlayer.Attribute( "AddressToStartPlayer" ) ),
                AddressToTriggerPlaying = GR.Convert.ToI32( xmlSFXPlayer.Attribute( "AddressToTriggerPlaying" ) ),
                ValueToTriggerPlaying   = GR.Convert.ToU8( xmlSFXPlayer.Attribute( "ValueToTriggerPlaying" ) ),
                CanReplay               = GR.Convert.ToBoolean( xmlSFXPlayer.Attribute( "CanReplay" ) )
              };

              var xmlParams = xmlSFXPlayer.FindByType( "Parameters" );
              if ( xmlParams != null )
              {
                foreach ( var xmlParam in xmlParams )
                {
                  var valueDescriptor = new ValueDescriptor()
                  {
                    Name              = xmlParam.Attribute( "Name" ),
                    AddressToWriteTo  = GR.Convert.ToI32( xmlParam.Attribute( "AddressToWriteTo" ) ),
                    MinValue          = GR.Convert.ToI32( xmlParam.Attribute( "MinValue" ) ),
                    MaxValue          = GR.Convert.ToI32( xmlParam.Attribute( "MaxValue" ) ),
                    ShiftBitsLeft     = GR.Convert.ToI32( xmlParam.Attribute( "ShiftBitsLeft" ) ),
                    ShiftBitsRight    = GR.Convert.ToI32( xmlParam.Attribute( "ShiftBitsRight" ) ),
                    RelevantBits      = GR.Convert.ToI32( xmlParam.Attribute( "RelevantBits" ) )
                  };
                  foreach ( var validValue in xmlParam )
                  {
                    if ( validValue.Type == "Value" )
                    {
                      valueDescriptor.ValidValues.Add( validValue.Attribute( "Name" ), GR.Convert.ToI32( validValue.Attribute( "Value" ) ) );
                    }
                  }

                  sfxPlayer.Parameters.Add( valueDescriptor );
                }
              }
              if ( CompilePlayer( sfxPlayer, GR.Path.Append( playerFolder, sfxPlayer.PlayerCodeAssembly ) ) )
              {
                SFXPlayers.Add( sfxPlayer );
              }
            }
          }
          else
          {
            Debug.Log( $"Failed to parse metadata.xml: {parser.ParseError()}" );
          }
        }
      }
    }



    private bool CompilePlayer( SFXPlayerDescriptor sfxPlayer, string assemblyPath )
    {
      var assembler = new RetroDevStudio.Parser.ASMFileParser();

      assembler.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      var config = new RetroDevStudio.Parser.CompileConfig()
      {
        OutputFile = "player.bin",
        TargetType = RetroDevStudio.Types.CompileTargetType.PLAIN,
        Assembler  = RetroDevStudio.Types.AssemblerType.C64_STUDIO
      };

      bool parseResult = assembler.ParseFile( assemblyPath, GR.IO.File.ReadAllText( assemblyPath ), null, config, null,
                                               null, out Types.ASM.FileInfo asmFileInfo );
      if ( !parseResult )
      {
        Debug.Log( "Testassemble failed:" );
        foreach ( var msg in asmFileInfo.Messages )
        {
          Debug.Log( msg.Key + ":" + msg.Value.AlternativeFile + ":" + msg.Value.AlternativeLineIndex + ":" + msg.Value.Message );
        }
        return false;
      }
      if ( !assembler.Assemble( new Parser.CompileConfig() { OutputFile = "player.bin" } ) )
      {
        Debug.Log( $"failed to assemble player for {sfxPlayer.Name}: {assembler.Errors} errors" );
        return false;
      }
      sfxPlayer.PlayerCode = assembler.AssembledOutput.Assembly;

      return true;
    }



  }
}
