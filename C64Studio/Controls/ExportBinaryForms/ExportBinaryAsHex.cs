using RetroDevStudio;
using RetroDevStudio.Formats;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;



namespace RetroDevStudio.Controls
{
  public partial class ExportBinaryAsHex : ExportBinaryFormBase
  {
    public ExportBinaryAsHex() :
      base( null )
    { 
    }



    public ExportBinaryAsHex( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();
    }



    public override bool HandleExport( ExportBinaryInfo info, DocumentInfo DocInfo )
    {
      editTextOutput.Text = info.Data.ToString();
      return true;
    }



  }
}
