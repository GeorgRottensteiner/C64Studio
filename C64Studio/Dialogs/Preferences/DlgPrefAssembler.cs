﻿using GR.Strings;
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
  [Description( "Assembler.Warnings" )]
  public partial class DlgPrefAssembler : DlgPrefBase
  {
    public DlgPrefAssembler()
    {
      InitializeComponent();
    }



    public DlgPrefAssembler( StudioCore Core ) : base( Core )
    {
      _Keywords.AddRange( new string[] { "asm", "assembler", "warnings", "hack", "ignore", "label" } );

      InitializeComponent();
    }



    public override void ApplySettingsToControls()
    {
      RefillIgnoredMessageList();
      RefillWarningsAsErrorList();
      RefillC64StudioHackList();

      checkASMAutoTruncateLiteralValues.Checked   = Core.Settings.ASMAutoTruncateLiteralValues;
      checkLabelFileSkipAssemblerIDLabels.Checked = Core.Settings.ASMLabelFileIgnoreAssemblerIDLabels;
    }



    public override void ExportSettings( XMLElement SettingsRoot )
    {
      GR.Strings.XMLElement     xmlSettingRoot = AddOrFind( SettingsRoot, "IgnoredMessages" );
      foreach ( Types.ErrorCode element in Core.Settings.IgnoredWarnings )
      {
        if ( GR.EnumHelper.GetAttributeOfType<UsedForBASICAttribute>( element ) != null )
        {
          // don't export BASIC warnings
          continue;
        }
        var xmlColor = new GR.Strings.XMLElement( "Message" );
        xmlColor.AddAttribute( "Index", ( (int)element ).ToString() );

        xmlSettingRoot.AddChild( xmlColor );
      }

      xmlSettingRoot = AddOrFind( SettingsRoot, "WarningsAsErrors" );
      foreach ( Types.ErrorCode element in Core.Settings.TreatWarningsAsErrors )
      {
        if ( GR.EnumHelper.GetAttributeOfType<UsedForBASICAttribute>( element ) != null )
        {
          // don't export BASIC warnings
          continue;
        }

        var xmlColor = new GR.Strings.XMLElement( "Message" );
        xmlColor.AddAttribute( "Index", ( (int)element ).ToString() );
        xmlSettingRoot.AddChild( xmlColor );
      }

      xmlSettingRoot = AddOrFind( SettingsRoot, "AssemblerHacks" );
      foreach ( var hack in Core.Settings.EnabledC64StudioHacks )
      {
        var xmlHack = new GR.Strings.XMLElement( "Hack" );
        xmlHack.AddAttribute( "Type", hack.ToString() );

        xmlSettingRoot.AddChild( xmlHack );
      }

      SettingsRoot.AddChild( "AutoTruncateLiterals" ).AddAttribute( "Enabled", Core.Settings.ASMAutoTruncateLiteralValues ? "yes" : "no" );
      SettingsRoot.AddChild( "LabelFileIgnoreAssemblerIDLabels" ).AddAttribute( "Enabled", Core.Settings.ASMLabelFileIgnoreAssemblerIDLabels ? "yes" : "no" );
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
              if ( GR.EnumHelper.GetAttributeOfType<UsedForBASICAttribute>( message ) != null )
              {
                // only import non-BASIC warnings
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
              if ( GR.EnumHelper.GetAttributeOfType<UsedForBASICAttribute>( message ) != null )
              {
                // only import non-BASIC warnings
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

      xmlSettingRoot = SettingsRoot.FindByTypeRecursive( "AssemblerHacks" );
      if ( xmlSettingRoot != null )
      {
        Core.Settings.EnabledC64StudioHacks.Clear();
        foreach ( var xmlKey in xmlSettingRoot.ChildElements )
        {
          if ( xmlKey.Type == "Hack" )
          {
            try
            {
              var hack = (AssemblerSettings.Hacks)Enum.Parse( typeof( AssemblerSettings.Hacks ), xmlKey.Attribute( "Type" ) );
              Core.Settings.EnabledC64StudioHacks.Add( hack );
            }
            catch ( Exception ex )
            {
              Core.AddToOutput( "Could not parse element: " + ex.Message + System.Environment.NewLine );
            }
          }
        }
      }

      var xmlAutoTruncateLiterals = SettingsRoot.FindByTypeRecursive( "AutoTruncateLiterals" );
      if ( xmlAutoTruncateLiterals != null )
      {
        Core.Settings.ASMAutoTruncateLiteralValues = IsSettingTrue( xmlAutoTruncateLiterals.Attribute( "Enabled" ) );
      }
      var xmlIgnoreAssemblerIDLabels = SettingsRoot.FindByTypeRecursive( "LabelFileIgnoreAssemblerIDLabels" );
      if ( xmlIgnoreAssemblerIDLabels != null )
      {
        Core.Settings.ASMLabelFileIgnoreAssemblerIDLabels = IsSettingTrue( xmlIgnoreAssemblerIDLabels.Attribute( "Enabled" ) );
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
        &&   ( code < Types.ErrorCode.WARNING_LAST_PLUS_ONE ) )
        {
          if ( GR.EnumHelper.GetAttributeOfType<UsedForBASICAttribute>( code ) != null )
          {
            // only import non-BASIC warnings
            continue;
          }

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
        &&   ( code < Types.ErrorCode.WARNING_LAST_PLUS_ONE ) )
        {
          if ( GR.EnumHelper.GetAttributeOfType<UsedForBASICAttribute>( code ) != null )
          {
            // only import non-BASIC warnings
            continue;
          }

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
      Core.MainForm.RaiseApplicationEvent( new Types.ApplicationEvent( Types.ApplicationEvent.Type.MARK_ALL_ASSEMBLIES_AS_DIRTY ) );
    }



    private void checkLabelFileSkipAssemblerIDLabels_CheckedChanged( object sender, EventArgs e )
    {
      Core.Settings.ASMLabelFileIgnoreAssemblerIDLabels = checkLabelFileSkipAssemblerIDLabels.Checked;
    }



  }
}
