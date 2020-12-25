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
    private Machine             m_Emulator = new Machine();
    private BackgroundWorker    m_BW = new BackgroundWorker();
    private volatile bool       m_EmulatorRunning = true;



    public FormMain()
    {
      InitializeComponent();

      m_BW.DoWork += OnEmulatorRun;

      m_Emulator.Display.SetTarget( pictureOutput );

      m_BW.RunWorkerAsync();
    }



    private void OnEmulatorRun( object sender, DoWorkEventArgs e )
    {
      m_Emulator.HardReset();

      try
      {
        while ( m_EmulatorRunning )
        {
          m_Emulator.RunCycle();
        }
      }
      catch ( Exception ex )
      {
        Debug.Log( "Exception: " + ex );
      }
    }



    private void exitToolStripMenuItem_Click( object sender, EventArgs e )
    {
      m_EmulatorRunning = false;

      Close();
    }



  }
}
