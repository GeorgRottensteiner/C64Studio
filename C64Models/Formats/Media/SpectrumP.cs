using GR.Memory;
using RetroDevStudio;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Schema;

namespace RetroDevStudio.Formats
{
  [MediaType( MediaType.TAPE )]
  [MediaFormat( MediaFormatType.P )]
  [Category( "Spectrum" )]
  public class SpectrumP : MediaFormat
  {
    string                      _LastError = "";
    List<ByteBuffer>            _Blocks = new List<ByteBuffer>();



    public override GR.Memory.ByteBuffer Compile()
    {
      _LastError = "";

      var result = new GR.Memory.ByteBuffer();

      //result.Append( Data );
      return result;
    }



    public override void CreateEmptyMedia()
    {
      _LastError = "";
    }



    public override void Clear()
    {
      _LastError = "";
      _Blocks.Clear();
    }



    public override List<RetroDevStudio.Types.FileInfo> Files()
    {
      _LastError = "";
      var  fileList = new List<RetroDevStudio.Types.FileInfo>();

      int entryIndex = 0;

      foreach ( var block in _Blocks )
      {
        var info = new RetroDevStudio.Types.FileInfo()
        {
          Size          = (int)block.Length - 1,
          Type          = Types.FileType.FILE,
          Data          = block.SubBuffer( 1 ),
          DirEntryIndex = entryIndex
        };
        ++entryIndex;
        fileList.Add( info );

        info.NativeType = (FileTypeNative)block.ByteAt( 0 );
        info.Info       = GR.EnumHelper.GetDescription( info.NativeType );

        if ( (FileTypeNative)block.ByteAt( 0 ) == FileTypeNative.TZX_STANDARD_SPEED_DATA_BLOCK )
        {
          if ( ( block.ByteAt( 1 + 0 ) == 0 )
          &&   ( block.ByteAt( 1 + 1 ) == 0 ) )
          {
            fileList.Last().Filename = block.SubBuffer( 1 + 2, 10 );
            fileList.Last().Type     = Types.FileType.FILE;
          }
        }
      }

      return fileList;
    }



    public override bool Load( string Filename )
    {
      _LastError = "";
      Clear();
      var data = GR.IO.File.ReadAllBytes( Filename );
      if ( data == null )
      {
        _LastError = "could not open/read file";
        return false;
      }
      string lastFilename = "";
      ushort lastDataLength = 0;
      ushort lastAutostartLine = 0;
      ushort lastProgramLength = 0;

      var reader = data.MemoryReader();
      while ( reader.DataAvailable )
      {
        if ( reader.Position + 2 > reader.Size )
        {
          _LastError = "Not enough data left for block size";
          return false;
        }
        ushort blockSize = reader.ReadUInt16();
        if ( reader.Position + blockSize > reader.Size )
        {
          _LastError = "Not enough data left for block data";
          return false;
        }
        var block = new ByteBuffer();
        reader.ReadBlock( block, blockSize );

        /*
        switch ( blockID )
        {
          case FileTypeNative.TZX_STANDARD_SPEED_DATA_BLOCK:
            {
              ushort  pause       = reader.ReadUInt16();
              ushort  dataLength  = reader.ReadUInt16();

              var binData = new ByteBuffer();
              reader.ReadBlock( binData, dataLength );

              if ( binData.Length >= 2 )
              {
                if ( binData.ByteAt( 0 ) == 0 )
                {
                  if ( !VerifyChecksum( binData ) )
                  {
                    return false;
                  }
                  switch ( binData.ByteAt( 1 ) )
                  {
                    case 0:
                      if ( dataLength != 19 )
                      {
                        _LastError = $"Data block with length <> 19 encountered";
                        return false;
                      }
                      lastFilename      = binData.ToAsciiString( 2, 10 ).TrimEnd();
                      lastDataLength    = binData.UInt16At( 12 );
                      lastAutostartLine = binData.UInt16At( 14 );
                      lastProgramLength = binData.UInt16At( 16 );
                      binData.Insert( 0, (byte)blockID, 1 );
                      _Blocks.Add( binData );
                      break;
                    default:
                      _LastError = $"standard ROM block data type {binData.ByteAt( 1 )} not supported";
                      return false;
                  }
                }
                else if ( binData.ByteAt( 0 ) == 255 )
                {
                  // data block
                  if ( !VerifyChecksum( binData ) )
                  {
                    return false;
                  }
                  binData.Insert( 0, (byte)blockID, 1 );
                  _Blocks.Add( binData );
                }
                else
                {
                  _LastError = $"fragmented block {binData.ByteAt( 0 )} not supported";
                  return false;
                }
              }
            }
            break;
          case FileTypeNative.TZX_TURBO_SPEED_DATA_BLOCK:
            _LastError = $"block type {blockID} currently not supported";
            return false;
          case FileTypeNative.TZX_PURE_TONE:
            _LastError = $"block type {blockID} currently not supported";
            return false;
          case FileTypeNative.TZX_PULSE_SEQUENCE:
            _LastError = $"block type {blockID} currently not supported";
            return false;
          case FileTypeNative.TZX_PURE_DATA_BLOCK:
            _LastError = $"block type {blockID} currently not supported";
            return false;
          case FileTypeNative.TZX_DIRECT_RECORDING_BLOCK:
            _LastError = $"block type {blockID} currently not supported";
            return false;
          case FileTypeNative.TZX_C64_ROM_BLOCK:
            _LastError = $"block type {blockID} currently not supported";
            return false;
          case FileTypeNative.TZX_C64_TURBO_BLOCK:
            _LastError = $"block type {blockID} currently not supported";
            return false;
          case FileTypeNative.TZX_CSW_RECORDING_BLOCK:
            _LastError = $"block type {blockID} currently not supported";
            return false;
          case FileTypeNative.TZX_GENERALIZED_DATA_BLOCK:
            _LastError = $"block type {blockID} currently not supported";
            return false;
          case FileTypeNative.TZX_PAUSE_OR_STOP_TAPE:
            _LastError = $"block type {blockID} currently not supported";
            return false;
          case FileTypeNative.TZX_GROUP_START:
            _LastError = $"block type {blockID} currently not supported";
            return false;
          case FileTypeNative.TZX_GROUP_END:
            _LastError = $"block type {blockID} currently not supported";
            return false;
          case FileTypeNative.TZX_JUMP_TO_BLOCK:
            _LastError = $"block type {blockID} currently not supported";
            return false;
          case FileTypeNative.TZX_LOOP_START:
            _LastError = $"block type {blockID} currently not supported";
            return false;
          case FileTypeNative.TZX_LOOP_END:
            _LastError = $"block type {blockID} currently not supported";
            return false;
          case FileTypeNative.TZX_CALL_SEQUENCE:
            _LastError = $"block type {blockID} currently not supported";
            return false;
          case FileTypeNative.TZX_RETURN_FROM_SEQUENCE:
            _LastError = $"block type {blockID} currently not supported";
            return false;
          case FileTypeNative.TZX_SELECT_BLOCK:
            _LastError = $"block type {blockID} currently not supported";
            return false;
          case FileTypeNative.TZX_STOP_TAPE_48K:
            _LastError = $"block type {blockID} currently not supported";
            return false;
          case FileTypeNative.TZX_SET_SIGNAL:
            _LastError = $"block type {blockID} currently not supported";
            return false;
          case FileTypeNative.TZX_TEXT_DESCRIPTION:
            {
              var totalBlock = new ByteBuffer();
              totalBlock.AppendU8( (byte)blockID );

              uint   length = reader.ReadUInt8();

              totalBlock.AppendU8( (byte)length );

              var text = new ByteBuffer();
              reader.ReadBlock( totalBlock, length );

              _Blocks.Add( totalBlock );
            }
            break;
          case FileTypeNative.TZX_MESSAGE:
            _LastError = $"block type {blockID} currently not supported";
            return false;
          case FileTypeNative.TZX_ARCHIVE_INFO:
            _LastError = $"block type {blockID} currently not supported";
            return false;
          case FileTypeNative.TZX_HARDWARE_TYPE:
            _LastError = $"block type {blockID} currently not supported";
            return false;
          case FileTypeNative.TZX_EMULATION_INFO:
            _LastError = $"block type {blockID} currently not supported";
            return false;
          case FileTypeNative.TZX_CUSTOM_INFO:
            _LastError = $"block type {blockID} currently not supported";
            return false;
          case FileTypeNative.TZX_SCREEN_BLOCK:
            _LastError = $"block type {blockID} currently not supported";
            return false;
          case FileTypeNative.TZX_SKIP_GLUE_BLOCK:
            _LastError = $"block type {blockID} currently not supported";
            return false;
          default:
            _LastError = $"Unsupported block ID {blockID}";
            return false;
        }*/
      }
      return true;
    }



    private bool VerifyChecksum( ByteBuffer data )
    {
      byte  checkSum = 0;

      for ( int i = 0; i < data.Length - 1; ++i )
      {
        checkSum ^= data.ByteAt( i );
      }
      return checkSum == data.ByteAt( (int)data.Length - 1 );
    }



    public override bool Save( string Filename )
    {
      _LastError = "";
      GR.Memory.ByteBuffer data = Compile();
      return GR.IO.File.WriteAllBytes( Filename, data );
    }



    public override Types.FileInfo LoadFile( GR.Memory.ByteBuffer Filename )
    {
      _LastError = "";

      var files = Files();

      var file = files.FirstOrDefault( f => f.Filename == Filename );
      if ( file == null )
      {
        _LastError = "file not found";
        return null;
      }
      return file;
    }



    public override bool WriteFile( GR.Memory.ByteBuffer Filename, GR.Memory.ByteBuffer Content, Types.FileTypeNative Type )
    {
      _LastError = "";
      /*
      if ( ( Data.Length > 0 )
      ||   ( Type != Types.FileType.PRG ) )
      {
        _LastError = "invalid file type";
        return false;
      }
      Data = new GR.Memory.ByteBuffer( Content );
      this.Filename = Filename;*/
      return true;
    }



    public override bool RenameFile( GR.Memory.ByteBuffer Filename, GR.Memory.ByteBuffer NewFilename )
    {
      _LastError = "";
      //NewFilename.CopyTo( this.Filename, 0, 16 );
      return true;
    }



    public override bool DeleteFile( GR.Memory.ByteBuffer Filename, bool CompleteDelete )
    {
      _LastError = "";
      /*
      if ( Filename.Compare( this.Filename ) != 0 )
      {
        _LastError = "file not found";
        return false;
      }
      Data.Clear();*/
      return true;
    }



    public override int FreeSlots
    {
      get
      {
        /*
        if ( Data.Length == 0 )
        {
          return 1;
        }
        */
        return 0;
      }
    }



    public override int Slots
    {
      get
      {
        return 1;
      }
    }



    public override string FileFilter
    {
      get
      {
        return "TZX Files|*.TZX|" + base.FileFilter;
      }
    }



    public override string LastError
    {
      get
      { 
        return _LastError;
      }
    }



    public override MediaFilenameType FilenameType
    {
      get
      {
        return MediaFilenameType.SPECTRUM;
      }
    }




  }



}
