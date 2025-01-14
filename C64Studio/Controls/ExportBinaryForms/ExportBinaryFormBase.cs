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
  public partial class ExportBinaryFormBase : UserControl
  {
    public StudioCore                   Core = null;



    public ExportBinaryFormBase()
    {
      AutoScaleMode = AutoScaleMode.None;
      InitializeComponent();
    }



    public ExportBinaryFormBase( StudioCore Core )
    {
      this.Core         = Core;
      AutoScaleMode = AutoScaleMode.None;

      InitializeComponent();
    }



    public virtual bool HandleExport( ExportBinaryInfo Info, DocumentInfo DocInfo )
    {
      return false;
    }



  }
}
