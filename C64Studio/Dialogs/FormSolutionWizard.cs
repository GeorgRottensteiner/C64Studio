using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace C64Studio
{
  public partial class FormSolutionWizard : Form
  {
    public string           SolutionPath = "";
    public string           SolutionName = "";
    public string           SolutionFilename = "";
    public string           ProjectFilename = "";

    private StudioSettings  Settings;


    public FormSolutionWizard( string ProjectName, StudioSettings Settings )
    {
      this.Settings = Settings;
      InitializeComponent();
      editSolutionName.Text = ProjectName;
      editBasePath.Text = Settings.DefaultProjectBasePath;

      btnOK.Enabled = false;
      UpdateSummary();
    }
    


    private void btnOK_Click( object sender, EventArgs e )
    {
      DialogResult = DialogResult.OK;
      SolutionName    = editSolutionName.Text;
      ProjectFilename = editBasePath.Text;
      Settings.DefaultProjectBasePath = ProjectFilename;
      ProjectFilename = System.IO.Path.Combine( ProjectFilename, SolutionName );
      SolutionPath = ProjectFilename;
      SolutionFilename = System.IO.Path.Combine( ProjectFilename, SolutionName + ".s64" );
      ProjectFilename = System.IO.Path.Combine( ProjectFilename, SolutionName + ".c64" );
      Close();
    }



    private void UpdateSummary()
    {
      if ( editSolutionName.Text.Length == 0 )
      {
        labelSolutionSummary.Text = "Please choose a valid solution name";
        btnOK.Enabled = false;
        return;
      }
      if ( editSolutionName.Text.IndexOfAny( System.IO.Path.GetInvalidFileNameChars() ) != -1 )
      {
        labelSolutionSummary.Text = "The solution name may only contain characters valid for a folder name";
        btnOK.Enabled = false;
        return;
      }
      if ( editBasePath.Text.Length == 0 )
      {
        labelSolutionSummary.Text = "Please choose the base path the solution will be created in";
        btnOK.Enabled = false;
        return;
      }
      string    finalPath = editBasePath.Text;
      finalPath = System.IO.Path.Combine( finalPath, editSolutionName.Text );
      string solutionPath = System.IO.Path.Combine( finalPath, editSolutionName.Text + ".s64" );
      string projectPath = System.IO.Path.Combine( finalPath, editSolutionName.Text + ".c64" );
      labelSolutionSummary.Text = "The solution file will be created as " + solutionPath + "." + System.Environment.NewLine
                                + "The project file will be created as " + projectPath + ".";
      btnOK.Enabled = true;
    }



    private void btnBrowseBasePath_Click( object sender, EventArgs e )
    {
      FolderBrowserDialog  dlgFolder = new FolderBrowserDialog();

      dlgFolder.Description = "Select new solution base path";
      dlgFolder.SelectedPath = editBasePath.Text;
      dlgFolder.ShowNewFolderButton = true;
      if ( dlgFolder.ShowDialog() == DialogResult.OK )
      {
        editBasePath.Text = dlgFolder.SelectedPath;

        UpdateSummary();
      }
    }



    private void editSolutionName_TextChanged( object sender, EventArgs e )
    {
      UpdateSummary();
    }



    private void editBasePath_TextChanged( object sender, EventArgs e )
    {
      UpdateSummary();
    }

  }
}
