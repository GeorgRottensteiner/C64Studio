using System;



namespace RetroDevStudio
{
  [AttributeUsage( AttributeTargets.Field | AttributeTargets.Class )]
  public class RuntimeArgumentNameAttribute : Attribute
  {
    public string  ArgumentName = "";



    public RuntimeArgumentNameAttribute( string argumentName )
    {
      ArgumentName = argumentName;
    }



  }

}