using RetroDevStudio.Formats;
using System.Windows.Forms;
using RetroDevStudio.Documents;



namespace RetroDevStudio.Controls
{
  public partial class ImportCharsetFromHex : ImportCharsetFormBase
  {
    public ImportCharsetFromHex() :
      base( null )
    { 
    }



    public ImportCharsetFromHex( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();
    }



    public override bool HandleImport( ImportCharsetInfo importInfo, CharsetEditor Editor )
    {
      string    binaryText = editInput.Text.Replace( " ", "" ).Replace( "\r", "" ).Replace( "\n", "" );

      var charData = new GR.Memory.ByteBuffer( binaryText );
      if ( charData == null )
      {
        return false;
      }
      int numBytesOfChar = Lookup.NumBytesOfSingleCharacterBitmap( importInfo.Charset.Mode );
      int charsToImport = (int)charData.Length / numBytesOfChar;
      if ( charsToImport > importInfo.ImportIndices.Count )
      {
        charsToImport = importInfo.ImportIndices.Count;
      }

      for ( int i = 0; i < charsToImport; ++i )
      {
        charData.CopyTo( importInfo.Charset.Characters[importInfo.ImportIndices[i]].Tile.Data, i * numBytesOfChar, numBytesOfChar );
        Editor.CharacterChanged( importInfo.ImportIndices[i] );
      }

      return true;
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
