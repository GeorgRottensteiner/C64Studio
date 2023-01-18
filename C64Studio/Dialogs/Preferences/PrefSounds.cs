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
  public partial class PrefSounds : PrefBase
  {
    public PrefSounds()
    {
      InitializeComponent();
    }



    public PrefSounds( StudioCore Core ) : base( Core )
    {
      _Keywords.AddRange( new string[] { "sound", "play" } );
      InitializeComponent();

      checkPlaySoundCompileSuccessful.Checked   = Core.Settings.PlaySoundOnSuccessfulBuild;
      checkPlaySoundCompileFail.Checked         = Core.Settings.PlaySoundOnBuildFailure;
      checkPlaySoundSearchTextNotFound.Checked  = Core.Settings.PlaySoundOnSearchFoundNoItem;
    }



    private void btnImportSettings_Click( object sender, EventArgs e )
    {

    }



    private void btnExportSettings_Click( object sender, EventArgs e )
    {

    }



    private void checkPlaySoundCompileFail_CheckedChanged( object sender, EventArgs e )
    {
      Core.Settings.PlaySoundOnBuildFailure = checkPlaySoundCompileFail.Checked;
    }



    private void checkPlaySoundCompileSuccessful_CheckedChanged( object sender, EventArgs e )
    {
      Core.Settings.PlaySoundOnSuccessfulBuild = checkPlaySoundCompileSuccessful.Checked;
    }



    private void checkPlaySoundSearchTextNotFound_CheckedChanged( object sender, EventArgs e )
    {
      Core.Settings.PlaySoundOnSearchFoundNoItem = checkPlaySoundSearchTextNotFound.Checked;
    }



  }
}
