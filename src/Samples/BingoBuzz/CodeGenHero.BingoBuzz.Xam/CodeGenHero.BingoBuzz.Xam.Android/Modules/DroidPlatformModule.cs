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
using CodeGenHero.BingoBuzz.Xam.Interfaces;
using CodeGenHero.BingoBuzz.Xam.Droid.Services;
using Ninject.Modules;

namespace CodeGenHero.BingoBuzz.Xam.Droid.Modules
{
    public class DroidPlatformModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISQLite>()
                .To<DroidSQLite>()
                .InSingletonScope();

            Bind<IMemoryReporterService>()
               .To<DroidMemoryReporterService>()
               .InSingletonScope();
        }
    }
}