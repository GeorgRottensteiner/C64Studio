using RetroDevStudio;
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
  public partial class ColorSettingsBase : UserControl
  {
    public StudioCore                   Core = null;
    public ColorSettings                Colors = null;

    protected ColorType                 _CurrentColorType;
    protected byte                      _CustomColor = 1;
    protected byte                      _SelectedCustomColor = 1;

    public delegate void PaletteModifiedHandler( ColorSettings Colors, int CustomColor, List<int> PaletteIndexMapping );
    public delegate void PaletteSelectedHandler( ColorSettings Colors );
    public delegate void PaletteMappingModifiedHandler( ColorSettings Colors );
    public delegate void PaletteMappingSelectedHandler( ColorSettings Colors );
    public delegate void ColorsModifiedHandler( ColorType Color, ColorSettings Colors, int CustomColor );
    public delegate void ColorSelectedHandler( ColorType Color );
    public delegate void CustomColorSelectedHandler( int SelectedCustomColor );
    public delegate void ExchangeColorsHandler( ColorType Color1, ColorType Color2 );
    public delegate void MulticolorFlagChangedHandler();

    public event PaletteModifiedHandler         PaletteModified;
    public event PaletteSelectedHandler         PaletteSelected;
    public event PaletteMappingModifiedHandler  PaletteMappingModified;
    public event PaletteMappingSelectedHandler  PaletteMappingSelected;
    public event ColorsModifiedHandler          ColorsModified;
    public event ColorSelectedHandler           SelectedColorChanged;
    public event CustomColorSelectedHandler     SelectedCustomColorChanged;
    public event ExchangeColorsHandler          ColorsExchanged;
    public event MulticolorFlagChangedHandler   MulticolorFlagChanged;



    public virtual byte CustomColor
    {
      get; set;
    }



    public virtual int PaletteOffset
    {
      get; set;
    }



    public virtual ColorType SelectedColor
    {
      get
      {
        return _CurrentColorType;
      }
      set
      {
        _CurrentColorType = value;
      }
    }



    public virtual byte SelectedCustomColor
    {
      get
      {
        return _SelectedCustomColor;
      }
      set
      {
        _SelectedCustomColor = value;
      }
    }



    public virtual bool MultiColorEnabled 
    {
      get
      {
        return false;
      }
      set
      {
      }
    }



    public virtual int ActivePalette
    {
      get
      {
        return 0;
      }
      set
      {
      }
    }



    public ColorSettingsBase()
    {
      InitializeComponent();
    }



    public ColorSettingsBase( StudioCore Core, ColorSettings Colors, byte CustomColor )
    {
      this.Colors         = new ColorSettings( Colors );
      this.Core           = Core;
      this.CustomColor    = CustomColor;

      InitializeComponent();
    }



    protected void RaisePaletteModifiedEvent( List<int> PaletteMapping )
    {
      if ( PaletteModified != null )
      {
        PaletteModified( Colors, CustomColor, PaletteMapping );
      }
    }



    protected void RaisePaletteSelectedEvent()
    {
      if ( PaletteSelected != null )
      {
        PaletteSelected( Colors );
      }
    }



    protected void RaiseColorsModifiedEvent( ColorType Color )
    {
      if ( ColorsModified != null )
      {
        ColorsModified( Color, Colors, CustomColor );
      }
    }



    protected void RaisePaletteMappingModifiedEvent()
    {
      if ( PaletteMappingModified != null )
      {
        PaletteMappingModified( Colors );
      }
    }



    protected void RaiseColorSelectedEvent()
    {
      if ( SelectedColorChanged != null )
      {
        SelectedColorChanged( _CurrentColorType );
      }
    }



    protected void RaisePaletteMappingSelectedEvent()
    {
      if ( PaletteMappingSelected != null )
      {
        PaletteMappingSelected( Colors );
      }
    }



    protected void RaiseCustomColorSelectedEvent()
    {
      if ( SelectedCustomColorChanged != null )
      {
        SelectedCustomColorChanged( _SelectedCustomColor );
      }
    }



    protected void RaiseColorsExchangedEvent( ColorType Color1, ColorType Color2 )
    {
      if ( ColorsExchanged != null )
      {
        ColorsExchanged( Color1, Color2 );
      }
    }



    protected void RaiseMulticolorFlagChanged()
    {
      if ( MulticolorFlagChanged != null )
      {
        MulticolorFlagChanged();
      }
    }



    public virtual void ColorChanged( ColorType Color, int Value )
    {
    }



    public void PaletteChanged( Palette Palette )
    {
      Colors.Palette = new Palette( Palette );
      Invalidate();
    }



    public virtual void PalettesChanged()
    {
    }



  }
}
