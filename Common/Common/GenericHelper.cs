using System;
using System.ComponentModel;
using System.Reflection;

namespace GR
{
  public class EnumHelper
  {
    public static string GetDescription( Enum en )
    {
      Type type = en.GetType();

      MemberInfo[] memInfo = type.GetMember( en.ToString() );

      if ( memInfo != null && memInfo.Length > 0 )
      {
        object[] attrs = memInfo[0].GetCustomAttributes( typeof( DescriptionAttribute ), false );

        if ( attrs != null && attrs.Length > 0 )
        {
          return ( (DescriptionAttribute)attrs[0] ).Description;
        }
      }

      return en.ToString();
    }



    public static bool TryParse<T>( Type type, string machine, out T machineType ) where T: Enum
    {
      foreach ( var member in Enum.GetValues( type ) )
      {
        if ( string.Compare( machine, member.ToString(), true ) == 0 )
        {
          machineType = (T)member;
          return true;
        }
      }
      machineType = default;
      return false;
    }



  }

  namespace Generic
  {

    public class Tupel<T1, T2> : IComparable
    {
      public T1  first = default( T1 );
      public T2  second = default( T2 );

      public Tupel()
      {
      }

      public Tupel( T1 Value1, T2 Value2 )
      {
        first = Value1;
        second = Value2;
      }

      public override int GetHashCode()
      {
        return (int)( ( ( first.GetHashCode() << 5 ) + first.GetHashCode() ) ^ second.GetHashCode() );
      }

      public override string ToString()
      {
        if ( first is string )
        {
          return first.ToString();
        }
        if ( second is string )
        {
          return second.ToString();
        }
        return base.ToString();
      }

      public override bool Equals( object obj )
      {
        if ( obj is Tupel<T1, T2> )
        {
          return Equals( (Tupel<T1, T2>)obj );
        }
        return false;
      }

      public static bool operator ==( Tupel<T1, T2> Obj1, Tupel<T1, T2> Obj2 )
      {
        if ( object.ReferenceEquals( Obj1, null ) )
        {
          if ( object.ReferenceEquals( Obj2, null ) )
          {
            return true;
          }
          return false;
        }
        if ( object.ReferenceEquals( Obj2, null ) )
        {
          return false;
        }
        return Obj1.Equals( Obj2 );
      }



      public static bool operator !=( Tupel<T1, T2> Obj1, Tupel<T1, T2> Obj2 )
      {
        if ( object.ReferenceEquals( Obj1, null ) )
        {
          if ( object.ReferenceEquals( Obj2, null ) )
          {
            return false;
          }
          return true;
        }
        if ( object.ReferenceEquals( Obj2, null ) )
        {
          return true;
        }
        return !Obj1.Equals( Obj2 );
      }



      public bool Equals( Tupel<T1,T2> OtherTupel )
      {
        if ( object.ReferenceEquals( OtherTupel, null ) )
        {
          return false;
        }
        return ( ( OtherTupel.first.Equals( first ) )
             &&  ( OtherTupel.second.Equals( second ) ) );
      }



      public int CompareTo( object obj )
      {
        if ( ( obj == null ) 
        ||   ( !( obj is Tupel<T1,T2> ) ) )
        {
          return -1;
        }
        if ( ( this is IComparable )
        &&   ( obj is IComparable ) )
        {
          var tupel = (Tupel<T1,T2>)obj;

          if ( first.Equals( tupel.first ) )
          {
            return ( (IComparable)second ).CompareTo( tupel.second );
          }
          return ( (IComparable)first ).CompareTo( tupel.first );
        }
        return GetHashCode() - ( (Tupel<T1,T2>)obj ).GetHashCode();
      }



    }
  }
  
  namespace Color
  {
    public static class Helper
    {
      public static System.Drawing.Color FromARGB( UInt32 Color )
      {
        return System.Drawing.Color.FromArgb( (int)Color );
      }
    }
      
  }
  
}