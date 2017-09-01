using System.Windows.Forms;
using System.Collections;

namespace GR
{
  namespace Forms
  {
    public class ListViewItemComparer : IComparer
    {
      private int col;
      private SortOrder order;
      private int textOffset = 0;



      public ListViewItemComparer()
      {
        col = 0;
        order = SortOrder.Ascending;
        textOffset = 0;
      }



      public ListViewItemComparer( int Column, SortOrder Order )
      {
        col = Column;
        this.order = Order;
        textOffset = 0;
      }



      public ListViewItemComparer( int Column, SortOrder Order, int TextOffset )
      {
        col = Column;
        this.order = Order;
        textOffset = TextOffset;
      }
      
      
      
      public int Compare( object x, object y ) 
      {
        int returnVal= -1;
        if ( ( ( (ListViewItem)x ).SubItems[col].Text.Length <= textOffset )
        &&   ( ( (ListViewItem)y ).SubItems[col].Text.Length <= textOffset ) )
        {
          return 0;
        }
        if ( ( (ListViewItem)x ).SubItems[col].Text.Length <= textOffset )
        {
          return -1;
        }
        if ( ( (ListViewItem)y ).SubItems[col].Text.Length <= textOffset )
        {
          return 1;
        }
        returnVal = string.Compare( ( (ListViewItem)x ).SubItems[col].Text.Substring( textOffset ),
                                    ( (ListViewItem)y ).SubItems[col].Text.Substring( textOffset ) );
        // Determine whether the sort order is descending.
        if ( order == SortOrder.Descending )
        {
          // Invert the value returned by String.Compare.
          returnVal *= -1;
        }
        return returnVal;
      }
    }



    public class NumericListViewItemComparer : IComparer
    {
      private int col;
      private SortOrder order;
      private int textOffset = 0;



      public class ItemOnTopDesc
      {
        public int Column = -1;
        public string Name = "";
      }



      public ItemOnTopDesc ItemOnTop
      {
        get;
        set;
      }



      public NumericListViewItemComparer()
      {
        col = 0;
        order = SortOrder.Ascending;
        ItemOnTop = new ItemOnTopDesc();
        textOffset = 0;
      }



      public NumericListViewItemComparer( int column, SortOrder order )
      {
        col = column;
        this.order = order;
        ItemOnTop = new ItemOnTopDesc();
        textOffset = 0;
      }



      public NumericListViewItemComparer( int column, SortOrder order, int TextOffset )
      {
        textOffset = TextOffset;
        col = column;
        this.order = order;
        ItemOnTop = new ItemOnTopDesc();
      }



      public int Compare( object x, object y )
      {
        int returnVal= -1;

        if ( ItemOnTop.Column != -1 )
        {
          if ( ( (ListViewItem)x ).SubItems[ItemOnTop.Column].Text == ItemOnTop.Name )
          {
            return -1;
          }
          if ( ( (ListViewItem)y ).SubItems[ItemOnTop.Column].Text == ItemOnTop.Name )
          {
            return 1;
          }
        }

        if ( ( ( (ListViewItem)x ).SubItems[col].Text.Length <= textOffset )
        &&   ( ( (ListViewItem)y ).SubItems[col].Text.Length <= textOffset ) )
        {
          return 0;
        }
        if ( ( (ListViewItem)x ).SubItems[col].Text.Length <= textOffset )
        {
          return -1;
        }
        if ( ( (ListViewItem)y ).SubItems[col].Text.Length <= textOffset )
        {
          return 1;
        }

        System.Int64 val1 = 0;
        System.Int64.TryParse( ( (ListViewItem)x ).SubItems[col].Text.Substring( textOffset ).Replace( ".", "" ), out val1 );
        System.Int64 val2 = 0;
        System.Int64.TryParse( ( (ListViewItem)y ).SubItems[col].Text.Substring( textOffset ).Replace( ".", "" ), out val2 );

        if ( val1 == val2 )
        {
          return 0;
        }
        returnVal = ( val1 < val2 ) ? -1 : 1;
        // Determine whether the sort order is descending.
        if ( order == SortOrder.Descending )
        {
          // Invert the value returned by String.Compare.
          returnVal *= -1;
        }
        return returnVal;
      }
    }



    class CaseInsensitiveListViewItemComparer : IComparer
    {
      private int col;
      private SortOrder order;

      public class ItemOnTopDesc
      {
        public int Column = -1;
        public string Name = "";
      }



      public ItemOnTopDesc ItemOnTop
      {
        get;
        set;
      }


      public CaseInsensitiveListViewItemComparer()
      {
        col = 0;
        order = SortOrder.Ascending;
        ItemOnTop = new ItemOnTopDesc();
      }



      public CaseInsensitiveListViewItemComparer( int column, SortOrder order )
      {
        col = column;
        this.order = order;
        ItemOnTop = new ItemOnTopDesc();
      }



      public int Compare( object x, object y )
      {
        int returnVal = -1;

        if ( ItemOnTop.Column != -1 )
        {
          if ( ( (ListViewItem)x ).SubItems[ItemOnTop.Column].Text == ItemOnTop.Name )
          {
            return -1;
          }
          if ( ( (ListViewItem)y ).SubItems[ItemOnTop.Column].Text == ItemOnTop.Name )
          {
            return 1;
          }
        }

        returnVal = string.Compare( ( (ListViewItem)x ).SubItems[col].Text.ToUpper(),
                                    ( (ListViewItem)y ).SubItems[col].Text.ToUpper() );
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
}