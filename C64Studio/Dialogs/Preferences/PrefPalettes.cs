using GR.Strings;
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
  public partial class PrefPalettes : PrefBase
  {
    public PrefPalettes()
    {
      InitializeComponent();
    }



    public PrefPalettes( StudioCore Core ) : base( Core )
    {
      _Keywords.AddRange( new string[] { "colors", "palette" } );
      InitializeComponent();

      RefillPaletteList();
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
      GR.Strings.XMLElement     xmlSettingRoot = SettingsRoot.FindByTypeRecursive( "Palettes" );
      if ( xmlSettingRoot == null )
      {
        return;
      }

      foreach ( var xmlKey in xmlSettingRoot.ChildElements )
      {
        if ( xmlKey.Type == "Color" )
        {
        }
      }
      PalettesChanged();
    }



    public override void ExportSettings( XMLElement SettingsRoot )
    {
      GR.Strings.XMLElement     xmlSettingRoot = new GR.Strings.XMLElement( "Palettes" );
      SettingsRoot.AddChild( xmlSettingRoot );

      foreach ( Types.ColorableElement element in System.Enum.GetValues( typeof( Types.ColorableElement ) ) )
      {
        var xmlColor = new GR.Strings.XMLElement( "Color" );
      }
    }



    private void RefillPaletteList()
    {
      paletteEditor.Palettes = Core.Settings.Palettes;
    }



    private void PalettesChanged()
    {
      // TODO - ?
      RefreshDisplayOnDocuments();
    }



    private void paletteEditor_PaletteOrderModified( PaletteType Type )
    {
      Core.MainForm.RaiseApplicationEvent( new ApplicationEvent( ApplicationEvent.Type.DEFAULT_PALETTE_CHANGED ) { OriginalValue = Type.ToString() } );
    }



  }
}
