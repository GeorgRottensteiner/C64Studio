using GR.Collections;
using RetroDevStudio.Dialogs;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;



namespace RetroDevStudio.Documents
{
  public partial class Bookmarks : BaseDocument
  {
    private int       listMessagesSortColumn = 1;



    public Bookmarks( StudioCore Core )
    {
      this.Core = Core;

      InitializeComponent();

      listMessages.Sorting = SortOrder.Ascending;
      listMessages.ListViewItemSorter = new BookmarkItemComparer( listMessagesSortColumn, listMessages.Sorting );

      Core.MainForm.ApplicationEvent += MainForm_ApplicationEvent;
    }



    private void MainForm_ApplicationEvent( Types.ApplicationEvent Event )
    {
      OnApplicationEvent( Event );
    }



    public void ClearMessages()
    {
      listMessages.Items.Clear();
    }



    private void listMessages_ItemActivate( object sender, EventArgs e )
    {
      if ( listMessages.SelectedItems.Count == 0 )
      {
        return;
      }
      JumpToFile( listMessages.SelectedItems[0] );
    }



    private void JumpToFile( ListViewItem Item )
    {
      var doc = (DocumentInfo)Item.Tag;

      int lineNumber = -1;
      int.TryParse( Item.SubItems[1].Text, out lineNumber );
      string fileToJumpTo = Item.SubItems[2].Text;

      Core.Navigating.OpenDocumentAndGotoLine( doc.Project, doc, lineNumber - 1 );
    }



    private void listMessages_ColumnClick( object sender, ColumnClickEventArgs e )
    {
      if ( e.Column != listMessagesSortColumn )
      {
        // Set the sort column to the new column.
        listMessagesSortColumn = e.Column;
        // Set the sort order to ascending by default.
        listMessages.Sorting = SortOrder.Ascending;
      }
      else
      {
        // Determine what the last sort order was and change it.
        if ( listMessages.Sorting == SortOrder.Ascending )
        {
          listMessages.Sorting = SortOrder.Descending;
        }
        else
        {
          listMessages.Sorting = SortOrder.Ascending;
        }
      }
      listMessages.ListViewItemSorter = new BookmarkItemComparer( listMessagesSortColumn, listMessages.Sorting );
      listMessages.Sort();
    }



    private void jumpToFileToolStripMenuItem_Click( object sender, EventArgs e )
    {
      if ( listMessages.SelectedItems.Count == 0 )
      {
        return;
      }
      JumpToFile( listMessages.SelectedItems[0] );
    }



    public override void ApplyDisplayDetails( GR.Memory.ByteBuffer Buffer )
    {
      GR.IO.MemoryReader    memIn = Buffer.MemoryReader();

      int     numColumns = memIn.ReadInt32();
      for ( int i = 0; i < numColumns; ++i )
      {
        if ( i < listMessages.Columns.Count )
        {
          listMessages.Columns[i].Width = memIn.ReadInt32();
        }
      }

      listMessagesSortColumn = memIn.ReadInt32();
      listMessages.Sorting = (SortOrder)memIn.ReadInt32();
    }



    public override GR.Memory.ByteBuffer DisplayDetails()
    {
      GR.Memory.ByteBuffer    bufferData = new GR.Memory.ByteBuffer();

      bufferData.AppendI32( listMessages.Columns.Count );
      for ( int i = 0; i < listMessages.Columns.Count; ++i )
      {
        bufferData.AppendI32( listMessages.Columns[i].Width );
      }

      bufferData.AppendI32( listMessagesSortColumn );
      bufferData.AppendI32( (int)listMessages.Sorting );
      return bufferData;
    }



    public void AddBookmark( int LineIndex, DocumentInfo Doc )
    {
      var item = new ListViewItem();

      item.ImageIndex = 0;
      item.Text       = "0";
      item.Tag        = Doc;

      item.SubItems.Add( ( LineIndex + 1 ).ToString() );
      item.SubItems.Add( Doc.FullPath );
      

      listMessages.Items.Add( item );
    }



    public void RemoveBookmark( int LineIndex, DocumentInfo Doc )
    {
      for ( int i = listMessages.Items.Count - 1; i >= 0; --i )
      {
        if ( ( ( (DocumentInfo)listMessages.Items[i].Tag ) == Doc )
        &&   ( GR.Convert.ToI32( listMessages.Items[i].SubItems[1].Text ) == LineIndex + 1 ) )
        {
          listMessages.Items.RemoveAt( i );
          return;
        }
      }
    }



    public void RemoveAllBookmarksForDocument( DocumentInfo Doc )
    {
      for ( int i = listMessages.Items.Count - 1; i >= 0; --i )
      {
        if ( ( (DocumentInfo)listMessages.Items[i].Tag ) == Doc )
        {
          listMessages.Items.RemoveAt( i );
        }
      }
    }



    public override void OnApplicationEvent( Types.ApplicationEvent Event )
    {
      switch ( Event.EventType )
      {
        case Types.ApplicationEvent.Type.DOCUMENT_INFO_CREATED:
          foreach ( var bm in Event.Doc.Bookmarks )
          {
            AddBookmark( bm, Event.Doc );
          }
          break;
        case Types.ApplicationEvent.Type.DOCUMENT_INFO_REMOVED:
          RemoveAllBookmarksForDocument( Event.Doc );
          break;
      }
    }



    internal void UpdateAllBookmarksForDocument( DocumentInfo Doc )
    {
      listMessages.BeginUpdate();
      RemoveAllBookmarksForDocument( Doc );
      foreach ( var bm in Doc.Bookmarks )
      {
        AddBookmark( bm, Doc );
      }
      listMessages.EndUpdate();
    }



    private void deleteBookmarkToolStripMenuItem_Click( object sender, EventArgs e )
    {
      var bookmarksToDelete = new List<GR.Generic.Tupel<DocumentInfo,int>>();

      foreach ( ListViewItem selectedItem in listMessages.SelectedItems )
      {
        var doc = (DocumentInfo)selectedItem.Tag;
        int lineNumber = -1;
        int.TryParse( selectedItem.SubItems[1].Text, out lineNumber );

        bookmarksToDelete.Add( new GR.Generic.Tupel<DocumentInfo, int>( doc, lineNumber ) );
      }

      foreach ( var bookmark in bookmarksToDelete )
      {
        bookmark.first.Bookmarks.Remove( bookmark.second - 1 );
        if ( bookmark.first.BaseDoc != null )
        {
          bookmark.first.BaseDoc.RemoveBookmark( bookmark.second - 1 );
        }
      }
    }



    private void deleteAllBookmarksToolStripMenuItem_Click( object sender, EventArgs e )
    {
      var docsToDeleteFrom = new Set<DocumentInfo>();

      foreach ( ListViewItem selectedItem in listMessages.SelectedItems )
      {
        var doc = (DocumentInfo)selectedItem.Tag;
        docsToDeleteFrom.Add( doc );
      }
      foreach ( var doc in docsToDeleteFrom )
      {
        doc.Bookmarks.Clear();
        if ( doc.BaseDoc != null )
        {
          doc.BaseDoc.ApplyFunction( Function.BOOKMARK_DELETE_ALL );
        }
      }
    }



  }
}
