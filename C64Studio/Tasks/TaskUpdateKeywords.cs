﻿using RetroDevStudio.Documents;



namespace RetroDevStudio.Tasks
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
      if ( Doc.DocumentInfo.HasCustomBuild )
      {
        return true;
      }
      Doc.OnKnownKeywordsChanged();

      if ( Doc.InvokeRequired )
      {
        return (bool)Doc.Invoke( new ProcessTaskCallback( ProcessTaskPart2 ) );
      }
      //Debug.Log( "Update keywords for " + Doc.DocumentFilename );*/

      //Debug.Log( "Update keywords done for " + Doc.DocumentFilename );
      return true;
    }



    protected bool ProcessTaskPart2()
    {
      Doc.OnKnownTokensChanged();
      //Debug.Log( "Update keywords done for " + Doc.DocumentFilename );
      return true;
    }



  }
}
