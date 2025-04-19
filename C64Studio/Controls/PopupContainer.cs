using GR.Image;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;



namespace RetroDevStudio.Controls
{
  public class PopupContainer : Control, IDisposable
  {
    private Control             _ChildControl = null;



    public PopupContainer()
    {
      SetTopLevel( true );
    }



    ~PopupContainer()
    {
      Dispose( false );
    }



    public PopupContainer( Control ChildControl )
    {
      Controls.Add( ChildControl );
      _ChildControl = ChildControl;
      ChildControl.SizeChanged += ChildControl_SizeChanged;
      Size = _ChildControl.Size;
    }



    private void ChildControl_SizeChanged( object sender, EventArgs e )
    {
      Size = _ChildControl.Size;
    }



    protected override CreateParams CreateParams
    {
      get
      {
        CreateParams p = base.CreateParams;

        int WS_EX_TOPMOST = 0x00000008;

        // WS_CAPTION | WS_SYSMENU
        p.Style &= ~( 0x00C00000 | 0x00080000 );

        // WS_BORDER | WS_POPUP
        p.Style |= 0x00800000 | unchecked((int)0x80000000);

        p.ExStyle |= WS_EX_TOPMOST;

        return p;
      }
    }



    protected override void Dispose( bool disposing )
    {
      base.Dispose( disposing );
    }


  }

}
