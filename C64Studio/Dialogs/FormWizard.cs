using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace C64Studio
{
  public partial class FormWizard : Form
  {
    public FormWizard()
    {
      InitializeComponent();
      btnOK.Enabled = false;

      labelACMEPath.Visible = false;
      editPathACME.Visible = false;
      btnBrowseACME.Visible = false;
    }
    
    private void btnOK_Click( object sender, EventArgs e )
    {
      DialogResult = DialogResult.OK;
      Close();
    }



    private string FilterString( string Source )
    {
      return Source.Substring( 0, Source.Length - 1 );
    }



    private void btnBrowseACME_Click( object sender, EventArgs e )
    {
      OpenFileDialog    dlgBrowse = new OpenFileDialog();

      dlgBrowse.Title = "Select ACME executable";
      dlgBrowse.Filter = FilterString( Types.Constants.FILEFILTER_ACME + Types.Constants.FILEFILTER_EXECUTABLE + Types.Constants.FILEFILTER_ALL );

      if ( dlgBrowse.ShowDialog() == DialogResult.OK )
      {
        editPathACME.Text = dlgBrowse.FileName;
      }
    }



    private void btnBrowseVice_Click( object sender, EventArgs e )
    {
      OpenFileDialog    dlgBrowse = new OpenFileDialog();

      dlgBrowse.Title = "Select Vice executable";
      dlgBrowse.Filter = FilterString( Types.Constants.FILEFILTER_VICE + Types.Constants.FILEFILTER_EXECUTABLE + Types.Constants.FILEFILTER_ALL );

      if ( dlgBrowse.ShowDialog() == DialogResult.OK )
      {
        editPathVice.Text = dlgBrowse.FileName;
      }
    }

    private void editPath_TextChanged( object sender, EventArgs e )
    {
      EnableOK();
    }

    private void EnableOK()
    {
      btnOK.Enabled = ( ( editPathVice.Text.Length > 0 && File.Exists(editPathVice.Text) ) || ( editPathACME.Text.Length > 0 && File.Exists(editPathACME.Text) ) );
    }

  }
}
