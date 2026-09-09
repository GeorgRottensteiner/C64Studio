using GR.Memory;
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
  public partial class ExportPathAsFile : ExportPathFormBase
  {
    public ExportPathAsFile() :
      base( null )
    { 
    }



    public ExportPathAsFile( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();
    }



    public override bool HandleExport( ExportPathInfo info, DocumentInfo DocInfo )
    {
      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.FileName  = DocInfo.FullPath;
      saveDlg.Title     = "Export binary data to";
      saveDlg.Filter    = "All Files|*.*";
      if ( DocInfo.Project != null )
      {
        saveDlg.InitialDirectory = DocInfo.Project.Settings.BasePath;
      }
      if ( saveDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
      {
        return false;
      }
      var data = new ByteBuffer();
      foreach ( var path in info.DataPerPath )
      {
        data.Append( path.second );
      }
      GR.IO.File.WriteAllBytes( saveDlg.FileName, data );
      return true;
    }



  }
}
