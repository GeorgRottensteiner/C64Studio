using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;



namespace C64Studio
{
  public partial class PropBuildEvents : PropertyTabs.PropertyTabBase
  {
    ProjectElement                  Element;
    StudioCore                      Core;
    PropertyTabs.PropertyTabBase    BuildEventDetails = null;



    public PropBuildEvents( ProjectElement Element, StudioCore Core )
    {
      this.Element = Element;
      this.Core = Core;
      TopLevel = false;
      Text = "Build Events";
      InitializeComponent();

      comboBuildEvents.Items.Add( new GR.Generic.Tupel<string,C64Studio.ProjectElement.PerConfigSettings.BuildEvent>( "Pre Build", C64Studio.ProjectElement.PerConfigSettings.BuildEvent.PRE ) );
      comboBuildEvents.Items.Add( new GR.Generic.Tupel<string, C64Studio.ProjectElement.PerConfigSettings.BuildEvent>( "Pre Build Chain", C64Studio.ProjectElement.PerConfigSettings.BuildEvent.PRE_BUILD_CHAIN ) );
      comboBuildEvents.Items.Add( new GR.Generic.Tupel<string, C64Studio.ProjectElement.PerConfigSettings.BuildEvent>( "Custom Build", C64Studio.ProjectElement.PerConfigSettings.BuildEvent.CUSTOM ) );
      comboBuildEvents.Items.Add( new GR.Generic.Tupel<string, C64Studio.ProjectElement.PerConfigSettings.BuildEvent>( "Post Build Chain", C64Studio.ProjectElement.PerConfigSettings.BuildEvent.POST_BUILD_CHAIN ) );
      comboBuildEvents.Items.Add( new GR.Generic.Tupel<string, C64Studio.ProjectElement.PerConfigSettings.BuildEvent>( "Post Build", C64Studio.ProjectElement.PerConfigSettings.BuildEvent.POST ) );
      comboBuildEvents.SelectedIndex = 4;

      foreach ( var configName in Element.DocumentInfo.Project.Settings.GetConfigurationNames() )
      {
        comboConfig.Items.Add( configName );
      }
      comboConfig.SelectedItem = Element.DocumentInfo.Project.Settings.CurrentConfig.Name;
    }



    public override void OnClose()
    {
    }



    private void comboBuildEvents_SelectedIndexChanged( object sender, EventArgs e )
    {
      string config = (string)comboConfig.SelectedItem;
      if ( string.IsNullOrEmpty( config ) )
      {
        config = "Default";
      }
      ProjectElement.PerConfigSettings    configSetting = Element.Settings[config];

      if ( BuildEventDetails != null )
      {
        BuildEventDetails.Close();
        BuildEventDetails = null;
      }

      var buildEvent = ( (GR.Generic.Tupel<string,C64Studio.ProjectElement.PerConfigSettings.BuildEvent>)comboBuildEvents.SelectedItem ).second;
      switch ( buildEvent )
      {
        case ProjectElement.PerConfigSettings.BuildEvent.PRE:
        case ProjectElement.PerConfigSettings.BuildEvent.CUSTOM:
        case ProjectElement.PerConfigSettings.BuildEvent.POST:
          BuildEventDetails = new PropBuildEventScript( Element, Core, configSetting, buildEvent );
          break;
        case ProjectElement.PerConfigSettings.BuildEvent.PRE_BUILD_CHAIN:
          BuildEventDetails = new PropBuildEventBuildChain( Element, Core, configSetting.PreBuildChain, config );
          break;
        case ProjectElement.PerConfigSettings.BuildEvent.POST_BUILD_CHAIN:
          BuildEventDetails = new PropBuildEventBuildChain( Element, Core, configSetting.PostBuildChain, config );
          break;
      }

      Core.Theming.ApplyTheme( BuildEventDetails );

      BuildEventDetails.Parent    = this;
      BuildEventDetails.Location  = new Point( 6, 68 );
      BuildEventDetails.Visible   = true;

      GR.Image.DPIHandler.ResizeControlsForDPI( BuildEventDetails );
    }



    private void comboConfig_SelectedIndexChanged( object sender, EventArgs e )
    {
      string    selectedConfig = (string)comboConfig.SelectedItem;

      comboBuildEvents_SelectedIndexChanged( null, e );
    }
  }
}
