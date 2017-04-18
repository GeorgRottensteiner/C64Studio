using System;
using System.Collections.Generic;

namespace Be.Windows.Forms
{
  public class DynamicByteSelectionProvider : ISelectedByteProvider
  {
    List<bool> _selectedState;

    public DynamicByteSelectionProvider( long NumBytes )
    {
      _selectedState = new List<bool>();
      for ( long i = 0; i < NumBytes; ++i )
      {
        _selectedState.Add( false );
      }
    }


    public bool IsByteSelected( long index )
    {
      if ( ( index < 0 )
      ||   ( index >= _selectedState.Count ) )
      {
        throw new IndexOutOfRangeException( "Index " + index + " is out of bounds 0," + _selectedState.Count );
      }
      return _selectedState[(int)index];
    }



    public void SetByteSelectionState( long index, bool Selected )
    {
      if ( ( index < 0 )
      ||   ( index >= _selectedState.Count ) )
      {
        throw new IndexOutOfRangeException( "Index " + index + " is out of bounds 0," + _selectedState.Count );
      }
      _selectedState[(int)index] = Selected;
    }



    public void InsertBytes( long index, long length )
    {
      for ( int i = 0; i < length; ++i )
      {
        _selectedState.Insert( (int)index, false );
      }
    }



    public void DeleteBytes( long index, long length )
    {
      _selectedState.RemoveRange( (int)index, (int)length );
    }

  }

}
