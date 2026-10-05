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
      _Keywords.AddRange( new string[] { "editor", "paint", "draw", "select", "selection" } );

      InitializeComponent();
    }



    public override void ApplySettingsToControls()
    {
      checkRightClickIsBGColor.Checked = Core.Settings.BehaviourRightClickIsBGColorPaint;
      checkImageSelectionAsFrame.Checked = Core.Settings.BehaviourImageSelectionAsFrame;
    }



    public override void ExportSettings( GR.Strings.XMLElement SettingsRoot )
    {
      SettingsRoot.AddChild( "Editor.Behaviour", new GR.Strings.XMLElement( "RightButton", Core.Settings.BehaviourRightClickIsBGColorPaint ? "PaintBackground" : "PickColor" ) );
      SettingsRoot.AddChild( "Editor.Behaviour", new GR.Strings.XMLElement( "ImageSelectionAsFrame", Core.Settings.BehaviourImageSelectionAsFrame.ToString().ToLower() ) );
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
      var imageSelection = SettingsRoot.FindByType( "Editor.Behaviour.ImageSelectionAsFrame" );
      if ( imageSelection != null )
      {
        Core.Settings.BehaviourImageSelectionAsFrame = GR.Convert.ToBoolean( imageSelection.Content );
      }
    }



    private void checkRightClickIsBGColor_CheckedChanged( object sender, EventArgs e )
    {
      Core.Settings.BehaviourRightClickIsBGColorPaint = checkRightClickIsBGColor.Checked;
    }



    private void checkImageSelectionAsFrame_CheckedChanged( object sender, EventArgs e )
    {
      Core.Settings.BehaviourImageSelectionAsFrame = checkImageSelectionAsFrame.Checked;
      RefreshDisplayOnDocuments();
    }



  }
}
