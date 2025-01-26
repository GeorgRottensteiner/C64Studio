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
  [Description( "BASIC.Key Binding" )]
  public partial class DlgPrefBASICKeyBindings : DlgPrefBase
  {
    private System.Windows.Forms.Keys       m_PressedKeyMapKey = Keys.None;


    public DlgPrefBASICKeyBindings()
    {
      InitializeComponent();
    }



    public DlgPrefBASICKeyBindings( StudioCore Core ) : base( Core )
    {
      _Keywords.AddRange( new string[] { "keys", "binding", "hotkey", "basic" } );

      InitializeComponent();
    }



    public override void ApplySettingsToControls()
    {
      RefillBASICKeyMappingList();
    }



    public override void ImportSettings( XMLElement SettingsRoot )
    {
      GR.Strings.XMLElement     xmlSettingRoot = SettingsRoot.FindByTypeRecursive( "BASICKeyMap" );
      if ( xmlSettingRoot == null )
      {
        return;
      }

      Core.Settings.BASICKeyMap.Keymap.Clear();
      foreach ( var xmlKey in xmlSettingRoot.ChildElements )
      {
        if ( xmlKey.Type == "Key" )
        {
          string    c64key = xmlKey.Attribute( "C64Key" );

          try
          {
            Keys key = (Keys)Enum.Parse( typeof( Keys ), xmlKey.Attribute( "FormsKey" ), true );
            Types.KeyboardKey c64Key = (Types.KeyboardKey)Enum.Parse( typeof( Types.KeyboardKey ), xmlKey.Attribute( "C64Key" ), true );

            Core.Settings.BASICKeyMap.Keymap.Add( key, new KeymapEntry() { Key = key, KeyboardKey = c64Key } );
          }
          catch ( Exception ex )
          {
            Core.AddToOutput( "Could not parse element: " + ex.Message + System.Environment.NewLine );
          }
        }
      }
    }



    public override void ExportSettings( XMLElement SettingsRoot )
    {
      GR.Strings.XMLElement     xmlSettingRoot = new GR.Strings.XMLElement( "BASICKeyMap" );
      SettingsRoot.AddChild( xmlSettingRoot );

      foreach ( var entry in Core.Settings.BASICKeyMap.Keymap )
      {
        var xmlKey = new GR.Strings.XMLElement( "Key" );
        xmlKey.AddAttribute( "FormsKey", entry.Key.ToString() );
        xmlKey.AddAttribute( "C64Key", entry.Value.KeyboardKey.ToString() );

        xmlSettingRoot.AddChild( xmlKey );
      }
    }



    private void RefillBASICKeyMappingList()
    {
      listBASICKeyMap.Items.Clear();
      foreach ( RetroDevStudio.Types.KeyboardKey realKey in Enum.GetValues( typeof( RetroDevStudio.Types.KeyboardKey ) ) )
      {
        if ( !IsKeyMappable( realKey ) )
        {
          continue;
        }

        ListViewItem    item = new ListViewItem( realKey.ToString() );

        if ( ConstantData.PhysicalKeyInfo.ContainsKey( realKey ) )
        {
          var charInfo = ConstantData.PhysicalKeyInfo[realKey];

          item.Text = charInfo.Normal.Desc;
          item.SubItems.Add( charInfo.Normal.PetSCIIValue.ToString( "X02" ) );
        }
        else if ( realKey == Types.KeyboardKey.KEY_SIM_CURSOR_LEFT )
        {
          item.Text = "CURSOR LEFT (simulated)";
          item.SubItems.Add( "??" );
        }
        else if ( realKey == Types.KeyboardKey.KEY_SIM_CURSOR_UP )
        {
          item.Text = "CURSOR UP (simulated)";
          item.SubItems.Add( "??" );
        }
        else
        {
          item.SubItems.Add( "??" );
        }

        var keyMapEntry = FindBASICKeyMapEntry( realKey );
        if ( keyMapEntry != null )
        {
          item.SubItems.Add( keyMapEntry.Key.ToString() );
        }
        else
        {
          item.SubItems.Add( "--" );
        }
        // ?
        item.SubItems.Add( "--" );

        item.Tag = realKey;
        listBASICKeyMap.Items.Add( item );
      }
    }



    private bool IsKeyMappable( Types.KeyboardKey Key )
    {
      if ( ( Key == RetroDevStudio.Types.KeyboardKey.UNDEFINED )
      || ( Key == Types.KeyboardKey.LAST_ENTRY )
      || ( Key == Types.KeyboardKey.KEY_RESTORE )
      || ( Key == Types.KeyboardKey.KEY_SHIFT_LOCK )
      || ( Key == Types.KeyboardKey.KEY_COMMODORE )
      || ( Key == Types.KeyboardKey.KEY_SHIFT_LEFT )
      || ( Key == Types.KeyboardKey.KEY_SHIFT_RIGHT )
      || ( Key == Types.KeyboardKey.KEY_CTRL ) )
      {
        return false;
      }
      return true;
    }



    private KeymapEntry FindBASICKeyMapEntry( Types.KeyboardKey RealKey )
    {
      foreach ( var entry in Core.Settings.BASICKeyMap.Keymap )
      {
        if ( entry.Value.KeyboardKey == RealKey )
        {
          return entry.Value;
        }
      }
      return null;
    }



    private void editBASICKeyMapBinding_PreviewKeyDown( object sender, PreviewKeyDownEventArgs e )
    {
      m_PressedKeyMapKey = e.KeyData;
      editBASICKeyMapBinding.Text = e.KeyData.ToString();
      e.IsInputKey = true;
    }



    private void btnBindBASICKeyMapBinding_Click( DecentForms.ControlBase Sender )
    {
      if ( listBASICKeyMap.SelectedItems.Count == 0 )
      {
        return;
      }
      var   realKey = (Types.KeyboardKey)listBASICKeyMap.SelectedItems[0].Tag;
      var   keyMapEntry = FindBASICKeyMapEntry( realKey );

      if ( ( m_PressedKeyMapKey != Keys.None )
      &&   ( ( keyMapEntry == null )
      ||     ( m_PressedKeyMapKey != keyMapEntry.Key ) ) )
      {
        restart: ;
        foreach ( var keyInfo in Core.Settings.BASICKeyMap.Keymap )
        {
          if ( keyInfo.Value.KeyboardKey == realKey )
          {
            Core.Settings.BASICKeyMap.Keymap.Remove( keyInfo.Key );
            goto restart;
          }
        }
        Core.Settings.BASICKeyMap.Keymap.Remove( m_PressedKeyMapKey );

        keyMapEntry = Core.Settings.BASICKeyMap.AllKeyInfos[realKey];
        if ( keyMapEntry != null )
        {
          keyMapEntry.Key = m_PressedKeyMapKey;
          Core.Settings.BASICKeyMap.Keymap.Add( m_PressedKeyMapKey, keyMapEntry );
        }
        else
        {
          keyMapEntry = new KeymapEntry()
          {
            KeyboardKey = realKey,
            Key         = m_PressedKeyMapKey
          };
          Core.Settings.BASICKeyMap.Keymap.Add( m_PressedKeyMapKey, keyMapEntry );
        }

        listBASICKeyMap.SelectedItems[0].SubItems[2].Text = m_PressedKeyMapKey.ToString();
        btnUnbindBASICKeyMapBinding.Enabled = true;
      }
    }



    private void btnUnbindBASICKeyMapBinding_Click( DecentForms.ControlBase Sender )
    {
      if ( listBASICKeyMap.SelectedItems.Count == 0 )
      {
        return;
      }
      var    realKey = (Types.KeyboardKey)listBASICKeyMap.SelectedItems[0].Tag;

      restart:;
      foreach ( var keyInfo in Core.Settings.BASICKeyMap.Keymap )
      {
        if ( keyInfo.Value.KeyboardKey == realKey )
        {
          Core.Settings.BASICKeyMap.Keymap.Remove( keyInfo.Key );
          goto restart;
        }
      }
      if ( Core.Settings.BASICKeyMap.AllKeyInfos.ContainsKey( realKey ) )
      {
        Core.Settings.BASICKeyMap.AllKeyInfos[realKey].Key = Keys.None;
      }
      listBASICKeyMap.SelectedItems[0].SubItems[2].Text = "--";
      btnUnbindBASICKeyMapBinding.Enabled = false;
    }



    private void listBASICKeyMap_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( listBASICKeyMap.SelectedItems.Count == 0 )
      {
        editBASICKeyMapBinding.Enabled = false;
        btnUnbindBASICKeyMapBinding.Enabled = false;
        btnBindBASICKeyMapBinding.Enabled = false;
        return;
      }
      var    realKey = (Types.KeyboardKey)listBASICKeyMap.SelectedItems[0].Tag;

      var keyMapEntry = FindBASICKeyMapEntry( realKey );
      btnUnbindBASICKeyMapBinding.Enabled = ( keyMapEntry != null );
      editBASICKeyMapBinding.Enabled = true;
      btnBindBASICKeyMapBinding.Enabled = true;
    }



  }



}
