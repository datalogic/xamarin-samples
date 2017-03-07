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
    [Activity(Label = "CradleCustomAreaActivity")]
    public class CradleCustomAreaActivity : Activity
    {
        private byte[] customValues;

        private EditText text;

        private ICradleJoyaTouch jtCradle;

        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_cradle_custom_area);

            text = (EditText)FindViewById(Resource.Id.customValuesText);

            JoyaTouchCradleApplication application = (JoyaTouchCradleApplication)ApplicationContext;
            jtCradle = application.CradleJoyaTouch;

            if (savedInstanceState == null)
            {
                CustomArea custom = new CustomArea();
                if (jtCradle.ReadCustomArea(custom, custom.Size))
                {
                    customValues = custom.GetContent();
                    setTextUTF();
                }
                else
                {
                    Toast.MakeText(this, "Failure reading custom area. Retry.", ToastLength.Long).Show();
                }
            }

            // Handle READ button press
            Button readButton = FindViewById<Button>(Resource.Id.buttonReadCustom);
            readButton.Click += delegate
            {
                CustomArea custom = new CustomArea();
                if (jtCradle.ReadCustomArea(custom, custom.Size))
                {
                    customValues = custom.GetContent();
                    setTextUTF();
                }
                else
                {
                    Toast.MakeText(this, "Failure reading custom area. Retry.", ToastLength.Long).Show();
                }
            };

            // Handle CLEAR button press
            Button clearButton = FindViewById<Button>(Resource.Id.buttonClearCustom);
            clearButton.Click += delegate
            {
                CustomArea custom = new CustomArea();
                customValues = custom.GetContent();
                for (int i = 0; i < customValues.Length; i++)
                    customValues[i] = 0;
                if (jtCradle.WriteCustomArea(custom, custom.Size))
                {
                    text.Text = "";
                }
                else
                {
                    Toast.MakeText(this, "Failure clearing custom area. Retry.", ToastLength.Long).Show();
                }
            };

            // Handle WRITE button press
            Button writeButton = FindViewById<Button>(Resource.Id.buttonWriteCustom);
            writeButton.Click += delegate
            {
                try
                {
                    customValues = System.Text.Encoding.UTF8.GetBytes(text.Text);
                }
                catch (UnsupportedEncodingException)
                {
                    Toast.MakeText(this, "Wrong conversion of the UTF-8 string into custom area bytes.",
                            ToastLength.Long).Show();
                    return;
                }

                CustomArea custom = new CustomArea((byte[])(Array)customValues);

                if (customValues == null || customValues.Length == 0 || customValues.Length > custom.Size)
                {
                    Toast.MakeText(this, "Invalid custom area bytes size.", ToastLength.Long).Show();
                    return;
                }

                if (jtCradle.WriteCustomArea(custom, custom.Size))
                    Toast.MakeText(this, "Custom data written successfully.", ToastLength.Long).Show();
                else
                    Toast.MakeText(this, "Failure writing custom area. Retry.", ToastLength.Long).Show();
            };
        }

        void setTextUTF()
        {
            // Count the number of bytes before finding a 0
            int numBytes = 0;
            for (int i = 0; i < customValues.Length; i++)
            {
                if (customValues[i] == (byte)0)
                {
                    numBytes = i;
                    break;
                }
            }

            if (numBytes != 0)
            {
                byte[] customValuesValid = new byte[numBytes];
                System.Array.Copy(customValues, 0, customValuesValid, 0, numBytes);
                try
                {
                    string utfValues = System.Text.Encoding.UTF8.GetString(customValuesValid);
                    text.Text = utfValues;
                    
                }
                catch (UnsupportedEncodingException)
                {
                    Toast.MakeText(this, "Wrong conversion of the custom area bytes into a UTF-8 string.",
                            ToastLength.Long).Show();
                }
            }
            else
            {
                Toast.MakeText(this, "Custom area contains no bytes before the first null value.",
                        ToastLength.Long).Show();
            }
        }

    }
}