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
using System.Xml.Serialization;



namespace RetroDevStudio.Dialogs.Preferences
{
  [Description( "General.Palettes" )]
  public partial class DlgPrefPalettes : DlgPrefBase
  {
    public DlgPrefPalettes()
    {
      InitializeComponent();
    }



    public DlgPrefPalettes( StudioCore Core ) : base( Core )
    {
      _Keywords.AddRange( new string[] { "colors", "palette" } );

      InitializeComponent();
    }



    public override void ApplySettingsToControls()
    {
      RefillPaletteList();
      PalettesChanged();
    }



    public override void ImportSettings( XMLElement SettingsRoot )
    {
      GR.Strings.XMLElement     xmlSettingRoot = SettingsRoot.FindByTypeRecursive( "Palettes" );
      if ( xmlSettingRoot == null )
      {
        return;
      }

      Core.Settings.Palettes.Clear();
      foreach ( var xmlPalette in xmlSettingRoot.ChildElements )
      {
        if ( xmlPalette.Type == "Palette" )
        {
          var palType = (PaletteType)Enum.Parse( typeof( PaletteType ), xmlPalette.Attribute( "PaletteType" ), true );

          var pal = PaletteManager.PaletteFromType( palType );

          pal.Name = xmlPalette.Attribute( "Name" );

          int colorIndex = 0;
          foreach ( var xmlColor in xmlPalette )
          {
            if ( xmlColor.Type == "Color" )
            {
              pal.ColorValues[colorIndex] = GR.Convert.ToU32( xmlColor.Content, 16 ) | 0xff000000;

              ++colorIndex;
            }
          }
          if ( !Core.Settings.Palettes.ContainsKey( palType ) )
          {
            Core.Settings.Palettes.Add( palType, new List<Palette>() );
          }
          Core.Settings.Palettes[palType].Add( pal );
        }
      }
      // make sure we have at least one palette per system
      Core.Settings.SanitizePalettes();
    }



    public override void ExportSettings( XMLElement SettingsRoot )
    {
      GR.Strings.XMLElement     xmlSettingRoot = new GR.Strings.XMLElement( "Palettes" );
      SettingsRoot.AddChild( xmlSettingRoot );

      foreach ( var paletteSystems in Core.Settings.Palettes )
      {
        foreach ( var palette in paletteSystems.Value )
        {
          var xmlPalette = new GR.Strings.XMLElement( "Palette" );
          xmlPalette.AddAttribute( "PaletteType", paletteSystems.Key.ToString() );
          xmlPalette.AddAttribute( "Name", palette.Name );

          for ( int i = 0; i < palette.NumColors; i++ )
          {
            xmlPalette.AddChild( "Color", palette.ColorValues[i].ToString( "X4" ) );
          }
          xmlSettingRoot.AddChild( xmlPalette );
        }
      }
    }



    private void RefillPaletteList()
    {
      paletteEditor.Palettes = Core.Settings.Palettes;
    }



    private void PalettesChanged()
    {
      Core.MainForm.RaiseApplicationEvent( new ApplicationEvent( Types.ApplicationEvent.Type.DEFAULT_PALETTE_CHANGED ) );
    }



    private void paletteEditor_PaletteOrderModified( PaletteType Type )
    {
      Core.MainForm.RaiseApplicationEvent( new ApplicationEvent( ApplicationEvent.Type.DEFAULT_PALETTE_CHANGED ) { OriginalValue = Type.ToString() } );
    }



    private void paletteEditor_PaletteModified( PaletteType Type, Palette Palette )
    {
      Core.MainForm.RaiseApplicationEvent( new ApplicationEvent( Types.ApplicationEvent.Type.DEFAULT_PALETTE_CHANGED ) );
    }



  }
}
