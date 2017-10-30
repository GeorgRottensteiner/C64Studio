using System;
using System.Collections.Generic;
using System.Text;
using WeifenLuo.WinFormsUI.Docking;

namespace C64Studio
{
  public class Navigating
  {
    public StudioCore         Core = null;

    public GR.Collections.MultiMap<int, C64Studio.Parser.ParserBase.ParseMessage>   CompileMessages = null;
    public Project            Project = null;
    public Types.ASM.FileInfo ASMInfo = null;

    public int    LastShownMessageIndex = -1;

    public delegate void OpenDocumentAndGotoLineCallback( Project MarkProject, string DocumentFilename, int Line );



    public Navigating( StudioCore Core )
    {
      this.Core = Core;
    }



    public void UpdateFromMessages( GR.Collections.MultiMap<int, C64Studio.Parser.ParserBase.ParseMessage> Messages,
                                    Types.ASM.FileInfo ASMInfo,
                                    Project ParsedProject )
    {
      CompileMessages = Messages;
      Project         = ParsedProject;
      this.ASMInfo    = ASMInfo;

      LastShownMessageIndex = -1;
    }



    public Types.ASM.FileInfo DetermineASMFileInfo( DocumentInfo doc )
    {
      DocumentInfo possibleDoc = Core.MainForm.DetermineDocumentToCompile();

      if ( ( possibleDoc != null )
      &&   ( possibleDoc.Type == ProjectElement.ElementType.ASM_SOURCE ) )
      {
        return possibleDoc.ASMFileInfo;
      }
      if ( doc.Type == ProjectElement.ElementType.ASM_SOURCE )
      {
        return doc.ASMFileInfo;
      }
      return null;
    }



    public Types.ASM.FileInfo DetermineLocalASMFileInfo( DocumentInfo doc )
    {
      if ( doc.Type == ProjectElement.ElementType.ASM_SOURCE )
      {
        return doc.ASMFileInfo;
      }
      return null;
    }



    public void OpenDocumentAndGotoLine( Project MarkProject, string DocumentFilename, int Line )
    {
      if ( Core.MainForm.InvokeRequired )
      {
        Core.MainForm.Invoke( new OpenDocumentAndGotoLineCallback( OpenDocumentAndGotoLine ), new object[] { MarkProject, DocumentFilename, Line } );
        return;
      }
      string  inPath = DocumentFilename.Replace( "\\", "/" );
      foreach ( IDockContent dockContent in Core.MainForm.panelMain.Documents )
      {
        BaseDocument baseDoc = (BaseDocument)dockContent;
        if ( baseDoc.DocumentInfo.FullPath == null )
        {
          continue;
        }

        string    myPath = baseDoc.DocumentInfo.FullPath.Replace( "\\", "/" );
        if ( String.Compare( myPath, inPath, true ) == 0 )
        {
          baseDoc.Show();
          baseDoc.SetCursorToLine( Line, true );
          return;
        }
      }

      if ( MarkProject != null )
      {
        foreach ( ProjectElement element in MarkProject.Elements )
        {
          if ( GR.Path.IsPathEqual( GR.Path.Append( MarkProject.Settings.BasePath, element.Filename ), inPath ) )
          {
            BaseDocument doc = MarkProject.ShowDocument( element );
            if ( doc != null )
            {
              doc.SetCursorToLine( Line, true );
            }
            return;
          }
        }
      }
      if ( DocumentFilename.Length > 0 )
      {
        // file is not part of project
        BaseDocument newDoc = Core.MainForm.OpenFile( DocumentFilename );
        if ( newDoc != null )
        {
          newDoc.SetCursorToLine( Line, true );
        }
      }
    }



    internal void OpenSourceOfNextMessage()
    {
      if ( LastShownMessageIndex == -1 )
      {
        LastShownMessageIndex = 0;
      }
      else
      {
        ++LastShownMessageIndex;
      }
      if ( LastShownMessageIndex >= CompileMessages.Count )
      {
        LastShownMessageIndex = -1;
        return;
      }

      int     offset = LastShownMessageIndex;

      foreach ( var message in CompileMessages )
      {
        if ( offset == 0 )
        {
          int lineNumber = message.Key;

          string documentFile = "";
          int documentLine = -1;
          ASMInfo.FindTrueLineSource( lineNumber, out documentFile, out documentLine );

          if ( !string.IsNullOrEmpty( message.Value.AlternativeFile ) )
          {
            documentFile = message.Value.AlternativeFile;
            documentLine = message.Value.AlternativeLineIndex;
          }
          OpenDocumentAndGotoLine( Project, documentFile, documentLine );
          return;
        }
        --offset;
      }
    }



    public void GotoDeclaration( DocumentInfo ASMDoc, string Word, string Zone, string CheapLabelParent )
    {
      Types.ASM.FileInfo fileToDebug = DetermineASMFileInfo( ASMDoc );

      Types.SymbolInfo tokenInfo = fileToDebug.TokenInfoFromName( Word, Zone, CheapLabelParent );
      if ( tokenInfo == null )
      {
        fileToDebug = ASMDoc.ASMFileInfo;
        tokenInfo = ASMDoc.ASMFileInfo.TokenInfoFromName( Word, Zone, CheapLabelParent );
      }
      if ( tokenInfo != null )
      {
        string documentFile = "";
        int documentLine = -1;
        if ( ( tokenInfo.LineIndex == 0 )
        && ( !string.IsNullOrEmpty( tokenInfo.DocumentFilename ) ) )
        {
          // try stored info first
          OpenDocumentAndGotoLine( ASMDoc.Project, tokenInfo.DocumentFilename, tokenInfo.LocalLineIndex );
          return;
        }

        if ( fileToDebug.FindTrueLineSource( tokenInfo.LineIndex, out documentFile, out documentLine ) )
        {
          OpenDocumentAndGotoLine( ASMDoc.Project, documentFile, documentLine );
        }
      }
      else
      {
        System.Windows.Forms.MessageBox.Show( "Could not determine item source" );
      }
    }




  }
}
