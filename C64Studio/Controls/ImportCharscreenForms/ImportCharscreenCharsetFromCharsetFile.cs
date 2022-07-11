using RetroDevStudio.Formats;
using RetroDevStudio.Documents;



namespace RetroDevStudio.Controls
{
  public partial class ImportCharscreenCharsetFromCharsetFile : ImportCharscreenFormBase
  {
    public ImportCharscreenCharsetFromCharsetFile() :
      base( null )
    { 
    }



    public ImportCharscreenCharsetFromCharsetFile( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();
    }



    public override bool HandleImport( CharsetScreenProject CharScreen, CharsetScreenEditor Editor )
    {
      return Editor.OpenExternalCharset();
    }



  }
}
