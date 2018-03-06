using System;
using Android.App;
using Android.OS;
using Android.Widget;
using Android.Util;
using Com.Datalogic.Device;
using Com.Datalogic.Device.Notification;

namespace DeviceSampleAPI
{
    //this is a class whose purpose is to showcase the feature of the phone that handles turning the LED on and off
    [Activity(Label = "NotificationActivity")]
    public class NotificationActivity : Activity
    {
        private LedManager led;
        private Button btnLed;
        private Button btnLedEnable;
        private Boolean enable = false;
        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_notification);

            ErrorManager.EnableExceptions(true);

            try
            {
                //create a new LedManager which is responsible for everything related to LED
                led = new LedManager();
            } catch (Exception e)
            {
                Log.Error(this.GetType().Name, "Error while creating LedManager");
            }

            //this button will cause the LED to blink a set number of times
            btnLed = (Button) FindViewById(Resource.Id.btnLed);
            //wire up our event handler for when the button is clicked
            btnLed.Click += BtnLedBlinkClicked;
            //this button will cause the LED to turn on or off
            btnLedEnable = (Button)FindViewById(Resource.Id.btnLedEnable);
            //wire up our event handler for when the button is clicked
            btnLedEnable.Click += BtnLedEnableClicked;
            //this method will initially turn the LED on.
            TurnOnGreenSpotLed();     
        }

        //the method turns the LED on for the device
        private void TurnOnGreenSpotLed()
        {
            try
            {
                //set the LED to be on from the LedManager
                led.SetLed(Led.LedGreenSpot, enable);
                //set the button text to be different depending on whether the LED is on or off
                btnLedEnable.Text = "green spot is " + (enable ? "on" : "off");
            } catch(Exception exception)
            {
                Log.Error(this.GetType().Name, "Cannot set Green spot");
            }
        }
        //this is our event handler that turns on the LED light
        private void BtnLedEnableClicked(object sender, EventArgs e)
        {
            enable = !enable;
            TurnOnGreenSpotLed();
        }
        //this is our event handler that causes the LED light to blink 10 times
        private void BtnLedBlinkClicked(object sender, EventArgs e)
        {
            try
            {
                //tell the LedManager to cause the LED to blink 10 times
                led.BlinkLed(Led.LedGreenSpot, 10, 500, 500);
            } catch(Exception exception)
            {
                //If there was an exception then log that exception
                Log.Error(this.GetType().Name, "Cannot blink Green spot");        
            }

        }
        protected override void OnPause()
        {
            base.OnPause();
            enable = false;
            TurnOnGreenSpotLed();
        }
        protected override void OnStop()
        {
            base.OnStop();
            enable = false;
            TurnOnGreenSpotLed();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            enable = false;
            TurnOnGreenSpotLed();
        }

    }
}