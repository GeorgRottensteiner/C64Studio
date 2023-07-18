using System;
using System.Collections.Generic;
using System.Text;

namespace RetroDevStudio.Types
{
  public class ApplicationEvent
  {
    public enum Type
    {
      NONE = 0,
      DOCUMENT_CREATED,
      DOCUMENT_OPENED,
      DOCUMENT_CLOSED,
      DOCUMENT_ACTIVATED,
      DOCUMENT_FILENAME_CHANGED,
      DOCUMENT_SAVED,
      ELEMENT_CREATED,
      ELEMENT_OPENED,
      ELEMENT_CLOSED,
      ELEMENT_REMOVED,
      ELEMENT_ADDED,
      ACTIVE_PROJECT_CHANGED,
      SOLUTION_OPENED,
      SOLUTION_CLOSED,
      SOLUTION_SAVED,
      SOLUTION_RENAMED,
      DOCUMENT_INFO_CREATED,
      DOCUMENT_INFO_REMOVED,
      EMULATOR_LIST_CHANGED,
      KEY_BINDINGS_MODIFIED,
      PROJECT_RENAMED,
      SETTING_MODIFIED,         // Originalvalue = name of modified setting
      PROJECT_OPENED,
      PROJECT_CLOSED,
      SOURCE_CONTROL_STATE_MODIFIED
    }

    public Type             EventType = Type.NONE;
    public DocumentInfo     Doc = null;
    public ProjectElement   Element = null;
    public Project          Project = null;
    public string           OriginalValue = "";
    public string           UpdatedValue = "";



    public ApplicationEvent( Type EventType )
    {
      this.EventType = EventType;
    }



    public ApplicationEvent( Type EventType, DocumentInfo Doc )
    {
      this.EventType = EventType;
      this.Doc = Doc;
      
      if ( Doc != null )
      {
        Project = Doc.Project;
        if ( Doc.Element != null )
        {
          this.Element = Doc.Element;
        }
      }
    }



    public ApplicationEvent( Type EventType, ProjectElement Element )
    {
      this.EventType = EventType;
      this.Element  = Element;
      if ( Element != null )
      {
        this.Project  = Element.DocumentInfo.Project;
        this.Doc      = Element.DocumentInfo;
      }
    }



    public ApplicationEvent( Type EventType, Project Project )
    {
      this.EventType = EventType;
      this.Project = Project;
    }
  }
}
