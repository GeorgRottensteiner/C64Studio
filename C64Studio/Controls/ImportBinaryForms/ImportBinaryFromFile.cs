using RetroDevStudio.Formats;
using RetroDevStudio.Types;
using RetroDevStudio.Documents;
using System.Windows.Forms;
using GR.Memory;



namespace RetroDevStudio.Controls
{
  public partial class ImportBinaryFromFile : ImportBinaryFormBase
  {
    public ImportBinaryFromFile() :
      base( null )
    { 
    }



    public ImportBinaryFromFile( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();
    }



    public override bool HandleImport( DocumentInfo docInfo, BinaryDisplay parent, out ByteBuffer importedData )
    {
      importedData = null;

      int bytesToSkip = GR.Convert.ToI32( editImportSkipBytes.Text );
      if ( bytesToSkip < 0 )
      {
        bytesToSkip = 0;
      }

      if ( !parent.OpenFile( "Select file to import", RetroDevStudio.Types.Constants.FILEFILTER_ALL, out string filename ) )
      {
        return false;
      }
      importedData = GR.IO.File.ReadAllBytes( filename );
      if ( bytesToSkip >= importedData.Length )
      {
        importedData.Clear();
        return false;
      }
      importedData.TruncateFront( bytesToSkip );
      return true;
    }



  }
}
