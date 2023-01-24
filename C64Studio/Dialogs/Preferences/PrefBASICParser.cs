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
  public partial class PrefBASICParser : PrefBase
  {
    public PrefBASICParser()
    {
      InitializeComponent();
    }



    public PrefBASICParser( StudioCore Core ) : base( Core )
    {
      _Keywords.AddRange( new string[] { "basic", "parser" } );
      InitializeComponent();

      checkBASICStripSpaces.Checked           = Core.Settings.BASICStripSpaces;
      checkBASICShowControlCodes.Checked      = Core.Settings.BASICShowControlCodesAsChars;
      checkBASICAutoToggleEntryMode.Checked   = Core.Settings.BASICAutoToggleEntryMode;
      checkBASICStripREM.Checked              = Core.Settings.BASICStripREM;
    }



    private void btnImportSettings_Click( object sender, EventArgs e )
    {

    }



    private void btnExportSettings_Click( object sender, EventArgs e )
    {

    }



    private void checkBASICStripSpaces_CheckedChanged( object sender, EventArgs e )
    {
      Core.Settings.BASICStripSpaces = checkBASICStripSpaces.Checked;
      Core.Compiling.ParserBasic.Settings.StripSpaces = Core.Settings.BASICStripSpaces;
    }



    private void checkBASICStripREM_CheckedChanged( object sender, EventArgs e )
    {
      Core.Settings.BASICStripREM = checkBASICStripREM.Checked;
      Core.Compiling.ParserBasic.Settings.StripREM = Core.Settings.BASICStripREM;
    }



    private void checkBASICShowControlCodes_CheckedChanged( object sender, EventArgs e )
    {
      Core.Settings.BASICShowControlCodesAsChars = checkBASICShowControlCodes.Checked;
    }



    private void checkBASICAutoToggleEntryMode_CheckedChanged( object sender, EventArgs e )
    {
      Core.Settings.BASICAutoToggleEntryMode = checkBASICAutoToggleEntryMode.Checked;
    }


  }
}
