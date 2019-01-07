using System;

namespace GR
{
  namespace Memory
  {
	  /// <summary>
	  /// Zusammenfassung für ByteBuffer.
	  /// </summary>
	  public class ByteBuffer
	  {
      private static readonly byte[] EmptyByteArray = new byte[0];  // alternatively alias System.Array.Empty<byte>()

      private System.Runtime.InteropServices.GCHandle   m_PinnedHandle = default( System.Runtime.InteropServices.GCHandle );

      private byte[] m_Data = EmptyByteArray;

      private UInt32 m_UsedBytes = 0;



		  public ByteBuffer()
		  {
		  }



		  public ByteBuffer( GR.Memory.ByteBuffer bbRHS )
		  {
        if ( bbRHS.Length > 0 )
        {
          Append( bbRHS.Data() );
        }
		  }



      public ByteBuffer( UInt32 InitialSize )
      {
        Resize( InitialSize );
      }



      public ByteBuffer( byte[] bData )
      {
        Append( bData );
      }



		  public ByteBuffer( string strHexData )
		  {
        FromHexString( strHexData );
		  }



      public static ByteBuffer operator +( ByteBuffer bb1, ByteBuffer bb2 )
      {
        ByteBuffer bbNew = new ByteBuffer( bb1.Data() );

        bbNew.Append( bb2 );

        return bbNew;
      }



      public int Compare( GR.Memory.ByteBuffer OtherBuffer )
      {
        for ( int i = 0; i < Length; ++i )
        {
          if ( i >= OtherBuffer.Length )
          {
            return 1;
          }
          if ( ByteAt( i ) < OtherBuffer.ByteAt( i ) )
          {
            return -1;
          }
          else if ( ByteAt( i ) > OtherBuffer.ByteAt( i ) )
          {
            return 1;
          }
        }
        if ( OtherBuffer.Length > Length )
        {
          return -1;
        }
        return 0;
      }



      public GR.Memory.ByteBuffer SubBuffer( int iIndex )
      {
        GR.Memory.ByteBuffer    bbResult = new GR.Memory.ByteBuffer();

        if ( iIndex >= Length )
        {
          return bbResult;
        }
        for ( int i = iIndex; i < Length; ++i )
        {
          bbResult.AppendU8( ByteAt( i ) );
        }
        return bbResult;
      }



      public GR.Memory.ByteBuffer SubBuffer( int iIndex, int iLength )
      {
        GR.Memory.ByteBuffer    bbResult = new GR.Memory.ByteBuffer();

        if ( ( iIndex >= Length )
        ||   ( iIndex + iLength > Length ) )
        {
          return bbResult;
        }
        for ( int i = iIndex; i < iIndex + iLength; ++i )
        {
          bbResult.AppendU8( ByteAt( i ) );
        }
        return bbResult;
      }

      public byte[] Data()
      {
        if ( m_Data == null )
        {
          return null;
        }
        if ( m_UsedBytes == m_Data.Length )
        {
          return m_Data;
        }
        byte[]    SubArray = new byte[m_UsedBytes];

        Array.Copy( m_Data, SubArray, m_UsedBytes );
        return SubArray;
      }

      public byte[] Data( int iIndex, int iBytes )
      {
        if ( ( iIndex < 0 )
        ||   ( iIndex + iBytes >= Length ) )
        {
          return EmptyByteArray;
        }
        byte[] bReturn = new byte[iBytes];

        Array.Copy( m_Data, iIndex, bReturn, 0, iBytes );
        return bReturn;
      }

      public void Append( ByteBuffer bbOther )
      {
        if ( bbOther != null )
        {
          Append( bbOther.Data() );
        }
      }

      public void AppendRepeated( System.Byte bByte, int iCount )
      {
        for ( int i = 0; i < iCount; ++i )
        {
          AppendU8( bByte );
        }
      }

      public bool AppendHex( string strHexData )
      {
        if ( ( strHexData.Length == 0 )
        ||   ( ( strHexData.Length % 2 ) == 1 ) )
        {
          return false;
        }

        for ( int i = 0; i < strHexData.Length; i += 2 )
        {
          AppendU8( GR.Convert.ToU8( strHexData.Substring( i, 2 ), 16 ) );
        }
        return true;
      }

      public void AppendU16( UInt16 wData )
      {
        AppendU8( (byte)( wData & 0xff ) );
        AppendU8( (byte)( wData >> 8 ) );
      }

      public void AppendU16NetworkOrder( UInt16 wData )
      {
        AppendU8( (byte)( wData >> 8 ) );
        AppendU8( (byte)( wData & 0xff ) );
      }

      public void AppendU32( UInt32 dwData )
      {
        AppendU8( (byte)( dwData & 0xff ) );
        AppendU8( (byte)( dwData >> 8 ) );
        AppendU8( (byte)( dwData >> 16 ) );
        AppendU8( (byte)( dwData >> 24 ) );
        
      }

      public void AppendI32( Int32 dwData )
      {
        AppendU32( (UInt32)dwData );
      }

      public void AppendU32NetworkOrder( UInt32 dwData )
      {
        AppendU8( (byte)( dwData >> 24 ) );
        AppendU8( (byte)( dwData >> 16 ) );
        AppendU8( (byte)( dwData >> 8 ) );
        AppendU8( (byte)( dwData & 0xff ) );
      }

      public void AppendString( string strData )
      {
        if ( strData == null )
        {
          AppendU32( 0 );
          return;
        }

        AppendU32( (UInt32)strData.Length );
        for ( int i = 0; i < strData.Length; ++i )
        {
          AppendU8( (byte)strData[i] );
        }
      }

      public void AppendU8( byte bData )
      {
        if ( m_Data == null )
        {
          m_Data = new byte[1];

          m_Data.SetValue( bData, 0 );
          m_UsedBytes = 1;
        }
        else
        {
          if ( m_UsedBytes < m_Data.Length )
          {
            m_Data[m_UsedBytes] = bData;
            ++m_UsedBytes;
          }
          else
          {
            byte[] bTemp = new byte[m_Data.Length + 1];

            m_Data.CopyTo( bTemp, 0 );
            bTemp.SetValue( bData, m_Data.Length );

            m_Data = bTemp;
            m_UsedBytes = (UInt32)m_Data.Length;
          }
        }
      }

      public void AppendU8Front( byte bData )
      {
        if ( m_Data == null )
        {
          m_Data = new byte[1];

          m_Data.SetValue( bData, 0 );
          m_UsedBytes = 1;
        }
        else
        {
          byte[] bTemp = new byte[m_Data.Length + 1];
             
          m_Data.CopyTo( bTemp, 1 );
          bTemp[0] = bData;
          m_Data = bTemp;
          m_UsedBytes = (UInt32)m_Data.Length;
        }
      }

      public void SetU32At( int iIndex, System.UInt32 dwValue )
      {
        if ( ( iIndex < 0 )
        ||   ( iIndex + 3 >= Length ) )
        {
          return;
        }
        m_Data[iIndex]     = (byte)( dwValue & 0xff );
        m_Data[iIndex + 1] = (byte)( ( dwValue >> 8 ) & 0xff );
        m_Data[iIndex + 2] = (byte)( ( dwValue >> 16 ) & 0xff );
        m_Data[iIndex + 3] = (byte)( ( dwValue >> 24 ) & 0xff );
      }

      public void SetU16At( int iIndex, System.UInt16 wWord )
      {
        if ( ( iIndex < 0 )
        ||   ( iIndex + 1 >= Length ) )
        {
          return;
        }
        m_Data[iIndex]     = (byte)( wWord & 0xff );
        m_Data[iIndex + 1] = (byte)( wWord >> 8 );
      }



      public void SetU8At( int iIndex, System.Byte Byte )
      {
        if ( ( iIndex < 0 )
        ||   ( iIndex >= Length ) )
        {
          return;
        }
        m_Data[iIndex] = Byte;
      }



      public void Append( byte[] bData )
      {
        if ( bData == null )
        {
          Debug.Log( "Fehler: GR.Memory.ByteBuffer, trying to append null" );
          return;
        }
        if ( m_Data == null )
        {
          m_Data = new byte[bData.Length];

          bData.CopyTo( m_Data, 0 );
          m_UsedBytes = (UInt32)bData.Length;
        }
        else
        {
          if ( m_UsedBytes + bData.Length <= m_Data.Length )
          {
            bData.CopyTo( m_Data, m_UsedBytes );
            m_UsedBytes += (UInt32)bData.Length;
          }
          else
          {
            byte[] bTemp = new byte[m_UsedBytes + bData.Length];

            Array.Copy( m_Data, 0, bTemp, 0, m_UsedBytes );
            //m_Data.CopyTo( bTemp, 0 );
            bData.CopyTo( bTemp, m_UsedBytes );

            m_Data = bTemp;
            m_UsedBytes = (UInt32)m_Data.Length;
          }
        }
      }

      public void Append( byte[] bData, int iStartIndex, int iLength )
      {
        if ( bData == null )
        {
          Debug.Log( "Fehler: GR.Memory.ByteBuffer, trying to append null" );
          return;
        }
        if ( ( iStartIndex >= bData.Length )
        ||   ( iStartIndex + iLength > bData.Length ) )
        {
          Debug.Log( "Fehler: GR.Memory.ByteBuffer trying to append Array out of bounds" );
          return;
        }
        if ( m_Data == null )
        {
          m_Data = new byte[iLength];

          Array.Copy( bData, iStartIndex, m_Data, 0, iLength );
          m_UsedBytes = (UInt32)iLength;
        }
        else
        {
          if ( m_UsedBytes + iLength <= m_Data.Length )
          {
            Array.Copy( bData, iStartIndex, m_Data, m_UsedBytes, iLength );
            m_UsedBytes += (UInt32)iLength;
          }
          else
          {
            byte[] bTemp = new byte[m_Data.Length + iLength];

            m_Data.CopyTo( bTemp, 0 );
            Array.Copy( bData, iStartIndex, bTemp, m_Data.Length, iLength );
            m_Data = bTemp;
            m_UsedBytes = (UInt32)m_Data.Length;
          }
        }
      }

      public bool TruncateAt( UInt32 TruncateIndex )
      {
        if ( m_Data == null )
        {
          return false;
        }
        if ( m_UsedBytes < TruncateIndex )
        {
          return false;
        }
        if ( m_UsedBytes == TruncateIndex )
        {
          Clear();
          return true;
        }
        m_UsedBytes = TruncateIndex;
        return true;
      }

      public bool Truncate( UInt32 NumberOfBytesToTruncate )
      {
        if ( m_Data == null )
        {
          return false;
        }
        if ( NumberOfBytesToTruncate >= m_UsedBytes )
        {
          Clear();
          return true;
        }
        m_UsedBytes -= NumberOfBytesToTruncate;
        return true;
      }

      public bool TruncateFront( int iBytesToTruncate )
      {
        if ( m_Data == null )
        {
          return false;
        }
        if ( m_Data.Length <= iBytesToTruncate )
        {
          m_Data = null;
          return true;
        }

        byte[] bTemp = new byte[m_Data.Length - iBytesToTruncate];
            
        for ( int i = iBytesToTruncate; i < m_Data.Length; ++i )
        {
          bTemp.SetValue( m_Data[i], i - iBytesToTruncate );
        }
        m_Data      = bTemp;
        m_UsedBytes = (UInt32)m_Data.Length;

        return true;
      }

      public bool Empty()
      {
        if ( m_Data == null )
        {
          return true;
        }
        return ( m_UsedBytes == 0 );
      }

      public void Clear()
      {
        m_Data = EmptyByteArray;
        m_UsedBytes = 0;
      }

      public string ToAsciiString()
      {
        if ( m_Data == null )
        {
          return "";
        }

        string    strDaten = System.Text.ASCIIEncoding.UTF8.GetString( m_Data, 0, (int)m_UsedBytes );
        /*
        string    strDaten = "";

        for ( int i = 0; i < m_UsedBytes; i++ )
			  {
				  strDaten += (char)m_Data[i];
          //strDaten += m_Data[i].ToString( System.Text.ASCIIEncoding.UTF8.GetString.Environment.Cu;
			  }
         */
			  return strDaten;

      }



      public override string ToString()
      {
        return ArrayToHexString( m_Data, 0, (int)m_UsedBytes );
      }



      public string ToString( UInt32 iStartIndex )
      {
        if ( m_Data == null )
        {
          return "";
        }
        if ( iStartIndex >= m_UsedBytes )
        {
          return "";
        }
        UInt32 iAnzahl = m_UsedBytes - iStartIndex;

        return ArrayToHexString( m_Data, (int)iStartIndex, (int)iAnzahl );
      }

      public string ToString( int iStartIndex, int iAnzahl )
      {
        if ( m_Data == null )
        {
          return "";
        }
        if ( iStartIndex >= m_UsedBytes )
        {
          return "";
        }
        if ( iStartIndex + iAnzahl > m_UsedBytes )
        {
          return "";
        }

        return ArrayToHexString( m_Data, iStartIndex, iAnzahl );
      }



      private static string ArrayToHexString( byte[] dataArray, int startIndex, int byteCount )
      {
        if ( dataArray == null )
        {
          return string.Empty;
        }

        var sb = new System.Text.StringBuilder( byteCount * 2 );

        byte dataByte;
        int stopIndex = startIndex + byteCount;
        const string HexChars = "0123456789ABCDEF";
        for ( int i = startIndex; i < stopIndex; i++ )
        {
          dataByte = dataArray[i];
          sb.Append( HexChars[(int)(dataByte >> 4)] );
          sb.Append( HexChars[(int)(dataByte & 0xF)] );
        }

        return sb.ToString();
      }

      public UInt32 Length
      {
        get
        {
          if ( m_Data == null )
          {
            return 0;
          }
          return m_UsedBytes;
        }
      }

      public string StringAt( int iIndex )
      {
        if ( ( iIndex < 0 )
        ||   ( iIndex >= Length ) )
        {
          return "";
        }

        int iLength = (int)UInt32At( iIndex );
        iIndex += 4;
        if ( iIndex + iLength > Length )
        {
          return "";
        }

        string      strResult = "";

        for ( int i = 0; i < iLength; ++i )
        {
          char      cChar = (char)ByteAt( iIndex + i );

          strResult += cChar;
        }

        return strResult;
      }

      public System.UInt16 UInt16At( int iIndex )
      {
        if ( ( iIndex < 0 )
        || ( iIndex + 1 >= Length ) )
        {
          return 0;
        }
        System.UInt16   Value =   (System.UInt16)( ( ByteAt( iIndex + 1 ) << 8 )
                                                 + ByteAt( iIndex ) );
        return Value;
      }

      public System.UInt16 UInt16NetworkOrderAt( int iIndex )
      {
        if ( ( iIndex < 0 )
        || ( iIndex + 1 >= Length ) )
        {
          return 0;
        }
        System.UInt16 Value = (System.UInt16)( ( ByteAt( iIndex ) << 8 )
                                                 + ByteAt( iIndex + 1 ) );
        return Value;
      }

      public System.UInt32 UInt32At( int iIndex )
      {
        if ( ( iIndex < 0 )
        ||   ( iIndex + 3 >= Length ) )
        {
          return 0;
        }
        System.UInt32   dwValue = ( (System.UInt32)ByteAt( iIndex + 3 ) << 24 )
                                + ( (System.UInt32)ByteAt( iIndex + 2 ) << 16 )
                                + ( (System.UInt32)ByteAt( iIndex + 1 ) << 8 )
                                + ( (System.UInt32)ByteAt( iIndex ) );
        return dwValue;
      }

      public byte ByteAt( int iIndex )
      {
        if ( m_Data == null )
        {
          return 0;
        }
        if ( ( iIndex < 0 )
        ||   ( iIndex >= Length ) )
        {
          return 0;
        }
        return m_Data[iIndex];
      }

      public void FromString( string strData )
      {
        Clear();
        for ( int i = 0; i < strData.Length; ++i )
        {
          AppendU8( System.Convert.ToByte( strData[i] ) );
        }
      }

      public bool FromHexString( string strHexData )
      {
        Clear();
        return AppendHex( strHexData );
      }

      public void Reserve( int BytesToReserve )
      {
        if ( ( m_Data != null )
        &&   ( m_Data.Length >= BytesToReserve ) )
        {
          return;
        }
        if ( m_Data == null )
        {
          m_Data = new byte[BytesToReserve];
          return;
        }
        byte[] bTemp = new byte[BytesToReserve];

        m_Data.CopyTo( bTemp, 0 );
        m_Data = bTemp;
      }

      public void Resize( UInt32 iSize )
      {
        if ( m_Data == null )
        {
          m_Data = new byte[iSize];
        }
        else
        {
          if ( m_Data.Length >= iSize )
          {
            m_UsedBytes = iSize;
            return;
          }
          byte[] bTemp = new byte[iSize];

          m_Data.CopyTo( bTemp, 0 );
          m_Data = bTemp;
        }
        m_UsedBytes = iSize;
      }



      public GR.IO.MemoryReader MemoryReader()
      {
        return new GR.IO.MemoryReader( this );
      }



      public System.IO.MemoryStream MemoryStream()
      {
        return new System.IO.MemoryStream( Data() );
      }

      public int Find( byte Value )
      {
        return Find( Value, 0 );
      }

      public int Find( byte Value, int Offset )
      {
        if ( m_Data == null )
        {
          return -1;
        }
        if ( Offset >= Length )
        {
          return -1;
        }
        for ( int i = Offset;  i < Length; ++i )
        {
          if ( m_Data[i] == Value )
          {
            return i;
          }
        }
        return -1;
      }

      public int PackedStringLength( string strText )
      {
        int   iBytes = strText.Length * 2;

        int   iLength = strText.Length * 2;

        while ( iLength > 127 )
        {
          iLength >>= 7;
          ++iBytes;
        }
        ++iBytes;

        return iBytes;
      }



      public bool CopyTo( GR.Memory.ByteBuffer TargetBuffer )
      {
        return CopyTo( TargetBuffer, 0, (int)Length, 0 );
      }



      public bool CopyTo( GR.Memory.ByteBuffer TargetBuffer, int Offset, int NumBytes )
      {
        return CopyTo( TargetBuffer, Offset, NumBytes, 0 );
      }



      public bool CopyTo( GR.Memory.ByteBuffer TargetBuffer, int Offset, int NumBytes, int TargetOffset )
      {
        if ( ( Offset < 0 )
        ||   ( Offset + NumBytes > Length )
        ||   ( TargetBuffer == null )
        ||   ( TargetOffset < 0 )
        ||   ( TargetOffset + NumBytes > TargetBuffer.Length ) )
        {
          return false;
        }
        for ( int i = 0; i < NumBytes; ++i )
        {
          TargetBuffer.SetU8At( TargetOffset + i, ByteAt( Offset + i ) );
        }
        return true;
      }



      public IntPtr PinData()
      {
        if ( m_PinnedHandle != default( System.Runtime.InteropServices.GCHandle ) )
        {
          return m_PinnedHandle.AddrOfPinnedObject();
        }

        m_PinnedHandle = System.Runtime.InteropServices.GCHandle.Alloc( m_Data, System.Runtime.InteropServices.GCHandleType.Pinned );
        return m_PinnedHandle.AddrOfPinnedObject();
      }



      public void UnpinData()
      {
        if ( m_PinnedHandle == default( System.Runtime.InteropServices.GCHandle ) )
        {
          return;
        }
        m_PinnedHandle.Free();
        m_PinnedHandle = default( System.Runtime.InteropServices.GCHandle );
      }

	  }
  }
}
