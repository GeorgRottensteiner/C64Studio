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
  public partial class ExportSpriteAsBASICData : ExportSpriteFormBase
  {
    public ExportSpriteAsBASICData() :
      base( null )
    { 
    }



    public ExportSpriteAsBASICData( StudioCore Core ) :
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
        return GR.Convert.ToI32( editWrapByteCount.Text );
      }
      return 0;
    }



    private int GetExportCharCount()
    {
      if ( checkWrapAtMaxChars.Checked )
      {
        return GR.Convert.ToI32( editWrapCharCount.Text );
      }
      return 80;
    }



    public override bool HandleExport( ExportSpriteInfo Info, TextBox EditOutput, DocumentInfo DocInfo )
    {
      var   sb = new StringBuilder();

      int startLine = GR.Convert.ToI32( editExportBASICLineNo.Text );
      if ( ( startLine < 0 )
      ||   ( startLine > 63999 ) )
      {
        startLine = 10;
      }
      int lineOffset = GR.Convert.ToI32( editExportBASICLineOffset.Text );
      if ( ( lineOffset < 0 )
      ||   ( lineOffset > 63999 ) )
      {
        lineOffset = 10;
      }

      int wrapByteCount = GetExportWrapCount();
      bool asHex = checkExportHex.Checked;
      bool insertSpaces = checkInsertSpaces.Checked;
      int wrapCharCount = GetExportCharCount();

      List<int>     exportIndices = Info.ExportIndices;

      if ( asHex )
      {
        sb.Append( Util.ToBASICHexData( Info.ExportData, startLine, lineOffset, wrapByteCount, wrapCharCount, insertSpaces ) );
      }
      else
      {
        sb.Append( Util.ToBASICData( Info.ExportData, startLine, lineOffset, wrapByteCount, wrapCharCount, insertSpaces ) );
      }

      EditOutput.Font = Core.Imaging.FontFromMachine( MachineType.C64, Core.Settings.BASICSourceFontSize * 0.8f );
      EditOutput.Text = sb.ToString();
      return true;
    }



  }
}
