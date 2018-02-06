using System;
using Android.App;
using Android.OS;
using Android.Util;
using Android.Widget;
using Com.Datalogic.Device;

namespace DeviceSampleAPI
{
    //This is a class that allows the user to either reset their device, do a factory reset, or an enterprise reset of their Datalogic scanning device
    [Activity(Label = "ResetActivity")]
    public class ResetActivity : Activity
    {
        private static String[] listArray = null;
        private static BootType[] bootTypes = null;

        private ListView listReset;
        private Com.Datalogic.Device.Power.PowerManager pm; 

        //called when the activity is created
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_reset);

            ErrorManager.EnableExceptions(true);

            try
            {              
                pm = new Com.Datalogic.Device.Power.PowerManager();
            }
            catch(DeviceException exception)
            {
                Log.Error(this.GetType().Name, "While creating ResetActivity");
            }
            //set listArray with types of device resetting
            setArray();
            //create an array adapter which will hold our data for our ListView
            ArrayAdapter<Object> adapter = new ArrayAdapter<object>(this, Android.Resource.Layout.SimpleListItem1, listArray);
            //create our list view
            listReset = (ListView)FindViewById(Resource.Id.listReset);
            //set it's adapter to be our custom adapter we just created
            listReset.Adapter = adapter;
            //Here we set the event handler that is called when an item in our List View is clicked
            listReset.ItemClick += resetEventHandler;
            
        }
        //this method sets the listArray with the avaliable BootTypes.  These will allow different types of reset, either Enterprise, Factory, or regular reset 
        private void setArray()
        {
            if(listArray == null || bootTypes == null)
            {
                //Get the various Boot Types from the BootType enum
                bootTypes = BootType.Values();
                //create a data structure to hold our various Boot Types
                listArray = new String[bootTypes.Length];
                //Loop over our Boot Types and add each to our new Boot Types data structure
                for(int i = 0; i < bootTypes.Length; i++)
                {
                    String bootTypeName = bootTypes[i].Name();
                    listArray[i] = bootTypes[i].Name();
                }
            }
        }
        //this method is called when a user clicks on an item in the ListView
        private void resetEventHandler(object sender, AdapterView.ItemClickEventArgs e)
        {
            try
            {
                //The power manager reboots the android device.  The reboot type depends on what the user clicked on in the List View
                pm.Reboot(bootTypes[e.Position]);
            }
            catch(DeviceException exception)
            {
                //There was a device exception so we should log this
                Log.Error(this.GetType().Name, "While onItemClick"); 
            }
        }

    }
}