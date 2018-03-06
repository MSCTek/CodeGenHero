using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace CodeGenHero.BingoBuzz.Xam.Controls
{
    public class PlayerViewModel : ObservableObject
    {
        private string _playerName;
        private int _score;

        public string PlayerName
        {
            get { return _playerName; }
            set { Set(ref _playerName, value); }
        }

        public int Score
        {
            get { return _score; }
            set { Set(ref _score, value); }
        }
    }

    public class PlayerWrapLayout : ContentView, IDisposable
    {
        public static readonly BindableProperty SourcePlayersProperty = BindableProperty.Create(propertyName: "SourcePlayers",
            returnType: typeof(List<PlayerViewModel>),
            declaringType: typeof(PlayerWrapLayout),
            propertyChanged: OnPropertyChanged,
            defaultValue: default(List<PlayerViewModel>));

        private bool _disposed;

        public PlayerWrapLayout()
        {
            Init();
        }

        public List<PlayerViewModel> SourcePlayers
        {
            get { return (List<PlayerViewModel>)GetValue(SourcePlayersProperty); }
            set { SetValue(SourcePlayersProperty, value); }
        }

        public void CleanUp()
        {
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
            var ch = (PlayerWrapLayout)bindable;
            ch.Init();
        }

        private void Init()
        {
            if (SourcePlayers != null && SourcePlayers.Any())
            {
                var playerWrapLayout = new WrapLayout();

                foreach (var l in SourcePlayers)
                {
                    StackLayout sl = new StackLayout();
                    sl.Spacing = 0;

                    var nameLabel = new Label();
                    nameLabel.Margin = new Thickness(5, 5, 5, 0);
                    nameLabel.Text = l.PlayerName;
                    if (Device.RuntimePlatform != Device.UWP) nameLabel.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
                    else nameLabel.FontSize = 10;
                    sl.Children.Add(nameLabel);

                    var scoreLabel = new Label();
                    scoreLabel.Margin = new Thickness(5, 0, 5, 5);
                    scoreLabel.Text = l.Score.ToString();
                    if (Device.RuntimePlatform != Device.UWP) scoreLabel.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
                    else scoreLabel.FontSize = 14;

                    sl.Children.Add(scoreLabel);

                    playerWrapLayout.Children.Add(sl);
                }

                Content = playerWrapLayout;
            }
        }
    }
}