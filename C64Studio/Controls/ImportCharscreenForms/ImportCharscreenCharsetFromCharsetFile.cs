using C64Studio.Formats;
using C64Studio.Types;
using GR.Memory;
using RetroDevStudio;
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
using static C64Studio.BaseDocument;

namespace C64Studio.Controls
{
  public partial class ImportCharscreenCharsetFromCharsetFile : ImportCharscreenFormBase
  {
    public ImportCharscreenCharsetFromCharsetFile() :
      base( null )
    { 
    }



    public ImportCharscreenCharsetFromCharsetFile( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();
    }



    public override bool HandleImport( CharsetScreenProject CharScreen, CharsetScreenEditor Editor )
    {
      return Editor.OpenExternalCharset();
    }



  }
}
