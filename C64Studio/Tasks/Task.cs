using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio.Tasks
{
  public class Task
  {
    protected delegate bool ProcessTaskCallback();



    public enum TaskType
    {
      INVALID,
      PARSE_FILE,
      OPEN_SOLUTION
    }

    public TaskType     Type = TaskType.INVALID;
    public string       Description = "";
    public bool         TaskSuccessful = false;
    public StudioCore   Core = null;



    public delegate void delTaskFinished( Task FinishedTask );

    public event delTaskFinished TaskFinished;



    public Task()
    {
    }



    public Task( TaskType Type, string Description )
    {
      this.Type         = Type;
      this.Description  = Description;
    }



    public void RunTask()
    {
      try
      {
        TaskSuccessful = ProcessTask();
      }
      catch ( Exception ex )
      {
        Core.AddToOutput( "An exception occurred: " + ex.Message );
      }
      if ( TaskFinished != null )
      {
        TaskFinished( this );
      }
    }



    protected virtual bool ProcessTask()
    {
      return true;
    }

  }
}
