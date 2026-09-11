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

    private ExportPathFormBase        _ExportForm = null;



    public PathEditor()
    {
      InitializeComponent();

      GR.Image.DPIHandler.ResizeControlsForDPI( this );
    }



    public PathEditor( StudioCore core )
    {
      Core = core;
      InitializeComponent();

      DocumentInfo.Type = ProjectElement.ElementType.PATH_EDITOR;
      DocumentInfo.UndoManager.MainForm = Core.MainForm;

      _updatingParams = true;
      foreach ( PathProject.StepType step in Enum.GetValues( typeof( PathProject.StepType ) ) )
      {
        comboStepTypes.Items.Add( new GR.Generic.Tupel<PathProject.StepType, string>( step, GR.EnumHelper.GetDescription( step ) ) );
        comboMappingStepType.Items.Add( new GR.Generic.Tupel<PathProject.StepType, string>( step, GR.EnumHelper.GetDescription( step ) ) );
      }
      comboStepTypes.SelectedIndex = 0;
      comboMappingStepType.SelectedIndex = 0;

      comboExportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "as assembly", typeof( ExportPathAsAssembly ) ) );
      comboExportMethod.Items.Add( new GR.Generic.Tupel<string, Type>( "as binary file", typeof( ExportPathAsFile ) ) );

      comboExportMethod.SelectedIndex = 0;

      SetDefaultDescriptors();
      FillMappings();

      _updatingParams = false;

      GR.Image.DPIHandler.ResizeControlsForDPI( this );
    }



    private void FillMappings()
    {
      listMappings.BeginUpdate();

      listMappings.Items.Clear();

      int totalNumberOfBytes = _project.DetermineTotalNumberOfBytes();

      foreach ( var mapping in _project.ValueDescriptors )
      {
        var item = new ArrangedItemEntry();

        item.Text = GenerateStepMappingToText( mapping, totalNumberOfBytes );
        item.Tag = mapping;
        listMappings.Items.Add( item );
      }

      listMappings.EndUpdate();
    }



    private bool IsMappingValid( out List<PathProject.ValueDescriptor> overlappingInMappings, out GR.Collections.Set<PathProject.ValueDescriptor> overlappingBetweenMappings )
    {
      overlappingInMappings = new List<PathProject.ValueDescriptor>();
      overlappingBetweenMappings = new GR.Collections.Set<PathProject.ValueDescriptor>();

      int totalNumberOfBytes = _project.DetermineTotalNumberOfBytes();

      foreach ( var mapping in _project.ValueDescriptors )
      {
        int bitCount = CountBits( mapping.RelevantBitsStep, out int highestBit, out int lowestBit );
        var usedBits = new GR.Collections.Set<int>();
        for ( int i = 0; i < bitCount; ++i )
        {
          int bitIndex = mapping.AddressOffsetStep * 8 + i + 7 - highestBit;

          if ( usedBits.Contains( bitIndex ) )
          {
            overlappingInMappings.Add( mapping );
          }
          usedBits.Add( bitIndex );
        }

        bitCount = CountBits( mapping.RelevantBitsDuration, out highestBit, out lowestBit );
        for ( int i = 0; i < bitCount; ++i )
        {
          int bitIndex = mapping.AddressOffsetDuration * 8 + i + 7 - highestBit;

          if ( usedBits.Contains( bitIndex ) )
          {
            overlappingInMappings.Add( mapping );
          }
          usedBits.Add( bitIndex );
        }

        bitCount = CountBits( _project.ValueLastFlag, out highestBit, out lowestBit );
        for ( int i = 0; i < bitCount; ++i )
        {
          int bitIndex = _project.AddressOffsetLastFlag * 8 + i + 7 - highestBit;

          if ( usedBits.Contains( bitIndex ) )
          {
            overlappingInMappings.Add( mapping );
          }
          usedBits.Add( bitIndex );
        }

        foreach ( var otherMapping in _project.ValueDescriptors )
        {
          if ( mapping == otherMapping )
          {
            continue;
          }
          bitCount = CountBits( mapping.RelevantBitsStep, out highestBit, out lowestBit );
          for ( int i = 0; i < bitCount; ++i )
          {
            int bitIndex = mapping.AddressOffsetStep * 8 + i + 7 - highestBit;

            if ( usedBits.Contains( bitIndex ) )
            {
              overlappingBetweenMappings.Add( mapping );
              overlappingBetweenMappings.Add( otherMapping );
            }
          }

          bitCount = CountBits( mapping.RelevantBitsDuration, out highestBit, out lowestBit );
          for ( int i = 0; i < bitCount; ++i )
          {
            int bitIndex = mapping.AddressOffsetDuration * 8 + i + 7 - highestBit;

            if ( usedBits.Contains( bitIndex ) )
            {
              overlappingBetweenMappings.Add( mapping );
              overlappingBetweenMappings.Add( otherMapping );
            }
          }

          bitCount = CountBits( _project.ValueLastFlag, out highestBit, out lowestBit );
          for ( int i = 0; i < bitCount; ++i )
          {
            int bitIndex = _project.AddressOffsetLastFlag * 8 + i + 7 - highestBit;

            if ( usedBits.Contains( bitIndex ) )
            {
              overlappingBetweenMappings.Add( mapping );
              overlappingBetweenMappings.Add( otherMapping );
            }
          }
        }
      }
      
      return ( ( overlappingInMappings.Count == 0 ) 
        &&     ( overlappingBetweenMappings.Count == 0 ) );
    }



    private string GenerateMaskString( PathProject.ValueDescriptor mapping, int totalNumberOfBytes )
    {
      string    fullBits = new string( '*', totalNumberOfBytes * 8 );

      int bitCount = CountBits( mapping.RelevantBitsStep, out int highestBit, out int lowestBit );
      for ( int i = 0; i < bitCount; ++i )
      {
        int bitIndex = mapping.AddressOffsetStep * 8 + i + 7 - highestBit;
        fullBits = fullBits.Substring( 0, bitIndex ) + "S" + fullBits.Substring( bitIndex + 1 );
      }

      bitCount = CountBits( mapping.RelevantBitsDuration, out highestBit, out lowestBit );
      for ( int i = 0; i < bitCount; ++i )
      {
        int bitIndex = mapping.AddressOffsetDuration * 8 + i + 7 - highestBit;
        fullBits = fullBits.Substring( 0, bitIndex ) + "D" + fullBits.Substring( bitIndex + 1 );
      }

      bitCount = CountBits( _project.ValueLastFlag, out highestBit, out lowestBit );
      for ( int i = 0; i < bitCount; ++i )
      {
        int bitIndex = _project.AddressOffsetLastFlag * 8 + i + 7 - highestBit;
        fullBits = fullBits.Substring( 0, bitIndex ) + "L" + fullBits.Substring( bitIndex + 1 );
      }

      string finalString = "";
      for ( int i = 0; i < totalNumberOfBytes; ++i )
      {
        finalString += fullBits.Substring( i * 8, 8 );
        if ( i + 1 < totalNumberOfBytes )
        {
          finalString += " ";
        }
      }
      return finalString;
    }



    private string GenerateStepMappingToText( PathProject.ValueDescriptor mapping, int totalNumberOfBytes )
    {
      string maskString = GenerateMaskString( mapping, totalNumberOfBytes );

      return $"{GR.EnumHelper.GetDescription( mapping.Step )} byte {mapping.AddressOffsetStep}: {maskString}";
    }



    private int CountBits( uint relevantBitsStep, out int highestBit, out int lowestBit )
    {
      int count = 0;
      lowestBit = int.MaxValue;
      highestBit = int.MinValue;
      for ( int i = 0; i < 32; ++i )
      {
        if ( ( relevantBitsStep & ( 1 << i ) ) != 0 )
        {
          if ( lowestBit == int.MaxValue )
          {
            lowestBit = i;
          }
          count++;
          highestBit = i;
        }
      }
      return count;
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
      editMappingLastStepAddressOffset.Text = _project.AddressOffsetLastFlag.ToString( "X2" );
      editMappingLastStepValue.Text = _project.ValueLastFlag.ToString( "X2" );
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
      else
      {
        _project.Paths.Add( (PathProject.Path)Item.Tag );
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
      || ( listPathSteps.SelectedItem == null ) )
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
      || ( listPathSteps.SelectedItem == null ) )
      {
        return;
      }
      var step = (PathProject.Step)listPathSteps.SelectedItem.Tag;
      int newValue = GR.Convert.ToI32( editStepLength.Text );
      if ( step.Duration != newValue )
      {
        step.Duration = newValue;

        var path = (PathProject.Path)listPaths.SelectedItem.Tag;
        int totalNumberOfBytes = _project.DetermineTotalNumberOfBytes();
        listPathSteps.SelectedItem.Text = GenerateStepDescription( step, totalNumberOfBytes );
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
      || ( listPathSteps.SelectedItem == null ) )
      {
        return;
      }

      var step = (PathProject.Step)listPathSteps.SelectedItem.Tag;
      if ( step.Type != (StepType)comboStepTypes.SelectedIndex )
      {
        int totalNumberOfBytes = _project.DetermineTotalNumberOfBytes();
        step.Type = (StepType)comboStepTypes.SelectedIndex;
        listPathSteps.SelectedItem.Text = GenerateStepDescription( step, totalNumberOfBytes );
        SetModified();
        RedrawPathPreview();
      }
    }



    private string GenerateStepDescription( Step step, int totalNumberOfBytes )
    {
      return GR.EnumHelper.GetDescription( step.Type ) + ": " + step.Duration;
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



    private void listMappings_SelectedIndexChanged( object sender, ArrangedItemEntry Item )
    {
      if ( Item == null )
      {
        groupStepValues.Enabled = false;
        groupDurationValues.Enabled = false;
        return;
      }
      groupStepValues.Enabled = true;
      groupDurationValues.Enabled = true;

      var mapping = (PathProject.ValueDescriptor)Item.Tag;

      editMappingStepOffset.Text          = mapping.AddressOffsetStep.ToString();
      editMappingStepValue.Text           = mapping.ValueStep.ToString( "X2" );
      editMappingStepMask.Text            = mapping.RelevantBitsStep.ToString( "X2" );
      comboMappingStepType.SelectedIndex  = (int)mapping.Step;

      editMappingDurationOffset.Text = mapping.AddressOffsetDuration.ToString();
      editMappingDurationShiftLeft.Text = mapping.ShiftBitsLeftDuration.ToString();
      editMappingDurationShiftRight.Text = mapping.ShiftBitsRightDuration.ToString();
      editMappingDurationMask.Text = mapping.RelevantBitsDuration.ToString( "X2" );
    }



    private void editMappingStepOffset_TextChanged( object sender, EventArgs e )
    {
      if ( listMappings.SelectedItem == null )
      {
        return;
      }
      var mapping = (PathProject.ValueDescriptor)listMappings.SelectedItem.Tag;
      var newValue = GR.Convert.ToI32( editMappingStepOffset.Text );
      if ( mapping.AddressOffsetStep != newValue )
      {
        mapping.AddressOffsetStep = newValue;
        SetModified();
        listMappings.SelectedItem.Text = GenerateStepMappingToText( mapping, _project.DetermineTotalNumberOfBytes() );
      }
    }



    private void editMappingStepValue_TextChanged( object sender, EventArgs e )
    {
      if ( listMappings.SelectedItem == null )
      {
        return;
      }
      var mapping = (PathProject.ValueDescriptor)listMappings.SelectedItem.Tag;
      var newValue = GR.Convert.ToU8( editMappingStepValue.Text );
      if ( mapping.ValueStep != newValue )
      {
        mapping.ValueStep = newValue;
        SetModified();
        listMappings.SelectedItem.Text = GenerateStepMappingToText( mapping, _project.DetermineTotalNumberOfBytes() );
      }
    }



    private void editMappingStepMask_TextChanged( object sender, EventArgs e )
    {
      if ( listMappings.SelectedItem == null )
      {
        return;
      }
      var mapping = (PathProject.ValueDescriptor)listMappings.SelectedItem.Tag;
      var newValue = GR.Convert.ToU8( editMappingStepMask.Text, 16 );
      if ( mapping.RelevantBitsStep != newValue )
      {
        mapping.RelevantBitsStep = newValue;
        SetModified();
        listMappings.SelectedItem.Text = GenerateStepMappingToText( mapping, _project.DetermineTotalNumberOfBytes() );
      }
    }



    private void editMappingDurationOffset_TextChanged( object sender, EventArgs e )
    {
      if ( listMappings.SelectedItem == null )
      {
        return;
      }
      var mapping = (PathProject.ValueDescriptor)listMappings.SelectedItem.Tag;
      var newValue = GR.Convert.ToI32( editMappingDurationOffset.Text );
      if ( mapping.AddressOffsetDuration != newValue )
      {
        mapping.AddressOffsetDuration = newValue;
        SetModified();
        listMappings.SelectedItem.Text = GenerateStepMappingToText( mapping, _project.DetermineTotalNumberOfBytes() );
      }
    }



    private void editMappingDurationShiftLeft_TextChanged( object sender, EventArgs e )
    {
      if ( listMappings.SelectedItem == null )
      {
        return;
      }
      var mapping = (PathProject.ValueDescriptor)listMappings.SelectedItem.Tag;
      var newValue = GR.Convert.ToI32( editMappingDurationShiftLeft.Text );
      if ( mapping.ShiftBitsLeftDuration != newValue )
      {
        mapping.ShiftBitsLeftDuration = newValue;
        SetModified();
        listMappings.SelectedItem.Text = GenerateStepMappingToText( mapping, _project.DetermineTotalNumberOfBytes() );
      }
    }



    private void editMappingDurationShiftRight_TextChanged( object sender, EventArgs e )
    {
      if ( listMappings.SelectedItem == null )
      {
        return;
      }
      var mapping = (PathProject.ValueDescriptor)listMappings.SelectedItem.Tag;
      var newValue = GR.Convert.ToI32( editMappingDurationShiftRight.Text );
      if ( mapping.ShiftBitsRightDuration != newValue )
      {
        mapping.ShiftBitsRightDuration = newValue;
        SetModified();
        listMappings.SelectedItem.Text = GenerateStepMappingToText( mapping, _project.DetermineTotalNumberOfBytes() );
      }
    }



    private void editMappingDurationMask_TextChanged( object sender, EventArgs e )
    {
      if ( listMappings.SelectedItem == null )
      {
        return;
      }
      var mapping = (PathProject.ValueDescriptor)listMappings.SelectedItem.Tag;
      var newValue = GR.Convert.ToU32( editMappingDurationMask.Text, 16 );
      if ( mapping.RelevantBitsDuration != newValue )
      {
        mapping.RelevantBitsDuration = newValue;
        SetModified();
        listMappings.SelectedItem.Text = GenerateStepMappingToText( mapping, _project.DetermineTotalNumberOfBytes() );
      }
    }



    private void btnExport_Click( DecentForms.ControlBase Sender )
    {
      var exportInfo = new ExportPathInfo()
      {
        DataPerPath = _project.ExportData()
      };

      _ExportForm.HandleExport( exportInfo, DocumentInfo );
    }



    private void comboExportMethod_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( _ExportForm != null )
      {
        _ExportForm.Dispose();
        _ExportForm = null;
      }

      var item = (GR.Generic.Tupel<string, Type>)comboExportMethod.SelectedItem;
      if ( ( item == null )
      || ( item.second == null ) )
      {
        return;
      }
      _ExportForm = (ExportPathFormBase)Activator.CreateInstance( item.second, new object[] { Core } );
      _ExportForm.AutoScaleMode = AutoScaleMode.None;
      _ExportForm.Dock = DockStyle.Fill;
      _ExportForm.Parent = panelExport;
      _ExportForm.CreateControl();
    }



    private ArrangedItemEntry listMappings_CloningItem( object sender, ArrangedItemEntry Item )
    {
      var vd = (PathProject.ValueDescriptor)Item.Tag;

      var clonedItem = new ArrangedItemEntry() { Text = Item.Text };
      var clonedVD = new PathProject.ValueDescriptor( vd );

      clonedItem.Tag = clonedVD;
      return clonedItem;
    }



    private void listMappings_ItemAdded( object sender, ArrangedItemEntry Item )
    {
      if ( _updatingParams )
      {
        return;
      }
      if ( Item.Tag == null )
      {
        var newVD = new PathProject.ValueDescriptor();
        newVD.Step = ( (GR.Generic.Tupel<PathProject.StepType, string>)comboMappingStepType.SelectedItem ).first;
        newVD.ValueStep = GR.Convert.ToU8( editMappingStepValue.Text, 16 );
        newVD.AddressOffsetStep = GR.Convert.ToI32( editMappingStepOffset.Text );
        newVD.RelevantBitsStep = GR.Convert.ToU8( editMappingStepMask.Text, 16 );

        newVD.AddressOffsetDuration = GR.Convert.ToI32( editMappingDurationOffset.Text );
        newVD.ShiftBitsLeftDuration = GR.Convert.ToI32( editMappingDurationShiftLeft.Text );
        newVD.ShiftBitsRightDuration = GR.Convert.ToI32( editMappingDurationShiftRight.Text );
        newVD.RelevantBitsDuration = GR.Convert.ToU32( editMappingDurationMask.Text, 16 );

        _project.ValueDescriptors.Add( newVD );

        Item.Tag = newVD;
        Item.Text = GenerateStepMappingToText( newVD, _project.DetermineTotalNumberOfBytes() );
      }
      else
      {
        _project.Paths.Add( (PathProject.Path)Item.Tag );
      }

      SetModified();
    }



    private void listMappings_ItemMoved( object sender, ArrangedItemEntry Item, int originalIndex )
    {
      var newList = new List<PathProject.ValueDescriptor>();
      foreach ( var item in listMappings.Items )
      {
        newList.Add( (PathProject.ValueDescriptor)( (ArrangedItemEntry)item ).Tag );
      }

      _project.ValueDescriptors = newList;
      SetModified();
    }



    private void listMappings_ItemRemoved( object sender, ArrangedItemEntry Item )
    {
      var vd = (PathProject.ValueDescriptor)Item.Tag;
      _project.ValueDescriptors.Remove( vd );
      SetModified();
    }



    private void editMappingLastStepAddressOffset_TextChanged( object sender, EventArgs e )
    {
      int newValue = GR.Convert.ToI32( editMappingLastStepAddressOffset.Text );
      if ( _project.AddressOffsetLastFlag != newValue )
      {
        _project.AddressOffsetLastFlag = newValue;

        FillMappings();
        SetModified();
      }
    }



    private void editMappingLastStepValue_TextChanged( object sender, EventArgs e )
    {
      byte newValue = GR.Convert.ToU8( editMappingLastStepValue.Text, 16 );
      if ( _project.ValueLastFlag != newValue )
      {
        _project.ValueLastFlag = newValue;

        FillMappings();
        SetModified();
      }
    }



    private void listMappings_CustomDrawItem( DecentForms.ControlRenderer renderer, ArrangedItemEntry item, GR.Math.Rectangle rect, DecentForms.ListBox.ItemState state )
    {
      uint color = DecentForms.ControlRenderer.ColorControlText;

      switch ( state )
      {
        case DecentForms.ListBox.ItemState.SELECTED:
          color = DecentForms.ControlRenderer.ColorControlTextSelected;
          break;
        case DecentForms.ListBox.ItemState.MOUSE_OVER:
          color = DecentForms.ControlRenderer.ColorControlTextMouseOver;
          break;
      }
      // return $"{GR.EnumHelper.GetDescription( mapping.Step )} byte {mapping.AddressOffsetStep}: {finalString}";
      var vd = (PathProject.ValueDescriptor)item.Tag;

      int offset1 = GR.Image.DPIHandler.AdjustPixelSize( 90 );
      int offset2 = GR.Image.DPIHandler.AdjustPixelSize( 140 );

      renderer.DrawText( GR.EnumHelper.GetDescription( vd.Step ),
                         rect.Left, rect.Top, rect.Width, rect.Height,
                          DecentForms.TextAlignment.LEFT | DecentForms.TextAlignment.CENTERED_V, color );

      renderer.DrawText( $"Byte {vd.AddressOffsetStep}:",
                         rect.Left + offset1, rect.Top, rect.Width - offset1, rect.Height,
                         DecentForms.TextAlignment.LEFT | DecentForms.TextAlignment.CENTERED_V, color );

      renderer.DrawText( GenerateMaskString( vd, _project.DetermineTotalNumberOfBytes() ),
                         rect.Left + offset2, rect.Top, rect.Width - offset2, rect.Height,
                         DecentForms.TextAlignment.LEFT | DecentForms.TextAlignment.CENTERED_V, color );
    }



  }
}
