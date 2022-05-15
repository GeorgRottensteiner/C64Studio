using RetroDevStudio.Parser;



namespace RetroDevStudio.Parser
{
  public partial class BasicFileParser : ParserBase
  {
    private bool MetaDataInclude( int LineIndex, string MetaData )
    {
      // followed by string literal for file name


      return true;
    }



  }
}
