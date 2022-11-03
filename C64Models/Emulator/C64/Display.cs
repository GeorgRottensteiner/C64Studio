using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
#endif


namespace Tiny64
{
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows")]
#endif
  public unsafe class Display
	{
		private Control     _TargetControl = null;
		private Bitmap      _Screen;
		private uint[]			_ColorValues = new uint[17];
		private Color[]     _Colors = new Color[17];

		private Rectangle		_bitmapRect = new Rectangle( 0, 0, 504, 312 );
		private BitmapData _bitmapData = null;
		private uint*       _bitmapPtr;

    private volatile bool     _DisplayWasUpdated = true;


		// PAL      size = 403 * 312
		// NTSC#1   size = 411 * 262
		// NTSC#2   size = 418 * 263



		public unsafe Display()
		{
			_ColorValues[0] = 0xff000000;
			_ColorValues[1] = 0xffffffff;
			_ColorValues[2] = 0xff8B4131;
			_ColorValues[3] = 0xff7BBDC5;
			_ColorValues[4] = 0xff8B41AC;
			_ColorValues[5] = 0xff6AAC41;
			_ColorValues[6] = 0xff3931A4;
			_ColorValues[7] = 0xffD5DE73;
			_ColorValues[8] = 0xff945A20;
			_ColorValues[9] = 0xff5A4100;
			_ColorValues[10] = 0xffBD736A;
			_ColorValues[11] = 0xff525252;
			_ColorValues[12] = 0xff838383;
			_ColorValues[13] = 0xffACEE8B;
			_ColorValues[14] = 0xff7B73DE;
			_ColorValues[15] = 0xffACACAC;
			_ColorValues[16] = 0xff80ff80;
			for ( int i = 0; i < 16; ++i )
			{
				_Colors[i] = GR.Color.Helper.FromARGB( _ColorValues[i] );
			}

			_Screen = new Bitmap( 504, 312, PixelFormat.Format32bppArgb );

			_bitmapData = _Screen.LockBits( _bitmapRect, ImageLockMode.ReadWrite, _Screen.PixelFormat );
			_bitmapPtr = (uint*)_bitmapData.Scan0.ToPointer();
			_Screen.UnlockBits( _bitmapData );

			for ( int i = 504 * 312 - 1; i >= 0; i-- )
			{
				_bitmapPtr[i] = 0xffffff;
			}
		}



		public unsafe void SetPixel( int X, int Y, byte Color )
		{
			_bitmapPtr[X + Y * 504] = _ColorValues[Color];
		}



		public void SetTarget( Control Control )
		{
			_TargetControl = Control;
		}



		public unsafe void Flush()
		{
			_TargetControl?.BeginInvoke( new DrawDelegate( DrawAsync ), _Screen );
		}



		private void DrawAsync( Bitmap bmp )
		{
      if ( _TargetControl.Visible )
      {
        if ( _DisplayWasUpdated )
        {
          _DisplayWasUpdated = false;
          _TargetControl.Invoke( new DrawDelegate( Draw ), bmp );
        }
      }
		}



		private delegate void DrawDelegate( Bitmap bmp );

		private void Draw( Bitmap bmp )
		{
      _DisplayWasUpdated = true;

      Graphics g = _TargetControl.CreateGraphics();
			g.DrawImage( bmp, 0, 0 );

			//g.FillRectangle( _clearBrush, 0, 400, 50, 50 );
			//g.DrawString( _fps.ToString(), _font, _brush, 0, 400 );
		}
	}

}
