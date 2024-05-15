using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;



namespace RetroDevStudio.Dialogs
{
  public partial class FormWizard : Form
  {
    public FormWizard( StudioCore Core )
    {
      InitializeComponent();
      btnOK.Enabled = false;

      Core.Theming.ApplyTheme( this );
    }
    


    private void btnOK_Click( DecentForms.ControlBase Sender )
    {
      DialogResult = DialogResult.OK;
      Close();
    }



    private string FilterString( string Source )
    {
      return Source.Substring( 0, Source.Length - 1 );
    }



    private void btnBrowseVice_Click( DecentForms.ControlBase Sender )
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



    private void btnCancel_Click( DecentForms.ControlBase Sender )
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }



  }
}
