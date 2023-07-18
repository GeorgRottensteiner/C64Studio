using RetroDevStudio.Controls;
using RetroDevStudio.Documents;
using SourceControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

namespace RetroDevStudio.Dialogs
{
  public partial class FormCommitChanges : Form
  {
    private Project         _Project = null;
    private ImageList       _TypeImages = null;
    private ImageList       _SCImages = null;
    private int             listCommitFilesColumn = 0;
    private ProjectElement  _Element = null;
    private StudioCore       _Core = null;

    private string          _CommitAuthor = "";
    private string          _CommitAuthorEmail = "";
    private string          _CommitMessage = "";
    private List<string>    _CheckedFiles = new List<string>();



    public string CommitAuthor
    {
      get
      {
        return _CommitAuthor;
      }
    }



    public string CommitAuthorEmail
    {
      get
      {
        return _CommitAuthorEmail;
      }
    }



    public string CommitMessage
    {
      get
      {
        return _CommitMessage;
      }
    }



    public List<string> SelectedFiles
    {
      get
      {
        return _CheckedFiles;
      }
    }



    public FormCommitChanges( StudioCore Core, Project Project, ProjectElement Element, ImageList TypeImages, ImageList SCImages )
    {
      _Element    = Element;
      _Project    = Project;
      _TypeImages = TypeImages;
      _SCImages   = SCImages;
      _Core       = Core;
      InitializeComponent();

      editCommitAuthor.Text = Core.Settings.SourceControlInfo.CommitAuthor;
      editCommitEmail.Text  = Core.Settings.SourceControlInfo.CommitAuthorEmail;
      ValidateCommitInfo();

      Core.Theming.ApplyTheme( this );

      var currentState = _Project.SourceControl.GetCurrentRepositoryState();

      listCommitFiles.SmallImageList = _TypeImages;
      foreach ( var state in currentState )
      {
        var item = new ListViewItem( "" );
        item.SubItems.Add( state.FileState.ToString() );
        item.SubItems.Add( state.Filename );
        item.SubItems.Add( System.IO.Path.GetExtension( state.Filename ) );
        item.Tag = state.FileState;

        var element = _Project.GetElementByFilename( state.Filename );
        if ( element != null )
        {
          item.ImageIndex = (int)element.DocumentInfo.Type;
        }

        if ( state.Filename == _Element.Filename )
        {
          item.Checked = true;
        }

        listCommitFiles.Items.Add( item );
      }
      listCommitFiles.Sorting = SortOrder.Descending;
      listCommitFiles.ListViewItemSorter = new CommitFilesItemComparer( listCommitFilesColumn, listCommitFiles.Sorting );
      listCommitFiles.Sort();
    }



    public FormCommitChanges( StudioCore Core, Project Project, ImageList TypeImages, ImageList SCImages )
    {
      _Project    = Project;
      _TypeImages = TypeImages;
      _SCImages   = SCImages;
      _Core = Core;
      InitializeComponent();
      editCommitAuthor.Text = Core.Settings.SourceControlInfo.CommitAuthor;
      editCommitEmail.Text = Core.Settings.SourceControlInfo.CommitAuthorEmail;
      ValidateCommitInfo();

      Core.Theming.ApplyTheme( this );

      var currentState = _Project.SourceControl.GetCurrentRepositoryState();

      listCommitFiles.SmallImageList = _TypeImages;
      foreach ( var state in currentState )
      {
        var item = new ListViewItem( "" );
        item.SubItems.Add( state.FileState.ToString() );
        item.SubItems.Add( state.Filename );
        item.SubItems.Add( System.IO.Path.GetExtension( state.Filename ) );
        item.Tag = state.FileState;

        var element = _Project.GetElementByFilename( state.Filename );
        if ( element != null )
        {
          item.ImageIndex = (int)element.DocumentInfo.Type;
        }
        else if ( ( System.IO.Path.GetExtension( state.Filename ).ToUpper() == ".C64" )
        ||        ( System.IO.Path.GetExtension( state.Filename ).ToUpper() == ".S64" ) )
        {
          // a project/solution
          item.ImageIndex = 0;
        }
        else
        {
          // any other file type (assume binary)
          item.ImageIndex = 12;
        }

        if ( _Project.SourceControl.CanCommit( state.FileState ) )
        {
          item.Checked = true;
        }
        listCommitFiles.Items.Add( item );
      }
    }
    


    private void btnOK_Click( object sender, EventArgs e )
    {
      DialogResult = DialogResult.OK;

      _Core.Settings.SourceControlInfo.CommitAuthor      = _CommitAuthor;
      _Core.Settings.SourceControlInfo.CommitAuthorEmail = _CommitAuthorEmail;

      foreach ( ListViewItem item in listCommitFiles.CheckedItems )
      {
        _CheckedFiles.Add( item.SubItems[2].Text );
      }

      Close();
    }



    private void listCommitFiles_ColumnClick( object sender, ColumnClickEventArgs e )
    {
      if ( e.Column != listCommitFilesColumn )
      {
        // Set the sort column to the new column.
        listCommitFilesColumn = e.Column;
        // Set the sort order to ascending by default.
        listCommitFiles.Sorting = SortOrder.Ascending;
      }
      else
      {
        // Determine what the last sort order was and change it.
        if ( listCommitFiles.Sorting == SortOrder.Ascending )
        {
          listCommitFiles.Sorting = SortOrder.Descending;
        }
        else
        {
          listCommitFiles.Sorting = SortOrder.Ascending;
        }
      }
      listCommitFiles.ListViewItemSorter = new CommitFilesItemComparer( listCommitFilesColumn, listCommitFiles.Sorting );
      listCommitFiles.Sort();
    }



    private void listCommitFiles_DrawItemImage( Graphics G, int X, int Y, ListViewItem Item,  ListViewItem.ListViewSubItem SubItem )
    {
      Image nodeImg = _TypeImages.Images[Item.ImageIndex];
      Point ptNodeIcon = new Point( X, Y );

      G.DrawImage( nodeImg, ptNodeIcon );

      var scImageIndex = SolutionExplorer.SourceControlIconFromState( (FileState)Item.Tag );

      if ( scImageIndex != -1 )
      {
        Point ptSCIcon = new Point( ptNodeIcon.X + nodeImg.Width - 8, ptNodeIcon.Y + nodeImg.Height - 8 );

        G.DrawImage( _SCImages.Images[scImageIndex], ptSCIcon.X, ptSCIcon.Y, 8, 8 );
      }
    }



    private void listCommitFiles_ColumnWidthChanging( object sender, ColumnWidthChangingEventArgs e )
    {
      if ( e.ColumnIndex <= 1 )
      {
        e.Cancel = true;
        e.NewWidth = listCommitFiles.Columns[e.ColumnIndex].Width;
      }
    }



    private void editCommitAuthor_TextChanged( object sender, EventArgs e )
    {
      ValidateCommitInfo();
    }



    private void editCommitEmail_TextChanged( object sender, EventArgs e )
    {
      ValidateCommitInfo();
    }



    private void editCommitMessage_TextChanged( object sender, EventArgs e )
    {
      ValidateCommitInfo();
    }



    private void ValidateCommitInfo()
    {
      _CommitAuthor       = editCommitAuthor.Text;
      _CommitAuthorEmail  = editCommitEmail.Text;
      _CommitMessage      = editCommitMessage.Text;

      if ( ( !string.IsNullOrEmpty( _CommitAuthor ) )
      &&   ( !string.IsNullOrEmpty( _CommitAuthorEmail ) )
      &&   ( !string.IsNullOrEmpty( _CommitMessage ) ) )
      {
        btnOK.Enabled = true;
      }
      else
      {
        btnOK.Enabled = false;
      }
    }



    private void FormCommitChanges_Load( object sender, EventArgs e )
    {
      ActiveControl = editCommitMessage;
      editCommitMessage.Focus();
    }



    private void listCommitFiles_MouseClick( object sender, MouseEventArgs e )
    {
      if ( e.Button == MouseButtons.Right )
      {
        bool    canAdd = true;
        bool    canRemove = true;
        bool    canIgnore = true;
        bool    canRevert = true;

        foreach ( ListViewItem selItem in listCommitFiles.SelectedItems )
        {
          var fileState = (FileState)selItem.Tag;

          if ( !_Project.SourceControl.CanAddToRepository( fileState ) )
          {
            canAdd = false;
          }
          if ( !_Project.SourceControl.CanRemoveFromRepository( fileState ) )
          {
            canRemove = false;
          }
          if ( !_Project.SourceControl.CanAddToIgnore( fileState ) )
          {
            canIgnore = false;
          }
          if ( !_Project.SourceControl.CanRevertChanges( fileState ) )
          {
            canRevert = false;
          }
        }
        if ( ( !canAdd )
        &&   ( !canRemove )
        &&   ( !canIgnore ) 
        &&   ( !canRevert ) )
        {
          return;
        }
        var menu = new ContextMenuStrip();
        if ( canAdd )
        {
          menu.Items.Add( new ToolStripMenuItem( "Add to Repository", null, OnAddToRepository ) );
        }
        if ( canRemove )
        {
          menu.Items.Add( new ToolStripMenuItem( "Remove from Repository", null, OnRemoveFromRepository ) );
        }
        if ( canIgnore )
        {
          menu.Items.Add( new ToolStripMenuItem( "Add to Ignore", null, OnAddToIgnore ) );
        }
        if ( canRevert )
        {
          menu.Items.Add( new ToolStripMenuItem( "Revert Changes", null, OnRevertChanges ) );
        }
        menu.Show( listCommitFiles.PointToScreen( e.Location ) );
      }
    }



    private void OnAddToRepository( object sender, EventArgs e )
    {
      bool  hadChanges = false;
      foreach ( ListViewItem selItem in listCommitFiles.SelectedItems )
      {
        var fileName = selItem.SubItems[2].Text;

        if ( _Project.SourceControl.AddFileToRepository( fileName ) )
        {
          selItem.Tag = _Project.SourceControl.GetFileState( fileName );
          hadChanges = true;
        }
      }
      if ( hadChanges )
      {
        listCommitFiles.Invalidate();

        _Core.MainForm.RaiseApplicationEvent( new Types.ApplicationEvent( Types.ApplicationEvent.Type.SOURCE_CONTROL_STATE_MODIFIED ) );
      }
    }



    private void OnAddToIgnore( object sender, EventArgs e )
    {
      bool  hadChanges = false;
      foreach ( ListViewItem selItem in listCommitFiles.SelectedItems )
      {
        var fileName = selItem.SubItems[2].Text;

        if ( _Project.SourceControl.Ignore( fileName ) )
        {
          selItem.Tag = _Project.SourceControl.GetFileState( fileName );
          hadChanges = true;
        }
      }
      if ( hadChanges )
      {
        listCommitFiles.Invalidate();

        _Core.MainForm.RaiseApplicationEvent( new Types.ApplicationEvent( Types.ApplicationEvent.Type.SOURCE_CONTROL_STATE_MODIFIED ) );
      }
    }



    private void OnRemoveFromRepository( object sender, EventArgs e )
    {
      bool  hadChanges = false;
      foreach ( ListViewItem selItem in listCommitFiles.SelectedItems )
      {
        var fileName = selItem.SubItems[2].Text;

        if ( _Project.SourceControl.RemoveFileFromIndex( fileName ) )
        {
          hadChanges = true;
          selItem.Tag = _Project.SourceControl.GetFileState( fileName );
        }
      }
      if ( hadChanges )
      {
        listCommitFiles.Invalidate();

        _Core.MainForm.RaiseApplicationEvent( new Types.ApplicationEvent( Types.ApplicationEvent.Type.SOURCE_CONTROL_STATE_MODIFIED ) );
      }
    }



    private void OnRevertChanges( object sender, EventArgs e )
    {
      var files = new List<string>();

      foreach ( ListViewItem selItem in listCommitFiles.SelectedItems )
      {
        var fileName = selItem.SubItems[2].Text;
        files.Add( fileName );
      }


      if ( _Project.SourceControl.RevertChanges( files ) )
      {
        foreach ( ListViewItem selItem in listCommitFiles.SelectedItems )
        {
          var fileName = selItem.SubItems[2].Text;
          selItem.Tag = _Project.SourceControl.GetFileState( fileName );
        }
        listCommitFiles.Invalidate();

        _Core.MainForm.RaiseApplicationEvent( new Types.ApplicationEvent( Types.ApplicationEvent.Type.SOURCE_CONTROL_STATE_MODIFIED ) );
      }
    }


  }
}