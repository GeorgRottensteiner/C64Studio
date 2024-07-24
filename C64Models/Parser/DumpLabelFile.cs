using GR.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using RetroDevStudio.Types.ASM;



namespace RetroDevStudio.Parser
{
  public class DumpLabelFile
  {
    public static void Dump( FileInfo FileInfo, LabelDumpSettings Settings )
    {
      StringBuilder sb = new StringBuilder();

      // sort by defining order
      var resultingInfo = new GR.Collections.MultiMap<int,SymbolInfo>();

      foreach ( var labelInfo in FileInfo.Labels )
      {
        if ( ( labelInfo.Key == ASMFileParser.ASSEMBLER_ID_C64STUDIO )
        ||   ( labelInfo.Key == ASMFileParser.ASSEMBLER_ID_RETRODEVSTUDIO ) )
        {
          if ( !Settings.IncludeAssemblerIDLabels )
          {
            continue;
          }
        }
        if ( ( Settings.IgnoreInternalLabels )
        &&   ( labelInfo.Key.Contains( $"_{ASMFileParser.INTERNAL_LABEL_PREFIX}_" ) ) )
        {
          continue;
        }
        if ( ( Settings.IgnoreLocalLabels )
        &&   ( labelInfo.Key.StartsWith( ASMFileParser.INTERNAL_LOCAL_LABEL_PREFIX ) ) )
        {
          continue;
        }
        resultingInfo.Add( labelInfo.Value.LineIndex, labelInfo.Value );
      }

      var uniqueTempLabels = new GR.Collections.Map<string,TemporaryLabelInfo>();
      foreach ( var tempLabel in FileInfo.TempLabelInfo )
      {
        // for loop labels have a length set
        if ( tempLabel.Length > 0 )
        {
          uniqueTempLabels.Add( tempLabel.Name, tempLabel );
        }
      }
      foreach ( var tempLabel in uniqueTempLabels )
      {
        resultingInfo.Add( tempLabel.Value.LineIndex, tempLabel.Value.Symbol );
      }

      foreach ( var info in resultingInfo.Values )
      {
        if ( ( Settings.IgnoreUnusedLabels )
        &&   ( info.References.Count == 0 ) )
        {
          continue;
        }
        sb.Append( info.Name );
        sb.Append( " = $" );
        if ( info.AddressOrValue > 255 )
        {
          sb.Append( info.AddressOrValue.ToString( "X4" ) );
        }
        else
        {
          sb.Append( info.AddressOrValue.ToString( "X2" ) );
        }
        sb.Append( "; " );
        if ( info.References.Count == 0 )
        {
          sb.AppendLine( "unused" );
        }
        else
        {
          sb.AppendLine();
        }
      }
      GR.IO.File.WriteAllText( FileInfo.LabelDumpSettings.Filename, sb.ToString() );
    }

  }


}
