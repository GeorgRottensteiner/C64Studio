using System;
using System.ComponentModel;

namespace RetroDevStudio
{
  [AttributeUsageAttribute( AttributeTargets.Field | AttributeTargets.Class )]
  public class AdditionalArgumentCountAttribute : Attribute
  {
    public int NumArguments = 0;



    public AdditionalArgumentCountAttribute( int NumArguments )
    {
      this.NumArguments = NumArguments;
    }



  }

}