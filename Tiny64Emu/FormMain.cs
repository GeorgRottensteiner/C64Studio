using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tiny64Emu
{
  public partial class FormMain : Form
  {
    private         Tiny64.Machine        m_Emulator = new Tiny64.Machine();



    public FormMain()
    {
      InitializeComponent();
    }
  }
}
