using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using RetroDevStudio;

namespace RetroDevStudio
{
  public partial class DebugBreakpoints : BaseDocument
  {
    public Project                    DebuggedProject = null;

    private GR.Collections.MultiMap<string, SymbolInfo>       m_TokenInfos = null;

    private delegate void SetTokensCallback( GR.Collections.MultiMap<string, SymbolInfo> TokenInfo );



    public DebugBreakpoints()
    {
      InitializeComponent();

      GR.Image.DPIHandler.ResizeControlsForDPI( this );
    }



    public void AddBreakpoint( Types.Breakpoint Breakpoint )
    {
      ListViewItem item = new ListViewItem();

      for ( int i = 0; i < 5; ++i )
      {
        item.SubItems.Add( "" );
      }
      AdjustItemText( item, Breakpoint );
      item.Tag = Breakpoint;

      listBreakpoints.Items.Add( item );
    }



    private void AdjustItemText( ListViewItem Item, Types.Breakpoint Breakpoint )
    {
      if ( Breakpoint.RemoteIndex == -1 )
      {
        Item.Text = "-";
      }
      else
      {
        Item.Text = Breakpoint.RemoteIndex.ToString();
      }

      if ( Breakpoint.DocumentFilename != "RetroDevStudio.DebugBreakpoints" )
      {
        Item.SubItems[1].Text = Breakpoint.DocumentFilename;
      }
      else
      {
        Item.SubItems[1].Text = "---";
      }

      if ( Breakpoint.LineIndex == -1 )
      {
        Item.SubItems[2].Text = "-";
      }
      else
      {
        Item.SubItems[2].Text = ( Breakpoint.LineIndex + 1 ).ToString();
      }

      string triggerDesc = "";
      if ( Breakpoint.TriggerOnExec )
      {
        triggerDesc += "X";
      }
      if ( Breakpoint.TriggerOnStore )
      {
        triggerDesc += "S";
      }
      if ( Breakpoint.TriggerOnLoad )
      {
        triggerDesc += "L";
      }
      Item.SubItems[3].Text = triggerDesc;

      Item.SubItems[4].Text = Breakpoint.Conditions;

      if ( Breakpoint.AddressSource != null )
      {
        if ( Breakpoint.Address != -1 )
        {
          Item.SubItems[5].Text = Breakpoint.AddressSource + " ($" + Breakpoint.Address.ToString( "x4" ) + ")";
        }
        else
        {
          Item.SubItems[5].Text = Breakpoint.AddressSource;
        }
      }
      else if ( Breakpoint.Address != -1 )
      {
        Item.SubItems[5].Text = "$" + Breakpoint.Address.ToString( "x4" );
      }
      else
      {
        Item.SubItems[5].Text = "----";
      }
    }



    public void RemoveBreakpoint( Types.Breakpoint Breakpoint )
    {
      foreach ( ListViewItem item in listBreakpoints.Items )
      {
        if ( item.Tag == Breakpoint )
        {
          listBreakpoints.Items.Remove( item );
          return;
        }
      }
    }



    private void listBreakpoints_SelectedIndexChanged( object sender, EventArgs e )
    {
      btnDeleteBreakpoint.Enabled = ( listBreakpoints.SelectedItems.Count != 0 );
      btnApplyChanges.Enabled = false;

      if ( listBreakpoints.SelectedItems.Count > 0 )
      {
        Types.Breakpoint bp = (Types.Breakpoint)listBreakpoints.SelectedItems[0].Tag;

        editBPAddress.Text = bp.Address.ToString( "x" );
        checkTriggerExec.Checked = bp.TriggerOnExec;
        checkTriggerLoad.Checked = bp.TriggerOnLoad;
        checkTriggerStore.Checked = bp.TriggerOnStore;
        editTriggerConditions.Text = bp.Conditions;
      }
    }



    private bool AreValidBreakpointSettings()
    {
      if ( ( !checkTriggerExec.Checked )
      &&   ( !checkTriggerLoad.Checked )
      &&   ( !checkTriggerStore.Checked ) )
      {
        return false;
      }
      int address = GR.Convert.ToI32( editBPAddress.Text, 16 );
      if ( ( address < 0 )
      ||   ( address > 65535 ) )
      {
        return false;
      }
      return true;
    }



    private void btnAddBreakpoint_Click( object sender, EventArgs e )
    {
      Types.Breakpoint bp = new RetroDevStudio.Types.Breakpoint();

      bp.Address = GR.Convert.ToI32( editBPAddress.Text, 16 );
      bp.TriggerOnExec  = checkTriggerExec.Checked;
      bp.TriggerOnLoad  = checkTriggerLoad.Checked;
      bp.TriggerOnStore = checkTriggerStore.Checked;
      bp.LineIndex = -1;
      bp.Conditions = editTriggerConditions.Text;
      bp.DocumentFilename = "RetroDevStudio.DebugBreakpoints";

      // set marker in associated file
      if ( DebuggedProject != null )
      {
        var element = DebuggedProject.GetElementByFilename( DebuggedProject.Settings.MainDocument );
        if ( element != null )
        {
          var asmFileInfo = Core.Navigating.DetermineASMFileInfo( element.DocumentInfo );

          if ( asmFileInfo.DocumentAndLineFromAddress( bp.Address, out string DocumentFilename, out int lineIndex ) )
          {
            element = DebuggedProject.GetElementByFilename( DocumentFilename );
            if ( element.DocumentInfo.Type == ProjectElement.ElementType.ASM_SOURCE )
            {
              var sourceFile = element.Document as SourceASMEx;
              bp.LineIndex = lineIndex;
              bp.DocumentFilename = DocumentFilename;
              sourceFile.AddBreakpoint( bp );
            }
          }
        }
      }

      RaiseDocEvent( new DocEvent( DocEvent.Type.BREAKPOINT_ADDED, bp ) );
    }



    private void editBPAddress_TextChanged( object sender, EventArgs e )
    {
      btnAddBreakpoint.Enabled = AreValidBreakpointSettings();

      UpdateApplyButton();
    }



    private void UpdateApplyButton()
    {
      if ( listBreakpoints.SelectedItems.Count > 0 )
      {
        Types.Breakpoint bp = (Types.Breakpoint)listBreakpoints.SelectedItems[0].Tag;

        if ( ( bp.Address != GR.Convert.ToI32( editBPAddress.Text, 16 ) )
        ||   ( bp.Conditions != editTriggerConditions.Text )
        ||   ( bp.TriggerOnExec != checkTriggerExec.Checked )
        ||   ( bp.TriggerOnLoad != checkTriggerLoad.Checked )
        ||   ( bp.TriggerOnStore != checkTriggerStore.Checked ) )
        {
          btnApplyChanges.Enabled = true;
        }
        else
        {
          btnApplyChanges.Enabled = false;
        }
      }
      else
      {
        btnApplyChanges.Enabled = false;
      }
    }



    private void editTriggerConditions_TextChanged( object sender, EventArgs e )
    {
      btnAddBreakpoint.Enabled = AreValidBreakpointSettings();
      UpdateApplyButton();
    }



    private void checkTriggerExec_CheckedChanged( object sender, EventArgs e )
    {
      btnAddBreakpoint.Enabled = AreValidBreakpointSettings();
      UpdateApplyButton();
    }



    private void checkTriggerLoad_CheckedChanged( object sender, EventArgs e )
    {
      btnAddBreakpoint.Enabled = AreValidBreakpointSettings();
      UpdateApplyButton();
    }



    private void checkTriggerStore_CheckedChanged( object sender, EventArgs e )
    {
      btnAddBreakpoint.Enabled = AreValidBreakpointSettings();
      UpdateApplyButton();
    }



    public void UpdateBreakpoint( Types.Breakpoint Breakpoint )
    {
      foreach ( ListViewItem item in listBreakpoints.Items )
      {
        if ( item.Tag == Breakpoint )
        {
          AdjustItemText( item, Breakpoint );
          return;
        }
      }
    }



    public void SetTokens( GR.Collections.MultiMap<string, SymbolInfo> TokenInfo )
    {
      if ( InvokeRequired )
      {
        Invoke( new SetTokensCallback( SetTokens ), new object[] { TokenInfo } );
        return;
      }

      comboSymbols.Items.Clear();

      GR.Collections.Set<string>    keys = TokenInfo.GetUniqueKeys();
      foreach ( var token in keys )
      {
        comboSymbols.Items.Add( token );
      }
      m_TokenInfos = TokenInfo;
    }



    private void comboSymbols_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( ( comboSymbols.SelectedIndex == -1 )
      ||   ( m_TokenInfos == null ) )
      {
        return;
      }
      string    key = (string)comboSymbols.SelectedItem;
      if ( m_TokenInfos.ContainsKey( key ) )
      {
        List<SymbolInfo> tokens = m_TokenInfos.GetValues( key, false );
        if ( tokens == null )
        {
          return;
        }
        editBPAddress.Text = tokens[0].AddressOrValue.ToString( "x" );
      }
    }



    private void btnDeleteBreakpoint_Click( object sender, EventArgs e )
    {
      if ( listBreakpoints.SelectedIndices.Count == 0 )
      {
        return;
      }
      Types.Breakpoint bp = (Types.Breakpoint)listBreakpoints.SelectedItems[0].Tag;

      RaiseDocEvent( new DocEvent( DocEvent.Type.BREAKPOINT_REMOVED, bp ) );
    }



    private void btnApplyChanges_Click( object sender, EventArgs e )
    {
      if ( listBreakpoints.SelectedIndices.Count == 0 )
      {
        return;
      }
      Types.Breakpoint bp = (Types.Breakpoint)listBreakpoints.SelectedItems[0].Tag;

      bool    addressChanged = ( bp.Address != GR.Convert.ToI32( editBPAddress.Text, 16 ) );
      bp.Conditions = editTriggerConditions.Text;
      if ( addressChanged )
      {
        bp.Address = GR.Convert.ToI32( editBPAddress.Text, 16 );
      }
      bp.TriggerOnExec = checkTriggerExec.Checked;
      bp.TriggerOnLoad = checkTriggerLoad.Checked;
      bp.TriggerOnStore = checkTriggerStore.Checked;
      bp.RemoteIndex = -1;

      RaiseDocEvent( new DocEvent( DocEvent.Type.BREAKPOINT_REMOVED, bp ) );

      if ( addressChanged )
      {
        // address was changed, so line index doesn't apply anymore
        bp.LineIndex = -1;
      }
      RaiseDocEvent( new DocEvent( DocEvent.Type.BREAKPOINT_ADDED, bp ) );
    }



    private void btnDeleteAllBreakpoints_Click( object sender, EventArgs e )
    {
      List<Types.Breakpoint>    bpsToRemove = new List<Types.Breakpoint>();
      foreach ( ListViewItem item in listBreakpoints.Items )
      {
        var bp = (Types.Breakpoint)item.Tag;
        bpsToRemove.Add( bp );
      }

      foreach ( var bp in bpsToRemove )
      {
        RaiseDocEvent( new DocEvent( DocEvent.Type.BREAKPOINT_REMOVED, bp ) );
      }
    }



  }
}
