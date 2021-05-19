using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio.Tasks
{
  /// <summary>
  /// preparses a single file (without project) and populates hover and label info
  /// </summary>
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
      if ( m_Document.Type != ProjectElement.ElementType.ASM_SOURCE )
      {
        return true;
      }

      Parser.ASMFileParser parser = new Parser.ASMFileParser();

      var compileConfig = new C64Studio.Parser.CompileConfig();
      if ( m_Document.Element != null )
      {
        compileConfig.Assembler = m_Document.Element.AssemblerType;
      }

      string sourceCode = "";

      if ( m_Document.BaseDoc != null )
      {
        if ( m_Document.Type == ProjectElement.ElementType.ASM_SOURCE )
        {
          sourceCode = ( (SourceASMEx)m_Document.BaseDoc ).editSource.Text;
        }
        else if ( m_Document.Type == ProjectElement.ElementType.BASIC_SOURCE )
        {
          sourceCode = ( (SourceBasicEx)m_Document.BaseDoc ).editSource.Text;
        }
      }

      parser.ParseFile( m_Document.FullPath, sourceCode, m_Configuration, compileConfig, null );

      if ( ( compileConfig.Assembler != C64Studio.Types.AssemblerType.AUTO )
      &&   ( m_Document.BaseDoc != null )
      &&   ( m_Document.Element != null ) )
      {
        if ( m_Document.Element.AssemblerType != compileConfig.Assembler )
        {
          m_Document.Element.AssemblerType = compileConfig.Assembler;
          m_Document.BaseDoc.SetModified();
        }
      }

      ( (SourceASMEx)m_Document.BaseDoc ).SetLineInfos( parser.ASMFileInfo );

      var knownTokens = parser.KnownTokens();
      GR.Collections.MultiMap<string, C64Studio.Types.SymbolInfo> knownTokenInfos = parser.KnownTokenInfo();

      m_Document.SetASMFileInfo( parser.ASMFileInfo, knownTokens, knownTokenInfos );

      var task = new Tasks.TaskUpdateKeywords( m_Document.BaseDoc );
      task.Core = Core;
      task.RunTask();

      return true;
    }
  }
}
