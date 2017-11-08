using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tiny64;



namespace Tiny64Emu
{
  public partial class FormMain : Form
  {
    private         Machine        m_Emulator = new Machine();



    public FormMain()
    {
      InitializeComponent();
    }
  }
}
