using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MSC.BingoBuzz.Xam.Controls
{
    public class CustomHeader : ContentView, IDisposable
    {
        public static readonly BindableProperty BackButtonCommandProperty = BindableProperty.Create(propertyName: "BackButtonCommand",
              returnType: typeof(RelayCommand),
              declaringType: typeof(CustomHeader),
              propertyChanged: OnPropertyChanged,
              defaultValue: default(RelayCommand));

        public static readonly BindableProperty HamburgerCommandProperty = BindableProperty.Create(propertyName: "HamburgerCommand",
              returnType: typeof(RelayCommand),
              declaringType: typeof(CustomHeader),
              propertyChanged: OnPropertyChanged,
              defaultValue: default(RelayCommand));

        public static readonly BindableProperty ShowBackButtonProperty = BindableProperty.Create(propertyName: "ShowBackButton",
                returnType: typeof(bool),
                declaringType: typeof(CustomHeader),
                propertyChanged: OnPropertyChanged,
                defaultValue: true);

        public static readonly BindableProperty ShowHamburgerProperty = BindableProperty.Create(propertyName: "ShowHamburger",
               returnType: typeof(bool),
               declaringType: typeof(CustomHeader),
               propertyChanged: OnPropertyChanged,
               defaultValue: true);

        public static readonly BindableProperty TitleProperty = BindableProperty.Create(propertyName: "Title",
                returnType: typeof(string),
                declaringType: typeof(CustomHeader),
                propertyChanged: OnPropertyChanged,
                defaultValue: default(string));

        //private IconLabel _backButtonIcon;

        private Label _backButtonIcon;

        private StackLayout _backButtonStackLayout;

        private bool _disposed;

        private IconLabel _hamburgerLabel;

        private StackLayout _hamburgerStackLayout;

        private Grid _headerGrid;

        private Label _headerLabel;

        private TapGestureRecognizer _tapGestureRecognizerBack;

        private TapGestureRecognizer _tapGestureRecognizerHam;

        public CustomHeader()
        {
            Init();
        }

        public RelayCommand BackButtonCommand
        {
            get { return (RelayCommand)GetValue(BackButtonCommandProperty); }
            set { SetValue(BackButtonCommandProperty, value); }
        }

        public RelayCommand HamburgerCommand
        {
            get { return (RelayCommand)GetValue(HamburgerCommandProperty); }
            set { SetValue(HamburgerCommandProperty, value); }
        }

        //NOTE: default is text only
        //NOTE: default is white text

        /*   public HeaderTheme HeaderTheme
           {
               get { return (HeaderTheme)GetValue(HeaderThemeProperty); }
               set { SetValue(HeaderThemeProperty, value); }
           }*/

        public bool ShowBackButton
        {
            get { return (bool)GetValue(ShowBackButtonProperty); }
            set { SetValue(ShowBackButtonProperty, value); }
        }

        public bool ShowHamburger
        {
            get { return (bool)GetValue(ShowHamburgerProperty); }
            set { SetValue(ShowHamburgerProperty, value); }
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public void CleanUp()
        {
            _backButtonIcon = null;
            _backButtonStackLayout = null;
            _headerLabel = null;
            _hamburgerStackLayout = null;
            _headerGrid = null;
            _tapGestureRecognizerHam = null;
            _tapGestureRecognizerBack = null;
            _hamburgerLabel = null;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // free other managed objects that implement
                // IDisposable only
                CleanUp();
            }

            // release any unmanaged objects
            // set the object references to null

            _disposed = true;
        }

        private static void OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var ch = (CustomHeader)bindable;
            ch.Init();
        }

        private void Init()
        {
            _headerGrid = new Grid();
            _headerGrid.BackgroundColor = (Color)App.Current.Resources["PI.Transparent"];
            _headerGrid.Margin = 0;
            _headerGrid.Padding = 0;
            _headerGrid.HorizontalOptions = LayoutOptions.FillAndExpand;
            _headerGrid.VerticalOptions = LayoutOptions.FillAndExpand;
            //change the height of the grid depending on the type of header here:
            _headerGrid.HeightRequest = 48;

            if (ShowBackButton)
            {
                _backButtonStackLayout = new StackLayout();
                _backButtonStackLayout.Orientation = StackOrientation.Horizontal;
                _backButtonStackLayout.HorizontalOptions = LayoutOptions.StartAndExpand;

                /*_backButtonIcon = new IconLabel();
                _backButtonIcon.Text = (string)App.Current.Resources["Icon.BackArrow"]; //= "\ue800";*/

                //TODO: get font awesome in here
                _backButtonIcon = new Label();
                _backButtonIcon.Text = "<";
                _backButtonIcon.FontAttributes = FontAttributes.Bold;


                _backButtonIcon.FontSize = 21;
                // _backButtonIcon.TextColor = (Color)App.Current.Resources["PI.Charcoal"];
                _backButtonIcon.Margin = new Thickness(12);
                _backButtonIcon.HorizontalOptions = LayoutOptions.Start;
                _backButtonIcon.VerticalOptions = LayoutOptions.Center;
                _backButtonIcon.AutomationId = "BackButton";

                _backButtonStackLayout.Children.Add(_backButtonIcon);

                _tapGestureRecognizerBack = new TapGestureRecognizer();
                _tapGestureRecognizerBack.SetBinding(TapGestureRecognizer.CommandProperty, "BackCommand", BindingMode.TwoWay);
                _tapGestureRecognizerBack.AutomationId = "BackButton";
                _backButtonStackLayout.GestureRecognizers.Add(_tapGestureRecognizerBack);

                _headerGrid.Children.Add(_backButtonStackLayout); 

            }

            if (ShowHamburger)
            {
                _hamburgerStackLayout = new StackLayout();
                _hamburgerStackLayout.Orientation = StackOrientation.Horizontal;
                _hamburgerStackLayout.Margin = new Thickness(0);
                _hamburgerStackLayout.HorizontalOptions = LayoutOptions.StartAndExpand;
                _hamburgerStackLayout.VerticalOptions = LayoutOptions.End;

                _hamburgerLabel = new IconLabel();
                _hamburgerLabel.BackgroundColor = (Color)App.Current.Resources["PI.Transparent"];
                _hamburgerLabel.Text = (string)App.Current.Resources["Icon.Hamburger"]; //"\uf008";
                _hamburgerLabel.FontSize = 21;
                //_hamburgerLabel.TextColor = (Color)App.Current.Resources["PI.Charcoal"];
                _hamburgerLabel.Margin = new Thickness(12);
                _hamburgerLabel.HorizontalOptions = LayoutOptions.Start;
                _hamburgerLabel.VerticalOptions = LayoutOptions.Center;
                _hamburgerLabel.AutomationId = "HamburgerButton";

                _hamburgerStackLayout.Children.Add(_hamburgerLabel);

                _tapGestureRecognizerHam = new TapGestureRecognizer();
                _tapGestureRecognizerHam.SetBinding(TapGestureRecognizer.CommandProperty, "HamburgerCommand", BindingMode.TwoWay);
                _tapGestureRecognizerHam.AutomationId = "HamburgerButton";
                _hamburgerStackLayout.GestureRecognizers.Add(_tapGestureRecognizerHam);

                _headerGrid.Children.Add(_hamburgerStackLayout);
            }

            _headerLabel = new Label();
            _headerLabel.Text = !string.IsNullOrEmpty(Title) ? Title : string.Empty;
            _headerLabel.FontSize = 18;
            _headerLabel.FontAttributes = FontAttributes.Bold;
            _headerLabel.LineBreakMode = LineBreakMode.TailTruncation;
            _headerLabel.HorizontalOptions = LayoutOptions.Center;
            _headerLabel.VerticalOptions = LayoutOptions.Center;
            _headerLabel.Margin = 5;
            //_headerLabel.TextColor = (Color)App.Current.Resources["PI.Charcoal"];

            _headerGrid.Children.Add(_headerLabel);

            Content = _headerGrid;
        }
    }
}