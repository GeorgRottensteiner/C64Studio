using RetroDevStudio.Documents;
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
      ||   ( lineStart >= 64000 ))
      {
        labelRenumberInfo.Text = "Starting line number is invalid, must be greater or equal zero and less than 64000";
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

      Parser.BasicFileParser.RenumberResult res = m_Core.Compiling.ParserBasic.CanRenumber( lineStart, lineStep, firstLineNumber, lastLineNumber );
      switch ( res )
      {
        case RetroDevStudio.Parser.BasicFileParser.RenumberResult.OK:
          labelRenumberInfo.Text = "Press OK to renumber current listing";
          btnOK.Enabled = true;
          break;
        case RetroDevStudio.Parser.BasicFileParser.RenumberResult.TOO_MANY_LINES:
          labelRenumberInfo.Text = "Last line number is higher or equal 64000, reduce step value";
          btnOK.Enabled = false;
          break;
        case RetroDevStudio.Parser.BasicFileParser.RenumberResult.NOTHING_TO_DO:
          labelRenumberInfo.Text = "Nothing to do (Maybe parsing was not completed yet)";
          btnOK.Enabled = false;
          break;
      }
    }



    private void btnOK_Click( object sender, EventArgs e )
    {
      int lineStart = GR.Convert.ToI32( editStartLine.Text );
      int lineStep = GR.Convert.ToI32( editLineStep.Text );
      int     firstLineNumber = GR.Convert.ToI32( editFirstLineNumber.Text );
      int     lastLineNumber  = GR.Convert.ToI32( editLastLineNumber.Text );

      string newText = m_Core.Compiling.ParserBasic.Renumber( lineStart, lineStep, firstLineNumber, lastLineNumber );

      if ( m_SymbolMode )
      {
        bool hadError = false;
        newText = Parser.BasicFileParser.ReplaceAllMacrosBySymbols( newText, out hadError );
      }
      else
      {
        newText = m_Core.Compiling.ParserBasic.ReplaceAllSymbolsByMacros( newText );
      }

      if ( m_Basic.m_LowerCaseMode )
      {
        newText = Parser.BasicFileParser.MakeLowerCase( newText, !m_Core.Settings.BASICUseNonC64Font );
      }

      m_Basic.FillContent( newText, true );
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
      if ( !m_Core.MainForm.ParseFile( m_Core.Compiling.ParserBasic, m_Basic.DocumentInfo, config, null, true, false, false ) )
      {
        System.Windows.Forms.MessageBox.Show( "Renumber is only possible on compilable code", "Cannot renumber" );
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



  }
}
