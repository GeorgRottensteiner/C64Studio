using System;
using System.Collections.Generic;
using System.Text;

namespace RetroDevStudio.Tasks
{
  public class TaskRefreshSourceControlState : Task
  {
    DocumentInfo        _document = null;



    public TaskRefreshSourceControlState( DocumentInfo doc )
    {
      _document = doc;
    }



    protected override bool ProcessTask()
    {
      if ( ( _document.Project != null )
      &&   ( _document.Project.SourceControl != null ) )
      {
        var modifiedLines = _document.Project.SourceControl.ListModifiedLines( _document.DocumentFilename );

        _document.OnMarkModifiedLines( modifiedLines );
      }
      return true;
    }
  }
}
