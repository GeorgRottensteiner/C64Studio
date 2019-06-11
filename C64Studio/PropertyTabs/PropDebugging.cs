using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace C64Studio
{
  public partial class PropDebugging : PropertyTabs.PropertyTabBase
  {
    ProjectElement        Element;
    StudioCore            Core;



    public PropDebugging( ProjectElement Element, StudioCore Core )
    {
      this.Element = Element;
      this.Core = Core;
      TopLevel = false;
      Text = "Debug";
      InitializeComponent();

      comboDebugFileType.Items.Add( "None" );
      comboDebugFileType.Items.Add( "Plain" );
      comboDebugFileType.Items.Add( "PRG (cbm)" );
      comboDebugFileType.Items.Add( "T64" );
      comboDebugFileType.Items.Add( "8 KB Cartridge (bin)" );
      comboDebugFileType.Items.Add( "8 KB Cartridge (crt)" );
      comboDebugFileType.Items.Add( "16 KB Cartridge (bin)" );
      comboDebugFileType.Items.Add( "16 KB Cartridge (crt)" );
      comboDebugFileType.Items.Add( "D64" );
      comboDebugFileType.Items.Add( "Magic Desk 64 KB Cartridge (bin)" );
      comboDebugFileType.Items.Add( "Magic Desk 64 KB Cartridge (crt)" );
      comboDebugFileType.Items.Add( "TAP" );
      comboDebugFileType.Items.Add( "Easyflash Cartridge (bin)" );
      comboDebugFileType.Items.Add( "Easyflash Cartridge (crt)" );
      comboDebugFileType.Items.Add( "RGCD 64 KB Cartridge (bin)" );
      comboDebugFileType.Items.Add( "RGCD 64 KB Cartridge (crt)" );

      foreach ( var configName in Element.DocumentInfo.Project.Settings.GetConfigurationNames() )
      {
        comboConfig.Items.Add( configName );
      }
      comboConfig.SelectedItem = Element.DocumentInfo.Project.Settings.CurrentConfig.Name;

      ProjectElement.PerConfigSettings    configSettings = Element.Settings[Element.DocumentInfo.Project.Settings.CurrentConfig.Name];

      comboDebugFileType.SelectedIndex  = (int)configSettings.DebugFileType;
      editDebugCommand.Text             = configSettings.DebugFile;
    }



    private void btnMacros_Click( object sender, EventArgs e )
    {
      FormMacros    formMacro = new FormMacros( Core, Element.DocumentInfo, editDebugCommand );

      formMacro.ShowDialog();
    }



    private void comboConfig_SelectedIndexChanged( object sender, EventArgs e )
    {
      string    selectedConfig = comboConfig.SelectedItem.ToString();

      ProjectElement.PerConfigSettings    configSettings = Element.Settings[selectedConfig];

      comboDebugFileType.SelectedIndex = (int)configSettings.DebugFileType;
      editDebugCommand.Text = configSettings.DebugFile;
    }



    private void editDebugCommand_TextChanged( object sender, EventArgs e )
    {
      string    selectedConfig = comboConfig.SelectedItem.ToString();

      ProjectElement.PerConfigSettings    configSettings = Element.Settings[selectedConfig];

      if ( configSettings.DebugFile != editDebugCommand.Text )
      {
        configSettings.DebugFile = editDebugCommand.Text;
        Element.DocumentInfo.Project.SetModified();
      }
    }



    private void comboDebugFileType_SelectedIndexChanged( object sender, EventArgs e )
    {
      string    selectedConfig = comboConfig.SelectedItem.ToString();

      ProjectElement.PerConfigSettings    configSettings = Element.Settings[selectedConfig];

      if ( (int)configSettings.DebugFileType != comboDebugFileType.SelectedIndex )
      {
        configSettings.DebugFileType = (C64Studio.Types.CompileTargetType)comboDebugFileType.SelectedIndex;
        Element.DocumentInfo.Project.SetModified();
      }
    }



  }
}
