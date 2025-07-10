using RetroDevStudio.Formats;
using System.Windows.Forms;
using RetroDevStudio.Documents;



namespace RetroDevStudio.Controls
{
  public partial class ImportCharsetFormBase : UserControl
  {
    public StudioCore                   Core = null;



    public ImportCharsetFormBase()
    {
      InitializeComponent();
    }



    public ImportCharsetFormBase( StudioCore Core )
    {
      this.Core         = Core;

      InitializeComponent();
    }



    public virtual bool HandleImport( ImportCharsetInfo importInfo, CharsetEditor Editor )
    {
      return false;
    }



  }
}
