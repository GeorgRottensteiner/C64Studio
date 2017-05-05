using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio.Tasks
{
  public class TaskUpdateKeywords : Task
  {
    private BaseDocument      m_Doc;



    public TaskUpdateKeywords( BaseDocument Document )
    {
      m_Doc = Document;
    }



    protected override bool ProcessTask()
    {
      if ( m_Doc.InvokeRequired )
      {
        return (bool)m_Doc.Invoke( new ProcessTaskCallback( ProcessTask ) );
      }
      m_Doc.OnKnownKeywordsChanged();
      m_Doc.OnKnownTokensChanged();
      return true;
    }



  }
}
