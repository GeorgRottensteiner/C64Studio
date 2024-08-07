﻿using RetroDevStudio;
using RetroDevStudio.Formats;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RetroDevStudio.Controls
{
  public partial class ExportCharsetFormBase : UserControl
  {
    public StudioCore                   Core = null;



    public ExportCharsetFormBase()
    {
      InitializeComponent();
    }



    public ExportCharsetFormBase( StudioCore Core )
    {
      this.Core         = Core;

      InitializeComponent();
    }



    public virtual bool HandleExport( ExportCharsetInfo Info, TextBox EditOutput, DocumentInfo DocInfo )
    {
      return false;
    }



  }
}
