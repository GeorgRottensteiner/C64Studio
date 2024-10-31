using RetroDevStudio.Formats;
using System.Windows.Forms;
using RetroDevStudio.Documents;



namespace RetroDevStudio.Controls
{
  public partial class ImportGraphicScreenFormBase : UserControl
  {
    public StudioCore                   Core = null;



    public ImportGraphicScreenFormBase()
    {
      InitializeComponent();
    }



    public ImportGraphicScreenFormBase( StudioCore Core )
    {
      this.Core         = Core;

      InitializeComponent();
    }



    public virtual bool HandleImport( GraphicScreenProject Project, GraphicScreenEditor Editor )
    {
      return false;
    }



  }
}
