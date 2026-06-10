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
  public partial class ExportCharsetAsCArray : ExportCharsetFormBase
  {
    public ExportCharsetAsCArray() :
      base( null )
    { 
    }



    public ExportCharsetAsCArray( StudioCore Core ) :
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



    public override bool HandleExport( ExportCharsetInfo Info, TextBox EditOutput, DocumentInfo DocInfo )
    {
      bool wrapData = checkExportToDataWrap.Checked;
      int wrapByteCount = GetExportWrapCount();

      GR.Memory.ByteBuffer charSet = new GR.Memory.ByteBuffer();
      foreach ( int index in Info.ExportIndices )
      {
        charSet.Append( Info.Charset.Characters[index].Tile.Data );
      }

      EditOutput.Text = Util.ToCArray( charSet, wrapData, wrapByteCount, "charset_", checkExportHex.Checked );
      return true;
    }



  }
}
