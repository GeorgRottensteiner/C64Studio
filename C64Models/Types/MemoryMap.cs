using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio.Types
{
  public class MemoryMapEntry
  {
    public int        StartAddress = -1;
    public int        Length = 0;
    public string     Description = "";
    public MemoryMap  SubEntries = new MemoryMap();



    public MemoryMapEntry( int StartAddress, int Length )
    {
      this.StartAddress = StartAddress;
      this.Length = Length;
    }

    public MemoryMapEntry( int StartAddress, int Length, string Desc )
    {
      this.StartAddress = StartAddress;
      this.Length = Length;
      Description = Desc;
    }
  }



  public class MemoryMap
  {
    public List<MemoryMapEntry>      Entries = new List<MemoryMapEntry>();



    public void InsertEntry( MemoryMapEntry Entry )
    {
      /*
      if ( Entry.Length < 0 )
      {
        Debug.Log( "wss" );
      }*/

      bool entryAdded = false;

      for ( int i = 0; i < Entries.Count; ++i )
      {
        var entry = Entries[i];

        if ( Entry.StartAddress + Entry.Length <= entry.StartAddress )
        {
          Entries.Insert( i, Entry );
          return;
        }

        // already inside??
        if ( ( Entry.StartAddress == entry.StartAddress )
        &&   ( Entry.Length == entry.Length )
        &&   ( Entry.Description == entry.Description ) )
        {
          return;
        }

        if ( ( Entry.StartAddress >= entry.StartAddress )
        &&   ( Entry.StartAddress + Entry.Length <= entry.StartAddress + entry.Length ) )
        {
          // completely inside
          entry.SubEntries.InsertEntry( Entry );
          return;
        }
        // half overlaps should add to both potential entries
        if ( ( Entry.StartAddress + Entry.Length > entry.StartAddress )
        &&   ( Entry.StartAddress < entry.StartAddress + entry.Length ) )
        {
          // add to this entry

          // update parent size
          if ( ( entry.StartAddress == Entry.StartAddress )
          &&   ( Entry.Length > entry.Length ) )
          {
            entry.Length = Entry.Length;
          }
          entry.SubEntries.InsertEntry( Entry );
          entryAdded = true;
        }
      }
      if ( !entryAdded )
      {
        // entry not found, append
        Entries.Add( Entry );
      }
    }



    public void Dump()
    {
      foreach ( var mapEntry in Entries )
      {
        Debug.Log( "Block from $" + mapEntry.StartAddress.ToString( "X4" ) + " to $" + ( mapEntry.StartAddress + mapEntry.Length - 1 ).ToString( "X4" ) + ", " + mapEntry.Length + " bytes, " + mapEntry.Description );

        if ( mapEntry.SubEntries.Entries.Count > 0 )
        {
          Debug.Log( " contains" );
          foreach ( var subEntry in mapEntry.SubEntries.Entries )
          {
            Debug.Log( " sub block from $" + subEntry.StartAddress.ToString( "X4" ) + " to $" + ( subEntry.StartAddress + subEntry.Length - 1 ).ToString( "X4" ) + ", " + subEntry.Length + " bytes, " + subEntry.Description );
          }
        }
      }
    }

  }
}
