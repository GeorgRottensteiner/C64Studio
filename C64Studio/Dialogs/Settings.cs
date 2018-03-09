using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace C64Studio
{
  public partial class Settings : Form
  {
    public enum TabPage
    {
      TOOLS,
      KEY_BINDINGS,
      GENERAL,
      ERRORS_WARNINGS,
      COLORS
    };



    private StudioCore                      Core = null;

    private System.Windows.Forms.Keys       m_PressedKey = Keys.None;
    private System.Windows.Forms.Keys       m_PressedKeyMapKey = Keys.None;

    private bool                            m_ResizeHackApplied = false;



    public Settings( StudioCore Core, TabPage PageToOpen )
    {
      this.Core = Core;
      InitializeComponent();

      labelFontPreview.Font = new Font( Core.Settings.SourceFontFamily, Core.Settings.SourceFontSize );

      if ( !Core.Settings.BASICUseNonC64Font )
      {
        labelBASICFontPreview.Font = new System.Drawing.Font( Core.MainForm.m_FontC64.Families[0], Core.Settings.SourceFontSize, System.Drawing.GraphicsUnit.Pixel );
      }
      else
      {
        labelBASICFontPreview.Font = new Font( Core.Settings.BASICSourceFontFamily, Core.Settings.BASICSourceFontSize );
      }

      RefillIgnoredMessageList();

      comboToolType.Items.Add( "<Choose one>" );
      comboToolType.Items.Add( "Assembler" );
      comboToolType.Items.Add( "Emulator" );

      RefillBASICKeyMappingList();
      RefillToolInfoList();

      RefillAcceleratorList();

      // make sure empty space is on top
      /*
      ListViewItem itemSC = new ListViewItem( GR.EnumHelper.GetDescription( Types.SyntaxElement.EMPTY_SPACE ) );
      itemSC.Tag = Core.Settings.SyntaxColoring[Types.SyntaxElement.EMPTY_SPACE];
      listColoring.Items.Add( itemSC );*/

      RefillColorList();
      comboElementBG.Items.Add( new Types.ColorSetting( "Auto", 0xffffffff ) );

      AddColor( "Custom", 0xffffffff );

      AddColor( "Black", 0xff000000 );
      AddColor( "White", 0xffffffff );
      AddColor( "Yellow", 0xffffff00 );
      AddColor( "Green", 0xff00ff00 );
      AddColor( "Red", 0xffff0000 );
      AddColor( "Blue", 0xff0000ff );
      AddColor( "Dark Green", 0xff008000 );
      AddColor( "Dark Red", 0xff800000 );
      AddColor( "Dark Blue", 0xff000080 );

      checkPlaySoundCompileSuccessful.Checked = Core.Settings.PlaySoundOnSuccessfulBuild;
      checkPlaySoundCompileFail.Checked       = Core.Settings.PlaySoundOnBuildFailure;
      checkPlaySoundSearchTextNotFound.Checked = Core.Settings.PlaySoundOnSearchFoundNoItem;
      checkConvertTabsToSpaces.Checked        = Core.Settings.TabConvertToSpaces;
      checkAllowTabs.Checked                  = Core.Settings.AllowTabs;
      editTabSize.Text                        = Core.Settings.TabSize.ToString();
      checkBASICUseC64Font.Checked            = !Core.Settings.BASICUseNonC64Font;
      checkBASICStripSpaces.Checked           = Core.Settings.BASICStripSpaces;
      checkBASICShowControlCodes.Checked      = Core.Settings.BASICShowControlCodesAsChars;
      checkASMShowLineNumbers.Checked         = !Core.Settings.ASMHideLineNumbers;
      checkAutoOpenLastSolution.Checked       = Core.Settings.AutoOpenLastSolution;
      checkASMShowCycles.Checked              = Core.Settings.ASMShowCycles;
      checkASMShowSizes.Checked               = Core.Settings.ASMShowBytes;
      checkASMShowMiniMap.Checked             = Core.Settings.ASMShowMiniView;
      checkASMAutoTruncateLiteralValues.Checked = Core.Settings.ASMAutoTruncateLiteralValues;
      checkASMShowAutoComplete.Checked        = Core.Settings.ASMShowAutoComplete;

      comboAppMode.SelectedIndex = (int)Core.Settings.StudioAppMode;

      btnChangeBASICFont.Enabled = !checkBASICUseC64Font.Checked;

      switch ( PageToOpen )
      {
        case TabPage.COLORS:
          tabPreferences.SelectedIndex = 4;
          break;
        case TabPage.ERRORS_WARNINGS:
          tabPreferences.SelectedIndex = 3;
          break;
        case TabPage.GENERAL:
          tabPreferences.SelectedIndex = 0;
          break;
        case TabPage.KEY_BINDINGS:
          tabPreferences.SelectedIndex = 2;
          break;
        case TabPage.TOOLS:
          tabPreferences.SelectedIndex = 1;
          break;
      }

      foreach ( var libPath in Core.Settings.ASMLibraryPaths )
      {
        asmLibraryPathList.Items.Add( libPath );
      }

      //asmLibraryPathList.PerformAutoScale();
      //asmLibraryPathList.PerformLayout();
    }



    private void RefillToolInfoList()
    {
      listTools.Items.Clear();
      foreach ( ToolInfo tool in Core.Settings.ToolInfos )
      {
        listTools.Items.Add( new GR.Generic.Tupel<string, ToolInfo>( tool.Name, tool ) );
      }
    }



    private void RefillAcceleratorList()
    {
      listFunctions.Items.Clear();
      foreach ( Types.Function function in Enum.GetValues( typeof( Types.Function ) ) )
      {
        if ( function == C64Studio.Types.Function.NONE )
        {
          continue;
        }
        ListViewItem itemF = new ListViewItem();

        itemF.Text = GR.EnumHelper.GetDescription( Core.Settings.Functions[function].State );
        itemF.SubItems.Add( Core.Settings.Functions[function].Description );

        AcceleratorKey key = Core.Settings.DetermineAccelerator( function );
        if ( key != null )
        {
          itemF.SubItems.Add( key.Key.ToString() );
        }
        else
        {
          itemF.SubItems.Add( "" );
        }
        itemF.Tag = function;

        listFunctions.Items.Add( itemF );
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
          int itemIndex = listIgnoredWarnings.Items.Add( new GR.Generic.Tupel<string, Types.ErrorCode>( GR.EnumHelper.GetDescription( code ), code ) );
          if ( Core.Settings.IgnoredWarnings.ContainsValue( code ) )
          {
            listIgnoredWarnings.SetItemChecked( itemIndex, true );
          }
        }
      }
      listIgnoredWarnings.EndUpdate();
    }



    private void RefillColorList()
    {
      listColoring.Items.Clear();
      foreach ( Types.ColorableElement element in Core.Settings.SyntaxColoring.Keys )
      {
        if ( element == C64Studio.Types.ColorableElement.LAST_ENTRY )
        {
          continue;
        }
        if ( element >= Types.ColorableElement.FIRST_GUI_ELEMENT )
        {
          // TODO - for now GUI elements not custom drawn (yet)
          break;
        }
        ListViewItem itemSCLocal = new ListViewItem( GR.EnumHelper.GetDescription( element ) );
        itemSCLocal.Tag = Core.Settings.SyntaxColoring[element];
        listColoring.Items.Add( itemSCLocal );
      }

    }



    private void RefillBASICKeyMappingList()
    {
      listBASICKeyMap.Items.Clear();
      foreach ( C64Studio.Types.KeyboardKey realKey in Enum.GetValues( typeof( C64Studio.Types.KeyboardKey ) ) )
      {
        if ( !IsKeyMappable( realKey ) )
        {
          continue;
        }

        ListViewItem    item = new ListViewItem( realKey.ToString() );

        if ( C64Studio.Types.ConstantData.PhysicalKeyInfo.ContainsKey( realKey ) )
        {
          var charInfo = C64Studio.Types.ConstantData.PhysicalKeyInfo[realKey];

          item.Text = charInfo.Normal.Desc;
          item.SubItems.Add( charInfo.Normal.PetSCIIValue.ToString( "X02" ) );
        }
        else
        {
          item.SubItems.Add( "??" );
        }

        var keyMapEntry = FindBASICKeyMapEntry( realKey );
        if ( keyMapEntry != null )
        {
          item.SubItems.Add( keyMapEntry.Key.ToString() );
        }
        else
        {
          item.SubItems.Add( "--" );
        }
        // ?
        item.SubItems.Add( "--" );

        item.Tag = realKey;
        listBASICKeyMap.Items.Add( item );
      }
    }



    private bool IsKeyMappable( Types.KeyboardKey Key )
    {
      if ( ( Key == C64Studio.Types.KeyboardKey.UNDEFINED )
      ||   ( Key == Types.KeyboardKey.LAST_ENTRY )
      ||   ( Key == Types.KeyboardKey.KEY_RESTORE )
      ||   ( Key == Types.KeyboardKey.KEY_SHIFT_LOCK )
      ||   ( Key == Types.KeyboardKey.KEY_COMMODORE )
      ||   ( Key == Types.KeyboardKey.KEY_SHIFT_LEFT )
      ||   ( Key == Types.KeyboardKey.KEY_SHIFT_RIGHT )
      ||   ( Key == Types.KeyboardKey.KEY_CTRL ) )
      {
        return false;
      }
      return true;
    }



    private KeymapEntry FindBASICKeyMapEntry( Types.KeyboardKey RealKey )
    {
      foreach ( var entry in Core.Settings.BASICKeyMap.Keymap )
      {
        if ( entry.Value.KeyboardKey == RealKey )
        {
          return entry.Value;
        }
      }
      return null;
    }



    private void AddColor( string Name, uint Value )
    {
      comboElementBG.Items.Add( new Types.ColorSetting( Name, Value ) );
      comboElementFG.Items.Add( new Types.ColorSetting( Name, Value ) );
    }



    private void btnOK_Click( object sender, EventArgs e )
    {
      Core.Settings.PlaySoundOnSuccessfulBuild    = checkPlaySoundCompileSuccessful.Checked;
      Core.Settings.PlaySoundOnBuildFailure       = checkPlaySoundCompileFail.Checked;
      Core.Settings.PlaySoundOnSearchFoundNoItem = checkPlaySoundSearchTextNotFound.Checked;

      Core.Settings.TabSize                     = GR.Convert.ToI32( editTabSize.Text );
      if ( ( Core.Settings.TabSize <= 0 )
      ||   ( Core.Settings.TabSize > 100 ) )
      {
        Core.Settings.TabSize = 2;
      }
      Core.Settings.TabConvertToSpaces = checkConvertTabsToSpaces.Checked;

      Close();
    }



    private void btnAddTool_Click( object sender, EventArgs e )
    {
      ToolInfo    tool = new ToolInfo();

      tool.Name = "New Tool";
      Core.Settings.ToolInfos.AddLast( tool );

      listTools.Items.Add( new GR.Generic.Tupel<string,ToolInfo>( tool.Name, tool ) );

      if ( tool.Type == ToolInfo.ToolType.EMULATOR )
      {
        Core.MainForm.RaiseApplicationEvent( new C64Studio.Types.ApplicationEvent( C64Studio.Types.ApplicationEvent.Type.EMULATOR_LIST_CHANGED ) );
      }
    }



    private string FilterString( string Source )
    {
      return Source.Substring( 0, Source.Length - 1 );
    }



    private void btnBrowseTool_Click( object sender, EventArgs e )
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
        dlgTool.InitialDirectory = System.IO.Path.GetDirectoryName( labelToolPath.Text );
      }
      if ( dlgTool.ShowDialog() == DialogResult.OK )
      {
        labelToolPath.Text = dlgTool.FileName;

        if ( tool.Filename != labelToolPath.Text )
        {
          tool.Filename = labelToolPath.Text;
        }
      }
    }



    private void listTools_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( listTools.SelectedItem == null )
      {
        editToolPRGArguments.Enabled = false;
        editToolDebugArguments.Enabled = false;
        editToolCartArguments.Enabled = false;
        editToolTrueDriveOnArguments.Enabled = false;
        editToolTrueDriveOffArguments.Enabled = false;
        btnBrowseTool.Enabled = false;
        checkPassLabelsToEmulator.Enabled = false;
        btnCloneTool.Enabled = false;
        return;
      }
      editToolPRGArguments.Enabled = true;
      editToolDebugArguments.Enabled = true;
      editToolCartArguments.Enabled = true;
      editToolTrueDriveOnArguments.Enabled = true;
      editToolTrueDriveOffArguments.Enabled = true;
      btnBrowseTool.Enabled = true;
      btnCloneTool.Enabled = true;
      checkPassLabelsToEmulator.Enabled = true;

      ToolInfo    tool = ( (GR.Generic.Tupel<string,ToolInfo>)listTools.SelectedItem ).second;
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



    private ToolInfo SelectedTool()
    {
      if ( listTools.SelectedItem == null )
      {
        return null;
      }
      var toolInfo = (GR.Generic.Tupel<string, ToolInfo>)listTools.SelectedItem;

      return toolInfo.second;
    }



    private void btnApplyToolSettings_click( object sender, EventArgs e )
    {
      var tool = SelectedTool();
      if ( tool == null )
      {
        return;
      }

      bool  emulatorAffected = ( tool.Type == ( ToolInfo.ToolType.EMULATOR ) );

      tool.Name           = editToolName.Text;
      tool.Filename       = labelToolPath.Text;
      tool.PRGArguments   = editToolPRGArguments.Text;
      tool.DebugArguments = editToolDebugArguments.Text;
      tool.WorkPath       = editWorkPath.Text;
      tool.CartArguments  = editToolCartArguments.Text;
      tool.TrueDriveOnArguments = editToolTrueDriveOnArguments.Text;
      tool.TrueDriveOffArguments = editToolTrueDriveOffArguments.Text;
      tool.PassLabelsToEmulator = checkPassLabelsToEmulator.Checked;

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

      // butt ugly hack to enforce list item text update (didn't update if case changes)
      listTools.Items[listTools.SelectedIndex] = new GR.Generic.Tupel<string, ToolInfo>( "", null );
      listTools.Items[listTools.SelectedIndex] = new GR.Generic.Tupel<string, ToolInfo>( tool.Name, tool );

      if ( emulatorAffected )
      {
        Core.MainForm.RaiseApplicationEvent( new C64Studio.Types.ApplicationEvent( C64Studio.Types.ApplicationEvent.Type.EMULATOR_LIST_CHANGED ) );
      }
    }



    private void btnBrowseToolWorkPath_Click( object sender, EventArgs e )
    {
      var tool = SelectedTool();
      if ( tool == null )
      {
        return;
      }


      System.Windows.Forms.FolderBrowserDialog    dlgFolder = new FolderBrowserDialog();

      if ( dlgFolder.ShowDialog() == DialogResult.OK )
      {
        tool.WorkPath = dlgFolder.SelectedPath;
        editWorkPath.Text = dlgFolder.SelectedPath;
      }
    }



    private void listFunctions_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( listFunctions.SelectedItems.Count == 0 )
      {
        editKeyBinding.Enabled = false;
        btnUnbindKey.Enabled = false;
        btnBindKey.Enabled = false;
        return;
      }
      Types.Function function = (Types.Function)listFunctions.SelectedItems[0].Tag;

      editKeyBinding.Enabled = true;
      btnUnbindKey.Enabled = ( Core.Settings.DetermineAccelerator( function ) != null );
      btnBindKey.Enabled = true;      
    }



    private void editKeyBinding_PreviewKeyDown( object sender, PreviewKeyDownEventArgs e )
    {
      m_PressedKey        = e.KeyData;
      editKeyBinding.Text = e.KeyData.ToString();
      e.IsInputKey        = true;

      //btnBindKey.Enabled = !Core.Settings.Accelerators.ContainsKey( m_PressedKey );
    }



    private void btnBindKey_Click( object sender, EventArgs e )
    {
      if ( listFunctions.SelectedItems.Count == 0 )
      {
        return;
      }
      Types.Function function = (Types.Function)listFunctions.SelectedItems[0].Tag;

      foreach ( var accPair in Core.Settings.Accelerators )
      {
        if ( accPair.Value.Function == function )
        {
          Core.Settings.Accelerators.Remove( accPair.Key, accPair.Value );
          break;
        }
      }

      if ( m_PressedKey != Keys.None )
      {
        AcceleratorKey key = new AcceleratorKey( m_PressedKey, function );
        key.Key = m_PressedKey;
        Core.Settings.Accelerators.Add( key.Key, key );
      }

      listFunctions.SelectedItems[0].SubItems[2].Text = m_PressedKey.ToString();
      btnUnbindKey.Enabled = ( Core.Settings.DetermineAccelerator( function ) != null );

      Core.MainForm.RaiseApplicationEvent( new C64Studio.Types.ApplicationEvent( C64Studio.Types.ApplicationEvent.Type.KEY_BINDINGS_MODIFIED ) );
      RefreshDisplayOnDocuments();
    }



    private void btnRemove_Click( object sender, EventArgs e )
    {
      if ( listTools.SelectedItem == null )
      {
        return;
      }

      var toolInfo = (GR.Generic.Tupel<string, ToolInfo>)listTools.SelectedItem;
      ToolInfo    tool = toolInfo.second;

      listTools.Items.Remove( toolInfo );
      Core.Settings.ToolInfos.Remove( tool );

      if ( tool.Type == ToolInfo.ToolType.EMULATOR )
      {
        Core.MainForm.RaiseApplicationEvent( new C64Studio.Types.ApplicationEvent( C64Studio.Types.ApplicationEvent.Type.EMULATOR_LIST_CHANGED ) );
      }
    }



    private void button1_Click( object sender, EventArgs e )
    {
      string    macroInfo = "";

      bool   error = false;

      macroInfo = "$(Filename) = " + Core.MainForm.FillParameters( "$(Filename)", Core.MainForm.ActiveDocumentInfo, false, out error ) + System.Environment.NewLine;
      macroInfo += "$(FilenameWithoutExtension) = " + Core.MainForm.FillParameters( "$(FilenameWithoutExtension)", Core.MainForm.ActiveDocumentInfo, false, out error ) + System.Environment.NewLine;
      macroInfo += "$(FilePath) = " + Core.MainForm.FillParameters( "$(FilePath)", Core.MainForm.ActiveDocumentInfo, false, out error ) + System.Environment.NewLine;
      macroInfo += "$(BuildTargetFilename) = " + Core.MainForm.FillParameters( "$(BuildTargetFilename)", Core.MainForm.ActiveDocumentInfo, false, out error ) + System.Environment.NewLine;
      macroInfo += "$(BuildTargetFilenameWithoutExtension) = " + Core.MainForm.FillParameters( "$(BuildTargetFilenameWithoutExtension)", Core.MainForm.ActiveDocumentInfo, false, out error ) + System.Environment.NewLine;
      macroInfo += "$(DebugStartAddress) = " + Core.MainForm.FillParameters( "$(DebugStartAddress)", Core.MainForm.ActiveDocumentInfo, false, out error ) + System.Environment.NewLine;
      macroInfo += "$(DebugStartAddressHex) = " + Core.MainForm.FillParameters( "$(DebugStartAddressHex)", Core.MainForm.ActiveDocumentInfo, false, out error ) + System.Environment.NewLine;

      System.Windows.Forms.MessageBox.Show( macroInfo );
    }



    private void RefreshDisplayOnDocuments()
    {
      Core.Settings.RefreshDisplayOnAllDocuments( Core );
    }



    private void RefreshGUIColors()
    {
      // is that enough?
      Core.MainForm.RefreshGUIColors();
    }



    private void btnChooseFont_Click( object sender, EventArgs e )
    {
      System.Windows.Forms.FontDialog fontDialog = new FontDialog();

      fontDialog.Font = labelFontPreview.Font;

      if ( fontDialog.ShowDialog() == DialogResult.OK )
      {
        Core.Settings.SourceFontFamily = fontDialog.Font.FontFamily.Name;
        Core.Settings.SourceFontSize = fontDialog.Font.SizeInPoints;
        labelFontPreview.Font = fontDialog.Font;

        RefreshDisplayOnDocuments();
      }
    }



    private void listColoring_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( listColoring.SelectedItems.Count == 0 )
      {
        panelElementPreview.Invalidate();
        return;
      }
      Types.ColorSetting color = (Types.ColorSetting)listColoring.SelectedItems[0].Tag;

      bool colorFound = false;
      for ( int i = 1; i < comboElementFG.Items.Count; ++i )
      {
        Types.ColorSetting colorCombo = (Types.ColorSetting)comboElementFG.Items[i];

        if ( colorCombo.FGColor == ( color.FGColor | 0xff000000 ) )
        {
          comboElementFG.SelectedIndex = i;
          colorFound = true;
          break;
        }
      }
      if ( !colorFound )
      {
        // Custom
        Types.ColorSetting colorCombo = (Types.ColorSetting)comboElementFG.Items[0];
        colorCombo.FGColor = color.FGColor;
        comboElementFG.SelectedIndex = 0;
      }

      if ( color.Name == "Empty Space" )
      {
        foreach ( Types.ColorSetting color2 in comboElementBG.Items )
        {
          if ( color2.Name == "Auto" )
          {
            comboElementBG.Items.Remove( color2 );
            break;
          }
        }
      }
      else
      {
        if ( ( (Types.ColorSetting)comboElementBG.Items[0] ).Name != "Auto" )
        {
          comboElementBG.Items.Insert( 0, new Types.ColorSetting( "Auto" ) );
        }
      }

      if ( color.BGColorAuto )
      {
        comboElementBG.SelectedIndex = 0;
      }
      else
      {
        colorFound          = false;
        int     startIndex  = 2;
        if ( color.Name == "Empty Space" )
        {
          startIndex = 1;
        }

        for ( int i = startIndex; i < comboElementBG.Items.Count; ++i )
        {
          Types.ColorSetting colorCombo = (Types.ColorSetting)comboElementBG.Items[i];

          if ( colorCombo.FGColor == color.BGColor )
          {
            comboElementBG.SelectedIndex = i;
            colorFound = true;
            break;
          }
        }
        if ( !colorFound )
        {
          // Custom
          Types.ColorSetting colorCombo = (Types.ColorSetting)comboElementBG.Items[1];
          colorCombo.FGColor = color.BGColor;
          if ( color.Name == "Empty Space" )
          {
            comboElementBG.SelectedIndex = 0;
          }
          else
          {
            comboElementBG.SelectedIndex = 1;
          }
        }
      }
      comboElementFG.Invalidate();
      comboElementBG.Invalidate();
      panelElementPreview.Invalidate();
    }



    private void btnChooseFG_Click( object sender, EventArgs e )
    {
      if ( listColoring.SelectedItems.Count == 0 )
      {
        return;
      }
      Types.ColorSetting color = (Types.ColorSetting)listColoring.SelectedItems[0].Tag;

      System.Windows.Forms.ColorDialog colDlg = new ColorDialog();

      colDlg.Color = GR.Color.Helper.FromARGB( color.FGColor );
      if ( colDlg.ShowDialog() == DialogResult.OK )
      {
        Types.ColorSetting comboColor = (Types.ColorSetting)comboElementFG.Items[0];
        comboColor.FGColor = (uint)colDlg.Color.ToArgb();
        color.FGColor = (uint)colDlg.Color.ToArgb();
        comboElementFG.SelectedIndex = 0;
        comboElementFG.Invalidate();
        panelElementPreview.Invalidate();
        RefreshDisplayOnDocuments();
      }
    }



    private void btnChooseBG_Click( object sender, EventArgs e )
    {
      if ( listColoring.SelectedItems.Count == 0 )
      {
        return;
      }
      Types.ColorSetting color = (Types.ColorSetting)listColoring.SelectedItems[0].Tag;

      System.Windows.Forms.ColorDialog colDlg = new ColorDialog();

      colDlg.Color = GR.Color.Helper.FromARGB( color.BGColor );
      if ( colDlg.ShowDialog() == DialogResult.OK )
      {
        Types.ColorSetting comboColor = (Types.ColorSetting)comboElementBG.Items[1];
        comboColor.FGColor = (uint)colDlg.Color.ToArgb();
        color.BGColor = (uint)colDlg.Color.ToArgb();
        color.BGColorAuto = false;
        comboElementBG.SelectedIndex = 1;
        comboElementBG.Invalidate();
        panelElementPreview.Invalidate();
        RefreshDisplayOnDocuments();
      }
    }



    private void comboElementFG_DrawItem( object sender, DrawItemEventArgs e )
    {
      e.DrawBackground();
      if ( e.Index == -1 )
      {
        return;
      }
      Types.ColorSetting   color = (Types.ColorSetting)comboElementFG.Items[e.Index];

      System.Drawing.Rectangle colorBox = new Rectangle( e.Bounds.Left + 2, e.Bounds.Top + 2, 50, e.Bounds.Height - 4 );
      
      e.Graphics.FillRectangle( new System.Drawing.SolidBrush( GR.Color.Helper.FromARGB( color.FGColor ) ), colorBox );
      e.Graphics.DrawRectangle( System.Drawing.SystemPens.WindowFrame, colorBox );

      e.Graphics.DrawString( color.Name, comboElementFG.Font, new System.Drawing.SolidBrush( e.ForeColor ), e.Bounds.Left + 60, e.Bounds.Top + 2 );
    }



    private void comboElementBG_DrawItem( object sender, DrawItemEventArgs e )
    {
      e.DrawBackground();
      if ( e.Index == -1 )
      {
        return;
      }
      Types.ColorSetting color = (Types.ColorSetting)comboElementBG.Items[e.Index];
      if ( color.Name == "Auto" )
      {
        color = new C64Studio.Types.ColorSetting( "Auto", Core.Settings.SyntaxColoring[C64Studio.Types.ColorableElement.EMPTY_SPACE].BGColor, Core.Settings.SyntaxColoring[C64Studio.Types.ColorableElement.EMPTY_SPACE].BGColor );
      }
      System.Drawing.Rectangle colorBox = new Rectangle( e.Bounds.Left + 2, e.Bounds.Top + 2, 50, e.Bounds.Height - 4 );

      e.Graphics.FillRectangle( new System.Drawing.SolidBrush( GR.Color.Helper.FromARGB( color.FGColor ) ), colorBox );
      e.Graphics.DrawRectangle( System.Drawing.SystemPens.WindowFrame, colorBox );

      e.Graphics.DrawString( color.Name, comboElementBG.Font, new System.Drawing.SolidBrush( e.ForeColor ), e.Bounds.Left + 60, e.Bounds.Top + 2 );
    }



    private void ColorsChanged( Types.ColorableElement Color )
    {
      if ( Color < Types.ColorableElement.FIRST_GUI_ELEMENT )
      {
        RefreshDisplayOnDocuments();
      }
      else
      {
        RefreshGUIColors();
      }
    }



    private void comboElementFG_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( listColoring.SelectedItems.Count == 0 )
      {
        panelElementPreview.Invalidate();
        return;
      }
      Types.ColorSetting color = (Types.ColorSetting)listColoring.SelectedItems[0].Tag;
      Types.ColorSetting comboColor = (Types.ColorSetting)comboElementFG.SelectedItem;

      if ( color.FGColor != comboColor.FGColor )
      {
        color.FGColor = comboColor.FGColor;
        panelElementPreview.Invalidate();

        ColorsChanged( (Types.ColorableElement)listColoring.SelectedIndices[0] );
      }
    }



    private void comboElementBG_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( listColoring.SelectedItems.Count == 0 )
      {
        panelElementPreview.Invalidate();
        return;
      }
      Types.ColorSetting color = (Types.ColorSetting)listColoring.SelectedItems[0].Tag;
      Types.ColorSetting comboColor = (Types.ColorSetting)comboElementBG.SelectedItem;

      bool    changed = false;

      if ( ( comboElementBG.SelectedIndex == 0 ) != color.BGColorAuto )
      {
        changed = true;
      }
      if ( ( comboElementBG.SelectedIndex > 0 )
      &&   ( ( color.BGColorAuto )
      ||     ( color.BGColor != comboColor.FGColor ) ) )
      {
        changed = true;
      }
      if ( changed )
      {
        if ( comboElementBG.SelectedIndex == 0 )
        {
          color.BGColorAuto = true;
        }
        else
        {
          color.BGColorAuto = false;
          color.BGColor = comboColor.FGColor;
        }
        panelElementPreview.Invalidate();
        RefreshDisplayOnDocuments();
      }
    }



    private void panelElementPreview_Paint( object sender, PaintEventArgs e )
    {
      if ( listColoring.SelectedItems.Count == 0 )
      {
        e.Graphics.FillRectangle( System.Drawing.SystemBrushes.Window, e.ClipRectangle );
        return;
      }
      Types.ColorSetting color = (Types.ColorSetting)listColoring.SelectedItems[0].Tag;

      // Set format of string.
      StringFormat drawFormat = new StringFormat();
      drawFormat.Alignment = StringAlignment.Center;
      drawFormat.LineAlignment = StringAlignment.Center;


      if ( color.BGColorAuto )
      {
        Types.ColorSetting bgColor = Core.Settings.SyntaxColoring[C64Studio.Types.ColorableElement.EMPTY_SPACE];
        e.Graphics.FillRectangle( new System.Drawing.SolidBrush( GR.Color.Helper.FromARGB( bgColor.BGColor ) ), panelElementPreview.ClientRectangle );
      }
      else
      {
        e.Graphics.FillRectangle( new System.Drawing.SolidBrush( GR.Color.Helper.FromARGB( color.BGColor ) ), panelElementPreview.ClientRectangle );
      }

      e.Graphics.DrawString( "Sample Text", 
                             panelElementPreview.Font, 
                             new System.Drawing.SolidBrush( GR.Color.Helper.FromARGB( color.FGColor ) ),
                             new System.Drawing.RectangleF( 0.0f, 0.0f, (float)panelElementPreview.ClientRectangle.Width, (float)panelElementPreview.ClientRectangle.Height ),
                             drawFormat );
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



    private void btnUnbindKey_Click( object sender, EventArgs e )
    {
      if ( listFunctions.SelectedItems.Count == 0 )
      {
        return;
      }
      Types.Function function = (Types.Function)listFunctions.SelectedItems[0].Tag;
      foreach ( var accPair in Core.Settings.Accelerators )
      {
        if ( accPair.Value.Function == function )
        {
          Core.Settings.Accelerators.Remove( accPair.Key, accPair.Value );

          Core.MainForm.RaiseApplicationEvent( new C64Studio.Types.ApplicationEvent( C64Studio.Types.ApplicationEvent.Type.KEY_BINDINGS_MODIFIED ) );
          break;
        }
      }

      listFunctions.SelectedItems[0].SubItems[2].Text = "";
      btnUnbindKey.Enabled = false;
    }



    private void checkBASICUseC64Font_CheckedChanged( object sender, EventArgs e )
    {
      if ( Core.Settings.BASICUseNonC64Font != !checkBASICUseC64Font.Checked )
      {
        Core.Settings.BASICUseNonC64Font = !checkBASICUseC64Font.Checked;

        if ( !Core.Settings.BASICUseNonC64Font )
        {
          labelBASICFontPreview.Font = new System.Drawing.Font( Core.MainForm.m_FontC64.Families[0], Core.Settings.SourceFontSize, System.Drawing.GraphicsUnit.Pixel );
          btnChangeBASICFont.Enabled = false;
        }
        else
        {
          labelBASICFontPreview.Font = new Font( Core.Settings.BASICSourceFontFamily, Core.Settings.BASICSourceFontSize );
          btnChangeBASICFont.Enabled = true;
        }
        RefreshDisplayOnDocuments();
      }
    }



    private void btnChooseBASICFont_Click( object sender, EventArgs e )
    {
      System.Windows.Forms.FontDialog fontDialog = new FontDialog();

      fontDialog.Font = labelBASICFontPreview.Font;

      if ( fontDialog.ShowDialog() == DialogResult.OK )
      {
        Core.Settings.BASICSourceFontFamily = fontDialog.Font.FontFamily.Name;
        Core.Settings.BASICSourceFontSize   = fontDialog.Font.SizeInPoints;
        labelBASICFontPreview.Font              = new Font( Core.Settings.BASICSourceFontFamily, Core.Settings.BASICSourceFontSize );

        RefreshDisplayOnDocuments();
      }
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
        Core.MainForm.RaiseApplicationEvent( new C64Studio.Types.ApplicationEvent( C64Studio.Types.ApplicationEvent.Type.EMULATOR_LIST_CHANGED ) );
      }
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



    private void editWorkPath_TextChanged( object sender, EventArgs e )
    {
      var tool = SelectedTool();
      if ( tool == null )
      {
        return;
      }
      tool.WorkPath = editWorkPath.Text;
    }



    private void listBASICKeyMap_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( listBASICKeyMap.SelectedItems.Count == 0 )
      {
        editBASICKeyMapBinding.Enabled = false;
        btnUnbindBASICKeyMapBinding.Enabled = false;
        btnBindBASICKeyMapBinding.Enabled = false;
        return;
      }
      var    realKey = (Types.KeyboardKey)listBASICKeyMap.SelectedItems[0].Tag;

      var keyMapEntry = FindBASICKeyMapEntry( realKey );
      btnUnbindBASICKeyMapBinding.Enabled = ( keyMapEntry != null );
      editBASICKeyMapBinding.Enabled = true;
      btnBindBASICKeyMapBinding.Enabled = true;      
    }



    private void btnBindBASICKeyMapBinding_Click( object sender, EventArgs e )
    {
      if ( listBASICKeyMap.SelectedItems.Count == 0 )
      {
        return;
      }
      var   realKey = (Types.KeyboardKey)listBASICKeyMap.SelectedItems[0].Tag;
      var   keyMapEntry = FindBASICKeyMapEntry( realKey );
      var   physicalKeyInfo = C64Studio.Types.ConstantData.PhysicalKeyInfo[realKey];

      if ( ( m_PressedKeyMapKey != Keys.None )
      &&   ( ( keyMapEntry == null )
      ||     ( m_PressedKeyMapKey != keyMapEntry.Key ) ) )
      {
        restart: ;
        foreach ( var keyInfo in Core.Settings.BASICKeyMap.Keymap )
        {
          if ( keyInfo.Value.KeyboardKey == realKey )
          {
            Core.Settings.BASICKeyMap.Keymap.Remove( keyInfo.Key );
            goto restart;
          }
        }
        Core.Settings.BASICKeyMap.Keymap.Remove( m_PressedKeyMapKey );

        keyMapEntry = Core.Settings.BASICKeyMap.AllKeyInfos[realKey];
        keyMapEntry.Key = m_PressedKeyMapKey;
        Core.Settings.BASICKeyMap.Keymap.Add( m_PressedKeyMapKey, keyMapEntry );
        

        listBASICKeyMap.SelectedItems[0].SubItems[2].Text = m_PressedKeyMapKey.ToString();
        btnUnbindBASICKeyMapBinding.Enabled = true;
      }
    }



    private void btnUnbindBASICKeyMapBinding_Click( object sender, EventArgs e )
    {
      if ( listBASICKeyMap.SelectedItems.Count == 0 )
      {
        return;
      }
      var    realKey = (Types.KeyboardKey)listBASICKeyMap.SelectedItems[0].Tag;

      restart:;
      foreach ( var keyInfo in Core.Settings.BASICKeyMap.Keymap )
      {
        if ( keyInfo.Value.KeyboardKey == realKey )
        {
          Core.Settings.BASICKeyMap.Keymap.Remove( keyInfo.Key );
          goto restart;
        }
      }
      Core.Settings.BASICKeyMap.AllKeyInfos[realKey].Key = Keys.None;
      listBASICKeyMap.SelectedItems[0].SubItems[2].Text = "--";
      btnUnbindBASICKeyMapBinding.Enabled = false;
    }



    private void editKeyMapBinding_PreviewKeyDown( object sender, PreviewKeyDownEventArgs e )
    {
      m_PressedKeyMapKey      = e.KeyData;
      editBASICKeyMapBinding.Text  = e.KeyData.ToString();
      e.IsInputKey = true;
    }



    private void checkBASICStripSpaces_CheckedChanged( object sender, EventArgs e )
    {
      Core.Settings.BASICStripSpaces        = checkBASICStripSpaces.Checked;
      Core.Compiling.ParserBasic.Settings.StripSpaces = Core.Settings.BASICStripSpaces;
    }



    private void editTabSize_TextChanged( object sender, EventArgs e )
    {
      int     tabSize = GR.Convert.ToI32( editTabSize.Text );

      if ( tabSize >= 1 )
      {
        Core.Settings.TabSize = tabSize;
        RefreshDisplayOnDocuments();
      }
    }



    private void checkConvertTabsToSpaces_CheckedChanged( object sender, EventArgs e )
    {
      if ( Core.Settings.TabConvertToSpaces != checkConvertTabsToSpaces.Checked )
      {
        Core.Settings.TabConvertToSpaces = checkConvertTabsToSpaces.Checked;
        if ( Core.Settings.TabConvertToSpaces )
        {
          checkAllowTabs.Checked = false;
        }
        RefreshDisplayOnDocuments();
      }
    }



    private void checkAllowTabs_CheckedChanged( object sender, EventArgs e )
    {
      if ( Core.Settings.AllowTabs != checkAllowTabs.Checked )
      {
        Core.Settings.AllowTabs = checkAllowTabs.Checked;
        if ( Core.Settings.AllowTabs )
        {
          checkConvertTabsToSpaces.Checked = false;
        }
        RefreshDisplayOnDocuments();
      }
    }



    private void checkASMShowLineNumbers_CheckedChanged( object sender, EventArgs e )
    {
      if ( Core.Settings.ASMHideLineNumbers == checkASMShowLineNumbers.Checked )
      {
        Core.Settings.ASMHideLineNumbers = !checkASMShowLineNumbers.Checked;
        RefreshDisplayOnDocuments();
      }
    }



    private void checkOpenLastSolution_CheckedChanged( object sender, EventArgs e )
    {
      if ( Core.Settings.AutoOpenLastSolution != checkAutoOpenLastSolution.Checked )
      {
        Core.Settings.AutoOpenLastSolution = checkAutoOpenLastSolution.Checked;
      }
    }



    private void btnSetDefaultsKeyBinding_Click( object sender, EventArgs e )
    {
      Core.Settings.SetDefaultKeyBinding();

      for ( int i = 0; i < listFunctions.Items.Count; ++i )
      {
        Types.Function function = (Types.Function)listFunctions.Items[i].Tag;

        bool  foundEntry = false;
        foreach ( var accPair in Core.Settings.Accelerators )
        {
          if ( accPair.Value.Function == function )
          {
            listFunctions.Items[i].SubItems[2].Text = accPair.Key.ToString();
            foundEntry = true;
            break;
          }
        }
        if ( !foundEntry )
        {
          listFunctions.Items[i].SubItems[2].Text = "";
        }
      }

      if ( listFunctions.SelectedItems.Count != 0 )
      {
        btnUnbindKey.Enabled = ( Core.Settings.DetermineAccelerator( (Types.Function)listFunctions.SelectedItems[0].Tag ) != null );
      }
      Core.MainForm.RaiseApplicationEvent( new C64Studio.Types.ApplicationEvent( C64Studio.Types.ApplicationEvent.Type.KEY_BINDINGS_MODIFIED ) );
    }



    private void btnSetDefaultsColors_Click( object sender, EventArgs e )
    {
      Core.Settings.SetDefaultColors();
      listColoring_SelectedIndexChanged( listColoring, e );
      RefreshDisplayOnDocuments();
    }



    private void btnSetDefaultsFont_Click( object sender, EventArgs e )
    {
      Core.Settings.SourceFontFamily      = "Consolas";
      Core.Settings.SourceFontSize        = 9.0f;
      Core.Settings.BASICSourceFontFamily = "Consolas";
      Core.Settings.BASICSourceFontSize   = 9.0f;
      Core.Settings.BASICUseNonC64Font    = true;

      labelFontPreview.Font = new Font( Core.Settings.SourceFontFamily, Core.Settings.SourceFontSize );
      if ( !Core.Settings.BASICUseNonC64Font )
      {
        labelBASICFontPreview.Font = new System.Drawing.Font( Core.MainForm.m_FontC64.Families[0], Core.Settings.SourceFontSize, System.Drawing.GraphicsUnit.Pixel );
      }
      else
      {
        labelBASICFontPreview.Font = new Font( Core.Settings.BASICSourceFontFamily, Core.Settings.BASICSourceFontSize );
      }

      RefreshDisplayOnDocuments();
    }



    private void checkASMShowCycles_CheckedChanged( object sender, EventArgs e )
    {
      if ( checkASMShowCycles.Checked != Core.Settings.ASMShowCycles )
      {
        Core.Settings.ASMShowCycles = checkASMShowCycles.Checked;
        RefreshDisplayOnDocuments();
      }
    }



    private void checkASMShowSizes_CheckedChanged( object sender, EventArgs e )
    {
      if ( checkASMShowSizes.Checked != Core.Settings.ASMShowBytes )
      {
        Core.Settings.ASMShowBytes = checkASMShowSizes.Checked;
        RefreshDisplayOnDocuments();
      }
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



    private void checkASMShowMiniView_CheckedChanged( object sender, EventArgs e )
    {
      if ( checkASMShowMiniMap.Checked != Core.Settings.ASMShowMiniView )
      {
        Core.Settings.ASMShowMiniView = checkASMShowMiniMap.Checked;
        RefreshDisplayOnDocuments();
      }
    }



    private void btnCloneTool_Click( object sender, EventArgs e )
    {
      if ( listTools.SelectedItem == null )
      {
        return;
      }
      var toolInfo = (GR.Generic.Tupel<string, ToolInfo>)listTools.SelectedItem;

      var tool = toolInfo.second;

      string    newName = toolInfo.first + " copy";

      ToolInfo    newTool =
         new ToolInfo()
         {
           CartArguments = tool.CartArguments,
           DebugArguments = tool.DebugArguments,
           Filename = tool.Filename,
           Name = newName,
           PassLabelsToEmulator = tool.PassLabelsToEmulator,
           PRGArguments = tool.PRGArguments,
           TrueDriveOffArguments = tool.TrueDriveOffArguments,
           TrueDriveOnArguments = tool.TrueDriveOnArguments,
           Type = tool.Type,
           WorkPath = tool.WorkPath
         };


      Core.Settings.ToolInfos.AddLast( newTool );

      listTools.Items.Add( new GR.Generic.Tupel<string, ToolInfo>( newTool.Name, newTool ) );

      if ( newTool.Type == ToolInfo.ToolType.EMULATOR )
      {
        Core.MainForm.RaiseApplicationEvent( new C64Studio.Types.ApplicationEvent( C64Studio.Types.ApplicationEvent.Type.EMULATOR_LIST_CHANGED ) );
      }
    }



    private bool GetBooleanFromString( string Content )
    {
      string    value = Content.ToUpper();

      return ( ( value == "YES" )
            || ( value == "1" )
            || ( value == "Y" )
            || ( value == "TRUE" ) );
    }



    private void ImportGenericSettings( GR.Strings.XMLElement XMLRoot )
    {
      GR.Strings.XMLElement     xmlSettingRoot = XMLRoot.FindByTypeRecursive( "Generic" );
      if ( xmlSettingRoot == null )
      {
        return;
      }

      foreach ( var xmlKey in xmlSettingRoot.ChildElements )
      {
        if ( xmlKey.Type == "Sounds" )
        {
          foreach ( var xmlValue in xmlKey.ChildElements )
          {
            if ( xmlValue.Type == "Sound" )
            {
              switch ( xmlValue.Attribute( "Reason" ) )
              {
                case "FailedBuild":
                  Core.Settings.PlaySoundOnBuildFailure = GetBooleanFromString( xmlValue.Attribute( "Play" ) );
                  break;
                case "SearchFoundNoItem":
                  Core.Settings.PlaySoundOnSearchFoundNoItem = GetBooleanFromString( xmlValue.Attribute( "Play" ) );
                  break;
                case "SuccessfulBuild":
                  Core.Settings.PlaySoundOnSuccessfulBuild = GetBooleanFromString( xmlValue.Attribute( "Play" ) );
                  break;
              }
            }
          }
        }
        else if ( xmlKey.Type == "Tabs" )
        {
          Core.Settings.TabConvertToSpaces  = GetBooleanFromString( xmlKey.Attribute( "ConvertTabsToSpaces" ) );
          Core.Settings.AllowTabs           = GetBooleanFromString( xmlKey.Attribute( "AllowTabs" ) );
          Core.Settings.TabSize             = GR.Convert.ToI32( xmlKey.Attribute( "TabSize" ) );
        }
        else if ( xmlKey.Type == "AssemblerEditor" )
        {
          Core.Settings.ASMHideLineNumbers  = !GetBooleanFromString( xmlKey.Attribute( "ShowLineNumbers" ) );
          Core.Settings.ASMShowBytes        = GetBooleanFromString( xmlKey.Attribute( "ShowByteSize" ) );
          Core.Settings.ASMShowCycles       = GetBooleanFromString( xmlKey.Attribute( "ShowCycles" ) );
          Core.Settings.ASMShowMiniView     = GetBooleanFromString( xmlKey.Attribute( "ShowMiniView" ) );
        }
        else if ( xmlKey.Type == "Environment" )
        {
          Core.Settings.AutoOpenLastSolution = GetBooleanFromString( xmlKey.Attribute( "OpenLastSolutionOnStartup" ) );
        }
        else if ( xmlKey.Type == "Fonts" )
        {
          foreach ( var xmlValue in xmlKey.ChildElements )
          {
            if ( xmlValue.Type == "Font" )
            {
              switch ( xmlValue.Attribute( "Type" ) )
              {
                case "ASM":
                  Core.Settings.SourceFontFamily  = xmlValue.Attribute( "Family" );
                  Core.Settings.SourceFontSize    = GR.Convert.ToF32( xmlValue.Attribute( "Size" ) );
                  break;
                case "BASIC":
                  Core.Settings.BASICSourceFontFamily = xmlValue.Attribute( "Family" );
                  Core.Settings.BASICSourceFontSize   = GR.Convert.ToF32( xmlValue.Attribute( "Size" ) );
                  Core.Settings.BASICUseNonC64Font    = !GetBooleanFromString( xmlValue.Attribute( "UseC64Font" ) );
                  break;
              }
            }
          }
        }
      }

      checkPlaySoundCompileSuccessful.Checked = Core.Settings.PlaySoundOnSuccessfulBuild;
      checkPlaySoundCompileFail.Checked = Core.Settings.PlaySoundOnBuildFailure;
      checkPlaySoundSearchTextNotFound.Checked = Core.Settings.PlaySoundOnSearchFoundNoItem;

      checkConvertTabsToSpaces.Checked = Core.Settings.TabConvertToSpaces;
      checkAllowTabs.Checked = Core.Settings.AllowTabs;
      editTabSize.Text = Core.Settings.TabSize.ToString();

      checkBASICStripSpaces.Checked = Core.Settings.BASICStripSpaces;

      checkAutoOpenLastSolution.Checked = Core.Settings.AutoOpenLastSolution;

      checkASMShowLineNumbers.Checked = !Core.Settings.ASMHideLineNumbers;
      checkASMShowCycles.Checked = Core.Settings.ASMShowCycles;
      checkASMShowSizes.Checked = Core.Settings.ASMShowBytes;
      checkASMShowMiniMap.Checked = Core.Settings.ASMShowMiniView;

      labelFontPreview.Font = new Font( Core.Settings.SourceFontFamily, Core.Settings.SourceFontSize );

      if ( !Core.Settings.BASICUseNonC64Font )
      {
        labelBASICFontPreview.Font = new System.Drawing.Font( Core.MainForm.m_FontC64.Families[0], Core.Settings.SourceFontSize, System.Drawing.GraphicsUnit.Pixel );
      }
      else
      {
        labelBASICFontPreview.Font = new Font( Core.Settings.BASICSourceFontFamily, Core.Settings.BASICSourceFontSize );
      }
      checkBASICUseC64Font.Checked = !Core.Settings.BASICUseNonC64Font;
      btnChangeBASICFont.Enabled = !checkBASICUseC64Font.Checked;

      RefreshDisplayOnDocuments();
    }



    private void ImportTools( GR.Strings.XMLElement XMLRoot )
    {
      GR.Strings.XMLElement     xmlSettingRoot = XMLRoot.FindByTypeRecursive( "Tools" );
      if ( xmlSettingRoot == null )
      {
        return;
      }

      Core.Settings.ToolInfos.Clear();

      foreach ( var xmlKey in xmlSettingRoot.ChildElements )
      {
        if ( xmlKey.Type == "Tool" )
        {
          try
          {
            ToolInfo.ToolType toolType = (ToolInfo.ToolType)Enum.Parse( typeof( ToolInfo.ToolType ), xmlKey.Attribute( "Type" ), true );

            ToolInfo    toolInfo = new ToolInfo();
            toolInfo.Type = toolType;
            toolInfo.Name           = xmlKey.Attribute( "Name" );
            toolInfo.Filename       = xmlKey.Attribute( "Executable" );
            toolInfo.CartArguments  = xmlKey.Attribute( "CartArgs" );
            toolInfo.DebugArguments = xmlKey.Attribute( "DebugArgs" );
            toolInfo.PRGArguments   = xmlKey.Attribute( "PRGArgs" );
            toolInfo.TrueDriveOffArguments = xmlKey.Attribute( "TrueDriveOffArgs" );
            toolInfo.TrueDriveOnArguments = xmlKey.Attribute( "TrueDriveOnArgs" );
            toolInfo.WorkPath       = xmlKey.Attribute( "WorkPath" );
            toolInfo.PassLabelsToEmulator = GetBooleanFromString( xmlKey.Attribute( "PassLabelsToEmulator" ) );

            Core.Settings.ToolInfos.AddLast( toolInfo );
          }
          catch ( Exception ex )
          {
            Core.AddToOutput( "Could not parse element: " + ex.Message + System.Environment.NewLine );
          }
        }
      }
      RefillToolInfoList();
      if ( listTools.Items.Count > 0 )
      {
        listTools.SelectedIndex = 0;
      }
      Core.MainForm.RaiseApplicationEvent( new C64Studio.Types.ApplicationEvent( C64Studio.Types.ApplicationEvent.Type.EMULATOR_LIST_CHANGED ) );
    }



    private void ImportAccelerators( GR.Strings.XMLElement XMLRoot )
    {
      GR.Strings.XMLElement     xmlSettingRoot = XMLRoot.FindByTypeRecursive( "Accelerators" );
      if ( xmlSettingRoot == null )
      {
        return;
      }

      Core.Settings.Accelerators.Clear();
      foreach ( var xmlKey in xmlSettingRoot.ChildElements )
      {
        if ( xmlKey.Type == "Function" )
        {
          try
          {
            Types.Function function = (Types.Function)Enum.Parse( typeof( Types.Function ), xmlKey.Attribute( "Function" ), true );

            Keys key = (Keys)Enum.Parse( typeof( Keys ), xmlKey.Attribute( "Key" ), true );

            Core.Settings.Accelerators.Add( key, new AcceleratorKey( key, function ) );
          }
          catch ( Exception ex )
          {
            Core.AddToOutput( "Could not parse element: " + ex.Message + System.Environment.NewLine );
          }
        }
      }
      RefillAcceleratorList();
      RefreshDisplayOnDocuments();
    }



    private void ImportIgnoredMessages( GR.Strings.XMLElement XMLRoot )
    {
      GR.Strings.XMLElement     xmlSettingRoot = XMLRoot.FindByTypeRecursive( "IgnoredMessages" );
      if ( xmlSettingRoot == null )
      {
        return;
      }

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



    private void ImportEditorColors( GR.Strings.XMLElement XMLRoot )
    {
      GR.Strings.XMLElement     xmlSettingRoot = XMLRoot.FindByTypeRecursive( "EditorColors" );
      if ( xmlSettingRoot == null )
      {
        return;
      }

      Core.Settings.SetDefaultColors();

      foreach ( var xmlKey in xmlSettingRoot.ChildElements )
      {
        if ( xmlKey.Type == "Color" )
        {
          try
          {
            Types.ColorableElement element = (Types.ColorableElement)Enum.Parse( typeof( Types.ColorableElement ), xmlKey.Attribute( "Element" ), true );

            if ( !Core.Settings.SyntaxColoring.ContainsKey( element ) )
            {
              Core.Settings.SyntaxColoring[element] = new Types.ColorSetting( GR.EnumHelper.GetDescription( element ) );
            }
            Core.Settings.SyntaxColoring[element].FGColor = GR.Convert.ToU32( xmlKey.Attribute( "FGColor" ), 16 );

            Core.Settings.SyntaxColoring[element].BGColorAuto = ( xmlKey.Attribute( "BGColor" ).ToUpper() == "AUTO" );
            Core.Settings.SyntaxColoring[element].BGColor = GR.Convert.ToU32( xmlKey.Attribute( "BGColor" ), 16 );
          }
          catch ( Exception ex )
          {
            Core.AddToOutput( "Could not parse element: " + ex.Message + System.Environment.NewLine );
          }
        }
      }
      RefillColorList();

      listColoring.SelectedIndices.Add( 0 );
      ColorsChanged( Types.ColorableElement.EMPTY_SPACE );
    }



    private void ImportBASICKeyMapping( GR.Strings.XMLElement XMLRoot )
    {
      GR.Strings.XMLElement     xmlSettingRoot = XMLRoot.FindByTypeRecursive( "BASICKeyMap" );
      if ( xmlSettingRoot == null )
      {
        return;
      }

      Core.Settings.BASICKeyMap.Keymap.Clear();
      foreach ( var xmlKey in xmlSettingRoot.ChildElements )
      {
        if ( xmlKey.Type == "Key" )
        {
          string    c64key = xmlKey.Attribute( "C64Key" );

          try
          {
            Keys key = (Keys)Enum.Parse( typeof( Keys ), xmlKey.Attribute( "FormsKey" ), true );
            Types.KeyboardKey c64Key = (Types.KeyboardKey)Enum.Parse( typeof( Types.KeyboardKey ), xmlKey.Attribute( "C64Key" ), true );

            Core.Settings.BASICKeyMap.Keymap.Add( key, new KeymapEntry() { Key = key, KeyboardKey = c64Key } );
          }
          catch ( Exception ex )
          {
            Core.AddToOutput( "Could not parse element: " + ex.Message + System.Environment.NewLine );
          }

        }
      }
      RefillBASICKeyMappingList();
    }



    private void ImportBASICEditorSettings( GR.Strings.XMLElement XMLRoot )
    {
      GR.Strings.XMLElement     xmlSettingRoot = XMLRoot.FindByTypeRecursive( "BASICEditor" );
      if ( xmlSettingRoot == null )
      {
        return;
      }

      foreach ( var xmlKey in xmlSettingRoot.ChildElements )
      {
        if ( xmlKey.Type == "StripSpaces" )
        {
          Core.Settings.BASICStripSpaces = GetBooleanFromString( xmlKey.Content );
        }
        else if ( xmlKey.Type == "ShowControlCodesAsChars" )
        {
          Core.Settings.BASICShowControlCodesAsChars = GetBooleanFromString( xmlKey.Content );
        }
      }

      checkBASICStripSpaces.Checked = Core.Settings.BASICStripSpaces;
    }



    private void ExportGenericSettings( GR.Strings.XMLElement XMLRoot )
    {
      GR.Strings.XMLElement     xmlSettingRoot = new GR.Strings.XMLElement( "Generic" );
      XMLRoot.AddChild( xmlSettingRoot );

      GR.Strings.XMLElement     xmlSounds = new GR.Strings.XMLElement( "Sounds" );
      xmlSettingRoot.AddChild( xmlSounds );

      var sound = new GR.Strings.XMLElement( "Sound" );
      sound.AddAttribute( "Reason", "FailedBuild" );
      sound.AddAttribute( "Play", Core.Settings.PlaySoundOnBuildFailure ? "yes" : "no" );
      xmlSounds.AddChild( sound );

      sound = new GR.Strings.XMLElement( "Sound" );
      sound.AddAttribute( "Reason", "SearchFoundNoItem" );
      sound.AddAttribute( "Play", Core.Settings.PlaySoundOnSearchFoundNoItem ? "yes" : "no" );
      xmlSounds.AddChild( sound );

      sound = new GR.Strings.XMLElement( "Sound" );
      sound.AddAttribute( "Reason", "SuccessfulBuild" );
      sound.AddAttribute( "Play", Core.Settings.PlaySoundOnSuccessfulBuild ? "yes" : "no" );
      xmlSounds.AddChild( sound );


      GR.Strings.XMLElement     xmlTabs = new GR.Strings.XMLElement( "Tabs" );
      xmlSettingRoot.AddChild( xmlTabs );
      xmlTabs.AddAttribute( "TabSize", Core.Settings.TabSize.ToString() );
      xmlTabs.AddAttribute( "AllowTabs", Core.Settings.AllowTabs ? "yes" : "no" );
      xmlTabs.AddAttribute( "ConvertTabsToSpaces", Core.Settings.TabConvertToSpaces ? "yes" : "no" );

      GR.Strings.XMLElement     xmlASMEditor = new GR.Strings.XMLElement( "AssemblerEditor" );
      xmlSettingRoot.AddChild( xmlASMEditor );
      xmlASMEditor.AddAttribute( "ShowLineNumbers", Core.Settings.ASMHideLineNumbers ? "no" : "yes" );
      xmlASMEditor.AddAttribute( "ShowByteSize", Core.Settings.ASMShowBytes ? "yes" : "no" );
      xmlASMEditor.AddAttribute( "ShowCycles", Core.Settings.ASMShowCycles ? "yes" : "no" );
      xmlASMEditor.AddAttribute( "ShowMiniView", Core.Settings.ASMShowMiniView ? "yes" : "no" );

      GR.Strings.XMLElement     xmlEnvironment = new GR.Strings.XMLElement( "Environment" );
      xmlSettingRoot.AddChild( xmlEnvironment );
      xmlEnvironment.AddAttribute( "OpenLastSolutionOnStartup", Core.Settings.AutoOpenLastSolution ? "yes" : "no" );

      GR.Strings.XMLElement     xmlFonts = new GR.Strings.XMLElement( "Fonts" );
      xmlSettingRoot.AddChild( xmlFonts );

      var xmlFont = new GR.Strings.XMLElement( "Font" );
      xmlFont.AddAttribute( "Type", "ASM" );
      xmlFont.AddAttribute( "Family", Core.Settings.SourceFontFamily );
      xmlFont.AddAttribute( "Size", Core.Settings.SourceFontSize.ToString() );
      xmlFonts.AddChild( xmlFont );

      xmlFont = new GR.Strings.XMLElement( "Font" );
      xmlFont.AddAttribute( "Type", "BASIC" );
      xmlFont.AddAttribute( "Family", Core.Settings.BASICSourceFontFamily );
      xmlFont.AddAttribute( "Size", Core.Settings.BASICSourceFontSize.ToString() );
      xmlFont.AddAttribute( "UseC64Font", Core.Settings.BASICUseNonC64Font ? "no" : "yes" );
      xmlFonts.AddChild( xmlFont );
    }



    private void ExportTools( GR.Strings.XMLElement XMLRoot )
    {
      GR.Strings.XMLElement     xmlSettingRoot = new GR.Strings.XMLElement( "Tools" );
      XMLRoot.AddChild( xmlSettingRoot );

      foreach ( ToolInfo tool in Core.Settings.ToolInfos )
      {
        var xmlKey = new GR.Strings.XMLElement( "Tool" );
        xmlKey.AddAttribute( "Type", tool.Type.ToString() );
        xmlKey.AddAttribute( "Executable", tool.Filename );
        xmlKey.AddAttribute( "Name", tool.Name );
        xmlKey.AddAttribute( "CartArgs", tool.CartArguments );
        xmlKey.AddAttribute( "DebugArgs", tool.DebugArguments );
        xmlKey.AddAttribute( "PassLabelsToEmulator", tool.PassLabelsToEmulator ? "yes" : "no" );
        xmlKey.AddAttribute( "PRGArgs", tool.PRGArguments );
        xmlKey.AddAttribute( "TrueDriveOffArgs", tool.TrueDriveOffArguments );
        xmlKey.AddAttribute( "TrueDriveOnArgs", tool.TrueDriveOnArguments );
        xmlKey.AddAttribute( "WorkPath", tool.WorkPath );

        xmlSettingRoot.AddChild( xmlKey );
      }
    }



    private void ExportAccelerators( GR.Strings.XMLElement XMLRoot )
    {
      GR.Strings.XMLElement     xmlSettingRoot = new GR.Strings.XMLElement( "Accelerators" );
      XMLRoot.AddChild( xmlSettingRoot );

      foreach ( Types.Function function in Enum.GetValues( typeof( Types.Function ) ) )
      {
        if ( function == C64Studio.Types.Function.NONE )
        {
          continue;
        }

        AcceleratorKey key = Core.Settings.DetermineAccelerator( function );
        if ( key != null )
        {
          var xmlKey = new GR.Strings.XMLElement( "Function" );
          xmlKey.AddAttribute( "Function", function.ToString() );
          xmlKey.AddAttribute( "Key", key.Key.ToString() );

          xmlSettingRoot.AddChild( xmlKey );
        }
      }
    }



    private void ExportIgnoredWarnings( GR.Strings.XMLElement XMLRoot )
    {
      GR.Strings.XMLElement     xmlSettingRoot = new GR.Strings.XMLElement( "IgnoredMessages" );
      XMLRoot.AddChild( xmlSettingRoot );

      foreach ( Types.ErrorCode element in Core.Settings.IgnoredWarnings )
      {
        var xmlColor = new GR.Strings.XMLElement( "Message" );
        xmlColor.AddAttribute( "Index", ( (int)element ).ToString() );

        xmlSettingRoot.AddChild( xmlColor );
      }
    }



    private void ExportColorSettings( GR.Strings.XMLElement XMLRoot )
    {
      GR.Strings.XMLElement     xmlSettingRoot = new GR.Strings.XMLElement( "EditorColors" );
      XMLRoot.AddChild( xmlSettingRoot );

      foreach ( Types.ColorableElement element in Core.Settings.SyntaxColoring.Keys )
      {
        if ( element == C64Studio.Types.ColorableElement.LAST_ENTRY )
        {
          continue;
        }
        if ( element >= Types.ColorableElement.FIRST_GUI_ELEMENT )
        {
          // TODO - for now GUI elements not custom drawn (yet)
          break;
        }

        var xmlColor = new GR.Strings.XMLElement( "Color" );
        xmlColor.AddAttribute( "Element", element.ToString() );

        xmlColor.AddAttribute( "FGColor", Core.Settings.SyntaxColoring[element].FGColor.ToString( "X4" ) );
        if ( Core.Settings.SyntaxColoring[element].BGColorAuto )
        {
          xmlColor.AddAttribute( "BGColor", "Auto" );
        }
        else
        {
          xmlColor.AddAttribute( "BGColor", Core.Settings.SyntaxColoring[element].BGColor.ToString( "X4" ) );
        }
        xmlSettingRoot.AddChild( xmlColor );
      }
    }



    private void ExportBASICEditorSettings( GR.Strings.XMLElement XMLRoot )
    {
      GR.Strings.XMLElement     xmlSettingRoot = new GR.Strings.XMLElement( "BASICEditor" );
      XMLRoot.AddChild( xmlSettingRoot );

      xmlSettingRoot.AddChild( "StripSpaces", Core.Settings.BASICStripSpaces ? "yes" : "no" );
      xmlSettingRoot.AddChild( "ShowControlCodesAsChars", Core.Settings.BASICShowControlCodesAsChars ? "yes" : "no" );
    }



    private void ExportBASICKeyMap( GR.Strings.XMLElement XMLRoot )
    {
      GR.Strings.XMLElement     xmlSettingRoot = new GR.Strings.XMLElement( "BASICKeyMap" );
      XMLRoot.AddChild( xmlSettingRoot );

      foreach ( var entry in Core.Settings.BASICKeyMap.Keymap )
      {
        var xmlKey = new GR.Strings.XMLElement( "Key" );
        xmlKey.AddAttribute( "FormsKey", entry.Key.ToString() );
        xmlKey.AddAttribute( "C64Key", entry.Value.KeyboardKey.ToString() );

        xmlSettingRoot.AddChild( xmlKey );
      }
    }



    private void btnImportCurrentPageSettings_Click( object sender, EventArgs e )
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
        return;
      }

      var xmlSettings = xml.FindByTypeRecursive( "C64StudioSettings" );
      if ( xmlSettings == null )
      {
        return;
      }

      switch ( tabPreferences.SelectedIndex )
      {
        case 0:
          ImportGenericSettings( xmlSettings );
          break;
        case 1:
          ImportTools( xmlSettings );
          break;
        case 2:
          ImportAccelerators( xmlSettings );
          break;
        case 3:
          ImportIgnoredMessages( xmlSettings );
          break;
        case 4:
          ImportEditorColors( xmlSettings );
          break;
        case 5:
          ImportBASICKeyMapping( xmlSettings );
          break;
        case 6:
          ImportBASICEditorSettings( xmlSettings );
          break;
      }
    }



    private void btnExportCurrentPageSettings_Click( object sender, EventArgs e )
    {
      SaveFileDialog    saveDlg = new SaveFileDialog();

      saveDlg.Title = "Choose a target file";
      saveDlg.Filter = "XML Files|*.xml|All Files|*.*";

      if ( saveDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
      {
        return;
      }

      GR.Strings.XMLParser      xml = new GR.Strings.XMLParser();

      GR.Strings.XMLElement     xmlRoot = new GR.Strings.XMLElement( "C64StudioSettings" );
      xml.AddChild( xmlRoot );

      switch ( tabPreferences.SelectedIndex )
      {
        case 0:
          ExportGenericSettings( xmlRoot );
          break;
        case 1:
          ExportTools( xmlRoot );
          break;
        case 2:
          ExportAccelerators( xmlRoot );
          break;
        case 3:
          ExportIgnoredWarnings( xmlRoot );
          break;
        case 4:
          ExportColorSettings( xmlRoot );
          break;
        case 5:
          ExportBASICKeyMap( xmlRoot );
          break;
        case 6:
          ExportBASICEditorSettings( xmlRoot );
          break;
      }

      GR.IO.File.WriteAllText( saveDlg.FileName, xml.ToText() );
    }



    private void btnExportAllSettings_Click( object sender, EventArgs e )
    {
      SaveFileDialog    saveDlg = new SaveFileDialog();

      saveDlg.Title = "Choose a target file";
      saveDlg.Filter = "XML Files|*.xml|All Files|*.*";

      if ( saveDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
      {
        return;
      }

      GR.Strings.XMLParser      xml = new GR.Strings.XMLParser();

      GR.Strings.XMLElement     xmlRoot = new GR.Strings.XMLElement( "C64StudioSettings" );
      xml.AddChild( xmlRoot );

      ExportGenericSettings( xmlRoot );
      ExportTools( xmlRoot );
      ExportAccelerators( xmlRoot );
      ExportIgnoredWarnings( xmlRoot );
      ExportColorSettings( xmlRoot );
      ExportBASICKeyMap( xmlRoot );
      ExportBASICEditorSettings( xmlRoot );

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
        return;
      }

      var xmlSettings = xml.FindByTypeRecursive( "C64StudioSettings" );
      if ( xmlSettings == null )
      {
        return;
      }
      ImportGenericSettings( xmlSettings );
      ImportTools( xmlSettings );
      ImportAccelerators( xmlSettings );
      ImportIgnoredMessages( xmlSettings );
      ImportEditorColors( xmlSettings );
      ImportBASICEditorSettings( xmlSettings );
      ImportBASICKeyMapping( xmlSettings );
    }



    private void checkBASICShowControlCodesAsChars_CheckedChanged( object sender, EventArgs e )
    {
      Core.Settings.BASICShowControlCodesAsChars = checkBASICShowControlCodes.Checked;
    }



    private void checkASMAutoTruncateLiteralValues_CheckedChanged( object sender, EventArgs e )
    {
      Core.Settings.ASMAutoTruncateLiteralValues = checkASMAutoTruncateLiteralValues.Checked;
    }



    private void comboAppMode_SelectedIndexChanged( object sender, EventArgs e )
    {
      Core.Settings.StudioAppMode = (AppMode)comboAppMode.SelectedIndex;
    }



    private void editToolName_TextChanged( object sender, EventArgs e )
    {
      var tool = SelectedTool();
      if ( tool == null )
      {
        return;
      }

      bool  emulatorAffected = ( tool.Type == ( ToolInfo.ToolType.EMULATOR ) );

      tool.Name = editToolName.Text;
      // butt ugly hack to enforce list item text update (didn't update if case changes)
      listTools.Items[listTools.SelectedIndex] = new GR.Generic.Tupel<string, ToolInfo>( "", null );
      listTools.Items[listTools.SelectedIndex] = new GR.Generic.Tupel<string, ToolInfo>( tool.Name, tool );

      if ( emulatorAffected )
      {
        Core.MainForm.RaiseApplicationEvent( new C64Studio.Types.ApplicationEvent( C64Studio.Types.ApplicationEvent.Type.EMULATOR_LIST_CHANGED ) );
      }

    }



    private void checkASMShowAutoComplete_CheckedChanged( object sender, EventArgs e )
    {
      if ( checkASMShowAutoComplete.Checked != Core.Settings.ASMShowAutoComplete )
      {
        Core.Settings.ASMShowAutoComplete = checkASMShowAutoComplete.Checked;
        RefreshDisplayOnDocuments();
      }
    }



    private ListViewItem asmLibraryPathList_AddingItem( object sender )
    {
      var newEntry = new ListViewItem( editASMLibraryPath.Text );
      return newEntry;
    }



    private void tabPreferences_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( !m_ResizeHackApplied )
      {
        m_ResizeHackApplied = true;
        asmLibraryPathList.PerformLayout();
      }
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



    private void asmLibraryPathList_ItemAdded( object sender, ListViewItem Item )
    {
      Core.Settings.ASMLibraryPaths.Clear();
      foreach ( ListViewItem entry in asmLibraryPathList.Items )
      {
        Core.Settings.ASMLibraryPaths.Add( entry.Text );
      }
    }



    private void asmLibraryPathList_ItemMoved( object sender, ListViewItem Item1, ListViewItem Item2 )
    {
      Core.Settings.ASMLibraryPaths.Clear();
      foreach ( ListViewItem entry in asmLibraryPathList.Items )
      {
        Core.Settings.ASMLibraryPaths.Add( entry.Text );
      }
    }



    private void asmLibraryPathList_ItemRemoved( object sender, ListViewItem Item )
    {
      Core.Settings.ASMLibraryPaths.Clear();
      foreach ( ListViewItem entry in asmLibraryPathList.Items )
      {
        Core.Settings.ASMLibraryPaths.Add( entry.Text );
      }
    }

  }
}

