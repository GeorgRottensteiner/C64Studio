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



    public override bool HandleImport( ImportCharsetInfo importInfo, CharsetEditor Editor )
    {
      string filename;

      if ( !Editor.OpenFile( "Import Charset from Image", RetroDevStudio.Types.Constants.FILEFILTER_IMAGE_FILES, out filename ) )
      {
        return false;
      }

      var imgClip = Core.Imaging.LoadImageFromFile( filename );

      Editor.characterEditor.PasteImage( filename, imgClip, false );
      return true;
    }



  }
}
