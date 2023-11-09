using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GR.Collections
{

  public static class SortedListExtensions
  {
    /// <summary>
    /// Performs a binary search on the specified collection.
    /// </summary>
    /// <typeparam name="TItem">The type of the item.</typeparam>
    /// <typeparam name="TSearch">The type of the searched item.</typeparam>
    /// <param name="List">The list to be searched.</param>
    /// <param name="Value">The value to search for.</param>
    /// <param name="Comparer">The comparer that is used to compare the value
    /// with the list items.</param>
    /// <returns></returns>
    public static int BinarySearch<TItem, TSearch>( this IList<TItem> List, TSearch Value, Func<TSearch, TItem, int> Comparer )
    {
      if ( List == null )
      {
        throw new ArgumentNullException( "List" );
      }
      if ( Comparer == null )
      {
        throw new ArgumentNullException( "Comparer" );
      }

      int lower = 0;
      int upper = List.Count - 1;

      while ( lower <= upper )
      {
        int middle = lower + ( upper - lower ) / 2;
        int comparisonResult = Comparer( Value, List[middle] );
        if ( comparisonResult < 0 )
        {
          upper = middle - 1;
        }
        else if ( comparisonResult > 0 )
        {
          lower = middle + 1;
        }
        else
        {
          return middle;
        }
      }

      return ~lower;
    }



    /// <summary>
    /// Performs a binary search on the specified collection.
    /// </summary>
    /// <typeparam name="TItem">The type of the item.</typeparam>
    /// <param name="List">The list to be searched.</param>
    /// <param name="value">The value to search for.</param>
    /// <returns></returns>
    public static int BinarySearch<TItem>( this IList<TItem> List, TItem Value )
    {
      return BinarySearch( List, Value, Comparer<TItem>.Default );
    }



    /// <summary>
    /// Performs a binary search on the specified collection.
    /// </summary>
    /// <typeparam name="TItem">The type of the item.</typeparam>
    /// <param name="List">The list to be searched.</param>
    /// <param name="value">The value to search for.</param>
    /// <param name="Comparer">The comparer that is used to compare the value
    /// with the list items.</param>
    /// <returns></returns>
    public static int BinarySearch<TItem>( this IList<TItem> List, TItem value, IComparer<TItem> Comparer )
    {
      return List.BinarySearch( value, Comparer.Compare );
    }



    public static IEnumerable<KeyValuePair<K, V>> Head<K, V>( this SortedList<K, V> List, K ToKey, bool Inclusive = true )
    {
      var binarySearchResult = List.Keys.BinarySearch( ToKey );
      if ( binarySearchResult < 0 )
      {
        binarySearchResult = ~binarySearchResult;
      }
      else if ( Inclusive )
      {
        binarySearchResult++;
      }
      return System.Linq.Enumerable.Take( List, binarySearchResult );
    }



    public static IEnumerable<KeyValuePair<K, V>> Tail<K, V>( this SortedList<K, V> List, K FromKey, bool Inclusive = true )
    {
      var binarySearchResult = List.Keys.BinarySearch( FromKey );
      if ( binarySearchResult < 0 )
      {
        binarySearchResult = ~binarySearchResult;
      }
      else if ( !Inclusive )
      {
        binarySearchResult++;
      }
      return new ListOffsetEnumerable<K, V>( List, binarySearchResult );
    }



    public static bool TryGetCeilingValue<K, V>( this SortedList<K, V> List, K Key, out V Value )
    {
      var binarySearchResult = List.Keys.BinarySearch( Key );
      if ( binarySearchResult < 0 )
      {
        binarySearchResult = ~binarySearchResult;
      }
      if ( binarySearchResult >= List.Count )
      {
        Value = default( V );
        return false;
      }
      Value = List.Values[binarySearchResult];
      return true;
    }



    public static bool TryGetHigherValue<K, V>( this SortedList<K, V> List, K Key, out V Value )
    {
      var binarySearchResult = List.Keys.BinarySearch( Key );
      if ( binarySearchResult < 0 )
      {
        binarySearchResult = ~binarySearchResult;
      }
      else
      {
        binarySearchResult++;
      }
      if ( binarySearchResult >= List.Count )
      {
        Value = default( V );
        return false;
      }
      Value = List.Values[binarySearchResult];
      return true;
    }



    public static bool TryGetHigherKey<K, V>( this SortedList<K, V> List, K Key, out K Value )
    {
      var binarySearchResult = List.Keys.BinarySearch( Key );
      if ( binarySearchResult < 0 )
      {
        binarySearchResult = ~binarySearchResult;
      }
      else
      {
        binarySearchResult++;
      }
      if ( binarySearchResult >= List.Count )
      {
        if ( ( List.Count > 0 )
        &&   ( Comparer<K>.Default.Compare( Key, List.Keys[0] ) < 0 ) )
        {
          // outside top, return first key
          Value = List.Keys[0];
          return true;
        }
        Value = default( K );
        return false;
      }
      Value = List.Keys[binarySearchResult];
      return true;
    }



    public static bool TryGetFloorValue<K, V>( this SortedList<K, V> List, K Key, out V Value )
    {
      var binarySearchResult = List.Keys.BinarySearch( Key );
      if ( binarySearchResult < 0 )
      {
        binarySearchResult = ~binarySearchResult;
      }
      else
      {
        binarySearchResult++;
      }
      if ( binarySearchResult >= List.Count )
      {
        Value = default( V );
        return false;
      }
      Value = List.Values[binarySearchResult];
      return true;
    }



    public static bool TryGetLowerValue<K, V>( this SortedList<K, V> List, K Key, out V Value )
    {
      var binarySearchResult = List.Keys.BinarySearch( Key );
      if ( binarySearchResult < 0 )
      {
        binarySearchResult = ~binarySearchResult;
      }
      if ( binarySearchResult >= List.Count )
      {
        Value = default( V );
        return false;
      }
      Value = List.Values[binarySearchResult];
      return true;
    }



    public static bool TryGetLowerKey<K, V>( this SortedList<K, V> List, K Key, out K Value )
    {
      var binarySearchResult = List.Keys.BinarySearch( Key );
      if ( binarySearchResult < 0 )
      {
        binarySearchResult = ~binarySearchResult;
        --binarySearchResult;
      }
      if ( binarySearchResult >= List.Count )
      {
        if ( ( List.Count > 0 )
        &&   ( Comparer<K>.Default.Compare( Key, List.Keys[0] ) > 0 ) )
        {
          // outside bottom, return highest key
          Value = List.Keys[List.Count - 1];
          return true;
        }
        Value = default( K );
        return false;
      }
      if ( binarySearchResult < 0 )
      {
        Value = default( K );
        return false;
      }
      Value = List.Keys[binarySearchResult];
      return true;
    }



    class ListOffsetEnumerable<K, V> : IEnumerable<KeyValuePair<K, V>>
    {
      private readonly SortedList<K, V>   _SortedList;
      private readonly int                _Offset;



      public ListOffsetEnumerable( SortedList<K, V> SortedList, int Offset )
      {
        _SortedList = SortedList;
        _Offset     = Offset;
      }



      public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
      {
        return new ListOffsetEnumerator<K, V>( _SortedList, _Offset );
      }



      IEnumerator IEnumerable.GetEnumerator()
      {
        return GetEnumerator();
      }
    }



    class ListOffsetEnumerator<K, V> : IEnumerator<KeyValuePair<K, V>>
    {
      private readonly SortedList<K, V> _SortedList;
      private int                       _Index;

      public ListOffsetEnumerator( SortedList<K, V> SortedList, int Offset )
      {
        _SortedList = SortedList;
        _Index      = Offset - 1;
      }



      public bool MoveNext()
      {
        if ( _Index >= _SortedList.Count )
        {
          return false;
        }
        _Index++;
        return _Index < _SortedList.Count;
      }



      public KeyValuePair<K, V> Current
      {
        get
        {
          return new KeyValuePair<K, V>( _SortedList.Keys[_Index], _SortedList.Values[_Index] );
        }
      }



      object IEnumerator.Current
      {
        get
        {
          return Current;
        }
      }



      public void Dispose()
      {
      }
      

      
      public void Reset()
      {
        throw new NotSupportedException();
      }

    }
  }
}

