using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace SkiResort.Droid
{
    [Activity(Label = "SkiResort", Icon = "@mipmap/icon", 
              Theme = "@style/MainTheme", MainLauncher = false, 
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            this.Window.AddFlags(WindowManagerFlags.Fullscreen);
            this.Window.DecorView.SystemUiVisibility = StatusBarVisibility.Hidden;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
    }
}