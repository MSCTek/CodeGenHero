using GalaSoft.MvvmLight.Ioc;
using MSC.BingoBuzz.Xam.Interfaces;
using MSC.BingoBuzz.Xam.ViewModels;
using Ninject;
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
    public partial class WelcomePage : ContentPage, IContentPage
    {
        public WelcomePage()
        {
            InitializeComponent();
        }

        public void PrepareForDispose()
        {
        }
    }
}