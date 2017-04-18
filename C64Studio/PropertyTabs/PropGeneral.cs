using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace C64Studio
{
  public partial class PropGeneral : PropertyTabs.PropertyTabBase
  {
    ProjectElement        Element;
    StudioCore            Core;



    public PropGeneral( ProjectElement Element, StudioCore Core )
    {
      this.Element = Element;
      this.Core = Core;
      TopLevel = false;
      Text = "General";
      InitializeComponent();

      labelFilename.Text = Element.Filename;
      labelFilePath.Text = Element.DocumentInfo.Project.FullPath( Element.Filename );
    }

  }
}
