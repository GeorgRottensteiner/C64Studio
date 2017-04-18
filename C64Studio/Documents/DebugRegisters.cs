using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace C64Studio
{
  public partial class DebugRegisters : BaseDocument
  {
    public Project          DebuggedProject = null;



    public DebugRegisters()
    {
      InitializeComponent();
    }



    private void SetRegister( string Content, TextBox Edit, TextBox EditDec )
    {
      Content = Content.ToUpper();
      if ( Edit.Text != Content )
      {
        Edit.Text         = Content;
        EditDec.Text      = GR.Convert.ToI32( Content, 16 ).ToString();
        Edit.ForeColor    = Color.Red;
        EditDec.ForeColor = Color.Red;
      }
      else
      {
        Edit.ForeColor    = Color.Black;
        EditDec.ForeColor = Color.Black;
      }
    }



    private void SetRegister( string Content, TextBox Edit )
    {
      if ( Edit.Text != Content )
      {
        Edit.Text = Content;
        Edit.ForeColor = Color.Red;
      }
      else
      {
        Edit.ForeColor = Color.Black;
      }
    }



    public void SetRegisters( string A, string X, string Y, string Stack, string Status, string PC, string LIN, string Cycle, string ProcessorPort )
    {
      SetRegister( A, editA, editADec );
      SetRegister( X, editX, editXDec );
      SetRegister( Y, editY, editYDec );
      SetRegister( Stack, editStack, editStackDec );
      SetRegister( PC, editPC, editPCDec );

      SetRegister( LIN, editLIN );
      SetRegister( Cycle, editCycle );
      SetRegister( ProcessorPort, edit01 );

      // NV-BDIZC
      SetRegister( Status, editStatus );
    }
  }
}
