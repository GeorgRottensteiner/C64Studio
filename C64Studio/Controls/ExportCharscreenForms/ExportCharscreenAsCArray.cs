using GR.Memory;
using RetroDevStudio;
using RetroDevStudio.Formats;
using RetroDevStudio.Types;
using SharpSid;
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
  public partial class ExportCharscreenAsCArray : ExportCharscreenFormBase
  {
    public ExportCharscreenAsCArray() :
      base( null )
    { 
    }



    public ExportCharscreenAsCArray( StudioCore Core ) :
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



    public override bool HandleExport( ExportCharsetScreenInfo Info, TextBox EditOutput, DocumentInfo DocInfo )
    {
      var             sb = new StringBuilder();
      bool            useHex = checkExportHex.Checked;
      bool            wrapData = checkExportToDataWrap.Checked;
      int             wrapByteCount = GetExportWrapCount();     

      for ( int i = 0; i < Info.ScreensToExport.Count; ++i )
      {
        switch ( Info.Data )
        {
          case ExportCharsetScreenInfo.ExportData.CHARSET:
            sb.AppendLine( Util.ToCArray( Info.CharsetData, wrapData, wrapByteCount, "charset_data", useHex ) );
            i = Info.ScreensToExport.Count;
            break;
          case ExportCharsetScreenInfo.ExportData.CHAR_THEN_COLOR:
            sb.AppendLine( Util.ToCArray( Info.ScreenCharData[i], wrapData, wrapByteCount, "screen_chars_" + Info.Charscreen.Screens[Info.ScreensToExport[i]].Name, useHex ) );
            sb.AppendLine( Util.ToCArray( Info.ScreenColorData[i], wrapData, wrapByteCount, "screen_colors_" + Info.Charscreen.Screens[Info.ScreensToExport[i]].Name, useHex ) );
            break;
          case ExportCharsetScreenInfo.ExportData.COLOR_THEN_CHAR:
            sb.AppendLine( Util.ToCArray( Info.ScreenColorData[i], wrapData, wrapByteCount, "screen_colors_" + Info.Charscreen.Screens[Info.ScreensToExport[i]].Name, useHex ) );
            sb.AppendLine( Util.ToCArray( Info.ScreenCharData[i], wrapData, wrapByteCount, "screen_chars_" + Info.Charscreen.Screens[Info.ScreensToExport[i]].Name, useHex ) );
            break;
          case ExportCharsetScreenInfo.ExportData.COLOR_ONLY:
            sb.AppendLine( Util.ToCArray( Info.ScreenColorData[i], wrapData, wrapByteCount, "screen_colors_" + Info.Charscreen.Screens[Info.ScreensToExport[i]].Name, useHex ) );
            break;
          case ExportCharsetScreenInfo.ExportData.CHAR_ONLY:
            sb.AppendLine( Util.ToCArray( Info.ScreenCharData[i], wrapData, wrapByteCount, "screen_chars_" + Info.Charscreen.Screens[Info.ScreensToExport[i]].Name, useHex ) );
            break;
          case ExportCharsetScreenInfo.ExportData.CHAR_AND_COLOR_INTERLEAVED:
            {
              var interleavedBuffer = new ByteBuffer( Info.ScreenCharData[i].Length + Info.ScreenColorData[i].Length );
              for ( int ii = 0; ii < Info.ScreenCharData[i].Length; ++ii )
              {
                interleavedBuffer.SetU8At( ii * 2, Info.ScreenCharData[i].ByteAt( ii ) );
                interleavedBuffer.SetU8At( ii * 2 + 1, Info.ScreenColorData[i].ByteAt( ii ) );
              }
              sb.AppendLine( Util.ToCArray( interleavedBuffer, wrapData, wrapByteCount, "screen_interleaved_" + Info.Charscreen.Screens[Info.ScreensToExport[i]].Name, useHex ) );
            }
            break;
        }
      }
      EditOutput.Text = sb.ToString();
      return true;
    }



  }
}
