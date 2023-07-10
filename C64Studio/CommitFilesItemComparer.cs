using SourceControl;
using System;
using System.Collections;
using System.Text;
using System.Windows.Forms;

namespace RetroDevStudio
{
  public class CommitFilesItemComparer : IComparer
  {
    private int col;
    private SortOrder order;


    public CommitFilesItemComparer()
    {
      col = 0;
      order = SortOrder.Ascending;
    }



    public CommitFilesItemComparer( int column, SortOrder order )
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
      int returnVal = -1;
      if ( x == null )
      {
        if ( y == null )
        {
          returnVal = 0;
        }
        returnVal = 1;
      }
      else if ( y == null )
      {
        returnVal = -1;
      }
      else
      {
        var msg1 = (FileState)( ( (ListViewItem)x ).Tag );
        var msg2 = (FileState)( ( (ListViewItem)y ).Tag );

        // cols are check state, filestatus, filename
        // sort by cols now

        if ( col == 0 )
        {
          // Type
          returnVal = IntCompare( ( (ListViewItem)x ).Checked ? 1 : 0, ( (ListViewItem)y ).Checked ? 1 : 0 );
        }
        if ( ( col == 1 )
        ||   ( ( returnVal == 0 )
        &&     ( col < 1 ) ) )
        {
          // filestate
          returnVal = IntCompare( (int)msg1, (int)msg2 );
        }
        if ( ( col == 2 )
        ||   ( ( returnVal == 0 )
        &&     ( col < 2 ) ) )
        {
          // file
          returnVal = string.Compare( ( (ListViewItem)x ).SubItems[2].Text, ( (ListViewItem)y ).SubItems[2].Text, true );
        }
        if ( ( col == 3 )
        ||   ( ( returnVal == 0 )
        &&     ( col < 3 ) ) )
        {
          // extension
          returnVal = string.Compare( ( (ListViewItem)x ).SubItems[3].Text, ( (ListViewItem)y ).SubItems[3].Text, true );
        }
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
