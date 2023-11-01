﻿using RetroDevStudio;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaManager
{
  public class Manager
  {
    public int Handle( string[] args )
    {
      if ( args.Length == 0 )
      {
        System.Console.WriteLine( "MediaManager V" + System.Windows.Forms.Application.ProductVersion );
        System.Console.WriteLine( "" );
        System.Console.WriteLine( "Call with mediamanager" );
        System.Console.WriteLine( "  [-d64,-d81 <disk image>]" );
        System.Console.WriteLine( "  [-t64 <tape file>]" );
        System.Console.WriteLine( "  [-import <file name>[,load address]]" );
        System.Console.WriteLine( "  [-filetype <prg,seq,rel,usr,del>" );
        System.Console.WriteLine( "  [-export <file name>]" );
        System.Console.WriteLine( "  [-rename <file name>]" );
        System.Console.WriteLine( "  [-renameto <file name>]" );
        System.Console.WriteLine( "  [-delete <file name>]" );
        System.Console.WriteLine( "  [-listfiles]" );
        System.Console.WriteLine( "  [-nosuccessmessages]" );
        System.Console.WriteLine( "" );
        System.Console.WriteLine( "load address can be given as decimal, hexadecimal (prefix $ or 0x). If load address is given it is prepended to the import file data." );
        System.Console.WriteLine( "The filename given to -renameto is used for the actually written file when exporting" );
        System.Console.WriteLine( "The filename given to -renameto is used for the file info entry when importing" );
        return 1;
      }

      bool    expectingParameter = false;
      string  expectingParameterName = "";
      string  methodToUse = "";
      bool    verboseLog = true;
      RetroDevStudio.Types.FileType fileType = RetroDevStudio.Types.FileType.PRG;

      GR.Collections.Map<string,string>   paramMap = new GR.Collections.Map<string,string>();

      for ( int i = 0; i < args.Length; ++i )
      {
        if ( expectingParameter )
        {
          paramMap[expectingParameterName] = args[i];
          expectingParameter = false;

          if ( expectingParameterName == "-FILETYPE" )
          { 
            switch ( args[i].ToUpper() )
            {
              case "PRG":
                fileType = RetroDevStudio.Types.FileType.PRG;
                break;
              case "SEQ":
                fileType = RetroDevStudio.Types.FileType.SEQ;
                break;
              case "REL":
                fileType = RetroDevStudio.Types.FileType.REL;
                break;
              case "USR":
                fileType = RetroDevStudio.Types.FileType.USR;
                break;
              case "DEL":
                fileType = RetroDevStudio.Types.FileType.DEL;
                break;
              default:
                System.Console.Error.WriteLine( "Unsupported file type " + args[i] );
                System.Console.Error.WriteLine( "Only PRG, USR, SEQ, REL and DEL are allowed" );
                return 1;
            }
          }
        }
        else if ( ( args[i].ToUpper() == "-D64" )
        ||        ( args[i].ToUpper() == "-D81" )
        ||        ( args[i].ToUpper() == "-T64" )
        ||        ( args[i].ToUpper() == "-IMPORT" )
        ||        ( args[i].ToUpper() == "-DELETE" )
        ||        ( args[i].ToUpper() == "-RENAME" )
        ||        ( args[i].ToUpper() == "-RENAMETO" )
        ||        ( args[i].ToUpper() == "-EXPORT" ) )
        {
          expectingParameter = true;
          expectingParameterName = args[i].ToUpper();

          if ( ( expectingParameterName == "-IMPORT" )
          ||   ( expectingParameterName == "-EXPORT" )
          ||   ( expectingParameterName == "-DELETE" )
          ||   ( expectingParameterName == "-RENAME" ) )
          {
            methodToUse = expectingParameterName;
          }
        }
        else if ( args[i].ToUpper() == "-FILETYPE" )
        {
          expectingParameter = true;
          expectingParameterName = args[i].ToUpper();
        }
        else if ( args[i].ToUpper() == "-LISTFILES" )
        {
          paramMap[args[i].ToUpper()] = "";
          methodToUse = args[i].ToUpper();
        }
        else if ( args[i].ToUpper() == "-NOSUCCESSMESSAGES" )
        {
          verboseLog = true;
        }
        else
        {
          System.Console.Error.WriteLine( "Unsupported option " + args[i] );
          return 1;
        }
      }
      if ( expectingParameter )
      {
        System.Console.Error.WriteLine( "Missing value for " + expectingParameterName );
        return 1;
      }
      // do we have a container?
      if ( ( !paramMap.ContainsKey( "-D64" ) )
      &&   ( !paramMap.ContainsKey( "-D81" ) )
      &&   ( !paramMap.ContainsKey( "-T64" ) ) )
      {
        System.Console.Error.WriteLine( "Missing medium" );
        return 1;
      }

      // load
      RetroDevStudio.Formats.MediaFormat   medium = null;
      string                          mediumFilename = "";
      if ( paramMap.ContainsKey( "-D64" ) )
      {
        medium = new RetroDevStudio.Formats.D64();
        mediumFilename = paramMap["-D64"];
      }
      else if ( paramMap.ContainsKey( "-D81" ) )
      {
        medium = new RetroDevStudio.Formats.D81();
        mediumFilename = paramMap["-D81"];
      }
      else if ( paramMap.ContainsKey( "-T64" ) )
      {
        medium = new RetroDevStudio.Formats.T64();
        mediumFilename = paramMap["-T64"];
      }

      if ( !medium.Load( mediumFilename ) )
      {
        System.Console.WriteLine( "No image found, start empty" );
        medium.CreateEmptyMedia();
      }

      // handle command
      if ( methodToUse == "-LISTFILES" )
      {
        List<RetroDevStudio.Types.FileInfo> files = medium.Files();

        foreach ( RetroDevStudio.Types.FileInfo file in files )
        {
          string    filename = Util.FilenameToReadableUnicode( file.Filename );
          filename = filename.PadRight( 16 );
          System.Console.WriteLine( "\"" + filename + "\"  " + file.Blocks + " blocks  " + file.Type.ToString() + "  " + file.Filename );
        }
        System.Console.WriteLine( files.Count + " files" );
      }
      else if ( methodToUse == "-EXPORT" )
      {
        RetroDevStudio.Types.FileInfo fileInfo = medium.LoadFile( Util.ToFilename( medium.FilenameType, paramMap["-EXPORT"] ) );
        if ( fileInfo != null )
        {
          string outputFilename = paramMap["-EXPORT"];
          if ( paramMap.ContainsKey( "-RENAMETO" ) )
          {
            outputFilename = paramMap["-RENAMETO"];
          }
          GR.IO.File.WriteAllBytes( outputFilename, fileInfo.Data );
          if ( verboseLog )
          {
            System.Console.WriteLine( "File " + paramMap["-EXPORT"] + " exported" );
          }
        }
        else
        {
          System.Console.Error.WriteLine( "File " + paramMap["-EXPORT"] + " not found in medium" );
        }
      }
      else if ( methodToUse == "-DELETE" )
      {
        RetroDevStudio.Types.FileInfo fileInfo = medium.LoadFile( Util.ToFilename( medium.FilenameType, paramMap["-DELETE"] ) );
        if ( fileInfo != null )
        {
          if ( !medium.DeleteFile( Util.ToFilename( medium.FilenameType, paramMap["-DELETE"] ) ) )
          {
            System.Console.Error.WriteLine( "File could not be deleted: " + medium.LastError );
          }
          else
          {
            if ( verboseLog )
            {
              System.Console.WriteLine( "File deleted" );
            }
            medium.Save( mediumFilename );
          }
        }
        else
        {
          System.Console.Error.WriteLine( "File " + paramMap["-DELETE"] + " not found in medium" );
        }
      }
      else if ( methodToUse == "-IMPORT" )
      {
        bool    addAddress = false;
        ushort startAddress = 0x0801;

        string filenameImport = paramMap["-IMPORT"];

        string[] paramList = filenameImport.Split( ',' );

        if ( ( paramList.Length == 0 )
        ||   ( paramList.Length > 2 ) )
        {
          System.Console.Error.WriteLine( "Invalid parameter value for -IMPORT" );
          return 1;
        }

        filenameImport = paramList[0];

        if ( paramList.Length >= 2 )
        {
          addAddress = true;
          string loadAdressPart = paramList[1];
          if ( loadAdressPart.StartsWith( "0x" ) )
          {
            ushort.TryParse( loadAdressPart.Substring( 2 ), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out startAddress );
          }
          else if ( loadAdressPart.StartsWith( "$" ) )
          {
            ushort.TryParse( loadAdressPart.Substring( 1 ), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out startAddress );
          }
          else
          {
            ushort.TryParse( loadAdressPart, out startAddress );
          }
        }

        GR.Memory.ByteBuffer data = GR.IO.File.ReadAllBytes( filenameImport );
        if ( data == null )
        {
          System.Console.Error.WriteLine( "Could not read file " + paramMap["-IMPORT"] );
          return 1;
        }

        // insert load address
        if ( addAddress )
        {
          GR.Memory.ByteBuffer    newData = new GR.Memory.ByteBuffer( 2 + data.Length );

          newData.SetU16At( 0, startAddress );
          data.CopyTo( newData, 0, (int)data.Length, 2 );
          data = newData;
        }

        if ( paramMap.ContainsKey( "-RENAMETO" ) )
        {
          filenameImport = paramMap["-RENAMETO"];
        }
        if ( !medium.WriteFile( Util.ToFilename( medium.FilenameType, filenameImport ), data, fileType ) )
        {
          System.Console.Error.WriteLine( "Could not write file to medium: " + medium.LastError );
          return 1;
        }
        if ( verboseLog )
        {
          System.Console.WriteLine( "File imported" );
        }
        medium.Save( mediumFilename );
      }
      else if ( methodToUse == "-RENAME" )
      {
        if ( !paramMap.ContainsKey( "-RENAMETO" ) )
        {
          System.Console.Error.WriteLine( "Missing -renameto directive" );
          return 1;
        }
        string origFilename = paramMap["-RENAME"];
        GR.Memory.ByteBuffer    origFilenameBuffer = Util.ToFilename( medium.FilenameType, origFilename );
        string targetFilename = paramMap["-RENAMETO"];
        GR.Memory.ByteBuffer    targetFilenameBuffer = Util.ToFilename( medium.FilenameType, targetFilename );

        if ( !medium.RenameFile( origFilenameBuffer, targetFilenameBuffer ) )
        {
          System.Console.Error.WriteLine( "Failed to rename file" );
          return 1;
        }
        if ( verboseLog )
        {
          System.Console.WriteLine( "File renamed" );
        }
        medium.Save( mediumFilename );
      }
      else
      {
        System.Console.Error.WriteLine( "Unsupported method " + methodToUse );
      }
      return 0;
    }

  }
}
