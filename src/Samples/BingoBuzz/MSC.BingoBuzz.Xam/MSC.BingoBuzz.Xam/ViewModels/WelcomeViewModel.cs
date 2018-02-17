using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MSC.BingoBuzz.Xam.Interfaces;
using MSC.BingoBuzz.Xam.ModelObj.BB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace MSC.BingoBuzz.Xam.ViewModels
{
    public class WelcomeViewModel : CustomViewModelBase
    {
        private ObservableCollection<Meeting> _meetings;

        public WelcomeViewModel(INavigationService navService, IDataService dataService) : base(navService, dataService)
        {
            Meetings = new ObservableCollection<Meeting>();

            var meeting1 = new Meeting()
            {
                Name = "BingoBuzz Mockup Review",
                MeetingId = new Guid(),
            };
            meeting1.MeetingSchedules.Add(new MeetingSchedule()
            {
                StartDate = DateTime.Now
            });
            Meetings.Add(meeting1);

            var meeting2 = new Meeting()
            {
                Name = "BingoBuzz UI Review",
                MeetingId = new Guid()
            };
            meeting2.MeetingSchedules.Add(new MeetingSchedule()
            {
                StartDate = DateTime.Now.AddDays(2)
            });

            Meetings.Add(meeting2);
        }

        public ObservableCollection<Meeting> Meetings
        {
            get { return _meetings; }
            set { Set<ObservableCollection<Meeting>>(() => Meetings, ref _meetings, value); }
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

        public override async Task Init()
        {
        }
    }
}