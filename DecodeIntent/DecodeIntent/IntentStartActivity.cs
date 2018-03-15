using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Widget;

namespace DecodeIntent
{
    /*
     This is the activity that is called by the Datalogic SDK after the barcode
     has been scanned. 
     */
    [Activity(Label = "IntentStartActivity")]
    [IntentFilter(new[] { "com.datalogic.examples.STARTINTENT" },
    Categories = new[] { "android.intent.category.DEFAULT" })]
    public class IntentStartActivity : Activity
    {
        private TextView textMsg;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_intent_start);

            String action = Intent.Action;
            // Get the Intent that created this activity.
            Intent currentIntent = Intent;
            ShowMessage("Started IntentActivity");
            Log.Info(this.GetType().Name, "Started Activity with Intent");

            ICollection<string> category_all = Intent.Categories;
            StringBuilder category = new StringBuilder();
            foreach (String currentCategory in category_all)
            {
                category.Append(currentCategory);
            }

            //Get the barcode type
            String type = Intent.GetStringExtra(IntentWedgeSample.ExtraType);

            //get barcode value
            String data = Intent.GetStringExtra(IntentWedgeSample.ExtraDataString);

            textMsg = (TextView)FindViewById(Resource.Id.textResult);
            //create the message which will hold all of our data sent by the SDK
            string message = "Action: " + action + "\n"
                + "Category: " + category.ToString() + "\n"
                + "Type: " + type + "\n"
                + "Data: " + data;
            //apply the message to the TextView
            textMsg.Append(message);
        }
        private void ShowMessage(String message)
        {
            Toast.MakeText(this, message, ToastLength.Short).Show();
        }
    }
}