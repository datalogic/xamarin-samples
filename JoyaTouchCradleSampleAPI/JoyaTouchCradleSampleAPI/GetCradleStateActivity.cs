// ©2017 Datalogic Inc. and/or its affiliates. All rights reserved.

using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using Com.Datalogic.Extension.Selfshopping.Cradle.Joyatouch;

namespace JoyaTouchCradleSampleAPI
{
    /**
     * Activity for Cradle state information.
     */
    [Activity(Label = "GetCradleStateActivity")]
    public class GetCradleStateActivity : Activity
    {
        private TextView textDeviceInCradle;
        private TextView textState;

        private ICradleJoyaTouch jtCradle;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_cradle_state_info);

            JoyaTouchCradleApplication application = (JoyaTouchCradleApplication)ApplicationContext;
            jtCradle = application.CradleJoyaTouch;

            textDeviceInCradle = (TextView)FindViewById(Resource.Id.textDeviceInCradle);
            textState = (TextView)FindViewById(Resource.Id.textState);

            if (savedInstanceState == null)
            {
                // Change displayed text.
                updateContent();
            }
        }

        /**
         * Update showed TextViews with Cradle State Info if the device is into the Cradle, otherwise
         * show an error message.
         */
        public void updateContent()
        {
            bool inCradle = jtCradle.IsDeviceInCradle;
            //if (inCradle)
            {
                StateInfo state = new StateInfo();
                if (jtCradle.GetCradleState(state))
                {
                    textDeviceInCradle.SetTextColor(Color.Blue);
                    textDeviceInCradle.SetText(Resource.String.device_in_cradle);

                    textState.Text = "";
                    textState.Text = "Application version: " + state.ApplVersion + "\n"
                            + "Bootloader version: " + state.BtldrVersion + "\n"
                            + "Insertion count: " + state.InsertionCount + "\n"
                            + "Slot index: " + state.SlotIndex + "\n"
                            + "Fast charge available: " + state.IsFastChargeAvailable + "\n";
                }
                else
                {
                    textDeviceInCradle.SetTextColor(Color.Red);
                    textDeviceInCradle.SetText(Resource.String.get_cradle_state_failed);

                    textState.Text = "";
                }
            }
            //else
            //{
            //    textDeviceInCradle.SetTextColor(Color.Red);
            //    textDeviceInCradle.SetText(Resource.String.device_not_in_cradle);
            //    textState.Text = "";
            //}
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater inflater = MenuInflater;
            inflater.Inflate(Resource.Menu.cradle_state_info_menu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            // Handle item selection
            switch (item.ItemId)
            {
                case Resource.Id.item_refresh:
                    updateContent();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}