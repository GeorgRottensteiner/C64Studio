using C64Studio.Controls;
using C64Studio.Types;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace C64Studio.CustomRenderer
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



  public class StudioTheme
  {
    private StudioCore        Core;


    public SolidBrush       BrushBackgroundControl = null;
    public Pen              PenBackgroundControl = null;


    public StudioTheme( StudioCore Core )
    {
      this.Core = Core;
    }



    public void ApplyThemeToToolStripItems( ToolStripItemCollection Items )
    {
      foreach ( ToolStripItem item in Items )
      {
        item.BackColor = GR.Color.Helper.FromARGB( Core.Settings.BGColor( ColorableElement.BACKGROUND_CONTROL ) );
        item.ForeColor = GR.Color.Helper.FromARGB( Core.Settings.FGColor( ColorableElement.CONTROL_TEXT ) );
        if ( item is ToolStripSeparator )
        {
          var tsItem = item as ToolStripSeparator;

          tsItem.BackColor = GR.Color.Helper.FromARGB( Core.Settings.BGColor( ColorableElement.BACKGROUND_CONTROL ) );
          tsItem.ForeColor = GR.Color.Helper.FromARGB( Core.Settings.FGColor( ColorableElement.CONTROL_TEXT ) );
        }
        if ( item is ToolStripDropDownItem )
        {
          var tsItem = item as ToolStripDropDownItem;
          ApplyThemeToToolStripItems( tsItem.DropDownItems );
        }
      }
    }



    internal void ApplyTheme( BaseDocument BaseDoc )
    {
      BaseDoc.BackColor = GR.Color.Helper.FromARGB( Core.Settings.BGColor( ColorableElement.BACKGROUND_CONTROL ) );

      RecolorControlsRecursive( BaseDoc.Controls );
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

        control.ForeColor = GR.Color.Helper.FromARGB( Core.Settings.FGColor( ColorableElement.CONTROL_TEXT ) );

        if ( control is ComboBox )
        {
          var combo = control as ComboBox;

          combo.BackColor = GR.Color.Helper.FromARGB( Core.Settings.BGColor( ColorableElement.BACKGROUND_CONTROL ) );
          combo.ForeColor = GR.Color.Helper.FromARGB( Core.Settings.FGColor( ColorableElement.CONTROL_TEXT ) );
        }
        if ( control is ToolStrip )
        {
          var toolStrip = control as ToolStrip;

          ApplyThemeToToolStripItems( toolStrip.Items );
        }

        /*
        if ( control is ListView )
        {
          var lv = control as ListView;

          lv.OwnerDraw = true;
          lv.DrawColumnHeader += ListView_DrawColumnHeader;
        }*/
        RecolorControlsRecursive( control.Controls );
      }
    }



    private void ListView_DrawColumnHeader( object sender, DrawListViewColumnHeaderEventArgs e )
    {
      e.DrawBackground();
      //e.DrawDefault = true;

      //e.Graphics.FillRectangle( new SolidBrush( GR.Color.Helper.FromARGB( Core.Settings.SyntaxColoring[ColorableElement.BACKGROUND_CONTROL].BGColor ) ), e.Bounds );
      e.Graphics.FillRectangle( new SolidBrush( GR.Color.Helper.FromARGB( 0xffff00ff ) ), e.Bounds );

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

      e.Graphics.DrawString( e.Header.Text, e.Header.ListView.Font, Brushes.White, e.Bounds, stringFormat );
      //e.DrawText();
    }



    internal Color DarkenColor( Color OrigColor )
    {
      float   darkFactor = 0.85f; // 0.75f
      return System.Drawing.Color.FromArgb( OrigColor.A, (int)( OrigColor.R * darkFactor ), (int)( OrigColor.G * darkFactor ), (int)( OrigColor.B * darkFactor ) );
    }



    public void DrawSingleColorComboBox( ComboBox Combo, DrawItemEventArgs e )
    {
      e.DrawBackground();
      if ( e.Index == -1 )
      {
        return;
      }

      int offset = (int)e.Graphics.MeasureString( "22", Combo.Font ).Width + 5 + 3;

      System.Drawing.Rectangle itemRect = new System.Drawing.Rectangle( e.Bounds.Left + offset, e.Bounds.Top, e.Bounds.Width - offset, e.Bounds.Height );
      if ( ( e.State & DrawItemState.Disabled ) != 0 )
      {
        e.Graphics.FillRectangle( System.Drawing.SystemBrushes.GrayText, itemRect );
        e.Graphics.DrawString( Combo.Items[e.Index].ToString(), Combo.Font, new System.Drawing.SolidBrush( System.Drawing.Color.Gray ), 3.0f, e.Bounds.Top + 1.0f );
      }
      else if ( ( e.State & DrawItemState.Selected ) != 0 )
      {
        e.Graphics.FillRectangle( Types.ConstantData.Palette.ColorBrushes[e.Index], itemRect );
        e.Graphics.DrawString( Combo.Items[e.Index].ToString(), Combo.Font, new System.Drawing.SolidBrush( Combo.ForeColor ), 3.0f, e.Bounds.Top + 1.0f );
      }
      else
      {
        e.Graphics.FillRectangle( Types.ConstantData.Palette.ColorBrushes[e.Index], itemRect );
        e.Graphics.DrawString( Combo.Items[e.Index].ToString(), Combo.Font, new System.Drawing.SolidBrush( Combo.ForeColor ), 3.0f, e.Bounds.Top + 1.0f );
      }
    }



    public void DrawMultiColorComboBox( ComboBox Combo, DrawItemEventArgs e )
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
        e.Graphics.FillRectangle( Types.ConstantData.Palette.ColorBrushes[e.Index], itemRect );
        itemRect = new System.Drawing.Rectangle( e.Bounds.Left + offset + ( e.Bounds.Width - offset ) / 2, e.Bounds.Top, e.Bounds.Width - ( e.Bounds.Width - offset ) / 2, e.Bounds.Height );
        e.Graphics.FillRectangle( Types.ConstantData.Palette.ColorBrushes[e.Index - 8], itemRect );
      }
      else
      {
        e.Graphics.FillRectangle( Types.ConstantData.Palette.ColorBrushes[e.Index], itemRect );
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
