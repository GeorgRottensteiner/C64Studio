using RetroDevStudio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace RetroDevStudio.Formats
{
  [MediaType( MediaType.TAPE )]
  [MediaFormat( MediaFormatType.PRG )]
  [Category( "Commodore" )]
  public class PRG : MediaFormat
  {
    ushort                      LoadAddress = 0x0801;
    GR.Memory.ByteBuffer        Filename = new GR.Memory.ByteBuffer();
    GR.Memory.ByteBuffer        Data = new GR.Memory.ByteBuffer();
    string                      _LastError = "";



    public override GR.Memory.ByteBuffer Compile()
    {
      _LastError = "";
      GR.Memory.ByteBuffer result = new GR.Memory.ByteBuffer();

      result.AppendU16( LoadAddress );
      result.Append( Data );
      return result;
    }



    public override void CreateEmptyMedia()
    {
      _LastError = "";
      Data.Clear();
      LoadAddress = 0x0801;
    }



    public override void Clear()
    {
      _LastError = "";
      Data.Clear();
      LoadAddress = 0x0801;
    }



    public override List<RetroDevStudio.Types.FileInfo> Files()
    {
      _LastError = "";
      var  fileList = new List<RetroDevStudio.Types.FileInfo>();

      var info = new RetroDevStudio.Types.FileInfo();
      info.Filename = Filename;
      info.Blocks   = (int)Data.Length / 254;
      info.Type     = Types.FileType.PRG;
      info.DirEntryIndex = 0;

      fileList.Add( info );
      return fileList;
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
      if ( data.Length < 2 )
      {
        _LastError = "file size is too small";
        return false;
      }
      LoadAddress = data.UInt16At( 0 );
      Data        = new GR.Memory.ByteBuffer( data );
      this.Filename = Util.ToPETSCII( GR.Path.GetFileNameWithoutExtension( Filename ).ToUpper() );
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
      if ( Filename.Compare( this.Filename ) != 0 )
      {
        _LastError = "file not found";
        return null;
      }
      var fileInfo = new Types.FileInfo();
      fileInfo.Type = Types.FileType.PRG;
      fileInfo.Data = new GR.Memory.ByteBuffer( Data );
      fileInfo.Filename = new GR.Memory.ByteBuffer( Filename );

      return fileInfo;
    }



    public override bool WriteFile( GR.Memory.ByteBuffer Filename, GR.Memory.ByteBuffer Content, Types.FileType Type )
    {
      _LastError = "";
      if ( ( Data.Length > 0 )
      ||   ( Type != Types.FileType.PRG ) )
      {
        _LastError = "invalid file type";
        return false;
      }
      Data = new GR.Memory.ByteBuffer( Content );
      this.Filename = Filename;
      return true;
    }



    public override bool RenameFile( GR.Memory.ByteBuffer Filename, GR.Memory.ByteBuffer NewFilename )
    {
      _LastError = "";
      NewFilename.CopyTo( this.Filename, 0, 16 );
      return true;
    }



    public override bool DeleteFile( GR.Memory.ByteBuffer Filename, bool CompleteDelete )
    {
      _LastError = "";
      if ( Filename.Compare( this.Filename ) != 0 )
      {
        _LastError = "file not found";
        return false;
      }
      Data.Clear();
      return true;
    }



    public override int FreeSlots
    {
      get
      {
        if ( Data.Length == 0 )
        {
          return 1;
        }
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
        return "PRG Files|*.PRG|" + base.FileFilter;
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
