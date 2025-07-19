using GR.Strings;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace RetroDevStudio.Dialogs.Preferences
{
  [Description( "General.Application" )]
  public partial class DlgPrefApplication : DlgPrefBase
  {
    public DlgPrefApplication()
    {
      InitializeComponent();

      comboAppMode.SelectedIndex = 0;
    }



    public DlgPrefApplication( StudioCore Core ) : base( Core )
    {
      _Keywords.AddRange( new string[] { "application", "general", "generic", "mode", "mru", "solution", "compiler", "messages", "environment", "update", "check" } );

      InitializeComponent();
    }



    public override void ApplySettingsToControls()
    {
      comboAppMode.SelectedIndex                  = (int)Core.Settings.StudioAppMode;
      editDefaultOpenSolutionPath.Text            = Core.Settings.DefaultProjectBasePath;
      editMaxMRUEntries.Text                      = Core.Settings.MRUMaxCount.ToString();
      checkAutoOpenLastSolution.Checked           = Core.Settings.AutoOpenLastSolution;
      checkShowCompilerMessagesAfterBuild.Checked = Core.Settings.ShowCompilerMessagesAfterBuild;
      checkShowOutputDisplayAfterBuild.Checked    = Core.Settings.ShowOutputDisplayAfterBuild;
      checkForUpdate.Checked                      = Core.Settings.CheckForUpdates;
      checkAutoSaveSettings.Checked               = Core.Settings.AutoSaveSettings;
      editAutoSaveDelay.Text                      = Core.Settings.AutoSaveSettingsDelayMilliSeconds.ToString();
    }



    private void comboAppMode_SelectedIndexChanged( object sender, EventArgs e )
    {
      Core.Settings.StudioAppMode = (AppMode)comboAppMode.SelectedIndex;
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
      xmlEnvironment.AddAttribute( "CheckForUpdatesOnStartup", Core.Settings.CheckForUpdates ? "yes" : "no" );
      xmlEnvironment.AddAttribute( "AutoSaveSettings", Core.Settings.AutoSaveSettings ? "yes" : "no" );
      xmlEnvironment.AddAttribute( "AutoSaveSettingsDelayMS", Core.Settings.AutoSaveSettingsDelayMilliSeconds.ToString() );
    }



    public override void ImportSettings( XMLElement SettingsRoot )
    {
      var xmlEnvironment = SettingsRoot.FindByType( "Generic.Environment" );
      if ( xmlEnvironment != null )
      {
        Core.Settings.StudioAppMode                 = (AppMode)GR.Convert.ToI32( xmlEnvironment.Attribute( "ApplicationMode" ) );
        Core.Settings.DefaultProjectBasePath        = xmlEnvironment.Attribute( "DefaultOpenSolutionPath" );
        Core.Settings.MRUMaxCount                   = GR.Convert.ToI32( xmlEnvironment.Attribute( "MaxMRUCount" ) );
        Core.Settings.AutoOpenLastSolution          = IsSettingTrue( xmlEnvironment.Attribute( "OpenLastSolutionOnStartup" ) );
        Core.Settings.ShowCompilerMessagesAfterBuild = IsSettingTrue( xmlEnvironment.Attribute( "ShowCompilerMessagesAfterBuild" ) );
        Core.Settings.ShowOutputDisplayAfterBuild   = IsSettingTrue( xmlEnvironment.Attribute( "ShowOutputDisplayAfterBuild" ) );
        Core.Settings.CheckForUpdates               = IsSettingTrue( xmlEnvironment.Attribute( "CheckForUpdatesOnStartup" ) );
        Core.Settings.AutoSaveSettings              = IsSettingTrue( xmlEnvironment.Attribute( "AutoSaveSettings" ) );
        Core.Settings.AutoSaveSettingsDelayMilliSeconds = GR.Convert.ToI32( xmlEnvironment.Attribute( "AutoSaveSettingsDelayMS" ) );
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



    private void btnBrowseDefaultOpenSolutionPath_Click( DecentForms.ControlBase Sender )
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



    private void checkForUpdate_CheckedChanged( object sender, EventArgs e )
    {
      if ( Core.Settings.CheckForUpdates != checkForUpdate.Checked )
      {
        Core.Settings.CheckForUpdates = checkForUpdate.Checked;
      }
    }


    public enum HChangeNotifyEventID
    {
      SHCNE_ASSOCCHANGED = 0x08000000
    }

    [Flags]
    public enum HChangeNotifyFlags
    {
      SHCNF_IDLIST = 0x0000
    }

    [DllImport( "Shell32.dll", CharSet = CharSet.Auto, SetLastError = true )]
    public static extern void SHChangeNotify( HChangeNotifyEventID wEventId, HChangeNotifyFlags uFlags, IntPtr dwItem1, IntPtr dwItem2 );



    private void btnRegisterSolutionFileType_Click( DecentForms.ControlBase Sender )
    {
      try
      {
        string ext = ".s64";
        string handlerName = "retrodevstudio_solution";
        string desc = "RetroDevStudio Solution";

        Registry.SetValue( "HKEY_CURRENT_USER\\Software\\Classes\\" + ext, "", handlerName );
        Registry.SetValue( "HKEY_CURRENT_USER\\Software\\Classes\\" + ext + "\\OpenWithProgids", handlerName, new byte[0] );
        Registry.SetValue( "HKEY_CURRENT_USER\\Software\\Classes\\" + handlerName, "", desc );

        Registry.SetValue( "HKEY_CURRENT_USER\\Software\\Classes\\" + handlerName + "\\DefaultIcon", "", "\"" + Application.StartupPath + "Icons\\retrodevstudio.ico\"" );

        Registry.SetValue( "HKEY_CURRENT_USER\\Software\\Classes\\" + handlerName + "\\shell", "", "" );
        Registry.SetValue( "HKEY_CURRENT_USER\\Software\\Classes\\" + handlerName + "\\shell\\open", "", "" );
        Registry.SetValue( "HKEY_CURRENT_USER\\Software\\Classes\\" + handlerName + "\\shell\\open\\command", "", "\"" + Application.ExecutablePath + "\" \"%L\"" );

        /*
        RegistryKey classesKey = Registry.CurrentUser.OpenSubKey( "Software\\Classes", RegistryKeyPermissionCheck.ReadWriteSubTree );

        RegistryKey key = classesKey.CreateSubKey( ext );
        key.SetValue( "", "retrodevstudio_solution" );
        key.Close();

        // create RetroDevStudio handler key
        key = classesKey.CreateSubKey( "retrodevstudio_solution" );
        key.Close();

        key = classesKey.CreateSubKey( "retrodevstudio_solution" + "\\Shell\\Open\\command" );

        key.SetValue( "", "\"" + Application.ExecutablePath + "\" \"%L\"" );
        key.Close();

        key = classesKey.CreateSubKey( "retrodevstudio_solution" + "\\DefaultIcon" );
        key.SetValue( "", "\"" + Application.StartupPath + "Icons\\retrodevstudio.ico\"" );
        key.Close();

        classesKey.Close();
        */
        SHChangeNotify( HChangeNotifyEventID.SHCNE_ASSOCCHANGED, HChangeNotifyFlags.SHCNF_IDLIST, IntPtr.Zero, IntPtr.Zero );
      }
      catch ( Exception ex )
      {
        Core.Notification.MessageBox( "Failed to register file type", "Could not access registry keys:\r\n" + ex.Message );
      }
    }

    
    
    private void btnRegisterProjectFileType_Click( DecentForms.ControlBase Sender )
    {

    }



    private void checkAutoSaveSettings_CheckedChanged( object sender, EventArgs e )
    {
      if ( Core.Settings.AutoSaveSettings != checkAutoSaveSettings.Checked )
      {
        Core.Settings.AutoSaveSettings = checkAutoSaveSettings.Checked;
        labelAutoSaveInfo1.Enabled = checkAutoSaveSettings.Checked;
        labelAutoSaveInfo2.Enabled = checkAutoSaveSettings.Checked;
        editAutoSaveDelay.Enabled = checkAutoSaveSettings.Checked;
      }
    }



    private void editAutoSaveDelay_TextChanged( object sender, EventArgs e )
    {
      if ( int.TryParse( editAutoSaveDelay.Text, out int delayMS ) )
      {
        if ( delayMS <= 0 )
        {
          delayMS = 300000;
        }
        if ( delayMS != Core.Settings.AutoSaveSettingsDelayMilliSeconds )
        {
          Core.Settings.AutoSaveSettingsDelayMilliSeconds = delayMS;
        }
      }
    }
  }
}
