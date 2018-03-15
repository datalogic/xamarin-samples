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

namespace DecodeIntent
{   /*
    This activity is called from our broadcast receiver:DecodeWedgeIntentReceiver
    defined in IntentWedgeSample.  This activity is called from the broadcast
    receiver and simply is a way to show the data sent to the broadcast receiver
    through a nice user interface
    */
    [Activity(Label = "ActivityStartedFromBroadcast")]
    public class ActivityStartedFromBroadcast : Activity
    {
        TextView textMsg;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_started_from_broadcast);
            // Create your application here
            textMsg = (TextView)FindViewById(Resource.Id.textResult);

            //data is the bardcode string, type is the barcode type.
            //these are all sent from the broadcast receiver which obtained
            //it's barcode data from the sdk after the sdk scanned the barcode
            //and collected the barcode data
            String data = Intent.GetStringExtra(IntentWedgeSample.ExtraDataString);
            String type = Intent.GetStringExtra(IntentWedgeSample.ExtraType);
            String category = Intent.GetStringExtra("category");

            string message = "Category: " + category.ToString() + "\n"
                + "Type: " + type + "\n"
                + "Data: " + data;

            textMsg.Append(message);
        }
    }
}