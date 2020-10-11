using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio.Undo
{
  public abstract class UndoTask
  {
    public UndoTask()
    {
      UndoGroup = 0;
    }



    public int UndoGroup
    {
      get;
      set;
    }



    public virtual string Description
    {
      get;
    }



    public abstract UndoTask CreateComplementaryTask();
    public abstract void Apply();
  }
}
