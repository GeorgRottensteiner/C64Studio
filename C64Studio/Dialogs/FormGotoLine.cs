using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RetroDevStudio.Dialogs
{
  public partial class FormGotoLine : Form
  {
    public int LineNumber { get; set; }



    public FormGotoLine( StudioCore Core )
    {
      InitializeComponent();
      btnOK.Enabled = false;

      Core.Theming.ApplyTheme( this );
    }
    


    private void btnOK_Click( DecentForms.ControlBase Sender )
    {
      DialogResult = DialogResult.OK;
      Close();
    }



    private void editAddress_TextChanged(object sender, EventArgs e)
    {
      int lineNo = -1;

      if ( ( int.TryParse( editLineNo.Text, out lineNo ) )
      &&   ( lineNo >= 1 ) )
      {
        LineNumber = lineNo;
        btnOK.Enabled = true;
      }
      else
      {
        btnOK.Enabled = false;
      }
    }



    private void btnCancel_Click( DecentForms.ControlBase Sender )
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }



  }
}