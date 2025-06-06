﻿using RetroDevStudio;
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
  public partial class ColorPickerCharsBase : UserControl
  {
    public StudioCore                   Core = null;

    private ushort                      _SelectedColor = 1;
    private ushort                      _SelectedChar = 0;
    private int                         _SelectedPaletteMapping = 0;

    protected CharsetProject            _Charset = null;



    public delegate void PaletteMappingSelectedHandler( int PaletteMapping );
    public delegate void ColorSelectedHandler( ushort Color );

    public event PaletteMappingSelectedHandler  PaletteMappingSelected;
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



    public ushort SelectedChar
    {
      get
      {
        return _SelectedChar;
      }
      set
      {
        _SelectedChar = value;
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



    public ColorPickerCharsBase()
    {
      InitializeComponent();
    }



    public ColorPickerCharsBase( StudioCore Core, CharsetProject Charset, ushort SelectedChar, byte SelectedColor )
    {
      this.Core         = Core;
      _Charset          = Charset;  
      _SelectedColor    = SelectedColor;
      _SelectedChar     = SelectedChar;

      InitializeComponent();
    }



    protected void RaiseColorSelectedEvent()
    {
      if ( SelectedColorChanged != null )
      {
        SelectedColorChanged( _SelectedColor );
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
