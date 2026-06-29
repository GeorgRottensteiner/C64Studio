using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;



namespace DecentForms
{
  public partial class ListControl
  {
    [DebuggerDisplay( "Count = {Count}" )]
    public class ListControlCheckedItemCollection : IEnumerable<ListControlItem>
    {
      private ListControl               _Owner;
      private List<ListControlItem>     _Items = new List<ListControlItem>();



      internal ListControlCheckedItemCollection( ListControl Owner )
      {
        _Owner = Owner;
      }




      public int Count
      {
        get
        {
          return _Items.Count;
        }
      }



      public int IndexOf( ListControlItem Item )
      {
        return _Items.IndexOf( Item );
      }



      public int Add( ListControlItem Item )
      {
        _Items.Add( Item );
        Item.Checked = true;

        return Item._Index;
      }



      public int Add( string Text )
      {
        return Add( new ListControlItem( Text ) );
      }



      public void AddRange( string[] Items )
      {
        foreach ( var item in Items )
        {
          var newItem = new ListControlItem( item );
          _Items.Add( newItem );
          newItem.Checked = true;
        }
      }



      public bool Remove( ListControlItem Item )
      {
        if ( Item.Index == -1 )
        {
          return false;
        }
        _Items.Remove( Item );
        if ( Item.Checked )
        {
          Item.Checked = false;
        }
        return true;
      }



      public void RemoveAt( int Index )
      {
        if ( ( Index < 0 )
        ||   ( Index >= _Items.Count ) )
        {
          return;
        }
        _Items.Remove( _Items[Index] );
        _Items[Index].Checked = false;
      }



      public void RemoveRange( int Index, int Count )
      {
        if ( Index < 0 )
        {
          Index = 0;
        }
        if ( Count <= 0 )
        {
          return;
        }
        if ( Index + Count > _Items.Count )
        {
          Count = _Items.Count - Index;
        }
        if ( Count > 0 )
        {
          for ( int i = 0; i < Count;  ++i )
          {
            _Items.RemoveAt( Index + Count - i - 1 );
            _Items[Index + Count - i - 1].Checked = false;
          }
        }
      }



      public ListControlItem this[int Index]
      {
        get
        {
          if ( ( Index < 0 )
          ||   ( Index >= _Items.Count ) )
          {
            throw new ArgumentOutOfRangeException( $"Tried to access item {Index} of {_Items.Count}" );
          }
          return _Items[Index];
        }
        set
        {
          if ( ( Index < 0 )
          ||   ( Index >= _Items.Count ) )
          {
            throw new ArgumentOutOfRangeException( $"Tried to access item {Index} of {_Items.Count}" );
          }
          _Items[Index] = value;
          _Owner.ItemModified( Index );
        }
      }



      public IEnumerator<ListControlItem> GetEnumerator()
      {
        return _Items.GetEnumerator();
      }



      IEnumerator IEnumerable.GetEnumerator()
      {
        return _Items.GetEnumerator();
      }



      public void Insert( int InsertAtIndex, ListControlItem Item )
      {
        if ( ( InsertAtIndex < 0 )
        ||   ( InsertAtIndex > _Items.Count ) )
        {
          throw new ArgumentOutOfRangeException( $"Trying to insert item at index {InsertAtIndex} of {_Items.Count}" );
        }
        _Items.Insert( InsertAtIndex, Item );
        Item.Checked = true;
      }



      public void Clear()
      {
        if ( _Items.Count > 0 )
        {
          var oldItems = _Items;
          while ( _Items.Count > 0 )
          {
            _Items[0].Checked = false;
          }
        }
      }



    }



  }


}