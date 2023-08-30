using RetroDevStudio.Documents;
using RetroDevStudio.Tasks;
using System;
using System.Windows.Forms;



namespace RetroDevStudio.Dialogs
{
  public partial class FormRenumberBASICLabelMode : Form
  {
    SourceBasicEx m_Basic = null;
    StudioCore    m_Core = null;



    public FormRenumberBASICLabelMode( StudioCore Core, SourceBasicEx Basic )
    {
      m_Basic = Basic;
      m_Core  = Core;

      InitializeComponent();

      editStartLine.Text  = Basic.m_LastLabelAutoRenumberStartLine;
      editLineStep.Text   = Basic.m_LastLabelAutoRenumberLineStep;

      Core.Theming.ApplyTheme( this );

      CheckRenumbering();
    }



    private void editStartLine_TextChanged( object sender, EventArgs e )
    {
      CheckRenumbering();
    }



    private void editLineStep_TextChanged( object sender, EventArgs e )
    {
      CheckRenumbering();      
    }



    private void CheckRenumbering()
    {
      int     lineStart       = GR.Convert.ToI32( editStartLine.Text );
      int     lineStep        = GR.Convert.ToI32( editLineStep.Text );

      if ( ( lineStart < 0 )
      ||   ( lineStart > m_Basic.BASICDialect.MaxLineNumber ) )
      {
        labelRenumberInfo.Text = $"Starting line number is invalid, must be greater or equal zero and less or equal than {m_Basic.BASICDialect.MaxLineNumber}";
        btnOK.Enabled = false;
        return;
      }
      if ( lineStep <= 0 )
      {
        labelRenumberInfo.Text = "Line Step must be greater than zero";
        btnOK.Enabled = false;
        return;
      }

      labelRenumberInfo.Text = "Renumber values are good";
      btnOK.Enabled = true;
    }



    private void btnOK_Click( object sender, EventArgs e )
    {
      int lineStart = GR.Convert.ToI32( editStartLine.Text );
      int lineStep = GR.Convert.ToI32( editLineStep.Text );

      m_Basic.m_LastLabelAutoRenumberStartLine  = lineStart.ToString();
      m_Basic.m_LastLabelAutoRenumberLineStep   = lineStep.ToString();
      DialogResult = DialogResult.OK;

      Close();
    }



    private void editLastLineNumber_TextChanged( object sender, EventArgs e )
    {
      CheckRenumbering();
    }



    private void editFirstLineNumber_TextChanged( object sender, EventArgs e )
    {
      CheckRenumbering();
    }



  }
}
