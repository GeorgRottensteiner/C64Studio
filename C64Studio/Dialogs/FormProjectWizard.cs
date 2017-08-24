using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace C64Studio
{
  public partial class FormProjectWizard : Form
  {
    public string           ProjectPath = "";
    public string           ProjectName = "";
    public string           ProjectFilename = "";

    private StudioSettings  Settings;


    public FormProjectWizard( string ProjectName, StudioSettings Settings )
    {
      this.Settings = Settings;
      InitializeComponent();
      editProjectName.Text = ProjectName;
      editBasePath.Text = Settings.DefaultProjectBasePath;

      btnOK.Enabled = false;
      UpdateSummary();
    }
    


    private void btnOK_Click( object sender, EventArgs e )
    {
      DialogResult = DialogResult.OK;
      ProjectName = editProjectName.Text;
      ProjectFilename = editBasePath.Text;
      Settings.DefaultProjectBasePath = ProjectFilename;
      ProjectFilename = System.IO.Path.Combine( ProjectFilename, ProjectName );
      ProjectPath = ProjectFilename;
      ProjectFilename = System.IO.Path.Combine( ProjectFilename, ProjectName + ".c64" );
      Close();
    }



    private void UpdateSummary()
    {
      if ( editProjectName.Text.Length == 0 )
      {
        labelProjectSummary.Text = "Please choose a valid project name";
        btnOK.Enabled = false;
        return;
      }
      if ( editProjectName.Text.IndexOfAny( System.IO.Path.GetInvalidFileNameChars() ) != -1 )
      {
        labelProjectSummary.Text = "The project name may only contain characters valid for a folder name";
        btnOK.Enabled = false;
        return;
      }
      if ( editBasePath.Text.Length == 0 )
      {
        labelProjectSummary.Text = "Please choose the base path the project will be created in";
        btnOK.Enabled = false;
        return;
      }
      string    finalPath = editBasePath.Text;
      finalPath = System.IO.Path.Combine( finalPath, editProjectName.Text );
      finalPath = System.IO.Path.Combine( finalPath, editProjectName.Text + ".c64" );
      labelProjectSummary.Text = "The project file will be created as " + finalPath;
      btnOK.Enabled = true;
    }



    private void btnBrowseBasePath_Click( object sender, EventArgs e )
    {
      FolderBrowserDialog  dlgFolder = new FolderBrowserDialog();

      dlgFolder.Description = "Select new project base path";
      //dlgFolder.RootFolder = Environment.SpecialFolder..CommonApplicationData;
      dlgFolder.SelectedPath = editBasePath.Text;
      dlgFolder.ShowNewFolderButton = true;
      if ( dlgFolder.ShowDialog() == DialogResult.OK )
      {
        editBasePath.Text = dlgFolder.SelectedPath;

        UpdateSummary();
      }
    }



    private void editProjectName_TextChanged( object sender, EventArgs e )
    {
      UpdateSummary();
    }



    private void editBasePath_TextChanged( object sender, EventArgs e )
    {
      UpdateSummary();
    }

  }
}
