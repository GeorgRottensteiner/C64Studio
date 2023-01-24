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
  public partial class PrefAssembler : PrefBase
  {
    public PrefAssembler()
    {
      InitializeComponent();
    }



    public PrefAssembler( StudioCore Core ) : base( Core )
    {
      _Keywords.AddRange( new string[] { "asm", "assembler", "warnings" } );

      InitializeComponent();

      RefillIgnoredMessageList();
      RefillWarningsAsErrorList();
      RefillC64StudioHackList();

      foreach ( var libPath in Core.Settings.ASMLibraryPaths )
      {
        asmLibraryPathList.Items.Add( libPath );
      }

      var allEncodings = System.Text.Encoding.GetEncodings();

      foreach ( var encoding in allEncodings )
      {
        comboASMEncoding.Items.Add( new GR.Generic.Tupel<string, Encoding>( encoding.DisplayName + "    Codepage " + encoding.CodePage, encoding.GetEncoding() ) );
        if ( encoding.GetEncoding() == Core.Settings.SourceFileEncoding )
        {
          comboASMEncoding.SelectedIndex = comboASMEncoding.Items.Count - 1;
        }
      }
    }



    private void btnImportSettings_Click( object sender, EventArgs e )
    {

    }



    private void btnExportSettings_Click( object sender, EventArgs e )
    {

    }



    private void ApplyLibraryPathsFromList()
    {
      Core.Settings.ASMLibraryPaths.Clear();
      foreach ( ArrangedItemEntry entry in asmLibraryPathList.Items )
      {
        Core.Settings.ASMLibraryPaths.Add( entry.Text );
      }
    }



    private void RefillC64StudioHackList()
    {
      listHacks.Items.Clear();
      listHacks.BeginUpdate();
      foreach ( AssemblerSettings.Hacks hack in Enum.GetValues( typeof( AssemblerSettings.Hacks ) ) )
      {
        int itemIndex = listHacks.Items.Add( new GR.Generic.Tupel<string, AssemblerSettings.Hacks>( GR.EnumHelper.GetDescription( hack ), hack ) );
        if ( Core.Settings.EnabledC64StudioHacks.ContainsValue( hack ) )
        {
          listHacks.SetItemChecked( itemIndex, true );
        }
      }
      listHacks.EndUpdate();
    }



    private void RefillIgnoredMessageList()
    {
      listIgnoredWarnings.Items.Clear();
      listIgnoredWarnings.BeginUpdate();
      foreach ( Types.ErrorCode code in Enum.GetValues( typeof( Types.ErrorCode ) ) )
      {
        if ( ( code > Types.ErrorCode.WARNING_START )
        && ( code < Types.ErrorCode.WARNING_LAST_PLUS_ONE ) )
        {
          int itemIndex = listIgnoredWarnings.Items.Add( new GR.Generic.Tupel<string, Types.ErrorCode>( GR.EnumHelper.GetDescription( code ), code ) );
          if ( Core.Settings.IgnoredWarnings.ContainsValue( code ) )
          {
            listIgnoredWarnings.SetItemChecked( itemIndex, true );
          }
        }
      }
      listIgnoredWarnings.EndUpdate();
    }



    private void RefillWarningsAsErrorList()
    {
      listWarningsAsErrors.Items.Clear();
      listWarningsAsErrors.BeginUpdate();
      foreach ( Types.ErrorCode code in Enum.GetValues( typeof( Types.ErrorCode ) ) )
      {
        if ( ( code > Types.ErrorCode.WARNING_START )
        && ( code < Types.ErrorCode.WARNING_LAST_PLUS_ONE ) )
        {
          int itemIndex = listWarningsAsErrors.Items.Add( new GR.Generic.Tupel<string, Types.ErrorCode>( GR.EnumHelper.GetDescription( code ), code ) );
          if ( Core.Settings.TreatWarningsAsErrors.ContainsValue( code ) )
          {
            listWarningsAsErrors.SetItemChecked( itemIndex, true );
          }
        }
      }
      listWarningsAsErrors.EndUpdate();
    }



    private void checkASMAutoTruncateLiteralValues_CheckedChanged( object sender, EventArgs e )
    {
      Core.Settings.ASMAutoTruncateLiteralValues = checkASMAutoTruncateLiteralValues.Checked;
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



    private void btmASMLibraryPathBrowse_Click( object sender, EventArgs e )
    {
      FolderBrowserDialog dlg = new FolderBrowserDialog();

      dlg.Description = "Choose Library Folder";
      if ( dlg.ShowDialog() == DialogResult.OK )
      {
        editASMLibraryPath.Text = dlg.SelectedPath;
      }
    }



    private void listIgnoredWarnings_ItemCheck( object sender, ItemCheckEventArgs e )
    {
      GR.Generic.Tupel<string, Types.ErrorCode> item = (GR.Generic.Tupel<string, Types.ErrorCode>)listIgnoredWarnings.Items[e.Index];

      if ( e.NewValue != CheckState.Checked )
      {
        Core.Settings.IgnoredWarnings.Remove( item.second );
      }
      else
      {
        Core.Settings.IgnoredWarnings.Add( item.second );
      }
    }



    private void listWarningsAsErrors_ItemCheck( object sender, ItemCheckEventArgs e )
    {
      GR.Generic.Tupel<string, Types.ErrorCode> item = (GR.Generic.Tupel<string, Types.ErrorCode>)listWarningsAsErrors.Items[e.Index];

      if ( e.NewValue != CheckState.Checked )
      {
        Core.Settings.TreatWarningsAsErrors.Remove( item.second );
      }
      else
      {
        Core.Settings.TreatWarningsAsErrors.Add( item.second );
      }
    }



    private void listHacks_ItemCheck( object sender, ItemCheckEventArgs e )
    {
      GR.Generic.Tupel<string, Parser.AssemblerSettings.Hacks> item = (GR.Generic.Tupel<string, Parser.AssemblerSettings.Hacks>)listHacks.Items[e.Index];

      if ( e.NewValue != CheckState.Checked )
      {
        Core.Settings.EnabledC64StudioHacks.Remove( item.second );
      }
      else
      {
        Core.Settings.EnabledC64StudioHacks.Add( item.second );
      }
    }



    private void comboASMEncoding_SelectedIndexChanged( object sender, EventArgs e )
    {
      var  newEncoding = (GR.Generic.Tupel<string, Encoding>)comboASMEncoding.SelectedItem;

      Core.Settings.SourceFileEncoding = newEncoding.second;
    }



  }
}
