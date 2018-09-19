using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio
{
  public class BASICKeyMap
  {
    public GR.Collections.Map<uint,GR.Collections.Map<System.Windows.Forms.Keys, KeymapEntry>> DefaultKeymaps = new GR.Collections.Map<uint, GR.Collections.Map<System.Windows.Forms.Keys, KeymapEntry>>();

    public GR.Collections.Map<System.Windows.Forms.Keys,KeymapEntry>    Keymap = new GR.Collections.Map<System.Windows.Forms.Keys, KeymapEntry>();

    public GR.Collections.Map<Types.KeyboardKey,KeymapEntry>            AllKeyInfos = new GR.Collections.Map<Types.KeyboardKey,KeymapEntry>();



    public BASICKeyMap()
    {
      InitDefault();
    }



    public void InitDefault()
    {
      // german
      AddKeymapEntry( Types.KeyboardKey.KEY_RUN_STOP, 7, System.Windows.Forms.Keys.Escape );

      AddKeymapEntry( Types.KeyboardKey.KEY_Q, 7, System.Windows.Forms.Keys.Q );
      AddKeymapEntry( Types.KeyboardKey.KEY_W, 7, System.Windows.Forms.Keys.W );
      AddKeymapEntry( Types.KeyboardKey.KEY_E, 7, System.Windows.Forms.Keys.E );
      AddKeymapEntry( Types.KeyboardKey.KEY_R, 7, System.Windows.Forms.Keys.R );
      AddKeymapEntry( Types.KeyboardKey.KEY_T, 7, System.Windows.Forms.Keys.T );
      AddKeymapEntry( Types.KeyboardKey.KEY_SPACE, 7, System.Windows.Forms.Keys.Space );

      AddKeymapEntry( Types.KeyboardKey.KEY_Y, 7, System.Windows.Forms.Keys.Z );
      AddKeymapEntry( Types.KeyboardKey.KEY_U, 7, System.Windows.Forms.Keys.U );
      AddKeymapEntry( Types.KeyboardKey.KEY_I, 7, System.Windows.Forms.Keys.I );
      AddKeymapEntry( Types.KeyboardKey.KEY_O, 7, System.Windows.Forms.Keys.O );
      AddKeymapEntry( Types.KeyboardKey.KEY_P, 7, System.Windows.Forms.Keys.P );

      // TODO - Layout! (DE<>EN, Dvorak?)
      AddKeymapEntry( Types.KeyboardKey.KEY_A, 7, System.Windows.Forms.Keys.A );
      AddKeymapEntry( Types.KeyboardKey.KEY_S, 7, System.Windows.Forms.Keys.S );
      AddKeymapEntry( Types.KeyboardKey.KEY_D, 7, System.Windows.Forms.Keys.D );
      AddKeymapEntry( Types.KeyboardKey.KEY_F, 7, System.Windows.Forms.Keys.F );
      AddKeymapEntry( Types.KeyboardKey.KEY_G, 7, System.Windows.Forms.Keys.G );
      AddKeymapEntry( Types.KeyboardKey.KEY_H, 7, System.Windows.Forms.Keys.H );
      AddKeymapEntry( Types.KeyboardKey.KEY_J, 7, System.Windows.Forms.Keys.J );
      AddKeymapEntry( Types.KeyboardKey.KEY_K, 7, System.Windows.Forms.Keys.K );
      AddKeymapEntry( Types.KeyboardKey.KEY_L, 7, System.Windows.Forms.Keys.L );
      AddKeymapEntry( Types.KeyboardKey.KEY_Z, 7, System.Windows.Forms.Keys.Y );
      AddKeymapEntry( Types.KeyboardKey.KEY_X, 7, System.Windows.Forms.Keys.X );
      AddKeymapEntry( Types.KeyboardKey.KEY_C, 7, System.Windows.Forms.Keys.C );
      AddKeymapEntry( Types.KeyboardKey.KEY_V, 7, System.Windows.Forms.Keys.V );
      AddKeymapEntry( Types.KeyboardKey.KEY_B, 7, System.Windows.Forms.Keys.B );
      AddKeymapEntry( Types.KeyboardKey.KEY_N, 7, System.Windows.Forms.Keys.N );
      AddKeymapEntry( Types.KeyboardKey.KEY_M, 7, System.Windows.Forms.Keys.M );
      AddKeymapEntry( Types.KeyboardKey.KEY_AT, 7, System.Windows.Forms.Keys.Oem1 );
      AddKeymapEntry( Types.KeyboardKey.KEY_COLON, 7, System.Windows.Forms.Keys.Oemtilde );
      AddKeymapEntry( Types.KeyboardKey.KEY_SEMI_COLON, 7, System.Windows.Forms.Keys.Oem7 );
      AddKeymapEntry( Types.KeyboardKey.KEY_ARROW_LEFT, 7, System.Windows.Forms.Keys.Oem5 );
      AddKeymapEntry( Types.KeyboardKey.KEY_STAR, 7, System.Windows.Forms.Keys.Oemplus );
      AddKeymapEntry( Types.KeyboardKey.KEY_MINUS, 7, System.Windows.Forms.Keys.Oem6 );
      AddKeymapEntry( Types.KeyboardKey.KEY_POUND, 7, System.Windows.Forms.Keys.Insert );
      AddKeymapEntry( Types.KeyboardKey.KEY_ARROW_UP, 7, System.Windows.Forms.Keys.Delete );
      AddKeymapEntry( Types.KeyboardKey.KEY_EQUAL, 7, System.Windows.Forms.Keys.Oem2 );
      AddKeymapEntry( Types.KeyboardKey.KEY_SLASH, 7, System.Windows.Forms.Keys.OemMinus );
      AddKeymapEntry( Types.KeyboardKey.KEY_COMMA, 7, System.Windows.Forms.Keys.Oemcomma );
      AddKeymapEntry( Types.KeyboardKey.KEY_DOT, 7, System.Windows.Forms.Keys.OemPeriod );
      AddKeymapEntry( Types.KeyboardKey.KEY_1, 7, System.Windows.Forms.Keys.D1 );
      AddKeymapEntry( Types.KeyboardKey.KEY_2, 7, System.Windows.Forms.Keys.D2 );
      AddKeymapEntry( Types.KeyboardKey.KEY_3, 7, System.Windows.Forms.Keys.D3 );
      AddKeymapEntry( Types.KeyboardKey.KEY_4, 7, System.Windows.Forms.Keys.D4 );
      AddKeymapEntry( Types.KeyboardKey.KEY_5, 7, System.Windows.Forms.Keys.D5 );
      AddKeymapEntry( Types.KeyboardKey.KEY_6, 7, System.Windows.Forms.Keys.D6 );
      AddKeymapEntry( Types.KeyboardKey.KEY_7, 7, System.Windows.Forms.Keys.D7 );
      AddKeymapEntry( Types.KeyboardKey.KEY_8, 7, System.Windows.Forms.Keys.D8 );
      AddKeymapEntry( Types.KeyboardKey.KEY_9, 7, System.Windows.Forms.Keys.D9 );
      AddKeymapEntry( Types.KeyboardKey.KEY_0, 7, System.Windows.Forms.Keys.D0 );

      AddKeymapEntry( Types.KeyboardKey.KEY_PLUS, 7, System.Windows.Forms.Keys.OemOpenBrackets );

      AddKeymapEntry( Types.KeyboardKey.KEY_CLR_HOME, 7, System.Windows.Forms.Keys.Home );
      AddKeymapEntry( Types.KeyboardKey.KEY_INST_DEL, 7, System.Windows.Forms.Keys.Back );
      AddKeymapEntry( Types.KeyboardKey.KEY_RETURN, 7, System.Windows.Forms.Keys.Return );
      AddKeymapEntry( Types.KeyboardKey.KEY_F1, 7, System.Windows.Forms.Keys.F1 );
      AddKeymapEntry( Types.KeyboardKey.KEY_F3, 7, System.Windows.Forms.Keys.F3 );
      AddKeymapEntry( Types.KeyboardKey.KEY_F5, 7, System.Windows.Forms.Keys.F5 );
      AddKeymapEntry( Types.KeyboardKey.KEY_F7, 7, System.Windows.Forms.Keys.F7 );

      // cursor keys
      AddKeymapEntry( Types.KeyboardKey.KEY_CURSOR_UP_DOWN, 7, System.Windows.Forms.Keys.Down );
      AddKeymapEntry( Types.KeyboardKey.KEY_CURSOR_LEFT_RIGHT, 7, System.Windows.Forms.Keys.Right );

      //AddKeymapEntry( Types.KeyboardKey.KEY_CURSOR_UP_DOWN, 7, System.Windows.Forms.Keys.Up, (char)0xeed1, 145, -1, 0, -1, 0 );
      //AddKeymapEntry( Types.KeyboardKey.KEY_CURSOR_LEFT_RIGHT, 7, System.Windows.Forms.Keys.Left, (char)0xeedd, 157, -1, 0, -1, 0 );

      // english
      AddKeymapEntry( Types.KeyboardKey.KEY_RUN_STOP, 9, System.Windows.Forms.Keys.Escape );

      AddKeymapEntry( Types.KeyboardKey.KEY_SPACE, 9, System.Windows.Forms.Keys.Space );

      AddKeymapEntry( Types.KeyboardKey.KEY_Q, 9, System.Windows.Forms.Keys.Q );
      AddKeymapEntry( Types.KeyboardKey.KEY_W, 9, System.Windows.Forms.Keys.W );
      AddKeymapEntry( Types.KeyboardKey.KEY_E, 9, System.Windows.Forms.Keys.E );
      AddKeymapEntry( Types.KeyboardKey.KEY_R, 9, System.Windows.Forms.Keys.R );
      AddKeymapEntry( Types.KeyboardKey.KEY_T, 9, System.Windows.Forms.Keys.T );
                                                                              
                                                                              
      AddKeymapEntry( Types.KeyboardKey.KEY_Y, 9, System.Windows.Forms.Keys.Y );
      AddKeymapEntry( Types.KeyboardKey.KEY_U, 9, System.Windows.Forms.Keys.U );
      AddKeymapEntry( Types.KeyboardKey.KEY_I, 9, System.Windows.Forms.Keys.I );
      AddKeymapEntry( Types.KeyboardKey.KEY_O, 9, System.Windows.Forms.Keys.O );
      AddKeymapEntry( Types.KeyboardKey.KEY_P, 9, System.Windows.Forms.Keys.P );
                                                                              
      // TODO - Layout! (DE<>EN, Dvorak?)                                     
      AddKeymapEntry( Types.KeyboardKey.KEY_A, 9, System.Windows.Forms.Keys.A );
      AddKeymapEntry( Types.KeyboardKey.KEY_S, 9, System.Windows.Forms.Keys.S );
      AddKeymapEntry( Types.KeyboardKey.KEY_D, 9, System.Windows.Forms.Keys.D );
      AddKeymapEntry( Types.KeyboardKey.KEY_F, 9, System.Windows.Forms.Keys.F );
      AddKeymapEntry( Types.KeyboardKey.KEY_G, 9, System.Windows.Forms.Keys.G );
      AddKeymapEntry( Types.KeyboardKey.KEY_H, 9, System.Windows.Forms.Keys.H );
      AddKeymapEntry( Types.KeyboardKey.KEY_J, 9, System.Windows.Forms.Keys.J );
      AddKeymapEntry( Types.KeyboardKey.KEY_K, 9, System.Windows.Forms.Keys.K );
      AddKeymapEntry( Types.KeyboardKey.KEY_L, 9, System.Windows.Forms.Keys.L );
      AddKeymapEntry( Types.KeyboardKey.KEY_Z, 9, System.Windows.Forms.Keys.Z );
      AddKeymapEntry( Types.KeyboardKey.KEY_X, 9, System.Windows.Forms.Keys.X );
      AddKeymapEntry( Types.KeyboardKey.KEY_C, 9, System.Windows.Forms.Keys.C );
      AddKeymapEntry( Types.KeyboardKey.KEY_V, 9, System.Windows.Forms.Keys.V );
      AddKeymapEntry( Types.KeyboardKey.KEY_B, 9, System.Windows.Forms.Keys.B );
      AddKeymapEntry( Types.KeyboardKey.KEY_N, 9, System.Windows.Forms.Keys.N );
      AddKeymapEntry( Types.KeyboardKey.KEY_M, 9, System.Windows.Forms.Keys.M );
      AddKeymapEntry( Types.KeyboardKey.KEY_AT, 9, System.Windows.Forms.Keys.Oem1 );
      AddKeymapEntry( Types.KeyboardKey.KEY_COLON, 9, System.Windows.Forms.Keys.Oemtilde );
      AddKeymapEntry( Types.KeyboardKey.KEY_SEMI_COLON, 9, System.Windows.Forms.Keys.Oem7 );
      AddKeymapEntry( Types.KeyboardKey.KEY_ARROW_LEFT, 9, System.Windows.Forms.Keys.Oem5 );
      AddKeymapEntry( Types.KeyboardKey.KEY_STAR, 9, System.Windows.Forms.Keys.Oemplus );
      AddKeymapEntry( Types.KeyboardKey.KEY_MINUS, 9, System.Windows.Forms.Keys.Oem6 );
      AddKeymapEntry( Types.KeyboardKey.KEY_POUND, 9, System.Windows.Forms.Keys.Insert );
      AddKeymapEntry( Types.KeyboardKey.KEY_ARROW_UP, 9, System.Windows.Forms.Keys.Delete );
      AddKeymapEntry( Types.KeyboardKey.KEY_EQUAL, 9, System.Windows.Forms.Keys.Oem2 );
      AddKeymapEntry( Types.KeyboardKey.KEY_SLASH, 9, System.Windows.Forms.Keys.OemMinus );
      AddKeymapEntry( Types.KeyboardKey.KEY_COMMA, 9, System.Windows.Forms.Keys.Oemcomma );
      AddKeymapEntry( Types.KeyboardKey.KEY_DOT, 9, System.Windows.Forms.Keys.OemPeriod );

      AddKeymapEntry( Types.KeyboardKey.KEY_1, 9, System.Windows.Forms.Keys.D1 );
      AddKeymapEntry( Types.KeyboardKey.KEY_2, 9, System.Windows.Forms.Keys.D2 );
      AddKeymapEntry( Types.KeyboardKey.KEY_3, 9, System.Windows.Forms.Keys.D3 );
      AddKeymapEntry( Types.KeyboardKey.KEY_4, 9, System.Windows.Forms.Keys.D4 );
      AddKeymapEntry( Types.KeyboardKey.KEY_5, 9, System.Windows.Forms.Keys.D5 );
      AddKeymapEntry( Types.KeyboardKey.KEY_6, 9, System.Windows.Forms.Keys.D6 );
      AddKeymapEntry( Types.KeyboardKey.KEY_7, 9, System.Windows.Forms.Keys.D7 );
      AddKeymapEntry( Types.KeyboardKey.KEY_8, 9, System.Windows.Forms.Keys.D8 );
      AddKeymapEntry( Types.KeyboardKey.KEY_9, 9, System.Windows.Forms.Keys.D9 );
      AddKeymapEntry( Types.KeyboardKey.KEY_0, 9, System.Windows.Forms.Keys.D0 );

      AddKeymapEntry( Types.KeyboardKey.KEY_PLUS, 9, System.Windows.Forms.Keys.OemOpenBrackets );

      AddKeymapEntry( Types.KeyboardKey.KEY_CLR_HOME, 9, System.Windows.Forms.Keys.Home );
      AddKeymapEntry( Types.KeyboardKey.KEY_INST_DEL, 9, System.Windows.Forms.Keys.Back );
      AddKeymapEntry( Types.KeyboardKey.KEY_RETURN, 9, System.Windows.Forms.Keys.Return );
      AddKeymapEntry( Types.KeyboardKey.KEY_F1, 9, System.Windows.Forms.Keys.F1 );
      AddKeymapEntry( Types.KeyboardKey.KEY_F3, 9, System.Windows.Forms.Keys.F3 );
      AddKeymapEntry( Types.KeyboardKey.KEY_F5, 9, System.Windows.Forms.Keys.F5 );
      AddKeymapEntry( Types.KeyboardKey.KEY_F7, 9, System.Windows.Forms.Keys.F7 );
    }



    private void AddKeymapEntry( Types.KeyboardKey KeyboardKey, uint NeutralLangID, System.Windows.Forms.Keys Key )
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
