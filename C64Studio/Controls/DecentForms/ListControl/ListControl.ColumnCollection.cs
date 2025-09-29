using RetroDevStudio.Controls;
using System;
using System.Collections;
using System.Collections.Generic;



namespace DecentForms
{
  public class ColumnCollection : IEnumerable<Column>
  {
    private ListControl      _Owner;
    private List<Column>     _Items = new List<Column>();



    internal ColumnCollection( ListControl Owner )
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



    public int IndexOf( Column Item )
    {
      return _Items.IndexOf( Item );
    }



    public int Add( Column Item )
    {
      _Items.Add( Item );
      Item._Owner = _Owner;
      _Owner.ItemsModified();

      return Item.Index;
    }



    public int Add( string Text, int width = 20 )
    {
      return Add( new Column( Text, width ) );
    }



    public void AddRange( string[] Items )
    {
      foreach ( var item in Items )
      {
        var newItem = new Column( item );
        newItem._Owner = _Owner;
        _Items.Add( newItem );
      }
      _Owner.ItemsModified();
    }



    public bool Remove( Column Item )
    {
      if ( Item.Index == -1 )
      {
        return false;
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
      _Items[Index]._Owner = null;
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
        for ( int i = 0; i < Count;  ++i )
        {
          _Items[Index + i]._Owner = null;
        }
        _Items.RemoveRange( Index, Count );
        _Owner.ItemsModified();
      }
    }



    public Column this[int Index]
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
        _Owner.Invalidate();
      }
    }



    public IEnumerator<Column> GetEnumerator()
    {
      return _Items.GetEnumerator();
    }



    IEnumerator IEnumerable.GetEnumerator()
    {
      return _Items.GetEnumerator();
    }



    public void Insert( int InsertAtIndex, Column Item )
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