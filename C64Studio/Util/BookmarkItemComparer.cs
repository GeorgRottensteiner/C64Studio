using System;
using System.Collections;
using System.Text;
using System.Windows.Forms;

namespace RetroDevStudio
{
  public class BookmarkItemComparer : IComparer
  {
    private int col;
    private SortOrder order;


    public BookmarkItemComparer()
    {
      col = 0;
      order = SortOrder.Ascending;
    }



    public BookmarkItemComparer( int column, SortOrder order )
    {
      col = column;
      this.order = order;
    }



    private int IntCompare( int Int1, int Int2 )
    {
      if ( Int1 < Int2 )
      {
        return -1;
      }
      else if ( Int1 > Int2 )
      {
        return 1;
      }
      return 0;
    }



    public int Compare( object x, object y )
    {
      var item1 = (ListViewItem)x;
      var item2 = (ListViewItem)y;
      int returnVal = -1;

      // cols are type, line, file
      // sort by cols now
      if ( col == 0 )
      {
        // Type
        returnVal = string.Compare( item1.SubItems[col].Text, item1.SubItems[col].Text );
      }
      else if ( col == 1 )
      {
        // line
        returnVal = IntCompare( GR.Convert.ToI32( item1.SubItems[1].Text ), GR.Convert.ToI32( item2.SubItems[1].Text ) );
      }
      else if ( col == 2 )
      {
        // file
        returnVal = string.Compare( item1.SubItems[col].Text, item1.SubItems[col].Text );
      }

      // Determine whether the sort order is descending.
      if ( order == SortOrder.Descending )
      {
        // Invert the value returned by String.Compare.
        returnVal *= -1;
      }
      return returnVal;
    }
  }

}
