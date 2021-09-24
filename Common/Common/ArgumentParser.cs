using System;
using System.Collections.Generic;



namespace GR.Text
{
  public class ArgumentParser
  {
    public class ParameterInfo
    {
      public enum ParameterResult
      {
        NOT_SET = 0,
        SET,
        VALUE_MISSING,
        INVALID_VALUE,
        SET_MULTIPLE_TIMES
      };

      public enum ParameterType
      {
        SINGLE_VALUE,
        SWITCH      
      };


      public ParameterResult      Result  = ParameterResult.NOT_SET;
      public ParameterType        Type    = ParameterType.SINGLE_VALUE;
      public string               Name    = "";
      public string               Value   = "";
      public List<string>         ValidValues = null;
      public bool                 Optional = true;



      public ParameterInfo( string Param, bool Optional )
      {
        this.Name = Param;
        this.Optional = Optional;
        this.Type = ParameterType.SINGLE_VALUE;
      }



      public ParameterInfo( string Param, bool Optional, ParameterType Type )
      {
        this.Name = Param;
        this.Optional = Optional;
        this.Type = Type;
      }
    };


    private List<ParameterInfo> AllowedParameters = new List<ParameterInfo>();
    private List<string>        UnknownParameters = new List<string>();



    public void Clear()
    {
      AllowedParameters.Clear();
      UnknownParameters.Clear();
    }



    public bool CheckParameters( string[] Arguments )
    {
      foreach ( ParameterInfo paramInfo in AllowedParameters )
      {
        paramInfo.Result = ParameterInfo.ParameterResult.NOT_SET;
      }
      UnknownParameters.Clear();
      
      
      string            currentParameter = "";
      
      ParameterInfo     currentInfo = null;
      bool              foundError = false;
      
      foreach ( string paramArg in Arguments )
      {
        // no line breaks
        string param = paramArg.TrimEnd( new char[]{ '\r', '\n' } );
        if ( string.IsNullOrEmpty( param ) )
        {
          continue;
        }
        if ( param[0] == '-' )
        {
          // a switch
          if ( currentInfo != null )
          {
            currentInfo.Result = ParameterInfo.ParameterResult.VALUE_MISSING;
            currentParameter = "";
            currentInfo = null;
            foundError = true;
          }
          
          string newParameter = param.ToUpper().Substring( 1 );
          
          bool isKnownParameter = false;
          foreach ( ParameterInfo paramInfo in AllowedParameters )
          {
            if ( paramInfo.Name.ToUpper() == newParameter )
            {
              isKnownParameter = true;
              if ( paramInfo.Type == ParameterInfo.ParameterType.SINGLE_VALUE )
              {
                if ( paramInfo.Value.Length == 0 )
                {
                  currentParameter = paramInfo.Name;
                }
                else
                {
                  paramInfo.Result = ParameterInfo.ParameterResult.SET_MULTIPLE_TIMES;
                  foundError = true;
                }
                currentInfo = paramInfo;
              }
              else if ( paramInfo.Type == ParameterInfo.ParameterType.SWITCH )
              {
                paramInfo.Result = ParameterInfo.ParameterResult.SET;
                currentInfo = paramInfo;
              }
              break;
            }
          }
          if ( !isKnownParameter )
          {
            UnknownParameters.Add( param );
          }
        }
        else
        {
          // a value
          if ( currentInfo == null )
          {
            UnknownParameters.Add( param );
          }
          else
          {
            if ( currentInfo.Value.Length == 0 )
            {
              if ( currentInfo.ValidValues == null )
              {
                currentInfo.Value = param;
                currentInfo.Result = ParameterInfo.ParameterResult.SET;
              }
              else
              {
                string newParameter = param.ToUpper();
                
                if ( currentInfo.ValidValues.Contains( newParameter ) )
                {
                  currentInfo.Value   = newParameter;
                  currentInfo.Result = ParameterInfo.ParameterResult.SET;
                }
                else 
                {
                  currentInfo.Result = ParameterInfo.ParameterResult.INVALID_VALUE;
                  foundError = true;
                }
              }
            }
            else
            {
              currentInfo.Result = ParameterInfo.ParameterResult.SET_MULTIPLE_TIMES;
              foundError = true;
            }
            currentInfo = null;
            currentParameter = "";
          }
        }
      }
      
      if ( currentInfo != null )
      {
        if ( currentInfo.ValidValues != null )
        {
          currentInfo.Result = ParameterInfo.ParameterResult.VALUE_MISSING;
          currentInfo = null;
          foundError = true;
        }
      }
    
      foreach ( ParameterInfo paramInfo in AllowedParameters )
      {
        if ( ( paramInfo.Result == ParameterInfo.ParameterResult.NOT_SET )
        &&   ( !paramInfo.Optional ) )
        {
          foundError = true;
        }
      }
      
      return !foundError;
    }



    public void AddSwitch( string Switch, bool Optional )
    {
      AllowedParameters.Add ( new ParameterInfo( Switch, Optional, ParameterInfo.ParameterType.SWITCH ) );
    }



    public void AddSwitchValue( string Name, string Value )
    {
      foreach ( ParameterInfo paramInfo in AllowedParameters )
      {
        if ( string.Compare( Name, paramInfo.Name, true ) == 0 )
        {
          if ( paramInfo.ValidValues == null )
          {
            paramInfo.ValidValues = new List<string>();
          }
          paramInfo.ValidValues.Add( Value );
          return;
        }
      }
    }



    public void AddParameter( string Param )
    {
      AllowedParameters.Add( new ParameterInfo( Param, false ) );
    }



    public void AddParameter( string Param, bool Optional )
    {
      AllowedParameters.Add( new ParameterInfo( Param, Optional ) );
    }



    public void AddParameter( ParameterInfo ParamInfo )
    {
      AllowedParameters.Add( ParamInfo );
    }



    public void AddOptionalParameter( string Param )
    {
      AddParameter( Param, true );
    }



    public bool IsParameterSet( string Name )
    {
      foreach ( ParameterInfo paramInfo in AllowedParameters )
      {
        if ( ( paramInfo.Result == ParameterInfo.ParameterResult.SET )
        &&   ( string.Compare( Name, paramInfo.Name, true ) == 0 ) )
        {
          return true;
        }
      }
      return false;
    }



    public string Parameter( string Name )
    {
      foreach ( ParameterInfo paramInfo in AllowedParameters )
      {
        if ( ( paramInfo.Result == ParameterInfo.ParameterResult.SET )
        &&   ( string.Compare( Name, paramInfo.Name, true ) == 0 ) )
        {
          return paramInfo.Value;
        }
      }
      return "";
    }



    public string ErrorInfo()
    {
      System.Text.StringBuilder sb = new System.Text.StringBuilder();

      foreach ( ParameterInfo paramInfo in AllowedParameters )
      {
        if ( paramInfo.Result == ParameterInfo.ParameterResult.INVALID_VALUE )
        {
          sb.AppendLine( "Invalid value for parameter " + paramInfo.Name );
        }
        else if ( paramInfo.Result == ParameterInfo.ParameterResult.VALUE_MISSING )
        {
          sb.AppendLine( "Value missing for parameter " + paramInfo.Name );
        }
        else if ( paramInfo.Result == ParameterInfo.ParameterResult.SET_MULTIPLE_TIMES )
        {
          sb.AppendLine( "Value set several times for parameter " + paramInfo.Name );
        }
        else if ( paramInfo.Result == ParameterInfo.ParameterResult.NOT_SET )
        {
          if ( !paramInfo.Optional )
          {
            sb.AppendLine( "Mandatory parameter " + paramInfo.Name + " not set" );
          }
        }
      }

      foreach ( string param in UnknownParameters )
      {
        sb.AppendLine( "Unknown parameter " + param + " passed" );
      }
      return sb.ToString();
    }



    public string CallInfo()
    {
      System.Text.StringBuilder sb = new System.Text.StringBuilder();

      foreach ( ParameterInfo paramInfo in AllowedParameters )
      {
        switch ( paramInfo.Type )
        {
          case ParameterInfo.ParameterType.SINGLE_VALUE:
            if ( paramInfo.Optional )
            {
              sb.Append( "[" );
            }
            sb.Append( "-" + paramInfo.Name );
            if ( paramInfo.Optional )
            {
              sb.Append( "]" );
            }
            sb.AppendLine();
            break;
          case ParameterInfo.ParameterType.SWITCH:
            if ( paramInfo.Optional )
            {
              sb.Append( "[" );
            }
            sb.Append( "-" + paramInfo.Name );
            if ( paramInfo.ValidValues != null )
            {
              sb.Append( " <" );
              int index = 0;
              foreach ( string value in paramInfo.ValidValues )
              {
                sb.Append( value );
                if ( index + 1 < paramInfo.ValidValues.Count )
                {
                  sb.Append( "," );
                }
                ++index;
              }
              sb.Append( ">" );
            }
            if ( paramInfo.Optional )
            {
              sb.Append( "]" );
            }
            sb.AppendLine();
            break;
        }
      }
      return sb.ToString();
    }



    public int UnknownArgumentCount()
    {
      return UnknownParameters.Count;
    }



    public string UnknownArgument( int Index )
    {
      if ( ( Index < 0 )
      ||   ( Index >= UnknownParameters.Count ) )
      {
        return "Unknown argument index " + Index + " out of bounds";
      }
      return UnknownParameters[Index];
    }

  }    

}
