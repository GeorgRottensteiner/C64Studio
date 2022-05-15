using RetroDevStudio.Types;
using RetroDevStudio;
using RetroDevStudio.Formats;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static RetroDevStudio.BaseDocument;

namespace RetroDevStudio.Controls
{
  public partial class ExportCharscreenAsBinaryFile : ExportCharscreenFormBase
  {
    public ExportCharscreenAsBinaryFile() :
      base( null )
    { 
    }



    public ExportCharscreenAsBinaryFile( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();
    }



    public override bool HandleExport( ExportCharsetScreenInfo Info, TextBox EditOutput, DocumentInfo DocInfo )
    {
      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Save data as";
      saveDlg.Filter = "Binary Data|*.bin|All Files|*.*";
      if ( DocInfo.Project != null )
      {
        saveDlg.InitialDirectory = DocInfo.Project.Settings.BasePath;
      }
      if ( saveDlg.ShowDialog() != DialogResult.OK )
      {
        return false;
      }

      // prepare data
      GR.Memory.ByteBuffer finalData = null;

      switch ( Info.Data )
      {
        case ExportCharsetScreenInfo.ExportData.CHAR_THEN_COLOR:
          finalData = Info.ScreenCharData + Info.ScreenColorData;
          break;
        case ExportCharsetScreenInfo.ExportData.CHAR_ONLY:
          finalData = Info.ScreenCharData;
          break;
        case ExportCharsetScreenInfo.ExportData.COLOR_ONLY:
          finalData = Info.ScreenColorData;
          break;
        case ExportCharsetScreenInfo.ExportData.COLOR_THEN_CHAR:
          finalData = Info.ScreenColorData + Info.ScreenCharData;
          break;
        case ExportCharsetScreenInfo.ExportData.CHARSET:
          finalData = Info.CharsetData;
          break;
        default:
          return false;
      }
      if ( finalData != null )
      {
        GR.IO.File.WriteAllBytes( saveDlg.FileName, finalData );
      }
      return true;
    }



  }
}
