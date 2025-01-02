using GR.Memory;
using RetroDevStudio;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;



namespace RetroDevStudio.Formats
{
  [MediaType( MediaType.DISK )]
  [MediaFormat( MediaFormatType.D64 )]
  [MediaFormat( MediaFormatType.D64_40 )]
  [Category( "Commodore" )] 
  public class D64 : CommodoreDisk
  {
    public D64()
    {
      TRACK_HEADER      = 18;
      SECTOR_HEADER     = 0;

      TRACK_BAM         = 18;
      SECTOR_BAM        = 0;

      TRACK_DIRECTORY   = 18;
      SECTOR_DIRECTORY  = 1;
    }



    public override void CreateEmptyMedia()
    {
      _LastError = "";
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
      CreateBAM();
    }



    public void CreateEmptyMedia40Tracks()
    {
      _LastError = "";
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
      }
      CreateBAM();
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



    public override bool Load( string Filename )
    {
      _LastError = "";
      GR.Memory.ByteBuffer diskData = GR.IO.File.ReadAllBytes( Filename );
      if ( diskData == null )
      {
        _LastError = "Could not open/read file";
        return false;
      }

      /*
      35 track, no errors        174848
      35 track, 683 error bytes  175531
      40 track, no errors        196608
      40 track, 768 error bytes  197376
      */

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
      }
      return true;
    }



    private bool IsSectorMarkedAsUsedInBAM( int Track, int Sector )
    {
      _LastError = "";
      if ( ( Track < 1 )
      ||   ( Track > Tracks.Count ) )
      {
        _LastError = "track index out of bounds";
        return false;
      }
      Track track = Tracks[Track - 1];

      if ( ( Sector < 0 )
      ||   ( Sector >= track.Sectors.Count ) )
      {
        _LastError = "sector index out of bounds";
        return false;
      }
      Sector  bam = Tracks[TRACK_BAM - 1].Sectors[SECTOR_BAM];

      byte mask = (byte)( 1 << ( Sector & 7 ) );

      if ( ( bam.Data.ByteAt( Track * 4 + Sector / 8 + 1 ) & mask ) == 0 )
      {
        return true;
      }
      return false;
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



    bool AddDirectoryEntry( GR.Memory.ByteBuffer Filename, int StartTrack, int StartSector, int SectorsWritten, Types.FileTypeNative Type )
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
            sect.Data.SetU8At( BYTES_PER_DIR_ENTRY * i + 2, (byte)( Type | FileTypeNative.COMMODORE_CLOSED ) );
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
        do
        {
          sector = ( sector + directoryInterleave ) % dirTrack.Sectors.Count;

          // do NOT write into BAM
        }
        while ( sector == SECTOR_BAM );

        if ( sector == 1 )
        {
          // arrived at starting sector, disk full!
          break;
        }
        if ( sect.Data.ByteAt( 0 ) == 0 )
        {
          // current sector was last dir sector
          sect.Data.SetU8At( 0, dirTrackIndex );
          sect.Data.SetU8At( 1, (byte)( sector ) );
          AllocSector( dirTrackIndex, sector );

          dirTrack.Sectors[sector].Data.SetU8At( 0, 0 );
          dirTrack.Sectors[sector].Data.SetU8At( 1, 0xff );
        }
      }
      while ( true );
      _LastError = "disk is full";
      return false;
    }



    public override string FileFilter
    {
      get
      {
        return "Disk Files|*.D64|" + base.FileFilter;
      }
    }



  }
}
