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
    [Activity(Label = "ControlCradleLockActivity")]
    public class ControlCradleLockActivity : Activity
    {
        private static string[] NAMES =
        {
            "LOCK",
            "LOCK WITH LED OFF",
            "UNLOCK",
            "UNLOCK WITH LED ON"
        };

        private static LockAction[] ACTIONS =
        {
            LockAction.Lock,
            LockAction.LockWithLedOff,
            LockAction.Unlock,
            LockAction.UnlockWithLedOn
        };

        private ICradleJoyaTouch jtCradle;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_cradle_lock);

            JoyaTouchCradleApplication application = (JoyaTouchCradleApplication)ApplicationContext;
            jtCradle = application.CradleJoyaTouch;

            // Fill the Adapter with the available LED names/actions
            ListView listMainActivities = (ListView)FindViewById(Resource.Id.listLockActions);
            ArrayAdapter<String> adapter = new ArrayAdapter<String>(this,
                    Android.Resource.Layout.SimpleListItem1, NAMES);
            listMainActivities.Adapter = adapter;
            listMainActivities.ItemClick += delegate (object sender, ListView.ItemClickEventArgs e)
            {
                SetLock(ACTIONS[e.Position]);
            };
	    }

        public void SetLock(LockAction action)
        {
            if (jtCradle.ControlLock(action))
            {
                Toast.MakeText(this, action.ToString(), ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(this, "Failed to control the lock. Check if the device is inserted in the Cradle",
                        ToastLength.Long).Show();
            }
        }
    }
}