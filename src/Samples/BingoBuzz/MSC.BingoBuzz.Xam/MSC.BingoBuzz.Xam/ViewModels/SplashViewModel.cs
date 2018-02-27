using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSC.BingoBuzz.Xam.Interfaces;
using MSC.BingoBuzz.Xam.Views;

namespace MSC.BingoBuzz.Xam.ViewModels
{
    public class SplashViewModel : CustomViewModelBase
    {
        public SplashViewModel(INavigationService navService, IDataRetrievalService dataRetrievalService, IStateService stateService) : base(navService, dataRetrievalService, stateService)
        {
        }

        public override async Task Init()
        {
            await NavService.StartNavStack(typeof(WelcomeViewModel));
        }
    }
}