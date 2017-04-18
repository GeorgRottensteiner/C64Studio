using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio.Tasks
{
  public class TaskParseFile : Task
  {
    private DocumentInfo    m_Document;
    private ProjectConfig   m_Configuration;



    public TaskParseFile( DocumentInfo Document, ProjectConfig Configuration )
    {
      m_Document = Document;
      m_Configuration = Configuration;
    }



    protected override bool ProcessTask()
    {
      Parser.ASMFileParser parser = new Parser.ASMFileParser();

      var compileConfig = new C64Studio.Parser.CompileConfig();
      compileConfig.Assembler = m_Document.Element.AssemblerType;

      parser.ParseFile( m_Document, m_Configuration, compileConfig );

      return true;
    }
  }
}
