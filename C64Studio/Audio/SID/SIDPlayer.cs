using GR.Collections;
using GR.Memory;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Text;

namespace RetroDevStudio.Audio.SID
{
  internal class SIDPlayer : IDisposable
  {
    private SharpSid.Player     _player = null; 



    public SIDPlayer()
    {
      _player = new SharpSid.Player();
    }



    public bool Play( byte[] sidData, int dataStartAddress, int songStartAddress )
    {
      try
      {
        // ; Reset BASIC pointers
        // JSR $A659; CLR — Clear variables( BASIC routine )
        // 
        // ; Set BASIC execution pointer to start of program
        // LDA $2B; Low byte of start - of - BASIC pointer
        // STA $7A; TXTPTR low
        // LDA $2C; High byte
        // STA $7B; TXTPTR high
        // 
        // ; Jump to BASIC main loop( warm start )
        // JMP $A7AE; BASIC warm start( same as pressing RUN / STOP + RESTORE )

        // 0D080A0053B23534323732001C0814008158B23135A430A9AB31004F081E009753AA32342C583A9753AA352C31353A9753AA362C303A9753AA312C34303A97532C3230303A9753AA342C31323900550828008200670832009753AA342C303A9753AA352C30000000

        var basic = new ByteBuffer( "0D080A0053B23534323732001C0814008158B23135A430A9AB31004F081E009753AA32342C583A9753AA352C31353A9753AA362C303A9753AA312C34303A97532C3230303A9753AA342C31323900550828008200670832009753AA342C303A9753AA352C30000000" );
        sidData = basic.Data();

        dataStartAddress = 0x0801;
        songStartAddress = 0xa7ae;

        _player.Stop();
        if ( sidData == null )
        {
          return false;
        }
        var hex = ByteArrayToHexString( sidData );
        return _player.PlayFromBinary( hex, dataStartAddress, songStartAddress );
      }
      catch ( Exception ex )
      {
        System.Diagnostics.Debug.WriteLine( "Exception during SID playback: " + ex.Message );
        return false;
      }
    }



    public void Stop()
    {
      _player.Stop();
    }



    public bool IsPlaying()
    {
      return _player.State == SharpSid.State.PLAYING;
    }



    private static string ByteArrayToHexString( byte[] data )
    {
      if ( data == null || data.Length == 0 )
      {
        return string.Empty;
      }
      var sb = new StringBuilder( data.Length * 2 );
      for ( int i = 0; i < data.Length; ++i )
      {
        sb.Append( data[i].ToString( "X2" ) );
      }
      return sb.ToString();
    }



    public void Dispose()
    {
      if ( _player != null )
      {
        _player.Stop();
        _player.Dispose();
        _player = null;
      }
    }



  }
}
