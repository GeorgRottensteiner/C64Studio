using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RetroDevStudio
{
  public partial class PropCompileTarget : PropertyTabs.PropertyTabBase
  {
    ProjectElement        Element;
    StudioCore            Core;

    public PropCompileTarget( ProjectElement Element, StudioCore Core )
    {
      this.Element = Element;
      this.Core = Core;
      TopLevel = false;
      Text = "Compile Target";
      InitializeComponent();

      comboTargetType.Items.Add( "None" );
      comboTargetType.Items.Add( "Plain" );
      comboTargetType.Items.Add( "PRG (cbm)" );
      comboTargetType.Items.Add( "T64" );
      comboTargetType.Items.Add( "8 KB Cartridge (bin)" );
      comboTargetType.Items.Add( "8 KB Cartridge (crt)" );
      comboTargetType.Items.Add( "16 KB Cartridge (bin)" );
      comboTargetType.Items.Add( "16 KB Cartridge (crt)" );
      comboTargetType.Items.Add( "D64" );
      comboTargetType.Items.Add( "Magic Desk 64 KB Cartridge (bin)" );
      comboTargetType.Items.Add( "Magic Desk 64 KB Cartridge (crt)" );
      comboTargetType.Items.Add( "TAP" );
      comboTargetType.Items.Add( "Easyflash Cartridge (bin)" );
      comboTargetType.Items.Add( "Easyflash Cartridge (crt)" );
      comboTargetType.Items.Add( "RGCD 64 KB Cartridge (bin)" );
      comboTargetType.Items.Add( "RGCD 64 KB Cartridge (crt)" );
      comboTargetType.Items.Add( "GMOD2 Cartridge (bin)" );
      comboTargetType.Items.Add( "GMOD2 Cartridge (crt)" );
      comboTargetType.Items.Add( "D81" );

      comboTargetType.SelectedIndex = (int)Element.TargetType;

      editTargetFilename.Text = Element.TargetFilename;

      // own project first
      foreach ( ProjectElement element in Element.DocumentInfo.Project.Elements )
      {
        VerifyElement( element );
      }
      foreach ( var project in Core.Navigating.Solution.Projects )
      {
        if ( project != Element.DocumentInfo.Project )
        {
          foreach ( ProjectElement element in project.Elements )
          {
            VerifyElement( element );
          }
        }
      }

      foreach ( var entry in Element.ExternalDependencies.DependentOnFile )
      {
        listExternalDependencies.Items.Add( entry.Filename );
      }

      Core.Settings.DialogSettings.RestoreListViewColumns( "CompileTargetDependencies", listDependencies );
    }



    private void VerifyElement( ProjectElement element )
    {
      if ( ( element != Element )
      &&   ( element.DocumentInfo.Type != ProjectElement.ElementType.FOLDER ) )
      {
        var dependencies = Element.DocumentInfo.Project.GetDependencies( element );

        FileDependency.DependencyInfo   depInfo = Element.ForcedDependency.FindDependency( element.DocumentInfo.Project.Settings.Name, element.Filename );
        if ( depInfo == null )
        {
          depInfo = new FileDependency.DependencyInfo( element.DocumentInfo.Project.Settings.Name, element.Filename, false, false );
        }

        bool isDependent = false;
        foreach ( var dependency in dependencies )
        {
          if ( dependency.Filename == Element.Filename )
          {
            isDependent = true;
            break;
          }
        }
        if ( !isDependent )
        {
          // not itself!
          DependencyItem depItem = new DependencyItem( depInfo, element.DocumentInfo.Type == ProjectElement.ElementType.ASM_SOURCE );


          ListViewItem    item = new ListViewItem( element.DocumentInfo.Project.Settings.Name );
          item.SubItems.Add( element.Filename );

          if ( Element.ForcedDependency.DependsOn( element.DocumentInfo.Project.Settings.Name, element.Filename ) )
          {
            item.SubItems.Add( "1" );
          }
          else
          {
            item.SubItems.Add( "0" );
          }
          if ( depInfo.IncludeSymbols )
          {
            item.SubItems.Add( "1" );
          }
          else
          {
            item.SubItems.Add( "0" );
          }
          item.Tag = depItem;
          listDependencies.Items.Add( item );
        }
      }
    }

    private void btnParseTarget_Click( DecentForms.ControlBase Sender )
    {
      Core.MainForm.EnsureFileIsParsed();

      if ( Element.CompileTargetFile == null )
      {
        editTargetFilename.Text = "";
      }
      else
      {
        string relativeFilename = GR.Path.RelativePathTo( Element.CompileTargetFile, false, System.IO.Path.GetFullPath( Element.DocumentInfo.Project.Settings.BasePath ), true );
        editTargetFilename.Text = relativeFilename;
      }
      comboTargetType.SelectedIndex = (int)Element.CompileTarget;
    }



    public override void OnClose()
    {
      Element.TargetFilename = editTargetFilename.Text;
      Element.TargetType = (Types.CompileTargetType)comboTargetType.SelectedIndex;

      // rebuild dependency list
      Element.ForcedDependency.DependentOnFile.Clear();
      foreach ( ListViewItem item in listDependencies.Items )
      {
        DependencyItem depItem = (DependencyItem)item.Tag;

        if ( depItem.DependencyInfo.Dependent )
        {
          Element.ForcedDependency.DependentOnFile.Add( depItem.DependencyInfo );
        }
      }
      Core.Settings.DialogSettings.StoreListViewColumns( "CompileTargetDependencies", listDependencies );
    }



    private void listDependencies_DrawSubItem( object sender, DrawListViewSubItemEventArgs e )
    {
      if ( ( e.Item == null )
      ||   ( e.ColumnIndex <= 1 ) )
      {
        e.DrawDefault = true;
        return;
      }
      e.DrawBackground();
      DependencyItem    depItem = (DependencyItem)e.Item.Tag;
      if ( e.ColumnIndex == 2 )
      {
        System.Windows.Forms.ControlPaint.DrawCheckBox( e.Graphics, e.SubItem.Bounds, depItem.DependencyInfo.Dependent ? ButtonState.Checked : ButtonState.Normal );
      }
      else if ( e.ColumnIndex == 3 )
      {
        if ( depItem.CanIncludeSymbols )
        {
          System.Windows.Forms.ControlPaint.DrawCheckBox( e.Graphics, e.SubItem.Bounds, depItem.DependencyInfo.IncludeSymbols ? ButtonState.Checked : ButtonState.Normal );
        }
      }
    }



    private void listDependencies_DrawColumnHeader( object sender, DrawListViewColumnHeaderEventArgs e )
    {
      e.DrawDefault = true;
    }



    private void listDependencies_DrawItem( object sender, DrawListViewItemEventArgs e )
    {
      e.DrawBackground();
      e.DrawText();
      e.DrawFocusRectangle();
    }



    private void listDependencies_MouseDown( object sender, MouseEventArgs e )
    {
      var hitInfo = listDependencies.HitTest( e.Location );
      if ( hitInfo.Item != null )
      {
        DependencyItem    depItem = (DependencyItem)hitInfo.Item.Tag;

        if ( hitInfo.SubItem == hitInfo.Item.SubItems[2] )
        {
          // dependent
          depItem.DependencyInfo.Dependent = !depItem.DependencyInfo.Dependent;
          if ( !depItem.DependencyInfo.Dependent )
          {
            depItem.DependencyInfo.IncludeSymbols = false;
          }
          Element.DocumentInfo.Project.SetModified();
          listDependencies.Invalidate( hitInfo.Item.Bounds );
        }
        else if ( hitInfo.SubItem == hitInfo.Item.SubItems[3] )
        {
          // include symbols
          if ( ( depItem.DependencyInfo.Dependent )
          &&   ( depItem.CanIncludeSymbols ) )
          {
            depItem.DependencyInfo.IncludeSymbols = !depItem.DependencyInfo.IncludeSymbols;
            Element.DocumentInfo.Project.SetModified();
            listDependencies.Invalidate( hitInfo.Item.Bounds );
          }
        }
      }
    }



    private void editTargetFilename_TextChanged( object sender, EventArgs e )
    {
      if ( editTargetFilename.Text != Element.TargetFilename )
      {
        Element.DocumentInfo.Project.SetModified();
      }
    }



    private void comboTargetType_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( comboTargetType.SelectedIndex != (int)Element.TargetType )
      {
        Element.DocumentInfo.Project.SetModified();
      }
    }



    private void listExternalDependencies_SelectedIndexChanged( object sender, EventArgs e )
    {
      btnRemoveExternalDependency.Enabled = ( listExternalDependencies.SelectedIndex != -1 );
    }



    private string BuildFullPath( string ParentPath, string SubFilename )
    {
      if ( System.IO.Path.IsPathRooted( SubFilename ) )
      {
        return SubFilename;
      }
      return GR.Path.Append( ParentPath, SubFilename );
    }



    private void btnAddExternalDependency_Click( DecentForms.ControlBase Sender )
    {
      var dlg = new OpenFileDialog();

      dlg.Title = "Select external dependency";

      if ( dlg.ShowDialog() != DialogResult.OK )
      {
        return;
      }

      // no duplicates!
      foreach ( var entry in Element.ExternalDependencies.DependentOnFile )
      {
        if ( GR.Path.IsPathEqual( BuildFullPath( Element.DocumentInfo.Project.Settings.BasePath, entry.Filename ), dlg.FileName ) )
        {
          MessageBox.Show( "File " + dlg.FileName + " is already set as external dependency!", "Dependency already exists" );
          return;
        }
      }
      string  relativeFilename = GR.Path.RelativePathTo( dlg.FileName, false, Element.DocumentInfo.Project.Settings.BasePath, true );
      Element.ExternalDependencies.DependentOnFile.Add( new FileDependency.DependencyInfo( "", relativeFilename, true, false ) );
      listExternalDependencies.Items.Add( relativeFilename );
      Element.DocumentInfo.Project.SetModified();
    }



    private void btnRemoveExternalDependency_Click( DecentForms.ControlBase Sender )
    {
      if ( listExternalDependencies.SelectedIndex == -1 )
      {
        return;
      }
      Element.ExternalDependencies.DependentOnFile.RemoveAt( listExternalDependencies.SelectedIndex );
      listExternalDependencies.Items.RemoveAt( listExternalDependencies.SelectedIndex );
      Element.DocumentInfo.Project.SetModified();
    }



    public class DependencyItem
    {
      public FileDependency.DependencyInfo    DependencyInfo = new FileDependency.DependencyInfo( "", "", false, false );
      public bool                             CanIncludeSymbols = false;


      public DependencyItem( FileDependency.DependencyInfo Info, bool CanIncludeSymbols )
      {
        DependencyInfo = Info;
        this.CanIncludeSymbols = CanIncludeSymbols;
      }
    }



  }
}
