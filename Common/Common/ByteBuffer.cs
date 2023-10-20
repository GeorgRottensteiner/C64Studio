using System;



namespace GR
{
  namespace Memory
  {
	  /// <summary>
	  /// Zusammenfassung für ByteBuffer.
	  /// </summary>
	  public class ByteBuffer : IComparable<ByteBuffer> 
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



      public ByteBuffer( byte[] Data )
      {
        Append( Data );
      }



		  public ByteBuffer( string HexData )
		  {
        FromHexString( HexData );
		  }



      public static ByteBuffer operator +( ByteBuffer BB1, ByteBuffer BB2 )
      {
        ByteBuffer newBuffer = new ByteBuffer( BB1.Data() );

        newBuffer.Append( BB2 );

        return newBuffer;
      }



      public static bool operator ==( ByteBuffer BB1, ByteBuffer BB2 )
      {
        if ( object.ReferenceEquals( BB1, null ) )
        {
          return object.ReferenceEquals( BB2, null );
        }
        if ( object.ReferenceEquals( BB2, null ) )
        {
          return false;
        }
        return ( BB1.Compare( BB2 ) == 0 );
      }



      public override bool Equals( object obj )
      {
        return this == (ByteBuffer)obj;
      }



      public override int GetHashCode()
      {
        return base.GetHashCode();
      }



      public static bool operator !=( ByteBuffer BB1, ByteBuffer BB2 )
      {
        if ( object.ReferenceEquals( BB1, null ) )
        {
          return !object.ReferenceEquals( BB2, null );
        }
        if ( object.ReferenceEquals( BB2, null ) )
        {
          return true;
        }
        return ( BB1.Compare( BB2 ) != 0 );
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



      public GR.Memory.ByteBuffer SubBuffer( int StartIndex )
      {
        if ( StartIndex >= Length )
        {
          return new GR.Memory.ByteBuffer();
        }
        return new ByteBuffer( Data( StartIndex, (int)Length - StartIndex ) );
      }



      public GR.Memory.ByteBuffer SubBuffer( int StartIndex, int NumBytes )
      {
        return new GR.Memory.ByteBuffer( Data( StartIndex, NumBytes ) );
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



      public byte[] Data( int StartIndex, int NumBytes )
      {
        if ( ( StartIndex < 0 )
        ||   ( StartIndex + NumBytes > Length ) )
        {
          return EmptyByteArray;
        }
        byte[] result = new byte[NumBytes];

        Array.Copy( m_Data, StartIndex, result, 0, NumBytes );
        return result;
      }



      public void Append( ByteBuffer Other )
      {
        if ( Other != null )
        {
          Append( Other.Data() );
        }
      }



      public void AppendRepeated( byte ByteValue, int Count )
      {
        for ( int i = 0; i < Count; ++i )
        {
          AppendU8( ByteValue );
        }
      }



      public bool AppendHex( string HexData )
      {
        if ( ( HexData.Length == 0 )
        ||   ( ( HexData.Length % 2 ) == 1 ) )
        {
          return false;
        }

        int   curPos = (int)Length;
        Resize( (uint)( Length + HexData.Length / 2 ) );
        for ( int i = 0; i < HexData.Length; i += 2 )
        {
          m_Data[curPos] = GR.Convert.ToU8( HexData.Substring( i, 2 ), 16 );
          ++curPos;
        }
        return true;
      }



      public void AppendU16( UInt16 WordValue )
      {
        AppendU8( (byte)( WordValue & 0xff ) );
        AppendU8( (byte)( WordValue >> 8 ) );
      }



      public void AppendU16NetworkOrder( UInt16 WordValue )
      {
        AppendU8( (byte)( WordValue >> 8 ) );
        AppendU8( (byte)( WordValue & 0xff ) );
      }



      public void AppendU24( UInt32 DWordValue )
      {
        AppendU8( (byte)( DWordValue & 0xff ) );
        AppendU8( (byte)( DWordValue >> 8 ) );
        AppendU8( (byte)( DWordValue >> 16 ) );

      }



      public void AppendU24NetworkOrder( UInt32 DWordValue )
      {
        AppendU8( (byte)( DWordValue >> 16 ) );
        AppendU8( (byte)( DWordValue >> 8 ) );
        AppendU8( (byte)( DWordValue & 0xff ) );
      }



      public void AppendU32( UInt32 DWordValue )
      {
        AppendU8( (byte)( DWordValue & 0xff ) );
        AppendU8( (byte)( DWordValue >> 8 ) );
        AppendU8( (byte)( DWordValue >> 16 ) );
        AppendU8( (byte)( DWordValue >> 24 ) );
        
      }

      public void AppendI32( Int32 DWordValue )
      {
        AppendU32( (UInt32)DWordValue );
      }



      public void AppendU32NetworkOrder( UInt32 DWordValue )
      {
        AppendU8( (byte)( DWordValue >> 24 ) );
        AppendU8( (byte)( DWordValue >> 16 ) );
        AppendU8( (byte)( DWordValue >> 8 ) );
        AppendU8( (byte)( DWordValue & 0xff ) );
      }



      public void AppendString( string TextData )
      {
        if ( TextData == null )
        {
          AppendU32( 0 );
          return;
        }

        AppendU32( (UInt32)TextData.Length );
        for ( int i = 0; i < TextData.Length; ++i )
        {
          AppendU8( (byte)TextData[i] );
        }
      }



      public void AppendU8( byte ByteValue )
      {
        if ( m_Data == null )
        {
          m_Data = new byte[1];

          m_Data.SetValue( ByteValue, 0 );
          m_UsedBytes = 1;
        }
        else
        {
          if ( m_UsedBytes < m_Data.Length )
          {
            m_Data[m_UsedBytes] = ByteValue;
            ++m_UsedBytes;
          }
          else
          {
            byte[] bTemp = new byte[m_Data.Length + 1];

            m_Data.CopyTo( bTemp, 0 );
            bTemp.SetValue( ByteValue, m_Data.Length );

            m_Data = bTemp;
            m_UsedBytes = (UInt32)m_Data.Length;
          }
        }
      }



      public void AppendU8Front( byte ByteValue )
      {
        if ( m_Data == null )
        {
          m_Data = new byte[1];

          m_Data.SetValue( ByteValue, 0 );
          m_UsedBytes = 1;
        }
        else
        {
          byte[] bTemp = new byte[m_Data.Length + 1];
             
          m_Data.CopyTo( bTemp, 1 );
          bTemp[0] = ByteValue;
          m_Data = bTemp;
          m_UsedBytes = (UInt32)m_Data.Length;
        }
      }



      public void SetU32At( int Index, System.UInt32 DWordValue )
      {
        if ( ( Index < 0 )
        ||   ( Index + 3 >= Length ) )
        {
          return;
        }
        m_Data[Index]     = (byte)( DWordValue & 0xff );
        m_Data[Index + 1] = (byte)( ( DWordValue >> 8 ) & 0xff );
        m_Data[Index + 2] = (byte)( ( DWordValue >> 16 ) & 0xff );
        m_Data[Index + 3] = (byte)( ( DWordValue >> 24 ) & 0xff );
      }



      public void SetU32NetworkOrderAt( int Index, System.UInt32 DWordValue )
      {
        if ( ( Index < 0 )
        ||   ( Index + 3 >= Length ) )
        {
          return;
        }
        m_Data[Index + 0] = (byte)( ( DWordValue >> 24 ) & 0xff );
        m_Data[Index + 1] = (byte)( ( DWordValue >> 16 ) & 0xff );
        m_Data[Index + 2] = (byte)( ( DWordValue >> 8 ) & 0xff );
        m_Data[Index + 3] = (byte)  ( DWordValue & 0xff );
      }



      public void SetU24At( int Index, System.UInt32 DWordValue )
      {
        if ( ( Index < 0 )
        ||   ( Index + 2 >= Length ) )
        {
          return;
        }
        m_Data[Index]     = (byte)( DWordValue & 0xff );
        m_Data[Index + 1] = (byte)( ( DWordValue >> 8 ) & 0xff );
        m_Data[Index + 2] = (byte)( ( DWordValue >> 16 ) & 0xff );
      }



      public void SetU24NetworkOrderAt( int Index, System.UInt32 DWordValue )
      {
        if ( ( Index < 0 )
        ||   ( Index + 2 >= Length ) )
        {
          return;
        }
        m_Data[Index + 0] = (byte)( ( DWordValue >> 16 ) & 0xff );
        m_Data[Index + 1] = (byte)( ( DWordValue >> 8 ) & 0xff );
        m_Data[Index + 2] = (byte)  ( DWordValue & 0xff );
      }



      public void SetU16At( int Index, System.UInt16 WordValue )
      {
        if ( ( Index < 0 )
        ||   ( Index + 1 >= Length ) )
        {
          return;
        }
        m_Data[Index]     = (byte)( WordValue & 0xff );
        m_Data[Index + 1] = (byte)( WordValue >> 8 );
      }



      public void SetU16NetworkOrderAt( int Index, System.UInt16 WordValue )
      {
        if ( ( Index < 0 )
        ||   ( Index + 1 >= Length ) )
        {
          return;
        }
        m_Data[Index]     = (byte)( WordValue >> 8 );
        m_Data[Index + 1] = (byte)( WordValue & 0xff );
      }



      public void SetU8At( int Index, System.Byte Byte )
      {
        if ( ( Index < 0 )
        ||   ( Index >= Length ) )
        {
          return;
        }
        m_Data[Index] = Byte;
      }



      public void AppendF32( float Value )
      {
        Append( System.BitConverter.GetBytes( Value ) );
      }



      public void Append( byte[] Array )
      {
        if ( Array == null )
        {
          Debug.Log( "Fehler: GR.Memory.ByteBuffer, trying to append null" );
          return;
        }
        if ( m_Data == null )
        {
          m_Data = new byte[Array.Length];

          Array.CopyTo( m_Data, 0 );
          m_UsedBytes = (UInt32)Array.Length;
        }
        else
        {
          if ( m_UsedBytes + Array.Length <= m_Data.Length )
          {
            Array.CopyTo( m_Data, m_UsedBytes );
            m_UsedBytes += (UInt32)Array.Length;
          }
          else
          {
            int     newSize = m_Data.Length * 2;
            if ( newSize < m_UsedBytes + Array.Length )
            {
              newSize = (int)( m_UsedBytes + Array.Length );
            }

            Reserve( newSize );
            Append( Array );
            /*
            byte[] bTemp = new byte[m_UsedBytes + Array.Length];

            System.Array.Copy( m_Data, 0, bTemp, 0, m_UsedBytes );
            //m_Data.CopyTo( bTemp, 0 );
            Array.CopyTo( bTemp, m_UsedBytes );

            m_Data = bTemp;
            m_UsedBytes = (UInt32)m_Data.Length;*/
          }
        }
      }



      public void Append( byte[] Array, int StartIndex, int NumBytes )
      {
        if ( Array == null )
        {
          Debug.Log( "Fehler: GR.Memory.ByteBuffer, trying to append null" );
          return;
        }
        if ( ( StartIndex >= Array.Length )
        ||   ( StartIndex + NumBytes > Array.Length ) )
        {
          Debug.Log( "Fehler: GR.Memory.ByteBuffer trying to append Array out of bounds" );
          return;
        }
        if ( m_Data == null )
        {
          m_Data = new byte[NumBytes];

          System.Array.Copy( Array, StartIndex, m_Data, 0, NumBytes );
          m_UsedBytes = (UInt32)NumBytes;
        }
        else
        {
          if ( m_UsedBytes + NumBytes <= m_Data.Length )
          {
            System.Array.Copy( Array, StartIndex, m_Data, m_UsedBytes, NumBytes );
            m_UsedBytes += (UInt32)NumBytes;
          }
          else
          {
            byte[] bTemp = new byte[m_Data.Length + NumBytes];

            m_Data.CopyTo( bTemp, 0 );
            System.Array.Copy( Array, StartIndex, bTemp, m_Data.Length, NumBytes );
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



      public bool TruncateFront( int NumberOfBytesToTruncate )
      {
        if ( m_Data == null )
        {
          return false;
        }
        if ( m_Data.Length <= NumberOfBytesToTruncate )
        {
          m_Data = null;
          return true;
        }

        byte[] bTemp = new byte[m_Data.Length - NumberOfBytesToTruncate];
            
        for ( int i = NumberOfBytesToTruncate; i < m_Data.Length; ++i )
        {
          bTemp.SetValue( m_Data[i], i - NumberOfBytesToTruncate );
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

        return System.Text.ASCIIEncoding.UTF8.GetString( m_Data, 0, (int)m_UsedBytes );
      }



      public override string ToString()
      {
        return ArrayToHexString( m_Data, 0, (int)m_UsedBytes );
      }



      public string ToString( UInt32 StartIndex )
      {
        if ( m_Data == null )
        {
          return "";
        }
        if ( StartIndex >= m_UsedBytes )
        {
          return "";
        }
        UInt32 iAnzahl = m_UsedBytes - StartIndex;

        return ArrayToHexString( m_Data, (int)StartIndex, (int)iAnzahl );
      }



      public string ToString( int StartIndex, int NumBytes )
      {
        if ( m_Data == null )
        {
          return "";
        }
        if ( StartIndex >= m_UsedBytes )
        {
          return "";
        }
        if ( StartIndex + NumBytes > m_UsedBytes )
        {
          return "";
        }

        return ArrayToHexString( m_Data, StartIndex, NumBytes );
      }



      private static string ArrayToHexString( byte[] Array, int StartIndex, int NumBytes )
      {
        if ( Array == null )
        {
          return string.Empty;
        }
        if ( ( StartIndex < 0 )
        ||   ( StartIndex >= Array.Length )
        ||   ( NumBytes < 0 )
        ||   ( StartIndex + NumBytes > Array.Length ) )
        {
          return string.Empty;
        }

        var sb = new System.Text.StringBuilder( NumBytes * 2 );

        byte dataByte;
        int stopIndex = StartIndex + NumBytes;
        const string HexChars = "0123456789ABCDEF";
        for ( int i = StartIndex; i < stopIndex; i++ )
        {
          dataByte = Array[i];
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



      public string StringAt( int Index )
      {
        if ( ( Index < 0 )
        ||   ( Index >= Length ) )
        {
          return "";
        }

        int length = (int)UInt32At( Index );
        Index += 4;
        if ( Index + length > Length )
        {
          return "";
        }

        string      result = "";

        for ( int i = 0; i < length; ++i )
        {
          char      cChar = (char)ByteAt( Index + i );

          result += cChar;
        }

        return result;
      }



      public System.UInt16 UInt16At( int Index )
      {
        if ( ( Index < 0 )
        ||   ( Index + 1 >= Length ) )
        {
          return 0;
        }
        System.UInt16   Value =   (System.UInt16)( ( ByteAt( Index + 1 ) << 8 )
                                                 + ByteAt( Index ) );
        return Value;
      }



      public System.UInt16 UInt16NetworkOrderAt( int Index )
      {
        if ( ( Index < 0 )
        ||   ( Index + 1 >= Length ) )
        {
          return 0;
        }
        System.UInt16 Value = (System.UInt16)( ( ByteAt( Index ) << 8 )
                                                 + ByteAt( Index + 1 ) );
        return Value;
      }



      public System.UInt32 UInt24At( int Index )
      {
        if ( ( Index < 0 )
        ||   ( Index + 2 >= Length ) )
        {
          return 0;
        }
        System.UInt32   dwValue = ( (System.UInt32)ByteAt( Index + 2 ) << 16 )
                                + ( (System.UInt32)ByteAt( Index + 1 ) << 8 )
                                + ( (System.UInt32)ByteAt( Index ) );
        return dwValue;
      }



      public System.UInt32 UInt24NetworkOrderAt( int Index )
      {
        if ( ( Index < 0 )
        ||   ( Index + 2 >= Length ) )
        {
          return 0;
        }
        System.UInt32   dwValue = ( (System.UInt32)ByteAt( Index + 0 ) << 24 )
                                + ( (System.UInt32)ByteAt( Index + 1 ) << 16 )
                                + ( (System.UInt32)ByteAt( Index + 2 ) << 8 );
        return dwValue;
      }



      public System.UInt32 UInt32At( int Index )
      {
        if ( ( Index < 0 )
        ||   ( Index + 3 >= Length ) )
        {
          return 0;
        }
        System.UInt32   dwValue = ( (System.UInt32)ByteAt( Index + 3 ) << 24 )
                                + ( (System.UInt32)ByteAt( Index + 2 ) << 16 )
                                + ( (System.UInt32)ByteAt( Index + 1 ) << 8 )
                                + ( (System.UInt32)ByteAt( Index ) );
        return dwValue;
      }



      public System.UInt32 UInt32NetworkOrderAt( int Index )
      {
        if ( ( Index < 0 )
        ||   ( Index + 3 >= Length ) )
        {
          return 0;
        }
        System.UInt32   dwValue = ( (System.UInt32)ByteAt( Index + 0 ) << 24 )
                                + ( (System.UInt32)ByteAt( Index + 1 ) << 16 )
                                + ( (System.UInt32)ByteAt( Index + 2 ) << 8 )
                                + ( (System.UInt32)ByteAt( Index + 3 ) );
        return dwValue;
      }



      public byte ByteAt( int Index )
      {
        if ( m_Data == null )
        {
          return 0;
        }
        if ( ( Index < 0 )
        ||   ( Index >= Length ) )
        {
          return 0;
        }
        return m_Data[Index];
      }



      public void FromString( string Text )
      {
        Clear();
        for ( int i = 0; i < Text.Length; ++i )
        {
          AppendU8( System.Convert.ToByte( Text[i] ) );
        }
      }



      public bool FromHexString( string HexData )
      {
        Clear();
        return AppendHex( HexData );
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



      public void Resize( UInt32 Size )
      {
        if ( m_Data == null )
        {
          m_Data = new byte[Size];
        }
        else
        {
          if ( m_Data.Length >= Size )
          {
            m_UsedBytes = Size;
            return;
          }
          byte[] bTemp = new byte[Size];

          m_Data.CopyTo( bTemp, 0 );
          m_Data = bTemp;
        }
        m_UsedBytes = Size;
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



      public int Find( byte Value, int StartIndex )
      {
        if ( m_Data == null )
        {
          return -1;
        }
        if ( StartIndex >= Length )
        {
          return -1;
        }
        for ( int i = StartIndex;  i < Length; ++i )
        {
          if ( m_Data[i] == Value )
          {
            return i;
          }
        }
        return -1;
      }



      public int Find( ByteBuffer Key )
      {
        return Find( Key, 0 );
      }



      public int Find( ByteBuffer Key, int StartIndex )
      {
        if ( m_Data == null )
        {
          return -1;
        }
        if ( StartIndex + Key.Length > Length )
        {
          return -1;
        }
        for ( int i = StartIndex; i < Length - Key.Length; ++i )
        {
          bool  foundMatch = true;
          for ( int j = 0; j < Key.Length; ++j )
          {
            if ( m_Data[i + j] != Key.m_Data[j] )
            {
              foundMatch = false;
              break;
            }
          }
          if ( foundMatch )
          {
            return i;
          }
        }
        return -1;
      }



      public int PackedStringLength( string Text )
      {
        int   numBytes = Text.Length * 2;

        int   length = Text.Length * 2;

        while ( length > 127 )
        {
          length >>= 7;
          ++numBytes;
        }
        ++numBytes;

        return numBytes;
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



      public int CompareTo( ByteBuffer OtherBuffer )
      {
        return Compare( OtherBuffer );
      }



    }
  }
}
