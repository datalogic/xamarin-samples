using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Util;
using Android.Content;

namespace DeviceSampleAPI
{
    [Activity(Label = "DeviceSampleAPI", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        public static String[] ACT_NAMES =
        {
            "Battery",
            "Location - LocationManager",
            "NFC - NfcManager",
            "Notifications - LedManager",
            "Touch - TouchManager",
            "Sleep and Wakeup - PowerManager",
            "Informations - SYSTEM",
            "Reset device"
        };

        private static Type[] ACT_CLASSES =
        {
            typeof(BatteryActivity),
            typeof(LocationActivity),
            typeof(NfcActivity),
            typeof(NotificationActivity),
            typeof(TouchActivity),
            typeof(SleepActivity),
            typeof(InfoActivity),
            typeof(ResetActivity)
        };



        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            // Fill the Adapter with the available activity names/classes
            ListView listMainActivities = (ListView)FindViewById(Resource.Id.listMainActivities);
            ArrayAdapter<String> adapter = new ArrayAdapter<string>(this,
                Android.Resource.Layout.SimpleListItem1, ACT_NAMES);
            listMainActivities.Adapter = adapter;
            listMainActivities.ItemClick += delegate (object sender, ListView.ItemClickEventArgs e)
            {
                // Open the activity of corresponding position. pos is the index in ACT_CLASSES
                try
                {
                    Intent intent = new Intent(this, ACT_CLASSES[e.Position]);
                    StartActivity(intent);
                }
                catch(Exception exception)
                {
                    Log.Error(this.GetType().Name, "While creating activity");
                }

            };
           
        }

    }
}

