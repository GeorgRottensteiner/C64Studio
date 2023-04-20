using System;
using System.Collections.Generic;
using System.Text;



namespace RetroDevStudio
{
  public class UtilForms
  {
    public static void UpdateColorComboItemCount( System.Windows.Forms.ComboBox Combo, int ExpectedNumColors )
    {
      if ( Combo.Items.Count != ExpectedNumColors )
      {
        Combo.BeginUpdate();

        while ( Combo.Items.Count < ExpectedNumColors )
        {
          Combo.Items.Add( Combo.Items.Count.ToString( "d2" ) );
        }
        while ( Combo.Items.Count > ExpectedNumColors )
        {
          Combo.Items.RemoveAt( ExpectedNumColors );
        }
        Combo.EndUpdate();
      }
    }



  }
}
