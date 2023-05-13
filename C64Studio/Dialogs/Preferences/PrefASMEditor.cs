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
  public partial class PrefASMEditor : PrefBase
  {
    public PrefASMEditor()
    {
      InitializeComponent();
    }



    public PrefASMEditor( StudioCore Core ) : base( Core )
    {
      _Keywords.AddRange( new string[] { "asm", "editor", "assembler" } );

      InitializeComponent();

      checkConvertTabsToSpaces.Checked  = Core.Settings.TabConvertToSpaces;
      editTabSize.Text                  = Core.Settings.TabSize.ToString();
      checkStripTrailingSpaces.Checked  = Core.Settings.StripTrailingSpaces;
      labelFontPreview.Font             = new Font( Core.Settings.SourceFontFamily, Core.Settings.SourceFontSize, Core.Settings.SourceFontStyle );

      checkASMShowLineNumbers.Checked   = !Core.Settings.ASMHideLineNumbers;
      checkASMShowCycles.Checked        = Core.Settings.ASMShowCycles;
      checkASMShowSizes.Checked         = Core.Settings.ASMShowBytes;
      checkASMShowMiniMap.Checked       = Core.Settings.ASMShowMiniView;
      checkASMShowAutoComplete.Checked  = Core.Settings.ASMShowAutoComplete;
      checkASMShowAddress.Checked       = Core.Settings.ASMShowAddress;
      editMaxLineLengthIndicatorColumn.Text = Core.Settings.ASMShowMaxLineLengthIndicatorLength.ToString();
      checkEditorShowMaxLineLengthIndicator.Checked = ( Core.Settings.ASMShowMaxLineLengthIndicatorLength != 0 );

      var allEncodings = System.Text.Encoding.GetEncodings();

      foreach ( var encoding in allEncodings )
      {
        comboASMEncoding.Items.Add( new GR.Generic.Tupel<string, Encoding>( encoding.DisplayName + "      Codepage " + encoding.CodePage, encoding.GetEncoding() ) );
        if ( encoding.Name == Core.Settings.SourceFileEncoding.WebName )
        {
          comboASMEncoding.SelectedIndex = comboASMEncoding.Items.Count - 1;
        }
      }
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
      var xmlTabs = SettingsRoot.AddChild( "Generic.Tabs" );
      var xmlASMEditor = SettingsRoot.AddChild( "Generic.AssemblerEditor" );
      var xmlFonts = SettingsRoot.AddChild( "Generic.Fonts" );

      xmlTabs.AddAttribute( "TabSize", Core.Settings.TabSize.ToString() );
      xmlTabs.AddAttribute( "ConvertTabsToSpaces", Core.Settings.TabConvertToSpaces ? "yes" : "no" );
      xmlTabs.AddAttribute( "StripTrailingSpaces", Core.Settings.StripTrailingSpaces ? "yes" : "no" );

      xmlASMEditor.AddAttribute( "ShowLineNumbers", !Core.Settings.ASMHideLineNumbers ? "yes" : "no" );
      xmlASMEditor.AddAttribute( "ShowByteSize", Core.Settings.ASMShowBytes ? "yes" : "no" );
      xmlASMEditor.AddAttribute( "ShowCycles", Core.Settings.ASMShowCycles ? "yes" : "no" );
      xmlASMEditor.AddAttribute( "ShowMiniView", Core.Settings.ASMShowMiniView ? "yes" : "no" );
      xmlASMEditor.AddAttribute( "ShowAddress", Core.Settings.ASMShowAddress ? "yes" : "no" );
      xmlASMEditor.AddAttribute( "ShowAutoComplete", Core.Settings.ASMShowAutoComplete ? "yes" : "no" );
      xmlASMEditor.AddAttribute( "ShowMaxLineLengthIndicatorLength", Core.Settings.ASMShowMaxLineLengthIndicatorLength.ToString() );

      xmlASMEditor.AddAttribute( "Encoding", Core.Settings.SourceFileEncoding.WebName );

      var xmlFont = xmlFonts.AddChild( "Font" );
      xmlFont.AddAttribute( "Type", "ASM" );
      xmlFont.AddAttribute( "Family", Core.Settings.SourceFontFamily );
      xmlFont.AddAttribute( "Size", Util.DoubleToString( Core.Settings.SourceFontSize ) );
      xmlFont.AddAttribute( "Style", ( (int)Core.Settings.SourceFontStyle ).ToString() );
    }



    public override void ImportSettings( XMLElement SettingsRoot )
    {
      var xmlTabs = SettingsRoot.FindByType( "Generic.Tabs" );
      if ( xmlTabs != null )
      {
        editTabSize.Text = GR.Convert.ToI32( xmlTabs.Attribute( "TabSize" ) ).ToString();
        checkConvertTabsToSpaces.Checked = IsSettingTrue( xmlTabs.Attribute( "ConvertTabsToSpaces" ) );
        checkStripTrailingSpaces.Checked = IsSettingTrue( xmlTabs.Attribute( "StripTrailingSpaces" ) );
      }

      var xmlASMEditor = SettingsRoot.FindByType( "Generic.AssemblerEditor" );
      if ( xmlASMEditor != null )
      {
        checkASMShowLineNumbers.Checked   = IsSettingTrue( xmlASMEditor.Attribute( "ShowLineNumbers" ) );
        checkASMShowAddress.Checked       = IsSettingTrue( xmlASMEditor.Attribute( "ShowAddress" ) );
        checkASMShowAutoComplete.Checked  = IsSettingTrue( xmlASMEditor.Attribute( "ShowAutoComplete" ) );
        checkASMShowCycles.Checked        = IsSettingTrue( xmlASMEditor.Attribute( "ShowCycles" ) );
        checkASMShowMiniMap.Checked       = IsSettingTrue( xmlASMEditor.Attribute( "ShowMiniView" ) );
        checkASMShowSizes.Checked         = IsSettingTrue( xmlASMEditor.Attribute( "ShowByteSize" ) );
        editMaxLineLengthIndicatorColumn.Text = xmlASMEditor.Attribute( "ShowMaxLineLengthIndicatorLength" );

        try
        {
          string  encodingName = xmlASMEditor.Attribute( "Encoding" );
          int   itemIndex = 0;
          foreach ( GR.Generic.Tupel<string, Encoding> encodingItem in comboASMEncoding.Items )
          {
            if ( encodingItem.second.WebName == encodingName )
            {
              comboASMEncoding.SelectedIndex = itemIndex;
              break;
            }
            ++itemIndex;
          }
        }
        catch ( Exception )
        { 
        }
      }

      var xmlFonts = SettingsRoot.FindByType( "Generic.Fonts" );
      if ( xmlFonts != null )
      {
        foreach ( var xmlFont in xmlFonts )
        {
          if ( xmlFont.Attribute( "Type" ) == "ASM" )
          {
            Core.Settings.SourceFontFamily = xmlFont.Attribute( "Family" );
            Core.Settings.SourceFontSize = (float)Util.StringToDouble( xmlFont.Attribute( "Size" ) );
            Core.Settings.SourceFontStyle = (FontStyle)GR.Convert.ToI32( xmlFont.Attribute( "Style" ) );

            labelFontPreview.Font = new Font( Core.Settings.SourceFontFamily, Core.Settings.SourceFontSize, Core.Settings.SourceFontStyle );
          }
        }
      }
      RefreshDisplayOnDocuments();
    }



    private void checkConvertTabsToSpaces_CheckedChanged( object sender, EventArgs e )
    {
      if ( Core.Settings.TabConvertToSpaces != checkConvertTabsToSpaces.Checked )
      {
        Core.Settings.TabConvertToSpaces = checkConvertTabsToSpaces.Checked;
        RefreshDisplayOnDocuments();
      }
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



    private void checkStripTrailingSpaces_CheckedChanged( object sender, EventArgs e )
    {
      Core.Settings.StripTrailingSpaces = checkStripTrailingSpaces.Checked;
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
        labelFontPreview.Font = fontDialog.Font;

        RefreshDisplayOnDocuments();
      }
    }



    private void btnSetDefaultsFont_Click( object sender, EventArgs e )
    {
      Core.Settings.SourceFontFamily = "Consolas";
      Core.Settings.SourceFontSize = 9.0f;

      labelFontPreview.Font = new Font( Core.Settings.SourceFontFamily, Core.Settings.SourceFontSize, Core.Settings.SourceFontStyle );
      RefreshDisplayOnDocuments();
    }



    private void checkASMShowLineNumbers_CheckedChanged( object sender, EventArgs e )
    {
      if ( Core.Settings.ASMHideLineNumbers == checkASMShowLineNumbers.Checked )
      {
        Core.Settings.ASMHideLineNumbers = !checkASMShowLineNumbers.Checked;
        RefreshDisplayOnDocuments();
      }
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



    private void checkASMShowMiniMap_CheckedChanged( object sender, EventArgs e )
    {
      if ( checkASMShowMiniMap.Checked != Core.Settings.ASMShowMiniView )
      {
        Core.Settings.ASMShowMiniView = checkASMShowMiniMap.Checked;
        RefreshDisplayOnDocuments();
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



    private void checkASMShowAddress_CheckedChanged( object sender, EventArgs e )
    {
      if ( checkASMShowAddress.Checked != Core.Settings.ASMShowAddress )
      {
        Core.Settings.ASMShowAddress = checkASMShowAddress.Checked;
        RefreshDisplayOnDocuments();
      }
    }



    private void comboASMEncoding_SelectedIndexChanged( object sender, EventArgs e )
    {
      var  newEncoding = (GR.Generic.Tupel<string, Encoding>)comboASMEncoding.SelectedItem;

      Core.Settings.SourceFileEncoding = newEncoding.second;
    }



    private void checkEditorShowMaxLineLengthIndicator_CheckedChanged( object sender, EventArgs e )
    {
      editMaxLineLengthIndicatorColumn.Enabled = checkEditorShowMaxLineLengthIndicator.Checked;
      if ( !checkEditorShowMaxLineLengthIndicator.Checked )
      {
        Core.Settings.ASMShowMaxLineLengthIndicatorLength = 0;
        RefreshDisplayOnDocuments();
      }
    }



    private void editMaxLineLengthIndicatorColumn_TextChanged( object sender, EventArgs e )
    {
      int   maxColIndicator = GR.Convert.ToI32( editMaxLineLengthIndicatorColumn.Text );
      if ( ( maxColIndicator <= 0 )
      ||   ( maxColIndicator >= 200 ) )
      {
        maxColIndicator = 0;
      }
      if ( Core.Settings.ASMShowMaxLineLengthIndicatorLength != maxColIndicator )
      {
        Core.Settings.ASMShowMaxLineLengthIndicatorLength = maxColIndicator;
        RefreshDisplayOnDocuments();
      }
    }



  }
}
