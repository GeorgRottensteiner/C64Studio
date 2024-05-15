using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;



namespace RetroDevStudio.Dialogs
{
  public partial class ProjectProperties : Form
  {
    private Project             m_Project = null;
    private ProjectSettings     m_Settings = null;
    private ProjectConfig       m_ProjectConfig = null;
    public bool                 Modified = false;
    private StudioCore          m_Core = null;



    public ProjectProperties( Project MyProject, ProjectSettings Settings, StudioCore Core )
    {
      m_Settings  = Settings;
      m_Core      = Core;
      m_Project   = MyProject;
      InitializeComponent();

      editProjectName.Text      = m_Settings.Name;
      labelProjectFile.Text     = MyProject.Settings.Filename;

      foreach ( var configName in m_Settings.GetConfigurationNames() )
      {
        comboConfiguration.Items.Add( configName );
      }
      if ( m_Settings.CurrentConfig == null )
      {
        m_Settings.CurrentConfig = m_Settings.GetConfigurations().First();
      }
      comboConfiguration.SelectedItem = m_Settings.CurrentConfig.Name;

      Core.Theming.ApplyTheme( this );
    }



    private void btnClose_Click( DecentForms.ControlBase Sender )
    {
      Close();
    }



    private void editProjectName_TextChanged( object sender, EventArgs e )
    {
      if ( m_Core.Navigating.Solution.IsValidProjectName( editProjectName.Text ) )
      {
        m_Core.Navigating.Solution.RenameProject( m_Project, editProjectName.Text );
      }
    }



    private void comboConfiguration_SelectedIndexChanged( object sender, EventArgs e )
    {
      m_ProjectConfig = m_Settings.Configuration( comboConfiguration.SelectedItem.ToString() );
      if ( m_ProjectConfig == null )
      {
        groupConfigSettings.Enabled = false;
        groupConfigSettings.Text = "Configuration: None selected";
        return;
      }

      groupConfigSettings.Enabled = true;
      groupConfigSettings.Text = "Configuration: " + m_ProjectConfig.Name;

      // cannot delete default config
      btnDeleteConfig.Enabled = ( m_ProjectConfig.Name != "Default" );

      editConfigName.Text         = m_ProjectConfig.Name;
      editPreDefines.Text         = m_ProjectConfig.Defines;
      editDebugStartAddress.Text  = m_ProjectConfig.DebugStartAddressLabel;

      btnApplyChanges.Enabled = false;
    }



    private void btnAddConfig_Click( DecentForms.ControlBase Sender )
    {
      string    newConfigName = editConfigName.Text;

      var existingConfig = m_Settings.Configuration( newConfigName );
      if ( existingConfig != null )
      {
        return;
      }
      ProjectConfig   config = new ProjectConfig();
      config.Name                   = newConfigName;
      config.Defines                = editPreDefines.Text;
      config.DebugStartAddressLabel = editDebugStartAddress.Text;

      foreach ( ProjectElement element in m_Project.Elements )
      {
        ProjectElement.PerConfigSettings    configSettings = element.Settings["Default"];
        ProjectElement.PerConfigSettings    newConfigSettings = new ProjectElement.PerConfigSettings();

        newConfigSettings.PreBuild = configSettings.PreBuild;
        newConfigSettings.CustomBuild = configSettings.CustomBuild;
        newConfigSettings.PostBuild = configSettings.PostBuild;

        element.Settings[newConfigName] = newConfigSettings;
        lock ( element.DocumentInfo.DeducedDependency )
        {
          element.DocumentInfo.DeducedDependency[newConfigName] = new DependencyBuildState();
        }
      }

      m_Settings.Configuration( config.Name, config );
      comboConfiguration.Items.Add( config.Name );
      m_Core.MainForm.mainToolConfig.Items.Add( config.Name );

      Modified = true;
    }



    private bool DebugStartAddressFromControl( out int Address )
    {
      Address = -1;
      string    debugStartAddress = editDebugStartAddress.Text;
      if ( debugStartAddress.StartsWith( "0x" ) )
      {
        if ( debugStartAddress.Length >= 3 )
        {
          Address = System.Convert.ToUInt16( debugStartAddress.Substring( 2 ), 16 );
          return true;
        }
      }
      else if ( debugStartAddress.StartsWith( "$" ) )
      {
        if ( debugStartAddress.Length >= 2 )
        {
          Address = System.Convert.ToUInt16( debugStartAddress.Substring( 1 ), 16 );
          return true;
        }
      }
      ushort     debugStartAddressValue = 0;
      if ( ushort.TryParse( debugStartAddress, out debugStartAddressValue ) )
      {
        Address = debugStartAddressValue;
        return true;
      }
      return false;
    }



    private void btnDeleteConfig_Click( DecentForms.ControlBase Sender )
    {
      string configName = comboConfiguration.SelectedItem.ToString();

      var existingConfig = m_Settings.Configuration( configName );
      if ( existingConfig != null )
      {
        if ( MessageBox.Show( "Are you sure you want to delete configuration '" + configName + "'?", "Delete configuration?", MessageBoxButtons.YesNo ) == DialogResult.Yes )
        {
          m_Settings.DeleteConfiguration( configName );

          foreach ( ProjectElement element in m_Project.Elements )
          {
            element.Settings.Remove( configName );
          }

          comboConfiguration.Items.Remove( configName );
          if ( comboConfiguration.Items.Count > 0 )
          {
            comboConfiguration.SelectedIndex = 0;
          }

          m_Core.MainForm.mainToolConfig.Items.Remove( configName );
          if ( m_Core.MainForm.mainToolConfig.Items.Count > 0 )
          {
            m_Core.MainForm.mainToolConfig.SelectedIndex = 0;
          }
          Modified = true;
        }
      }

    }



    private void btnApplyChanges_Click( DecentForms.ControlBase Sender )
    {
      if ( comboConfiguration.SelectedItem == null )
      {
        return;
      }

      string configName = comboConfiguration.SelectedItem.ToString();

      var existingConfig = m_Settings.Configuration( configName );

      if ( existingConfig != null )
      {
        m_ProjectConfig.Defines                 = editPreDefines.Text;
        m_ProjectConfig.DebugStartAddressLabel  = editDebugStartAddress.Text;
        Modified = true;

        // invalidate elements to they have to be built
        m_Core.MainForm.ProjectConfigChanged();
      }
    }



    private void editPreDefines_TextChanged( object sender, EventArgs e )
    {
      btnApplyChanges.Enabled = true;
    }



    private void editDebugStartAddress_TextChanged( object sender, EventArgs e )
    {
      if ( editDebugStartAddress.Text != m_ProjectConfig.DebugStartAddressLabel )
      {
        btnApplyChanges.Enabled = true;
      }
    }



  }
}
