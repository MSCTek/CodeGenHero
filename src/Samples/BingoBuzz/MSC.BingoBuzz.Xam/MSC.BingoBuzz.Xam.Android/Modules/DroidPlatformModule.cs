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
using MSC.BingoBuzz.Xam.Droid.Services;
using MSC.BingoBuzz.Xam.Interfaces;
using Ninject.Modules;

namespace MSC.BingoBuzz.Xam.Droid.Modules
{
    public class DroidPlatformModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISQLite>()
                .To<DroidSQLite>()
                .InSingletonScope();

            //Bind<IMemoryService>()
            //   .To<DroidMemoryService>()
            //   .InSingletonScope();
        }
    }
}