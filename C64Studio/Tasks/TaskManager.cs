﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;



namespace RetroDevStudio.Tasks
{
  public class TaskManager
  {
    private object                m_TaskQueueLock = new object();            
    private List<Tasks.Task>      m_TaskQueue = new List<Tasks.Task>();

    private BackgroundWorker      m_BackgroundWorker = new BackgroundWorker();

    private StudioCore            Core;



    public TaskManager( StudioCore Core )
    {
      this.Core = Core;
      m_BackgroundWorker.DoWork += m_BackgroundWorker_DoWork;
    }



    void m_BackgroundWorker_DoWork( object sender, DoWorkEventArgs e )
    {
      while ( !Core.ShuttingDown )
      {
        Tasks.Task    newTask = null;
        lock ( m_TaskQueueLock )
        {
          if ( m_TaskQueue.Count == 0 )
          {
            m_BackgroundWorker = null;
            return;
          }
          newTask = m_TaskQueue[0];
          m_TaskQueue.RemoveAt( 0 );
        }
        try
        {
          newTask.RunTask();
        }
        catch ( Exception ex )
        {
          Debug.Log( "Task " + newTask + " had an exception: " + ex.Message );
        }
      }
    }



    public void AddTask( Tasks.Task Task )
    {
      Task.Core = Core;
      lock ( m_TaskQueueLock )
      {
        var tasksToRemove = new List<Task>();

        // remove duplicate update keyword tasks!
        foreach ( var task in m_TaskQueue )
        {
          if ( ( task is TaskUpdateKeywords )
          &&   ( Task is TaskUpdateKeywords ) )
          {
            var  oldTask  = task as TaskUpdateKeywords;
            var  newTask  = Task as TaskUpdateKeywords;

            if ( oldTask.Doc == newTask.Doc )
            {
              tasksToRemove.Add( oldTask );
            }
          }
        }
        foreach ( var taskToRemove in tasksToRemove )
        {
          m_TaskQueue.Remove( taskToRemove );
        }

        m_TaskQueue.Add( Task );
        if ( ( m_BackgroundWorker == null )
        ||   ( !m_BackgroundWorker.IsBusy ) )
        {
          m_BackgroundWorker = new BackgroundWorker();
          m_BackgroundWorker.DoWork += m_BackgroundWorker_DoWork;
          m_BackgroundWorker.RunWorkerAsync();
        }
      }
    }

  }
}
