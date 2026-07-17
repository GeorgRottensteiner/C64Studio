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
  [Description( "Debugger.Behaviour" )]
  public partial class DlgPrefDebugger : DlgPrefBase
  {
    public DlgPrefDebugger()
    {
      InitializeComponent();
    }



    public DlgPrefDebugger( StudioCore Core ) : base( Core )
    {
      _Keywords.AddRange( new string[] { "debug", "denise", "vice" } );

      InitializeComponent();
    }



    public override void ApplySettingsToControls()
    {
      checkDebuggerDeniseStepOverJMPBranches.Checked  = Core.Settings.DebuggerDeniseStepOverJMPAndBranches;
    }



    public override void ExportSettings( XMLElement SettingsRoot )
    {
      GR.Strings.XMLElement     xmlSettingRoot = new GR.Strings.XMLElement( "Debugger" );
      SettingsRoot.AddChild( xmlSettingRoot );

      xmlSettingRoot.AddChild( "DeniseStepOverJMPAndBranches", Core.Settings.DebuggerDeniseStepOverJMPAndBranches ? "yes" : "no" );
    }



    public override void ImportSettings( XMLElement SettingsRoot )
    {
      GR.Strings.XMLElement     xmlSettingRoot = SettingsRoot.FindByTypeRecursive( "Debugger" );
      if ( xmlSettingRoot == null )
      {
        return;
      }

      foreach ( var xmlKey in xmlSettingRoot.ChildElements )
      {
        if ( xmlKey.Type == "DeniseStepOverJMPAndBranches" )
        {
          Core.Settings.DebuggerDeniseStepOverJMPAndBranches = IsSettingTrue( xmlKey.Content );
        }
      }
    }



    private void checkDebuggerDeniseStepOverJMPBranches_CheckedChanged( object sender, EventArgs e )
    {
      Core.Settings.DebuggerDeniseStepOverJMPAndBranches = checkDebuggerDeniseStepOverJMPBranches.Checked;
      Core.Debugging.Debugger?.SetSetting( DebuggerSetting.STEP_OVER_ALSO_STEPS_OVER_JMP_AND_BRANCHES, Core.Settings.DebuggerDeniseStepOverJMPAndBranches.ToString() );
    }



  }
}
