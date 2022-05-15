using RetroDevStudio.Formats;
using RetroDevStudio;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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



    public virtual bool HandleImport( CharsetProject CharScreen, CharsetEditor Editor )
    {
      return false;
    }



  }
}
