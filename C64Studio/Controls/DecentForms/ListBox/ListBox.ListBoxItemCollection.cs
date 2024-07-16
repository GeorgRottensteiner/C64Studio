using RetroDevStudio.Controls;
using System;
using System.Collections;
using System.Collections.Generic;



namespace DecentForms
{
  public partial class ListBox
  {
    public class ListBoxItemCollection : IEnumerable<ListBoxItem>
    {
      private ListBox               _Owner;
      private List<ListBoxItem>     _Items = new List<ListBoxItem>();



      internal ListBoxItemCollection( ListBox Owner )
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



      public int IndexOf( ListBoxItem Item )
      {
        return _Items.IndexOf( Item );
      }



      public int Add( ListBoxItem Item )
      {
        Item._Index   = _Items.Count;
        _Items.Add( Item );
        _Owner.ItemsModified();

        return Item._Index;
      }



      public int Add( string Text )
      {
        return Add( new ListBoxItem( Text ) );
      }



      public void AddRange( string[] Items )
      {
        foreach ( var item in Items )
        {
          var newItem = new ListBoxItem( item );
          newItem._Index = _Items.Count;
          _Items.Add( newItem );
        }
        _Owner.ItemsModified();
      }



      public bool Remove( ListBoxItem Item )
      {
        if ( Item.Index == -1 )
        {
          return false;
        }
        if ( !_Items.Remove( Item ) )
        {
          return false;
        }
        for ( int i = Item.Index + 1; i < _Items.Count; i++ )
        {
          --_Items[i]._Index;
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
        for ( int i = _Items[Index].Index + 1; i < _Items.Count; i++ )
        {
          --_Items[i]._Index;
        }
        _Items[Index]._Index = -1;
        _Items.RemoveAt( Index );
        _Owner.ItemsModified();
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
          for ( int i = Index + Count; i < _Items.Count; i++ )
          {
            _Items[i]._Index -= Count;
          }
          _Items.RemoveRange( Index, Count );
          _Owner.ItemsModified();
        }
      }



      public ListBoxItem this[int Index]
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
          _Owner.ItemModified( value );
        }
      }



      public IEnumerator<ListBoxItem> GetEnumerator()
      {
        return _Items.GetEnumerator();
      }



      IEnumerator IEnumerable.GetEnumerator()
      {
        return _Items.GetEnumerator();
      }



      public void Insert( int InsertAtIndex, ListBoxItem Item )
      {
        if ( ( InsertAtIndex < 0 )
        ||   ( InsertAtIndex > _Items.Count ) )
        {
          throw new ArgumentOutOfRangeException( $"Trying to insert item at index {InsertAtIndex} of {_Items.Count}" );
        }
        _Items.Insert( InsertAtIndex, Item );
        for ( int i = InsertAtIndex; i < _Items.Count; ++i )
        {
          _Items[i]._Index = i;
        }
        _Owner.ItemsModified();
      }



      public void Clear()
      {
        _Items.Clear();
        _Owner.ItemsModified();
      }



    }



  }


}