using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Com.Datalogic.Device.Info;
using Android.Graphics;

namespace DeviceSampleAPI
{
    
    //This is a class that shows various hardware and software information  
    [Activity(Label = "InfoActivity")]
    public class InfoActivity : Activity
    {
        //It will show device associated icon
        private ImageButton btnBg;
        //Containing device infos
        private TextView txtInfo;     

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_info);

            //Create our widgets
            btnBg = (ImageButton)FindViewById(Resource.Id.btnBg);
            txtInfo = (TextView)FindViewById(Resource.Id.txtInfo);

            btnBg.Click += btnBgOnClick;

            //load the txtInfo view with hardware and software data
            getInfo();
        }

        //changes the button image to a datalogic scanner device image
        private void btnBgOnClick(object sender, EventArgs e)
        {
            Bitmap img = SYSTEM.DeviceImage;
            float ratio =  (float)img.Height/(float)img.Width;
            int width = 200;
            int height = (int)Math.Ceiling(width * ratio);
            btnBg.SetImageBitmap(Bitmap.CreateScaledBitmap(img, width, height, true));
        }

        //creates our menu system
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.reset, menu);
            return base.OnPrepareOptionsMenu(menu);
        }
        //event handler called whenever a menu item is selected
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if(item.ItemId == Resource.Id.action_reset)
            {
                getInfo();
                btnBg.SetImageResource(Resource.Drawable.ic_launcher);
                return true;
            }
            return base.OnOptionsItemSelected(item);

        }
        //sets the textInfo textview to show the software/hardware information
        private void getInfo()
        {
            txtInfo.Text = getDescription();
        }   
        //gets the Scanner type from the System's
        public String getScannerType()
        {
            return SYSTEM.BarcodeScannerType.ToString();    
        }
        //gets the string that has the hardware/software information
        private String getDescription()
        {
            //get Boot type from System
            String bootType = SYSTEM.BootType.ToString();
            //get device model
            String deviceModel = Android.OS.Build.Model;
            //get Wifi type
            String wifiType = SYSTEM.WifiType.ToString();
            //Get firmware version from System's versions data structure
            String firmwareVersion = SYSTEM.Versions["FIRMWARE"];                                
            //Get kernel version from System's versions data structure
            String kernelVersion = SYSTEM.Versions["KERNEL"];
            //create our string builder
            StringBuilder builder = new StringBuilder(
                  "Scanner Type: " + getScannerType() + "\n"
                + "Boot Type: " + bootType + "\n"
                + "Device Model: " + deviceModel + "\n"
                + "WiFi type: " + wifiType + "\n"
                + "Firmware Version: " + firmwareVersion + "\n"
                + "Kernel Version: " + kernelVersion + "\n");

            //create a filter for our intent
            IntentFilter iFilter = new IntentFilter(SYSTEM.Version.ActionDeviceInfo);    
            //create our intent with our filter
            Intent info = this.RegisterReceiver(null, iFilter);
            //get our bundle from our intent
            Bundle b = info.Extras;
            ICollection<String> bundleStrings = b.KeySet();
            Object tmp;
            //iterate over strings returned in bundle
            foreach(var st in bundleStrings)
            {
                builder.Append(st + ": ");
                tmp = b.Get(st);
                if(tmp != null)
                {
                    //add bundle string to string builder
                    builder.Append(tmp);
                }                                                              
                builder.Append("\n");
            }
            //return our builder string
            return builder.ToString();
        }
    }
}