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
  [Description( "General.Editors" )]
  public partial class DlgPrefEditorBehaviour : DlgPrefBase
  {
    public DlgPrefEditorBehaviour()
    {
      InitializeComponent();
    }



    public DlgPrefEditorBehaviour( StudioCore Core ) : base( Core )
    {
      _Keywords.AddRange( new string[] { "editor", "paint", "draw" } );

      InitializeComponent();
    }



    public override void ApplySettingsToControls()
    {
      checkRightClickIsBGColor.Checked = Core.Settings.BehaviourRightClickIsBGColorPaint;
    }



    public override void ExportSettings( GR.Strings.XMLElement SettingsRoot )
    {
      SettingsRoot.AddChild( "Editor.Behaviour", new GR.Strings.XMLElement( "RightButton", Core.Settings.BehaviourRightClickIsBGColorPaint ? "PaintBackground" : "PickColor" ) );
    }



    public override void ImportSettings( XMLElement SettingsRoot )
    {
      var behaviour = SettingsRoot.FindByType( "Editor.Behaviour.RightButton" );
      if ( behaviour != null )
      {
        if ( behaviour.Content == "PaintBackground" )
        {
          Core.Settings.BehaviourRightClickIsBGColorPaint = true;
        }
        else if ( behaviour.Content == "PickColor" )
        {
          Core.Settings.BehaviourRightClickIsBGColorPaint = false;
        }
      }
    }



    private void checkRightClickIsBGColor_CheckedChanged( object sender, EventArgs e )
    {
      Core.Settings.BehaviourRightClickIsBGColorPaint = checkRightClickIsBGColor.Checked;
    }



  }
}
