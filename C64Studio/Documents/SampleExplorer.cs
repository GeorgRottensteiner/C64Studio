using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using RetroDevStudio.Types;
using System.Drawing;
using DecentForms;
using System.Linq;
using RetroDevStudio.Dialogs;
using RetroDevStudio.Controls;

namespace RetroDevStudio.Documents
{
  public partial class SampleExplorer : BaseDocument
  {
    private BackgroundWorker  sampleLoader;
    private Font              _titleFont;



    public SampleExplorer( StudioCore Core )
    {
      this.Core = Core;
      HideOnClose = true;

      _titleFont = new Font( Font.Name, Font.Size + 2, FontStyle.Bold );

      InitializeComponent();
      InitializeSampleLoader();

      /*
      gridSamples.Items.Add( new DecentForms.GridList.GridListItem() { Text = "Sample 1" } );
      gridSamples.Items.Add( new DecentForms.GridList.GridListItem() { Text = "Sample 2" } );
      gridSamples.Items.Add( new DecentForms.GridList.GridListItem() { Text = "Sample 3" } );*/
    }



    private void InitializeSampleLoader()
    {
      sampleLoader = new BackgroundWorker();
      sampleLoader.DoWork += SampleLoader_DoWork;
      sampleLoader.RunWorkerCompleted += SampleLoader_RunWorkerCompleted;
      sampleLoader.WorkerSupportsCancellation = true;
    }



    private void SampleLoader_DoWork(object sender, DoWorkEventArgs e)
    {
#if DEBUG
      string    sampleBasePath = @"../../../../C64StudioRelease/shared content/Sample Projects";
#else
      string    sampleBasePath = @"Sample Projects";
#endif

      sampleBasePath = System.IO.Path.GetFullPath( sampleBasePath );
      var folders = System.IO.Directory.GetDirectories( sampleBasePath );
      var samples = new List<SampleProject>();
      foreach ( var systemfolder in folders )
      {
        var sysFolderName = GR.Path.GetFileName( systemfolder ).ToUpper();
        if ( !Enum.GetNames( typeof( MachineType ) ).Contains( sysFolderName ) )
        {
          //Debug.Log( $"Unsupported sample machine type {sysFolderName}" );
          continue; 
        }
        var machine = (MachineType)System.Enum.Parse( typeof( MachineType ), sysFolderName, true );

        var sampleFolders = System.IO.Directory.GetDirectories( systemfolder );
        foreach ( var sampleFolder in sampleFolders )
        {
          var metaFile = GR.Path.Append( sampleFolder, "metadata.xml" );
          var thumbFile = GR.Path.Append( sampleFolder, "thumbnail.png" );

          var parser = new GR.Strings.XMLParser();

          if ( parser.Parse( GR.IO.File.ReadAllText( metaFile ), false ) )
          {
            var xmlSample = parser.FindByType( "Sample" );
            if ( xmlSample != null ) 
            {
              var project = new SampleProject()
              {
                Machine           = machine,
                SourceFolder      = sampleFolder,
                Name              = xmlSample.Attribute( "Name" ),
                ShortDescription  = xmlSample.Attribute( "ShortDescription" ),
                LongDescription   = xmlSample.Attribute( "Desc" )
              };
              try
              {
                project.Image = Bitmap.FromFile( thumbFile );
              }
              catch ( Exception )
              {
              }

              samples.Add( project );
            }
          }
        }
      }
      e.Result = samples;
    }



    private void SampleLoader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Error != null)
      {
        Core.AddToOutputLine(string.Format("Error loading samples: {0}", e.Error.Message));
        return;
      }

      if (!e.Cancelled)
      {
        List<SampleProject> samples = e.Result as List<SampleProject>;
        if ( samples != null )
        {
          gridSamples.Items.Clear();
          foreach ( SampleProject sample in samples )
          {
            gridSamples.Items.Add( new DecentForms.GridList.GridListItem 
            { 
              Text = sample.Name,
              Tag = sample
            });
          }
        }
      }
    }



    private Rectangle GetSampleLinkRect( DecentForms.GridList.GridListItem item )
    {
      var bounds = gridSamples.GetItemRect( item.Index );

      return new Rectangle( bounds.X + bounds.Width / 2 - 18,
                            bounds.Bottom - 24,
                            2 * 18,
                            28 - 9 );
    }



    private void gridSamples_CustomEventHandler( DecentForms.ControlBase Sender, DecentForms.GridList.GridListItem item, DecentForms.ControlEvent e )
    {
      switch ( e.Type )
      {
        case DecentForms.ControlEvent.EventType.SET_CURSOR:
          {
            var bounds = GetSampleLinkRect( item );
            if ( bounds.Contains( e.MouseX, e.MouseY ) )
            {
              Sender.SetCursor( DecentForms.ControlBase.CursorType.CURSOR_HAND );
              e.Handled = true;
              return;
            }
          }
          break;
        case DecentForms.ControlEvent.EventType.MOUSE_DOWN:
          {
            var bounds = GetSampleLinkRect( item );
            if ( bounds.Contains( e.MouseX, e.MouseY ) )
            {
              SetupSample( ( (SampleProject)item.Tag ) );
            }
          }
          break;
      }
      //Debug.Log(string.Format("SampleExplorer: Event {0} on item {1}", e.Type, item.Text));
    }



    private void SetupSample( SampleProject sample )
    {
      if ( !Core.MainForm.CloseSolution() )
      {
        return;
      }
      var project = Core.MainForm.AddNewSolution( sample.Name );
      if ( project == null )
      {
        return;
      }
      var solutionFilename = Core.Navigating.Solution.Filename;
      var projectBasePath = project.Settings.BasePath;
      var projectFilename = project.Settings.Filename;
      Core.MainForm.CloseSolution();

      // copy all files
      var sampleFiles = System.IO.Directory.GetFiles( sample.SourceFolder );

      foreach ( var file in sampleFiles )
      {
        string targetFilename = "";
        try
        {
          var pureFilename = GR.Path.GetFileName( file );

          // skip sample explorer files
          if ( ( pureFilename == "metadata.xml" )
          ||   ( pureFilename == "thumbnail.png" )
          ||   ( GR.Path.GetExtension( pureFilename ).ToUpper() == ".S64" ) )
          {
            continue;
          }
          targetFilename = GR.Path.Append( project.Settings.BasePath, pureFilename );
          if ( GR.Path.GetExtension( targetFilename ).ToUpper() == ".C64" )
          {
            // the project file may be renamed
            targetFilename = projectFilename;
          }

          System.IO.File.Copy( file, targetFilename, true );
        }
        catch ( Exception ex )
        {
          Core.MessageBox( $"Could not copy file {file} to the target folder as {targetFilename}:\r\n{ex.Message}", "An error occurred" );
        }
      }

      // re-open again (should load with new project now)
      if ( !Core.MainForm.OpenSolution( solutionFilename ) )
      {
        return;
      }

      // add files to project
      foreach ( var file in sampleFiles )
      {
        string targetFilename = "";
        var pureFilename = GR.Path.GetFileName( file );

        // skip sample explorer files
        if ( ( pureFilename == "metadata.xml" )
        ||   ( pureFilename == "thumbnail.png" )
        ||   ( GR.Path.GetExtension( pureFilename ).ToUpper() == ".S64" )
        ||   ( GR.Path.GetExtension( pureFilename ).ToUpper() == ".C64" ) )
        {
          continue;
        }
        targetFilename = GR.Path.Append( project.Settings.BasePath, pureFilename );
        Core.MainForm.AddExistingFileToProject( project, project.Node, targetFilename, true, false );
      }

      Core.MainForm.SaveSolution();
      if ( Core.MainForm.SaveProject( project ) )
      {
        if ( global::SourceControl.Controller.IsFolderUnderSourceControl( project.FullPath( "" ) ) )
        {
          foreach ( var file in sampleFiles )
          {
            var pureFilename = GR.Path.GetFileName( file );

            // skip sample explorer files
            if ( ( pureFilename == "metadata.xml" )
            ||   ( pureFilename == "thumbnail.png" )
            ||   ( GR.Path.GetExtension( pureFilename ).ToUpper() == ".S64" )
            ||   ( GR.Path.GetExtension( pureFilename ).ToUpper() == ".C64" ) )
            {
              continue;
            }

            string targetFilename = GR.Path.Append( project.Settings.BasePath, pureFilename );
            project.SourceControl.AddFileToRepository( GR.Path.GetFileName( targetFilename ) );
          }
          project.SourceControl.StageAllChanges();
          project.SourceControl.CommitAllChanges( Core.Settings.SourceControlInfo.CommitAuthor, Core.Settings.SourceControlInfo.CommitAuthorEmail, "Sample Setup" );

          // force source control update of Solution Explorer
          Core.MainForm.RaiseApplicationEvent( new ApplicationEvent( ApplicationEvent.Type.SOURCE_CONTROL_STATE_MODIFIED ) );
        }
      }
    }



    private void gridSamples_DrawItem( DecentForms.ControlBase sender, DecentForms.GridListItemEventArgs e )
    {
      var sample = e.Item.Tag as SampleProject;

      e.Renderer.FillRectangle( e.Bounds, 0xffffffff );
      e.Renderer.DrawRectangle( e.Bounds, 0xff000000 );
      e.Renderer.DrawText( _titleFont, sample.Name, e.Bounds.X + 2, e.Bounds.Y + 2, e.Bounds.Width - 4, e.Bounds.Height, DecentForms.TextAlignment.LEFT | DecentForms.TextAlignment.TOP, 0xff000000 );

      e.Renderer.DrawText( sample.ShortDescription, e.Bounds.X + 4, e.Bounds.Y + 20, e.Bounds.Width - 8, e.Bounds.Height - 20, DecentForms.TextAlignment.LEFT | DecentForms.TextAlignment.TOP, 0xff000000 );
      e.Renderer.DrawImage( sample.Image, e.Bounds.X + e.Bounds.Width - 132, e.Bounds.Y + 4, 128, 96 );

      e.Renderer.DrawText( sample.Machine.ToString(), e.Bounds.X + 4, e.Bounds.Bottom - 26, 100, 20, DecentForms.TextAlignment.LEFT | DecentForms.TextAlignment.CENTERED_V, 0xff000000 );

      var bounds = GetSampleLinkRect( e.Item );
      e.Renderer.DrawText( "Create", e.Bounds.X, e.Bounds.Bottom - 28, e.Bounds.Width, 20, DecentForms.TextAlignment.CENTERED_H | DecentForms.TextAlignment.BOTTOM, 0xff4040ff );
      e.Renderer.DrawLine( e.Bounds.X + e.Bounds.Width / 2 - 18, e.Bounds.Bottom - 9, e.Bounds.X + e.Bounds.Width / 2 + 18, e.Bounds.Bottom - 9, 0xff4040ff );

      DrawSampleText( e.Renderer, e.Bounds, sample.LongDescription );
    }



    private void DrawSampleText( ControlRenderer renderer, Rectangle bounds, string longDescription )
    {
      var textBounds1 = new Rectangle( bounds.X + 4, bounds.Y + 40, bounds.Width - 132 - 2 * 4, 104 - 40 );
      var textBounds2 = new Rectangle( bounds.X + 4, bounds.Y + 104, bounds.Width - 2 * 4, bounds.Height - 20 - 104 );

      var rects = new List<Rectangle>() { textBounds1, textBounds2 };

      renderer.DrawWrappedText( longDescription, rects );
    }



    private void SampleExplorer_Load( object sender, EventArgs e )
    {
      if ( !sampleLoader.IsBusy )
      {
        sampleLoader.RunWorkerAsync();
      }
    }



    private void editSampleFilter_TextChanged( object sender, EventArgs e )
    {
      if ( sampleLoader.IsBusy )
      {
        return;
      }

      gridSamples.BeginUpdate();
      foreach ( var item in gridSamples.Items )
      {
        if ( item.Tag is SampleProject sample )
        {
          item.Visible = ( editSampleFilter.Text.Length == 0 )
            || ( sample.Name.IndexOf( editSampleFilter.Text, StringComparison.InvariantCultureIgnoreCase ) != -1 )
            || ( sample.ShortDescription.IndexOf( editSampleFilter.Text, StringComparison.InvariantCultureIgnoreCase ) != -1 );
        }
      }
      gridSamples.EndUpdate();
    }



  }
}
