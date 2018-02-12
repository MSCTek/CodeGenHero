using MSC.BingoBuzz.Xam.Interfaces;
using MSC.BingoBuzz.Xam.iOS.Services;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSC.BingoBuzz.Xam.iOS.Modules
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