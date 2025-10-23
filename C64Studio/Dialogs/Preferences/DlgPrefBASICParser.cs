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
  [Description( "BASIC.Behaviour" )]
  public partial class DlgPrefBASICParser : DlgPrefBase
  {
    public DlgPrefBASICParser()
    {
      InitializeComponent();
    }



    public DlgPrefBASICParser( StudioCore Core ) : base( Core )
    {
      _Keywords.AddRange( new string[] { "basic", "parser" } );

      InitializeComponent();
    }



    public override void ApplySettingsToControls()
    {
      checkBASICStripSpaces.Checked                   = Core.Settings.BASICStripSpaces;
      checkBASICShowControlCodes.Checked              = Core.Settings.BASICShowControlCodesAsChars;
      checkBASICAutoToggleEntryMode.Checked           = Core.Settings.BASICAutoToggleEntryMode;
      checkBASICStripREM.Checked                      = Core.Settings.BASICStripREM;
      checkBASICAutoToggleEntryModeOnPosition.Checked = Core.Settings.BASICAutoToggleEntryModeOnPosition;
      checkBASICAlwaysMappedKeyMode.Checked           = Core.Settings.BASICAlwaysMappedKeyMode;
    }



    public override void ExportSettings( XMLElement SettingsRoot )
    {
      GR.Strings.XMLElement     xmlSettingRoot = new GR.Strings.XMLElement( "BASICEditor" );
      SettingsRoot.AddChild( xmlSettingRoot );

      xmlSettingRoot.AddChild( "StripSpaces", Core.Settings.BASICStripSpaces ? "yes" : "no" );
      xmlSettingRoot.AddChild( "ShowControlCodesAsChars", Core.Settings.BASICShowControlCodesAsChars ? "yes" : "no" );
      xmlSettingRoot.AddChild( "AutoToggleEntryMode", Core.Settings.BASICAutoToggleEntryMode ? "yes" : "no" );
      xmlSettingRoot.AddChild( "AutoToggleEntryModeOnPosition", Core.Settings.BASICAutoToggleEntryModeOnPosition ? "yes" : "no" );
      xmlSettingRoot.AddChild( "StripREM", Core.Settings.BASICStripREM ? "yes" : "no" );
      xmlSettingRoot.AddChild( "AlwaysMappedKeyMode", Core.Settings.BASICAlwaysMappedKeyMode ? "yes" : "no" );
    }



    public override void ImportSettings( XMLElement SettingsRoot )
    {
      GR.Strings.XMLElement     xmlSettingRoot = SettingsRoot.FindByTypeRecursive( "BASICEditor" );
      if ( xmlSettingRoot == null )
      {
        return;
      }

      foreach ( var xmlKey in xmlSettingRoot.ChildElements )
      {
        if ( xmlKey.Type == "StripSpaces" )
        {
          Core.Settings.BASICStripSpaces = IsSettingTrue( xmlKey.Content );
        }
        else if ( xmlKey.Type == "StripREM " )
        {
          Core.Settings.BASICStripREM = IsSettingTrue( xmlKey.Content );
        }
        else if ( xmlKey.Type == "AutoToggleEntryMode" )
        {
          Core.Settings.BASICAutoToggleEntryMode = IsSettingTrue( xmlKey.Content );
        }
        else if ( xmlKey.Type == "AutoToggleEntryModeOnPosition" )
        {
          Core.Settings.BASICAutoToggleEntryModeOnPosition = IsSettingTrue( xmlKey.Content );
        }
        else if ( xmlKey.Type == "ShowControlCodesAsChars" )
        {
          Core.Settings.BASICShowControlCodesAsChars = IsSettingTrue( xmlKey.Content );
        }
        else if ( xmlKey.Type == "AlwaysMappedKeyMode" )
        {
          Core.Settings.BASICAlwaysMappedKeyMode = IsSettingTrue( xmlKey.Content );
        }
      }
    }



    private void checkBASICStripSpaces_CheckedChanged( object sender, EventArgs e )
    {
      Core.Settings.BASICStripSpaces = checkBASICStripSpaces.Checked;
      Core.Compiling.ParserBasic.Settings.StripSpaces = Core.Settings.BASICStripSpaces;
    }



    private void checkBASICStripREM_CheckedChanged( object sender, EventArgs e )
    {
      Core.Settings.BASICStripREM = checkBASICStripREM.Checked;
      Core.Compiling.ParserBasic.Settings.StripREM = Core.Settings.BASICStripREM;
    }



    private void checkBASICShowControlCodes_CheckedChanged( object sender, EventArgs e )
    {
      Core.Settings.BASICShowControlCodesAsChars = checkBASICShowControlCodes.Checked;
    }



    private void checkBASICAutoToggleEntryMode_CheckedChanged( object sender, EventArgs e )
    {
      Core.Settings.BASICAutoToggleEntryMode = checkBASICAutoToggleEntryMode.Checked;
    }



    private void checkBASICAutoToggleEntryModeOnPosition_CheckedChanged( object sender, EventArgs e )
    {
      Core.Settings.BASICAutoToggleEntryModeOnPosition = checkBASICAutoToggleEntryModeOnPosition.Checked;
    }



    private void checkBASICAlwaysMappedKeyMode_CheckedChanged( object sender, EventArgs e )
    {
      Core.Settings.BASICAlwaysMappedKeyMode = checkBASICAlwaysMappedKeyMode.Checked;
    }



  }
}
