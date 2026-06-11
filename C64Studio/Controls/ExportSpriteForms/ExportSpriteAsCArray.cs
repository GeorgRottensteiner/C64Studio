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
  public partial class ExportSpriteAsCArray : ExportSpriteFormBase
  {
    public ExportSpriteAsCArray() :
      base( null )
    { 
    }



    public ExportSpriteAsCArray( StudioCore Core ) :
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



    public override bool HandleExport( ExportSpriteInfo Info, TextBox EditOutput, DocumentInfo DocInfo )
    {
      int wrapByteCount = GetExportWrapCount();
      bool hexExport = checkExportHex.Checked;
      bool wrapData = checkExportToDataWrap.Checked;

      GR.Memory.ByteBuffer charSet = new GR.Memory.ByteBuffer();
      foreach ( int index in Info.ExportIndices )
      {
        charSet.Append( Info.Project.Sprites[index].Tile.Data );
      }

      string line = Util.ToCArray( Info.ExportData, wrapData, wrapByteCount, "sprites_", hexExport );
      EditOutput.Text = line;
      return true;
    }



  }
}
