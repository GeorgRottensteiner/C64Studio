using C64Studio.Parser;
using C64Studio.Types;
using GR.Image;
using RetroDevStudio;
using System;
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

      DPIHandler.ResizeControlsForDPI( this );

      labelFontPreview.Font = new Font( Core.Settings.SourceFontFamily, Core.Settings.SourceFontSize, Core.Settings.SourceFontStyle );

      if ( !Core.Settings.BASICUseNonC64Font )
      {
        labelBASICFontPreview.Font = new System.Drawing.Font( Core.MainForm.m_FontC64.Families[0], Core.Settings.SourceFontSize, System.Drawing.GraphicsUnit.Pixel );
      }
      else
      {
        labelBASICFontPreview.Font = new Font( Core.Settings.BASICSourceFontFamily, Core.Settings.BASICSourceFontSize, Core.Settings.BASICSourceFontStyle );
      }

      RefillIgnoredMessageList();
      RefillWarningsAsErrorList();
      RefillC64StudioHackList();

      comboToolType.Items.Add( "<Choose one>" );
      comboToolType.Items.Add( "Assembler" );
      comboToolType.Items.Add( "Emulator" );

      RefillBASICKeyMappingList();
      RefillToolInfoList();

      RefillAcceleratorList();

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
      //checkAllowTabs.Checked                  = Core.Settings.AllowTabs;
      editTabSize.Text                        = Core.Settings.TabSize.ToString();
      checkStripTrailingSpaces.Checked        = Core.Settings.StripTrailingSpaces;
      checkBASICUseC64Font.Checked            = !Core.Settings.BASICUseNonC64Font;
      checkBASICStripSpaces.Checked           = Core.Settings.BASICStripSpaces;
      checkBASICShowControlCodes.Checked      = Core.Settings.BASICShowControlCodesAsChars;
      checkBASICAutoToggleEntryMode.Checked   = Core.Settings.BASICAutoToggleEntryMode;
      checkBASICStripREM.Checked              = Core.Settings.BASICStripREM;
      checkASMShowLineNumbers.Checked         = !Core.Settings.ASMHideLineNumbers;
      checkAutoOpenLastSolution.Checked       = Core.Settings.AutoOpenLastSolution;
      checkASMShowCycles.Checked              = Core.Settings.ASMShowCycles;
      checkASMShowSizes.Checked               = Core.Settings.ASMShowBytes;
      checkASMShowMiniMap.Checked             = Core.Settings.ASMShowMiniView;
      checkASMAutoTruncateLiteralValues.Checked = Core.Settings.ASMAutoTruncateLiteralValues;
      checkASMShowAutoComplete.Checked        = Core.Settings.ASMShowAutoComplete;
      checkASMShowAddress.Checked             = Core.Settings.ASMShowAddress;

      editDefaultOpenSolutionPath.Text        = Core.Settings.DefaultProjectBasePath;
      editMaxMRUEntries.Text                  = Core.Settings.MRUMaxCount.ToString();

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

      var allEncodings = System.Text.Encoding.GetEncodings();

      foreach ( var encoding in allEncodings )
      {
        comboASMEncoding.Items.Add( new GR.Generic.Tupel<string, Encoding>( encoding.DisplayName + "    Codepage " + encoding.CodePage, encoding.GetEncoding() ) );
        if ( encoding.GetEncoding() == Core.Settings.SourceFileEncoding )
        {
          comboASMEncoding.SelectedIndex = comboASMEncoding.Items.Count - 1;
        }
      }

      Core.Theming.ApplyTheme( this );
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



    private void RefillToolInfoList()
    {
      alistTools.Items.Clear();
      foreach ( ToolInfo tool in Core.Settings.ToolInfos )
      {
        alistTools.Items.Add( new ArrangedItemEntry( tool.Name ) { Tag = tool } );
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
          itemF.SubItems.Add( key.SecondaryKey.ToString() );
        }
        else
        {
          itemF.SubItems.Add( "" );
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



    private void RefillWarningsAsErrorList()
    {
      listWarningsAsErrors.Items.Clear();
      listWarningsAsErrors.BeginUpdate();
      foreach ( Types.ErrorCode code in Enum.GetValues( typeof( Types.ErrorCode ) ) )
      {
        if ( ( code > Types.ErrorCode.WARNING_START )
        &&   ( code < Types.ErrorCode.WARNING_LAST_PLUS_ONE ) )
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



    private void RefillColorList()
    {
      listColoring.Items.Clear();
      foreach ( Types.ColorableElement element in System.Enum.GetValues( typeof( Types.ColorableElement ) ) )
      {
        if ( element == C64Studio.Types.ColorableElement.LAST_ENTRY )
        {
          continue;
        }
        /*
        if ( element >= Types.ColorableElement.FIRST_GUI_ELEMENT )
        {
          // TODO - for now GUI elements not custom drawn (yet)
          break;
        }*/
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

        if ( ConstantData.PhysicalKeyInfo.ContainsKey( realKey ) )
        {
          var charInfo = ConstantData.PhysicalKeyInfo[realKey];

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

          // auto-fill initial entries if nothing is set
          if ( ( string.IsNullOrEmpty( tool.PRGArguments ) )
          &&   ( string.IsNullOrEmpty( tool.DebugArguments ) )
          &&   ( string.IsNullOrEmpty( tool.CartArguments ) )
          &&   ( string.IsNullOrEmpty( tool.TrueDriveOffArguments ) )
          &&   ( string.IsNullOrEmpty( tool.TrueDriveOnArguments ) ) )
          {
            EmulatorInfo.SetDefaultRunArguments( tool );
            alistTools_SelectedIndexChanged( null, null );
          }
        }
      }
    }



    private ToolInfo SelectedTool()
    {
      if ( alistTools.SelectedItem == null )
      {
        return null;
      }
      return (ToolInfo)alistTools.SelectedItem.Tag;
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
        btnBindKeySecondary.Enabled = false;
        return;
      }
      Types.Function function = (Types.Function)listFunctions.SelectedItems[0].Tag;

      editKeyBinding.Enabled = true;
      btnUnbindKey.Enabled = ( Core.Settings.DetermineAccelerator( function ) != null );
      btnBindKey.Enabled = true;
      btnBindKeySecondary.Enabled = true;
    }



    private void editKeyBinding_PreviewKeyDown( object sender, PreviewKeyDownEventArgs e )
    {
      m_PressedKey        = e.KeyData;
      editKeyBinding.Text = e.KeyData.ToString();
      e.IsInputKey        = true;
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



    private void button1_Click( object sender, EventArgs e )
    {
      string    macroInfo = "";
      bool      error = false;

      var Document = Core.MainForm.ActiveDocumentInfo;
      if ( Document == null )
      {
        macroInfo = "Sorry, but no document is currently active.";
        error = true;
      }
      else
      {
        macroInfo = "$(Filename) = " + Core.MainForm.FillParameters( "$(Filename)", Document, false, out error ) + System.Environment.NewLine;
        macroInfo += "$(FilenameWithoutExtension) = " + Core.MainForm.FillParameters( "$(FilenameWithoutExtension)", Document, false, out error ) + System.Environment.NewLine;
        macroInfo += "$(FilePath) = " + Core.MainForm.FillParameters( "$(FilePath)", Document, false, out error ) + System.Environment.NewLine;
        macroInfo += "$(BuildTargetFilename) = " + Core.MainForm.FillParameters( "$(BuildTargetFilename)", Document, false, out error ) + System.Environment.NewLine;
        macroInfo += "$(BuildTargetFilenameWithoutExtension) = " + Core.MainForm.FillParameters( "$(BuildTargetFilenameWithoutExtension)", Document, false, out error ) + System.Environment.NewLine;
        macroInfo += "$(DebugStartAddress) = " + Core.MainForm.FillParameters( "$(DebugStartAddress)", Document, false, out error ) + System.Environment.NewLine;
        macroInfo += "$(DebugStartAddressHex) = " + Core.MainForm.FillParameters( "$(DebugStartAddressHex)", Document, false, out error ) + System.Environment.NewLine;

        macroInfo += System.Environment.NewLine + System.Environment.NewLine + "Any other value will be calculated as expression, including symbols of the current build. Prefix with '0x' to output the value hexadecimal.";
      }
      System.Windows.Forms.MessageBox.Show( macroInfo, "Macros" );
    }



    private void RefreshDisplayOnDocuments()
    {
      Core.Theming.ApplyTheme( this );
      Core.Settings.RefreshDisplayOnAllDocuments( Core );
    }



    private void btnChooseFont_Click( object sender, EventArgs e )
    {
      System.Windows.Forms.FontDialog fontDialog = new FontDialog();

      fontDialog.Font = labelFontPreview.Font;

      if ( fontDialog.ShowDialog() == DialogResult.OK )
      {
        Core.Settings.SourceFontFamily  = fontDialog.Font.FontFamily.Name;
        Core.Settings.SourceFontSize    = fontDialog.Font.SizeInPoints;
        Core.Settings.SourceFontStyle   = fontDialog.Font.Style;
        labelFontPreview.Font           = fontDialog.Font;

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

      if ( ( color.BGColorAuto )
      &&   ( color.Name != "Empty Space" ) )
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

        // select custom entry
        comboElementBG.SelectedIndex = ColorComboIndexOfCustomItem( color );
        comboElementBG.Invalidate();
        panelElementPreview.Invalidate();
        RefreshDisplayOnDocuments();
      }
    }



    private int ColorComboIndexOfCustomItem( ColorSetting Color )
    {
      // empty space has no "Auto" item
      if ( Color.Name == "Empty Space" )
      {
        return 0;
      }
      return 1;
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
      uint colorRGB = color.FGColor;
      if ( color.Name == "Auto" )
      {
        color = new C64Studio.Types.ColorSetting( "Auto", Core.Settings.BGColor( C64Studio.Types.ColorableElement.EMPTY_SPACE ), Core.Settings.BGColor( C64Studio.Types.ColorableElement.EMPTY_SPACE ) );
        colorRGB = color.FGColor;
      }
      else if ( color.Name == "Custom" )
      {
        colorRGB = color.FGColor;
      }
      System.Drawing.Rectangle colorBox = new Rectangle( e.Bounds.Left + 2, e.Bounds.Top + 2, 50, e.Bounds.Height - 4 );

      e.Graphics.FillRectangle( new System.Drawing.SolidBrush( GR.Color.Helper.FromARGB( colorRGB ) ), colorBox );
      e.Graphics.DrawRectangle( System.Drawing.SystemPens.WindowFrame, colorBox );

      e.Graphics.DrawString( color.Name, comboElementBG.Font, new System.Drawing.SolidBrush( e.ForeColor ), e.Bounds.Left + 60, e.Bounds.Top + 2 );
    }



    private void ColorsChanged( Types.ColorableElement Color )
    {
      RefreshDisplayOnDocuments();
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
        var bgElementColor = Core.Settings.BGColor( C64Studio.Types.ColorableElement.EMPTY_SPACE );
        e.Graphics.FillRectangle( new System.Drawing.SolidBrush( GR.Color.Helper.FromARGB( bgElementColor ) ), panelElementPreview.ClientRectangle );
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
      listFunctions.SelectedItems[0].SubItems[3].Text = "";
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
      Core.Settings.BASICStripSpaces                  = checkBASICStripSpaces.Checked;
      Core.Compiling.ParserBasic.Settings.StripSpaces = Core.Settings.BASICStripSpaces;
    }



    private void editTabSize_TextChanged( object sender, EventArgs e )
    {
      int     tabSize = GR.Convert.ToI32( editTabSize.Text );

      if ( ( tabSize >= 1 )
      &&   ( Core.Settings.TabSize != tabSize ) )
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
        RefreshDisplayOnDocuments();
      }
    }



    private void checkAllowTabs_CheckedChanged( object sender, EventArgs e )
    {
      /*
      if ( Core.Settings.AllowTabs != checkAllowTabs.Checked )
      {
        Core.Settings.AllowTabs = checkAllowTabs.Checked;
        if ( Core.Settings.AllowTabs )
        {
          checkConvertTabsToSpaces.Checked = false;
        }
        RefreshDisplayOnDocuments();
      }*/
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

      labelFontPreview.Font = new Font( Core.Settings.SourceFontFamily, Core.Settings.SourceFontSize, Core.Settings.SourceFontStyle );
      if ( !Core.Settings.BASICUseNonC64Font )
      {
        labelBASICFontPreview.Font = new System.Drawing.Font( Core.MainForm.m_FontC64.Families[0], Core.Settings.SourceFontSize, Core.Settings.SourceFontStyle, System.Drawing.GraphicsUnit.Pixel );
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
          //Core.Settings.AllowTabs           = GetBooleanFromString( xmlKey.Attribute( "AllowTabs" ) );
          Core.Settings.TabSize             = GR.Convert.ToI32( xmlKey.Attribute( "TabSize" ) );
          Core.Settings.StripTrailingSpaces = GetBooleanFromString( xmlKey.Attribute( "StripTrailingSpaces" ) );
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
          Core.Settings.MRUMaxCount = GR.Convert.ToI32( xmlKey.Attribute( "MaxMRUCount" ) );
          if ( ( Core.Settings.MRUMaxCount < 1 )
          ||   ( Core.Settings.MRUMaxCount > 99 ) )
          {
            Core.Settings.MRUMaxCount = 4;
          }
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
                  Core.Settings.SourceFontStyle   = (FontStyle)GR.Convert.ToI32( xmlValue.Attribute( "FontStyle" ) );
                  break;
                case "BASIC":
                  Core.Settings.BASICSourceFontFamily = xmlValue.Attribute( "Family" );
                  Core.Settings.BASICSourceFontSize   = GR.Convert.ToF32( xmlValue.Attribute( "Size" ) );
                  Core.Settings.BASICSourceFontStyle  = (FontStyle)GR.Convert.ToI32( xmlValue.Attribute( "FontStyle" ) );
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
      //checkAllowTabs.Checked = Core.Settings.AllowTabs;
      editTabSize.Text = Core.Settings.TabSize.ToString();
      checkStripTrailingSpaces.Checked = Core.Settings.StripTrailingSpaces;

      checkBASICStripSpaces.Checked = Core.Settings.BASICStripSpaces;

      checkAutoOpenLastSolution.Checked = Core.Settings.AutoOpenLastSolution;

      checkASMShowLineNumbers.Checked = !Core.Settings.ASMHideLineNumbers;
      checkASMShowCycles.Checked = Core.Settings.ASMShowCycles;
      checkASMShowSizes.Checked = Core.Settings.ASMShowBytes;
      checkASMShowMiniMap.Checked = Core.Settings.ASMShowMiniView;

      labelFontPreview.Font = new Font( Core.Settings.SourceFontFamily, Core.Settings.SourceFontSize, Core.Settings.SourceFontStyle );

      if ( !Core.Settings.BASICUseNonC64Font )
      {
        labelBASICFontPreview.Font = new System.Drawing.Font( Core.MainForm.m_FontC64.Families[0], Core.Settings.SourceFontSize, Core.Settings.SourceFontStyle, System.Drawing.GraphicsUnit.Pixel );
      }
      else
      {
        labelBASICFontPreview.Font = new Font( Core.Settings.BASICSourceFontFamily, Core.Settings.BASICSourceFontSize, Core.Settings.BASICSourceFontStyle );
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

            Core.Settings.ToolInfos.Add( toolInfo );
          }
          catch ( Exception ex )
          {
            Core.AddToOutput( "Could not parse element: " + ex.Message + System.Environment.NewLine );
          }
        }
      }
      RefillToolInfoList();
      if ( alistTools.Items.Count > 0 )
      {
        alistTools.SelectedIndex = 0;
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



    private void ImportWarningsAsErrors( GR.Strings.XMLElement XMLRoot )
    {
      GR.Strings.XMLElement     xmlSettingRoot = XMLRoot.FindByTypeRecursive( "WarningsAsErrors" );
      if ( xmlSettingRoot == null )
      {
        return;
      }

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

            Core.Settings.SetSyntaxColor( element,
                                          GR.Convert.ToU32( xmlKey.Attribute( "FGColor" ), 16 ),
                                          GR.Convert.ToU32( xmlKey.Attribute( "BGColor" ), 16 ),
                                          ( xmlKey.Attribute( "BGColor" ).ToUpper() == "AUTO" ) );
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
        else if ( xmlKey.Type == "StripREM " )
        {
          Core.Settings.BASICStripREM = GetBooleanFromString( xmlKey.Content );
        }
        else if ( xmlKey.Type == "AutoToggleEntryMode" )
        {
          Core.Settings.BASICAutoToggleEntryMode = GetBooleanFromString( xmlKey.Content );
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
      //xmlTabs.AddAttribute( "AllowTabs", Core.Settings.AllowTabs ? "yes" : "no" );
      xmlTabs.AddAttribute( "ConvertTabsToSpaces", Core.Settings.TabConvertToSpaces ? "yes" : "no" );
      xmlTabs.AddAttribute( "StripTrailingSpaces", Core.Settings.StripTrailingSpaces ? "yes" : "no" );

      GR.Strings.XMLElement     xmlASMEditor = new GR.Strings.XMLElement( "AssemblerEditor" );
      xmlSettingRoot.AddChild( xmlASMEditor );
      xmlASMEditor.AddAttribute( "ShowLineNumbers", Core.Settings.ASMHideLineNumbers ? "no" : "yes" );
      xmlASMEditor.AddAttribute( "ShowByteSize", Core.Settings.ASMShowBytes ? "yes" : "no" );
      xmlASMEditor.AddAttribute( "ShowCycles", Core.Settings.ASMShowCycles ? "yes" : "no" );
      xmlASMEditor.AddAttribute( "ShowMiniView", Core.Settings.ASMShowMiniView ? "yes" : "no" );

      GR.Strings.XMLElement     xmlEnvironment = new GR.Strings.XMLElement( "Environment" );
      xmlSettingRoot.AddChild( xmlEnvironment );
      xmlEnvironment.AddAttribute( "OpenLastSolutionOnStartup", Core.Settings.AutoOpenLastSolution ? "yes" : "no" );
      xmlEnvironment.AddAttribute( "MaxMRUCount", Core.Settings.MRUMaxCount.ToString() );

      GR.Strings.XMLElement     xmlFonts = new GR.Strings.XMLElement( "Fonts" );
      xmlSettingRoot.AddChild( xmlFonts );

      var xmlFont = new GR.Strings.XMLElement( "Font" );
      xmlFont.AddAttribute( "Type", "ASM" );
      xmlFont.AddAttribute( "Family", Core.Settings.SourceFontFamily );
      xmlFont.AddAttribute( "Size", Core.Settings.SourceFontSize.ToString() );
      xmlFont.AddAttribute( "Style", ( (int)Core.Settings.SourceFontStyle ).ToString() );
      xmlFonts.AddChild( xmlFont );

      xmlFont = new GR.Strings.XMLElement( "Font" );
      xmlFont.AddAttribute( "Type", "BASIC" );
      xmlFont.AddAttribute( "Family", Core.Settings.BASICSourceFontFamily );
      xmlFont.AddAttribute( "Size", Core.Settings.BASICSourceFontSize.ToString() );
      xmlFont.AddAttribute( "Style", ( (int)Core.Settings.BASICSourceFontStyle ).ToString() );
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



    private void ExportWarningsAsErrors( GR.Strings.XMLElement XMLRoot )
    {
      GR.Strings.XMLElement     xmlSettingRoot = new GR.Strings.XMLElement( "WarningsAsErrors" );
      XMLRoot.AddChild( xmlSettingRoot );

      foreach ( Types.ErrorCode element in Core.Settings.TreatWarningsAsErrors )
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

      foreach ( Types.ColorableElement element in System.Enum.GetValues( typeof( Types.ColorableElement ) ) )
      {
        if ( element == C64Studio.Types.ColorableElement.LAST_ENTRY )
        {
          continue;
        }
        var xmlColor = new GR.Strings.XMLElement( "Color" );
        xmlColor.AddAttribute( "Element", element.ToString() );

        xmlColor.AddAttribute( "FGColor", Core.Settings.FGColor( element ).ToString( "X4" ) );
        if ( Core.Settings.BGColorIsAuto( element ) )
        {
          xmlColor.AddAttribute( "BGColor", "Auto" );
        }
        else
        {
          xmlColor.AddAttribute( "BGColor", Core.Settings.BGColor( element ).ToString( "X4" ) );
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
      xmlSettingRoot.AddChild( "AutoToggleEntryMode", Core.Settings.BASICAutoToggleEntryMode ? "yes" : "no" );
      xmlSettingRoot.AddChild( "StripREM", Core.Settings.BASICStripREM ? "yes" : "no" );
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
          ImportWarningsAsErrors( xmlSettings );
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
          ExportWarningsAsErrors( xmlRoot );
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
      ExportWarningsAsErrors( xmlRoot );
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
      ImportWarningsAsErrors( xmlSettings );
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
      alistTools.SelectedItem.Text = tool.Name;

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



    private ArrangedItemEntry asmLibraryPathList_AddingItem( object sender )
    {
      var newEntry = new ArrangedItemEntry( editASMLibraryPath.Text );
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



    private void asmLibraryPathList_ItemAdded( object sender, ArrangedItemEntry Item )
    {
      ApplyLibraryPathsFromList();
    }



    private void ApplyLibraryPathsFromList()
    {
      Core.Settings.ASMLibraryPaths.Clear();
      foreach ( ArrangedItemEntry entry in asmLibraryPathList.Items )
      {
        Core.Settings.ASMLibraryPaths.Add( entry.Text );
      }
    }



    private void asmLibraryPathList_ItemMoved( object sender, ArrangedItemEntry Item1, ArrangedItemEntry Item2 )
    {
      ApplyLibraryPathsFromList();
    }



    private void asmLibraryPathList_ItemRemoved( object sender, ArrangedItemEntry Item )
    {
      ApplyLibraryPathsFromList();
    }



    private void btnBrowseDefaultOpenSolutionPath_Click( object sender, EventArgs e )
    {
      FolderBrowserDialog   dlg = new FolderBrowserDialog();

      dlg.SelectedPath = Core.Settings.DefaultProjectBasePath;
      dlg.Description = "Choose default open solution/project path";
      if ( dlg.ShowDialog() == DialogResult.OK )
      {
        Core.Settings.DefaultProjectBasePath = dlg.SelectedPath;
        editDefaultOpenSolutionPath.Text = dlg.SelectedPath;
      }
    }



    private void checkASMShowAddress_CheckedChanged( object sender, EventArgs e )
    {
      if ( checkASMShowAddress.Checked != Core.Settings.ASMShowAddress )
      {
        Core.Settings.ASMShowAddress = checkASMShowAddress.Checked;
        RefreshDisplayOnDocuments();
      }
    }



    private void btnBindKey2_Click( object sender, EventArgs e )
    {
      if ( listFunctions.SelectedItems.Count == 0 )
      {
        return;
      }
      Types.Function function = (Types.Function)listFunctions.SelectedItems[0].Tag;

      bool hadAccelerator = false;
      foreach ( var accPair in Core.Settings.Accelerators )
      {
        if ( accPair.Value.Function == function )
        {
          if ( m_PressedKey != Keys.None )
          {
            accPair.Value.SecondaryKey = m_PressedKey;
            hadAccelerator = true;
            listFunctions.SelectedItems[0].SubItems[3].Text = m_PressedKey.ToString();
          }
          break;
        }
      }
      if ( !hadAccelerator )
      {
        // no entry yet, add as primary key
        if ( m_PressedKey != Keys.None )
        {
          AcceleratorKey key = new AcceleratorKey( m_PressedKey, function );
          key.Key = m_PressedKey;
          Core.Settings.Accelerators.Add( key.Key, key );
          listFunctions.SelectedItems[0].SubItems[2].Text = m_PressedKey.ToString();
        }
      }

      btnUnbindKey.Enabled = ( Core.Settings.DetermineAccelerator( function ) != null );

      Core.MainForm.RaiseApplicationEvent( new C64Studio.Types.ApplicationEvent( C64Studio.Types.ApplicationEvent.Type.KEY_BINDINGS_MODIFIED ) );
      RefreshDisplayOnDocuments();
    }



    private void checkBASICAutoToggleEntryMode_CheckedChanged( object sender, EventArgs e )
    {
      Core.Settings.BASICAutoToggleEntryMode = checkBASICAutoToggleEntryMode.Checked;
    }



    private void checkBASICStripREM_CheckedChanged( object sender, EventArgs e )
    {
      Core.Settings.BASICStripREM = checkBASICStripREM.Checked;
      Core.Compiling.ParserBasic.Settings.StripREM = Core.Settings.BASICStripREM;
    }



    private void checkStripTrailingSpaces_CheckedChanged( object sender, EventArgs e )
    {
      Core.Settings.StripTrailingSpaces = checkStripTrailingSpaces.Checked;
    }



    private void editMaxMRUEntries_TextChanged( object sender, EventArgs e )
    {
      int     mruCount = GR.Convert.ToI32( editMaxMRUEntries.Text );
      if ( ( mruCount < 1 )
      ||   ( mruCount > 99 ) )
      {
        editMaxMRUEntries.Text = "4";
        mruCount = 4;
      }
      Core.Settings.MRUMaxCount = mruCount;
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



    private ArrangedItemEntry alistTools_AddingItem( object sender )
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



    private void alistTools_ItemMoved( object sender, ArrangedItemEntry Item1, ArrangedItemEntry Item2 )
    {
      Core.Settings.ToolInfos.Clear();
      foreach ( ArrangedItemEntry item in alistTools.Items )
      {
        Core.Settings.ToolInfos.Add( (ToolInfo)item.Tag );
      }
    }



    private void alistTools_ItemRemoved( object sender, ArrangedItemEntry Item )
    {
      Core.Settings.ToolInfos.Clear();
      foreach ( ArrangedItemEntry item in alistTools.Items )
      {
        Core.Settings.ToolInfos.Add( (ToolInfo)item.Tag );
      }
    }



    private void alistTools_ItemAdded( object sender, ArrangedItemEntry Item )
    {
      var tool = (ToolInfo)Item.Tag;
      Core.Settings.ToolInfos.Add( tool );
      if ( tool.Type == ToolInfo.ToolType.EMULATOR )
      {
        Core.MainForm.RaiseApplicationEvent( new C64Studio.Types.ApplicationEvent( C64Studio.Types.ApplicationEvent.Type.EMULATOR_LIST_CHANGED ) );
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



    private void listC64StudioHacks_ItemCheck( object sender, ItemCheckEventArgs e )
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



    private void comboASMEncoding_SelectedIndexChanged( object sender, EventArgs e )
    {
      var  newEncoding = (GR.Generic.Tupel<string, Encoding>)comboASMEncoding.SelectedItem;

      Core.Settings.SourceFileEncoding = newEncoding.second;
    }



  }
}

