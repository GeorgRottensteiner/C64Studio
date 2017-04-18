using Be.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio.Converter
{
  public class PETSCIIToCharConverter : IByteCharConverter
  {
    public char ToChar( byte b )
    {
      return Types.ConstantData.ScreenCodeToChar[b].CharValue;
    }



    public byte ToByte( char c )
    {
      return Types.ConstantData.PETSCII[c];
    }

  }
}
