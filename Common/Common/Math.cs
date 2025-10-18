using GR.Math;
using System;
using System.Diagnostics;



namespace GR
{
  public static class MathUtil
  {
    public static int Clamp( int minimum, int currentValue, int maximum )
    {
      if ( currentValue < minimum )
      {
        return minimum;
      }
      if ( currentValue > maximum )
      {
        return maximum;
      }
      return currentValue;
    }

  }



}