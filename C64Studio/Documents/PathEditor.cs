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
using static RetroDevStudio.Formats.PathProject;



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

      SetDefaultDescriptors();
      FillMappings();
    }



    private void FillMappings()
    {
      listMappings.BeginUpdate();

      listMappings.Items.Clear();
      foreach ( var mapping in _project.ValueDescriptors )
      {
        var item = new ArrangedItemEntry();

        // TODO - duration info
        item.Text = StepMappingToText( mapping );
        item.Tag = mapping;
        listMappings.Items.Add( item );
      }

      listMappings.EndUpdate();
    }



    private string StepMappingToText( PathProject.ValueDescriptor mapping )
    {
      return $"{mapping.Step} byte {mapping.AddressOffsetStep}";
    }



    private void SetDefaultDescriptors()
    {
      _project.ValueDescriptors.Clear();
      _project.SetDefaultDescriptors();
    }



    public override void OnApplicationEvent( ApplicationEvent Event )
    {
      base.OnApplicationEvent( Event );
    }



    public PathEditor( StudioCore core )
    {
      Core = core;
      InitializeComponent();

      foreach ( PathProject.StepType step in Enum.GetValues( typeof( PathProject.StepType ) ) )
      {
        comboStepTypes.Items.Add( new GR.Generic.Tupel<PathProject.StepType, string>( step, GR.EnumHelper.GetDescription( step ) ) );
      }
      comboStepTypes.SelectedIndex = 0;

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


      FillPathList();
      FillMappings();
      return true;
    }



    protected override bool QueryFilename( string PreviousFilename, out string Filename )
    {
      Filename = "";

      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Save Path Project as";
      saveDlg.Filter = "Path Projects|*.pathproject|All Files|*.*";
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



    private void FillPathList()
    {
      _updatingParams = true;
      listPaths.BeginUpdate();
      listPaths.Items.Clear();

      foreach ( var path in _project.Paths )
      {
        var item = new ArrangedItemEntry( path.Name );
        item.Tag = path;
        listPaths.Items.Add( item );
      }
      listPaths.EndUpdate();
      _updatingParams = false;
    }



    private void FillPathStepList()
    {
      _updatingParams = true;
      listPathSteps.BeginUpdate();
      listPathSteps.Items.Clear();

      if ( listPaths.SelectedIndex != -1 )
      {
        var pathSteps = (PathProject.Path)listPaths.SelectedItem.Tag;

        foreach ( var step in pathSteps.Steps )
        {
          var item = new ArrangedItemEntry( GR.EnumHelper.GetDescription( step.Type ) + ": " + step.Duration );
          item.Tag = step;
          listPathSteps.Items.Add( item );
        }
      }
      listPathSteps.EndUpdate();
      _updatingParams = false;
    }



    private void editPathName_TextChanged( object sender, EventArgs e )
    {
      if ( _updatingParams )
      {
        return;
      }

      listPaths.AddButtonEnabled = !string.IsNullOrEmpty( editPathName.Text );
      if ( listPaths.SelectedItem != null )
      {
        var path = (PathProject.Path)listPaths.SelectedItem.Tag;
        if ( path.Name != editPathName.Text )
        {
          path.Name = editPathName.Text;
          listPaths.SelectedItem.Text = editPathName.Text;
          listPaths.Invalidate();
          SetModified();
        }
      }
    }



    private void listPaths_ItemAdded( object sender, ArrangedItemEntry Item )
    {
      if ( _updatingParams )
      {
        return;
      }
      if ( Item.Tag == null )
      {
        var newPath = new PathProject.Path();
        newPath.Name = editPathName.Text;
        _project.Paths.Add( newPath );

        Item.Tag = newPath;
        Item.Text = newPath.Name;
      }

      SetModified();
    }



    private void listPaths_ItemMoved( object sender, ArrangedItemEntry Item, int originalIndex )
    {
      RebuildPathList();
    }



    private void listPaths_ItemRemoved( object sender, ArrangedItemEntry Item )
    {
      RebuildPathList();
    }



    private void RebuildPathList()
    {
      var paths = new List<PathProject.Path>();
      foreach ( var item in listPaths.Items )
      {
        paths.Add( (PathProject.Path)( (ArrangedItemEntry)item ).Tag );
      }
      _project.Paths = paths;
    }



    private void RebuildPathStepList()
    {
      if ( listPaths.SelectedItem == null )
      {
        return;
      }
      var path = (PathProject.Path)listPaths.SelectedItem.Tag;

      path.Steps.Clear();
      foreach ( ArrangedItemEntry item in listPathSteps.Items )
      {
        var step = (PathProject.Step)item.Tag;

        path.Steps.Add( step );
      }
    }



    private void listPaths_SelectedIndexChanged( object sender, ArrangedItemEntry Item )
    {
      bool  enableStepList = listPaths.SelectedIndex != -1;

      labelStepType.Enabled = enableStepList;
      comboStepTypes.Enabled = enableStepList;
      labelStepLength.Enabled = enableStepList;
      editStepLength.Enabled = enableStepList;
      listPathSteps.Enabled = enableStepList;

      FillPathStepList();

      if ( listPaths.SelectedItem != null )
      {
        var path = (PathProject.Path)listPaths.SelectedItem.Tag;

        editPathName.Text = path.Name;
      }
      RedrawPathPreview();
    }



    private void listPathSteps_ItemAdded( object sender, ArrangedItemEntry Item )
    {
      if ( _updatingParams )
      {
        return;
      }
      if ( listPaths.SelectedItem == null )
      {
        return;
      }
      var path = (PathProject.Path)listPaths.SelectedItem.Tag;

      var newStep = new PathProject.Step()
      {
        Type = ( (GR.Generic.Tupel<PathProject.StepType, string>)comboStepTypes.SelectedItem ).first,
        Duration = GR.Convert.ToI32( editStepLength.Text )
      };
      path.Steps.Add( newStep );

      Item.Text = GR.EnumHelper.GetDescription( newStep.Type ) + ": " + newStep.Duration;
      Item.Tag = newStep;
      SetModified();
      RedrawPathPreview();
    }



    private void RedrawPathPreview()
    {
      if ( listPaths.SelectedItem == null )
      {
        pictureEditor.DisplayPage.Box( 0, 0, pictureEditor.DisplayPage.Width, pictureEditor.DisplayPage.Height, 0xff000000 );
        return;
      }
      var path = (PathProject.Path)listPaths.SelectedItem.Tag;
      var min = new GR.Math.Point();
      var max = new GR.Math.Point();

      int curX = 0;
      int curY = 0;
      foreach ( var step in path.Steps )
      {
        step.AdvancePosition( ref curX, ref curY );
        min.X = Math.Min( min.X, curX );
        min.Y = Math.Min( min.Y, curY );
        max.X = Math.Max( max.X, curX );
        max.Y = Math.Max( max.Y, curY );
      }

      pictureEditor.DisplayPage.Box( 0, 0, pictureEditor.DisplayPage.Width, pictureEditor.DisplayPage.Height, 0xff000000 );
      if ( path.Steps.Count > 0 )
      {
        int   fullWidth = max.X - min.X + 1;
        int   fullHeight = max.Y - min.Y + 1;

        int   maxSize = Math.Max( fullHeight, fullWidth );

        curX = 0;
        curY = 0;
        int inset = 10;
        int availableWidth = pictureEditor.DisplayPage.Width - 2 * inset;
        int availableHeight = pictureEditor.DisplayPage.Height - 2 * inset;

        // Use the same uniform scale factor for both X and Y
        float scale = Math.Min( (float)availableWidth / maxSize, (float)availableHeight / maxSize );

        // Center the scaled path in the available space
        int scaledWidth = (int)( fullWidth * scale );
        int scaledHeight = (int)( fullHeight * scale );
        int offsetX = inset + ( availableWidth - scaledWidth ) / 2;
        int offsetY = inset + ( availableHeight - scaledHeight ) / 2;

        foreach ( var step in path.Steps )
        {
          int prevX = curX;
          int prevY = curY;

          step.AdvancePosition( ref curX, ref curY );

          int finalX1 = (int)( ( prevX - min.X ) * scale );
          int finalY1 = (int)( ( prevY - min.Y ) * scale );
          int finalX2 = (int)( ( curX - min.X ) * scale );
          int finalY2 = (int)( ( curY - min.Y ) * scale );
          pictureEditor.DisplayPage.Rectangle( offsetX + finalX1 - 2,
                                               offsetY + finalY1 - 2,
                                               5, 5, 0xffffffff );
          pictureEditor.DisplayPage.Rectangle( offsetX + finalX2 - 2,
                                               offsetY + finalY2 - 2,
                                               5, 5, 0xffffffff );
          pictureEditor.DisplayPage.Line( offsetX + finalX1,
                                          offsetY + finalY1,
                                          offsetX + finalX2,
                                          offsetY + finalY2,
                                          0xffff00ff );
        }
      }
      pictureEditor.Invalidate();
    } 



    private void listPathSteps_ItemMoved( object sender, ArrangedItemEntry Item, int originalIndex )
    {
      RebuildPathStepList();
      SetModified();
      RedrawPathPreview();
    }



    private void listPathSteps_ItemRemoved( object sender, ArrangedItemEntry Item )
    {
      RebuildPathStepList();
      SetModified();
      RedrawPathPreview();
    }



    private void listPathSteps_SelectedIndexChanged( object sender, ArrangedItemEntry Item )
    {
      if ( _updatingParams )
      {
        return;
      }
      if ( ( listPaths.SelectedItem == null )
      ||   ( listPathSteps.SelectedItem == null ) )
      {
        return;
      }
      var path = (PathProject.Path)listPaths.SelectedItem.Tag;
      var step = (PathProject.Step)listPathSteps.SelectedItem.Tag;

      editStepLength.Text = step.Duration.ToString();
      comboStepTypes.SelectedIndex = (int)step.Type;
    }



    private void editStepLength_TextChanged( object sender, EventArgs e )
    {
      if ( _updatingParams )
      {
        return;
      }

      if ( ( listPaths.SelectedItem == null )
      ||   ( listPathSteps.SelectedItem == null ) )
      {
        return;
      }
      var step = (PathProject.Step)listPathSteps.SelectedItem.Tag;
      int newValue = GR.Convert.ToI32( editStepLength.Text );
      if ( step.Duration != newValue )
      {
        step.Duration = newValue;
        listPathSteps.SelectedItem.Text = GR.EnumHelper.GetDescription( step.Type ) + ": " + step.Duration;
        SetModified();
        RedrawPathPreview();
      }
    }



    private void comboStepTypes_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( _updatingParams )
      {
        return;
      }

      if ( ( listPaths.SelectedItem == null )
      ||   ( listPathSteps.SelectedItem == null ) )
      {
        return;
      }

      var step = (PathProject.Step)listPathSteps.SelectedItem.Tag;
      if ( step.Type != (StepType)comboStepTypes.SelectedIndex )
      {
        step.Type = (StepType)comboStepTypes.SelectedIndex;
        listPathSteps.SelectedItem.Text = GR.EnumHelper.GetDescription( step.Type ) + ": " + step.Duration;
        SetModified();
        RedrawPathPreview();
      }
    }



    private ArrangedItemEntry listPaths_CloningItem( object sender, ArrangedItemEntry Item )
    {
      var path = (PathProject.Path)Item.Tag;

      var clonedItem =new ArrangedItemEntry() { Text = Item.Text };
      var clonedPath = new PathProject.Path() { Name = path.Name };

      // clone steps as well
      foreach ( var step in path.Steps )
      {
        clonedPath.Steps.Add( new Step() { Type = step.Type, Duration = step.Duration } );
      }
      clonedItem.Tag = clonedPath;
      return clonedItem;
    }



  }
}
