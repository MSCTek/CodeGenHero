using CodeGenHero.BingoBuzz.Xam.Interfaces;
using CodeGenHero.BingoBuzz.Xam.ViewModels;
using CodeGenHero.Logging;
using Ninject.Modules;

namespace CodeGenHero.BingoBuzz.Xam.Modules
{
    public class CoreModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDataLoadService>().To<Services.DataLoadService>().InSingletonScope();
            Bind<IDataRetrievalService>().To<Services.DataRetrievalService>().InSingletonScope();
            Bind<IStateService>().To<Services.StateService>().InSingletonScope();
            Bind<IDatabase>().To<Database.Database>().InSingletonScope();
            Bind<ILoggingService>().To<Services.LoggingService>().InSingletonScope();

            Bind<SplashViewModel>().ToSelf().InSingletonScope();
            Bind<WelcomeViewModel>().ToSelf().InSingletonScope();
            Bind<ProfileViewModel>().ToSelf().InSingletonScope();
            Bind<StatsViewModel>().ToSelf().InSingletonScope();
            Bind<GameViewModel>().ToSelf().InSingletonScope();
            Bind<NewMeetingViewModel>().ToSelf().InSingletonScope();
        }
    }
}