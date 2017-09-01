using System.Drawing;
using System.Windows.Forms;

namespace GR
{
  namespace Forms
  {
    public class WindowStateManager
    {
      private static bool GeometryIsBizarreLocation( Point loc, Size size )
      {
        Rectangle rect = new Rectangle( loc, size );

        return ( Screen.FromRectangle( rect ) == null ) ? false : true;
      }

      public static void GeometryFromString( string thisWindowGeometry, Form formIn )
      {
        if ( string.IsNullOrEmpty( thisWindowGeometry ) )
        {
          return;
        }
        string[] numbers = thisWindowGeometry.Split( '|' );
        if ( numbers.Length <= 4 )
        {
          return;
        }
        string windowString = numbers[4];
        if ( windowString == "Normal" )
        {
          Point windowPoint = new Point( int.Parse( numbers[0] ), int.Parse( numbers[1] ) );
          Size windowSize = new Size( int.Parse( numbers[2] ), int.Parse( numbers[3] ) );

          bool locOkay = GeometryIsBizarreLocation( windowPoint, windowSize );
          bool sizeOkay = GeometryIsBizarreSize( windowSize );

          if ( locOkay == true && sizeOkay == true )
          {
            formIn.Location = windowPoint;
            formIn.Size = windowSize;
            formIn.StartPosition = FormStartPosition.Manual;
            formIn.WindowState = FormWindowState.Normal;
          }
          else if ( sizeOkay == true )
          {
            formIn.Size = windowSize;
          }
        }
        else if ( windowString == "Maximized" )
        {
          formIn.Location = new Point( 100, 100 );
          formIn.StartPosition = FormStartPosition.Manual;
          formIn.WindowState = FormWindowState.Maximized;
        }
      }



      private static bool GeometryIsBizarreSize( Size size )
      {
        return ( size.Height <= Screen.PrimaryScreen.WorkingArea.Height &&
            size.Width <= Screen.PrimaryScreen.WorkingArea.Width );
      }



      public static string GeometryToString( Form Window )
      {
        return Window.Location.X.ToString() + "|" +
               Window.Location.Y.ToString() + "|" +
               Window.Size.Width.ToString() + "|" +
               Window.Size.Height.ToString() + "|" +
               Window.WindowState.ToString();
      }
    }
  }
}