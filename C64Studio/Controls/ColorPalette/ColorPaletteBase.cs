using GR.Generic;
using RetroDevStudio;
using RetroDevStudio.Formats;
using RetroDevStudio.Types;
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
  public partial class ColorPaletteBase : UserControl
  {
    public StudioCore                   Core = null;

    protected int                       _SelectedColor = 1;
    private int                         _SelectedPaletteMapping = 0;

    protected ColorSettings             _Colors = new ColorSettings();

    protected List<ColorEntry>          _ColorIndices = new List<ColorEntry>();

    protected ColorEntry                _LastSelectedEntry = new ColorEntry();



    public delegate void ColorSelectedHandler( ColorEntry color );
    public delegate void PaletteModifiedHandler( GR.Image.Palette palette );

    public event ColorSelectedHandler           SelectedColorChanged;
    public event PaletteModifiedHandler         PaletteModified;



    public virtual int PaletteOffset
    {
      get; set;
    }



    public byte DetermineListIndex( ColorEntry previousSelectedColor )
    {
      var entry = _ColorIndices.FirstOrDefault( e => ( e.Type == previousSelectedColor.Type ) && ( e.ColorIndex == previousSelectedColor.ColorIndex ) );
      if ( entry == null )
      {
        entry = _ColorIndices.FirstOrDefault( e => e.Type == previousSelectedColor.Type );
        if ( entry == null )
        {
          entry = _ColorIndices.FirstOrDefault( e => e.Type == ColorType.CUSTOM_COLOR );
        }
      }
      return (byte)_ColorIndices.IndexOf( entry );
    }



    public ColorEntry SelectedColor
    {
      get
      {
        return _LastSelectedEntry;
      }
      set
      {
        int index = DetermineListIndex( value );
          //_ColorIndices.IndexOf( value );
        if ( index == -1 )
        {
          index = 0;
        }
        _SelectedColor      = (ushort)index;
        _LastSelectedEntry  = _ColorIndices[_SelectedColor];
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



    public ColorPaletteBase()
    {
      InitializeComponent();
    }



    public ColorPaletteBase( StudioCore Core, ColorSettings colors, List<ColorEntry> colorIndices, byte SelectedColor )
    {
      this.Core           = Core;
      _SelectedColor      = SelectedColor;
      _Colors.Palette     = colors.Palette;
      _ColorIndices       = colorIndices;
      _LastSelectedEntry  = _ColorIndices[_SelectedColor];

      InitializeComponent();
    }



    protected void RaiseColorSelectedEvent()
    {
      if ( SelectedColorChanged != null )
      {
        SelectedColorChanged( _ColorIndices[_SelectedColor] );
      }
    }



    protected void RaisePaletteModifiedEvent( GR.Image.Palette palette )
    {
      if ( PaletteModified != null )
      {
        PaletteModified( palette );
      }
    }



    public virtual void Redraw()
    {
    }



    public virtual void UpdateColors( ColorSettings colors )
    {
      _Colors = colors;
      Redraw();
    }



    public virtual void UpdatePalette( GR.Image.Palette palette )
    {
      _Colors.Palette = palette;
      Redraw();
    }



  }
}
