using RetroDevStudio.Formats;
using RetroDevStudio.Documents;



namespace RetroDevStudio.Controls
{
  public partial class ImportCharsetFromImageFile : ImportCharsetFormBase
  {
    public ImportCharsetFromImageFile() :
      base( null )
    { 
    }



    public ImportCharsetFromImageFile( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();
    }



    public override bool HandleImport( CharsetProject Charset, CharsetEditor Editor )
    {
      string filename;

      if ( !Editor.OpenFile( "Import Charset from Image", RetroDevStudio.Types.Constants.FILEFILTER_IMAGE_FILES, out filename ) )
      {
        return false;
      }

      GR.Image.FastImage imgClip = Core.Imaging.LoadImageFromFile( filename );

      Editor.characterEditor.PasteImage( filename, imgClip, false );
      return true;
    }



  }
}
