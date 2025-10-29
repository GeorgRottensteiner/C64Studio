using System;

namespace GR
{
  public static class Path
  {
    public const string PotentialPathSeparators = "\\/";



    public static bool IsSeparator( char Char, string Separator = PotentialPathSeparators )
    {
      return ( Separator.IndexOf( Char ) != -1 );
    }



    public static string AddSeparator( string Path, string Separators = PotentialPathSeparators )
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



    public static string RemoveSeparator( string Path, string Separators = PotentialPathSeparators )
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



    public static string Append( string Path, string SecondPath, string Separators = PotentialPathSeparators )
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



    public static string Normalize( string Path, bool IsDir, string Separators = PotentialPathSeparators )
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
        while ( pos >= 0 )
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
        if ( pos == -1 )
        {
          // no more path left after removing the filename!
          return "";
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



    public static int FindNextSeparator( string Path, int Offset, string Separator = PotentialPathSeparators )
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



    public static string GetDirectoryName( string Path, string Separators = PotentialPathSeparators )
    {
      string    cleanEnd = RemoveSeparator( Path, Separators );

      int     prevPos = cleanEnd.LastIndexOfAny( Separators.ToCharArray() );
      if ( prevPos == -1 )
      {
        return "";
      }
      return cleanEnd.Substring( 0, prevPos );
    }



    public static string CommonPrefix( string PathArg1, string PathArg2, string Separators = PotentialPathSeparators )
    {
      if ( ( String.IsNullOrEmpty( PathArg1 ) )
      ||   ( String.IsNullOrEmpty( PathArg2 ) ) )
      {
        return "";
      }
      string    path1 = Normalize( PathArg1, false );
      string    path2 = Normalize( PathArg2, false );

      if ( path1.ToUpper() == path2.ToUpper() )
      {
        return RemoveSeparator( path1, Separators );
      }

      string     result = "";

      int   length = path1.Length;
      if ( path2.Length > length )
      {
        length = path2.Length;
      }

      int    backslashPos1 = 0;
      int    backslashPos2 = 0;
      while ( true )
      {
        if ( ( backslashPos1 >= path1.Length )
        &&   ( backslashPos2 >= path2.Length ) )
        {
          break;
        }
        int newBackslashPos1 = FindNextSeparator( path1, backslashPos1, Separators );
        int newBackslashPos2 = FindNextSeparator( path2, backslashPos2, Separators );

        if ( newBackslashPos1 == -1 )
        {
          newBackslashPos1 = path1.Length;
        }
        if ( newBackslashPos2 == -1 )
        {
          newBackslashPos2 = path2.Length;
        }
        if ( newBackslashPos1 != newBackslashPos2 )
        {
          // ab hier gibt es Unterschiede
          break;
        }
        bool            DifferenceFound = false;
        string          SubResult = "";
        for ( int i = backslashPos1; i < newBackslashPos1; ++i )
        {
          if ( Char.ToUpper( path1[i] ) != Char.ToUpper( path2[i] ) )
          {
            if ( ( IsSeparator( path1[i], Separators ) )
            &&   ( IsSeparator( path2[i], Separators ) ) )
            {
            }
            else
            {
              DifferenceFound = true;
              break;
            }
          }
          SubResult += path1[i];
        }
        if ( !DifferenceFound )
        {
          result += SubResult;
          result += Separators[0];
        }
        backslashPos1 = newBackslashPos1 + 1;
        backslashPos2 = newBackslashPos2 + 1;
      }

      if ( result.Length > 3 )
      {
        // bei mehr als nur Root soll kein Backslash dran sein
        result = RemoveSeparator( result, Separators );
      }
      return result;
    }



    public static string RelativePathTo( string From, bool FromIsDir, string To, bool ToIsDir, string Separators = PotentialPathSeparators )
    {
      string     tempFrom = From;
      string     tempTo   = To;

      if ( ( String.IsNullOrEmpty( From ) )
      ||   ( String.IsNullOrEmpty( To ) ) )
      {
        return To;
      }

      if ( !FromIsDir )
      {
        tempFrom = GetDirectoryName( tempFrom, Separators );
      }
      if ( !ToIsDir )
      {
        tempTo = GetDirectoryName( tempTo, Separators );
      }

      tempFrom = AddSeparator( tempFrom );
      tempTo = AddSeparator( tempTo );


      if ( Char.ToUpper( From[0] ) != Char.ToUpper( To[0] ) )
      {
        // unterschiedliches Hauptverzeichnis
        if ( ( ToIsDir )
        &&   ( !FromIsDir ) )
        {
          // add file name to result
          return From;
        }
        return To;
      }

      string      common = CommonPrefix( tempFrom, tempTo, Separators );
      string      result = "";

      int         pos =  common.Length;

      while ( pos < tempFrom.Length )
      {
        if ( !IsSeparator( tempFrom[pos], Separators ) )
        {
          int iPos2 = FindNextSeparator( tempFrom, pos + 1, Separators );
          if ( iPos2 != -1 )
          {
            result += "..";
            result += Separators[0];
          }
          else
          {
            result += "..";
            break;
          }
          pos = iPos2;
        }

        ++pos;
      }

      if ( common.Length < To.Length )
      {
        result = Append( result, To.Substring( common.Length ), Separators );
      }

      if ( ( result.Length > 0 )
      &&   ( IsSeparator( result[0], Separators ) ) )
      {
        result = result.Substring( 1 );
      }
      result = RemoveSeparator( result, Separators );

      if ( ( ToIsDir )
      &&   ( !FromIsDir ) )
      {
        // re-append filename
        result = Append( result, GetFileName( From, Separators ), Separators );
      }
      return result;
    }



    public static bool IsPathEqual( string Path1, string Path2, string Separators = PotentialPathSeparators )
    {
      string commonPrefix = CommonPrefix( Path1, Path2, Separators );
      if ( ( commonPrefix.Length == RemoveSeparator( Normalize( Path1, false, Separators ), Separators).Length )
      &&   ( commonPrefix.Length == RemoveSeparator( Normalize( Path2, false, Separators ), Separators ).Length ) )
      {
        return true;
      }
      return false;
    }



    public static bool IsSubPath( string ParentPath, string PathToCheck, string Separators = PotentialPathSeparators )
    {
      // if parent is longer no check is necessary
      if ( ParentPath.Length > PathToCheck.Length )
      {
        return false;
      }
      string    normalizedParentPath = Normalize( ParentPath, false, Separators );
      string    normalizedCheckPath = Normalize( PathToCheck, false, Separators );

      return IsPathEqual( normalizedCheckPath.Substring( 0, normalizedParentPath.Length ), normalizedParentPath, Separators );
    }



    /// <summary>
    /// renames the extension of a file while keeping directories and filename intact
    /// </summary>
    /// <param name="OrigFilename"></param>
    /// <param name="NewExtension">new extensions, must start with a '.'</param>
    /// <param name="Separators"></param>
    /// <returns></returns>
    public static string RenameExtension( string OrigFilename, string NewExtension, string Separators = PotentialPathSeparators )
    {
      if ( !NewExtension.StartsWith( "." ) )
      {
        NewExtension = "." + NewExtension;
      }
      return Append( GetDirectoryName( OrigFilename, PotentialPathSeparators ), GetFileNameWithoutExtension( OrigFilename, Separators ) + NewExtension );
    }



    /// <summary>
    /// renames the file without extension in a full path and returns the new full path
    /// </summary>
    /// <param name="FullPath"></param>
    /// <param name="NewFilename"></param>
    /// <returns>full path with renamed file</returns>
    public static string RenameFilenameWithoutExtension( string FullPath, string NewFilename, string Separators = PotentialPathSeparators )
    {
      return Append( GetDirectoryName( FullPath, Separators ), NewFilename + GetExtension( FullPath, Separators ), Separators );
    }



    public static string RenameFile( string OriginalFullPath, string NewFileName, string Separators = PotentialPathSeparators )
    {
      return Append( GetDirectoryName( OriginalFullPath, Separators ), NewFileName, Separators );
    }



    public static string GetFileName( string Filename, string Separators = PotentialPathSeparators )
    {
      if ( Filename == null )
      {
        return "";
      }
      int               pos = Filename.Length;

      while ( pos > 0 )
      {
        --pos;
        if ( IsSeparator( Filename[pos], Separators ) )
        {
          return Filename.Substring( pos + 1 );
        }
      }
      return Filename;
    }



    public static string GetFileNameWithoutExtension( string Filename, string Separators = PotentialPathSeparators )
    {
      string    filenameWithExtension = GetFileName( Filename, Separators );

      int   dotPos = filenameWithExtension.LastIndexOf( '.' );
      if ( dotPos == -1 )
      {
        return filenameWithExtension;
      }
      return filenameWithExtension.Substring( 0, dotPos );
    }



    public static string GetExtension( string Filename, string Separators = PotentialPathSeparators )
    {
      string    filenameWithExtension = GetFileName( Filename, Separators );

      int   dotPos = filenameWithExtension.LastIndexOf( '.' );
      if ( dotPos == -1 )
      {
        return "";
      }
      return filenameWithExtension.Substring( dotPos );
    }



    public static bool IsPathRooted( string Path, string Separators = PotentialPathSeparators )
    {
      if ( string.IsNullOrEmpty( Path ) )
      {
        return false;
      }
      if ( GR.OS.IsWindows )
      {
        if ( Path.Length < 3 )
        {
          return false;
        }
        // drive letter?
        if ( ( char.IsLetter( Path[0] ) )
        &&   ( Path[1] == ':' )
        &&   ( Separators.IndexOf( Path[2] ) != -1 ) )
        {
          return true;
        }
        // UNC format?
        if ( Path.StartsWith( @"\\" ) )
        {
          return true;
        }
        return false;
      }
      // assume unix
      return Separators.IndexOf( Path[0] ) != -1;
    }
  }
}
