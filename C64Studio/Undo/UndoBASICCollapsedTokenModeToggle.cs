using FastColoredTextBoxNS;
using RetroDevStudio.Controls;
using RetroDevStudio.Documents;
using RetroDevStudio.Formats;
using System.Collections.Generic;

namespace RetroDevStudio.Undo
{
  public class UndoBASICCollapsedTokenModeToggle : UndoTask
  {
    private SourceBasicEx   Document = null;



    public UndoBASICCollapsedTokenModeToggle( SourceBasicEx Doc )
    {
      Document = Doc;
    }



    public override string Description
    {
      get
      {
        return "BASIC Collapsed Token Mode Toggle";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoBASICCollapsedTokenModeToggle( Document );
    }



    public override void Apply()
    {
      Document.CollapsedTokenModeToggled();
    }



  }
}
