using RetroDevStudio.Types;
using RetroDevStudio;
using System;
using System.Collections.Generic;
using System.Text;
using GR.Memory;

namespace RetroDevStudio.Formats
{
  public class AmigaDisk : MediaFormat
  {
    protected string        _LastError = "";

    protected int           BOOT_BLOCK_INDEX  = 0;
    protected int           ROOT_BLOCK_INDEX  = 880;    // for SD, 1760 for HD
    protected int           BLOCK_SIZE        = 512;

    protected string        _CurrentDirectory = "/";

    protected ByteBuffer    _DiskImage = new ByteBuffer();



    // Object		      Related logical blocks
    // ------------+----------------------------------------------------------------
    // Volume         Rootblock, Bitmap block
    // File           File Header block, Extension block, Data block, Link block
    // Directory      Rootblock, Directory block, Directory Cache block, Link block



    public AmigaDisk()
    {
      RootFolder = "/";
    }



    public override bool SupportsFolders
    {
      get
      {
        return true;
      }
    }



    public bool IsProfessionalFileSystem
    {
      get
      {
        if ( ( _DiskImage.Length == 0 )
        ||   ( _DiskImage.ByteAt( BOOT_BLOCK_INDEX * BLOCK_SIZE + 0 ) != 'P' )
        ||   ( _DiskImage.ByteAt( BOOT_BLOCK_INDEX * BLOCK_SIZE + 1 ) != 'F' )
        ||   ( _DiskImage.ByteAt( BOOT_BLOCK_INDEX * BLOCK_SIZE + 2 ) != 'S' ) )
        {
          return false;
        }
        return true;
      }
    }



    public bool IsFastFileSystem
    {
      get
      {
        if ( _DiskImage.Length == 0 )
        {
          return false;
        }
        if ( ( _DiskImage.ByteAt( BOOT_BLOCK_INDEX * BLOCK_SIZE + 0 ) != 'D' )
        ||   ( _DiskImage.ByteAt( BOOT_BLOCK_INDEX * BLOCK_SIZE + 1 ) != 'O' )
        ||   ( _DiskImage.ByteAt( BOOT_BLOCK_INDEX * BLOCK_SIZE + 2 ) != 'S' ) )
        {
          return false;
        }
        return ( _DiskImage.ByteAt( BOOT_BLOCK_INDEX * BLOCK_SIZE + 3 ) & 1 ) != 0;
      }
    }



    private ByteBuffer Block( int BlockIndex )
    {
      if ( ( BlockIndex < 0 )
      ||   ( BlockIndex * BLOCK_SIZE >= _DiskImage.Length ) )
      {
        return new ByteBuffer();
      }

      return _DiskImage.SubBuffer( BLOCK_SIZE * BlockIndex, BLOCK_SIZE );
    }



    public override void Clear()
    {
      _LastError = "";
      CreateEmptyMedia();
    }



    public override bool Load( string Filename )
    {
      _LastError = "";
      GR.Memory.ByteBuffer diskData = GR.IO.File.ReadAllBytes( Filename );
      if ( diskData == null )
      {
        _LastError = "Could not open/read file";
        return false;
      }

      // bytes/sector	sector/track	track/cyl	cyl/disk
      // ------------------------------------------------------------------------
      // DD disks	512  11               2      80
      // HD disks 512	 22               2      80

      if ( ( diskData.Length != 901120 )          // DD
      &&   ( diskData.Length != 2 * 901120 ) )    // HD
      {
        _LastError = "disk image size is not supported";
        return false;
      }

      _DiskImage = diskData;
      switch ( _DiskImage.Length )
      {
        case 901120:
          CreateEmptyMedia();
          break;
        case 2 * 901120:
          //CreateEmptyMedia40Tracks();
          break;
      }

      _CurrentDirectory           = "/";
      return true;
    }



    public override bool Save( string Filename )
    {
      GR.Memory.ByteBuffer data = Compile();
      return GR.IO.File.WriteAllBytes( Filename, data );
    }



    public override Types.FileInfo LoadFile( GR.Memory.ByteBuffer Filename )
    {
      _LastError = "File not found";
      return null;
    }



    public override bool DeleteFile( GR.Memory.ByteBuffer Filename )
    {
      _LastError = "";
      _LastError = "file not found";
      return false;
    }



    public override bool RenameFile( GR.Memory.ByteBuffer Filename, GR.Memory.ByteBuffer NewFilename )
    {
      _LastError = "file not found";
      return false;
    }



    public override bool WriteFile( GR.Memory.ByteBuffer Filename, GR.Memory.ByteBuffer Content, Types.FileType Type )
    {
      _LastError = "file too large";
      return false;
    }



    public bool MoveFileUp( Types.FileInfo File )
    {
      _LastError = "";
      return false;
    }



    public bool MoveFileDown( Types.FileInfo File )
    {
      _LastError = "";
      return false;
    }



    public override string FileFilter
    {
      get
      {
        return "Disk Files|*.ADF|" + base.FileFilter;
      }
    }



    public override string LastError
    {
      get 
      {
        return _LastError;
      }
    }



    private int LocateDirectoryBlock( string DirectoryName )
    {
      string    dir = DirectoryName;
      if ( dir.EndsWith( "/" ) )
      {
        dir = dir.Substring( 0, dir.Length - 1 );
      }
      var parts = dir.Split( '/' );
      int partPos = 1;

      int curDirectoryBlock = ROOT_BLOCK_INDEX;
      int blockOffset = BLOCK_SIZE * curDirectoryBlock;

      if ( dir == "" )
      {
        return curDirectoryBlock;
      }

      while ( partPos < parts.Length )
      {
        uint hashTableSize = _DiskImage.UInt32NetworkOrderAt( blockOffset + 12 );
        bool foundDirEntry = false;
        for ( uint i = 0; i < hashTableSize; ++i )
        {
          uint hashEntry = _DiskImage.UInt32NetworkOrderAt( (int)( blockOffset + 24 + i * 4 ) );
          while ( hashEntry != 0 )
          {
            //int filenameLength  = _DiskImage.ByteAt( (int)( hashEntry * BLOCK_SIZE + BLOCK_SIZE - 80 ) );
            //info.Filename = _DiskImage.SubBuffer( (int)( hashEntry * BLOCK_SIZE + BLOCK_SIZE - 79 ), filenameLength );
            //info.Size = (int)_DiskImage.UInt32NetworkOrderAt( (int)( hashEntry * BLOCK_SIZE + BLOCK_SIZE - 188 ) );

            uint fileType = _DiskImage.UInt32NetworkOrderAt( (int)( hashEntry * BLOCK_SIZE + BLOCK_SIZE - 4 ) );

            //uint fileType = _DiskImage.UInt32NetworkOrderAt( (int)( hashEntry + BLOCK_SIZE - 4 ) );
            if ( fileType == 2 )
            {
              int filenameLength  = _DiskImage.ByteAt( (int)( hashEntry * BLOCK_SIZE + BLOCK_SIZE - 80 ) );
              var filename        = _DiskImage.SubBuffer( (int)( hashEntry * BLOCK_SIZE + BLOCK_SIZE - 79 ), filenameLength );

              if ( filename.ToAsciiString() == parts[partPos] )
              {
                curDirectoryBlock = (int)hashEntry;
                blockOffset       = BLOCK_SIZE * curDirectoryBlock;
                foundDirEntry     = true;
                break;
              }
            }
            if ( foundDirEntry )
            {
              break;
            }
            // follow hash chain
            hashEntry = _DiskImage.UInt32NetworkOrderAt( (int)( hashEntry * BLOCK_SIZE + BLOCK_SIZE - 16 ) );
          }
          if ( foundDirEntry )
          {
            break;
          }
        }
        if ( !foundDirEntry )
        {
          return -1;
        }
        ++partPos;
      }
      Debug.Log( $"Directory {DirectoryName} found at block {curDirectoryBlock}" );

      return curDirectoryBlock;
    }



    private int LocateFileBlock( string Filename )
    {
      var parts = Filename.Split( '/' );
      int partPos = 1;

      int curDirectoryBlock = ROOT_BLOCK_INDEX;
      int blockOffset = BLOCK_SIZE * curDirectoryBlock;

      while ( partPos < parts.Length )
      {
        uint hashTableSize = _DiskImage.UInt32NetworkOrderAt( blockOffset + 12 );
        bool foundDirEntry = false;
        for ( uint i = 0; i < hashTableSize; ++i )
        {
          uint hashEntry = _DiskImage.UInt32NetworkOrderAt( (int)( blockOffset + 24 + i * 4 ) );
          while ( hashEntry != 0 )
          {
            uint fileType = _DiskImage.UInt32NetworkOrderAt( (int)( blockOffset + BLOCK_SIZE - 4 ) );
            if ( ( partPos + 1 < parts.Length )
            &&   ( fileType == 2 ) )
            {
              int filenameLength  = _DiskImage.ByteAt( (int)( blockOffset + BLOCK_SIZE - 80 ) );
              var filename        = _DiskImage.SubBuffer( (int)( blockOffset + BLOCK_SIZE - 79 ), filenameLength );

              if ( filename.ToAsciiString() == parts[partPos] )
              {
                curDirectoryBlock = (int)hashEntry;
                foundDirEntry = true;
                break;
              }
            }
            if ( ( partPos + 1 == parts.Length )
            &&   ( fileType != 2 ) )
            {
              // the actual file
              int filenameLength  = _DiskImage.ByteAt( (int)( blockOffset + BLOCK_SIZE - 80 ) );
              var filename        = _DiskImage.SubBuffer( (int)( blockOffset + BLOCK_SIZE - 79 ), filenameLength );

              if ( filename.ToAsciiString() == parts[partPos] )
              {
                return (int)hashEntry;
              }
            }
            if ( foundDirEntry )
            {
              break;
            }
            // follow hash chain
            hashEntry = _DiskImage.UInt32NetworkOrderAt( (int)( hashEntry * BLOCK_SIZE + BLOCK_SIZE - 16 ) );
          }
        }
        if ( !foundDirEntry )
        {
          return -1;
        }
      }
      return -1;
    }



    public override List<RetroDevStudio.Types.FileInfo> Files()
    {
      _LastError = "";
      var files = new List<FileInfo>();

      int dirBlockIndex = LocateDirectoryBlock( _CurrentDirectory );

      //uint hashTableSize = _DiskImage.UInt32NetworkOrderAt( BLOCK_SIZE * dirBlockIndex + 12 );
      uint hashTableSize = (uint)( ( BLOCK_SIZE / 4 ) - 56 );
      for ( uint i = 0; i < hashTableSize; ++i )
      {
        uint hashEntry = _DiskImage.UInt32NetworkOrderAt( (int)( BLOCK_SIZE * dirBlockIndex + 24 + i * 4 ) );
        while ( hashEntry != 0 )
        {
          var info = new FileInfo();

          int filenameLength  = _DiskImage.ByteAt( (int)( hashEntry * BLOCK_SIZE + BLOCK_SIZE - 80 ) );
          info.Filename       = _DiskImage.SubBuffer( (int)( hashEntry * BLOCK_SIZE + BLOCK_SIZE - 79 ), filenameLength );
          info.Size           = (int)_DiskImage.UInt32NetworkOrderAt( (int)( hashEntry * BLOCK_SIZE + BLOCK_SIZE - 188 ) );

          uint fileType = _DiskImage.UInt32NetworkOrderAt( (int)( hashEntry * BLOCK_SIZE + BLOCK_SIZE - 4 ) );

          Debug.Log( $"HashEntry #{i}, hash value {hashEntry}, has entry {info.Filename.ToAsciiString()}, type {fileType}" );

          switch ( fileType )
          {
            case 0xfffffffd:
              // -3
              info.Type = FileType.PRG;
              break;
            case 2:
              info.Type = FileType.DIR;
              break;
            default:
              Debug.Log( $"Unsupported file type in block! {fileType.ToString( "X" )}" );
              break;
          }

          files.Add( info );

          // follow hash chain
          hashEntry = _DiskImage.UInt32NetworkOrderAt( (int)( hashEntry * BLOCK_SIZE + BLOCK_SIZE - 16 ) );
        }
      }

      return files;
    }



    public override void CreateEmptyMedia()
    {
      // TODO
    }



    public override ByteBuffer Compile()
    {
      return new ByteBuffer( _DiskImage );
    }



    public override MediaFilenameType FilenameType
    {
      get
      {
        return MediaFilenameType.AMIGA;
      }
    }



    public override bool ChangeDirectory( ByteBuffer DirName )
    {
      string  newDir = _CurrentDirectory + DirName.ToAsciiString() + "/";

      if ( LocateDirectoryBlock( newDir ) == -1 )
      {
        return false;
      }
      _CurrentDirectory = newDir;
      CurrentFolder = _CurrentDirectory;

      return true;
    }



    public override bool ChangeDirectoryUp()
    {
      if ( _CurrentDirectory == RootFolder )
      {
        return false;
      }
      int   endPos = _CurrentDirectory.LastIndexOf( '/', _CurrentDirectory.Length - 2 );
      if ( endPos != -1 )
      {
        string  newDir = _CurrentDirectory.Substring( 0, endPos + 1 );

        if ( LocateDirectoryBlock( newDir ) == -1 )
        {
          return false;
        }
        _CurrentDirectory = newDir;
        CurrentFolder = _CurrentDirectory;
        return true;
      }
      return false;
    }




  }
}
