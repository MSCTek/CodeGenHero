using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using CodeGenHero.BingoBuzz.Xam.Interfaces;
using CodeGenHero.BingoBuzz.Xam.Mappers;
using CodeGenHero.BingoBuzz.Xam.ModelData.DemoBB;
using CodeGenHero.BingoBuzz.Xam.ModelObj.BB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Reflection;
using CodeGenHero.BingoBuzz.Xam.Resources;
using CodeGenHero.Logging;

namespace CodeGenHero.BingoBuzz.Xam.ViewModels
{
    public class WelcomeViewModel : CustomViewModelBase
    {
        private bool _hasLoaded;
        private ObservableCollection<Meeting> _meetings;

        public WelcomeViewModel(INavigationService navService, IDataRetrievalService dataRetrievalService, IDataDownloadService dataDownloadService, IStateService stateService, ILoggingService loggingService) 
            : base(navService, dataRetrievalService, dataDownloadService, stateService, loggingService)
        {
            Meetings = new ObservableCollection<Meeting>();
            _hasLoaded = false;
        }

        public ObservableCollection<Meeting> Meetings
        {
            get { return _meetings; }
            set { Set<ObservableCollection<Meeting>>(() => Meetings, ref _meetings, value); }
        }

        public RelayCommand MyProfileCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    await NavService.NavigateTo<ProfileViewModel>();
                });
            }
        }

        public RelayCommand MyStatsCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    await NavService.NavigateTo<StatsViewModel>();
                });
            }
        }

        public RelayCommand NewMeetingCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    await NavService.NavigateTo<NewMeetingViewModel>();
                });
            }
        }

        public RelayCommand<Guid> OpenMeetingCommand
        {
            get
            {
                return new RelayCommand<Guid>(async (Guid meetingId) =>
                {
                    await NavService.NavigateTo<GameViewModel, Guid>(meetingId);
                });
            }
        }

        public override async Task Init()
        {
            if (!_hasLoaded)
            {
                /* TOGGLE DEMO MODE HERE */

                /* DEMO MODE ON */
                //For development only -> Demo mode
                //StateService.SetCurrentUser(DemoUser.UserGeorge.ToModelObj());
                //await ((App)Application.Current).SetModeAndSync(true);

                /* DEMO MODE OFF - BYPASS AZURE AD AUTHENTICATION */
                // Hardcode authentication by doing this - robin's dev user - already in the db
                //await DataDownloadService.InsertOrReplaceAuthenticatedUser(Guid.Parse("b79ed0e3-ddb9-4920-8900-ffc55a73b4b5"));
                //var user = await DataRetrievalService.GetUserByEmailOrNullAsync("robin@msctek.com");

                /* DEMO MODE OFF - USE AZURE AD AUTHENTICATION */
                //Assuming here that we will be using the same user that just authenticated
                //now that the user has logged in, we need to see if they are a bingoBuzz user, if not, we can add them
                //TODO: should probably do a try parse here if we add more providers
                await DataDownloadService.InsertOrReplaceAuthenticatedUser(
                    StateService.GetAuthEmail(),
                    Guid.Parse(StateService.GetAuthId()),
                    StateService.GetAuthGivenName(),
                    StateService.GetAuthSurName()
                );
                var user = await DataRetrievalService.GetUserByEmailOrNullAsync(StateService.GetAuthEmail());

                /* END TOGGLE DEMO MODE HERE */

                if (user != null)
                {
                    StateService.SetCurrentUser(user);
                }
                else
                {
                    string whatHappened = "Can't find this BingoBuzz user!";
                    LoggingService.Error(whatHappened, LogMessageType.Instance.Info_Synchronization);
                    throw new Exception(whatHappened);
                }
                await ((App)Application.Current).SetModeAndSync(user.UserId, false);

                //Load the contents of the welcome page
                Meetings = (await DataRetrievalService.GetMeetingsAsync()).ToObservableCollection();
                _hasLoaded = true;
            }
        }

        public async Task RefreshMeetings()
        {
            Meetings = (await DataRetrievalService.GetMeetingsAsync()).ToObservableCollection();
        }
    }
}