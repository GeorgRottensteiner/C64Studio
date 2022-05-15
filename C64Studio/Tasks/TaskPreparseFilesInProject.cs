using System;
using System.Collections.Generic;
using System.Text;

namespace RetroDevStudio.Tasks
{
  public class TaskPreparseFilesInProject : Task
  {
    private Project       m_Project;
    private string        m_SelectedConfig;



    public TaskPreparseFilesInProject( Project Project, string SelectedConfig )
    {
      m_Project = Project;
      m_SelectedConfig = SelectedConfig;
    }



    protected override bool ProcessTask()
    {
      Core.MainForm.PreparseFilesInProject( m_Project, m_SelectedConfig );
      return true;
    }
  }
}
