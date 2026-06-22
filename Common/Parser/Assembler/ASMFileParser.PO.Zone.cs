using GR.Collections;
using GR.Memory;
using RetroDevStudio.Formats;
using RetroDevStudio.Parser;
using RetroDevStudio.Types;
using RetroDevStudio.Types.ASM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tiny64;

namespace RetroDevStudio.Parser
{
  public partial class ASMFileParser : ParserBase
  {
    private void POZone( LineInfo info, List<TokenInfo> lineTokenInfos, bool AutoGlobalLabel )
    {
      bool  scopedZone = false;

      if ( lineTokenInfos[lineTokenInfos.Count - 1].Content == "{" )
      {
        lineTokenInfos.RemoveAt( lineTokenInfos.Count - 1 );

        // TODO - check if zonescope exists, no nestes zone scopes!

        Types.ScopeInfo   zoneScope = new RetroDevStudio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.ZONE );
        zoneScope.StartIndex = _ParseContext.LineIndex;
        _ParseContext.Scopes.Add( zoneScope );
        OnScopeAdded( zoneScope );
        scopedZone = true;
      }
      if ( ScopeInsideMacroDefinition() )
      {
        return;
      }
      if ( lineTokenInfos.Count > 2 )
      {
        AddError( _ParseContext.LineIndex, Types.ErrorCode.E1303_MALFORMED_ZONE_DESCRIPTOR, "Expected single zone descriptor" );
      }
      else
      {
        var   zoneToken = lineTokenInfos[0];
        if ( lineTokenInfos.Count == 1 )
        {
          // anonymous zone -> all upper case 
          // back to global zone 
          DetermineActiveZone();
          return;
        }
        else
        {
          m_CurrentZoneName = DeQuote( lineTokenInfos[1].Content );
          if ( !scopedZone )
          {
            m_CurrentGlobalZoneName = m_CurrentZoneName;
          }
          else
          {
            _ParseContext.Scopes.Last().Name = m_CurrentZoneName;
          }
          zoneToken = lineTokenInfos[1];
        }
        info.Zone = m_CurrentZoneName;

        AddZone( m_CurrentZoneName, _ParseContext.LineIndex, zoneToken.StartPos, zoneToken.Length, m_CompileCurrentAddress );
        if ( AutoGlobalLabel )
        {
          var label = AddLabel( m_CurrentZoneName, m_CompileCurrentAddress, _ParseContext.LineIndex, m_CurrentZoneName, zoneToken.StartPos, zoneToken.Length );

          label.Info = m_CurrentCommentSB.ToString();
          m_CurrentCommentSB = new StringBuilder();
        }
      }
    }



    // TMP styled zone (.block), is always scoped
    private void POScopedZone( LineInfo info, List<TokenInfo> lineTokenInfos, bool AutoGlobalLabel )
    {
      if ( lineTokenInfos.Count != 1 )
      {
        AddError( _ParseContext.LineIndex, Types.ErrorCode.E1000_SYNTAX_ERROR, "Expected single .block PO" );
        return;
      }

      StartScopedZone( "TMPZONE_" + _ParseContext.LineIndex.ToString() );
    }



    private ScopeInfo StartScopedZone( string zoneName )
    {
      var zoneScope = new RetroDevStudio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.ZONE );
      zoneScope.Name = zoneName;
      zoneScope.StartIndex = _ParseContext.LineIndex;
      m_CurrentZoneName = zoneScope.Name;
      _ParseContext.Scopes.Add( zoneScope );
      OnScopeAdded( zoneScope );
      return zoneScope;
    }



    private ParseLineResult POScopedZoneEnd( LineInfo info, List<TokenInfo> lineTokenInfos )
    {
      if ( _ParseContext.Scopes.Count == 0 )
      {
        AddError( _ParseContext.LineIndex, Types.ErrorCode.E1007_MISSING_LOOP_START, "Missing block start" );
        return ParseLineResult.ERROR_ABORT;
      }

      Types.ScopeInfo   lastOpenedScope = _ParseContext.Scopes[_ParseContext.Scopes.Count - 1];

      if ( lastOpenedScope.Type != Types.ScopeInfo.ScopeType.ZONE )
      {
        AddError( _ParseContext.LineIndex, Types.ErrorCode.E1007_MISSING_LOOP_START, "Missing block start" );
        return ParseLineResult.ERROR_ABORT;
      }
      OnScopeRemoved( _ParseContext.LineIndex );
      _ParseContext.Scopes.RemoveAt( _ParseContext.Scopes.Count - 1 );
      DetermineActiveZone();
      return ParseLineResult.OK;
    }

  }
}
