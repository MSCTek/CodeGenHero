using MSC.BingoBuzz.Xam.Interfaces;
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
    public partial class ProfilePage : ContentPage, IContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
        }

        public void PrepareForDispose()
        {
        }
    }
}