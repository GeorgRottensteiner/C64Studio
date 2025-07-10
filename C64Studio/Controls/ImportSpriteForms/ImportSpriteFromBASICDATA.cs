using RetroDevStudio.Formats;
using GR.Memory;
using System.Windows.Forms;
using RetroDevStudio.Documents;



namespace RetroDevStudio.Controls
{
  public partial class ImportSpriteFromBASICDATA : ImportSpriteFormBase
  {
    public ImportSpriteFromBASICDATA() :
      base( null )
    { 
    }



    public ImportSpriteFromBASICDATA( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();

      editInput.Font = Core.Imaging.FontFromMachine( MachineType.C64, Core.Settings.BASICSourceFontSize * 0.8f );
    }



    public override bool HandleImport( SpriteProject Project, SpriteEditor Editor )
    {
      ByteBuffer    data;

      if ( checkHexData.Checked )
      {
        data = Util.FromBASICHex( editInput.Text );
      }
      else
      {
        data = Util.FromBASIC( editInput.Text );
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
