using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace RetroDevStudio.Formats
{
  public abstract class MediaFormat
  {
    public abstract void                  CreateEmptyMedia();
    public abstract GR.Memory.ByteBuffer  Compile();
    public abstract void                  Clear();
    public abstract List<RetroDevStudio.Types.FileInfo> Files();
    public abstract bool Load( string Filename );
    public abstract bool Save( string Filename );

    public abstract Types.FileInfo LoadFile( GR.Memory.ByteBuffer Filename );
    public abstract bool WriteFile( GR.Memory.ByteBuffer Filename, GR.Memory.ByteBuffer Content, RetroDevStudio.Types.FileType Type );
    public abstract bool DeleteFile( GR.Memory.ByteBuffer Filename, bool CompleteDelete = true );
    public abstract bool RenameFile( GR.Memory.ByteBuffer Filename, GR.Memory.ByteBuffer NewFilename );



    protected MediaFormat()
    {
      SupportsRenamingTitle = false;
    }



    public virtual void Validate()
    {
    }



    public abstract string LastError
    {
      get;
    }



    public virtual bool SupportsFolders
    {
      get
      {
        return false;
      }
    }



    public string CurrentFolder { get; set; } = "";

    public string RootFolder { get; protected set; } = "";



    public virtual bool ChangeDirectory( GR.Memory.ByteBuffer DirName )
    {
      return false;
    }



    public virtual bool ChangeDirectoryUp()
    {
      return false;
    }



    public abstract MediaFilenameType FilenameType 
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



    public bool SupportsRenamingTitle 
    { 
      get; internal set; 
    }



    public virtual void ChangeFileType( FileInfo FileToChange, FileType NewFileType )
    {
      FileToChange.Type = NewFileType;
    }

  }
}
