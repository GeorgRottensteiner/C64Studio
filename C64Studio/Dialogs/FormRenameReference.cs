using RetroDevStudio.Types;
using RetroDevStudio;
using System;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using RetroDevStudio.Documents;
using static RetroDevStudio.Dialogs.FormFindReplace;
using RetroDevStudio.Parser;



namespace RetroDevStudio.Dialogs
{
  public partial class FormRenameReference : Form
  {
    private string                  PreviousSearchedFile = "";
    private string                  PreviousSearchedFileContent = null;
    private DateTime                PreviousSearchedFileTimeStamp;

    StudioCore                      Core;
    SymbolInfo                      _Symbol;
    Types.ASM.FileInfo              _ASMInfo;
    ASMFileParser                   _Parser;
    GR.Collections.Set<int>         _AllReferences = null;



    public FormRenameReference( StudioCore Core, SymbolInfo Symbol, GR.Collections.Set<int> AllReferences, Types.ASM.FileInfo ASMInfo, ASMFileParser Parser )
    {
      this.Core       = Core;
      _Symbol         = Symbol;
      _ASMInfo        = ASMInfo;
      _Parser         = Parser;
      _AllReferences  = AllReferences;

      InitializeComponent();

      editReferenceName.Text = _Symbol.Name;

      Core.Theming.ApplyTheme( this );

      ValidateNewName();
      editReferenceName.Select();
      editReferenceName.SelectAll();
    }



    private void btnOK_Click( object sender, EventArgs e )
    {
      string newName = editReferenceName.Text;

      if ( newName == _Symbol.Name )
      {
        DialogResult = DialogResult.OK;
        Close();
        return;
      }

      RenameReference();

      DialogResult = DialogResult.OK;
      Close();
    }



    private void editSolutionName_TextChanged( object sender, EventArgs e )
    {
      ValidateNewName();
    }



    private bool ValidateNewName()
    {
      string    newName     = editReferenceName.Text;

      if ( newName == _Symbol.Name )
      {
        labelRenameInfo.Text = $"The new name matches the old.";
        btnOK.Enabled = false;
        return false;
      }
      if ( string.IsNullOrEmpty( newName ) ) 
      {
        labelRenameInfo.Text = $"The new name must be at least one character.";
        btnOK.Enabled = false;
        return false;
      }

      var validChars = _ASMInfo.AssemblerSettings.AllowedTokenChars[TokenInfo.TokenType.LABEL_GLOBAL];
      for ( int i = 0; i < newName.Length; ++i )
      {
        if ( !validChars.Contains( newName[i] ) )
        {
          labelRenameInfo.Text = $"The new name contains invalid character '{newName[i]}'";
          btnOK.Enabled = false;
          return false;
        }
      }

      labelRenameInfo.Text = $"The new name is valid.";
      btnOK.Enabled = true;
      return true;
    }



    internal string GetTextFromFile( string Filename )
    {
      // can we use cached text?
      bool    cacheIsUpToDate = false;

      DateTime    lastAccessTimeStamp;

      try
      {
        lastAccessTimeStamp = System.IO.File.GetLastWriteTime( Filename );

        cacheIsUpToDate = ( lastAccessTimeStamp <= PreviousSearchedFileTimeStamp );

        PreviousSearchedFileTimeStamp = lastAccessTimeStamp;
      }
      catch ( Exception )
      {
      }

      if ( ( GR.Path.IsPathEqual( PreviousSearchedFile, Filename ) )
      &&   ( cacheIsUpToDate )
      &&   ( PreviousSearchedFileContent != null ) )
      {
        return PreviousSearchedFileContent;
      }

      PreviousSearchedFileContent = GR.IO.File.ReadAllText( Filename );
      PreviousSearchedFile = Filename;
      return PreviousSearchedFileContent;
    }



    private void RenameReference()
    {
      string    newName = editReferenceName.Text;

      GR.Collections.Set<int>   usedReferences = new GR.Collections.Set<int>( _AllReferences );

      if ( !_Symbol.References.ContainsValue( _Symbol.LineIndex ) )
      {
        usedReferences.Add( _Symbol.LineIndex );
      }

      foreach ( var refPos in usedReferences )
      {
        if ( _ASMInfo.FindTrueLineSource( refPos, out string fileName, out int localLineIndex ) )
        {
          if ( _ASMInfo.IsDocumentPart( fileName ) )
          {
            var doc = Core.Navigating.FindDocumentByPath( fileName );
            if ( doc != null )
            {
              if ( doc.DocumentInfo.Type == ProjectElement.ElementType.ASM_SOURCE )
              {
                var asm = (SourceASMEx)doc;
                string textFromElement = Core.Searching.GetDocumentInfoText( doc.DocumentInfo );

                string replacedText = ReplaceReferenceInLine( textFromElement, localLineIndex, _Symbol.Name, newName, out int posOfLineStart, out int posOfLineEnd );

                asm.SetLineText( replacedText, localLineIndex );
              }
            }
            else
            {
              string textFromElement = GetTextFromFile( fileName );

              string replacedText = ReplaceReferenceInLine( textFromElement, localLineIndex, _Symbol.Name, newName, out int posOfLineStart, out int posOfLineEnd );

              textFromElement = textFromElement.Substring( 0, posOfLineStart ) + replacedText + textFromElement.Substring( posOfLineEnd );

              GR.IO.File.WriteAllText( fileName, textFromElement );
            }
          }
          else
          {
            string textFromElement = GetTextFromFile( fileName );

            string replacedText = ReplaceReferenceInLine( textFromElement, localLineIndex, _Symbol.Name, newName, out int posOfLineStart, out int posOfLineEnd );

            textFromElement = textFromElement.Substring( 0, posOfLineStart ) + replacedText + textFromElement.Substring( posOfLineEnd );

            GR.IO.File.WriteAllText( fileName, textFromElement );
          }
        }
      }

      Core.SetStatus( $"Replaced {usedReferences.Count} references." );
    }



    private bool FindLineRange( string Text, int LineIndex, out int LineStartPos, out int LineEndPos )
    {
      int curLine = 0;
      int lastLineStartPos = 0;
      int lastLineEndPos = -1;

      LineStartPos = -1;
      LineEndPos = -1;

      do
      {
        ++curLine;

        lastLineStartPos = lastLineEndPos + 1;
        lastLineEndPos = Text.IndexOf( '\n', lastLineEndPos + 1 );

        //Debug.Log( $"Line {curLine - 1} = {Text.Substring( lastPos, curPos - lastPos + 1 )}" );
      }
      while ( ( curLine <= LineIndex )
      &&      ( lastLineStartPos < Text.Length ) );

      if ( ( curLine < LineIndex )
      &&   ( lastLineStartPos == -1 ) )
      {
        // not found??
        return false;
      }

      LineStartPos  = lastLineStartPos;
      LineEndPos    = lastLineEndPos;
      if ( LineEndPos == -1 )
      {
        LineEndPos = Text.Length;
      }
      else
      {
        ++LineEndPos;
      }

      return true;
    }



    private string ReplaceReferenceInLine( string TextFromElement, int LineIndex, string Name, string NewName, out int PosOfLineStart, out int PosOfLineEnd )
    {
      if ( !FindLineRange( TextFromElement, LineIndex, out PosOfLineStart, out PosOfLineEnd ) )
      {
        return "";
      }

      string  lineText = TextFromElement.Substring( PosOfLineStart, PosOfLineEnd - PosOfLineStart ).TrimEnd();

      // sub trimmed chars
      PosOfLineEnd -= PosOfLineEnd - PosOfLineStart - lineText.Length;

      reparse:
      var tokens = _Parser.ParseTokenInfo( lineText, 0, lineText.Length, new GR.Collections.Map<byte, byte>() );
      foreach ( var token in tokens )
      {
        if ( ( token.Type == TokenInfo.TokenType.LABEL_LOCAL )
        ||   ( token.Type == TokenInfo.TokenType.LABEL_GLOBAL ) )
        {
          if ( token.Content == Name )
          {
            lineText = lineText.Substring( 0, token.StartPos ) + NewName + lineText.Substring( token.EndPos + 1 );
            goto reparse;
          }
        }
      }
      return lineText;
    }



  }
}
