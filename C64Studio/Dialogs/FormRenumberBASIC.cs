using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace C64Studio
{
  public partial class FormRenumberBASIC : Form
  {
    SourceBasicEx m_Basic = null;
    StudioCore    m_Core = null;



    public FormRenumberBASIC( StudioCore Core, SourceBasicEx Basic )
    {
      m_Basic = Basic;
      m_Core  = Core;

      InitializeComponent();

      editStartLine.Text = "10";
      editLineStep.Text = "10";
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
      int     lineStart = GR.Convert.ToI32( editStartLine.Text );
      int     lineStep = GR.Convert.ToI32( editLineStep.Text );

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
      Parser.BasicFileParser.RenumberResult res = m_Core.Compiling.ParserBasic.CanRenumber( lineStart, lineStep );
      switch ( res )
      {
        case C64Studio.Parser.BasicFileParser.RenumberResult.OK:
          labelRenumberInfo.Text = "Press OK to renumber current listing";
          btnOK.Enabled = true;
          break;
        case C64Studio.Parser.BasicFileParser.RenumberResult.TOO_MANY_LINES:
          labelRenumberInfo.Text = "Last line number is higher or equal 64000, reduce step value";
          btnOK.Enabled = false;
          break;
        case C64Studio.Parser.BasicFileParser.RenumberResult.NOTHING_TO_DO:
          labelRenumberInfo.Text = "Nothing to do";
          btnOK.Enabled = false;
          break;
      }
    }



    private void btnOK_Click( object sender, EventArgs e )
    {
      int lineStart = GR.Convert.ToI32( editStartLine.Text );
      int lineStep = GR.Convert.ToI32( editLineStep.Text );

      string newText = m_Core.Compiling.ParserBasic.Renumber( lineStart, lineStep );
      m_Basic.FillContent( newText );
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
        config = m_Basic.DocumentInfo.Project.Settings.Configs[configName];
      }
      if ( !m_Core.MainForm.ParseFile( m_Core.Compiling.ParserBasic, m_Basic.DocumentInfo, config, true ) )
      {
        System.Windows.Forms.MessageBox.Show( "Renumber is only possible on compilable code", "Cannot renumber" );
        Close();
        return;
      }
    }

  }
}
