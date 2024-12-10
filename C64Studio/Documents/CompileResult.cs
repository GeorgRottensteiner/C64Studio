using RetroDevStudio.Controls;
using RetroDevStudio.Dialogs;
using RetroDevStudio.Types;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using static RetroDevStudio.Parser.ParserBase;



namespace RetroDevStudio.Documents
{
  public partial class CompileResult : BaseDocument
  {
    private int       listMessagesSortColumn = 1;

    private Project   m_ListProject = null;



    public CompileResult( StudioCore Core ) 
    {
      InitializeComponent();

      listMessages.Sorting = SortOrder.Ascending;
      listMessages.ListViewItemSorter = new CompileResultItemComparer( listMessagesSortColumn, listMessages.Sorting );

      this.Core = Core;
    }



    public void ClearMessages()
    {
      listMessages.Items.Clear();
    }



    delegate void UpdateFromMessagesCallback( Types.ASM.FileInfo ASMFileInfo, Project ParsedProject );



    public void UpdateFromMessages( Types.ASM.FileInfo ASMFileInfo, Project ParsedProject )
    {
      if ( InvokeRequired )
      {
        Invoke( new UpdateFromMessagesCallback( UpdateFromMessages ), new object[] { ASMFileInfo, ParsedProject } );
        return;
      }

      m_ListProject = ParsedProject;
      ClearMessages();
      if ( ASMFileInfo == null )
      {
        return;
      }

      listMessages.BeginUpdate();
      SortOrder oldOrder = listMessages.Sorting;
      listMessages.Sorting = SortOrder.None;
      listMessages.ListViewItemSorter = null; 

      foreach ( var msg in ASMFileInfo.Messages )
      {
        int lineIndex = msg.Key;
        Parser.ParserBase.ParseMessage message = msg.Value;

        var msgType = message.Type;

        if ( Core.Settings.IgnoredWarnings.ContainsValue( message.Code ) )
        {
          // ignore warning
          continue;
        }

        string documentFile = "";
        int documentLine = -1;

        ASMFileInfo.FindTrueLineSource( lineIndex, out documentFile, out documentLine );
        if ( message.AlternativeFile == null )
        {
          message.AlternativeFile = documentFile;
          message.AlternativeLineIndex = documentLine;
        }

        ++documentLine;

        ListViewItem item = new ListViewItem();

        if ( msgType == RetroDevStudio.Parser.ParserBase.ParseMessage.LineType.ERROR )
        {
          item.ImageIndex = 0;
          item.Text = "0";
        }
        else if ( msgType == RetroDevStudio.Parser.ParserBase.ParseMessage.LineType.WARNING )
        {
          item.ImageIndex = 1;
          item.Text = "2";
        }
        else if ( msgType == RetroDevStudio.Parser.ParserBase.ParseMessage.LineType.SEVERE_WARNING )
        {
          item.ImageIndex = 2;
          item.Text = "1";
        }
        else
        {
          item.ImageIndex = 3;
          item.Text = "3";
        }
        item.SubItems.Add( documentLine.ToString() );

        string    messageCode = message.Code.ToString();
        if ( messageCode.Length >= 5 )
        {
          item.SubItems.Add( messageCode.Substring( 0, 5 ) );
        }
        else
        {
          item.SubItems.Add( messageCode );
        }
        if ( documentFile != null )
        {
          item.SubItems.Add( new CSListViewSubItem( documentFile ) { Trimming = StringTrimming.EllipsisPath } );
        }
        else
        {
          item.SubItems.Add( "--" );
        }
        item.SubItems.Add( new CSListViewSubItem( message.Message ) { Trimming = StringTrimming.EllipsisPath } );
        item.Tag = message;

        listMessages.Items.Add( item );
        if ( message.ChildMessages != null )
        {
          foreach ( var childMessage in message.ChildMessages )
          {
            ListViewItem childItem = new ListViewItem();

            if ( childMessage.Type == RetroDevStudio.Parser.ParserBase.ParseMessage.LineType.ERROR )
            {
              childItem.ImageIndex = 0;
              childItem.Text = "0";
            }
            else if ( childMessage.Type == RetroDevStudio.Parser.ParserBase.ParseMessage.LineType.WARNING )
            {
              childItem.ImageIndex = 1;
              childItem.Text = "2";
            }
            else if ( childMessage.Type == RetroDevStudio.Parser.ParserBase.ParseMessage.LineType.SEVERE_WARNING )
            {
              childItem.ImageIndex = 2;
              childItem.Text = "1";
            }
            else
            {
              childItem.Text = "3";
            }

            int    messageDocLine = documentLine;
            if ( childMessage.AlternativeLineIndex != -1 )
            {
              messageDocLine = childMessage.AlternativeLineIndex;
            }
            string  messageDoc = documentFile;
            if ( !string.IsNullOrEmpty( childMessage.AlternativeFile ) )
            {
              messageDoc = childMessage.AlternativeFile;
            }
            childItem.SubItems.Add( messageDocLine.ToString() );
            if ( childMessage.Code.ToString().Length >= 5 )
            {
              childItem.SubItems.Add( childMessage.Code.ToString().Substring( 0, 5 ) );
            }
            else
            {
              childItem.SubItems.Add( childMessage.Code.ToString() );
            }
            childItem.SubItems.Add( new CSListViewSubItem( messageDoc ) { Trimming = StringTrimming.EllipsisPath } );
            childItem.SubItems.Add( new CSListViewSubItem( childMessage.Message ) { Trimming = StringTrimming.EllipsisPath } );
            childItem.Tag = childMessage;

            listMessages.Items.Add( childItem );
          }
        }
      }
      listMessages.Sorting = oldOrder;
      listMessages.ListViewItemSorter = new CompileResultItemComparer( listMessagesSortColumn, listMessages.Sorting );
      listMessages.EndUpdate();
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
      Parser.ParserBase.ParseMessage Message = (Parser.ParserBase.ParseMessage)Item.Tag;

      int lineNumber = -1;
      int.TryParse( Item.SubItems[1].Text, out lineNumber );
      string fileToJumpTo = Item.SubItems[3].Text;

      if ( Message.AlternativeFile != null )
      {
        fileToJumpTo = Message.AlternativeFile;
        lineNumber   = Message.AlternativeLineIndex + 1;
      }

      // double click override last shown message index
      int   index = 0;
      foreach ( var message in Core.Navigating.CompileMessages )
      {
        if ( message.Value == Message )
        {
          Core.Navigating.LastShownMessageIndex = index;
          break;
        }
        ++index;
      }
      Core.Navigating.OpenDocumentAndGotoLine( m_ListProject, Core.Navigating.FindDocumentInfoByPath( fileToJumpTo ), lineNumber - 1 );
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
      listMessages.ListViewItemSorter = new CompileResultItemComparer( listMessagesSortColumn, listMessages.Sorting );
      listMessages.Sort();
    }



    private void contextCompilerMessage_Opening( object sender, CancelEventArgs e )
    {
      copyListToClipboardToolStripMenuItem.Enabled = ( listMessages.Items.Count != 0 );
      if ( listMessages.SelectedItems.Count == 0 )
      {
        ignoreWarningToolStripMenuItem.Enabled = false;
        jumpToFileToolStripMenuItem.Enabled = false;
        return;
      }
      jumpToFileToolStripMenuItem.Enabled = true;
      copyListToClipboardToolStripMenuItem.Enabled = true;
      bool hasWarnings = false;
      foreach ( ListViewItem item in listMessages.SelectedItems )
      {
        Parser.ParserBase.ParseMessage message = (Parser.ParserBase.ParseMessage)item.Tag;

        if ( message.Type == RetroDevStudio.Parser.ParserBase.ParseMessage.LineType.WARNING )
        {
          hasWarnings = true;
          break;
        }
      }
      ignoreWarningToolStripMenuItem.Enabled = hasWarnings;
    }



    private void jumpToFileToolStripMenuItem_Click( object sender, EventArgs e )
    {
      if ( listMessages.SelectedItems.Count == 0 )
      {
        return;
      }
      JumpToFile( listMessages.SelectedItems[0] );
    }



    private void ignoreWarningToolStripMenuItem_Click( object sender, EventArgs e )
    {
      foreach ( ListViewItem item in listMessages.SelectedItems )
      {
        Parser.ParserBase.ParseMessage message = (Parser.ParserBase.ParseMessage)item.Tag;

        if ( message.Type == RetroDevStudio.Parser.ParserBase.ParseMessage.LineType.WARNING )
        {
          Core.Settings.IgnoredWarnings.Add( message.Code );
        }
      }
    }



    private void manageWarningIgnoreListToolStripMenuItem_Click( object sender, EventArgs e )
    {
      var prefDlg = new FormPreferences( Core, "Warnings" );

      prefDlg.ShowDialog();

      Core.MainForm.SaveSettings();
    }



    private void copyListToClipboardToolStripMenuItem_Click( object sender, EventArgs e )
    {
      StringBuilder   sb = new StringBuilder();

      foreach ( ListViewItem item in listMessages.Items )
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



    public override void OnApplicationEvent( ApplicationEvent Event )
    {
      base.OnApplicationEvent( Event );
      switch ( Event.EventType )
      {
        case ApplicationEvent.Type.SETTINGS_LOADED:
          Core.Settings.DialogSettings.RestoreListViewColumns( "CompileResult", listMessages );
          break;
        case ApplicationEvent.Type.SHUTTING_DOWN:
          Core.Settings.DialogSettings.StoreListViewColumns( "CompileResult", listMessages );
          break;
      }
    }



    public void SelectMessage( Parser.ParserBase.ParseMessage Message )
    {
      listMessages.SelectedItems.Clear();
      foreach ( ListViewItem item in listMessages.Items )
      {
        if ( item.Tag == Message )
        {
          item.Selected = true;
          listMessages.EnsureVisible( item.Index );
        }
      }
    }



  }
}
