using RetroDevStudio.Formats;
using RetroDevStudio.Types;
using System;
using System.Windows.Forms;
using RetroDevStudio.Documents;
using RetroDevStudio.Parser.BASIC;



namespace RetroDevStudio.Controls
{
  public partial class ImportCharscreenFromBASIC : ImportCharscreenFormBase
  {
    public ImportCharscreenFromBASIC() :
      base( null )
    { 
    }



    public ImportCharscreenFromBASIC( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();

      editInput.Font = Core.Imaging.FontFromMachine( MachineType.C64 );
    }



    public override bool HandleImport( CharsetScreenProject CharScreen, CharsetScreenEditor Editor )
    {
      var settings = new BasicFileParser.ParserSettings();
      settings.StripREM = true;
      settings.StripSpaces = true;
      settings.BASICDialect = Dialect.BASICV2;

      var parser = new BasicFileParser( settings );

      string[] lines = editInput.Text.Split( new char[]{ '\n' }, StringSplitOptions.RemoveEmptyEntries );
      int lastLineNumber = -1;

      int     cursorX = 0;
      int     cursorY = 0;
      byte    curColor = 14;
      bool    reverseMode = false;

      foreach ( var line in lines )
      {
        var  cleanLine = line.Trim();

        var lineInfo = parser.TokenizeLine( cleanLine, 1, ref lastLineNumber );

        for ( int i = 0; i < lineInfo.Tokens.Count; ++i )
        {
          var token = lineInfo.Tokens[i];

          if ( ( token.TokenType == BasicFileParser.Token.Type.BASIC_TOKEN )
          &&   ( token.ByteValue == 0x99 ) )
          {
            // a PRINT statement
            bool    hasSemicolonAtEnd = false;
            while ( ( i + 1 < lineInfo.Tokens.Count )
            && ( lineInfo.Tokens[i + 1].Content != ":" ) )
            {
              ++i;

              var nextToken = lineInfo.Tokens[i];

              if ( nextToken.TokenType == BasicFileParser.Token.Type.STRING_LITERAL )
              {
                // handle incoming PETSCII plus control codes!
                bool hadError = false;
                var  actualString = BasicFileParser.ReplaceAllMacrosBySymbols( nextToken.Content.Substring( 1, nextToken.Content.Length - 2 ), MachineType.C64, out hadError );
                foreach ( var singleChar in actualString )
                {
                  var key = ConstantData.AllPhysicalKeyInfos[MachineType.C64].Find( x => x.CharValue == singleChar );
                  if ( key != null )
                  {
                    if ( ( key.Type == KeyType.GRAPHIC_SYMBOL )
                    ||   ( key.Type == KeyType.NORMAL ) )
                    {
                      if ( reverseMode )
                      {
                        Editor.SetCharacter( cursorX, cursorY, (ushort)( key.ScreenCodeValue + 128 ), curColor, 0 );
                      }
                      else
                      {
                        Editor.SetCharacter( cursorX, cursorY, key.ScreenCodeValue, curColor, 0 );
                      }
                      ++cursorX;
                      if ( cursorX >= CharScreen.ScreenWidth )
                      {
                        cursorX = 0;
                        ++cursorY;
                      }
                    }
                    else if ( key.Type == KeyType.CONTROL_CODE )
                    {
                      switch ( key.NativeValue )
                      {
                        case 5:
                          curColor = 1;
                          break;
                        case 17:
                          ++cursorY;
                          break;
                        case 18:
                          reverseMode = true;
                          break;
                        case 28:
                          curColor = 2;
                          break;
                        case 29:
                          ++cursorX;
                          break;
                        case 30:
                          curColor = 5;
                          break;
                        case 31:
                          curColor = 6;
                          break;
                        case 129:
                          curColor = 8;
                          break;
                        case 144:
                          curColor = 0;
                          break;
                        case 145:
                          --cursorY;
                          break;
                        case 146:
                          reverseMode = false;
                          break;
                        case 149:
                          curColor = 9;
                          break;
                        case 150:
                          curColor = 10;
                          break;
                        case 151:
                          curColor = 11;
                          break;
                        case 152:
                          curColor = 12;
                          break;
                        case 153:
                          curColor = 13;
                          break;
                        case 154:
                          curColor = 14;
                          break;
                        case 155:
                          curColor = 15;
                          break;
                        case 156:
                          curColor = 4;
                          break;
                        case 157:
                          --cursorX;
                          if ( cursorX < 0 )
                          {
                            cursorX += CharScreen.ScreenWidth;
                            --cursorY;
                          }
                          break;
                        case 158:
                          curColor = 7;
                          break;
                        case 159:
                          curColor = 3;
                          break;
                      }
                    }
                  }
                }
                continue;
              }
              else if ( nextToken.Content == ";" )
              {
                hasSemicolonAtEnd = true;
              }
              else
              {
                hasSemicolonAtEnd = false;
                reverseMode = false;
              }
            }
            if ( !hasSemicolonAtEnd )
            {
              cursorX = 0;
              ++cursorY;

              // TODO - Scroll up
            }
          }
        }
      }
      return true;
    }



    private void editInput_KeyPress( object sender, KeyPressEventArgs e )
    {
      if ( ( ModifierKeys == Keys.Control )
      &&   ( e.KeyChar == 1 ) )
      {
        editInput.SelectAll();
        e.Handled = true;
      }
    }



  }
}
