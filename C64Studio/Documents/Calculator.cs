using RetroDevStudio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using WeifenLuo.WinFormsUI.Docking;

namespace C64Studio
{
  public partial class Calculator : BaseDocument
  {
    bool          m_Modifying = false;



    public Calculator()
    {
      InitializeComponent();

      GR.Image.DPIHandler.ResizeControlsForDPI( this );
    }



    private void editDec_TextChanged( object sender, EventArgs e )
    {
      if ( m_Modifying )
      {
        return;
      }
      m_Modifying = true;
      string dec = editDec.Text;
      long   val = GR.Convert.ToI64( dec );
      editHex.Text = val.ToString( "x" );
      editBin.Text = System.Convert.ToString( val, 2 );
      m_Modifying = false;
    }



    private void editHex_TextChanged( object sender, EventArgs e )
    {
      if ( m_Modifying )
      {
        return;
      }
      m_Modifying = true;
      long val = GR.Convert.ToI64( editHex.Text, 16 );
      editDec.Text = val.ToString();
      editBin.Text = System.Convert.ToString( val, 2 );
      m_Modifying = false;
    }



    private void editBin_TextChanged( object sender, EventArgs e )
    {
      if ( m_Modifying )
      {
        return;
      }
      m_Modifying = true;
      string bin = editBin.Text;
      long val = GR.Convert.ToI64( bin, 2 );
      editHex.Text = val.ToString( "x" );
      editDec.Text = val.ToString();
      m_Modifying = false;
    }



    public override System.Drawing.Size GetPreferredSize( System.Drawing.Size proposedSize )
    {
      return new System.Drawing.Size( 274, 159 );
    }



    private void editCalc_TextChanged( object sender, EventArgs e )
    {
      Parser.ASMFileParser    parser = new C64Studio.Parser.ASMFileParser();
      var tokens = parser.ParseTokenInfo( editCalc.Text, 0, editCalc.TextLength );
      if ( tokens != null )
      {
        if ( parser.EvaluateTokens( -1, tokens, out SymbolInfo result ) )
        {
          editResult.Text = "$" + result.ToInteger().ToString( "X" ) + ", " + result.ToInteger().ToString();
          editCalc.BackColor = System.Drawing.SystemColors.Window;
        }
        else
        {
          editCalc.BackColor = System.Drawing.Color.LightPink;
        }
      }
      else
      {
        editCalc.BackColor = System.Drawing.Color.LightPink;
      }
    }
  }
}
