using RetroDevStudio.Formats;
using RetroDevStudio.Documents;



namespace RetroDevStudio.Controls
{
  public partial class ImportMapCharsetFromCharsetFile : ImportMapFormBase
  {
    public ImportMapCharsetFromCharsetFile() :
      base( null )
    { 
    }



    public ImportMapCharsetFromCharsetFile( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();
    }



    public override bool HandleImport( MapProject Map, MapEditor Editor )
    {
      Editor.ImportCharset();
      return true;
    }



  }
}
