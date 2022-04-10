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



namespace C64Studio.Controls
{
  public partial class ExportAsBASICData : ExportCharscreenFormBase
  {
    public ExportAsBASICData() :
      base( null )
    { 
    }



    public ExportAsBASICData( StudioCore Core ) :
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
      return 80;
    }



    public override bool HandleExport( ExportCharsetScreenInfo Info, TextBox EditOutput, DocumentInfo DocInfo )
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
        startLine = 10;
      }

      int wrapByteCount = GetExportWrapCount();
      bool asHex = checkExportHex.Checked;

      switch ( Info.Data )
      {
        case ExportCharsetScreenInfo.ExportData.CHAR_THEN_COLOR:
          if ( asHex )
          {
            sb.Append( Util.ToBASICHexData( Info.ScreenCharData + Info.ScreenColorData, startLine, lineOffset, wrapByteCount ) );
          }
          else
          {
            sb.Append( Util.ToBASICData( Info.ScreenCharData + Info.ScreenColorData, startLine, lineOffset, wrapByteCount ) );
          }
          break;
        case ExportCharsetScreenInfo.ExportData.CHAR_ONLY:
          if ( asHex )
          {
            sb.Append( Util.ToBASICHexData( Info.ScreenCharData, startLine, lineOffset, wrapByteCount ) );
          }
          else
          {
            sb.Append( Util.ToBASICData( Info.ScreenCharData, startLine, lineOffset, wrapByteCount ) );
          }
          break;
        case ExportCharsetScreenInfo.ExportData.COLOR_ONLY:
          if ( asHex )
          {
            sb.Append( Util.ToBASICHexData( Info.ScreenColorData, startLine, lineOffset, wrapByteCount ) );
          }
          else
          {
            sb.Append( Util.ToBASICData( Info.ScreenColorData, startLine, lineOffset, wrapByteCount ) );
          }
          break;
        case ExportCharsetScreenInfo.ExportData.COLOR_THEN_CHAR:
          if ( asHex )
          {
            sb.Append( Util.ToBASICHexData( Info.ScreenColorData + Info.ScreenCharData, startLine, lineOffset, wrapByteCount ) );
          }
          else
          {
            sb.Append( Util.ToBASICData( Info.ScreenColorData + Info.ScreenCharData, startLine, lineOffset, wrapByteCount ) );
          }
          break;
        default:
          return false;
      }

      EditOutput.Font = new System.Drawing.Font( Core.MainForm.m_FontC64.Families[0], 16, System.Drawing.GraphicsUnit.Pixel );
      EditOutput.Text = sb.ToString();
      return true;
    }



  }
}
