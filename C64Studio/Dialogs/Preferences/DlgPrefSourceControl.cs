using GR.Strings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RetroDevStudio.Dialogs.Preferences
{
  [Description( "Source Control.GIT" )]
  public partial class DlgPrefSourceControl : DlgPrefBase
  {
    public DlgPrefSourceControl()
    {
      InitializeComponent();
    }



    public DlgPrefSourceControl( StudioCore Core ) : base( Core )
    {
      _Keywords.AddRange( new string[] { "source", "git", "control", "commit", "author", "mail", "email", "repository" } );

      InitializeComponent();
    }



    public override void ApplySettingsToControls()
    {
      editCommitterEmail.Text  = Core.Settings.SourceControlInfo.CommitAuthorEmail;
      editCommitAuthor.Text    = Core.Settings.SourceControlInfo.CommitAuthor;

      checkGenerateSolutionRepository.Checked = Core.Settings.SourceControlInfo.CreateSolutionRepository;
      checkGenerateProjectRepository.Checked  = Core.Settings.SourceControlInfo.CreateProjectRepository;
    }



    public override void ExportSettings( XMLElement SettingsRoot )
    {
      var xmlSourceControl = SettingsRoot.AddChild( "SourceControl" );

      xmlSourceControl.AddAttribute( "Author", Core.Settings.SourceControlInfo.CommitAuthor );
      xmlSourceControl.AddAttribute( "Email", Core.Settings.SourceControlInfo.CommitAuthorEmail );
      xmlSourceControl.AddAttribute( "CreateSolutionRepository", Core.Settings.SourceControlInfo.CreateSolutionRepository ? "yes" : "no" );
      xmlSourceControl.AddAttribute( "CreateProjectRepository", Core.Settings.SourceControlInfo.CreateProjectRepository ? "yes" : "no" );
    }



    public override void ImportSettings( XMLElement SettingsRoot )
    {
      var xmlSourceControl = SettingsRoot.FindByType( "SourceControl" );
      if ( xmlSourceControl != null )
      {
        Core.Settings.SourceControlInfo.CommitAuthor      = xmlSourceControl.Attribute( "Author" );
        Core.Settings.SourceControlInfo.CommitAuthorEmail = xmlSourceControl.Attribute( "Email" );

        Core.Settings.SourceControlInfo.CreateSolutionRepository  = IsSettingTrue( xmlSourceControl.Attribute( "CreateSolutionRepository" ) );
        Core.Settings.SourceControlInfo.CreateProjectRepository   = IsSettingTrue( xmlSourceControl.Attribute( "CreateProjectRepository" ) );
      }
    }
    
    
    
    private void editCommitterEmail_TextChanged( object sender, EventArgs e )
    {
      Core.Settings.SourceControlInfo.CommitAuthorEmail = editCommitterEmail.Text;
    }



    private void editCommitAuthor_TextChanged( object sender, EventArgs e )
    {
      Core.Settings.SourceControlInfo.CommitAuthor = editCommitAuthor.Text;
    }



    private void checkGenerateSolutionRepository_CheckedChanged( object sender, EventArgs e )
    {
      Core.Settings.SourceControlInfo.CreateSolutionRepository = checkGenerateSolutionRepository.Checked;
    }



    private void checkGenerateProjectRepository_CheckedChanged( object sender, EventArgs e )
    {
      Core.Settings.SourceControlInfo.CreateProjectRepository = checkGenerateProjectRepository.Checked;
    }



  }
}
