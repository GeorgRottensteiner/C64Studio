using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace RetroDevStudio.Dialogs
{
  public partial class DlgDeactivatableMessage : Form
  {
    public enum UserChoice
    {
      OK,
      CANCEL,
      YES,
      NO
    }

    public enum MessageButtons
    {
      OK,
      OK_ALL,
      OK_CANCEL,
      YES_NO,
      YES_NO_ALL,
      YES_NO_CANCEL,
      YES_NO_CANCEL_ALL
    }




    public UserChoice ChosenResult
    {
      get;
      private set;
    }



    public bool StoreChoice
    {
      get;
      private set;
    }



    private MessageButtons  _buttons = MessageButtons.OK;
    private bool            _isClosing = false;



    public DlgDeactivatableMessage( MessageButtons buttons, string caption, string message, StudioCore core )
    {
      ChosenResult  = UserChoice.CANCEL;
      _buttons      = buttons;
      StoreChoice   = false;

      switch ( buttons )
      {
        case MessageButtons.OK:
        case MessageButtons.OK_ALL:
          InitializeComponent();
          btnYes.Visible              = false;
          btnNo.Visible               = true;
          btnNo.Text                  = "OK";
          btnNo.Tag                   = UserChoice.OK;
          btnCancel.Visible           = false;
          btnCancel.Tag               = UserChoice.CANCEL;
          ControlBox                  = false;
          checkRememberDecision.Text  = "Don't show this again";
          checkRememberDecision.Visible = ( buttons == MessageButtons.OK_ALL );
          break;
        case MessageButtons.OK_CANCEL:
          InitializeComponent();
          btnYes.Visible              = true;
          btnYes.Text                 = "OK";
          btnYes.Tag                  = UserChoice.OK;
          btnNo.Visible               = false;
          btnCancel.Visible           = true;
          btnCancel.Tag               = UserChoice.CANCEL;
          checkRememberDecision.Visible = false;
          CenterTwoButtons();
          break;
        case MessageButtons.YES_NO:
        case MessageButtons.YES_NO_ALL:
          InitializeComponent();
          btnYes.Visible    = true;
          btnYes.Tag        = UserChoice.YES;
          btnNo.Visible     = false;
          btnCancel.Text    = "No";
          btnCancel.Tag     = UserChoice.NO;
          ControlBox        = false;
          CenterTwoButtons();
          checkRememberDecision.Visible = ( buttons == MessageButtons.YES_NO_ALL );
          break;
        case MessageButtons.YES_NO_CANCEL:
        case MessageButtons.YES_NO_CANCEL_ALL:
          InitializeComponent();
          btnYes.Visible    = true;
          btnYes.Tag        = UserChoice.YES;
          btnNo.Visible     = true;
          btnNo.Tag         = UserChoice.NO;
          btnCancel.Visible = true;
          btnCancel.Tag     = UserChoice.CANCEL;
          checkRememberDecision.Visible = ( buttons == MessageButtons.YES_NO_CANCEL_ALL );
          break;
        default:
          throw new ArgumentOutOfRangeException( nameof( buttons ), buttons, null );
      }
      labelImageInfo.Text = message;
      Text                = caption;

      if ( ( !btnCancel.Visible )
      &&   ( buttons != MessageButtons.OK )
      &&   ( buttons != MessageButtons.OK_ALL ) )
      {
        CancelButton = null;
      }
      core.Theming.ApplyTheme( this );
    }



    private void CenterTwoButtons()
    {
      // only btnYes and btnCancel are visible
      int distance = btnNo.Left - btnYes.Right;

      int center  = ( Width - btnYes.Width - btnCancel.Width - distance ) / 2;
      btnYes.Left = center;
      btnCancel.Left  = center + btnYes.Width + distance;
    }



    private void btn_Click( DecentForms.ControlBase Sender )
    {
      ChosenResult = (UserChoice)Sender.Tag;
      DialogResult = DialogResult.OK;

      if ( checkRememberDecision.Visible )
      {
        StoreChoice = checkRememberDecision.Checked;
      }
      _isClosing = true;
      Close();
    }



    private void DlgDeactivatableMessage_FormClosing( object sender, FormClosingEventArgs e )
    {
      if ( _isClosing )
      {
        return;
      }
      switch ( _buttons )
      {
        case MessageButtons.OK:
        case MessageButtons.OK_ALL:
          ChosenResult = UserChoice.OK;
          DialogResult = DialogResult.OK;
          Close();
          break;
        case MessageButtons.YES_NO_CANCEL:
        case MessageButtons.YES_NO_CANCEL_ALL:
        case MessageButtons.OK_CANCEL:
          ChosenResult = UserChoice.CANCEL;
          DialogResult = DialogResult.Cancel;
          Close();
          break;
        default:
          e.Cancel = true;
          break;
      }
    }



  }
}
