using RetroDevStudio.Formats;
using System.Windows.Forms;
using RetroDevStudio.Documents;



namespace RetroDevStudio.Controls
{
  public partial class ImportSpriteFromASM : ImportSpriteFormBase
  {
    public ImportSpriteFromASM() :
      base( null )
    { 
    }



    public ImportSpriteFromASM( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();
    }



    public override bool HandleImport( SpriteProject CharSet, SpriteEditor Editor )
    {
      var data = Util.FromASMData( editInput.Text );
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
