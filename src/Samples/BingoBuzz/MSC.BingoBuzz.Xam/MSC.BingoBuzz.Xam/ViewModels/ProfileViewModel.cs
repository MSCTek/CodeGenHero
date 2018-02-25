using GalaSoft.MvvmLight;
using MSC.BingoBuzz.Xam.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSC.BingoBuzz.Xam.ViewModels
{
    public class ProfileViewModel : CustomViewModelBase
    {
        public ProfileViewModel(INavigationService navService, IDataRetrievalService dataRetrievalService, IStateService stateService) : base(navService, dataRetrievalService, stateService)
        {
        }

        public override async Task Init()
        {
        }
    }
}