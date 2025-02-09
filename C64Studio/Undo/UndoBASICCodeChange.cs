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
    private DocumentInfo        _affectedDocument = null;
    private bool                IsUndo = true;



    public UndoBASICCodeChange( DocumentInfo doc, FastColoredTextBox Edit, bool Undo )
    {
      this.Edit         = Edit;
      IsUndo            = Undo;
      _affectedDocument = doc;
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
      return new UndoBASICCodeChange( _affectedDocument, Edit, !IsUndo );
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
      _affectedDocument.MarkAsDirty();
    }



  }
}
