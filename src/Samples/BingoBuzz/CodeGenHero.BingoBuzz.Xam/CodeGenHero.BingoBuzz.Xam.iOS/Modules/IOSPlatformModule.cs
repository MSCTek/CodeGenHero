using CodeGenHero.BingoBuzz.Xam.Interfaces;
using CodeGenHero.BingoBuzz.Xam.iOS.Services;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenHero.BingoBuzz.Xam.iOS.Modules
{
    public class IOSPlatformModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISQLite>()
                 .To<IOSSQLite>()
                 .InSingletonScope();

            //Bind<IMemoryService>()
            //    .To<IOSMemoryService>()
            //    .InSingletonScope();
        }
    }
}