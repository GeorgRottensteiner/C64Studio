using GR.Strings;
using RetroDevStudio.Controls;
using RetroDevStudio.Parser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;



namespace RetroDevStudio.Dialogs.Preferences
{
  [Description( "Assembler.Library Paths" )]
  public partial class DlgPrefLibraryPaths : DlgPrefBase
  {
    public DlgPrefLibraryPaths()
    {
      InitializeComponent();
    }



    public DlgPrefLibraryPaths( StudioCore Core ) : base( Core )
    {
      _Keywords.AddRange( new string[] { "asm", "assembler", "library", "path" } );

      InitializeComponent();
    }



    public override void ApplySettingsToControls()
    {
      asmLibraryPathList.BeginUpdate();

      asmLibraryPathList.Items.Clear();
      foreach ( var libPath in Core.Settings.ASMLibraryPaths )
      {
        asmLibraryPathList.Items.Add( libPath );
      }

      asmLibraryPathList.EndUpdate();
    }



    public override void ExportSettings( XMLElement SettingsRoot )
    {
      var xmlSettingRoot = new GR.Strings.XMLElement( "LibraryPaths" );
      SettingsRoot.AddChild( xmlSettingRoot );

      foreach ( var path in Core.Settings.ASMLibraryPaths )
      {
        xmlSettingRoot.AddChild( "Path", path );
      }
    }



    public override void ImportSettings( XMLElement SettingsRoot )
    {
      var xmlSettingRoot = SettingsRoot.FindByTypeRecursive( "LibraryPaths" );
      if ( xmlSettingRoot != null )
      {
        Core.Settings.ASMLibraryPaths.Clear();
        foreach ( var xmlKey in xmlSettingRoot.ChildElements )
        {
          
          if ( xmlKey.Type == "Path" )
          {
            Core.Settings.ASMLibraryPaths.Add( xmlKey.Content );
          }
        }
      }
    }



    private void ApplyLibraryPathsFromList()
    {
      Core.Settings.ASMLibraryPaths.Clear();
      foreach ( ArrangedItemEntry entry in asmLibraryPathList.Items )
      {
        Core.Settings.ASMLibraryPaths.Add( entry.Text );
      }
    }



    private Controls.ArrangedItemEntry asmLibraryPathList_AddingItem( object sender )
    {
      var newEntry = new ArrangedItemEntry( editASMLibraryPath.Text );
      return newEntry;
    }



    private void asmLibraryPathList_ItemAdded( object sender, ArrangedItemEntry Item )
    {
      ApplyLibraryPathsFromList();
    }



    private void asmLibraryPathList_ItemMoved( object sender, ArrangedItemEntry Item1, ArrangedItemEntry Item2 )
    {
      ApplyLibraryPathsFromList();
    }



    private void asmLibraryPathList_ItemRemoved( object sender, ArrangedItemEntry Item )
    {
      ApplyLibraryPathsFromList();
    }



    private void btmASMLibraryPathBrowse_Click( DecentForms.ControlBase Sender )
    {
      FolderBrowserDialog dlg = new FolderBrowserDialog();

      dlg.Description = "Choose Library Folder";
      if ( dlg.ShowDialog() == DialogResult.OK )
      {
        editASMLibraryPath.Text = dlg.SelectedPath;
      }
    }



  }
}
