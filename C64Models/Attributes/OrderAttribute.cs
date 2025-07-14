using System;



namespace RetroDevStudio
{
  [AttributeUsage( AttributeTargets.Field )]
  public class OrderAttribute : Attribute
  {
    public int Order = 0;



    public OrderAttribute( int order )
    {
      Order = order;
    }



  }

}