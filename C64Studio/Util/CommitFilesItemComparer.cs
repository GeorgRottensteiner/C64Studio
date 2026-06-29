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
    private DecentForms.SortOrder order;


    public CommitFilesItemComparer()
    {
      col = 0;
      order = DecentForms.SortOrder.ASCENDING;
    }



    public CommitFilesItemComparer( int column, DecentForms.SortOrder order )
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
      if ( object.ReferenceEquals( x, null ) )
      {
        if ( object.ReferenceEquals( y, null ) )
        {
          returnVal = 0;
        }
        returnVal = 1;
      }
      else if ( object.ReferenceEquals( y, null ) )
      {
        returnVal = -1;
      }
      else
      {
        var item1 = (DecentForms.ListControlItem)x;
        var item2 = (DecentForms.ListControlItem)y;

        if ( item1.GroupHeader )
        {
          if ( item2.GroupHeader )
          {
            return IntCompare( item1.Index, item2.Index );
          }
          if ( item1.GroupIndex == item2.GroupIndex )
          {
            return -1;
          }
          return IntCompare( item1.GroupIndex, item2.GroupIndex );
        }
        else if ( item2.GroupHeader )
        {
          if ( item1.GroupIndex == item2.GroupIndex )
          {
            return 1;
          }
          return IntCompare( item1.GroupIndex, item2.GroupIndex );
        }
        else if ( item1.GroupIndex != item2.GroupIndex )
        {
          return IntCompare( item1.GroupIndex, item2.GroupIndex );
        }
        else if ( col == 0 )
        {
          // checked
          returnVal = IntCompare( item1.Checked ? 1 : 0, item2.Checked ? 1 : 0 );
        }
        if ( ( col == 1 )
        ||   ( ( returnVal == 0 )
        &&     ( col < 1 ) ) )
        {
          // filestate/type
        var msg1 = (FileState)( item1.Tag );
        var msg2 = (FileState)( item2.Tag );

          returnVal = IntCompare( (int)msg1, (int)msg2 );
        }
        if ( ( col == 2 )
        ||   ( ( returnVal == 0 )
        &&     ( col < 2 ) ) )
        {
          // file
          returnVal = string.Compare( item1.SubItems[2].Text, item2.SubItems[2].Text, true );
        }
        if ( ( col == 3 )
        ||   ( ( returnVal == 0 )
        &&     ( col < 3 ) ) )
        {
          // extension
          returnVal = string.Compare( item1.SubItems[3].Text, item2.SubItems[3].Text, true );
        }
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
