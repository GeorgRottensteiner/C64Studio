using GR;
using GR.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RetroDevStudio.Audio
{
  internal class AudioHandler : IDisposable
  {
    private SFXPlayerDescriptor _currentSFXPlayer = null; 

    private SIDPlayer           _sidPlayer = null;



    public void Dispose()
    {
      StopAll();
    }



    public void StopAll()
    {
      _sidPlayer?.Stop();
      _sidPlayer?.Dispose();
      _sidPlayer = null;

      _currentSFXPlayer = null;
    }



    internal void SetSFXPlayer( SFXPlayerDescriptor player )
    {
      _currentSFXPlayer = player;

      var playerClass = EnumHelper.GetAttributeOfType<TypeAttribute>( player.Player );

      // TODO
      if ( _sidPlayer == null )
      {
        _sidPlayer = new SIDPlayer();
      }
      //ObjectType instance = (ObjectType)Activator.CreateInstance(objectType);
    }



    internal void Play( ByteBuffer playerData )
    {
      if ( _sidPlayer == null )
      {
        _sidPlayer = new SIDPlayer();
      }
      _sidPlayer.Play( playerData.Data(), _currentSFXPlayer.PlayerCodeAddress, _currentSFXPlayer.AddressToStartPlayer );
    }



  }
}
