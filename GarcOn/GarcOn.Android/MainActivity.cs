using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using ImageCircle.Forms.Plugin.Droid;
using Android.Content;
using System.Diagnostics;

namespace GarcOn.Droid
{
    [Activity(MainLauncher = true, Label = "GarcOn", Icon = "@drawable/garcon_icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [IntentFilter(new [] { Intent.ActionMain }, Categories = new [] { Intent.CategoryHome, Intent.CategoryDefault })]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            SetStatusBarColor(Android.Graphics.Color.Rgb(25, 25, 19));

            base.OnCreate(savedInstanceState);

            Plugin.InputKit.Platforms.Droid.Config.Init(this, savedInstanceState);

            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            ImageCircleRenderer.Init();

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }

        Stopwatch stopWatch = new Stopwatch();
        public int countClick = 0;

        //Disable back button
        public override void OnBackPressed()
        {
            //10 clicks in 5 seconds, close app
            countClick++;

            if (countClick == 1)
            {
                stopWatch.Start();
                return;
            }

            if (countClick == 10 && stopWatch.Elapsed <= TimeSpan.FromSeconds(5))
            {
                countClick = 0;
                stopWatch.Reset();

                Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
            }
            else if (stopWatch.Elapsed > TimeSpan.FromSeconds(5))
            {
                countClick = 0;
                stopWatch.Reset();
            }
        }

        //Disable status bar
        public override void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);

            if (!hasFocus)
            {
                Intent closeDialog = new Intent(Intent.ActionCloseSystemDialogs);
                SendBroadcast(closeDialog);
            }
        }

        //Disable recent button
        protected override void OnPause()
        {
            base.OnPause();

            ActivityManager activityManager = (ActivityManager)ApplicationContext
                    .GetSystemService(Context.ActivityService);

            activityManager.MoveTaskToFront(TaskId, 0);
        }
    }
}