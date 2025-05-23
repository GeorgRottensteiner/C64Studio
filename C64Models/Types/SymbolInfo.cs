﻿using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RetroDevStudio
{
  public class SymbolInfo
  {
    public enum Types
    {
      UNKNOWN = 0,
      LABEL,
      PREPROCESSOR_LABEL,
      PREPROCESSOR_CONSTANT_1,
      PREPROCESSOR_CONSTANT_2,
      PREPROCESSOR_CONSTANT_NUMBER,
      PREPROCESSOR_CONSTANT_STRING,
      CONSTANT_1,
      CONSTANT_2,
      ZONE,
      CONSTANT_REAL_NUMBER,
      CONSTANT_STRING,
      VARIABLE_NUMBER,
      VARIABLE_INTEGER,
      VARIABLE_STRING,
      VARIABLE_ARRAY,
      TEMP_LABEL,
      MACRO,
      TEXT_LABEL
    };

    public Types      Type = Types.UNKNOWN;
    public string     Name = "";
    public int        LineIndex = 0;            // global
    public int        LineCount = -1;           // global (-1 is for complete file)
    public string     DocumentFilename = "";
    public int        LocalLineIndex = 0;
    public long       AddressOrValue = -1;
    public string     String = "";
    public double     RealValue = 0;
    public string     Zone = "";
    public bool       FromDependency = false;
    public string     Info = "";
    public int        CharIndex = -1;
    public int        Length = 0;
    public RetroDevStudio.Types.ASM.SourceInfo SourceInfo = null;
    public int        NumArguments = 0;
    public GR.Collections.MultiMap<int,SymbolReference>   References = new GR.Collections.MultiMap<int, SymbolReference>();



    public SymbolInfo()
    { 
    }



    public SymbolInfo( SymbolInfo RHS )
    {
      Type                  = RHS.Type;
      Name                  = RHS.Name;
      LineIndex             = RHS.LineIndex;            // global
      LineCount             = RHS.LineCount;           // global (-1 is for complete file)
      DocumentFilename      = RHS.DocumentFilename;
      LocalLineIndex        = RHS.LocalLineIndex;
      AddressOrValue        = RHS.AddressOrValue;
      String                = RHS.String;
      RealValue             = RHS.RealValue;
      Zone                  = RHS.Zone;
      FromDependency        = RHS.FromDependency;
      Info                  = RHS.Info;
      CharIndex             = RHS.CharIndex;
      Length                = RHS.Length;
      SourceInfo            = RHS.SourceInfo;
      References            = RHS.References;
    }



    public override string ToString()
    {
      if ( Type == Types.CONSTANT_REAL_NUMBER )
      {
        return Util.DoubleToString( RealValue );
      }
      else if ( Type == Types.CONSTANT_STRING )
      {
        return Util.RemoveQuotes( String );
      }
      else if ( IsInteger() )
      {
        return AddressOrValue.ToString();
      }
      return Name;
    }



    public bool IsInteger()
    {
      if ( ( Type == Types.CONSTANT_1 )
      ||   ( Type == Types.CONSTANT_2 )
      ||   ( Type == Types.TEMP_LABEL )
      ||   ( Type == Types.LABEL )
      ||   ( Type == Types.PREPROCESSOR_CONSTANT_1 )
      ||   ( Type == Types.PREPROCESSOR_CONSTANT_2 ) )
      {
        return true;
      }
      return false;
    }



    public bool IsNumber()
    {
      if ( ( Type == Types.CONSTANT_REAL_NUMBER )
      ||   ( Type == Types.PREPROCESSOR_CONSTANT_NUMBER )
      ||   ( Type == Types.VARIABLE_NUMBER ) )
      {
        return true;
      }
      return false;
    }



    public bool IsString()
    {
      if ( ( Type == Types.CONSTANT_STRING )
      ||   ( Type == Types.PREPROCESSOR_CONSTANT_STRING )
      ||   ( Type == Types.VARIABLE_STRING ) )
      {
        return true;
      }
      return false;
    }



    public int ToInt32()
    {
      return (int)ToInteger();
    }



    public long ToInteger()
    {
      if ( Type == Types.CONSTANT_REAL_NUMBER )
      {
        return (int)RealValue;
      }
      if ( Type == Types.CONSTANT_STRING )
      {
        if ( String.Length == 0 )
        {
          return 0;
        }
        if ( ( String.StartsWith( "\"" ) )
        &&   ( String.EndsWith( "\"" ) ) )
        {
          if ( String.Length > 2 )
          {
            return (char)String[1];
          }
          return 0;
        }
        if ( ( String.StartsWith( "'" ) )
        &&   ( String.EndsWith( "'" ) ) )
        {
          if ( String.Length > 2 )
          {
            return (char)String[1];
          }
          return 0;
        }
        return (char)String[0];
      }
      return AddressOrValue;
    }



    public double ToNumber()
    {
      if ( Type == Types.CONSTANT_REAL_NUMBER )
      {
        return RealValue;
      }
      return (double)AddressOrValue;
    }



    public void AddReference( int GlobalLineIndex, TokenInfo TokenInfo )
    {
      if ( !References.Any( r => ( r.Key == GlobalLineIndex ) && ( r.Value.TokenInfo.StartPos == TokenInfo.StartPos ) && ( r.Value.TokenInfo.Length == TokenInfo.Length ) ) )
      {
        References.Add( GlobalLineIndex,
            new SymbolReference()
            {
              GlobalLineIndex = GlobalLineIndex,
              TokenInfo = TokenInfo
            }
            );
      }
    }



  }


}