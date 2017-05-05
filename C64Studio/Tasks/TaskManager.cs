using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace C64Studio.Tasks
{
  public class TaskManager
  {
    private List<Tasks.Task>      m_TaskQueue = new List<Tasks.Task>();

    private BackgroundWorker      m_BackgroundWorker = new BackgroundWorker();

    private StudioCore            Core;



    public TaskManager( StudioCore Core )
    {
      this.Core = Core;
      m_BackgroundWorker.DoWork += m_BackgroundWorker_DoWork;
    }



    void m_BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      while ( true )
      {
        Tasks.Task    newTask = null;
        lock ( m_TaskQueue )
        {
          if ( m_TaskQueue.Count == 0 )
          {
            return;
          }
          newTask = m_TaskQueue[0];
          m_TaskQueue.RemoveAt( 0 );
        }
        newTask.RunTask();
      }
    }



    public void AddTask( Tasks.Task Task )
    {
      Task.Core = Core;
      lock ( m_TaskQueue )
      {
        m_TaskQueue.Add( Task );
      }
      if ( !m_BackgroundWorker.IsBusy )
      {
        m_BackgroundWorker.RunWorkerAsync();
      }
    }

  }
}
