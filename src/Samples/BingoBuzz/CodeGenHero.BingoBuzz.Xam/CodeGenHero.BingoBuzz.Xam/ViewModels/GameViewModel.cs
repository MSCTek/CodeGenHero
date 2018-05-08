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
using System.Diagnostics;
using Plugin.Vibrate;
using Xamarin.Forms;

//using Plugin.Toasts;

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
                return new RelayCommand<Guid>(async (bingoInstanceContentId) =>
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
                    DataRetrievalService.CreateSendNewBingoInstanceEvent(bingoInstanceContentId, BingoInstance.BingoInstanceId, Constants.Enums.BingoInstanceEventType.SquareClicked);
                    if (await CheckForBingo())
                    {
                        DataRetrievalService.CreateSendNewBingoInstanceEvent(bingoInstanceContentId, BingoInstance.BingoInstanceId, Constants.Enums.BingoInstanceEventType.Bingo);
                        //TODO: game is over
                        await NavService.NavigateToNoAnimation<WelcomeViewModel>();
                    }
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

        private async Task<bool> Bingo(string message)
        {
            Debug.WriteLine(message);

            await NavService.PushBingoPopupAsync();

            CrossVibrate.Current.Vibration(TimeSpan.FromSeconds(1)); // 1 second vibration

            return true;
            //var notificator = DependencyService.Get<IToastNotificator>();
            //var options = new NotificationOptions()
            //{
            //    Title = "BINGO",
            //    Description = message
            //};

            //var result = await notificator.Notify(options);
        }

        private async Task<bool> CheckForBingo()
        {
            var selected = BingoInstanceContent.Where(x => x.IsSelected).ToList();

            //vertical bingo
            for (var i = 0; i < BingoInstance.NumberOfRows; i++)
            {
                //iterate through the rows
                //see if you have the same number of selected squares in the same row as you have columns, if so, you have horizontal bingo.
                if (selected.Where(x => x.Row == i).Count() == BingoInstance.NumberOfColumns)
                {
                    return await Bingo("Horizontal Bingo!");
                }
            }

            //horizontal bingo
            for (var i = 0; i < BingoInstance.NumberOfColumns; i++)
            {
                //iterate through the columns
                //see if you have the same number of selected squares in the same column as you have rows, if so, you have vertical bingo.
                if (selected.Where(x => x.Col == i).Count() == BingoInstance.NumberOfRows)
                {
                    return await Bingo("Vertical Bingo!");
                }
            }

            //this will only work if we have a square board...
            if (BingoInstance.NumberOfColumns == BingoInstance.NumberOfRows)
            {
                int countDiaSqs = 0;
                //diagonal bingo 0,0 to x,x - top left to bottom right
                for (var i = 0; i < BingoInstance.NumberOfColumns; i++)
                {
                    bool isAContender = true;
                    //iterate through the columns
                    //see if the selected squares have a row and column number that match
                    if (selected.Where(x => x.Row == i && x.Col == i).Any() && isAContender)
                    {
                        countDiaSqs++;
                    }
                    else
                    {
                        isAContender = false;
                    }

                    if (isAContender && countDiaSqs == BingoInstance.NumberOfColumns)
                    {
                        return await Bingo("Diagonal Bingo top left to bottom right!");
                    }
                }
            }

            //this will only work if we have a square board...
            if (BingoInstance.NumberOfColumns == BingoInstance.NumberOfRows)
            {
                //diagonal bingo bottom left to top right
                //see if the selected squares have a row and col number that add up to the total number of cols -1
                if (selected.Where(x => x.Row + x.Col == (BingoInstance.NumberOfColumns - 1)).Count() == BingoInstance.NumberOfColumns)
                {
                    return await Bingo("Diagonal Bingo  bottom left to top right!");
                }
            }
            return false;
        }
    }
}