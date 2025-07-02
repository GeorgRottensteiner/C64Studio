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
  public partial class ExportCharscreenAsBASICData : ExportCharscreenFormBase
  {
    public ExportCharscreenAsBASICData() :
      base( null )
    { 
    }



    public ExportCharscreenAsBASICData( StudioCore Core ) :
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



    private int GetExportCharCount()
    {
      if ( checkWrapAtMaxChars.Checked )
      {
        return GR.Convert.ToI32( editWrapCharCount.Text );
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
      bool insertSpaces = checkInsertSpaces.Checked;
      int wrapCharCount = GetExportCharCount();

      switch ( Info.Data )
      {
        case ExportCharsetScreenInfo.ExportData.CHAR_THEN_COLOR:
          if ( asHex )
          {
            sb.Append( Util.ToBASICHexData( Info.ScreenCharData + Info.ScreenColorData, startLine, lineOffset, wrapByteCount, wrapCharCount, insertSpaces ) );
          }
          else
          {
            sb.Append( Util.ToBASICData( Info.ScreenCharData + Info.ScreenColorData, startLine, lineOffset, wrapByteCount, wrapCharCount, insertSpaces ) );
          }
          break;
        case ExportCharsetScreenInfo.ExportData.CHAR_ONLY:
          if ( asHex )
          {
            sb.Append( Util.ToBASICHexData( Info.ScreenCharData, startLine, lineOffset, wrapByteCount, wrapCharCount, insertSpaces ) );
          }
          else
          {
            sb.Append( Util.ToBASICData( Info.ScreenCharData, startLine, lineOffset, wrapByteCount, wrapCharCount, insertSpaces ) );
          }
          break;
        case ExportCharsetScreenInfo.ExportData.COLOR_ONLY:
          if ( asHex )
          {
            sb.Append( Util.ToBASICHexData( Info.ScreenColorData, startLine, lineOffset, wrapByteCount, wrapCharCount, insertSpaces ) );
          }
          else
          {
            sb.Append( Util.ToBASICData( Info.ScreenColorData, startLine, lineOffset, wrapByteCount, wrapCharCount, insertSpaces ) );
          }
          break;
        case ExportCharsetScreenInfo.ExportData.COLOR_THEN_CHAR:
          if ( asHex )
          {
            sb.Append( Util.ToBASICHexData( Info.ScreenColorData + Info.ScreenCharData, startLine, lineOffset, wrapByteCount, wrapCharCount, insertSpaces ) );
          }
          else
          {
            sb.Append( Util.ToBASICData( Info.ScreenColorData + Info.ScreenCharData, startLine, lineOffset, wrapByteCount, wrapCharCount, insertSpaces ) );
          }
          break;
        default:
          return false;
      }

      EditOutput.Font = Core.Imaging.FontFromMachine( MachineType.C64, Core.Settings.BASICSourceFontSize * 0.8f );
      EditOutput.Text = sb.ToString();
      return true;
    }



    private void checkWrapAtMaxChars_CheckedChanged( object sender, EventArgs e )
    {
      editWrapCharCount.Enabled = checkWrapAtMaxChars.Checked;
    }



  }
}
