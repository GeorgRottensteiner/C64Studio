using System;
using System.Collections.Generic;
using System.Text;

namespace RetroDevStudio.Tasks
{
  public class TaskUpdateCompileResult : Task
  {
    private DocumentInfo      m_Doc;
    private Parser.ParserBase m_Parser;



    public TaskUpdateCompileResult( Parser.ParserBase Parser, DocumentInfo Document )
    {
      m_Doc = Document;
      m_Parser = Parser;
    }



    protected override bool ProcessTask()
    {
      if ( Core.MainForm.InvokeRequired )
      {
        return (bool)Core.MainForm.Invoke( new ProcessTaskCallback( ProcessTask ) );
      }
      Core.Navigating.UpdateFromMessages( m_Parser.Messages,
                                          ( m_Parser is Parser.ASMFileParser ) ? ( (Parser.ASMFileParser)m_Parser ).ASMFileInfo : null,
                                          m_Doc.Project );
      Core.MainForm.m_CompileResult.UpdateFromMessages( m_Parser, m_Doc.Project );
      return true;
    }



    public override string ToString()
    {
      return base.ToString() + " - " + m_Doc.DocumentFilename;
    }



  }
}
