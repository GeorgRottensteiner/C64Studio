using GR.Image;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;



namespace DecentForms
{
  public class Label : ControlBase
  {
    private System.Drawing.Image    _Image = null;
    private TextAlignment           _Alignment = TextAlignment.TOP | TextAlignment.LEFT;



    public Label()
    {
      AccessibleRole = AccessibleRole.StaticText;
    }



    [DesignerSerializationVisibility( DesignerSerializationVisibility.Visible )]
    public TextAlignment TextAlignment
    {
      get
      {
        return _Alignment;
      }
      set 
      {
        if ( _Alignment != value )
        {
          _Alignment = value;
          Invalidate();
        }
      }
    }



    [DesignerSerializationVisibility( DesignerSerializationVisibility.Visible )]
    public System.Drawing.Image Image
    {
      get
      {
        return _Image;
      }
      set
      {
        _Image = value.GetImageStretchedDPI();
        Invalidate();
      }
    }


    
    protected override void OnPaint( ControlRenderer Renderer )
    {
      Renderer.RenderLabel( Enabled );
    }



  }
}