using GalaSoft.MvvmLight;
using CodeGenHero.BingoBuzz.Xam.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenHero.BingoBuzz.Xam.ViewModels
{
    public class StatsViewModel : CustomViewModelBase
    {
        public StatsViewModel(INavigationService navService, IDataRetrievalService dataRetrievalService, IDataDownloadService dataDownloadService, IStateService stateService) : base(navService, dataRetrievalService, dataDownloadService, stateService)
        {
        }

        public override async Task Init()
        {
        }
    }
}