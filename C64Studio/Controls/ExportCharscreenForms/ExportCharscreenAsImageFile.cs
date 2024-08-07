﻿using RetroDevStudio.Types;
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



namespace RetroDevStudio.Controls
{
  public partial class ExportCharscreenAsImageFile : ExportCharscreenFormBase
  {
    public ExportCharscreenAsImageFile() :
      base( null )
    { 
    }



    public ExportCharscreenAsImageFile( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();
    }



    public override bool HandleExport( ExportCharsetScreenInfo Info, TextBox EditOutput, DocumentInfo DocInfo )
    {
      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Export Screen to Image";
      saveDlg.Filter = "PNG File|*.png";
      if ( saveDlg.ShowDialog() != DialogResult.OK )
      {
        return false;
      }

      int     neededWidth   = Info.Charscreen.ScreenWidth * 8;
      int     neededHeight  = Info.Charscreen.ScreenHeight * 8;

      GR.Image.MemoryImage targetImg = new GR.Image.MemoryImage( neededWidth, neededHeight, GR.Drawing.PixelFormat.Format32bppRgb );

      Info.Image.DrawTo( targetImg, 0, 0 );

      System.Drawing.Bitmap bmpTarget = targetImg.GetAsBitmap();
      bmpTarget.Save( saveDlg.FileName, System.Drawing.Imaging.ImageFormat.Png );
      return true;
    }



  }
}
