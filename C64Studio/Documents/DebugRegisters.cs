using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using GR.Memory;
using C64Studio.Types;

namespace C64Studio
{
  public partial class DebugRegisters : BaseDocument
  {
    public Project          DebuggedProject = null;
    private bool            RegisterAChanged = false;
    private bool            RegisterXChanged = false;
    private bool            RegisterYChanged = false;
    private bool            RegisterSPChanged = false;
    private bool            RegisterPCChanged = false;
    private bool            RegisterRasterLineChanged = false;
    private bool            RegisterCyclesChanged = false;
    private bool            RegisterPort01Changed = false;
    private bool            RegisterStatusChanged = false;
    private Color           UnchangedColor = Color.Black;
    private Color           ChangedColor = Color.Red;



    public DebugRegisters()
    {
      InitializeComponent();
    }



    private void SetRegister( byte Content, TextBox Edit, TextBox EditDec, ref bool Changed )
    {
      string content = Content.ToString( "X2" );
      if ( Edit.Text != content )
      {
        Edit.Text         = content;
        EditDec.Text      = Content.ToString();
        Edit.ForeColor    = ChangedColor;
        EditDec.ForeColor = ChangedColor;
        Changed           = true;
      }
      else
      {
        Edit.ForeColor    = UnchangedColor;
        EditDec.ForeColor = UnchangedColor;
        Changed           = false;
      }
    }



    private void SetRegister( ushort Content, TextBox Edit, TextBox EditDec, ref bool Changed )
    {
      string content = Content.ToString( "X4" );
      if ( Edit.Text != content )
      {
        Edit.Text = content;
        EditDec.Text = Content.ToString();
        Edit.ForeColor = ChangedColor;
        EditDec.ForeColor = ChangedColor;
        Changed = true;
      }
      else
      {
        Edit.ForeColor = UnchangedColor;
        EditDec.ForeColor = UnchangedColor;
        Changed = false;
      }
    }



    private void SetRegister( string Content, TextBox Edit, TextBox EditDec, ref bool Changed )
    {
      Content = Content.ToUpper();
      if ( Edit.Text != Content )
      {
        Edit.Text         = Content;
        EditDec.Text      = GR.Convert.ToI32( Content, 16 ).ToString();
        Edit.ForeColor    = ChangedColor;
        EditDec.ForeColor = ChangedColor;
        Changed = true;
      }
      else
      {
        Edit.ForeColor    = UnchangedColor;
        EditDec.ForeColor = UnchangedColor;
        Changed = false;
      }
    }



    private void SetRegister( string Content, TextBox Edit, ref bool Changed )
    {
      if ( Edit.Text != Content )
      {
        Edit.Text = Content;
        Edit.ForeColor = ChangedColor;
        Changed = true;
      }
      else
      {
        Edit.ForeColor = UnchangedColor;
        Changed = false;
      }
    }



    private void SetRegister( int Content, TextBox Edit, ref bool Changed )
    {
      string    content = Content.ToString();
      if ( Edit.Text != content )
      {
        Edit.Text       = content;
        Edit.ForeColor  = ChangedColor;
        Changed = true;
      }
      else
      {
        Edit.ForeColor = UnchangedColor;
        Changed = false;
      }
    }



    private void SetRegister( byte Content, TextBox Edit, ref bool Changed )
    {
      string    content = Content.ToString( "X2" );
      if ( Edit.Text != content )
      {
        Edit.Text = content;
        Edit.ForeColor = ChangedColor;
        Changed = true;
      }
      else
      {
        Edit.ForeColor = UnchangedColor;
        Changed = false;
      }
    }



    public void SetRegisters( string A, string X, string Y, string Stack, string Status, string PC, string LIN, string Cycle, string ProcessorPort )
    {
      SetRegister( A, editA, editADec, ref RegisterAChanged );
      SetRegister( X, editX, editXDec, ref RegisterXChanged );
      SetRegister( Y, editY, editYDec, ref RegisterYChanged );
      SetRegister( Stack, editStack, editStackDec, ref RegisterSPChanged );
      SetRegister( PC, editPC, editPCDec, ref RegisterPCChanged );

      SetRegister( LIN, editLIN, ref RegisterRasterLineChanged );
      SetRegister( Cycle, editCycle, ref RegisterCyclesChanged );
      SetRegister( ProcessorPort, edit01, ref RegisterPort01Changed );

      // NV-BDIZC
      SetRegister( Status, editStatus, ref RegisterStatusChanged );
    }



    public void SetRegisters( RegisterInfo Registers )
    {
      SetRegister( Registers.A, editA, editADec, ref RegisterAChanged );
      SetRegister( Registers.X, editX, editXDec, ref RegisterXChanged );
      SetRegister( Registers.Y, editY, editYDec, ref RegisterYChanged );
      SetRegister( Registers.StackPointer, editStack, editStackDec, ref RegisterSPChanged );
      SetRegister( Registers.PC, editPC, editPCDec, ref RegisterPCChanged );

      SetRegister( Registers.RasterLine, editLIN, ref RegisterRasterLineChanged );
      SetRegister( Registers.Cycles, editCycle, ref RegisterCyclesChanged );
      SetRegister( Registers.ProcessorPort01, edit01, ref RegisterPort01Changed );

      // NV-BDIZC
      SetRegister( FlagByteToString( Registers.StatusFlags ), editStatus, ref RegisterStatusChanged );
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



    public override void RefreshDisplayOptions()
    {
      base.RefreshDisplayOptions();

      ChangedColor = GR.Color.Helper.FromARGB( Core.Settings.FGColor( ColorableElement.CHANGED_DEBUG_ELEMENT ) );
      UnchangedColor = GR.Color.Helper.FromARGB( Core.Settings.FGColor( ColorableElement.BACKGROUND_CONTROL ) );

      ApplyColor( editA, RegisterAChanged );
      ApplyColor( editADec, RegisterAChanged );
      ApplyColor( editX, RegisterXChanged );
      ApplyColor( editXDec, RegisterXChanged );
      ApplyColor( editY, RegisterYChanged );
      ApplyColor( editYDec, RegisterYChanged );

      ApplyColor( editStack, RegisterSPChanged );
      ApplyColor( editStackDec, RegisterSPChanged );
      ApplyColor( editPC, RegisterPCChanged );
      ApplyColor( editPCDec, RegisterPCChanged );

      ApplyColor( editLIN, RegisterRasterLineChanged );
      ApplyColor( editCycle, RegisterCyclesChanged );
      ApplyColor( edit01, RegisterPort01Changed );

      ApplyColor( editStatus, RegisterStatusChanged );
    }



    private void ApplyColor( TextBox Edit, bool Changed )
    {
      if ( Changed )
      {
        Edit.ForeColor = GR.Color.Helper.FromARGB( Core.Settings.FGColor( ColorableElement.CHANGED_DEBUG_ELEMENT ) );
      }
      else
      {
        Edit.ForeColor = GR.Color.Helper.FromARGB( Core.Settings.FGColor( ColorableElement.BACKGROUND_CONTROL ) );
      }
    }

  }
}
