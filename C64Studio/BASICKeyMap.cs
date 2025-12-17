using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace RetroDevStudio
{
  public class BASICKeyMap
  {
    public GR.Collections.Map<uint,GR.Collections.Map<System.Windows.Forms.Keys, KeymapEntry>> DefaultKeymaps = new GR.Collections.Map<uint, GR.Collections.Map<System.Windows.Forms.Keys, KeymapEntry>>();

    public GR.Collections.Map<System.Windows.Forms.Keys,KeymapEntry>    Keymap = new GR.Collections.Map<System.Windows.Forms.Keys, KeymapEntry>();

    public GR.Collections.Map<Types.PhysicalKey,KeymapEntry>            AllKeyInfos = new GR.Collections.Map<Types.PhysicalKey,KeymapEntry>();



    public BASICKeyMap()
    {
      InitDefault();
    }



    public void InitDefault()
    {
      // german
      AddKeymapEntry( Types.PhysicalKey.KEY_RUN_STOP, 7, System.Windows.Forms.Keys.Escape );

      AddKeymapEntry( Types.PhysicalKey.KEY_Q, 7, System.Windows.Forms.Keys.Q );
      AddKeymapEntry( Types.PhysicalKey.KEY_W, 7, System.Windows.Forms.Keys.W );
      AddKeymapEntry( Types.PhysicalKey.KEY_E, 7, System.Windows.Forms.Keys.E );
      AddKeymapEntry( Types.PhysicalKey.KEY_R, 7, System.Windows.Forms.Keys.R );
      AddKeymapEntry( Types.PhysicalKey.KEY_T, 7, System.Windows.Forms.Keys.T );
      AddKeymapEntry( Types.PhysicalKey.KEY_SPACE, 7, System.Windows.Forms.Keys.Space );

      AddKeymapEntry( Types.PhysicalKey.KEY_Y, 7, System.Windows.Forms.Keys.Z );
      AddKeymapEntry( Types.PhysicalKey.KEY_U, 7, System.Windows.Forms.Keys.U );
      AddKeymapEntry( Types.PhysicalKey.KEY_I, 7, System.Windows.Forms.Keys.I );
      AddKeymapEntry( Types.PhysicalKey.KEY_O, 7, System.Windows.Forms.Keys.O );
      AddKeymapEntry( Types.PhysicalKey.KEY_P, 7, System.Windows.Forms.Keys.P );

      // TODO - Layout! (DE<>EN, Dvorak?)
      AddKeymapEntry( Types.PhysicalKey.KEY_A, 7, System.Windows.Forms.Keys.A );
      AddKeymapEntry( Types.PhysicalKey.KEY_S, 7, System.Windows.Forms.Keys.S );
      AddKeymapEntry( Types.PhysicalKey.KEY_D, 7, System.Windows.Forms.Keys.D );
      AddKeymapEntry( Types.PhysicalKey.KEY_F, 7, System.Windows.Forms.Keys.F );
      AddKeymapEntry( Types.PhysicalKey.KEY_G, 7, System.Windows.Forms.Keys.G );
      AddKeymapEntry( Types.PhysicalKey.KEY_H, 7, System.Windows.Forms.Keys.H );
      AddKeymapEntry( Types.PhysicalKey.KEY_J, 7, System.Windows.Forms.Keys.J );
      AddKeymapEntry( Types.PhysicalKey.KEY_K, 7, System.Windows.Forms.Keys.K );
      AddKeymapEntry( Types.PhysicalKey.KEY_L, 7, System.Windows.Forms.Keys.L );
      AddKeymapEntry( Types.PhysicalKey.KEY_Z, 7, System.Windows.Forms.Keys.Y );
      AddKeymapEntry( Types.PhysicalKey.KEY_X, 7, System.Windows.Forms.Keys.X );
      AddKeymapEntry( Types.PhysicalKey.KEY_C, 7, System.Windows.Forms.Keys.C );
      AddKeymapEntry( Types.PhysicalKey.KEY_V, 7, System.Windows.Forms.Keys.V );
      AddKeymapEntry( Types.PhysicalKey.KEY_B, 7, System.Windows.Forms.Keys.B );
      AddKeymapEntry( Types.PhysicalKey.KEY_N, 7, System.Windows.Forms.Keys.N );
      AddKeymapEntry( Types.PhysicalKey.KEY_M, 7, System.Windows.Forms.Keys.M );
      AddKeymapEntry( Types.PhysicalKey.KEY_AT, 7, System.Windows.Forms.Keys.Oem1 );
      AddKeymapEntry( Types.PhysicalKey.KEY_COLON, 7, System.Windows.Forms.Keys.Oemtilde );
      AddKeymapEntry( Types.PhysicalKey.KEY_SEMI_COLON, 7, System.Windows.Forms.Keys.Oem7 );
      AddKeymapEntry( Types.PhysicalKey.KEY_ARROW_LEFT, 7, System.Windows.Forms.Keys.Oem5 );
      AddKeymapEntry( Types.PhysicalKey.KEY_STAR, 7, System.Windows.Forms.Keys.Oemplus );
      AddKeymapEntry( Types.PhysicalKey.KEY_MINUS, 7, System.Windows.Forms.Keys.Oem6 );
      AddKeymapEntry( Types.PhysicalKey.KEY_POUND, 7, System.Windows.Forms.Keys.Insert );
      AddKeymapEntry( Types.PhysicalKey.KEY_ARROW_UP, 7, System.Windows.Forms.Keys.Delete );
      AddKeymapEntry( Types.PhysicalKey.KEY_EQUAL, 7, System.Windows.Forms.Keys.Oem2 );
      AddKeymapEntry( Types.PhysicalKey.KEY_SLASH, 7, System.Windows.Forms.Keys.OemMinus );
      AddKeymapEntry( Types.PhysicalKey.KEY_COMMA, 7, System.Windows.Forms.Keys.Oemcomma );
      AddKeymapEntry( Types.PhysicalKey.KEY_DOT, 7, System.Windows.Forms.Keys.OemPeriod );
      AddKeymapEntry( Types.PhysicalKey.KEY_1, 7, System.Windows.Forms.Keys.D1 );
      AddKeymapEntry( Types.PhysicalKey.KEY_2, 7, System.Windows.Forms.Keys.D2 );
      AddKeymapEntry( Types.PhysicalKey.KEY_3, 7, System.Windows.Forms.Keys.D3 );
      AddKeymapEntry( Types.PhysicalKey.KEY_4, 7, System.Windows.Forms.Keys.D4 );
      AddKeymapEntry( Types.PhysicalKey.KEY_5, 7, System.Windows.Forms.Keys.D5 );
      AddKeymapEntry( Types.PhysicalKey.KEY_6, 7, System.Windows.Forms.Keys.D6 );
      AddKeymapEntry( Types.PhysicalKey.KEY_7, 7, System.Windows.Forms.Keys.D7 );
      AddKeymapEntry( Types.PhysicalKey.KEY_8, 7, System.Windows.Forms.Keys.D8 );
      AddKeymapEntry( Types.PhysicalKey.KEY_9, 7, System.Windows.Forms.Keys.D9 );
      AddKeymapEntry( Types.PhysicalKey.KEY_0, 7, System.Windows.Forms.Keys.D0 );

      AddKeymapEntry( Types.PhysicalKey.KEY_PLUS, 7, System.Windows.Forms.Keys.OemOpenBrackets );

      AddKeymapEntry( Types.PhysicalKey.KEY_CLR_HOME, 7, System.Windows.Forms.Keys.Home );
      AddKeymapEntry( Types.PhysicalKey.KEY_INST_DEL, 7, System.Windows.Forms.Keys.Back );
      AddKeymapEntry( Types.PhysicalKey.KEY_RETURN, 7, System.Windows.Forms.Keys.Return );
      AddKeymapEntry( Types.PhysicalKey.KEY_F1, 7, System.Windows.Forms.Keys.F1 );
      AddKeymapEntry( Types.PhysicalKey.KEY_F3, 7, System.Windows.Forms.Keys.F3 );
      AddKeymapEntry( Types.PhysicalKey.KEY_F5, 7, System.Windows.Forms.Keys.F5 );
      AddKeymapEntry( Types.PhysicalKey.KEY_F7, 7, System.Windows.Forms.Keys.F7 );

      // cursor keys
      AddKeymapEntry( Types.PhysicalKey.KEY_CURSOR_UP_DOWN, 7, System.Windows.Forms.Keys.Down );
      AddKeymapEntry( Types.PhysicalKey.KEY_CURSOR_LEFT_RIGHT, 7, System.Windows.Forms.Keys.Right );

      AddKeymapEntry( Types.PhysicalKey.KEY_SIM_CURSOR_LEFT, 7, System.Windows.Forms.Keys.Left );
      AddKeymapEntry( Types.PhysicalKey.KEY_SIM_CURSOR_UP, 7, System.Windows.Forms.Keys.Up );

      AddKeymapEntry( Types.PhysicalKey.KEY_FLASH, 7, System.Windows.Forms.Keys.OemBackslash );

      // english
      AddKeymapEntry( Types.PhysicalKey.KEY_RUN_STOP, 9, System.Windows.Forms.Keys.Escape );

      AddKeymapEntry( Types.PhysicalKey.KEY_SPACE, 9, System.Windows.Forms.Keys.Space );

      AddKeymapEntry( Types.PhysicalKey.KEY_Q, 9, System.Windows.Forms.Keys.Q );
      AddKeymapEntry( Types.PhysicalKey.KEY_W, 9, System.Windows.Forms.Keys.W );
      AddKeymapEntry( Types.PhysicalKey.KEY_E, 9, System.Windows.Forms.Keys.E );
      AddKeymapEntry( Types.PhysicalKey.KEY_R, 9, System.Windows.Forms.Keys.R );
      AddKeymapEntry( Types.PhysicalKey.KEY_T, 9, System.Windows.Forms.Keys.T );
                                                                              
                                                                              
      AddKeymapEntry( Types.PhysicalKey.KEY_Y, 9, System.Windows.Forms.Keys.Y );
      AddKeymapEntry( Types.PhysicalKey.KEY_U, 9, System.Windows.Forms.Keys.U );
      AddKeymapEntry( Types.PhysicalKey.KEY_I, 9, System.Windows.Forms.Keys.I );
      AddKeymapEntry( Types.PhysicalKey.KEY_O, 9, System.Windows.Forms.Keys.O );
      AddKeymapEntry( Types.PhysicalKey.KEY_P, 9, System.Windows.Forms.Keys.P );
                                                                              
      // TODO - Layout! (DE<>EN, Dvorak?)                                     
      AddKeymapEntry( Types.PhysicalKey.KEY_A, 9, System.Windows.Forms.Keys.A );
      AddKeymapEntry( Types.PhysicalKey.KEY_S, 9, System.Windows.Forms.Keys.S );
      AddKeymapEntry( Types.PhysicalKey.KEY_D, 9, System.Windows.Forms.Keys.D );
      AddKeymapEntry( Types.PhysicalKey.KEY_F, 9, System.Windows.Forms.Keys.F );
      AddKeymapEntry( Types.PhysicalKey.KEY_G, 9, System.Windows.Forms.Keys.G );
      AddKeymapEntry( Types.PhysicalKey.KEY_H, 9, System.Windows.Forms.Keys.H );
      AddKeymapEntry( Types.PhysicalKey.KEY_J, 9, System.Windows.Forms.Keys.J );
      AddKeymapEntry( Types.PhysicalKey.KEY_K, 9, System.Windows.Forms.Keys.K );
      AddKeymapEntry( Types.PhysicalKey.KEY_L, 9, System.Windows.Forms.Keys.L );
      AddKeymapEntry( Types.PhysicalKey.KEY_Z, 9, System.Windows.Forms.Keys.Z );
      AddKeymapEntry( Types.PhysicalKey.KEY_X, 9, System.Windows.Forms.Keys.X );
      AddKeymapEntry( Types.PhysicalKey.KEY_C, 9, System.Windows.Forms.Keys.C );
      AddKeymapEntry( Types.PhysicalKey.KEY_V, 9, System.Windows.Forms.Keys.V );
      AddKeymapEntry( Types.PhysicalKey.KEY_B, 9, System.Windows.Forms.Keys.B );
      AddKeymapEntry( Types.PhysicalKey.KEY_N, 9, System.Windows.Forms.Keys.N );
      AddKeymapEntry( Types.PhysicalKey.KEY_M, 9, System.Windows.Forms.Keys.M );
      AddKeymapEntry( Types.PhysicalKey.KEY_AT, 9, System.Windows.Forms.Keys.Oem1 );
      AddKeymapEntry( Types.PhysicalKey.KEY_COLON, 9, System.Windows.Forms.Keys.Oemtilde );
      AddKeymapEntry( Types.PhysicalKey.KEY_SEMI_COLON, 9, System.Windows.Forms.Keys.Oem7 );
      AddKeymapEntry( Types.PhysicalKey.KEY_ARROW_LEFT, 9, System.Windows.Forms.Keys.Oem5 );
      AddKeymapEntry( Types.PhysicalKey.KEY_STAR, 9, System.Windows.Forms.Keys.Oemplus );
      AddKeymapEntry( Types.PhysicalKey.KEY_MINUS, 9, System.Windows.Forms.Keys.Oem6 );
      AddKeymapEntry( Types.PhysicalKey.KEY_POUND, 9, System.Windows.Forms.Keys.Insert );
      AddKeymapEntry( Types.PhysicalKey.KEY_ARROW_UP, 9, System.Windows.Forms.Keys.Delete );
      AddKeymapEntry( Types.PhysicalKey.KEY_EQUAL, 9, System.Windows.Forms.Keys.Oem2 );
      AddKeymapEntry( Types.PhysicalKey.KEY_SLASH, 9, System.Windows.Forms.Keys.OemMinus );
      AddKeymapEntry( Types.PhysicalKey.KEY_COMMA, 9, System.Windows.Forms.Keys.Oemcomma );
      AddKeymapEntry( Types.PhysicalKey.KEY_DOT, 9, System.Windows.Forms.Keys.OemPeriod );

      AddKeymapEntry( Types.PhysicalKey.KEY_1, 9, System.Windows.Forms.Keys.D1 );
      AddKeymapEntry( Types.PhysicalKey.KEY_2, 9, System.Windows.Forms.Keys.D2 );
      AddKeymapEntry( Types.PhysicalKey.KEY_3, 9, System.Windows.Forms.Keys.D3 );
      AddKeymapEntry( Types.PhysicalKey.KEY_4, 9, System.Windows.Forms.Keys.D4 );
      AddKeymapEntry( Types.PhysicalKey.KEY_5, 9, System.Windows.Forms.Keys.D5 );
      AddKeymapEntry( Types.PhysicalKey.KEY_6, 9, System.Windows.Forms.Keys.D6 );
      AddKeymapEntry( Types.PhysicalKey.KEY_7, 9, System.Windows.Forms.Keys.D7 );
      AddKeymapEntry( Types.PhysicalKey.KEY_8, 9, System.Windows.Forms.Keys.D8 );
      AddKeymapEntry( Types.PhysicalKey.KEY_9, 9, System.Windows.Forms.Keys.D9 );
      AddKeymapEntry( Types.PhysicalKey.KEY_0, 9, System.Windows.Forms.Keys.D0 );

      AddKeymapEntry( Types.PhysicalKey.KEY_PLUS, 9, System.Windows.Forms.Keys.OemOpenBrackets );

      AddKeymapEntry( Types.PhysicalKey.KEY_CLR_HOME, 9, System.Windows.Forms.Keys.Home );
      AddKeymapEntry( Types.PhysicalKey.KEY_INST_DEL, 9, System.Windows.Forms.Keys.Back );
      AddKeymapEntry( Types.PhysicalKey.KEY_RETURN, 9, System.Windows.Forms.Keys.Return );
      AddKeymapEntry( Types.PhysicalKey.KEY_F1, 9, System.Windows.Forms.Keys.F1 );
      AddKeymapEntry( Types.PhysicalKey.KEY_F3, 9, System.Windows.Forms.Keys.F3 );
      AddKeymapEntry( Types.PhysicalKey.KEY_F5, 9, System.Windows.Forms.Keys.F5 );
      AddKeymapEntry( Types.PhysicalKey.KEY_F7, 9, System.Windows.Forms.Keys.F7 );

      AddKeymapEntry( Types.PhysicalKey.KEY_CURSOR_UP_DOWN, 9, System.Windows.Forms.Keys.Down );
      AddKeymapEntry( Types.PhysicalKey.KEY_CURSOR_LEFT_RIGHT, 9, System.Windows.Forms.Keys.Right );
      AddKeymapEntry( Types.PhysicalKey.KEY_SIM_CURSOR_LEFT, 9, System.Windows.Forms.Keys.Left );
      AddKeymapEntry( Types.PhysicalKey.KEY_SIM_CURSOR_UP, 9, System.Windows.Forms.Keys.Up );

      AddKeymapEntry( Types.PhysicalKey.KEY_FLASH, 9, System.Windows.Forms.Keys.OemBackslash );
    }



    private void AddKeymapEntry( Types.PhysicalKey KeyboardKey, uint NeutralLangID, System.Windows.Forms.Keys Key )
    {
      KeymapEntry entry = new KeymapEntry();

      entry.KeyboardKey = KeyboardKey;
      entry.Key = Key;
      if ( DefaultKeymaps[NeutralLangID] == null )
      {
        DefaultKeymaps[NeutralLangID] = new GR.Collections.Map<System.Windows.Forms.Keys, KeymapEntry>();
      }
      DefaultKeymaps[NeutralLangID][Key] = entry;

      AllKeyInfos[KeyboardKey] = entry;
    }



    public KeymapEntry GetKeymapEntry( System.Windows.Forms.Keys keyData )
    {
      if ( !Keymap.ContainsKey( keyData ) )
      {
        return null;
      }
      return Keymap[keyData];
    }



    public KeymapEntry GetDefaultKeymapEntry( uint curNeutralLangID, System.Windows.Forms.Keys keyData )
    {
      if ( !DefaultKeymaps.ContainsKey( curNeutralLangID ) )
      {
        // fallback to english
        if ( curNeutralLangID == 9 )
        {
          return null;
        }
        if ( curNeutralLangID != 9 )
        {
          curNeutralLangID = 9;
        }
      }
      if ( ( !DefaultKeymaps.ContainsKey( curNeutralLangID ) )
      &&   ( !DefaultKeymaps[curNeutralLangID].ContainsKey( keyData ) ) )
      {
        return null;
      }
      return DefaultKeymaps[curNeutralLangID][keyData];
    }



    public bool DefaultKeymapEntryExists( uint curNeutralLangID, System.Windows.Forms.Keys keyData )
    {
      if ( !DefaultKeymaps.ContainsKey( curNeutralLangID ) )
      {
        return false;
      }
      return DefaultKeymaps[curNeutralLangID].ContainsKey( keyData );
    }



    public bool KeymapEntryExists( System.Windows.Forms.Keys keyData )
    {
      return Keymap.ContainsKey( keyData );
    }




  }
}
