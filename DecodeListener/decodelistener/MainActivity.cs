using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Util;
using Com.Datalogic.Device;
using Com.Datalogic.Decode;
using Com.Datalogic.Device.Configuration;
using Com.Datalogic.Decode.Configuration;

namespace decodelistener
{
    [Activity(Label = "DecodeListener", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private readonly string LOGTAG = typeof(MainActivity).Name;

        BarcodeManager decoder = null;
		MyReadListener readListener = new MyReadListener();
        TextView mBarcodeText;
        TextView mSymbology;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Retrieve the TextView from the displayed layout.
            mBarcodeText = FindViewById<TextView>(Resource.Id.editText1);
            mSymbology = FindViewById<TextView>(Resource.Id.textSymbology);
        }

        protected override void OnResume()
        {
            base.OnResume();

            Log.Info(LOGTAG, "OnResume");

            // If the decoder instance is null, create it.
            if (decoder == null)
            {
                // Remember an onPause call will set it to null.
                decoder = new BarcodeManager();
            }

            // From here on, we want to be notified with exceptions in case of errors.
            ErrorManager.EnableExceptions(true);

            try
            {
                // add our class as a listener
                decoder.AddReadListener(readListener);
            }
            catch (DecodeException e)
            {
                Log.Error(LOGTAG, "Error while trying to bind a listener to BarcodeManager", e);
            }
        }

        protected override void OnPause()
        {
            base.OnPause();

            Log.Info(LOGTAG, "onPause");

            // If we have an instance of BarcodeManager.
            if (decoder != null)
            {
                try
                {
                    // Unregister our listener from it and free resources
                    decoder.RemoveReadListener(readListener);
                }
                catch (Exception e)
                {
                    Log.Error(LOGTAG, "Error while trying to remove a listener from BarcodeManager", e);
                }
            }
        }


    }
}

