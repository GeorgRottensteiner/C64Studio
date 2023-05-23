using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using GR.Image;
using static System.Windows.Forms.AxHost;
using System.Windows.Forms.VisualStyles;

namespace RetroDevStudio.Controls
{
  public class CSListView : System.Windows.Forms.ListView
  {
    public uint SelectedTextColor { get; set; }
    public uint SelectedTextBGColor { get; set; }

    public delegate void DrawItemImageHandler( Graphics G, int X, int Y, ListViewItem Item, ListViewItem.ListViewSubItem SubItem );


    public event DrawItemImageHandler     DrawItemImage;



    public CSListView()
    {
      OwnerDraw = true;
      SelectedTextColor = 0xffffffff;
      SelectedTextBGColor = 0xff0000ff;
    }



    protected override void OnDrawColumnHeader( DrawListViewColumnHeaderEventArgs e )
    {
      e.DrawDefault = true;
    }



    protected override void OnDrawItem( DrawListViewItemEventArgs e )
    {
    }



    protected override void OnDrawSubItem( DrawListViewSubItemEventArgs e )
    {
      var trimming = StringTrimming.None;
      if ( e.SubItem is CSListViewSubItem )
      {
        trimming = ( (CSListViewSubItem)e.SubItem ).Trimming;
      }
      bool firstItem = ( e.Item.SubItems.IndexOf( e.SubItem ) == 0 );
      bool realFirstItem = firstItem;
      if ( CheckBoxes )
      {
        firstItem = ( e.Item.SubItems.IndexOf( e.SubItem ) == 1 );
      }

      // the file path
      var itemBounds = e.SubItem.Bounds;
      if ( firstItem )
      {
        itemBounds = new Rectangle( itemBounds.Left, itemBounds.Top, Columns[0].Width, itemBounds.Height );
      }
      var textBounds = new Rectangle( itemBounds.Left + 3, itemBounds.Top, itemBounds.Width - 6, itemBounds.Height );

      var checkBoxSize = CheckBoxRenderer.GetGlyphSize( e.Graphics, e.Item.Checked ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal );

      if ( e.Item.Selected )
      {
        var color = SelectedTextBGColor;

        e.Graphics.FillRectangle( new SolidBrush( GR.Color.Helper.FromARGB( color ) ), e.Bounds );

        if ( ( realFirstItem )
        &&   ( CheckBoxes ) )
        {
          CheckBoxRenderer.DrawCheckBox( e.Graphics,
                                         new Point( itemBounds.Location.X + ( Columns[0].Width - checkBoxSize.Width ) / 2,
                                                    itemBounds.Location.Y + ( itemBounds.Height - checkBoxSize.Height ) / 2 ),
                                         e.Item.Checked ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal );
        }
        if ( ( firstItem )
        &&   ( e.Item.ImageList != null )
        &&   ( e.Item.ImageIndex >= 0 )
        &&   ( e.Item.ImageIndex < e.Item.ImageList.Images.Count ) )
        {
          var image = e.Item.ImageList.Images[e.Item.ImageIndex];

          if ( DrawItemImage != null )
          {
            DrawItemImage( e.Graphics, itemBounds.Left + 3, itemBounds.Top, e.Item, e.SubItem );
          }
          else
          {
            e.Graphics.DrawImage( image, itemBounds.Left + 3, itemBounds.Top );
          }
          textBounds = new Rectangle( textBounds.Left + image.Width + 6, textBounds.Top, textBounds.Width - image.Width - 6, textBounds.Height );
        }

        e.Graphics.DrawString( e.SubItem.Text, e.SubItem.Font, new SolidBrush( GR.Color.Helper.FromARGB( SelectedTextColor ) ), textBounds,
          new StringFormat() { Trimming = trimming, LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.NoWrap } );
      }
      else
      {
        e.DrawBackground();

        if ( ( realFirstItem )
        &&   ( CheckBoxes ) )
        {
          CheckBoxRenderer.DrawCheckBox( e.Graphics,
                                         new Point( itemBounds.Location.X + ( Columns[0].Width - checkBoxSize.Width ) / 2,
                                                    itemBounds.Location.Y + ( itemBounds.Height - checkBoxSize.Height ) / 2 ),
                                         e.Item.Checked ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal );
        }

        if ( ( firstItem )
        &&   ( e.Item.ImageList != null )
        &&   ( e.Item.ImageIndex >= 0 )
        &&   ( e.Item.ImageIndex < e.Item.ImageList.Images.Count ) )
        {
          var image = e.Item.ImageList.Images[e.Item.ImageIndex];

          if ( DrawItemImage != null )
          {
            DrawItemImage( e.Graphics, itemBounds.Left + 3, itemBounds.Top, e.Item, e.SubItem );
          }
          else
          {
            e.Graphics.DrawImage( image, itemBounds.Left + 3, itemBounds.Top );
          }
          textBounds = new Rectangle( textBounds.Left + image.Width + 6, textBounds.Top, textBounds.Width - image.Width - 6, textBounds.Height );
        }

        e.Graphics.DrawString( e.SubItem.Text, e.SubItem.Font, new SolidBrush( ForeColor ), textBounds,
          new StringFormat() { Trimming = trimming, LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.NoWrap } );
      }

      if ( e.ItemState == ListViewItemStates.Focused )
      {
        e.DrawFocusRectangle( e.Bounds );
      }
    }


  }



  public class CSListViewSubItem : ListViewItem.ListViewSubItem
  {
    public StringTrimming Trimming { get; set; } = StringTrimming.None;


    public CSListViewSubItem()
    {
    }



    public CSListViewSubItem( string Text, StringTrimming Trimming = StringTrimming.None )
    {
      this.Text = Text;
      this.Trimming = Trimming;
    }



  }



}
