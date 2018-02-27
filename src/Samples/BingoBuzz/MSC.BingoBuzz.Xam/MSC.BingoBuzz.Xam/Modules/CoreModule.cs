using CodeGenHero.EAMVCXamPOCO.Interface;
using MSC.BingoBuzz.Xam.Interfaces;
using MSC.BingoBuzz.Xam.ViewModels;
using Ninject.Modules;

namespace MSC.BingoBuzz.Xam.Modules
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