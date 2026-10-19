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



    public int DetermineListIndex( ColorEntry previousSelectedColor )
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
      return _ColorIndices.IndexOf( entry );
    }



    public ColorEntry SelectedColor
    {
      get
      {
        if ( _Colors.CharMode == TextCharMode.NES )
        {
          int index = _ColorIndices.IndexOf( _LastSelectedEntry );

          return new ColorEntry( ColorType.CUSTOM_COLOR, (byte)index );
        }

        return _LastSelectedEntry;
      }
      set
      {
        if ( _Colors.CharMode == TextCharMode.NES )
        {
          _SelectedColor = value.ColorIndex % 4;
          _LastSelectedEntry = _ColorIndices[_SelectedColor];
          Redraw();
          return;
        }

        int index = DetermineListIndex( value );
        if ( ( index < 0 ) 
        ||   ( index >= _ColorIndices.Count ) )
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
      _Colors.CharMode    = colors.CharMode;
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
      if ( _Colors.CharMode == TextCharMode.NES )
      {
        var palColors = new ColorSettings( colors );
        for ( int i = 0; i < 4; ++i )
        {
          palColors.Palette.ColorValues[i] = colors.Palette.ColorValues[colors.PaletteIndexMapping[colors.PaletteMappingIndex][i]];
        }
      }
      Redraw();
    }



    public virtual void UpdatePalette( GR.Image.Palette palette )
    {
      _Colors.Palette = palette;
      Redraw();
    }



    public void ToggleSelectedColor()
    {
      if ( _ColorIndices.Count <= 1 )
      {
        return;
      }
      int   index = _ColorIndices.IndexOf( SelectedColor );
      var newColor = _ColorIndices[( index + 1 ) % _ColorIndices.Count];
      SelectedColor = newColor;
    }




  }



}
