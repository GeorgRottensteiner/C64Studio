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



    public override bool HandleExport( GraphicScreenEditor editor, ExportGraphicScreenInfo Info, TextBox EditOutput, DocumentInfo DocInfo )
    {
      if ( comboCharScreens.SelectedIndex == -1 )
      {
        return false;
      }
      Debug.Log( "HandleExport a" );
      if ( !ApplyCharsetChecks( editor, Info, false, out var charScreenData, out var charsetData ) )
      {
        return false;
      }
      Debug.Log( "HandleExport b" );

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

        project.Chars = charScreenData;
        project.CharSet.Colors = charset.Colors;

        Debug.Log( "HandleExport c" );
        charset.TotalNumberOfCharacters = Lookup.NumCharactersForMode( Lookup.CharacterModeFromCheckType( Info.Project.SelectedCheckType ) );
        for  ( int i = 0; i < charset.Characters.Count; ++i )
        {
          charset.Characters[i].Tile = new GraphicTile( Lookup.CharacterWidthInPixel( Lookup.GraphicTileModeFromTextCharMode( Lookup.CharacterModeFromCheckType( Info.Project.SelectedCheckType ), 0 ) ),
                                    Lookup.CharacterHeightInPixel( Lookup.GraphicTileModeFromTextCharMode( Lookup.CharacterModeFromCheckType( Info.Project.SelectedCheckType ), 0 ) ),
                                    Lookup.GraphicTileModeFromTextCharMode( Lookup.CharacterModeFromCheckType( Info.Project.SelectedCheckType ), 1 ),
                                    charset.Colors );
        }
        Debug.Log( "HandleExport d" );
        while ( charset.Characters.Count < charset.TotalNumberOfCharacters )
        {
          var newChar = new CharData()
          {
            Tile = new GraphicTile( Lookup.CharacterWidthInPixel( Lookup.GraphicTileModeFromTextCharMode( Lookup.CharacterModeFromCheckType( Info.Project.SelectedCheckType ), 0 ) ),
                                    Lookup.CharacterHeightInPixel( Lookup.GraphicTileModeFromTextCharMode( Lookup.CharacterModeFromCheckType( Info.Project.SelectedCheckType ), 0 ) ), 
                                    Lookup.GraphicTileModeFromTextCharMode( Lookup.CharacterModeFromCheckType( Info.Project.SelectedCheckType ), 1 ),
                                    charset.Colors )
          };
          newChar.Tile.CustomColor = 1;
          charset.Characters.Add( newChar );
        }
        Debug.Log( "HandleExport e" );

        for ( int i = 0; i < charset.TotalNumberOfCharacters; ++i )
        {
          charsetData.CopyTo( charset.Characters[i].Tile.Data, i * Lookup.NumBytesOfSingleCharacterBitmap( charset.Mode ), Lookup.NumBytesOfSingleCharacterBitmap( charset.Mode ) );
        }
        Debug.Log( "HandleExport f" );
        charScreen.InjectProjects( project, charset );
        charScreen.SetModified();
      }

      return true;
    }



  }
}
