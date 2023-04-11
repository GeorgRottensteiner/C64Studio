using RetroDevStudio.Documents;



namespace RetroDevStudio.Tasks
{
  public class TaskRefreshOutlineAndLabelExplorer : Task
  {
    private BaseDocument    m_Document;



    public TaskRefreshOutlineAndLabelExplorer( BaseDocument Document )
    {
      m_Document = Document;
    }



    protected override bool ProcessTask()
    {
      Core.MainForm.m_Outline.RefreshFromDocument( m_Document );
      Core.MainForm.m_LabelExplorer.RefreshFromDocument( m_Document );
      return true;
    }
  }
}
