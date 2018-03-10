﻿using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using Plugin.Toasts;

namespace CodeGenHero.BingoBuzz.Xam.Droid
{
    [Activity(Label = "BingoBuzz", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            DependencyService.Register<ToastNotification>(); // Register your dependency
            ToastNotification.Init(this);

            Rg.Plugins.Popup.Popup.Init(this, bundle);

            LoadApplication(new App(new Modules.DroidPlatformModule()));
            //LoadApplication(new App());
        }
    }
}