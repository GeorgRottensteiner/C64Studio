using RetroDevStudio;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;



namespace RetroDevStudio.Formats
{
  [MediaType( MediaType.DISK )]
  [MediaFormat( MediaFormatType.D81 )]
  [DefaultFileExtension( ".d81" )]
  [Category( "Commodore" )]
  public class D81 : CommodoreDisk
  {
    public D81()
    { 
      TRACK_HEADER      = 40;
      SECTOR_HEADER     = 0;

      TRACK_BAM         = 40;
      SECTOR_BAM        = 1;

      TRACK_DIRECTORY   = 40;
      SECTOR_DIRECTORY  = 3;
    }



    public override void CreateEmptyMedia()
    {
      _LastError = "";
      Tracks.Clear();
      for ( int i = 0; i < 80; ++i )
      {
        Tracks.Add( new Track( i + 1, 40 ) );
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
          Tracks[TRACK_HEADER - 1].Sectors[0].Data.SetU8At( 4 + i, 0xa0 );
        }
        else
        {
          Tracks[TRACK_HEADER - 1].Sectors[0].Data.SetU8At( 4 + i, (byte)Diskname[i] );
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
          Tracks[TRACK_HEADER - 1].Sectors[SECTOR_HEADER].Data.SetU8At( 0x16 + i, 0xa0 );
          Tracks[TRACK_BAM - 1].Sectors[SECTOR_BAM].Data.SetU8At( 4 + i, 0xa0 );
          Tracks[TRACK_BAM - 1].Sectors[SECTOR_BAM + 1].Data.SetU8At( 4 + i, 0xa0 );
        }
        else
        {
          Tracks[TRACK_HEADER - 1].Sectors[SECTOR_HEADER].Data.SetU8At( 0x16 + i, (byte)DiskID[i] );
          Tracks[TRACK_BAM - 1].Sectors[SECTOR_BAM].Data.SetU8At( 4 + i, (byte)DiskID[i] );
          Tracks[TRACK_BAM - 1].Sectors[SECTOR_BAM + 1].Data.SetU8At( 4 + i, (byte)DiskID[i] );
        }
      }
    }



    void CreateBAM()
    {
      _LastError = "";

      // Track/Sector of first directory sector
      Track track40 = Tracks[TRACK_HEADER - 1];

      track40.Sectors[SECTOR_HEADER].Data.SetU8At( 0, (byte)TRACK_DIRECTORY );
      track40.Sectors[SECTOR_HEADER].Data.SetU8At( 1, (byte)SECTOR_DIRECTORY );

      // DOS Version
      track40.Sectors[SECTOR_HEADER].Data.SetU8At( 2, 0x44 );

      track40.Sectors[SECTOR_HEADER].Free = false;

      // mark first directory entry as empty
      track40.Sectors[SECTOR_BAM].Data.SetU8At( 0, 0 );
      track40.Sectors[SECTOR_BAM].Data.SetU8At( 1, 0xff );
      track40.Sectors[SECTOR_BAM].Free = false;

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
      Track trackBAM = Tracks[TRACK_BAM - 1];
      Sector sectorBAM = trackBAM.Sectors[SECTOR_BAM];
      Sector sectorBAM2 = trackBAM.Sectors[SECTOR_BAM + 1];
      int sectorOffset = 0;

      sectorBAM.Free = false;
      sectorBAM2.Free = false;

      // mark first directory sector as used
      track40.Sectors[SECTOR_DIRECTORY].Free = false;

      // point to second BAM sector
      sectorBAM.Data.SetU8At( 0, (byte)TRACK_BAM );
      sectorBAM.Data.SetU8At( 1, (byte)( SECTOR_BAM + 1 ) );
      sectorBAM.Data.SetU8At( 2, 0x44 );   // version
      sectorBAM.Data.SetU8At( 3, 0xbb );   // one complement from version
      sectorBAM.Data.SetU8At( 6, 0xc0 );   // IO byte
      sectorBAM2.Data.SetU8At( 0, 0 );
      sectorBAM2.Data.SetU8At( 1, 0xff );
      sectorBAM2.Data.SetU8At( 2, 0x44 );   // version
      sectorBAM2.Data.SetU8At( 3, 0xbb );   // one complement from version
      sectorBAM2.Data.SetU8At( 6, 0xc0 );   // IO byte
      for ( int i = 0; i < Tracks.Count; ++i )
      {
        if ( i == 40 )
        {
          sectorBAM = sectorBAM2;
          sectorOffset = -40;
        }
        sectorBAM.Data.SetU8At( 16 + 6 * ( i + sectorOffset ), (byte)Tracks[i].FreeSectors );

        sectorBAM.Data.SetU8At( 16 + 6 * ( i + sectorOffset ) + 1, 0 );
        sectorBAM.Data.SetU8At( 16 + 6 * ( i + sectorOffset ) + 2, 0 );
        sectorBAM.Data.SetU8At( 16 + 6 * ( i + sectorOffset ) + 3, 0 );
        sectorBAM.Data.SetU8At( 16 + 6 * ( i + sectorOffset ) + 4, 0 );
        sectorBAM.Data.SetU8At( 16 + 6 * ( i + sectorOffset ) + 5, 0 );

        for ( int j = 0; j < Tracks[i].Sectors.Count; ++j )
        {
          if ( Tracks[i].Sectors[j].Free )
          {
            byte oldValue = sectorBAM.Data.ByteAt( 16 + 6 * ( i + sectorOffset ) + 1 + j / 8 );
            oldValue |= (byte)( 1 << ( j % 8 ) );
            sectorBAM.Data.SetU8At( 16 + 6 * ( i + sectorOffset ) + 1 + j / 8, oldValue );
          }
        }
      }

      // disk name (padded with 0xa0)
      SetDiskName( "EMPTY DISK" );

      track40.Sectors[SECTOR_HEADER].Data.SetU8At( 0x14, 0xa0 );
      track40.Sectors[SECTOR_HEADER].Data.SetU8At( 0x15, 0xa0 );

      SetDiskID( "1a" );

      track40.Sectors[SECTOR_HEADER].Data.SetU8At( 0x18, 0xa0 );

      // DOS type
      track40.Sectors[SECTOR_HEADER].Data.SetU8At( 0x19, (byte)'3' );
      track40.Sectors[SECTOR_HEADER].Data.SetU8At( 0x1a, (byte)'D' );
      track40.Sectors[SECTOR_HEADER].Data.SetU8At( 0x1b, 0xa0 );
      track40.Sectors[SECTOR_HEADER].Data.SetU8At( 0x1c, 0xa0 );

      // track pointer of first directory sector
      track40.Sectors[SECTOR_DIRECTORY].Data.SetU8At( 0, 0 );
      track40.Sectors[SECTOR_DIRECTORY].Data.SetU8At( 1, 0xff );
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
      if ( ( diskData.Length != 822400 )      // with error bytes
      &&   ( diskData.Length != 819200 ) )
      {
        _LastError = "disk image size is not equal 819200 or 822400 bytes";
        return false;
      }
      CreateEmptyMedia();

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

      if ( diskData.Length == 822400 )
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
      int     trackOffset = -1;
      if ( Track >= 41 )
      {
        bam = Tracks[TRACK_BAM - 1].Sectors[SECTOR_BAM + 1];
        trackOffset = -41;
      }

      // mask out sector
      byte mask = (byte)( 1 << ( Sector & 7 ) );

      if ( ( bam.Data.ByteAt( 16 + ( Track + trackOffset ) * 6 + Sector / 8 + 1 ) & mask ) == 0 )
      {
        return true;
      }
      return false;
    }



    public override bool IsSectorAllocated( int Track, int Sector )
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
      int     trackOffset = -1;
      if ( Track >= 41 )
      {
        bam = Tracks[TRACK_BAM - 1].Sectors[SECTOR_BAM + 1];
        trackOffset = -41;
      }

      byte mask = (byte)( 1 << ( Sector & 7 ) );
      if ( ( bam.Data.ByteAt( 16 + ( Track + trackOffset ) * 6 + Sector / 8 + 1 ) & mask ) != 0 )
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
      int     trackOffset = -1;
      if ( Track >= 41 )
      {
        bam = Tracks[TRACK_BAM - 1].Sectors[SECTOR_BAM + 1];
        trackOffset = -41;
      }

      // adjust free sectors
      bam.Data.SetU8At( 16 + ( Track + trackOffset ) * 6, (byte)( bam.Data.ByteAt( 16 + ( Track + trackOffset ) * 6 ) - 1 ) );

      // mask out sector
      byte mask = (byte)( 1 << ( Sector & 7 ) );
      bam.Data.SetU8At( 16 + ( Track + trackOffset ) * 6 + Sector / 8 + 1, (byte)( bam.Data.ByteAt( 16 + ( Track + trackOffset ) * 6 + Sector / 8 + 1 ) & ~mask ) );

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
      Sector bam = Tracks[17].Sectors[0];

      // adjust free sectors
      bam.Data.SetU8At( Track * 4, (byte)( bam.Data.ByteAt( Track * 4 ) + 1 ) );

      // mask in sector
      byte mask = (byte)( 1 << ( Sector & 7 ) );
      bam.Data.SetU8At( Track * 4 + Sector / 8 + 1, (byte)( bam.Data.ByteAt( Track * 4 + Sector / 8 + 1 ) | mask ) );

      Tracks[Track - 1].Sectors[Sector].Free = true;
    }



    public override string FileFilter
    {
      get
      {
        return "Disk Files|*.D81|" + base.FileFilter;
      }
    }




  }
}
