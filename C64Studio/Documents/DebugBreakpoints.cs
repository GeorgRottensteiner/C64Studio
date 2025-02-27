﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using RetroDevStudio;
using RetroDevStudio.Types;
using GR.Collections;

namespace RetroDevStudio.Documents
{
  public partial class DebugBreakpoints : BaseDocument
  {
    private GR.Collections.MultiMap<string, SymbolInfo>       m_TokenInfos = null;

    private delegate void SetTokensCallback( GR.Collections.MultiMap<string, SymbolInfo> TokenInfo );



    public DebugBreakpoints( StudioCore Core )
    {
      this.Core = Core;
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

        if ( bp.AddressSource != null )
        {
          try
          {
            comboSymbols.SelectedItem = bp.AddressSource;
          }
          catch ( Exception )
          {
          }
        }
        else
        {
          editBPAddress.Text = bp.Address.ToString( "x" );
        }
        checkTriggerExec.Checked    = bp.TriggerOnExec;
        checkTriggerLoad.Checked    = bp.TriggerOnLoad;
        checkTriggerStore.Checked   = bp.TriggerOnStore;
        editTriggerConditions.Text  = bp.Conditions;
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



    private void btnAddBreakpoint_Click( DecentForms.ControlBase Sender )
    {
      Types.Breakpoint bp = new RetroDevStudio.Types.Breakpoint();

      bp.Address          = GR.Convert.ToI32( editBPAddress.Text, 16 );
      bp.TriggerOnExec    = checkTriggerExec.Checked;
      bp.TriggerOnLoad    = checkTriggerLoad.Checked;
      bp.TriggerOnStore   = checkTriggerStore.Checked;
      bp.LineIndex        = -1;
      bp.Conditions       = editTriggerConditions.Text;
      bp.DocumentFilename = "RetroDevStudio.DebugBreakpoints";

      if ( comboSymbols.SelectedIndex != -1 )
      {
        bp.AddressSource = (string)comboSymbols.SelectedItem;
      }

      // set marker in associated file
      var docInfo = DetermineAffectedDocument( bp.Address, out bp.DocumentFilename, out int lineIndex );
      if ( docInfo != null )
      {
        var sourceFile = docInfo.BaseDoc as SourceASMEx;
        if ( sourceFile != null )
        {
          sourceFile.AddBreakpoint( bp );
        }
      }
      RaiseDocEvent( new DocEvent( DocEvent.Type.BREAKPOINT_ADDED, bp ) );
    }



    private DocumentInfo DetermineAffectedDocument( int BreakpointAddress, out string DocumentFilename, out int LineIndex )
    {
      DocumentFilename  = "";
      LineIndex         = -1;
      if ( Core.MainForm.CurrentProject != null )
      {
        if ( !string.IsNullOrEmpty( Core.MainForm.CurrentProject.Settings.MainDocument ) )
        {
          var element = Core.MainForm.CurrentProject.GetElementByFilename( Core.MainForm.CurrentProject.Settings.MainDocument );
          if ( ( element != null )
          &&   ( element.DocumentInfo.Type == ProjectElement.ElementType.ASM_SOURCE ) )
          {
            var asmFileInfo2 = Core.Navigating.DetermineASMFileInfo( element.DocumentInfo );
            if ( ( asmFileInfo2 != null )
            &&   ( asmFileInfo2.DocumentAndLineFromAddress( BreakpointAddress, out DocumentFilename, out LineIndex ) ) )
            {
              DocumentFilename = element.DocumentInfo.RelativePath;
              return element.DocumentInfo;
            }
          }
        }
      }
      var asmFileInfo = Core.Navigating.DetermineASMFileInfo( Core.Navigating.LastActiveCodeDocument );
      if ( ( asmFileInfo != null )
      &&   ( asmFileInfo.DocumentAndLineFromAddress( BreakpointAddress, out DocumentFilename, out LineIndex ) ) )
      {
        if ( Core.Navigating.LastActiveCodeDocument.Type == ProjectElement.ElementType.ASM_SOURCE )
        {
          var docInfo = Core.Navigating.FindDocumentByFilename( DocumentFilename );
          if ( docInfo != null )
          {
            DocumentFilename = docInfo.RelativePath;
          }
          return Core.Navigating.LastActiveCodeDocument;
        }
      }
      return null;
    }



    private void editBPAddress_TextChanged( object sender, EventArgs e )
    {
      btnAddBreakpoint.Enabled = AreValidBreakpointSettings();

      if ( ( comboSymbols.SelectedIndex != -1 )
      &&   ( m_TokenInfos.ContainsKey( (string)comboSymbols.SelectedItem ) ) )
      {
        List<SymbolInfo> tokens = m_TokenInfos.GetValues( (string)comboSymbols.SelectedItem, false );
        if ( tokens == null )
        {
          return;
        }
        if ( editBPAddress.Text != tokens[0].AddressOrValue.ToString( "x" ) )
        {
          comboSymbols.SelectedIndex = -1;
        }
      }

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



    private void btnDeleteBreakpoint_Click( DecentForms.ControlBase Sender )
    {
      if ( listBreakpoints.SelectedIndices.Count == 0 )
      {
        return;
      }
      var bps = new List<Types.Breakpoint>();
      foreach ( ListViewItem item in listBreakpoints.SelectedItems )
      {
        bps.Add( (Types.Breakpoint)item.Tag );
      }
      foreach ( var bp in bps )
      {
        RaiseDocEvent( new DocEvent( DocEvent.Type.BREAKPOINT_REMOVED, bp ) );
      }
    }



    private void btnApplyChanges_Click( DecentForms.ControlBase Sender )
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



    private void btnDeleteAllBreakpoints_Click( DecentForms.ControlBase Sender )
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



    private void listBreakpoints_ItemActivate( object sender, EventArgs e )
    {
      if ( listBreakpoints.SelectedItems.Count > 0 )
      {
        Types.Breakpoint bp = (Types.Breakpoint)listBreakpoints.SelectedItems[0].Tag;

        if ( ( bp.LineIndex != -1 )
        &&   ( !string.IsNullOrEmpty( bp.DocumentFilename ) ) )
        {
          Core.Navigating.OpenDocumentAndGotoLine( Core.Navigating.Project, Core.Navigating.FindDocumentInfoByPath( bp.DocumentFilename ), bp.LineIndex );
        }
      }
    }



    public override void OnApplicationEvent( ApplicationEvent Event )
    {
      base.OnApplicationEvent( Event );
      switch ( Event.EventType )
      {
        case ApplicationEvent.Type.SETTINGS_LOADED:
          Core.Settings.DialogSettings.RestoreListViewColumns( "Debug.Breakpoints", listBreakpoints );
          break;
        case ApplicationEvent.Type.SHUTTING_DOWN:
          Core.Settings.DialogSettings.StoreListViewColumns( "Debug.Breakpoints", listBreakpoints );
          break;
        case ApplicationEvent.Type.DOCUMENT_ACTIVATED:
          {
            listBreakpoints.Items.Clear();
            if ( ( Event.Doc != null )
            &&   ( Event.Doc.Project != null ) )
            {
              RefillBreakpointList( Event.Doc.Project.Settings.BreakPoints );
            }
            else if ( Core.Debugging.Debugger != null )
            {
              // projectless debugging, use watches from debugger
              RefillBreakpointList( Core.Debugging.BreakPoints );
            }
            else if ( ( Event.Doc != null )
            &&        ( Event.Doc.Type == ProjectElement.ElementType.ASM_SOURCE ) )
            {
              // assembler without project
              var mappedBreakpoints = ( (SourceASMEx)Event.Doc.BaseDoc ).MapBreakpoints();
              RefillBreakpointList( mappedBreakpoints );
            }
          }
          break;
      }
    }



    public void RefillBreakpointList( Map<string, List<Breakpoint>> BreakPoints )
    {
      foreach ( var bpList in BreakPoints.Values )
      {
        foreach ( var bp in bpList )
        {
          ListViewItem item = new ListViewItem();

          for ( int i = 0; i < 5; ++i )
          {
            item.SubItems.Add( "" );
          }
          AdjustItemText( item, bp );
          item.Tag = bp;

          listBreakpoints.Items.Add( item );
        }
      }
    }



  }
}
