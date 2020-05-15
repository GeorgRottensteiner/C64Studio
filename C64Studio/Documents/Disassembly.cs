using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio
{
  public class Disassembly : ReadOnlyFile
  {
    private FastColoredTextBoxNS.TextStyle[]          m_TextStyles = new FastColoredTextBoxNS.TextStyle[(int)Types.ColorableElement.LAST_ENTRY];
    private System.Text.RegularExpressions.Regex[]    m_TextRegExp = new System.Text.RegularExpressions.Regex[(int)Types.ColorableElement.LAST_ENTRY];



    public Disassembly( StudioCore Core )
      : base( Core )
    {
      editText.TextChanged += new EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>( editText_TextChanged );

      string opCodes = @"\b(lda|sta|ldy|sty|ldx|stx|rts|jmp|jsr|rti|sei|cli|asl|lsr|inc|dec|inx|dex|iny|dey|cpx|cpy|cmp|bit|bne|beq|bcc|bcs|bpl|bmi|adc|sec|clc|sbc|tax|tay|tya|txa|pha|pla|eor|and|ora|ror|rol|php|plp|clv|cld|bvc|bvs|brk|nop|txs|tsx|slo|rla|sre|rra|sax|lax|dcp|isc|anc|alr|arr|xaa|axs|ahx|shy|shx|tas|las|sed)\b";
      string pseudoOps = @"(!byte|!by|!basic|!8|!08|!word|!wo|!16|!text|!tx|!scr|!pet|!raw|!pseudopc|!realpc|!bank|!convtab|!ct|!binary|!bin|!bi|!source|!src|!to|!zone|!zn|!error|!serious|!warn|"
        + @"!message|!ifdef|!ifndef|!if|!fill|!fi|!align|!endoffile|!nowarn|!for|!end|!macro|!trace|!media|!mediasrc|!sl|!cpu|!set)\b";

      m_TextRegExp[(int)Types.ColorableElement.LITERAL_NUMBER] = new System.Text.RegularExpressions.Regex( @"\b\d+[\.]?\d*([eE]\-?\d+)?[lLdDfF]?\b|\B\$[a-fA-F\d]+\b|\b0x[a-fA-F\d]+\b" );
      m_TextRegExp[(int)Types.ColorableElement.LITERAL_STRING] = new System.Text.RegularExpressions.Regex( @"""""|''|"".*?[^\\]""|'.*?[^\\]'" );

      m_TextRegExp[(int)Types.ColorableElement.CODE] = new System.Text.RegularExpressions.Regex( opCodes, System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled );
      m_TextRegExp[(int)Types.ColorableElement.PSEUDO_OP] = new System.Text.RegularExpressions.Regex( pseudoOps, System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled );

      m_TextRegExp[(int)Types.ColorableElement.LABEL] = new System.Text.RegularExpressions.Regex( @"[+\-a-zA-Z]+[a-zA-Z_\d]*[:]*" );
      m_TextRegExp[(int)Types.ColorableElement.COMMENT] = new System.Text.RegularExpressions.Regex( @";.*" );

      RefreshDisplayOptions();
    }



    void ApplySyntaxColoring( Types.ColorableElement Element )
    {
      System.Drawing.Brush      foreBrush = new System.Drawing.SolidBrush( GR.Color.Helper.FromARGB( Core.Settings.FGColor( Element ) ) );
      System.Drawing.Brush      backBrush = null;
      System.Drawing.FontStyle  fontStyle = System.Drawing.FontStyle.Regular;

      backBrush = new System.Drawing.SolidBrush( GR.Color.Helper.FromARGB( Core.Settings.BGColor( Element ) ) );
      m_TextStyles[SyntaxElementStylePrio( Element )] = new FastColoredTextBoxNS.TextStyle( foreBrush, backBrush, fontStyle );

      //editSource.AddStyle( m_TextStyles[(int)Element] );
      editText.Styles[SyntaxElementStylePrio( Element )] = m_TextStyles[SyntaxElementStylePrio( Element )];
    }



    public override void RefreshDisplayOptions()
    {
      // Font
      editText.Font = new System.Drawing.Font( Core.Settings.SourceFontFamily, Core.Settings.SourceFontSize, Core.Settings.SourceFontStyle );

      // Colors
      ApplySyntaxColoring( Types.ColorableElement.COMMENT );
      ApplySyntaxColoring( Types.ColorableElement.LITERAL_NUMBER );
      ApplySyntaxColoring( Types.ColorableElement.LITERAL_STRING );
      ApplySyntaxColoring( Types.ColorableElement.LABEL );
      ApplySyntaxColoring( Types.ColorableElement.CODE );
      ApplySyntaxColoring( Types.ColorableElement.EMPTY_SPACE );
      ApplySyntaxColoring( Types.ColorableElement.OPERATOR );
      ApplySyntaxColoring( Types.ColorableElement.PSEUDO_OP );

      editText.Language = FastColoredTextBoxNS.Language.VB;
      editText.CommentPrefix = ";";

      //editText.Indentation.UseTabs = !Core.Settings.TabConvertToSpaces;
      editText.TabLength = Core.Settings.TabSize;

      //call OnTextChanged for refresh syntax highlighting
      editText.OnTextChanged();
    }



    int SyntaxElementStylePrio( Types.ColorableElement Element )
    {
      int     value = 10;

      switch ( Element )
      {
        case C64Studio.Types.ColorableElement.CODE:
          value = 6;
          break;
        case C64Studio.Types.ColorableElement.COMMENT:
          value = 2;
          break;
        case C64Studio.Types.ColorableElement.CURRENT_DEBUG_LINE:
          value = 1;
          break;
        case C64Studio.Types.ColorableElement.EMPTY_SPACE:
          value = 0;
          break;
        case C64Studio.Types.ColorableElement.LABEL:
          value = 7;
          break;
        case C64Studio.Types.ColorableElement.LITERAL_NUMBER:
          value = 3;
          break;
        case C64Studio.Types.ColorableElement.LITERAL_STRING:
          value = 4;
          break;
        case C64Studio.Types.ColorableElement.PSEUDO_OP:
          value = 5;
          break;
        case C64Studio.Types.ColorableElement.NONE:
          value = 9;
          break;
        case C64Studio.Types.ColorableElement.OPERATOR:
          value = 8;
          break;
      }
      return value;
    }

    
    
    void editText_TextChanged( object sender, FastColoredTextBoxNS.TextChangedEventArgs e )
    {
      //clear previous highlighting
      e.ChangedRange.ClearStyle( m_TextStyles );

      // apply all styles
      e.ChangedRange.SetStyle( m_TextStyles[SyntaxElementStylePrio( Types.ColorableElement.LITERAL_NUMBER )], m_TextRegExp[(int)Types.ColorableElement.LITERAL_NUMBER] );
      e.ChangedRange.SetStyle( m_TextStyles[SyntaxElementStylePrio( Types.ColorableElement.LITERAL_STRING )], m_TextRegExp[(int)Types.ColorableElement.LITERAL_STRING] );
      e.ChangedRange.SetStyle( m_TextStyles[SyntaxElementStylePrio( Types.ColorableElement.CODE )], m_TextRegExp[(int)Types.ColorableElement.CODE] );
      e.ChangedRange.SetStyle( m_TextStyles[SyntaxElementStylePrio( Types.ColorableElement.PSEUDO_OP )], m_TextRegExp[(int)Types.ColorableElement.PSEUDO_OP] );
      e.ChangedRange.SetStyle( m_TextStyles[SyntaxElementStylePrio( Types.ColorableElement.LABEL )], m_TextRegExp[(int)Types.ColorableElement.LABEL] );
      e.ChangedRange.SetStyle( m_TextStyles[SyntaxElementStylePrio( Types.ColorableElement.COMMENT )], m_TextRegExp[(int)Types.ColorableElement.COMMENT] );
    }




  }
}
