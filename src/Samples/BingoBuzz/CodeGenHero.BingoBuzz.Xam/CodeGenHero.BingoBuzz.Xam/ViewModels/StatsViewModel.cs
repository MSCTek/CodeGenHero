using GalaSoft.MvvmLight;
using CodeGenHero.BingoBuzz.Xam.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CodeGenHero.Logging;

namespace CodeGenHero.BingoBuzz.Xam.ViewModels
{
    public class StatsViewModel : CustomViewModelBase
    {
        private int _numSquaresClicked;
        private int _numBingos;
        private int _numGames;

        public StatsViewModel(INavigationService navService, IDataRetrievalService dataRetrievalService, IDataDownloadService dataDownloadService, IStateService stateService, ILoggingService loggingService)
            : base(navService, dataRetrievalService, dataDownloadService, stateService, loggingService)
        {
        }

        public override async Task Init()
        {
            NumSquaresClicked = await DataRetrievalService.GetTotalNumberOfSquareClicks();
            NumBingos = await DataRetrievalService.GetTotalNumberOfBingos();
            NumGames = await DataRetrievalService.GetTotalNumberOfGames();
        }

        public int NumGames
        {
            get { return _numGames; }
            set { Set(ref _numGames, value); }
        }

        public int NumSquaresClicked
        {
            get { return _numSquaresClicked; }
            set { Set(ref _numSquaresClicked, value); }
        }

        public int NumBingos
        {
            get { return _numBingos; }
            set { Set(ref _numBingos, value); }
        }
    }
}