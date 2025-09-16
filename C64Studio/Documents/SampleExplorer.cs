using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace RetroDevStudio.Documents
{
  public partial class SampleExplorer : BaseDocument
  {
    public SampleExplorer( StudioCore Core )
    {
      this.Core = Core;
      HideOnClose = true;

      InitializeComponent();

      gridSamples.Items.Add( new DecentForms.GridList.GridListItem() { Text = "Sample 1" } );
      gridSamples.Items.Add( new DecentForms.GridList.GridListItem() { Text = "Sample 2" } );
      gridSamples.Items.Add( new DecentForms.GridList.GridListItem() { Text = "Sample 3" } );
    }



  }
}
