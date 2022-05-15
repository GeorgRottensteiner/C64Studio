using System;
using System.Collections.Generic;
using System.Text;

namespace RetroDevStudio
{
  public class OutputDisplay : ReadOnlyFile
  {
    public OutputDisplay() : base( null )
    {
      InitializeComponent();

      GR.Image.DPIHandler.ResizeControlsForDPI( this );

      editText.Language = FastColoredTextBoxNS.Language.Custom;
    }



    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( OutputDisplay ) );
      ( (System.ComponentModel.ISupportInitialize)( this.editText ) ).BeginInit();
      ( (System.ComponentModel.ISupportInitialize)( this.m_FileWatcher ) ).BeginInit();
      this.SuspendLayout();
      // 
      // editText
      // 
      this.editText.AutoScrollMinSize = new System.Drawing.Size( 2, 13 );
      this.editText.CharHeight = 13;
      this.editText.CharWidth = 7;
      this.editText.Font = new System.Drawing.Font( "Courier New", 9F );
      this.editText.ShowLineNumbers = false;
      // 
      // OutputDisplay
      // 
      this.ClientSize = new System.Drawing.Size( 534, 390 );
      this.Icon = ( (System.Drawing.Icon)( resources.GetObject( "$this.Icon" ) ) );
      this.Name = "OutputDisplay";
      this.Text = "Output";
      this.Controls.SetChildIndex( this.editText, 0 );
      ( (System.ComponentModel.ISupportInitialize)( this.editText ) ).EndInit();
      ( (System.ComponentModel.ISupportInitialize)( this.m_FileWatcher ) ).EndInit();
      this.ResumeLayout( false );

    }



    private void editText_MouseDoubleClick( object sender, System.Windows.Forms.MouseEventArgs e )
    {
      int position  = editText.PointToPosition( e.Location );
      int lineBelow = editText.PositionToPlace( position ).iLine;

      //dh.Log( "Clicked at: " + editText.Lines[lineBelow].Text );

      // TODO - Totally depends on the tool called!

      string    clickedLine = editText.Lines[lineBelow];

      // Search result: devilronin.asm(27):SPRITE_CASTLE_ENEMY_SWORDSMAN             = SPRITE_BASE + 40
      System.Text.RegularExpressions.Regex    searchPattern = new System.Text.RegularExpressions.Regex( "^(.+)\\((\\d+)" );
      System.Text.RegularExpressions.Match    matchSearch = searchPattern.Match( clickedLine );
      if ( matchSearch.Groups.Count == 3 )
      {
        string    fileName = matchSearch.Groups[1].Value;
        int       lineNumber = GR.Convert.ToI32( matchSearch.Groups[2].Value );

        if ( ( Core.MainForm.CurrentProject != null )
        &&   ( !System.IO.Path.IsPathRooted( fileName ) ) )
        {
          fileName = GR.Path.Normalize( GR.Path.Append( Core.MainForm.CurrentProject.Settings.BasePath, fileName ), false );
        }
        Core.Navigating.OpenDocumentAndGotoLine( Core.MainForm.CurrentProject, Core.Navigating.FindDocumentInfoByPath( fileName ), lineNumber - 1 );
        return;
      }
      


      // Error - File testfile.asm, line 1 (Zone <untitled>): Program counter is unset.
      if ( clickedLine.StartsWith( "Error - " ) )
      {
        int     filePos = clickedLine.IndexOf( "File" );
        if ( filePos != -1 )
        {
          int linePos = clickedLine.IndexOf( ", line", filePos );
          if ( linePos != -1 )
          {
            int lineEndPos = clickedLine.IndexOf( " (", linePos );
            if ( lineEndPos != -1 )
            {
              string fileName = clickedLine.Substring( filePos + 4, linePos - filePos - 4 ).Trim();
              int     lineNumber = 0;

              if ( int.TryParse( clickedLine.Substring( linePos + 6, lineEndPos - linePos - 6 ), out lineNumber ) )
              {
                Core.Navigating.OpenDocumentAndGotoLine( Core.MainForm.CurrentProject, Core.Navigating.FindDocumentInfoByPath( fileName ), lineNumber - 1 );
              }
            }
          }
        }
      }
    }
  }
}
