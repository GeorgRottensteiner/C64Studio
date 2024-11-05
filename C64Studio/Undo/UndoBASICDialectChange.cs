using FastColoredTextBoxNS;
using RetroDevStudio.Controls;
using RetroDevStudio.Documents;
using RetroDevStudio.Formats;
using System.Collections.Generic;

namespace RetroDevStudio.Undo
{
  public class UndoBASICDialectChange : UndoTask
  {
    private SourceBasicEx   Document = null;
    private string          Dialect = "";



    public UndoBASICDialectChange( SourceBasicEx Doc, string PreviousDialect )
    {
      Document  = Doc;
      Dialect   = PreviousDialect;
    }



    public override string Description
    {
      get
      {
        return "BASIC Dialect Change";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoBASICDialectChange( Document, Document.BASICDialect.Name );
    }



    public override void Apply()
    {
      Document.SetBASICDialect( Document.Core.Compiling.BASICDialects[Dialect] );
    }



  }
}
