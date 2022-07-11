using RetroDevStudio.Formats;
using System.Windows.Forms;
using RetroDevStudio.Documents;



namespace RetroDevStudio.Controls
{
  public partial class ImportSpriteFormBase : UserControl
  {
    public StudioCore                   Core = null;



    public ImportSpriteFormBase()
    {
      InitializeComponent();
    }



    public ImportSpriteFormBase( StudioCore Core )
    {
      this.Core         = Core;

      InitializeComponent();
    }



    public virtual bool HandleImport( SpriteProject Project, SpriteEditor Editor )
    {
      return false;
    }



  }
}
