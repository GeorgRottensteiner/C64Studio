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
  public partial class PrefBase : UserControl
  {
    protected StudioCore      Core = null;

    protected List<string>    _Keywords = new List<string>();



    public PrefBase()
    {
      InitializeComponent();
    }



    public PrefBase( StudioCore Core )
    {
      this.Core = Core;
      InitializeComponent();
    }



    protected void RefreshDisplayOnDocuments()
    {
      Core.Theming.ApplyTheme( ParentForm );
      Core.Settings.RefreshDisplayOnAllDocuments( Core );
    }



    public bool MatchesKeyword( string Keyword )
    {
      if ( string.IsNullOrEmpty( Keyword ) )
      {
        return true;
      }
      string  kw = Keyword.ToUpper();
      foreach ( var keyword in _Keywords )
      {
        if ( keyword.ToUpper().Contains( kw ) )
        {
          return true;
        }
      }
      return false;
    }



  }
}
