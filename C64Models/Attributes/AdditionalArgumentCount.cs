using System;



namespace RetroDevStudio
{
  [AttributeUsage( AttributeTargets.Field | AttributeTargets.Class )]
  public class AdditionalArgumentCountAttribute : Attribute
  {
    public int NumOptionalArguments = 0;
    public int NumMandatoryArguments = 0;



    public AdditionalArgumentCountAttribute( int numOptionalArguments, int numMandatoryArguments )
    {
      NumOptionalArguments  = numOptionalArguments;
      NumMandatoryArguments = numMandatoryArguments;
    }



  }

}