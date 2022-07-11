using RetroDevStudio.Formats;
using System.Windows.Forms;
using RetroDevStudio.Documents;



namespace RetroDevStudio.Controls
{
  public partial class ImportSpriteFromHEX : ImportSpriteFormBase
  {
    public ImportSpriteFromHEX() :
      base( null )
    { 
    }



    public ImportSpriteFromHEX( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();
    }



    public override bool HandleImport( SpriteProject CharSet, SpriteEditor Editor )
    {
      string    binaryText = editInput.Text.Replace( " ", "" ).Replace( "\r", "" ).Replace( "\n", "" );

      GR.Memory.ByteBuffer    data = new GR.Memory.ByteBuffer( binaryText );
      if ( data == null )
      {
        return false;
      }

      Editor.ImportFromData( data );
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
