using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;



namespace RetroDevStudio.Dialogs
{
  public partial class FormSolutionWizard : Form
  {
    public string           SolutionPath = "";
    public string           SolutionName = "";
    public string           SolutionFilename = "";
    public string           ProjectFilename = "";
    public bool             CreateNewFolderForSolution = true;
    public bool             CreateRepository = false;
    public bool             CreateRepositoryForProject = false;

    private StudioSettings  Settings;


    public FormSolutionWizard( string ProjectName, StudioSettings Settings )
    {
      this.Settings           = Settings;
      InitializeComponent();
      editSolutionName.Text   = ProjectName;
      editProjectName.Text    = ProjectName;
      SolutionName            = ProjectName;
      editBasePath.Text       = Settings.DefaultProjectBasePath;

      checkCreateRepository.Visible = global::SourceControl.Controller.IsFunctional;
      checkCreateRepository.Checked = global::SourceControl.Controller.IsFunctional;

      btnOK.Enabled = false;
      UpdateSummary();
    }
    


    private void btnOK_Click( DecentForms.ControlBase Sender )
    {
      DialogResult                    = DialogResult.OK;
      SolutionName                    = editSolutionName.Text;
      ProjectFilename                 = editBasePath.Text;
      Settings.DefaultProjectBasePath = ProjectFilename;

      string    finalSolutionPath = editBasePath.Text;
      if ( CreateNewFolderForSolution )
      {
        finalSolutionPath = System.IO.Path.Combine( finalSolutionPath, editSolutionName.Text );
      }
      string finalPathProject = finalSolutionPath;

      if ( checkCreateProjectInSeparateFolder.Checked )
      {
        finalPathProject = System.IO.Path.Combine( finalSolutionPath, editProjectName.Text );
      }
      string solutionPath = System.IO.Path.Combine( finalSolutionPath, editSolutionName.Text + ".s64" );
      string projectPath  = System.IO.Path.Combine( finalPathProject, editProjectName.Text + ".c64" );

      SolutionPath                    = finalSolutionPath;
      SolutionFilename                = solutionPath;
      ProjectFilename                 = projectPath;
      CreateRepository                = checkCreateRepository.Checked;
      CreateRepositoryForProject      = checkSeparateRepositoryForProject.Checked;

      if ( global::SourceControl.Controller.IsFolderUnderSourceControl( finalSolutionPath ) )
      {
        CreateRepository            = false;
        CreateRepositoryForProject  = false;
      }

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
      if ( CreateNewFolderForSolution )
      {
        finalPath = System.IO.Path.Combine( finalPath, editSolutionName.Text );
      }
      string finalPathProject = finalPath;
      if ( checkCreateProjectInSeparateFolder.Checked )
      {
        finalPathProject = System.IO.Path.Combine( finalPath, editProjectName.Text );
      }
      string solutionPath = System.IO.Path.Combine( finalPath, editSolutionName.Text + ".s64" );
      string projectFullFilename  = System.IO.Path.Combine( finalPathProject, editProjectName.Text + ".c64" );

      labelSolutionSummary.Text = "The solution file will be created as " + solutionPath + "." + System.Environment.NewLine
                                + "The project file will be created as " + projectFullFilename + "." + System.Environment.NewLine;

      checkCreateRepository.Enabled             = true;
      checkSeparateRepositoryForProject.Enabled = true;
      if ( checkCreateRepository.Checked )
      {
        var gitPath1 = System.IO.Path.Combine( finalPath, ".git" );
        if ( checkSeparateRepositoryForProject.Checked )
        {
          var gitPath2 = System.IO.Path.Combine( finalPathProject, ".git" );
          labelSolutionSummary.Text += $"A repository will be created in {gitPath1}, a second will be created in {gitPath2}.";
        }
        else
        {
          labelSolutionSummary.Text += $"A repository will be created in {gitPath1}.";
        }
      }
      else
      {
        labelSolutionSummary.Text += "No repository will be created.";
      }


      btnOK.Enabled = true;
    }



    private void btnBrowseBasePath_Click( DecentForms.ControlBase Sender )
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
      if ( editProjectName.Text == SolutionName )
      {
        editProjectName.Text = editSolutionName.Text;
      }
      SolutionName = editSolutionName.Text;

      UpdateSummary();
    }



    private void editBasePath_TextChanged( object sender, EventArgs e )
    {
      UpdateSummary();
    }



    private void checkCreateRepository_CheckedChanged( object sender, EventArgs e )
    {
      UpdateSummary();
    }



    private void checkSeparateRepositoryForProject_CheckedChanged( object sender, EventArgs e )
    {
      UpdateSummary();
    }



    private void checkCreateProjectInSeparateFolder_CheckedChanged( object sender, EventArgs e )
    {
      if ( !checkCreateProjectInSeparateFolder.Checked )
      {
        checkSeparateRepositoryForProject.Enabled = false;
        checkSeparateRepositoryForProject.Checked = false;
      }
      else
      {
        checkSeparateRepositoryForProject.Enabled = true;
      }
      UpdateSummary();
    }



    private void editProjectName_TextChanged( object sender, EventArgs e )
    {
      UpdateSummary();
    }



    private void checkCreateNewSolutionFolder_CheckedChanged( object sender, EventArgs e )
    {
      CreateNewFolderForSolution = checkCreateSolutionFolder.Checked;
      UpdateSummary();
    }



    private void btnCancel_Click( DecentForms.ControlBase Sender )
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }



  }
}
