using System;
using System.Collections.Generic;
using System.Text;

namespace RetroDevStudio.Undo
{
  public abstract class UndoTask
  {
    protected DocumentInfo      ParentDocument = null;



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



    public void MarkParentAsModified()
    {
      ParentDocument?.BaseDoc?.SetModified();
    }



  }
}
