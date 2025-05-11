using RetroDevStudio.Documents;
using RetroDevStudio.Parser.BASIC;
using RetroDevStudio.Tasks;
using System;
using System.Windows.Forms;



namespace RetroDevStudio.Dialogs
{
  public partial class FormRenumberBASIC : Form
  {
    SourceBasicEx m_Basic = null;
    StudioCore    m_Core = null;
    bool          m_SymbolMode = false;



    public FormRenumberBASIC( StudioCore Core, SourceBasicEx Basic, bool SymbolMode, int FirstLineNumber, int LastLineNumber )
    {
      m_Basic = Basic;
      m_Core  = Core;
      m_SymbolMode = SymbolMode;

      InitializeComponent();

      editStartLine.Text = "10";
      editLineStep.Text = "10";
      editFirstLineNumber.Text = FirstLineNumber.ToString();
      editLastLineNumber.Text = LastLineNumber.ToString();

      Core.Theming.ApplyTheme( this );
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
      int     firstLineNumber = GR.Convert.ToI32( editFirstLineNumber.Text );
      int     lastLineNumber  = GR.Convert.ToI32( editLastLineNumber.Text );

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
      if ( firstLineNumber > lastLineNumber )
      {
        labelRenumberInfo.Text = "First line number must be smaller than the last line number";
        btnOK.Enabled = false;
        return;
      }

      BasicFileParser.RenumberResult res = m_Core.Compiling.ParserBasic.CanRenumber( lineStart, lineStep, firstLineNumber, lastLineNumber, out string errorMessage );
      switch ( res )
      {
        case BasicFileParser.RenumberResult.OK:
          labelRenumberInfo.Text = "Press OK to renumber current listing";
          btnOK.Enabled = true;
          break;
        default:
          labelRenumberInfo.Text = errorMessage;
          btnOK.Enabled = false;
          break;
      }
    }



    private void btnOK_Click( DecentForms.ControlBase Sender )
    {
      int lineStart = GR.Convert.ToI32( editStartLine.Text );
      int lineStep = GR.Convert.ToI32( editLineStep.Text );
      int     firstLineNumber = GR.Convert.ToI32( editFirstLineNumber.Text );
      int     lastLineNumber  = GR.Convert.ToI32( editLastLineNumber.Text );

      string newText = m_Core.Compiling.ParserBasic.Renumber( lineStart, lineStep, firstLineNumber, lastLineNumber );
      if ( m_Basic.m_LowerCaseMode )
      {
        newText = BasicFileParser.MakeLowerCase( newText, !m_Core.Settings.BASICUseNonC64Font );
      }

      m_Basic.FillContent( newText, true, true );
      Close();
    }



    private void FormRenumberBASIC_Load( object sender, EventArgs e )
    {
      string configName = "";
      if ( m_Core.MainForm.mainToolConfig.SelectedItem != null )
      {
        configName = m_Core.MainForm.mainToolConfig.SelectedItem.ToString();
      }
      ProjectConfig config = null;
      if ( m_Basic.DocumentInfo.Project != null )
      {
        config = m_Basic.DocumentInfo.Project.Settings.Configuration( configName );
      }

      var taskCompile = new TaskCompile( m_Basic.DocumentInfo, m_Basic.DocumentInfo, m_Basic.DocumentInfo, m_Basic.DocumentInfo, m_Core.Navigating.Solution, false, false, false, false );
      taskCompile.Core = m_Core;
      taskCompile.RunTask();
      if ( !taskCompile.TaskSuccessful )
      {
        m_Core.Notification.MessageBox( "Cannot renumber", "Renumber is only possible on compilable code" );
        Close();
        return;
      }
      CheckRenumbering();
    }



    private void editLastLineNumber_TextChanged( object sender, EventArgs e )
    {
      CheckRenumbering();
    }



    private void editFirstLineNumber_TextChanged( object sender, EventArgs e )
    {
      CheckRenumbering();
    }



    private void btnCancel_Click( DecentForms.ControlBase Sender )
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }



  }
}
