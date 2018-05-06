using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeGenHero.BingoBuzz.Xam.Interfaces;
using CodeGenHero.BingoBuzz.Xam.ModelObj.BB;
using GalaSoft.MvvmLight.Command;

namespace CodeGenHero.BingoBuzz.Xam.ViewModels
{
    public class NewMeetingViewModel : CustomViewModelBase
    {
        private IDataUploadService _dataUploadService;
        private string _meetingName;
        private ObservableCollection<User> _users;

        public NewMeetingViewModel(INavigationService navService, IDataRetrievalService dataRetrievalService, IDataDownloadService dataDownloadService, IDataUploadService dataUploadService, IStateService stateService) : base(navService, dataRetrievalService, dataDownloadService, stateService)
        {
            _dataUploadService = dataUploadService;
        }

        public RelayCommand MakeNewMeetingCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    if (!string.IsNullOrEmpty(MeetingName) && Users.Where(x => x.IsSelected).ToList().Any())
                    {
                        //TODO: move this somewhere else
                        //write this meeting to SQLite and queue it for upload
                        var meeting = new Meeting()
                        {
                            MeetingId = Guid.NewGuid(),
                            Name = MeetingName
                        };

                        var selectedUsers = Users.Where(x => x.IsSelected).ToList();

                        if (await DataRetrievalService.CreateNewMeeting(meeting, selectedUsers))
                        {
                            NavService.NavigateTo<WelcomeViewModel>();
                        }
                        else
                        {
                            //TODO: something really bad happened here
                        }
                    }
                    else
                    {
                        NavService.PushAlertPopupAsync("Please enter a meeting name and choose at least one attendee.");
                    }
                });
            }
        }

        public string MeetingName
        {
            get { return _meetingName; }
            set { Set(ref _meetingName, value); }
        }

        public ObservableCollection<User> Users
        {
            get { return _users; }
            set { Set(ref _users, value); }
        }

        public override async Task Init()
        {
            _dataUploadService.StartSafeQueuedUpdates();

            Users = (await DataRetrievalService.GetUsersAsync()).ToObservableCollection();
        }
    }
}