using GR.Memory;
using RetroDevStudio;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;



namespace RetroDevStudio.Formats
{
  [MediaType( MediaType.TAPE )]
  [MediaFormat( MediaFormatType.P )]
  [DefaultFileExtension( ".p" )]
  [Category( "Spectrum" )]
  public class SpectrumP : MediaFormat
  {
    string        _LastError = "";
    ByteBuffer    _data = new ByteBuffer();
    string        _fileName = "";



    public override GR.Memory.ByteBuffer Compile()
    {
      _LastError = "";

      return new GR.Memory.ByteBuffer( _data );
    }



    public override void CreateEmptyMedia()
    {
      _LastError = "";
    }



    public override void Clear()
    {
      _LastError = "";
      _data.Clear();
    }



    public override List<RetroDevStudio.Types.FileInfo> Files()
    {
      _LastError = "";
      var  fileList = new List<RetroDevStudio.Types.FileInfo>();

      var info = new RetroDevStudio.Types.FileInfo()
      {
        Size          = (int)_data.Length,
        Type          = FileType.FILE,
        Data          = new ByteBuffer( _data ),
        Filename      = Util.ToFilename( FilenameType, _fileName ),
        DirEntryIndex = 0
      };
      fileList.Add( info );

      info.NativeType = FileTypeNative.SPECTRUM_P;
      info.Info       = GR.EnumHelper.GetDescription( info.NativeType );

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

      _data     = data;
      _fileName = GR.Path.GetFileNameWithoutExtension( Filename );

      return true;
    }



    public override bool Save( string fileName )
    {
      _LastError = "";
      GR.Memory.ByteBuffer data = Compile();
      if ( !GR.IO.File.WriteAllBytes( fileName, data ) )
      {
        return false;
      }
      _fileName = GR.Path.GetFileNameWithoutExtension( fileName );
      return true;
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

      if ( Filename != Util.ToFilename( FilenameType, _fileName ) )
      {
        return false;
      }

      _data = new ByteBuffer( Content );
      return true;
    }



    public override bool RenameFile( GR.Memory.ByteBuffer Filename, GR.Memory.ByteBuffer NewFilename )
    {
      _LastError = "";
      if ( _fileName != Util.FilenameToUnicode( FilenameType, Filename ) )
      {
        _LastError = "file not found";
        return false;
      }
      _fileName  = Util.FilenameToUnicode( FilenameType, NewFilename );
      return true;
    }



    public override bool DeleteFile( GR.Memory.ByteBuffer Filename, bool CompleteDelete )
    {
      if ( _fileName != Util.FilenameToUnicode( FilenameType, Filename ) )
      {
        _LastError = "file not found";
        return false;
      }
      _LastError = "Cannot delete single file";
      return false;
    }



    public override int FreeSlots
    {
      get
      {
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
        return "P Files|*.P|" + base.FileFilter;
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
