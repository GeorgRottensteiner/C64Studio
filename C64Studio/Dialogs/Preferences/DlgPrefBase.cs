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
  [Description( "Base.Base" )]
  public partial class DlgPrefBase : UserControl
  {
    protected StudioCore      Core = null;

    protected List<string>    _Keywords = new List<string>();



    public DlgPrefBase()
    {
      InitializeComponent();
    }



    public DlgPrefBase( StudioCore Core )
    {
      this.Core = Core;
      InitializeComponent();
    }



    protected XMLElement AddOrFind( XMLElement parent, string childName )
    {
      if ( parent == null )
      {
        return null;
      }
      var existingChild = parent.FindByType( childName );
      if ( existingChild != null )
      {
        return existingChild;
      }
      var newChild = new XMLElement( childName );

      parent.AddChild( newChild );

      return newChild;
    }



    protected void RefreshDisplayOnDocuments()
    {
      if ( ParentForm != null )
      {
        Core.Theming.ApplyTheme( ParentForm );
        ( (FormPreferences)ParentForm ).RefreshDisplayOptions();
      }
      Core.Settings.RefreshDisplayOnAllDocuments( Core );
    }



    public bool MatchesKeyword( string Keyword )
    {
      if ( string.IsNullOrEmpty( Keyword ) )
      {
        return true;
      }
      string  kw = Keyword.ToUpper();
      foreach ( var keyword in _Keywords )
      {
        if ( keyword.ToUpper().Contains( kw ) )
        {
          return true;
        }
      }
      return false;
    }



    public bool SaveLocalSettings()
    {
      SaveFileDialog    saveDlg = new SaveFileDialog();

      saveDlg.Title = "Choose a target file";
      saveDlg.Filter = "XML Files|*.xml|All Files|*.*";

      if ( saveDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
      {
        return false;
      }

      GR.Strings.XMLParser      xml = new GR.Strings.XMLParser();
      GR.Strings.XMLElement     xmlRoot = new GR.Strings.XMLElement( "RetroDevStudioSettings" );

      xml.AddChild( xmlRoot );

      ExportSettings( xmlRoot );

      GR.IO.File.WriteAllText( saveDlg.FileName, xml.ToText() );

      return true;
    }



    public virtual void ExportSettings( GR.Strings.XMLElement SettingsRoot )
    {
    }



    public virtual void ApplySettingsToControls()
    {
    }



    public bool ImportLocalSettings()
    {
      OpenFileDialog    openDlg = new OpenFileDialog();

      openDlg.Title = "Choose a settings file";
      openDlg.Filter = "XML Files|*.xml|All Files|*.*";

      if ( openDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
      {
        return false;
      }

      string    file = GR.IO.File.ReadAllText( openDlg.FileName );
      if ( string.IsNullOrEmpty( file ) )
      {
        return false;
      }

      GR.Strings.XMLParser      xml = new GR.Strings.XMLParser();
      if ( !xml.Parse( file ) )
      {
        return false;
      }

      var xmlSettings = xml.FindByTypeRecursive( "C64StudioSettings" );
      if ( xmlSettings == null )
      {
        xmlSettings = xml.FindByTypeRecursive( "RetroDevStudioSettings" );
        if ( xmlSettings == null )
        {
          return false;
        }
      }
      ImportSettings( xmlSettings );
      return true;
    }



    public virtual void ImportSettings( GR.Strings.XMLElement SettingsRoot )
    {
    }



    protected bool IsSettingTrue( string Value )
    {
      if ( ( !string.IsNullOrEmpty( Value ) )
      &&   ( Value.ToUpper() == "YES" ) )
      {
        return true;
      }
      return false;
    }



  }
}
