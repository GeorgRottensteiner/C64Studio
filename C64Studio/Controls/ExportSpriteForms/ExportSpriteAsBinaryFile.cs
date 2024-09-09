using RetroDevStudio.Types;
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
  public partial class ExportSpriteAsBinaryFile : ExportSpriteFormBase
  {
    public ExportSpriteAsBinaryFile() :
      base( null )
    { 
    }



    public ExportSpriteAsBinaryFile( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();
    }



    public override bool HandleExport( ExportSpriteInfo Info, TextBox EditOutput, DocumentInfo DocInfo )
    {
      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.FileName  = Info.Project.ExportFilename;
      saveDlg.Title     = "Export Sprites to";
      saveDlg.Filter    = "Sprites|*.spr|All Files|*.*";
      if ( DocInfo.Project != null )
      {
        saveDlg.InitialDirectory = DocInfo.Project.Settings.BasePath;
      }
      if ( saveDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
      {
        return false;
      }
      GR.Memory.ByteBuffer exportData = Info.ExportData;

      if ( checkExportAddPadding.Checked )
      {
        int     numBytesWithoutPadding  = Lookup.NumBytesOfSingleSprite( Info.Project.Mode );
        int     numBytesWithPadding     = Lookup.NumPaddedBytesOfSingleSprite( Info.Project.Mode );

        if ( ( numBytesWithPadding != numBytesWithoutPadding )
        &&   ( Info.Project.Mode != SpriteProject.SpriteProjectMode.COMMODORE_24_X_21_HIRES_OR_MC ) )
        {
          exportData.Clear();
          for ( int i = 0; i < Info.ExportIndices.Count; ++i )
          {
            exportData.Append( Info.Project.Sprites[Info.ExportIndices[i]].Tile.Data );
            if ( i + 1 < Info.ExportIndices.Count )
            {
              exportData.Append( new ByteBuffer( (uint)( numBytesWithPadding - numBytesWithoutPadding ), 0 ) );
            }
          }
        }
      }

      if ( checkPrefixLoadAddress.Checked )
      {
        ushort address = GR.Convert.ToU16( editPrefixLoadAddress.Text, 16 );

        var addressData = new ByteBuffer();
        addressData.AppendU16( address );
        exportData = addressData + exportData;
      }
      GR.IO.File.WriteAllBytes( saveDlg.FileName, exportData );
      return true;
    }



    private void checkPrefixLoadAddress_CheckedChanged(object sender, EventArgs e)
    {
      editPrefixLoadAddress.Enabled = checkPrefixLoadAddress.Checked;
    }



  }
}
