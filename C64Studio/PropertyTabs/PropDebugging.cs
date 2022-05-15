using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RetroDevStudio
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

      AddType( CompileTargetType.NONE );
      AddType( CompileTargetType.PLAIN );
      AddType( CompileTargetType.PRG );
      AddType( CompileTargetType.T64 );
      AddType( CompileTargetType.TAP );
      AddType( CompileTargetType.D64 );
      AddType( CompileTargetType.D81 );

      if ( Element.DocumentInfo.Type == ProjectElement.ElementType.ASM_SOURCE )
      {
        AddType( CompileTargetType.CARTRIDGE_8K_BIN );
        AddType( CompileTargetType.CARTRIDGE_8K_CRT );
        AddType( CompileTargetType.CARTRIDGE_16K_BIN );
        AddType( CompileTargetType.CARTRIDGE_16K_CRT );
        AddType( CompileTargetType.CARTRIDGE_MAGICDESK_BIN );
        AddType( CompileTargetType.CARTRIDGE_MAGICDESK_CRT );
        AddType( CompileTargetType.CARTRIDGE_EASYFLASH_BIN );
        AddType( CompileTargetType.CARTRIDGE_EASYFLASH_CRT );
        AddType( CompileTargetType.CARTRIDGE_RGCD_BIN );
        AddType( CompileTargetType.CARTRIDGE_RGCD_CRT );
      }

      foreach ( var configName in Element.DocumentInfo.Project.Settings.GetConfigurationNames() )
      {
        comboConfig.Items.Add( configName );
      }
      comboConfig.SelectedItem = Element.DocumentInfo.Project.Settings.CurrentConfig.Name;

      ProjectElement.PerConfigSettings    configSettings = Element.Settings[Element.DocumentInfo.Project.Settings.CurrentConfig.Name];

      SelectDebugType( configSettings.DebugFileType );
      editDebugCommand.Text             = configSettings.DebugFile;
    }



    private void SelectDebugType( CompileTargetType Type )
    {
      int     itemIndex = 0;
      foreach ( GR.Generic.Tupel<string, CompileTargetType> item in comboDebugFileType.Items )
      {
        if ( item.second == Type )
        {
          comboDebugFileType.SelectedIndex = itemIndex;
          return;
        }
        ++itemIndex;
      }
    }



    private void AddType( CompileTargetType Type )
    {
      comboDebugFileType.Items.Add( new GR.Generic.Tupel<string, CompileTargetType>( GR.EnumHelper.GetDescription( Type ), Type ) );
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

      SelectDebugType( configSettings.DebugFileType );
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

      var debugType = ( (GR.Generic.Tupel<string, CompileTargetType>)comboDebugFileType.SelectedItem ).second;

      if ( configSettings.DebugFileType != debugType )
      {
        configSettings.DebugFileType = debugType;
        Element.DocumentInfo.Project.SetModified();
      }
    }



  }
}
