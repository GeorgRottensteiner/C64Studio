using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace C64Studio
{
  public partial class FormSetOffset : Form
  {
    private long        _DisplayOffset = 0;



    public long DisplayOffset
    {
      get
      {
        return _DisplayOffset;
      }
      set
      {
        _DisplayOffset = value;
        editDisplayOffset.Text = "$" + _DisplayOffset.ToString( "X4" );
      }
    }



    public FormSetOffset( StudioCore Core )
    {
      InitializeComponent();

      Core.Theming.ApplyTheme( this );
    }
    


    private void btnOK_Click( object sender, EventArgs e )
    {
      DialogResult = DialogResult.OK;
      Close();
    }



    private void editDisplayOffset_TextChanged(object sender, EventArgs e)
    {
      long lineNo = -1;

      string    text = editDisplayOffset.Text;
      if ( text.StartsWith( "$" ) )
      {
        lineNo = GR.Convert.ToI64( text.Substring( 1 ), 16 );
      }
      else if ( text.ToUpper().StartsWith( "0X" ) )
      {
        lineNo = GR.Convert.ToI64( text.Substring( 2 ), 16 );
      }
      else
      {
        lineNo = GR.Convert.ToI64( text );
      }
      if ( lineNo < 0 )
      {
        lineNo = 0;
      }
      _DisplayOffset = lineNo;
    }
    

  }
}