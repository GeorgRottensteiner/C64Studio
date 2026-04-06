using RetroDevStudio.Dialogs;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace RetroDevStudio.Documents
{
  public partial class SearchResults : BaseDocument
  {
    private int       listResultsSortColumn = 1;

    private Project   m_ListProject = null;



    public SearchResults()
    {
      InitializeComponent();

      listResults.Columns.Add( "Line", 50 );
      listResults.Columns.Add( "File", 200 );
      listResults.Columns.Add( "Text", 400 );
      listResults.Columns.Add( "Add. Info", 120 );
      listResults.SortOrder           = DecentForms.SortOrder.NONE;
      listResults.ListViewItemSorter  = new GR.Forms.NumericListViewItemComparer();
    }



    public void ClearResults()
    {
      m_ListProject = Core.MainForm.CurrentProject;
      listResults.Items.Clear();
    }



    public void AddSearchResult( FormFindReplace.SearchLocation FoundInfo )
    {
      var    item = new DecentForms.ListControlItem();

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
      var   items = new DecentForms.ListControlItem[FoundInfos.Count];
      int   i = 0;

      foreach ( var foundInfo in FoundInfos )
      {
        items[i] = new DecentForms.ListControlItem();

        var item = items[i];

        item.Text = foundInfo.LineNumber.ToString();
        if ( foundInfo.FoundInDocument == null )
        {
          item.SubItems.Add( "Unknown location" );
          item.Tag = null;
        }
        else if ( foundInfo.FoundInDocument.DocumentFilename == null )
        {
          item.SubItems.Add( "(unnamed file)" );
          item.Tag = foundInfo.FoundInDocument;
        }
        else
        {
          item.SubItems.Add( foundInfo.FoundInDocument.DocumentFilename );
          item.Tag = foundInfo.FoundInDocument;
        }
        item.SubItems.Add( foundInfo.AdditionalInfo );
        item.SubItems.Add( foundInfo.FoundLine );
        ++i;
      }
      listResults.Items.AddRange( items );
    }



    private void listMessages_ColumnClick( DecentForms.ControlBase control )
    {
      if ( listResults.SortColumn == 0 )
      {
        listResults.ListViewItemSorter = new GR.Forms.NumericListViewItemComparer();
      }
      else
      {
        listResults.ListViewItemSorter = new GR.Forms.ListViewItemComparer();
      }
    }



    private void copyListToClipboardToolStripMenuItem_Click( object sender, EventArgs e )
    {
      StringBuilder   sb = new StringBuilder();

      foreach ( var item in listResults.Items )
      {
        sb.Append( item.SubItems[1].Text );
        sb.Append( ';' );
        sb.Append( item.SubItems[2].Text );
        sb.Append( ';' );
        sb.Append( item.SubItems[3].Text );
        sb.Append( ';' );
        sb.Append( item.SubItems[4].Text );
        sb.Append( ';' );
        sb.Append( item.SubItems[5].Text );
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
      listResults.SortOrder = (DecentForms.SortOrder)memIn.ReadInt32();
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
      bufferData.AppendI32( (int)listResults.SortOrder );
      return bufferData;
    }



    private void listResults_ItemActivate( DecentForms.ControlBase sender )
    {
      if ( listResults.SelectedIndices.Count == 0 )
      {
        return;
      }
      int     index = listResults.SelectedIndices[0];

      int     lineNumber = GR.Convert.ToI32( listResults.Items[index].Text );
      string  fileName = listResults.Items[index].SubItems[1].Text;
      string  fullPath = ( (DocumentInfo)listResults.Items[index].Tag ).FullPath;

      DocumentInfo    docToOpen = null;
      if ( !string.IsNullOrEmpty( fullPath ) )
      {
        fileName = fullPath;
        docToOpen = Core.Navigating.FindDocumentInfoByPath( fileName );
      }
      else
      {
        if ( fileName == "(unnamed file)" )
        {
          docToOpen = (DocumentInfo)listResults.Items[index].Tag;
        }
        else if ( ( m_ListProject != null )
        &&        ( !GR.Path.IsPathRooted( fileName ) ) )
        {
          fileName = GR.Path.Normalize( GR.Path.Append( m_ListProject.Settings.BasePath, fileName ), false );
          docToOpen = Core.Navigating.FindDocumentInfoByPath( fileName );
        }
      }
      Core.Navigating.OpenDocumentAndGotoLine( m_ListProject, docToOpen, lineNumber - 1 );
    }



    public void SearchComplete()
    {
      listResults.Sort();
    }



    public override void OnApplicationEvent( ApplicationEvent Event )
    {
      base.OnApplicationEvent( Event );
      switch ( Event.EventType )
      {
        case ApplicationEvent.Type.SETTINGS_LOADED:
          Core.Settings.DialogSettings.RestoreListViewColumns( "SearchResults", listResults );
          break;
        case ApplicationEvent.Type.SHUTTING_DOWN:
          Core.Settings.DialogSettings.StoreListViewColumns( "SearchResults", listResults );
          break;
      }
    }



  }
}
