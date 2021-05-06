using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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



    private void btnBrowseVice_Click( object sender, EventArgs e )
    {
      OpenFileDialog    dlgBrowse = new OpenFileDialog();

      dlgBrowse.Title = "Select Vice executable";
      dlgBrowse.Filter = FilterString( Types.Constants.FILEFILTER_VICE + Types.Constants.FILEFILTER_EXECUTABLE + Types.Constants.FILEFILTER_ALL );

      if ( dlgBrowse.ShowDialog() == DialogResult.OK )
      {
        editPathEmulator.Text = dlgBrowse.FileName;
      }
    }



    private void editPathEmulator_TextChanged( object sender, EventArgs e )
    {
      btnOK.Enabled = ( editPathEmulator.Text.Length > 0 ) && ( System.IO.File.Exists( editPathEmulator.Text ) );
    }



  }
}
