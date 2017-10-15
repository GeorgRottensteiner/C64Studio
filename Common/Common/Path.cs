using System;

namespace GR
{
  public static class Path
  {
    public static string PotentialPathSeparators = "\\/";



    public static bool IsSeparator( char Char )
    {
      return IsSeparator( Char, PotentialPathSeparators );
    }

    public static bool IsSeparator( char Char, string Separator )
    {
      return ( Separator.IndexOf( Char ) != -1 );
    }

    public static string AddBackslash( string Path )
    {
      return AddBackslash( Path, PotentialPathSeparators );
    }

    public static string AddBackslash( string Path, string Separators )
    {

      if ( String.IsNullOrEmpty( Path ) )
      {
        return Separators.Substring( 0, 1 );
      }

      if ( !IsSeparator( Path[Path.Length - 1], Separators ) )
      {
        return Path + Separators[0];
      }
      return Path;
    }

    public static string RemoveBackslash( string Path )
    {
      return RemoveBackslash( Path, PotentialPathSeparators );
    }

    public static string RemoveBackslash( string Path, string Separators )
    {
      if ( String.IsNullOrEmpty( Path ) )
      {
        return Path;
      }

      if ( IsSeparator( Path[Path.Length - 1], Separators ) )
      {
        return Path.Substring( 0, Path.Length - 1 );
      }
      return Path;
    }



    public static string Append( string Path, string SecondPath )
    {
      return Append( Path, SecondPath, PotentialPathSeparators );
    }



    public static string Append( string Path, string SecondPath, string Separators )
    {
      string       result = Path;

      if ( ( !String.IsNullOrEmpty( result ) )
      &&   ( !IsSeparator( result[result.Length - 1], Separators ) ) )
      {
        result += Separators[0];
      }
      if ( ( !String.IsNullOrEmpty( SecondPath ) )
      &&   ( IsSeparator( SecondPath[0], Separators ) ) )
      {
        result += SecondPath.Substring( 1 );
      }
      else
      {
        result += SecondPath;
      }

      return result;
    }



    public static string Normalize( string Path, bool IsDir )
    {
      return Normalize( Path, IsDir, PotentialPathSeparators );
    }



    public static string Normalize( string Path, bool IsDir, string Separators )
    {
      string result = "";

      int pos = Path.Length - 1;
      int lastSeparatorPos = -1;

      int skipNextDirCount = 0;

      if ( Path.Length == 0 )
      {
        return Path;
      }

      if ( !IsDir )
      {
        while ( true )
        {
          char letter = Path[pos];
          --pos;
          if ( IsSeparator( letter, Separators ) )
          {
            lastSeparatorPos = pos + 1;
            result = Path.Substring( lastSeparatorPos + 1 );
            break;
          }
        }
        if ( lastSeparatorPos == -1 )
        {
          return Path;
        }
      }

      while ( true )
      {
        char letter = Path[pos];

        if ( IsSeparator( letter, Separators ) )
        {
          // auf .. prüfen
          string subPath;
          if ( lastSeparatorPos == -1 )
          {
            // bis zum Ende des Strings
            subPath = Path.Substring( pos + 1 );
          }
          else
          {
            subPath = Path.Substring( pos + 1, lastSeparatorPos - pos - 1 );
          }
          if ( ( subPath == "." )
          ||   ( subPath.Length == 0 ) )
          {
            // komplett ignorieren
          }
          else if ( subPath == ".." )
          {
            ++skipNextDirCount;
          }
          else
          {
            if ( skipNextDirCount == 0 )
            {
              if ( result.Length == 0 )
              {
                result = subPath;
              }
              else
              {
                result = Append( subPath, result, Separators );
              }
            }
            else
            {
              --skipNextDirCount;
            }
          }
          lastSeparatorPos = pos;
        }
        if ( pos == 0 )
        {
          break;
        }
        --pos;
      }
      if ( lastSeparatorPos == -1 )
      {
        result = Path;
      }
      else if ( skipNextDirCount == 0 )
      {
        result = Append( Path.Substring( 0, lastSeparatorPos ), result, Separators );
      }
      return result;
    }



    public static int FindNextSeparator( string Path, int Offset )
    {
      return FindNextSeparator( Path, Offset, PotentialPathSeparators );
    }



    public static int FindNextSeparator( string Path, int Offset, string Separator )
    {
      while ( Offset < Path.Length )
      {
        if ( IsSeparator( Path[Offset], Separator ) )
        {
          return Offset;
        }
        ++Offset;
      }
      return -1;
    }





    public static string RemoveFileSpec( string Path )
    {
      return RemoveFileSpec( Path, PotentialPathSeparators );
    }



    public static string RemoveFileSpec( string Path, string Separators )
    {
      string            result = Path;

      int               pos = result.Length;

      while ( pos > 0 )
      {
        --pos;
        if ( IsSeparator( result[pos], Separators ) )
        {
          return result.Substring( 0, pos );
        }
      }
      return result;
    }



    public static string CommonPrefix( string Path1, string Path2 )
    {
      return CommonPrefix( Path1, Path2, PotentialPathSeparators );
    }



    public static string CommonPrefix( string Path1, string Path2, string Separator )
    {
      if ( ( String.IsNullOrEmpty( Path1 ) )
      ||   ( String.IsNullOrEmpty( Path2 ) ) )
      {
        return "";
      }
      if ( Path1.ToUpper() == Path2.ToUpper() )
      {
        return Path1;
      }

      string     strResult = "";

      int   iLength = Path1.Length;
      if ( Path2.Length > iLength )
      {
        iLength = Path2.Length;
      }

      int    BackslashPos1 = 0;
      int    BackslashPos2 = 0;
      while ( true )
      {
        if ( ( BackslashPos1 >= Path1.Length )
        &&   ( BackslashPos2 >= Path2.Length ) )
        {
          break;
        }
        int NewBackslashPos1 = FindNextSeparator( Path1, BackslashPos1, Separator );
        int NewBackslashPos2 = FindNextSeparator( Path2, BackslashPos2, Separator );

        if ( NewBackslashPos1 == -1 )
        {
          NewBackslashPos1 = Path1.Length;
        }
        if ( NewBackslashPos2 == -1 )
        {
          NewBackslashPos2 = Path2.Length;
        }
        if ( NewBackslashPos1 != NewBackslashPos2 )
        {
          // ab hier gibt es Unterschiede
          break;
        }
        bool            DifferenceFound = false;
        string          SubResult = "";
        for ( int i = BackslashPos1; i < NewBackslashPos1; ++i )
        {
          if ( Char.ToUpper( Path1[i] ) != Char.ToUpper( Path2[i] ) )
          {
            if ( ( IsSeparator( Path1[i], Separator ) )
            &&   ( IsSeparator( Path2[i], Separator ) ) )
            {
            }
            else
            {
              DifferenceFound = true;
              break;
            }
          }
          SubResult += Path1[i];
        }
        if ( !DifferenceFound )
        {
          strResult += SubResult;
          strResult += Separator[0];
        }
        BackslashPos1 = NewBackslashPos1 + 1;
        BackslashPos2 = NewBackslashPos2 + 1;
      }

      if ( strResult.Length > 3 )
      {
        // bei mehr als nur Root soll kein Backslash dran sein
        strResult = RemoveBackslash( strResult, Separator );
      }
      return strResult;
    }



    public static string RelativePathTo( string From, bool FromIsDir, string To, bool ToIsDir )
    {
      return RelativePathTo( From, FromIsDir, To, ToIsDir, PotentialPathSeparators );
    }



    public static string RelativePathTo( string strFrom, bool bFromDir, string strTo, bool bToDir, string Separators )
    {
      string     strTempFrom = strFrom;
      string     strTempTo   = strTo;

      if ( ( String.IsNullOrEmpty( strFrom ) )
      ||   ( String.IsNullOrEmpty( strTo ) ) )
      {
        return strTo;
      }

      if ( !bFromDir )
      {
        strTempFrom = RemoveFileSpec( strTempFrom, Separators );
      }
      if ( !bToDir )
      {
        strTempTo = RemoveFileSpec( strTempTo, Separators );
      }

      strTempFrom = AddBackslash( strTempFrom );
      strTempTo = AddBackslash( strTempTo );


      if ( Char.ToUpper( strFrom[0] ) != Char.ToUpper( strTo[0] ) )
      {
        // unterschiedliches Hauptverzeichnis
        if ( ( bToDir )
        &&   ( !bFromDir ) )
        {
          // add file name to result
          return strFrom;
        }
        return strTo;
      }

      string      strCommon = CommonPrefix( strTempFrom, strTempTo, Separators );
      string      strResult = "";

      int         iPos =  strCommon.Length;

      while ( iPos < strTempFrom.Length )
      {
        if ( Separators.IndexOf( strTempFrom[iPos] ) == -1 )
        {
          int iPos2 = FindNextSeparator( strTempFrom, iPos + 1, Separators );
          if ( iPos2 != -1 )
          {
            strResult += "..";
            strResult += Separators[0];
          }
          else
          {
            strResult += "..";
            break;
          }
          iPos = iPos2;
        }

        ++iPos;
      }

      if ( strCommon.Length < strTo.Length )
      {
        strResult = Append( strResult, strTo.Substring( strCommon.Length ) );
      }

      if ( ( strResult.Length > 0 )
      &&   ( Separators.IndexOf( strResult[0] ) != -1 ) )
      {
        strResult = strResult.Substring( 1 );
      }
      strResult = RemoveBackslash( strResult, Separators );

      if ( ( bToDir )
      &&   ( !bFromDir ) )
      {
        // re-append filename
        strResult = Append( strResult, System.IO.Path.GetFileName( strFrom ) );
      }

      return strResult;
    }



    public static bool IsPathEqual( string Path1, string Path2 )
    {
      return IsPathEqual( Path1, Path2, PotentialPathSeparators );
    }



    public static bool IsPathEqual( string Path1, string Path2, string Separators )
    {
      if ( ( string.IsNullOrEmpty( Path1 ) )
      &&   ( string.IsNullOrEmpty( Path2 ) ) )
      {
        return true;
      }
      if ( ( string.IsNullOrEmpty( Path1 ) )
      ||   ( string.IsNullOrEmpty( Path2 ) ) )
      {
        return false;
      }
      if ( Path2.Length > Path1.Length )
      {
        return ( CommonPrefix( Path1, Path2, Separators ).Length == Path2.Length );
      }
      return ( CommonPrefix( Path1, Path2, Separators ).Length == Path1.Length );
    }



    public static bool IsSubPath( string ParentPath, string PathToCheck )
    {
      return IsSubPath( ParentPath, PathToCheck, PotentialPathSeparators );
    }



    public static bool IsSubPath( string ParentPath, string PathToCheck, string Separators )
    {
      // if parent is longer no check is necessary
      if ( ParentPath.Length > PathToCheck.Length )
      {
        return false;
      }
      string    normalizedParentPath = Normalize( ParentPath, false );
      string    normalizedCheckPath = Normalize( PathToCheck,false );

      return IsPathEqual( normalizedCheckPath.Substring( 0, normalizedParentPath.Length ), normalizedParentPath );
    }


    public static string RenameExtension( string OrigFilename, string NewExtension )
    {
      return System.IO.Path.GetFileNameWithoutExtension( OrigFilename ) + NewExtension;
    }



    public static string RenameFilenameWithoutExtension( string OrigFilename, string NewFilename )
    {
      return Append( System.IO.Path.GetDirectoryName( OrigFilename ), NewFilename + System.IO.Path.GetExtension( OrigFilename ) );
    }

  }
}
