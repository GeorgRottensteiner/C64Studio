using GR.Strings;
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
  [Description( "General.Cached Data" )]
  public partial class DlgPrefCachedData : DlgPrefBase
  {
    public DlgPrefCachedData()
    {
      InitializeComponent();
    }



    public DlgPrefCachedData( StudioCore Core ) : base( Core )
    {
      _Keywords.AddRange( new string[] { "cache", "clear", "search", "replace", "history" } );

      InitializeComponent();
    }



    private void btnClearSearchHistory_Click( DecentForms.ControlBase Sender )
    {
      Core.Settings.FindArguments.Clear();
      Core.MainForm.RaiseApplicationEvent( new Types.ApplicationEvent( Types.ApplicationEvent.Type.SEARCH_HISTORY_UPDATED ) );
    }



    private void btnClearReplaceSearchHistory_Click( DecentForms.ControlBase Sender )
    {
      Core.Settings.ReplaceArguments.Clear();
      Core.MainForm.RaiseApplicationEvent( new Types.ApplicationEvent( Types.ApplicationEvent.Type.REPLACE_SEARCH_HISTORY_UPDATED ) );
    }



    private void btnClearReplaceWithHistory_Click( DecentForms.ControlBase Sender )
    {
      Core.Settings.ReplaceWithArguments.Clear();
      Core.MainForm.RaiseApplicationEvent( new Types.ApplicationEvent( Types.ApplicationEvent.Type.REPLACE_WITH_HISTORY_UPDATED ) );
    }



    private void btnClearAll_Click( DecentForms.ControlBase Sender )
    {
      Core.Settings.FindArguments.Clear();
      Core.MainForm.RaiseApplicationEvent( new Types.ApplicationEvent( Types.ApplicationEvent.Type.SEARCH_HISTORY_UPDATED ) );
      Core.Settings.ReplaceArguments.Clear();
      Core.MainForm.RaiseApplicationEvent( new Types.ApplicationEvent( Types.ApplicationEvent.Type.REPLACE_SEARCH_HISTORY_UPDATED ) );
      Core.Settings.ReplaceWithArguments.Clear();
      Core.MainForm.RaiseApplicationEvent( new Types.ApplicationEvent( Types.ApplicationEvent.Type.REPLACE_WITH_HISTORY_UPDATED ) );
      Core.Settings.StoredDialogResults.Clear();
    }



    private void btnClearDecisionCache_Click( DecentForms.ControlBase Sender )
    {
      Core.Settings.StoredDialogResults.Clear();
    }



  }
}
