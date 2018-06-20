using GalaSoft.MvvmLight;
using CodeGenHero.BingoBuzz.Xam.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CodeGenHero.Logging;
using CodeGenHero.BingoBuzz.Xam.ModelObj.BB;

namespace CodeGenHero.BingoBuzz.Xam.ViewModels
{
    public class ProfileViewModel : CustomViewModelBase
    {
        public ProfileViewModel(INavigationService navService, IDataRetrievalService dataRetrievalService, IDataDownloadService dataDownloadService, IStateService stateService, ILoggingService loggingService)
            : base(navService, dataRetrievalService, dataDownloadService, stateService, loggingService)
        {
        }

        public User CurrentUser
        {
            get { return _currentUser; }
            set { Set(ref _currentUser, value); }
        }
        public Company CurrentCompany
        {
            get { return _currentCompany; }
            set { Set(ref _currentCompany, value); }
        }

        private Company _currentCompany;
        private User _currentUser;

        public override async Task Init()
        {
            await NavService.PushAlertPopupAsync("Loading...");

            CurrentUser = StateService.GetCurrentUser();
            CurrentCompany = await DataRetrievalService.GetCompanyByIdOrNull(CurrentUser.CompanyId);

            await NavService.PopAlertPopupsAsync();
        }
    }
}