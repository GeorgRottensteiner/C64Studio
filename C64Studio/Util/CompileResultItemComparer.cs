using System;
using System.Collections;
using System.Text;
using System.Windows.Forms;

namespace RetroDevStudio
{
  public class CompileResultItemComparer : IComparer
  {
    private int col;
    private SortOrder order;


    public CompileResultItemComparer()
    {
      col = 0;
      order = SortOrder.Ascending;
    }



    public CompileResultItemComparer( int column, SortOrder order )
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
      Parser.ParserBase.ParseMessage msg1 = (Parser.ParserBase.ParseMessage)( ( (ListViewItem)x ).Tag );
      Parser.ParserBase.ParseMessage msg2 = (Parser.ParserBase.ParseMessage)( ( (ListViewItem)y ).Tag );

      int returnVal = -1;

      if ( msg1 == null )
      {
        if ( msg2 == null )
        {
          returnVal = 0;
        }
        returnVal = 1;
      }
      else if ( msg2 == null )
      {
        returnVal = -1;
      }

      // sub messages always below the parent message
      if ( msg2.ParentMessage == msg1 )
      {
        return -1;
      }
      else if ( msg1.ParentMessage == msg2 )
      {
        return 1;
      }

      if ( ( msg1.ParentMessage != null )
      &&   ( msg2.ParentMessage != null )
      &&   ( msg1.ParentMessage == msg2.ParentMessage ) )
      {
        return msg1.ParentMessage.ChildMessages.IndexOf( msg1 ) - msg2.ParentMessage.ChildMessages.IndexOf( msg2 );
      }

      // sort by parent
      if ( msg1.ParentMessage != null )
      {
        msg1 = msg1.ParentMessage;
      }
      if ( msg2.ParentMessage != null )
      {
        msg2 = msg2.ParentMessage;
      }
      // cols are type, line, code, file, message
      // sort by cols now

      if ( col == 0 )
      {
        // Type
        returnVal = IntCompare( (int)msg1.Type, (int)msg2.Type );
      }
      else if ( col == 1 )
      {
        // line
        returnVal = IntCompare( (int)msg1.AlternativeLineIndex, (int)msg2.AlternativeLineIndex );
      }
      else if ( col == 2 )
      {
        // code
        returnVal = IntCompare( (int)msg1.Code, (int)msg2.Code );
      }
      else if ( col == 3 )
      {
        // file
        returnVal = string.Compare( msg1.AlternativeFile, msg2.AlternativeFile );
      }
      else if ( col == 4 )
      {
        // message
        returnVal = string.Compare( msg1.Message, msg2.Message );
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
