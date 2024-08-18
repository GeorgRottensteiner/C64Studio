using RetroDevStudio.Types;
using RetroDevStudio;
using System;
using System.Collections.Generic;
using System.Text;
using GR.Memory;
using System.ComponentModel;



namespace RetroDevStudio.Formats
{
  public abstract class CommodoreDisk : MediaFormat
  {
    protected const int BYTES_PER_DIR_ENTRY = 32;

    protected int TRACK_HEADER = -1;
    protected int SECTOR_HEADER = -1;

    protected int TRACK_BAM = -1;
    protected int SECTOR_BAM = -1;

    protected int TRACK_DIRECTORY = -1;
    protected int SECTOR_DIRECTORY = -1;



    public class Location
    {
      public int     Track = 0;
      public int     Sector = -1;



      public Location()
      {
      }



      public Location( int Track, int Sector )
      {
        this.Track = Track;
        this.Sector = Sector;
      }
    }



    public class DirEntryLocation : Location
    {
      public int     DirEntry = -1;



      public DirEntryLocation()
      {
      }



      public DirEntryLocation( int Track, int Sector, int DirEntry )
      {
        this.Track    = Track;
        this.Sector   = Sector;
        this.DirEntry = DirEntry;
      }
    }



    protected class Sector
    {
      public bool                 Free = true;
      public GR.Memory.ByteBuffer Data = new GR.Memory.ByteBuffer( 256 );
      public Location             Location = new Location();
      public byte                 SectorErrorCode = 0;



      public int TrackNo
      {
        get
        {
          return Location.Track;
        }
        set
        {
          Location.Track = value;
        }
      }



      public int SectorNo
      {
        get
        {
          return Location.Sector;
        }
        set
        {
          Location.Sector = value;
        }
      }



      public Location NextLocation
      {
        get
        {
          int     nextTrack = Data.ByteAt( 0 );
          if ( nextTrack == 0 )
          {
            return null;
          }
          return new Location( nextTrack, Data.ByteAt( 1 ) );
        }
        set
        {
          Location = value;
        }
      }



      public Sector( int TrackNo, int SectorNo )
      {
        Location.Track  = TrackNo;
        Location.Sector = SectorNo;
      }
    }



    protected class Track
    {
      public List<Sector> Sectors = new List<Sector>();
      public int TrackNo = 0;


      public Track( int TrackNo, int SectorCount )
      {
        Sectors.Clear();
        this.TrackNo = TrackNo;
        for ( int i = 0; i < SectorCount; ++i )
        {
          Sectors.Add( new Sector( TrackNo, i ) );
        }
      }



      public int FreeSectors
      {
        get
        {
          int     free = 0;
          for ( int i = 0; i < Sectors.Count; ++i )
          {
            if ( Sectors[i].Free )
            {
              ++free;
            }
          }
          return free;
        }
      }
    }



    protected List<Track> Tracks = new List<Track>();

    protected string  _LastError = "";



    protected CommodoreDisk()
    {
      SupportsRenamingTitle = true;
    }



    public override void Clear()
    {
      _LastError = "";
      CreateEmptyMedia();
    }



    protected ByteBuffer PadFilename( ByteBuffer Filename )
    {
      if ( Filename.Length < 16 )
      {
        var padded = new ByteBuffer( 16, 0xa0 );
        Filename.CopyTo( padded );

        return padded;
      }
      return Filename;
    }



    void SetDiskName( string Diskname )
    {
      _LastError = "";
      if ( Diskname.Length > 16 )
      {
        Diskname = Diskname.Substring( 0, 16 );
      }
      for ( int i = 0; i < 16; ++i )
      {
        if ( i >= Diskname.Length )
        {
          Tracks[TRACK_HEADER - 1].Sectors[SECTOR_HEADER].Data.SetU8At( 0x90 + i, 0xa0 );
        }
        else
        {
          Tracks[TRACK_HEADER - 1].Sectors[SECTOR_HEADER].Data.SetU8At( 0x90 + i, (byte)Diskname[i] );
        }
      }
    }



    void SetDiskID( string DiskID )
    {
      _LastError = "";
      if ( DiskID.Length > 2 )
      {
        DiskID = DiskID.Substring( 0, 2 );
      }

      for ( int i = 0; i < 2; ++i )
      {
        if ( i >= DiskID.Length )
        {
          Tracks[TRACK_HEADER - 1].Sectors[SECTOR_HEADER].Data.SetU8At( 0xA2 + i, 0xa0 );
        }
        else
        {
          Tracks[TRACK_HEADER - 1].Sectors[SECTOR_HEADER].Data.SetU8At( 0xA2 + i, (byte)DiskID[i] );
        }
      }
    }



    public ByteBuffer DiskID
    {
      get
      {
        if ( TRACK_HEADER - 1 >= Tracks.Count )
        {
          return new ByteBuffer( 5 );
        }
        Track trackHeader = Tracks[TRACK_HEADER - 1];

        return trackHeader.Sectors[SECTOR_HEADER].Data.SubBuffer( 0xa2, 5 );
      }
      set
      {
        Track trackHeader = Tracks[TRACK_HEADER - 1];

        value.CopyTo( trackHeader.Sectors[SECTOR_HEADER].Data, 0, 2, 0xa2 );
        value.CopyTo( trackHeader.Sectors[SECTOR_HEADER].Data, 3, 2, 0xa2 + 3 );
      }
    }



    public ushort DOSType
    {
      get
      {
        Track trackHeader = Tracks[TRACK_HEADER - 1];

        return trackHeader.Sectors[SECTOR_HEADER].Data.UInt16At( 0xa5 );
      }
      set
      {
        Track trackHeader = Tracks[TRACK_HEADER - 1];

        trackHeader.Sectors[SECTOR_HEADER].Data.SetU16At( 0xa5, value );
      }
    }



    public GR.Memory.ByteBuffer DiskName
    {
      get
      {
        _LastError = "";
        if ( Tracks.Count < TRACK_HEADER - 1 )
        {
          _LastError = "Missing track " + TRACK_HEADER;
          return null;
        }

        Track trackHeader = Tracks[TRACK_HEADER - 1];

        return trackHeader.Sectors[SECTOR_HEADER].Data.SubBuffer( 0x90, 16 );
      }
      set
      {
        Track trackHeader = Tracks[TRACK_HEADER - 1];

        if ( value.Length >= 16 )
        {
          value.CopyTo( trackHeader.Sectors[SECTOR_HEADER].Data, 0, 16, 0x90 );
        }
        else
        {
          value.CopyTo( trackHeader.Sectors[SECTOR_HEADER].Data, 0, (int)value.Length, 0x90 );
        }
      }
    }



    public byte DiskDOSVersion
    {
      set
      {
        Track trackHeader = Tracks[TRACK_HEADER - 1];

        trackHeader.Sectors[SECTOR_HEADER].Data.SetU8At( 2, value );
      }
      get
      {
        Track trackHeader = Tracks[TRACK_HEADER - 1];

        return trackHeader.Sectors[SECTOR_HEADER].Data.ByteAt( 2 );
      }
    }



    void CreateBAM()
    {
      _LastError = "";

      // Track/Sector of first directory sector
      Track trackHeader = Tracks[TRACK_HEADER - 1];
      Track trackDirectory = Tracks[TRACK_DIRECTORY - 1];
      Track trackBAM = Tracks[TRACK_BAM - 1];

      trackHeader.Sectors[SECTOR_HEADER].Data.SetU8At( 0, (byte)TRACK_DIRECTORY );
      trackHeader.Sectors[SECTOR_HEADER].Data.SetU8At( 1, (byte)SECTOR_DIRECTORY );

      // DOS Version
      trackHeader.Sectors[SECTOR_HEADER].Data.SetU8At( 2, 0x41 );

      trackHeader.Sectors[SECTOR_HEADER].Free = false;

      // mark first directory entry as empty
      trackDirectory.Sectors[SECTOR_DIRECTORY].Data.SetU8At( 0, 0 );
      trackDirectory.Sectors[SECTOR_DIRECTORY].Data.SetU8At( 1, 0xff );
      trackDirectory.Sectors[SECTOR_DIRECTORY].Free = false;

      /*
         04-8F: BAM entries for each track, in groups  of  four  bytes  per
                track, starting on track 1 (see below for more details)
         The BAM entries require a bit (no pun intended) more of a breakdown. Take
          the first entry at bytes $04-$07 ($12 $FF $F9 $17). The first byte ($12) is
          the number of free sectors on that track. Since we are looking at the track
          1 entry, this means it has 18 (decimal) free sectors. The next three  bytes
          represent the bitmap of which sectors are used/free. Since it is 3 bytes (8
          bits/byte) we have 24 bits of storage. Remember that at  most,  each  track
          only has 21 sectors, so there are a few unused bits.
       */
      trackHeader.Sectors[SECTOR_HEADER].Free = false;
      for ( int i = 0; i < Tracks.Count; ++i )
      {
        trackBAM.Sectors[SECTOR_BAM].Data.SetU8At( 4 + 4 * i, (byte)Tracks[i].FreeSectors );

        trackBAM.Sectors[SECTOR_BAM].Data.SetU8At( 4 + 4 * i + 1, 0 );
        trackBAM.Sectors[SECTOR_BAM].Data.SetU8At( 4 + 4 * i + 2, 0 );
        trackBAM.Sectors[SECTOR_BAM].Data.SetU8At( 4 + 4 * i + 3, 0 );

        for ( int j = 0; j < Tracks[i].Sectors.Count; ++j )
        {
          if ( Tracks[i].Sectors[j].Free )
          {
            byte oldValue = trackBAM.Sectors[SECTOR_BAM].Data.ByteAt( 4 + 4 * i + 1 + j / 8 );
            oldValue |= (byte)( 1 << ( j % 8 ) );
            trackBAM.Sectors[SECTOR_BAM].Data.SetU8At( 4 + 4 * i + 1 + j / 8, oldValue );
          }
        }
      }

      // disk name (padded with 0xa0)
      SetDiskName( "EMPTY DISK" );

      trackHeader.Sectors[SECTOR_HEADER].Data.SetU8At( 0xa0, 0xa0 );
      trackHeader.Sectors[SECTOR_HEADER].Data.SetU8At( 0xa1, 0xa0 );

      SetDiskID( "ID" );

      trackHeader.Sectors[SECTOR_HEADER].Data.SetU8At( 0xA4, 0xa0 );

      // DOS type
      trackHeader.Sectors[SECTOR_HEADER].Data.SetU8At( 0xA5, (byte)'2' );
      trackHeader.Sectors[SECTOR_HEADER].Data.SetU8At( 0xA6, (byte)'A' );

      for ( int i = 0; i < 4; ++i )
      {
        trackHeader.Sectors[SECTOR_HEADER].Data.SetU8At( 0xA7 + i, 0xA0 );
      }
    }



    public void DumpBAM()
    {
      var bamData = Tracks[TRACK_BAM - 1].Sectors[SECTOR_BAM].Data.SubBuffer( 0x04, Tracks.Count * 4 );

      Debug.Log( "BAM" );
      for ( int i = 0; i < bamData.Length; i += 4 )
      {
        int trackNo = i / 4 + 1;
        Debug.Log( $"  Track {trackNo}: Free Sectors {bamData.ByteAt( i )}" );
        for ( int j = 0; j < Tracks[trackNo - 1].Sectors.Count; ++j )
        {
          if ( IsSectorAllocated( trackNo, j ) )
          {
            Debug.Log( $"    Sector {j} allocated" );
          }
        }
      }
    }



    public void DumpTrack( int Track )
    {
      Debug.Log( "Track " + Track.ToString() );
      for ( int i = 0; i < Tracks[Track - 1].Sectors.Count; ++i )
      {
        Debug.Log( Tracks[Track - 1].Sectors[i].Data.ToString() );
      }
    }



    public override bool Save( string Filename )
    {
      GR.Memory.ByteBuffer data = Compile();
      return GR.IO.File.WriteAllBytes( Filename, data );
    }



    protected bool LocateFile( GR.Memory.ByteBuffer Filename, out Location FileLocation, out Types.FileInfo FileInfo )
    {
      _LastError = "";
      FileLocation = null;
      FileInfo = null;

      Filename = PadFilename( Filename );

      Location    curLoc = new Location( TRACK_DIRECTORY, SECTOR_DIRECTORY );

      bool endFound = false;

      while ( !endFound )
      {
        Sector sec = Tracks[curLoc.Track - 1].Sectors[curLoc.Sector];

        for ( int i = 0; i < 8; ++i )
        {
          int fileTrack = sec.Data.ByteAt( 0x20 * i + 3 );
          int fileSector = sec.Data.ByteAt( 0x20 * i + 4 );
          if ( fileTrack != 0 )
          {
            // valid entry?
            if ( sec.Data.ByteAt( 0x20 * i + 2 ) != (byte)Types.FileType.SCRATCHED )
            {
              GR.Memory.ByteBuffer filename = sec.Data.SubBuffer( 0x20 * i + 5, 16 );
              if ( Filename.Compare( filename ) == 0 )
              {
                FileLocation = new Location( fileTrack, fileSector );

                FileInfo = new Types.FileInfo();
                FileInfo.Filename = new GR.Memory.ByteBuffer( filename );

                SetFileInfo( FileInfo, fileTrack, fileSector, sec.Data.ByteAt( 0x20 * i + 2 ), 0 );
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



    private void SetFileInfo( FileInfo FileInfo, int StartTrack, int StartSector, byte FileType, int NumBlocks )
    {
      FileInfo.StartTrack   = StartTrack;
      FileInfo.StartSector  = StartSector;
      FileInfo.Type         = (Types.FileType)( FileType & 0x07 );
      FileInfo.Blocks       = NumBlocks;

      FileInfo.ReadOnly     = ( FileType & 64 ) != 0;
      FileInfo.NotClosed    = ( FileType & 128 ) == 0;
    }



    public override Types.FileInfo LoadFile( GR.Memory.ByteBuffer Filename )
    {
      _LastError = "";

      Location fileLocation;
      Types.FileInfo    fileInfo;

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
        fileLocation = sec.NextLocation;
        if ( fileLocation == null )
        {
          result.Append( sec.Data.SubBuffer( 2, sec.Data.ByteAt( 1 ) - 1 ) );
          endFound = true;
          break;
        }
        result.Append( sec.Data.SubBuffer( 2 ) );
      }
      fileInfo.Data = result;
      return fileInfo;
    }



    public virtual bool IsSectorAllocated( int Track, int Sector )
    {
      _LastError = "";

      if ( ( Track < 1 )
      ||   ( Track > Tracks.Count ) )
      {
        _LastError = "Track index out of bounds";
        return false;
      }
      Track track = Tracks[Track - 1];

      if ( ( Sector < 0 )
      ||   ( Sector >= track.Sectors.Count ) )
      {
        _LastError = "Sector index out of bounds";
        return false;
      }
      Sector bam = Tracks[TRACK_BAM - 1].Sectors[SECTOR_BAM];

      byte mask = (byte)( 1 << ( Sector & 7 ) );
      if ( ( bam.Data.ByteAt( Track * 4 + Sector / 8 + 1 ) & mask ) != 0 )
      {
        return false;
      }
      return true;
    }



    private void AllocSector( int Track, int Sector )
    {
      _LastError = "";
      if ( ( Track < 1 )
      ||   ( Track > Tracks.Count ) )
      {
        _LastError = "track index out of bounds";
        return;
      }
      Track track = Tracks[Track - 1];

      if ( ( Sector < 0 )
      ||   ( Sector >= track.Sectors.Count ) )
      {
        _LastError = "sector index out of bounds";
        return;
      }
      if ( IsSectorAllocated( Track, Sector ) )
      {
        return;
      }
      Sector  bam = Tracks[TRACK_BAM - 1].Sectors[SECTOR_BAM];

      // adjust free sectors
      bam.Data.SetU8At( Track * 4, (byte)( bam.Data.ByteAt( Track * 4 ) - 1 ) );

      // mask out sector
      byte mask = (byte)( 1 << ( Sector & 7 ) );
      bam.Data.SetU8At( Track * 4 + Sector / 8 + 1, (byte)( bam.Data.ByteAt( Track * 4 + Sector / 8 + 1 ) & ~mask ) );

      Tracks[Track - 1].Sectors[Sector].Free = false;
    }



    private void FreeSector( int Track, int Sector )
    {
      _LastError = "";
      if ( ( Track < 1 )
      ||   ( Track > Tracks.Count ) )
      {
        _LastError = "track index out of bounds";
        return;
      }
      Track track = Tracks[Track - 1];

      if ( ( Sector < 0 )
      ||   ( Sector >= track.Sectors.Count ) )
      {
        _LastError = "sector index out of bounds";
        return;
      }
      if ( !IsSectorAllocated( Track, Sector ) )
      {
        return;
      }
      Sector bam = Tracks[TRACK_BAM - 1].Sectors[SECTOR_BAM];

      // adjust free sectors
      bam.Data.SetU8At( Track * 4, (byte)( bam.Data.ByteAt( Track * 4 ) + 1 ) );

      // mask in sector
      byte mask = (byte)( 1 << ( Sector & 7 ) );
      bam.Data.SetU8At( Track * 4 + Sector / 8 + 1, (byte)( bam.Data.ByteAt( Track * 4 + Sector / 8 + 1 ) | mask ) );

      Tracks[Track - 1].Sectors[Sector].Free = true;
    }



    public int FreeBytes()
    {
      int freeUsableBytes = 0;
      int freeBytesTotal = 0;

      foreach ( Track track in Tracks )
      {
        foreach ( Sector sector in track.Sectors )
        {
          if ( sector.Free )
          {
            freeBytesTotal += 256;
            freeUsableBytes += 254;
          }
        }
      }
      return freeUsableBytes;
    }



    bool AddDirectoryEntry( GR.Memory.ByteBuffer Filename, int StartTrack, int StartSector, int SectorsWritten, Types.FileType Type )
    {
      _LastError = "";
      Track   dirTrack = Tracks[TRACK_DIRECTORY - 1];
      byte    dirTrackIndex = (byte)TRACK_DIRECTORY;

      int     directoryInterleave = 3;

      int     sector = 1;
      do
      {
        Sector sect = dirTrack.Sectors[sector];
        for ( int i = 0; i < 8; ++i )
        {
          if ( sect.Data.ByteAt( BYTES_PER_DIR_ENTRY * i + 2 ) == 0 )
          {
            // scratched (empty) entry

            // default set PRG
            if ( i > 0 )
            {
              // set track/sector of next dir sector
              sect.Data.SetU8At( 0, 0 );
              sect.Data.SetU8At( 1, 0 );
            }
            sect.Data.SetU8At( BYTES_PER_DIR_ENTRY * i + 2, (byte)( Type | FileType.CLOSED ) );
            sect.Data.SetU8At( BYTES_PER_DIR_ENTRY * i + 3, (byte)StartTrack );
            sect.Data.SetU8At( BYTES_PER_DIR_ENTRY * i + 4, (byte)StartSector );

            for ( int j = 0; j < 16; ++j )
            {
              sect.Data.SetU8At( BYTES_PER_DIR_ENTRY * i + 5 + j, Filename.ByteAt( j ) );
            }
            sect.Data.SetU16At( BYTES_PER_DIR_ENTRY * i + 30, (UInt16)SectorsWritten );
            return true;
          }
        }
        // do we need to alloc next dir sector?
        sector = ( sector + directoryInterleave ) % dirTrack.Sectors.Count;
        if ( sector == 1 )
        {
          // disk full!!
          break;
        }
        if ( sect.Data.ByteAt( 0 ) == 0 )
        {
          // current sector was last dir sector
          sect.Data.SetU8At( 0, dirTrackIndex );
          sect.Data.SetU8At( 1, (byte)( sector ) );
          AllocSector( dirTrackIndex, sector );

          // new sector was alloced for directory, clean out any existing entries!
          dirTrack.Sectors[sector].Data.Fill( 0, (int)dirTrack.Sectors[sector].Data.Length, 0 );

          dirTrack.Sectors[sector].Data.SetU8At( 0, 0 );
          dirTrack.Sectors[sector].Data.SetU8At( 1, 0xff );
        }
      }
      while ( true );
      _LastError = "disk is full";
      return false;
    }



    public override bool DeleteFile( GR.Memory.ByteBuffer Filename, bool CompleteDelete )
    {
      _LastError = "";
      int   curTrack = TRACK_DIRECTORY;
      int   curSector = SECTOR_DIRECTORY;

      Filename = PadFilename( Filename );
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
                int nextTrack = fileSector.Data.ByteAt( 0 );
                int nextSector = fileSector.Data.ByteAt( 1 );

                if ( CompleteDelete )
                {
                  Tracks[startTrack - 1].Sectors[startSector].Data.Fill( 0, (int)Tracks[startTrack - 1].Sectors[startSector].Data.Length, 0 );
                }

                FreeSector( startTrack, startSector );

                startTrack = nextTrack;
                startSector = nextSector;
              }
              if ( CompleteDelete )
              {
                // remove all traces from directory entry
                // keep 2 bytes of T/S link intact
                sect.Data.Fill( BYTES_PER_DIR_ENTRY * i + 2, BYTES_PER_DIR_ENTRY - 2, 0 );
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
      }
    }



    public override bool RenameFile( GR.Memory.ByteBuffer Filename, GR.Memory.ByteBuffer NewFilename )
    {
      _LastError = "";
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
      }
    }



    public override void Validate()
    {
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
      }
    }



    public override bool WriteFile( GR.Memory.ByteBuffer Filename, GR.Memory.ByteBuffer Content, Types.FileType Type )
    {
      _LastError = "";
      GR.Memory.ByteBuffer    dataToWrite = new GR.Memory.ByteBuffer( Content );
      if ( dataToWrite.Length > FreeBytes() )
      {
        _LastError = "file too large";
        return false;
      }

      Filename = PadFilename( Filename );

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
              //Debug.Log( "Write to T/S " + trackIndex + "," + searchSector );
              dataToWrite.CopyTo( previousSector.Data, writeOffset, 254, 2 );
              previousSector.Free = false;
              writeOffset += 254;
              bytesToWrite -= 254;
              ++sectorsWritten;
              prevSector = searchSector;
              goto write_next_sector;
            }
            // last sector
            //Debug.Log( "Write to T/S " + trackIndex + "," + searchSector );
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
      }
      _LastError = "disk is full";
      return false;
    }



    public override GR.Memory.ByteBuffer Compile()
    {
      _LastError = "";
      GR.Memory.ByteBuffer result = new GR.Memory.ByteBuffer();

      foreach ( Track track in Tracks )
      {
        foreach ( Sector sector in track.Sectors )
        {
          result.Append( sector.Data );
        }
      }
      return result;
    }



    public int Blocks
    {
      get
      {
        int blocks = 0;

        int trackIndex = 1;
        foreach ( Track track in Tracks )
        {
          if ( trackIndex != TRACK_DIRECTORY )
          {
            blocks += track.Sectors.Count;
          }
          ++trackIndex;
        }
        return blocks;
      }
    }



    public virtual int FreeBlocks
    {
      get
      {
        int blocks = 0;

        int trackIndex = 1;
        foreach ( Track track in Tracks )
        {
          if ( trackIndex != TRACK_DIRECTORY )
          {
            blocks += track.FreeSectors;
          }
          ++trackIndex;
        }
        return blocks;
      }
    }



    public override int FreeSlots
    {
      get
      {
        return FreeBlocks;
      }
    }



    public override int Slots
    {
      get
      {
        return Blocks;
      }
    }



    private void FollowChain( int Track,
                              int Sector,
                              GR.Collections.Map<GR.Generic.Tupel<int, int>, GR.Memory.ByteBuffer> UsedSectors,
                              GR.Memory.ByteBuffer Filename,
                              List<string> Errors )
    {
      int fileTrack = Track;
      int fileSector = Sector;

      while ( fileTrack != 0 )
      {
        GR.Generic.Tupel<int,int>     location = new GR.Generic.Tupel<int,int>( fileTrack, fileSector );

        if ( ( UsedSectors[location].Length > 0 )
        && ( UsedSectors[location] != Filename ) )
        {
          Errors.Add( "Sector " + location.first + ", Track " + location.second + " is referenced by more than one file" );
        }

        int newTrack = Tracks[fileTrack - 1].Sectors[fileSector].Data.ByteAt( 0 );
        int newSector = Tracks[fileTrack - 1].Sectors[fileSector].Data.ByteAt( 1 );

        if ( newTrack == 0 )
        {
          return;
        }

        if ( ( newTrack < 1 )
        || ( newTrack > Tracks.Count ) )
        {
          Errors.Add( "Reference to invalid track " + newTrack + " encountered in track " + fileTrack + ", Sector " + fileSector );
          return;
        }
        fileTrack = newTrack;
        fileSector = newSector;
      }
    }



    private Sector FindPreviousDirSector( int Track, int Sector )
    {
      int   curTrack = Track;

      foreach ( Sector sec in Tracks[curTrack - 1].Sectors )
      {
        if ( ( sec.Data.ByteAt( 0 ) == Track )
        &&   ( sec.Data.ByteAt( 1 ) == Sector ) )
        {
          // this sector points at me
          return sec;
        }
      }
      return null;
    }



    private Sector FindNextDirSector( int Track, int Sector )
    {
      int nextTrack = Tracks[Track - 1].Sectors[Sector].Data.ByteAt( 0 );
      if ( nextTrack == 0 )
      {
        return null;
      }
      int nextSector = Tracks[Track - 1].Sectors[Sector].Data.ByteAt( 1 );
      return Tracks[nextTrack - 1].Sectors[nextSector];
    }



    public List<string> CheckForErrors()
    {
      List<string> errors = new List<string>();

      GR.Collections.Map<GR.Generic.Tupel<int, int>, GR.Memory.ByteBuffer> usedSectors = new GR.Collections.Map<GR.Generic.Tupel<int, int>, GR.Memory.ByteBuffer>();

      int curTrack = TRACK_DIRECTORY;
      int curSector = SECTOR_DIRECTORY;
      bool endFound = false;

      while ( !endFound )
      {
        Sector sec = Tracks[curTrack - 1].Sectors[curSector];

        for ( int i = 0; i < 8; ++i )
        {
          int fileTrack = sec.Data.ByteAt( 0x20 * i + 3 );
          int fileSector = sec.Data.ByteAt( 0x20 * i + 4 );
          if ( sec.Data.ByteAt( 0x20 * i + 2 ) != (byte)Types.FileType.SCRATCHED )
          {
            // valid entry?
            var info = new Types.FileInfo();

            FollowChain( fileTrack, fileSector, usedSectors, info.Filename, errors );
          }
        }

        curTrack = sec.Data.ByteAt( 0 );
        curSector = sec.Data.ByteAt( 1 );

        if ( curTrack == 0 )
        {
          // track = 0 marks last directory entry
          endFound = true;
        }
      }

      return errors;
    }



    private DirEntryLocation LocateDirEntry( int Track, int Sector )
    {
      Location  curLoc = new Location( TRACK_DIRECTORY, SECTOR_DIRECTORY );

      while ( true )
      {
        Sector sec = Tracks[curLoc.Track - 1].Sectors[curLoc.Sector];

        for ( int i = 0; i < 8; ++i )
        {
          int fileTrack  = sec.Data.ByteAt( BYTES_PER_DIR_ENTRY * i + 3 );
          int fileSector = sec.Data.ByteAt( BYTES_PER_DIR_ENTRY * i + 4 );
          if ( sec.Data.ByteAt( BYTES_PER_DIR_ENTRY * i + 2 ) != (byte)Types.FileType.SCRATCHED )
          {
            // valid entry?
            if ( ( fileTrack == Track )
            &&   ( fileSector == Sector ) )
            {
              return new DirEntryLocation( curLoc.Track, curLoc.Sector, i );
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
      return null;
    }



    private DirEntryLocation LocateDirEntry( int DirEntryIndex )
    {
      Location  curLoc = new Location( TRACK_DIRECTORY, SECTOR_DIRECTORY );

      int     entryIndex = 0;

      while ( true )
      {
        Sector sec = Tracks[curLoc.Track - 1].Sectors[curLoc.Sector];

        for ( int i = 0; i < 8; ++i )
        {
          int fileTrack  = sec.Data.ByteAt( 0x20 * i + 3 );
          int fileSector = sec.Data.ByteAt( 0x20 * i + 4 );
          if ( sec.Data.ByteAt( 0x20 * i + 2 ) != (byte)Types.FileType.SCRATCHED )
          {
            // valid entry?
            if ( DirEntryIndex == entryIndex )
            {
              return new DirEntryLocation( curLoc.Track, curLoc.Sector, i );
            }
            ++entryIndex;
          }
        }
        curLoc = sec.NextLocation;
        if ( curLoc == null )
        {
          // track = 0 marks last directory entry
          break;
        }
      }
      return null;
    }



    private bool FindPreviousDirEntry( DirEntryLocation DirEntry, out DirEntryLocation ResultDirEntry )
    {
      ResultDirEntry = null;

      if ( DirEntry.DirEntry > 0 )
      {
        ResultDirEntry = new DirEntryLocation( DirEntry.Track, DirEntry.Sector, DirEntry.DirEntry - 1 );
        return true;
      }
      int curTrack = DirEntry.Track;
      foreach ( Sector sec in Tracks[curTrack - 1].Sectors )
      {
        if ( sec.NextLocation == null )
        {
          continue;
        }
        if ( ( sec.NextLocation.Track == DirEntry.Track )
        &&   ( sec.NextLocation.Sector == DirEntry.Sector ) )
        {
          // this sector points at me
          ResultDirEntry = new DirEntryLocation( sec.TrackNo, sec.SectorNo, 7 );
          return true;
        }
      }
      return false;
    }



    private bool FindNextDirEntry( DirEntryLocation DirEntry, out DirEntryLocation ResultDirEntry )
    {
      ResultDirEntry = null;

      if ( DirEntry.DirEntry < 7 )
      {
        ResultDirEntry = new DirEntryLocation( DirEntry.Track, DirEntry.Sector, DirEntry.DirEntry + 1 );
        return true;
      }
      Location    nextSector = Tracks[DirEntry.Track - 1].Sectors[DirEntry.Sector].NextLocation;
      if ( nextSector == null )
      {
        return false;
      }
      Sector sec = Tracks[nextSector.Track - 1].Sectors[nextSector.Sector];
      ResultDirEntry = new DirEntryLocation( sec.TrackNo, sec.SectorNo, 0 );
      return true;
    }



    public bool MoveFileUp( Types.FileInfo File )
    {
      _LastError = "";
      if ( File.DirEntryIndex == 0 )
      {
        return false;
      }
      DirEntryLocation dirLocOrig = LocateDirEntry( File.DirEntryIndex );
      if ( dirLocOrig == null )
      {
        return false;
      }
      DirEntryLocation dirLocPrev = LocateDirEntry( File.DirEntryIndex - 1 );
      Sector secOrig = Tracks[dirLocOrig.Track - 1].Sectors[dirLocOrig.Sector];
      Sector secPrev = Tracks[dirLocPrev.Track - 1].Sectors[dirLocPrev.Sector];

      // exchange dir entry content
      GR.Memory.ByteBuffer tempData = secOrig.Data.SubBuffer( 0x20 * dirLocOrig.DirEntry + 2, 30 );
      secPrev.Data.CopyTo( secOrig.Data, 0x20 * dirLocPrev.DirEntry + 2, 30, 0x20 * dirLocOrig.DirEntry + 2 );
      tempData.CopyTo( secPrev.Data, 0, 30, 0x20 * dirLocPrev.DirEntry + 2 );
      return true;
    }



    public bool MoveFileDown( Types.FileInfo File )
    {
      _LastError = "";
      DirEntryLocation dirLocOrig = LocateDirEntry( File.DirEntryIndex );
      if ( dirLocOrig == null )
      {
        return false;
      }
      DirEntryLocation dirLocNext = LocateDirEntry( File.DirEntryIndex + 1 );
      if ( dirLocNext == null )
      {
        _LastError = "could not find next directory entry";
        return false;
      }
      Sector secOrig = Tracks[dirLocOrig.Track - 1].Sectors[dirLocOrig.Sector];
      Sector secPrev = Tracks[dirLocNext.Track - 1].Sectors[dirLocNext.Sector];

      // exchange dir entry content
      GR.Memory.ByteBuffer tempData = secOrig.Data.SubBuffer( 0x20 * dirLocOrig.DirEntry + 2, 30 );
      secPrev.Data.CopyTo( secOrig.Data, 0x20 * dirLocNext.DirEntry + 2, 30, 0x20 * dirLocOrig.DirEntry + 2 );
      tempData.CopyTo( secPrev.Data, 0, 30, 0x20 * dirLocNext.DirEntry + 2 );
      return true;
    }



    public override string FileFilter
    {
      get
      {
        return base.FileFilter;
      }
    }



    public override GR.Memory.ByteBuffer Title
    {
      get
      {
        GR.Memory.ByteBuffer    title = new GR.Memory.ByteBuffer();

        title.Append( Util.ToPETSCII( "0 \"" ) );
        title.Append( DiskName );
        title.Append( Util.ToPETSCII( "\" " ) );
        title.Append( DiskID );
        return title;
      }
    }



    public override void ChangeFileType( FileInfo FileToChange, FileType NewFileType )
    {
      base.ChangeFileType( FileToChange, NewFileType );

      var dirEntry = LocateDirEntry( FileToChange.DirEntryIndex );
      if ( dirEntry == null )
      {
        return; 
      }

      Sector secOrig = Tracks[dirEntry.Track - 1].Sectors[dirEntry.Sector];

      secOrig.Data.SetU8At( 0x20 * dirEntry.DirEntry + 2, (byte)( NewFileType | FileType.CLOSED ) );
    }



    public override string LastError
    {
      get 
      {
        return _LastError;
      }
    }



    public override List<RetroDevStudio.Types.FileInfo> Files()
    {
      _LastError = "";

      var files = new List<FileInfo>();

      int   curTrack = TRACK_DIRECTORY;
      int   curSector = SECTOR_DIRECTORY;
      bool  endFound = false;
      int   dirEntryIndex = 0;

      if ( Tracks.Count < curTrack - 1 )
      {
        return files;
      }

      while ( !endFound )
      {
        Sector sec = Tracks[curTrack - 1].Sectors[curSector];

        for ( int i = 0; i < 8; ++i )
        {
          int fileTrack  = sec.Data.ByteAt( 0x20 * i + 3 );
          int fileSector = sec.Data.ByteAt( 0x20 * i + 4 );
          if ( sec.Data.ByteAt( 0x20 * i + 2 ) != (byte)Types.FileType.SCRATCHED )
          {
            // valid entry?
            Types.FileInfo info = new Types.FileInfo();

            info.Filename       = sec.Data.SubBuffer( 0x20 * i + 5, 16 );

            SetFileInfo( info, 
                         fileTrack, fileSector, 
                         sec.Data.ByteAt( 0x20 * i + 2 ), 
                         sec.Data.ByteAt( 0x20 * i + 30 ) + 256 * sec.Data.ByteAt( 0x20 * i + 31 ) );

            info.DirEntryIndex  = dirEntryIndex;
            ++dirEntryIndex;
            files.Add( info );
          }
        }

        curTrack = sec.Data.ByteAt( 0 );
        curSector = sec.Data.ByteAt( 1 );

        if ( curTrack == 0 )
        {
          // track = 0 marks last directory entry
          endFound = true;
        }
      }
      return files;
    }



    public override MediaFilenameType FilenameType
    {
      get
      {
        return MediaFilenameType.COMMODORE;
      }
    }



    public ByteBuffer ReadSector( int TrackNo, int SectorNo )
    {
      if ( ( TrackNo < 1 )
      ||   ( TrackNo + 1 >= Tracks.Count )
      ||   ( SectorNo < 0 )
      ||   ( SectorNo >= Tracks[TrackNo].Sectors.Count ) )
      {
        return new ByteBuffer();
      }
      return new ByteBuffer( Tracks[TrackNo - 1].Sectors[SectorNo].Data );
    }




  }
}
