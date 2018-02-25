using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSC.BingoBuzz.Xam.Interfaces;

namespace MSC.BingoBuzz.Xam.ViewModels
{
    public class NewMeetingViewModel : CustomViewModelBase
    {
        public NewMeetingViewModel(INavigationService navService, IDataRetrievalService dataRetrievalService, IStateService stateService) : base(navService, dataRetrievalService, stateService)
        {
        }

        public override async Task Init()
        {
        }
    }
}