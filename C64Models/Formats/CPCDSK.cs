using GR.Memory;
using RetroDevStudio;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.Text;



namespace RetroDevStudio.Formats
{
  public class CPCDSK : MediaFormat
  {
    protected bool        _Extended = true;
    protected string      _LastError = "";

    protected DiskInformationBlock    _DiskInfoBlock = new DiskInformationBlock();



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
      public string             Description = "Track-Info\r\n";
      public ushort             Unused = 0;
      public byte               TrackNumber = 40;
      public byte               SideNumber = 1;
      public ushort             Unused2 = 0;
      public byte               SectorSize = 0;
      public byte               NumberOfSectors = 0;
      public byte               GAP3Length = 0;
      public byte               Filler = 0;
      public List<SectorInformationList>    SectorInformation = new List<SectorInformationList>();
      public List<ByteBuffer>   Sectors = new List<ByteBuffer>();
    }



    protected class SectorInformationList
    {
      public byte     Track = 0;
      public byte     Side = 0;
      public byte     SectorID = 0;
      public byte     SectorSize = 0;
      public byte     FDCStatusRegister1 = 0;
      public byte     FDCStatusRegister2 = 0;
      public ushort   ActualDataLength = 0;
    }



    public CPCDSK()
    {
    }



    public override void CreateEmptyMedia()
    {
      _LastError = "";
      /*
      Tracks.Clear();
      for ( int i = 0; i < 17; ++i )
      {
        Tracks.Add( new Track( i + 1, 21 ) );
      }
      for ( int i = 0; i < 7; ++i )
      {
        Tracks.Add( new Track( i + 18, 19 ) );
      }
      for ( int i = 0; i < 6; ++i )
      {
        Tracks.Add( new Track( i + 25, 18 ) );
      }
      for ( int i = 0; i < 5; ++i )
      {
        Tracks.Add( new Track( i + 30, 17 ) );
      }
      CreateBAM();*/
    }



    public void CreateEmptyMedia40Tracks()
    {
      _LastError = "";
      /*
      Tracks.Clear();
      for ( int i = 0; i < 17; ++i )
      {
        Tracks.Add( new Track( i + 1, 21 ) );
      }
      for ( int i = 0; i < 7; ++i )
      {
        Tracks.Add( new Track( i + 18, 19 ) );
      }
      for ( int i = 0; i < 6; ++i )
      {
        Tracks.Add( new Track( i + 25, 18 ) );
      }
      for ( int i = 0; i < 10; ++i )
      {
        Tracks.Add( new Track( i + 30, 17 ) );
      }*/
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

          trackInfo.Description     = diskData.ToAsciiString( (uint)curPos, 13 );
          trackInfo.TrackNumber     = diskData.ByteAt( curPos + 16 );
          trackInfo.SideNumber      = diskData.ByteAt( curPos + 17 );
          trackInfo.SectorSize      = diskData.ByteAt( curPos + 20 );
          trackInfo.NumberOfSectors = diskData.ByteAt( curPos + 21 );
          trackInfo.GAP3Length      = diskData.ByteAt( curPos + 22 );

          for ( int sector = 0; sector < trackInfo.NumberOfSectors; ++sector )
          {
            var sectorInfo = new SectorInformationList()
            {
              Track               = diskData.ByteAt( curPos + 24 + sector * 8 ),
              Side                = diskData.ByteAt( curPos + 24 + sector * 8 + 1 ),
              SectorID            = diskData.ByteAt( curPos + 24 + sector * 8 + 2 ),
              SectorSize          = diskData.ByteAt( curPos + 24 + sector * 8 + 3 ),
              FDCStatusRegister1  = diskData.ByteAt( curPos + 24 + sector * 8 + 4 ),
              FDCStatusRegister2  = diskData.ByteAt( curPos + 24 + sector * 8 + 5 ),
              ActualDataLength    = diskData.UInt16At( curPos + 24 + sector * 8 + 6 )
            };

            trackInfo.SectorInformation.Add( sectorInfo );
          }

          // sector data always follows with 256 bytes offset from track info block
          curPos += 256;

          for ( int sector = 0; sector < trackInfo.NumberOfSectors; ++sector )
          {
            ByteBuffer sectorData = diskData.SubBuffer( curPos, trackInfo.SectorInformation[sector].ActualDataLength );

            trackInfo.Sectors.Add( sectorData );

            curPos += trackInfo.SectorInformation[sector].ActualDataLength;
          }
        }
      }

      /*
      if ( ( diskData.Length != 174848 )
      &&   ( diskData.Length != 175531 )
      &&   ( diskData.Length != 196608 )
      &&   ( diskData.Length != 197376 ) )
      {
        _LastError = "disk image size is not supported";
        return false;
      }
      switch ( diskData.Length )
      {
        case 174848:
        case 175531:
          CreateEmptyMedia();
          break;
        case 196608:
        case 197376:
          CreateEmptyMedia40Tracks();
          break;
      }

      int     dataPos = 0;
      for ( int i = 0; i < Tracks.Count; ++i )
      {
        for ( int j = 0; j < Tracks[i].Sectors.Count; ++j )
        {
          diskData.CopyTo( Tracks[i].Sectors[j].Data, dataPos, 256 );
          dataPos += 256;
        }
      }
      for ( int i = 0; i < Tracks.Count; ++i )
      {
        for ( int j = 0; j < Tracks[i].Sectors.Count; ++j )
        {
          Tracks[i].Sectors[j].Free = !IsSectorMarkedAsUsedInBAM( i + 1, j );
        }
      }

      if ( ( diskData.Length == 175531 )
      ||   ( diskData.Length == 197376 ) )
      {
        // error info appended
        for ( int i = 0; i < Tracks.Count; ++i )
        {
          for ( int j = 0; j < Tracks[i].Sectors.Count; ++j )
          {
            Tracks[i].Sectors[j].SectorErrorCode = diskData.ByteAt( dataPos );
            ++dataPos;
          }
        }
      }*/
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
      Types.FileInfo    fileInfo = null;

      _LastError = "";
      /*
      Location fileLocation;
      

      if ( !LocateFile( Filename, out fileLocation, out fileInfo ) )
      {
        _LastError = "File not found";
        return null;
      }

      GR.Memory.ByteBuffer result = new GR.Memory.ByteBuffer();

      bool endFound = false;

      while ( !endFound )
      {
        Sector  sec       = Tracks[fileLocation.Track - 1].Sectors[fileLocation.Sector];
        fileLocation      = sec.NextLocation;
        if ( fileLocation == null )
        {
          result.Append( sec.Data.SubBuffer( 2, sec.Data.ByteAt( 1 ) - 1 ) );
          endFound = true;
          break;
        }
        result.Append( sec.Data.SubBuffer( 2 ) );
      }
      fileInfo.Data = result;
      */
      return fileInfo;
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



    public override GR.Memory.ByteBuffer Compile()
    {
      _LastError = "";
      GR.Memory.ByteBuffer result = new GR.Memory.ByteBuffer();
      /*
      foreach ( Track track in Tracks )
      {
        foreach ( Sector sector in track.Sectors )
        {
          result.Append( sector.Data );
        }
      }*/
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
      throw new NotImplementedException();
    }



  }
}
