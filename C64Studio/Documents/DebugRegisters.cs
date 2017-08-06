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



    private void SetRegister( byte Content, TextBox Edit, TextBox EditDec )
    {
      string content = Content.ToString( "X2" );
      if ( Edit.Text != content )
      {
        Edit.Text         = content;
        EditDec.Text      = Content.ToString();
        Edit.ForeColor    = Color.Red;
        EditDec.ForeColor = Color.Red;
      }
      else
      {
        Edit.ForeColor    = Color.Black;
        EditDec.ForeColor = Color.Black;
      }
    }



    private void SetRegister( ushort Content, TextBox Edit, TextBox EditDec )
    {
      string content = Content.ToString( "X4" );
      if ( Edit.Text != content )
      {
        Edit.Text = content;
        EditDec.Text = Content.ToString();
        Edit.ForeColor = Color.Red;
        EditDec.ForeColor = Color.Red;
      }
      else
      {
        Edit.ForeColor = Color.Black;
        EditDec.ForeColor = Color.Black;
      }
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



    private void SetRegister( int Content, TextBox Edit )
    {
      string    content = Content.ToString();
      if ( Edit.Text != content )
      {
        Edit.Text       = content;
        Edit.ForeColor  = Color.Red;
      }
      else
      {
        Edit.ForeColor = Color.Black;
      }
    }



    private void SetRegister( byte Content, TextBox Edit )
    {
      string    content = Content.ToString( "X2" );
      if ( Edit.Text != content )
      {
        Edit.Text = content;
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



    public void SetRegisters( RegisterInfo Registers )
    {
      SetRegister( Registers.A, editA, editADec );
      SetRegister( Registers.X, editX, editXDec );
      SetRegister( Registers.Y, editY, editYDec );
      SetRegister( Registers.StackPointer, editStack, editStackDec );
      SetRegister( Registers.PC, editPC, editPCDec );

      SetRegister( Registers.RasterLine, editLIN );
      SetRegister( Registers.Cycles, editCycle );
      SetRegister( Registers.ProcessorPort01, edit01 );

      // NV-BDIZC
      SetRegister( FlagByteToString( Registers.StatusFlags ), editStatus );
    }



    private string FlagByteToString( byte Flags )
    {
      StringBuilder   sb = new StringBuilder();

      if ( ( Flags & 0x80 ) != 0 )
      {
        sb.Append( 'N' );
      }
      else
      {
        sb.Append( '.' );
      }
      if ( ( Flags & 0x40 ) != 0 )
      {
        sb.Append( 'V' );
      }
      else
      {
        sb.Append( '.' );
      }
      sb.Append( '-' );
      if ( ( Flags & 0x10 ) != 0 )
      {
        sb.Append( 'B' );
      }
      else
      {
        sb.Append( '.' );
      }
      if ( ( Flags & 0x08 ) != 0 )
      {
        sb.Append( 'D' );
      }
      else
      {
        sb.Append( '.' );
      }
      if ( ( Flags & 0x04 ) != 0 )
      {
        sb.Append( 'I' );
      }
      else
      {
        sb.Append( '.' );
      }
      if ( ( Flags & 0x02 ) != 0 )
      {
        sb.Append( 'Z' );
      }
      else
      {
        sb.Append( '.' );
      }
      if ( ( Flags & 0x01 ) != 0 )
      {
        sb.Append( 'C' );
      }
      else
      {
        sb.Append( '.' );
      }
      return sb.ToString();
    }



  }
}
