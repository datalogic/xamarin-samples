using System;
using System.Threading;
using Android.App;
using Android.OS;
using Android.Util;
using Android.Widget;
using Com.Datalogic.Device.Input;

namespace DeviceSampleAPI
{
    //this is a class whose primary goal is to showcase the TouchManager's lockInput method which prevents further input for a specified period of time
    [Activity(Label = "TouchActivity")]
    public class TouchActivity : Activity
    {
        //the first 4 buttons show a toast when pressed and are there to show that when you perform a Touchmanager's lockInput method no further input is allowed
        private Button btn1, btn2, btn3, btn4, btnTLock;
        private TouchManager tm;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_touch);
            //create our button and add our event handler
            btn1 = (Button)FindViewById(Resource.Id.button1);
            btn1.Click += Button1Clicked;
            //create our button and add our event handler
            btn2 = (Button)FindViewById(Resource.Id.button2);
            btn2.Click += Button2Clicked;
            //create our button and add our event handler
            btn3 = (Button)FindViewById(Resource.Id.button3);
            btn3.Click += Button3Clicked;
            //create our button and add our event handler
            btn4 = (Button)FindViewById(Resource.Id.button4);
            btn4.Click += Button4Clicked;
            //btnTLock is button whose event handler calls TouchManager's lockInput method.
            btnTLock = (Button)FindViewById(Resource.Id.btnTLock);
            btnTLock.Click += btnTLockOnClick;

            try
            {
                tm = new TouchManager();
            } catch(Exception exception)
            {
                Log.Error(this.GetType().Name, "While creating activity");
                return;
            }
        }

        //these next 4 methods are click handlers to our 4 buttons which are arranged in each corner.  
        //These buttons each open up a toast, and are used to prove that the thread is locked if the user clicks the "Lock Touch for 2 seconds" button.
        private void Button1Clicked(object sender, EventArgs e)
        {
            Toast.MakeText(this, "Button 1 clicked", ToastLength.Short).Show();
        }
        private void Button2Clicked(object sender, EventArgs e)
        {
            Toast.MakeText(this, "Button 2 clicked", ToastLength.Short).Show();
        }
        private void Button3Clicked(object sender, EventArgs e)
        {
            Toast.MakeText(this, "Button 3 clicked", ToastLength.Short).Show();
        }
        private void Button4Clicked(object sender, EventArgs e)
        {
            Toast.MakeText(this, "Button 4 clicked", ToastLength.Short).Show();
        }
        
        //this method is designed to show how you can lock the application, preventing input for a particular amount of time
        private void btnTLockOnClick(object sender, EventArgs e) 
        {        
            Thread myThread = new Thread(() =>
            {
                try
                {
                    //Lock the input which prevents any further inputs until the thread is unlocked
                    tm.LockInput(true);                    
                    //here we are choosing how long we would like to have our thread's input locked for.
                    Thread.Sleep(2000);
                    //now we unlock the input which will cause inputs to no longer be blocked
                    tm.LockInput(false);

                } catch(Exception exception)
                {
                    //If there was an error then log that error
                    Log.Error(this.GetType().Name, "run in LockThread");
                }
            });
            //Start the thread in question
            myThread.Start();
        }
    }
}