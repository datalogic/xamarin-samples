using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading;
using Android.Util;
using Com.Datalogic.Device;
using Com.Datalogic.Device.Location;

namespace DeviceSampleAPI
{
    [Activity(Label = "LocationActivity")]
    class LocationActivity : Activity
    {
        private TextView gpsStatus;
        private Button gpsBtn;
        private Button settingsBtn;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_location);

            gpsStatus = (TextView)FindViewById(Resource.Id.gpsStatus);

            bool newVal;
            newVal = IsGPSEnabled();
            gpsStatus.Text = "GPS is " + (newVal ? "" : "not ") + "enabled";

            gpsBtn = (Button)FindViewById(Resource.Id.btnGps);
            gpsBtn.Click += delegate
            {
                newVal = !IsGPSEnabled();
                SetGPSState(newVal);
                try
                {
                    Thread.Sleep(300);
                }
                catch (ThreadInterruptedException e)
                {
                    // It should not fail
                    Log.Error(this.LocalClassName, "Error during sleep", e);
                }
                newVal = IsGPSEnabled();
                gpsStatus.Text = "GPS is " + (newVal ? "" : "not ") + "enabled";
            };

            settingsBtn = (Button)FindViewById(Resource.Id.btnLocationSettings);
            settingsBtn.Click += delegate
            {
                Intent viewIntent = new Intent(Android.Provider.Settings.ActionLocationSourceSettings);
                StartActivity(viewIntent);
            };
        }


        /**
         * Use android.location.LocationManager to determine if GPS is enabled.
         */
        public bool IsGPSEnabled()
        {
            Android.Locations.LocationManager loc = (Android.Locations.LocationManager)GetSystemService(Context.LocationService);
            return loc.IsProviderEnabled(Android.Locations.LocationManager.GpsProvider);
        }

        /**
         * Use LocationManager to set the gps as enabled (true), or disabled
         * (false).
         */
        public void SetGPSState(bool enable)
        {
            LocationManager gps = null;

            // Store previous exception preference.
            bool previous = ErrorManager.AreExceptionsEnabled();

            // We want to be notified through an exception if something goes wrong.
            ErrorManager.EnableExceptions(true);
            try
            {
                gps = new LocationManager();
                gps.SetLocationMode(enable ? LocationMode.SensorsAndNetwork : LocationMode.Off);
            }
            catch (DeviceException e1)
            {
                // Just in case we get an error.
                Log.Error(this.LocalClassName, "Exception while switching location mode ", e1);
            }
            // Set previous value.
            ErrorManager.EnableExceptions(previous);
        }
    }
}