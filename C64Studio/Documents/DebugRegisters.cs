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
    private bool            SuppressTextChange = false;



    public DebugRegisters()
    {
      InitializeComponent();
    }



    private void SetRegister( byte Content, TextBox Edit, TextBox EditDec, TextBox EditBinary, ref bool Changed )
    {
      string content = Content.ToString( "X2" );
      if ( Edit.Text != content )
      {
        SuppressTextChange = true;
        Edit.Text         = content;
        EditDec.Text      = Content.ToString();
        Edit.ForeColor    = ChangedColor;
        EditDec.ForeColor = ChangedColor;

        if ( EditBinary != null )
        {
          EditBinary.Text       = Convert.ToString( Content, 2 ).PadLeft( 8, '0' );
          EditBinary.ForeColor  = ChangedColor;
        }
        Changed           = true;
        SuppressTextChange = false;
      }
      else
      {
        SuppressTextChange = true;
        Edit.ForeColor    = UnchangedColor;
        EditDec.Text      = Content.ToString();
        EditDec.ForeColor = UnchangedColor;

        if ( EditBinary != null )
        {
          EditBinary.Text       = Convert.ToString( Content, 2 ).PadLeft( 8, '0' );
          EditBinary.ForeColor  = UnchangedColor;
        }
        Changed           = false;
        SuppressTextChange = false;
      }
    }



    private void SetRegister( ushort Content, TextBox Edit, TextBox EditDec, ref bool Changed )
    {
      SuppressTextChange = true;
      string content = Content.ToString( "X4" );
      if ( Edit.Text != content )
      {
        Edit.Text     = content;
        EditDec.Text  = Content.ToString();
        Edit.ForeColor = ChangedColor;
        EditDec.ForeColor = ChangedColor;
        Changed = true;
      }
      else
      {
        EditDec.Text = Content.ToString();
        Edit.ForeColor = UnchangedColor;
        EditDec.ForeColor = UnchangedColor;
        Changed = false;
      }
      SuppressTextChange = false;
    }



    private void SetRegister( string Content, TextBox Edit, ref bool Changed )
    {
      SuppressTextChange = true;
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
      SuppressTextChange = false;
    }



    private void SetRegister( int Content, TextBox Edit, ref bool Changed )
    {
      SuppressTextChange = true;
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
      SuppressTextChange = false;
    }



    private void SetRegister( byte Content, TextBox Edit, ref bool Changed )
    {
      SuppressTextChange = true;
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
      SuppressTextChange = false;
    }



    public void SetRegisters( RegisterInfo Registers )
    {
      SetRegister( Registers.A, editA, editADec, editABin, ref RegisterAChanged );
      SetRegister( Registers.X, editX, editXDec, editXBin, ref RegisterXChanged );
      SetRegister( Registers.Y, editY, editYDec, editYBin, ref RegisterYChanged );
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



    private byte FlagStringToByte( string Flags )
    {
      byte  result = 0;

      string    workFlags = Flags;
      if ( workFlags.Length < 8 )
      {
        workFlags.PadRight( 8, '.' );
      }

      for ( int i = 0; i < 8; ++i )
      {
        if ( ( workFlags[i] != '.' )
        &&   ( workFlags[i] != '0' ) )
        {
          result |= (byte)( 1 << ( 7 - i ) );
        }
      }
      return result;
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



    public void EnableRegisterOverrides( bool Enable )
    {
      editA.ReadOnly          = !Enable;
      editADec.ReadOnly       = !Enable;
      editABin.ReadOnly       = !Enable;
      editX.ReadOnly          = !Enable;
      editXDec.ReadOnly       = !Enable;
      editXBin.ReadOnly       = !Enable;
      editY.ReadOnly          = !Enable;
      editYDec.ReadOnly       = !Enable;
      editYBin.ReadOnly       = !Enable;
      editPC.ReadOnly         = !Enable;
      editPCDec.ReadOnly      = !Enable;
      editStack.ReadOnly      = !Enable;
      editStackDec.ReadOnly   = !Enable;
      editStatus.ReadOnly     = !Enable;
    }



    private void editA_TextChanged( object sender, EventArgs e )
    {
      if ( SuppressTextChange )
      {
        return;
      }
      Core.Debugging.DebugSetRegister( "A", GR.Convert.ToI32( editA.Text, 16 ) );
    }



    private void editADec_TextChanged( object sender, EventArgs e )
    {
      if ( SuppressTextChange )
      {
        return;
      }
      Core.Debugging.DebugSetRegister( "A", GR.Convert.ToI32( editADec.Text ) & 0xff );
    }



    private void editABin_TextChanged( object sender, EventArgs e )
    {
      if ( SuppressTextChange )
      {
        return;
      }
      Core.Debugging.DebugSetRegister( "A", GR.Convert.ToI32( editABin.Text, 2 ) );
    }



    private void editPC_TextChanged( object sender, EventArgs e )
    {
      if ( SuppressTextChange )
      {
        return;
      }
      Core.Debugging.DebugSetRegister( "P", GR.Convert.ToI32( editPC.Text, 16 ) );
    }



    private void editPCDec_TextChanged( object sender, EventArgs e )
    {
      if ( SuppressTextChange )
      {
        return;
      }
      Core.Debugging.DebugSetRegister( "P", GR.Convert.ToI32( editPC.Text ) );
    }



    private void editX_TextChanged( object sender, EventArgs e )
    {
      if ( SuppressTextChange )
      {
        return;
      }
      Core.Debugging.DebugSetRegister( "X", GR.Convert.ToI32( editX.Text, 16 ) );
    }



    private void editXDec_TextChanged( object sender, EventArgs e )
    {
      if ( SuppressTextChange )
      {
        return;
      }
      Core.Debugging.DebugSetRegister( "X", GR.Convert.ToI32( editXDec.Text ) );
    }



    private void editXBin_TextChanged( object sender, EventArgs e )
    {
      if ( SuppressTextChange )
      {
        return;
      }
      Core.Debugging.DebugSetRegister( "X", GR.Convert.ToI32( editXBin.Text, 2 ) );
    }



    private void editY_TextChanged( object sender, EventArgs e )
    {
      if ( SuppressTextChange )
      {
        return;
      }
      Core.Debugging.DebugSetRegister( "Y", GR.Convert.ToI32( editY.Text, 16 ) );
    }



    private void editYDec_TextChanged( object sender, EventArgs e )
    {
      if ( SuppressTextChange )
      {
        return;
      }
      Core.Debugging.DebugSetRegister( "Y", GR.Convert.ToI32( editYDec.Text ) );
    }



    private void editYBin_TextChanged( object sender, EventArgs e )
    {
      if ( SuppressTextChange )
      {
        return;
      }
      Core.Debugging.DebugSetRegister( "Y", GR.Convert.ToI32( editYBin.Text, 2 ) );
    }



    private void editStack_TextChanged( object sender, EventArgs e )
    {
      if ( SuppressTextChange )
      {
        return;
      }
      Core.Debugging.DebugSetRegister( "S", GR.Convert.ToI32( editStack.Text, 16 ) );
    }



    private void editStackDec_TextChanged( object sender, EventArgs e )
    {
      if ( SuppressTextChange )
      {
        return;
      }
      Core.Debugging.DebugSetRegister( "S", GR.Convert.ToI32( editStackDec.Text ) );
    }



    private void editStatus_TextChanged( object sender, EventArgs e )
    {
      if ( SuppressTextChange )
      {
        return;
      }
      Core.Debugging.DebugSetRegister( "F", FlagStringToByte( editStatus.Text ) );
    }



  }
}
