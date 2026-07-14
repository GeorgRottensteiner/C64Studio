using GR.Strings;
using RetroDevStudio.Controls;
using RetroDevStudio.Emulators;
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
  [Description( "BASIC.Warnings" )]
  public partial class DlgPrefBASICWarnings : DlgPrefBase
  {
    public DlgPrefBASICWarnings()
    {
      InitializeComponent();
    }



    public DlgPrefBASICWarnings( StudioCore Core ) : base( Core )
    {
      _Keywords.AddRange( new string[] { "basic", "warnings", "ignore" } );

      InitializeComponent();
    }



    public override void ApplySettingsToControls()
    {
      RefillIgnoredMessageList();
      RefillWarningsAsErrorList();
    }



    public override void ExportSettings( XMLElement SettingsRoot )
    {
      GR.Strings.XMLElement     xmlSettingRoot = AddOrFind( SettingsRoot, "IgnoredMessages" );
      foreach ( Types.ErrorCode element in Core.Settings.IgnoredWarnings )
      {
        if ( GR.EnumHelper.GetAttributeOfType<UsedForBASICAttribute>( element ) == null )
        {
          // only export BASIC warnings
          continue;
        }

        var xmlColor = new GR.Strings.XMLElement( "Message" );
        xmlColor.AddAttribute( "Index", ( (int)element ).ToString() );

        xmlSettingRoot.AddChild( xmlColor );
      }

      xmlSettingRoot = AddOrFind( SettingsRoot, "WarningsAsErrors" );
      foreach ( Types.ErrorCode element in Core.Settings.TreatWarningsAsErrors )
      {
        if ( GR.EnumHelper.GetAttributeOfType<UsedForBASICAttribute>( element ) == null )
        {
          // only export BASIC warnings
          continue;
        }

        var xmlColor = new GR.Strings.XMLElement( "Message" );
        xmlColor.AddAttribute( "Index", ( (int)element ).ToString() );

        xmlSettingRoot.AddChild( xmlColor );
      }
    }



    public override void ImportSettings( XMLElement SettingsRoot )
    {
      GR.Strings.XMLElement     xmlSettingRoot = SettingsRoot.FindByTypeRecursive( "IgnoredMessages" );
      if ( xmlSettingRoot != null )
      {
        foreach ( Types.ErrorCode code in Enum.GetValues( typeof( Types.ErrorCode ) ) )
        {
          if ( GR.EnumHelper.GetAttributeOfType<UsedForBASICAttribute>( code ) != null )
          {
            // only import BASIC warnings
            Core.Settings.IgnoredWarnings.Remove( code );
          }
        }
        foreach ( var xmlKey in xmlSettingRoot.ChildElements )
        {
          if ( xmlKey.Type == "Message" )
          {
            try
            {
              Types.ErrorCode   message = (Types.ErrorCode)GR.Convert.ToI32( xmlKey.Attribute( "Index" ) );

              if ( GR.EnumHelper.GetAttributeOfType<UsedForBASICAttribute>( message ) == null )
              {
                // only import BASIC warnings
                continue;
              }
              Core.Settings.IgnoredWarnings.Add( message );
            }
            catch ( Exception ex )
            {
              Core.AddToOutput( "Could not parse element: " + ex.Message + System.Environment.NewLine );
            }
          }
        }
      }

      xmlSettingRoot = SettingsRoot.FindByTypeRecursive( "WarningsAsErrors" );
      if ( xmlSettingRoot != null )
      {
        foreach ( Types.ErrorCode code in Enum.GetValues( typeof( Types.ErrorCode ) ) )
        {
          if ( GR.EnumHelper.GetAttributeOfType<UsedForBASICAttribute>( code ) != null )
          {
            // only import BASIC warnings
            Core.Settings.TreatWarningsAsErrors.Remove( code );
          }
        }
        foreach ( var xmlKey in xmlSettingRoot.ChildElements )
        {
          if ( xmlKey.Type == "Message" )
          {
            try
            {
              Types.ErrorCode   message = (Types.ErrorCode)GR.Convert.ToI32( xmlKey.Attribute( "Index" ) );
              if ( GR.EnumHelper.GetAttributeOfType<UsedForBASICAttribute>( message ) == null )
              {
                // only import BASIC warnings
                continue;
              }
              Core.Settings.TreatWarningsAsErrors.Add( message );
            }
            catch ( Exception ex )
            {
              Core.AddToOutput( "Could not parse element: " + ex.Message + System.Environment.NewLine );
            }
          }
        }
      }
    }



    private void RefillIgnoredMessageList()
    {
      listIgnoredWarnings.Items.Clear();
      listIgnoredWarnings.BeginUpdate();
      foreach ( Types.ErrorCode code in Enum.GetValues( typeof( Types.ErrorCode ) ) )
      {
        if ( ( code > Types.ErrorCode.WARNING_START )
        &&   ( code < Types.ErrorCode.WARNING_LAST_PLUS_ONE ) )
        {
          if ( GR.EnumHelper.GetAttributeOfType<UsedForBASICAttribute>( code ) == null )
          {
            // only import BASIC warnings
            continue;
          }

          int itemIndex = listIgnoredWarnings.Items.Add( GR.EnumHelper.GetDescription( code ) );
          listIgnoredWarnings.Items[itemIndex].Tag = code;
          if ( Core.Settings.IgnoredWarnings.ContainsValue( code ) )
          {
            listIgnoredWarnings.Items[itemIndex].Checked = true;
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
        &&   ( code < Types.ErrorCode.WARNING_LAST_PLUS_ONE ) )
        {
          if ( GR.EnumHelper.GetAttributeOfType<UsedForBASICAttribute>( code ) == null )
          {
            // only import BASIC warnings
            continue;
          }

          int itemIndex = listWarningsAsErrors.Items.Add( GR.EnumHelper.GetDescription( code ) );
          listWarningsAsErrors.Items[itemIndex].Tag = code;
          if ( Core.Settings.TreatWarningsAsErrors.ContainsValue( code ) )
          {
            listWarningsAsErrors.Items[itemIndex].Checked = true;
          }
        }
      }
      listWarningsAsErrors.EndUpdate();
    }



    private void listIgnoredWarnings_ItemCheck( DecentForms.ControlBase sender )
    {
      if ( listIgnoredWarnings.SelectedIndex != -1 )
      {
        Types.ErrorCode code = (Types.ErrorCode)listIgnoredWarnings.Items[listIgnoredWarnings.SelectedIndex].Tag;

        if ( !listIgnoredWarnings.Items[listIgnoredWarnings.SelectedIndex].Checked )
        {
          Core.Settings.IgnoredWarnings.Remove( code );
        }
        else
        {
          Core.Settings.IgnoredWarnings.Add( code );
        } 
      }
    }



    private void listWarningsAsErrors_ItemCheck( DecentForms.ControlBase sender )
    {
      if ( listWarningsAsErrors.SelectedIndex != -1 )
      {
        Types.ErrorCode code = (Types.ErrorCode)listWarningsAsErrors.Items[listWarningsAsErrors.SelectedIndex].Tag;

        if ( !listWarningsAsErrors.Items[listWarningsAsErrors.SelectedIndex].Checked )
        {
          Core.Settings.TreatWarningsAsErrors.Remove( code );
        }
        else
        {
          Core.Settings.TreatWarningsAsErrors.Add( code );
        }
      }
    }



  }
}
