using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using CodeGenHero.BingoBuzz.Xam.Interfaces;
using CodeGenHero.BingoBuzz.Xam.ModelObj.BB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeGenHero.BingoBuzz.Xam.Controls;

namespace CodeGenHero.BingoBuzz.Xam.ViewModels
{
    public class GameViewModel : CustomViewModelBase<Guid>
    {
        private BingoInstance _bingoInstance;
        private List<BingoInstanceContent> _bingoInstanceContent;
        private Meeting _meeting;
        private List<PlayerViewModel> _players;

        public GameViewModel(INavigationService navService, IDataRetrievalService dataRetrievalService, IStateService stateService) : base(navService, dataRetrievalService, stateService)
        {
            BingoInstanceContent = new List<ModelObj.BB.BingoInstanceContent>();
            Players = new List<PlayerViewModel>();
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

        public List<PlayerViewModel> Players
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
                var players = await DataRetrievalService.GetMeetingAttendeesAsync(meetingId);
                List<PlayerViewModel> playerVms = new List<PlayerViewModel>();
                foreach (var p in players)
                {
                    playerVms.Add(new PlayerViewModel()
                    {
                        PlayerName = $"{p.User_UserId.FirstName}  {p.User_UserId.LastName.Substring(0, 1)}.",
                        Score = 0 //TODO: need to wire this up
                    });
                }
                Players = playerVms;
                RaisePropertyChanged(nameof(Players));

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