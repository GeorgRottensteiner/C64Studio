using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace RetroDevStudio.Documents
{
  public partial class DebugStack : BaseDocument
  {
    public DebugStack()
    {
      InitializeComponent();

      GR.Image.DPIHandler.ResizeControlsForDPI( this );
    }



  }
}
