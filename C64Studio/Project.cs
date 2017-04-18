using System;
using System.Collections.Generic;
using System.Text;
using WeifenLuo.WinFormsUI.Docking;

namespace C64Studio
{
  public class Project
  {
    public ProjectSettings    Settings = new ProjectSettings();
    public MainForm           MainForm = null;
    private bool              m_Modified = false;


    public System.Collections.Generic.LinkedList<ProjectElement>   Elements = new LinkedList<ProjectElement>();


    public bool Modified
    {
      get
      {
        return m_Modified;
      }
    }

    public bool         Save( string Filename )
    {
      GR.Memory.ByteBuffer bufferProject = new GR.Memory.ByteBuffer();

      GR.IO.FileChunk       chunkProject = new GR.IO.FileChunk( Types.FileChunk.PROJECT );

      chunkProject.AppendU32( 1 );
      chunkProject.AppendString( Settings.Name );
      chunkProject.AppendString( Filename );
      chunkProject.AppendU16( Settings.DebugPort );
      chunkProject.AppendU16( Settings.DebugStartAddress );
      chunkProject.AppendString( Settings.BuildTool );
      chunkProject.AppendString( Settings.RunTool );
      chunkProject.AppendString( Settings.MainDocument );

      bufferProject.Append( chunkProject.ToBuffer() );

      foreach ( ProjectElement element in Elements )
      {
        GR.IO.FileChunk chunkElement = new GR.IO.FileChunk( Types.FileChunk.PROJECT_ELEMENT );

        chunkElement.AppendU32( 1 );
        chunkElement.AppendU32( (uint)element.Type );
        chunkElement.AppendString( element.Name );
        chunkElement.AppendString( element.Filename );

        GR.IO.FileChunk chunkElementData = new GR.IO.FileChunk( Types.FileChunk.PROJECT_ELEMENT_DATA );

        element.Document.SaveToChunk( chunkElementData );

        chunkElement.Append( chunkElementData.ToBuffer() );
        chunkElement.AppendString( element.TargetFilename );
        chunkElement.AppendU32( (uint)element.TargetType );

        bufferProject.Append( chunkElement.ToBuffer() );
      }

      try
      {
        System.IO.File.WriteAllBytes( Filename, bufferProject.Data() );
      }
      catch ( System.IO.IOException )
      {
        return false;
      }
      m_Modified = false;
      return true;
    }



    public bool Load( string Filename )
    {
      byte[] projectData = null;
      try
      {
        projectData = System.IO.File.ReadAllBytes( Filename );
      }
      catch ( System.IO.IOException )
      {
        return false;
      }

      Settings.Filename = Filename;
      Settings.BasePath = System.IO.Path.GetDirectoryName( Settings.Filename );

      GR.IO.MemoryReader memIn = new GR.IO.MemoryReader( projectData );

      GR.IO.FileChunk           chunk = new GR.IO.FileChunk();

      while ( chunk.ReadFromStream( memIn ) )
      {
        GR.IO.MemoryReader      memChunk = chunk.MemoryReader();
        switch ( chunk.Type )
        {
          case Types.FileChunk.PROJECT:
            // Project Info

            // Version
            memChunk.ReadUInt32();
            Settings.Name = memChunk.ReadString();
            Settings.Filename = memChunk.ReadString();
            Settings.DebugPort = memChunk.ReadUInt16();
            Settings.DebugStartAddress = memChunk.ReadUInt16();
            Settings.BuildTool = memChunk.ReadString();
            Settings.RunTool = memChunk.ReadString();
            Settings.MainDocument = memChunk.ReadString();
            break;
          case Types.FileChunk.PROJECT_ELEMENT:
            // Element Info
            {
              // Version
              int version = (int)memChunk.ReadUInt32();

              ProjectElement.ElementType type = (ProjectElement.ElementType)memChunk.ReadUInt32();

              ProjectElement element = CreateElement( type );
              element.Name = memChunk.ReadString();
              element.Filename = memChunk.ReadString();
              ShowDocument( element );

              GR.IO.FileChunk           subChunk = new GR.IO.FileChunk();

              if ( !subChunk.ReadFromStream( memChunk ) )
              {
                return false;
              }
              if ( subChunk.Type != Types.FileChunk.PROJECT_ELEMENT_DATA )
              {
                return false;
              }
              // Element Data
              if ( element.Document != null )
              {
                if ( !element.Document.ReadFromReader( subChunk.MemoryReader() ) )
                {
                  Elements.Remove( element );
                  element.Document.Dispose();
                  element = null;
                }
              }
              element.TargetFilename = memChunk.ReadString();
              element.TargetType = (FileParser.CompileTargetType)memChunk.ReadUInt32();

              if ( ( element != null )
              &&   ( element.Document != null ) )
              {
                element.Document.ShowHint = DockState.Document;
                element.Document.Show( MainForm.panelMain );
              }
            }
            break;
        }
      }
      Settings.Filename = Filename;
      m_Modified = false;
      return true;
    }



    public BaseDocument CreateDocument( ProjectElement.ElementType Type )
    {
      BaseDocument document = null;

      switch ( Type )
      {
        case ProjectElement.ElementType.ASM_SOURCE:
          document = new SourceASM( MainForm );
          break;
      }

      return document;
    }



    public BaseDocument ShowDocument( ProjectElement Element )
    {
      if ( Element.Document == null )
      {
        BaseDocument    document = CreateDocument( Element.Type );

        Element.Document = document;
        Element.Document.ShowHint = DockState.Document;
        document.SetProjectElement( Element );
        document.MainForm = MainForm;
        document.SetDocumentFilename( Element.Filename );
        if ( document.DocumentFilename == null )
        {
          // a new file
          Element.Name = Element.Name;
          Element.Document.Show( MainForm.panelMain );
        }
        else if ( document.Load() )
        {
          Element.Name = Element.Name;
          Element.Document.Show( MainForm.panelMain );
        }
        else
        {
          Element.Document = null;
          return null;
        }
      }
      Element.Document.Select();
      return Element.Document;
    }



    public ProjectElement CreateElement( ProjectElement.ElementType Type )
    {
      ProjectElement    element = new ProjectElement();
      element.Type      = Type;
      element.Node      = new System.Windows.Forms.TreeNode();
      element.Node.Tag  = element;
      MainForm.m_ProjectExplorer.NodeProject.Nodes.Add( element.Node );
      element.Node.Parent.Expand();

      Elements.AddLast( element );

      m_Modified = true;
      return element;
    }



    public void RemoveElement( ProjectElement Element )
    {
      Elements.Remove( Element );
      m_Modified = true;
    }

  }
}
