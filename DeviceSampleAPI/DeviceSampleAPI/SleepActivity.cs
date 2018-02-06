using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using Com.Datalogic.Device;
using Com.Datalogic.Device.Power;

namespace DeviceSampleAPI
{
    //this is a class whose job is to set the power configuration and wakeup sources chosen by the user.  
    [Activity(Label = "SleepActivity")]
    public class SleepActivity : Activity
    {
        private static String[] timeouts = null;
        private static SuspendTimeout[] timeoutVals = null;
        private String[] sources = null;
        //this member variable hows the various WakeupSources chosen from the ListView for avaialable sources
        private static List<WakeupSource> sourceVals = new List<WakeupSource>();

        private TextView txtSleep;
        private ListView listSuspendTimeout;
        private ListView listWakeupSource;

        private Com.Datalogic.Device.Power.PowerManager pm;
        private List<WakeupSource> sourceList = new List<WakeupSource>();


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_sleep);
            //set ErrorManager to all exceptions
            ErrorManager.EnableExceptions(true);

            try
            {
                //create our PowerManager for later use
                pm = new Com.Datalogic.Device.Power.PowerManager();
            } catch(DeviceException exception)
            {
                Log.Error(this.GetType().Name, "While creating activity"); //TODO IN ANDROID VERSION THIS PASSES IN THE EXCEPTION THROWN...HOW DO THAT IN XAMARIN
            }
            //this will hold our sleeping configuration data
            txtSleep = (TextView)FindViewById(Resource.Id.txtSleep);
            //load available timeouts
            setTimeouts();
            //load wakeup sources
            setSources();

            //here we create an array adapter which we will use to hold our timouts avaiable for each source
            ArrayAdapter<String> timeoutAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, timeouts);
            //Here we create an array adapter which we will use to hold our sources avaialable which will be used with the timeouts
            ArrayAdapter<String> sourceAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, sources);

            //create our timeout list view, set our adapter for our timeouts and wire up our event handler
            listSuspendTimeout = (ListView)FindViewById(Resource.Id.listSuspendTimeout);
            listSuspendTimeout.Adapter = timeoutAdapter;
            listSuspendTimeout.ItemClick += TimeoutListListener;
            //create our sources list view, set our adapter for our sources, and wire up our event handler
            listWakeupSource = (ListView)FindViewById(Resource.Id.listWakeupSource);
            listWakeupSource.Adapter = sourceAdapter;
            listWakeupSource.ItemClick += SourceListListener;


        }
        //This method sets the text for our TextView with description data obtained from the user clicking on our ListViews for both timeouts and sources
        private void setText()
        {
            txtSleep.Text = getDescription();
        }
        //this method builds a string of the various timeouts and sources the user clicked from the ListViews
        private string getDescription()
        {
            //create our string builder
            StringBuilder outVal = new StringBuilder();

            try
            {
                //add our suspending timeouts from the PowerManager to the String Builder
                outVal.Append("Suspend Timeout (external): " + pm.GetSuspendTimeout(true) + "\n");
                outVal.Append("Suspend Timeout (internal): " + pm.GetSuspendTimeout(false) + "\n");
                //iterate over each WakeupSource and adds the source and the timeout for the sources to the string builder
                foreach(WakeupSource currentWakeupSource in sourceVals)
                {
                    outVal.Append("isWakeupActive");
                    outVal.Append("(" + currentWakeupSource.Name() + "): ");
                    outVal.Append("" + pm.IsWakeupActive(currentWakeupSource) + "\n");
                }
                try
                {
                    //here we try to print what the reason was for the last wakeup
                    outVal.Append("getWakeupReason: " + pm.WakeupReason);
                } catch(Exception e)
                {
                    Log.Error(this.GetType().Name, "Did the device go to sleep?"); 
                }
            } catch(Exception exception)
            {
                Log.Error(this.GetType().Name, "getDescription"); 
            }
            //return the string builders string
            return outVal.ToString();
        }
        //here we set the available sources.  These sources will be used in the "sources" array and will be used by the ArrayAdapter
        private void setSources()
        {
            if(sources == null)
            {
                //Get an array of the available sources
                WakeupSource[] definedSources = WakeupSource.Values();
                //iterate over all the sources and if wakeup is supported for the source then add them to the sourceVals member variable
                for(int i = 0; i < definedSources.Length; i++)
                {
                    if (pm.IsWakeupSupported(definedSources[i]))
                    {
                        sourceVals.Add(definedSources[i]);
                    }
                }
                sources = new String[sourceVals.Count()];
                //for each of the sources we found that are wakeable add each to the "sources" member variable which will beused by the ArrayAdapter
                for(int i = 0; i < sources.Length; i++)
                {
                    sources[i] = sourceVals.ElementAt(i).Name();
                }
            }
        }
        //this method sets our timeout values from our SuspendTimeout class
        private void setTimeouts()
        {
            if(timeouts == null)
            {
                timeoutVals = SuspendTimeout.Values();
                timeouts = new String[timeoutVals.Length];
                for(int i = 0; i < timeouts.Length; i++)
                {
                    timeouts[i] = "" + timeoutVals[i].Name();
                }
            }
        }
        //This handles the event where a user clicks on a menu item
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.reset, menu);
            return base.OnPrepareOptionsMenu(menu);
        }
        //this is the event handler that is called when someone clicks on a menu item
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_reset:
                    setText();
                    clearWakeup();
                    break;
                default:
                    return base.OnOptionsItemSelected(item);               
            }
            return base.OnOptionsItemSelected(item);
        }
        //this is a method that is called when someone goes to a different activity through the menu item.  It's purpose is to remove our need to wakup from our sleep for this activity.
        private void clearWakeup()
        {
            foreach(var source in sourceList)
            {
                try
                {
                    pm.ClearWakeup(source);
                }
                catch(DeviceException exception){
                    Log.Error(this.GetType().Name, "clearWakeup"); 
                }
                sourceList.Clear();

                try
                {
                    //This adds a delay to our method 
                    Thread.Sleep(100);
                }
                catch(ThreadInterruptedException exception){}
                //this set the text that shows what sources and values for those sources we have configured through the app
                setText();
            }
        }
        //Here we set the source for our sleep
        private void SourceListListener(object sender, AdapterView.ItemClickEventArgs e)
        {
            try
            {
                if (!pm.IsWakeupActive(sourceVals.ElementAt(e.Position)))
                {
                    //tell the power manager what source button should activate the wakeup
                    pm.ActivateWakeup(sourceVals.ElementAt(e.Position));
                }
                else
                {
                    //here we would clear the current source value if they click the source button but it has been clicked before
                    pm.ClearWakeup(sourceVals.ElementAt(e.Position));
                }
                //add the either newly added source or newly removed source from the available source lists
                sourceList.Add(sourceVals.ElementAt(e.Position));
            }
            catch (DeviceException exception)
            {
                Log.Error(this.GetType().Name, "onItemClick");
            }

            try
            {
                Thread.Sleep(100);
            }
            catch (ThreadInterruptedException exception) { } 

            //this sets the text that shows the current sources for wakeup and their values
            setText();
        }
        //Here is our event handler that sets how long your would like to sleep for for the source
        private void TimeoutListListener(object sender, AdapterView.ItemClickEventArgs e)
        {

            try
            {
                //here we set the suspend timeout value which will set this for the whole android device (verifiable in setttings)
                pm.SetSuspendTimeout(timeoutVals[e.Position], true);
            }
            catch (DeviceException exception)
            {
                Log.Error(this.GetType().Name, "onItemClick"); 
            }

            try
            {
                //Make the app sleep for 100 milliseconds
                Thread.Sleep(100);
            }
            catch (ThreadInterruptedException exception){}
            setText();
        }

    }
}