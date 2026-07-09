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
    private Project   m_ListProject = null;



    public CompileResult( StudioCore Core ) 
    {
      InitializeComponent();

      listMessages.Columns.Add( "", 20 );
      listMessages.Columns[0].Sizable = false;
      listMessages.Columns.Add( "Line", 50 );
      listMessages.Columns.Add( "Code", 49 );
      listMessages.Columns.Add( "File", 200 );
      listMessages.Columns[3].Format = DecentForms.TextFormat.PATH_ELLIPSIS;
      listMessages.Columns.Add( "Message", 400 );
      listMessages.Columns[4].Format = DecentForms.TextFormat.PATH_ELLIPSIS;

      listMessages.ImageList = new DecentForms.ImageList();
      foreach ( System.Drawing.Image image in imageListCompileResult.Images )
      {
        listMessages.ImageList.Add( image );
      }

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
      var oldOrder = listMessages.SortOrder;
      var oldColumn = listMessages.SortColumn;
      listMessages.SortOrder = DecentForms.SortOrder.NONE;
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

        var item = new DecentForms.ListControlItem();

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
          item.SubItems.Add( new DecentForms.ListControlSubItem( documentFile ) );
        }
        else
        {
          item.SubItems.Add( "--" );
        }
        item.SubItems.Add( new DecentForms.ListControlSubItem( message.Message ) );
        item.Tag = message;

        listMessages.Items.Add( item );
        if ( message.ChildMessages != null )
        {
          foreach ( var childMessage in message.ChildMessages )
          {
            var childItem = new DecentForms.ListControlItem();

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
            childItem.SubItems.Add( new DecentForms.ListControlSubItem( messageDoc ) );
            childItem.SubItems.Add( new DecentForms.ListControlSubItem( childMessage.Message ) );
            childItem.Tag = childMessage;

            listMessages.Items.Add( childItem );
          }
        }
      }
      listMessages.SortOrder  = oldOrder;
      listMessages.SortColumn = oldColumn;
      listMessages.ListViewItemSorter = new CompileResultItemComparer( oldColumn, oldOrder );
      listMessages.EndUpdate();
    }



    private void listMessages_ItemActivate( DecentForms.ControlBase sender )
    {
      if ( listMessages.SelectedItems.Count == 0 )
      {
        return;
      }
      JumpToFile( listMessages.SelectedItems[0] );
    }



    private void JumpToFile( DecentForms.ListControlItem Item )
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
      foreach ( var item in listMessages.SelectedItems )
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
      foreach ( var item in listMessages.SelectedItems )
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

      foreach ( var item in listMessages.Items )
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
        case ApplicationEvent.Type.LINES_INSERTED:
          {
            foreach ( var item in listMessages.Items )
            {
              Parser.ParserBase.ParseMessage message = (Parser.ParserBase.ParseMessage)item.Tag;

              string  docFile = item.SubItems[3].Text;
              int     localLineIndex = GR.Convert.ToI32( item.SubItems[1].Text ) - 1;
              if ( docFile == Event.Doc.FullPath )
              {
                if ( localLineIndex >= Event.LineIndex )
                {
                  localLineIndex += Event.LineCount;
                  item.SubItems[1].Text = ( localLineIndex + 1 ).ToString();
                }
                if ( message.AlternativeLineIndex >= Event.LineIndex )
                {
                  message.AlternativeLineIndex += Event.LineCount;
                }
              }
            }
          }
          break;
        case ApplicationEvent.Type.LINES_REMOVED:
          {
            var itemsToRemove = new System.Collections.Generic.List<DecentForms.ListControlItem>();
            foreach ( var item in listMessages.Items )
            {
              Parser.ParserBase.ParseMessage message = (Parser.ParserBase.ParseMessage)item.Tag;

              string  docFile = item.SubItems[3].Text;
              int     localLineIndex = GR.Convert.ToI32( item.SubItems[1].Text ) - 1;
              if ( docFile == Event.Doc.FullPath )
              {
                if ( ( localLineIndex >= Event.LineIndex )
                &&   ( localLineIndex < Event.LineIndex + Event.LineCount ) ) 
                {
                  itemsToRemove.Add( item );
                }
                else if ( localLineIndex >= Event.LineIndex + Event.LineCount )
                {
                  localLineIndex -= Event.LineCount;
                  item.SubItems[1].Text = ( localLineIndex + 1 ).ToString();
                }
              }
              if ( message.AlternativeFile == Event.Doc.FullPath )
              {
                if ( ( message.AlternativeLineIndex >= Event.LineIndex )
                &&   ( message.AlternativeLineIndex < Event.LineIndex + Event.LineCount ) ) 
                {
                  itemsToRemove.Add( item );
                }
                else if ( message.AlternativeLineIndex >= Event.LineIndex + Event.LineCount )
                {
                  message.AlternativeLineIndex -= Event.LineCount;
                }
              }
            }
            foreach ( var item in itemsToRemove )
            {
              listMessages.Items.Remove( item );
            }
          }
          break;
      }
    }



    public void SelectMessage( Parser.ParserBase.ParseMessage Message )
    {
      listMessages.SelectedItems.Clear();
      foreach ( var item in listMessages.Items )
      {
        if ( item.Tag == Message )
        {
          item.Selected = true;
          listMessages.EnsureVisible( item.Index );
        }
      }
    }



    private void listMessages_ColumnClicked( DecentForms.ControlBase Sender )
    {
      listMessages.ListViewItemSorter = new CompileResultItemComparer( listMessages.SelectedColumn, listMessages.SortOrder );
    }



  }
}
