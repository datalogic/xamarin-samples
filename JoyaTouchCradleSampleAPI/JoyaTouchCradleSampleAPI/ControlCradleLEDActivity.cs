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
using Com.Datalogic.Extension.Selfshopping.Cradle.Joyatouch;
using Java.IO;

namespace JoyaTouchCradleSampleAPI
{
    [Activity(Label = "ControlCradleLEDActivity")]
    public class ControlCradleLEDActivity : Activity
    {
        private static string[] NAMES =
        {
            "LED ON",
            "LED OFF",
            "LED BLINK FAST",
            "LED BLINK SLOW",
            "LED TOGGLE"
        };

        private static LedAction[] ACTIONS =
        {
            LedAction.On,
            LedAction.Off,
            LedAction.BlinkFast,
            LedAction.BlinkSlow,
            LedAction.Toggle
        };

        private ICradleJoyaTouch jtCradle;

        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_cradle_led);

            JoyaTouchCradleApplication application = (JoyaTouchCradleApplication)ApplicationContext;
            jtCradle = application.CradleJoyaTouch;

            // Fill the Adapter with the available LED names/actions
            ListView listMainActivities = (ListView)FindViewById(Resource.Id.listLedActions);
            ArrayAdapter<String> adapter = new ArrayAdapter<String>(this,
                    Android.Resource.Layout.SimpleListItem1, NAMES);
            listMainActivities.Adapter = adapter;
            listMainActivities.ItemClick += delegate (object sender, ListView.ItemClickEventArgs e)
            {
                SetLed(ACTIONS[e.Position]);
            };
	    }

        public void SetLed(LedAction action)
        {
            if (jtCradle.ControlLed(action))
            {
                Toast.MakeText(this, action.ToString(), ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(this, "Failed to control the LED. Check if the device is inserted in the Cradle",
                        ToastLength.Long).Show();
            }
        }
    }
}