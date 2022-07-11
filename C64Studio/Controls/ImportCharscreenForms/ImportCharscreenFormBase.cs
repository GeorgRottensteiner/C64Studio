using RetroDevStudio.Formats;
using System.Windows.Forms;
using RetroDevStudio.Documents;



namespace RetroDevStudio.Controls
{
  public partial class ImportCharscreenFormBase : UserControl
  {
    public StudioCore                   Core = null;



    public ImportCharscreenFormBase()
    {
      InitializeComponent();
    }



    public ImportCharscreenFormBase( StudioCore Core )
    {
      this.Core         = Core;

      InitializeComponent();
    }



    public virtual bool HandleImport( CharsetScreenProject CharScreen, CharsetScreenEditor Editor )
    {
      return false;
    }



  }
}
