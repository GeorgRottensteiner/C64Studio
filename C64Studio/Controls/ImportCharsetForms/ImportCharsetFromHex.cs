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



    public override bool HandleImport( CharsetProject CharSet, CharsetEditor Editor )
    {
      string    binaryText = editInput.Text.Replace( " ", "" ).Replace( "\r", "" ).Replace( "\n", "" );

      GR.Memory.ByteBuffer    charData = new GR.Memory.ByteBuffer( binaryText );
      if ( charData == null )
      {
        return false;
      }
      int charsToImport = (int)charData.Length / 8;
      if ( charsToImport > CharSet.TotalNumberOfCharacters )
      {
        charsToImport = CharSet.TotalNumberOfCharacters;
      }

      for ( int i = 0; i < charsToImport; ++i )
      {
        charData.CopyTo( CharSet.Characters[i].Tile.Data, i * 8, 8 );
        Editor.CharacterChanged( i );
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
