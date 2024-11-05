using FastColoredTextBoxNS;
using RetroDevStudio.Controls;
using RetroDevStudio.Documents;
using RetroDevStudio.Formats;
using System.Collections.Generic;

namespace RetroDevStudio.Undo
{
  public class UndoBASICCodeChange : UndoTask
  {
    private FastColoredTextBox  Edit = null;
    private bool                IsUndo = true;



    public UndoBASICCodeChange( FastColoredTextBox Edit, bool Undo )
    {
      this.Edit = Edit;
      IsUndo = Undo;
    }



    public override string Description
    {
      get
      {
        return "BASIC Code Change";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoBASICCodeChange( Edit, !IsUndo );
    }



    public override void Apply()
    {
      if ( IsUndo )
      {
        Edit.Undo();
      }
      else
      {
        Edit.Redo();
      }
    }



  }
}
