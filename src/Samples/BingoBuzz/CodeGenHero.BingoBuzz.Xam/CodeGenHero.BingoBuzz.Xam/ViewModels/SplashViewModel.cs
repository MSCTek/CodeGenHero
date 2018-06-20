using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CodeGenHero.Logging;
using CodeGenHero.BingoBuzz.Xam.Interfaces;
using CodeGenHero.BingoBuzz.Xam.Views;
using Newtonsoft.Json.Linq;

namespace CodeGenHero.BingoBuzz.Xam.ViewModels
{
    public class SplashViewModel : CustomViewModelBase<JObject>
    {
        public SplashViewModel(INavigationService navService, IDataRetrievalService dataRetrievalService, IDataDownloadService dataDownloadService, IStateService stateService, ILoggingService loggingService)
            : base(navService, dataRetrievalService, dataDownloadService, stateService, loggingService)
        {
        }

        public override async Task Init(JObject authenticationObject)
        {
            if (authenticationObject != null)
            {
                //set this in the StateService, effectively logging the user in
                StateService.SetAuthenticationObject(authenticationObject);

                await NavService.StartNavStack(typeof(WelcomeViewModel));
            }
            else
            {
                throw new NotSupportedException("User is not logged in.");
            }
        }
    }
}