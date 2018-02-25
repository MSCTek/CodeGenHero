using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MSC.BingoBuzz.Xam.Interfaces;
using MSC.BingoBuzz.Xam.ModelObj.BB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSC.BingoBuzz.Xam.ViewModels
{
    public class GameViewModel : CustomViewModelBase<Guid>
    {
        private BingoInstance _bingoInstance;
        private List<BingoInstanceContent> _bingoInstanceContent;
        private Meeting _meeting;
        private List<MeetingAttendee> _players;

        public GameViewModel(INavigationService navService, IDataRetrievalService dataRetrievalService, IStateService stateService) : base(navService, dataRetrievalService, stateService)
        {
            BingoInstanceContent = new List<ModelObj.BB.BingoInstanceContent>();
            Players = new List<MeetingAttendee>();
        }

        public BingoInstance BingoInstance
        {
            get { return _bingoInstance; }
            set { Set(ref _bingoInstance, value); }
        }

        public List<BingoInstanceContent> BingoInstanceContent
        {
            get { return _bingoInstanceContent; }
            set { Set(ref _bingoInstanceContent, value); }
        }

        public Meeting Meeting
        {
            get { return _meeting; }
            set { Set(ref _meeting, value); }
        }

        public List<MeetingAttendee> Players
        {
            get { return _players; }
            set { Set(ref _players, value); }
        }

        public RelayCommand<Guid> SquareTappedCommand
        {
            get
            {
                return new RelayCommand<Guid>((bingoInstanceContentId) =>
                {
                    var selectedContent = BingoInstanceContent.Where(x => x.BingoInstanceContentId == bingoInstanceContentId).FirstOrDefault();
                    if (selectedContent.IsSelected)
                    {
                        selectedContent.IsSelected = false;
                    }
                    else
                    {
                        selectedContent.IsSelected = true;
                    }
                    RaisePropertyChanged(nameof(BingoInstanceContent));
                });
            }
        }

        public override async Task Init(Guid meetingId)
        {
            Meeting = await DataRetrievalService.GetMeetingOrNullAsync(meetingId);

            if (Meeting != null)
            {
                Players = await DataRetrievalService.GetMeetingAttendeesAsync(meetingId);
                BingoInstance = await DataRetrievalService.GetCurrentBingoInstanceOrNullAsync(meetingId);
                if (BingoInstance == null)
                {
                    //we need to make a new instance of this meeting, and make new content too
                    BingoInstance = await DataRetrievalService.CreateNewBingoInstance(meetingId);
                }

                BingoInstanceContent = await DataRetrievalService.GetBingoInstanceContentAsync(BingoInstance.BingoInstanceId);
                RaisePropertyChanged(nameof(BingoInstanceContent));
            }
        }
    }
}