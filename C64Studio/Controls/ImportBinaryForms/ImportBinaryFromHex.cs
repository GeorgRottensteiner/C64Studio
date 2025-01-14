using RetroDevStudio.Formats;
using System.Windows.Forms;
using RetroDevStudio.Documents;
using GR.Memory;



namespace RetroDevStudio.Controls
{
  public partial class ImportBinaryFromHex : ImportBinaryFormBase
  {
    public ImportBinaryFromHex() :
      base( null )
    { 
    }



    public ImportBinaryFromHex( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();
    }



    public override bool HandleImport( DocumentInfo docInfo, BinaryDisplay parent, out ByteBuffer importedData )
    {
      string    binaryText = editInput.Text.Replace( " ", "" ).Replace( "\r", "" ).Replace( "\n", "" );
      importedData = null;

      importedData = new GR.Memory.ByteBuffer( binaryText );
      return importedData != null;
    }



    private void editInput_KeyPress( object sender, KeyPressEventArgs e )
    {
      if ( ( ModifierKeys == Keys.Control )
      &&   ( e.KeyChar == 1 ) )
      {
        editInput.SelectAll();
        e.Handled = true;
      }
    }



  }
}
