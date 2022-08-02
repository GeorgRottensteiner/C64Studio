using RetroDevStudio.Types;
using RetroDevStudio;
using System;
using System.Windows.Forms;
using System.Linq;

namespace RetroDevStudio.Dialogs
{
  public partial class FormRenameSolution : Form
  {
    StudioCore                      Core;
    string                          _OriginalSolutionName;
    string                          _OriginalSolutionFilename;



    public FormRenameSolution( StudioCore Core, string OriginalSolutionName )
    {
      this.Core = Core;
      _OriginalSolutionName     = OriginalSolutionName;
      _OriginalSolutionFilename = Core.Navigating.Solution.Filename;

      InitializeComponent();

      editSolutionName.Text = _OriginalSolutionName;

      Core.Theming.ApplyTheme( this );

      ValidateNewName();
      editSolutionName.Select();
      editSolutionName.SelectAll();
    }



    private void btnOK_Click( object sender, EventArgs e )
    {
      string newName = editSolutionName.Text;

      if ( newName == _OriginalSolutionName )
      {
        DialogResult = DialogResult.OK;
        Close();
        return;
      }

      RenameSolution();

      DialogResult = DialogResult.OK;
      Close();
    }



    private void editSolutionName_TextChanged( object sender, EventArgs e )
    {
      ValidateNewName();
    }



    private bool ValidateNewName()
    {
      string    newFilename     = editSolutionName.Text;
      string    newSolutionPath = GR.Path.RenameFilenameWithoutExtension( _OriginalSolutionFilename, editSolutionName.Text );
      string    solutionFullDir = System.IO.Path.GetDirectoryName( _OriginalSolutionFilename );
      string    solutionParentDir = solutionFullDir.Substring( System.IO.Path.GetDirectoryName( solutionFullDir ).Length + 1 );

      if ( GR.Path.IsPathEqual( newFilename, _OriginalSolutionName ) )
      {
        labelRenameInfo.Text = $"The new solution file name matches the old.";
        btnOK.Enabled = false;
        return false;
      }

      var invalidChars = System.IO.Path.GetInvalidFileNameChars();
      for ( int i = 0; i < newFilename.Length; ++i )
      {
        if ( invalidChars.Contains( newFilename[i] ) )
        {
          labelRenameInfo.Text = $"The new solution file name contains invalid character '{newFilename[i]}'";
          btnOK.Enabled = false;
          return false;
        }
      }

      if ( System.IO.File.Exists( newSolutionPath ) )
      {
        labelRenameInfo.Text = $"There already exists a solution file with the name '{newSolutionPath}'";
        btnOK.Enabled = false;
        return false;
      }

      labelRenameInfo.Text = $"The solution file will be renamed to {newSolutionPath}.";
      if ( solutionParentDir != _OriginalSolutionName )
      {
        labelRenameInfo.Text += $"\r\nThe solution parent directory {solutionParentDir} does not match the solution name {_OriginalSolutionName} and will not be renamed.";
      }
      else
      {
        string    newSolutionParentDir = GR.Path.RenameFile( solutionParentDir, newFilename );
        labelRenameInfo.Text += $"\r\nThe solution parent directory {solutionParentDir} will be renamed to {newSolutionParentDir}.";
      }

      btnOK.Enabled = true;
      return true;
    }



    private void RenameSolution()
    {
      string    newFilename             = editSolutionName.Text;
      string    newSolutionPath         = GR.Path.RenameFilenameWithoutExtension( _OriginalSolutionFilename, newFilename );

      Core.Navigating.RenameSolution( newSolutionPath );
    }




  }
}
