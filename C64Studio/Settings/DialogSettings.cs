using GR.Collections;
using GR.IO;
using GR.Memory;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace RetroDevStudio
{
  public class DialogSettings
  {
    private Dictionary<string,string>       _Settings = new Dictionary<string,string>();



    public void StoreSetting( string Key, string Value )
    {
      _Settings[Key] = Value;
    }



    public void StoreSetting( string Key, int Value )
    {
      _Settings[Key] = Value.ToString();
    }



    public string GetSetting( string Key )
    {
      if ( _Settings.ContainsKey( Key ) )
      {
        return _Settings[Key];
      }
      return "";
    }



    public int GetSettingI( string Key )
    {
      return GR.Convert.ToI32( GetSetting( Key ) );
    }



    public void StoreAppearance( string ControlKey, Form Form )
    {
      StoreSetting( ControlKey + ".State", (int)Form.WindowState );
      if ( Form.WindowState == FormWindowState.Maximized )
      {
        StoreSetting( ControlKey + ".X", Form.RestoreBounds.X );
        StoreSetting( ControlKey + ".Y", Form.RestoreBounds.Y );
        StoreSetting( ControlKey + ".W", Form.RestoreBounds.Width );
        StoreSetting( ControlKey + ".H", Form.RestoreBounds.Height );
      }
      else
      {
        StoreSetting( ControlKey + ".X", Form.Location.X );
        StoreSetting( ControlKey + ".Y", Form.Location.Y );
        StoreSetting( ControlKey + ".W", Form.Width );
        StoreSetting( ControlKey + ".H", Form.Height );
      }
    }



    public void RestoreAppearance( string ControlKey, Form Form )
    {
      var state = (FormWindowState)GetSettingI( ControlKey + ".State" );

      int x = GetSettingI( ControlKey + ".X" );
      int y = GetSettingI( ControlKey + ".Y" );
      int w = GetSettingI( ControlKey + ".W" );
      int h = GetSettingI( ControlKey + ".H" );

      if ( ( w > 0 )
      &&   ( h > 0 ) )
      {
        Form.SetBounds( x, y, w, h );
        if ( state != FormWindowState.Normal )
        {
          Form.WindowState = state;
        }
      }
    }



    public void StoreListViewColumns( string ControlKey, ListView List )
    {
      foreach ( ColumnHeader col in List.Columns )
      {
        StoreSetting( ControlKey + ".ColumnWidth." + col.Index, col.Width.ToString() );
      }
    }



    public void RestoreListViewColumns( string ControlKey, ListView List )
    {
      foreach ( ColumnHeader col in List.Columns )
      {
        int   width = GetSettingI( ControlKey + ".ColumnWidth." + col.Index );
        if ( width > 0 )
        {
          col.Width = width;
        }
      }
    }



    public ByteBuffer ToBuffer()
    {
      var result = new ByteBuffer();

      GR.IO.FileChunk   chunkDetails = new GR.IO.FileChunk( FileChunkConstants.SETTINGS_DIALOG_APPEARANCE );

      chunkDetails.AppendI32( _Settings.Count );
      foreach ( var entry in _Settings )
      {
        chunkDetails.AppendString( entry.Key );
        chunkDetails.AppendString( entry.Value );
      }

      return chunkDetails.ToBuffer();
    }



    public void ReadFromBuffer( IReader BinIn )
    {
      _Settings.Clear();

      int   numEntries = BinIn.ReadInt32();
      for ( int i = 0; i < numEntries; ++i )
      {
        string    key = BinIn.ReadString();
        string    value = BinIn.ReadString();

        _Settings[key] = value;
      }
    }



  }
}