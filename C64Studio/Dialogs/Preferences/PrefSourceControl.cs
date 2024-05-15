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
  public partial class PrefSourceControl : PrefBase
  {
    public PrefSourceControl()
    {
      InitializeComponent();
    }



    public PrefSourceControl( StudioCore Core ) : base( Core )
    {
      _Keywords.AddRange( new string[] { "source", "git", "control", "commit", "author", "mail", "email" } );

      InitializeComponent();

      editCommitterEmail.Text  = Core.Settings.SourceControlInfo.CommitAuthorEmail;
      editCommitAuthor.Text    = Core.Settings.SourceControlInfo.CommitAuthor;
    }



    private void btnImportSettings_Click( DecentForms.ControlBase Sender )
    {
      ImportLocalSettings();
    }



    private void btnExportSettings_Click( DecentForms.ControlBase Sender )
    {
      SaveLocalSettings();
    }



    public override void ExportSettings( XMLElement SettingsRoot )
    {
      var xmlSourceControl = SettingsRoot.AddChild( "SourceControl" );

      xmlSourceControl.AddAttribute( "Author", Core.Settings.SourceControlInfo.CommitAuthor );
      xmlSourceControl.AddAttribute( "Email", Core.Settings.SourceControlInfo.CommitAuthorEmail );
    }



    public override void ImportSettings( XMLElement SettingsRoot )
    {
      var xmlSourceControl = SettingsRoot.FindByType( "SourceControl" );
      if ( xmlSourceControl != null )
      {
        editCommitAuthor.Text    = xmlSourceControl.Attribute( "Author" );
        editCommitterEmail.Text  = xmlSourceControl.Attribute( "Email" );
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



  }
}
