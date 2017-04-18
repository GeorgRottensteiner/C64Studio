using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;



namespace C64Studio
{
  public partial class PropBuildEventScript : PropertyTabs.PropertyTabBase
  {
    ProjectElement        Element;
    StudioCore            Core;
    C64Studio.ProjectElement.PerConfigSettings            Settings;
    C64Studio.ProjectElement.PerConfigSettings.BuildEvent Event;



    public PropBuildEventScript( ProjectElement Element, StudioCore Core, C64Studio.ProjectElement.PerConfigSettings Settings, C64Studio.ProjectElement.PerConfigSettings.BuildEvent Event )
    {
      this.Element = Element;
      this.Core = Core;
      this.Settings = Settings;
      this.Event = Event;
      TopLevel = false;
      InitializeComponent();

      switch ( Event )
      {
        case ProjectElement.PerConfigSettings.BuildEvent.PRE:
          editBuildCommand.Text = Settings.PreBuild;
          break;
        case ProjectElement.PerConfigSettings.BuildEvent.CUSTOM:
          editBuildCommand.Text = Settings.CustomBuild;
          break;
        case ProjectElement.PerConfigSettings.BuildEvent.POST:
          editBuildCommand.Text = Settings.PostBuild;
          break;
      }
    }



    public override void OnClose()
    {
    }



    private void btnMacros_Click( object sender, EventArgs e )
    {
      FormMacros    formMacro = new FormMacros( Core, Element.DocumentInfo, editBuildCommand );

      formMacro.ShowDialog();
    }



    private void editBuildCommand_TextChanged( object sender, EventArgs e )
    {
      string command = editBuildCommand.Text;

      switch ( Event )
      {
        case ProjectElement.PerConfigSettings.BuildEvent.PRE:
          if ( Settings.PreBuild != command )
          {
            Settings.PreBuild = command;
          }
          break;
        case ProjectElement.PerConfigSettings.BuildEvent.CUSTOM:
          if ( Settings.CustomBuild != command )
          {
            Settings.CustomBuild = command;
          }
          break;
        case ProjectElement.PerConfigSettings.BuildEvent.POST:
          if ( Settings.PostBuild != command )
          {
            Settings.PostBuild = command;
          }
          break;
      }
    }


  }
}
