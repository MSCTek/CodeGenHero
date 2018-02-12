using MSC.BingoBuzz.Xam.Interfaces;
using MSC.BingoBuzz.Xam.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MSC.BingoBuzz.Xam.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewMeetingPage : ContentPage, IContentPage
    {
        public NewMeetingPage()
        {
            InitializeComponent();
        }

        private NewMeetingViewModel vm
        {
            get { return BindingContext as NewMeetingViewModel; }
        }

        public void PrepareForDispose()
        {
            if (vm != null)
            {
                vm.Cleanup();
                //clean up view here
                vm.Dispose();
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (vm != null)
            {
            }
        }

        protected override bool OnBackButtonPressed()
        {
            //by calling the base, we allow the android back button to go back through the nav stack
            return base.OnBackButtonPressed();
            //by returning TRUE,we cancel the hardware backbutton on Android only.
            //return true;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (vm != null)
            {
            }
        }
    }
}