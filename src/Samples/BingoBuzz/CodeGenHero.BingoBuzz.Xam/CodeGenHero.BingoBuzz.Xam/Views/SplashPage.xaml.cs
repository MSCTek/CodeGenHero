﻿using CodeGenHero.BingoBuzz.Xam.Interfaces;
using CodeGenHero.BingoBuzz.Xam.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CodeGenHero.BingoBuzz.Xam.Views
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

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (vm != null)
            {
            }
        }
    }
}