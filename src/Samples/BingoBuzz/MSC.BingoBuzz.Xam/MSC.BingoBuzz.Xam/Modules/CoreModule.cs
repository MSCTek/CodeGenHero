﻿using MSC.BingoBuzz.Xam.Interfaces;
using MSC.BingoBuzz.Xam.ViewModels;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSC.BingoBuzz.Xam.Modules
{
    public class CoreModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDataService>().To<Services.DataService>().InSingletonScope();
            //Bind<IDatabase>().To<SQLiteDB.Database>().InSingletonScope();

            Bind<WelcomeViewModel>().ToSelf().InSingletonScope();
            Bind<ProfileViewModel>().ToSelf().InSingletonScope();
            Bind<StatsViewModel>().ToSelf().InSingletonScope();
            Bind<GameViewModel>().ToSelf().InSingletonScope();
        }
    }
}