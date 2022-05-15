using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RetroDevStudio
{
  public partial class PropBuildEventBuildChain : PropertyTabs.PropertyTabBase
  {
    ProjectElement        Element;
    StudioCore            Core;
    BuildChain            BuildChain;
    string                CurrentBuildConfig;



    public PropBuildEventBuildChain( ProjectElement Element, StudioCore Core, BuildChain BuildChain, string CurrentBuildConfig )
    {
      this.Element = Element;
      this.Core = Core;
      this.BuildChain = BuildChain;
      this.CurrentBuildConfig = CurrentBuildConfig;
      TopLevel = false;

      InitializeComponent();

      // Build Chains
      if ( Core.Navigating.Solution != null )
      {
        foreach ( var project in Core.Navigating.Solution.Projects )
        {
          comboBuildChainProject.Items.Add( project.Settings.Name );
        }
        if ( comboBuildChainProject.Items.Count > 0 )
        {
          comboBuildChainProject.SelectedIndex = 0;
        }

        foreach ( var entry in BuildChain.Entries )
        {
          var item = new ArrangedItemEntry();

          item.Text = entry.ProjectName + " - " + entry.Config;
          item.Tag = entry;

          listBuildChainProjects.Items.Add( item );
        }
      }

      checkBuildChainActive.Checked = BuildChain.Active;
    }



    public override void OnClose()
    {
    }



    private void checkBuildChainActive_CheckedChanged( object sender, EventArgs e )
    {
      listBuildChainProjects.Enabled = checkBuildChainActive.Checked;
      labelProject.Enabled = checkBuildChainActive.Checked;
      labelConfig.Enabled = checkBuildChainActive.Checked;
      labelDefines.Enabled = checkBuildChainActive.Checked;
      labelFile.Enabled = checkBuildChainActive.Checked;
      comboBuildChainConfig.Enabled = checkBuildChainActive.Checked;
      comboBuildChainProject.Enabled = checkBuildChainActive.Checked;
      comboBuildChainFile.Enabled = checkBuildChainActive.Checked;
      editBuildChainDefines.Enabled = checkBuildChainActive.Checked;

      if ( BuildChain.Active != checkBuildChainActive.Checked )
      {
        BuildChain.Active = checkBuildChainActive.Checked;
        Element.DocumentInfo.Project.SetModified();
      }
    }



    private ArrangedItemEntry listBuildChainProjects_AddingItem( object sender )
    {
      if ( ( comboBuildChainProject.SelectedIndex == -1 )
      ||   ( comboBuildChainConfig.SelectedIndex == -1 )
      ||   ( comboBuildChainFile.SelectedIndex == -1 ) )
      {
        return null;
      }
      var entry = new BuildChainEntry();
      entry.ProjectName = (string)comboBuildChainProject.SelectedItem;
      entry.Config      = (string)comboBuildChainConfig.SelectedItem;
      entry.DocumentFilename = (string)comboBuildChainFile.SelectedItem;
      entry.PreDefines  = editBuildChainDefines.Text;

      
      var item = new ArrangedItemEntry();

      item.Text = entry.ProjectName + " - " + entry.Config;
      item.Tag = entry;

      return item;
    }



    private void listBuildChainProjects_ItemAdded( object sender, ArrangedItemEntry Item )
    {
      var entry = (BuildChainEntry)Item.Tag;

      BuildChain.AddEntry( entry );
      Element.DocumentInfo.Project.SetModified();
    }



    private void listBuildChainProjects_ItemMoved( object sender, ArrangedItemEntry Item1, ArrangedItemEntry Item2 )
    {
      BuildChain.Entries.Clear();
      foreach ( ArrangedItemEntry item in listBuildChainProjects.Items )
      {
        var entry = (BuildChainEntry)item.Tag;
        BuildChain.Entries.Add( entry );
      }
      Element.DocumentInfo.Project.SetModified();
    }



    private void listBuildChainProjects_ItemRemoved( object sender, ArrangedItemEntry Item )
    {
      var entry = (BuildChainEntry)Item.Tag;
      BuildChain.Entries.Remove( entry );
      Element.DocumentInfo.Project.SetModified();
    }



    private void listBuildChainProjects_SelectedIndexChanged( object sender, ArrangedItemEntry Item )
    {
      if ( listBuildChainProjects.SelectedIndices.Count == 0 )
      {
        return;
      }
      var buildChainEntry = (BuildChainEntry)listBuildChainProjects.SelectedItems[0].Tag;

      comboBuildChainProject.SelectedItem = buildChainEntry.ProjectName;
      comboBuildChainConfig.SelectedItem  = buildChainEntry.Config;
      comboBuildChainFile.SelectedItem    = buildChainEntry.DocumentFilename;
      editBuildChainDefines.Text = buildChainEntry.PreDefines;
    }



    private void editBuildChainDefines_TextChanged( object sender, EventArgs e )
    {
      if ( listBuildChainProjects.SelectedIndices.Count == 0 )
      {
        return;
      }
      var buildChainEntry = (BuildChainEntry)listBuildChainProjects.SelectedItems[0].Tag;
      if ( buildChainEntry.PreDefines != editBuildChainDefines.Text )
      {
        buildChainEntry.PreDefines = editBuildChainDefines.Text;
        Element.DocumentInfo.Project.SetModified();
        Core.MainForm.MarkAsDirty( Element.DocumentInfo );
      }
    }



    private void comboBuildChainProject_SelectedIndexChanged( object sender, EventArgs e )
    {
      comboBuildChainConfig.Items.Clear();
      comboBuildChainFile.Items.Clear();
      if ( comboBuildChainProject.SelectedItem == null )
      {
        return;
      }

      var  projectName = (string)comboBuildChainProject.SelectedItem;
      var project = Core.Navigating.Solution.GetProjectByName( projectName );

      if ( project != null )
      {
        foreach ( var config in project.Settings.GetConfigurationNames() )
        {
          comboBuildChainConfig.Items.Add( config );
        }
        foreach ( var element in project.Elements )
        {
          if ( element.DocumentInfo.Compilable )
          {
            comboBuildChainFile.Items.Add( element.DocumentInfo.DocumentFilename );
          }
        }
        if ( listBuildChainProjects.SelectedIndices.Count == 0 )
        {
          return;
        }
        var buildChainEntry = (BuildChainEntry)listBuildChainProjects.SelectedItems[0].Tag;
        if ( buildChainEntry.ProjectName != projectName )
        {
          buildChainEntry.ProjectName = projectName;

          listBuildChainProjects.SelectedItems[0].Text = buildChainEntry.ProjectName + " - " + buildChainEntry.Config;
          Element.DocumentInfo.Project.SetModified();
          Core.MainForm.MarkAsDirty( Element.DocumentInfo );
          listBuildChainProjects.Update();
        }
      }
    }



    private void comboBuildChainConfig_SelectedIndexChanged( object sender, EventArgs e )
    {
      UpdateAddButton();

      if ( listBuildChainProjects.SelectedIndices.Count == 0 )
      {
        return;
      }
      string    newConfig = (string)comboBuildChainConfig.SelectedItem;
      var buildChainEntry = (BuildChainEntry)listBuildChainProjects.SelectedItems[0].Tag;
      if ( buildChainEntry.Config != newConfig )
      {
        buildChainEntry.Config = newConfig;

        listBuildChainProjects.SelectedItems[0].Text = buildChainEntry.ProjectName + " - " + buildChainEntry.Config;

        Element.DocumentInfo.Project.SetModified();
        Core.MainForm.MarkAsDirty( Element.DocumentInfo );

        listBuildChainProjects.Update();
      }
    }



    private void UpdateAddButton()
    {
      bool    canEnableAddButton = ( ( comboBuildChainConfig.SelectedIndex != -1 )
                                  && ( comboBuildChainProject.SelectedIndex != -1 )
                                  && ( comboBuildChainFile.SelectedIndex != -1 ) );
      if ( canEnableAddButton )
      {
        // do not allow to add current build config
        var projectName = (string)comboBuildChainProject.SelectedItem;
        var project     = Core.Navigating.Solution.GetProjectByName( projectName );
        string config   = (string)comboBuildChainConfig.SelectedItem;
        string file     = (string)comboBuildChainFile.SelectedItem;

        if ( ( project == Element.DocumentInfo.Project )
        &&   ( config == CurrentBuildConfig )
        &&   ( file == Element.DocumentInfo.DocumentFilename ) )
        {
          canEnableAddButton = false;
        }
      }
      listBuildChainProjects.AddButtonEnabled = canEnableAddButton;
    }



    private void comboBuildChainFile_SelectedIndexChanged( object sender, EventArgs e )
    {
      UpdateAddButton();

      if ( listBuildChainProjects.SelectedIndices.Count == 0 )
      {
        return;
      }
      string    newFile = (string)comboBuildChainFile.SelectedItem;
      var buildChainEntry = (BuildChainEntry)listBuildChainProjects.SelectedItems[0].Tag;
      if ( buildChainEntry.DocumentFilename != newFile )
      {
        buildChainEntry.DocumentFilename = newFile;
        Element.DocumentInfo.Project.SetModified();
        Core.MainForm.MarkAsDirty( Element.DocumentInfo );

        listBuildChainProjects.Invalidate();
      }
    }



    private ArrangedItemEntry listBuildChainProjects_CloningItem( object sender, ArrangedItemEntry Item )
    {
      var origItem = (BuildChainEntry)Item.Tag;

      var entry = new BuildChainEntry();
      entry.ProjectName = origItem.ProjectName;
      entry.Config      = origItem.Config;
      entry.DocumentFilename = origItem.DocumentFilename;
      entry.PreDefines  = origItem.PreDefines;

      var item = new ArrangedItemEntry();

      item.Text = entry.ProjectName + " - " + entry.Config;
      item.Tag = entry;

      return item;
    }

  }
}
