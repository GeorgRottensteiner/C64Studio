using System.Windows.Forms;
using System.Collections;

namespace RetroDevStudio
{
  public class ListViewItemComparer : IComparer
  {
    private int col;
    private DecentForms.SortOrder order;


    public ListViewItemComparer()
    {
      col = 0;
      order = DecentForms.SortOrder.ASCENDING;
    }



    public ListViewItemComparer( int column, DecentForms.SortOrder order )
    {
      col = column;
      this.order = order;
    }



    public int Compare( object x, object y ) 
    {
      var a = (DecentForms.ListControlItem)x;
      var b = (DecentForms.ListControlItem)y;

      int result = string.Compare( a.SubItems[col].Text, b.SubItems[col].Text );

      if ( order == DecentForms.SortOrder.DESCENDING )
      {
        result = -result;
      }
      return result;
    }
  }



  public class NumericListViewItemComparer : IComparer
  {
    private int col;
    private DecentForms.SortOrder order;



    public NumericListViewItemComparer()
    {
      col = 0;
      order = DecentForms.SortOrder.ASCENDING;
    }



    public NumericListViewItemComparer( int column, DecentForms.SortOrder order )
    {
      col = column;
      this.order = order;
    }



    public int Compare( object x, object y )
    {
      var a = (DecentForms.ListControlItem)x;
      var b = (DecentForms.ListControlItem)y;

      int returnVal= -1;

      System.Int64 val1 = 0;
      System.Int64.TryParse( a.SubItems[col].Text, out val1 );
      System.Int64 val2 = 0;
      System.Int64.TryParse( b.SubItems[col].Text, out val2 );

      if ( val1 == val2 )
      {
        return 0;
      }
      returnVal = ( val1 < val2 ) ? -1 : 1;
      if ( order == DecentForms.SortOrder.DESCENDING )
      {
        returnVal = -returnVal;
      }
      return returnVal;
    }
  }



}
