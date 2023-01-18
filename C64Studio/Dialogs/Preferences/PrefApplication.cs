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
  public partial class PrefApplication : PrefBase
  {
    public PrefApplication()
    {
      InitializeComponent();

      comboAppMode.SelectedIndex = 0;
    }



    public PrefApplication( StudioCore Core ) : base( Core )
    {
      _Keywords.AddRange( new string[] { "application", "general", "generic", "mode" } );

      InitializeComponent();

      comboAppMode.SelectedIndex        = (int)Core.Settings.StudioAppMode;
      editDefaultOpenSolutionPath.Text  = Core.Settings.DefaultProjectBasePath;
      editMaxMRUEntries.Text            = Core.Settings.MRUMaxCount.ToString();
    }



    private void comboAppMode_SelectedIndexChanged( object sender, EventArgs e )
    {
      Core.Settings.StudioAppMode = (AppMode)comboAppMode.SelectedIndex;
    }



    private void btnImportSettings_Click( object sender, EventArgs e )
    {

    }



    private void btnExportSettings_Click( object sender, EventArgs e )
    {

    }



    private void checkAutoOpenLastSolution_CheckedChanged( object sender, EventArgs e )
    {
      if ( Core.Settings.AutoOpenLastSolution != checkAutoOpenLastSolution.Checked )
      {
        Core.Settings.AutoOpenLastSolution = checkAutoOpenLastSolution.Checked;
      }
    }



    private void editDefaultOpenSolutionPath_TextChanged( object sender, EventArgs e )
    {
      Core.Settings.DefaultProjectBasePath = editDefaultOpenSolutionPath.Text;
    }



    private void btnBrowseDefaultOpenSolutionPath_Click( object sender, EventArgs e )
    {
      FolderBrowserDialog   dlg = new FolderBrowserDialog();

      dlg.SelectedPath = Core.Settings.DefaultProjectBasePath;
      dlg.Description = "Choose default open solution/project path";
      if ( dlg.ShowDialog() == DialogResult.OK )
      {
        Core.Settings.DefaultProjectBasePath = dlg.SelectedPath;
        editDefaultOpenSolutionPath.Text = dlg.SelectedPath;
      }
    }



    private void editMaxMRUEntries_TextChanged( object sender, EventArgs e )
    {
      int     mruCount = GR.Convert.ToI32( editMaxMRUEntries.Text );
      if ( ( mruCount < 1 )
      ||   ( mruCount > 99 ) )
      {
        editMaxMRUEntries.Text = "4";
        mruCount = 4;
      }
      Core.Settings.MRUMaxCount = mruCount;
    }



  }
}
