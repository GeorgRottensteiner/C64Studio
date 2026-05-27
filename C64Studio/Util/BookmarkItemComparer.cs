using System;
using System.Collections;
using System.Text;
using System.Windows.Forms;

namespace RetroDevStudio
{
  public class BookmarkItemComparer : IComparer
  {
    private int col;
    private DecentForms.SortOrder order;


    public BookmarkItemComparer()
    {
      col = 0;
      order = DecentForms.SortOrder.ASCENDING;
    }



    public BookmarkItemComparer( int column, DecentForms.SortOrder order )
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
      var item1 = (DecentForms.ListControlItem)x;
      var item2 = (DecentForms.ListControlItem)y;
      int returnVal = -1;

      // cols are type, line, file
      // sort by cols now
      if ( ( col == 0 )
      ||   ( col == 2 ) )
      {
        // Type/File
        returnVal = string.Compare( item1.SubItems[col].Text, item2.SubItems[col].Text );
      }
      else if ( col == 1 )
      {
        // line
        returnVal = IntCompare( GR.Convert.ToI32( item1.SubItems[1].Text ), GR.Convert.ToI32( item2.SubItems[1].Text ) );
      }

      // Determine whether the sort order is descending.
      if ( order == DecentForms.SortOrder.DESCENDING )
      {
        // Invert the value returned by String.Compare.
        returnVal *= -1;
      }
      return returnVal;
    }
  }

}
