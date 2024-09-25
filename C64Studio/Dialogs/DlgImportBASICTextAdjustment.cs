using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace RetroDevStudio.Dialogs
{
  partial class DlgImportBASICTextAdjustment : Form
  {
    public bool AdjustCasing { get;  private set; } = true;
    public bool ReplaceInvalidChars { get; private set; } = false;
    public bool SkipInvalidChars { get; private set; } = false;

    private bool      _CaseAdjustmentSolvesIssues = false;
    private bool      _CaseAdjustmentSolvesIssues = false;




    public DlgImportBASICTextAdjustment( bool UpperCaseActive, bool HasUpperCase, bool HasLowerCase, bool UppercaseInvalidChars, bool LowercaseInvalidChars, StudioCore Core )
    {
      InitializeComponent();

      if ( ( UppercaseInvalidChars )
      &&   ( LowercaseInvalidChars ) )
      {
        _CaseAdjustmentSolvesIssues = false;
      }
      else if ( UppercaseInvalidChars )
      {
        if ( HasLowerCase )
        {

        }
      }
      else if ( LowercaseInvalidChars )
      {
      }

      checkAdjustCasing.Checked = ( ( UpperCaseActive )
                               &&   ( HasLowerCase ) )
                               || ( ( !UpperCaseActive )
                               &&   ( HasUpperCase ) );

      if ( ( ( UpperCaseActive )
      &&     ( UppercaseInvalidChars ) )
      ||   ( ( !UpperCaseActive )
      &&     ( LowercaseInvalidChars ) ) )
      {
        checkSkipInvalidCharacters.Checked    = true;
        checkReplaceInvalidCharacters.Checked = true;
      }
      if ( ( UppercaseInvalidChars
      checkSkipInvalidCharacters.Checked = UppercaseInvalidChars;

      labelIssueInfo.Text = "The imported image has the size " + ImageWidth + "x" + ImageHeight + ".\r\n"
                          + "The current screen has the size " + ScreenWidth + "x" + ScreenHeight + ".\r\n"
                          + "\r\nClip the image (if it is bigger than the screen), adjust the screen size to the image size or cancel the import?";

      Core.Theming.ApplyTheme( this );
    }



    private void btnClip_Click( DecentForms.ControlBase Sender )
    {
      ChosenResult = ImportBehaviour.CLIP_IMAGE;
      DialogResult = DialogResult.OK;
      Close();
    }



    private void btnAdjustScreenSize_Click( DecentForms.ControlBase Sender )
    {
      ChosenResult = ImportBehaviour.ADJUST_SCREEN_SIZE;
      DialogResult = DialogResult.OK;
      Close();
    }



    private void btnCancel_Click( DecentForms.ControlBase Sender )
    {
      ChosenResult = ImportBehaviour.CANCEL;
      DialogResult = DialogResult.Cancel;
      Close();
    }


  }
}
