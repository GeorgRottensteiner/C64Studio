using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RetroDevStudio.Dialogs
{
  public partial class DlgInterleaveData : Form
  {
    public GR.Memory.ByteBuffer         OrigData;
    public GR.Memory.ByteBuffer         InterleavedData;



    public DlgInterleaveData( StudioCore Core, GR.Memory.ByteBuffer OrigData )
    {
      this.OrigData = new GR.Memory.ByteBuffer( OrigData );
      InitializeComponent();

      SetHexData( hexOrig, OrigData );
      InterleavedData = new GR.Memory.ByteBuffer( OrigData );
      editInterleave.Text = "8";

      Core.Theming.ApplyTheme( this );
    }



    private void btnOK_Click( DecentForms.ControlBase Sender )
    {
      DialogResult = DialogResult.OK;
      Close();
    }



    private void SetHexData( Be.Windows.Forms.HexBox HexBox, GR.Memory.ByteBuffer Data )
    {
      HexBox.ByteProvider = new Be.Windows.Forms.DynamicByteProvider( Data.Data() );
    }



    private void CalcInterleavedData()
    {
      int   interleaveValue = GR.Convert.ToI32( editInterleave.Text );

      if ( OrigData.Length <= 1 )
      {
        return;
      }

      int     origPos = 0;
      for ( uint i = 0; i < InterleavedData.Length; ++i )
      {
        InterleavedData.SetU8At( (int)i, OrigData.ByteAt( origPos ) );
        origPos = origPos + interleaveValue;

        while ( origPos >= (int)OrigData.Length )
        {
          origPos -= (int)OrigData.Length;
          ++origPos;
        }
      }

      SetHexData( hexPreview, InterleavedData );
    }



    private void editInterleave_TextChanged( object sender, EventArgs e )
    {
      int   interleaveValue = GR.Convert.ToI32( editInterleave.Text );
      if ( interleaveValue > 0 )
      {
        CalcInterleavedData();
      }
    }



    private void btnCancel_Click( DecentForms.ControlBase Sender )
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }



  }
}
