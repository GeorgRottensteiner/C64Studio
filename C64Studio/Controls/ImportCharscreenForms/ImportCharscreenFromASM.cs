using RetroDevStudio.Formats;
using System.Windows.Forms;
using RetroDevStudio.Documents;



namespace RetroDevStudio.Controls
{
  public partial class ImportCharscreenFromASM : ImportCharscreenFormBase
  {
    public ImportCharscreenFromASM() :
      base( null )
    { 
    }



    public ImportCharscreenFromASM( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();
    }



    public override bool HandleImport( CharsetScreenProject CharScreen, CharsetScreenEditor Editor )
    {
      return Editor.ImportFromData( Util.FromASMData( editInput.Text ) );
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
