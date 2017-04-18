using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace C64Studio.CustomRenderer
{
  internal class LightToolStripRenderer : ToolStripProfessionalRenderer
  {
    protected override void OnRenderMenuItemBackground( ToolStripItemRenderEventArgs e )
    {
      e.Item.BackColor = Color.FromArgb( 0xd6, 0xdb, 0xe9 );
      e.Item.ForeColor = System.Drawing.Color.Black;
      if ( ( e.Item.Pressed )
      ||   ( e.Item.Selected ) )
      {
        e.Graphics.FillRectangle( new SolidBrush( Color.FromArgb( 0xff, 0xff, 0x00 ) ), 0, 0, e.Item.Width, e.Item.Height );
        //base.OnRenderMenuItemBackground( e );
      }
      else
      {
        e.Graphics.FillRectangle( new SolidBrush( Color.FromArgb( 0xd6, 0xdb, 0xe9 ) ), 0, 0, e.Item.Width, e.Item.Height );
      }
    }

    protected override void OnRenderToolStripBackground( ToolStripRenderEventArgs e )
    {
      e.Graphics.FillRectangle( new SolidBrush( Color.FromArgb( 0xd6, 0xdb, 0xe9 ) ), e.AffectedBounds );
    }

    protected override void OnRenderButtonBackground( ToolStripItemRenderEventArgs e )
    {
      // - selected menu item
      e.Graphics.FillRectangle( new SolidBrush( Color.FromArgb( 0xd6, 0xdb, 0xe9 ) ), e.Item.Bounds );
    }

    protected override void OnRenderSeparator( ToolStripSeparatorRenderEventArgs e )
    {
      e.Graphics.FillRectangle( new SolidBrush( Color.FromArgb( 0xff, 0x00, 0xff ) ), e.Item.Bounds );
      //base.OnRenderSeparator( e );
    }
  }


  internal class CustomToolStripRenderer : ToolStripProfessionalRenderer
  {
    protected override void OnRenderMenuItemBackground( ToolStripItemRenderEventArgs e )
    {
      base.OnRenderMenuItemBackground( e );
      /*
      if ( e.Item.Pressed || e.Item.Selected )
      {
        LinearGradientBrush brd = new LinearGradientBrush( e.Item.Bounds,
            Color.DarkGray, Color.LightSteelBlue, 90 );
        e.Graphics.FillRectangle( brd, 0, 0, e.Item.Width, e.Item.Height );
      }
      else
      {
        base.OnRenderButtonBackground( e );
      }*/
    }

    protected override void OnRenderToolStripBackground( ToolStripRenderEventArgs e )
    {
      LinearGradientBrush brd = new LinearGradientBrush( e.AffectedBounds,
          Color.White, Color.LightSteelBlue, 90 );
      e.Graphics.FillRectangle( brd, e.AffectedBounds );
    }

    protected override void OnRenderItemText( ToolStripItemTextRenderEventArgs e )
    {
      e.TextColor = Color.Black;
      base.OnRenderItemText( e );
    }

    protected override void OnRenderButtonBackground( ToolStripItemRenderEventArgs e )
    {
      if ( e.Item.Pressed || e.Item.Selected )
      {
        LinearGradientBrush brd = new LinearGradientBrush( e.Item.Bounds,
            Color.DarkGray, Color.LightSteelBlue, 90 );
        e.Graphics.FillRectangle( brd, 0, 0, e.Item.Width, e.Item.Height );
      }
      else
      {
        base.OnRenderButtonBackground( e );
      }

    }

  }
}
