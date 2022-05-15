using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace RetroDevStudio
{
  public partial class SearchResults : BaseDocument
  {
    private int       listResultsSortColumn = 1;

    private Project   m_ListProject = null;



    public SearchResults()
    {
      InitializeComponent();

      listResults.Sorting = SortOrder.Ascending;
      listResults.ListViewItemSorter = new GR.Forms.ListViewItemComparer( listResultsSortColumn, listResults.Sorting );
    }



    public void ClearResults()
    {
      m_ListProject = Core.MainForm.CurrentProject;
      listResults.Items.Clear();
    }



    public void AddSearchResult( FormFindReplace.SearchLocation FoundInfo )
    {
      ListViewItem    item = new ListViewItem();

      item.Text = FoundInfo.LineNumber.ToString();
      if ( FoundInfo.FoundInDocument == null )
      {
        item.SubItems.Add( "Unknown location" );
        item.Tag = null;
      }
      else
      {
        item.SubItems.Add( FoundInfo.FoundInDocument.DocumentFilename );
        item.Tag = FoundInfo.FoundInDocument;
      }
      item.SubItems.Add( FoundInfo.FoundLine );

      listResults.Items.Add( item );
    }



    public void AddSearchResults( List<FormFindReplace.SearchLocation> FoundInfos )
    {
      ListViewItem[]    items = new ListViewItem[FoundInfos.Count];
      int               i = 0;

      foreach ( var foundInfo in FoundInfos )
      {
        items[i] = new ListViewItem();

        ListViewItem    item = items[i];

        item.Text = foundInfo.LineNumber.ToString();
        if ( foundInfo.FoundInDocument == null )
        {
          item.SubItems.Add( "Unknown location" );
          item.Tag = null;
        }
        else
        {
          item.SubItems.Add( foundInfo.FoundInDocument.DocumentFilename );
          item.Tag = foundInfo.FoundInDocument;
        }
        item.SubItems.Add( foundInfo.FoundLine );
        ++i;
      }
      listResults.Items.AddRange( items );
    }



    private void listMessages_ColumnClick( object sender, ColumnClickEventArgs e )
    {
      if ( e.Column != listResultsSortColumn )
      {
        // Set the sort column to the new column.
        listResultsSortColumn = e.Column;
        // Set the sort order to ascending by default.
        listResults.Sorting = SortOrder.Ascending;
      }
      else
      {
        // Determine what the last sort order was and change it.
        if ( listResults.Sorting == SortOrder.Ascending )
        {
          listResults.Sorting = SortOrder.Descending;
        }
        else
        {
          listResults.Sorting = SortOrder.Ascending;
        }
      }
      if ( e.Column == 0 )
      {
        listResults.ListViewItemSorter = new GR.Forms.NumericListViewItemComparer( listResultsSortColumn, listResults.Sorting );
      }
      else
      {
        listResults.ListViewItemSorter = new GR.Forms.ListViewItemComparer( listResultsSortColumn, listResults.Sorting );
      }
      listResults.Sort();
    }



    private void copyListToClipboardToolStripMenuItem_Click( object sender, EventArgs e )
    {
      StringBuilder   sb = new StringBuilder();

      foreach ( ListViewItem item in listResults.Items )
      {
        sb.Append( item.SubItems[1].Text );
        sb.Append( ';' );
        sb.Append( item.SubItems[2].Text );
        sb.Append( ';' );
        sb.Append( item.SubItems[3].Text );
        sb.Append( ';' );
        sb.Append( item.SubItems[4].Text );
        sb.AppendLine();
      }
      System.Windows.Forms.Clipboard.SetText( sb.ToString() );
    }



    public override void ApplyDisplayDetails( GR.Memory.ByteBuffer Buffer )
    {
      GR.IO.MemoryReader    memIn = Buffer.MemoryReader();

      int     numColumns = memIn.ReadInt32();
      for ( int i = 0; i < numColumns; ++i )
      {
        if ( i < listResults.Columns.Count )
        {
          listResults.Columns[i].Width = memIn.ReadInt32();
        }
      }

      listResultsSortColumn = memIn.ReadInt32();
      listResults.Sorting = (SortOrder)memIn.ReadInt32();
    }



    public override GR.Memory.ByteBuffer DisplayDetails()
    {
      GR.Memory.ByteBuffer    bufferData = new GR.Memory.ByteBuffer();

      bufferData.AppendI32( listResults.Columns.Count );
      for ( int i = 0; i < listResults.Columns.Count; ++i )
      {
        bufferData.AppendI32( listResults.Columns[i].Width );
      }

      bufferData.AppendI32( listResultsSortColumn );
      bufferData.AppendI32( (int)listResults.Sorting );
      return bufferData;
    }



    private void listResults_ItemActivate( object sender, EventArgs e )
    {
      if ( listResults.SelectedIndices.Count == 0 )
      {
        return;
      }
      int     index = listResults.SelectedIndices[0];

      int     lineNumber = GR.Convert.ToI32( listResults.Items[index].Text );
      string  fileName = listResults.Items[index].SubItems[1].Text;
      string  fullPath = ( (DocumentInfo)listResults.Items[index].Tag ).FullPath;
      if ( !string.IsNullOrEmpty( fullPath ) )
      {
        fileName = fullPath;
      }
      else
      {
        if ( ( m_ListProject != null )
        &&   ( !System.IO.Path.IsPathRooted( fileName ) ) )
        {
          fileName = GR.Path.Normalize( GR.Path.Append( m_ListProject.Settings.BasePath, fileName ), false );
        }
      }
      Core.Navigating.OpenDocumentAndGotoLine( m_ListProject, Core.Navigating.FindDocumentInfoByPath( fileName ), lineNumber - 1 );
    }



    public void SearchComplete()
    {
      listResults.Sort();
    }


  }
}
