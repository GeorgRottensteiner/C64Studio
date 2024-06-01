using RetroDevStudio;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;



namespace RetroDevStudio.Documents
{
  public partial class Outline : BaseDocument
  {
    public DecentForms.TreeView.TreeNode      NodeRoot = null;

    private Project                           OutlineProject = null;
    private GR.Collections.MultiMap<string,SymbolInfo>       OutlineTokens = new GR.Collections.MultiMap<string, SymbolInfo>();
    private DocumentInfo                      ActiveDocumentInfo = null;
    private Types.ASM.FileInfo                ActiveASMFileInfo = null;

    private Dictionary<Types.ASM.FileInfo,GR.Collections.Map<string, bool>> _ExpandedNodesPerProject = new Dictionary<Types.ASM.FileInfo, GR.Collections.Map<string, bool>>();
    private Dictionary<string,Types.ASM.FileInfo>     _FileInfoPerFileCache = new Dictionary<string, Types.ASM.FileInfo>();



    public Outline( StudioCore Core )
    {
      this.Core = Core;

      InitializeComponent();

      GR.Image.DPIHandler.ResizeControlsForDPI( this );

      NodeRoot = new DecentForms.TreeView.TreeNode( "Outline" );
      NodeRoot.ImageIndex = 0;
      treeProject.Nodes.Add( NodeRoot );

      checkShowShortCutLabels.Image = Core.Settings.OutlineShowShortCutLabels ? RetroDevStudio.Properties.Resources.flag_blue_on.ToBitmap() : RetroDevStudio.Properties.Resources.flag_blue_off.ToBitmap();
      checkShowLocalLabels.Image    = Core.Settings.OutlineShowLocalLabels ? RetroDevStudio.Properties.Resources.flag_green_on.ToBitmap() : RetroDevStudio.Properties.Resources.flag_green_off.ToBitmap();
    }



    private void treeProject_NodeMouseDoubleClick( DecentForms.ControlBase Sender, DecentForms.TreeView.TreeNodeMouseClickEventArgs e )
    {
      SymbolInfo tokenInfo = (SymbolInfo)e.Node.Tag;
      if ( tokenInfo == null )
      {
        if ( e.Node == NodeRoot )
        {
          // project node
          return;
        }
        //// zone node
        //MainForm.OpenDocumentAndGotoLine( tokenInfo.DocumentFilename, tokenInfo.LocalLineIndex );
        return;
      }
      Core.Navigating.OpenDocumentAndGotoLine( OutlineProject, Core.Navigating.FindDocumentInfoByPath( tokenInfo.DocumentFilename ), tokenInfo.LocalLineIndex, tokenInfo.CharIndex, tokenInfo.Length );
    }



    private void treeProject_NodeMouseClick( DecentForms.ControlBase Sender, DecentForms.TreeView.TreeNodeMouseClickEventArgs e )
    {
      if ( ( e.Button == 2 )
      &&   ( e.Node != null ) )
      {
        if ( OutlineProject == null )
        {
          return;
        }
      }
    }



    public override void OnApplicationEvent( ApplicationEvent Event )
    {
      switch ( Event.EventType )
      {
        case ApplicationEvent.Type.PROJECT_OPENED:
          break;
        case ApplicationEvent.Type.DOCUMENT_CLOSED:
          if ( Event.Doc.ASMFileInfo != null )
          {
            if ( _ExpandedNodesPerProject.ContainsKey( Event.Doc.ASMFileInfo ) )
            {
              _ExpandedNodesPerProject.Remove( Event.Doc.ASMFileInfo );
            }
          }
          if ( ActiveDocumentInfo == Event.Doc )
          {
            ActiveDocumentInfo = null;
            NodeRoot.Nodes.Clear();
          }
          break;
        case ApplicationEvent.Type.PROJECT_CLOSED:
          
          break;
      }
    }



    private void StoreOpenNodes()
    {
      if ( ( ActiveDocumentInfo != null )
      &&   ( ActiveDocumentInfo.ASMFileInfo != null ) )
      {
        if ( !_ExpandedNodesPerProject.ContainsKey( ActiveDocumentInfo.ASMFileInfo ) )
        {
          _ExpandedNodesPerProject.Add( ActiveDocumentInfo.ASMFileInfo, new GR.Collections.Map<string, bool>() );
        }
        var expandedNodesEntry = _ExpandedNodesPerProject[ActiveDocumentInfo.ASMFileInfo];
        expandedNodesEntry.Clear();
        foreach ( var node in NodeRoot.Nodes )
        {
          expandedNodesEntry[node.Text] = node.IsExpanded;
        }
      }
    }



    public void RefreshFromDocument( BaseDocument Doc )
    {
      if ( ( Doc == null )
      ||   ( Doc.DocumentInfo.FullPath == null ) )
      {
        return;
      }

      if ( ActiveASMFileInfo == Doc.DocumentInfo.ASMFileInfo )
      {
        // nothing to do
        ActiveDocumentInfo  = Doc.DocumentInfo;
        OutlineProject      = Doc.DocumentInfo.Project;
        return;
      }

      if ( _FileInfoPerFileCache.TryGetValue( Doc.DocumentInfo.FullPath, out Types.ASM.FileInfo cachedASMFileInfo ) )
      {
        if ( ( cachedASMFileInfo != Doc.DocumentInfo.ASMFileInfo )
        &&   ( cachedASMFileInfo != null ) )
        {
          _ExpandedNodesPerProject.Remove( cachedASMFileInfo );
        }
        _FileInfoPerFileCache.Remove( Doc.DocumentInfo.FullPath );
      }
      _FileInfoPerFileCache.Add( Doc.DocumentInfo.FullPath, Doc.DocumentInfo.ASMFileInfo );

      if ( InvokeRequired )
      {
        Invoke( new MainForm.DocCallback( RefreshFromDocument ), new object[] { Doc } );
        return;
      }

      StoreOpenNodes();

      ActiveDocumentInfo  = Doc.DocumentInfo;
      OutlineProject      = Doc.DocumentInfo.Project;

      ActiveASMFileInfo   = Doc.DocumentInfo.ASMFileInfo;
      OutlineTokens       = Doc.DocumentInfo.KnownTokens;
      
      RefreshNodes();
    }



    private void RefreshNodes()
    {
      treeProject.BeginUpdate();
      NodeRoot.Nodes.Clear();

      if ( ActiveASMFileInfo == null )
      {
        treeProject.EndUpdate();
        return;
      }

      if ( !_ExpandedNodesPerProject.ContainsKey( ActiveASMFileInfo ) )
      {
        _ExpandedNodesPerProject.Add( ActiveASMFileInfo, new GR.Collections.Map<string, bool>() );
      }
      var expandedNodes = _ExpandedNodesPerProject[ActiveASMFileInfo];

      IList<SymbolInfo>     sortedTokens = null;

      // sort by line number
      if ( Core.Settings.OutlineSortByIndex )
      {
        GR.Collections.MultiMap<int, SymbolInfo> sortedTokensInner = new GR.Collections.MultiMap<int, SymbolInfo>();
        foreach ( KeyValuePair<string, SymbolInfo> token in OutlineTokens )
        {
          if ( !string.IsNullOrEmpty( Core.Settings.OutlineFilter ) )
          {
            if ( token.Key.ToUpper().Contains( Core.Settings.OutlineFilter.ToUpper() ) )
            {
              sortedTokensInner.Add( token.Value.LineIndex, token.Value );
            }
          }
          else
          {
            sortedTokensInner.Add( token.Value.LineIndex, token.Value );
          }
        }
        foreach ( var macro in ActiveASMFileInfo.Macros )
        {
          if ( string.IsNullOrEmpty( Core.Settings.OutlineFilter ) )
          {
            sortedTokensInner.Add( macro.Value.LineIndex, macro.Value.Symbol );
          }
          else if ( macro.Key.first.ToUpper().Contains( Core.Settings.OutlineFilter.ToUpper() ) )
          {
            sortedTokensInner.Add( macro.Value.LineIndex, macro.Value.Symbol );
          }
        }
        sortedTokens = sortedTokensInner.Values;
      }
      else
      {
        GR.Collections.MultiMap<string, SymbolInfo> sortedTokensInner = new GR.Collections.MultiMap<string, SymbolInfo>();
        foreach ( KeyValuePair<string, SymbolInfo> token in OutlineTokens )
        {
          if ( !string.IsNullOrEmpty( Core.Settings.OutlineFilter ) )
          {
            if ( token.Key.ToUpper().Contains( Core.Settings.OutlineFilter.ToUpper() ) )
            {
              sortedTokensInner.Add( token.Key.ToUpper(), token.Value );
            }
          }
          else
          {
            sortedTokensInner.Add( token.Key.ToUpper(), token.Value );
          }
        }
        foreach ( var macro in ActiveASMFileInfo.Macros )
        {
          if ( string.IsNullOrEmpty( Core.Settings.OutlineFilter ) )
          {
            sortedTokensInner.Add( macro.Key.first.ToUpper(), macro.Value.Symbol );
          }
          else if ( macro.Key.first.ToUpper().Contains( Core.Settings.OutlineFilter.ToUpper() ) )
          {
            sortedTokensInner.Add( macro.Key.first.ToUpper(), macro.Value.Symbol );
          }
        }
        sortedTokens = sortedTokensInner.Values;
      }

      string curZone = "";

      var parentNode = new DecentForms.TreeView.TreeNode();
      parentNode = new DecentForms.TreeView.TreeNode();
      parentNode.Text = "Global Zone";
      parentNode.ImageIndex = 0;
      NodeRoot.Nodes.Add( parentNode );
      var globalZone = parentNode;

      var zoneNodes = new Dictionary<string, DecentForms.TreeView.TreeNode>();
      zoneNodes.Add( parentNode.Text, globalZone );

      // add zone nodes first
      foreach ( var token in sortedTokens )
      {
        if ( token.Type == SymbolInfo.Types.ZONE )
        {
          if ( zoneNodes.ContainsKey( token.Zone ) )
          {
            continue;
          }
          var node = new System.Windows.Forms.TreeNode();

          node.Text = token.Name;
          node.Tag = token;

          parentNode = new DecentForms.TreeView.TreeNode();
          parentNode.Text = token.Zone;
          parentNode.ImageIndex = 0;
          NodeRoot.Nodes.Add( parentNode );
          parentNode.Tag = token;

          zoneNodes.Add( parentNode.Text, parentNode );
        }
      }

      // now add child nodes
      parentNode = globalZone;
      foreach ( var token in sortedTokens )
      {
        if ( token.Type == SymbolInfo.Types.ZONE )
        {
          curZone = token.Zone;
          parentNode = zoneNodes[token.Zone];
          continue;
        }
        if ( token.Type == SymbolInfo.Types.TEMP_LABEL )
        {
          continue;
        }

        var   node = new DecentForms.TreeView.TreeNode();
        bool  addNode = true;
        bool  addToGlobalNode = false;

        node.Text = token.Name;
        node.Tag = token;

        switch ( token.Type )
        {
          case SymbolInfo.Types.VARIABLE_NUMBER:
            node.Text += " = Number";
            break;
          case SymbolInfo.Types.VARIABLE_STRING:
            node.Text += " = String";
            break;
          case SymbolInfo.Types.VARIABLE_ARRAY:
            node.Text += " = Array";
            break;
          case SymbolInfo.Types.VARIABLE_INTEGER:
            node.Text += " = Integer";
            break;
          case SymbolInfo.Types.CONSTANT_STRING:
            node.Text += " = " + token.String;
            break;
          case SymbolInfo.Types.CONSTANT_1:
          case SymbolInfo.Types.CONSTANT_2:
            node.ImageIndex = 2;
            node.Text += " = $" + token.AddressOrValue.ToString( "X4" );
            break;
          case SymbolInfo.Types.LABEL:
            if ( ( token.Name.Contains( "." ) )
            &&   ( !Core.Settings.OutlineShowLocalLabels ) )
            {
              addNode = false;
            }
            if ( ( token.Name.StartsWith( RetroDevStudio.Parser.ASMFileParser.InternalLabelPrefix ) )
            &&   ( !Core.Settings.OutlineShowShortCutLabels ) )
            {
              addNode = false;
            }
            node.ImageIndex = 1;
            node.Text += " = $" + token.AddressOrValue.ToString( "X4" );
            break;
          case SymbolInfo.Types.PREPROCESSOR_LABEL:
          case SymbolInfo.Types.PREPROCESSOR_CONSTANT_1:
          case SymbolInfo.Types.PREPROCESSOR_CONSTANT_2:
            node.ImageIndex = 3;
            node.Text += " = $" + token.AddressOrValue.ToString( "X4" );
            break;
          case SymbolInfo.Types.UNKNOWN:
            break;
          case SymbolInfo.Types.MACRO:
            node.ImageIndex = 4;
            if ( ActiveASMFileInfo.Macros[new GR.Generic.Tupel<string,int>( token.Name, token.NumArguments )].ParameterNames.Count > 0 ) 
            {
              node.Text += " " + string.Join( ", ", ActiveASMFileInfo.Macros[new GR.Generic.Tupel<string, int>( token.Name, token.NumArguments )].ParameterNames.ToArray() );
            }
            break;
        }

        if ( !addNode )
        {
          continue;
        }

        if ( ( !token.Name.StartsWith( curZone + "." ) )
        &&   ( !token.Name.StartsWith( RetroDevStudio.Parser.ASMFileParser.InternalLabelPrefix ) ) )
        {
          addToGlobalNode = true;
        }

        // cut off zone
        try
        {
          // find parent node
          if ( ( string.IsNullOrEmpty( token.Zone ) )
          ||   ( !zoneNodes.ContainsKey( token.Zone ) )
          ||   ( addToGlobalNode ) )
          {
            globalZone.Nodes.Add( node );
            if ( ( expandedNodes.ContainsKey( globalZone.Text ) )
            &&   ( expandedNodes[globalZone.Text] ) )
            {
              globalZone.Expand();
            }
          }
          else 
          {
            var   parentZoneNode = zoneNodes[token.Zone];

            int dotPos = node.Text.IndexOf( '.' );
            string    nodeParentText = parentZoneNode.Text;
            if ( dotPos != -1 )
            {
              node.Text = node.Text.Substring( dotPos );
            }
            parentZoneNode.Nodes.Add( node );

            if ( ( expandedNodes.ContainsKey( nodeParentText ) )
            &&   ( expandedNodes[nodeParentText] ) )
            {
              parentZoneNode.Expand();
            }
          }
        }
        catch ( Exception ex )
        {
          Debug.Log( ex.Message );
        }

      }
      NodeRoot.Expand();
      treeProject.EndUpdate();
    }



    private DecentForms.TreeView.TreeNode FindNodeByText( string Text, TreeNode ParentNode )
    {
      foreach ( var node in NodeRoot.Nodes )
      {
        if ( node.Text == Text )
        {
          return node;
        }
      }
      return null;
    }



    private void checkShowLocalLabels_Click( object sender, EventArgs e )
    {
      Core.Settings.OutlineShowLocalLabels = !Core.Settings.OutlineShowLocalLabels;

      checkShowLocalLabels.Image = Core.Settings.OutlineShowLocalLabels ? RetroDevStudio.Properties.Resources.flag_green_on.ToBitmap() : RetroDevStudio.Properties.Resources.flag_green_off.ToBitmap();

      StoreOpenNodes();
      RefreshNodes();
    }



    private void checkShowShortCutLabels_Click( object sender, EventArgs e )
    {
      Core.Settings.OutlineShowShortCutLabels = !Core.Settings.OutlineShowShortCutLabels;

      checkShowShortCutLabels.Image = Core.Settings.OutlineShowShortCutLabels ? RetroDevStudio.Properties.Resources.flag_blue_on.ToBitmap() : RetroDevStudio.Properties.Resources.flag_blue_off.ToBitmap();

      StoreOpenNodes();
      RefreshNodes();
    }



    private void checkSortBySource_Click( object sender, EventArgs e )
    {
      Core.Settings.OutlineSortByIndex    = true;
      Core.Settings.OutlineSortByAlphabet = false;

      checkSortAlphabetically.Enabled = true;
      checkSortBySource.Enabled = false;

      StoreOpenNodes();
      RefreshNodes();
    }



    private void checkSortAlphabetically_Click( object sender, EventArgs e )
    {
      Core.Settings.OutlineSortByIndex = false;
      Core.Settings.OutlineSortByAlphabet = true;

      checkSortBySource.Enabled = true;
      checkSortAlphabetically.Enabled = false;

      StoreOpenNodes();
      RefreshNodes();
    }



    private void editOutlineFilter_TextChanged( object sender, EventArgs e )
    {
      Core.Settings.OutlineFilter = editOutlineFilter.Text;
      StoreOpenNodes();
      RefreshNodes();
    }



    private void treeProject_AfterSelect( DecentForms.ControlBase Sender, DecentForms.TreeView.TreeViewEventArgs e )
    {
      SymbolInfo tokenInfo = (SymbolInfo)e.Node.Tag;
      if ( tokenInfo == null )
      {
        return;
      }

      var doc = Core.Navigating.FindDocumentByPath( tokenInfo.DocumentFilename );
      if ( doc != null )
      {
        doc.HighlightText( tokenInfo.LocalLineIndex, tokenInfo.CharIndex, tokenInfo.Length );
      }

    }



  }
}
