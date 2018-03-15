using Android.App;
using Android.Widget;
using Android.OS;
using Com.Datalogic.Decode.Configuration;
using Android.Content;
using Com.Datalogic.Decode;
using System;
using Com.Datalogic.Device;
using Android.Util;
using Com.Datalogic.Device.Configuration;
using System.Collections.Generic;
using System.Text;

namespace DecodeIntent
{
    [Activity(Label = "DecodeIntent", MainLauncher = true, Icon = "@mipmap/icon")]
    public class IntentWedgeSample : Activity
    {
        public const string ACTION_BROADCAST_RECEIVER = "com.datalogic.examples.decode_action";
	    public const string CATEGORY_BROADCAST_RECEIVER = "com.datalogic.examples.decode_category";

        // Default Extra contents added to the intent containing results.
        public const string ExtraData = IntentWedge.ExtraBarcodeData;
        public const string ExtraDataString = IntentWedge.ExtraBarcodeString;

        public const string ExtraType = IntentWedge.ExtraBarcodeType;

	    // Action and Category defined in AndroidManifest.xml, associated to a dedicated activity.
	    private const string ACTION = "com.datalogic.examples.STARTINTENT" ;
	    private const string CATEGORY = "android.intent.category.DEFAULT";

        private BroadcastReceiver receiver = null;
        private IntentFilter filter = null;

        private RadioGroup radioGroup;
        private BarcodeManager manager;
        private ScannerProperties configuration;

        Button startBtn;
        Button stopBtn;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.intent_wedge_sample);

            //create our stop and start buttons which will be used to initiate an intent driven barcode scan
            startBtn = (Button) FindViewById(Resource.Id.startBtn);
            startBtn.Click += StartButtonClicked;
            stopBtn = (Button)FindViewById(Resource.Id.stoptBtn);
            stopBtn.Click += StopButtonClicked;

            // Create a BarcodeManager. It will be used later to change intent delivery modes.
            manager = new BarcodeManager();

            //get the radio group from the displayed layout
            radioGroup = (RadioGroup) FindViewById(Resource.Id.radioGroup);
            // Associate a specific listener.
            radioGroup.CheckedChange += MyClickedItemListener;
            // Clear check and force a default radio button checked.
            radioGroup.ClearCheck();
            radioGroup.Check(Resource.Id.radioStartActivity);


        }
        // Called when stop button is pressed.
        private void StopButtonClicked(object sender, EventArgs e)
        {
            //stop the barcode decoding process
            StopDecode();
        }
        // Called when start button is pressed.
        private void StartButtonClicked(object sender, EventArgs e)
        {
            //start the barcode decoding process
            StartDecode();
        }

        protected override void OnResume()
        {
            base.OnResume();
            // Register dynamically decode wedge intent broadcast receiver.
            receiver = new DecodeWedgeIntentReceiver();
            filter = new IntentFilter();
            filter.AddAction(ACTION_BROADCAST_RECEIVER);
            filter.AddCategory(CATEGORY_BROADCAST_RECEIVER);
            RegisterReceiver(receiver, filter);
        }
        protected override void OnPause()
        {
            Log.Info(this.GetType().Name, "OnPause");
            base.OnPause();
            // Unregister our BroadcastReceiver.
            UnregisterReceiver(receiver);
            receiver = null;
            filter = null;
        }
        // Creates an intent and start decoding.
        private void StartDecode()
        {
            Intent myIntent = new Intent();
            myIntent.SetAction(BarcodeManager.ActionStartDecode);
            SendBroadcast(myIntent);
        }
        // Creates an intent and stop decoding.
        private void StopDecode()
        {
            Intent myIntent = new Intent();
            myIntent.SetAction(BarcodeManager.ActionStopDecode);
            SendBroadcast(myIntent);
        }

        // Receives action ACTION_BROADCAST_RECEIVER
        [BroadcastReceiver(Enabled = true)]
        [IntentFilter(new[] { "com.datalogic.examples.STARTINTENT" },
            Categories = new[] { "android.intent.category.DEFAULT" })]
        private class DecodeWedgeIntentReceiver : BroadcastReceiver
        {

            public override void OnReceive(Context context, Intent wedgeIntent)
            {
                string action = wedgeIntent.Action;
                if (action.Equals(ACTION_BROADCAST_RECEIVER))
                {

                    ICollection<string> category_all = wedgeIntent.Categories;
                    StringBuilder category = new StringBuilder();
                    foreach (String currentCategory in category_all)
                    {
                        category.Append(currentCategory);
                    }
                    // Read content of result intent.
                    String barcode = wedgeIntent.GetStringExtra(ExtraDataString);            
                    String type = wedgeIntent.GetStringExtra(IntentWedgeSample.ExtraType);

                    var startActivityFromBroadcast = new Intent(context, typeof(ActivityStartedFromBroadcast));

                    startActivityFromBroadcast.PutExtra(IntentWedgeSample.ExtraDataString, barcode);
                    startActivityFromBroadcast.PutExtra(IntentWedgeSample.ExtraType, type);
                    startActivityFromBroadcast.PutExtra("category", category.ToString());

                    //pass the resulting barcode information to an activity class which we will use to display the barcode data
                    context.StartActivity(startActivityFromBroadcast);
                }

            }
        }
        private void MyClickedItemListener(object sender, RadioGroup.CheckedChangeEventArgs e)
        {
            // Handle errors through java Exceptions
            ErrorManager.EnableExceptions(true);

            try
            {
                // get the current settings from the BarcodeManager
                configuration = ScannerProperties.Edit(manager);
                // disables KeyboardWedge
                configuration.KeyboardWedge.Enable.Set(false);
                // enable wedge intent
                configuration.IntentWedge.Enable.Set(true);

                switch (e.CheckedId)
                {
                    case Resource.Id.radioBroadcast:
                        // set wedge intent action and category
                        configuration.IntentWedge.Action.Set(ACTION_BROADCAST_RECEIVER);
                        configuration.IntentWedge.Category.Set(CATEGORY_BROADCAST_RECEIVER);
                        // set wedge intent delivery through broadcast
                        configuration.IntentWedge.DeliveryMode.Set(IntentDeliveryMode.Broadcast);
                        configuration.Store(manager, false);
                        break;
                    case Resource.Id.radioStartActivity:
                        // set wedge intent action and category
                        configuration.IntentWedge.Action.Set(ACTION);
                        configuration.IntentWedge.Category.Set(CATEGORY);
                        // intent delivery startActivity
                        configuration.IntentWedge.DeliveryMode.Set(IntentDeliveryMode.StartActivity);
                        configuration.Store(manager, false);
                        break;
                }
            }
            catch (Exception exception) //catch any errors that occured.
            {
                if(exception is ConfigException)
                {
                    ConfigException ex = (ConfigException)exception;
                    Log.Info(this.GetType().Name, "Error while retrieving/setting properties:" + exception.Message);
                }
                else if(exception is DecodeException)
                {
                    DecodeException ex = (DecodeException)exception;
                    Log.Info(this.GetType().Name, "Error while retrieving/setting properties:" + exception.Message);
                }
                else
                {
                    Log.Info(this.GetType().Name, "Error while retrieving/setting properties:" + exception.Message);
                }
                
            }
        }
        private void ShowMessage(String message)
        {
            Toast.MakeText(this, message, ToastLength.Long).Show();
        }
    }
}

