﻿using System;
using System.Diagnostics;
using CodeGenHero.BingoBuzz.Xam.Interfaces;
using CodeGenHero.BingoBuzz.Xam.Modules;
using CodeGenHero.BingoBuzz.Xam.ViewModels;
using CodeGenHero.BingoBuzz.Xam.Views;
using Ninject;
using Ninject.Modules;
using Xamarin.Forms;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System.Threading.Tasks;
using CodeGenHero.BingoBuzz.Constants;

namespace CodeGenHero.BingoBuzz.Xam
{
    public partial class App : Application
    {
        private bool _isDemoMode;
        private bool isAppCenterStarted;

        public App(params INinjectModule[] platformModules)
        {
            InitializeComponent();

            var mainPage = new SplashPage() as ContentPage;
            var navPage = new NavigationPage();

            // Register core services
            Kernel = new StandardKernel(new CoreModule(), new NavigationModule(navPage));

            // Register platform specific services
            Kernel.Load(platformModules);

            //initialize the singleton
            var asyncconn = Kernel.Get<ISQLite>().GetAsyncConnection();
            var conn = Kernel.Get<ISQLite>().GetConnection();
            if (conn != null && asyncconn != null)
            {
                var db = Kernel.Get<IDatabase>();
                db.SetConnection(conn, asyncconn);
                db.CreateTables();
            }
            else
            {
                Debug.WriteLine("ERROR: SQLite Database could not be created.");
                throw new InvalidOperationException("ERROR: SQLite Database could not be created.");
            }

            mainPage.BindingContext = Kernel.Get<SplashViewModel>();
            MainPage = mainPage;
        }

        public IKernel Kernel { get; set; }

        public async Task SetDemoMode(bool isOn)
        {
            //if the new value is different from the existing value
            if (_isDemoMode != isOn)
            {
                _isDemoMode = isOn;
                var db = Kernel.Get<IDatabase>();
                await db.DropTablesAsync();
                db.CreateTables();

                if (_isDemoMode)
                {
                    //rebind the data load service so we can load the demo data instead of using the web api
                    Kernel.Rebind<IDataLoadService>().To<Services.Design.DesignDataLoadService>().InSingletonScope();
                }
                else
                {
                    Kernel.Rebind<IDataLoadService>().To<Services.DataLoadService>().InSingletonScope();
                }

                var newDataLoadService = Kernel.Get<IDataLoadService>();
                await newDataLoadService.InsertAllDataCleanLocalDB();
            }
        }

        public void StartAppCenter()
        {
            if (Xamarin.Forms.Device.RuntimePlatform != Xamarin.Forms.Device.UWP)
            {
                if (!isAppCenterStarted)
                {
                    //UI Tests have inconsistent popups for in-app distributions being disabled from side loading - just getting rid of them here to get a build to Xam Test Cloud

                    Type[] p = new Type[2] { typeof(Analytics), typeof(Crashes) };

                    string secrets = $"ios={Consts.AppCenterSecretiOS};android={Consts.AppCenterSecretAndroid};uwp={Consts.AppCenterSecretUWP}";

                    //AppCenter.LogLevel = LogLevel.Verbose;
                    AppCenter.Start(secrets, p);

                    Analytics.TrackEvent("App Center is Started");
                    isAppCenterStarted = true;
                }
            }
            else
            {
                //for now, just start analytics & crashes for UWP in App Center
                Type[] p = new Type[2] { typeof(Analytics), typeof(Crashes) };
                string secrets = $"ios={Consts.AppCenterSecretiOS};android={Consts.AppCenterSecretAndroid};uwp={Consts.AppCenterSecretUWP}";

                //AppCenter.LogLevel = LogLevel.Verbose;
                AppCenter.Start(secrets, p);

                Analytics.TrackEvent("App Center is Started");
            }
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            Debug.WriteLine("BingoBuzz is resuming...");
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            Debug.WriteLine("BingoBuzz is sleeping...");
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            Debug.WriteLine("BingoBuzz is starting up...");
            StartAppCenter();
        }
    }
}