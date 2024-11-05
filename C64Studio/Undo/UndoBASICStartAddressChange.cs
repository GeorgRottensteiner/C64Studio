using FastColoredTextBoxNS;
using RetroDevStudio.Controls;
using RetroDevStudio.Documents;
using RetroDevStudio.Formats;
using System.Collections.Generic;

namespace RetroDevStudio.Undo
{
  public class UndoBASICStartAddressChange : UndoTask
  {
    private SourceBasicEx   Document = null;
    private string          Address = "";



    public UndoBASICStartAddressChange( SourceBasicEx Doc, string PreviousAddress )
    {
      Document  = Doc;
      Address   = PreviousAddress;
    }



    public override string Description
    {
      get
      {
        return "BASIC Start Address Change";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoBASICStartAddressChange( Document, Document.StartAddress.ToString() );
    }



    public override void Apply()
    {
      Document.SetStartAddress( Address );
    }



  }
}
