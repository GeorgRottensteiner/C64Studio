using System;
using System.Collections.Generic;
using System.Text;

namespace RetroDevStudio.Formats
{
  public class Tap : MediaFormat
  {
    public class FileEntry
    {
      public ushort   StartAddress = 0;
      public ushort   EndAddress = 0;
      public GR.Memory.ByteBuffer Filename = new GR.Memory.ByteBuffer();
      public GR.Memory.ByteBuffer Data = new GR.Memory.ByteBuffer();
    };

    private List<FileEntry>               TapFiles = new List<FileEntry>();

    private byte                          CheckSum = 0;

    private string                        _LastError = "";



    private void AppendByte( GR.Memory.ByteBuffer Data, byte Value )
    {
      // new data marker
      Data.AppendU8( 0x56 );
      Data.AppendU8( 0x42 );

      byte    checkBit = 1;

      for ( int i = 0; i < 8; ++i )
      {
        if ( ( Value & ( 1 << i ) ) != 0 )
        {
          // bit set
          Data.AppendU8( 0x42 );
          Data.AppendU8( 0x30 );

          checkBit ^= 1;
        }
        else
        {
          Data.AppendU8( 0x30 );
          Data.AppendU8( 0x42 );
          checkBit ^= 0;
        }
      }

      if ( checkBit == 1 )
      {
        // bit set
        Data.AppendU8( 0x42 );
        Data.AppendU8( 0x30 );
      }
      else
      {
        Data.AppendU8( 0x30 );
        Data.AppendU8( 0x42 );
      }
      CheckSum ^= Value;
    }



    private void AppendSync( GR.Memory.ByteBuffer Data )
    {
      /*
      Sync
      It consists in a sync train (9 bytes). 

      Both HEADER and DATA blocks have the following sequence: 

          $89 $88 $87 $86 $85 $84 $83 $82 $81
      Both HEADER REPEATED and DATA REPEATED blocks have the same sequence with bit 7 clear: 

          $09 $08 $07 $06 $05 $04 $03 $02 $01
       */
      AppendByte( Data, 0x89 );
      AppendByte( Data, 0x88 );
      AppendByte( Data, 0x87 );
      AppendByte( Data, 0x86 );
      AppendByte( Data, 0x85 );
      AppendByte( Data, 0x84 );
      AppendByte( Data, 0x83 );
      AppendByte( Data, 0x82 );
      AppendByte( Data, 0x81 );
    }



    private void AppendSyncRepeated( GR.Memory.ByteBuffer Data )
    {
      /*
      Sync
      It consists in a sync train (9 bytes). 

      Both HEADER and DATA blocks have the following sequence: 

          $89 $88 $87 $86 $85 $84 $83 $82 $81
      Both HEADER REPEATED and DATA REPEATED blocks have the same sequence with bit 7 clear: 

          $09 $08 $07 $06 $05 $04 $03 $02 $01
       */
      AppendByte( Data, 0x09 );
      AppendByte( Data, 0x08 );
      AppendByte( Data, 0x07 );
      AppendByte( Data, 0x06 );
      AppendByte( Data, 0x05 );
      AppendByte( Data, 0x04 );
      AppendByte( Data, 0x03 );
      AppendByte( Data, 0x02 );
      AppendByte( Data, 0x01 );
    }



    public override GR.Memory.ByteBuffer Compile()
    {
      _LastError = "";
      GR.Memory.ByteBuffer result = new GR.Memory.ByteBuffer();

      // C64-TAPE-RAW
      result.AppendHex( "4336342D544150452D524157" );
      // version (0 or 1)
      result.AppendU8( 0 );
      // reserved
      result.AppendU8( 0 );
      result.AppendU8( 0 );
      result.AppendU8( 0 );
      // size
      result.AppendU32( 0 );


      foreach ( FileEntry file in TapFiles )
      {
        AppendFile( result, file );
      }

      result.SetU32At( 16, result.Length - 20 );
      return result;
    }



    void AppendHeader( GR.Memory.ByteBuffer Buffer, FileEntry File )
    {
      _LastError = "";
      // For any HEADER the following information is sent after the sync sequence:
      /*
      1 Byte   : File type.

        $01= relocatable program
        $02= Data block for SEQ file
        $03= non-relocatable program
        $04= SEQ file header
        $05= End-of-tape marker

      Here starts what I refer to as HEADER "payload".
      In case File type is not $02, the following bytes have this meaning:

        2 Bytes  : Start Address (LSBF).
        2 Bytes  : End Address+1 (LSBF).
        16 Bytes : File Name (PETSCII format, padded with blanks).

      When File type is $02, SEQ file data starts immediately after File Type thus
      allowing the use of those 20 bytes to store additional data.

      After the File Name there is HEADER "body": 171 bytes, often used by commercial
      loaders to store executable loader code or any additional data and code the
      loader or program may require.
      It encapsulates Data for segmented SEQ files too, as discussed before.

      The default behaviour of the Kernal SAVE command is to pad the File Name with
      blanks so that the total length of the name portion equals 187 bytes.

      Last Byte: Data checkbyte, computed as:

        0 XOR all other HEADER bytes, from "File type" to end of "body".

      After the checkbyte there may or may not be an "end-of-data marker".
      */
      CheckSum = 0;
      // file type
      AppendByte( Buffer, 0x03 );
      // start address
      AppendByte( Buffer, (byte)( File.StartAddress & 0x00ff ) );
      AppendByte( Buffer, (byte)( ( File.StartAddress & 0xff00 ) >> 8 ) );
      // end address + 1
      AppendByte( Buffer, (byte)( ( File.EndAddress + 1 ) & 0x00ff ) );
      AppendByte( Buffer, (byte)( ( ( File.EndAddress + 1 ) & 0xff00 ) >> 8 ) );
      // file name
      for ( int i = 0; i < 16; ++i )
      {
        AppendByte( Buffer, File.Filename.ByteAt( i ) );
      }

      // pad with blanks
      for ( int i = 0; i < 171; ++i )
      {
        AppendByte( Buffer, 0xa0 );
      }
      AppendByte( Buffer, CheckSum );
      AppendEOD( Buffer );
    }



    void AppendData( GR.Memory.ByteBuffer Buffer, GR.Memory.ByteBuffer Data )
    {
      _LastError = "";
      /*
      For any DATA the following information is sent after the sync sequence:

      DATA body

      Last Byte: Data checkbyte, computed as:

        0 XOR all DATA "body" bytes.

      After the checkbyte there may or may not be an "end-of-data marker".

      */
      CheckSum = 0;
      for ( int i = 0; i < Data.Length; ++i )
      {
        AppendByte( Buffer, Data.ByteAt( i ) );
      }
      AppendByte( Buffer, CheckSum );
      AppendEOD( Buffer );
    }



    void AppendEOD( GR.Memory.ByteBuffer Buffer )
    {
      _LastError = "";
      // end of data marker
      Buffer.AppendU8( 0x56 );
      Buffer.AppendU8( 0x30 );
    }



    bool AppendFile( GR.Memory.ByteBuffer Buffer, FileEntry File )
    {
      _LastError = "";

      Buffer.Reserve( (int)Buffer.Length + 3 * 27136 + 4 * 188 * 20 + 2 * (int)File.Data.Length * 20 );
      // leading silence, 27136 bytes 0x30 (approx. 10 seconds)
      Buffer.AppendRepeated( 0x30, 27136 );
      //Buffer.AppendU32NetworkOrder( 0x000180DC );

      // Sync for header
      AppendSync( Buffer );
      AppendHeader( Buffer, File );

      // interblock gap
      Buffer.AppendRepeated( 0x30, 0x4f );

      // Sync for header repeated
      AppendSyncRepeated( Buffer );
      AppendHeader( Buffer, File );

      // trailer
      Buffer.AppendRepeated( 0x30, 0x4e );


      // leading silence, 27136 bytes 0x30 (approx. 10 seconds)
      Buffer.AppendRepeated( 0x30, 27136 );
      //Buffer.AppendU32NetworkOrder( 0x000180DC );

      // Sync
      AppendSync( Buffer );
      AppendData( Buffer, File.Data );

      // interblock gap
      Buffer.AppendRepeated( 0x30, 0x4f );


      // Sync for header repeated
      AppendSyncRepeated( Buffer );
      AppendData( Buffer, File.Data );

      // trailer
      Buffer.AppendRepeated( 0x30, 0x4e );


      /*
      // data trailer

      // leading silence, 27136 bytes 0x30 (approx. 10 seconds)
      Buffer.AppendRepeated( 0x30, 27136 );

      // Sync for header
      AppendSync( Buffer );
      AppendHeader( Buffer, File );

      // interblock gap
      Buffer.AppendRepeated( 0x30, 0x4f );

      // Sync for header repeated
      AppendSyncRepeated( Buffer );
      AppendHeader( Buffer, File );

      // trailer
      Buffer.AppendRepeated( 0x30, 0x4e );
       */
      return true;      
    }



    public override void CreateEmptyMedia()
    {
      _LastError = "";
      TapFiles.Clear();
    }



    public override void Clear()
    {
      _LastError = "";
      TapFiles.Clear();
    }



    public override List<Types.FileInfo> Files()
    {
      _LastError = "";
      var files = new List<Types.FileInfo>();
      int dirEntryIndex = 0;

      foreach ( FileEntry file in TapFiles )
      {
        var info = new Types.FileInfo();

        info.Filename = new GR.Memory.ByteBuffer( file.Filename );
        info.Blocks = (int)( file.Data.Length + 253 ) / 254;
        info.Type = Types.FileType.PRG;
        info.DirEntryIndex = dirEntryIndex;
        ++dirEntryIndex;

        files.Add( info );
      }
      return files;
    }



    public override bool Load( string Filename )
    {
      _LastError = "loading not supported";
      return false;
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
      var fileInfo = new Types.FileInfo();
      foreach ( FileEntry file in TapFiles )
      {
        if ( file.Filename.Compare( Filename ) == 0 )
        {
          fileInfo.Filename = new GR.Memory.ByteBuffer( file.Filename );
          fileInfo.Data = file.Data;
          fileInfo.Type = Types.FileType.PRG;
          return fileInfo;
        }
      }
      _LastError = "file not found";
      return null;
    }



    public override bool WriteFile( GR.Memory.ByteBuffer Filename, GR.Memory.ByteBuffer Content, Types.FileType Type )
    {
      _LastError = "";
      FileEntry   file = new FileEntry();

      file.Filename     = new GR.Memory.ByteBuffer( Filename );
      file.Data         = new GR.Memory.ByteBuffer( Content );
      if ( Type == Types.FileType.PRG )
      {
        if ( Content.Length >= 2 )
        {
          file.StartAddress = Content.UInt16At( 0 );
          file.Data = Content.SubBuffer( 2 );
        }
      }
      file.EndAddress = (ushort)( file.StartAddress + file.Data.Length - 1 );
      if ( file.StartAddress + file.Data.Length >= 65536 )
      {
        _LastError = "file size too large";
        return false;
      }
      TapFiles.Add( file );
      return true;
    }



    public override bool RenameFile( GR.Memory.ByteBuffer Filename, GR.Memory.ByteBuffer NewFilename )
    {
      _LastError = "";
      foreach ( FileEntry file in TapFiles )
      {
        if ( file.Filename.Compare( Filename ) == 0 )
        {
          NewFilename.CopyTo( file.Filename, 0, 16 );
          return true;
        }
      }
      _LastError = "file not found";
      return false;
    }



    public override bool DeleteFile( GR.Memory.ByteBuffer Filename )
    {
      _LastError = "";
      foreach ( FileEntry file in TapFiles )
      {
        if ( file.Filename.Compare( Filename ) == 0 )
        {
          TapFiles.Remove( file );
          return true;
        }
      }
      _LastError = "file not found";
      return false;
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
        return MediaFilenameType.COMMODORE;
      }
    }




  }
}
