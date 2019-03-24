using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace C64Studio
{
  public partial class DebugWatch : BaseDocument
  {
    public List<WatchEntry>         m_WatchEntries = new List<WatchEntry>();
    private int                     m_ListWatchSortColumn = 0;

    public Project                  DebuggedProject = null;



    public DebugWatch()
    {
      InitializeComponent();
    }



    public void AddWatchEntry( WatchEntry Watch )
    {
      ListViewItem  item = new ListViewItem();

      item.Text = Watch.Name;
      if ( Watch.IndexedX )
      {
        item.Text += ",x";
      }
      if ( Watch.IndexedY )
      {
        item.Text += ",y";
      }
      item.SubItems.Add( TypeToString( Watch ) );
      if ( Watch.DisplayMemory )
      {
        item.SubItems.Add( "(unread)" );
        m_WatchEntries.Add( Watch );
      }
      else if ( !Watch.DisplayMemory )
      {
        if ( Watch.SizeInBytes == 1 )
        {
          GR.Memory.ByteBuffer data = new GR.Memory.ByteBuffer( Watch.Address.ToString( "x02" ) );
          Watch.CurrentValue = data;
          item.SubItems.Add( data.ToString() );
        }
        else if ( Watch.SizeInBytes == 2 )
        {
          GR.Memory.ByteBuffer data = new GR.Memory.ByteBuffer( Watch.Address.ToString( "x04" ) );
          Watch.CurrentValue = data;
          item.SubItems.Add( data.ToString() );
        }
      }
      else
      {
        item.SubItems.Add( "(unread)" );
        m_WatchEntries.Add( Watch );
      }
      item.Tag = Watch;

      listWatch.Items.Add( item );
    }



    public void ClearAllWatchEntries()
    {
      m_WatchEntries.Clear();
      listWatch.Items.Clear();
    }



    public void RemoveWatchEntry( WatchEntry Watch )
    {
      m_WatchEntries.Remove( Watch );
      foreach ( ListViewItem item in listWatch.Items )
      {
        WatchEntry watchEntry = (WatchEntry)item.Tag;

        if ( watchEntry == Watch )
        {
          listWatch.Items.Remove( item );
          if ( DebuggedProject != null )
          {
            DebuggedProject.SetModified();
          }
          return;
        }
      }
    }



    public void UpdateValue( string WatchVar, bool IndexedX, bool IndexedY, GR.Memory.ByteBuffer Data )
    {
      foreach ( ListViewItem item in listWatch.Items )
      {
        WatchEntry watchEntry = (WatchEntry)item.Tag;

        if ( ( watchEntry.Name == WatchVar )
        &&   ( watchEntry.IndexedX == IndexedX )
        &&   ( watchEntry.IndexedY == IndexedY ) )
        {
          watchEntry.CurrentValue = Data;

          if ( watchEntry.SizeInBytes != watchEntry.CurrentValue.Length )
          {
            Debug.Log( "Watch entry received different size than expected!" );
          }

          if ( watchEntry.CurrentValue.Length == 0 )
          {
            item.SubItems[2].Text = "(unread)";
            continue;
          }

          switch ( watchEntry.Type )
          {
            case WatchEntry.DisplayType.HEX:
              if ( watchEntry.DisplayMemory )
              {
                StringBuilder sb = new StringBuilder();

                sb.Append( "$" );
                if ( watchEntry.BigEndian )
                {
                  for ( int i = 0; i < Data.Length; ++i )
                  {
                    sb.Append( Data.ByteAt( i ).ToString( "x2" ) );
                    if ( i + 1 < Data.Length )
                    {
                      sb.Append( " " );
                    }
                  }
                }
                else
                {
                  for ( int i = 0; i < Data.Length; ++i )
                  {
                    sb.Append( Data.ByteAt( (int)Data.Length - 1 - i ).ToString( "x2" ) );
                    if ( i + 1 < Data.Length )
                    {
                      sb.Append( " " );
                    }
                  }
                }
                item.SubItems[2].Text = sb.ToString();
              }
              else
              {
                item.SubItems[2].Text = "$" + watchEntry.Address.ToString( "x4" );
              }
              break;
            case WatchEntry.DisplayType.DEZ:
              if ( !watchEntry.DisplayMemory )
              {
                item.SubItems[2].Text = watchEntry.Address.ToString();
              }
              else if ( watchEntry.SizeInBytes == 1 )
              {
                item.SubItems[2].Text = Data.ByteAt( 0 ).ToString();
              }
              else if ( watchEntry.BigEndian )
              {
                string totalText = "";
                for ( uint i = 0; i < Data.Length; ++i )
                {
                  totalText += Data.ByteAt( (int)i ).ToString( "d" ) + " ";
                }
                item.SubItems[2].Text = totalText;
              }
              else
              {
                string totalText = "";
                for ( uint i = 0; i < Data.Length; ++i )
                {
                  totalText += Data.ByteAt( (int)Data.Length - 1 - (int)i ).ToString( "d" ) + " ";
                }
                item.SubItems[2].Text = totalText;
              }
              break;
            case WatchEntry.DisplayType.BINARY:
              if ( !watchEntry.DisplayMemory )
              {
                item.SubItems[2].Text = "%" + Convert.ToString( watchEntry.Address, 2 );
              }
              else if ( watchEntry.SizeInBytes == 1 )
              {
                item.SubItems[2].Text = "%" + Convert.ToString( Data.ByteAt( 0 ), 2 );
              }
              else if ( watchEntry.SizeInBytes == 2 )
              {
                item.SubItems[2].Text = "%" + Convert.ToString( Data.UInt16At( 0 ), 2 );
              }
              else
              {
                item.SubItems[2].Text = Data.ToString();
              }
              break;
            default:
              if ( watchEntry.DisplayMemory )
              {
                item.SubItems[2].Text = Data.ByteAt( 0 ).ToString();
              }
              else
              {
                item.SubItems[2].Text = watchEntry.Address.ToString( "x4" );
              }
              break;
          }
        }
      }
    }



    public void UpdateValue( VICERemoteDebugger.RequestData WatchData, GR.Memory.ByteBuffer Data )
    {
      int     delta = WatchData.AdjustedStartAddress - WatchData.Parameter1;
      int     expectedSize = (int)Data.Length;
      if ( ( WatchData.MemDumpOffsetX )
      ||   ( WatchData.MemDumpOffsetY ) )
      {
        delta -= WatchData.AppliedOffset;
      }
      if ( WatchData.Parameter2 != -1 )
      {
        expectedSize = WatchData.Parameter2 - WatchData.Parameter1 + 1;
      }
      if ( delta > 0 )
      {
        // offset had to be adjusted due to VICE weird binary offset/end offset format
        if ( delta < Data.Length )
        {
          UpdateValue( WatchData.Info, WatchData.MemDumpOffsetX, WatchData.MemDumpOffsetY, Data.SubBuffer( delta, expectedSize ) );
        }
      }
      else if ( expectedSize < Data.Length )
      {
        UpdateValue( WatchData.Info, WatchData.MemDumpOffsetX, WatchData.MemDumpOffsetY, Data.SubBuffer( 0, expectedSize ) );
      }
      else
      {
        UpdateValue( WatchData.Info, WatchData.MemDumpOffsetX, WatchData.MemDumpOffsetY, Data );
      }
    }



    private void listWatch_KeyDown( object sender, KeyEventArgs e )
    {
      if ( e.KeyCode == Keys.Delete )
      {
        foreach ( ListViewItem item in listWatch.SelectedItems )
        {
          Core.MainForm.RemoveWatchEntry( (WatchEntry)item.Tag );
        }
      }
    }



    private void hexToolStripMenuItem_Click( object sender, EventArgs e )
    {
      foreach ( ListViewItem item in listWatch.SelectedItems )
      {
        WatchEntry    entry = (WatchEntry)item.Tag;

        entry.Type = WatchEntry.DisplayType.HEX;
        item.SubItems[1].Text = TypeToString( entry );
        UpdateValue( entry.Name, entry.IndexedX, entry.IndexedY, entry.CurrentValue );
      }
    }



    private void decimalToolStripMenuItem_Click( object sender, EventArgs e )
    {
      foreach ( ListViewItem item in listWatch.SelectedItems )
      {
        WatchEntry entry = (WatchEntry)item.Tag;

        entry.Type = WatchEntry.DisplayType.DEZ;
        item.SubItems[1].Text = TypeToString( entry );
        UpdateValue( entry.Name, entry.IndexedX, entry.IndexedY, entry.CurrentValue );
      }
    }



    private void binaryToolStripMenuItem_Click( object sender, EventArgs e )
    {
      foreach ( ListViewItem item in listWatch.SelectedItems )
      {
        WatchEntry entry = (WatchEntry)item.Tag;

        entry.Type = WatchEntry.DisplayType.BINARY;
        item.SubItems[1].Text = TypeToString( entry );
        UpdateValue( entry.Name, entry.IndexedX, entry.IndexedY, entry.CurrentValue );
      }
    }



    private void watchReadFromMemoryToolStripMenuItem_Click( object sender, EventArgs e )
    {
      watchReadFromMemoryToolStripMenuItem.Checked = !watchReadFromMemoryToolStripMenuItem.Checked;
      displayBoundsToolStripMenuItem.Visible = watchReadFromMemoryToolStripMenuItem.Checked;
      foreach ( ListViewItem item in listWatch.SelectedItems )
      {
        WatchEntry entry = (WatchEntry)item.Tag;

        entry.DisplayMemory = watchReadFromMemoryToolStripMenuItem.Checked;
        UpdateValue( entry.Name, entry.IndexedX, entry.IndexedY, entry.CurrentValue );
      }
      if ( watchReadFromMemoryToolStripMenuItem.Checked )
      {
        Core.Debugging.Debugger.RefreshRegistersAndWatches();
      }
    }



    private void removeEntryToolStripMenuItem_Click( object sender, EventArgs e )
    {
      foreach ( ListViewItem item in listWatch.SelectedItems )
      {
        Core.MainForm.RemoveWatchEntry( (WatchEntry)item.Tag );
      }
    }



    private void contextDebugItem_Opening( object sender, CancelEventArgs e )
    {
      if ( listWatch.SelectedItems.Count == 0 )
      {
        e.Cancel = true;
        return;
      }
      WatchEntry entry = (WatchEntry)listWatch.SelectedItems[0].Tag;

      watchReadFromMemoryToolStripMenuItem.Checked = entry.DisplayMemory;
      displayBoundsToolStripMenuItem.Visible = entry.DisplayMemory;
      toggleEndiannessToolStripMenuItem.Checked = !entry.BigEndian;

      moveDownToolStripMenuItem.Enabled = ( listWatch.SelectedIndices[0] + 1 < listWatch.Items.Count );
      moveUpToolStripMenuItem.Enabled = ( listWatch.SelectedIndices[0] > 0 );

      if ( displayBoundsToolStripMenuItem.Visible )
      {
        bytes1ToolStripMenuItem.Checked = ( entry.SizeInBytes == 1 );
        bytes2ToolStripMenuItem.Checked = ( entry.SizeInBytes == 2 );
        bytes4ToolStripMenuItem.Checked = ( entry.SizeInBytes == 4 );
        bytes8ToolStripMenuItem.Checked = ( entry.SizeInBytes == 8 );
        bytes16ToolStripMenuItem.Checked = ( entry.SizeInBytes == 16 );
        bytes32ToolStripMenuItem.Checked = ( entry.SizeInBytes == 32 );
      }
    }



    public void UpdateValues()
    {
      if ( InvokeRequired )
      {
        Invoke( new MainForm.ParameterLessCallback( UpdateValues ) );
        return;
      }

      foreach ( var watchEntry in m_WatchEntries )
      {
        ListViewItem itemToModify = null;
        foreach ( ListViewItem item in listWatch.Items )
        {
          WatchEntry oldWatchEntry = (WatchEntry)item.Tag;

          if ( oldWatchEntry == watchEntry )
          {
            itemToModify = item;
            break;
          }
        }
        if ( itemToModify == null )
        {
          itemToModify = new ListViewItem();

          itemToModify.Text = watchEntry.Name;
          if ( watchEntry.IndexedX )
          {
            itemToModify.Text += ",x";
          }
          if ( watchEntry.IndexedY )
          {
            itemToModify.Text += ",y";
          }
          itemToModify.SubItems.Add( TypeToString( watchEntry ) );
          if ( watchEntry.DisplayMemory )
          {
            itemToModify.SubItems.Add( "(unread)" );
          }
          else if ( !watchEntry.DisplayMemory )
          {
            if ( watchEntry.SizeInBytes == 1 )
            {
              GR.Memory.ByteBuffer data = new GR.Memory.ByteBuffer( watchEntry.Address.ToString( "x02" ) );
              watchEntry.CurrentValue = data;
              itemToModify.SubItems.Add( data.ToString() );
            }
            else if ( watchEntry.SizeInBytes == 2 )
            {
              GR.Memory.ByteBuffer data = new GR.Memory.ByteBuffer( watchEntry.Address.ToString( "x04" ) );
              watchEntry.CurrentValue = data;
              itemToModify.SubItems.Add( data.ToString() );
            }
          }
          else
          {
            itemToModify.SubItems.Add( "(unread)" );
          }
          itemToModify.Tag = watchEntry;
          listWatch.Items.Add( itemToModify );
        }

        if ( !watchEntry.DisplayMemory )
        {
          switch ( watchEntry.Type )
          {
            case WatchEntry.DisplayType.HEX:
              itemToModify.SubItems[2].Text = "$" + watchEntry.Address.ToString( "x4" );
              break;
            case WatchEntry.DisplayType.DEZ:
              itemToModify.SubItems[2].Text = watchEntry.Address.ToString();
              break;
            case WatchEntry.DisplayType.BINARY:
              itemToModify.SubItems[2].Text = "%" + Convert.ToString( watchEntry.Address, 2 );
              break;
            default:
              itemToModify.SubItems[2].Text = watchEntry.Address.ToString( "x4" );
              break;
          }
        }
      }
    }



    private string TypeToString( WatchEntry Entry )
    {
      string    result = Entry.Type.ToString();
      if ( !Entry.BigEndian )
      {
        result += ", LE";
      }
      return result;
    }



    public void ReseatWatches( Types.ASM.FileInfo ASMFileInfo )
    {
      List<WatchEntry>    entriesToRemove = new List<WatchEntry>();
      bool                hadChanges = false;
      foreach ( var watchEntry in m_WatchEntries )
      {
        if ( watchEntry.LiteralValue )
        {
          // is literal value, no need to reseat
          continue;
        }
        int addressOfEntry = ASMFileInfo.AddressFromToken( watchEntry.Name );
        if ( addressOfEntry == -1 )
        {
          entriesToRemove.Add( watchEntry );
          hadChanges = true;
        }
        else
        {
          if ( watchEntry.Address != addressOfEntry )
          {
            watchEntry.Address = addressOfEntry;
            hadChanges = true;
          }
        }
      }
      foreach ( var entryToRemove in entriesToRemove )
      {
        m_WatchEntries.Remove( entryToRemove );
      }
      if ( hadChanges )
      {
        Core.TaskManager.AddTask( new Tasks.TaskUpdateWatches() );
      }
    }



    private void listWatch_ColumnClick( object sender, ColumnClickEventArgs e )
    {
      if ( e.Column != m_ListWatchSortColumn )
      {
        // Set the sort column to the new column.
        m_ListWatchSortColumn = e.Column;
        // Set the sort order to ascending by default.
        listWatch.Sorting = SortOrder.Ascending;
      }
      else
      {
        // Determine what the last sort order was and change it.
        if ( listWatch.Sorting == SortOrder.Ascending )
        {
          listWatch.Sorting = SortOrder.Descending;
        }
        else
        {
          listWatch.Sorting = SortOrder.Ascending;
        }
      }
      listWatch.ListViewItemSorter = new GR.Forms.ListViewItemComparer( m_ListWatchSortColumn, listWatch.Sorting );
      listWatch.Sort();
    }



    private void bytes1ToolStripMenuItem_Click( object sender, EventArgs e )
    {
      foreach ( ListViewItem item in listWatch.SelectedItems )
      {
        WatchEntry entry = (WatchEntry)item.Tag;

        entry.SizeInBytes = 1;
        UpdateValue( entry.Name, entry.IndexedX, entry.IndexedY, entry.CurrentValue );

        if ( Core.MainForm.AppState == Types.StudioState.DEBUGGING_BROKEN )
        {
          Core.Debugging.Debugger.RefreshRegistersAndWatches();
        }
      }
    }



    private void bytes2ToolStripMenuItem_Click( object sender, EventArgs e )
    {
      foreach ( ListViewItem item in listWatch.SelectedItems )
      {
        WatchEntry entry = (WatchEntry)item.Tag;

        entry.SizeInBytes = 2;
        UpdateValue( entry.Name, entry.IndexedX, entry.IndexedY, entry.CurrentValue );
        if ( Core.MainForm.AppState == Types.StudioState.DEBUGGING_BROKEN )
        {
          Core.Debugging.Debugger.RefreshRegistersAndWatches();
        }
      }
    }



    private void bytes16ToolStripMenuItem_Click( object sender, EventArgs e )
    {
      foreach ( ListViewItem item in listWatch.SelectedItems )
      {
        WatchEntry entry = (WatchEntry)item.Tag;

        entry.SizeInBytes = 16;
        UpdateValue( entry.Name, entry.IndexedX, entry.IndexedY, entry.CurrentValue );
        if ( Core.MainForm.AppState == Types.StudioState.DEBUGGING_BROKEN )
        {
          Core.Debugging.Debugger.RefreshRegistersAndWatches();
        }
      }
    }



    private void bytes32ToolStripMenuItem_Click( object sender, EventArgs e )
    {
      foreach ( ListViewItem item in listWatch.SelectedItems )
      {
        WatchEntry entry = (WatchEntry)item.Tag;

        entry.SizeInBytes = 32;
        UpdateValue( entry.Name, entry.IndexedX, entry.IndexedY, entry.CurrentValue );
        if ( Core.MainForm.AppState == Types.StudioState.DEBUGGING_BROKEN )
        {
          Core.Debugging.Debugger.RefreshRegistersAndWatches();
        }
      }
    }



    private void bytes8ToolStripMenuItem_Click( object sender, EventArgs e )
    {
      foreach ( ListViewItem item in listWatch.SelectedItems )
      {
        WatchEntry entry = (WatchEntry)item.Tag;

        entry.SizeInBytes = 8;
        UpdateValue( entry.Name, entry.IndexedX, entry.IndexedY, entry.CurrentValue );
        if ( Core.MainForm.AppState == Types.StudioState.DEBUGGING_BROKEN )
        {
          Core.Debugging.Debugger.RefreshRegistersAndWatches();
        }
      }

    }


    public override void ApplyDisplayDetails( GR.Memory.ByteBuffer Buffer )
    {
      GR.IO.MemoryReader binReader = Buffer.MemoryReader();

      foreach ( ColumnHeader column in listWatch.Columns )
      {
        int     width = binReader.ReadInt32();
        if ( width != 0 )
        {
          column.Width = width;
        }
      }
    }



    public override GR.Memory.ByteBuffer DisplayDetails()
    {
      GR.Memory.ByteBuffer      displayData = new GR.Memory.ByteBuffer();

      foreach ( ColumnHeader column in listWatch.Columns )
      {
        displayData.AppendI32( column.Width );
      }
      return displayData;
    }



    private void bytes4ToolStripMenuItem_Click( object sender, EventArgs e )
    {
      foreach ( ListViewItem item in listWatch.SelectedItems )
      {
        WatchEntry entry = (WatchEntry)item.Tag;

        entry.SizeInBytes = 4;
        UpdateValue( entry.Name, entry.IndexedX, entry.IndexedY, entry.CurrentValue );
        if ( Core.MainForm.AppState == Types.StudioState.DEBUGGING_BROKEN )
        {
          Core.Debugging.Debugger.RefreshRegistersAndWatches();
        }
      }
    }



    private void moveUpToolStripMenuItem_Click( object sender, EventArgs e )
    {
      if ( listWatch.SelectedItems.Count == 0 )
      {
        return;
      }
      if ( listWatch.SelectedIndices[0] == 0 )
      {
        return;
      }
      int     index = listWatch.SelectedIndices[0];

      WatchEntry entry = (WatchEntry)listWatch.SelectedItems[0].Tag;
      var     item = listWatch.Items[index];

      listWatch.Items.RemoveAt( index );
      listWatch.Items.Insert( index - 1, item );

      // also exchange in project settings!
      var oldItem = m_WatchEntries[index];
      m_WatchEntries.RemoveAt( index );
      m_WatchEntries.Insert( index - 1, oldItem );
    }



    private void moveDownToolStripMenuItem_Click( object sender, EventArgs e )
    {
      if ( listWatch.SelectedItems.Count == 0 )
      {
        return;
      }
      if ( listWatch.SelectedIndices[0] + 1 == listWatch.Items.Count )
      {
        return;
      }
      int     index = listWatch.SelectedIndices[0];

      WatchEntry entry = (WatchEntry)listWatch.SelectedItems[0].Tag;
      var     item = listWatch.Items[index];

      listWatch.Items.RemoveAt( index );
      listWatch.Items.Insert( index + 1, item );

      // also exchange in project settings!
      var oldItem = m_WatchEntries[index];
      m_WatchEntries.RemoveAt( index );
      m_WatchEntries.Insert( index + 1, oldItem );

      
    }



    private void toggleEndiannessToolStripMenuItem_Click( object sender, EventArgs e )
    {
      toggleEndiannessToolStripMenuItem.Checked = !toggleEndiannessToolStripMenuItem.Checked;
      foreach ( ListViewItem item in listWatch.SelectedItems )
      {
        WatchEntry entry = (WatchEntry)item.Tag;

        entry.BigEndian = !toggleEndiannessToolStripMenuItem.Checked;
        item.SubItems[1].Text = TypeToString( entry );

        UpdateValue( entry.Name, entry.IndexedX, entry.IndexedY, entry.CurrentValue );
      }
    }



  }
}
