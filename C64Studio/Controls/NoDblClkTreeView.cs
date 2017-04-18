using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace C64Studio
{
  public partial class NoDblClkTreeView : TreeView
  {
    public NoDblClkTreeView()
    {
      InitializeComponent();
    }



    protected override void DefWndProc( ref System.Windows.Forms.Message m )
    {
      if ( m.Msg == 515 )
      {
        // swallow double click
        /* WM_LBUTTONDBLCLK */
        return;
      }
      base.DefWndProc( ref m );
    }
  }
}
