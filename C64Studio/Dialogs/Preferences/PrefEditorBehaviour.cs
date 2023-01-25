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
  public partial class PrefEditorBehaviour : PrefBase
  {
    public PrefEditorBehaviour()
    {
      InitializeComponent();
    }



    public PrefEditorBehaviour( StudioCore Core ) : base( Core )
    {
      _Keywords.AddRange( new string[] { "editor", "paint", "draw" } );
      InitializeComponent();

      checkRightClickIsBGColor.Checked = Core.Settings.BehaviourRightClickIsBGColorPaint;
    }



    private void btnImportSettings_Click( object sender, EventArgs e )
    {
      ImportLocalSettings();
    }



    private void btnExportSettings_Click( object sender, EventArgs e )
    {
      SaveLocalSettings();
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
          checkRightClickIsBGColor.Checked = true;
        }
        else if ( behaviour.Content == "PickColor" )
        {
          checkRightClickIsBGColor.Checked = false;
        }
      }
    }



    private void checkRightClickIsBGColor_CheckedChanged( object sender, EventArgs e )
    {
      Core.Settings.BehaviourRightClickIsBGColorPaint = checkRightClickIsBGColor.Checked;
    }



  }
}
