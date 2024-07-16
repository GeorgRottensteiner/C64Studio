using System;
using System.Collections;
using System.Collections.Generic;



namespace DecentForms
{
  public partial class ListBox
  {
    public class ListBoxItemIndexCollection : IEnumerable<int>, ICollection
    {
      private ListBox         _Owner;
      private List<int>       _Items = new List<int>();

      private object          _SyncRoot = new object();
      private bool            _IsSynchronized = false;



      internal ListBoxItemIndexCollection( ListBox Owner )
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



      public object SyncRoot
      {
        get
        {
          return _SyncRoot;
        }
      }



      public bool IsSynchronized
      {
        get
        {
          return _IsSynchronized;
        }
      }



      public int IndexOf( int Item )
      {
        return _Items.IndexOf( Item );
      }



      public void Add( int Item )
      {
        _Items.Add( Item );
        _Owner.ItemsModified();
      }



      public void AddRange( int[] Items )
      {
        foreach ( var item in Items )
        {
          _Items.Add( item );
        }
        _Owner.ItemsModified();
      }



      public void Remove( int Index )
      {
        if ( _Items.Remove( Index ) )
        {
          _Owner.ItemsModified();
        }
      }



      public void RemoveAt( int Index )
      {
        if ( ( Index < 0 )
        ||   ( Index >= _Items.Count ) )
        {
          return;
        }
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
          _Items.RemoveRange( Index, Count );
          _Owner.ItemsModified();
        }
      }



      public int this[int Index]
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
          _Owner.ItemModified( _Owner.Items[Index] );
        }
      }



      public IEnumerator<int> GetEnumerator()
      {
        return _Items.GetEnumerator();
      }



      IEnumerator IEnumerable.GetEnumerator()
      {
        return _Items.GetEnumerator();
      }

      public void CopyTo( Array array, int index )
      {
        throw new NotImplementedException();
      }
    }



  }


}