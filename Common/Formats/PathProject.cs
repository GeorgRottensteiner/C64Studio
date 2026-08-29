using GR.Memory;
using RetroDevStudio;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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



      public void AdvancePosition( ref int curX, ref int curY )
      {
        switch ( Type )
        {
          case StepType.EAST:
            curX += Duration;
            break;
          case StepType.WEST:
            curX -= Duration;
            break;
          case StepType.NORTH:
            curY -= Duration;
            break;
          case StepType.SOUTH:
            curY += Duration;
            break;
          case StepType.SOUTH_WEST:
            curX -= Duration;
            curY += Duration;
            break;
          case StepType.SOUTH_EAST:
            curX += Duration;
            curY += Duration;
            break;
          case StepType.NORTH_WEST:
            curX -= Duration;
            curY -= Duration;
            break;
          case StepType.NORTH_EAST:
            curX += Duration;
            curY -= Duration;
            break;
        }
      }
    }

    public class Path
    {
      public string     Name = "";
      public List<Step> Steps = new List<Step>();
    }

    public class ValueDescriptor
    {
      // false = StepType, true = Duration
      public StepType   Step = StepType.NO_MOVEMENT;

      // default 0, may be 1 or more (split value as more than one byte)
      public int        AddressOffsetStep = 0;
      public int        AddressOffsetDuration = 0;
      public byte       ValueStep = 0;

      // how to apply the value in the final byte
      public byte       RelevantBitsStep        = 0xff;
      public int        ShiftBitsLeftDuration   = 0;
      public int        ShiftBitsRightDuration  = 0;
      public uint       RelevantBitsDuration    = 0xff;
    }



    public List<Path> Paths = new List<Path>();
    public List<ValueDescriptor> ValueDescriptors = new List<ValueDescriptor>();



    public void Clear()
    {
      Paths.Clear();
      ValueDescriptors.Clear();
    }



    public GR.Memory.ByteBuffer SaveToBuffer()
    {
      var projectFile = new GR.Memory.ByteBuffer();

      // version
      projectFile.AppendU32( 1 );

      var chunkProject = new GR.IO.FileChunk( FileChunkConstants.PATH_PROJECT );

      var chunkInfo = new GR.IO.FileChunk( FileChunkConstants.PATH_PROJECT_INFO );
      chunkProject.Append( chunkInfo.ToBuffer() );

      foreach ( var valueDesc in ValueDescriptors )
      {
        var chunkVD = new GR.IO.FileChunk( FileChunkConstants.PATH_PROJECT_VALUE_DESCRIPTOR );
        chunkVD.AppendI32( (int)valueDesc.Step );

        chunkVD.AppendU8( valueDesc.ValueStep );
        chunkVD.AppendI32( valueDesc.AddressOffsetStep );
        chunkVD.AppendU8( valueDesc.RelevantBitsStep );

        chunkVD.AppendI32( valueDesc.AddressOffsetDuration );
        chunkVD.AppendU32( valueDesc.RelevantBitsDuration );
        chunkVD.AppendI32( valueDesc.ShiftBitsLeftDuration );
        chunkVD.AppendI32( valueDesc.ShiftBitsRightDuration );
        
        chunkProject.Append( chunkVD.ToBuffer() );
      }

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
                  case FileChunkConstants.PATH_PROJECT_VALUE_DESCRIPTOR:
                    {
                      var subChunkInfo = new GR.IO.FileChunk();

                      while ( subChunkInfo.ReadFromStream( subChunkReader ) )
                      {
                        var    subChunkReaderVD = subChunkInfo.MemoryReader();

                        // nothing yet
                        var vd = new ValueDescriptor
                        {
                          Step = (StepType)subChunkReaderVD.ReadInt32(),
                          ValueStep = subChunkReaderVD.ReadUInt8(),
                          AddressOffsetStep = subChunkReaderVD.ReadInt32(),
                          RelevantBitsStep = subChunkReaderVD.ReadUInt8(),

                          AddressOffsetDuration = subChunkReaderVD.ReadInt32(),
                          RelevantBitsDuration = subChunkReaderVD.ReadUInt32(),
                          ShiftBitsLeftDuration = subChunkReaderVD.ReadInt32(),
                          ShiftBitsRightDuration = subChunkReaderVD.ReadInt32()
                        };

                        ValueDescriptors.Add( vd );
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
      FillDefaultDescriptors();
      return true;
    }



    public void SetDefaultDescriptors()
    {
      ValueDescriptors.Clear();
      FillDefaultDescriptors();
    }



    public void FillDefaultDescriptors()
    {
      if ( !ValueDescriptors.Any( vd => vd.Step == StepType.NO_MOVEMENT ) )
      {
        ValueDescriptors.Add( new PathProject.ValueDescriptor() 
          {
            AddressOffsetStep     = 0,
            Step                  = StepType.NO_MOVEMENT,
            ValueStep             = 0,
            RelevantBitsStep      = 0x0f,
            AddressOffsetDuration = 1,
            RelevantBitsDuration  = 0xff
          } );
      }
      if ( !ValueDescriptors.Any( vd => vd.Step == StepType.NORTH ) )
      {
        ValueDescriptors.Add( new PathProject.ValueDescriptor() 
        {
          AddressOffsetStep     = 0,
          Step                  = StepType.NORTH,
          ValueStep             = 1,
          RelevantBitsStep      = 0x0f,
          AddressOffsetDuration = 1,
          RelevantBitsDuration  = 0xff
        } );
      }
      if ( !ValueDescriptors.Any( vd => vd.Step == StepType.NORTH_EAST ) )
      {
        ValueDescriptors.Add( new PathProject.ValueDescriptor() 
        {
          AddressOffsetStep     = 0,
          Step                  = StepType.NORTH_EAST,
          ValueStep             = 2,
          RelevantBitsStep      = 0x0f,
          AddressOffsetDuration = 1,
          RelevantBitsDuration  = 0xff
        } );
      }
      if ( !ValueDescriptors.Any( vd => vd.Step == StepType.EAST ) )
      {
        ValueDescriptors.Add( new PathProject.ValueDescriptor() 
        {
          AddressOffsetStep     = 0,
          Step                  = StepType.EAST,
          ValueStep             = 3,
          RelevantBitsStep      = 0x0f,
          AddressOffsetDuration = 1,
          RelevantBitsDuration  = 0xff
        } );
      }
      if ( !ValueDescriptors.Any( vd => vd.Step == StepType.SOUTH_EAST ) )
      {
        ValueDescriptors.Add( new PathProject.ValueDescriptor() 
        {
          AddressOffsetStep     = 0,
          Step                  = StepType.SOUTH_EAST,
          ValueStep             = 4,
          RelevantBitsStep      = 0x0f,
          AddressOffsetDuration = 1,
          RelevantBitsDuration  = 0xff
        } );
      }
      if ( !ValueDescriptors.Any( vd => vd.Step == StepType.SOUTH ) )
      {
        ValueDescriptors.Add( new PathProject.ValueDescriptor() 
        {
          AddressOffsetStep     = 0,
          Step                  = StepType.SOUTH,
          ValueStep             = 5,
          RelevantBitsStep      = 0x0f,
          AddressOffsetDuration = 1,
          RelevantBitsDuration  = 0xff
        } );
      }
      if ( !ValueDescriptors.Any( vd => vd.Step == StepType.SOUTH_WEST ) )
      {
        ValueDescriptors.Add( new PathProject.ValueDescriptor() 
        {
          AddressOffsetStep     = 0,
          Step                  = StepType.SOUTH_WEST,
          ValueStep             = 6,
          RelevantBitsStep      = 0x0f,
          AddressOffsetDuration = 1,
          RelevantBitsDuration  = 0xff
        } );
      }
      if ( !ValueDescriptors.Any( vd => vd.Step == StepType.WEST ) )
      {
        ValueDescriptors.Add( new PathProject.ValueDescriptor() 
        {
          AddressOffsetStep     = 0,
          Step                  = StepType.WEST,
          ValueStep             = 7,
          RelevantBitsStep      = 0x0f,
          AddressOffsetDuration = 1,
          RelevantBitsDuration  = 0xff
        } );
      }
      if ( !ValueDescriptors.Any( vd => vd.Step == StepType.NORTH_WEST ) )
      {
        ValueDescriptors.Add( new PathProject.ValueDescriptor() 
        {
          AddressOffsetStep     = 0,
          Step                  = StepType.NORTH_WEST,
          ValueStep             = 8,
          RelevantBitsStep      = 0x0f,
          AddressOffsetDuration = 1,
          RelevantBitsDuration  = 0xff
        } );
      }
    }



  }



}