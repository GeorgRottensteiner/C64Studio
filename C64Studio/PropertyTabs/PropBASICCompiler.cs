using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RetroDevStudio
{
  public partial class PropBASICCompiler : PropertyTabs.PropertyTabBase
  {
    ProjectElement        Element;
    StudioCore            Core;



    public PropBASICCompiler( ProjectElement Element, StudioCore Core )
    {
      this.Element = Element;
      this.Core = Core;
      TopLevel = false;
      Text = "BASIC Settings";
      InitializeComponent();

      checkBASICWriteTempFileWithoutMetaData.Checked = Element.BASICWriteTempFileWithoutMetaData;
    }



    public override void OnClose()
    {
      Element.BASICWriteTempFileWithoutMetaData = checkBASICWriteTempFileWithoutMetaData.Checked;
    }



    private void checkBASICWriteTempFileWithoutMetaData_CheckedChanged( object sender, EventArgs e )
    {

    }
  }
}
