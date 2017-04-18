using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio
{
  public class ProjectElement
  {
    public enum ElementType
    {
      INVALID,
      ASM_SOURCE
    };

    public enum BuildTypes
    {
      NONE,
      ASSEMBLER,
      COMMAND_LINE
    };

    public ElementType      Type = ElementType.INVALID;
    public BuildTypes       BuildType = BuildTypes.NONE;
    private string          m_Name = "";
    public BaseDocument     Document = null;
    public System.Windows.Forms.TreeNode Node = null;
    public string           Filename = null;
    public string           TargetFilename = null;
    public FileParser.CompileTargetType TargetType = FileParser.CompileTargetType.NONE;

    public string           Name
    {
      get
      {
        return m_Name;
      }
      set
      {
        m_Name = value;
        if ( Node != null )
        {
          Node.Text = value;
        }
        if ( Document != null )
        {
          Document.Text = value;
        }
      }
    }
  }
}
