using GR.Image;
using RetroDevStudio.Dialogs.Preferences;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RetroDevStudio.Dialogs
{
  public partial class FormPreferences : Form
  {
    private StudioCore            Core = null;

    private List<PrefBase>        _PreferencePanes = new List<PrefBase>();



    public FormPreferences( StudioCore Core, string Key = "" )
    {
      this.Core = Core;
      InitializeComponent();

      DPIHandler.ResizeControlsForDPI( this );

      _PreferencePanes.Add( new PrefTools( Core ) );
      _PreferencePanes.Add( new PrefKeyBindings( Core ) );
      _PreferencePanes.Add( new PrefColorTheme( Core ) );
      _PreferencePanes.Add( new PrefApplication( Core ) );
      _PreferencePanes.Add( new PrefSounds( Core ) );
      _PreferencePanes.Add( new PrefEditorBehaviour( Core ) );
      _PreferencePanes.Add( new PrefASMEditor( Core ) );
      _PreferencePanes.Add( new PrefAssembler( Core ) );
      _PreferencePanes.Add( new PrefBASICEditor( Core ) );
      _PreferencePanes.Add( new PrefBASICKeyBindings( Core ) );
      _PreferencePanes.Add( new PrefBASICParser( Core ) );


      int   curY = 0;
      foreach ( var entry in _PreferencePanes )
      {
        entry.Location = new Point( 0, curY );
        entry.Width = panelPreferences.ClientSize.Width - 2 * System.Windows.Forms.SystemInformation.VerticalScrollBarWidth;
        curY += entry.Height;
        panelPreferences.Controls.Add( entry );

        DPIHandler.ResizeControlsForDPI( entry );
      }
      panelPreferences.SizeChanged += PanelPreferences_SizeChanged;

      Core.Theming.ApplyTheme( this );

      editPreferencesFilter.Text = Key;
    }



    private void PanelPreferences_SizeChanged( object sender, EventArgs e )
    {
      foreach ( var entry in _PreferencePanes )
      {
        entry.Width = panelPreferences.ClientSize.Width - 2 * System.Windows.Forms.SystemInformation.VerticalScrollBarWidth;
      }
    }



    private void btnOK_Click( object sender, EventArgs e )
    {
      /*
      Core.Settings.PlaySoundOnSuccessfulBuild    = checkPlaySoundCompileSuccessful.Checked;
      Core.Settings.PlaySoundOnBuildFailure       = checkPlaySoundCompileFail.Checked;
      Core.Settings.PlaySoundOnSearchFoundNoItem = checkPlaySoundSearchTextNotFound.Checked;

      Core.Settings.TabSize                     = GR.Convert.ToI32( editTabSize.Text );
      if ( ( Core.Settings.TabSize <= 0 )
      ||   ( Core.Settings.TabSize > 100 ) )
      {
        Core.Settings.TabSize = 2;
      }
      Core.Settings.TabConvertToSpaces = checkConvertTabsToSpaces.Checked;*/

      Close();
    }



    private void btnExportAllSettings_Click( object sender, EventArgs e )
    {
      var xml     = new GR.Strings.XMLParser();
      var xmlRoot = new GR.Strings.XMLElement( "RetroDevStudioSettings" );

      xml.AddChild( xmlRoot );

      foreach ( var pane in _PreferencePanes )
      {
        pane.ExportSettings( xmlRoot );
      }

      SaveFileDialog    saveDlg = new SaveFileDialog();

      saveDlg.Title = "Choose a target file";
      saveDlg.Filter = "XML Files|*.xml|All Files|*.*";

      if ( saveDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
      {
        return;
      }

      GR.IO.File.WriteAllText( saveDlg.FileName, xml.ToText() );
    }



    private void btnImportAllSettings_Click( object sender, EventArgs e )
    {
      OpenFileDialog    openDlg = new OpenFileDialog();

      openDlg.Title = "Choose a settings file";
      openDlg.Filter = "XML Files|*.xml|All Files|*.*";

      if ( openDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
      {
        return;
      }

      string    file = GR.IO.File.ReadAllText( openDlg.FileName );
      if ( string.IsNullOrEmpty( file ) )
      {
        return;
      }

      GR.Strings.XMLParser      xml = new GR.Strings.XMLParser();
      if ( !xml.Parse( file ) )
      {
        MessageBox.Show( "The settings file is malformed!", "Malformed file" );
        return;
      }

      var xmlSettings = xml.FindByTypeRecursive( "C64StudioSettings" );
      if ( xmlSettings == null )
      {
        xmlSettings = xml.FindByTypeRecursive( "RetroDevStudioSettings" );
        if ( xmlSettings == null )
        {
          return;
        }
      }

      foreach ( var pane in _PreferencePanes )
      {
        pane.ImportSettings( xmlSettings );
      }
    }



    private void editPreferencesFilter_TextChanged( object sender, EventArgs e )
    {
      string[]    keyWords = editPreferencesFilter.Text.Split( ' ' );

      int   curY = 0;

      panelPreferences.Controls.Clear();

      foreach ( var entry in _PreferencePanes )
      {
        bool    matches = false;
        foreach ( var keyword in keyWords )
        {
          if ( entry.MatchesKeyword( keyword ) )
          {
            matches = true;
            break;
          }
        }

        if ( matches )
        {
          entry.Location = new Point( 0, curY );
          entry.Width = panelPreferences.ClientSize.Width - 2 * System.Windows.Forms.SystemInformation.VerticalScrollBarWidth;
          curY += entry.Height;
          if ( !panelPreferences.Controls.Contains( entry ) )
          {
            panelPreferences.Controls.Add( entry );
          }
        }
        else if ( panelPreferences.Controls.Contains( entry ) ) 
        { 
          panelPreferences.Controls.Remove( entry );
        }
      }

    }



    private void FormPreferences_Load( object sender, EventArgs e )
    {
      if ( Height > Screen.PrimaryScreen.WorkingArea.Height )
      {
        Height = Screen.PrimaryScreen.WorkingArea.Height;
      }
      MinimumSize = Size;
      MaximumSize = new Size( MinimumSize.Width, Screen.PrimaryScreen.WorkingArea.Height );
    }



  }
}
