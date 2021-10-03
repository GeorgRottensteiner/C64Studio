using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace C64Studio
{
  public partial class FormAppMode : Form
  {
    StudioCore    Core = null;



    public FormAppMode( StudioCore Core )
    {
      this.Core = Core;
      InitializeComponent();

      Core.Theming.ApplyTheme( this );
    }



    private void btnNormalAppMode_Click( object sender, EventArgs e )
    {
      Core.Settings.StudioAppMode = AppMode.GOOD_APP;

      DialogResult = DialogResult.OK;
      Close();
    }



    private void btnPortableMode_Click( object sender, EventArgs e )
    {
      Core.Settings.StudioAppMode = AppMode.PORTABLE_APP;

      DialogResult = DialogResult.OK;
      Close();
    }



    private void btnAskLater_Click( object sender, EventArgs e )
    {
      Core.Settings.StudioAppMode = AppMode.UNDECIDED;

      DialogResult = DialogResult.OK;
      Close();
    }

  }
}
