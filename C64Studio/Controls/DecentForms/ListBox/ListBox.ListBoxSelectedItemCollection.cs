﻿using RetroDevStudio.Controls;
using System;
using System.Collections;
using System.Collections.Generic;



namespace DecentForms
{
  public partial class ListBox
  {
    public class ListBoxSelectedItemCollection : IEnumerable<ListBoxItem>
    {
      private ListBox               _Owner;
      private List<ListBoxItem>     _Items = new List<ListBoxItem>();



      internal ListBoxSelectedItemCollection( ListBox Owner )
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
        _Items.Remove( Item );
        if ( Item.Selected )
        {
          Item.Selected = false;
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
        _Items.Remove( _Items[Index] );
        _Items[Index].Selected = false;
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
            _Items.RemoveAt( Index + Count - i - 1 );
            _Items[Index + Count - i - 1].Selected = false;
          }
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
        _Owner.ItemsModified();
      }



      public void Clear()
      {
        if ( _Items.Count > 0 )
        {
          var oldItems = _Items;
          while ( _Items.Count > 0 )
          {
            _Items[0].Selected = false;
          }
          _Owner.ItemsModified();
        }
      }



    }



  }


}