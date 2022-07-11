using RetroDevStudio.Formats;
using RetroDevStudio.Types;
using RetroDevStudio.Documents;



namespace RetroDevStudio.Controls
{
  public partial class ImportCharsetFromBinaryFile : ImportCharsetFormBase
  {
    public ImportCharsetFromBinaryFile() :
      base( null )
    { 
    }



    public ImportCharsetFromBinaryFile( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();
    }



    public override bool HandleImport( CharsetProject Charset, CharsetEditor Editor )
    {
      string filename;

      //Clear();
      if ( Editor.OpenFile( "Open charset", RetroDevStudio.Types.Constants.FILEFILTER_CHARSET + RetroDevStudio.Types.Constants.FILEFILTER_CHARSET_CHARPAD + RetroDevStudio.Types.Constants.FILEFILTER_ALL, out filename ) )
      {
        if ( System.IO.Path.GetExtension( filename ).ToUpper() == ".CHARSETPROJECT" )
        {
          // a project
          GR.Memory.ByteBuffer projectFile = GR.IO.File.ReadAllBytes( filename );

          RetroDevStudio.Formats.CharsetProject project = new RetroDevStudio.Formats.CharsetProject();
          if ( !project.ReadFromBuffer( projectFile ) )
          {
            return false;
          }
          Charset.Colors = new ColorSettings( project.Colors );
          Charset.ExportNumCharacters = project.ExportNumCharacters;
          Charset.ShowGrid = project.ShowGrid;

          for ( int i = 0; i < Charset.TotalNumberOfCharacters; ++i )
          {
            Charset.Characters[i].Tile.CustomColor = project.Characters[i].Tile.CustomColor;
            Charset.Characters[i].Tile.Data = new GR.Memory.ByteBuffer( project.Characters[i].Tile.Data );
          }

          Editor.CharsetWasImported();

          Editor.SetModified();
          return true;
        }
        else if ( System.IO.Path.GetExtension( filename ).ToUpper() == ".CTM" )
        {
          // a charpad project file
          GR.Memory.ByteBuffer projectFile = GR.IO.File.ReadAllBytes( filename );

          Formats.CharpadProject    cpProject = new RetroDevStudio.Formats.CharpadProject();
          if ( !cpProject.LoadFromFile( projectFile ) )
          {
            return false;
          }

          Charset.Colors.BackgroundColor = cpProject.BackgroundColor;
          Charset.Colors.MultiColor1 = cpProject.MultiColor1;
          Charset.Colors.MultiColor2 = cpProject.MultiColor2;

          Charset.ExportNumCharacters = cpProject.NumChars;
          if ( Charset.ExportNumCharacters > 256 )
          {
            Charset.ExportNumCharacters = 256;
          }
          for ( int charIndex = 0; charIndex < Charset.ExportNumCharacters; ++charIndex )
          {
            Charset.Characters[charIndex].Tile.Data = cpProject.Characters[charIndex].Data;
            Charset.Characters[charIndex].Tile.CustomColor = cpProject.Characters[charIndex].Color;
          }

          Editor.CharsetWasImported();
          Editor.SetModified();
          return true;
        }

        // treat as binary .chr file
        GR.Memory.ByteBuffer charData = GR.IO.File.ReadAllBytes( filename );

        Editor.ImportFromData( charData );
      }
      return true;
    }



  }
}
