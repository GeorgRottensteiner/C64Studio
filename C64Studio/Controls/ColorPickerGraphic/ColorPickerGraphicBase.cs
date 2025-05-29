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
  public partial class ColorPickerGraphicBase : UserControl
  {
    public StudioCore                   Core = null;

    private ushort                      _SelectedColor = 1;
    private int                         _SelectedPaletteMapping = 0;

    protected ColorSettings             _Colors = new ColorSettings();



    public delegate void ColorSelectedHandler( ushort Color );

    public event ColorSelectedHandler           SelectedColorChanged;



    public virtual int PaletteOffset
    {
      get; set;
    }



    public ushort SelectedColor
    {
      get
      {
        return _SelectedColor;
      }
      set
      {
        _SelectedColor = value;
        Redraw();
      }
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



    public ColorPickerGraphicBase()
    {
      InitializeComponent();
    }



    public ColorPickerGraphicBase( StudioCore Core, byte SelectedColor )
    {
      this.Core         = Core;
      _SelectedColor    = SelectedColor;

      InitializeComponent();
    }



    protected void RaiseColorSelectedEvent()
    {
      if ( SelectedColorChanged != null )
      {
        SelectedColorChanged( _SelectedColor );
      }
    }



    public virtual void Redraw()
    {
    }



    public virtual void UpdatePalette( Palette palette )
    {
    }



  }
}
