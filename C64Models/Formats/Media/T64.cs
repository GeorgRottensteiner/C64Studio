using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace RetroDevStudio.Formats
{
  [MediaType( MediaType.TAPE )]
  [MediaFormat( MediaFormatType.T64 )]
  [Category( "Commodore" )]
  public class T64 : MediaFormat
  {
    /*
    11.1.1 T64 File structure
    Offset Size Description 
    0 64 tape record 
    64 32*n file records for n directory entries 
    64+32*n varies binary contents of the files 

    11.1.2 Tape Record
    Offset Size Description 
    0 32 DOS tape description + EOF (for type) 
    32 2 tape version ($0200) 
    34 2 number of directory entries 
    36 2 number of used entries (can be 0 in my loader) 
    38 2 free 
    40 24 user description as displayed in tape menu 

    11.1.3 File record
    Offset Size Description 
    0 1 entry type (see below) 
    1 1 C64 file type 
    2 2 start address 
    4 2 end address 
    6 2 free 
    8 4 offset of file contents start within T64 file 
    12 4 free 
    16 16 C64 file name 

    Valid entry types are: 

    Code Explanation 
    0 free entry 
    1 normal tape file 
    2 tape file with header: header is saved just before file data 
    3 memory snapshot v0.9, uncompressed 
    4 tape block 
    5 digitized stream 
    6 ... 255 reserved 

    Notes: 

    VICE only supports file type 1. 
    Types 3, 4 and 5 are subject to change (and are rarely used). 
     */

    public class TapeRecord
    {
      public string   Description = "";
      public ushort   Version = 0x0100;
      public ushort   NumberEntries = 0;
      public ushort   NumberUsedEntries = 0;
      public ushort   Filler = 0;
      public string   UserDescription = "";
    };

    public class FileRecord
    {
      public byte     EntryType = 0;
      public Types.FileTypeNative FileTypeNative =  Types.FileTypeNative.COMMODORE_SCRATCHED;
      public ushort   StartAddress = 0;
      public ushort   EndAddress = 0;
      public ushort   Filler = 0;
      public UInt32   FileOffset = 0;
      public UInt32   Filler2 = 0;
      public GR.Memory.ByteBuffer Filename = new GR.Memory.ByteBuffer();
    };

    public TapeRecord                   TapeInfo = new TapeRecord();
    public List<FileRecord>             FileRecords = new List<FileRecord>();
    public List<GR.Memory.ByteBuffer>   FileDatas = new List<GR.Memory.ByteBuffer>();
    private string                      _LastError = "";



    private GR.Memory.ByteBuffer PadString( GR.Memory.ByteBuffer Text, int Digits, byte fillChar )
    {
      GR.Memory.ByteBuffer result = new GR.Memory.ByteBuffer();

      int     useCharCount = Digits;
      if ( Text.Length < useCharCount )
      {
        useCharCount = (int)Text.Length;
      }
      for ( int i = 0; i < useCharCount; ++i )
      {
        result.AppendU8( Text.ByteAt( i ) );
      }
      while ( result.Length < Digits )
      {
        result.AppendU8( fillChar );
      }
      return result;
    }



    private GR.Memory.ByteBuffer PadString( string Text, int Digits, byte fillChar )
    {
      GR.Memory.ByteBuffer result = new GR.Memory.ByteBuffer();

      int useCharCount = Digits;
      if ( Text.Length < useCharCount )
      {
        useCharCount = Text.Length;
      }
      for ( int i = 0; i < useCharCount; ++i )
      {
        result.AppendU8( (byte)Text[i] );
      }
      while ( result.Length < Digits )
      {
        result.AppendU8( fillChar );
      }
      return result;
    }



    public override void CreateEmptyMedia()
    {
      _LastError = "";
      Clear();

      TapeInfo.Description = "C64S tape file\r\nDemo tape";
      for ( int i = 0; i < 30; ++i )
      {
        FileRecords.Add( new FileRecord() );
        FileDatas.Add( new GR.Memory.ByteBuffer() );
      }
    }



    public override GR.Memory.ByteBuffer Compile()
    {
      _LastError = "";

      GR.Memory.ByteBuffer result = new GR.Memory.ByteBuffer();

      // Tape Header
      // 0 32 DOS tape description + EOF (for type) 
      // 32 2 tape version ($0200) 
      // 34 2 number of directory entries 
      // 36 2 number of used entries (can be 0 in my loader) 
      // 38 2 free 
      // 40 24 user description as displayed in tape menu
      int usedEntries = 0;
      for ( int i = 0; i < 30; ++i )
      {
        // File Header
        // Offset Size Description 
        // 0 1 entry type (see below) 
        // 1 1 C64 file type 
        // 2 2 start address 
        // 4 2 end address 
        // 6 2 free 
        // 8 4 offset of file contents start within T64 file 
        // 12 4 free 
        // 16 16 C64 file name 
        if ( ( i >= FileRecords.Count )
        ||   ( FileRecords[i].EntryType == 0 ) )
        {
        }
        else
        {
          ++usedEntries;
        }
      }

      result.Append( PadString( TapeInfo.Description + (char)0x1a, 32, 0x2e ) );
      result.AppendU16( TapeInfo.Version );
      result.AppendU16( 30 );
      result.AppendU16( (ushort)usedEntries );
      result.AppendU16( 0 );
      result.Append( PadString( TapeInfo.UserDescription, 24, 0x20 ) );

      int     completeOffset = 64 + 30 * 32;
      for ( int i = 0; i < 30; ++i )
      {
        // File Header
        // Offset Size Description 
        // 0 1 entry type (see below) 
        // 1 1 C64 file type 
        // 2 2 start address 
        // 4 2 end address 
        // 6 2 free 
        // 8 4 offset of file contents start within T64 file 
        // 12 4 free 
        // 16 16 C64 file name 
        if ( ( i >= FileRecords.Count )
        ||   ( FileRecords[i].EntryType == 0 ) )
        {
          GR.Memory.ByteBuffer    dummy = new GR.Memory.ByteBuffer( 32 );
          result.Append( dummy );
        }
        else
        {
          result.AppendU8( (byte)FileRecords[i].EntryType );
          result.AppendU8( (byte)FileRecords[i].FileTypeNative );
          result.AppendU16( FileRecords[i].StartAddress );
          result.AppendU16( (ushort)( FileRecords[i].StartAddress + FileDatas[i].Length ) );
          result.AppendU16( 0 );
          result.AppendU32( (uint)completeOffset );
          result.AppendU32( 0 );
          result.Append( PadString( FileRecords[i].Filename, 16, 0x20 ) );

          completeOffset += (int)FileDatas[i].Length;
        }
      }
      for ( int i = 0; i < FileRecords.Count; ++i )
      {
        result.Append( FileDatas[i] );
      }
      return result;
    }



    public override void Clear()
    {
      _LastError = "";

      TapeInfo.Description = "";
      TapeInfo.UserDescription = "";
      TapeInfo.NumberEntries = 30;
      TapeInfo.NumberUsedEntries = 0;

      FileRecords.Clear();
    }



    public override List<RetroDevStudio.Types.FileInfo> Files()
    {
      _LastError = "";

      var files = new List<RetroDevStudio.Types.FileInfo>();
      int dirEntryIndex = 0;

      foreach ( FileRecord file in FileRecords )
      {
        if ( file.EntryType == 1 )
        {
          var info = new Types.FileInfo();

          info.Filename   = file.Filename;
          info.Blocks     = ( file.EndAddress - file.StartAddress ) / 254 + 1;
          info.Type       = Types.FileType.FILE;
          info.NativeType = Types.FileTypeNative.COMMODORE_PRG;
          info.ReadOnly   = ( file.FileTypeNative & Types.FileTypeNative.COMMODORE_LOCKED ) != 0;
          info.NotClosed  = ( file.FileTypeNative & Types.FileTypeNative.COMMODORE_CLOSED ) == 0;
          info.DirEntryIndex = dirEntryIndex;
          ++dirEntryIndex;

          files.Add( info );
        }
      }
      return files;
    }



    public override bool Load( string Filename )
    {
      _LastError = "";
      Clear();
      GR.Memory.ByteBuffer data = GR.IO.File.ReadAllBytes( Filename );
      if ( data == null )
      {
        _LastError = "could not open/read file";
        return false;
      }
      if ( data.Length < 64 )
      {
        _LastError = "file size is too small";
        return false;
      }

      // Tape Header
      // 0 32 DOS tape description + EOF (for type) 
      // 32 2 tape version ($0200) 
      // 34 2 number of directory entries 
      // 36 2 number of used entries (can be 0 in my loader) 
      // 38 2 free 
      // 40 24 user description as displayed in tape menu 

      for ( int i = 0; i < 32; ++i )
      {
        if ( data.ByteAt( i ) == (char)0x1a )
        {
          break;
        }
        TapeInfo.Description += (char)data.ByteAt( i );
      }
      ushort version = data.UInt16At( 32 );
      /*
      if ( version != 0x0200 )
      {
        return false;
      }*/
      TapeInfo.NumberEntries      = data.UInt16At( 34 );
      TapeInfo.NumberUsedEntries  = data.UInt16At( 36 );

      for ( int i = 0; i < 24; ++i )
      {
        if ( data.ByteAt( 40 + i ) == (char)0x20 )
        {
          break;
        }
        TapeInfo.UserDescription += (char)data.ByteAt( i );
      }
      int entryPos = 64;
      for ( int i = 0; i < TapeInfo.NumberEntries; ++i )
      {
        // File Header
        // Offset Size Description 
        // 0 1 entry type (see below) 
        // 1 1 C64 file type 
        // 2 2 start address 
        // 4 2 end address 
        // 6 2 free 
        // 8 4 offset of file contents start within T64 file 
        // 12 4 free 
        // 16 16 C64 file name 

        // Code Explanation 
        // 0 free entry 
        // 1 normal tape file 
        // 2 tape file with header: header is saved just before file data 
        // 3 memory snapshot v0.9, uncompressed 
        // 4 tape block 
        // 5 digitized stream 
        // 6 ... 255 reserved 
        FileRecord    file = new FileRecord();

        file.EntryType = data.ByteAt( entryPos + 0 );
        if ( ( file.EntryType != 1 )
        &&   ( file.EntryType != 0 ) )
        {
          // unsupported type!
          _LastError = "unsupported entry type";
          return false;
        }
        if ( file.EntryType == 0 )
        {
          FileRecords.Add( file );
          FileDatas.Add( new GR.Memory.ByteBuffer() );

          entryPos += 32;
          continue;
        }
        file.FileTypeNative = (Types.FileTypeNative)data.ByteAt( entryPos + 1 );
        file.StartAddress   = data.UInt16At( entryPos + 2 );
        file.EndAddress     = data.UInt16At( entryPos + 4 );
        file.FileOffset     = data.UInt32At( entryPos + 8 );
        for ( int j = 0; j < 16; ++j )
        {
          file.Filename.AppendU8( data.ByteAt( entryPos + 16 + j ) );
        }
        FileRecords.Add( file );
        FileDatas.Add( data.SubBuffer( (int)file.FileOffset, (int)( file.EndAddress - file.StartAddress ) ) );
        entryPos += 32;
      }
      return true;
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
      int    fileIndex = 0;
      var    fileInfo = new Types.FileInfo();
      foreach ( FileRecord file in FileRecords )
      {
        if ( file.EntryType == 1 )
        {
          if ( file.Filename == Filename )
          {
            GR.Memory.ByteBuffer exportData = new GR.Memory.ByteBuffer();
            exportData.AppendU16( file.StartAddress );
            exportData.Append( FileDatas[fileIndex] );

            fileInfo.Data       = exportData;
            fileInfo.Filename   = new GR.Memory.ByteBuffer( file.Filename );
            fileInfo.Type       = Types.FileType.FILE;
            fileInfo.NativeType = Types.FileTypeNative.COMMODORE_PRG;
            return fileInfo;
          }
        }
        ++fileIndex;
      }
      _LastError = "file not found";
      return null;
    }



    public override bool WriteFile( GR.Memory.ByteBuffer Filename, GR.Memory.ByteBuffer Content, Types.FileTypeNative Type )
    {
      _LastError = "";

      int fileIndex = 0;
      foreach ( FileRecord file in FileRecords )
      {
        if ( file.EntryType == 0 )
        {
          // free slot found
          file.EntryType      = 1;
          file.FileTypeNative = Type;
          file.StartAddress   = Content.UInt16At( 0 );
          if ( Content.Length < 2 )
          {
            FileDatas[fileIndex] = new GR.Memory.ByteBuffer();
          }
          else
          {
            FileDatas[fileIndex] = Content.SubBuffer( 2 );
          }
          file.EndAddress = (ushort)( file.StartAddress + Content.Length );
          file.FileOffset = 0;
          file.Filename   = Filename;
          return true;
        }
        ++fileIndex;
      }
      _LastError = "tape image is full";
      return false;
    }



    public override bool RenameFile( GR.Memory.ByteBuffer Filename, GR.Memory.ByteBuffer NewFilename )
    {
      _LastError = "";
      int fileIndex = 0;
      foreach ( FileRecord file in FileRecords )
      {
        if ( file.EntryType == 1 )
        {
          if ( file.Filename == Filename )
          {
            NewFilename.CopyTo( file.Filename, 0, 16 );
            return true;
          }
        }
        ++fileIndex;
      }
      _LastError = "file not found";
      return false;
    }



    public override bool DeleteFile( GR.Memory.ByteBuffer Filename, bool CompleteDelete )
    {
      _LastError = "";
      int fileIndex = 0;
      foreach ( FileRecord file in FileRecords )
      {
        if ( file.EntryType == 1 )
        {
          if ( file.Filename == Filename )
          {
            file.EntryType      = 0;
            file.FileTypeNative = Types.FileTypeNative.COMMODORE_SCRATCHED;
            file.StartAddress   = 0;
            file.EndAddress     = 0;
            file.FileOffset     = 0;

            FileDatas[fileIndex] = new GR.Memory.ByteBuffer();
            return true;
          }
        }
        ++fileIndex;
      }
      _LastError = "file not found";
      return false;
    }



    public override int FreeSlots
    {
      get
      {
        int slotCount = 0;
        foreach ( FileRecord file in FileRecords )
        {
          if ( file.EntryType == 0 )
          {
            ++slotCount;
          }
        }
        return slotCount;
      }
    }



    public override int Slots
    {
      get
      {
        return FileRecords.Count;
      }
    }



    public override string FileFilter
    {
      get
      {
        return "Tape Files|*.T64|" + base.FileFilter;
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
        return MediaFilenameType.COMMODORE;
      }
    }



  }
}
