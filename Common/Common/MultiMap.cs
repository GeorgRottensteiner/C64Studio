using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Collections
{
	public class MultiMap<TKey, TValue> : IEnumerable<KeyValuePair<TKey,TValue>>
	{
    private SortedDictionary<TKey, List<TValue>>    Dictionary = new SortedDictionary<TKey, List<TValue>>();



    public MultiMap()
		{
		}



    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
    {
      foreach ( TKey key in Dictionary.Keys )
      {
        List<TValue> values = Dictionary[key];
        foreach ( TValue value in values )
        {
          yield return new KeyValuePair<TKey,TValue>( key, value );
        }
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



    public List<TKey> Keys
    {
      get
      {
        List<TKey> keyList = new List<TKey>( Count );

        foreach ( var value in this )
        {
          keyList.Add( value.Key );
        }
        return keyList;
      }
    }



    public List<TValue> Values 
    {
      get
      {
        List<TValue> valueList = new List<TValue>( Count );

        foreach ( var value in this )
        {
          valueList.Add( value.Value );
        }
        return valueList;
      }
    }



    public GR.Collections.Set<TKey> GetUniqueKeys()
    {
      GR.Collections.Set<TKey> keys = new Set<TKey>();

      keys.AddRange( Dictionary.Keys );
      return keys;
    }



    public TKey FirstKey
    {
      get
      {
        if ( Dictionary.Count == 0 )
        {
          return default( TKey );
        }
        foreach ( var value in this )
        {
          return value.Key;
        }
        return default( TKey );
      }
    }



    public TKey LastKey
    {
      get
      {
        if ( Dictionary.Count == 0 )
        {
          return default( TKey );
        }
        // ugh
        TKey  prevKey = default( TKey );
        foreach ( var value in this )
        {
          prevKey = value.Key;
        }
        return prevKey;
      }
    }



    public void Add( TKey Key, TValue Value )
		{
      if ( Key == null )
      {
        //throw new InvalidArgumentException();
        return;
      }

      List<TValue> container = null;
      if ( !Dictionary.TryGetValue( Key, out container ) )
			{
				container = new List<TValue>();
        Dictionary.Add( Key, container );
			}
      container.Add( Value );
		}



		public bool ContainsValue( TKey key, TValue value )
		{
      if ( key == null )
      {
        return false;
      }
			bool toReturn = false;

			List<TValue> values = null;
      if ( Dictionary.TryGetValue( key, out values ) )
			{
				toReturn = values.Contains( value );
			}
			return toReturn;
		}



    public bool ContainsKey( TKey key )
    {
      if ( key == null )
      {
        return false;
      }
      return Dictionary.ContainsKey( key );
    }



    public void Remove( TKey key, TValue value )
		{
      if ( key == null )
      {
        return;
      }

			List<TValue> container = null;
      if ( Dictionary.TryGetValue( key, out container ) )
			{
				container.Remove( value );
				if ( container.Count <= 0 )
				{
          Dictionary.Remove( key );
				}
			}
		}



    public void Merge( MultiMap<TKey, TValue> MergeWith )
		{ 
			if ( MergeWith == null )
			{
				return;
			}

      foreach ( KeyValuePair<TKey, List<TValue>> pair in MergeWith.Dictionary )
			{
				foreach ( TValue value in pair.Value )
				{
					Add( pair.Key, value );
				}
			}
		}


		public List<TValue> GetValues( TKey Key, bool ReturnEmptySetIfKeyNotFound = true )
		{
			List<TValue> toReturn = null;
      if ( ( !Dictionary.TryGetValue( Key, out toReturn ) )
      &&   ( ReturnEmptySetIfKeyNotFound ) )
			{
				toReturn = new List<TValue>();
			}
			return toReturn;
		}
	}
}

