using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace C64Studio
{
  public partial class CompileResult : BaseDocument
  {
    private int       listMessagesSortColumn = 1;

    private Project   m_ListProject = null;



    public CompileResult()
    {
      InitializeComponent();

      listMessages.Sorting = SortOrder.Ascending;
      listMessages.ListViewItemSorter = new CompileResultItemComparer( listMessagesSortColumn, listMessages.Sorting );
    }



    public void ClearMessages()
    {
      listMessages.Items.Clear();
    }



    delegate void UpdateFromMessagesCallback( Parser.ParserBase Parser, Project ParsedProject );

    public void UpdateFromMessages( Parser.ParserBase Parser, Project ParsedProject )
    {
      if ( InvokeRequired )
      {
        Invoke( new UpdateFromMessagesCallback( UpdateFromMessages ), new object[] { Parser, ParsedProject } );
        return;
      }

      m_ListProject = ParsedProject;
      ClearMessages();

      listMessages.BeginUpdate();
      SortOrder oldOrder = listMessages.Sorting;
      listMessages.Sorting = SortOrder.None;
      listMessages.ListViewItemSorter = null; 

      foreach ( var docInfos in Core.MainForm.DocumentInfos )
      {
        if ( ( docInfos.Type == ProjectElement.ElementType.ASM_SOURCE )
        &&   ( docInfos.BaseDoc != null ) )
        {
          SourceASMEx   source = (SourceASMEx)docInfos.BaseDoc;

          source.RemoveAllErrorMarkings();
          //source.UpdateFoldingBlocks();
        }
      }
      foreach ( System.Collections.Generic.KeyValuePair<int, Parser.ParserBase.ParseMessage> msg in Parser.Messages )
      {
        int lineIndex = msg.Key;
        Parser.ParserBase.ParseMessage message = msg.Value;

        /*
        if ( message.Type == C64Studio.Parser.ParserBase.ParseMessage.LineType.MESSAGE )
        {
          continue;
        }*/
        if ( Core.Settings.IgnoredWarnings.ContainsValue( message.Code ) )
        {
          // ignore warning
          continue;
        }

        string documentFile = "";
        int documentLine = -1;
        //Debug.Log( "a" );
        Parser.DocumentAndLineFromGlobalLine( lineIndex, out documentFile, out documentLine );
        //Debug.Log( "b" );
        if ( message.AlternativeFile == null )
        {
          message.AlternativeFile = documentFile;
          message.AlternativeLineIndex = documentLine;
        }

        if ( message.CharIndex != -1 )
        {
          if ( ParsedProject == null )
          {
            var sourceDocInfo = Core.MainForm.DetermineDocumentByFileName( documentFile );
            if ( ( sourceDocInfo != null )
            &&   ( sourceDocInfo.BaseDoc != null )
            &&   ( sourceDocInfo.Type == ProjectElement.ElementType.ASM_SOURCE ) )
            {
              var  sourceFile = (SourceASMEx)sourceDocInfo.BaseDoc;
              if ( sourceFile != null )
              {
                sourceFile.MarkTextAsError( documentLine, message.CharIndex, message.Length );
              }
            }
          }
          else
          {
            var  sourceElement = ParsedProject.GetElementByFilename( documentFile );
            if ( sourceElement != null )
            {
              var  sourceFile = (SourceASMEx)sourceElement.Document;
              if ( sourceFile != null )
              {
                sourceFile.MarkTextAsError( documentLine, message.CharIndex, message.Length );
              }
              else
              {
                // TODO - have no document to mark?
              }
            }
            else
            {
              // TODO - have no document to mark?
            }
          }
        }

        //dh.Log( "Error in " + lineIndex );

        ++documentLine;

        ListViewItem item = new ListViewItem();

        if ( message.Type == C64Studio.Parser.ParserBase.ParseMessage.LineType.ERROR )
        {
          item.ImageIndex = 0;
          item.Text = "0";
        }
        else if ( message.Type == C64Studio.Parser.ParserBase.ParseMessage.LineType.WARNING )
        {
          item.ImageIndex = 1;
          item.Text = "2";
        }
        else if ( message.Type == C64Studio.Parser.ParserBase.ParseMessage.LineType.SEVERE_WARNING )
        {
          item.ImageIndex = 2;
          item.Text = "1";
        }
        else
        {
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
          item.SubItems.Add( documentFile.ToString() );
        }
        else
        {
          item.SubItems.Add( "--" );
        }
        item.SubItems.Add( message.Message );
        item.Tag = message;

        //Debug.Log( "c" );
        listMessages.Items.Add( item );
        //Debug.Log( "d" );
        if ( message.ChildMessages != null )
        {
          foreach ( var childMessage in message.ChildMessages )
          {
            ListViewItem childItem = new ListViewItem();

            if ( childMessage.Type == C64Studio.Parser.ParserBase.ParseMessage.LineType.ERROR )
            {
              childItem.ImageIndex = 0;
              childItem.Text = "0";
            }
            else if ( childMessage.Type == C64Studio.Parser.ParserBase.ParseMessage.LineType.WARNING )
            {
              childItem.ImageIndex = 1;
              childItem.Text = "2";
            }
            else if ( childMessage.Type == C64Studio.Parser.ParserBase.ParseMessage.LineType.SEVERE_WARNING )
            {
              childItem.ImageIndex = 2;
              childItem.Text = "1";
            }
            else
            {
              childItem.Text = "3";
            }
            childItem.SubItems.Add( documentLine.ToString() );
            if ( childMessage.Code.ToString().Length >= 5 )
            {
              childItem.SubItems.Add( childMessage.Code.ToString().Substring( 0, 5 ) );
            }
            else
            {
              childItem.SubItems.Add( childMessage.Code.ToString() );
            }
            childItem.SubItems.Add( documentFile.ToString() );
            childItem.SubItems.Add( childMessage.Message );
            childItem.Tag = childMessage;

            //Debug.Log( "c" );
            listMessages.Items.Add( childItem );
          }
        }
      }
      //Debug.Log( "e" );
      listMessages.Sorting = oldOrder;
      listMessages.ListViewItemSorter = new CompileResultItemComparer( listMessagesSortColumn, listMessages.Sorting );
      //listMessages.Sort();
      //Debug.Log( "f" );
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
      Core.Navigating.OpenDocumentAndGotoLine( m_ListProject, fileToJumpTo, lineNumber - 1 );
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

        if ( message.Type == C64Studio.Parser.ParserBase.ParseMessage.LineType.WARNING )
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

        if ( message.Type == C64Studio.Parser.ParserBase.ParseMessage.LineType.WARNING )
        {
          Core.Settings.IgnoredWarnings.Add( message.Code );
        }
      }
    }



    private void manageWarningIgnoreListToolStripMenuItem_Click( object sender, EventArgs e )
    {
      Settings prefDlg = new Settings( Core, C64Studio.Settings.TabPage.ERRORS_WARNINGS );

      prefDlg.ShowDialog();
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


  }
}
