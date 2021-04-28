using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Text;
using System.Windows.Forms;
using C64Studio.Formats;
using C64Studio.Types;
using GR.Memory;
using WeifenLuo.WinFormsUI.Docking;

namespace C64Studio
{
  public partial class FileManager : BaseDocument
  {
    public class DragData
    {
      public FileManager Parent = null;
      public Formats.MediaFormat Media = null;
      public List<C64Studio.Types.FileInfo> Files = null;
    };

    string          m_Filename;

    Formats.MediaFormat m_Media = null;

    System.Drawing.Font     oldFont = null;


    public FileManager( StudioCore Core, string Filename )
    {
      m_Filename    = Filename;
      m_IsSaveable  = true;
      this.Core     = Core;
      DocumentInfo.UndoManager.MainForm = Core.MainForm;
      DocumentInfo.Type = ProjectElement.ElementType.MEDIA_MANAGER;

      InitializeComponent();

      oldFont = listFiles.Font;

      listFiles.ItemFonts.Add( new System.Drawing.Font( Core.MainForm.m_FontC64.Families[0], 16, System.Drawing.GraphicsUnit.Pixel ) );
      statusFileManager.Font = new System.Drawing.Font( Core.MainForm.m_FontC64.Families[0], 8, System.Drawing.GraphicsUnit.Pixel );
      labelMediaTitle.Font = statusFileManager.Font;

      GR.Image.DPIHandler.ResizeControlsForDPI( this );

      if ( Filename.Length > 0 )
      {
        LoadMediaFile();
      }
      RefreshFileView();
      UpdateStatusInfo();
    }



    public void CreateEmptyDiskImage()
    {
      CloseMedia();
      m_Media = new C64Studio.Formats.D64();
      m_Media.CreateEmptyMedia();
      SetUnmodified();
      UpdateStatusInfo();
      validateMediumToolStripMenuItem.Enabled = true;

      TabText = "Empty Disk";
      Text = "Empty Disk";
    }



    public void CreateEmptyTapeImage()
    {
      CloseMedia();
      m_Media = new C64Studio.Formats.T64();
      m_Media.CreateEmptyMedia();
      SetUnmodified();
      UpdateStatusInfo();
      validateMediumToolStripMenuItem.Enabled = true;
      TabText = "Empty Tape";
      Text = "Empty Tape";
    }



    public bool LoadMediaFile()
    {
      string upperName = m_Filename.ToUpper();
      validateMediumToolStripMenuItem.Enabled = false;

      if ( upperName.EndsWith( ".D64" ) )
      {
        // disk file
        m_Media = new C64Studio.Formats.D64();
      }
      else if ( upperName.EndsWith( ".D71" ) )
      {
        // disk file
        m_Media = new C64Studio.Formats.D71();
      }
      else if ( upperName.EndsWith( ".D81" ) )
      {
        // disk file
        m_Media = new C64Studio.Formats.D81();
      }
      else if ( upperName.EndsWith( ".T64" ) )
      {
        // tape file
        m_Media = new C64Studio.Formats.T64();
      }
      else if ( upperName.EndsWith( ".PRG" ) )
      {
        // prg file
        m_Media = new C64Studio.Formats.PRG();
      }
      else
      {
        System.Windows.Forms.MessageBox.Show( "The file " + m_Filename + " cannot be read, unknown format" );
        return false;
      }

      if ( !m_Media.Load( m_Filename ) )
      {
        System.Windows.Forms.MessageBox.Show( "The file " + m_Filename + " cannot be properly be read, maybe corrupted?" );
        return false;
      }

      SetUnmodified();
      DocumentInfo.DocumentFilename = m_Filename;
      if ( string.IsNullOrEmpty( m_FileWatcher.Path ) )
      {
        SetupWatcher();
        EnableFileWatcher();
      }
      RefreshFileView();
      UpdateStatusInfo();
      validateMediumToolStripMenuItem.Enabled = true;
      return true;

    }



    public override bool Save()
    {
      if ( String.IsNullOrEmpty( DocumentInfo.DocumentFilename ) )
      {
        // TODO - nur sinnvoll, wenn leere Medien angelegt werden können
        System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

        saveDlg.Title = "Save media as";
        if ( m_Media != null )
        {
          saveDlg.Filter = m_Media.FileFilter;
        }
        if ( saveDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
        {
          return false;
        }
        SetDocumentFilename( saveDlg.FileName );
        if ( DocumentInfo.Element != null )
        {
          if ( string.IsNullOrEmpty( DocumentInfo.Project.Settings.BasePath ) )
          {
            DocumentInfo.DocumentFilename = saveDlg.FileName;
          }
          else
          {
            DocumentInfo.DocumentFilename = GR.Path.RelativePathTo( saveDlg.FileName, false, System.IO.Path.GetFullPath( DocumentInfo.Project.Settings.BasePath ), true );
          }
          DocumentInfo.Element.Name = System.IO.Path.GetFileNameWithoutExtension( DocumentInfo.DocumentFilename );
          DocumentInfo.Element.Node.Text = System.IO.Path.GetFileName( DocumentInfo.DocumentFilename );
          DocumentInfo.Element.Filename = DocumentInfo.DocumentFilename;
        }
      }
      return DoSave( DocumentInfo.FullPath );
    }



    public override bool SaveAs()
    {
      // TODO - nur sinnvoll, wenn leere Medien angelegt werden können
      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Save media as";
      if ( m_Media != null )
      {
        saveDlg.Filter = m_Media.FileFilter;
      }
      if ( saveDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
      {
        return false;
      }
      return DoSave( saveDlg.FileName );
    }



    private bool DoSave( string Filename )
    {
      DisableFileWatcher();
      GR.Memory.ByteBuffer mediaData = null;

      if ( m_Media != null )
      {
        mediaData = m_Media.Compile();
      }
      if ( mediaData != null )
      {
        if ( !GR.IO.File.WriteAllBytes( Filename, mediaData ) )
        {
          System.Windows.Forms.MessageBox.Show( "Could not save file " + Filename + ".", "Could not save file" );
          EnableFileWatcher();
          return false;
        }
        SetUnmodified();
      }
      SetDocumentFilename( Filename );
      EnableFileWatcher();
      return true;
    }



    public void RefreshFileView()
    {
      int offset = 0;
      if ( listFiles.TopItem != null )
      {
        offset = listFiles.TopItem.Index;
      }
      var origSelections = new List<int>();
      foreach ( int entry in listFiles.SelectedIndices )
      {
        origSelections.Add( entry );
      }

      listFiles.BeginUpdate();
      listFiles.Items.Clear();

      List<C64Studio.Types.FileInfo> files = null;

      if ( m_Media != null )
      {
        labelMediaTitle.Text = C64Studio.Util.PETSCIIToUnicode( m_Media.Title );
        files = m_Media.Files();
      }
      else
      {
        labelMediaTitle.Text = "";
      }
      if ( files != null )
      {
        foreach ( C64Studio.Types.FileInfo file in files )
        {
          string filename = Util.FilenameToUnicode( file.Filename );

          ListViewItem item = new ListViewItem( filename );
          item.SubItems.Add( file.Blocks.ToString() );
          string     fileType = "";

          if ( ( file.Type & C64Studio.Types.FileType.CLOSED ) != 0 )
          {
            switch ( (Types.FileType)( (byte)file.Type & ( 0x0f | (byte)Types.FileType.CLOSED ) ) )
            {
              case C64Studio.Types.FileType.DEL:
                fileType = "DEL";
                break;
              case C64Studio.Types.FileType.PRG:
                fileType = "PRG";
                break;
              case C64Studio.Types.FileType.REL:
                fileType = "REL";
                break;
              case C64Studio.Types.FileType.SEQ:
                fileType = "SEQ";
                break;
              case C64Studio.Types.FileType.USR:
                fileType = "USR";
                break;
            }
          }

          
          if ( ( file.Type & C64Studio.Types.FileType.CLOSED ) == 0 )
          {
            fileType += "*";
          }
          if ( ( file.Type & C64Studio.Types.FileType.LOCKED ) != 0 )
          {
            fileType += "<";
          }
          item.SubItems.Add( fileType );
          item.SubItems.Add( file.StartTrack.ToString() );
          item.SubItems.Add( file.StartSector.ToString() );
          item.Tag = file;

          listFiles.Items.Add( item );
        }

        foreach ( int entry in origSelections )
        {
          listFiles.SelectedIndices.Add( entry );
        }
      }
      listFiles.EndUpdate();
      if ( offset < listFiles.Items.Count )
      {
        listFiles.TopItem = listFiles.Items[offset];
      }
    }



    private void openToolStripMenuItem_Click( object sender, EventArgs e )
    {
      OpenFileDialog      openDlg = new OpenFileDialog();

      openDlg.Title = "Choose an emulator medium to open";
      openDlg.Filter = Core.MainForm.FilterString( C64Studio.Types.Constants.FILEFILTER_MEDIA_FILES );
      openDlg.Multiselect = false;
      if ( openDlg.ShowDialog() == DialogResult.OK )
      {
        m_Filename = openDlg.FileName;
        if ( !LoadMediaFile() )
        {
          m_Filename = "";
        }
      }
    }



    private void DeleteSelectedItems()
    {
      if ( listFiles.SelectedItems.Count > 0 )
      {
        if ( System.Windows.Forms.MessageBox.Show( "Do you really want to delete the selected files?", "Delete Files?", MessageBoxButtons.YesNo ) == DialogResult.Yes )
        {
          bool deletedFile = false;
          foreach ( ListViewItem item in listFiles.SelectedItems )
          {
            C64Studio.Types.FileInfo fileToDelete = (C64Studio.Types.FileInfo)item.Tag;

            if ( m_Media != null )
            {
              if ( m_Media.DeleteFile( fileToDelete.Filename ) )
              {
                deletedFile = true;
              }
            }
          }
          if ( deletedFile )
          {
            listFiles.SelectedIndices.Clear();
            RefreshFileView();
            SetModified();
            UpdateStatusInfo();
          }
        }
      }
    }



    private void ExportSelectedItems()
    {
      foreach ( ListViewItem item in listFiles.SelectedItems )
      {
        C64Studio.Types.FileInfo fileToExport = (C64Studio.Types.FileInfo)item.Tag;
        C64Studio.Types.FileInfo fileInfo = null;

        if ( m_Media != null )
        {
          fileInfo = m_Media.LoadFile( fileToExport.Filename );
        }
        if ( fileInfo != null )
        {
          System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

          string readableFilename = Util.FilenameToReadableUnicode( fileToExport.Filename ).TrimEnd();

          char[] invalidChars = System.IO.Path.GetInvalidFileNameChars();
          foreach ( char invChar in invalidChars )
          {
            readableFilename = readableFilename.Replace( invChar, '_' );
          }

          saveDlg.Title = "Select target file name for " + readableFilename;
          saveDlg.Filter = "All Files|*.*";
          saveDlg.FileName = readableFilename;
          if ( fileToExport.Type == C64Studio.Types.FileType.PRG )
          {
            saveDlg.FileName += ".prg";
          }
          if ( saveDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK )
          {
            GR.IO.File.WriteAllBytes( saveDlg.FileName, fileInfo.Data );
          }
        }
      }
    }



    private void listFiles_PreviewKeyDown( object sender, PreviewKeyDownEventArgs e )
    {
      if ( e.KeyCode == Keys.Delete )
      {
        DeleteSelectedItems();
      }
      else if ( e.KeyCode == Keys.F2 )
      {
        RenameFile();
      }
    }



    private void listFiles_ItemDrag( object sender, ItemDragEventArgs e )
    {
      if ( listFiles.SelectedItems.Count == 0 )
      {
        return;
      }
      DragData dragData = new DragData();
      dragData.Parent = this;
      dragData.Files = new List<C64Studio.Types.FileInfo>();
      dragData.Media = m_Media;

      foreach ( ListViewItem item in listFiles.SelectedItems )
      {
        dragData.Files.Add( (C64Studio.Types.FileInfo)item.Tag );
      }
      listFiles.DoDragDrop( dragData, DragDropEffects.Copy );
    }



    private void listFiles_DragEnter( object sender, DragEventArgs e )
    {
      /*
      foreach ( string format in e.Data.GetFormats() )
      {
        Debug.Log( "Format: " + format );
      }*/
      if ( e.Data.GetDataPresent( DataFormats.FileDrop ) )
      {
        e.Effect = DragDropEffects.Copy;
        return;
      }

      if ( !e.Data.GetDataPresent( "C64Studio.FileManager+DragData" ) )
      {
        e.Effect = DragDropEffects.None;
        return;
      }
      DragData dragData = (DragData)e.Data.GetData( "C64Studio.FileManager+DragData" );

      if ( dragData.Parent == this )
      {
        e.Effect = DragDropEffects.None;
        return;
      }
      e.Effect = DragDropEffects.Copy;
    }



    private void listFiles_DragDrop( object sender, DragEventArgs e )
    {
      DragData dragData = null;

      if ( e.Data.GetDataPresent( DataFormats.FileDrop ) )
      {
        // insert file(s)
        string[] files = (string[])e.Data.GetData( DataFormats.FileDrop, false );
        bool     changedFile = false;
        foreach ( string file in files )
        {
          if ( ImportFile( file ) )
          {
            changedFile = true;
          }
        }
        if ( changedFile )
        {
          RefreshFileView();
          SetModified();
          UpdateStatusInfo();
        }
        return;
      }
      else if ( !e.Data.GetDataPresent( "C64Studio.FileManager+DragData" ) )
      {
        return;
      }
      else
      {
        dragData = (DragData)e.Data.GetData( "C64Studio.FileManager+DragData" );
        if ( dragData.Parent == this )
        {
          return;
        }
      }

      // insert file
      bool changed = false;
      foreach ( C64Studio.Types.FileInfo file in dragData.Files )
      {
        Types.FileInfo          fileInfo = null;
        if ( dragData.Media != null )
        {
          fileInfo = dragData.Media.LoadFile( file.Filename );
        }

        if ( fileInfo != null )
        {
          if ( m_Media != null )
          {
            if ( m_Media.WriteFile( file.Filename, fileInfo.Data, file.Type ) )
            {
              changed = true;
            }
          }
        }
      }
      if ( changed )
      {
        RefreshFileView();
        SetModified();
        UpdateStatusInfo();
      }
    }



    private bool ImportFile( string file )
    {
      GR.Memory.ByteBuffer fileData = GR.IO.File.ReadAllBytes( file );
      if ( ( fileData != null )
      &&   ( fileData.Length >= 0 ) )
      {
        GR.Memory.ByteBuffer fileName = new GR.Memory.ByteBuffer( 16 );
        string shortName = System.IO.Path.GetFileNameWithoutExtension( file ).ToUpper();
        int nameLength = shortName.Length;
        if ( nameLength > 16 )
        {
          nameLength = 16;
        }
        for ( int i = 0; i < 16; ++i )
        {
          if ( i < nameLength )
          {
            fileName.SetU8At( i, (byte)shortName[i] );
          }
          else
          {
            fileName.SetU8At( i, 0xA0 );
          }
        }
        if ( m_Media != null )
        {
          if ( m_Media.WriteFile( fileName, fileData, C64Studio.Types.FileType.PRG ) )
          {
            RefreshFileView();
            UpdateStatusInfo();
            return true;
          }
        }
      }
      return false;
    }



    private void listFiles_ItemSelectionChanged( object sender, ListViewItemSelectionChangedEventArgs e )
    {
      UpdateStatusInfo();
    }



    private void UpdateStatusInfo()
    {
      if ( m_Media == null )
      {
        saveToolStripMenuItem.Enabled = false;
        saveasToolStripMenuItem.Enabled = false;
        validateMediumToolStripMenuItem.Enabled = false;
      }
      else
      {
        saveToolStripMenuItem.Enabled = Modified;
        saveasToolStripMenuItem.Enabled = true;
        validateMediumToolStripMenuItem.Enabled = true;
      }

      if ( listFiles.SelectedItems.Count == 0 )
      {
        if ( ( m_Media is Formats.D64 )
        ||   ( m_Media is Formats.D71 )
        ||   ( m_Media is Formats.D81 ) )
        {
          statusFileManager.Text = m_Media.FreeSlots.ToString() + "/" + m_Media.Slots.ToString() + " blocks free"
              + "  " + listFiles.Items.Count + " files";
          return;
        }
        if ( m_Media != null )
        {
          statusFileManager.Text = m_Media.FreeSlots.ToString() + "/" + m_Media.Slots.ToString() + " files free"
            + "  " + listFiles.Items.Count + " files";
          return;
        }
        statusFileManager.Text = "No media opened";
        return;
      }
      C64Studio.Types.FileInfo file = (C64Studio.Types.FileInfo)listFiles.SelectedItems[0].Tag;

      statusFileManager.Text = Util.FilenameToUnicode( file.Filename ) + " " + file.Blocks.ToString() + " blocks";
    }



    private void listFiles_MouseClick( object sender, MouseEventArgs e )
    {
      if ( ( e.Button != MouseButtons.Right )
      ||   ( listFiles.SelectedItems.Count == 0 ) )
      {
        return;
      }
      System.Windows.Forms.ContextMenuStrip   contextMenu = new System.Windows.Forms.ContextMenuStrip();

      System.Windows.Forms.ToolStripMenuItem item = new System.Windows.Forms.ToolStripMenuItem( "Export" );
      item.Tag = 0;
      contextMenu.Items.Add( item );

      System.Windows.Forms.ToolStripMenuItem subItem = new System.Windows.Forms.ToolStripMenuItem( "Export to file" );
      subItem.Tag = 0;
      subItem.Click += new EventHandler( itemExport_Click );
      item.DropDownItems.Add( subItem );

      bool  exportToBasicPossible = true;
      foreach ( ListViewItem listItem in listFiles.SelectedItems )
      {
        C64Studio.Types.FileInfo fileInfo = (C64Studio.Types.FileInfo)listItem.Tag;
        if ( fileInfo.Type != C64Studio.Types.FileType.PRG )
        {
          exportToBasicPossible = false;
        }
      }
      if ( exportToBasicPossible )
      {
        item.DropDownItems.Add( "--" );
        foreach ( var dialect in Core.Compiling.BASICDialects )
        {
          subItem = new System.Windows.Forms.ToolStripMenuItem( "Export to Basic code: " + dialect.Key );
          subItem.Tag = dialect.Value;
          subItem.Click += new EventHandler( itemExportToBasic_Click );
          item.DropDownItems.Add( subItem );
        }
      }

      // view in Hex display
      item = new System.Windows.Forms.ToolStripMenuItem( "View in Hex Editor" );
      item.Tag = 2;
      item.Click += new EventHandler( itemViewInHexEditor_Click );
      contextMenu.Items.Add( item );

      item = new System.Windows.Forms.ToolStripMenuItem( "Change File Type" );
      item.Tag = 2;
      //item.Click += new EventHandler( itemChangeType_Click );
      contextMenu.Items.Add( item );

      subItem = new ToolStripMenuItem( "PRG" );
      subItem.Tag = FileType.PRG;
      subItem.Click += new EventHandler( itemChangeType_Click );
      item.DropDownItems.Add( subItem );

      subItem = new ToolStripMenuItem( "DEL" );
      subItem.Tag = FileType.DEL;
      subItem.Click += new EventHandler( itemChangeType_Click );
      item.DropDownItems.Add( subItem );

      subItem = new ToolStripMenuItem( "USR" );
      subItem.Tag = FileType.USR;
      subItem.Click += new EventHandler( itemChangeType_Click );
      item.DropDownItems.Add( subItem );

      subItem = new ToolStripMenuItem( "REL" );
      subItem.Tag = FileType.REL;
      subItem.Click += new EventHandler( itemChangeType_Click );
      item.DropDownItems.Add( subItem );

      subItem = new ToolStripMenuItem( "SEQ" );
      subItem.Tag = FileType.SEQ;
      subItem.Click += new EventHandler( itemChangeType_Click );
      item.DropDownItems.Add( subItem );

      item = new System.Windows.Forms.ToolStripMenuItem( "Rename" );
      item.Tag = 2;
      item.Click += new EventHandler( itemRename_Click );
      contextMenu.Items.Add( item );

      item = new System.Windows.Forms.ToolStripMenuItem( "Delete" );
      item.Tag = 1;
      item.Click += new EventHandler( itemDelete_Click );
      contextMenu.Items.Add( item );

      contextMenu.Show( listFiles.PointToScreen( e.Location ) );
    }



    private void itemChangeType_Click( object sender, EventArgs e )
    {
      var fileType = (FileType)( (ToolStripMenuItem)sender ).Tag;
      ChangeFileType( fileType );
    }



    private void ChangeFileType( FileType NewFileType )
    {
      bool changedType = false;
      foreach ( ListViewItem item in listFiles.SelectedItems )
      {
        C64Studio.Types.FileInfo fileToChange = (C64Studio.Types.FileInfo)item.Tag;
        if ( fileToChange.Type != NewFileType )
        {
          m_Media.ChangeFileType( fileToChange, NewFileType );
          changedType = true;
        }
      }

      if ( changedType )
      {
        RefreshFileView();
        SetModified();
        UpdateStatusInfo();
      }
    }



    public override bool Load()
    {
      return LoadMediaFile();
    }



    void OpenInHexEditor( C64Studio.Types.FileInfo FileToImport )
    {
      C64Studio.Types.FileInfo fileInfo = null;

      if ( m_Media != null )
      {
        fileInfo = m_Media.LoadFile( FileToImport.Filename );
      }
      if ( fileInfo != null )
      {
        BinaryDisplay display = new BinaryDisplay( Core, fileInfo.Data, false, true );
        display.Show( Core.MainForm.panelMain, DockState.Float );
      }
    }



    void itemViewInHexEditor_Click( object sender, EventArgs e )
    {
      OpenSelectedItemsInHexEditor();
    }



    void OpenSelectedItemsInHexEditor()
    {
      foreach ( ListViewItem item in listFiles.SelectedItems )
      {
        C64Studio.Types.FileInfo fileToExport = (C64Studio.Types.FileInfo)item.Tag;

        OpenInHexEditor( fileToExport );
      }
    }



    void itemExportToBasic_Click( object sender, EventArgs e )
    {
      var menuItem = (ToolStripMenuItem)sender;

      var dialect = (C64Models.BASIC.Dialect)menuItem.Tag;
      ExportSelectedItemsToBASIC( dialect );
    }



    void ExportSelectedItemsToBASIC( C64Models.BASIC.Dialect Dialect )
    {
      foreach ( ListViewItem item in listFiles.SelectedItems )
      {
        C64Studio.Types.FileInfo  fileToExport = (C64Studio.Types.FileInfo)item.Tag;

        ExportToBASIC( fileToExport, Dialect );
      }
    }



    private void ExportToBASIC( Types.FileInfo fileToExport, C64Models.BASIC.Dialect Dialect )
    {
      C64Studio.Types.FileInfo  fileInfo = null;

      if ( m_Media != null )
      {
        fileInfo = m_Media.LoadFile( fileToExport.Filename );
      }
      if ( fileInfo != null )
      {
        if ( fileInfo.Data.Length >= 2 )
        {
          //if ( startAddress == 0x0801 )
          {
            // can only load from that address

            List<string>    lines;

            // trunc load address
            var parser = new Parser.BasicFileParser( new Parser.BasicFileParser.ParserSettings() );
            parser.SetBasicDialect( Dialect );
            if ( parser.Disassemble( fileInfo.Data.SubBuffer( 2 ), out lines ) )
            {
              var document = new SourceBasicEx( Core );
              document.ShowHint = DockState.Document;

              document.Core = Core;
              document.Show( Core.MainForm.panelMain );
              document.SetBASICDialect( Dialect );

              StringBuilder sb = new StringBuilder();
              foreach ( string line in lines )
              {
                sb.AppendLine( line );
              }
              document.FillContent( sb.ToString(), false );
            }
          }
        }
      }
    }



    void itemExport_Click( object sender, EventArgs e )
    {
      ExportSelectedItems();
    }



    void itemDelete_Click( object sender, EventArgs e )
    {
      DeleteSelectedItems();
    }



    void itemRename_Click( object sender, EventArgs e )
    {
      RenameFile();
    }



    void RenameFile()
    {
      bool renamedFile = false;
      foreach ( ListViewItem item in listFiles.SelectedItems )
      {
        C64Studio.Types.FileInfo fileToRename = (C64Studio.Types.FileInfo)item.Tag;

        FormRenameFile formRename = new FormRenameFile( Core, fileToRename.Filename );

        if ( formRename.ShowDialog() == DialogResult.OK )
        {
          if ( m_Media.RenameFile( fileToRename.Filename, formRename.Filename ) )
          {
            renamedFile = true;
          }
        }
      }
      if ( renamedFile )
      {
        RefreshFileView();
        SetModified();
        UpdateStatusInfo();
      }
    }



    private bool CloseMedia()
    {
      if ( Modified )
      {
        DialogResult saveRequestResult = CloseAfterModificationRequest();

        if ( saveRequestResult == DialogResult.Cancel )
        {
          return false;
        }
      }
      m_Media = null;
      DocumentInfo.DocumentFilename = "";
      RefreshFileView();
      UpdateStatusInfo();
      return true;
    }



    private void createEmptyTapeToolStripMenuItem_Click( object sender, EventArgs e )
    {
      CloseMedia();

      m_Media = new C64Studio.Formats.T64();
      m_Media.CreateEmptyMedia();
      SetUnmodified();
      RefreshFileView();
      UpdateStatusInfo();
    }



    private void validateMediumToolStripMenuItem_Click( object sender, EventArgs e )
    {
      if ( m_Media != null )
      {
        m_Media.Validate();
        RefreshFileView();
        SetModified();
        UpdateStatusInfo();
      }
    }



    private void toolStripBtnMoveFileUp_Click( object sender, EventArgs e )
    {
      if ( m_Media is Formats.D64 )
      {
        Formats.D64 disk = m_Media as Formats.D64;
        C64Studio.Types.FileInfo fileToMove = (C64Studio.Types.FileInfo)listFiles.SelectedItems[0].Tag;

        int oldIndex = listFiles.SelectedIndices[0];
        if ( disk.MoveFileUp( fileToMove ) )
        {
          RefreshFileView();
          listFiles.SelectedIndices.Clear();
          listFiles.SelectedIndices.Add( oldIndex - 1 );
          SetModified();
          UpdateStatusInfo();
        }
      }
      else if ( m_Media is Formats.D81 )
      {
        Formats.D81 disk = m_Media as Formats.D81;
        C64Studio.Types.FileInfo fileToMove = (C64Studio.Types.FileInfo)listFiles.SelectedItems[0].Tag;

        int oldIndex = listFiles.SelectedIndices[0];
        if ( disk.MoveFileUp( fileToMove ) )
        {
          RefreshFileView();
          listFiles.SelectedIndices.Clear();
          listFiles.SelectedIndices.Add( oldIndex - 1 );
          SetModified();
          UpdateStatusInfo();
        }
      }
    }



    private void toolStripBtnMoveFileDown_Click( object sender, EventArgs e )
    {
      if ( m_Media is Formats.D64 )
      {
        Formats.D64 disk = m_Media as Formats.D64;
        C64Studio.Types.FileInfo fileToMove = (C64Studio.Types.FileInfo)listFiles.SelectedItems[0].Tag;

        int oldIndex = listFiles.SelectedIndices[0];
        if ( disk.MoveFileDown( fileToMove ) )
        {
          RefreshFileView();
          listFiles.SelectedIndices.Clear();
          listFiles.SelectedIndices.Add( oldIndex + 1 );
          SetModified();
          UpdateStatusInfo();
        }
      }
      else if ( m_Media is Formats.D71 )
      {
        Formats.D71 disk = m_Media as Formats.D71;
        C64Studio.Types.FileInfo fileToMove = (C64Studio.Types.FileInfo)listFiles.SelectedItems[0].Tag;

        int oldIndex = listFiles.SelectedIndices[0];
        if ( disk.MoveFileDown( fileToMove ) )
        {
          RefreshFileView();
          listFiles.SelectedIndices.Clear();
          listFiles.SelectedIndices.Add( oldIndex + 1 );
          SetModified();
          UpdateStatusInfo();
        }
      }
      else if ( m_Media is Formats.D81 )
      {
        Formats.D81 disk = m_Media as Formats.D81;
        C64Studio.Types.FileInfo fileToMove = (C64Studio.Types.FileInfo)listFiles.SelectedItems[0].Tag;

        int oldIndex = listFiles.SelectedIndices[0];
        if ( disk.MoveFileDown( fileToMove ) )
        {
          RefreshFileView();
          listFiles.SelectedIndices.Clear();
          listFiles.SelectedIndices.Add( oldIndex + 1 );
          SetModified();
          UpdateStatusInfo();
        }
      }
    }



    private void listFiles_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( listFiles.SelectedIndices.Count == 0 )
      {
        toolStripBtnOpenHex.Enabled = false;
        toolStripBtnOpenBASIC.Enabled = false;
        toolStripBtnExportToFile.Enabled = false;
      }
      else
      {
        toolStripBtnOpenHex.Enabled = true;
        toolStripBtnExportToFile.Enabled = true;

        bool  exportToBasicPossible = true;
        foreach ( ListViewItem listItem in listFiles.SelectedItems )
        {
          C64Studio.Types.FileInfo fileInfo = (C64Studio.Types.FileInfo)listItem.Tag;
          if ( fileInfo.Type != C64Studio.Types.FileType.PRG )
          {
            exportToBasicPossible = false;
            break;
          }
        }
        toolStripBtnOpenBASIC.Enabled = exportToBasicPossible;
      }

      if ( ( listFiles.Items.Count <= 1 )
      ||   ( listFiles.SelectedIndices.Count == 0 ) )
      {
        toolStripBtnMoveFileDown.Enabled = false;
        toolStripBtnMoveFileUp.Enabled = false;

        return;
      }
      toolStripBtnMoveFileUp.Enabled = ( listFiles.SelectedIndices[0] > 0 );
      toolStripBtnMoveFileDown.Enabled = ( listFiles.SelectedIndices[0] + 1 < listFiles.Items.Count );
    }




    private void importFileToolStripMenuItem_Click( object sender, EventArgs e )
    {
      string filename;

      if ( OpenFile( "Choose file to import", C64Studio.Types.Constants.FILEFILTER_PRG + C64Studio.Types.Constants.FILEFILTER_ALL, out filename ) )
      {
        ImportFile( filename );
      }
    }



    public override System.Drawing.Size GetPreferredSize( System.Drawing.Size proposedSize )
    {
      return new System.Drawing.Size( 580, 380 );
    }



    private void saveasToolStripMenuItem_Click( object sender, EventArgs e )
    {
      SaveAs();
    }



    private void saveToolStripMenuItem_Click( object sender, EventArgs e )
    {
      Save();
    }



    private void d64ToolStripMenuItem_Click( object sender, EventArgs e )
    {
      CloseMedia();

      m_Media = new C64Studio.Formats.D64();
      m_Media.CreateEmptyMedia();
      SetUnmodified();
      RefreshFileView();
      UpdateStatusInfo();
    }



    private void d81ToolStripMenuItem_Click( object sender, EventArgs e )
    {
      CloseMedia();

      m_Media = new C64Studio.Formats.D81();
      m_Media.CreateEmptyMedia();
      SetUnmodified();
      RefreshFileView();
      UpdateStatusInfo();
    }



    private void d64With40TracksToolStripMenuItem_Click( object sender, EventArgs e )
    {
      CloseMedia();

      var d64 = new C64Studio.Formats.D64();
      d64.CreateEmptyMedia40Tracks();
      m_Media = d64;
      SetUnmodified();
      RefreshFileView();
      UpdateStatusInfo();
    }



    private void toolStripBtnOpenHex_Click( object sender, EventArgs e )
    {
      OpenSelectedItemsInHexEditor();
    }



    private void toolStripBtnOpenBASIC_Click( object sender, EventArgs e )
    {
      ExportSelectedItemsToBASIC( C64Models.BASIC.Dialect.BASICV2 );
    }



    private void toolStripBtnExportToFile_Click( object sender, EventArgs e )
    {
      ExportSelectedItems();
    }



    private void toolStripBtnImportFile_Click( object sender, EventArgs e )
    {
      OpenFileDialog      openDlg = new OpenFileDialog();

      openDlg.Title = "Choose a file to import";
      openDlg.Filter = Core.MainForm.FilterString( C64Studio.Types.Constants.FILEFILTER_ALL );
      openDlg.Multiselect = true;
      if ( openDlg.ShowDialog() == DialogResult.OK )
      {
        bool  changedFile = false;

        foreach ( var file in openDlg.FileNames )
        {
          if ( ImportFile( file ) )
          {
            changedFile = true;
          }
        }
        if ( changedFile )
        {
          RefreshFileView();
          SetModified();
          UpdateStatusInfo();
        }
      }
    }



    private void d71ToolStripMenuItem_Click( object sender, EventArgs e )
    {
      CloseMedia();

      m_Media = new C64Studio.Formats.D71();
      m_Media.CreateEmptyMedia();
      SetUnmodified();
      RefreshFileView();
      UpdateStatusInfo();
    }



    private void toolStripBtnSave_Click( object sender, EventArgs e )
    {
      Save();
    }



    public override void OnApplicationEvent( ApplicationEvent Event )
    {
      base.OnApplicationEvent( Event );

      switch ( Event.EventType )
      {
        case ApplicationEvent.Type.DOCUMENT_ACTIVATED:
          UpdateStatusInfo();
          break;
      }
    }



    private void btnAddNew_Click( object sender, EventArgs e )
    {
      if ( m_Media != null )
      {
        var emptyFile = new ByteBuffer();
        var fileName = Util.ToFilename( "NEW FILE" );
        if ( m_Media.WriteFile( fileName, emptyFile, C64Studio.Types.FileType.PRG ) )
        {
          RefreshFileView();
          UpdateStatusInfo();
        }
      }
    }



    private void labelMediaTitle_Click( object sender, EventArgs e )
    {
      if ( ( m_Media != null )
      &&   ( m_Media.SupportsRenamingTitle ) )
      {
        var disk = (Disk)m_Media;

        var diskID = new GR.Memory.ByteBuffer();
        diskID.AppendU16NetworkOrder( disk.DiskID );
        var renameDisk = new FormRenameDisk( Core, disk.DiskName, diskID );
        if ( renameDisk.ShowDialog() != DialogResult.OK )
        {
          return;
        }
        disk.DiskName = renameDisk.DiskName;
        disk.DiskID   = renameDisk.DiskID.UInt16NetworkOrderAt( 0 );
        RefreshFileView();
        SetModified();
      }
    }



    private void importDirArtFilesToolStripMenuItem_Click( object sender, EventArgs e )
    {
      var importDirArt = new DlgImportDirArt( Core );

      if ( importDirArt.ShowDialog() != DialogResult.OK )
      {
        return;
      }

      int     pos = 0;
      bool    changed = false;

      while ( pos < importDirArt.ResultingData.Length )
      {
        int     len = 16;
        if ( pos + len > importDirArt.ResultingData.Length )
        {
          len = (int)importDirArt.ResultingData.Length - pos;
        }
        var emptyFile = new ByteBuffer();
        var sourceName = importDirArt.ResultingData.SubBuffer( pos, len );
        var fileName = new ByteBuffer();

        for ( int i = 0; i < sourceName.Length; ++i )
        {
          byte scrCode = sourceName.ByteAt( i );
          if ( !Types.ConstantData.ScreenCodeToChar[scrCode].HasPetSCII )
          {
            Debug.Log( "Missing PETSCII!! for " + scrCode );
          }

          fileName.AppendU8( Types.ConstantData.ScreenCodeToChar[scrCode].PetSCIIValue );
        }
        if ( m_Media.WriteFile( fileName, emptyFile, C64Studio.Types.FileType.USR ) )
        {
          changed = true;
        }
        pos += 16;
      }

      if ( changed )
      {
        RefreshFileView();
        UpdateStatusInfo();
      }
    }



  }
}
