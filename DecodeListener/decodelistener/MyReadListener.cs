using System;
using Com.Datalogic.Decode;
using Android.Util;
using Android.Runtime;

namespace decodelistener
{
	public class MyReadListener : Java.Lang.Object, IReadListener
	{
		public MyReadListener()
		{

		}

		void IReadListener.OnRead(IDecodeResult decodeResult)
		{
			// Change the displayed text to the current received result.
			Log.Debug("", "Text: " + decodeResult.Text + " barcodeID " + decodeResult.BarcodeID.ToString());
		}
	}
}