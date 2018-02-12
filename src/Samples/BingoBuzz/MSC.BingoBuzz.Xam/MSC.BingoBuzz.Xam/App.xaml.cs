using System;
using System.Diagnostics;
using MSC.BingoBuzz.Xam.Interfaces;
using MSC.BingoBuzz.Xam.Modules;
using MSC.BingoBuzz.Xam.ViewModels;
using MSC.BingoBuzz.Xam.Views;
using Ninject;
using Ninject.Modules;
using Xamarin.Forms;

namespace MSC.BingoBuzz.Xam
{
    public partial class App : Application
    {
        public App(params INinjectModule[] platformModules)
        {
            InitializeComponent();

            var mainPage = new SplashPage() as ContentPage;

            var navPage = new NavigationPage();

            // Register core services
            Kernel = new StandardKernel(
                new CoreModule(),
                new NavigationModule(navPage));

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

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            //StartAppCenter();
        }

        /*   public void StartAppCenter()
           {
               if (Xamarin.Forms.Device.RuntimePlatform != Xamarin.Forms.Device.UWP)
               {
                   if (!isAppCenterStarted)
                   {
                       //UI Tests have inconsistent popups for in-app distributions being disabled from side loading - just getting rid of them here to get a build to Xam Test Cloud
   #if (UITEST)
                       Type[] p = new Type[3] { typeof(Analytics), typeof(Crashes), typeof(Push) };
   #else
                       Type[] p = new Type[4] { typeof(Analytics), typeof(Crashes), typeof(Distribute), typeof(Push) };
   #endif
                       string secrets = $"ios={Constants.Constants.MobileCenterAppSecretiOS};android={Constants.Constants.MobileCenterAppSecretDroid};uwp={Constants.Constants.MobileCenterAppSecretUWP}";

                       //AppCenter.LogLevel = LogLevel.Verbose;
                       AppCenter.Start(secrets, p);

                       //ordinarily we would do this explicity and not with anon, but it should only be unsubscribed to when the app closes, so it doesn't matter
                       Push.PushNotificationReceived += (sender, e) =>
                       {
                           // Add the notification message and title to the message
                           var summary = $"Push notification received:" +
                                                   $"\n\tNotification title: {e.Title}" +
                                                   $"\n\tMessage: {e.Message}";
                           // If there is custom data associated with the notification,
                           // print the entries
                           if (e.CustomData != null)
                           {
                               summary += "\n\tCustom data:\n";
                               foreach (var key in e.CustomData.Keys)
                               {
                                   summary += $"\t\t{key} : {e.CustomData[key]}\n";
                               }
                           }

                           // Send the notification summary to debug output
                           System.Diagnostics.Debug.WriteLine(summary);
                           Analytics.TrackEvent($"Push Notification Received: { e.Title}");

                           CrossLocalNotifications.Current.Show(e.Title, e.Message);
                       };

                       Analytics.TrackEvent("App Center is Started");
                       isAppCenterStarted = true;
                   }
               }
               else
               {
                   //for now, just start analytics for UWP in App Center
                   Type[] p = new Type[1] { typeof(Analytics) };
                   string secrets = $"ios={Constants.Constants.MobileCenterAppSecretiOS};android={Constants.Constants.MobileCenterAppSecretDroid};uwp={Constants.Constants.MobileCenterAppSecretUWP}";

                   //AppCenter.LogLevel = LogLevel.Verbose;
                   AppCenter.Start(secrets, p);

                   Analytics.TrackEvent("App Center Analytics is the only service functional for UWP right now");
               }
           }*/
    }
}