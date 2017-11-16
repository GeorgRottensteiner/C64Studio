using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace C64Studio
{
  public partial class FormGoto : Form
  {
    public FormGoto()
    {
      InitializeComponent();
      btnOK.Enabled = false;
    }
    
    private void btnOK_Click( object sender, EventArgs e )
    {
      DialogResult = DialogResult.OK;
      Close();
    }

    private void editAddress_TextChanged(object sender, EventArgs e)
    {
      int address = -1;
      if (chkHex.Checked)
      {
        int.TryParse(editAddress.Text, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out address);
      }
      else
      {
        int.TryParse(editAddress.Text, out address);
      }
      if (address >= 0 && address <= 65535)
      {
        btnOK.Enabled = true;
      }
      else
      {
        btnOK.Enabled = false;
      }
    }
    

  }
}
