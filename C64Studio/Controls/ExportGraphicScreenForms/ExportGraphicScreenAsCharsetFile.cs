using GR.Memory;
using RetroDevStudio;
using RetroDevStudio.Documents;
using RetroDevStudio.Formats;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;



namespace RetroDevStudio.Controls
{
  public partial class ExportGraphicScreenAsCharsetFile : ExportGraphicScreenFormBase
  {
    public ExportGraphicScreenAsCharsetFile() :
      base( null )
    { 
    }



    public ExportGraphicScreenAsCharsetFile( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();

      comboCharScreens.Items.Add( new Types.ComboItem( "To new file" ) );
      foreach ( DocumentInfo doc in Core.MainForm.DocumentInfos )
      {
        if ( doc.Type == ProjectElement.ElementType.CHARACTER_SCREEN )
        {
          comboCharScreens.Items.Add( new Types.ComboItem( doc.DocumentFilename, doc ) );
        }
      }
      comboCharScreens.SelectedIndex = 0;
    }



    public override bool HandleExport( ExportGraphicScreenInfo Info, TextBox EditOutput, DocumentInfo DocInfo )
    {
      if ( comboCharScreens.SelectedIndex == -1 )
      {
        return false;
      }
      // automatic check
      if ( Info.Chars.Any( c => !string.IsNullOrEmpty( c.Error ) ) )
      {
        MessageBox.Show( "Cannot export to charset, conversion had errors!", "Cannot export to charset" );
        return false;
      }

      DocumentInfo    docToImportTo = (DocumentInfo)( (Types.ComboItem)comboCharScreens.SelectedItem ).Tag;

      CharsetScreenEditor   charScreen = null;

      if ( docToImportTo == null )
      {
        // import to new file
        charScreen = (CharsetScreenEditor)Core.MainForm.CreateNewDocument( ProjectElement.ElementType.CHARACTER_SCREEN, Core.MainForm.CurrentProject );
      }
      else
      {
        // import to existing file
        if ( docToImportTo.BaseDoc == null )
        {
          docToImportTo.Project.ShowDocument( docToImportTo.Element );
        }
        charScreen = (CharsetScreenEditor)docToImportTo.BaseDoc;
      }

      if ( charScreen != null )
      {
        Formats.CharsetScreenProject    project = new RetroDevStudio.Formats.CharsetScreenProject();
        Formats.CharsetProject          charset = new RetroDevStudio.Formats.CharsetProject();

        project.SetScreenSize( Info.BlockWidth, Info.BlockHeight );

        project.CharSet.Colors.BackgroundColor      = Info.Project.Colors.BackgroundColor;
        project.CharSet.Colors.MultiColor1          = Info.Project.Colors.MultiColor1;
        project.CharSet.Colors.MultiColor2          = Info.Project.Colors.MultiColor2;
        project.CharSet.Colors.BGColor4             = Info.Project.Colors.BGColor4;
        project.CharSet.Colors.PaletteIndexMapping  = Info.Project.Colors.PaletteIndexMapping;
        project.CharSet.Colors.PaletteMappingIndex  = Info.Project.Colors.PaletteMappingIndex;

        switch ( Info.Project.SelectedCheckType )
        {
          case Formats.GraphicScreenProject.CheckType.HIRES_BITMAP:
          case Formats.GraphicScreenProject.CheckType.HIRES_CHARSET:
            project.Mode = TextMode.COMMODORE_40_X_25_HIRES;
            charset.Mode = TextCharMode.COMMODORE_HIRES;
            break;
          case Formats.GraphicScreenProject.CheckType.MULTICOLOR_BITMAP:
          case Formats.GraphicScreenProject.CheckType.MULTICOLOR_CHARSET:
            project.Mode = TextMode.COMMODORE_40_X_25_MULTICOLOR;
            charset.Mode = TextCharMode.COMMODORE_MULTICOLOR;
            break;
          case Formats.GraphicScreenProject.CheckType.MEGA65_FCM_CHARSET:
            project.Mode = TextMode.MEGA65_40_X_25_FCM;
            charset.Mode = TextCharMode.MEGA65_FCM;
            break;
          case Formats.GraphicScreenProject.CheckType.MEGA65_FCM_CHARSET_16BIT:
            project.Mode = TextMode.MEGA65_40_X_25_FCM_16BIT;
            charset.Mode = TextCharMode.MEGA65_FCM_16BIT;
            break;
          case Formats.GraphicScreenProject.CheckType.VIC20_CHARSET:
            project.Mode = TextMode.COMMODORE_VIC20_8_X_8;
            charset.Mode = TextCharMode.VIC20;
            break;
          case Formats.GraphicScreenProject.CheckType.VIC20_CHARSET_8X16:
            project.Mode = TextMode.COMMODORE_VIC20_8_X_16;
            charset.Mode = TextCharMode.VIC20_8X16;
            break;
          default:
            Debug.Log( $"ExportToCharscreen: Unsupported Check Type {Info.Project.SelectedCheckType}" );
            break;
        }
        charset.Colors.Palette = new Palette( Info.Project.Colors.Palette );
        charset.Colors.BackgroundColor = project.CharSet.Colors.BackgroundColor;
        charset.Colors.MultiColor1 = project.CharSet.Colors.MultiColor1;
        charset.Colors.MultiColor2 = project.CharSet.Colors.MultiColor2;
        charset.Colors.BGColor4 = project.CharSet.Colors.BGColor4;
        charset.Colors.PaletteIndexMapping = project.CharSet.Colors.PaletteIndexMapping;
        charset.Colors.PaletteMappingIndex = project.CharSet.Colors.PaletteMappingIndex;

        for ( int i = 0; i < Info.Chars.Count; ++i )
        {
          project.Chars[i] = (uint)( ( Info.Chars[i].Index & 0xffff ) | ( Info.Chars[i].Tile.CustomColor << 16 ) );
          if ( Info.Chars[i].Replacement == null )
          {
            charset.Characters[Info.Chars[i].Index].Tile.Data = Info.Chars[i].Tile.Data;
          }
          else
          {
            project.Chars[i] = (uint)( ( Info.Chars[i].Replacement.Index & 0xffff ) | ( Info.Chars[i].Tile.CustomColor << 16 ) );
          }
        }

        charScreen.InjectProjects( project, charset );
        charScreen.SetModified();
      }

      return true;
    }



  }
}
