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
  public partial class PrefBASICEditor : PrefBase
  {
    public PrefBASICEditor()
    {
      InitializeComponent();
    }



    public PrefBASICEditor( StudioCore Core ) : base( Core )
    {
      _Keywords.AddRange( new string[] { "basic", "editor", "font" }  );

      InitializeComponent();

      checkBASICUseC64Font.Checked = !Core.Settings.BASICUseNonC64Font;
      editBASICC64FontSize.Enabled = !Core.Settings.BASICUseNonC64Font;
      if ( !Core.Settings.BASICUseNonC64Font )
      {
        labelBASICFontPreview.Font = new System.Drawing.Font( Core.MainForm.m_FontC64.Families[0], Core.Settings.BASICSourceFontSize, System.Drawing.GraphicsUnit.Pixel );
      }
      else
      {
        labelBASICFontPreview.Font = new Font( Core.Settings.BASICSourceFontFamily, Core.Settings.BASICSourceFontSize, Core.Settings.BASICSourceFontStyle );
      }
      editBASICC64FontSize.Text = ( (int)Core.Settings.BASICSourceFontSize ).ToString();
      editMaxLineLengthIndicatorColumn.Text = Core.Settings.BASICShowMaxLineLengthIndicatorLength.ToString();
      checkBASICEditorShowMaxLineLengthIndicator.Checked = ( Core.Settings.BASICShowMaxLineLengthIndicatorLength != 0 );
    }



    private void btnImportSettings_Click( DecentForms.ControlBase Sender )
    {
      ImportLocalSettings();
    }



    private void btnExportSettings_Click( DecentForms.ControlBase Sender )
    {
      SaveLocalSettings();
    }



    public override void ImportSettings( XMLElement SettingsRoot )
    {
      var xmlFonts = SettingsRoot.FindByType( "Generic.Fonts" );
      if ( xmlFonts != null )
      {
        foreach ( var xmlFont in xmlFonts )
        {
          if ( xmlFont.Attribute( "Type" ) == "BASIC" )
          {
            Core.Settings.BASICSourceFontFamily                 = xmlFont.Attribute( "Family" );
            Core.Settings.BASICSourceFontSize                   = GR.Convert.ToF32( xmlFont.Attribute( "Size" ) );
            Core.Settings.BASICSourceFontStyle                  = (FontStyle)GR.Convert.ToI32( xmlFont.Attribute( "FontStyle" ) );
            Core.Settings.BASICUseNonC64Font                    = !IsSettingTrue( xmlFont.Attribute( "UseC64Font" ) );

            checkBASICUseC64Font.Checked = !Core.Settings.BASICUseNonC64Font;
            editBASICC64FontSize.Enabled = !Core.Settings.BASICUseNonC64Font;
            if ( !Core.Settings.BASICUseNonC64Font )
            {
              labelBASICFontPreview.Font = new System.Drawing.Font( Core.MainForm.m_FontC64.Families[0], Core.Settings.BASICSourceFontSize, System.Drawing.GraphicsUnit.Pixel );
            }
            else
            {
              labelBASICFontPreview.Font = new Font( Core.Settings.BASICSourceFontFamily, Core.Settings.BASICSourceFontSize, Core.Settings.BASICSourceFontStyle );
            }
            editBASICC64FontSize.Text = ( (int)Core.Settings.BASICSourceFontSize ).ToString();
            editMaxLineLengthIndicatorColumn.Text = Core.Settings.BASICShowMaxLineLengthIndicatorLength.ToString();
            checkBASICEditorShowMaxLineLengthIndicator.Checked = ( Core.Settings.BASICShowMaxLineLengthIndicatorLength != 0 );
          }
        }
      }
      var xmlBASICEditor = SettingsRoot.FindByType( "BASICEditor" );
      if ( xmlBASICEditor != null )
      {
        Core.Settings.BASICShowMaxLineLengthIndicatorLength = GR.Convert.ToI32( xmlBASICEditor.Attribute( "ShowMaxLineLengthIndicatorLength" ) );
      }
    }



    public override void ExportSettings( XMLElement SettingsRoot )
    {
      var xmlFont = SettingsRoot.AddChild( "Generic.Fonts.Font" );

      xmlFont.AddAttribute( "Type", "BASIC" );
      xmlFont.AddAttribute( "Family", Core.Settings.BASICSourceFontFamily );
      xmlFont.AddAttribute( "Size", Core.Settings.BASICSourceFontSize.ToString() );
      xmlFont.AddAttribute( "Style", ( (int)Core.Settings.BASICSourceFontStyle ).ToString() );
      xmlFont.AddAttribute( "UseC64Font", Core.Settings.BASICUseNonC64Font ? "no" : "yes" );

      var xmlBASICEditor = SettingsRoot.AddChild( "BASICEditor" );
      xmlBASICEditor.AddAttribute( "ShowMaxLineLengthIndicatorLength", Core.Settings.BASICShowMaxLineLengthIndicatorLength.ToString() );
    }



    private void checkBASICUseC64Font_CheckedChanged( object sender, EventArgs e )
    {
      if ( Core.Settings.BASICUseNonC64Font != !checkBASICUseC64Font.Checked )
      {
        Core.Settings.BASICUseNonC64Font = !checkBASICUseC64Font.Checked;

        if ( !Core.Settings.BASICUseNonC64Font )
        {
          labelBASICFontPreview.Font    = new System.Drawing.Font( Core.MainForm.m_FontC64.Families[0], Core.Settings.BASICSourceFontSize, System.Drawing.GraphicsUnit.Pixel );
          btnChangeBASICFont.Enabled    = false;
          labelBASICC64FontSize.Enabled = true;
          editBASICC64FontSize.Enabled  = true;
        }
        else
        {
          labelBASICFontPreview.Font    = new Font( Core.Settings.BASICSourceFontFamily, Core.Settings.BASICSourceFontSize );
          btnChangeBASICFont.Enabled    = true;
          labelBASICC64FontSize.Enabled = false;
          editBASICC64FontSize.Enabled  = false;
        }
        RefreshDisplayOnDocuments();
      }
    }



    private void btnChangeBASICFont_Click( DecentForms.ControlBase Sender )
    {
      System.Windows.Forms.FontDialog fontDialog = new FontDialog();

      fontDialog.Font = labelBASICFontPreview.Font;
      try
      {
        if ( fontDialog.ShowDialog() == DialogResult.OK )
        {
          Core.Settings.BASICSourceFontFamily = fontDialog.Font.FontFamily.Name;
          Core.Settings.BASICSourceFontSize   = fontDialog.Font.SizeInPoints;
          labelBASICFontPreview.Font          = new Font( Core.Settings.BASICSourceFontFamily, Core.Settings.BASICSourceFontSize );

          RefreshDisplayOnDocuments();
        }
      }
      catch ( Exception ex )
      {
        System.Windows.Forms.MessageBox.Show( $"The system returned the error {ex.Message}.\r\nPlease verify whether the chosen font is properly installed for all users.", "Error during selecting font" );
      }
    }
  
    
    
    private void editBASICC64FontSize_TextChanged( object sender, EventArgs e )
    {
      int     fontSize = GR.Convert.ToI32( editBASICC64FontSize.Text );
      if ( ( fontSize <= 0 )
      ||   ( fontSize >= 200 ) )
      {
        fontSize = 9;
      }
      if ( fontSize != Core.Settings.BASICSourceFontSize )
      {
        Core.Settings.BASICSourceFontSize = fontSize;
        labelBASICFontPreview.Font = new Font( Core.MainForm.m_FontC64.Families[0], Core.Settings.BASICSourceFontSize, System.Drawing.GraphicsUnit.Pixel );
        RefreshDisplayOnDocuments();
      }
    }



    private void checkBASICEditorShowMaxLineLengthIndicator_CheckedChanged( object sender, EventArgs e )
    {
      editMaxLineLengthIndicatorColumn.Enabled = checkBASICEditorShowMaxLineLengthIndicator.Checked;
      if ( !checkBASICEditorShowMaxLineLengthIndicator.Checked )
      {
        Core.Settings.BASICShowMaxLineLengthIndicatorLength = 0;
        RefreshDisplayOnDocuments();
      }
    }



    private void editMaxLineLengthIndicatorColumn_TextChanged( object sender, EventArgs e )
    {
      int   maxColIndicator = GR.Convert.ToI32( editMaxLineLengthIndicatorColumn.Text );
      if ( ( maxColIndicator <= 0 )
      ||   ( maxColIndicator >= 200 ) )
      {
        maxColIndicator = 80;
      }
      if ( Core.Settings.BASICShowMaxLineLengthIndicatorLength != maxColIndicator )
      {
        Core.Settings.BASICShowMaxLineLengthIndicatorLength = maxColIndicator;
        RefreshDisplayOnDocuments();
      }
    }



  }
}
