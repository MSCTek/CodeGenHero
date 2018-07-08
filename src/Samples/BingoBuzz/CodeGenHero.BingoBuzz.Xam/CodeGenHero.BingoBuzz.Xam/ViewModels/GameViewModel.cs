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
using CodeGenHero.Logging;

//using Plugin.Toasts;

namespace CodeGenHero.BingoBuzz.Xam.ViewModels
{
    public class GameViewModel : CustomViewModelBase<Guid>
    {
        private BingoInstance _bingoInstance;
        private List<BingoInstanceContent> _bingoInstanceContent;
        private Meeting _meeting;
        private List<PlayerViewModel> _players;

        public GameViewModel(INavigationService navService, IDataRetrievalService dataRetrievalService, IDataDownloadService dataDownloadService, IStateService stateService, ILoggingService loggingService)
            : base(navService, dataRetrievalService, dataDownloadService, stateService, loggingService)
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
                    var selected = BingoInstanceContent.Where(x => x.IsSelected).Select(x => x.ToDto()).ToList();
                    var potentialBingo = BingoBuzz.Helpers.CheckForBingo(BingoInstance.ToDto(), selected);
                    if (potentialBingo != null)
                    {
                        await Bingo(potentialBingo);
                        DataRetrievalService.CreateSendNewBingoInstanceEvent(bingoInstanceContentId, BingoInstance.BingoInstanceId, Constants.Enums.BingoInstanceEventType.Bingo);
                        //TODO: game is over
                        await NavService.NavigateToNoAnimation<WelcomeViewModel>();
                    }
                });
            }
        }

        public override async Task Init(Guid meetingId)
        {
            await NavService.PushAlertPopupAsync("Loading...");

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

            await NavService.PopAlertPopupsAsync();
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
    }
}