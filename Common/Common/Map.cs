using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace GR.Collections
{
  [DebuggerDisplay( "Count = {Count}" )]
	public class Map<TKey, TValue> : IEnumerable<KeyValuePair<TKey,TValue>>
	{
    private SortedDictionary<TKey, TValue> Dictionary = new SortedDictionary<TKey, TValue>();


    public Map()
		{
		}



    public Map( Map<TKey,TValue> OtherMap )
    {
      foreach ( KeyValuePair<TKey, TValue> pair in OtherMap )
      {
        Add( pair.Key, pair.Value );
      }
    }



    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
    {
      foreach ( KeyValuePair<TKey,TValue> pair in Dictionary )
      {
        yield return pair;
      }
    }



    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      throw new NotImplementedException();
    }



    public void Clear()
    {
      Dictionary.Clear();
    }



    public int Count
    {
      get
      {
        int     entries = 0;
        foreach ( var value in this )
        {
          ++entries;
        }
        return entries;
      }
    }



    public SortedDictionary<TKey, TValue>.ValueCollection Values
    {
      get
      {
        return Dictionary.Values;
      }
    }



    public SortedDictionary<TKey, TValue>.KeyCollection Keys
    {
      get
      {
        return Dictionary.Keys;
      }
    }



    public TValue this[TKey Key] 
    {
      get
      {
        if ( !Dictionary.ContainsKey( Key ) )
        {
          Add( Key, default( TValue ) );            
        }
        return Dictionary[Key];
      }
      set
      {
        Add( Key, value );
      }
    }



		public void Add( TKey Key, TValue Value )
		{
      if ( Key == null )
      {
        //throw new InvalidArgumentException();
        return;
      }
      if ( Dictionary.ContainsKey( Key ) )
      {
        Dictionary[Key] = Value;
      }
      else
      {
        Dictionary.Add( Key, Value );
      }
		}



    public bool ContainsKey( TKey Key )
		{
      return Dictionary.ContainsKey( Key );
		}



    public bool ContainsValue( TValue Value )
    {
      return Dictionary.ContainsValue( Value );
    }



    public void Remove( TKey Key )
		{
      Dictionary.Remove( Key );
		}



    public void Merge( Map<TKey, TValue> MergeWith )
		{ 
			if ( MergeWith == null )
			{
				return;
			}

      foreach ( KeyValuePair<TKey, TValue> pair in MergeWith.Dictionary )
			{
			  Add( pair.Key, pair.Value );
			}
		}

	}
}

