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
    public LinkedList<WatchEntry>   m_WatchEntries = new LinkedList<WatchEntry>();
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
      item.SubItems.Add( Watch.Type.ToString() );
      if ( Watch.DisplayMemory )
      {
        item.SubItems.Add( "(unread)" );
        m_WatchEntries.AddLast( Watch );
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
        m_WatchEntries.AddLast( Watch );
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
          switch ( watchEntry.Type )
          {
            case WatchEntry.DisplayType.HEX:
              if ( watchEntry.DisplayMemory )
              {
                StringBuilder sb = new StringBuilder();

                sb.Append( "$" );
                for ( int i = 0; i < Data.Length; ++i )
                {
                  sb.Append( Data.ByteAt( i ).ToString( "x2" ) );
                  if ( i + 1 < Data.Length )
                  {
                    sb.Append( " " );
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
              else
              {
                string totalText = "";
                for ( uint i = 0; i < Data.Length; ++i )
                {
                  totalText += Data.ByteAt( (int)i ).ToString( "d" ) + " ";
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



    public void UpdateValue( RemoteDebugger.RequestData WatchData, GR.Memory.ByteBuffer Data )
    {
      UpdateValue( WatchData.Info, WatchData.MemDumpOffsetX, WatchData.MemDumpOffsetY, Data );
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
        item.SubItems[1].Text = entry.Type.ToString();
        UpdateValue( entry.Name, entry.IndexedX, entry.IndexedY, entry.CurrentValue );
      }
    }



    private void decimalToolStripMenuItem_Click( object sender, EventArgs e )
    {
      foreach ( ListViewItem item in listWatch.SelectedItems )
      {
        WatchEntry entry = (WatchEntry)item.Tag;

        entry.Type = WatchEntry.DisplayType.DEZ;
        item.SubItems[1].Text = entry.Type.ToString();
        UpdateValue( entry.Name, entry.IndexedX, entry.IndexedY, entry.CurrentValue );
      }
    }



    private void binaryToolStripMenuItem_Click( object sender, EventArgs e )
    {
      foreach ( ListViewItem item in listWatch.SelectedItems )
      {
        WatchEntry entry = (WatchEntry)item.Tag;

        entry.Type = WatchEntry.DisplayType.BINARY;
        item.SubItems[1].Text = entry.Type.ToString();
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
        Core.Debugging.Debugger.QueueRequest( RemoteDebugger.Request.REFRESH_VALUES );
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



    public void ReseatWatches( Types.ASM.FileInfo ASMFileInfo )
    {
      restart:;
      foreach ( ListViewItem item in listWatch.Items )
      {
        WatchEntry watchEntry = (WatchEntry)item.Tag;

        if ( watchEntry.LiteralValue )
        {
          // is literal value, no need to reseat
          continue;
        }
        int addressOfEntry = ASMFileInfo.AddressFromToken( watchEntry.Name );
        if ( addressOfEntry == -1 )
        {
          m_WatchEntries.Remove( watchEntry );
          listWatch.Items.Remove( item );
          goto restart;
        }
        else
        {
          if ( watchEntry.Address != addressOfEntry )
          {
            watchEntry.Address = addressOfEntry;
            if ( !watchEntry.DisplayMemory )
            {
              switch ( watchEntry.Type )
              {
                case WatchEntry.DisplayType.HEX:
                  item.SubItems[2].Text = "$" + watchEntry.Address.ToString( "x4" );
                  break;
                case WatchEntry.DisplayType.DEZ:
                  item.SubItems[2].Text = watchEntry.Address.ToString();
                  break;
                case WatchEntry.DisplayType.BINARY:
                  item.SubItems[2].Text = "%" + Convert.ToString( watchEntry.Address, 2 );
                  break;
                default:
                  item.SubItems[2].Text = watchEntry.Address.ToString( "x4" );
                  break;
              }
            }
          }
        }
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
          Core.Debugging.Debugger.QueueRequest( RemoteDebugger.Request.REFRESH_VALUES );
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
          Core.Debugging.Debugger.QueueRequest( RemoteDebugger.Request.REFRESH_VALUES );
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
          Core.Debugging.Debugger.QueueRequest( RemoteDebugger.Request.REFRESH_VALUES );
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
          Core.Debugging.Debugger.QueueRequest( RemoteDebugger.Request.REFRESH_VALUES );
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
          Core.Debugging.Debugger.QueueRequest( RemoteDebugger.Request.REFRESH_VALUES );
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
          Core.Debugging.Debugger.QueueRequest( RemoteDebugger.Request.REFRESH_VALUES );
        }
      }
    }


  }
}
