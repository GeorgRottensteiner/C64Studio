using FastColoredTextBoxNS;
using RetroDevStudio.Controls;
using RetroDevStudio.Documents;
using RetroDevStudio.Formats;
using System.Collections.Generic;

namespace RetroDevStudio.Undo
{
  public class UndoBASICLabelModeToggle : UndoTask
  {
    private SourceBasicEx   Document = null;



    public UndoBASICLabelModeToggle( SourceBasicEx Doc )
    {
      Document = Doc;
    }



    public override string Description
    {
      get
      {
        return "BASIC Label Mode Toggle";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoBASICLabelModeToggle( Document );
    }



    public override void Apply()
    {
      Document.LabelModeToggled();
    }



  }
}
