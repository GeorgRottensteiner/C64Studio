using FastColoredTextBoxNS;
using RetroDevStudio.Controls;
using RetroDevStudio.Documents;
using RetroDevStudio.Formats;
using System.Collections.Generic;

namespace RetroDevStudio.Undo
{
  public class UndoBASICSymbolModeToggle : UndoTask
  {
    private SourceBasicEx   Document = null;



    public UndoBASICSymbolModeToggle( SourceBasicEx Doc )
    {
      Document = Doc;
    }



    public override string Description
    {
      get
      {
        return "BASIC Symbol Mode Toggle";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoBASICSymbolModeToggle( Document );
    }



    public override void Apply()
    {
      Document.SymbolModeToggled();
    }



  }
}
