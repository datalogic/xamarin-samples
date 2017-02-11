// ©2017 Datalogic S.p.A. and/or its affiliates. All rights reserved.

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Com.Datalogic.Device.Info;

// Activity for battery information

namespace DeviceSampleAPI
{
    [Activity(Label = "BatteryActivity")]
    class BatteryActivity : Activity
    {
        private TextView txtPower;
        private Intent batteryStatus;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_battery);

            txtPower = (TextView)FindViewById(Resource.Id.txtPower);

            // Get Android's status.
            batteryStatus = RegisterReceiver(null, new IntentFilter(
                Intent.ActionBatteryChanged));

            // Change displayed text.
            SetText();
        }

        // updates showed TextView with battery info.
        public void SetText()
        {
            txtPower.Text = "";
            txtPower.Text = "Battery Info: \n" + GetBatteryInfo() + "\n"
                + "Battery Status: " + GetStatus() + "\n"
                + "External AC Power: " + GetExtPowerStatus() + "\n"
                + "External USB Power: " + GetUsbPowerStatus() + "\n"
                + "Current level: " + GetCurrentLevel() + "\n";
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            // Inflate the menu; this adds items to the action bar if it is 
            // present.
            MenuInflater.Inflate(Resource.Menu.reset, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.action_reset)
            {
                SetText();
            }

            return base.OnOptionsItemSelected(item);
        }

        public string GetBatteryInfo()
        {
            string output = "  capacity:" + SYSTEM.BatteryInfo.Capacity + "\n"
                + "  year:" + SYSTEM.BatteryInfo.Year + "\n"
                + "  week:" + SYSTEM.BatteryInfo.SerialNumber + "\n"
                + "  serial_number:" + SYSTEM.BatteryInfo.Manufacturer + "\n";
            return output;
        }

        public bool GetExtPowerStatus()
        {
            BatteryPlugged plugged = (BatteryPlugged)batteryStatus.GetIntExtra(BatteryManager.ExtraPlugged, 0);
            if (plugged == BatteryPlugged.Ac)
                return true;
            else
                return false;
        }

        public bool GetUsbPowerStatus()
        {
            BatteryPlugged plugged = (BatteryPlugged)batteryStatus.GetIntExtra(BatteryManager.ExtraPlugged, 0);
            if (plugged == BatteryPlugged.Usb)
                return true;
            else
                return false;
        }

        public string GetStatus()
        {
            BatteryStatus status = (BatteryStatus)batteryStatus.GetIntExtra(BatteryManager.ExtraStatus, -1);
            return status.ToString();
        }

        public bool GetChargingStatus()
        {
            int status = batteryStatus.GetIntExtra(BatteryManager.ExtraStatus, -1);
            return status == (int)BatteryStatus.Charging;
        }

        public bool getDischargingStatus()
        {
            int status = batteryStatus.GetIntExtra(BatteryManager.ExtraStatus, -1);
            return status == (int)BatteryStatus.Discharging;
        }

        public float GetCurrentLevel()
        {
            int level = batteryStatus.GetIntExtra(BatteryManager.ExtraLevel, -1);
            int scale = batteryStatus.GetIntExtra(BatteryManager.ExtraScale, -1);
            return level / (float)scale;
        }
    }
}