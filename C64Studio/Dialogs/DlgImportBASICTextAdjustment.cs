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
    public bool   AdjustCasing { get;  private set; } = true;
    public bool   ReplaceInvalidChars { get; private set; } = false;
    public bool   SkipInvalidChars { get; private set; } = false;



    public DlgImportBASICTextAdjustment( bool UpperCaseActive, bool HasUpperCase, bool HasLowerCase, bool UppercaseInvalidChars, bool LowercaseInvalidChars, StudioCore Core )
    {
      InitializeComponent();

      checkReplaceInvalidCharacters.Enabled = false;
      checkSkipInvalidCharacters.Enabled    = false;
      checkAdjustCasing.Enabled             = true;

      string  fullInfo = "";

      if ( ( ( UpperCaseActive ) 
      &&     ( HasLowerCase ) ) 
      ||   ( ( !UpperCaseActive )
      &&     ( HasUpperCase ) ) )
      {
        checkAdjustCasing.Checked = true;

        fullInfo += "The pasted text has mismatching casing.\r\n";
      }

      if ( ( UppercaseInvalidChars )
      &&   ( LowercaseInvalidChars ) )
      {
        checkReplaceInvalidCharacters.Enabled = true;
        checkSkipInvalidCharacters.Enabled    = true;
        checkReplaceInvalidCharacters.Checked = true;

        fullInfo += "The pasted text has invalid characters.\r\n";
      }
      else if ( UppercaseInvalidChars )
      {
        fullInfo += "The pasted text has invalid characters.\r\n";
        if ( UpperCaseActive )
        {
          checkReplaceInvalidCharacters.Enabled = true;
          checkSkipInvalidCharacters.Enabled    = true;
        }
        if ( HasLowerCase )
        {
          checkAdjustCasing.Enabled = true;
        }
      }
      else if ( LowercaseInvalidChars )
      {
        fullInfo += "The pasted text has invalid characters.\r\n";
        if ( !UpperCaseActive )
        {
          checkReplaceInvalidCharacters.Enabled = true;
          checkSkipInvalidCharacters.Enabled    = true;
        }
        if ( HasUpperCase )
        {
          checkAdjustCasing.Enabled = true;
        }
      }
      else
      {
        checkReplaceInvalidCharacters.Enabled = false;
        checkSkipInvalidCharacters.Enabled    = false;
      }

      
      labelIssueInfo.Text = fullInfo + "\r\nChoose to auto-adjust casing and how to handle invalid characters";

      Core.Theming.ApplyTheme( this );
    }



    private void btnOK_Click( DecentForms.ControlBase Sender )
    {
      AdjustCasing        = checkAdjustCasing.Checked;
      ReplaceInvalidChars = checkReplaceInvalidCharacters.Checked;
      SkipInvalidChars    = checkSkipInvalidCharacters.Checked;

      DialogResult = DialogResult.OK;
      Close();
    }



    private void btnCancel_Click( DecentForms.ControlBase Sender )
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }



  }
}
