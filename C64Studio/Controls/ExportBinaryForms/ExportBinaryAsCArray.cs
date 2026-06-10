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
  public partial class ExportBinaryAsCArray : ExportBinaryFormBase
  {
    public ExportBinaryAsCArray() :
      base( null )
    { 
    }



    public ExportBinaryAsCArray( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();
    }



    private void checkExportToDataWrap_CheckedChanged( object sender, EventArgs e )
    {
      editWrapByteCount.Enabled = checkExportToDataWrap.Checked;
    }



    private int GetExportWrapCount()
    {
      if ( checkExportToDataWrap.Checked )
      {
        int wrapByteCount = GR.Convert.ToI32( editWrapByteCount.Text );
        if ( wrapByteCount <= 0 )
        {
          wrapByteCount = 8;
        }
        return wrapByteCount;
      }
      return 80;
    }



    public override bool HandleExport( ExportBinaryInfo info, DocumentInfo DocInfo )
    {
      int wrapByteCount = GetExportWrapCount();

      bool wrapData = checkExportToDataWrap.Checked;

      string resultText = Util.ToCArray( info.Data, wrapData, wrapByteCount, "data", checkExportHex.Checked );

      editTextOutput.Text = resultText;
      return true;
    }



  }
}
