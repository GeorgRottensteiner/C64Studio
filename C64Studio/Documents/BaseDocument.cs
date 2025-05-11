using RetroDevStudio.Types;
using GR.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.Drawing;
using RetroDevStudio.Controls;
using System.Linq;

namespace RetroDevStudio.Documents
{
  public class BaseDocument : DockContent, IComparable
  {
    public enum SaveMethod
    {
      SAVE,
      SAVE_AS,
      SAVE_COPY_AS
    }



    public class DocEvent
    {
      public enum Type
      {
        NONE = 0,
        BREAKPOINT_ADDED,
        BREAKPOINT_REMOVED,
        BREAKPOINT_UPDATED,
        BOOKMARK_ADDED,
        BOOKMARK_REMOVED,
        ALL_BOOKMARKS_OF_DOCUMENT_REMOVED,
        BOOKMARKS_UPDATED                   // sent when lines are inserted/deleted (and bookmarks move)
      }

      public Type EventType     = Type.NONE;
      public BaseDocument Doc   = null;
      public int LineIndex      = -1;
      public int OtherLineIndex = -1;
      public Types.Breakpoint Breakpoint = null;


      public DocEvent( Type EventType )
      {
        this.EventType = EventType;
      }

      public DocEvent( Type EventType, Types.Breakpoint Breakpoint )
      {
        this.EventType = EventType;
        this.Breakpoint = Breakpoint;
      }

      public DocEvent( Type EventType, Types.Breakpoint Breakpoint, int LineIndex )
      {
        this.EventType = EventType;
        this.Breakpoint = Breakpoint;
        this.LineIndex = LineIndex;
      }

      public DocEvent( Type EventType, int LineIndex )
      {
        this.EventType = EventType;
        this.LineIndex = LineIndex;
      }

      public DocEvent( Type EventType, int LineIndex, int OtherLineIndex )
      {
        this.EventType = EventType;
        this.LineIndex = LineIndex;
        this.OtherLineIndex = OtherLineIndex;
      }

    };


    public virtual DocumentInfo DocumentInfo
    {
      get;
      set;
    } = new DocumentInfo();

    private bool              m_Modified = false;

    private bool              m_ForceClose = false;
    protected bool            m_IsSaveable = false;

    // is internal states determines if a file must be saved if having changes before a build
    protected bool            m_IsInternal = false;
    private bool              m_DoNotUpdateFromControls = false;

    protected ContextMenuStrip contextMenuTab;
    private System.ComponentModel.IContainer components;
    private ToolStripMenuItem closeToolStripMenuItem;
    private ToolStripMenuItem closeAllButThisToolStripMenuItem;
    private ToolStripMenuItem closeAllToolStripMenuItem;

    protected System.IO.FileSystemWatcher     m_FileWatcher = new System.IO.FileSystemWatcher();


    public delegate           void DocumentEventHandler( DocEvent Event );

    public event              DocumentEventHandler DocumentEvent;



    private PopupContainer    _MacroPopup = null;
    private DecentForms.ListBox           _PopupList = null;
    private Control           _PopupControlSource = null;



    public BaseDocument()
    {
      FileParsed                = false;
      DocumentInfo.BaseDoc      = this;

      InitializeComponent();

      contextMenuTab.Opening += new System.ComponentModel.CancelEventHandler( contextMenuTab_Opening );
      closeToolStripMenuItem.Click += new EventHandler( closeToolStripMenuItem_Click );
      closeAllButThisToolStripMenuItem.Click +=new EventHandler(closeAllButThisToolStripMenuItem_Click);
      closeAllToolStripMenuItem.Click += new EventHandler( closeAllToolStripMenuItem_Click );

      MinimumSize = new Size( 20, 20 );
    }



    public virtual FastColoredTextBoxNS.FastColoredTextBox SourceControl
    {
      get
      {
        return null;
      }
    }



    protected bool DoNotUpdateFromControls
    {
      get
      {
        return m_DoNotUpdateFromControls;
      }
      set
      {
        m_DoNotUpdateFromControls = value;
      }
    }



    void closeAllButThisToolStripMenuItem_Click( object sender, EventArgs e )
    {
      var docList = DockPanel.Documents;

      List<BaseDocument>    docsToClose = new List<BaseDocument>();

      foreach ( BaseDocument doc in docList )
      {
        
        if ( doc != this )
        {
          if ( doc.HideOnClose )
          {
            doc.Hide();
            continue;
          }
          docsToClose.Add( doc );
        }
      }

      foreach ( var doc in docsToClose )
      {
        doc.Close();
      }
    }



    void closeToolStripMenuItem_Click( object sender, EventArgs e )
    {
      if ( HideOnClose )
      {
        Hide();
      }
      else
      {
        Close();
      }
    }




    void contextMenuTab_Opening( object sender, System.ComponentModel.CancelEventArgs e )
    {
      if ( DockState != DockState.Document )
      {
        e.Cancel = true;
      }
    }



    public virtual void OnApplicationEvent( Types.ApplicationEvent Event )
    {
    }



    public StudioCore Core
    {
      get;
      set;
    }



    public int CompareTo( object obj )
    {
      if ( obj is BaseDocument )
      {
        BaseDocument temp = (BaseDocument)obj;

        return GetHashCode().CompareTo( temp.GetHashCode() );
      }
      throw new ArgumentException( "object is not a BaseDocument" );
    }



    protected virtual void RaiseDocEvent( DocEvent Event )
    {
      Event.Doc = this;
      if ( DocumentEvent != null )
      {
        DocumentEvent( Event );
      }
    }
    


    public bool Modified
    {
      get
      {
        return m_Modified;
      }
    }



    public string DocumentFilename
    {
      get
      {
        return DocumentInfo.DocumentFilename;
      }
    }

    
    
    public bool FileParsed
    {
      get;
      set;
    }



    public bool IsInternal
    {
      get
      {
        return m_IsInternal;
      }
    }



    public bool IsSaveable
    {
      get
      {
        return m_IsSaveable;
      }
    }



    public virtual bool BreakpointToggleable
    {
      set
      {
      }
    }


    public virtual int CursorLine
    {
      get
      {
        return 0;
      }
    }



    public virtual int CursorPosInLine
    {
      get
      {
        return 0;
      }
    }



    public void SetInternal()
    {
      m_IsInternal = true;
    }



    public void SetModified()
    {
      if ( InvokeRequired )
      {
        Invoke( new RetroDevStudio.MainForm.ParameterLessCallback( SetModified ) );
        return;
      }

      if ( !m_Modified )
      {
        m_Modified = true;
        if ( !Text.EndsWith( "*" ) )
        {
          Text += "*";
        }
        if ( string.IsNullOrEmpty( TabText ) )
        {
          if ( DocumentFilename != null )
          {
            TabText = GR.Path.GetFileName( DocumentFilename );
          }
          else
          {
            TabText = Text;
          }
        }
        if ( !TabText.EndsWith( "*" ) )
        {
          TabText += "*";
        }
        if ( FloatPane != null )
        {
          FloatPane.Text = TabText;
          FloatPane.FloatWindow.Text = TabText;
        }
        if ( Core != null )
        {
          Core.MainForm.UpdateUndoSettings();
        }
        DocumentInfo.HasBeenSuccessfullyBuilt = false;
        FileParsed = false;
      }
    }



    public void SetUnmodified()
    {
      if ( m_Modified )
      {
        m_Modified = false;
        if ( Text.EndsWith( "*" ) )
        {
          Text = Text.Substring( 0, Text.Length - 1 );
        }
        if ( string.IsNullOrEmpty( TabText ) )
        {
          TabText = Text;
        }
        if ( TabText.EndsWith( "*" ) )
        {
          TabText = TabText.Substring( 0, TabText.Length - 1 );
        }
        if ( FloatPane != null )
        {
          FloatPane.Text = TabText;
          FloatPane.FloatWindow.Text = TabText;
        }
        if ( Core != null )
        {
          Core.MainForm.UpdateUndoSettings();
        }
      }
    }

    
    
    public void SetProjectElement( ProjectElement Element )
    {
      DocumentInfo = Element.DocumentInfo;
    }

    
    
    public void SetDocumentFilename( string DocumentFilename )
    {
      DocumentInfo.DocumentFilename = DocumentFilename;
      Text = GR.Path.GetFileName( DocumentFilename );
      TabText = GR.Path.GetFileName( DocumentFilename );
      if ( FloatPane != null )
      {
        FloatPane.Text = TabText;
        FloatPane.FloatWindow.Text = TabText;
      }
      Core.MainForm.RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.DOCUMENT_FILENAME_CHANGED, DocumentInfo ) );
    }



    public virtual void FillContent( string Text, bool KeepCursorPosIntact, bool KeepBookmarksIntact )
    {
    }



    public virtual void InsertText( string Text )
    {
    }



    public virtual void HighlightText( int LineIndex, int CharPos, int Length )
    {
    }



    public virtual void HighlightOccurrences( int LineIndex, int CharPos, int Length, List<TextLocation> Locations )
    {
    }



    public virtual bool LoadDocument()
    {
      return false;
    }



    public virtual GR.Memory.ByteBuffer SaveToBuffer()
    {
      return null;
    }



    public bool SaveToFile( string Filename )
    {
      GR.Memory.ByteBuffer dataToSave = SaveToBuffer();
      if ( dataToSave == null )
      {
        return false;
      }
      if ( !GR.IO.File.WriteAllBytes( Filename, dataToSave ) )
      {
        return false;
      }
      return true;
    }



    public bool Save( SaveMethod Method )
    {
      string  dummy;

      return Save( Method, out dummy );
    }



    public bool Save( SaveMethod Method, out string NewFilename )
    {
      NewFilename = "";
      if ( ( Method == SaveMethod.SAVE_AS )
      ||   ( Method == SaveMethod.SAVE_COPY_AS )
      ||   ( string.IsNullOrEmpty( DocumentInfo.DocumentFilename ) ) )
      {
        // we need a file name
        string    oldName = "";
        if ( ( Method == SaveMethod.SAVE_AS )
        &&   ( DocumentInfo.FullPath != null ) )
        {
          oldName = DocumentInfo.FullPath;
        }

        if ( !QueryFilename( DocumentInfo.FullPath, out NewFilename ) )
        {
          return false;
        }
        if ( DocumentInfo.DocumentFilename == null )
        {
          // a new filename
          if ( DocumentInfo.Project == null )
          {
            DocumentInfo.DocumentFilename = NewFilename;
          }
          else
          {
            DocumentInfo.DocumentFilename   = GR.Path.RelativePathTo( System.IO.Path.GetFullPath( DocumentInfo.Project.Settings.BasePath ), true, NewFilename, false );
            if ( DocumentInfo.Element != null )
            {
              DocumentInfo.Element.Name = GR.Path.GetFileName( DocumentInfo.DocumentFilename );
              DocumentInfo.Element.Node.Text = GR.Path.GetFileName( DocumentInfo.DocumentFilename );
              DocumentInfo.Element.Filename = DocumentInfo.DocumentFilename;
              if ( DocumentInfo.Element.Settings.Count == 0 )
              {
                DocumentInfo.Element.Settings["Default"] = new ProjectElement.PerConfigSettings();
              }
            }
          }
          Text    = GR.Path.GetFileNameWithoutExtension( DocumentInfo.DocumentFilename ) + "*";
          TabText = GR.Path.GetFileName( DocumentInfo.DocumentFilename );
          SetupWatcher();

          if ( !PerformSave( NewFilename ) )
          {
            return false;
          }
          Core.MainForm.RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.DOCUMENT_SAVED, DocumentInfo ) );
          SetUnmodified();
          return true;
        }

        if ( Method == SaveMethod.SAVE_AS )
        {
          if ( !PerformSave( NewFilename ) )
          {
            return false;
          }

          // rename during save
          if ( DocumentInfo.Project != null )
          {
            DocumentInfo.DocumentFilename = GR.Path.RelativePathTo( System.IO.Path.GetFullPath( DocumentInfo.Project.Settings.BasePath ), true, NewFilename, false );
            DocumentInfo.Element.Name = GR.Path.GetFileName( DocumentInfo.DocumentFilename );
            DocumentInfo.Element.Node.Text = GR.Path.GetFileName( DocumentInfo.DocumentFilename );
            DocumentInfo.Element.Filename = DocumentInfo.DocumentFilename;
            if ( DocumentInfo.Element.Settings.Count == 0 )
            {
              DocumentInfo.Element.Settings["Default"] = new ProjectElement.PerConfigSettings();
            }
          }
          else
          {
            DocumentInfo.DocumentFilename = NewFilename;
          }
          Text    = GR.Path.GetFileNameWithoutExtension( DocumentInfo.DocumentFilename ) + "*";
          TabText = GR.Path.GetFileName( DocumentInfo.DocumentFilename );

          // need to rename file in project (dependencies, etc.)
          string  newName = DocumentInfo.FullPath;

          if ( Core.Navigating.Solution != null )
          {
            Core.Navigating.Solution.RenameElement( DocumentInfo.Element, oldName, newName );
          }
          DocumentInfo?.Project?.SetModified();
          Core.MainForm.RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.DOCUMENT_SAVED, DocumentInfo ) );
          SetUnmodified();
          return true;
        }
        if ( !PerformSave( NewFilename ) )
        {
          return false;
        }
        Core.MainForm.RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.DOCUMENT_SAVED, DocumentInfo ) );
        SetUnmodified();
        return true;
      }

      if ( !PerformSave( DocumentInfo.FullPath ) )
      {
        return false;
      }
      Core.MainForm.RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.DOCUMENT_SAVED, DocumentInfo ) );
      SetUnmodified();
      return true;
    }



    protected virtual bool QueryFilename( string PreviousFilename, out string Filename )
    {
      Filename = "";
      return false;
    }



    protected virtual bool PerformSave( string FullPath )
    {
      return false;
    }



    public virtual string GetContent()
    {
      return "";
    }



    public virtual void SaveToChunk( GR.Memory.ByteBuffer OutBuffer )
    {
      OutBuffer.AppendString( DocumentInfo.DocumentFilename );
    }



    public virtual bool ReadFromReader( GR.IO.IReader Reader )
    {
      DocumentInfo.DocumentFilename = Reader.ReadString();
      Text = GR.Path.GetFileNameWithoutExtension( DocumentInfo.DocumentFilename );
      return true;
    }



    protected void SetupWatcher()
    {
      string    fullPath = DocumentInfo.FullPath;
      try
      {
        m_FileWatcher.Path = GR.Path.GetDirectoryName( fullPath );
      }
      catch ( System.ArgumentException )
      {
      }
      m_FileWatcher.Filter = GR.Path.GetFileName( fullPath );
      m_FileWatcher.NotifyFilter = System.IO.NotifyFilters.LastWrite;
      m_FileWatcher.Changed += new System.IO.FileSystemEventHandler( m_FileWatcher_Changed );
    }



    protected void DisposeWatcher()
    {
      DisableFileWatcher();
      m_FileWatcher.Changed -= new System.IO.FileSystemEventHandler( m_FileWatcher_Changed );
    }



    public void EnableFileWatcher()
    {
      try
      {
        m_FileWatcher.EnableRaisingEvents = true;
      }
      catch ( Exception )
      {
      }
    }



    public void DisableFileWatcher()
    {
      if ( ( m_FileWatcher.Path.Length == 0 )
      &&   ( DocumentInfo.DocumentFilename != null )
      &&   ( DocumentInfo.DocumentFilename.Length > 0 ) )
      {
        SetupWatcher();
      }
      m_FileWatcher.EnableRaisingEvents = false;
    }



    void m_FileWatcher_Changed( object sender, System.IO.FileSystemEventArgs e )
    {
      OnFileChanged();
    }



    protected virtual void OnFileChanged()
    {
      // notify main form
      Core.MainForm.OnDocumentExternallyChanged( this );
    }



    void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      this.contextMenuTab = new System.Windows.Forms.ContextMenuStrip( this.components );
      this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.closeAllButThisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.closeAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.contextMenuTab.SuspendLayout();
      this.SuspendLayout();
      // 
      // contextMenuTab
      // 
      this.contextMenuTab.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.closeToolStripMenuItem,
            this.closeAllButThisToolStripMenuItem,
            this.closeAllToolStripMenuItem} );
      this.contextMenuTab.Name = "contextMenuTab";
      this.contextMenuTab.Size = new System.Drawing.Size( 162, 92 );
      // 
      // closeToolStripMenuItem
      // 
      this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
      this.closeToolStripMenuItem.Size = new System.Drawing.Size( 161, 22 );
      this.closeToolStripMenuItem.Text = "&Close";
      // 
      // closeAllButThisToolStripMenuItem
      // 
      this.closeAllButThisToolStripMenuItem.Name = "closeAllButThisToolStripMenuItem";
      this.closeAllButThisToolStripMenuItem.Size = new System.Drawing.Size( 161, 22 );
      this.closeAllButThisToolStripMenuItem.Text = "Close all but this";
      // 
      // closeAllToolStripMenuItem
      // 
      this.closeAllToolStripMenuItem.Name = "closeAllToolStripMenuItem";
      this.closeAllToolStripMenuItem.Size = new System.Drawing.Size( 161, 22 );
      this.closeAllToolStripMenuItem.Text = "Close all";
      // 
      // BaseDocument
      // 
      this.ClientSize = new System.Drawing.Size( 292, 273 );
      this.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
      this.Name = "BaseDocument";
      this.TabPageContextMenuStrip = this.contextMenuTab;
      this.contextMenuTab.ResumeLayout( false );
      this.ResumeLayout( false );
    }



    public override Size GetPreferredSize( Size proposedSize )
    {
      // make sure to have at least 50x50
      var currentSize = Size;

      var preferredSize = base.GetPreferredSize( proposedSize );
      if ( ( preferredSize.Width < currentSize.Width )
      ||   ( preferredSize.Height < currentSize.Height ) )
      {
        preferredSize = new Size( Math.Max( preferredSize.Width, currentSize.Width ), Math.Max( preferredSize.Height, currentSize.Height ) );
      }
      if ( ( preferredSize.Width < 50 )
      ||   ( preferredSize.Height < 50 ) )
      {
        preferredSize = new Size( Math.Max( 50, proposedSize.Width ), Math.Max( 50, proposedSize.Height ) );
      }
      return preferredSize;
    }



    protected override void OnShown( EventArgs e )
    {
      if ( Visible )
      {
        if ( DockHandler.FloatPane != null )
        {
          //System.Drawing.Size newSize = GetPreferredSize( new System.Drawing.Size( 200, 200 ) );
          //DockHandler.FloatPane.FloatWindow.Bounds = new System.Drawing.Rectangle( DockHandler.FloatPane.FloatWindow.Bounds.Location, new System.Drawing.Size( 677, 417 ) );
          System.Drawing.Size newSize = GetPreferredSize( new System.Drawing.Size( 677, 417 ) );
          DockHandler.FloatPane.FloatWindow.ClientSize = newSize;
          //DockHandler.FloatPane.FloatWindow.Bounds = new System.Drawing.Rectangle( DockHandler.FloatPane.FloatWindow.Bounds.Location, newSize );
        }
      }
      base.OnShown( e );
    }



    protected override void OnClosed( EventArgs e )
    {
      if ( Core.MainForm == null )
      {
        return;
      }
      DisposeWatcher();
      Core.MainForm.ApplicationEvent -= OnApplicationEvent;
      Core.MainForm.RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.DOCUMENT_CLOSED, DocumentInfo ) );
      if ( DocumentInfo.Element != null )
      {
        Core.MainForm.RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.ELEMENT_CLOSED, DocumentInfo.Element ) );
        DocumentInfo.Element.IsShown = false;
        DocumentInfo.Element.Document = null;
        DocumentInfo.BaseDoc = null;
      }
      else
      {
        Core.MainForm.RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.DOCUMENT_INFO_REMOVED, DocumentInfo ) );
      }
    }



    public virtual Dialogs.DlgDeactivatableMessage.UserChoice CloseAfterModificationRequest()
    {
      var saveResult = Dialogs.DlgDeactivatableMessage.UserChoice.CANCEL;

      var endButtons = Dialogs.DlgDeactivatableMessage.MessageButtons.YES_NO_CANCEL;
      if ( Core.ShuttingDown )
      {
        endButtons = Dialogs.DlgDeactivatableMessage.MessageButtons.YES_NO;
      }
      if ( string.IsNullOrEmpty( DocumentInfo.DocumentFilename ) )
      {
        saveResult = Core.Notification.UserDecision( endButtons, "Save Changes?", $"The unnamed {DocumentInfo.Type} document has been modified. Do you want to save the changes now?" );
      }
      else
      {
        saveResult = Core.Notification.UserDecision( endButtons, "Save Changes?", "The item " + DocumentInfo.DocumentFilename + " has been modified. Do you want to save the changes now?" );
      }
      // remember decision for final modified item list
      if ( saveResult == Dialogs.DlgDeactivatableMessage.UserChoice.NO )
      {
        Core.ShuttingDownDeniedSave( this );
      }
      if ( ( saveResult == Dialogs.DlgDeactivatableMessage.UserChoice.CANCEL )
      ||   ( saveResult == Dialogs.DlgDeactivatableMessage.UserChoice.NO ) )
      {
        return saveResult;
      }
      if ( !Save( SaveMethod.SAVE ) )
      {
        saveResult = Dialogs.DlgDeactivatableMessage.UserChoice.CANCEL;
      }
      return saveResult;
    }



    protected override void OnClosing( System.ComponentModel.CancelEventArgs e )
    {
      if ( MainForm.s_SystemShutdown )
      {
        return;
      }

      if ( ( Modified )
      &&   ( !m_ForceClose ) )
      {
        var requestSaveResult = CloseAfterModificationRequest();
        if ( requestSaveResult == Dialogs.DlgDeactivatableMessage.UserChoice.CANCEL )
        {
          e.Cancel = true;
          return;
        }
        e.Cancel = false;
        if ( requestSaveResult == Dialogs.DlgDeactivatableMessage.UserChoice.NO )
        {
          // avoid double request
          SetUnmodified();
          return;
        }
        if ( !Save( SaveMethod.SAVE ) )
        {
          e.Cancel = true;
        }
      }
      if ( !e.Cancel )
      {
        DisposeWatcher();
      }
    }



    public void ForceClose()
    {
      m_ForceClose = true;
      Close();
    }



    public virtual void OnKnownKeywordsChanged()
    {
    }



    public virtual void OnKnownTokensChanged()
    {
    }



    public virtual void SetLineMarked( int Line, bool Set )
    {
    }



    public virtual void SetCursorToLine( int Line, int CharIndex, bool SetFocus )
    {
    }



    public virtual void SelectText( int Line, int CharIndex, int Length )
    {
    }



    public virtual int CurrentLineIndex
    {
      get
      {
        return 0;
      }
    }



    public virtual bool UndoPossible
    {
      get
      {
        return DocumentInfo.UndoManager.CanUndo;
      }
    }



    public virtual bool RedoPossible
    {
      get
      {
        return DocumentInfo.UndoManager.CanRedo;
      }
    }



    public virtual string UndoInfo
    {
      get
      {
        return DocumentInfo.UndoManager.UndoInfo;
      }
    }



    public virtual string RedoInfo
    {
      get
      {
        return DocumentInfo.UndoManager.RedoInfo;
      }
    }



    public virtual void Undo()
    {
      DocumentInfo.UndoManager.Undo();
    }



    public virtual void Redo()
    {
      DocumentInfo.UndoManager.Redo();
    }



    public virtual bool CutPossible
    {
      get
      {
        return false;
      }
    }



    public virtual bool CopyPossible
    {
      get
      {
        return false;
      }
    }



    public virtual bool PastePossible
    {
      get
      {
        return false;
      }
    }



    public virtual bool DeletePossible
    {
      get
      {
        return false;
      }
    }



    public virtual void Cut()
    {
      ApplyFunction( Function.CUT );
    }



    public virtual void Copy()
    {
      ApplyFunction( Function.COPY );
    }



    public virtual void Paste()
    {
      ApplyFunction( Function.PASTE );
    }



    public virtual void Delete()
    {
    }



    public virtual GR.Memory.ByteBuffer DisplayDetails()
    {
      return null;
    }



    public virtual void ApplyDisplayDetails( GR.Memory.ByteBuffer Buffer )
    {
    }



    public virtual void RefreshDisplayOptions()
    {
      Core.Theming.ApplyTheme( this );
    }



    protected override bool ProcessCmdKey( ref Message msg, Keys keyData )
    {
      if ( Core.MainForm.HandleCmdKey( ref msg, keyData ) )
      {
        return true;
      }
      return base.ProcessCmdKey( ref msg, keyData );
    }



    public virtual bool ApplyFunction( Types.Function Function )
    {
      return false;
    }



    public virtual bool OpenFile( string Caption, string FileFilter, out string Filename )
    {
      Filename = "";

      OpenFileDialog openDlg = new OpenFileDialog();

      openDlg.Title = Caption;
      openDlg.Filter = Core.MainForm.FilterString( FileFilter );
      if ( DocumentInfo.Project != null )
      {
        openDlg.InitialDirectory = DocumentInfo.Project.Settings.BasePath;
      }
      if ( ( openDlg.ShowDialog() != DialogResult.OK ) 
      ||   ( string.IsNullOrEmpty( openDlg.FileName ) ) )
      {
        return false;
      }
      Filename = openDlg.FileName;
      return true;
    }



    // used to save layout for dockpanelsuite
    protected override string GetPersistString()
    {
      if ( ( this is SourceASMEx )
      ||   ( this is SourceBasicEx ) )
      {
        return null;
      }
      return Text;
    }



    void closeAllToolStripMenuItem_Click( object sender, EventArgs e )    
    {
      var docList = DockPanel.Documents;

      List<BaseDocument>    docsToClose = new List<BaseDocument>();

      foreach ( BaseDocument doc in docList )
      {
        if ( doc.HideOnClose )
        {
          doc.Hide();
          continue;
        }
        docsToClose.Add( doc );
      }

      foreach ( var doc in docsToClose )
      {
        doc.Close();
      }
    }



    protected bool SaveDocumentData( string SaveFilename, ByteBuffer Data )
    {
      DisableFileWatcher();
      if ( !GR.IO.File.WriteAllBytes( SaveFilename, Data ) )
      {
        EnableFileWatcher();
        return false;
      }
      EnableFileWatcher();
      return true;
    }



    public virtual void RemoveBookmark( int LineIndex )
    {
    }



    public MachineType PreferredMachineType
    {
      get
      {
        if ( DocumentInfo.Project != null )
        {
          return DocumentInfo.Project.PreferredMachineType;
        }
        return Core.Settings.PreferredMachineType;
      }
    }



    public void DetectTextChange( TextBox Edit, List<string> completeItems )
    {
      Edit.PreviewKeyDown += Edit_PreviewKeyDown;
      var existingEntries = completeItems.Where( ci => ci.ToUpper().StartsWith( Edit.Text.ToUpper() ) );
      if ( !existingEntries.Any() )
      {
        ClosePopup();
        return;
      }

      _PopupControlSource = Edit;

      int   textPos = Edit.SelectionStart;
      if ( ( textPos == Edit.TextLength )
      &&   ( textPos > 0 ) )
      {
        --textPos;
      }
      bool  editCreatedNew = false;
      var   newLocation = Edit.Parent.PointToScreen( Edit.Location );
      newLocation = new Point( newLocation.X, newLocation.Y + Edit.Height );
      if ( _MacroPopup == null )
      {
        _PopupList = new DecentForms.ListBox();
        _PopupList.BorderStyle = DecentForms.BorderStyle.NONE;
        _PopupList.Width = Edit.Width;
        _PopupList.Height = 160;

        foreach ( var item in existingEntries )
        {
          _PopupList.Items.Add( item );
        }

        _PopupList.MouseClick += _PopupList_MouseClick;
        ResizeListBoxToContent();

        _MacroPopup = new PopupContainer( _PopupList );
        _MacroPopup.Location = newLocation;
        _MacroPopup.CreateControl();

        editCreatedNew = true;
      }
      else
      {
        _PopupList.BeginUpdate();
        _PopupList.Items.Clear();
        foreach ( var item in existingEntries )
        {
          _PopupList.Items.Add( item );
        }
        ResizeListBoxToContent();
        _PopupList.EndUpdate();
        _MacroPopup.Location = newLocation;
      }
      Edit.Focus();

      if ( editCreatedNew )
      {
        Edit.LostFocus += Edit_LostFocus;
        Edit.HandleDestroyed += Edit_HandleDestroyed;
        Edit.KeyDown += Edit_KeyDown;
      }
    }



    private void Edit_PreviewKeyDown( object sender, PreviewKeyDownEventArgs e )
    {
      if ( e.KeyCode == Keys.Enter )
      {
        if ( _PopupList != null )
        {
          e.IsInputKey = true;
        }
      }
      if ( e.KeyCode == Keys.Escape )
      {
        if ( _PopupList != null )
        {
          ClosePopup();
        }
      }
    }



    private void _PopupList_MouseClick( object sender, MouseEventArgs e )
    {
      var itemIndex = _PopupList.ItemIndexFromPosition( e.Location.X, e.Location.Y );
      if ( itemIndex != -1 )
      {
        if ( _PopupControlSource is TextBoxBase )
        {
          string    fullMacro = _PopupList.Items[itemIndex].Text;

          TextBoxBase  edit = _PopupControlSource as TextBoxBase;

          edit.Text = fullMacro;
          edit.SelectionStart = fullMacro.Length;
          ClosePopup();
        }
      }
    }



    private void Edit_KeyDown( object sender, KeyEventArgs e )
    {
      if ( e.KeyCode == Keys.Down )
      {
        if ( _PopupList.Items.Count == 0 )
        {
          return;
        }
        int   newIndex = ( _PopupList.SelectedIndex + 1 ) % _PopupList.Items.Count;
        _PopupList.SelectedIndex = newIndex;
        e.Handled = true;
      }
      else if ( e.KeyCode == Keys.Up )
      {
        if ( _PopupList.SelectedIndex > 0 )
        {
          --_PopupList.SelectedIndex;
          e.Handled = true;
        }
      }
      else if ( e.KeyCode == Keys.Enter )
      {
        if ( _PopupList.SelectedIndex != -1 )
        {
          string    fullMacro = _PopupList.Items[_PopupList.SelectedIndex].Text;

          TextBoxBase  edit = _PopupControlSource as TextBoxBase;

          edit.Text = fullMacro;
          edit.SelectionStart = fullMacro.Length;
          ClosePopup();
          e.Handled = true;
          e.SuppressKeyPress = true;
        }
      }
    }



    private void ResizeListBoxToContent()
    {
      var g = _PopupList.CreateGraphics();

      int     maxWidth = 0;
      int     maxHeight = _PopupList.Items.Count * _PopupList.Font.Height;

      var currentScreen = Screen.FromHandle( _PopupList.Handle );
      if ( maxHeight > currentScreen.Bounds.Height )
      {
        maxHeight = currentScreen.Bounds.Height;
      }
      if ( maxHeight > 160 )
      {
        maxHeight = 160;
      }

      foreach ( var item in _PopupList.Items )
      {
        int   itemWidth = (int)g.MeasureString( item.Text, _PopupList.Font ).Width;

        if ( itemWidth > maxWidth )
        {
          maxWidth = itemWidth;
        }
      }

      _PopupList.ClientSize = new Size( _PopupList.ClientSize.Width, maxHeight );
    }



    private void Edit_HandleDestroyed( object sender, EventArgs e )
    {
      ClosePopup();
    }



    private void Edit_LostFocus( object sender, EventArgs e )
    {
      if ( ( _MacroPopup.Focused )
      ||   ( _PopupList.Focused ) )
      {
        return;
      }
      ClosePopup();
    }



    private void ClosePopup()
    {
      if ( _PopupControlSource != null )
      {
        _PopupControlSource.PreviewKeyDown -= Edit_PreviewKeyDown;
        _PopupControlSource.LostFocus -= Edit_LostFocus;
        _PopupControlSource.HandleDestroyed -= Edit_HandleDestroyed;
        _PopupControlSource.KeyDown -= Edit_KeyDown;
        _PopupControlSource = null;
      }

      if ( _PopupList != null )
      {
        _PopupList.Dispose();
        _PopupList = null;
      }
      if ( _MacroPopup != null )
      {
        _MacroPopup.Dispose();
        _MacroPopup = null;
      }
    }




  }
}
