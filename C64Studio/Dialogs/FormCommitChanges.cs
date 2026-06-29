using DecentForms;
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
    private System.Windows.Forms.ImageList _TypeImages = null;
    private System.Windows.Forms.ImageList _SCImages = null;
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



    public FormCommitChanges( StudioCore Core, Project Project, ProjectElement Element, System.Windows.Forms.ImageList TypeImages, System.Windows.Forms.ImageList SCImages )
    {
      Setup( Core, Project, Element, TypeImages, SCImages );
    }



    private void Setup( StudioCore Core, Project Project, ProjectElement Element, System.Windows.Forms.ImageList TypeImages, System.Windows.Forms.ImageList SCImages )
    {
      _Element    = Element;
      _Project    = Project;
      _TypeImages = TypeImages;
      _SCImages   = SCImages;
      _Core       = Core;
      InitializeComponent();

      // check
      listCommitFiles.Columns.Add( "", 24 );
      listCommitFiles.Columns[0].Sizable = false;
      // type (image)
      listCommitFiles.Columns.Add( "", 24 );
      listCommitFiles.Columns[1].Sizable = false;
      listCommitFiles.Columns.Add( "Filename", 400 );
      listCommitFiles.Columns.Add( "Extension", 80 );

      editCommitAuthor.Text = Core.Settings.SourceControlInfo.CommitAuthor;
      editCommitEmail.Text  = Core.Settings.SourceControlInfo.CommitAuthorEmail;
      ValidateCommitInfo();

      Core.Theming.ApplyTheme( this );

      var imageList = new DecentForms.ImageList();
      foreach ( System.Drawing.Image image in _TypeImages.Images ) 
      {
        imageList.Add( image );
      }
      listCommitFiles.ImageList = imageList;

      RefillFileList();
    }



    private void RefillFileList()
    {
      listCommitFiles.Items.Clear();

      var currentState = _Project.SourceControl.GetCurrentRepositoryState();

      bool showUnversioned = checkShowUnversionedFiles.Checked;
      bool showUnmodified = checkShowUnmodifiedFiles.Checked;

      int groupIDModified = 0;
      int groupIDUnmodified = -1;
      int groupIDUnversioned = -1;
      int groupIDIgnored =-1;

      var groupVersioned = new DecentForms.ListControlItem( "Modified", true );
      listCommitFiles.Items.Add( groupVersioned );
      if ( showUnmodified )
      {
        var groupUnmodified = new DecentForms.ListControlItem( "Not Modified", true );
        listCommitFiles.Items.Add( groupUnmodified );
        groupIDUnmodified = 1;
      }
      if ( showUnversioned )
      {
        var groupUnversioned = new DecentForms.ListControlItem( "Not Versioned Files", true );
        listCommitFiles.Items.Add( groupUnversioned );
        groupIDUnversioned = listCommitFiles.Items.Count - 1;
      }
      var groupIgnored = new DecentForms.ListControlItem( "Ignored", true );
      listCommitFiles.Items.Add( groupIgnored );
      groupIDIgnored = listCommitFiles.Items.Count - 1;

      foreach ( var state in currentState )
      {
        int  groupIndex = groupIDUnversioned;
        switch ( SolutionExplorer.SourceControlIconFromState( state.FileState ) )
        {
          case 3:
          default:
            groupIndex = groupIDIgnored; 
            break;
          case 0:
          case 2:
          case 4:
            groupIndex = groupIDModified;
            break;
          case 1:
            groupIndex = groupIDUnmodified;
            break;
        }

        if ( state.FileState == FileState.NewInWorkdir )
        {
          groupIndex = groupIDUnversioned;
        }
        if ( groupIndex == -1 )
        {
          continue;
        }

        var item = new DecentForms.ListControlItem( "" );
        item.SubItems.Add( state.FileState.ToString() );
        item.SubItems.Add( state.Filename );
        item.SubItems.Add( GR.Path.GetExtension( state.Filename ) );
        item.Tag = state.FileState;

        item.GroupIndex = groupIndex;

        var element = _Project.GetElementByFilename( state.Filename );
        if ( element != null )
        {
          item.ImageIndex = (int)element.DocumentInfo.Type;
        }
        else
        {
          // binary for non project files
          item.ImageIndex = 12;
        }

        if ( ( ( _Element != null )
        &&     ( state.Filename == _Element.Filename ) )
        ||   ( ( _Element == null )
        &&     ( _Project.SourceControl.CanCommit( state.FileState ) ) ) )
        {
          item.Checked = true;
        }

        listCommitFiles.Items.Add( item );
      }
      listCommitFiles.SortOrder = DecentForms.SortOrder.DESCENDING;
      listCommitFiles.ListViewItemSorter = new CommitFilesItemComparer( listCommitFilesColumn, listCommitFiles.SortOrder );
      listCommitFiles.Sort();
    }



    public FormCommitChanges( StudioCore Core, Project Project, System.Windows.Forms.ImageList TypeImages, System.Windows.Forms.ImageList SCImages )
    {
      Setup( Core, Project, null, TypeImages, SCImages );
      /*
      _Project    = Project;
      _TypeImages = TypeImages;
      _SCImages   = SCImages;
      _Core = Core;
      InitializeComponent();

      // check
      listCommitFiles.Columns.Add( "", 24 );
      listCommitFiles.Columns[0].Sizable = false;
      // type (image)
      listCommitFiles.Columns.Add( "", 24 );
      listCommitFiles.Columns[1].Sizable = false;
      listCommitFiles.Columns.Add( "Filename", 400 );
      listCommitFiles.Columns.Add( "Extension", 80 );

      editCommitAuthor.Text = Core.Settings.SourceControlInfo.CommitAuthor;
      editCommitEmail.Text = Core.Settings.SourceControlInfo.CommitAuthorEmail;
      ValidateCommitInfo();

      Core.Theming.ApplyTheme( this );

      var currentState = _Project.SourceControl.GetCurrentRepositoryState();

      var imageList = new DecentForms.ImageList();
      foreach ( System.Drawing.Image image in _TypeImages.Images )
      {
        imageList.Add( image );
      }
      listCommitFiles.ImageList = imageList;

      foreach ( var state in currentState )
      {
        var item = new DecentForms.ListControlItem( "" );
        item.SubItems.Add( state.FileState.ToString() );
        item.SubItems.Add( state.Filename );
        item.SubItems.Add( GR.Path.GetExtension( state.Filename ) );
        item.Tag = state.FileState;

        var element = _Project.GetElementByFilename( state.Filename );
        if ( element != null )
        {
          item.ImageIndex = (int)element.DocumentInfo.Type;
        }
        else if ( ( GR.Path.GetExtension( state.Filename ).ToUpper() == ".C64" )
        ||        ( GR.Path.GetExtension( state.Filename ).ToUpper() == ".S64" ) )
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
      }*/
    }
    


    private void btnOK_Click( DecentForms.ControlBase Sender )
    {
      DialogResult = DialogResult.OK;

      _Core.Settings.SourceControlInfo.CommitAuthor      = _CommitAuthor;
      _Core.Settings.SourceControlInfo.CommitAuthorEmail = _CommitAuthorEmail;

      foreach ( var item in listCommitFiles.CheckedItems )
      {
        _CheckedFiles.Add( item.SubItems[2].Text );
      }

      Close();
    }



    private void listCommitFiles_ColumnClick( DecentForms.ControlBase sender )
    {
      listCommitFilesColumn = listCommitFiles.SelectedColumn;
      listCommitFiles.ListViewItemSorter = new CommitFilesItemComparer( listCommitFilesColumn, listCommitFiles.SortOrder );
    }



    private void listCommitFiles_DrawItemImage( DecentForms.ControlBase sender, ControlRenderer renderer, int x, int y, DecentForms.ListControlItem item )
    {
      Image nodeImg = _TypeImages.Images[item.ImageIndex];

      renderer.DrawImage( nodeImg, x, y );

      var scImageIndex = SolutionExplorer.SourceControlIconFromState( (FileState)item.Tag );

      if ( scImageIndex != -1 )
      {
        Point ptSCIcon = new Point( x + nodeImg.Width - 8, y + nodeImg.Height - 8 );

        renderer.DrawImage( _SCImages.Images[scImageIndex], ptSCIcon.X, ptSCIcon.Y, 8, 8 );
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



    private void listCommitFiles_MouseClick( DecentForms.ControlBase sender, DecentForms.ControlEvent e )
    {
      if ( e.MouseButtons == 2 )
      {
        bool    canAdd = true;
        bool    canRemove = true;
        bool    canIgnore = true;
        bool    canRevert = true;

        foreach ( var selItem in listCommitFiles.SelectedItems )
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
        menu.Show( listCommitFiles.PointToScreen( new Point( e.MouseX, e.MouseY ) ) );
      }
    }



    private void OnAddToRepository( object sender, EventArgs e )
    {
      bool  hadChanges = false;
      foreach ( var selItem in listCommitFiles.SelectedItems )
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
      foreach ( var selItem in listCommitFiles.SelectedItems )
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
      foreach ( var selItem in listCommitFiles.SelectedItems )
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

      foreach ( var selItem in listCommitFiles.SelectedItems )
      {
        var fileName = selItem.SubItems[2].Text;
        files.Add( fileName );
      }


      if ( _Project.SourceControl.RevertChanges( files ) )
      {
        foreach ( var selItem in listCommitFiles.SelectedItems )
        {
          var fileName = selItem.SubItems[2].Text;
          selItem.Tag = _Project.SourceControl.GetFileState( fileName );
        }
        listCommitFiles.Invalidate();

        _Core.MainForm.RaiseApplicationEvent( new Types.ApplicationEvent( Types.ApplicationEvent.Type.SOURCE_CONTROL_STATE_MODIFIED ) );
      }
    }



    private void btnCancel_Click( DecentForms.ControlBase Sender )
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }



    private void checkShowUnversionedFiles_CheckedChanged( ControlBase Sender )
    {
      RefillFileList();
    }



    private void checkShowUnmodifiedFiles_CheckedChanged( ControlBase Sender )
    {
      RefillFileList();
    }



  }
}