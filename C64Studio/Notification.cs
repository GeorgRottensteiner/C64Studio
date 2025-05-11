using RetroDevStudio.Dialogs;
using RetroDevStudio.Documents;
using RetroDevStudio.Properties;
using System;
using System.Collections.Generic;



namespace RetroDevStudio
{
  public class Notification
  {
    public StudioCore         Core = null;



    public Notification( StudioCore Core )
    {
      this.Core = Core;
    }



    public void BuildSuccess( bool Force = false )
    {
      if ( ( !Force )
      &&   ( !Core.Settings.PlaySoundOnSuccessfulBuild ) )
      {
        return;
      }
      var player = new System.Media.SoundPlayer();
      player.Stream = Resources.ResourceManager.GetStream( "build_success" );
      player.Play();
      //System.Media.SystemSounds.Asterisk.Play();
    }



    public void BuildFailure( bool Force = false )
    {
      if ( ( !Force )
      &&   ( !Core.Settings.PlaySoundOnBuildFailure ) )
      {
        return;
      }
      var player = new System.Media.SoundPlayer();
      player.Stream = Resources.ResourceManager.GetStream( "build_failure" );
      player.Play();
      //System.Media.SystemSounds.Exclamation.Play();
    }



    public void ItemNotFound( bool Force = false )
    {
      if ( ( !Force )
      &&   ( !Core.Settings.PlaySoundOnSearchFoundNoItem ) )
      {
        return;
      }
      var player = new System.Media.SoundPlayer();
      player.Stream = Resources.ResourceManager.GetStream( "item_notfound" );
      player.Play();
      //System.Media.SystemSounds.Question.Play();
    }



    public DlgDeactivatableMessage.UserChoice UserDecision( DlgDeactivatableMessage.MessageButtons buttons,
                                                            string caption,
                                                            string message )
    {
      var dlg = new DlgDeactivatableMessage( buttons, caption, message, Core );
      dlg.ShowDialog( Core.MainForm );

      return dlg.ChosenResult;
    }



    public DlgDeactivatableMessage.UserChoice UserDecision( DlgDeactivatableMessage.MessageButtons buttons, 
                                                            string caption, 
                                                            string message, 
                                                            string decisionKey )
    {
      if ( Core.Settings.StoredDialogResults.TryGetValue( decisionKey, out DlgDeactivatableMessage.UserChoice storedDecision ) )
      {
        return storedDecision;
      }
      var dlg = new DlgDeactivatableMessage( buttons, caption, message, Core );
      dlg.ShowDialog( Core.MainForm );

      if ( dlg.StoreChoice )
      {
        Core.Settings.StoredDialogResults.Add( decisionKey, dlg.ChosenResult );
      }
      return dlg.ChosenResult;
    }



    public void MessageBox( string caption, string message )
    {
      var dlg = new DlgDeactivatableMessage( DlgDeactivatableMessage.MessageButtons.OK, caption, message, Core );
      dlg.ShowDialog( Core.MainForm );
    }



  }
}
