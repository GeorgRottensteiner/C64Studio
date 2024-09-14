using RetroDevStudio.Formats;
using RetroDevStudio.Documents;



namespace RetroDevStudio.Controls
{
  public partial class ImportMapFromBinaryFile : ImportMapFormBase
  {
    public ImportMapFromBinaryFile() :
      base( null )
    { 
    }



    public ImportMapFromBinaryFile( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();
    }



    public override bool HandleImport( MapProject Map, MapEditor Editor )
    {
      if ( !Editor.OpenFile( "Open map project", RetroDevStudio.Types.Constants.FILEFILTER_MAP_SUPPORTED_FILES + RetroDevStudio.Types.Constants.FILEFILTER_CHARSET_CHARPAD + RetroDevStudio.Types.Constants.FILEFILTER_ALL, out string filename ) )
      {
        return false;
      }
      if ( GR.Path.GetExtension( filename ).ToUpper() == ".CHARSETPROJECT" )
      {
        Editor.OpenExternalCharset( filename );
        if ( ( Editor.DocumentInfo.Project == null )
        ||   ( string.IsNullOrEmpty( Editor.DocumentInfo.Project.Settings.BasePath ) ) )
        {
          Map.ExternalCharset = filename;
        }
        else
        {
          Map.ExternalCharset = GR.Path.RelativePathTo( filename, false, System.IO.Path.GetFullPath( Editor.DocumentInfo.Project.Settings.BasePath ), true );
        }
        Editor.SetModified();
        return true;
      }
      else if ( GR.Path.GetExtension( filename ).ToUpper() == ".CTM" )
      {
        // a charpad project file
        return Editor.OpenCharpadFile( filename );
      }
      return Editor.OpenProject( filename );
    }



  }
}
