using GR.Memory;
using RetroDevStudio;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace RetroDevStudio.Formats
{
  public class CPCDSK : MediaFormat
  {
    public enum SaveMode
    {
      NORMAL      = 0,
      PROTECTED   = 1,
      BINARY      = 2
    }



    protected bool          _Extended = true;
    protected string        _LastError = "";

    protected DiskInformationBlock          _DiskInfoBlock = new DiskInformationBlock();
    protected List<TrackInformationBlock>[]   _Tracks = new List<TrackInformationBlock>[2];
    protected int           _CurrentSide = 0;
    protected byte          _FirstSectorID = 0xc1;
    

    protected class DiskInformationBlock
    {
      public string         Description = "EXTENDED CPC DSK File\r\nDisk-Info\r\n";
      public string         Creator = "RetroDevStudio";
      public byte           NumberOfTracks = 40;
      public byte           NumberOfSides = 1;
      public ushort         Unused = 0;
      public ByteBuffer     TrackSizeTable = new ByteBuffer( 40 * 1 );
    }



    protected class TrackInformationBlock
    {
      public string         Description     = "Track-Info\r\n";
      public ushort         Unused          = 0;
      public byte           TrackNumber     = 40;
      public byte           SideNumber      = 1;
      public ushort         Unused2         = 0;
      public byte           SectorSize      = 0;
      public byte           NumberOfSectors = 0;
      public byte           GAP3Length      = 0;
      public byte           Filler          = 0xe5;
      public List<Sector>   Sectors         = new List<Sector>();
    }



    protected class Sector
    {
      public byte         Track               = 0;
      public byte         SectorIndex         = 0;
      public byte         Side                = 0;
      public byte         SectorID            = 0;
      public byte         SectorSize          = 0;
      public byte         FDCStatusRegister1  = 0;
      public byte         FDCStatusRegister2  = 0;
      public ushort       ActualDataLength    = 0;
      public ByteBuffer   Data                = new ByteBuffer( 512, 0xe5 );
    }



    public CPCDSK()
    {
      _Tracks[0] = new List<TrackInformationBlock>();
      _Tracks[1] = new List<TrackInformationBlock>();
    }



    public override void CreateEmptyMedia()
    {
      _LastError = "";

      _Tracks[0].Clear();
      _Tracks[1].Clear();

      _Extended       = true;
      _DiskInfoBlock  = new DiskInformationBlock();
      _FirstSectorID  = 0xc1;

      for ( int i = 0; i < 40; ++i )
      {
        _Tracks[0].Add( new TrackInformationBlock()
        {
          TrackNumber     = (byte)i,
          SideNumber      = 0,
          SectorSize      = 2,
          NumberOfSectors = 9,
          GAP3Length      = 0x52,
          Filler          = 0xe5
        } );
        for ( int j = 0; j < 9; ++j )
        {
          _Tracks[0][i].Sectors.Add( new Sector()
            {
              Track               = (byte)i,
              Side                = 0,
              SectorIndex         = (byte)j,
              SectorID            = (byte)( _FirstSectorID + ( ( j % 2 ) * 5 ) + j / 2 ),
              SectorSize          = 2,
              FDCStatusRegister1  = 0,
              FDCStatusRegister2  = 0,
              ActualDataLength    = 0x200
            } );
        }
      }
    }



    public override bool Load( string Filename )
    {
      _LastError = "";
      _Tracks[0].Clear();
      _Tracks[1].Clear();

      GR.Memory.ByteBuffer diskData = GR.IO.File.ReadAllBytes( Filename );
      if ( diskData == null )
      {
        _LastError = "Could not open/read file";
        return false;
      }
      if ( diskData.Length < 256 )
      {
        _LastError = "file is too small to hold disk information block";
        return false;
      }

      _DiskInfoBlock.Description    = diskData.ToAsciiString( 0, 34 );
      _DiskInfoBlock.Creator        = diskData.ToAsciiString( 34, 14 );
      _DiskInfoBlock.NumberOfTracks = diskData.ByteAt( 48 );
      _DiskInfoBlock.NumberOfSides  = diskData.ByteAt( 49 );
      _DiskInfoBlock.TrackSizeTable = new ByteBuffer( (uint)( _DiskInfoBlock.NumberOfTracks * _DiskInfoBlock.NumberOfSides ) );

      _Extended = _DiskInfoBlock.Description.StartsWith( "EXTENDED" );

      // 50 to 51 hold a single track size if not extended - TODO - verify

      int curPos = 256;

      for ( int track = 0; track < _DiskInfoBlock.NumberOfTracks; ++track )
      {
        for ( int side = 0; side < _DiskInfoBlock.NumberOfSides; ++side )
        {
          var trackInfo = new TrackInformationBlock();

          trackInfo.Description = diskData.ToAsciiString( (uint)curPos, 13 );
          trackInfo.TrackNumber = diskData.ByteAt( curPos + 16 );
          trackInfo.SideNumber = diskData.ByteAt( curPos + 17 );
          trackInfo.SectorSize = diskData.ByteAt( curPos + 20 );
          trackInfo.NumberOfSectors = diskData.ByteAt( curPos + 21 );
          trackInfo.GAP3Length = diskData.ByteAt( curPos + 22 );

          for ( int sector = 0; sector < trackInfo.NumberOfSectors; ++sector )
          {
            var sectorInfo = new Sector()
            {
              Track               = diskData.ByteAt( curPos + 24 + sector * 8 ),
              Side                = diskData.ByteAt( curPos + 24 + sector * 8 + 1 ),
              SectorID            = diskData.ByteAt( curPos + 24 + sector * 8 + 2 ),
              SectorSize          = diskData.ByteAt( curPos + 24 + sector * 8 + 3 ),
              FDCStatusRegister1  = diskData.ByteAt( curPos + 24 + sector * 8 + 4 ),
              FDCStatusRegister2  = diskData.ByteAt( curPos + 24 + sector * 8 + 5 ),
              ActualDataLength    = diskData.UInt16At( curPos + 24 + sector * 8 + 6 ),
              SectorIndex         = (byte)sector
            };

            trackInfo.Sectors.Add( sectorInfo );
          }

          // sector data always follows with 256 bytes offset from track info block
          curPos += 256;

          for ( int sector = 0; sector < trackInfo.NumberOfSectors; ++sector )
          {
            diskData.CopyTo( trackInfo.Sectors[sector].Data, curPos, trackInfo.Sectors[sector].ActualDataLength );
            curPos += trackInfo.Sectors[sector].ActualDataLength;
          }

          _Tracks[side].Add( trackInfo );
        }
      }

      return true;
    }



    public override bool Save( string Filename )
    {
      GR.Memory.ByteBuffer data = Compile();
      return GR.IO.File.WriteAllBytes( Filename, data );
    }



    /*
    bool LocateFile( GR.Memory.ByteBuffer Filename, out Location FileLocation, out Types.FileInfo FileInfo )
    {
      _LastError = "";
      FileLocation = null;
      FileInfo = null;

      Location    curLoc = new Location( TRACK_DIRECTORY, SECTOR_DIRECTORY );

      bool endFound = false;

      while ( !endFound )
      {
        Sector sec = Tracks[curLoc.Track - 1].Sectors[curLoc.Sector];

        for ( int i = 0; i < 8; ++i )
        {
          int fileTrack = sec.Data.ByteAt( BYTES_PER_DIR_ENTRY * i + 3 );
          int fileSector = sec.Data.ByteAt( BYTES_PER_DIR_ENTRY * i + 4 );
          if ( fileTrack != 0 )
          {
            // valid entry?
            if ( sec.Data.ByteAt( BYTES_PER_DIR_ENTRY * i + 2 ) != (byte)RetroDevStudio.Types.FileType.SCRATCHED )
            {
              GR.Memory.ByteBuffer filename = sec.Data.SubBuffer( BYTES_PER_DIR_ENTRY * i + 5, 16 );
              if ( Filename.Compare( filename ) == 0 )
              {
                FileLocation = new Location( fileTrack, fileSector );

                FileInfo              = new RetroDevStudio.Types.FileInfo();
                FileInfo.Filename     = new GR.Memory.ByteBuffer( filename );
                FileInfo.StartSector  = fileSector;
                FileInfo.StartTrack   = fileTrack;
                FileInfo.Type         = (RetroDevStudio.Types.FileType)sec.Data.ByteAt( BYTES_PER_DIR_ENTRY * i + 2 );
                FileInfo.Blocks       = 0;
                return true;
              }
            }
          }
        }

        curLoc = sec.NextLocation;
        if ( curLoc == null )
        {
          // track = 0 marks last directory entry
          break;
        }
      }
      _LastError = "Could not locate directory entry for file";
      return false;
    }
    */



    public override Types.FileInfo LoadFile( GR.Memory.ByteBuffer Filename )
    {
      ByteBuffer dirEntry = LocateDirEntry( _CurrentSide, Filename.SubBuffer( 0, 8 ), Filename.SubBuffer( 9, 3 ), 0 );
      if ( dirEntry == null )
      {
        return null;
      }

      var file = new FileInfo();
      int startSectorID = _Tracks[0][0].Sectors[0].SectorID;

      file.Filename   = Filename;

      bool  complete = false;
      int   fileEntryIndex = 0;

      while ( !complete )
      {
        for ( int j = 0; j < 16; ++j )
        {
          byte blockNo = dirEntry.ByteAt( 0x10 + j );
          if ( blockNo == 0 )
          {
            complete = true;
            break;
          }
          // 9 = number of sectors in track
          int blockTrack = ( blockNo * 2 ) / 9;
          int blockSector = ( blockNo * 2 ) % 9;

          if ( file.StartTrack == -1 )
          {
            file.StartTrack = blockTrack;
            file.StartSector = blockSector;
          }

          //Debug.Log( $"{blockTrack} A " + file.Data.Length.ToString( "X4" ) + ":" + ( startSectorID + blockSector ).ToString( "X2" ) );
          file.Data += GetSectorByID( _CurrentSide, blockTrack, startSectorID + blockSector ).Data;
          if ( blockSector + 1 >= 9 )
          {
            ++blockTrack;
          }
          //Debug.Log( $"{blockTrack} B " + file.Data.Length.ToString( "X4" ) + ":" + ( startSectorID + ( blockSector + 1 ) % 9 ).ToString( "X2" ) );
          file.Data += GetSectorByID( _CurrentSide, blockTrack, startSectorID + ( blockSector + 1 ) % 9 ).Data;
        }
        if ( !complete )
        {
          // find next block
          dirEntry = LocateDirEntry( _CurrentSide, file.Filename, dirEntry.SubBuffer( 9, 3 ), fileEntryIndex + 1 );
          if ( dirEntry == null )
          {
            // broken image?
            break;
          }
          fileEntryIndex = dirEntry.ByteAt( 0x0c );
        }
      }
      // check for file header
      if ( file.Data.Length >= 0x45 )
      {
        ushort  checkSum = 0;
        for ( int x = 0; x < 0x43; ++x )
        {
          checkSum += file.Data.ByteAt( x );
        }
        if ( file.Data.UInt16At( 0x43 ) == checkSum )
        {
          // a program header
          ushort programLength = file.Data.UInt16At( 0x18 );
          file.Size = programLength;

          // 64 to cut off program header, but WTF size + 64??
          file.Data = file.Data.SubBuffer( 128, file.Size );
        }
      }

      _LastError = "";
      return file;
    }



    public override bool DeleteFile( GR.Memory.ByteBuffer Filename )
    {
      _LastError = "";
      /*
      int   curTrack = TRACK_DIRECTORY;
      int   curSector = SECTOR_DIRECTORY;
      while ( true )
      {
        Track dirTrack = Tracks[curTrack - 1];

        Sector sect = dirTrack.Sectors[curSector];
        for ( int i = 0; i < 8; ++i )
        {
          if ( sect.Data.ByteAt( BYTES_PER_DIR_ENTRY * i + 2 ) != 0 )
          {
            // non empty
            bool  filenameMatches = true;
            for ( int j = 0; j < 16; ++j )
            {
              if ( Filename.ByteAt( j ) != sect.Data.ByteAt( BYTES_PER_DIR_ENTRY * i + 5 + j ) )
              {
                filenameMatches = false;
                break;
              }
            }

            if ( filenameMatches )
            {
              // scratch file
              sect.Data.SetU8At( BYTES_PER_DIR_ENTRY * i + 2, 0 );

              int startTrack  = sect.Data.ByteAt( BYTES_PER_DIR_ENTRY * i + 3 );
              int startSector = sect.Data.ByteAt( BYTES_PER_DIR_ENTRY * i + 4 );

              while ( startTrack != 0 )
              {
                Track fileTrack = Tracks[startTrack - 1];
                Sector fileSector = fileTrack.Sectors[startSector];

                FreeSector( startTrack, startSector );

                startTrack = fileSector.Data.ByteAt( 0 );
                startSector = fileSector.Data.ByteAt( 1 );
              }
              return true;
            }
          }
        }
        // find next dir sector
        if ( sect.Data.ByteAt( 0 ) == 0 )
        {
          // was last dir sector
          _LastError = "file not found";
          return false;
        }
        curTrack  = sect.Data.ByteAt( 0 );
        curSector = sect.Data.ByteAt( 1 );
      }*/
      return false;
    }



    public override bool RenameFile( GR.Memory.ByteBuffer Filename, GR.Memory.ByteBuffer NewFilename )
    {
      _LastError = "";
      /*
      int curTrack = TRACK_DIRECTORY;
      int curSector = SECTOR_DIRECTORY;
      while ( true )
      {
        Track dirTrack = Tracks[curTrack - 1];

        Sector sect = dirTrack.Sectors[curSector];
        for ( int i = 0; i < 8; ++i )
        {
          if ( sect.Data.ByteAt( BYTES_PER_DIR_ENTRY * i + 2 ) != 0 )
          {
            // non empty
            bool filenameMatches = true;
            for ( int j = 0; j < 16; ++j )
            {
              if ( Filename.ByteAt( j ) != sect.Data.ByteAt( BYTES_PER_DIR_ENTRY * i + 5 + j ) )
              {
                filenameMatches = false;
                break;
              }
            }

            if ( filenameMatches )
            {
              for ( int j = 0; j < 16; ++j )
              {
                sect.Data.SetU8At( BYTES_PER_DIR_ENTRY * i + 5 + j, NewFilename.ByteAt( j ) );
              }
              return true;
            }
          }
        }
        // find next dir sector
        if ( sect.Data.ByteAt( 0 ) == 0 )
        {
          // was last dir sector
          _LastError = "file not found";
          return false;
        }
        curTrack = sect.Data.ByteAt( 0 );
        curSector = sect.Data.ByteAt( 1 );
      }*/
      return false;
    }



    public override bool WriteFile( GR.Memory.ByteBuffer Filename, GR.Memory.ByteBuffer Content, Types.FileType Type )
    {
      _LastError = "";
      if ( LoadFile( Filename ) != null )
      {
        _LastError = "Filename already exists";
        return false;
      }

      bool  hasHeader = true;

      ushort fullLength = (ushort)Content.Length;
      if ( hasHeader )
      {
        fullLength = (ushort)( Content.Length + 256 );
      }

      if ( fullLength > FreeMemory )
      {
        _LastError = "Not enough free space on disk";
        return false;
      }

      ByteBuffer  fullContent;
      if ( hasHeader )
      {
        fullContent = CreateFileHeader( 0, Filename, SaveMode.NORMAL, 0x0170, (ushort)Content.Length ) + Content;
      }
      else
      {
        fullContent = Content;
      }

      int   writtenSize = 0;
      int   startTrack = FirstDirectoryTrack();
      int   startDataTrack = startTrack;
      int   curDirTrack = startTrack;
      int   curDirSector = _FirstSectorID;
      bool  readOnly = false;
      bool  hidden = false;
      while ( writtenSize < fullContent.Length )
      {
        int nextBatchSize = (int)Math.Min( fullContent.Length - writtenSize, 512 );

        var currentSector = GetSectorByID( _CurrentSide, curDirTrack, curDirSector );
        if ( currentSector == null )
        {
          break;
        }
        int   dirEntryPos = 0;
        for ( int i = 0; i < currentSector.Data.Length / 32; ++i )
        {
          if ( currentSector.Data.ByteAt( dirEntryPos ) == 0xe5 )
          {
            // a free slot
            // user type
            currentSector.Data.SetU8At( dirEntryPos + 0, 0 );
            Filename.CopyTo( currentSector.Data, 0, 8, dirEntryPos + 1 );
            // type
            Filename.CopyTo( currentSector.Data, 9, 3, dirEntryPos + 9 );
            // flags
            if ( readOnly )
            {
              currentSector.Data.SetU8At( dirEntryPos + 9, (byte)( currentSector.Data.ByteAt( dirEntryPos + 9 ) | 0x80 ) );
            }
            if ( hidden )
            {
              currentSector.Data.SetU8At( dirEntryPos + 10, (byte)( currentSector.Data.ByteAt( dirEntryPos + 10 ) | 0x80 ) );
            }
            currentSector.Data.SetU8At( dirEntryPos + 12, 0 );
            currentSector.Data.SetU8At( dirEntryPos + 13, 0 );
            currentSector.Data.SetU8At( dirEntryPos + 14, 0 );

            // length in records (record is 128 bytes)
            currentSector.Data.SetU8At( dirEntryPos + 15, (byte)( ( nextBatchSize + 127 ) / 128 ) );
            currentSector.Data.Fill( dirEntryPos + 16, 16, 0 );

            for ( int blockIndex = 0; blockIndex < 16; ++blockIndex )
            {
              var dataSector = FindFirstFreeDataSector();
              if ( dataSector == null )
              {
                // TODO - the image now has partially written content!
                _LastError = "No more free data sector(s) found";
                return false;
              }

              // mark block as used...
              int blockValue = ( ( dataSector.Track - startDataTrack ) * _Tracks[_CurrentSide][0].NumberOfSectors + dataSector.SectorID - _FirstSectorID ) / 2;
              currentSector.Data.SetU8At( dirEntryPos + 16 + blockIndex, (byte)blockValue );

              fullContent.CopyTo( dataSector.Data, writtenSize, nextBatchSize, 0 );
              // extranous data is nulled (at least in WinAPE)
              dataSector.Data.Fill( nextBatchSize, dataSector.SectorSize - nextBatchSize, 0 );
              writtenSize += nextBatchSize;

              // next batch
              nextBatchSize = (int)Math.Min( fullContent.Length - writtenSize, 512 );
              if ( nextBatchSize == 0 )
              {
                return true;
              }
              // ...and write second sector to fill up block
              int nextSector = dataSector.SectorIndex + 1;
              int nextTrack = dataSector.Track;
              if ( nextSector - _FirstSectorID >= _Tracks[_CurrentSide][dataSector.Track].NumberOfSectors )
              {
                nextSector = _FirstSectorID;
                ++nextTrack;
                if ( nextTrack >= _DiskInfoBlock.NumberOfTracks )
                {
                  // TODO - the image now has partially written content!
                  _LastError = "Disk is full!";
                  return false;
                }
              }
              dataSector = GetSectorByID( _CurrentSide, nextTrack, nextSector );
              if ( dataSector == null )
              {
                // TODO - the image now has partially written content!
                _LastError = "No more free data sector(s) found";
                return false;
              }
              fullContent.CopyTo( dataSector.Data, writtenSize, nextBatchSize, 0 );
              // extranous data is nulled (at least in WinAPE)
              dataSector.Data.Fill( nextBatchSize, dataSector.SectorSize - nextBatchSize, 0 );
              writtenSize += nextBatchSize;
              nextBatchSize = (int)Math.Max( fullContent.Length - writtenSize, 512 );
              if ( nextBatchSize == 0 )
              {
                return true;
              }
            }
          }
        }
      }


      /* 
        Byte &000 USER-Nummer (&OO-&OF) 
        Byte &001-&008  Programmname (ggf. mit Leerzeichen (&20) gefüllt) 
        Byte &009-&00B 
        Byte &013 Typenkennzeichnung 
                  Kennzeichnung, wie das Programm gesichert wurde: 
                  = &00 : Normal geSA VEt 
                  = &01 : Geschützt geSA VEt (Protected) 
                  = &02 : Binär geSA VEt (,B) 
        Byte &015-&016 Gibt die Adresse an (Low-und Highbyte), wohin das 
        Programm beim Laden gelegt wird (in dem Beispiel ist 
        es der BASIC-Anfang &0170) 
        Byte &018-&019  Programmlänge (Low-und Highbyte) 
        Byte &040-&041  Programmlänge (Low-und Highbyte) 
        Byte &043-&044  Summe der Bytes &000-&042 (Prüfsumme)!!
      */

      /*
      GR.Memory.ByteBuffer    dataToWrite = new GR.Memory.ByteBuffer( Content );
      if ( dataToWrite.Length > FreeBytes() )
      {
        _LastError = "file too large";
        return false;
      }

      Sector bam = Tracks[TRACK_BAM - 1].Sectors[SECTOR_BAM];

      int trackIndex = 1;
      int prevSector = 0;
      int fileInterleave = 10;
      int bytesToWrite = (int)dataToWrite.Length;
      int writeOffset = 0;
      Sector previousSector = null;
      int searchSector = -1; 

      int startSector = -1;
      int startTrack = -1;
      int sectorsWritten = 0;

      write_next_sector:;
      trackIndex = 1;
      foreach ( Track track in Tracks )
      {
        if ( trackIndex == TRACK_DIRECTORY )
        {
          // directory track
          ++trackIndex;
          continue;
        }
        if ( track.FreeSectors == 0 )
        {
          ++trackIndex;
          continue;
        }

        int     sectorsPerTrack = track.Sectors.Count;
        searchSector = ( prevSector + fileInterleave ) % sectorsPerTrack;

        while ( true )
        {
          if ( track.Sectors[searchSector].Free )
          {
            AllocSector( trackIndex, searchSector );
            if ( previousSector != null )
            {
              previousSector.Data.SetU8At( 0, (byte)trackIndex );
              previousSector.Data.SetU8At( 1, (byte)searchSector );
            }
            else
            {
              // first sector, add directory entry
              startSector = searchSector;
              startTrack = trackIndex;
            }
            previousSector = track.Sectors[searchSector];
            if ( bytesToWrite > 254 )
            {
              dataToWrite.CopyTo( previousSector.Data, writeOffset, 254, 2 );
              previousSector.Free = false;
              writeOffset += 254;
              bytesToWrite -= 254;
              ++sectorsWritten;
              prevSector = searchSector;
              goto write_next_sector;
            }
            // last sector
            previousSector.Free = false;
            previousSector.Data.SetU8At( 0, 0 );
            previousSector.Data.SetU8At( 1, (byte)( 1 + bytesToWrite ) );
            dataToWrite.CopyTo( previousSector.Data, writeOffset, bytesToWrite, 2 );
            writeOffset += bytesToWrite;
            bytesToWrite = 0;
            ++sectorsWritten;

            AddDirectoryEntry( Filename, startTrack, startSector, sectorsWritten, Type );
            return true;
          }
          else
          {
            ++searchSector;
            if ( searchSector >= sectorsPerTrack )
            {
              searchSector = 0;
            }
          }
        }
      }*/
      _LastError = "disk is full";
      return false;
    }



    private Sector FindFirstFreeDataSector()
    {
      int   curTrack  = FirstDirectoryTrack();
      int   curSector = FirstDataSector();
      while ( true )
      {
        if ( !IsSectorInUse( curTrack, curSector ) )
        {
          return GetSectorByID( _CurrentSide, curTrack, curSector );
        }
        ++curSector;
        if ( curSector - _FirstSectorID >= _Tracks[_CurrentSide][curTrack].NumberOfSectors )
        {
          curSector = _FirstSectorID;
          ++curTrack;
          if ( curTrack >= _Tracks[_CurrentSide].Count )
          {
            return null;
          }
        }
      }
    }



    private bool IsSectorInUse( int Track, int SectorID )
    {
      int curTrack = FirstDirectoryTrack();
      for ( int i = 0; i < 4; ++i )
      {
        var currentSector = GetSectorByID( _CurrentSide, curTrack, _FirstSectorID + i );
        int   dirEntryPos = 0;
        for ( int j = 0; j < currentSector.Data.Length / 32; ++j )
        {
          if ( currentSector.Data.ByteAt( dirEntryPos + 0x00 ) != 0xe5 )
          {
            for ( int k = 0; k < 16; ++k )
            {
              byte blockNo = currentSector.Data.ByteAt( dirEntryPos + 16 + k );
              int blockTrack = ( blockNo * 2 ) / 9;
              int blockSector = ( blockNo * 2 ) % 9;

              if ( ( blockTrack == Track )
              &&   ( blockSector == SectorID - _FirstSectorID ) )
              {
                return true;
              }
            }
          }
          dirEntryPos += 32;
        }
      }
      return false;
    }



    private ByteBuffer CreateFileHeader( byte UserByte, ByteBuffer Filename, SaveMode Mode, ushort StartAddress, ushort Length )
    {
      var header = new ByteBuffer( 0x80 );

      header.SetU8At( 0, UserByte );
      // Filename
      Filename.CopyTo( header, 0, 8, 1 );
      // type (extension plus extra bytes)
      Filename.CopyTo( header, 9, 3, 9 );
      header.SetU8At( 0x13, (byte)Mode );
      header.SetU16At( 0x15, StartAddress );
      header.SetU16At( 0x18, Length );
      header.SetU16At( 0x40, Length );

      ushort  checkSum = 0;
      for ( int i = 0; i < 0x42; ++i )
      {
        checkSum += header.ByteAt( i );
      }
      header.SetU16At( 0x43, checkSum );

      return header;
    }



    public override GR.Memory.ByteBuffer Compile()
    {
      _LastError = "";
      // should be 194816
      GR.Memory.ByteBuffer result = new GR.Memory.ByteBuffer( (uint)( 256 + 256 * _DiskInfoBlock.NumberOfTracks * _DiskInfoBlock.NumberOfSides + _DiskInfoBlock.NumberOfTracks * _DiskInfoBlock.NumberOfSides * _Tracks[0][0].NumberOfSectors * 512 ) );

      // disk info block
      ByteBuffer  discInfoDesc;
      if ( _Extended )
      {
        discInfoDesc = new ByteBuffer( "455854454E444544204350432044534B2046696c650d0a4469736b2d496e666f0d0a" );
      }
      else
      {
        discInfoDesc = new ByteBuffer( "4D56202D20435043454D55204469736B2D46696C650d0a4469736b2d496e666f0d0a" );
      }
      discInfoDesc.CopyTo( result, 0, (int)discInfoDesc.Length, 0 );

      var creator = new ByteBuffer( "526574726F44657653747564696F" );
      creator.CopyTo( result, 0, (int)creator.Length, 0x22 );
      result.SetU8At( 0x30, _DiskInfoBlock.NumberOfTracks );
      result.SetU8At( 0x31, _DiskInfoBlock.NumberOfSides );

      int trackSize = 256 + 512 * _Tracks[0][0].NumberOfSectors;
      if ( _Extended )
      {
        // track size table (it's the high byte of the actual size)
        for ( int i = 0; i < _DiskInfoBlock.NumberOfTracks; ++i )
        {
          for ( int j = 0; j < _DiskInfoBlock.NumberOfSides; ++j )
          {
            result.SetU8At( 0x34 + i * _DiskInfoBlock.NumberOfSides + j, (byte)( trackSize / 256 ) );
          }
        }
      }
      else
      {
        result.SetU16NetworkOrderAt( 0x32, (ushort)trackSize );
      }

      var trackInfo = new ByteBuffer( "547261636b2d496e666f0d0a" );
      for ( int track = 0; track < _DiskInfoBlock.NumberOfTracks; ++track )
      {
        for ( int side = 0; side < _DiskInfoBlock.NumberOfSides; ++side )
        {
          // track info block
          var currentTrack = _Tracks[side][track];

          int   trackStartPos = 0x100 + ( track * _DiskInfoBlock.NumberOfSides + side ) * trackSize;
          trackInfo.CopyTo( result, 0, (int)trackInfo.Length, trackStartPos );
          result.SetU8At( trackStartPos + 0x10, (byte)track );
          result.SetU8At( trackStartPos + 0x11, (byte)side );
          // sector size (High byte of 0x0200)
          result.SetU8At( trackStartPos + 0x14, 2 );
          result.SetU8At( trackStartPos + 0x15, (byte)currentTrack.NumberOfSectors );
          result.SetU8At( trackStartPos + 0x16, currentTrack.GAP3Length );
          result.SetU8At( trackStartPos + 0x17, currentTrack.Filler );

          // write sector info lists
          for ( int sector = 0; sector < currentTrack.NumberOfSectors; ++sector )
          {
            int sectorInfoOffset = trackStartPos + 0x18 + sector * 8;
            var curSector = currentTrack.Sectors[sector];

            result.SetU8At( sectorInfoOffset + 0, (byte)track );
            result.SetU8At( sectorInfoOffset + 1, (byte)side );
            result.SetU8At( sectorInfoOffset + 2, (byte)curSector.SectorID );
            result.SetU8At( sectorInfoOffset + 3, (byte)curSector.SectorSize );
            result.SetU8At( sectorInfoOffset + 4, (byte)curSector.FDCStatusRegister1 );
            result.SetU8At( sectorInfoOffset + 5, (byte)curSector.FDCStatusRegister2 );
            result.SetU16At( sectorInfoOffset + 6, curSector.ActualDataLength );
          }

          // write sector data
          for ( int sector = 0; sector < currentTrack.NumberOfSectors; ++sector )
          {
            int sectorDataOffset = trackStartPos + 0x100 + sector * currentTrack.SectorSize * 256;

            currentTrack.Sectors[sector].Data.CopyTo( result, 0, (int)currentTrack.Sectors[sector].Data.Length, sectorDataOffset );
          }
        }
      }

      return result;
    }



    public override string FileFilter
    {
      get
      {
        return "Disk Files|*.DSK|" + base.FileFilter;
      }
    }



    public override GR.Memory.ByteBuffer Title
    {
      get
      {
        GR.Memory.ByteBuffer    title = new GR.Memory.ByteBuffer();

        /*
        title.Append( Util.ToPETSCII( "0 \"" ) );
        title.Append( DiskName );
        title.Append( Util.ToPETSCII( "\" " ) );
        title.AppendU16NetworkOrder( DiskID );*/
        return title;
      }
    }



    public override string LastError
    {
      get 
      {
        return _LastError;
      }
    }



    private int FirstDirectoryTrack()
    {
      if ( _Tracks[_CurrentSide][0].Sectors[0].SectorID == 0x41 )
      {
        // CPM system disk
        return 2;
      }
      else if ( _Tracks[_CurrentSide][0].Sectors[0].SectorID == 0xC1 )
      {
        // CPM data disk
        return 0;
      }
      _LastError = "Unrecognized CPC disk type";
      return 0;
    }



    private int FirstDirectorySector()
    {
      return _FirstSectorID;
    }



    private int FirstDataSector()
    {
      return _FirstSectorID + 4;
    }



    public override int FreeSlots
    {
      get
      {
        int freeSlots = 0;

        int curTrack = FirstDirectoryTrack();
        int curSector = 0;
        int startTrack = curTrack;

        while ( true )
        {
          var currentSector = GetSector( _CurrentSide, curTrack, curSector );
          if ( currentSector == null )
          {
            break;
          }
          int   dirEntryPos = 0;
          for ( int i = 0; i < currentSector.Data.Length / 32; ++i )
          {
            if ( currentSector.Data.ByteAt( dirEntryPos + 0x00 ) == 0x35 )
            {
              ++freeSlots;
            }
            dirEntryPos += 32;
          }

          ++curSector;
          if ( curSector >= _Tracks[_CurrentSide][curTrack].Sectors.Count )
          {
            ++curTrack;
            curSector = 0;
            if ( ( curTrack >= startTrack + 4 )
            || ( curTrack >= _Tracks[_CurrentSide].Count ) )
            {
              break;
            }
          }
        }
        return freeSlots;
      }
    }



    public uint FreeMemory
    {
      get
      {
        uint  freeMemory = 0;

        int curTrack = FirstDirectoryTrack();
        int curSector = 0;
        int startTrack = curTrack;

        while ( true )
        {
          var currentSector = GetSector( _CurrentSide, curTrack, curSector );
          if ( currentSector == null )
          {
            break;
          }
          int   dirEntryPos = 0;
          for ( int i = 0; i < currentSector.Data.Length / 32; ++i )
          {
            if ( currentSector.Data.ByteAt( dirEntryPos + 0x00 ) == 0xe5 )
            {
              freeMemory += 512;
            }
            dirEntryPos += 32;
          }

          ++curSector;
          if ( curSector >= _Tracks[_CurrentSide][curTrack].Sectors.Count )
          {
            ++curTrack;
            curSector = 0;
            if ( ( curTrack >= startTrack + 4 )
            ||   ( curTrack >= _Tracks[_CurrentSide].Count ) )
            {
              break;
            }
          }
        }
        return freeMemory;
      }
    }



    public override void Validate()
    {
      /*
      var files = Files();

      GR.Collections.Set<GR.Generic.Tupel<int,int>>    usedTracksAndSectors = new GR.Collections.Set<GR.Generic.Tupel<int, int>>();

      usedTracksAndSectors.Add( new GR.Generic.Tupel<int, int>( TRACK_HEADER, SECTOR_HEADER ) );
      usedTracksAndSectors.Add( new GR.Generic.Tupel<int, int>( TRACK_BAM, SECTOR_BAM ) );

      int   curTrack = TRACK_DIRECTORY;
      int   curSector = SECTOR_DIRECTORY;
      usedTracksAndSectors.Add( new GR.Generic.Tupel<int, int>( curTrack, curSector ) );
      while ( true )
      {
        Sector sec = Tracks[curTrack - 1].Sectors[curSector];

        curTrack = sec.Data.ByteAt( 0 );
        curSector = sec.Data.ByteAt( 1 );

        if ( curTrack == 0 )
        {
          // track = 0 marks last directory entry
          break;
        }
        usedTracksAndSectors.Add( new GR.Generic.Tupel<int, int>( curTrack, curSector ) );
      }

      foreach ( var file in files )
      {
        curTrack = file.StartTrack;
        curSector = file.StartSector;

        while ( true )
        {
          Sector sec = Tracks[curTrack - 1].Sectors[curSector];

          usedTracksAndSectors.Add( new GR.Generic.Tupel<int, int>( curTrack, curSector ) );

          curTrack = sec.Data.ByteAt( 0 );
          curSector = sec.Data.ByteAt( 1 );

          if ( curTrack == 0 )
          {
            // track = 0 marks last directory entry
            break;
          }
        }
      }
      foreach ( var track in Tracks )
      {
        foreach ( var sector in track.Sectors )
        {
          if ( ( !sector.Free )
          &&   ( !usedTracksAndSectors.ContainsValue( new GR.Generic.Tupel<int, int>( track.TrackNo, sector.SectorNo ) ) ) )
          {
            sector.Free = true;
          }
        }
      }*/
    }



    public override void Clear()
    {
      throw new NotImplementedException();
    }



    public override List<FileInfo> Files()
    {
      int curTrack = FirstDirectoryTrack();
      int curSector = 0;
      int curSide = 0;
      var files = new List<FileInfo>();
      int startTrack    = curTrack;
      var currentSector = GetSector( curSide, curTrack, curSector );
      int sectorID      = _Tracks[curSide][curTrack].Sectors[curSector].SectorID;
      int startSectorID = sectorID;
      int sectorCount   = 0;

      while ( ( currentSector != null )
      &&      ( sectorCount < 4 ) )
      {
        int   dirEntryPos = 0;
        for ( int i = 0; i < currentSector.Data.Length / 32; ++i )
        {
          ByteBuffer  dirEntry = currentSector.Data.SubBuffer( dirEntryPos, 32 );
          ByteBuffer  typeID = dirEntry.SubBuffer( 9, 3 );
          bool        extended = false;
          byte        fileEntryIndex = dirEntry.ByteAt( 0x0c );

          if ( fileEntryIndex > 0 )
          {
            // skip extensions
            dirEntryPos += 32;
            continue;
          }

          if ( dirEntry.ByteAt( 0x0f ) == 0x80 )
          {
            // a follow up entry block
            extended = true;
          }
          var file = new FileInfo()
          {
            Filename    = dirEntry.SubBuffer( 1, 8 ),
            Type        = FileType.PRG
          };

          byte userLevel = dirEntry.ByteAt( dirEntryPos );
          if ( ( typeID.ByteAt( 0 ) & 0x80 ) > 0 )
          {
            file.ReadOnly = true;
            typeID.SetU8At( 0, (byte)( typeID.ByteAt( 0 ) & 0x7f ) );
          }
          if ( ( typeID.ByteAt( 1 ) & 0x80 ) > 0 )
          {
            file.Hidden = true;
            typeID.SetU8At( 1, (byte)( typeID.ByteAt( 1 ) & 0x7f ) );
          }
          // deleted entry?
          if ( userLevel == 0xe5 )
          {
            file.Type = FileType.SCRATCHED;
          }
          // records
          file.Size = dirEntry.ByteAt( 0x0f ) * 128;

          file.Filename += new ByteBuffer( "2E" ) + typeID;

          while ( extended )
          {
            for ( int j = 0; j < 16; ++j )
            {
              byte blockNo = dirEntry.ByteAt( 0x10 + j );
              if ( blockNo == 0 )
              {
                extended = false;
                break;
              }
              // 9 = number of sectors in track
              int blockTrack = ( blockNo * 2 ) / 9;
              int blockSector = ( blockNo * 2 ) % 9;

              if ( file.StartTrack == -1 )
              {
                file.StartTrack   = blockTrack;
                file.StartSector  = blockSector;
              }

              //Debug.Log( $"{blockTrack} A " + file.Data.Length.ToString( "X4" ) + ":" + ( startSectorID + blockSector ).ToString( "X2" ) );
              file.Data += GetSectorByID( curSide, blockTrack, startSectorID + blockSector ).Data;
              if ( blockSector + 1 >= 9 )
              {
                ++blockTrack;
              }
              //Debug.Log( $"{blockTrack} B " + file.Data.Length.ToString( "X4" ) + ":" + ( startSectorID + ( blockSector + 1 ) % 9 ).ToString( "X2" ) );
              file.Data += GetSectorByID( curSide, blockTrack, startSectorID + ( blockSector + 1 ) % 9 ).Data;
            }
            if ( extended )
            {
              // find next block
              dirEntry = LocateDirEntry( curSide, file.Filename, dirEntry.SubBuffer( 9, 3 ), fileEntryIndex + 1 );
              if ( dirEntry == null )
              {
                // broken image?
                break;
              }
              fileEntryIndex = dirEntry.ByteAt( 0x0c );
            }
          }
          // check for file header
          if ( file.Data.Length >= 0x45 )
          {
            ushort  checkSum = 0;
            for ( int x = 0; x < 0x43; ++x )
            {
              checkSum += file.Data.ByteAt( x );
            }
            if ( file.Data.UInt16At( 0x43 ) == checkSum )
            {
              // a program header
              ushort programLength = file.Data.UInt16At( 0x18 );
              file.Size = programLength;
              file.Data = file.Data.SubBuffer( 128, file.Size );
            }
            else
            {
            }
          }
             
          files.Add( file );
          //Debug.Log( "File is " + file.Data.ToString() );

          dirEntryPos += 32;
        }

        currentSector = GetNextSector( curSide, ref curTrack, startSectorID, ref sectorID );
        ++sectorCount;
      }


      return files;
    }



    private Sector GetNextSector( int Side, ref int Track, int StartSectorID, ref int SectorID )
    {
      while ( Track < _Tracks[Side].Count )
      {
        for ( int i = 0; i < _Tracks[Side][Track].Sectors.Count; ++i )
        {
          if ( _Tracks[Side][Track].Sectors[i].SectorID == SectorID + 1 )
          {
            ++SectorID;
            return _Tracks[Side][Track].Sectors[i];
          }
        }
        ++Track;
        SectorID = StartSectorID;
      }
      return null;
    }



    private ByteBuffer LocateDirEntry( int Side, ByteBuffer Filename, ByteBuffer TypeID, int EntryIndex )
    {
      int curTrack = FirstDirectoryTrack();
      int curSector = 0;
      int startTrack = curTrack;

      while ( true )
      {
        var currentSector = GetSector( Side, curTrack, curSector );
        if ( currentSector == null )
        {
          break;
        }
        int   dirEntryPos = 0;
        for ( int i = 0; i < currentSector.Data.Length / 32; ++i )
        {
          if ( ( currentSector.Data.SubBuffer( dirEntryPos + 1, 8 ) == Filename.SubBuffer( 0, 8 ) )
          &&   ( ( currentSector.Data.ByteAt( dirEntryPos + 9 ) & 0x7f ) == ( TypeID.ByteAt( 0 ) & 0x7f ) )
          &&   ( ( currentSector.Data.ByteAt( dirEntryPos + 10 ) & 0x7f ) == ( TypeID.ByteAt( 1 ) & 0x7f ) )
          &&   ( ( currentSector.Data.ByteAt( dirEntryPos + 11 ) & 0x7f ) == ( TypeID.ByteAt( 2 ) & 0x7f ) )
          &&   ( currentSector.Data.ByteAt( dirEntryPos + 0x0c ) == EntryIndex ) )
          {
            return currentSector.Data.SubBuffer( dirEntryPos, 32 );
          }
          dirEntryPos += 32;
        }

        ++curSector;
        if ( curSector >= _Tracks[Side][curTrack].Sectors.Count )
        {
          ++curTrack;
          curSector = 0;
          if ( ( curTrack >= startTrack + 4 )
          ||   ( curTrack >= _Tracks[Side].Count ) )
          {
            return null;
          }
        }
      }
      return null;
    }



    private Sector GetSector( int Side, int Track, int Sector )
    {
      if ( ( Side < 0 )
      ||   ( Side > 1 )
      ||   ( Track < 0 )
      ||   ( Sector < 0 ) 
      ||   ( Track >= _Tracks[Side].Count )
      ||   ( Sector >= _Tracks[Side][Track].Sectors.Count ) )
      {
        return null;
      }

      return _Tracks[Side][Track].Sectors[Sector];      
    }



    private Sector GetSectorByID( int Side, int Track, int SectorID )
    {
      if ( ( Side < 0 )
      ||   ( Side > 1 )
      ||   ( Track < 0 )
      ||   ( SectorID < 0 ) 
      ||   ( Track >= _Tracks[Side].Count ) )
      {
        return null;
      }

      for ( int i = 0; i < _Tracks[Side][Track].Sectors.Count; ++i )
      {
        if ( _Tracks[Side][Track].Sectors[i].SectorID == SectorID )
        {
          return _Tracks[Side][Track].Sectors[i];
        }
      }
      return null;
    }



  }
}
