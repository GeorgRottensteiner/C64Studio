using RetroDevStudio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using WeifenLuo.WinFormsUI.Docking;

namespace RetroDevStudio.Documents
{
  public partial class PaletteEditor : BaseDocument
  {
    public PaletteEditor()
    {
      InitializeComponent();

      GR.Image.DPIHandler.ResizeControlsForDPI( this );
    }



    public PaletteEditor( StudioCore Core )
    {
      this.Core = Core;

      InitializeComponent();

      GR.Image.DPIHandler.ResizeControlsForDPI( this );
    }



    public override System.Drawing.Size GetPreferredSize( System.Drawing.Size proposedSize )
    {
      return new System.Drawing.Size( 725, 429 );
    }



  }
}
