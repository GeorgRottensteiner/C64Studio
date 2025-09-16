using RetroDevStudio.Controls;
using System;
using System.Collections;
using System.Collections.Generic;



namespace DecentForms
{
  public partial class GridList
  {
    public class GridListItemCollection : IEnumerable<GridListItem>
    {
      private GridList               _Owner;
      private List<GridListItem>     _Items = new List<GridListItem>();



      internal GridListItemCollection( GridList Owner )
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



      public int IndexOf( GridListItem Item )
      {
        return _Items.IndexOf( Item );
      }



      public int Add( GridListItem Item )
      {
        _Items.Add( Item );
        Item._Owner = _Owner;
        _Owner.ItemsModified();

        return Item._Index;
      }



      public int Add( string Text )
      {
        return Add( new GridListItem( Text ) );
      }



      public void AddRange( string[] Items )
      {
        foreach ( var item in Items )
        {
          var newItem = new GridListItem( item );
          newItem._Owner = _Owner;
          _Items.Add( newItem );
        }
        _Owner.ItemsModified();
      }



      public bool Remove( GridListItem Item )
      {
        if ( Item.Index == -1 )
        {
          return false;
        }
        if ( Item.Selected )
        {
          Item.Selected = false;
        }
        if ( !_Items.Remove( Item ) )
        {
          return false;
        }
        Item._Owner = null;
        if ( _Owner.SelectedIndex == Item.Index )
        {
          // refresh selection
          _Owner.SelectedIndex = -1;
          if ( Item.Index < _Items.Count )
          {
            _Owner.SelectedIndex = Item.Index;
          }
        }
        _Owner.ItemsModified();
        return true;
      }



      public void RemoveAt( int Index )
      {
        if ( ( Index < 0 )
        ||   ( Index >= _Items.Count ) )
        {
          return;
        }
        _Items[Index].Selected = false;
        _Items[Index]._Owner = null;
        _Items.RemoveAt( Index );

        if ( _Owner.SelectedIndex == Index )
        {
          // refresh selection
          _Owner.SelectedIndex = -1;
          if ( Index < _Items.Count )
          {
            _Owner.SelectedIndex = Index;
          }
        }

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
          for ( int i = 0; i < Count;  ++i )
          {
            _Items[Index + i].Selected = false;
            _Items[Index + i]._Owner = null;
          }
          _Items.RemoveRange( Index, Count );
          _Owner.ItemsModified();
        }
      }



      public GridListItem this[int Index]
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



      public IEnumerator<GridListItem> GetEnumerator()
      {
        return _Items.GetEnumerator();
      }



      IEnumerator IEnumerable.GetEnumerator()
      {
        return _Items.GetEnumerator();
      }



      public void Insert( int InsertAtIndex, GridListItem Item )
      {
        if ( ( InsertAtIndex < 0 )
        ||   ( InsertAtIndex > _Items.Count ) )
        {
          throw new ArgumentOutOfRangeException( $"Trying to insert item at index {InsertAtIndex} of {_Items.Count}" );
        }
        Item._Owner = _Owner;
        _Items.Insert( InsertAtIndex, Item );
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