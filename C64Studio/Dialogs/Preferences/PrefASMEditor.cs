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
    }



    private void btnImportSettings_Click( object sender, EventArgs e )
    {

    }



    private void btnExportSettings_Click( object sender, EventArgs e )
    {

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



  }
}
