using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Util;
using Com.Datalogic.Device;
using Com.Datalogic.Decode;
using Com.Datalogic.Decode.Configuration;
using Com.Datalogic.Device.Configuration;
using Android.Views;
using Android.Content;

namespace DecodeConfigSampleAPI
{
    [Activity(Label = "DecodeConfigSampleAPI", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private readonly string LOGTAG = typeof(MainActivity).Name;

        BarcodeManager manager = null;
        ScannerProperties configuration = null;

        override protected void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create a BarcodeManager.
            manager = new BarcodeManager();
            SetContentView(Resource.Layout.Main);

            // setup button
            Button btn = (Button)FindViewById(Resource.Id.button1);
            btn.Click += delegate
            {
                StartSettingsActivity();
            };

            // Pass it to ScannerProperties class.
            // ScannerProperties cannot be instatiated directly, instead call edit.
            configuration = ScannerProperties.Edit(manager);

            // Now we can change some Scanner/Device configuration parameters.
            // These values are not applied, as long as store method is not called.
            configuration.Code39.Enable.Set(true);
            configuration.Code39.EnableChecksum.Set(true);
            configuration.Code39.FullAscii.Set(true);
            configuration.Code39.Length1.Set(20);
            configuration.Code39.Length2.Set(2);
            configuration.Code39.LengthMode.Set(LengthControlMode.TwoFixed);
            configuration.Code39.SendChecksum.Set(false);
            configuration.Code39.UserID.Set('x');

            configuration.Code128.Enable.Set(true);
            configuration.Code128.Length1.Set(6);
            configuration.Code128.Length2.Set(2);
            configuration.Code128.LengthMode.Set(LengthControlMode.Range);
            configuration.Code128.UserID.Set('y');

            if (configuration.QrCode.IsSupported)
            {
                configuration.QrCode.Enable.Set(false);
            }

            // Change IntentWedge action and category to specific ones.
            configuration.IntentWedge.Action.Set("com.datalogic.examples.decode_action");
            configuration.IntentWedge.Category.Set("com.datalogic.examples.decode_category");

            // From here on, we would like to get a return value instead of an exception in case of error
            ErrorManager.EnableExceptions(false);

            // Now we are ready to store them
            // Second parameter set to true saves configuration in a permanent way.
            // After boot settings will be still valid.
            int errorCode = configuration.Store(manager, true);

            // Check return value.
            if (errorCode != ConfigException.Success)
            {
                Log.Error(LOGTAG, "Error during store", ErrorManager.LastError);
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.main, menu);
            return true;
        }

        override public bool OnOptionsItemSelected(IMenuItem item)
        {
            // Handle menu item selection
            switch (item.ItemId)
            {
                case Resource.Id.action_settings:
                    StartSettingsActivity();
                    return true;
                default:
                    return Parent.OnOptionsItemSelected(item);
            }
        }

        public void buttonClicked(View v)
        {
            StartSettingsActivity();
        }

        private void StartSettingsActivity()
        {
            Intent dialogIntent = new Intent(Android.Provider.Settings.ActionSettings);
            dialogIntent.AddFlags(ActivityFlags.NewTask);
            StartActivity(dialogIntent);
        }
    }
}

