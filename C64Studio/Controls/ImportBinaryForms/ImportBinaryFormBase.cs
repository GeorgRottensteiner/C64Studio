using GR.Memory;
using RetroDevStudio;
using RetroDevStudio.Documents;
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

namespace RetroDevStudio.Controls
{
  public partial class ImportBinaryFormBase : UserControl
  {
    public StudioCore                   Core = null;



    public ImportBinaryFormBase()
    {
      AutoScaleMode = AutoScaleMode.None;
      InitializeComponent();
    }



    public ImportBinaryFormBase( StudioCore Core )
    {
      this.Core         = Core;
      AutoScaleMode = AutoScaleMode.None;

      InitializeComponent();
    }



    public virtual bool HandleImport( DocumentInfo docInfo, BinaryDisplay parent, out ByteBuffer importedData )
    {
      importedData = null;
      return false;
    }



  }
}
