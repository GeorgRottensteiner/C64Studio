using RetroDevStudio;
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
    public Solution           Solution = null;
    public Project            Project = null;
    public Types.ASM.FileInfo ASMInfo = null;

    public int    LastShownMessageIndex = -1;

    public List<GR.Generic.Tupel<DocumentInfo,int>>   SourcesVisited = new List<GR.Generic.Tupel<DocumentInfo, int>>();
    private int               CurrentVisitedSource = -1;



    public delegate void OpenDocumentAndGotoLineCallback( Project MarkProject, DocumentInfo Document, int Line, int CharIndex );



    public Navigating( StudioCore Core )
    {
      this.Core = Core;
    }



    public void UpdateFromMessages( GR.Collections.MultiMap<int, C64Studio.Parser.ParserBase.ParseMessage> Messages,
                                    Types.ASM.FileInfo ASMInfo,
                                    Project ParsedProject )
    {
      CompileMessages = Messages;
      Project = ParsedProject;
      this.ASMInfo = ASMInfo;

      LastShownMessageIndex = -1;
    }



    public bool DocumentHasASMFileInfo( DocumentInfo Doc )
    {
      if ( Doc == null )
      {
        return false;
      }
      if ( ( Doc.Type == ProjectElement.ElementType.ASM_SOURCE )
      ||   ( Doc.Type == ProjectElement.ElementType.BASIC_SOURCE ) )
      {
        return true;
      }
      return false;
    }



    public Types.ASM.FileInfo DetermineASMFileInfo( DocumentInfo doc )
    {
      DocumentInfo possibleDoc = Core.MainForm.DetermineDocumentToCompile( false );

      if ( ( possibleDoc != null )
      &&   ( DocumentHasASMFileInfo( possibleDoc ) ) )
      {
        return possibleDoc.ASMFileInfo;
      }
      if ( DocumentHasASMFileInfo( doc ) )
      {
        return doc.ASMFileInfo;
      }
      return null;
    }



    public Types.ASM.FileInfo DetermineLocalASMFileInfo( DocumentInfo doc )
    {
      if ( DocumentHasASMFileInfo( doc ) )
      {
        return doc.ASMFileInfo;
      }
      return null;
    }



    public void OpenDocumentAndGotoLine( Project MarkProject, DocumentInfo Document, int Line )
    {
      OpenDocumentAndGotoLine( MarkProject, Document, Line, 0 );
    }



    public void OpenDocumentAndGotoLine( Project MarkProject, DocumentInfo Document, int Line, int CharIndex )
    {
      if ( Core.MainForm.InvokeRequired )
      {
        Core.MainForm.Invoke( new OpenDocumentAndGotoLineCallback( OpenDocumentAndGotoLine ), new object[] { MarkProject, Document, Line, CharIndex } );
        return;
      }

      if ( CharIndex < 0 )
      {
        CharIndex = 0;
      }

      if ( Document != null )
      {
        var baseDoc = Core.Navigating.FindDocumentByPath( Document.FullPath );
        if ( baseDoc != null )
        {
          baseDoc.Show();
          baseDoc.SetCursorToLine( Line, CharIndex, true );
          return;
        }
      }

      if ( MarkProject != null )
      {
        string  inPath = Document.FullPath.Replace( "\\", "/" );

        foreach ( ProjectElement element in MarkProject.Elements )
        {
          if ( GR.Path.IsPathEqual( GR.Path.Append( MarkProject.Settings.BasePath, element.Filename ), inPath ) )
          {
            BaseDocument doc = MarkProject.ShowDocument( element );
            if ( doc != null )
            {
              doc.SetCursorToLine( Line, CharIndex, true );
            }
            return;
          }
        }
      }
      if ( Document.FullPath.Length > 0 )
      {
        // file is not part of project
        BaseDocument newDoc = Core.MainForm.OpenFile( Document.FullPath );
        if ( newDoc != null )
        {
          bool setFromMainDoc = false;
          if ( ( !setFromMainDoc )
          &&   ( Core.Compiling.ParserASM.ASMFileInfo.ContainsFile( newDoc.DocumentInfo.FullPath ) ) )
          {
            if ( !Core.Compiling.IsCurrentlyBuilding() )
            {
              newDoc.DocumentInfo.SetASMFileInfo( Core.Compiling.ParserASM.ASMFileInfo, Core.Compiling.ParserASM.KnownTokens(), Core.Compiling.ParserASM.KnownTokenInfo() );
            }
          }
          //Debug.Log( "m_Outline.RefreshFromDocument after showdoc" );
          //Core.MainForm.m_Outline.RefreshFromDocument( newDoc.DocumentInfo.BaseDoc );

          newDoc.SetCursorToLine( Line, CharIndex, true );
        }
      }
    }



    public BaseDocument FindDocumentByPath( string FullPath )
    {
      string  inPath = FullPath.Replace( "\\", "/" );
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
          return baseDoc;
        }
      }
      return null;
    }



    public DocumentInfo FindDocumentInfoByPath( string FullPath )
    {
      string  inPath = FullPath.Replace( "\\", "/" );
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
          return baseDoc.DocumentInfo;
        }
      }
      return new DocumentInfo() { DocumentFilename = FullPath };
    }



    internal void OpenSourceOfNextMessage()
    {
      if ( CompileMessages == null )
      {
        return;
      }

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

          OpenDocumentAndGotoLine( Project, FindDocumentInfoByPath( documentFile ), documentLine );
          return;
        }
        --offset;
      }
    }



    public void GotoDeclaration( DocumentInfo ASMDoc, string Word, string Zone, string CheapLabelParent )
    {
      Types.ASM.FileInfo fileToDebug = DetermineASMFileInfo( ASMDoc );

      SymbolInfo tokenInfo = fileToDebug.TokenInfoFromName( Word, Zone, CheapLabelParent );
      var macro = ASMDoc.ASMFileInfo.MacroFromName( Word );
      if ( macro != null )
      {
        if ( fileToDebug.FindTrueLineSource( macro.LineIndex, out string fileName, out int localLineIndex ) )
        {
          OpenDocumentAndGotoLine( ASMDoc.Project, FindDocumentInfoByPath( fileName ), localLineIndex );
          return;
        }
      }
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
        &&   ( !string.IsNullOrEmpty( tokenInfo.DocumentFilename ) ) )
        {
          // try stored info first
          OpenDocumentAndGotoLine( ASMDoc.Project, FindDocumentInfoByPath( tokenInfo.DocumentFilename ), tokenInfo.LocalLineIndex );
          return;
        }

        if ( fileToDebug.FindTrueLineSource( tokenInfo.LineIndex, out documentFile, out documentLine ) )
        {
          OpenDocumentAndGotoLine( ASMDoc.Project, FindDocumentInfoByPath( tokenInfo.DocumentFilename ), documentLine );
          return;
        }
      }
      System.Windows.Forms.MessageBox.Show( "Could not determine item source" );
    }



    internal void InsertLines( DocumentInfo DocumentInfo, int LocalLineIndex, int LineCount )
    {
      if ( ( DocumentInfo == null )
      ||   ( DocumentInfo.ASMFileInfo == null ) )
      {
        return;
      }
      int   globalLineIndex = -1;
      if ( !DocumentInfo.ASMFileInfo.FindGlobalLineIndex( LocalLineIndex, DocumentInfo.FullPath, out globalLineIndex ) )
      {
        return;
      }

      DocumentInfo.ASMFileInfo.InsertLines( globalLineIndex, LocalLineIndex, LineCount );
    }



    internal void RemoveLines( DocumentInfo DocumentInfo, int LocalLineIndex, int LineCount )
    {
      if ( ( DocumentInfo == null )
      ||   ( DocumentInfo.ASMFileInfo == null ) )
      {
        return;
      }
      int   globalLineIndex = -1;
      if ( !DocumentInfo.ASMFileInfo.FindGlobalLineIndex( LocalLineIndex, DocumentInfo.FullPath, out globalLineIndex ) )
      {
        return;
      }

      DocumentInfo.ASMFileInfo.RemoveLines( globalLineIndex, LocalLineIndex, LineCount );
    }



    internal DocumentInfo FindDocumentByFilename( string Filename )
    {
      if ( Project != null )
      {
        ProjectElement element = Project.GetElementByFilename( Filename );
        if ( ( element != null )
        &&   ( element.DocumentInfo != null ) )
        {
          return element.DocumentInfo;
        }
      }
      var docs = Core.MainForm.DocumentInfos;
      foreach ( var doc in docs )
      {
        if ( string.IsNullOrEmpty( doc.FullPath ) )
        {
          continue;
        }
        if ( GR.Path.IsPathEqual( doc.FullPath, Filename ) )
        {
          return doc;
        }
      }

      return null;
    }



    public void VisitedLine( DocumentInfo Doc, int LineIndex )
    {
      if ( CurrentVisitedSource == -1 )
      {
        SourcesVisited.Add( new GR.Generic.Tupel<DocumentInfo, int>( Doc, LineIndex ) );
      }
      else if ( CurrentVisitedSource < SourcesVisited.Count )
      {
        if ( ( SourcesVisited[CurrentVisitedSource].first == Doc )
        &&   ( SourcesVisited[CurrentVisitedSource].second == LineIndex ) )
        {
          // was a visit from the list
        }
        else
        {
          // a new move, remove all after the current position
          SourcesVisited.RemoveRange( CurrentVisitedSource, SourcesVisited.Count - CurrentVisitedSource );
          CurrentVisitedSource = -1;

          SourcesVisited.Add( new GR.Generic.Tupel<DocumentInfo, int>( Doc, LineIndex ) );
        }
      }
      else
      {
        CurrentVisitedSource = -1;
      }
      Core.MainForm.UpdateUndoSettings();
    }



    public bool NavigateForwardPossible
    {
      get
      {
        return ( ( SourcesVisited.Count > 0 )
            &&   ( CurrentVisitedSource != -1 )
            &&   ( CurrentVisitedSource + 1 < SourcesVisited.Count ) );
      }
    }



    public bool NavigateBackwardPossible
    {
      get
      {
        return ( ( SourcesVisited.Count > 0 )
              && ( ( CurrentVisitedSource == -1 )
              ||   ( CurrentVisitedSource > 0 ) ) );
      }
    }



    public void NavigateBack()
    {
      if ( SourcesVisited.Count == 0 )
      {
        return;
      }

      if ( CurrentVisitedSource == -1 )
      {
        CurrentVisitedSource = SourcesVisited.Count - 1;
      }
      if ( CurrentVisitedSource - 1 < 0 )
      {
        return;
      }
      --CurrentVisitedSource;

      Core.MainForm.UpdateUndoSettings();

      OpenDocumentAndGotoLine( null, SourcesVisited[CurrentVisitedSource].first, SourcesVisited[CurrentVisitedSource].second );
    }



    public void NavigateForward()
    {
      if ( SourcesVisited.Count == 0 )
      {
        return;
      }

      if ( CurrentVisitedSource == -1 )
      {
        CurrentVisitedSource = 0;
      }
      if ( CurrentVisitedSource + 1 >= SourcesVisited.Count )
      {
        return;
      }
      ++CurrentVisitedSource;

      Core.MainForm.UpdateUndoSettings();
      OpenDocumentAndGotoLine( null, SourcesVisited[CurrentVisitedSource].first, SourcesVisited[CurrentVisitedSource].second );
    }



  }
}
