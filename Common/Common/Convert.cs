using System;

namespace GR
{
	/// <summary>
	/// Zusammenfassung für Convert.
	/// </summary>
	public class Convert
	{
    public static GR.Memory.ByteBuffer ToBCD( int iNumber, int iStellen )
    {
      string    strResult = iNumber.ToString();

      while ( strResult.Length < iStellen )
      {
        strResult = "0" + strResult;
      }

      return new GR.Memory.ByteBuffer( strResult );
    }



    public static float ToF32( string Value )
    {
      float     dummy = 0;

      float.TryParse( Value, out dummy );

      return dummy;
    }



    public static System.UInt32 BCDToNumber( GR.Memory.ByteBuffer bbBuffer )
    {
      return System.Convert.ToUInt32( bbBuffer.ToString() );
    }



    public static bool ToBoolean( string Value )
    {
      if ( ( Value == "0" )
      ||   ( Value == "" )
      ||   ( Value == null )
      ||   ( Value == "N" )
      ||   ( Value == false.ToString() ) )
      {
        return false;
      }
      return true;
    }

    public static sbyte ToI8( string strValue, int iBase )
    {
      return (sbyte)ToI64( strValue, iBase );
    }

    public static sbyte ToI8( string strValue )
    {
      return (sbyte)ToI64( strValue );
    }

    public static byte ToU8( string strValue, int iBase )
    {
      return (byte)ToU64( strValue, iBase );
    }

    public static byte ToU8( string strValue )
    {
      return (byte)ToU64( strValue );
    }

    public static Int16 ToI16( string strValue, int iBase )
    {
      return (Int16)ToI64( strValue, iBase );
    }

    public static Int16 ToI16( string strValue )
    {
      return (Int16)ToI64( strValue );
    }

    public static UInt16 ToU16( string strValue, int iBase )
    {
      return (UInt16)ToU64( strValue, iBase );
    }

    public static UInt16 ToU16( string strValue )
    {
      return (UInt16)ToU64( strValue );
    }

    public static int ToI32( string strValue, int iBase )
    {
      return (Int32)ToI64( strValue, iBase );
    }

    public static int ToI32( string strValue )
    {
      return (Int32)ToI64( strValue );
    }

    public static UInt32 ToU32( string strValue, int iBase )
    {
      return (UInt32)ToU64( strValue, iBase );
    }

    public static UInt32 ToU32( string strValue )
    {
      return (UInt32)ToU64( strValue );
    }

    public static Int64 ToI64( string strValue, int iBase )
    {
      if ( strValue == null )
      {
        return 0;
      }
      Int64 iValue = 0;

      int iPos = 0;

      if ( ( strValue.Length >= 2 )
      && ( strValue.Substring( 0, 2 ) == "0x" ) )
      {
        iPos = 2;
        iBase = 16;
      }
      if ( ( strValue.Length >= 1 )
      && ( strValue[0] == '#' ) )
      {
        iPos = 1;
        iBase = 16;
      }

      // Space darf am Anfang sein
      while ( ( iPos < strValue.Length )
      && ( strValue[iPos] == ' ' ) )
      {
        ++iPos;
      }


      if ( iBase == 16 )
      {
        // hexadezimal
        while ( iPos < strValue.Length )
        {
          iValue <<= 4;
          char cChar = strValue[iPos];

          // avoided use of toupper

          if ( ( cChar >= '0' )
          && ( cChar <= '9' ) )
          {
            iValue += cChar - '0';
          }
          else if ( ( cChar >= 'a' )
          && ( cChar <= 'f' ) )
          {
            iValue += cChar + 10 - 'a';
          }
          else if ( ( cChar >= 'A' )
          && ( cChar <= 'F' ) )
          {
            iValue += cChar + 10 - 'A';
          }
          ++iPos;
        }
        return iValue;
      }
      if ( iBase == 2 )
      {
        while ( iPos < strValue.Length )
        {
          iValue <<= 1;
          char cChar = strValue[iPos];

          // avoided use of toupper

          if ( ( cChar >= '0' )
          &&   ( cChar <= '1' ) )
          {
            iValue += cChar - '0';
          }
          ++iPos;
        }
        return iValue;
      }
      return ToI64( strValue );
    }

    public static Int64 ToI64( string strValue )
    {
      if ( strValue == null )
      {
        return 0;
      }
      Int64 Value = 0;

      int iPos = 0;

      // Space darf am Anfang sein
      while ( ( iPos < strValue.Length )
      && ( strValue[iPos] == ' ' ) )
      {
        ++iPos;
      }


      bool bNegative = false;

      while ( iPos < strValue.Length )
      {
        Value *= 10;
        char cChar = strValue[iPos];

        // avoided use of toupper
        if ( ( iPos == 0 )
        &&   ( cChar == '-' ) )
        {
          bNegative = true;
        }
        else if ( ( cChar >= '0' )
        &&        ( cChar <= '9' ) )
        {
          Value += cChar - '0';
        }
        else
        {
          // ungültiges Zeichen
          Value /= 10;
          break;
        }
        ++iPos;
      }
      return bNegative ? -Value : Value;
    }

    public static UInt64 ToU64( string strValue, int iBase )
    {
      return (UInt64)ToI64( strValue, iBase );
    }

    public static UInt64 ToU64( string strValue )
    {
      return (UInt64)ToI64( strValue );
    }

	}
}
