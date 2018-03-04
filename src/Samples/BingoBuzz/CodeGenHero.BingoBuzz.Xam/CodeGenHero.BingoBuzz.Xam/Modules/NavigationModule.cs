using CodeGenHero.BingoBuzz.Xam.Interfaces;
using CodeGenHero.BingoBuzz.Xam.Services;
using CodeGenHero.BingoBuzz.Xam.ViewModels;
using CodeGenHero.BingoBuzz.Xam.Views;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CodeGenHero.BingoBuzz.Xam.Modules
{
    internal class NavigationModule : NinjectModule
    {
        private readonly INavigation _xfNav;
        private NavigationPage _navigationPage;

        public NavigationModule(NavigationPage navPage)
        {
            _navigationPage = navPage;
            _xfNav = navPage.Navigation;
        }

        public override void Load()
        {
            var navService = new NavigationService();

            RefreshNavModule(navService);

            if (Device.Idiom == TargetIdiom.Phone)
            {
                //for the phone
                //navService.RegisterViewMapping(typeof(myViewModel), typeof(myPage));
            }
            else
            {
                //for the tablet & desktop
                //navService.RegisterViewMapping(typeof(myViewModel), typeof(myPage));
            }

            //Mappings for Views and ViewModels that do not change for tablets or phones
            navService.RegisterViewMapping(typeof(SplashViewModel), typeof(SplashPage));
            navService.RegisterViewMapping(typeof(WelcomeViewModel), typeof(WelcomePage));
            navService.RegisterViewMapping(typeof(ProfileViewModel), typeof(ProfilePage));
            navService.RegisterViewMapping(typeof(GameViewModel), typeof(GamePage));
            navService.RegisterViewMapping(typeof(NewMeetingViewModel), typeof(NewMeetingPage));
            navService.RegisterViewMapping(typeof(StatsViewModel), typeof(StatsPage));

            Bind<INavigationService>().ToMethod(x => navService).InSingletonScope();
        }

        public void RefreshNavModule(NavigationService navService)
        {
            navService.NavigationPage = _navigationPage;
            navService.XamarinFormsNav = _navigationPage.Navigation;
        }
    }
}