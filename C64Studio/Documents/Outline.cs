using RetroDevStudio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;



namespace RetroDevStudio
{
  public partial class Outline : BaseDocument
  {
    public System.Windows.Forms.TreeNode      NodeRoot = null;
    private Project                           OutlineProject = null;
    private GR.Collections.MultiMap<string,SymbolInfo>       OutlineTokens = new GR.Collections.MultiMap<string, SymbolInfo>();
    private GR.Collections.Map<string,bool>   _ExpandedNodes = new GR.Collections.Map<string, bool>();



    public Outline()
    {
      InitializeComponent();

      GR.Image.DPIHandler.ResizeControlsForDPI( this );

      NodeRoot = new System.Windows.Forms.TreeNode( "Outline" );
      NodeRoot.ImageIndex = 0;
      NodeRoot.SelectedImageIndex = 0;
      treeProject.Nodes.Add( NodeRoot );
    }



    private void treeProject_NodeMouseDoubleClick( object sender, System.Windows.Forms.TreeNodeMouseClickEventArgs e )
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



    private void treeProject_NodeMouseClick( object sender, System.Windows.Forms.TreeNodeMouseClickEventArgs e )
    {
      if ( ( e.Button == System.Windows.Forms.MouseButtons.Right )
      &&   ( e.Node != null ) )
      {
        if ( OutlineProject == null )
        {
          return;
        }
      }
    }



    private void StoreOpenNodes()
    {
      _ExpandedNodes.Clear();

      foreach ( TreeNode node in NodeRoot.Nodes )
      {
        _ExpandedNodes[node.Text] = node.IsExpanded;
      }
    }



    public void RefreshFromDocument( BaseDocument Doc )
    {
      if ( Doc == null )
      {
        return;
      }
      if ( ( OutlineProject == Doc.DocumentInfo.Project )
      &&   ( OutlineTokens == Doc.DocumentInfo.KnownTokens ) )
      {
        // nothing to do
        return;
      }

      if ( InvokeRequired )
      {
        Invoke( new MainForm.DocCallback( RefreshFromDocument ), new object[] { Doc } );
        return;
      }

      OutlineProject = Doc.DocumentInfo.Project;
      OutlineTokens = Doc.DocumentInfo.KnownTokens;
      //Debug.Log( "Set to " + OutlineTokens.Count + " tokens from doc " + Doc.DocumentInfo.DocumentFilename );

      StoreOpenNodes();
      RefreshNodes();
    }



    private void RefreshNodes()
    {
      treeProject.BeginUpdate();
      NodeRoot.Nodes.Clear();

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
        sortedTokens = sortedTokensInner.Values;
      }

      string curZone = "";

      System.Windows.Forms.TreeNode parentNode = new System.Windows.Forms.TreeNode();
      parentNode = new System.Windows.Forms.TreeNode();
      parentNode.Text = "Global Zone";
      parentNode.ImageIndex = parentNode.SelectedImageIndex = 0;
      NodeRoot.Nodes.Add( parentNode );
      System.Windows.Forms.TreeNode globalZone = parentNode;

      var zoneNodes = new Dictionary<string, TreeNode>();
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
          System.Windows.Forms.TreeNode node = new System.Windows.Forms.TreeNode();

          node.Text = token.Name;
          node.Tag = token;

          parentNode = new System.Windows.Forms.TreeNode();
          parentNode.Text = token.Zone;
          parentNode.ImageIndex = parentNode.SelectedImageIndex = 0;
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

        System.Windows.Forms.TreeNode node = new System.Windows.Forms.TreeNode();
        bool addNode = true;

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
            node.ImageIndex = node.SelectedImageIndex = 2;
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
            node.ImageIndex = node.SelectedImageIndex = 1;
            node.Text += " = $" + token.AddressOrValue.ToString( "X4" );
            break;
          case SymbolInfo.Types.PREPROCESSOR_LABEL:
          case SymbolInfo.Types.PREPROCESSOR_CONSTANT_1:
          case SymbolInfo.Types.PREPROCESSOR_CONSTANT_2:
            node.ImageIndex = node.SelectedImageIndex = 3;
            node.Text += " = $" + token.AddressOrValue.ToString( "X4" );
            break;
          case SymbolInfo.Types.UNKNOWN:
            break;
        }

        if ( !addNode )
        {
          continue;
        }

        // cut off zone
        try
        {
          // find parent node
          if ( ( string.IsNullOrEmpty( token.Zone ) )
          ||   ( !zoneNodes.ContainsKey( token.Zone ) ) )
          {
            globalZone.Nodes.Add( node );
            if ( ( _ExpandedNodes.ContainsKey( globalZone.Text ) )
            &&   ( _ExpandedNodes[globalZone.Text] ) )
            {
              globalZone.Expand();
            }
          }
          else 
          //if ( curZone.Length > 0 )
          {
            var   parentZoneNode = zoneNodes[token.Zone];

            int dotPos = node.Text.IndexOf( '.' );
            string    nodeParentText = parentZoneNode.Text;
            if ( dotPos != -1 )
            {
              node.Text = node.Text.Substring( dotPos );
            }
            parentZoneNode.Nodes.Add( node );

            if ( ( _ExpandedNodes.ContainsKey( nodeParentText ) )
            &&   ( _ExpandedNodes[nodeParentText] ) )
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



    private TreeNode FindNodeByText( string Text, TreeNode ParentNode )
    {
      foreach ( System.Windows.Forms.TreeNode node in NodeRoot.Nodes )
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
      Core.Settings.OutlineSortByIndex = true;
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



    private void treeProject_AfterSelect( object sender, TreeViewEventArgs e )
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
