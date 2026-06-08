using GR.Memory;
using RetroDevStudio;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;



namespace RetroDevStudio.Formats
{
  public class SFXProject
  {
    public enum SFXPlayer
    {
      [Type( "RetroDevStudio.Audio.SIDPlayer" )]
      [Description( "RetroDev Studio SID Player (C64)" )]
      [Machine( MachineType.C64 )]
      RETRO_DEV_STUDIO_SID
    }



    public class SFXEffect
    {
      public string                 Name = "";
      public uint                   Flags = 0;      // reserved for future use
      public Dictionary<string,int> Parameters = new Dictionary<string, int>();
    }


    public SFXPlayer                Player = SFXPlayer.RETRO_DEV_STUDIO_SID;
    public List<SFXEffect>          Effects = new List<SFXEffect>();



    public void Clear()
    {
      Effects.Clear();
    }



    public GR.Memory.ByteBuffer SaveToBuffer()
    {
      var projectFile = new GR.Memory.ByteBuffer();

      // version
      projectFile.AppendU32( 1 );

      var chunkProject = new GR.IO.FileChunk( FileChunkConstants.SFX_PROJECT );

      var chunkInfo = new GR.IO.FileChunk( FileChunkConstants.SFX_PROJECT_INFO );
      chunkInfo.AppendI32( (int)Player );
      chunkProject.Append( chunkInfo.ToBuffer() );

      var chunkEffects = new GR.IO.FileChunk( FileChunkConstants.SFX_EFFECT_LIST );
      foreach ( var effect in Effects )
      {
        var chunkEffect = new GR.IO.FileChunk( FileChunkConstants.SFX_EFFECT );

        chunkEffect.AppendString( effect.Name );
        chunkEffect.AppendU32( effect.Flags );
        chunkEffect.AppendI32( effect.Parameters.Count );
        foreach ( var parameter in effect.Parameters )
        {
          chunkEffect.AppendString( parameter.Key );
          chunkEffect.AppendI32( parameter.Value );
        }

        chunkEffects.Append( chunkEffect.ToBuffer() );
      }
      chunkProject.Append( chunkEffects.ToBuffer() );

      projectFile.Append( chunkProject.ToBuffer() );

      return projectFile;
    }



    public bool ReadFromBuffer( GR.Memory.ByteBuffer DataIn )
    {
      if ( DataIn == null )
      {
        return false;
      }
      Clear();

      var memIn = DataIn.MemoryReader();
      uint     Version = memIn.ReadUInt32();

      if ( Version != 1 )
      {
        return false;
      }
      GR.IO.FileChunk   chunkMain = new GR.IO.FileChunk();

      while ( chunkMain.ReadFromStream( memIn ) )
      {
        switch ( chunkMain.Type )
        {
          case FileChunkConstants.SFX_PROJECT:
            {
              var    chunkReader = chunkMain.MemoryReader();

              GR.IO.FileChunk   subChunk = new GR.IO.FileChunk();

              while ( subChunk.ReadFromStream( chunkReader ) )
              {
                var    subChunkReader = subChunk.MemoryReader();

                switch ( subChunk.Type )
                {
                  case FileChunkConstants.SFX_PROJECT_INFO:
                    Player = (SFXPlayer)subChunkReader.ReadInt32();
                    break;
                  case FileChunkConstants.SFX_EFFECT_LIST:
                    {
                      var subChunkL = new GR.IO.FileChunk();

                      while ( subChunkL.ReadFromStream( subChunkReader ) )
                      {
                        var    subChunkReaderL = subChunkL.MemoryReader();

                        if ( subChunkL.Type == FileChunkConstants.SFX_EFFECT )
                        {
                          var sfxEffect = new SFXEffect();

                          sfxEffect.Name = subChunkReaderL.ReadString();
                          sfxEffect.Flags = subChunkReaderL.ReadUInt32();

                          int valueCount = subChunkReaderL.ReadInt32();
                          for ( int i = 0; i < valueCount; i++ )
                          {
                            string  paramName = subChunkReaderL.ReadString();
                            int paramValue    = subChunkReaderL.ReadInt32();

                            sfxEffect.Parameters.Add( paramName, paramValue );
                          }

                          Effects.Add( sfxEffect );
                        }
                      }
                    }
                    break;
                }
              }
            }
            break;
          default:
            Debug.Log( "SFXProject.ReadFromBuffer unexpected chunk type " + chunkMain.Type.ToString( "X" ) );
            return false;
        }
      }

      return true;
    }



  }



}