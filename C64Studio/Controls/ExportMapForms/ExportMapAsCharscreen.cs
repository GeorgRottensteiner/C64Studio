using RetroDevStudio.Types;
using RetroDevStudio.Formats;
using System.Windows.Forms;
using RetroDevStudio.Documents;
using static RetroDevStudio.Documents.BaseDocument;
using System.Xml.Linq;

namespace RetroDevStudio.Controls
{
  public partial class ExportMapAsCharscreen : ExportMapFormBase
  {
    public ExportMapAsCharscreen() :
      base( null )
    { 
    }



    public ExportMapAsCharscreen( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();

      comboCharscreenFiles.Items.Add( new Types.ComboItem( "To new char screen project" ) );
      UtilForms.FillComboWithFilesOfType( Core, comboCharscreenFiles, ProjectElement.ElementType.CHARACTER_SCREEN );
      comboCharscreenFiles.SelectedIndex = 0;

      Core.MainForm.ApplicationEvent += new MainForm.ApplicationEventHandler( MainForm_ApplicationEvent );
    }



    private void MainForm_ApplicationEvent( ApplicationEvent Event )
    {
      UtilForms.AdaptComboWithFilesOfType( Core, Event, comboCharscreenFiles, ProjectElement.ElementType.CHARACTER_SCREEN );
    }



    public override bool HandleExport( ExportMapInfo Info, TextBox EditOutput, DocumentInfo DocInfo )
    {
      var mapToExport = Info.CurrentMap;

      if ( mapToExport == null )
      {
        if ( Info.Map.Maps.Count == 0 )
        {
          MessageBox.Show( "There is no map to export.", "Cannot export" );
          return false;
        }
        mapToExport = Info.Map.Maps[0];
      }

      GR.Memory.ByteBuffer      charData = new GR.Memory.ByteBuffer( (uint)( mapToExport.Tiles.Width * mapToExport.TileSpacingX * mapToExport.Tiles.Height * mapToExport.TileSpacingY ) );
      GR.Memory.ByteBuffer      colorData = new GR.Memory.ByteBuffer( (uint)( mapToExport.Tiles.Width * mapToExport.TileSpacingX * mapToExport.Tiles.Height * mapToExport.TileSpacingY ) );

      for ( int y = 0; y < mapToExport.Tiles.Height; ++y )
      {
        for ( int x = 0; x < mapToExport.Tiles.Width; ++x )
        {
          int tileIndex = mapToExport.Tiles[x, y];
          if ( tileIndex < Info.Map.Tiles.Count )
          {
            // a real tile
            var tile = Info.Map.Tiles[tileIndex];

            for ( int j = 0; j < tile.Chars.Height; ++j )
            {
              for ( int i = 0; i < tile.Chars.Width; ++i )
              {
                charData.SetU8At( x * mapToExport.TileSpacingX + i + ( y * mapToExport.TileSpacingY + j ) * ( mapToExport.Tiles.Width * mapToExport.TileSpacingX ), tile.Chars[i, j].Character );
                colorData.SetU8At( x * mapToExport.TileSpacingX + i + ( y * mapToExport.TileSpacingY + j ) * ( mapToExport.Tiles.Width * mapToExport.TileSpacingX ), tile.Chars[i, j].Color );
              }
            }
          }
        }
      }

      Types.ComboItem comboItem = (Types.ComboItem)comboCharscreenFiles.SelectedItem;
      if ( comboItem.Tag == null )
      {
        // to new file
        BaseDocument document = null;
        if ( DocInfo.Project == null )
        {
          document = Core.MainForm.CreateNewDocument( ProjectElement.ElementType.CHARACTER_SCREEN, null );
        }
        else
        {
          document = Core.MainForm.CreateNewElement( ProjectElement.ElementType.CHARACTER_SCREEN, "Charset Screen", DocInfo.Project ).Document;
        }
        if ( document.DocumentInfo.Element != null )
        {
          string newFilename = "New CharScreen.charscreen";
          string newFilenameTemplate = DocInfo.Project.FullPath( newFilename );

          int     curAttempt = 2;
          while ( ( DocInfo.Project != null )
          &&      ( DocInfo.Project.IsFilenameInUse( newFilename ) ) )
          {
            newFilename = GR.Path.RenameFilenameWithoutExtension( newFilenameTemplate, System.IO.Path.GetFileNameWithoutExtension( newFilename ) + " " + curAttempt );
            ++curAttempt;
          }
          document.SetDocumentFilename( newFilename );
          document.DocumentInfo.Element.Filename = document.DocumentInfo.DocumentFilename;
        }
        CharsetScreenEditor   charEditor = (CharsetScreenEditor)document;
        charEditor.ImportFromData( mapToExport.TileSpacingX * mapToExport.Tiles.Width,
                                   mapToExport.TileSpacingY * mapToExport.Tiles.Height,
                                   charData, colorData, Info.Map.Charset );
        document.SetModified();
        document.Save( SaveMethod.SAVE );
      }
      else
      {
        var docInfo = (DocumentInfo)comboItem.Tag;
        CharsetScreenEditor   charEditor = (CharsetScreenEditor)docInfo.BaseDoc;
        if ( charEditor == null )
        {
          charEditor = (CharsetScreenEditor)docInfo.Project.ShowDocument( docInfo.Element );
        }

        charEditor.ImportFromData( mapToExport.TileSpacingX * mapToExport.Tiles.Width,
                                   mapToExport.TileSpacingY * mapToExport.Tiles.Height,
                                   charData, colorData, Info.Map.Charset );
        charEditor.SetModified();
      }
      return true;
    }



  }
}
