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

namespace CodeGenHero.BingoBuzz.Xam.ViewModels
{
    public class WelcomeViewModel : CustomViewModelBase
    {
        private bool _hasLoaded;
        private ObservableCollection<Meeting> _meetings;

        public WelcomeViewModel(INavigationService navService, IDataRetrievalService dataRetrievalService, IDataDownloadService dataDownloadService, IStateService stateService) : base(navService, dataRetrievalService, dataDownloadService, stateService)
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
                /* TOGGLE DEMO MODE */

                /* DEMO MODE ON */
                //For development only -> Demo mode
                //StateService.SetCurrentUser(DemoUser.UserGeorge.ToModelObj());
                //await ((App)Application.Current).SetModeAndSync(true);

                /* DEMO MODE OFF */
                //TODO: add authentication, for now, we will hardcode it
                await DataDownloadService.InsertOrReplaceAuthenticatedUser(Guid.Parse("b79ed0e3-ddb9-4920-8900-ffc55a73b4b5"));
                
                var user = await DataRetrievalService.GetUserByEmailOrNullAsync("robin@msctek.com");
                if (user != null)
                {
                    StateService.SetCurrentUser(user);
                }
                else
                {
                    //go get the user from the API, this user doesn't have any meetings assigned to them
                    //TODO
                }
                await ((App)Application.Current).SetModeAndSync(false);
                
                /* END TOGGLE DEMO MODE */

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