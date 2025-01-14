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
  public partial class ExportBinaryAsAssembly : ExportBinaryFormBase
  {
    public ExportBinaryAsAssembly() :
      base( null )
    { 
    }



    public ExportBinaryAsAssembly( StudioCore Core ) :
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
      string prefix = editPrefix.Text;

      bool wrapData = checkExportToDataWrap.Checked;
      bool prefixRes = checkExportToDataIncludeRes.Checked;

      string resultText = Util.ToASMData( info.Data, wrapData, wrapByteCount, prefixRes ? prefix : "", checkExportHex.Checked );

      editTextOutput.Text = resultText;
      return true;
    }



    private void checkExportToDataIncludeRes_CheckedChanged( object sender, EventArgs e )
    {
      editPrefix.Enabled = checkExportToDataIncludeRes.Checked;
    }



  }
}
