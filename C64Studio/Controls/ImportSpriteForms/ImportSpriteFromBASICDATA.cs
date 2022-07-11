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

      editInput.Font = new System.Drawing.Font( Core.MainForm.m_FontC64.Families[0], 16, System.Drawing.GraphicsUnit.Pixel );
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
