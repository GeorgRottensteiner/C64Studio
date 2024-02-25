using GR.Strings;
using RetroDevStudio.Controls;
using RetroDevStudio.Parser;
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
  public partial class PrefAssembler : PrefBase
  {
    public PrefAssembler()
    {
      InitializeComponent();
    }



    public PrefAssembler( StudioCore Core ) : base( Core )
    {
      _Keywords.AddRange( new string[] { "asm", "assembler", "warnings" } );

      InitializeComponent();

      RefillIgnoredMessageList();
      RefillWarningsAsErrorList();
      RefillC64StudioHackList();

      foreach ( var libPath in Core.Settings.ASMLibraryPaths )
      {
        asmLibraryPathList.Items.Add( libPath );
      }
      checkASMAutoTruncateLiteralValues.Checked   = Core.Settings.ASMAutoTruncateLiteralValues;
      checkLabelFileSkipAssemblerIDLabels.Checked = Core.Settings.ASMLabelFileIgnoreAssemblerIDLabels;
    }



    private void btnImportSettings_Click( object sender, EventArgs e )
    {
      ImportLocalSettings();
    }



    private void btnExportSettings_Click( object sender, EventArgs e )
    {
      SaveLocalSettings();
    }



    public override void ExportSettings( XMLElement SettingsRoot )
    {
      GR.Strings.XMLElement     xmlSettingRoot = new GR.Strings.XMLElement( "IgnoredMessages" );
      SettingsRoot.AddChild( xmlSettingRoot );

      foreach ( Types.ErrorCode element in Core.Settings.IgnoredWarnings )
      {
        var xmlColor = new GR.Strings.XMLElement( "Message" );
        xmlColor.AddAttribute( "Index", ( (int)element ).ToString() );

        xmlSettingRoot.AddChild( xmlColor );
      }

      xmlSettingRoot = new GR.Strings.XMLElement( "WarningsAsErrors" );
      SettingsRoot.AddChild( xmlSettingRoot );

      foreach ( Types.ErrorCode element in Core.Settings.TreatWarningsAsErrors )
      {
        var xmlColor = new GR.Strings.XMLElement( "Message" );
        xmlColor.AddAttribute( "Index", ( (int)element ).ToString() );

        xmlSettingRoot.AddChild( xmlColor );
      }

      xmlSettingRoot = new GR.Strings.XMLElement( "AssemblerHacks" );
      SettingsRoot.AddChild( xmlSettingRoot );

      foreach ( var hack in Core.Settings.EnabledC64StudioHacks )
      {
        var xmlHack = new GR.Strings.XMLElement( "Hack" );
        xmlHack.AddAttribute( "Type", hack.ToString() );

        xmlSettingRoot.AddChild( xmlHack );
      }

      xmlSettingRoot = new GR.Strings.XMLElement( "LibraryPaths" );
      SettingsRoot.AddChild( xmlSettingRoot );

      foreach ( var path in Core.Settings.ASMLibraryPaths )
      {
        xmlSettingRoot.AddChild( "Path", path );
      }

      SettingsRoot.AddChild( "AutoTruncateLiterals" ).AddAttribute( "Enabled", Core.Settings.ASMAutoTruncateLiteralValues ? "yes" : "no" );
      SettingsRoot.AddChild( "LabelFileIgnoreAssemblerIDLabels" ).AddAttribute( "Enabled", Core.Settings.ASMLabelFileIgnoreAssemblerIDLabels ? "yes" : "no" );
    }



    public override void ImportSettings( XMLElement SettingsRoot )
    {
      GR.Strings.XMLElement     xmlSettingRoot = SettingsRoot.FindByTypeRecursive( "IgnoredMessages" );
      if ( xmlSettingRoot != null )
      {
        Core.Settings.IgnoredWarnings.Clear();
        foreach ( var xmlKey in xmlSettingRoot.ChildElements )
        {
          if ( xmlKey.Type == "Message" )
          {
            try
            {
              Types.ErrorCode   message = (Types.ErrorCode)GR.Convert.ToI32( xmlKey.Attribute( "Index" ) );
              Core.Settings.IgnoredWarnings.Add( message );
            }
            catch ( Exception ex )
            {
              Core.AddToOutput( "Could not parse element: " + ex.Message + System.Environment.NewLine );
            }
          }
        }
        RefillIgnoredMessageList();
      }

      xmlSettingRoot = SettingsRoot.FindByTypeRecursive( "WarningsAsErrors" );
      if ( xmlSettingRoot != null )
      {
        Core.Settings.TreatWarningsAsErrors.Clear();
        foreach ( var xmlKey in xmlSettingRoot.ChildElements )
        {
          if ( xmlKey.Type == "Message" )
          {
            try
            {
              Types.ErrorCode   message = (Types.ErrorCode)GR.Convert.ToI32( xmlKey.Attribute( "Index" ) );
              Core.Settings.TreatWarningsAsErrors.Add( message );
            }
            catch ( Exception ex )
            {
              Core.AddToOutput( "Could not parse element: " + ex.Message + System.Environment.NewLine );
            }
          }
        }
        RefillWarningsAsErrorList();
      }

      var xmlAutoTruncateLiterals = SettingsRoot.FindByTypeRecursive( "AutoTruncateLiterals" );
      if ( xmlAutoTruncateLiterals != null )
      {
        Core.Settings.ASMAutoTruncateLiteralValues = IsSettingTrue( xmlAutoTruncateLiterals.Attribute( "Enabled" ) );
        checkASMAutoTruncateLiteralValues.Checked = Core.Settings.ASMAutoTruncateLiteralValues;
      }
      var xmlIgnoreAssemblerIDLabels = SettingsRoot.FindByTypeRecursive( "LabelFileIgnoreAssemblerIDLabels" );
      if ( xmlIgnoreAssemblerIDLabels != null )
      {
        Core.Settings.ASMLabelFileIgnoreAssemblerIDLabels = IsSettingTrue( xmlIgnoreAssemblerIDLabels.Attribute( "Enabled" ) );
        checkLabelFileSkipAssemblerIDLabels.Checked = Core.Settings.ASMLabelFileIgnoreAssemblerIDLabels;
      }

      xmlSettingRoot = SettingsRoot.FindByTypeRecursive( "LibraryPaths" );
      if ( xmlSettingRoot != null )
      {
        asmLibraryPathList.Items.Clear();
        foreach ( var xmlKey in xmlSettingRoot.ChildElements )
        {
          if ( xmlKey.Type == "Path" )
          {
            asmLibraryPathList.Items.Add( xmlKey.Content );
          }
        }
        ApplyLibraryPathsFromList();
      }
    }



    private void ApplyLibraryPathsFromList()
    {
      Core.Settings.ASMLibraryPaths.Clear();
      foreach ( ArrangedItemEntry entry in asmLibraryPathList.Items )
      {
        Core.Settings.ASMLibraryPaths.Add( entry.Text );
      }
    }



    private void RefillC64StudioHackList()
    {
      listHacks.Items.Clear();
      listHacks.BeginUpdate();
      foreach ( AssemblerSettings.Hacks hack in Enum.GetValues( typeof( AssemblerSettings.Hacks ) ) )
      {
        int itemIndex = listHacks.Items.Add( new GR.Generic.Tupel<string, AssemblerSettings.Hacks>( GR.EnumHelper.GetDescription( hack ), hack ) );
        if ( Core.Settings.EnabledC64StudioHacks.ContainsValue( hack ) )
        {
          listHacks.SetItemChecked( itemIndex, true );
        }
      }
      listHacks.EndUpdate();
    }



    private void RefillIgnoredMessageList()
    {
      listIgnoredWarnings.Items.Clear();
      listIgnoredWarnings.BeginUpdate();
      foreach ( Types.ErrorCode code in Enum.GetValues( typeof( Types.ErrorCode ) ) )
      {
        if ( ( code > Types.ErrorCode.WARNING_START )
        && ( code < Types.ErrorCode.WARNING_LAST_PLUS_ONE ) )
        {
          int itemIndex = listIgnoredWarnings.Items.Add( new GR.Generic.Tupel<string, Types.ErrorCode>( GR.EnumHelper.GetDescription( code ), code ) );
          if ( Core.Settings.IgnoredWarnings.ContainsValue( code ) )
          {
            listIgnoredWarnings.SetItemChecked( itemIndex, true );
          }
        }
      }
      listIgnoredWarnings.EndUpdate();
    }



    private void RefillWarningsAsErrorList()
    {
      listWarningsAsErrors.Items.Clear();
      listWarningsAsErrors.BeginUpdate();
      foreach ( Types.ErrorCode code in Enum.GetValues( typeof( Types.ErrorCode ) ) )
      {
        if ( ( code > Types.ErrorCode.WARNING_START )
        && ( code < Types.ErrorCode.WARNING_LAST_PLUS_ONE ) )
        {
          int itemIndex = listWarningsAsErrors.Items.Add( new GR.Generic.Tupel<string, Types.ErrorCode>( GR.EnumHelper.GetDescription( code ), code ) );
          if ( Core.Settings.TreatWarningsAsErrors.ContainsValue( code ) )
          {
            listWarningsAsErrors.SetItemChecked( itemIndex, true );
          }
        }
      }
      listWarningsAsErrors.EndUpdate();
    }



    private void checkASMAutoTruncateLiteralValues_CheckedChanged( object sender, EventArgs e )
    {
      Core.Settings.ASMAutoTruncateLiteralValues = checkASMAutoTruncateLiteralValues.Checked;
    }



    private Controls.ArrangedItemEntry asmLibraryPathList_AddingItem( object sender )
    {
      var newEntry = new ArrangedItemEntry( editASMLibraryPath.Text );
      return newEntry;
    }



    private void asmLibraryPathList_ItemAdded( object sender, ArrangedItemEntry Item )
    {
      ApplyLibraryPathsFromList();
    }



    private void asmLibraryPathList_ItemMoved( object sender, ArrangedItemEntry Item1, ArrangedItemEntry Item2 )
    {
      ApplyLibraryPathsFromList();
    }



    private void asmLibraryPathList_ItemRemoved( object sender, ArrangedItemEntry Item )
    {
      ApplyLibraryPathsFromList();
    }



    private void btmASMLibraryPathBrowse_Click( object sender, EventArgs e )
    {
      FolderBrowserDialog dlg = new FolderBrowserDialog();

      dlg.Description = "Choose Library Folder";
      if ( dlg.ShowDialog() == DialogResult.OK )
      {
        editASMLibraryPath.Text = dlg.SelectedPath;
      }
    }



    private void listIgnoredWarnings_ItemCheck( object sender, ItemCheckEventArgs e )
    {
      GR.Generic.Tupel<string, Types.ErrorCode> item = (GR.Generic.Tupel<string, Types.ErrorCode>)listIgnoredWarnings.Items[e.Index];

      if ( e.NewValue != CheckState.Checked )
      {
        Core.Settings.IgnoredWarnings.Remove( item.second );
      }
      else
      {
        Core.Settings.IgnoredWarnings.Add( item.second );
      }
    }



    private void listWarningsAsErrors_ItemCheck( object sender, ItemCheckEventArgs e )
    {
      GR.Generic.Tupel<string, Types.ErrorCode> item = (GR.Generic.Tupel<string, Types.ErrorCode>)listWarningsAsErrors.Items[e.Index];

      if ( e.NewValue != CheckState.Checked )
      {
        Core.Settings.TreatWarningsAsErrors.Remove( item.second );
      }
      else
      {
        Core.Settings.TreatWarningsAsErrors.Add( item.second );
      }
    }



    private void listHacks_ItemCheck( object sender, ItemCheckEventArgs e )
    {
      GR.Generic.Tupel<string, Parser.AssemblerSettings.Hacks> item = (GR.Generic.Tupel<string, Parser.AssemblerSettings.Hacks>)listHacks.Items[e.Index];

      if ( e.NewValue != CheckState.Checked )
      {
        Core.Settings.EnabledC64StudioHacks.Remove( item.second );
      }
      else
      {
        Core.Settings.EnabledC64StudioHacks.Add( item.second );
      }
    }



    private void checkLabelFileSkipAssemblerIDLabels_CheckedChanged( object sender, EventArgs e )
    {
      Core.Settings.ASMLabelFileIgnoreAssemblerIDLabels = checkLabelFileSkipAssemblerIDLabels.Checked;
    }



  }
}
