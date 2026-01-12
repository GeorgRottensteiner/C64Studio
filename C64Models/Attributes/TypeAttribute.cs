using System;
using System.ComponentModel;

namespace RetroDevStudio
{
  [AttributeUsage( AttributeTargets.Field )]
  public class TypeAttribute : Attribute
  {
    public Type Type = null;



    public TypeAttribute( Type type )
    {
      Type = type;
    }



  }

}