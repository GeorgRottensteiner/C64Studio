using C64Studio.Types;
using GR.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace C64Studio
{
  public class BaseDocument : DockContent, IComparable
  {
    public class DocEvent
    {
      public enum Type
      {
        NONE = 0,
        BREAKPOINT_ADDED,
        BREAKPOINT_REMOVED,
        BREAKPOINT_UPDATED
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


    public BaseDocument()
    {
      FileParsed                = false;
      DocumentInfo.BaseDoc      = this;

      InitializeComponent();

      contextMenuTab.Opening += new System.ComponentModel.CancelEventHandler( contextMenuTab_Opening );
      closeToolStripMenuItem.Click += new EventHandler( closeToolStripMenuItem_Click );
      closeAllButThisToolStripMenuItem.Click +=new EventHandler(closeAllButThisToolStripMenuItem_Click);
      closeAllToolStripMenuItem.Click += new EventHandler( closeAllToolStripMenuItem_Click );
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



    public virtual void OnApplicationEvent( C64Studio.Types.ApplicationEvent Event )
    {
    }



    public StudioCore   Core
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



    public void SetInternal()
    {
      m_IsInternal = true;
    }



    public void SetModified()
    {
      if ( InvokeRequired )
      {
        Invoke( new C64Studio.MainForm.ParameterLessCallback( SetModified ) );
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
            TabText = System.IO.Path.GetFileName( DocumentFilename );
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
      Text = System.IO.Path.GetFileName( DocumentFilename );
      TabText = System.IO.Path.GetFileName( DocumentFilename );
      if ( FloatPane != null )
      {
        FloatPane.Text = TabText;
        FloatPane.FloatWindow.Text = TabText;
      }
      Core.MainForm.RaiseApplicationEvent( new C64Studio.Types.ApplicationEvent( C64Studio.Types.ApplicationEvent.Type.DOCUMENT_FILENAME_CHANGED, DocumentInfo ) );
    }



    public virtual void FillContent( string Text )
    {
    }



    public virtual void InsertText( string Text )
    {
    }



    public new virtual bool Load()
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



    public virtual bool Save()
    {
      return false;
    }



    public virtual bool SaveAs()
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
      Text = System.IO.Path.GetFileNameWithoutExtension( DocumentInfo.DocumentFilename );
      return true;
    }



    protected void SetupWatcher()
    {
      string    fullPath = DocumentInfo.FullPath;
      try
      {
        m_FileWatcher.Path = GR.Path.RemoveFileSpec( fullPath );
      }
      catch ( System.ArgumentException )
      {
      }
      m_FileWatcher.Filter = System.IO.Path.GetFileName( fullPath );
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



    protected override void OnShown( EventArgs e )
    {
      if ( Visible )
      {
        if ( DockHandler.FloatPane != null )
        {
          //System.Drawing.Size newSize = GetPreferredSize( new System.Drawing.Size( 200, 200 ) );
          //DockHandler.FloatPane.FloatWindow.Bounds = new System.Drawing.Rectangle( DockHandler.FloatPane.FloatWindow.Bounds.Location, new System.Drawing.Size( 677, 417 ) );
          System.Drawing.Size newSize = GetPreferredSize( new System.Drawing.Size( 677, 417 ) );
          DockHandler.FloatPane.FloatWindow.Bounds = new System.Drawing.Rectangle( DockHandler.FloatPane.FloatWindow.Bounds.Location, newSize );
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
      Core.MainForm.RaiseApplicationEvent( new C64Studio.Types.ApplicationEvent( C64Studio.Types.ApplicationEvent.Type.DOCUMENT_CLOSED, DocumentInfo ) );
      if ( DocumentInfo.Element != null )
      {
        Core.MainForm.RaiseApplicationEvent( new C64Studio.Types.ApplicationEvent( C64Studio.Types.ApplicationEvent.Type.ELEMENT_CLOSED, DocumentInfo.Element ) );
        DocumentInfo.Element.IsShown = false;
        DocumentInfo.Element.Document = null;
        DocumentInfo.BaseDoc = null;
      }
      else
      {
        Core.MainForm.RaiseApplicationEvent( new C64Studio.Types.ApplicationEvent( C64Studio.Types.ApplicationEvent.Type.DOCUMENT_INFO_REMOVED, DocumentInfo ) );
      }
    }



    public virtual DialogResult CloseAfterModificationRequest()
    {
      System.Windows.Forms.DialogResult saveResult = DialogResult.Cancel;
      if ( string.IsNullOrEmpty( DocumentInfo.DocumentFilename ) )
      {
        saveResult = System.Windows.Forms.MessageBox.Show( "The " + DocumentInfo.Type.ToString() + " has been modified. Do you want to save the changes now?", "Save Changes?", MessageBoxButtons.YesNoCancel );
      }
      else
      {
        saveResult = System.Windows.Forms.MessageBox.Show( "The item " + DocumentInfo.DocumentFilename + " has been modified. Do you want to save the changes now?", "Save Changes?", MessageBoxButtons.YesNoCancel );
      }
      if ( ( saveResult == DialogResult.Cancel )
      ||   ( saveResult == DialogResult.No ) )
      {
        return saveResult;
      }
      if ( !Save() )
      {
        saveResult = DialogResult.Cancel;
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
        DialogResult requestSaveResult = CloseAfterModificationRequest();
        if ( requestSaveResult == DialogResult.Cancel )
        {
          e.Cancel = true;
          return;
        }
        e.Cancel = false;
        if ( requestSaveResult == DialogResult.No )
        {
          // avoid double request
          SetUnmodified();
          return;
        }
        if ( !Save() )
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



    public virtual void SetCursorToLine( int Line, bool SetFocus )
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
    }



    public virtual void Copy()
    {
    }



    public virtual void Paste()
    {
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



    protected virtual bool OpenFile( string Caption, string FileFilter, out string Filename )
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
        docsToClose.Add( doc );
      }

      foreach ( var doc in docsToClose )
      {
        doc.Close();
      }
    }



    protected bool SaveDocumentData( string SaveFilename, ByteBuffer Data, bool SaveAs )
    {
      DisableFileWatcher();
      if ( !GR.IO.File.WriteAllBytes( SaveFilename, Data ) )
      {
        EnableFileWatcher();
        return false;
      }
      if ( !SaveAs )
      {
        SetUnmodified();
      }
      EnableFileWatcher();
      return true;
    }


    
  }
}
