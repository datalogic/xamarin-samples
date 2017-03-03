// ©2017 Datalogic Inc. and/or its affiliates. All rights reserved.

using Android.App;
using Android.Runtime;
using Com.Datalogic.Extension.Selfshopping.Cradle.Joyatouch;
using System;

namespace JoyaTouchCradleSampleAPI
{
    [Application]
    public class JoyaTouchCradleApplication : Application, IJavaObject
    {
        /**
         * CradleJoyaTouch instance shared by the sample activities.
         */
        private ICradleJoyaTouch jtCradle = null;

        public JoyaTouchCradleApplication(IntPtr handle, JniHandleOwnership transfer)
            : base(handle,transfer)
        {
            // do any initialisation you want here (for example initialising properties)
        }

        public ICradleJoyaTouch CradleJoyaTouch
        {
            get { return jtCradle; }
            set { jtCradle = value; }
        }
    }

}