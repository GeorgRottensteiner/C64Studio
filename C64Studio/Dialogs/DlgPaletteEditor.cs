using RetroDevStudio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace C64Studio
{
  public partial class DlgPaletteEditor : Form
  {
    public Palette Palette
    {
      private set;
      get;
    }



    public DlgPaletteEditor( StudioCore Core, Palette Pal )
    {
      Palette = new Palette( Pal );
      InitializeComponent();

      paletteList.Items.Add( new ArrangedItemEntry() { Text = "Palette", Tag = Palette } );

      for ( int i = 0; i < Palette.NumColors; ++i )
      {
        listPalette.Items.Add( Palette.Colors[i] );
      }
      listPalette.SelectedIndex = 0;

      Core.Theming.ApplyTheme( this );
    }



    private void btnOK_Click( object sender, EventArgs e )
    {
      DialogResult = DialogResult.OK;
      Close();
    }



    private void listPalette_DrawItem( object sender, DrawItemEventArgs e )
    {
      e.DrawBackground();

      if ( e.Index == -1 )
      {
        return;
      }

      var textRect = new Rectangle( e.Bounds.Left, e.Bounds.Top, 30, e.Bounds.Height );
      if ( ( e.State & DrawItemState.Selected ) == DrawItemState.Selected )
      {
        e.Graphics.DrawString( e.Index.ToString(), listPalette.Font, System.Drawing.SystemBrushes.HighlightText, textRect );
      }
      else
      {
        e.Graphics.DrawString( e.Index.ToString(), listPalette.Font, System.Drawing.SystemBrushes.ControlText, textRect );
      }
      var smallerRect = new Rectangle( e.Bounds.Left + 30, e.Bounds.Top, e.Bounds.Width - 30, e.Bounds.Height );
      e.Graphics.FillRectangle( Palette.ColorBrushes[e.Index], smallerRect );
    }



    private void listPalette_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( listPalette.SelectedIndex == -1 )
      {
        return;
      }

      scrollR.Value   = Palette.Colors[listPalette.SelectedIndex].R;
      scrollG.Value   = Palette.Colors[listPalette.SelectedIndex].G;
      scrollB.Value   = Palette.Colors[listPalette.SelectedIndex].B;

      editR.Text = Palette.Colors[listPalette.SelectedIndex].R.ToString();
      editRHex.Text = Palette.Colors[listPalette.SelectedIndex].R.ToString( "X2" );
      editG.Text = Palette.Colors[listPalette.SelectedIndex].G.ToString();
      editGHex.Text = Palette.Colors[listPalette.SelectedIndex].G.ToString( "X2" );
      editB.Text = Palette.Colors[listPalette.SelectedIndex].B.ToString();
      editBHex.Text = Palette.Colors[listPalette.SelectedIndex].B.ToString( "X2" );

      panelColorPreview.Invalidate();
    }



    private void panelColorPreview_Paint( object sender, PaintEventArgs e )
    {
      if ( listPalette.SelectedIndex == -1 )
      {
        e.Graphics.FillRectangle( System.Drawing.SystemBrushes.Control, e.ClipRectangle );
        return;
      }

      e.Graphics.FillRectangle( Palette.ColorBrushes[listPalette.SelectedIndex], panelColorPreview.ClientRectangle );
    }



    private void scrollR_Scroll( object sender, ScrollEventArgs e )
    {
      if ( listPalette.SelectedIndex == -1 )
      {
        return;
      }

      var curColor = Palette.Colors[listPalette.SelectedIndex];

      Palette.ColorValues[listPalette.SelectedIndex] = (uint)( ( curColor.A << 24 ) | ( scrollR.Value << 16 ) | ( curColor.G << 8 ) | ( curColor.B ) );

      curColor = GR.Color.Helper.FromARGB( Palette.ColorValues[listPalette.SelectedIndex] );

      Palette.Colors[listPalette.SelectedIndex] = curColor;
      Palette.ColorBrushes[listPalette.SelectedIndex] = new System.Drawing.SolidBrush( curColor );

      editR.Text = Palette.Colors[listPalette.SelectedIndex].R.ToString();
      editRHex.Text = Palette.Colors[listPalette.SelectedIndex].R.ToString( "X2" );

      listPalette.Invalidate( listPalette.GetItemRectangle( listPalette.SelectedIndex ) );
      panelColorPreview.Invalidate();
    }



    private void scrollG_Scroll( object sender, ScrollEventArgs e )
    {
      if ( listPalette.SelectedIndex == -1 )
      {
        return;
      }

      var curColor = Palette.Colors[listPalette.SelectedIndex];

      Palette.ColorValues[listPalette.SelectedIndex] = (uint)( ( curColor.A << 24 ) | ( curColor.R << 16 ) | ( scrollG.Value << 8 ) | ( curColor.B ) );

      curColor = GR.Color.Helper.FromARGB( Palette.ColorValues[listPalette.SelectedIndex] );

      Palette.Colors[listPalette.SelectedIndex] = curColor;
      Palette.ColorBrushes[listPalette.SelectedIndex] = new System.Drawing.SolidBrush( curColor );

      listPalette.Invalidate( listPalette.GetItemRectangle( listPalette.SelectedIndex ) );
      editG.Text = Palette.Colors[listPalette.SelectedIndex].G.ToString();
      editGHex.Text = Palette.Colors[listPalette.SelectedIndex].G.ToString( "X2" );
      panelColorPreview.Invalidate();
    }



    private void scrollB_Scroll( object sender, ScrollEventArgs e )
    {
      if ( listPalette.SelectedIndex == -1 )
      {
        return;
      }

      var curColor = Palette.Colors[listPalette.SelectedIndex];

      Palette.ColorValues[listPalette.SelectedIndex] = (uint)( ( curColor.A << 24 ) | ( curColor.R << 16 ) | ( curColor.G << 8 ) | ( scrollB.Value ) );

      curColor = GR.Color.Helper.FromARGB( Palette.ColorValues[listPalette.SelectedIndex] );

      Palette.Colors[listPalette.SelectedIndex] = curColor;
      Palette.ColorBrushes[listPalette.SelectedIndex] = new System.Drawing.SolidBrush( curColor );

      listPalette.Invalidate( listPalette.GetItemRectangle( listPalette.SelectedIndex ) );
      editB.Text = Palette.Colors[listPalette.SelectedIndex].B.ToString();
      editBHex.Text = Palette.Colors[listPalette.SelectedIndex].B.ToString( "X2" );
      panelColorPreview.Invalidate();
    }



    private void editR_TextChanged( object sender, EventArgs e )
    {
      if ( listPalette.SelectedIndex == -1 )
      {
        return;
      }

      var curColor = Palette.Colors[listPalette.SelectedIndex];
      var newR = GR.Convert.ToI32( editR.Text );
      if ( newR != curColor.R )
      {
        Palette.ColorValues[listPalette.SelectedIndex] = (uint)( ( curColor.A << 24 ) | ( newR << 16 ) | ( curColor.G << 8 ) | ( curColor.B ) );

        curColor = GR.Color.Helper.FromARGB( Palette.ColorValues[listPalette.SelectedIndex] );

        Palette.Colors[listPalette.SelectedIndex] = curColor;
        Palette.ColorBrushes[listPalette.SelectedIndex] = new System.Drawing.SolidBrush( curColor );

        listPalette.Invalidate( listPalette.GetItemRectangle( listPalette.SelectedIndex ) );
        panelColorPreview.Invalidate();
      }
    }



    private void editRHex_TextChanged( object sender, EventArgs e )
    {
      if ( listPalette.SelectedIndex == -1 )
      {
        return;
      }

      var curColor = Palette.Colors[listPalette.SelectedIndex];
      var newR = GR.Convert.ToI32( editRHex.Text, 16 );
      if ( newR != curColor.R )
      {
        Palette.ColorValues[listPalette.SelectedIndex] = (uint)( ( curColor.A << 24 ) | ( newR << 16 ) | ( curColor.G << 8 ) | ( curColor.B ) );

        curColor = GR.Color.Helper.FromARGB( Palette.ColorValues[listPalette.SelectedIndex] );

        Palette.Colors[listPalette.SelectedIndex] = curColor;
        Palette.ColorBrushes[listPalette.SelectedIndex] = new System.Drawing.SolidBrush( curColor );

        listPalette.Invalidate( listPalette.GetItemRectangle( listPalette.SelectedIndex ) );
        panelColorPreview.Invalidate();
      }
    }



    private void editG_TextChanged( object sender, EventArgs e )
    {
      if ( listPalette.SelectedIndex == -1 )
      {
        return;
      }

      var curColor = Palette.Colors[listPalette.SelectedIndex];
      var newG = GR.Convert.ToI32( editG.Text );
      if ( newG != curColor.G )
      {
        Palette.ColorValues[listPalette.SelectedIndex] = (uint)( ( curColor.A << 24 ) | ( curColor.R << 16 ) | ( newG << 8 ) | ( curColor.B ) );

        curColor = GR.Color.Helper.FromARGB( Palette.ColorValues[listPalette.SelectedIndex] );

        Palette.Colors[listPalette.SelectedIndex] = curColor;
        Palette.ColorBrushes[listPalette.SelectedIndex] = new System.Drawing.SolidBrush( curColor );

        listPalette.Invalidate( listPalette.GetItemRectangle( listPalette.SelectedIndex ) );
        panelColorPreview.Invalidate();
      }
    }



    private void editGHex_TextChanged( object sender, EventArgs e )
    {
      if ( listPalette.SelectedIndex == -1 )
      {
        return;
      }

      var curColor = Palette.Colors[listPalette.SelectedIndex];
      var newG = GR.Convert.ToI32( editGHex.Text, 16 );
      if ( newG != curColor.G )
      {
        Palette.ColorValues[listPalette.SelectedIndex] = (uint)( ( curColor.A << 24 ) | ( curColor.R << 16 ) | ( newG << 8 ) | ( curColor.B ) );

        curColor = GR.Color.Helper.FromARGB( Palette.ColorValues[listPalette.SelectedIndex] );

        Palette.Colors[listPalette.SelectedIndex] = curColor;
        Palette.ColorBrushes[listPalette.SelectedIndex] = new System.Drawing.SolidBrush( curColor );

        listPalette.Invalidate( listPalette.GetItemRectangle( listPalette.SelectedIndex ) );
        panelColorPreview.Invalidate();
      }
    }



    private void editB_TextChanged( object sender, EventArgs e )
    {
      if ( listPalette.SelectedIndex == -1 )
      {
        return;
      }

      var curColor = Palette.Colors[listPalette.SelectedIndex];
      var newB = GR.Convert.ToI32( editB.Text );
      if ( newB != curColor.B )
      {
        Palette.ColorValues[listPalette.SelectedIndex] = (uint)( ( curColor.A << 24 ) | ( curColor.R << 16 ) | ( curColor.G << 8 ) | ( newB ) );

        curColor = GR.Color.Helper.FromARGB( Palette.ColorValues[listPalette.SelectedIndex] );

        Palette.Colors[listPalette.SelectedIndex] = curColor;
        Palette.ColorBrushes[listPalette.SelectedIndex] = new System.Drawing.SolidBrush( curColor );

        listPalette.Invalidate( listPalette.GetItemRectangle( listPalette.SelectedIndex ) );
        panelColorPreview.Invalidate();
      }
    }



    private void editBHex_TextChanged( object sender, EventArgs e )
    {
      if ( listPalette.SelectedIndex == -1 )
      {
        return;
      }

      var curColor = Palette.Colors[listPalette.SelectedIndex];
      var newB = GR.Convert.ToI32( editBHex.Text, 16 );
      if ( newB != curColor.B )
      {
        Palette.ColorValues[listPalette.SelectedIndex] = (uint)( ( curColor.A << 24 ) | ( curColor.R << 16 ) | ( curColor.G << 8 ) | ( newB ) );

        curColor = GR.Color.Helper.FromARGB( Palette.ColorValues[listPalette.SelectedIndex] );

        Palette.Colors[listPalette.SelectedIndex] = curColor;
        Palette.ColorBrushes[listPalette.SelectedIndex] = new System.Drawing.SolidBrush( curColor );

        listPalette.Invalidate( listPalette.GetItemRectangle( listPalette.SelectedIndex ) );
        panelColorPreview.Invalidate();
      }
    }



    private ArrangedItemEntry paletteList_AddingItem( object sender )
    {
      var palette = new Palette( Palette );

      ArrangedItemEntry item = new ArrangedItemEntry();
      item.Text = "Palette";
      if ( !string.IsNullOrEmpty( editPaletteName.Text ) )
      {
        item.Text = editPaletteName.Text;
      }
      item.Tag = palette;

      return item;
    }



    private void paletteList_SelectedIndexChanged( object sender, ArrangedItemEntry Item )
    {
      if ( Item == null )
      {
        editPaletteName.Enabled = false;
        return;
      }
      editPaletteName.Enabled = true;
      editPaletteName.Text    = Item.Text;


      Palette = (Palette)Item.Tag;
      for ( int i = 0; i < Palette.NumColors; ++i )
      {
        listPalette.Items[i] = Palette.Colors[i];
      }
      listPalette_SelectedIndexChanged( sender, new EventArgs() );
    }



    private void editPaletteName_TextChanged( object sender, EventArgs e )
    {
      if ( paletteList.SelectedItem != null )
      {
        paletteList.SelectedItem.Text = editPaletteName.Text;
        ( (Palette)paletteList.SelectedItem.Tag ).Name = editPaletteName.Text;
      }
    }



    private ArrangedItemEntry paletteList_CloningItem( object sender, ArrangedItemEntry Item )
    {
      var palette = new Palette( (Palette)Item.Tag );

      ArrangedItemEntry item = new ArrangedItemEntry();
      item.Text = "Palette";
      if ( !string.IsNullOrEmpty( editPaletteName.Text ) )
      {
        item.Text = editPaletteName.Text;
      }
      item.Tag = palette;

      return item;
    }



  }
}
