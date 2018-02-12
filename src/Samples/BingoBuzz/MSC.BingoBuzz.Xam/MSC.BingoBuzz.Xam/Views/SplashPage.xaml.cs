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
    public partial class SplashPage : ContentPage, IContentPage
    {
        public SplashPage()
        {
            InitializeComponent();
        }

        private SplashViewModel vm
        {
            get { return BindingContext as SplashViewModel; }
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
                await vm.Init();
            }
        }

        protected override bool OnBackButtonPressed()
        {
            //by returning TRUE and not calling base, we cancel the hardware backbutton on Android only.
            //return base.OnBackButtonPressed();
            return true;
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