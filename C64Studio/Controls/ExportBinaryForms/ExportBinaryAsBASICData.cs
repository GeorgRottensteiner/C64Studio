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
  public partial class ExportBinaryAsBASICData : ExportBinaryFormBase
  {
    public ExportBinaryAsBASICData() :
      base( null )
    { 
    }



    public ExportBinaryAsBASICData( StudioCore Core ) :
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



    private int GetExportPadCount()
    {
      if ( checkPad.Checked )
      {
        return GR.Convert.ToI32( editPadCount.Text );
      }
      return -1;
    }



    public override bool HandleExport( ExportBinaryInfo info, DocumentInfo DocInfo )
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
      int padCount = GetExportPadCount();

      if ( asHex )
      {
        sb.Append( Util.ToBASICHexData( info.Data, startLine, lineOffset, wrapByteCount, wrapCharCount, insertSpaces, padCount ) );
      }
      else
      {
        sb.Append( Util.ToBASICData( info.Data, startLine, lineOffset, wrapByteCount, wrapCharCount, insertSpaces, padCount ) );
      }

      editTextOutput.Font = Core.Imaging.FontFromMachine( MachineType.C64, Core.Settings.BASICSourceFontSize * 0.8f );
      editTextOutput.Text = sb.ToString();
      return true;
    }



    private void checkExportToDataWrap_CheckedChanged_1( object sender, EventArgs e )
    {
      editWrapByteCount.Enabled = checkExportToDataWrap.Checked;
    }



    private void checkWrapAtMaxChars_CheckedChanged( object sender, EventArgs e )
    {
      editWrapCharCount.Enabled = checkWrapAtMaxChars.Checked;
    }



    private void checkPad_CheckedChanged( object sender, EventArgs e )
    {
      editPadCount.Enabled = checkPad.Checked;
    }



  }
}
