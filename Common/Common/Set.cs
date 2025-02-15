using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GR.Collections
{
  [DebuggerDisplay( "Count = {Count}, First = {First}" )]
	public class Set<TValue> : IEnumerable<TValue>
	{
    private Dictionary<TValue,int>    Dictionary = new Dictionary<TValue, int>();



    public Set()
		{
		}



    public Set( Set<TValue> OtherSet )
    {
      foreach ( TValue value in OtherSet )
      {
        Add( value );
      }
    }



    public TValue First
    {
      get
      {
        foreach ( KeyValuePair<TValue, int> pair in Dictionary )
        {
          return pair.Key;
        }
        return default( TValue );
      }
    }



    IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
    {
      foreach ( KeyValuePair<TValue,int> pair in Dictionary )
      {
        yield return pair.Key;
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
        return Dictionary.Count;
      }
    }



		public void Add( TValue Value )
		{
      if ( Dictionary.ContainsKey( Value ) )
      {
        Dictionary[Value] = 1;
      }
      else
      {
        Dictionary.Add( Value, 1 );
      }
		}



    public void AddRange( IEnumerable<TValue> Collection )
    {
      foreach ( TValue key in Collection )
      {
        Add( key );
      }
    }



    public bool ContainsValue( TValue Value )
		{
      if ( Value == null )
      {
        return false;
      }
      return Dictionary.ContainsKey( Value );
		}



    public void Remove( TValue Value )
		{
      Dictionary.Remove( Value );
		}



    public void Merge( Set<TValue> MergeWith )
		{ 
			if ( MergeWith == null )
			{
				return;
			}

      foreach ( KeyValuePair<TValue,int> pair in MergeWith.Dictionary )
			{
			  Add( pair.Key );
			}
		}

	}
}

