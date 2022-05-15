using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RetroDevStudio
{
  public partial class FormDeltaValue : Form
  {
    public int    Delta;
    public bool   InsertAsHex = true;


    public FormDeltaValue( StudioCore Core )
    {
      InitializeComponent();
      btnOK.Enabled = false;

      Delta = 0;
      Core.Theming.ApplyTheme( this );
    }
    


    private void btnOK_Click( object sender, EventArgs e )
    {
      DialogResult = DialogResult.OK;
      Close();
    }



    private void editAddress_TextChanged(object sender, EventArgs e)
    {
      int.TryParse( editAddress.Text, out int address );
      if ( ( address >= -65535 )
      &&   ( address <= 65535 ) )
      {
        btnOK.Enabled = true;
        Delta = address;
      }
      else
      {
        btnOK.Enabled = false;
      }
    }



    private void chkHex_CheckedChanged( object sender, EventArgs e )
    {
      InsertAsHex = chkHex.Checked;
    }



  }
}