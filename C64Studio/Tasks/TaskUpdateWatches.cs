using RetroDevStudio.Parser;
using System;
using System.Collections.Generic;
using System.Text;

namespace RetroDevStudio.Tasks
{
  public class TaskUpdateWatches : Task
  {
    private Types.ASM.FileInfo _asmFileInfo;



    public TaskUpdateWatches( Types.ASM.FileInfo fileInfo )
    {
      _asmFileInfo = fileInfo;
    }



    protected override bool ProcessTask()
    {
      Core.MainForm.m_DebugWatch.UpdateValues( _asmFileInfo );
      return true;
    }
  }
}
