using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RetroDevStudio.Audio
{
  internal class AudioHandler : IDisposable
  {
    private SID.SIDPlayer     _sidPlayer = null;



    public void Dispose()
    {
      StopAll();
    }



    public void StopAll()
    {
      _sidPlayer?.Stop();
      _sidPlayer?.Dispose();
      _sidPlayer = null;
    }



    public void SID( byte[] sidData, int dataStartAddress, int songStartAddress )
    {
      StopAll();
      if ( _sidPlayer == null )
      {
        _sidPlayer = new SID.SIDPlayer();
        _sidPlayer.Play( sidData, dataStartAddress, songStartAddress );
      }
    }



  }
}
