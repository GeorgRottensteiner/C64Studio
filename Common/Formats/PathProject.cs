using GR.Memory;
using RetroDevStudio;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;



namespace RetroDevStudio.Formats
{
  public class PathProject
  {
    public enum StepType
    {
      [Description( "No movement" )]
      NO_MOVEMENT,
      [Description( "North" )]
      NORTH,
      [Description( "South" )]
      SOUTH,
      [Description( "East" )]
      EAST,
      [Description( "West" )]
      WEST,
      [Description( "North-East" )]
      NORTH_EAST,
      [Description( "North-West" )]
      NORTH_WEST,
      [Description( "South-East" )]
      SOUTH_EAST,
      [Description( "South-West" )]
      SOUTH_WEST
    }

    public class Step
    {
      public StepType Type = StepType.NO_MOVEMENT;
      public int      Duration = 0;
    }

    public class Path
    {
      public string     Name = "";
      public List<Step> Steps = new List<Step>();
    }

    public List<Path> Paths = new List<Path>();


    public void Clear()
    {
      Paths.Clear();
    }



    public GR.Memory.ByteBuffer SaveToBuffer()
    {
      var projectFile = new GR.Memory.ByteBuffer();

      // version
      projectFile.AppendU32( 1 );

      var chunkProject = new GR.IO.FileChunk( FileChunkConstants.PATH_PROJECT );

      var chunkInfo = new GR.IO.FileChunk( FileChunkConstants.PATH_PROJECT_INFO );
      chunkProject.Append( chunkInfo.ToBuffer() );

      foreach ( var path in Paths )
      {
        var chunkPath = new GR.IO.FileChunk( FileChunkConstants.PATH );
        var chunkPathInfo = new GR.IO.FileChunk( FileChunkConstants.PATH_INFO );
        chunkPathInfo.AppendString( path.Name );
        chunkPath.Append( chunkPathInfo.ToBuffer() );

        var chunkSteps = new GR.IO.FileChunk( FileChunkConstants.PATH_STEPS );
        foreach ( var step in path.Steps )
        {
          var chunkStep = new GR.IO.FileChunk( FileChunkConstants.PATH_STEP );

          chunkStep.AppendU32( (uint)step.Type );
          chunkStep.AppendI32( step.Duration );
          chunkSteps.Append( chunkStep.ToBuffer() );
        }
        chunkPath.Append( chunkSteps.ToBuffer() );

        chunkProject.Append( chunkPath.ToBuffer() );
      }
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
          case FileChunkConstants.PATH_PROJECT:
            {
              var    chunkReader = chunkMain.MemoryReader();

              GR.IO.FileChunk   subChunk = new GR.IO.FileChunk();

              while ( subChunk.ReadFromStream( chunkReader ) )
              {
                var    subChunkReader = subChunk.MemoryReader();

                switch ( subChunk.Type )
                {
                  case FileChunkConstants.PATH_PROJECT_INFO:
                    {
                      var subChunkInfo = new GR.IO.FileChunk();

                      while ( subChunkInfo.ReadFromStream( subChunkReader ) )
                      {
                        var    subChunkReaderInfo = subChunkInfo.MemoryReader();

                        // nothing yet
                      }
                    }
                    break;
                  case FileChunkConstants.PATH:
                    {
                      var subChunkPath = new GR.IO.FileChunk();

                      Path    newPath = new Path();
                      Paths.Add( newPath );

                      while ( subChunkPath.ReadFromStream( subChunkReader ) )
                      {
                        var     subChunkReaderPath = subChunkPath.MemoryReader();

                        switch ( subChunkPath.Type )
                        {
                          case FileChunkConstants.PATH_INFO:
                            newPath.Name = subChunkReaderPath.ReadString();
                            break;
                          case FileChunkConstants.PATH_STEPS:
                            {
                              var subChunkPathSteps = new GR.IO.FileChunk();

                              while ( subChunkPathSteps.ReadFromStream( subChunkReaderPath ) )
                              {
                                var    subChunkReaderSteps = subChunkPathSteps.MemoryReader();

                                if ( subChunkPathSteps.Type == FileChunkConstants.PATH_STEP )
                                {
                                  var step = new Step();
                                  step.Type= (StepType)subChunkReaderSteps.ReadUInt32();
                                  step.Duration = subChunkReaderSteps.ReadInt32();

                                  newPath.Steps.Add( step );
                                }
                              }
                            }
                            break;
                        }
                      }
                    }
                    break;
                }
              }
            }
            break;
          default:
            Debug.Log( "PathProject.ReadFromBuffer unexpected chunk type " + chunkMain.Type.ToString( "X" ) );
            return false;
        }
      }
      return true;
    }



  }



}