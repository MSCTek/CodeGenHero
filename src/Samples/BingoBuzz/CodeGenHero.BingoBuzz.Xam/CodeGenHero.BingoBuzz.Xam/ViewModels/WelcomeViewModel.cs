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

        public WelcomeViewModel(INavigationService navService, IDataRetrievalService dataRetrievalService, IStateService stateService) : base(navService, dataRetrievalService, stateService)
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
                //TODO: remove - for development only
                await ((App)Application.Current).SetDemoMode(false);
                StateService.SetCurrentUser(DemoUser.UserGeorge.ToModelObj());

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