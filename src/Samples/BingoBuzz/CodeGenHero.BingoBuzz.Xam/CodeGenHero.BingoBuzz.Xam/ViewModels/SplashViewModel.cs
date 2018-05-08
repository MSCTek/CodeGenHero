using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CodeGenHero.BingoBuzz.Xam.Interfaces;
using CodeGenHero.BingoBuzz.Xam.Views;

namespace CodeGenHero.BingoBuzz.Xam.ViewModels
{
    public class SplashViewModel : CustomViewModelBase
    {
        public SplashViewModel(INavigationService navService, IDataRetrievalService dataRetrievalService, IDataDownloadService dataDownloadService, IStateService stateService) : base(navService, dataRetrievalService, dataDownloadService, stateService)
        {
        }

        public override async Task Init()
        {
            await NavService.StartNavStack(typeof(WelcomeViewModel));
        }
    }
}