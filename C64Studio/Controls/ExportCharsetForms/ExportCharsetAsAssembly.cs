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
  public partial class ExportCharsetAsAssembly : ExportCharsetFormBase
  {
    public ExportCharsetAsAssembly() :
      base( null )
    { 
    }



    public ExportCharsetAsAssembly( StudioCore Core ) :
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
      int wrapByteCount = GetExportWrapCount();
      string prefix = editPrefix.Text;

      GR.Memory.ByteBuffer charSet = new GR.Memory.ByteBuffer();
      foreach ( int index in Info.ExportIndices )
      {
        charSet.Append( Info.Charset.Characters[index].Tile.Data );
      }

      bool wrapData = checkExportToDataWrap.Checked;
      bool prefixRes = checkExportToDataIncludeRes.Checked;

      string resultText = "CHARS" + System.Environment.NewLine;
      resultText += Util.ToASMData( charSet, wrapData, wrapByteCount, prefixRes ? prefix : "" );

      if ( checkIncludeColor.Checked )
      {
        resultText += System.Environment.NewLine + "COLORS" + System.Environment.NewLine;

        GR.Memory.ByteBuffer colorData = new GR.Memory.ByteBuffer();
        foreach ( int index in Info.ExportIndices )
        {
          colorData.AppendU8( (byte)Info.Charset.Characters[index].Tile.CustomColor );
        }
        resultText += Util.ToASMData( colorData, wrapData, wrapByteCount, prefixRes ? prefix : "" );
      }

      EditOutput.Text = resultText;
      return true;
    }



    private void checkExportToDataIncludeRes_CheckedChanged( object sender, EventArgs e )
    {
      editPrefix.Enabled = checkExportToDataIncludeRes.Checked;
    }



  }
}
