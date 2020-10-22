using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace C64Studio
{
  public partial class Outline : BaseDocument
  {
    public System.Windows.Forms.TreeNode      NodeRoot = null;
    private Project                           OutlineProject = null;
    private GR.Collections.MultiMap<string,Types.SymbolInfo>       OutlineTokens = new GR.Collections.MultiMap<string, Types.SymbolInfo>();
    private GR.Collections.Map<string,bool>   _ExpandedNodes = new GR.Collections.Map<string, bool>();



    public Outline()
    {
      InitializeComponent();

      NodeRoot = new System.Windows.Forms.TreeNode( "Outline" );
      NodeRoot.ImageIndex = 0;
      NodeRoot.SelectedImageIndex = 0;
      treeProject.Nodes.Add( NodeRoot );
    }



    private void treeProject_NodeMouseDoubleClick( object sender, System.Windows.Forms.TreeNodeMouseClickEventArgs e )
    {
      Types.SymbolInfo tokenInfo = (Types.SymbolInfo)e.Node.Tag;
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
      Core.Navigating.OpenDocumentAndGotoLine( OutlineProject, Core.Navigating.FindDocumentInfoByPath( tokenInfo.DocumentFilename ), tokenInfo.LocalLineIndex );
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

      IList<Types.SymbolInfo>     sortedTokens = null;

      // sort by line number
      if ( Core.Settings.OutlineSortByIndex )
      {
        GR.Collections.MultiMap<int, Types.SymbolInfo> sortedTokensInner = new GR.Collections.MultiMap<int, C64Studio.Types.SymbolInfo>();
        foreach ( KeyValuePair<string, Types.SymbolInfo> token in OutlineTokens )
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
        GR.Collections.MultiMap<string, Types.SymbolInfo> sortedTokensInner = new GR.Collections.MultiMap<string, C64Studio.Types.SymbolInfo>();
        foreach ( KeyValuePair<string, Types.SymbolInfo> token in OutlineTokens )
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

      Dictionary<string,TreeNode>     zoneNodes = new Dictionary<string, TreeNode>();
      zoneNodes.Add( parentNode.Text, globalZone );

      // add zone nodes first
      foreach ( var token in sortedTokens )
      {
        if ( token.Type == C64Studio.Types.SymbolInfo.Types.ZONE )
        {
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
        if ( token.Type == C64Studio.Types.SymbolInfo.Types.ZONE )
        {
          curZone = token.Zone;
          parentNode = zoneNodes[token.Zone];
          continue;
        }

        System.Windows.Forms.TreeNode node = new System.Windows.Forms.TreeNode();
        bool addNode = true;

        node.Text = token.Name;
        node.Tag = token;

        switch ( token.Type )
        {
          case C64Studio.Types.SymbolInfo.Types.CONSTANT_1:
          case C64Studio.Types.SymbolInfo.Types.CONSTANT_2:
            node.ImageIndex = node.SelectedImageIndex = 2;
            node.Text += " = $" + token.AddressOrValue.ToString( "X4" );
            break;
          case C64Studio.Types.SymbolInfo.Types.LABEL:
            if ( ( token.Name.Contains( "." ) )
            &&   ( !Core.Settings.OutlineShowLocalLabels ) )
            {
              addNode = false;
            }
            if ( ( token.Name.StartsWith( C64Studio.Parser.ASMFileParser.InternalLabelPrefix ) )
            &&   ( !Core.Settings.OutlineShowShortCutLabels ) )
            {
              addNode = false;
            }
            node.ImageIndex = node.SelectedImageIndex = 1;
            node.Text += " = $" + token.AddressOrValue.ToString( "X4" );
            break;
          case C64Studio.Types.SymbolInfo.Types.PREPROCESSOR_LABEL:
          case C64Studio.Types.SymbolInfo.Types.PREPROCESSOR_CONSTANT_1:
          case C64Studio.Types.SymbolInfo.Types.PREPROCESSOR_CONSTANT_2:
            node.ImageIndex = node.SelectedImageIndex = 3;
            node.Text += " = $" + token.AddressOrValue.ToString( "X4" );
            break;
          case C64Studio.Types.SymbolInfo.Types.UNKNOWN:
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

      checkShowLocalLabels.Image = Core.Settings.OutlineShowLocalLabels ? C64Studio.Properties.Resources.flag_green_on.ToBitmap() : C64Studio.Properties.Resources.flag_green_off.ToBitmap();

      StoreOpenNodes();
      RefreshNodes();
    }



    private void checkShowShortCutLabels_Click( object sender, EventArgs e )
    {
      Core.Settings.OutlineShowShortCutLabels = !Core.Settings.OutlineShowShortCutLabels;

      checkShowShortCutLabels.Image = Core.Settings.OutlineShowShortCutLabels ? C64Studio.Properties.Resources.flag_blue_on.ToBitmap() : C64Studio.Properties.Resources.flag_blue_off.ToBitmap();

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



  }
}
