using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MSC.BingoBuzz.Xam.Interfaces;
using MSC.BingoBuzz.Xam.Mappers;
using MSC.BingoBuzz.Xam.ModelObj.BB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MSC.BingoBuzz.Xam.ViewModels
{
    public class WelcomeViewModel : CustomViewModelBase
    {
        private ObservableCollection<Meeting> _meetings;

        public WelcomeViewModel(INavigationService navService, IDataService dataService) : base(navService, dataService)
        {
            Meetings = new ObservableCollection<Meeting>();
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

        public RelayCommand OpenMeetingCommand
        {
            get
            {
                return new RelayCommand(async () =>
                  {
                      await NavService.NavigateTo<GameViewModel>();
                  });
            }
        }

        public override async Task Init()
        {
            //TODO: remove - for development only
            await ((App)Application.Current).SetDemoMode(true);

            Meetings = (await DataService.GetCurrentFutureMeetingsAsync()).ToObservableCollection();
        }
    }
}