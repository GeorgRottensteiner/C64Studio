using RetroDevStudio.Parser;
using System;
using System.Collections.Generic;
using System.Text;
using static RetroDevStudio.Parser.ParserBase;



namespace RetroDevStudio.Tasks
{
  public class TaskUpdateCompileResult : Task
  {
    private DocumentInfo        m_Doc;
    private Types.ASM.FileInfo  m_ASMFileInfo;



    public TaskUpdateCompileResult( Types.ASM.FileInfo ASMFileInfo, DocumentInfo Document )
    {
      m_Doc         = Document;
      m_ASMFileInfo = ASMFileInfo;
    }



    protected override bool ProcessTask()
    {
      if ( Core.MainForm.InvokeRequired )
      {
        return (bool)Core.MainForm.Invoke( new ProcessTaskCallback( ProcessTask ) );
      }
      Core.Navigating.UpdateFromMessages( m_ASMFileInfo,
                                          m_Doc.Project );
      Core.MainForm.m_CompileResult.UpdateFromMessages( m_ASMFileInfo, m_Doc.Project );
      return true;
    }



    public override string ToString()
    {
      return base.ToString() + " - " + m_Doc.DocumentFilename;
    }



  }
}
