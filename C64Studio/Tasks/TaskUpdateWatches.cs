using System;
using System.Collections.Generic;
using System.Text;

namespace RetroDevStudio.Tasks
{
  public class TaskUpdateWatches : Task
  {
    public TaskUpdateWatches()
    {
    }



    protected override bool ProcessTask()
    {
      Core.MainForm.m_DebugWatch.UpdateValues();
      return true;
    }
  }
}
