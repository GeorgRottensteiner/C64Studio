using C64Studio.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace C64Studio
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

      editDebugStartAddress.Text  = m_Settings.DebugStartAddress.ToString();
      editProjectName.Text        = m_Settings.Name;

      foreach ( string configName in m_Settings.Configs.Keys )
      {
        comboConfiguration.Items.Add( configName );
      }
      if ( m_Settings.CurrentConfig == null )
      {
        foreach ( ProjectConfig config in m_Settings.Configs.Values )
        {
          m_Settings.CurrentConfig = config;
          break;
        }
      }
      comboConfiguration.SelectedItem = m_Settings.CurrentConfig.Name;
    }



    private void btnClose_Click( object sender, EventArgs e )
    {
      string    debugStartAddress = editDebugStartAddress.Text;
      if ( debugStartAddress.StartsWith( "0x" ) )
      {
        m_Settings.DebugStartAddress = System.Convert.ToUInt16( debugStartAddress.Substring( 2 ), 16 );
      }
      else if ( debugStartAddress.StartsWith( "$" ) )
      {
        m_Settings.DebugStartAddress = System.Convert.ToUInt16( debugStartAddress.Substring( 1 ), 16 );
      }
      else
      {
        ushort.TryParse( debugStartAddress, out m_Settings.DebugStartAddress );
      }
      Close();
    }



    private void editProjectName_TextChanged( object sender, EventArgs e )
    {
      if ( m_Core.MainForm.m_Solution.IsValidProjectName( editProjectName.Text ) )
      {
        m_Core.MainForm.m_Solution.RenameProject( m_Project, editProjectName.Text );
      }
    }



    private void comboConfiguration_SelectedIndexChanged( object sender, EventArgs e )
    {
      m_ProjectConfig = m_Settings.Configs[comboConfiguration.SelectedItem.ToString()];

      // cannot delete default config
      btnDeleteConfig.Enabled = ( m_ProjectConfig.Name != "Default" );

      editConfigName.Text = m_ProjectConfig.Name;
      editPreDefines.Text = m_ProjectConfig.Defines;

      btnApplyChanges.Enabled = false;
    }



    private void btnAddConfig_Click( object sender, EventArgs e )
    {
      string    newConfigName = editConfigName.Text;

      if ( m_Settings.Configs.ContainsKey( newConfigName ) )
      {
        return;
      }
      ProjectConfig   config = new ProjectConfig();
      config.Name = newConfigName;
      config.Defines = editPreDefines.Text;

      foreach ( ProjectElement element in m_Project.Elements )
      {
        ProjectElement.PerConfigSettings    configSettings = element.Settings["Default"];
        ProjectElement.PerConfigSettings    newConfigSettings = new ProjectElement.PerConfigSettings();

        newConfigSettings.PreBuild = configSettings.PreBuild;
        newConfigSettings.CustomBuild = configSettings.CustomBuild;
        newConfigSettings.PostBuild = configSettings.PostBuild;

        element.Settings[newConfigName] = newConfigSettings;
        element.DocumentInfo.DeducedDependency[newConfigName] = new DependencyBuildState();
      }

      m_Settings.Configs.Add( config.Name, config );
      comboConfiguration.Items.Add( config.Name );
      m_Core.MainForm.mainToolConfig.Items.Add( config.Name );

      Modified = true;
    }



    private void btnDeleteConfig_Click( object sender, EventArgs e )
    {
      string configName = comboConfiguration.SelectedItem.ToString();

      if ( m_Settings.Configs.ContainsKey( configName ) )
      {
        if ( MessageBox.Show( "Are you sure you want to delete configuration '" + configName + "'?", "Delete configuration?", MessageBoxButtons.YesNo ) == DialogResult.Yes )
        {
          m_Settings.Configs.Remove( configName );

          foreach ( ProjectElement element in m_Project.Elements )
          {
            element.Settings.Remove( configName );
          }

          comboConfiguration.Items.Remove( configName );
          Modified = true;
        }
      }

    }



    private void btnApplyChanges_Click( object sender, EventArgs e )
    {
      string configName = comboConfiguration.SelectedItem.ToString();

      if ( m_Settings.Configs.ContainsKey( configName ) )
      {
        m_ProjectConfig.Defines = editPreDefines.Text;
        Modified = true;

        // invalidate elements to they have to be built
        m_Core.MainForm.ProjectConfigChanged();
      }
    }



    private void editPreDefines_TextChanged( object sender, EventArgs e )
    {
      btnApplyChanges.Enabled = true;
    }



  }
}
