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
using GR.Memory;

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
        if ( checkPrefixLoadAddress.Checked )
        {
          ushort address = GR.Convert.ToU16( editPrefixLoadAddress.Text, 16 );

          var addressData = new ByteBuffer();
          addressData.AppendU16( address );
          finalData = addressData + finalData;
        }

        GR.IO.File.WriteAllBytes( saveDlg.FileName, finalData );
      }
      return true;
    }



    private void checkPrefixLoadAddress_CheckedChanged( object sender, EventArgs e )
    {
      editPrefixLoadAddress.Enabled = checkPrefixLoadAddress.Checked;
    }

    private void editPrefixLoadAddress_KeyPress( object sender, KeyPressEventArgs e )
    {
      if ( ( ( e.KeyChar >= '0' )
      &&     ( e.KeyChar <= '9' ) )
      ||   ( ( e.KeyChar >= 'A' )
      &&     ( e.KeyChar <= 'F' ) )
      ||   ( ( e.KeyChar >= 'a' )
      &&     ( e.KeyChar <= 'f' ) )
      ||   ( char.IsControl( e.KeyChar ) ) )
      {
      }
      else
      {
        e.Handled = true;
      }
    }



  }
}
