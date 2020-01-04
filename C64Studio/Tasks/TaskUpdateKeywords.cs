using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio.Tasks
{
  public class TaskUpdateKeywords : Task
  {
    public BaseDocument      Doc;



    public TaskUpdateKeywords( BaseDocument Document )
    {
      Doc = Document;
    }



    protected override bool ProcessTask()
    {
      if ( Doc == null )
      {
        return false;
      }
      if ( Doc.InvokeRequired )
      {
        return (bool)Doc.Invoke( new ProcessTaskCallback( ProcessTask ) );
      }
      Debug.Log( "ProcessTask TaskUpdateKeywords for " + Doc.DocumentInfo.FullPath );
      Doc.OnKnownKeywordsChanged();
      Doc.OnKnownTokensChanged();
      Debug.Log( "ProcessTask TaskUpdateKeywords for " + Doc.DocumentInfo.FullPath + " done" );
      return true;
    }



  }
}
