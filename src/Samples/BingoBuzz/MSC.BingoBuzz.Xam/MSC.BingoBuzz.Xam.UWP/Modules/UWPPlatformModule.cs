using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSC.BingoBuzz.Xam.UWP.Modules
{
    public class UWPPlatformModule : NinjectModule
    {
        public override void Load()
        {
            //Bind<ISQLite>()
            //      .To<UWPSQLite>()
            //      .InSingletonScope();

            // Bind<IMemoryService>()
            //    .To<UWPMemoryService>()
            //    .InSingletonScope();
        }
    }
}