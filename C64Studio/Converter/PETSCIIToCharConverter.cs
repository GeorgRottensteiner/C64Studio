using Be.Windows.Forms;
using RetroDevStudio;



namespace C64Studio.Converter
{
  public class PETSCIIToCharConverter : IByteCharConverter
  {
    public char ToChar( byte b )
    {
      if ( !ConstantData.ScreenCodeToChar.ContainsKey( b ) )
      {
        return '.';
      }
      return ConstantData.ScreenCodeToChar[b].CharValue;
    }



    public byte ToByte( char c )
    {
      return ConstantData.PETSCII[c];
    }

  }
}
