// ©2017 Datalogic Inc. and/or its affiliates. All rights reserved.

using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Com.Datalogic.Extension.Selfshopping.Cradle.Joyatouch;
using Android.Content;
using Com.Datalogic.Extension.Selfshopping.Cradle;

namespace JoyaTouchCradleSampleAPI
{
    [Activity(Label = "JoyaTouchCradleSampleAPI", MainLauncher = true, Icon = "@drawable/icon")]
    public class JoyaCradleAPITestActivity : Activity
    {
        private static string[] ACT_NAMES =
        {
            "Get Cradle State",
            "Control LED",
            "Control Lock",
            "Read / Write Config Area",
            "Read / Write Custom Area",
            "Reset"
        };

        private static Type[] ACT_CLASSES =
        {
            typeof(GetCradleStateActivity),
            typeof(ControlCradleLEDActivity),
            typeof(ControlCradleLockActivity),
            typeof(CradleConfigAreaActivity),
            typeof(CradleCustomAreaActivity),
            null
        };

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            // Fill the Adapter with the available activity names/classes
            ListView listMainActivities = (ListView)FindViewById(Resource.Id.listMainActivities);
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this,
                    Android.Resource.Layout.SimpleListItem1, ACT_NAMES);
            listMainActivities.Adapter = adapter;
            listMainActivities.ItemClick += delegate (object sender, ListView.ItemClickEventArgs e)
            {
                JoyaTouchCradleApplication application = (JoyaTouchCradleApplication)ApplicationContext;
                ICradleJoyaTouch jtCradle = application.CradleJoyaTouch;
                //if (jtCradle.IsDeviceInCradle)
                {
                    // Execute the reset without calling any activity
                    if (ACT_NAMES[e.Position].Equals("Reset"))
                    {
                        jtCradle.Reset();
                    }
                    else
                    {
                        Intent intent = new Intent(this, ACT_CLASSES[e.Position]);
                        StartActivity(intent);
                    }
                }
                //else
                //{
                //    Toast.MakeText(this, "Device is not in Cradle. Retry after insertion.",
                //            ToastLength.Long).Show();
                //}
            };


            if (savedInstanceState == null)
            {
                // Get the JoyaTouchCradle instance
                ICradle cradle = CradleManager.Cradle;

                if (cradle != null && cradle.Type == CradleType.JoyaTouchCradle)
                {
                    JoyaTouchCradleApplication application = (JoyaTouchCradleApplication)ApplicationContext;
                    application.CradleJoyaTouch = (ICradleJoyaTouch)cradle;
                }
                else
                {
                    Toast.MakeText(this, "JoyaTouchCradle not found. Cannot execute samples.",
                            ToastLength.Long).Show();
                    Finish();
                }
            }
        }
    }
}

