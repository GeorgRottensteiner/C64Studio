using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio.Formats
{
  public abstract class MediaFormat
  {
    public abstract void                  CreateEmptyMedia();
    public abstract GR.Memory.ByteBuffer  Compile();
    public abstract void                  Clear();
    public abstract List<C64Studio.Types.FileInfo> Files();
    public abstract bool Load( string Filename );
    public abstract bool Save( string Filename );

    public abstract Types.FileInfo LoadFile( GR.Memory.ByteBuffer Filename );
    public abstract bool WriteFile( GR.Memory.ByteBuffer Filename, GR.Memory.ByteBuffer Content, C64Studio.Types.FileType Type );
    public abstract bool DeleteFile( GR.Memory.ByteBuffer Filename );
    public abstract bool RenameFile( GR.Memory.ByteBuffer Filename, GR.Memory.ByteBuffer NewFilename );



    public abstract string LastError
    {
      get;
    }



    public virtual int FreeSlots
    {
      get
      {
        return 0;
      }
    }



    public virtual int Slots
    {
      get
      {
        return 0;
      }
    }



    public virtual string FileFilter
    {
      get
      {
        return "All Files|*.*";
      }
    }



    public virtual GR.Memory.ByteBuffer Title
    {
      get
      {
        return new GR.Memory.ByteBuffer();
      }
    }

  }
}
