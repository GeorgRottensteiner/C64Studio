﻿using RetroDevStudio.Types;
using GR.Memory;
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
  public partial class ExportCharsetAsBinaryFile : ExportCharsetFormBase
  {
    public ExportCharsetAsBinaryFile() :
      base( null )
    { 
    }



    public ExportCharsetAsBinaryFile( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();
    }



    public override bool HandleExport( ExportCharsetInfo Info, TextBox EditOutput, DocumentInfo DocInfo )
    {
      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.FileName = Info.Charset.ExportFilename;
      saveDlg.Title = "Export Charset to";
      saveDlg.Filter = "Charset|*.chr|All Files|*.*";
      if ( DocInfo.Project != null )
      {
        saveDlg.InitialDirectory = DocInfo.Project.Settings.BasePath;
      }
      if ( saveDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
      {
        return false;
      }
      GR.Memory.ByteBuffer charSet = new GR.Memory.ByteBuffer();

      List<int>     exportIndices = Info.ExportIndices;
      foreach ( int i in exportIndices )
      {
        charSet.Append( Info.Charset.Characters[i].Tile.Data );
      }
      if ( checkPrefixLoadAddress.Checked )
      {
        ushort address = GR.Convert.ToU16( editPrefixLoadAddress.Text, 16 );

        var addressData = new ByteBuffer();
        addressData.AppendU16( address );
        charSet = addressData + charSet;
      }
      GR.IO.File.WriteAllBytes( saveDlg.FileName, charSet );
      return true;
    }



    private void checkPrefixLoadAddress_CheckedChanged(object sender, EventArgs e)
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
