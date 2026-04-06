using System.Windows.Forms;
using System.Collections;

namespace GR
{
  namespace Forms
  {
    public class ListViewItemComparer : IComparer
    {
      public int Compare( object x, object y ) 
      {
        return string.Compare( (string)x, (string)y );
      }
    }



    public class NumericListViewItemComparer : IComparer
    {
      public int Compare( object x, object y )
      {
        int returnVal= -1;

        System.Int64 val1 = 0;
        System.Int64.TryParse( (string)x, out val1 );
        System.Int64 val2 = 0;
        System.Int64.TryParse( (string)y, out val2 );

        if ( val1 == val2 )
        {
          return 0;
        }
        returnVal = ( val1 < val2 ) ? -1 : 1;
        return returnVal;
      }
    }



  }
}