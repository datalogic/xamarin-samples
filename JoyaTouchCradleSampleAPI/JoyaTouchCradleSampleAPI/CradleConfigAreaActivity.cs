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

namespace JoyaTouchCradleSampleAPI
{
    [Activity(Label = "CradleConfigAreaActivity")]
    public class CradleConfigAreaActivity : Activity
    {
        // initialize with 32 bytes.
        // that way even if initial read in OnCreate fails, we can still fill the screen with 0's.
        private byte[] configValues = new byte[32] 
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        private GridView grid;

        private ICradleJoyaTouch jtCradle;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_cradle_config_area);

            JoyaTouchCradleApplication application = (JoyaTouchCradleApplication)ApplicationContext;
            jtCradle = application.CradleJoyaTouch;

            if (savedInstanceState == null)
            {
                ConfigArea config = new ConfigArea();
                if (jtCradle.ReadConfigArea(config))
                {
                    configValues = config.GetContent();
                }
                else
                {
                    Toast.MakeText(this, "Failure reading config area. Retry.", ToastLength.Long).Show();
                }
            }
            else
            {
                configValues = savedInstanceState.GetByteArray("configValues");
            }

            ConfigAreaAdapter adapter = new ConfigAreaAdapter(this, configValues);
            grid = (GridView)FindViewById(Resource.Id.configValuesGrid);
            grid.Adapter = adapter;

            // handle read button
            Button readButton = FindViewById<Button>(Resource.Id.buttonReadConfig);
            readButton.Click += delegate
            {
                ConfigArea config = new ConfigArea();
                if (jtCradle.ReadConfigArea(config))
                {
                    configValues = config.GetContent();
                    ConfigAreaAdapter aTemp = new ConfigAreaAdapter(this, configValues);
                    grid.Adapter = aTemp;
                    grid.Invalidate();
                }
                else
                {
                    Toast.MakeText(this, "Failure reading config area. Retry.", ToastLength.Long).Show();
                }
            };

            // handle write button
            Button writeButton = FindViewById<Button>(Resource.Id.buttonWriteConfig);
            writeButton.Click += delegate
            {
                ConfigArea config = new ConfigArea(configValues);
                if (jtCradle.WriteConfigArea(config))
                    Toast.MakeText(this, "Config data written successfully.", ToastLength.Long).Show();
                else
                    Toast.MakeText(this, "Failure writing config area. Retry.", ToastLength.Long).Show();
            };
    }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            outState.PutByteArray("configValues", configValues);
            base.OnSaveInstanceState(outState);
        }

    }
}