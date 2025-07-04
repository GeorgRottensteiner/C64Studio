﻿using RetroDevStudio.Formats;
using GR.Memory;
using System.Windows.Forms;
using RetroDevStudio.Documents;



namespace RetroDevStudio.Controls
{
  public partial class ImportBinaryFromBASICDATA : ImportBinaryFormBase
  {
    public ImportBinaryFromBASICDATA() :
      base( null )
    { 
    }



    public ImportBinaryFromBASICDATA( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();

      editInput.Font = Core.Imaging.FontFromMachine( MachineType.C64, Core.Settings.BASICSourceFontSize * 0.8f );
    }



    public override bool HandleImport( DocumentInfo docInfo, BinaryDisplay parent, out ByteBuffer importedData )
    {
      importedData = null;

      if ( checkHexData.Checked )
      {
        importedData = Util.FromBASICHex( editInput.Text );
      }
      else
      {
        importedData  = Util.FromBASIC( editInput.Text );
      }

      return ( importedData != null );
    }



    private void editInput_KeyPress( object sender, KeyPressEventArgs e )
    {
      if ( ( ModifierKeys == Keys.Control )
      &&   ( e.KeyChar == 1 ) )
      {
        editInput.SelectAll();
        e.Handled = true;
      }
    }



  }
}
