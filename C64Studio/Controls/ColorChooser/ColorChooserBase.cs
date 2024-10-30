using RetroDevStudio;
using RetroDevStudio.Types;
using RetroDevStudio.Formats;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;



namespace RetroDevStudio.Controls
{
  public partial class ColorChooserBase : UserControl
  {
    public StudioCore                   Core = null;

    private int                         _SelectedPaletteMapping = 0;

    protected ColorSettings             _Colors = null;



    public delegate void PaletteMappingSelectedHandler( int PaletteMapping );
    public delegate void ColorsModifiedHandler( ColorType Color, ColorSettings Colors );

    public event PaletteMappingSelectedHandler  PaletteMappingSelected;
    public event ColorsModifiedHandler          ColorsModified;



    public virtual int PaletteOffset
    {
      get; set;
    }



    public int SelectedPaletteMapping
    {
      get
      {
        return _SelectedPaletteMapping;
      }
      set
      {
        _SelectedPaletteMapping = value;
        Redraw();
      }
    }



    public ColorChooserBase()
    {
      InitializeComponent();
    }



    public ColorChooserBase( StudioCore Core, ColorSettings Colors )
    {
      this.Core   = Core;
      _Colors     = Colors;  

      InitializeComponent();
    }



    public virtual void ColorChanged( ColorType Color, int Value )
    {
    }



    protected void RaiseColorsModifiedEvent( ColorType Color )
    {
      if ( ColorsModified != null )
      {
        ColorsModified( Color, _Colors );
      }
    }



    protected void RaisePaletteMappingSelectedEvent()
    {
      if ( PaletteMappingSelected != null )
      {
        PaletteMappingSelected( _SelectedPaletteMapping );
      }
    }



    public virtual void Redraw()
    {
    }



  }
}
