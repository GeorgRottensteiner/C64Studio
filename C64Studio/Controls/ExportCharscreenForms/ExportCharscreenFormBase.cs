using RetroDevStudio;
using RetroDevStudio.Formats;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace C64Studio.Controls
{
  public partial class ExportCharscreenFormBase : UserControl
  {
    public StudioCore                   Core = null;
    public ColorSettings                Colors = null;



    public ExportCharscreenFormBase()
    {
      InitializeComponent();
    }



    public ExportCharscreenFormBase( StudioCore Core )
    {
      this.Core         = Core;

      InitializeComponent();
    }



    public virtual bool HandleExport( ExportCharsetScreenInfo Info, TextBox EditOutput, DocumentInfo DocInfo )
    {
      return false;
    }



  }
}
