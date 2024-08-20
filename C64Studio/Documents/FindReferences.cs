using RetroDevStudio.Types;
using RetroDevStudio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using RetroDevStudio.Dialogs;

namespace RetroDevStudio.Documents
{
  public partial class FindReferences : BaseDocument
  {
    private int       listResultsSortColumn = 1;

    private Project   m_ListProject = null;



    public FindReferences()
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
      //listResults.Items.Add( item );
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
      var docInfo = (DocumentInfo)listResults.Items[index].Tag;
      if ( ( docInfo != null )
      &&   ( !string.IsNullOrEmpty( docInfo.FullPath ) ) )
      {
        fileName = docInfo.FullPath;
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



    internal void UpdateReferences( Project Project, Types.ASM.FileInfo ASMInfo, GR.Collections.Set<int> References )
    {
      listResults.Items.Clear();

      foreach ( var reference in References )
      {
        AddReferenceItem( Project, ASMInfo, reference );
      }
    }



    private void AddReferenceItem( Project Project, Types.ASM.FileInfo ASMInfo, int GlobalLineIndex )
    {
      var item = new ListViewItem( GlobalLineIndex.ToString() );
      item.SubItems.Add( "" );
      item.SubItems.Add( "" );

      if ( ASMInfo.FindTrueLineSource( GlobalLineIndex, out string filename, out int localLineIndex, out Types.ASM.SourceInfo SourceInfo ) )
      {
        /*
        // do not list references inside macros - TODO would need to differ between real references, and inserted references
        if ( SourceInfo.Source == Types.ASM.SourceInfo.SourceInfoSource.MACRO )
        {
          return;
        }*/
        item.SubItems[0].Text = ( localLineIndex + 1 ).ToString();
        item.SubItems[1].Text = filename;

        string    lineContent = "";

        if ( Project != null )
        {
          var element = Project.GetElementByFilename( filename );
          if ( element != null )
          {
            if ( element.DocumentInfo != null )
            {
              item.Tag = element.DocumentInfo;
            }

            if ( element.Document != null )
            {
              if ( ( localLineIndex >= 0 )
              &&   ( localLineIndex < element.Document.SourceControl.LinesCount ) )
              {
                lineContent = element.Document.SourceControl.GetLine( localLineIndex ).Text;
              }
            }
            else
            {
              // fetch line from file
              string textFromElement = Core.Searching.GetDocumentInfoText( element.DocumentInfo );

              lineContent = FindLineInsideText( textFromElement, localLineIndex );
            }
          }
          else
          {
            // not a direct project member, read from file
            string textFromElement = Core.Searching.GetTextFromFile( filename );

            lineContent = FindLineInsideText( textFromElement, localLineIndex );
          }
        }
        else
        {
          var doc = Core.Navigating.FindDocumentInfoByPath( filename );
          if ( doc != null )
          {
            item.Tag = doc;
          }
          if ( ( doc != null )
          &&   ( doc.BaseDoc != null ) )
          {
            if ( ( localLineIndex >= 0 )
            &&   ( localLineIndex < doc.BaseDoc.SourceControl.LinesCount ) )
            {
              lineContent = doc.BaseDoc.SourceControl.GetLine( localLineIndex ).Text;
            }
          }
          else
          {
            // not a direct project member, read from file
            string textFromElement = Core.Searching.GetTextFromFile( filename );

            lineContent = FindLineInsideText( textFromElement, localLineIndex );
          }
        }

        item.SubItems[2].Text = lineContent.Replace( "\t", "  " );
      }
      listResults.Items.Add( item );
    }



    private string FindLineInsideText( string TextToSearch, int localLineIndex )
    {
      // find line number from text
      int numLines = 0;
      int curPos = 0;
      int lastPos = -1;

      while ( numLines <= localLineIndex )
      {
        lastPos = curPos;
        curPos = TextToSearch.IndexOf( '\n', curPos + 1 );
        ++numLines;
        if ( curPos == -1 )
        {
          // not found??
          return "";
        }
      }
      if ( ( curPos != -1 )
      &&   ( lastPos != -1 ) )
      {
        return TextToSearch.Substring( lastPos, curPos - lastPos - 1 );
      }
      return TextToSearch.Substring( lastPos );
    }



    public override void OnApplicationEvent( ApplicationEvent Event )
    {
      base.OnApplicationEvent( Event );
      switch ( Event.EventType )
      {
        case ApplicationEvent.Type.SETTINGS_LOADED:
          Core.Settings.DialogSettings.RestoreListViewColumns( "FindReferences", listResults );
          break;
        case ApplicationEvent.Type.SHUTTING_DOWN:
          Core.Settings.DialogSettings.StoreListViewColumns( "FindReferences", listResults );
          break;
      }
    }



  }
}
