using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using GR.Image;

namespace RetroDevStudio.Controls
{
  public class CSListView : System.Windows.Forms.ListView
  {
    public uint SelectedTextColor { get; set; }
    public uint SelectedTextBGColor { get; set; }



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

      // the file path
      var itemBounds = e.SubItem.Bounds;
      if ( firstItem )
      {
        itemBounds = new Rectangle( itemBounds.Left, itemBounds.Top, Columns[0].Width, itemBounds.Height );
      }
      var textBounds = new Rectangle( itemBounds.Left + 3, itemBounds.Top, itemBounds.Width - 6, itemBounds.Height );

      if ( e.Item.Selected )
      {
        var color = SelectedTextBGColor;

        e.Graphics.FillRectangle( new SolidBrush( GR.Color.Helper.FromARGB( color ) ), e.Bounds );
        if ( ( firstItem )
        &&   ( e.Item.ImageList != null )
        &&   ( e.Item.ImageIndex >= 0 )
        &&   ( e.Item.ImageIndex < e.Item.ImageList.Images.Count ) )
        {
          var image = e.Item.ImageList.Images[e.Item.ImageIndex];
          e.Graphics.DrawImage( image, itemBounds.Left + 3, itemBounds.Top );
          textBounds = new Rectangle( textBounds.Left + image.Width + 6, textBounds.Top, textBounds.Width - image.Width - 6, textBounds.Height );
        }

        e.Graphics.DrawString( e.SubItem.Text, e.SubItem.Font, new SolidBrush( GR.Color.Helper.FromARGB( SelectedTextColor ) ), textBounds,
          new StringFormat() { Trimming = trimming, LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.NoWrap } );
      }
      else
      {
        e.DrawBackground();
        if ( ( firstItem )
        &&   ( e.Item.ImageList != null )
        &&   ( e.Item.ImageIndex >= 0 )
        &&   ( e.Item.ImageIndex < e.Item.ImageList.Images.Count ) )
        {
          var image = e.Item.ImageList.Images[e.Item.ImageIndex];
          e.Graphics.DrawImage( image, itemBounds.Left + 3, itemBounds.Top );
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
