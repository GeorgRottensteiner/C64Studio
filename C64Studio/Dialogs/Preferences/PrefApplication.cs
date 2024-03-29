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
  public partial class PrefApplication : PrefBase
  {
    public PrefApplication()
    {
      InitializeComponent();

      comboAppMode.SelectedIndex = 0;
    }



    public PrefApplication( StudioCore Core ) : base( Core )
    {
      _Keywords.AddRange( new string[] { "application", "general", "generic", "mode", "mru", "solution", "compiler", "messages", "environment" } );

      InitializeComponent();

      comboAppMode.SelectedIndex                  = (int)Core.Settings.StudioAppMode;
      editDefaultOpenSolutionPath.Text            = Core.Settings.DefaultProjectBasePath;
      editMaxMRUEntries.Text                      = Core.Settings.MRUMaxCount.ToString();
      checkAutoOpenLastSolution.Checked           = Core.Settings.AutoOpenLastSolution;
      checkShowCompilerMessagesAfterBuild.Checked = Core.Settings.ShowCompilerMessagesAfterBuild;
      checkShowOutputDisplayAfterBuild.Checked    = Core.Settings.ShowOutputDisplayAfterBuild;
    }



    private void comboAppMode_SelectedIndexChanged( object sender, EventArgs e )
    {
      Core.Settings.StudioAppMode = (AppMode)comboAppMode.SelectedIndex;
    }



    private void btnImportSettings_Click( object sender, EventArgs e )
    {
      ImportLocalSettings();
    }



    private void btnExportSettings_Click( object sender, EventArgs e )
    {
      SaveLocalSettings();
    }



    public override void ExportSettings( XMLElement SettingsRoot )
    {
      var xmlEnvironment = SettingsRoot.AddChild( "Generic.Environment" );

      xmlEnvironment.AddAttribute( "OpenLastSolutionOnStartup", Core.Settings.AutoOpenLastSolution ? "yes" : "no" );
      xmlEnvironment.AddAttribute( "MaxMRUCount", Core.Settings.MRUMaxCount.ToString() );
      xmlEnvironment.AddAttribute( "DefaultOpenSolutionPath", Core.Settings.DefaultProjectBasePath );
      xmlEnvironment.AddAttribute( "ApplicationMode", ( (int)Core.Settings.StudioAppMode ).ToString() );
      xmlEnvironment.AddAttribute( "ShowCompilerMessagesAfterBuild", Core.Settings.ShowCompilerMessagesAfterBuild ? "yes" : "no" );
      xmlEnvironment.AddAttribute( "ShowOutputDisplayAfterBuild", Core.Settings.ShowOutputDisplayAfterBuild ? "yes" : "no" );
    }



    public override void ImportSettings( XMLElement SettingsRoot )
    {
      var xmlEnvironment = SettingsRoot.FindByType( "Generic.Environment" );
      if ( xmlEnvironment != null )
      {
        checkAutoOpenLastSolution.Checked           = IsSettingTrue( xmlEnvironment.Attribute( "OpenLastSolutionOnStartup" ) );
        editMaxMRUEntries.Text                      = GR.Convert.ToI32( xmlEnvironment.Attribute( "MaxMRUCount" ) ).ToString();
        editDefaultOpenSolutionPath.Text            = xmlEnvironment.Attribute( "DefaultOpenSolutionPath" );
        comboAppMode.SelectedIndex                  = GR.Convert.ToI32( xmlEnvironment.Attribute( "ApplicationMode" ) );
        checkShowCompilerMessagesAfterBuild.Checked = IsSettingTrue( xmlEnvironment.Attribute( "ShowCompilerMessagesAfterBuild" ) );
        checkShowOutputDisplayAfterBuild.Checked    = IsSettingTrue( xmlEnvironment.Attribute( "ShowOutputDisplayAfterBuild" ) );
      }
    }
    
    
    
    private void checkAutoOpenLastSolution_CheckedChanged( object sender, EventArgs e )
    {
      if ( Core.Settings.AutoOpenLastSolution != checkAutoOpenLastSolution.Checked )
      {
        Core.Settings.AutoOpenLastSolution = checkAutoOpenLastSolution.Checked;
      }
    }



    private void editDefaultOpenSolutionPath_TextChanged( object sender, EventArgs e )
    {
      Core.Settings.DefaultProjectBasePath = editDefaultOpenSolutionPath.Text;
    }



    private void btnBrowseDefaultOpenSolutionPath_Click( object sender, EventArgs e )
    {
      FolderBrowserDialog   dlg = new FolderBrowserDialog();

      dlg.SelectedPath = Core.Settings.DefaultProjectBasePath;
      dlg.Description = "Choose default open solution/project path";
      if ( dlg.ShowDialog() == DialogResult.OK )
      {
        Core.Settings.DefaultProjectBasePath = dlg.SelectedPath;
        editDefaultOpenSolutionPath.Text = dlg.SelectedPath;
      }
    }



    private void editMaxMRUEntries_TextChanged( object sender, EventArgs e )
    {
      int     mruCount = GR.Convert.ToI32( editMaxMRUEntries.Text );
      if ( ( mruCount < 1 )
      ||   ( mruCount > 99 ) )
      {
        editMaxMRUEntries.Text = "4";
        mruCount = 4;
      }
      Core.Settings.MRUMaxCount = mruCount;
    }



    private void checkShowCompilerMessagesAfterBuild_CheckedChanged( object sender, EventArgs e )
    {
      if ( Core.Settings.ShowCompilerMessagesAfterBuild != checkShowCompilerMessagesAfterBuild.Checked )
      {
        Core.Settings.ShowCompilerMessagesAfterBuild = checkShowCompilerMessagesAfterBuild.Checked;
      }
    }



    private void checkShowOutputDisplayAfterBuild_CheckedChanged( object sender, EventArgs e )
    {
      if ( Core.Settings.ShowOutputDisplayAfterBuild != checkShowOutputDisplayAfterBuild.Checked )
      {
        Core.Settings.ShowOutputDisplayAfterBuild = checkShowOutputDisplayAfterBuild.Checked;
      }
    }



  }
}
