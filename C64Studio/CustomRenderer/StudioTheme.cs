using RetroDevStudio.Controls;
using RetroDevStudio.Types;
using RetroDevStudio;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace RetroDevStudio.CustomRenderer
{
  public class ToolStripSeparatorRenderer : ToolStripProfessionalRenderer
  {
    private StudioCore      Core;

    public ToolStripSeparatorRenderer( StudioCore Core )
    {
      this.Core = Core;
    }



    protected override void OnRenderSeparator( ToolStripSeparatorRenderEventArgs e )
    {
      var bounds = new Rectangle( Point.Empty, e.Item.Size );

      e.Graphics.FillRectangle( new SolidBrush( GR.Color.Helper.FromARGB( Core.Settings.BGColor( ColorableElement.BACKGROUND_CONTROL ) ) ), bounds );

      int     lineY = bounds.Bottom - ( bounds.Height / 2 ) - 1;
      int     lineLeft = bounds.Left + 3;
      int     lineRight = bounds.Right;

      e.Graphics.DrawLine( new System.Drawing.Pen( GR.Color.Helper.FromARGB( Core.Settings.FGColor( ColorableElement.BACKGROUND_CONTROL ) ) ), lineLeft, lineY, lineRight, lineY );
      //base.OnRenderSeparator( e );
    }
  }



  public class StudioTheme : ToolStripProfessionalRenderer
  {
    private StudioCore      Core;


    public SolidBrush       BrushBackgroundControl = null;
    public Pen              PenBackgroundControl = null;


    public StudioTheme( StudioCore Core )
    {
      this.Core = Core;
    }



    protected override void OnRenderMenuItemBackground( ToolStripItemRenderEventArgs e )
    {
      UInt32      color = Core.Settings.BGColor( ColorableElement.BACKGROUND_CONTROL );
      if ( ( e.Item.Pressed )
      ||   ( e.Item.Selected ) )
      {
        e.Graphics.FillRectangle( new SolidBrush( GR.Color.Helper.FromARGB( color ) ), 0, 0, e.Item.Width, e.Item.Height );

        color = Core.Settings.FGColor( ColorableElement.SELECTED_TEXT );

        // make transparent
        if ( ( color & 0xff000000 ) == 0xff000000 )
        {
          color = ( color & 0x00ffffff ) | 0x40000000;
        }
      }
      e.Graphics.FillRectangle( new SolidBrush( GR.Color.Helper.FromARGB( color ) ), 0, 0, e.Item.Width, e.Item.Height );
    }



    protected override void OnRenderSeparator( ToolStripSeparatorRenderEventArgs e )
    {
      var bounds = new Rectangle( Point.Empty, e.Item.Size );
      bounds = new Rectangle( Point.Empty, e.Item.Bounds.Size );
      e.Graphics.FillRectangle( new SolidBrush( GR.Color.Helper.FromARGB( Core.Settings.BGColor( ColorableElement.BACKGROUND_CONTROL ) ) ), bounds );

      int     lineY = bounds.Bottom - ( bounds.Height / 2 ) - 1;
      int     lineLeft = bounds.Left + 3;
      int     lineRight = bounds.Right;

      e.Graphics.DrawLine( new System.Drawing.Pen( GR.Color.Helper.FromARGB( Core.Settings.FGColor( ColorableElement.BACKGROUND_CONTROL ) ) ), lineLeft, lineY, lineRight, lineY );
      //base.OnRenderSeparator( e );
    }



    public void ApplyThemeToToolStripItems( ToolStrip Strip, ToolStripItemCollection Items )
    {
      if ( Strip.Renderer != this )
      {
        Strip.Renderer = this;
      }

      foreach ( ToolStripItem item in Items )
      {
        item.BackColor = GR.Color.Helper.FromARGB( Core.Settings.BGColor( ColorableElement.BACKGROUND_CONTROL ) );
        item.ForeColor = GR.Color.Helper.FromARGB( Core.Settings.FGColor( ColorableElement.CONTROL_TEXT ) );
        if ( item is ToolStripSeparator )
        {
          var tsItem = item as ToolStripSeparator;

          tsItem.BackColor = GR.Color.Helper.FromARGB( Core.Settings.BGColor( ColorableElement.BACKGROUND_BUTTON ) );
          tsItem.ForeColor = GR.Color.Helper.FromARGB( Core.Settings.FGColor( ColorableElement.CONTROL_TEXT ) );
        }
        if ( item is ToolStripDropDownItem )
        {
          var tsItem = item as ToolStripDropDownItem;
          ApplyThemeToToolStripItems( Strip, tsItem.DropDownItems );
        }
      }
    }



    public void RecolorControlsRecursive( Control.ControlCollection Controls )
    {
      foreach ( Control control in Controls )
      {
        control.BackColor = GR.Color.Helper.FromARGB( Core.Settings.BGColor( ColorableElement.BACKGROUND_CONTROL ) );

        if ( control is ThemedButton )
        {
          var button = control as ThemedButton;

          button.DisabledTextColor = DarkenColor( GR.Color.Helper.FromARGB( Core.Settings.BGColor( ColorableElement.CONTROL_TEXT ) ) );
        }
        /*
        if ( control is MenuStrip )
        {
          var menuStrip = control as MenuStrip;
          if ( menuStrip.Renderer != this )
          {
            menuStrip.Renderer = this;
          }
        }*/

        control.ForeColor = GR.Color.Helper.FromARGB( Core.Settings.FGColor( ColorableElement.CONTROL_TEXT ) );

        if ( control is ComboBox )
        {
          var combo = control as ComboBox;

          combo.BackColor = GR.Color.Helper.FromARGB( Core.Settings.BGColor( ColorableElement.BACKGROUND_CONTROL ) );
          combo.ForeColor = GR.Color.Helper.FromARGB( Core.Settings.FGColor( ColorableElement.CONTROL_TEXT ) );
        }
        if ( control is Button )
        {
          var button = control as Button;

          button.BackColor = GR.Color.Helper.FromARGB( Core.Settings.BGColor( ColorableElement.BACKGROUND_BUTTON ) );
        }
        if ( control is RadioButton )
        {
          var button = control as RadioButton;
          if ( button.Appearance == Appearance.Button )
          {
            button.BackColor = GR.Color.Helper.FromARGB( Core.Settings.BGColor( ColorableElement.BACKGROUND_BUTTON ) );
          }
        }
        if ( control is CheckBox )
        {
          var button = control as CheckBox;

          if ( button.Appearance == Appearance.Button )
          {
            button.BackColor = GR.Color.Helper.FromARGB( Core.Settings.BGColor( ColorableElement.BACKGROUND_BUTTON ) );
          }
        }
        if ( control is ToolStrip )
        {
          var toolStrip = control as ToolStrip;

          ApplyThemeToToolStripItems( toolStrip, toolStrip.Items );
        }
        if ( control is TabControl )
        {
          var tabControl = control as TabControl;

          if ( tabControl.DrawMode != TabDrawMode.OwnerDrawFixed )
          {
            tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl.DrawItem += TabControl_DrawItem;
          }
        }
        if ( control is ListView )
        {
          var lv = control as ListView;

          if ( !lv.OwnerDraw )
          {
            lv.OwnerDraw = true;
            lv.DrawColumnHeader += ListView_DrawColumnHeader;
            lv.DrawItem += Lv_DrawItem;
            lv.DrawSubItem += Lv_DrawSubItem;
          }
        }
        if ( control is CSAutoCompleteComboBox )
        {
          continue;
        }
        RecolorControlsRecursive( control.Controls );
      }
    }



    private void Lv_DrawSubItem( object sender, DrawListViewSubItemEventArgs e )
    {
      e.DrawDefault = true;
    }



    private void Lv_DrawItem( object sender, DrawListViewItemEventArgs e )
    {
      e.DrawDefault = true;
    }



    private void TabControl_DrawItem( object sender, DrawItemEventArgs e )
    {
      var control = (TabControl)sender;

      TabPage page = control.TabPages[e.Index];

      var bgColorUnselected = DarkenColor( GR.Color.Helper.FromARGB( Core.Settings.BGColor( ColorableElement.BACKGROUND_CONTROL ) ) );
      var bgColorSelected = GR.Color.Helper.FromARGB( Core.Settings.BGColor( ColorableElement.BACKGROUND_CONTROL ) );

      if ( e.State == DrawItemState.Selected )
      {
        page.BackColor = bgColorSelected;
      }
      else
      {
        page.BackColor = bgColorUnselected;
      }
      e.Graphics.FillRectangle( new SolidBrush( page.BackColor ), e.Bounds );

      Rectangle paddedBounds = e.Bounds;
      int yOffset = (e.State == DrawItemState.Selected) ? -2 : 1;
      paddedBounds.Offset( 1, yOffset );
      TextRenderer.DrawText( e.Graphics, page.Text, e.Font, paddedBounds, page.ForeColor );
    }



    private void ListView_DrawColumnHeader( object sender, DrawListViewColumnHeaderEventArgs e )
    {
      e.DrawBackground();

      e.Graphics.FillRectangle( new SolidBrush( DarkenColor( GR.Color.Helper.FromARGB( Core.Settings.BGColor( ColorableElement.BACKGROUND_CONTROL ) ) ) ), e.Bounds );
      e.Graphics.FillRectangle( new SolidBrush( DarkenColor( DarkenColor( GR.Color.Helper.FromARGB( Core.Settings.BGColor( ColorableElement.CONTROL_TEXT ) ) ) ) ),
        new Rectangle( e.Bounds.Right - 1, e.Bounds.Top, 1, e.Bounds.Height ) );

      var stringFormat = new StringFormat();
      stringFormat.LineAlignment = StringAlignment.Center;

      if ( e.Header.TextAlign == HorizontalAlignment.Center )
      {
        stringFormat.Alignment |= StringAlignment.Center;
      }
      else if ( e.Header.TextAlign == HorizontalAlignment.Right )
      {
        stringFormat.Alignment |= StringAlignment.Far;
      }

      e.Graphics.DrawString( e.Header.Text, e.Header.ListView.Font, 
        new SolidBrush( GR.Color.Helper.FromARGB( Core.Settings.FGColor( ColorableElement.CONTROL_TEXT ) ) ),
        e.Bounds, stringFormat );
    }



    public void ApplyTheme( Form Form )
    {
      Form.BackColor = GR.Color.Helper.FromARGB( Core.Settings.BGColor( ColorableElement.BACKGROUND_CONTROL ) );

      RecolorControlsRecursive( Form.Controls );
    }



    internal Color DarkenColor( Color OrigColor )
    {
      float   darkFactor = 0.85f; // 0.75f
      return System.Drawing.Color.FromArgb( OrigColor.A, (int)( OrigColor.R * darkFactor ), (int)( OrigColor.G * darkFactor ), (int)( OrigColor.B * darkFactor ) );
    }



    public void DrawSingleColorComboBox( ComboBox Combo, DrawItemEventArgs e, Palette Pal )
    {
      DrawSingleColorComboBox( Combo, e, Pal, 0 );
    }



    public void DrawSingleColorComboBox( ComboBox Combo, DrawItemEventArgs e, Palette Pal, int PaletteOffset )
    {
      e.DrawBackground();
      if ( e.Index == -1 )
      {
        return;
      }

      int   indexToDraw = e.Index;
      int   indexInPalette = e.Index + PaletteOffset;

      int offset = (int)e.Graphics.MeasureString( "22", Combo.Font ).Width + 5 + 3;

      System.Drawing.Rectangle itemRect = new System.Drawing.Rectangle( e.Bounds.Left + offset, e.Bounds.Top, e.Bounds.Width - offset, e.Bounds.Height );
      if ( ( e.State & DrawItemState.Disabled ) != 0 )
      {
        e.Graphics.FillRectangle( System.Drawing.SystemBrushes.GrayText, itemRect );
        e.Graphics.DrawString( Combo.Items[indexToDraw].ToString(), Combo.Font, new System.Drawing.SolidBrush( System.Drawing.Color.Gray ), 3.0f, e.Bounds.Top + 1.0f );
      }
      else if ( ( e.State & DrawItemState.Selected ) != 0 )
      {
        e.Graphics.FillRectangle( Pal.ColorBrushes[indexInPalette], itemRect );
        e.Graphics.DrawString( Combo.Items[indexToDraw].ToString(), Combo.Font, new System.Drawing.SolidBrush( Combo.ForeColor ), 3.0f, e.Bounds.Top + 1.0f );
      }
      else
      {
        e.Graphics.FillRectangle( Pal.ColorBrushes[indexInPalette], itemRect );
        e.Graphics.DrawString( Combo.Items[indexToDraw].ToString(), Combo.Font, new System.Drawing.SolidBrush( Combo.ForeColor ), 3.0f, e.Bounds.Top + 1.0f );
      }
    }



    public void DrawMultiColorComboBox( ComboBox Combo, DrawItemEventArgs e, Palette Pal )
    {
      e.DrawBackground();
      if ( e.Index == -1 )
      {
        return;
      }

      int offset = (int)e.Graphics.MeasureString( "22", Combo.Font ).Width + 5 + 3;
      System.Drawing.Rectangle itemRect = new System.Drawing.Rectangle( e.Bounds.Left + offset, e.Bounds.Top, e.Bounds.Width - offset, e.Bounds.Height );


      if ( e.Index >= 8 )
      {
        itemRect = new System.Drawing.Rectangle( e.Bounds.Left + offset, e.Bounds.Top, ( e.Bounds.Width - offset ) / 2, e.Bounds.Height );
        e.Graphics.FillRectangle( Pal.ColorBrushes[e.Index], itemRect );
        itemRect = new System.Drawing.Rectangle( e.Bounds.Left + offset + ( e.Bounds.Width - offset ) / 2, e.Bounds.Top, e.Bounds.Width - ( e.Bounds.Width - offset ) / 2, e.Bounds.Height );
        e.Graphics.FillRectangle( Pal.ColorBrushes[e.Index - 8], itemRect );
      }
      else
      {
        e.Graphics.FillRectangle( Pal.ColorBrushes[e.Index], itemRect );
      }
      if ( ( e.State & DrawItemState.Disabled ) != 0 )
      {
        e.Graphics.DrawString( Combo.Items[e.Index].ToString(), Combo.Font, new System.Drawing.SolidBrush( Combo.ForeColor ), 3.0f, e.Bounds.Top + 1.0f );
      }
      else if ( ( e.State & DrawItemState.Selected ) != 0 )
      {
        e.Graphics.DrawString( Combo.Items[e.Index].ToString(), Combo.Font, new System.Drawing.SolidBrush( Combo.ForeColor ), 3.0f, e.Bounds.Top + 1.0f );
      }
      else
      {
        e.Graphics.DrawString( Combo.Items[e.Index].ToString(), Combo.Font, new System.Drawing.SolidBrush( Combo.ForeColor ), 3.0f, e.Bounds.Top + 1.0f );
      }
    }

  }
}
