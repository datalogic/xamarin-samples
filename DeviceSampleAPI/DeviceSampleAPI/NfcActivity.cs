using Android.App;
using Android.Content;
using Android.Nfc;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using Com.Datalogic.Device;
using System.Threading;

namespace DeviceSampleAPI
{

    //Activity to enable/disable Nfc
    [Activity(Label = "NfcActivity")]
    class NfcActivity : Activity
    {
        private TextView nfcStatus;
        private Button btnNfc;
        private Button btnNFCSettings;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_nfc);

            nfcStatus = (TextView)FindViewById(Resource.Id.nfcStatus);

            bool newVal = IsNfcEnabled();
            nfcStatus.Text = "Nfc is " + (newVal ? "" : "not ") + "enabled";

            btnNfc = (Button)FindViewById(Resource.Id.btnNfc);
            btnNfc.Click += delegate
            {
                Android.Nfc.NfcManager manager = (Android.Nfc.NfcManager)GetSystemService(Context.NfcService);
                if (manager == null)
                {
                    nfcStatus.Text = "Nfc is not supported on this device.";
                }
                else
                {
                    newVal = !IsNfcEnabled();
                    SetEnableNfc(newVal);
                    try
                    {
                        Thread.Sleep(500);
                    }
                    catch (ThreadInterruptedException e)
                    {
                        Log.Error("Thread exception", e.StackTrace);
                    }
                    newVal = IsNfcEnabled();
                    nfcStatus.Text = "Nfc is " + (newVal ? "" : "not ") + "enabled";
                }
            };

            btnNFCSettings = (Button)FindViewById(Resource.Id.btnNFCSettings);
            btnNFCSettings.Click += delegate
            {
                Intent viewIntent = new Intent(Android.Provider.Settings.ActionNfcSettings);
                StartActivity(viewIntent);
            };
        }

        /**
         * @return True if Nfc is enabled, false otherwise.
         */
        public bool IsNfcEnabled()
        {
            Android.Nfc.NfcManager manager = (Android.Nfc.NfcManager)GetSystemService(Context.NfcService);
            if (manager != null)
            {
                NfcAdapter adapter = manager.DefaultAdapter;
                if (adapter != null && adapter.IsEnabled)
                    return true;
            }

            // default is to return false. NFC is either not supported at all, or not enabled.
            return false;
        }

        /**
         * Enable or disable Nfc, using com.datalogic.device.nfc.NfcManager.
         */
        public void SetEnableNfc(bool enable)
        {
            bool previous = ErrorManager.AreExceptionsEnabled();
            ErrorManager.EnableExceptions(false);
            ErrorManager.ClearErrors();

            int error = new Com.Datalogic.Device.Nfc.NfcManager().EnableNfcAdapter(enable);
            if (error != DeviceException.Success)
            {
                Log.Error(this.LocalClassName, "Error while setting NFC", ErrorManager.LastError);
            }
            ErrorManager.EnableExceptions(previous);
        }
    }
}