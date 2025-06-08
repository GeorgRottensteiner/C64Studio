using GR.Image;
using RetroDevStudio.Dialogs.Preferences;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;



namespace RetroDevStudio.Dialogs
{
  public partial class FormPreferences : Form
  {
    private StudioCore            Core = null;

    private List<DlgPrefBase>        _PreferencePanes = new List<DlgPrefBase>();



    public FormPreferences( StudioCore Core, string Key = "" )
    {
      this.Core = Core;
      InitializeComponent();

      DPIHandler.ResizeControlsForDPI( this );

      _PreferencePanes.Add( new DlgPrefTools( Core ) );
      _PreferencePanes.Add( new DlgPrefKeyBindings( Core ) );
      _PreferencePanes.Add( new DlgPrefColorTheme( Core ) );
      _PreferencePanes.Add( new DlgPrefPalettes( Core ) );
      _PreferencePanes.Add( new DlgPrefApplication( Core ) );
      _PreferencePanes.Add( new DlgPrefSounds( Core ) );
      _PreferencePanes.Add( new DlgPrefEditorBehaviour( Core ) );
      _PreferencePanes.Add( new DlgPrefCachedData( Core ) );
      _PreferencePanes.Add( new DlgPrefASMEditor( Core ) );
      _PreferencePanes.Add( new DlgPrefLibraryPaths( Core ) );
      _PreferencePanes.Add( new DlgPrefAssembler( Core ) );
      _PreferencePanes.Add( new DlgPrefBASICEditor( Core ) );
      _PreferencePanes.Add( new DlgPrefBASICKeyBindings( Core ) );
      _PreferencePanes.Add( new DlgPrefBASICParser( Core ) );
      _PreferencePanes.Add( new DlgPrefBASICWarnings( Core ) );
      _PreferencePanes.Add( new DlgPrefSourceControl( Core ) );

      foreach ( var entry in _PreferencePanes )
      {
        DPIHandler.ResizeControlsForDPI( entry );

        entry.ApplySettingsToControls();
      }

      Core.Theming.ApplyTheme( this );

      editPreferencesFilter.Text = Key;
      ApplyPreferencesFilter();

      var baseNode = treePreferences.Nodes.First();
      if ( baseNode != null )
      { 
        var firstVisiblePrefNode = baseNode.NextVisibleNode;
        if ( ( firstVisiblePrefNode != null )
        &&   ( firstVisiblePrefNode.Tag != null ))
        {
          treePreferences.SelectedNode = firstVisiblePrefNode;
        }
      }
    }



    private void btnOK_Click( DecentForms.ControlBase Sender )
    {
      Close();
    }



    private void btnExportAllSettings_Click( DecentForms.ControlBase Sender )
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



    private void btnImportAllSettings_Click( DecentForms.ControlBase Sender )
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
        Core.Notification.MessageBox( "Malformed file", "The settings file is malformed!" );
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
      ApplyPreferencesFilter();
    }



    private void ApplyPreferencesFilter()
    {
      string[]    keyWords = editPreferencesFilter.Text.Split( ' ' );
      var         matchingPreferences = new List<DlgPrefBase>();
      var         visiblePreferences = new List<DlgPrefBase>();

      foreach ( var entry in _PreferencePanes )
      {
        foreach ( var keyword in keyWords )
        {
          if ( entry.MatchesKeyword( keyword ) )
          {
            visiblePreferences.Add( entry );
            break;
          }
        }
      }

      treePreferences.BeginUpdate();
      treePreferences.Nodes.Clear();

      var categories = new Dictionary<string, DecentForms.TreeView.TreeNode>();

      foreach ( var entry in visiblePreferences )
      {
        string  desc = Lookup.GetDescription( entry.GetType() );
        int dotPos = desc.IndexOf( '.' );
        string  category  = desc.Substring( 0, dotPos );
        string  name      = desc.Substring( dotPos + 1 );

        if ( !categories.ContainsKey( category ) )
        {
          var nodeC = new DecentForms.TreeView.TreeNode( category );
          treePreferences.Nodes.Add( nodeC );
          nodeC.Expand();
          categories.Add( category, nodeC );
        }

        var catNode = categories[category];

        var node = new DecentForms.TreeView.TreeNode( name );
        node.Tag = entry;
        catNode.Nodes.Add( node );
      }
      treePreferences.EndUpdate();
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



    [DllImport( "user32.dll" )]
    public static extern int SendMessage( IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam );



    private void treePreferences_AfterSelect( DecentForms.ControlBase Sender, DecentForms.TreeView.TreeViewEventArgs e )
    {
      if ( ( e.Node == null )
      ||   ( e.Node.Tag == null ) )
      {
        panelPreferences.Controls.Clear();
        btnExportHere.Enabled = false;
        btnImportHere.Enabled = false;
        return;
      }
      var prefBase = e.Node.Tag as DlgPrefBase;
      prefBase.Location = new Point( 0, 0 );
      prefBase.Size     = panelPreferences.ClientSize;

      foreach ( Control control in panelPreferences.Controls )
      {
        control.Visible = false;
      }
      panelPreferences.Controls.Clear();
      panelPreferences.Controls.Add( prefBase );
      prefBase.Visible = true;
      btnExportHere.Enabled = true;
      btnImportHere.Enabled = true;
    }



    private void btnExportCurrentSettings_Click( DecentForms.ControlBase Sender )
    {
      if ( ( treePreferences.SelectedNode == null )
      ||   ( treePreferences.SelectedNode.Tag == null ) )
      {
        return;
      }
      var prefBase = treePreferences.SelectedNode.Tag as DlgPrefBase;

      prefBase.SaveLocalSettings();
    }



    private void btnImportCurrentSettings_Click( DecentForms.ControlBase Sender )
    {
      if ( ( treePreferences.SelectedNode == null )
      ||   ( treePreferences.SelectedNode.Tag == null ) )
      {
        return;
      }
      var prefBase = treePreferences.SelectedNode.Tag as DlgPrefBase;

      prefBase.ImportLocalSettings();
    }



  }
}
