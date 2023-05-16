using RetroDevStudio.Formats;
using System.Windows.Forms;
using RetroDevStudio.Documents;



namespace RetroDevStudio.Controls
{
  public partial class ImportMapFormBase : UserControl
  {
    public StudioCore                   Core = null;



    public ImportMapFormBase()
    {
      InitializeComponent();
    }



    public ImportMapFormBase( StudioCore Core )
    {
      this.Core         = Core;

      InitializeComponent();
    }



    public virtual bool HandleImport( MapProject Map, MapEditor Editor )
    {
      return false;
    }



  }
}
