using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PIMobile.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActivityIndicatorTransparentPopup : PopupPage
    {
        public ActivityIndicatorTransparentPopup()
        {
            //InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        // Method for animation child in PopupPage
        // Invoced after custom animation end
        protected override void OnAppearingAnimationEnd()
        {
            base.OnAppearingAnimationEnd();
        }

        protected override bool OnBackButtonPressed()
        {
            // Prevent hide popup
            //return base.OnBackButtonPressed();
            return true;
        }

        // Invoced when background is clicked
        protected override bool OnBackgroundClicked()
        {
            // Return default value - CloseWhenBackgroundIsClicked
            //return base.OnBackgroundClicked();
            return false;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        // Method for animation child in PopupPage
        // Invoked before custom animation begin
        protected override void OnDisappearingAnimationBegin()
        {
            base.OnDisappearingAnimationBegin();
        }
    }
}