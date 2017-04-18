using System;

namespace GR
{
  namespace IO
  {
	  public static class File
	  {
      public static GR.Memory.ByteBuffer ReadAllBytes( string Filename )
      {
        byte[]    byteData = null;
        try
        {
          byteData = System.IO.File.ReadAllBytes( Filename );
        }
        catch ( Exception )
        {
          return null;
        }
        return new GR.Memory.ByteBuffer( byteData );
      }



      public static string ReadAllText( string Filename )
      {
        string text = "";
        try
        {
          text = System.IO.File.ReadAllText( Filename );
        }
        catch ( Exception )
        {
        }
        return text;
      }



      public static bool WriteAllBytes( string Filename, GR.Memory.ByteBuffer Data )
      {
        try
        {
          System.IO.File.WriteAllBytes( Filename, Data.Data() );
        }
        catch ( Exception )
        {
          return false;
        }
        return true;
      }



      public static bool WriteAllText( string Filename, string Text )
      {
        try
        {
          System.IO.File.WriteAllText( Filename, Text );
        }
        catch ( Exception )
        {
          return false;
        }
        return true;
      }
    }


  }
}