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
  public partial class PrefKeyBindings : PrefBase
  {
    private System.Windows.Forms.Keys       m_PressedKey = Keys.None;



    public PrefKeyBindings()
    {
      InitializeComponent();
    }



    public PrefKeyBindings( StudioCore Core ) : base( Core )
    {
      _Keywords.AddRange( new string[] { "keys", "binding", "hotkey" } );
      InitializeComponent();

      RefillAcceleratorList();
    }



    private void btnImportSettings_Click( object sender, EventArgs e )
    {
      ImportLocalSettings();
    }



    private void btnExportSettings_Click( object sender, EventArgs e )
    {
      SaveLocalSettings();
    }



    public override void ExportSettings( XMLElement SettingsRoot )
    {
      var xmlAccelerators = SettingsRoot.AddChild( "Accelerators" );

      foreach ( Types.Function function in Enum.GetValues( typeof( Types.Function ) ) )
      {
        if ( function == RetroDevStudio.Types.Function.NONE )
        {
          continue;
        }

        AcceleratorKey key = Core.Settings.DetermineAccelerator( function );
        if ( key != null )
        {
          var xmlKey = new GR.Strings.XMLElement( "Function" );
          xmlKey.AddAttribute( "Function", function.ToString() );
          xmlKey.AddAttribute( "Key", key.Key.ToString() );

          xmlAccelerators.AddChild( xmlKey );
        }
      }
    }



    public override void ImportSettings( XMLElement SettingsRoot )
    {
      GR.Strings.XMLElement     xmlSettingRoot = SettingsRoot.FindByTypeRecursive( "Accelerators" );
      if ( xmlSettingRoot == null )
      {
        return;
      }

      Core.Settings.Accelerators.Clear();
      foreach ( var xmlKey in xmlSettingRoot.ChildElements )
      {
        if ( xmlKey.Type == "Function" )
        {
          try
          {
            Types.Function function = (Types.Function)Enum.Parse( typeof( Types.Function ), xmlKey.Attribute( "Function" ), true );

            Keys key = (Keys)Enum.Parse( typeof( Keys ), xmlKey.Attribute( "Key" ), true );

            Core.Settings.Accelerators.Add( key, new AcceleratorKey( key, function ) );
          }
          catch ( Exception ex )
          {
            Core.AddToOutput( "Could not parse element: " + ex.Message + System.Environment.NewLine );
          }
        }
      }
      RefillAcceleratorList();
      RefreshDisplayOnDocuments();
    }



    private void RefillAcceleratorList()
    {
      listFunctions.Items.Clear();
      foreach ( Types.Function function in Enum.GetValues( typeof( Types.Function ) ) )
      {
        if ( function == RetroDevStudio.Types.Function.NONE )
        {
          continue;
        }
        if ( !Core.Settings.Functions.ContainsKey( function ) )
        {
          continue;
        }

        ListViewItem itemF = new ListViewItem();

        itemF.Text = GR.EnumHelper.GetDescription( Core.Settings.Functions[function].State );
        itemF.SubItems.Add( Core.Settings.Functions[function].Description );

        AcceleratorKey key = Core.Settings.DetermineAccelerator( function );
        if ( key != null )
        {
          itemF.SubItems.Add( key.Key.ToString() );
          itemF.SubItems.Add( key.SecondaryKey.ToString() );
        }
        else
        {
          itemF.SubItems.Add( "" );
          itemF.SubItems.Add( "" );
        }
        itemF.Tag = function;

        listFunctions.Items.Add( itemF );
      }
    }



    private void listFunctions_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( listFunctions.SelectedItems.Count == 0 )
      {
        editKeyBinding.Enabled = false;
        btnUnbindKey.Enabled = false;
        btnBindKey.Enabled = false;
        btnBindKeySecondary.Enabled = false;
        return;
      }
      Types.Function function = (Types.Function)listFunctions.SelectedItems[0].Tag;

      editKeyBinding.Enabled = true;
      btnUnbindKey.Enabled = ( Core.Settings.DetermineAccelerator( function ) != null );
      btnBindKey.Enabled = true;
      btnBindKeySecondary.Enabled = true;
    }



    private void editKeyBinding_PreviewKeyDown( object sender, PreviewKeyDownEventArgs e )
    {
      m_PressedKey = e.KeyData;
      editKeyBinding.Text = e.KeyData.ToString();
      e.IsInputKey = true;
    }



    private void btnBindKey_Click( object sender, EventArgs e )
    {
      if ( listFunctions.SelectedItems.Count == 0 )
      {
        return;
      }
      Types.Function function = (Types.Function)listFunctions.SelectedItems[0].Tag;

      foreach ( var accPair in Core.Settings.Accelerators )
      {
        if ( accPair.Value.Function == function )
        {
          Core.Settings.Accelerators.Remove( accPair.Key, accPair.Value );
          break;
        }
      }

      if ( m_PressedKey != Keys.None )
      {
        AcceleratorKey key = new AcceleratorKey( m_PressedKey, function );
        key.Key = m_PressedKey;
        Core.Settings.Accelerators.Add( key.Key, key );
      }

      listFunctions.SelectedItems[0].SubItems[2].Text = m_PressedKey.ToString();
      btnUnbindKey.Enabled = ( Core.Settings.DetermineAccelerator( function ) != null );

      Core.MainForm.RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.KEY_BINDINGS_MODIFIED ) );
      RefreshDisplayOnDocuments();
    }



    private void btnBindKeySecondary_Click( object sender, EventArgs e )
    {
      if ( listFunctions.SelectedItems.Count == 0 )
      {
        return;
      }
      Types.Function function = (Types.Function)listFunctions.SelectedItems[0].Tag;

      bool hadAccelerator = false;
      foreach ( var accPair in Core.Settings.Accelerators )
      {
        if ( accPair.Value.Function == function )
        {
          if ( m_PressedKey != Keys.None )
          {
            accPair.Value.SecondaryKey = m_PressedKey;
            hadAccelerator = true;
            listFunctions.SelectedItems[0].SubItems[3].Text = m_PressedKey.ToString();
          }
          break;
        }
      }
      if ( !hadAccelerator )
      {
        // no entry yet, add as primary key
        if ( m_PressedKey != Keys.None )
        {
          AcceleratorKey key = new AcceleratorKey( m_PressedKey, function );
          key.Key = m_PressedKey;
          Core.Settings.Accelerators.Add( key.Key, key );
          listFunctions.SelectedItems[0].SubItems[2].Text = m_PressedKey.ToString();
        }
      }

      btnUnbindKey.Enabled = ( Core.Settings.DetermineAccelerator( function ) != null );

      Core.MainForm.RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.KEY_BINDINGS_MODIFIED ) );
      RefreshDisplayOnDocuments();
    }



    private void btnUnbindKey_Click( object sender, EventArgs e )
    {
      if ( listFunctions.SelectedItems.Count == 0 )
      {
        return;
      }
      Types.Function function = (Types.Function)listFunctions.SelectedItems[0].Tag;
      foreach ( var accPair in Core.Settings.Accelerators )
      {
        if ( accPair.Value.Function == function )
        {
          Core.Settings.Accelerators.Remove( accPair.Key, accPair.Value );

          Core.MainForm.RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.KEY_BINDINGS_MODIFIED ) );
          break;
        }
      }

      listFunctions.SelectedItems[0].SubItems[2].Text = "";
      listFunctions.SelectedItems[0].SubItems[3].Text = "";
      btnUnbindKey.Enabled = false;
    }



    private void btnSetDefaultsKeyBinding_Click( object sender, EventArgs e )
    {
      Core.Settings.SetDefaultKeyBinding();

      for ( int i = 0; i < listFunctions.Items.Count; ++i )
      {
        Types.Function function = (Types.Function)listFunctions.Items[i].Tag;

        bool  foundEntry = false;
        foreach ( var accPair in Core.Settings.Accelerators )
        {
          if ( accPair.Value.Function == function )
          {
            listFunctions.Items[i].SubItems[2].Text = accPair.Key.ToString();
            foundEntry = true;
            break;
          }
        }
        if ( !foundEntry )
        {
          listFunctions.Items[i].SubItems[2].Text = "";
        }
      }

      if ( listFunctions.SelectedItems.Count != 0 )
      {
        btnUnbindKey.Enabled = ( Core.Settings.DetermineAccelerator( (Types.Function)listFunctions.SelectedItems[0].Tag ) != null );
      }
      Core.MainForm.RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.KEY_BINDINGS_MODIFIED ) );
    }



  }



}
