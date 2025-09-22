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
      List<SampleProject> samples = new List<SampleProject>();
      
      // TODO: Load sample projects here
      // This is where you would load sample projects from disk or a resource
      // For now adding some dummy data
      samples.Add(new SampleProject 
      { 
        Name = "Sample Project 1",
        Description = "A basic C64 project example",
        URL = "samples/project1"
      });
      samples.Add( new SampleProject
      {
        Name = "Sample Project 2",
        Description = "A basic C64 project example",
        URL = "samples/project2"
      } );
      samples.Add( new SampleProject
      {
        Name = "Sample Project 3",
        Description = "A basic C64 project example",
        URL = "samples/project3"
      } );

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
        if (samples != null)
        {
          gridSamples.Items.Clear();
          foreach (SampleProject sample in samples)
          {
            gridSamples.Items.Add(new DecentForms.GridList.GridListItem 
            { 
              Text = sample.Name,
              Tag = sample
            });
          }
        }
      }
    }



    private void gridSamples_CustomEventHandler( DecentForms.ControlBase Sender, DecentForms.GridList.GridListItem item, DecentForms.ControlEvent e )
    {
      //Debug.Log(string.Format("SampleExplorer: Event {0} on item {1}", e.Type, item.Text));
    }



    private void gridSamples_DrawItem( DecentForms.ControlBase sender, DecentForms.GridListItemEventArgs e )
    {
      var sample = e.Item.Tag as SampleProject;

      e.Renderer.FillRectangle( e.Bounds, 0xffffffff );
      e.Renderer.DrawRectangle( e.Bounds, 0xff000000 );
      e.Renderer.DrawText( _titleFont, sample.Name, e.Bounds.X + 2, e.Bounds.Y + 2, e.Bounds.Width - 4, e.Bounds.Height, DecentForms.TextAlignment.LEFT | DecentForms.TextAlignment.TOP, 0xff000000 );

      e.Renderer.DrawText( sample.Description, e.Bounds.X + 4, e.Bounds.Y + 20, e.Bounds.Width - 8, e.Bounds.Height - 20, DecentForms.TextAlignment.LEFT | DecentForms.TextAlignment.TOP, 0xff000000 );
      e.Renderer.DrawImage( sample.Image, e.Bounds.X + e.Bounds.Width - 132, e.Bounds.Y + 4, 128, 96 );
    }



    private void SampleExplorer_Load( object sender, EventArgs e )
    {
      if (!sampleLoader.IsBusy)
      {
        sampleLoader.RunWorkerAsync();
      }
    }
  }
}
