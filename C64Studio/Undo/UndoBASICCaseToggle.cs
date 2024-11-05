using FastColoredTextBoxNS;
using RetroDevStudio.Controls;
using RetroDevStudio.Documents;
using RetroDevStudio.Formats;
using System.Collections.Generic;

namespace RetroDevStudio.Undo
{
  public class UndoBASICCaseToggle : UndoTask
  {
    private SourceBasicEx   Document = null;



    public UndoBASICCaseToggle( SourceBasicEx Doc )
    {
      Document = Doc;
    }



    public override string Description
    {
      get
      {
        return "BASIC Case Toggle";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoBASICCaseToggle( Document );
    }



    public override void Apply()
    {
      Document.CaseToggled();
    }



  }
}
