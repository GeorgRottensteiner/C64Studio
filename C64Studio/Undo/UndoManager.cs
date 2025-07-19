using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RetroDevStudio.Undo
{
  public class UndoManager
  {
    private List<UndoTask>      UndoTasks = new List<UndoTask>();
    private List<UndoTask>      RedoTasks = new List<UndoTask>();
    private int                 CurrentUndoGroup = 0;
    public MainForm             MainForm = null;


    public bool CanUndo
    {
      get
      {
        return UndoTasks.Count > 0;
      }
    }



    public bool CanRedo
    {
      get
      {
        return RedoTasks.Count > 0;
      }
    }



    public string UndoInfo
    {
      get
      {
        if ( UndoTasks.Count == 0 )
        {
          return "Undo";
        }
        return "Undo " + UndoTasks[UndoTasks.Count - 1].Description;
      }
    }



    public string RedoInfo
    {
      get
      {
        if ( RedoTasks.Count == 0 )
        {
          return "Redo";
        }
        return "Redo " + RedoTasks[RedoTasks.Count - 1].Description;
      }
    }



    public bool InsideUndoRedo
    {
      get;
      internal set;
    } = false;



    public void Undo()
    {
      if ( UndoTasks.Count == 0 )
      {
        return;
      }
      InsideUndoRedo = true;
      int   currentGroup = UndoTasks[UndoTasks.Count - 1].UndoGroup;

      while ( ( UndoTasks.Count > 0 )
      &&      ( UndoTasks[UndoTasks.Count - 1].UndoGroup == currentGroup ) )
      {
        var task = UndoTasks[UndoTasks.Count - 1];

        var redoTask = UndoTasks[UndoTasks.Count - 1].CreateComplementaryTask();
        redoTask.UndoGroup = currentGroup;
        RedoTasks.Add( redoTask );
        UndoTasks.RemoveAt( UndoTasks.Count - 1 );

        task.Apply();
      }
      InsideUndoRedo = false;
      MainForm.UpdateUndoSettings();
    }



    public void Redo()
    {
      if ( RedoTasks.Count == 0 )
      {
        return;
      }

      int   currentGroup = RedoTasks[RedoTasks.Count - 1].UndoGroup;
      InsideUndoRedo = true;
      while ( ( RedoTasks.Count > 0 )
      &&      ( RedoTasks[RedoTasks.Count - 1].UndoGroup == currentGroup ) )
      {
        var task = RedoTasks[RedoTasks.Count - 1];

        var undoTask = RedoTasks[RedoTasks.Count - 1].CreateComplementaryTask();
        undoTask.UndoGroup = currentGroup;
        UndoTasks.Add( undoTask );
        RedoTasks.RemoveAt( RedoTasks.Count - 1 );

        task.Apply();
      }
      InsideUndoRedo = false;
      MainForm.UpdateUndoSettings();
    }



    public void StartUndoGroup()
    {
      ++CurrentUndoGroup;
    }



    public int NumUndos
    {
      get
      {
        return UndoTasks.Count;
      }
    }



    public int NumRedos
    {
      get
      {
        return RedoTasks.Count;
      }
    }



    public void AddUndoTask( UndoTask Task )
    {
      AddUndoTask( Task, true );
    }



    public void AddUndoTask( UndoTask Task, bool FirstUndoStep )
    {
      if ( FirstUndoStep )
      {
        ++CurrentUndoGroup;
      }
      Task.UndoGroup = CurrentUndoGroup;
      UndoTasks.Add( Task );
      RedoTasks.Clear();
      MainForm.UpdateUndoSettings();
    }



    public void AddGroupedUndoTask( UndoTask Task )
    {
      AddUndoTask( Task, false );
    }



    public void Clear()
    {
      UndoTasks.Clear();
      RedoTasks.Clear();
    }



  }
}
