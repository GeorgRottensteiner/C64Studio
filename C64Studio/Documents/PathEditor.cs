using GR.Memory;
using RetroDevStudio;
using RetroDevStudio.Audio;
using RetroDevStudio.Controls;
using RetroDevStudio.Formats;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;



namespace RetroDevStudio.Documents
{
  public partial class PathEditor : BaseDocument
  {
    private PathProject               _project = new PathProject();

    private bool                      _updatingParams = false;



    public PathEditor()
    {
      InitializeComponent();

      GR.Image.DPIHandler.ResizeControlsForDPI( this );
    }



    public override void OnApplicationEvent( ApplicationEvent Event )
    {
      base.OnApplicationEvent( Event );
    }



    public PathEditor( StudioCore core )
    {
      Core = core;
      InitializeComponent();
      GR.Image.DPIHandler.ResizeControlsForDPI( this );
    }



    protected override bool PerformSave( string FullPath )
    {
      return GR.IO.File.WriteAllBytes( FullPath, _project.SaveToBuffer() );
    }



    public override bool LoadDocument()
    {
      if ( string.IsNullOrEmpty( DocumentInfo.DocumentFilename ) )
      {
        return false;
      }
      try
      {
        if ( !OpenProject( DocumentInfo.FullPath ) )
        {
          return false;
        }
      }
      catch ( System.IO.IOException ex )
      {
        Core.Notification.MessageBox( "Could not load file", "Could not load path project file " + DocumentInfo.FullPath + ".\r\n" + ex.Message );
        return false;
      }
      SetUnmodified();
      return true;
    }



    private bool OpenProject( string fullPath )
    {
      var data = GR.IO.File.ReadAllBytes( fullPath );

      if ( !_project.ReadFromBuffer( data ) )
      {
        return false;
      }


      _updatingParams = true;
      listPaths.Items.Clear();
      /*
      foreach ( var effect in _project.Effects )
      {
        var item = new ArrangedItemEntry( effect.Name );
        item.Tag = effect;

        listPaths.Items.Add( item );
      }*/

      _updatingParams = false;
      return true;
    }



    protected override bool QueryFilename( string PreviousFilename, out string Filename )
    {
      Filename = "";

      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Save SFX Project as";
      saveDlg.Filter = "SFX Projects|*.sfxproject|All Files|*.*";
      saveDlg.FileName = GR.Path.GetFileName( PreviousFilename );
      if ( DocumentInfo.Project != null )
      {
        saveDlg.InitialDirectory = DocumentInfo.Project.Settings.BasePath;
      }
      if ( saveDlg.ShowDialog() != DialogResult.OK )
      {
        return false;
      }

      Filename = saveDlg.FileName;
      return true;
    }



    private void editPathName_TextChanged( object sender, EventArgs e )
    {
      listPaths.AddButtonEnabled = !string.IsNullOrEmpty( editPathName.Text );
    }



  }
}
