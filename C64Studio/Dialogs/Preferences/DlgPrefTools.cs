using GR.Strings;
using RetroDevStudio.Controls;
using RetroDevStudio.Types;
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
  [Description( "General.Emulators" )]
  public partial class DlgPrefTools : DlgPrefBase
  {
    TextBox   _lastFocusedEdit = null;



    public DlgPrefTools()
    {
      InitializeComponent();
    }



    public DlgPrefTools( StudioCore Core ) : base( Core )
    {
      _Keywords.AddRange( new string[] { "tools", "emulator" }  );

      InitializeComponent();
    }



    public override void ApplySettingsToControls()
    {
      comboToolType.BeginUpdate();
      comboToolType.Items.Clear();
      comboToolType.Items.Add( "<Choose one>" );
      comboToolType.Items.Add( "Assembler" );
      comboToolType.Items.Add( "Emulator" );
      comboToolType.EndUpdate();

      RefillToolInfoList();
      Core.MainForm.RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.EMULATOR_LIST_CHANGED ) );
    }


    public override void ExportSettings( XMLElement SettingsRoot )
    {
      var xmlTools = SettingsRoot.AddChild( "Tools" );

      foreach ( var tool in Core.Settings.ToolInfos )
      {
        var xmlTool = xmlTools.AddChild( "Tool" );

        xmlTool.AddAttribute( "Type", tool.Type.ToString() );

        xmlTool.AddAttribute( "Executable", tool.Filename );
        xmlTool.AddAttribute( "Name", tool.Name );
        xmlTool.AddAttribute( "WorkPath", tool.WorkPath );
        xmlTool.AddAttribute( "PassLabelsToEmulator", tool.PassLabelsToEmulator ? "yes" : "no" );
        xmlTool.AddAttribute( "CartArgs", tool.CartArguments );
        xmlTool.AddAttribute( "DebugArgs", tool.DebugArguments );
        xmlTool.AddAttribute( "PRGArgs", tool.PRGArguments );
        xmlTool.AddAttribute( "TrueDriveOffArgs", tool.TrueDriveOffArguments );
        xmlTool.AddAttribute( "TrueDriveOnArgs", tool.TrueDriveOnArguments );
        /*
        // new format with dynamic arguments?
        var xmlToolArg = xmlTool.AddChild( "Arguments" );
        AddArgument( xmlToolArg, "TrueDriveOn", tool.TrueDriveOnArguments );
        AddArgument( xmlToolArg, "RunCartridge", tool.CartArguments );
        AddArgument( xmlToolArg, "Debug", tool.DebugArguments );
        AddArgument( xmlToolArg, "RunSingleImage", tool.PRGArguments );
        AddArgument( xmlToolArg, "RunContainer", tool.PRGArguments );
        AddArgument( xmlToolArg, "TrueDriveOff", tool.TrueDriveOffArguments );
        AddArgument( xmlToolArg, "TrueDriveOn", tool.TrueDriveOnArguments );
        */
      }
    }



    private void AddArgument( XMLElement XMLToolArguments, string ArgType, string Arguments )
    {
      var xmlToolArg = new XMLElement( "Argument", Arguments );
      XMLToolArguments.AddChild( xmlToolArg );
      xmlToolArg.AddAttribute( "ArgType", ArgType );
    }



    public override void ImportSettings( XMLElement SettingsRoot )
    {
      var xmlTools = SettingsRoot.FindByType( "Tools" );
      if ( xmlTools != null )
      {
        Core.Settings.ToolInfos.Clear();

        foreach ( var xmlTool in xmlTools.ChildElements )
        {
          if ( xmlTool.Type != "Tool" )
          {
            continue;
          }
          var toolInfo = new ToolInfo();

          toolInfo.Name                   = xmlTool.Attribute( "Name" );
          toolInfo.Filename               = xmlTool.Attribute( "Executable" );
          toolInfo.Type                   = (ToolInfo.ToolType)Enum.Parse( typeof( ToolInfo.ToolType ), xmlTool.Attribute( "Type" ), true );
          toolInfo.WorkPath               = xmlTool.Attribute( "WorkPath" );
          toolInfo.CartArguments          = xmlTool.Attribute( "CartArgs" );
          toolInfo.DebugArguments         = xmlTool.Attribute( "DebugArgs" );
          toolInfo.PRGArguments           = xmlTool.Attribute( "PRGArgs" );
          toolInfo.TrueDriveOffArguments  = xmlTool.Attribute( "TrueDriveOffArgs" );
          toolInfo.TrueDriveOnArguments   = xmlTool.Attribute( "TrueDriveOnArgs" );
          toolInfo.PassLabelsToEmulator   = IsSettingTrue( xmlTool.Attribute( "PassLabelsToEmulator" ) );

          Core.Settings.ToolInfos.Add( toolInfo );
        }
      }
    }



    private void RefillToolInfoList()
    {
      alistTools.BeginUpdate();
      alistTools.Items.Clear();
      foreach ( ToolInfo tool in Core.Settings.ToolInfos )
      {
        alistTools.Items.Add( new ArrangedItemEntry( tool.Name ) { Tag = tool } );
      }
      alistTools.EndUpdate();
    }



    private ToolInfo SelectedTool()
    {
      if ( alistTools.SelectedItem == null )
      {
        return null;
      }
      return (ToolInfo)alistTools.SelectedItem.Tag;
    }



    private void editToolName_TextChanged( object sender, EventArgs e )
    {
      var tool = SelectedTool();
      if ( tool == null )
      {
        return;
      }

      bool  emulatorAffected = ( tool.Type == ( ToolInfo.ToolType.EMULATOR ) );

      if ( tool.Name != editToolName.Text )
      {
        tool.Name = editToolName.Text;
        alistTools.SelectedItem.Text = tool.Name;

        if ( emulatorAffected )
        {
          Core.MainForm.RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.EMULATOR_LIST_CHANGED ) );
        }
      }
    }



    private void comboToolType_SelectedIndexChanged( object sender, EventArgs e )
    {
      var tool = SelectedTool();
      if ( tool == null )
      {
        return;
      }

      bool  emulatorAffected = ( tool.Type == ( ToolInfo.ToolType.EMULATOR ) );

      switch ( comboToolType.SelectedIndex )
      {
        case 1:
          tool.Type = ToolInfo.ToolType.ASSEMBLER;
          break;
        case 2:
          tool.Type = ToolInfo.ToolType.EMULATOR;
          emulatorAffected = true;
          break;
        default:
          tool.Type = ToolInfo.ToolType.UNKNOWN;
          break;
      }

      if ( emulatorAffected )
      {
        Core.MainForm.RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.EMULATOR_LIST_CHANGED ) );
      }
    }



    private string FilterString( string Source )
    {
      return Source.Substring( 0, Source.Length - 1 );
    }



    private void btnBrowseTool_Click( DecentForms.ControlBase Sender )
    {
      var tool = SelectedTool();
      if ( tool == null )
      {
        return;
      }

      System.Windows.Forms.OpenFileDialog   dlgTool = new OpenFileDialog();

      dlgTool.Filter = FilterString( Types.Constants.FILEFILTER_EXECUTABLE + Types.Constants.FILEFILTER_ALL );
      if ( labelToolPath.Text.Length != 0 )
      {
        dlgTool.InitialDirectory = GR.Path.GetDirectoryName( labelToolPath.Text );
      }
      if ( dlgTool.ShowDialog() == DialogResult.OK )
      {
        labelToolPath.Text = dlgTool.FileName;

        if ( tool.Filename != labelToolPath.Text )
        {
          tool.Filename = labelToolPath.Text;

          // auto-fill initial entries if nothing is set
          if ( ( string.IsNullOrEmpty( tool.PRGArguments ) )
          &&   ( string.IsNullOrEmpty( tool.DebugArguments ) )
          &&   ( string.IsNullOrEmpty( tool.CartArguments ) )
          &&   ( string.IsNullOrEmpty( tool.TrueDriveOffArguments ) )
          &&   ( string.IsNullOrEmpty( tool.TrueDriveOnArguments ) ) )
          {
            ToolInfo.SetDefaultRunArguments( tool );
            alistTools_SelectedIndexChanged( null, null );
          }
        }
      }
    }



    private Controls.ArrangedItemEntry alistTools_AddingItem( object sender )
    {
      ToolInfo    tool = new ToolInfo();

      tool.Name = "New Tool";

      var item = new ArrangedItemEntry( tool.Name ) { Tag = tool };

      return item;
    }



    private ArrangedItemEntry alistTools_CloningItem( object sender, ArrangedItemEntry Item )
    {
      var item = new ArrangedItemEntry( Item.Text );
      var origTool = (ToolInfo)Item.Tag;

      var tool = new ToolInfo();

      tool.CartArguments          = origTool.CartArguments;
      tool.DebugArguments         = origTool.DebugArguments;
      tool.Filename               = origTool.Filename;
      tool.Name                   = origTool.Name;
      tool.PassLabelsToEmulator   = origTool.PassLabelsToEmulator;
      tool.PRGArguments           = origTool.PRGArguments;
      tool.WorkPath               = origTool.WorkPath;
      tool.TrueDriveOffArguments  = origTool.TrueDriveOffArguments;
      tool.TrueDriveOnArguments   = origTool.TrueDriveOnArguments;
      tool.Type                   = origTool.Type;

      item.Tag = tool;

      return item;
    }



    private void alistTools_ItemAdded( object sender, ArrangedItemEntry Item )
    {
      var tool = (ToolInfo)Item.Tag;
      Core.Settings.ToolInfos.Add( tool );
      if ( tool.Type == ToolInfo.ToolType.EMULATOR )
      {
        Core.MainForm.RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.EMULATOR_LIST_CHANGED ) );
      }
    }



    private void alistTools_ItemMoved( object sender, ArrangedItemEntry Item1, ArrangedItemEntry Item2 )
    {
      Core.Settings.ToolInfos.Clear();
      foreach ( ArrangedItemEntry item in alistTools.Items )
      {
        Core.Settings.ToolInfos.Add( (ToolInfo)item.Tag );
      }
      Core.MainForm.RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.EMULATOR_LIST_CHANGED ) );
    }



    private void alistTools_ItemRemoved( object sender, ArrangedItemEntry Item )
    {
      Core.Settings.ToolInfos.Clear();
      foreach ( ArrangedItemEntry item in alistTools.Items )
      {
        Core.Settings.ToolInfos.Add( (ToolInfo)item.Tag );
      }
    }



    private void alistTools_SelectedIndexChanged( object sender, ArrangedItemEntry Item )
    {
      if ( alistTools.SelectedItem == null )
      {
        editToolPRGArguments.Enabled = false;
        editToolDebugArguments.Enabled = false;
        editToolCartArguments.Enabled = false;
        editToolTrueDriveOnArguments.Enabled = false;
        editToolTrueDriveOffArguments.Enabled = false;
        btnBrowseTool.Enabled = false;
        checkPassLabelsToEmulator.Enabled = false;
        return;
      }
      editToolPRGArguments.Enabled = true;
      editToolDebugArguments.Enabled = true;
      editToolCartArguments.Enabled = true;
      editToolTrueDriveOnArguments.Enabled = true;
      editToolTrueDriveOffArguments.Enabled = true;
      btnBrowseTool.Enabled = true;
      checkPassLabelsToEmulator.Enabled = true;

      ToolInfo    tool = (ToolInfo)alistTools.Items[alistTools.SelectedIndex].Tag;
      if ( tool == null )
      {
        return;
      }

      editToolName.Text           = tool.Name;
      labelToolPath.Text          = tool.Filename;
      editToolPRGArguments.Text   = tool.PRGArguments;
      editToolDebugArguments.Text = tool.DebugArguments;
      editWorkPath.Text           = tool.WorkPath;
      editToolCartArguments.Text  = tool.CartArguments;
      editToolTrueDriveOnArguments.Text = tool.TrueDriveOnArguments;
      editToolTrueDriveOffArguments.Text = tool.TrueDriveOffArguments;
      checkPassLabelsToEmulator.Checked = tool.PassLabelsToEmulator;

      switch ( tool.Type )
      {
        case ToolInfo.ToolType.ASSEMBLER:
          comboToolType.SelectedIndex = 1;
          break;
        case ToolInfo.ToolType.EMULATOR:
          comboToolType.SelectedIndex = 2;
          break;
        default:
          comboToolType.SelectedIndex = 0;
          break;
      }
    }



    private void editWorkPath_TextChanged( object sender, EventArgs e )
    {
      var tool = SelectedTool();
      if ( tool == null )
      {
        return;
      }
      tool.WorkPath = editWorkPath.Text;
    }



    private void editToolPRGArguments_TextChanged( object sender, EventArgs e )
    {
      var tool = SelectedTool();
      if ( tool == null )
      {
        return;
      }
      tool.PRGArguments = editToolPRGArguments.Text;
    }




    private void editToolCartArguments_TextChanged( object sender, EventArgs e )
    {
      var tool = SelectedTool();
      if ( tool == null )
      {
        return;
      }

      tool.CartArguments = editToolCartArguments.Text;
    }




    private void editToolDebugArguments_TextChanged( object sender, EventArgs e )
    {
      var tool = SelectedTool();
      if ( tool == null )
      {
        return;
      }

      tool.DebugArguments = editToolDebugArguments.Text;
    }



    private void editToolTrueDriveOnArguments_TextChanged( object sender, EventArgs e )
    {
      var tool = SelectedTool();
      if ( tool == null )
      {
        return;
      }
      tool.TrueDriveOnArguments = editToolTrueDriveOnArguments.Text;
    }



    private void editToolTrueDriveOffArguments_TextChanged( object sender, EventArgs e )
    {
      var tool = SelectedTool();
      if ( tool == null )
      {
        return;
      }

      tool.TrueDriveOffArguments = editToolTrueDriveOffArguments.Text;
    }



    private void btnMacros_Click( DecentForms.ControlBase Sender )
    {
      var Document = Core.MainForm.ActiveDocumentInfo;

      var formMacros = new FormMacros( Core, Document, _lastFocusedEdit, true, "Macros.Tools" );
      formMacros.ShowDialog();
    }



    private void checkPassLabelsToEmulator_CheckedChanged( object sender, EventArgs e )
    {
      var tool = SelectedTool();
      if ( tool == null )
      {
        return;
      }
      tool.PassLabelsToEmulator = checkPassLabelsToEmulator.Checked;
    }



    private void btnBrowseToolWorkPath_Click( DecentForms.ControlBase Sender )
    {
      var tool = SelectedTool();
      if ( tool == null )
      {
        return;
      }

      var   dlgTool = new FolderBrowserDialog();

      if ( labelToolPath.Text.Length != 0 )
      {
        dlgTool.SelectedPath = GR.Path.GetDirectoryName( editWorkPath.Text );
      }
      if ( dlgTool.ShowDialog() == DialogResult.OK )
      {
        editWorkPath.Text = dlgTool.SelectedPath;
      }
    }



    private void editGotFocus( object sender, EventArgs e )
    {
      _lastFocusedEdit = sender as TextBox;
    }



  }
}
