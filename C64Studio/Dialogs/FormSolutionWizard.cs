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
      checkCreateRepository.Checked = ( global::SourceControl.Controller.IsFunctional ) && ( Settings.SourceControlInfo.CreateSolutionRepository );

      checkSeparateRepositoryForProject.Visible = global::SourceControl.Controller.IsFunctional;
      checkSeparateRepositoryForProject.Checked = ( global::SourceControl.Controller.IsFunctional ) && ( Settings.SourceControlInfo.CreateProjectRepository );

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
        finalSolutionPath = GR.Path.Append( finalSolutionPath, editSolutionName.Text );
      }
      string finalPathProject = finalSolutionPath;

      if ( checkCreateProjectInSeparateFolder.Checked )
      {
        finalPathProject = GR.Path.Append( finalSolutionPath, editProjectName.Text );
      }
      string solutionPath = GR.Path.Append( finalSolutionPath, editSolutionName.Text + ".s64" );
      string projectPath  = GR.Path.Append( finalPathProject, editProjectName.Text + ".c64" );

      SolutionPath                    = finalSolutionPath;
      SolutionFilename                = solutionPath;
      ProjectFilename                 = projectPath;
      CreateRepository                = checkCreateRepository.Checked;
      CreateRepositoryForProject      = checkSeparateRepositoryForProject.Checked;

      if ( global::SourceControl.Controller.IsFolderUnderSourceControl( finalSolutionPath ) )
      {
        // there is already a repo at the target folder
        CreateRepository            = false;
        CreateRepositoryForProject  = false;
      }
      else if ( global::SourceControl.Controller.IsFunctional )
      {
        Settings.SourceControlInfo.CreateSolutionRepository = CreateRepository;
        Settings.SourceControlInfo.CreateProjectRepository  = CreateRepositoryForProject;
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
        finalPath = GR.Path.Append( finalPath, editSolutionName.Text );
      }
      string finalPathProject = finalPath;
      if ( checkCreateProjectInSeparateFolder.Checked )
      {
        finalPathProject = GR.Path.Append( finalPath, editProjectName.Text );
      }
      string solutionPath = GR.Path.Append( finalPath, editSolutionName.Text + ".s64" );
      string projectFullFilename  = GR.Path.Append( finalPathProject, editProjectName.Text + ".c64" );

      if ( System.IO.File.Exists( solutionPath ) )
      {
        labelSolutionSummary.Text = $"The target solution file {solutionPath} already exists!";
        btnOK.Enabled = false;
        return;
      }
      if ( System.IO.File.Exists( projectFullFilename ) )
      {
        labelSolutionSummary.Text = $"The target project file {projectFullFilename} already exists!";
        btnOK.Enabled = false;
        return;
      }

      labelSolutionSummary.Text = "The solution file will be created as " + solutionPath + "." + System.Environment.NewLine
                                + "The project file will be created as " + projectFullFilename + "." + System.Environment.NewLine;

      if ( checkCreateRepository.Checked )
      {
        var gitPath1 = GR.Path.Append( finalPath, ".git" );
        if ( checkSeparateRepositoryForProject.Checked )
        {
          var gitPath2 = GR.Path.Append( finalPathProject, ".git" );
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
